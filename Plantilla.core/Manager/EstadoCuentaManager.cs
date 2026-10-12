using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class EstadoCuentaCorreoPayload
    {
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string Correos { get; set; }
    }

    public class EstadoCuentaManager : Manager
    {

        public List<PROC_OBT_SEMANAS_POR_ANIOResult> ObtenerSemana_Anio(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SEMANAS_POR_ANIOResult> result;
                try
                {

                    result = context.PROC_OBT_SEMANAS_POR_ANIO(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SEMANAS_POR_ANIO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<VIEW_OBT_RUTA> ListaRutas(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBT_RUTA> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBT_RUTA>(cia, "VIEW_OBT_RUTA");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_RUTAs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_CONDICION_PAGO_POR_CLIENTEResult> Obtener_Condicion_Pago(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CONDICION_PAGO_POR_CLIENTEResult> result;
                try
                {

                    result = context.PROC_OBT_CONDICION_PAGO_POR_CLIENTE(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CONDICION_PAGO_POR_CLIENTE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<PLC_PROC_OBT_DATOS_CLIENTE_ESTADO_CUENTAResult> ObtenerEstadoCuentaCLiente(string semanamulti, string cocatenar_diasemana, string ec_saldo, string ec_condicion_pago, string ec_estado_visita, string vendedor,string cobrador, string ec_cliente,string ec_fechas, string cia)
        {
            string vendedorParam = string.Equals(vendedor, "VND", StringComparison.OrdinalIgnoreCase)
                       ? "0"
                       : vendedor;

            string cobradorParam = string.Equals(cobrador, "CND", StringComparison.OrdinalIgnoreCase)
                                   ? "0"
                                   : cobrador;
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PLC_PROC_OBT_DATOS_CLIENTE_ESTADO_CUENTAResult> result;
                try
                {
                    result = context.PLC_PROC_OBT_DATOS_CLIENTE_ESTADO_CUENTA(semanamulti, cocatenar_diasemana, ec_saldo, ec_condicion_pago, ec_estado_visita, vendedorParam, cobradorParam, ec_cliente, ec_fechas, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PLC_PROC_OBT_DATOS_CLIENTE_ESTADO_CUENTA", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }


        public List<PROC_OBT_SUBTABLA_DOCUMENTOS_POR_CLIENTEResult> Obtener_SubTabla_Documentos(string cliente, string cia)

        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTABLA_DOCUMENTOS_POR_CLIENTEResult> result;
                try
                {
                    result = context.PROC_OBT_SUBTABLA_DOCUMENTOS_POR_CLIENTE(cliente, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTABLA_DOCUMENTOS_POR_CLIENTE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;


            }

        }

        public List<PROC_OBT_TABLA_DOCUMENTOS_POR_CLIENTEResult> Obtener_Tabla_Documentos(string cia,string cliente, DateTime? fechaInicio, DateTime? fechaFin, string tipoDocumento)
        {     using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TABLA_DOCUMENTOS_POR_CLIENTEResult> result;
                try
                {
                    result = context.PROC_OBT_TABLA_DOCUMENTOS_POR_CLIENTE(cia,cliente, fechaInicio, fechaFin, tipoDocumento).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_TABLA_DOCUMENTOS_POR_CLIENTE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }
        }
        public string EnviaEstadoCuentaCorreo(string cliente, string tipo, string nombreReporte, string correo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PCL_PROC_ENVIAR_ESTADO_CUENTA_CLIENTE_CORPORATIVO(cliente, tipo, nombreReporte, correo, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_CLIENTES_PTCs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public EstadoCuentaCorreoPayload ObtenerDataCorreoEstadoCuenta(string cliente, string tipo, string nombreReporte, string correo, string cia)
        {
            EstadoCuentaCorreoPayload result = null;
            try
            {
                using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
                using (var cn = new SqlConnection(context.Connection.ConnectionString))
                using (var cmd = new SqlCommand("[GLOBAL].[PCL_PROC_ENVIAR_ESTADO_CUENTA_CLIENTE_CORPORATIVO]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CLIENTE", cliente ?? string.Empty);
                    cmd.Parameters.AddWithValue("@TIPO", tipo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@NOMBRE_REPORTE", nombreReporte ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CORREO", correo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CIA", cia ?? string.Empty);

                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            result = new EstadoCuentaCorreoPayload
                            {
                                Asunto = dr["ASUNTO"] == DBNull.Value ? string.Empty : dr["ASUNTO"].ToString(),
                                Cuerpo = dr["CUERPO"] == DBNull.Value ? string.Empty : dr["CUERPO"].ToString(),
                                Correos = dr["CORREO"] == DBNull.Value ? string.Empty : dr["CORREO"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                e = e.Replace(Environment.NewLine, " ");
                e = e.Replace('"', ' ');
                e = e.Replace("'", " ");
                WriteLog("VIEW_OBT_CLIENTES_PTCs", e);
                result = null;
            }

            return result;
        }
        
        public List<PROC_REPORTE_CUADRE_AUXILIARResult> GenerarExcel_Reporte_Auxiliar(string cliente, string fecha_doc_ini, string fecha_doc_fin, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_REPORTE_CUADRE_AUXILIARResult> result;
                try
                {
                    result = context.PROC_REPORTE_CUADRE_AUXILIAR(cliente, fecha_doc_ini, fecha_doc_fin, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_REPORTE_CUADRE_AUXILIAR", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }


        public List<PROC_REPORTE_CUADRE_AUXILIARResult> Enviar_Reporte_Cuadre_Auxiliar(string cliente, string fecha_doc_ini, string fecha_doc_fin, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_REPORTE_CUADRE_AUXILIARResult> result;
                try
                {
                    result = context.PROC_REPORTE_CUADRE_AUXILIAR(cliente, fecha_doc_ini, fecha_doc_fin, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_REPORTE_CUADRE_AUXILIAR", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }

        public string EnviaCorreo_ReporteAuxiliar(string fecha_doc_ini, string fecha_doc_fin, string correo, string nombreReporte, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PCL_PROC_ENVIAR_REPORTE_CUADRE_AUXILIAR(fecha_doc_ini, fecha_doc_fin, correo, nombreReporte, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_CLIENTES_PTCs", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }
        public EstadoCuentaCorreoPayload ObtenerDataCorreoReporteAuxiliar(string fecha_doc_ini, string fecha_doc_fin, string correo, string nombreReporte, string cia)
        {
            EstadoCuentaCorreoPayload result = null;
            try
            {
                using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
                using (var cn = new SqlConnection(context.Connection.ConnectionString))
                using (var cmd = new SqlCommand("[GLOBAL].[PCL_PROC_ENVIAR_REPORTE_CUADRE_AUXILIAR]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CIA", SqlDbType.VarChar, 20).Value = cia;
                    cmd.Parameters.AddWithValue("@FECHA_DOC_INI", fecha_doc_ini ?? string.Empty);
                    cmd.Parameters.AddWithValue("@FECHA_DOC_FIN", fecha_doc_fin ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CORREO", correo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@NOMBRE_REPORTE", nombreReporte ?? string.Empty);

                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            result = new EstadoCuentaCorreoPayload
                            {
                                Asunto = dr["ASUNTO"] == DBNull.Value ? string.Empty : dr["ASUNTO"].ToString(),
                                Cuerpo = dr["CUERPO"] == DBNull.Value ? string.Empty : dr["CUERPO"].ToString(),
                                Correos = dr["CORREO"] == DBNull.Value ? string.Empty : dr["CORREO"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                e = e.Replace(Environment.NewLine, " ");
                e = e.Replace('"', ' ');
                e = e.Replace("'", " ");
                WriteLog("VIEW_OBT_CLIENTES_PTCs", e);
                result = null;
            }

            return result;
        }

        public List<PROC_REPORTE_EXCEL_ESTADO_CUENTA_CLIENTE_EN_BATCHResult> GenerarReporteExcelBatch(string cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_REPORTE_EXCEL_ESTADO_CUENTA_CLIENTE_EN_BATCHResult> result;
                try
                {
                    result = context.PROC_REPORTE_EXCEL_ESTADO_CUENTA_CLIENTE_EN_BATCH(cliente, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_REPORTE_EXCEL_ESTADO_CUENTA_CLIENTE_EN_BATCH", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }

        public List<PROC_OBT_ESTADO_VISITAResult> Obtener_Estado_Visita(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_ESTADO_VISITAResult> result;
                try
                {

                    result = context.PROC_OBT_ESTADO_VISITA(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_ESTADO_VISITA", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }
        //public List<PROC_OBT_ID_VISITAResult> Obtener_Id_Visita()
        //{
        //    using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
        //    {
        //        List<PROC_OBT_ID_VISITAResult> result;
        //        try
        //        {

        //            result = context.PROC_OBT_ID_VISITA().ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            string e = ex.Message;
        //            e = e.Replace(Environment.NewLine, " ");
        //            e = e.Replace('"', ' ');
        //            e = e.Replace("'", " ");
        //            WriteLog("PROC_OBT_ID_VISITA", e);
        //            result = null;
        //        }
        //        context.Connection.Close();
        //        return result;
        //    }

        //}

        public int Insertar_Visita(string cliente, string tipo_visita, DateTime fecha_visita, string latitud, string longitud, string estado_visita,
                             string comentario, string vendedor, string usuario, string doc_venc_descripcion, string direccion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                int resultado;
                try
                {
                    string IdVisita = string.Empty;

                    string vendedorFinal = vendedor == "0" ? "ND" : vendedor;

                    var result = context.PROC_INS_VISITA(cliente, tipo_visita, fecha_visita, latitud, longitud, estado_visita, comentario, "null", vendedorFinal, usuario, doc_venc_descripcion, direccion, cia, ref IdVisita).ToString();
                    resultado = Convert.ToInt32(IdVisita);
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_INS_VISITA", e);
                    resultado = 0; 
                }
                context.Connection.Close();
                return resultado;
            }
        }

        public OBTENER_PDF_BASE64Result ObtenerPDF(string factura, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                OBTENER_PDF_BASE64Result resultado;
                try
                {
                    //Context.Connection.ConnectionString = getConexion();
                    resultado = context.OBTENER_PDF_BASE64(factura, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("OBTENER_PDF_BASE64", e);
                    resultado = null;
                }
                context.Connection.Close();
                return resultado;
            }
        }

        public List<PROC_OBT_TIPO_VISITAResult> ObtenerTipoVisita(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TIPO_VISITAResult> result;
                try
                {

                    result = context.PROC_OBT_TIPO_VISITA(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_TIPO_VISITA", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_DOCUMENTOS_VENCIDOS_POR_CLIENTEResult> Obtener_Documentos_Vencidos(string cliente, string cia)

        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_DOCUMENTOS_VENCIDOS_POR_CLIENTEResult> result;
                try
                {
                    result = context.PROC_DOCUMENTOS_VENCIDOS_POR_CLIENTE(cliente, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_DOCUMENTOS_VENCIDOS_POR_CLIENTE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;


            }

        }
        public List<PROC_OBT_PERMISOS_ROL_MODULO_SUBResult>
        ObtenerPermisosRolModuloSub(int rol, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_PERMISOS_ROL_MODULO_SUBResult> result;
                try
                {
                   
                    result = context.PROC_OBT_PERMISOS_ROL_MODULO_SUB(rol, cia)
                                    .ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message
                                 .Replace(Environment.NewLine, " ")
                                 .Replace('"', ' ')
                                 .Replace("'", " ");
                    WriteLog("PROC_OBT_PERMISOS_ROL_MODULO_SUB", e);
                    result = null;     
                }
                context.Connection.Close();
                return result;
            }
        }

        public string Enviar_Correo_CLiente_FE(string cliente, string correos, string documento, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PCL_PROC_ENVIAR_FACTURA_AL_CLIENTE(cliente, correos, documento, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_ENVIAR_FACTURA_AL_CLIENTE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }
    }
}




