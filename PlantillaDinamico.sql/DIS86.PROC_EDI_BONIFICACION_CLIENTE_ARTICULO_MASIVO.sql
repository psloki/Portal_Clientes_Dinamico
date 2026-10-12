USE [PS_PORTAL_CLIENTE]
GO
/****** Object:  StoredProcedure [DIS86].[PROC_EDI_BONIFICACION_CLIENTE_ARTICULO_MASIVO]    Script Date: 10/8/2026 2:29:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [DIS86].[PROC_EDI_BONIFICACION_CLIENTE_ARTICULO_MASIVO]
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
    SET @Schema = QUOTENAME(ISNULL(@CIA, ''));
    SET @Tabla  = N'EXACTUSERP.' + @Schema + N'.[BONIF_ART_X_CLI]';

    IF NULLIF(LTRIM(RTRIM(@CIA)), '') IS NULL
        THROW 51002, 'CIA es obligatoria', 1;

    BEGIN TRAN;
    BEGIN TRY
        -- 1) Validar cliente
        DECLARE @ClienteExiste BIT = 0;
        DECLARE @SqlValidacion NVARCHAR(MAX) = N'SELECT @Existe = CASE WHEN EXISTS
            (SELECT 1 FROM EXACTUSERP.' + QUOTENAME(@CIA) + N'.CLIENTE WHERE CLIENTE = @Cliente)
            THEN 1 ELSE 0 END;';
        EXEC sys.sp_executesql @SqlValidacion,
            N'@Cliente VARCHAR(20), @Existe BIT OUTPUT',
            @Cliente = @CLIENTE, @Existe = @ClienteExiste OUTPUT;
        IF @ClienteExiste = 0
            THROW 51000, 'Cliente no existe', 1;

        -- 2) Poblar tabla temporal
        IF OBJECT_ID('tempdb..#Articulos') IS NOT NULL
            DROP TABLE #Articulos;
        CREATE TABLE #Articulos(ARTICULO VARCHAR(20) PRIMARY KEY);
        INSERT INTO #Articulos(ARTICULO)
        SELECT LTRIM(RTRIM(value))
        FROM STRING_SPLIT(@ARTICULOS_CSV, ',')
        WHERE value <> '';
        SET @SqlValidacion = N'DELETE A FROM #Articulos AS A
        WHERE NOT EXISTS (SELECT 1 FROM EXACTUSERP.' + QUOTENAME(@CIA) + N'.ARTICULO art
                          WHERE art.ARTICULO = A.ARTICULO);';
        EXEC sys.sp_executesql @SqlValidacion;
        IF NOT EXISTS (SELECT 1 FROM #Articulos)
            THROW 51001, 'Ningún artículo válido en la lista', 1;

        -- 3) Forzar actualización de todos los registros seleccionados
        SET @sql = N'
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
        FROM ' + @Tabla + N' AS T
        INNER JOIN #Articulos AS A
          ON T.CLIENTE = @CL
         AND T.ARTICULO = A.ARTICULO
         AND T.VERSION_BONIF = 1;';

        EXEC sp_executesql
          @sql,
          N'@CL VARCHAR(20), @ES INT, @CANT_MIN INT, @CANT_MAX INT,
            @FI DATETIME, @FF DATETIME, @FA INT, @ABI VARCHAR(20), @UNI INT',
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
        SET @sql = N'
        INSERT INTO ' + @Tabla + N' (
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
        LEFT JOIN ' + @Tabla + N' T
          ON T.CLIENTE = @CL
         AND T.ARTICULO = A.ARTICULO
         AND T.VERSION_BONIF = 1
         AND T.ESCALA_BONIF = @ES
        WHERE T.ARTICULO IS NULL;';

        EXEC sp_executesql
          @sql,
          N'@CL VARCHAR(20), @ES INT, @CANT_MIN INT, @CANT_MAX INT,
            @FI DATETIME, @FF DATETIME, @FA INT, @ABI VARCHAR(20), @UNI INT',
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
END
