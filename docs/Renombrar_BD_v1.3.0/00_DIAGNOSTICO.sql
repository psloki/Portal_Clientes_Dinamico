/* v1.3.1. Ejecutar el archivo completo en SSMS, conectado a master como sysadmin.
   No usa ROLLBACK IMMEDIATE ni modifica datos de negocio.
   Detener IIS y procesos que utilicen la base antes de aplicar. */
USE [master];
GO
IF OBJECT_ID(N'tempdb..#PortalRenameReplace') IS NOT NULL DROP PROCEDURE #PortalRenameReplace;
IF OBJECT_ID(N'tempdb..#PortalRenameAlter') IS NOT NULL DROP PROCEDURE #PortalRenameAlter;
DROP TABLE IF EXISTS #PR130_Blockers,#PR130_Modules,#PR130_RawDependencies,#PR130_Plan,#PR130_Edges;
GO
CREATE PROCEDURE #PortalRenameReplace
    @Text NVARCHAR(MAX) OUTPUT, @Old SYSNAME, @New SYSNAME
AS
BEGIN
    IF @Text IS NULL RETURN;
    DECLARE @p INT=1, @hit INT, @before NCHAR(1), @after NCHAR(1);
    WHILE 1=1
    BEGIN
        SET @hit=CHARINDEX(@Old COLLATE Latin1_General_100_CI_AS,@Text COLLATE Latin1_General_100_CI_AS,@p);
        IF @hit=0 BREAK;
        SET @before=CASE WHEN @hit=1 THEN N' ' ELSE SUBSTRING(@Text,@hit-1,1) END;
        SET @after=SUBSTRING(@Text,@hit+LEN(@Old),1);
        IF @before COLLATE Latin1_General_100_BIN2 NOT LIKE N'[A-Za-z0-9_@$#]'
           AND @after COLLATE Latin1_General_100_BIN2 NOT LIKE N'[A-Za-z0-9_@$#]'
        BEGIN
            SET @Text=STUFF(@Text,@hit,LEN(@Old),@New);
            SET @p=@hit+LEN(@New);
        END
        ELSE SET @p=@hit+LEN(@Old);
    END;
END;
GO
CREATE PROCEDURE #PortalRenameAlter
    @Text NVARCHAR(MAX) OUTPUT, @Declared NVARCHAR(776) OUTPUT
AS
BEGIN
    -- Lee la cabecera sin confundir CREATE dentro de comentarios con el DDL real.
    DECLARE @tokens TABLE(n INT IDENTITY PRIMARY KEY,a INT,b INT,v NVARCHAR(776));
    DECLARE @i INT=1,@a INT,@j INT,@depth INT,@ch NCHAR(1),@v NVARCHAR(776),@n INT=0;
    WHILE @i<=LEN(@Text) AND @n<10
    BEGIN
        SET @ch=SUBSTRING(@Text,@i,1);
        IF UNICODE(@ch) IN(9,10,13,32,65279) BEGIN SET @i+=1; CONTINUE; END;
        IF SUBSTRING(@Text,@i,2)=N'--'
        BEGIN SET @j=CHARINDEX(NCHAR(10),@Text,@i+2); SET @i=CASE WHEN @j=0 THEN LEN(@Text)+1 ELSE @j+1 END; CONTINUE; END;
        IF SUBSTRING(@Text,@i,2)=N'/*'
        BEGIN
            SET @depth=1; SET @i+=2;
            WHILE @i<=LEN(@Text) AND @depth>0
            BEGIN
                IF SUBSTRING(@Text,@i,2)=N'/*' BEGIN SET @depth+=1; SET @i+=2; END
                ELSE IF SUBSTRING(@Text,@i,2)=N'*/' BEGIN SET @depth-=1; SET @i+=2; END
                ELSE SET @i+=1;
            END;
            CONTINUE;
        END;
        SET @a=@i;
        IF @ch IN(N'[',N'"')
        BEGIN
            SET @ch=CASE @ch WHEN N'[' THEN N']' ELSE N'"' END; SET @i+=1;
            WHILE @i<=LEN(@Text)
            BEGIN
                IF SUBSTRING(@Text,@i,1)=@ch
                BEGIN
                    IF SUBSTRING(@Text,@i+1,1)=@ch SET @i+=2;
                    ELSE BEGIN SET @i+=1; BREAK; END;
                END
                ELSE SET @i+=1;
            END;
        END
        ELSE IF @ch IN(N'.',N'(',N')',N',',N';') SET @i+=1;
        ELSE
        BEGIN
            WHILE @i<=LEN(@Text) AND UNICODE(SUBSTRING(@Text,@i,1)) NOT IN(9,10,13,32)
              AND SUBSTRING(@Text,@i,1) NOT IN(N'.',N'(',N')',N',',N';') SET @i+=1;
        END;
        INSERT @tokens(a,b,v) VALUES(@a,@i,SUBSTRING(@Text,@a,@i-@a)); SET @n+=1;
    END;
    DECLARE @first NVARCHAR(776),@typeIndex INT=2,@end INT;
    SELECT @first=UPPER(v),@a=a,@end=b FROM @tokens WHERE n=1;
    IF @first=N'CREATE' AND EXISTS(SELECT 1 FROM @tokens WHERE n=2 AND UPPER(v)=N'OR')
    BEGIN
        IF NOT EXISTS(SELECT 1 FROM @tokens WHERE n=3 AND UPPER(v)=N'ALTER') THROW 51010,'Cabecera DDL no reconocida.',1;
        SET @typeIndex=4; SELECT @end=b FROM @tokens WHERE n=3;
    END;
    IF @first NOT IN(N'CREATE',N'ALTER') OR NOT EXISTS
       (SELECT 1 FROM @tokens WHERE n=@typeIndex AND UPPER(v) IN(N'PROC',N'PROCEDURE',N'VIEW',N'FUNCTION',N'TRIGGER'))
        THROW 51011,'Tipo de modulo no reconocido; requiere revision manual.',1;
    SELECT @Declared=v FROM @tokens WHERE n=@typeIndex+1;
    SET @n=@typeIndex+2;
    WHILE EXISTS(SELECT 1 FROM @tokens WHERE n=@n AND v=N'.')
    BEGIN
        SELECT @Declared+=N'.'+v FROM @tokens WHERE n=@n+1;
        SET @n+=2;
    END;
    IF @first=N'CREATE' SET @Text=STUFF(@Text,@a,@end-@a,N'ALTER');
END;
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET LOCK_TIMEOUT 15000;
DECLARE @From SYSNAME=N'PS_TRACKING_DINAMICO', @To SYSNAME=N'PS_PORTAL_CLIENTE', @Apply BIT=0;
DECLARE @DatabaseId INT=DB_ID(@From);
IF ISNULL(IS_SRVROLEMEMBER(N'sysadmin'),0)<>1 THROW 51000,'Ejecutar como sysadmin para revisar todas las bases, jobs y logins.',1;
IF @@TRANCOUNT<>0 THROW 51001,'Ejecutar fuera de una transaccion existente.',1;
IF DB_ID(@From) IS NULL THROW 51002,'La base de origen no existe. Verifique si ya fue renombrada.',1;
IF DB_ID(@To) IS NOT NULL THROW 51003,'El nombre de destino ya existe. No se realizo ningun cambio.',1;

CREATE TABLE #PR130_Blockers(Category NVARCHAR(80) COLLATE Latin1_General_100_CI_AS,DatabaseName SYSNAME COLLATE Latin1_General_100_CI_AS NULL,ObjectName NVARCHAR(776) COLLATE Latin1_General_100_CI_AS NULL,Reason NVARCHAR(2000) COLLATE Latin1_General_100_CI_AS);
CREATE TABLE #PR130_Modules(Id INT IDENTITY PRIMARY KEY,Db SYSNAME COLLATE Latin1_General_100_CI_AS,Obj INT,Name NVARCHAR(776) COLLATE Latin1_General_100_CI_AS,SchemaName SYSNAME COLLATE Latin1_General_100_CI_AS NULL,EntityName SYSNAME COLLATE Latin1_General_100_CI_AS,Def NVARCHAR(MAX) COLLATE Latin1_General_100_CI_AS,Ansi BIT,Quoted BIT,Blocked BIT,DdlTrigger BIT);
CREATE TABLE #PR130_RawDependencies(Db SYSNAME COLLATE Latin1_General_100_CI_AS,Obj INT,RefDb SYSNAME COLLATE Latin1_General_100_CI_AS,RefSchema SYSNAME COLLATE Latin1_General_100_CI_AS NULL,RefEntity SYSNAME COLLATE Latin1_General_100_CI_AS NULL,RefId INT NULL);
CREATE TABLE #PR130_Plan(Id INT IDENTITY PRIMARY KEY,Category VARCHAR(15) COLLATE Latin1_General_100_CI_AS,Db SYSNAME COLLATE Latin1_General_100_CI_AS NULL,Obj INT NULL,Name NVARCHAR(776) COLLATE Latin1_General_100_CI_AS,ForwardSql NVARCHAR(MAX) COLLATE Latin1_General_100_CI_AS,ReverseSql NVARCHAR(MAX) COLLATE Latin1_General_100_CI_AS,ExecutionOrder INT NULL);

INSERT #PR130_Blockers
SELECT N'BASE',name,NULL,N'La base de origen debe estar ONLINE, MULTI_USER, READ_WRITE y fuera de AG/replicacion/mirroring.'
FROM sys.databases d WHERE name COLLATE Latin1_General_100_CI_AS=@From COLLATE Latin1_General_100_CI_AS AND (state<>0 OR user_access<>0 OR is_read_only=1 OR replica_id IS NOT NULL OR is_published=1 OR is_subscribed=1 OR is_merge_published=1
 OR EXISTS(SELECT 1 FROM sys.database_mirroring m WHERE m.database_id=d.database_id AND m.mirroring_guid IS NOT NULL));
INSERT #PR130_Blockers SELECT N'ESTADISTICAS',name,NULL,N'AUTO_UPDATE_STATISTICS_ASYNC esta ON. Revisar y desactivar temporalmente antes de SINGLE_USER; conservar su valor para restaurarlo.' FROM sys.databases WHERE name COLLATE Latin1_General_100_CI_AS=@From COLLATE Latin1_General_100_CI_AS AND is_auto_update_stats_async_on=1;
INSERT #PR130_Blockers SELECT N'SNAPSHOT',name,NULL,N'Existe una instantanea de la base de origen; requiere revision manual.' FROM sys.databases WHERE source_database_id=DB_ID(@From);

DECLARE @db SYSNAME,@sql NVARCHAR(MAX),@params NVARCHAR(MAX)=N'@Old sysname,@New sysname';
DECLARE dbs CURSOR LOCAL FAST_FORWARD FOR SELECT name FROM sys.databases WHERE database_id<>2 AND source_database_id IS NULL ORDER BY database_id;
OPEN dbs; FETCH NEXT FROM dbs INTO @db;
WHILE @@FETCH_STATUS=0
BEGIN
    IF DATABASEPROPERTYEX(@db,N'Status')<>N'ONLINE' OR ISNULL(HAS_DBACCESS(@db),0)<>1
        INSERT #PR130_Blockers VALUES(N'BASE_NO_REVISADA',@db,NULL,N'No se puede inspeccionar esta base; no se garantiza ausencia de referencias externas.');
    ELSE
    BEGIN
        SET @sql=N'USE '+QUOTENAME(@db)+N';
        INSERT #PR130_Modules(Db,Obj,Name,SchemaName,EntityName,Def,Ansi,Quoted,Blocked,DdlTrigger)
        SELECT DB_NAME(),m.object_id,CASE WHEN t.parent_class=0 THEN QUOTENAME(t.name) COLLATE Latin1_General_100_CI_AS ELSE QUOTENAME(OBJECT_SCHEMA_NAME(m.object_id)) COLLATE Latin1_General_100_CI_AS+N''.''+QUOTENAME(o.name) COLLATE Latin1_General_100_CI_AS END,
         OBJECT_SCHEMA_NAME(m.object_id),COALESCE(o.name COLLATE Latin1_General_100_CI_AS,t.name COLLATE Latin1_General_100_CI_AS),m.definition,m.uses_ansi_nulls,m.uses_quoted_identifier,
         CASE WHEN m.is_schema_bound=1 OR m.uses_native_compilation=1 OR EXISTS(SELECT 1 FROM sys.crypt_properties c WHERE c.major_id=m.object_id AND c.class=1)
          OR EXISTS(SELECT 1 FROM sys.indexes i WHERE i.object_id=m.object_id AND i.index_id>0) OR DATABASEPROPERTYEX(DB_NAME(),''Updateability'')<>''READ_WRITE'' THEN 1 ELSE 0 END,
         CASE WHEN t.parent_class=0 THEN 1 ELSE 0 END
        FROM sys.sql_modules m LEFT JOIN sys.objects o ON o.object_id=m.object_id LEFT JOIN sys.triggers t ON t.object_id=m.object_id
        WHERE COALESCE(o.is_ms_shipped,t.is_ms_shipped,1)=0 AND (m.definition IS NULL OR CHARINDEX(@Old COLLATE Latin1_General_100_CI_AS,m.definition COLLATE Latin1_General_100_CI_AS)>0);
        INSERT #PR130_Blockers SELECT N''DESTINO_PREEXISTENTE'',DB_NAME(),OBJECT_NAME(object_id) COLLATE Latin1_General_100_CI_AS,N''Un modulo ya contiene el nombre destino; revisar para que la reversion no altere referencias anteriores.''
         FROM sys.sql_modules WHERE CHARINDEX(@New COLLATE Latin1_General_100_CI_AS,definition COLLATE Latin1_General_100_CI_AS)>0 AND OBJECTPROPERTYEX(object_id,''IsMSShipped'')=0;
        INSERT #PR130_Blockers SELECT N''SINONIMO'',DB_NAME(),QUOTENAME(SCHEMA_NAME(schema_id)) COLLATE Latin1_General_100_CI_AS+N''.''+QUOTENAME(name) COLLATE Latin1_General_100_CI_AS,N''Referencia afectada en sinonimo: requiere recreacion controlada con permisos, propietario y propiedades.'' FROM sys.synonyms WHERE CHARINDEX(@Old COLLATE Latin1_General_100_CI_AS,base_object_name COLLATE Latin1_General_100_CI_AS)>0;
        INSERT #PR130_Blockers SELECT N''EXPRESION'',DB_NAME(),OBJECT_NAME(parent_object_id),N''Referencia en CHECK; requiere revision manual.'' FROM sys.check_constraints WHERE CHARINDEX(@Old COLLATE Latin1_General_100_CI_AS,definition COLLATE Latin1_General_100_CI_AS)>0;
        INSERT #PR130_Blockers SELECT N''EXPRESION'',DB_NAME(),OBJECT_NAME(parent_object_id),N''Referencia en DEFAULT; requiere revision manual.'' FROM sys.default_constraints WHERE CHARINDEX(@Old COLLATE Latin1_General_100_CI_AS,definition COLLATE Latin1_General_100_CI_AS)>0;
        INSERT #PR130_Blockers SELECT N''EXPRESION'',DB_NAME(),OBJECT_NAME(object_id) COLLATE Latin1_General_100_CI_AS+N''.''+name,N''Referencia en columna calculada; requiere revision manual.'' FROM sys.computed_columns WHERE CHARINDEX(@Old COLLATE Latin1_General_100_CI_AS,definition COLLATE Latin1_General_100_CI_AS)>0;
        INSERT #PR130_Blockers SELECT N''CLR'',DB_NAME(),name,N''Modulo CLR en la base de origen: sus referencias internas no son inspeccionables por este script.'' FROM sys.objects WHERE DB_NAME() COLLATE Latin1_General_100_CI_AS=@Old COLLATE Latin1_General_100_CI_AS AND is_ms_shipped=0 AND type IN(''AF'',''FS'',''FT'',''PC'',''TA'');
        INSERT #PR130_Blockers SELECT N''PROCEDIMIENTO_NUMERADO'',DB_NAME(),OBJECT_NAME(object_id) COLLATE Latin1_General_100_CI_AS,N''Grupo de procedimientos numerados: requiere revision manual.'' FROM sys.numbered_procedures WHERE DB_NAME() COLLATE Latin1_General_100_CI_AS=@Old COLLATE Latin1_General_100_CI_AS OR definition IS NULL OR CHARINDEX(@Old COLLATE Latin1_General_100_CI_AS,definition COLLATE Latin1_General_100_CI_AS)>0;
        INSERT #PR130_Blockers SELECT N''REFERENCIA_REMOTA'',DB_NAME(),OBJECT_NAME(referencing_id),N''Referencia de cuatro partes al nombre anterior: confirmar el servidor remoto antes de sustituir.'' FROM sys.sql_expression_dependencies WHERE referenced_server_name IS NOT NULL AND referenced_database_name COLLATE Latin1_General_100_CI_AS=@Old COLLATE Latin1_General_100_CI_AS;
        INSERT #PR130_RawDependencies SELECT DB_NAME(),referencing_id,COALESCE(referenced_database_name COLLATE Latin1_General_100_CI_AS,DB_NAME() COLLATE Latin1_General_100_CI_AS),referenced_schema_name,referenced_entity_name,referenced_id FROM sys.sql_expression_dependencies WHERE referenced_server_name IS NULL;';
        BEGIN TRY EXEC sys.sp_executesql @sql,@params,@Old=@From,@New=@To; END TRY
        BEGIN CATCH INSERT #PR130_Blockers VALUES(N'LECTURA',@db,NULL,ERROR_MESSAGE()); END CATCH;
    END;
    FETCH NEXT FROM dbs INTO @db;
END;
CLOSE dbs; DEALLOCATE dbs;

INSERT #PR130_Blockers SELECT N'TRIGGER_SERVIDOR',N'master',t.name,N'Trigger de servidor afectado o cifrado; requiere revision manual.' FROM sys.server_sql_modules m JOIN sys.server_triggers t ON t.object_id=m.object_id WHERE t.is_ms_shipped=0 AND (m.definition IS NULL OR CHARINDEX(@From COLLATE Latin1_General_100_CI_AS,m.definition COLLATE Latin1_General_100_CI_AS)>0);
INSERT #PR130_Blockers SELECT N'LINKED_SERVER',NULL,name,N'El catalogo del servidor vinculado apunta al nombre anterior; requiere revision manual.' FROM sys.servers WHERE catalog COLLATE Latin1_General_100_CI_AS=@From COLLATE Latin1_General_100_CI_AS;

DECLARE @id INT,@obj INT,@name NVARCHAR(776),@entity SYSNAME,@def NVARCHAR(MAX),@original NVARCHAR(MAX),@ansi BIT,@quoted BIT,@blocked BIT,@ddl BIT,@declared NVARCHAR(776),@actual INT,@forward NVARCHAR(MAX),@reverse NVARCHAR(MAX),@dest SYSNAME;
DECLARE modules CURSOR LOCAL FAST_FORWARD FOR SELECT Id,Db,Obj,Name,EntityName,Def,Ansi,Quoted,Blocked,DdlTrigger FROM #PR130_Modules;
OPEN modules; FETCH NEXT FROM modules INTO @id,@db,@obj,@name,@entity,@def,@ansi,@quoted,@blocked,@ddl;
WHILE @@FETCH_STATUS=0
BEGIN
    IF @def IS NULL INSERT #PR130_Blockers VALUES(N'CIFRADO',@db,@name,N'Definicion no visible; no puede comprobarse ni actualizarse.');
    ELSE
    BEGIN
        SET @original=@def;
        EXEC #PortalRenameReplace @def OUTPUT,@From,@To;
        IF @def<>@original
        BEGIN
            IF @blocked=1 INSERT #PR130_Blockers VALUES(N'MODULO_ESPECIAL',@db,@name,N'Modulo firmado, schema-bound, nativo, vista indexada o base READ_ONLY. No se altera automaticamente.');
            ELSE
            BEGIN TRY
                EXEC #PortalRenameAlter @original OUTPUT,@declared OUTPUT;
                SET @actual=NULL;
                IF @ddl=0
                BEGIN
                    SET @sql=N'USE '+QUOTENAME(@db)+N'; SELECT @Id=OBJECT_ID(@N);';
                    EXEC sys.sp_executesql @sql,N'@N nvarchar(776),@Id int OUTPUT',@N=@declared,@Id=@actual OUTPUT;
                END
                ELSE IF PARSENAME(@declared,1) COLLATE Latin1_General_100_CI_AS=@entity COLLATE Latin1_General_100_CI_AS SET @actual=@obj;
                IF @actual IS NULL OR @actual<>@obj THROW 51012,'La cabecera no coincide con el objeto del catalogo; revisar posibles sp_rename anteriores.',1;
                EXEC #PortalRenameAlter @def OUTPUT,@declared OUTPUT;
                SET @dest=CASE WHEN @db COLLATE Latin1_General_100_CI_AS=@From COLLATE Latin1_General_100_CI_AS THEN @To ELSE @db END;
                -- Opciones originales en el ambito exterior; ALTER es el unico DDL del batch interior.
                SET @forward=CAST(N'USE ' AS NVARCHAR(MAX))+QUOTENAME(@dest)+N'; SET ANSI_NULLS '+CASE @ansi WHEN 1 THEN N'ON' ELSE N'OFF' END+N'; SET QUOTED_IDENTIFIER '+CASE @quoted WHEN 1 THEN N'ON' ELSE N'OFF' END+N'; EXEC sys.sp_executesql N'''+REPLACE(@def,N'''',N'''''')+N''';';
                SET @reverse=CAST(N'USE ' AS NVARCHAR(MAX))+QUOTENAME(@db)+N'; SET ANSI_NULLS '+CASE @ansi WHEN 1 THEN N'ON' ELSE N'OFF' END+N'; SET QUOTED_IDENTIFIER '+CASE @quoted WHEN 1 THEN N'ON' ELSE N'OFF' END+N'; EXEC sys.sp_executesql N'''+REPLACE(@original,N'''',N'''''')+N''';';
                INSERT #PR130_Plan(Category,Db,Obj,Name,ForwardSql,ReverseSql) VALUES('MODULE',@db,@obj,@name,@forward,@reverse);
            END TRY
            BEGIN CATCH INSERT #PR130_Blockers VALUES(N'CABECERA',@db,@name,ERROR_MESSAGE()); END CATCH;
        END;
    END;
    FETCH NEXT FROM modules INTO @id,@db,@obj,@name,@entity,@def,@ansi,@quoted,@blocked,@ddl;
END;
CLOSE modules; DEALLOCATE modules;

DECLARE @job UNIQUEIDENTIFIER,@step INT,@command NVARCHAR(MAX),@jobdb SYSNAME,@subsystem NVARCHAR(40);
DECLARE jobs CURSOR LOCAL FAST_FORWARD FOR
 SELECT s.job_id,s.step_id,j.name,s.command,s.database_name,s.subsystem FROM msdb.dbo.sysjobsteps s JOIN msdb.dbo.sysjobs j ON j.job_id=s.job_id
 WHERE s.database_name COLLATE Latin1_General_100_CI_AS=@From COLLATE Latin1_General_100_CI_AS OR CHARINDEX(@From COLLATE Latin1_General_100_CI_AS,s.command COLLATE Latin1_General_100_CI_AS)>0;
OPEN jobs; FETCH NEXT FROM jobs INTO @job,@step,@name,@command,@jobdb,@subsystem;
WHILE @@FETCH_STATUS=0
BEGIN
    IF @subsystem<>N'TSQL' INSERT #PR130_Blockers VALUES(N'JOB_NO_TSQL',N'msdb',@name,N'Paso no TSQL: revisar sus conexiones/argumentos manualmente.');
    ELSE
    BEGIN
        IF EXISTS(SELECT 1 FROM msdb.dbo.sysjobactivity WHERE job_id=@job AND session_id=(SELECT MAX(session_id) FROM msdb.dbo.syssessions) AND start_execution_date IS NOT NULL AND stop_execution_date IS NULL)
            INSERT #PR130_Blockers VALUES(N'JOB_EN_EJECUCION',N'msdb',@name,N'Detener o esperar el job antes de aplicar.');
        SET @def=@command; EXEC #PortalRenameReplace @def OUTPUT,@From,@To;
        SET @sql=CAST(N'EXEC msdb.dbo.sp_update_jobstep @job_id=N''' AS NVARCHAR(MAX))+CONVERT(NVARCHAR(36),@job)+N''',@step_id='+CONVERT(NVARCHAR(12),@step);
        SET @forward=@sql+N',@command=N'''+REPLACE(@def,N'''',N'''''')+N''''+CASE WHEN @jobdb COLLATE Latin1_General_100_CI_AS=@From COLLATE Latin1_General_100_CI_AS THEN N',@database_name=N'''+REPLACE(@To,N'''',N'''''')+N'''' ELSE N'' END+N';';
        SET @reverse=@sql+N',@command=N'''+REPLACE(@command,N'''',N'''''')+N''''+CASE WHEN @jobdb COLLATE Latin1_General_100_CI_AS=@From COLLATE Latin1_General_100_CI_AS THEN N',@database_name=N'''+REPLACE(@From,N'''',N'''''')+N'''' ELSE N'' END+N';';
        INSERT #PR130_Plan(Category,Db,Name,ForwardSql,ReverseSql) VALUES('JOB',N'msdb',@name+N' / paso '+CONVERT(NVARCHAR(12),@step),@forward,@reverse);
    END;
    FETCH NEXT FROM jobs INTO @job,@step,@name,@command,@jobdb,@subsystem;
END;
CLOSE jobs; DEALLOCATE jobs;
INSERT #PR130_Blockers SELECT N'DESTINO_PREEXISTENTE',N'msdb',j.name,N'Un paso ya contiene el nombre destino; requiere revision para una reversion segura.' FROM msdb.dbo.sysjobsteps s JOIN msdb.dbo.sysjobs j ON j.job_id=s.job_id WHERE s.database_name COLLATE Latin1_General_100_CI_AS=@To COLLATE Latin1_General_100_CI_AS OR CHARINDEX(@To COLLATE Latin1_General_100_CI_AS,s.command COLLATE Latin1_General_100_CI_AS)>0;
INSERT #PR130_Plan(Category,Db,Name,ForwardSql,ReverseSql)
SELECT 'LOGIN',N'master',name,CAST(N'ALTER LOGIN ' AS NVARCHAR(MAX))+QUOTENAME(name) COLLATE Latin1_General_100_CI_AS+N' WITH DEFAULT_DATABASE='+QUOTENAME(@To)+N';',CAST(N'ALTER LOGIN ' AS NVARCHAR(MAX))+QUOTENAME(name) COLLATE Latin1_General_100_CI_AS+N' WITH DEFAULT_DATABASE='+QUOTENAME(@From)+N';' FROM sys.server_principals WHERE default_database_name COLLATE Latin1_General_100_CI_AS=@From COLLATE Latin1_General_100_CI_AS;

-- Orden topologico: primero los modulos de los que dependen otros modulos.
SELECT DISTINCT src.Id AS Src,dst.Id AS Dst INTO #PR130_Edges
FROM #PR130_RawDependencies d JOIN #PR130_Plan src ON src.Category='MODULE' AND src.Db=d.Db AND src.Obj=d.Obj
JOIN #PR130_Modules m ON m.Db=d.RefDb AND ((d.RefId IS NOT NULL AND m.Obj=d.RefId AND d.Db=d.RefDb) OR (m.EntityName=d.RefEntity AND (m.SchemaName=d.RefSchema OR d.RefSchema IS NULL)))
JOIN #PR130_Plan dst ON dst.Category='MODULE' AND dst.Db=m.Db AND dst.Obj=m.Obj WHERE src.Id<>dst.Id;
DECLARE @order INT=0,@next INT;
WHILE EXISTS(SELECT 1 FROM #PR130_Plan WHERE ExecutionOrder IS NULL)
BEGIN
    SET @next=NULL;
    SELECT TOP(1) @next=p.Id FROM #PR130_Plan p WHERE p.ExecutionOrder IS NULL
      AND NOT EXISTS(SELECT 1 FROM #PR130_Edges e JOIN #PR130_Plan q ON q.Id=e.Dst WHERE e.Src=p.Id AND q.ExecutionOrder IS NULL)
      ORDER BY CASE p.Category WHEN 'MODULE' THEN 0 ELSE 1 END,p.Id;
    IF @next IS NULL BEGIN INSERT #PR130_Blockers VALUES(N'DEPENDENCIAS',NULL,NULL,N'Existe un ciclo entre los modulos afectados; requiere orden de migracion manual.'); BREAK; END;
    SET @order+=1; UPDATE #PR130_Plan SET ExecutionOrder=@order WHERE Id=@next;
END;

SELECT @From AS Origen,@To AS Destino,@Apply AS Aplicar,COUNT(*) AS CambiosCatalogo FROM #PR130_Plan;
SELECT * FROM #PR130_Blockers ORDER BY Category,DatabaseName,ObjectName;
SELECT ExecutionOrder,Category,Db,Name,ForwardSql AS SQL_APLICAR,ReverseSql AS SQL_RESTAURAR_DEFINICION_ORIGINAL FROM #PR130_Plan ORDER BY ExecutionOrder;
SELECT session_id,login_name,host_name,program_name FROM sys.dm_exec_sessions WHERE database_id=DB_ID(@From) AND session_id<>@@SPID;
IF @Apply=0
BEGIN
    PRINT N'DIAGNOSTICO: no se cambio la base ni sus objetos. Guardar resultados; resolver todos los bloqueos antes de aplicar.';
    DROP PROCEDURE #PortalRenameReplace; DROP PROCEDURE #PortalRenameAlter;
    RETURN;
END;
IF EXISTS(SELECT 1 FROM #PR130_Blockers) THROW 51004,'Migracion detenida antes del renombrado. Resolver la lista de bloqueos.',1;

DECLARE @renamed BIT=0,@single BIT=0,@committed BIT=0,@current SYSNAME=@From;
BEGIN TRY
    SET @sql=N'ALTER DATABASE '+QUOTENAME(@From)+N' SET SINGLE_USER WITH NO_WAIT;'; EXEC(@sql); SET @single=1;
    SET @sql=N'ALTER DATABASE '+QUOTENAME(@From)+N' MODIFY NAME = '+QUOTENAME(@To)+N';'; EXEC(@sql);
    SET @renamed=1; SET @current=@To;
    IF DB_ID(@To)<>@DatabaseId THROW 51013,'El identificador de la base cambio inesperadamente.',1;
    BEGIN TRANSACTION;
    DECLARE commands CURSOR LOCAL FAST_FORWARD FOR SELECT ForwardSql FROM #PR130_Plan ORDER BY ExecutionOrder;
    OPEN commands; FETCH NEXT FROM commands INTO @sql;
    WHILE @@FETCH_STATUS=0 BEGIN EXEC sys.sp_executesql @sql; FETCH NEXT FROM commands INTO @sql; END;
    CLOSE commands; DEALLOCATE commands;
    COMMIT TRANSACTION; SET @committed=1;
    SET @sql=N'ALTER DATABASE '+QUOTENAME(@To)+N' SET MULTI_USER WITH NO_WAIT;'; EXEC(@sql); SET @single=0;
    SELECT N'COMPLETADO' AS Estado,@To AS BaseActual,COUNT(*) AS ReferenciasActualizadas FROM #PR130_Plan;
END TRY
BEGIN CATCH
    IF XACT_STATE()<>0 ROLLBACK TRANSACTION;
    DECLARE @error NVARCHAR(2048)=ERROR_MESSAGE();
    BEGIN TRY
        IF @renamed=1 AND @committed=0
        BEGIN SET @sql=N'ALTER DATABASE '+QUOTENAME(@To)+N' MODIFY NAME = '+QUOTENAME(@From)+N';'; EXEC(@sql); SET @current=@From; END;
        IF @single=1 BEGIN SET @sql=N'ALTER DATABASE '+QUOTENAME(@current)+N' SET MULTI_USER WITH NO_WAIT;'; EXEC(@sql); END;
    END TRY
    BEGIN CATCH
        SELECT N'RECUPERACION_MANUAL_REQUERIDA' AS Estado,@current AS BaseActual,ERROR_MESSAGE() AS ErrorRecuperacion;
    END CATCH;
    SELECT @committed AS CambiosYaConfirmados,@current AS BaseActual,@error AS ErrorOriginal;
    THROW;
END CATCH;
DROP PROCEDURE #PortalRenameReplace;
DROP PROCEDURE #PortalRenameAlter;
