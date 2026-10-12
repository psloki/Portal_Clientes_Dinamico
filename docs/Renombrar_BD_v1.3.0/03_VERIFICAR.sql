-- v1.3.1. Solo lectura. Ejecutar despues de 01_RENOMBRAR_Y_ACTUALIZAR.sql.
USE [master];
GO
SET NOCOUNT ON;
IF ISNULL(IS_SRVROLEMEMBER(N'sysadmin'),0)<>1 THROW 51000,'Ejecutar como sysadmin.',1;
IF DB_ID(N'PS_TRACKING_DINAMICO') IS NOT NULL THROW 51001,'El nombre anterior todavia existe.',1;
IF DB_ID(N'PS_PORTAL_CLIENTE') IS NULL THROW 51002,'La base PS_PORTAL_CLIENTE no existe.',1;
SELECT name,database_id,state_desc,user_access_desc,is_read_only
FROM sys.databases WHERE name=N'PS_PORTAL_CLIENTE';
DROP TABLE IF EXISTS #PR130_Remaining;
CREATE TABLE #PR130_Remaining(DatabaseName SYSNAME COLLATE Latin1_General_100_CI_AS,Category NVARCHAR(50) COLLATE Latin1_General_100_CI_AS,ObjectName NVARCHAR(776) COLLATE Latin1_General_100_CI_AS,Detail NVARCHAR(2000) COLLATE Latin1_General_100_CI_AS);
DECLARE @db SYSNAME,@sql NVARCHAR(MAX),@old SYSNAME=N'PS_TRACKING_DINAMICO';
DECLARE dbs CURSOR LOCAL FAST_FORWARD FOR SELECT name FROM sys.databases WHERE database_id<>2 AND source_database_id IS NULL;
OPEN dbs; FETCH NEXT FROM dbs INTO @db;
WHILE @@FETCH_STATUS=0
BEGIN
    IF DATABASEPROPERTYEX(@db,N'Status')<>N'ONLINE' OR ISNULL(HAS_DBACCESS(@db),0)<>1
        INSERT #PR130_Remaining VALUES(@db,N'NO_REVISADA',NULL,N'Base no accesible.');
    ELSE
    BEGIN
        SET @sql=N'USE '+QUOTENAME(@db)+N';
        INSERT #PR130_Remaining SELECT DB_NAME(),N''MODULO'',COALESCE(QUOTENAME(OBJECT_SCHEMA_NAME(m.object_id)) COLLATE Latin1_General_100_CI_AS+N''.'',N'''')+QUOTENAME(COALESCE(o.name COLLATE Latin1_General_100_CI_AS,t.name COLLATE Latin1_General_100_CI_AS)),N''Contiene texto del nombre anterior.''
        FROM sys.sql_modules m LEFT JOIN sys.objects o ON o.object_id=m.object_id LEFT JOIN sys.triggers t ON t.object_id=m.object_id
        WHERE COALESCE(o.is_ms_shipped,t.is_ms_shipped,1)=0 AND CHARINDEX(@old COLLATE Latin1_General_100_CI_AS,m.definition COLLATE Latin1_General_100_CI_AS)>0;
        INSERT #PR130_Remaining SELECT DB_NAME(),N''DEPENDENCIA'',OBJECT_NAME(referencing_id),N''Dependencia registrada hacia el nombre anterior.'' FROM sys.sql_expression_dependencies WHERE referenced_database_name COLLATE Latin1_General_100_CI_AS=@old COLLATE Latin1_General_100_CI_AS;
        INSERT #PR130_Remaining SELECT DB_NAME(),N''SINONIMO'',QUOTENAME(SCHEMA_NAME(schema_id)) COLLATE Latin1_General_100_CI_AS+N''.''+QUOTENAME(name) COLLATE Latin1_General_100_CI_AS,N''Sinonimo con texto del nombre anterior.'' FROM sys.synonyms WHERE CHARINDEX(@old COLLATE Latin1_General_100_CI_AS,base_object_name COLLATE Latin1_General_100_CI_AS)>0;';
        BEGIN TRY EXEC sys.sp_executesql @sql,N'@old sysname',@old; END TRY
        BEGIN CATCH INSERT #PR130_Remaining VALUES(@db,N'ERROR',NULL,ERROR_MESSAGE()); END CATCH;
    END;
    FETCH NEXT FROM dbs INTO @db;
END;
CLOSE dbs; DEALLOCATE dbs;
INSERT #PR130_Remaining SELECT N'msdb',N'JOB',j.name+N' / paso '+CONVERT(NVARCHAR(12),s.step_id),N'Paso con texto o contexto del nombre anterior.' FROM msdb.dbo.sysjobsteps s JOIN msdb.dbo.sysjobs j ON j.job_id=s.job_id WHERE s.database_name COLLATE Latin1_General_100_CI_AS=@old COLLATE Latin1_General_100_CI_AS OR CHARINDEX(@old COLLATE Latin1_General_100_CI_AS,s.command COLLATE Latin1_General_100_CI_AS)>0;
INSERT #PR130_Remaining SELECT N'master',N'LOGIN',name,N'Base predeterminada anterior.' FROM sys.server_principals WHERE default_database_name COLLATE Latin1_General_100_CI_AS=@old COLLATE Latin1_General_100_CI_AS;
INSERT #PR130_Remaining SELECT N'master',N'TRIGGER_SERVIDOR',t.name,N'Trigger con texto del nombre anterior.' FROM sys.server_sql_modules m JOIN sys.server_triggers t ON t.object_id=m.object_id WHERE t.is_ms_shipped=0 AND CHARINDEX(@old COLLATE Latin1_General_100_CI_AS,m.definition COLLATE Latin1_General_100_CI_AS)>0;
SELECT * FROM #PR130_Remaining ORDER BY DatabaseName,Category,ObjectName;
SELECT COUNT(*) AS CoincidenciasPendientes FROM #PR130_Remaining;
PRINT N'Las coincidencias textuales pueden incluir comentarios o nombres con sufijos. Revisarlas; no sustituir a ciegas. Completar con pruebas funcionales del backend publicado.';
DROP TABLE #PR130_Remaining;
