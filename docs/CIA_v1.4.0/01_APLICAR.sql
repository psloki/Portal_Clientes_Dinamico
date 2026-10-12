/* APPLY v1.4.0. Ejecutar completo en SSMS. Compañía actual: DIS86.
   Dos SP corregidos, dos SP nuevos, tres vistas corregidas y dos vistas nuevas. No modifica GLOBAL ni datos.
   Respaldar la base y publicar el backend/RDL de esta versión después de aplicar. */
USE [PS_PORTAL_CLIENTE];
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON;
IF DB_NAME() <> N'PS_PORTAL_CLIENTE' THROW 51000, 'Base de destino incorrecta', 1;
IF SCHEMA_ID(N'DIS86') IS NULL THROW 51001, 'No existe el esquema DIS86', 1;
BEGIN TRY
    BEGIN TRANSACTION;
    -- DIS86.VIEW_OBTENER_HORA_CORTE
    EXEC sys.sp_executesql N'CREATE OR ALTER VIEW [DIS86].[VIEW_OBTENER_HORA_CORTE] 
AS 
SELECT DISTINCT U_HORA_CORTE+'':00'' U_HORA_CORTE
FROM EXACTUSERP.DIS86.RUTA';
    -- DIS86.PROC_EDI_DESCUENTO_CLIENTE_ARTICULO_MASIVO
    EXEC sys.sp_executesql N'CREATE OR ALTER PROCEDURE [DIS86].[PROC_EDI_DESCUENTO_CLIENTE_ARTICULO_MASIVO]
(
    @CLIENTE           VARCHAR(20),
    @ARTICULOS_CSV     VARCHAR(MAX),
    @ESCALA            INT,
    @CANTIDAD_MIN      INT,
    @CANTIDAD_MAX      INT,
    @FECHA_INI         DATETIME,
    @FECHA_FIN         DATETIME,
    @PORC_DESCUENTO    INT,
    @CIA               VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT, XACT_ABORT ON;
    IF NULLIF(LTRIM(RTRIM(@CIA)), '''') IS NULL
        THROW 51002, ''CIA es obligatoria'', 1;

    BEGIN TRAN;

    BEGIN TRY
        -- 1) Validar cliente
        DECLARE @ClienteExiste BIT = 0;
        DECLARE @SqlValidacion NVARCHAR(MAX) = N''SELECT @Existe = CASE WHEN EXISTS
            (SELECT 1 FROM EXACTUSERP.'' + QUOTENAME(@CIA) + N''.CLIENTE WHERE CLIENTE = @Cliente)
            THEN 1 ELSE 0 END;'';
        EXEC sys.sp_executesql @SqlValidacion,
            N''@Cliente VARCHAR(20), @Existe BIT OUTPUT'',
            @Cliente = @CLIENTE, @Existe = @ClienteExiste OUTPUT;
        IF @ClienteExiste = 0
            THROW 51000, ''Cliente no existe'', 1;

        -- 2) Poblar tabla temporal con los artículos recibidos
        IF OBJECT_ID(''tempdb..#Articulos'') IS NOT NULL
            DROP TABLE #Articulos;

        CREATE TABLE #Articulos (
            ARTICULO VARCHAR(20) PRIMARY KEY
        );

        INSERT INTO #Articulos(ARTICULO)
        SELECT LTRIM(RTRIM(value))
        FROM STRING_SPLIT(@ARTICULOS_CSV, '','')
        WHERE value <> '''';

        -- 3) Eliminar de #Articulos los que no existen en maestro
        SET @SqlValidacion = N''DELETE A FROM #Articulos AS A
        WHERE NOT EXISTS (SELECT 1 FROM EXACTUSERP.'' + QUOTENAME(@CIA) + N''.ARTICULO art
                          WHERE art.ARTICULO = A.ARTICULO);'';
        EXEC sys.sp_executesql @SqlValidacion;

        -- 4) Si quedó vacío, abortar
        IF NOT EXISTS (SELECT 1 FROM #Articulos)
            THROW 51001, ''Ningún artículo válido en la lista'', 1;

        -- 5) Construir nombre de la tabla destino
        DECLARE @Schema NVARCHAR(128) = QUOTENAME(ISNULL(@CIA, ''''));
        DECLARE @Tabla  NVARCHAR(256) = N''EXACTUSERP.'' + @Schema + N''.[DCTO_ART_X_CLI]'';

        -- 6) Actualizar filas existentes
        DECLARE @sql NVARCHAR(MAX) = N''
        UPDATE T
        SET
            CANTIDAD_INICIAL = @CANT_MIN,
            CANTIDAD_FINAL   = @CANT_MAX,
            FECHA_INICIAL    = @FI,
            FECHA_FINAL      = @FF,
            PORC_DESCUENTO   = @PD
        FROM '' + @Tabla + '' AS T
        INNER JOIN #Articulos AS A
            ON T.CLIENTE      = @CL
           AND T.ARTICULO     = A.ARTICULO
           AND T.VERSION_DCTO = 1
           AND T.ESCALA_DCTO  = @ES;'';

        EXEC sp_executesql
            @sql,
            N''@CL VARCHAR(20),
              @ES INT,
              @CANT_MIN INT,
              @CANT_MAX INT,
              @FI DATETIME,
              @FF DATETIME,
              @PD INT'',
            @CL        = @CLIENTE,
            @ES        = @ESCALA,
            @CANT_MIN  = @CANTIDAD_MIN,
            @CANT_MAX  = @CANTIDAD_MAX,
            @FI        = @FECHA_INI,
            @FF        = @FECHA_FIN,
            @PD        = @PORC_DESCUENTO;

        -- 7) Insertar los que no estaban
        SET @sql = N''
        INSERT INTO '' + @Tabla + '' (
            CLIENTE, ARTICULO, ESCALA_DCTO, VERSION_DCTO,
            CANTIDAD_INICIAL, CANTIDAD_FINAL,
            FECHA_INICIAL, FECHA_FINAL,
            PORC_DESCUENTO, TIPO_DESCUENTO
        )
        SELECT
            @CL, A.ARTICULO, @ES, 1,
            @CANT_MIN, @CANT_MAX,
            @FI, @FF,
            @PD, ''''P''''
        FROM #Articulos AS A
        LEFT JOIN '' + @Tabla + '' AS T
            ON T.CLIENTE      = @CL
           AND T.ARTICULO     = A.ARTICULO
           AND T.VERSION_DCTO = 1
           AND T.ESCALA_DCTO  = @ES
        WHERE T.ARTICULO IS NULL;'';

        EXEC sp_executesql
            @sql,
            N''@CL VARCHAR(20),
              @ES INT,
              @CANT_MIN INT,
              @CANT_MAX INT,
              @FI DATETIME,
              @FF DATETIME,
              @PD INT'',
            @CL        = @CLIENTE,
            @ES        = @ESCALA,
            @CANT_MIN  = @CANTIDAD_MIN,
            @CANT_MAX  = @CANTIDAD_MAX,
            @FI        = @FECHA_INI,
            @FF        = @FECHA_FIN,
            @PD        = @PORC_DESCUENTO;

        COMMIT;
        RETURN 1;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        RETURN 0;
    END CATCH
END';
    -- DIS86.PROC_EDI_BONIFICACION_CLIENTE_ARTICULO_MASIVO
    EXEC sys.sp_executesql N'CREATE OR ALTER PROCEDURE [DIS86].[PROC_EDI_BONIFICACION_CLIENTE_ARTICULO_MASIVO]
(
    @CLIENTE           VARCHAR(20),
    @ARTICULOS_CSV     VARCHAR(MAX),
    @ESCALA            INT,
    @CANTIDAD_MIN      INT,
    @CANTIDAD_MAX      INT,
    @FECHA_INI         DATETIME,
    @FECHA_FIN         DATETIME,
    @FACTOR            INT,
    @ARTICULO_BONI     VARCHAR(20),
    @UND_BONIFICAR     INT,
    @CIA               VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT   ON;
    SET XACT_ABORT ON;

    -- ① Declaración de variables
    DECLARE 
      @Schema NVARCHAR(128),
      @Tabla  NVARCHAR(256),
      @sql    NVARCHAR(MAX),
	  @EscUso   INT = CASE WHEN @ESCALA = 0 THEN 1 ELSE @ESCALA END; 
    -- ② Construcción del nombre del esquema/tabla
    SET @Schema = QUOTENAME(ISNULL(@CIA, ''''));
    SET @Tabla  = N''EXACTUSERP.'' + @Schema + N''.[BONIF_ART_X_CLI]'';

    IF NULLIF(LTRIM(RTRIM(@CIA)), '''') IS NULL
        THROW 51002, ''CIA es obligatoria'', 1;

    BEGIN TRAN;
    BEGIN TRY
        -- 1) Validar cliente
        DECLARE @ClienteExiste BIT = 0;
        DECLARE @SqlValidacion NVARCHAR(MAX) = N''SELECT @Existe = CASE WHEN EXISTS
            (SELECT 1 FROM EXACTUSERP.'' + QUOTENAME(@CIA) + N''.CLIENTE WHERE CLIENTE = @Cliente)
            THEN 1 ELSE 0 END;'';
        EXEC sys.sp_executesql @SqlValidacion,
            N''@Cliente VARCHAR(20), @Existe BIT OUTPUT'',
            @Cliente = @CLIENTE, @Existe = @ClienteExiste OUTPUT;
        IF @ClienteExiste = 0
            THROW 51000, ''Cliente no existe'', 1;

        -- 2) Poblar tabla temporal
        IF OBJECT_ID(''tempdb..#Articulos'') IS NOT NULL
            DROP TABLE #Articulos;
        CREATE TABLE #Articulos(ARTICULO VARCHAR(20) PRIMARY KEY);
        INSERT INTO #Articulos(ARTICULO)
        SELECT LTRIM(RTRIM(value))
        FROM STRING_SPLIT(@ARTICULOS_CSV, '','')
        WHERE value <> '''';
        SET @SqlValidacion = N''DELETE A FROM #Articulos AS A
        WHERE NOT EXISTS (SELECT 1 FROM EXACTUSERP.'' + QUOTENAME(@CIA) + N''.ARTICULO art
                          WHERE art.ARTICULO = A.ARTICULO);'';
        EXEC sys.sp_executesql @SqlValidacion;
        IF NOT EXISTS (SELECT 1 FROM #Articulos)
            THROW 51001, ''Ningún artículo válido en la lista'', 1;

        -- 3) Forzar actualización de todos los registros seleccionados
        SET @sql = N''
        UPDATE T
        SET
          ESCALA_BONIF      = @ES,
          CANTIDAD_INICIAL  = @CANT_MIN,
          CANTIDAD_FINAL    = @CANT_MAX,
          FECHA_INICIAL     = @FI,
          FECHA_FINAL       = @FF,
          FACTOR_BONIF      = @FA,
          ARTICULO_BONIF    = @ABI,
          UNIDADES_BONIF    = @UNI
        FROM '' + @Tabla + N'' AS T
        INNER JOIN #Articulos AS A
          ON T.CLIENTE = @CL
         AND T.ARTICULO = A.ARTICULO
         AND T.VERSION_BONIF = 1;'';

        EXEC sp_executesql
          @sql,
          N''@CL VARCHAR(20), @ES INT, @CANT_MIN INT, @CANT_MAX INT,
            @FI DATETIME, @FF DATETIME, @FA INT, @ABI VARCHAR(20), @UNI INT'',
          @CL        = @CLIENTE,
          @ES        = @EscUso,
          @CANT_MIN  = @CANTIDAD_MIN,
          @CANT_MAX  = @CANTIDAD_MAX,
          @FI        = @FECHA_INI,
          @FF        = @FECHA_FIN,
          @FA        = @FACTOR,
          @ABI       = @ARTICULO_BONI,
          @UNI       = @UND_BONIFICAR;

        -- 4) Insertar los faltantes
        SET @sql = N''
        INSERT INTO '' + @Tabla + N'' (
          CLIENTE, ARTICULO, ESCALA_BONIF, VERSION_BONIF,
          CANTIDAD_INICIAL, CANTIDAD_FINAL,
          FECHA_INICIAL, FECHA_FINAL,
          FACTOR_BONIF, ARTICULO_BONIF, UNIDADES_BONIF
        )
        SELECT
          @CL, A.ARTICULO, @ES, 1,
          @CANT_MIN, @CANT_MAX,
          @FI, @FF,
          @FA, @ABI, @UNI
        FROM #Articulos A
        LEFT JOIN '' + @Tabla + N'' T
          ON T.CLIENTE = @CL
         AND T.ARTICULO = A.ARTICULO
         AND T.VERSION_BONIF = 1
         AND T.ESCALA_BONIF = @ES
        WHERE T.ARTICULO IS NULL;'';

        EXEC sp_executesql
          @sql,
          N''@CL VARCHAR(20), @ES INT, @CANT_MIN INT, @CANT_MAX INT,
            @FI DATETIME, @FF DATETIME, @FA INT, @ABI VARCHAR(20), @UNI INT'',
          @CL        = @CLIENTE,
          @ES        = @EscUso,
          @CANT_MIN  = @CANTIDAD_MIN,
          @CANT_MAX  = @CANTIDAD_MAX,
          @FI        = @FECHA_INI,
          @FF        = @FECHA_FIN,
          @FA        = @FACTOR,
          @ABI       = @ARTICULO_BONI,
          @UNI       = @UND_BONIFICAR;

        COMMIT;
        RETURN 1;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        RETURN 0;
    END CATCH
END';
    -- DIS86.PROC_OBT_CATALOGO_CIA
    EXEC sys.sp_executesql N'CREATE OR ALTER PROCEDURE [DIS86].[PROC_OBT_CATALOGO_CIA]
    @CIA VARCHAR(20),
    @CATALOGO NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;
    IF NULLIF(LTRIM(RTRIM(@CIA)), '''') IS NULL OR SCHEMA_ID(@CIA) IS NULL
        THROW 51000, ''CIA requerida o esquema inexistente'', 1;
    IF @CATALOGO IS NULL OR @CATALOGO NOT IN (
        N''VIEW_OBTENER_AGENTE'',
        N''VIEW_OBTENER_BODEGA'',
        N''VIEW_OBTENER_CLIENTES'',
        N''VIEW_OBTENER_COBRADOR'',
        N''VIEW_OBTENER_DIA_RUTA'',
        N''VIEW_OBTENER_ESTADO_PEDIDO'',
        N''VIEW_OBTENER_HORA_CORTE'',
        N''VIEW_OBTENER_RUTA'',
        N''VIEW_OBTENER_SEGUIMIENTO_FACTURA'',
        N''VIEW_OBTENER_SUCESOS_SEGUIMIENTO'',
        N''VIEW_OBTENER_TLA_ESTADO'',
        N''VIEW_OBTENER_VENDEDOR'',
        N''VIEW_OBT_BANCOS'',
        N''VIEW_OBT_CLIENTES_PTC'',
        N''VIEW_OBT_ESTADO_FACTURA_PTC'',
        N''VIEW_OBT_ESTADO_LIQ'',
        N''VIEW_OBT_ESTADO_PEDIDO_PTC'',
        N''VIEW_OBT_LISTA_ARTICULOS'',
        N''VIEW_OBT_LISTA_ARTICULOS_BONIFICACION'',
        N''VIEW_OBT_RUTA'',
        N''VIEW_OBT_TIPO_DOC_UNICOS'')
        THROW 51001, ''Catálogo no permitido'', 1;
    DECLARE @Sql NVARCHAR(MAX) = N''SELECT * FROM '' + QUOTENAME(@CIA) + N''.'' + QUOTENAME(@CATALOGO) + N'';'';
    EXEC sys.sp_executesql @Sql;
END';
    -- DIS86.PROC_OBT_COMPROBANTE_PAGO_CIA
    EXEC sys.sp_executesql N'CREATE OR ALTER PROCEDURE [DIS86].[PROC_OBT_COMPROBANTE_PAGO_CIA]
    @DOCUMENTO VARCHAR(50),
    @CIA VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF NULLIF(LTRIM(RTRIM(@CIA)), '''') IS NULL OR SCHEMA_ID(@CIA) IS NULL
        THROW 51000, ''CIA requerida o esquema inexistente'', 1;
    IF NOT EXISTS (SELECT 1 FROM EXACTUSERP.ERPADMIN.CONJUNTO WHERE CONJUNTO = @CIA)
        THROW 51001, ''No se encontraron los datos de la compañía'', 1;
    DECLARE @Sql NVARCHAR(MAX) = N''
        SELECT V.CLIENTE, V.NOM_CLIENTE, V.TIPO_MOV,
               C.NIT AS ID_PROVEEDOR, C.NOMBRE AS NOMBRE_PROVEEDOR,
               C.DIREC1 AS DIRECCION_PROVEEDOR,
               V.REFERENCIA, V.FECHA_MOV, V.HORA,
               V.MONTO_CREDITO, V.MONTO_DEBITO, V.DETALLE
        FROM '' + QUOTENAME(@CIA) + N''.VIEW_REPORTE_COMPROBANTE_PAGO AS V
        CROSS JOIN EXACTUSERP.ERPADMIN.CONJUNTO AS C
        WHERE V.REFERENCIA = @Documento AND C.CONJUNTO = @Compania;'';
    EXEC sys.sp_executesql @Sql, N''@Documento VARCHAR(50), @Compania VARCHAR(20)'',
        @Documento = @DOCUMENTO, @Compania = @CIA;
END';
    -- DIS86.VIEW_OBT_ESTADO_AUTORIZA_PEDIDO
    EXEC sys.sp_executesql N'CREATE OR ALTER VIEW [DIS86].[VIEW_OBT_ESTADO_AUTORIZA_PEDIDO]
AS
SELECT DISTINCT AUT.PEDIDO AS PEDIDO_AUTORIZA,
    CONCAT(CASE WHEN LIM_CREDITO = ''S'' AND AUTORIZADO =''N'' THEN ''LIMITE CREDITO'' ELSE '''' END,
           CASE WHEN LIM_CREDITO = ''S'' AND AUTORIZADO =''N'' AND (DOC_VENCIDO = ''S'' OR DIAS_ATRASO = ''S'') THEN '' | '' ELSE '''' END,
           CASE WHEN DOC_VENCIDO = ''S'' AND AUTORIZADO =''N'' THEN ''DOC VENCIDO'' ELSE '''' END,
           CASE WHEN DOC_VENCIDO = ''S'' AND AUTORIZADO =''N'' AND DIAS_ATRASO = ''S'' THEN '' | '' ELSE '''' END,
           CASE WHEN DIAS_ATRASO = ''S'' AND AUTORIZADO =''N'' THEN ''DIAS ATRASO'' ELSE '''' END) AS ESTADO_CONCATENADO
FROM 
          EXACTUSERP.DIS86.PEDIDO_AUTORIZA  AS AUT
JOIN      EXACTUSERP.DIS86.PEDIDO	 AS PED   ON AUT.PEDIDO =PED.PEDIDO

--WHERE (LIM_CREDITO = ''S'' OR DOC_VENCIDO = ''S'' OR DIAS_ATRASO = ''S'')
--WHERE PED.PEDIDO=''R10PED0000000197''';
    -- DIS86.VIEW_OBT_PEDIDO_PTC
    EXEC sys.sp_executesql N'CREATE OR ALTER VIEW [DIS86].[VIEW_OBT_PEDIDO_PTC]
AS 
SELECT DISTINCT 
            ROW_NUMBER() OVER (Order by PD.PEDIDO)       AS  ID
		    ,PD.PEDIDO                                   AS  PEDIDO
		    ,ISNULL(PD.U_PS_TRACKING_ESTADO_PEDIDO,1)    AS  ESTADO_PEDIDO 
		    ,ISNULL(E.DESCRIPCION,''Sin Asignar'')		 AS  ESTADO_PEDIDO_DESCRIPCION
		    ,ISNULL(PAUT.ESTADO_CONCATENADO,''ND'')        AS  AUTORIZADO
	 	    ,CONVERT(varchar,PD.FECHA_HORA,23)	         AS  FECHA_PEDIDO
		    ,ISNULL(FAC.FACTURA,'''')                      AS  FACTURA 
		    ,CL.CLIENTE + '' - '' + CL.ALIAS               AS  CLIENTE 
		    ,CL.CLIENTE                                  AS  COD_CLIENTE
		    ,CL.ALIAS	                                 AS  CLIENTE_NOMBRE 
		    ,ISNULL(FAC.U_PS_TRACKING_ESTADO_FACTURA,1)  AS  SEGUIR_FAC
		    ,ISNULL(F.DESCRIPCION,''Sin Asignar'')		 AS  SEGUIR_FAC_DESCRIPCION
		    ,ISNULL(RT.U_DIAS_RUTA,'''')				     AS  DIAS
		    ,PD.TOTAL_MERCADERIA                         AS  SUBTOTAL
		    ,PD.TOTAL_A_FACTURAR                         AS  TOTAL
		    ,PD.CreateDate							     AS  CreateDate
		    ,ISNULL(E.COLOR_HEXA,''label-light-dark'')     AS  COLOR_HEXA_PED
		    ,ISNULL(F.COLOR_HEXA,''label-light-dark'')     AS  COLOR_HEXA_FAC
			,VEN.VENDEDOR                                AS  COD_VENDEDOR
			,PD.ESTADO                                   AS ESTADO_EXACTUSERP
		
FROM 
			EXACTUSERP.DIS86.PEDIDO			              AS  PD   WITH(NOLOCK)
JOIN		EXACTUSERP.DIS86.RUTA				              AS  RT   WITH(NOLOCK)  ON  PD.RUTA								= RT.RUTA
JOIN		EXACTUSERP.DIS86.CLIENTE			              AS  CL   WITH(NOLOCK)  ON  PD.CLIENTE							= CL.CLIENTE
LEFT JOIN	EXACTUSERP.DIS86.FACTURA			              AS  FAC  WITH(NOLOCK)  ON  PD.PEDIDO							= FAC.PEDIDO AND FAC.TIPO_DOCUMENTO = ''F''
JOIN	    EXACTUSERP.DIS86.VENDEDOR			              AS  VEN  WITH(NOLOCK)  ON  CL.VENDEDOR							= VEN.VENDEDOR
LEFT JOIN	PS_TRACKING.DIS86.ESTADO_PEDIDO				  AS  E	   WITH(NOLOCK)  ON  PD.U_PS_TRACKING_ESTADO_PEDIDO		= E.ESTADO_PEDIDO
LEFT JOIN	PS_TRACKING.DIS86.ESTADO_FACTURA_SEGUIMIENTO  AS  F	   WITH(NOLOCK)  ON  FAC.U_PS_TRACKING_ESTADO_FACTURA	= F.ESTADO_FACTURA_SEGUIMIENTO	
LEFT JOIN   DIS86.VIEW_OBT_ESTADO_AUTORIZA_PEDIDO             AS  PAUT     (NOLOCK)  ON  PAUT.PEDIDO_AUTORIZA = PD.PEDIDO
LEFT JOIN  (select CASE WHEN ISNULL(COUNT(ID_SEG.PEDIDO),''0'')<> ''0''  THEN ''1'' ELSE ''0'' END AS CANTIDAD_SEGUIMIENTO,ID_SEG.PEDIDO from PS_TRACKING.DIS86.SEGUIMIENTO ID_SEG GROUP BY ID_SEG.PEDIDO) AS SEG ON SEG.PEDIDO= PD.PEDIDO

WHERE PD.ESTADO IN(''A'',''B'',''F'',''N'',''C'') AND PD.AUTORIZADO IN(''N'',''S'') 
AND (PD.PEDIDO LIKE ''PT%'' OR PD.PEDIDO LIKE ''%|V%'')';
    -- DIS86.VIEW_REPORTE_COMPROBANTE_PAGO
    EXEC sys.sp_executesql N'CREATE OR ALTER VIEW  [DIS86].[VIEW_REPORTE_COMPROBANTE_PAGO]
AS 
SELECT    DCC.CLIENTE														   CLIENTE
       ,  CLI.NOMBRE														   NOM_CLIENTE 
	   ,  CASE WHEN  DCC.TIPO = ''REC'' THEN ''Recibo''
	           WHEN  DCC.TIPO = ''N/C'' THEN ''Nota de Crédito''
		       WHEN  DCC.TIPO = ''O/C'' THEN ''Otro Crédito''
			   WHEN  DCC.TIPO = ''TEF'' THEN ''T.E.F.''
			   WHEN  DCC.TIPO = ''DEP'' THEN ''Depósito''
			   WHEN  DCC.TIPO = ''DEP'' THEN ''Depósito''
			   ELSE  ''Comprobante'' END										    TIPO_MOV 
		 , C_CIA.NIT														    ID_PROVEEDOR
		 , C_CIA.NOMBRE												    NOMBRE_PROVEEDOR 
		 , DCC.DOCUMENTO														REFERENCIA	
		 , CONVERT(varchar, DCC.CreateDate,101)							        FECHA_MOV
		 , CONVERT(varchar, DCC.CreateDate, 108)							    HORA
		 , DCC.MONTO_LOCAL												        MONTO_CREDITO
		 , DCC.MONTO_LOCAL														MONTO_DEBITO
		 , CASE WHEN CAST(DCC.APLICACION AS VARCHAR(MAX)) = '''' 
		        THEN ''Sin detalle'' 
				ELSE isnull(CAST(DCC.APLICACION AS VARCHAR(MAX)),''Sin detalle'') end  DETALLE 

FROM  EXACTUSERP.DIS86.DOCUMENTOS_CC AS DCC
OUTER APPLY (SELECT NIT, NOMBRE FROM EXACTUSERP.ERPADMIN.CONJUNTO WHERE CONJUNTO = ''DIS86'') AS C_CIA 
INNER JOIN EXACTUSERP.DIS86.CLIENTE AS CLI ON DCC.CLIENTE = CLI.CLIENTE
WHERE  (DCC.DOCUMENTO LIKE ''C0%'' OR DCC.DOCUMENTO  LIKE ''CN%'')';
    -- DIS86.VIEW_REPORTE_COMPROBANTE_APLICACION_V2
    EXEC sys.sp_executesql N'CREATE OR ALTER VIEW  [DIS86].[VIEW_REPORTE_COMPROBANTE_APLICACION_V2]
AS
WITH AUX_AGRUPADO AS
(
    SELECT
        A.CREDITO,
        A.TIPO_CREDITO,
        SUM(A.MONTO_CREDITO) AS MONTO_CREDITO,
        SUM(A.MONTO_DEBITO) AS MONTO_DEBITO
    FROM EXACTUSERP.DIS86.AUXILIAR_CC A
    GROUP BY
        A.CREDITO,
        A.TIPO_CREDITO
),
AUX_PRIMERO AS
(
    SELECT
        A.CREDITO,
        A.TIPO_CREDITO,
        A.CreateDate,
        ROW_NUMBER() OVER (
            PARTITION BY A.CREDITO, A.TIPO_CREDITO
            ORDER BY A.CreateDate
        ) AS RN
    FROM EXACTUSERP.DIS86.AUXILIAR_CC A
),
AUX_FACTURAS AS
(
    SELECT
        X.CREDITO,
        X.TIPO_CREDITO,
        STUFF(
            (
                SELECT DISTINCT '', '' + A2.DEBITO
                FROM EXACTUSERP.DIS86.AUXILIAR_CC A2
                WHERE A2.CREDITO = X.CREDITO
                  AND A2.TIPO_CREDITO = X.TIPO_CREDITO
                FOR XML PATH('''')
            ),
            1, 2, ''''
        ) AS FACTURAS
    FROM
    (
        SELECT DISTINCT
            A.CREDITO,
            A.TIPO_CREDITO
        FROM EXACTUSERP.DIS86.AUXILIAR_CC A
    ) X
)
SELECT
      DCC.CLIENTE AS CLIENTE
    , CLI.NOMBRE AS NOM_CLIENTE
    , CASE
          WHEN DCC.TIPO = ''REC'' THEN ''Recibo''
          WHEN DCC.TIPO = ''N/C'' THEN ''Nota de Crédito''
          WHEN DCC.TIPO = ''O/C'' THEN ''Otro Crédito''
          WHEN DCC.TIPO = ''TEF'' THEN ''T.E.F.''
          WHEN DCC.TIPO = ''DEP'' THEN ''Depósito''
          ELSE ''Comprobante''
      END AS TIPO_MOV
    , C_CIA.NIT AS ID_PROVEEDOR
    , C_CIA.NOMBRE AS NOMBRE_PROVEEDOR
    , DCC.DOCUMENTO AS REFERENCIA
    , DCC.TIPO AS REFERENCIA_TIPO
    , DCC.SALDO
    --, CASE
    --      WHEN DCC.APLICACION = ''ADELANTO'' THEN CONVERT(VARCHAR, DCC.CreateDate, 103)
    --      ELSE CONVERT(VARCHAR, AP.CreateDate, 103)
    --  END AS FECHA_MOV
    --, CASE
    --      WHEN DCC.APLICACION = ''ADELANTO'' THEN CONVERT(VARCHAR, DCC.CreateDate, 108)
    --      ELSE CONVERT(VARCHAR, AP.CreateDate, 108)
    --  END AS HORA
    , CONVERT(VARCHAR, DCC.FECHA, 103) AS FECHA_MOV
    , CONVERT(VARCHAR, DCC.CreateDate, 108) AS HORA
    , CASE
          WHEN DCC.APLICACION = ''ADELANTO'' THEN ISNULL(DCC.MONTO, 0)
          ELSE ISNULL(AA.MONTO_CREDITO, 0)
      END AS MONTO_CREDITO
    , CASE
          WHEN DCC.APLICACION = ''ADELANTO'' THEN ISNULL(DCC.MONTO, 0)
          ELSE ISNULL(AA.MONTO_DEBITO, 0)
      END AS MONTO_DEBITO
    , CASE
          WHEN ISNULL(CAST(DCC.APLICACION AS VARCHAR(MAX)), '''') = ''''
              THEN ''Sin detalle''
          ELSE CAST(DCC.APLICACION AS VARCHAR(MAX))
      END AS DETALLE
    , AF.FACTURAS
    , M.NOMBRE AS MONEDA
    , DCC.NOTAS
    , COB.NOMBRE AS COBRADOR
    , DCC.FORMA_PAGO
    , (
        SELECT MP.DESCRIPCION
        FROM EXACTUSERP.DIS86.FORMAS_PAGO MP
        WHERE MP.CODIGO = DCC.FORMA_PAGO
    ) METODO_PAGO_DESC
FROM EXACTUSERP.DIS86.DOCUMENTOS_CC DCC
OUTER APPLY (SELECT NIT, NOMBRE FROM EXACTUSERP.ERPADMIN.CONJUNTO WHERE CONJUNTO = ''DIS86'') AS C_CIA
INNER JOIN EXACTUSERP.DIS86.CLIENTE CLI
    ON DCC.CLIENTE = CLI.CLIENTE
LEFT JOIN AUX_AGRUPADO AA
    ON AA.CREDITO = DCC.DOCUMENTO
   AND AA.TIPO_CREDITO = DCC.TIPO
LEFT JOIN AUX_PRIMERO AP
    ON AP.CREDITO = DCC.DOCUMENTO
   AND AP.TIPO_CREDITO = DCC.TIPO
   AND AP.RN = 1
LEFT JOIN AUX_FACTURAS AF
    ON AF.CREDITO = DCC.DOCUMENTO
   AND AF.TIPO_CREDITO = DCC.TIPO
LEFT JOIN EXACTUSERP.DIS86.MONEDA M
    ON M.MONEDA = DCC.MONEDA
LEFT JOIN EXACTUSERP.DIS86.COBRADOR COB
    ON COB.COBRADOR = DCC.COBRADOR
--WHERE DOCUMENTO in (''1865202254'', ''044020000030'')';
    COMMIT TRANSACTION;
    SELECT N'COMPLETADO' AS Estado, DB_NAME() AS BaseActual,
           2 AS SP_Corregidos, 2 AS SP_Nuevos, 3 AS Vistas_Corregidas, 2 AS Vistas_Nuevas;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
