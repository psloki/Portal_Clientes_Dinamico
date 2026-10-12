# Contexto de sesión — 2026-09-26

- Fecha y hora: 2026-09-26 16:38:10 (America/Costa_Rica).
- Feature/Proceso en curso: auditoría global e implementación CIA v1.2.0, autorizada y completada en archivos locales.
- El usuario confirmó pruebas funcionales satisfactorias de v1.1.0 y que ya ejecutó los 63 SP previos. Solicitó identificar exactamente los SP de esta nueva versión para ejecución manual.

## Decisiones tomadas

- Inventario de código compilado: 206 SP directos y 4 dependencias SQL.
- Plantilla.sql es la fuente de SP, vistas y funciones. PlantillaDinamico.sql se completó con 147 SP hasta llegar a 210.
- Se corrigieron 27 SP (26 con CIA existente pero referencias fijas, más un SP interno sin CIA) y seis asignaciones de CIA en cuatro Controllers.
- Guardas de CIA, QUOTENAME y parámetros de sp_executesql; se conservan contratos, OUTPUT, RETURN, transacciones y resultados.
- Se preservan los 63 archivos SQL previos. No se agregaron NOLOCK ni se modificaron vistas/funciones, DBML o Managers.
- Bundle manual docs/DEPLOY_CIA_v1.2.0.sql: solo los 27 SP corregidos, dependencia interna primero.

## Archivos modificados (rutas completas)

- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_EDI_ARTICULO_PRECIO_CLIENTE_ARTICULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_EDI_BONIFICACION_CLIENTE_ARTICULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_EDI_DESCUENTO_CLIENTE_ARTICULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_EDI_ESCALA_BONIFICACION.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_EDI_PORCENTAJE_DESCUENTO_MASIVO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_OBT_CATEGORIA_CLIENTE_NIVEL_PRECIO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_OBT_CLIENTE_COBRADOR_VENDEDOR.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_OBT_CLIENTE_FILTROS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_OBT_INFORMACION_ARTICULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_OBT_INFORMACION_ARTICULO_DCTO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_OBT_NIVEL_PRECIO_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PCL_PROC_ENVIAR_APLICACION_NC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PCL_PROC_ENVIAR_ESTADO_CUENTA_CLIENTE_CORPORATIVO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PCL_PROC_ENVIAR_FACTURA_AL_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PCL_PROC_OBT_SALDO_DOC_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_EDI_PEDIDO_LINEA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_INS_DOCUMENTOS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_INS_DOCUMENTOS_DET.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_INS_DOCUMENTOS_V3_MULTI.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_INS_DOCUMENTO_FUSION.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_INS_PEDIDO_LINEA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_INS_VISITA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_OBT_DOCUMENTO_ESTADO_CUENTA_BANCARIO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_PEDIDO_MIGRAR_SOFTLAND.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/GLOBAL.PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE_DEP.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.sql/DIS86.PROC_INSERTAR_LINEA_BONIFICADA_PEDIDO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.web/Controllers/ArticuloPrecioController.cs
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.web/Controllers/BonificacionesClienteController.cs
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.web/Controllers/BonificacionesController.cs
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/Plantilla.web/Controllers/DescuentoClienteController.cs

### Archivos agregados

- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_ARTICULO_CLIENTE_BONIFICADO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_ARTICULO_CLIENTE_BONIFICADO_MASIVO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_ARTICULO_PRECIO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_ARTICULO_PRECIO_CLIENTE_ARTICULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_ARTICULO_PRECIO_MASIVO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_BONIFICACION_CLIENTE_ARTICULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_DESCUENTO_ARTICULO_PRECIO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_DESCUENTO_CLIENTE_ARTICULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_ESCALA_BONIFICACION.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_PORCENTAJE_DESCUENTO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_PORCENTAJE_DESCUENTO_MASIVO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_SEGUI_FACTURA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_SEGUI_PEDIDO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_EDI_SUCESO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_ELI_DESCUENTO_ARTICULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_INS_SEGUI_FACTURA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_INS_SEGUI_PEDIDO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_INS_SUCESO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE_V2.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_BONIF_ART_CLIENTE_INDEX_V2.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_CASA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_CATEGORIA_CLIENTE_NIVEL_PRECIO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_CLIENTE_ARTICULO_BONIFICADO_INDEX_V2.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEX.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEX_V2.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_CLIENTE_COBRADOR_VENDEDOR.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_CLIENTE_FILTROS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_DCTO_ART_CLIENTE_INDEX_V2.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_EMAIL_CLIENTE_POR_COBRADOR.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_EMAIL_COBRADOR.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_INFORMACION_ARTICULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_INFORMACION_ARTICULO_DCTO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_MARCA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_NIVEL_ARTICULO_DESCUENTO_INDEX.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_NIVEL_PRECIO_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_SUBCASA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_TABLA_DOCUMENTOS_POR_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_OBT_VENDEDOR.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_ENVIAR_APLICACION_NC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_ENVIAR_ESTADO_CUENTA_CLIENTE_CORPORATIVO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_ENVIAR_FACTURA_AL_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_OBT_CORREO_CP_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUE_POSTFECHADO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_EFECTIVO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_SUB_INDEX.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PCL_PROC_OBT_SALDO_DOC_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_DEL_EXPEDIENTE_DIGITAL_PP.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_DEL_PEDIDO_LINEA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_CLIENTE_PEDIDO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_CONCILIACION_BANCARIA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_DOCUMENTO_BANCO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_DOCUMENTO_FUSION.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_MT940_NUMERO_BANCO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_NIVEL_PRECIO_DESTINO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_OCUPA_FACTURA_PEDIDO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_PEDIDO_LINEA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_PERMISOS_V2.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_EDI_PERMISOS_VERSION_NUEVA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INSERTAR_PEDIDO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INSERTAR_USUARIO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_DOCUMENTOS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_DOCUMENTOS_DET.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_DOCUMENTOS_V3_MULTI.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_DOCUMENTO_CC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_DOCUMENTO_CP.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_DOCUMENTO_FUSION.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_DOCUMENTO_MOV_BANCOS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_DOCUMENTO_MOV_BANCOS_NUEVO_DOC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_DOCUMENTO_MOV_REPORTADOS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_ESTADO_CUENTA_EXCEL.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_EXPEDIENTE_DIGITAL_PP.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_INTERESES_MORA_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_PEDIDO_LINEA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_INS_VISITA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_ARTICULOS_TODOS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_BANCO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_BODEGAS_TODAS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_CLIENTES_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_CLIENTE_CC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_CLIENTE_COBRADOR.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_CLIENTE_X_ID.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_COBRADOR_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_COBRADOR_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_COMPROBANTE_APLICACION.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_COMPROBANTE_APLICACION_DATOS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_CONCILIACIONES_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_CONCILIACIONES_TODAS_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_CONSECUTIVO_DOCUMENTOS_CC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_CUENTAS_BANCARIAS_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_CUENTAS_X_COBRAR.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_DOCUMENTOS_CLIENT_PROVEEDOR_EXPEDIENTE_DIGITAL.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_DOCUMENTOS_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_DOCUMENTOS_INDEX_CC_V2.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_DOCUMENTOS_INDEX_CC_V3.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_DOCUMENTOS_TOTAL_CC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_DOCUMENTO_ESTADO_CUENTA_BANCARIO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_DOC_CXC_EXPEDIENTE_DIGITAL.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_ESTADO_CUENTA_BANCARIO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_ESTADO_CUENTA_BANCARIO_PARA_ACCIONES.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_EXPEDIENTE_DIGITAL_PP.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_INDEX_KARDEX.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_INFORMACION_USUARIO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_INTERESES_MORA_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_MONEDA_X_CUENTA_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_NUM_DOC_EXPEDIENTE_DIGITAL.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_PERMISOS_ROL_MODULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_PERMISOS_ROL_MODULO_SUB.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_PERMISOS_ROL_V2.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_PERMISOS_V2.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_PERMISOS_X_SUBMODULO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_PROVEEDORES_CLIENTE_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_PROVEEDORES_CLIENTE_POR_DOCUMENTO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_PROVEEDORES_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_PROVEEDOR_EXPEDIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_ROLES.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SELECT_TIPO_EXPEDIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SELECT_USUARIOS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBMODULOS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPOS_CC_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPOS_CC_X_TIPO_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPOS_CONTROL_BANCARIO_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPOS_CP_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPOS_CP_X_TIPO_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPOS_DOCUMENTOS_CC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPOS_TODOS_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPOS_X_TIPO_CONTROL_BANCARIO_FILTRO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPO_DOC_CC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_SUBTIPO_MODULO_EXP.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_TIPOSINV.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_TIPOS_DOCUMENTOS_CC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_TIPOS_EXPEDIENTE_DIGITAL_PP.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_TIPO_CAMBIO_MONEDA_X_FECHA.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_TIPO_DOC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_TIPO_DOC_CC.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_TIPO_DOC_DETALLE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_USUARIO_MANTENIMIENTO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBT_VENDEDOR_CLIENTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_PEDIDO_MIGRAR_SOFTLAND.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_VERIFICAR_FUSION_DOCUMENTOS.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_VERIFICAR_SI_CONSECUTIVO_NUEVO_EXISTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE_DEP.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/DIS86.PROC_INSERTAR_LINEA_BONIFICADA_PEDIDO.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/PlantillaDinamico.sql/GLOBAL.PROC_OBTENER_CONSECUTIVO_SEGUN_ROL.StoredProcedure.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/DEPLOY_CIA_v1.2.0.sql
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/INVENTARIO_SP_CIA_v1.2.0.md
- C:/Users/produ/OneDrive/Documents/GitHub/PS_Portal_Clientes_Dinamico/docs/SESSION_SUMMARY_2026-09-26.md

## Validación

- Compilación Debug correcta; advertencias previas MSB3277/CS0414.
- 210 scripts y bundle de 27 definiciones válidos con ScriptDom SQL Server 2022.
- 288 comprobaciones de SQL dinámico (incluidas consultas anidadas), 34 comparaciones de equivalencia para DIS86 y control de contratos/huellas: correctos.
- Plantillas de correo e imágenes preservadas. Sin ejecución en SQL Server.

## Próximos pasos / pendientes

1. El usuario ejecutará manualmente docs/DEPLOY_CIA_v1.2.0.sql y desplegará el backend actualizado. No requiere repetir los 63 SP previos.
2. Pruebas funcionales con compañías reales, incluidas inserciones/actualizaciones, retornos, correo y PDF.
3. Revisar en un alcance posterior las vistas, funciones y consultas directas que conservan compañía fija. Ver ejemplos y detalles en INVENTARIO_SP_CIA_v1.2.0.md.

## Problemas conocidos / hallazgos

- Esta validación estática no verifica existencia, permisos ni definición de los objetos por compañía en SQL Server.
- Las funciones de descuentos y el acceso directo ANCLA.VIEW_REPORTE_COMPROBANTE_PAGO siguen pendientes; no fueron autorizados en v1.2.0.
- Las referencias a bases PS_TRACKING/EXACTUSERP se preservan. No se alteraron perfiles ni destinatarios de correo.
- No hay repositorio .git reconocible. Copias originales, inventario AST, comparaciones y reportes: C:/Users/produ/.codex/visualizations/2026/09/25/01a0d9a5-09e8-7910-bf18-3f322de3b956/cia_v120*.
