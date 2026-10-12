# Contexto de sesión — APPLY v1.1.0

- Fecha y hora: 2026-10-11 12:02 (hora local, Costa Rica).
- Feature/Proceso en curso: bloqueo temporal de acciones para evitar solicitudes repetidas, aprobado por el usuario con `APPLY v1.1.0`.
- Archivos modificados (rutas completas):
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Scripts\RegistroPagos\Index.js`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Views\RegistroPagos\Index.cshtml`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Scripts\Visitas\Index.js`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Scripts\Visitas\Documento.js`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Scripts\ExpedienteDigital\Index.js`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Views\ExpedienteDigital\Index.cshtml`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Scripts\ArticuloPrecio\Index.js`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Views\ArticuloPrecio\Index.cshtml`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Scripts\Descuentos\Index.js`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Scripts\DescuentoCliente\Index.js`
  - `C:\Users\produ\OneDrive\Documents\GitHub\PS_Portal_Clientes_Dinamico\Plantilla.web\Views\DescuentoCliente\Index.cshtml`
  - Este archivo de contexto.
- Próximos pasos / pendientes: ejecutar validación manual en navegador sobre los flujos de Registro de pagos, documentos, visitas, precios y descuentos cuando el usuario lo solicite. No se ejecutaron pruebas en este paso.
- Decisiones tomadas: los botones de guardar e imprimir/descargar recibos y adelantos se bloquean en pareja hasta completar la cadena de registro; el bloqueo se libera en `finally` o en el callback `complete` de `Dajax`. Los enlaces de visitas que abren otra pestaña solo pueden bloquearse durante una ventana corta (1.2 s), porque la vista de origen no conoce cuándo termina la página nueva. Las cargas Excel existentes se conservaron y se agregó reactivación en `finally` para Artículo precio y Descuento cliente. No se modificó SQL.
- Problemas conocidos / hallazgos: `guardarCincoDescuento()` en DescuentoCliente solo cierra el modal y muestra un mensaje; no realiza solicitud al servidor, por lo que no hay operación asíncrona que bloquear. El botón `guardarEntrega()` aparece en RegistroPagos, pero su definición se encuentra en `EstadosCuenta/Index.js` y no en RegistroPagos; se dejó intacto por ser un hallazgo incidental y no estar claro que esa lógica se cargue en esa pantalla. Liquidación ya tenía bloqueo con liberación en `finally`, por lo que no requirió cambios.
