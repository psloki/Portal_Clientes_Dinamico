USE [PS_PORTAL_CLIENTE]
GO
/****** Object:  StoredProcedure [DIS86].[PROC_EDI_DESCUENTO_CLIENTE_ARTICULO_MASIVO]    Script Date: 10/8/2026 2:25:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [DIS86].[PROC_EDI_DESCUENTO_CLIENTE_ARTICULO_MASIVO]
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

        -- 2) Poblar tabla temporal con los artículos recibidos
        IF OBJECT_ID('tempdb..#Articulos') IS NOT NULL
            DROP TABLE #Articulos;

        CREATE TABLE #Articulos (
            ARTICULO VARCHAR(20) PRIMARY KEY
        );

        INSERT INTO #Articulos(ARTICULO)
        SELECT LTRIM(RTRIM(value))
        FROM STRING_SPLIT(@ARTICULOS_CSV, ',')
        WHERE value <> '';

        -- 3) Eliminar de #Articulos los que no existen en maestro
        SET @SqlValidacion = N'DELETE A FROM #Articulos AS A
        WHERE NOT EXISTS (SELECT 1 FROM EXACTUSERP.' + QUOTENAME(@CIA) + N'.ARTICULO art
                          WHERE art.ARTICULO = A.ARTICULO);';
        EXEC sys.sp_executesql @SqlValidacion;

        -- 4) Si quedó vacío, abortar
        IF NOT EXISTS (SELECT 1 FROM #Articulos)
            THROW 51001, 'Ningún artículo válido en la lista', 1;

        -- 5) Construir nombre de la tabla destino
        DECLARE @Schema NVARCHAR(128) = QUOTENAME(ISNULL(@CIA, ''));
        DECLARE @Tabla  NVARCHAR(256) = N'EXACTUSERP.' + @Schema + N'.[DCTO_ART_X_CLI]';

        -- 6) Actualizar filas existentes
        DECLARE @sql NVARCHAR(MAX) = N'
        UPDATE T
        SET
            CANTIDAD_INICIAL = @CANT_MIN,
            CANTIDAD_FINAL   = @CANT_MAX,
            FECHA_INICIAL    = @FI,
            FECHA_FINAL      = @FF,
            PORC_DESCUENTO   = @PD
        FROM ' + @Tabla + ' AS T
        INNER JOIN #Articulos AS A
            ON T.CLIENTE      = @CL
           AND T.ARTICULO     = A.ARTICULO
           AND T.VERSION_DCTO = 1
           AND T.ESCALA_DCTO  = @ES;';

        EXEC sp_executesql
            @sql,
            N'@CL VARCHAR(20),
              @ES INT,
              @CANT_MIN INT,
              @CANT_MAX INT,
              @FI DATETIME,
              @FF DATETIME,
              @PD INT',
            @CL        = @CLIENTE,
            @ES        = @ESCALA,
            @CANT_MIN  = @CANTIDAD_MIN,
            @CANT_MAX  = @CANTIDAD_MAX,
            @FI        = @FECHA_INI,
            @FF        = @FECHA_FIN,
            @PD        = @PORC_DESCUENTO;

        -- 7) Insertar los que no estaban
        SET @sql = N'
        INSERT INTO ' + @Tabla + ' (
            CLIENTE, ARTICULO, ESCALA_DCTO, VERSION_DCTO,
            CANTIDAD_INICIAL, CANTIDAD_FINAL,
            FECHA_INICIAL, FECHA_FINAL,
            PORC_DESCUENTO, TIPO_DESCUENTO
        )
        SELECT
            @CL, A.ARTICULO, @ES, 1,
            @CANT_MIN, @CANT_MAX,
            @FI, @FF,
            @PD, ''P''
        FROM #Articulos AS A
        LEFT JOIN ' + @Tabla + ' AS T
            ON T.CLIENTE      = @CL
           AND T.ARTICULO     = A.ARTICULO
           AND T.VERSION_DCTO = 1
           AND T.ESCALA_DCTO  = @ES
        WHERE T.ARTICULO IS NULL;';

        EXEC sp_executesql
            @sql,
            N'@CL VARCHAR(20),
              @ES INT,
              @CANT_MIN INT,
              @CANT_MAX INT,
              @FI DATETIME,
              @FF DATETIME,
              @PD INT',
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
END
