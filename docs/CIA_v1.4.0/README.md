# APPLY v1.4.0 — CIA del portal

Compañía actual: **DIS86**. ANCLA no se usa como compañía predeterminada. GLOBAL y todas sus definiciones se conservaron sin cambios.

## Ejecución para DIS86

1. Respaldar la base PS_PORTAL_CLIENTE. Abrir y ejecutar **01_APLICAR.sql completo** en SSMS sobre esa base. El lote usa una transacción: un error revierte los cambios de este paquete. No modifica datos de negocio.
2. Publicar el backend de esta versión, `PlantillaDataContext.Cia.cs` compilado en Plantilla.core.dll, Web.config y `RPservices/ReporteComprobantePago.rdl`. Mantener las conexiones/credenciales del ambiente. Solo se agregaron las opciones `CompaniaPredeterminada=DIS86` y `LogoComprobante.DIS86=~/img/Cliente/cliente.png`.
3. Ejecutar **02_VERIFICAR.sql**. Revisar objetos FALTA, `DependenciasConError` y referencias a una compañía diferente. El verificador solo lee metadatos y compila consultas sin filas; no inserta documentos, visitas ni pagos.
4. Iniciar sesión nuevamente y probar los catálogos, pedidos, bonificaciones/descuentos masivos, comprobantes y registro de visitas. La sesión debe contener CIA=DIS86.

**No ejecutar nuevamente los 63, los 27 ni toda PlantillaDinamico.sql.** Este paquete contiene exactamente los objetos indicados abajo.

`03_PREPARAR_DEPENDENCIAS_CIA.sql` queda en **vista previa (`@Aplicar=0`)**. No es un segundo despliegue obligatorio: úsalo si el verificador detecta dependencias faltantes o para preparar otra compañía. Crea únicamente objetos ausentes; conserva los existentes. Las tablas/esquemas de la compañía y sus dependencias externas deben existir previamente. No crea compañías en Exactus ni modifica PS_TRACKING. Si faltan tablas/columnas o falla la compilación de una vista, revierte el lote y muestra el objeto afectado. Si una vista existente apunta a otra compañía, conservarla no la corrige: requiere revisión de su definición antes de habilitar esa CIA.

## Objetos de 01_APLICAR.sql

SP corregidos (aunque ya se hayan ejecutado antes, estos dos deben actualizarse):

- DIS86.PROC_EDI_DESCUENTO_CLIENTE_ARTICULO_MASIVO: la validación de cliente y artículos usa @CIA; se conserva su lógica de actualización/inserción.
- DIS86.PROC_EDI_BONIFICACION_CLIENTE_ARTICULO_MASIVO: misma corrección de validaciones por @CIA.

SP nuevos:

- DIS86.PROC_OBT_CATALOGO_CIA: catálogo autorizado por lista cerrada, nombre de esquema protegido con QUOTENAME; sustituye 35 lecturas directas de 21 tipos de vista en Managers.
- DIS86.PROC_OBT_COMPROBANTE_PAGO_CIA: documento parametrizado, vista de la CIA y datos fiscales desde ExactusERP.ERPADMIN.CONJUNTO.

Vistas nuevas:

- DIS86.VIEW_OBTENER_HORA_CORTE: equivalente por compañía de dbo.VIEW_OBTENER_HORA_CORTE.
- DIS86.VIEW_OBT_ESTADO_AUTORIZA_PEDIDO: equivalente por compañía de dbo.DIS86_VIEW_ESTADO_AUTORIZA_PEDIDO.

Vistas corregidas:

- DIS86.VIEW_OBT_PEDIDO_PTC: referencia explícita a la vista de autorizaciones de la compañía.
- DIS86.VIEW_REPORTE_COMPROBANTE_PAGO: nombre y cédula fiscal proceden de CONJUNTO.
- DIS86.VIEW_REPORTE_COMPROBANTE_APLICACION_V2: misma corrección de datos fiscales.

Las vistas dbo originales se conservaron. GLOBAL.PROC_OBT_CREDITOS_CLIENTE, el tercer SP facilitado por el usuario, se conserva: ya recibe CIA.

## Código

- Controller → Manager → DataContext → SP: CIA procede de la sesión. Las llamadas del frontend mantienen sus rutas y respuestas; no necesitan enviar una compañía distinta desde JavaScript.
- Nuevos imports/extensiones en la clase parcial PlantillaDataContext.Cia.cs; no se regeneraron los grandes archivos DBML/designer existentes.
- Los catálogos de usuarios y roles que consultan exclusivamente GLOBAL se conservan compartidos.
- Se eliminó el respaldo silencioso a ANCLA/DIS86 en las consultas posteriores al login. Una sesión sin compañía vuelve al login; la compañía inicial se obtiene de Web.config.
- El comprobante RDL deja de consultar ANCLA y obtiene nombre, identificación, dirección y logo según CIA. El logo de ANCLA incrustado fue retirado. Si otra CIA no tiene `LogoComprobante.<CIA>` configurado, no se muestra un logo ajeno.
- `DIS86` en el nombre de un SP/import es su punto de entrada estable; @CIA decide el esquema de los datos. No es necesario duplicar esos SP por compañía. Las vistas y funciones sí son objetos por esquema: SQL Server no admite parámetros en las vistas. El instalador proporciona sus definiciones por CIA.
- Las llamadas a DIS86.TF_SPLIT se conservan: es una utilidad de separación de texto y no consulta datos de compañías. DIS86rdp es el servidor/carpeta de infraestructura, no una CIA a sustituir.

## Verificación realizada y límites

- Solución compilada correctamente con MSBuild. Advertencias preexistentes de versiones de dependencias y archivo Settings duplicado.
- 311 lotes/definiciones validados con TSql150Parser (compatible con SQL Server 15.x de las capturas), incluyendo plantillas para identificadores con caracteres especiales.
- 40 consultas dinámicas generadas de los cuatro SP validadas: sintaxis y parámetros enlazados.
- Mapping LINQ comprobado en runtime: 21 tipos de resultado, sin abrir conexión SQL.
- RDL renderizado a PDF con datos ficticios, tanto sin logo como con el logo actual de DIS86; nombre/identificación/dirección provienen del dataset.
- Hashes de todos los archivos GLOBAL en las dos carpetas principales idénticos al inicio. Credenciales y strings de conexión sin cambios.
- No se ejecutaron scripts ni pruebas funcionales contra SQL Server. La revisión se refiere a las fuentes disponibles; 02_VERIFICAR y las pruebas del ambiente siguen pendientes.

**Pendiente concreto: reportes Crystal (.rpt).** El código heredado de RegistroPagos y Tracking carga reportes Crystal y asigna parámetros de documento/pedido, pero no asigna CIA. Los archivos no pudieron inspeccionarse con su motor: el runtime Crystal de este equipo no está operativo, aun cargando las bibliotecas del proyecto. No se modificaron a ciegas ni se certifican como dinámicos. Deben revisarse sus tablas/comandos/subreportes en un equipo con Crystal operativo antes de afirmar aislamiento total. Los nombres de archivo `ReporteFacturas_ANCLA_*` son nombres heredados; por sí solos no identifican el origen real de datos.

Los archivos históricos y diseños no consumidos por el flujo RDL modificado no se reescribieron. Las rutas compartidas heredadas de adjuntos/PDF tampoco quedan certificadas como aislamiento completo entre compañías por esta revisión de consultas SQL.
