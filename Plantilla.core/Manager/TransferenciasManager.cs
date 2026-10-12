using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plantilla.core.Manager
{
    public class TransferenciasManager : Manager
    {
        public static List<PROC_OBT_ESTADO_CUENTA_BANCARIOResult> ObtenerTransferencias(
          string fechaInicio, string fechaFin, string cuenta, string tipo, string subtipo, string documento,
          string proveedor, string cliente, string debitoXcredito, string cxc_cxp, string cargadoBancos, string cia
      )
        {
            using (BaseDataContext context = new BaseDataContext(GetConnectionString()))
            {
                List<PROC_OBT_ESTADO_CUENTA_BANCARIOResult> result = new List<PROC_OBT_ESTADO_CUENTA_BANCARIOResult>();
                try
                {
                    result = context.PROC_OBT_ESTADO_CUENTA_BANCARIO(
                          fechaInicio, fechaFin, cuenta, tipo, subtipo, documento,
                          proveedor, cliente, debitoXcredito, cxc_cxp, cargadoBancos, cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_ESTADO_CUENTA_BANCARIO", e);
                    return null;
                }
            }
        }


        public static string EditarNumBanco(string idLinea, string numNuevo, string cia)
        {
            using (BaseDataContext context = new BaseDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_MT940_NUMERO_BANCO(idLinea, numNuevo, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_MT940_NUMERO_BANCO", e);
                }
                return result;
            }
        }


        public static PROC_VERIFICAR_FUSION_DOCUMENTOSResult ValidarFusionDocs(int seleccion, string numero, string cuenta_banco, string fecha, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                PROC_VERIFICAR_FUSION_DOCUMENTOSResult result = new PROC_VERIFICAR_FUSION_DOCUMENTOSResult();
                try
                {
                    result = context.PROC_VERIFICAR_FUSION_DOCUMENTOS(seleccion, numero, cuenta_banco, fecha, cia).First();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_VERIFICAR_FUSION_DOCUMENTOS", e);
                }
                return result;
            }
        }

        public static PROC_VERIFICAR_SI_CONSECUTIVO_NUEVO_EXISTEResult ValidarNuevoConsecutivo(string numero, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                PROC_VERIFICAR_SI_CONSECUTIVO_NUEVO_EXISTEResult result = new PROC_VERIFICAR_SI_CONSECUTIVO_NUEVO_EXISTEResult();
                try
                {
                    result = context.PROC_VERIFICAR_SI_CONSECUTIVO_NUEVO_EXISTE(numero, cia).First();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_VERIFICAR_SI_CONSECUTIVO_NUEVO_EXISTE", e);
                }
                return result;
            }
        }
        public static List<PROC_OBT_ESTADO_CUENTA_BANCARIO_PARA_ACCIONESResult> ObtenerTransferenciasParaAcciones(int idLinea, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_ESTADO_CUENTA_BANCARIO_PARA_ACCIONESResult> result = new List<PROC_OBT_ESTADO_CUENTA_BANCARIO_PARA_ACCIONESResult>();
                try
                {
                    result = context.PROC_OBT_ESTADO_CUENTA_BANCARIO_PARA_ACCIONES(idLinea, cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_ESTADO_CUENTA_BANCARIO_PARA_ACCIONES", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_CLIENTES_FILTROResult> ObtenerClientesFiltro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTES_FILTROResult> result = new List<PROC_OBT_CLIENTES_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_CLIENTES_FILTRO(
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTES_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_PROVEEDORES_FILTROResult> ObtenerProveedoresFiltro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_PROVEEDORES_FILTROResult> result = new List<PROC_OBT_PROVEEDORES_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_PROVEEDORES_FILTRO(
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_PROVEEDORES_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_CONCILIACIONES_TODAS_FILTROResult> ObtenerConciliacionesFiltro(string cuenta, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CONCILIACIONES_TODAS_FILTROResult> result = new List<PROC_OBT_CONCILIACIONES_TODAS_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_CONCILIACIONES_TODAS_FILTRO(
                        cuenta,
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CONCILIACIONES_TODAS_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_CONCILIACIONES_FILTROResult> ObtenerConciliaciones(string cuenta, string fecha, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CONCILIACIONES_FILTROResult> result = new List<PROC_OBT_CONCILIACIONES_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_CONCILIACIONES_FILTRO(
                        cuenta,
                        fecha,
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CONCILIACIONES_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_CUENTAS_BANCARIAS_FILTROResult> ObtenerCuentasFiltro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CUENTAS_BANCARIAS_FILTROResult> result = new List<PROC_OBT_CUENTAS_BANCARIAS_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_CUENTAS_BANCARIAS_FILTRO(
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CUENTAS_BANCARIAS_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_DOCUMENTOS_FILTROResult> ObtenerDocumentosFiltro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_DOCUMENTOS_FILTROResult> result = new List<PROC_OBT_DOCUMENTOS_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_DOCUMENTOS_FILTRO(
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_DOCUMENTOS_FILTRO", e);
                    return null;
                }
            }
        }

        public static string InsertarDocumentoCC(string cuenta, string usuario, string documento, string tipo, int subtipo, string fecha, string concepto,
            string monto, string moneda, string idLinea, string cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_DOCUMENTO_CC(cuenta, usuario, documento, tipo, subtipo, fecha, concepto,
                    Convert.ToDecimal(monto), moneda, idLinea, cliente, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_DOCUMENTO_CC", e);
                }
                return result;
            }
        }

        public static string EditarDocumentoBanco(int idLinea, string doc_viejo, string doc_nuevo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_DOCUMENTO_BANCO(idLinea, doc_viejo, doc_nuevo, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_DOCUMENTO_BANCO", e);
                }
                return result;
            }
        }

        public static string InsertarDocumentoCP(string cuenta, string usuario, string documento, string tipo, int subtipo, string fecha, string concepto,
         string monto, string moneda, string idLinea, string proveedor, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_DOCUMENTO_CP(cuenta, usuario, documento, tipo, subtipo, fecha, concepto,
                    Convert.ToDecimal(monto), moneda, idLinea, proveedor, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_DOCUMENTO_CP", e);
                }
                return result;
            }
        }

        public static string InsertarDocumentoMovReportados(string cuenta, string documento, string tipo, string fecha, string concepto,
        string monto, string usuario, string idLinea, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_DOCUMENTO_MOV_REPORTADOS(cuenta, documento, tipo, fecha, concepto,
                    Convert.ToDecimal(monto), usuario, idLinea, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_DOCUMENTO_MOV_REPORTADOS", e);
                }
                return result;
            }
        }

        public static string EditarDocumentoFusion(int id, string documento_fusion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_DOCUMENTO_FUSION(id, documento_fusion, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_DOCUMENTO_FUSION", e);
                }
                return result;
            }
        }

        public static string InsertarDocumentoFusion(string documento_fusion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_DOCUMENTO_FUSION(documento_fusion, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_DOCUMENTO_FUSION", e);
                }
                return result;
            }
        }

        public static string InsertarMovBancos(string cuenta, string usuario, string documento, string monto, string moneda, string tipo, int subtipo, string fecha,
            string concepto, string idLinea, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_DOCUMENTO_MOV_BANCOS(cuenta, usuario, documento,
                    Convert.ToDecimal(monto), moneda, tipo, subtipo, fecha, concepto, idLinea, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_DOCUMENTO_MOV_BANCOS", e);
                }
                return result;
            }
        }

        public static PROC_OBT_TIPO_CAMBIO_MONEDA_X_FECHAResult ObtenerTipoCambioXFecha(string moneda, string fecha, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                PROC_OBT_TIPO_CAMBIO_MONEDA_X_FECHAResult result = new PROC_OBT_TIPO_CAMBIO_MONEDA_X_FECHAResult();
                try
                {
                    result = context.PROC_OBT_TIPO_CAMBIO_MONEDA_X_FECHA(moneda, fecha, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_OBT_TIPO_CAMBIO_MONEDA_X_FECHA", e);
                }
                return result;
            }
        }

        public static string InsertarConciliacionBancaria(string cuenta, string conciliacion, string usuario, string documento, string fecha, string concepto,
          string monto, string tipo, string idLinea, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_CONCILIACION_BANCARIA(cuenta, conciliacion, usuario, documento, fecha, concepto,
                    Convert.ToDecimal(monto), tipo, idLinea, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_CONCILIACION_BANCARIA", e);
                }
                return result;
            }
        }

        public static string InsertarDocumentoExcel(string cuenta, string documento, string tipo, int subtipo, string fecha, string concepto,
         string monto, string credito_debito, string usuario, string insMovReportados, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_ESTADO_CUENTA_EXCEL(cuenta, documento, tipo, subtipo, fecha, concepto,
                    Convert.ToDecimal(monto), credito_debito, usuario, insMovReportados, cia).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_ESTADO_CUENTA_EXCEL", e);
                }
                return result;
            }
        }

        public static List<PROC_OBT_SUBTIPOS_CC_FILTROResult> ObtenerSubtiposCC_Filtro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTIPOS_CC_FILTROResult> result = new List<PROC_OBT_SUBTIPOS_CC_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_SUBTIPOS_CC_FILTRO(
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPOS_CC_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_SUBTIPOS_CONTROL_BANCARIO_FILTROResult> ObtenerSubtiposControlBancario_Filtro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTIPOS_CONTROL_BANCARIO_FILTROResult> result = new List<PROC_OBT_SUBTIPOS_CONTROL_BANCARIO_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_SUBTIPOS_CONTROL_BANCARIO_FILTRO(
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPOS_CONTROL_BANCARIO_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_SUBTIPOS_X_TIPO_CONTROL_BANCARIO_FILTROResult> ObtenerSubtiposControlBancario_X_TIPO_Filtro(string tipo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTIPOS_X_TIPO_CONTROL_BANCARIO_FILTROResult> result = new List<PROC_OBT_SUBTIPOS_X_TIPO_CONTROL_BANCARIO_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_SUBTIPOS_X_TIPO_CONTROL_BANCARIO_FILTRO(
                         tipo,
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPOS_X_TIPO_CONTROL_BANCARIO_FILTRO", e);
                    return null;
                }
            }
        }



        public static List<PROC_OBT_SUBTIPOS_TODOS_FILTROResult> ObtenerSubtiposTODOS_Filtro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTIPOS_TODOS_FILTROResult> result = new List<PROC_OBT_SUBTIPOS_TODOS_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_SUBTIPOS_TODOS_FILTRO(
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPOS_TODOS_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_SUBTIPOS_CC_X_TIPO_FILTROResult> ObtenerSubtiposCC_XTipoFiltro(string tipo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTIPOS_CC_X_TIPO_FILTROResult> result = new List<PROC_OBT_SUBTIPOS_CC_X_TIPO_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_SUBTIPOS_CC_X_TIPO_FILTRO(
                        tipo,
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPOS_CC_X_TIPO_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_SUBTIPOS_CP_FILTROResult> ObtenerSubtiposCP_Filtro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTIPOS_CP_FILTROResult> result = new List<PROC_OBT_SUBTIPOS_CP_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_SUBTIPOS_CP_FILTRO(
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPOS_CP_FILTRO", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_SUBTIPOS_CP_X_TIPO_FILTROResult> ObtenerSubtiposCP_XTipoFiltro(string tipo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTIPOS_CP_X_TIPO_FILTROResult> result = new List<PROC_OBT_SUBTIPOS_CP_X_TIPO_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_SUBTIPOS_CP_X_TIPO_FILTRO(
                        tipo,
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPOS_CP_X_TIPO_FILTRO", e);
                    return null;
                }
            }
        }
    }
}
