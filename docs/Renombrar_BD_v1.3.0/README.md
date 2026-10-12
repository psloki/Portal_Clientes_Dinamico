# Renombrar PS_TRACKING_DINAMICO a PS_PORTAL_CLIENTE — v1.3.0

Actualización v1.3.1: los cuatro scripts de esta misma carpeta incorporan COLLATE explícito en las comparaciones entre contextos de catálogo y en las columnas de texto temporales. Corrige el conflicto Modern_Spanish_CI_AS / SQL_Latin1_General_CP1_CI_AS informado al inspeccionar GRUPOD86. No cambia la collation de las bases ni de las tablas del sistema. Se validó la sintaxis de los cuatro archivos y siete consultas dinámicas con T-SQL 150; falta repetir la ejecución en el servidor real. Cierra o recarga la copia anterior abierta en SSMS y ejecuta nuevamente 00_DIAGNOSTICO.sql completo. No continúes con 01 mientras existan bloqueos.

Los archivos del proyecto ya usan `PS_PORTAL_CLIENTE` donde antes usaban exactamente `PS_TRACKING_DINAMICO`. La base del servidor se cambia únicamente cuando ejecutes el script 01. Coordina ese cambio con la publicación del backend y los reportes actualizados.

## Ejecución manual

1. Obtén un respaldo recuperable de la base y conserva la publicación/configuración anterior del portal. El procedimiento puede actualizar referencias en otras bases, pasos de SQL Agent y bases predeterminadas de logins: conserva también sus definiciones y configuración antes de aplicar.
2. En SSMS, conectado a la instancia correcta como `sysadmin`, abre y ejecuta completo `00_DIAGNOSTICO.sql`. Usa `master` como base de la ventana. Este archivo solo inspecciona el servidor y crea objetos temporales de la sesión.
3. Revisa la tabla de bloqueos: debe estar vacía. Guarda los resultados completos del plan, especialmente `SQL_RESTAURAR_DEFINICION_ORIGINAL`, sin truncar el texto de las definiciones. Si hay bloqueos, resuélvelos antes de continuar; no omitas las comprobaciones del script.
4. Durante una ventana de mantenimiento, detén el portal/IIS, los jobs y otros procesos que puedan conectarse a la base. Cierra ventanas de consulta conectadas a ella y evita abrirla desde Object Explorer. No debe haber cambios simultáneos de objetos en las bases inspeccionadas.
5. Ejecuta completo `01_RENOMBRAR_Y_ACTUALIZAR.sql`. Repite el diagnóstico internamente y aplica el plan calculado contra el servidor real. Debe terminar con `Estado = COMPLETADO`. No desconecta usuarios por la fuerza: si no consigue acceso exclusivo, falla y debes revisar las conexiones antes de reintentar.
6. Publica el backend con los cambios de esta versión, incluidos los ensamblados y la configuración generada del proyecto Core. Publica también los RDL actualizados si usas reportes desplegados en SSRS. Comprueba las conexiones propias del ambiente publicado: las configuraciones externas al repositorio y los orígenes de datos compartidos de SSRS necesitan su actualización correspondiente.
7. Ejecuta `03_VERIFICAR.sql`. Confirma `PS_PORTAL_CLIENTE`, estado `ONLINE`, acceso `MULTI_USER` y `is_read_only = 0`. Revisa `CoincidenciasPendientes`: cualquier fila necesita explicación o corrección antes de dar por terminada la migración.
8. Inicia el portal y prueba inicio de sesión, consultas por compañía, pedidos, pagos, liquidaciones y reportes/PDF. Reactiva los procesos programados después de comprobar su funcionamiento. Si desactivaste estadísticas asíncronas para superar el diagnóstico, restaura su valor original al terminar.

**No ejecutes los 730 archivos de Plantilla.sql y PlantillaDinamico.sql para este cambio.** Son las fuentes actualizadas para futuras instalaciones. El script 01 obtiene los objetos que existen realmente en el servidor y altera las referencias que necesitan el nuevo nombre. Tampoco necesitas repetir `DEPLOY_CIA_v1.2.0.sql`: únicamente se actualizó su base destino para futuras ejecuciones.

## Qué hace el script 01

- Comprueba que exista el origen y que el destino no exista; conserva el identificador de la base.
- Inspecciona módulos SQL en las bases accesibles de la instancia, incluidos SP, vistas, funciones y triggers de base de datos. Reemplaza el nombre completo en sus definiciones mediante `ALTER`, conservando permisos y opciones ANSI_NULLS/QUOTED_IDENTIFIER. Ordena los módulos por sus dependencias conocidas.
- Actualiza los pasos T-SQL de SQL Agent que utilizan el nombre anterior y las bases predeterminadas de los logins afectados.
- Detiene la migración ante objetos que necesitan tratamiento específico: definiciones cifradas, módulos firmados o schema-bound, vistas indexadas, sinónimos afectados, expresiones de tablas afectadas, referencias remotas conocidas, entre otros. No intenta adivinar una transformación segura para ellos.
- Mantiene las actualizaciones del plan en una transacción. El renombrado de la base ocurre fuera de esa transacción; ante un fallo intenta deshacer el plan, recuperar el nombre anterior y restaurar MULTI_USER. Si la recuperación falla, muestra el estado y el error para intervención manual.

El análisis no puede descubrir aplicaciones externas, valores guardados en tablas de configuración ni nombres construidos por concatenación de fragmentos. Tampoco inspecciona el contenido remoto de servidores vinculados. Por eso la comprobación en el servidor y las pruebas funcionales son necesarias; la compilación local no demuestra por sí sola el funcionamiento de producción.

La base separada `PS_TRACKING` conserva su nombre y sus referencias. Las credenciales, la lógica de negocio y la selección de CIA conservan sus valores y comportamiento.

El cambio de nombre no requiere renombrar los archivos físicos o lógicos de datos y log; esos nombres se conservan. Referencia: [Microsoft — Rename a database](https://learn.microsoft.com/sql/relational-databases/databases/rename-a-database).

## Reversión opcional

`02_REVERTIR_RENOMBRADO.sql` **no es un paso normal de la instalación**. Úsalo solamente para volver de `PS_PORTAL_CLIENTE` a `PS_TRACKING_DINAMICO`, durante mantenimiento y sin cambios posteriores que deban preservarse. Inspecciona nuevamente el servidor y aplica la transformación inversa; también puede detenerse ante bloqueos.

Antes de revertir, detén los procesos y conserva los resultados originales del diagnóstico. Después de revertir la base, restaura/publica la versión anterior del backend, su configuración y los reportes. La reversión SQL no modifica los archivos del proyecto ni la publicación web. Las sentencias `SQL_RESTAURAR_DEFINICION_ORIGINAL` conservan las definiciones originales para una recuperación dirigida; no deben ejecutarse fuera de contexto ni sustituir el respaldo.

Si un fallo muestra `CambiosYaConfirmados = 1`, las referencias ya fueron confirmadas: revisa `BaseActual` y el error de recuperación antes de ejecutar cualquier otro archivo. Si la base ya tiene el nombre destino, el script 01 se detiene; no se debe ejecutar repetidamente como si fuera una reparación general.

## Validación local realizada

- Compilación del backend finalizada correctamente; permanecen advertencias existentes.
- 741 archivos existentes comprobados contra su copia original: 783 sustituciones limitadas al nombre de la base, conservando codificación y contenido restante.
- Siete archivos XML de configuración, DBML, RDL y publicación analizados correctamente.
- Sin coincidencias del nombre anterior en los archivos activos revisados; también se examinaron diez archivos RPT en ASCII y UTF-16LE.
- Los cuatro scripts pasan el parser de T-SQL 150 (SQL Server 2019); se analizaron además sus consultas dinámicas de catálogo.
- No se ejecutaron los scripts contra SQL Server. La comprobación semántica del catálogo real y las pruebas funcionales quedan para la ejecución manual.

El detalle de rutas está en [ARCHIVOS_MODIFICADOS.md](ARCHIVOS_MODIFICADOS.md). Los documentos históricos y los scripts de migración contienen intencionalmente el nombre anterior.
