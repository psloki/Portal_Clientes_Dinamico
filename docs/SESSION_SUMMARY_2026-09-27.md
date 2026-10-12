# Contexto de sesión

## APPLY v1.3.1 — corrección de collations

- Fecha: 2026-09-27, continuación de la sesión; el usuario ejecutó únicamente el diagnóstico y reportó un bloqueo LECTURA en GRUPOD86 por collations Modern_Spanish_CI_AS / SQL_Latin1_General_CP1_CI_AS.
- Proceso: corrección aprobada mediante APPLY v1.3.1. Se agregaron collations explícitas a comparaciones de nombres de catálogo, expresiones que combinan metadatos y columnas textuales temporales. No se modificó backend, datos ni collations persistentes.
- Archivos modificados (rutas completas):
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/00_DIAGNOSTICO.sql
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/01_RENOMBRAR_Y_ACTUALIZAR.sql
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/02_REVERTIR_RENOMBRADO.sql
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/03_VERIFICAR.sql
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/README.md
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/SESSION_SUMMARY_2026-09-27.md
- Validación: cuatro scripts y siete consultas dinámicas de catálogo analizados correctamente con TSql150Parser. No se ejecutó SQL contra el servidor real.
- Pendiente: recargar archivo actualizado en SSMS y repetir 00; continuar con 01 únicamente sin bloqueos y con los requisitos de mantenimiento cumplidos. CambiosCatalogo=0 del diagnóstico fallido anterior no constituye una auditoría completa.
- Nota: la plantilla generadora temporal v1.3.0 no es la fuente de esta corrección; no regenerar los scripts desde esa copia antigua.

## Estado anterior v1.3.0

- Fecha y hora: 2026-09-27 19:48, America/Costa_Rica.
- Feature/Proceso en curso: APPLY v1.3.0 aprobado; preparar renombrado manual de PS_TRACKING_DINAMICO a PS_PORTAL_CLIENTE y actualizar referencias del proyecto. Implementación local terminada; migración en servidor pendiente de ejecución por el usuario.
- Archivos modificados (rutas completas): inventario exhaustivo de los 741 archivos existentes en C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/ARCHIVOS_MODIFICADOS.md. Incluye diez archivos de aplicación, 520 SQL de Plantilla.sql, 210 SQL de PlantillaDinamico.sql y docs/DEPLOY_CIA_v1.2.0.sql. Se realizaron 783 sustituciones exactas, preservando el resto de los bytes.
- Archivos nuevos:
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/00_DIAGNOSTICO.sql
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/01_RENOMBRAR_Y_ACTUALIZAR.sql
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/02_REVERTIR_RENOMBRADO.sql
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/03_VERIFICAR.sql
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/README.md
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/Renombrar_BD_v1.3.0/ARCHIVOS_MODIFICADOS.md
  - C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/SESSION_SUMMARY_2026-09-27.md
- Próximos pasos / pendientes: el usuario debe respaldar, ejecutar 00 y resolver bloqueos, detener procesos/conexiones, ejecutar 01 desde master como sysadmin, publicar backend/configuración/reportes actualizados, ejecutar 03 y pruebas funcionales. No ejecutar nuevamente todos los archivos fuente SQL. El archivo 02 es solo reversión opcional y exige volver también a la publicación anterior.
- Decisiones tomadas: conservar PS_TRACKING y ExactusERP; reemplazar solo el nombre completo PS_TRACKING_DINAMICO. Mantener lógica CIA y credenciales. Alterar módulos del servidor real conservando permisos/opciones; orden de dependencias; actualizar pasos T-SQL de Agent y default_database de logins. Usar SINGLE_USER WITH NO_WAIT, sin expulsar sesiones. Detenerse ante cifrado, firmas, schema binding, vistas indexadas, sinónimos afectados, referencias remotas conocidas y otros casos no transformables automáticamente. Renombrado fuera de transacción con intento de recuperación; plan de referencias dentro de transacción. No renombrar archivos físicos/lógicos.
- Problemas conocidos / hallazgos: no hay ejecución real contra SQL Server ni validación funcional de producción. Referencias externas, valores de tablas y nombres ensamblados por fragmentos pueden escapar del análisis. El servidor de la captura es 15.x; scripts analizados con parser T-SQL 150. Publicación SSRS/configuración externa requiere actualización en su ambiente. Compilación local correcta con advertencias existentes (dependencias, archivo Settings.Designer.cs repetido, campo no usado). Cuatro scripts y sus consultas dinámicas de catálogo pasan análisis sintáctico; siete XML válidos; diez RPT examinados; sin nombre anterior en fuentes activas revisadas. Documentos históricos y scripts de migración lo conservan intencionalmente.
- Evidencia y recuperación local: C:/Users/produ/.codex/visualizations/2026/09/25/01a0d9a5-09e8-7910-bf18-3f322de3b956/rename_v130_original contiene las copias previas. rename_v130_changes.json contiene hashes y conteos; rename_v130_validation.json contiene la auditoría; build_v130.log contiene la compilación. Estos son respaldos de archivos locales, no de la base de datos.
- Contexto anterior: v1.2.0 ya aplicado por el usuario en SQL; sus 27 SP complementan los 63 iniciales. PlantillaDinamico.sql tiene 210 scripts (206 llamadas directas más 4 internas). Esta versión no cambia su comportamiento CIA, únicamente el nombre de la base. Vistas auditadas anteriormente sin modificaciones de lógica.

## APPLY v1.3.2 — salida de registro de visitas

- Fecha y hora: continuación posterior al log del 2026-09-27 21:37 (hora exacta de aplicación no registrada).
- Proceso: corregido OUTPUT faltante en la llamada exterior a sp_executesql de GLOBAL.PROC_INS_VISITA. El log registraba Input string was not in a correct format; el Manager convierte IdVisita vacío a entero.
- Archivos modificados:
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_INS_VISITA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_VISITA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/DEPLOY_CIA_v1.2.0.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/DEPLOY_VISITA_v1.3.2.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/SESSION_SUMMARY_2026-09-27.md
- Decisiones: único cambio funcional @ID_VISITA = @ID_VISITA OUTPUT. Sin cambios C#/DBML ni lógica de inserción. Bundle anterior sincronizado para futuras instalaciones; NO repetirlo para esta corrección.
- Validación: los cuatro SQL pasan TSql150Parser; contrato DBML ref string y lectura del parámetro 13 confirmados. No se ejecutó SQL real.
- Pendientes: ejecutar únicamente docs/DEPLOY_VISITA_v1.3.2.sql en PS_PORTAL_CLIENTE; revisar posibles visitas ya insertadas durante los intentos fallidos antes de repetir. Prueba funcional pendiente.
- Hallazgos: usuario confirmó renombrado COMPLETADO y verificación ONLINE/MULTI_USER, lectura-escritura, cero coincidencias pendientes. El defecto de salida corresponde a la adaptación CIA anterior.
