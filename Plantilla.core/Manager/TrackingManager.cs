using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class TrackingManager : Manager
    {

        public List<PROC_SEGUIMIENTO_TRACKING_COLOR_HEXA_V2Result> ObtenerPedTracking(string fechaInicio, string fechaFin, int estado_pedido, string fecha_prometida, string fecha_prometidaFin, int estado_factura,
                                                                                            string concatenar, string fpedido, string ffactura, string fcliente, string cocatenar_tla, string corte_tla,
                                                                                            string agente, string cobrador, string cocatenar_diasemana, int horaencortefuera, string fecha_CxcRecibe_Ini, string fecha_CxcRecibe_Fin, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_SEGUIMIENTO_TRACKING_COLOR_HEXA_V2Result> result;
                try
                {
                    result = context.PROC_SEGUIMIENTO_TRACKING_COLOR_HEXA_V2(fechaInicio, fechaFin, estado_pedido, fecha_prometida, fecha_prometidaFin, estado_factura, concatenar, fpedido,
                                                                             ffactura, fcliente, cocatenar_tla, corte_tla, agente, cobrador, cocatenar_diasemana, horaencortefuera, fecha_CxcRecibe_Ini, fecha_CxcRecibe_Fin, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_SEGUIMIENTO_TRACKING_COLOR_HEXA_V2", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }


        public List<VIEW_OBTENER_RUTA> ListaRutas(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_RUTA> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_RUTA>(cia, "VIEW_OBTENER_RUTA");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_RUTAs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }




        public List<VIEW_OBTENER_SEGUIMIENTO_FACTURA> ObtenerSeguimiento_Factura(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_SEGUIMIENTO_FACTURA> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_SEGUIMIENTO_FACTURA>(cia, "VIEW_OBTENER_SEGUIMIENTO_FACTURA");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_SEGUIMIENTO_FACTURAs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<VIEW_OBTENER_ESTADO_PEDIDO> Obtener_Estado_Pedido(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_ESTADO_PEDIDO> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_ESTADO_PEDIDO>(cia, "VIEW_OBTENER_ESTADO_PEDIDO");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_ESTADO_PEDIDOs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<VIEW_OBTENER_CLIENTE> Obtener_Lista_Clientes(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_CLIENTE> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_CLIENTE>(cia, "VIEW_OBTENER_CLIENTES");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_CLIENTEs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<VIEW_OBTENER_AGENTE> Obtener_Lista_Agente(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_AGENTE> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_AGENTE>(cia, "VIEW_OBTENER_AGENTE");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_AGENTEs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<VIEW_OBTENER_TLA_ESTADO> ObtenerTLA_Estado(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_TLA_ESTADO> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_TLA_ESTADO>(cia, "VIEW_OBTENER_TLA_ESTADO");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_TLA_ESTADOs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<PROC_OBTENER_COD_PEDIDOResult> ObtenerCod_Pedido(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBTENER_COD_PEDIDOResult> result;
                try
                {

                    result = context.PROC_OBTENER_COD_PEDIDO(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBTENER_COD_PEDIDO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBTENER_COD_PEDIDO_BUSQUEDAResult> ObtenerCod_Pedido_buscar(string pedido, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBTENER_COD_PEDIDO_BUSQUEDAResult> result;
                try
                {

                    result = context.PROC_OBTENER_COD_PEDIDO_BUSQUEDA(pedido, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBTENER_COD_PEDIDO_BUSQUEDA", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBTENER_COD_FACTURAResult> ObtenerCod_Factura(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBTENER_COD_FACTURAResult> result;
                try
                {

                    result = context.PROC_OBTENER_COD_FACTURA(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBTENER_COD_FACTURA", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<PROC_OBTENER_COD_FACTURA_BUSCARResult> ObtenerCod_Factura_Buscar(string term, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBTENER_COD_FACTURA_BUSCARResult> result;
                try
                {

                    result = context.PROC_OBTENER_COD_FACTURA_BUSCAR(term, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBTENER_COD_FACTURA_BUSCAR", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<VIEW_OBTENER_DIA_RUTA> ObtenerDia_Ruta(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_DIA_RUTA> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_DIA_RUTA>(cia, "VIEW_OBTENER_DIA_RUTA");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_DIA_RUTAs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<VIEW_OBTENER_HORA_CORTE> Obtener_HoraCorte(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_HORA_CORTE> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_HORA_CORTE>(cia, "VIEW_OBTENER_HORA_CORTE");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_HORA_CORTEs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public string ActualizaEstadoPedido(string pedido, int estado, DateTime fecha, string observacion, string usuario, string correo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_ESTADO_PEDIDO(pedido, estado, fecha, observacion, usuario, correo, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_UPDATE_ESTADO_PEDIDO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public string DELETE_SEGUIMIENTO(int id, string numpedido, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_DELETE_SEGUIMIENTO(id, numpedido, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_DELETE_SEGUIMIENTO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public string PURGAR_BLOQUEOS(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_PURGA_BLOQUEOS(cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_PURGA_BLOQUEOS", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public string ActualizaEstadoPedido_Multiple(string pedido, int estado, DateTime fecha_Pedido, string observacion, string usuario, string correo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_ESTADO_PEDIDO_MULTIPLE(pedido, estado, fecha_Pedido, observacion, usuario, correo, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_UPDATE_ESTADO_PEDIDO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }

        public string ActualizaEstadoFactura(string pedido, int estado, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_ESTADO_FACTURA(pedido, estado, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_UPDATE_ESTADO_FACTURA", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public string ActualizaEstadoFactura_Multiple(string pedido, int estado, string observacion, string usuario, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_ESTADO_FACTURA_MULTIPLE(pedido, estado, observacion, usuario, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_UPDATE_ESTADO_FACTURA_MULTIPLE", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public string ActualizaObservacion(string pedido, string observacion, string usuario, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_OBSERVACION(pedido, observacion, usuario, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_UPDATE_OBSERVACION", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public string ActualizaObservacion_SubTabla(int id_seguimiento, string pedido, string observacion, string usuario, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_SUBTABLA_OBSERVACION(id_seguimiento, pedido, observacion, usuario, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_UPDATE_SUBTABLA_OBSERVACION", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public string ActualizaSuceso_SubTabla(string pedido, string suceso, string correo, string usuario, string fecha, string observacion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    DateTime? fechaSuceso = fecha == null ? (DateTime?)null : DateTime.Parse(fecha, System.Globalization.CultureInfo.InvariantCulture);
                    result = context.PROC_UPDATE_SUBTABLA_SUCESO(pedido, suceso, correo, usuario, fechaSuceso, observacion, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_UPDATE_SUBTABLA_SUCESO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public string ActualizaFechaRuta(string pedido, DateTime fecha, string usuario, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_FECHA_RUTA(pedido, fecha, usuario, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_UPDATE_FECHA_RUTA", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public string ActualizaFechaRuta_Multiple(string pedido, DateTime fecha, string usuario, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_FECHA_RUTA_MULTIPLE(pedido, fecha, usuario, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_UPDATE_FECHA_RUTA_MULTIPLE", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<PROC_REPORTE_RUTA_PEDIDO_MULTIPLEResult> GenerarExcel_Reporte_Ruta(string pedido, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_REPORTE_RUTA_PEDIDO_MULTIPLEResult> result;
                try
                {
                    result = context.PROC_REPORTE_RUTA_PEDIDO_MULTIPLE(pedido, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_REPORTE_RUTA_PEDIDO_MULTIPLE", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }


        public List<PROC_REPORTE_RUTAResult> Enviar_Reporte_Ruta_TLA(string fechaPrometida, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_REPORTE_RUTAResult> result;
                try
                {
                    result = context.PROC_REPORTE_RUTA(fechaPrometida, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_REPORTE_RUTA", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }

        public string EnviaCorreo_ReporteRutas(string fechaPrometida, string correo, string nombreReporte, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PCL_PROC_ENVIAR_REPORTE_RUTAS_TRACKING(fechaPrometida, correo, nombreReporte, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_ENVIAR_REPORTE_RUTAS_TRACKING", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }



        public string EnviaCorreo_ReporteRutas_PDF_TLA(string ESTADO_FAC, string nombreReporte, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PCL_PROC_ENVIAR_REPORTE_RUTAS_TRACKING_PDF_TLA(ESTADO_FAC, nombreReporte, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_ENVIAR_REPORTE_RUTAS_TRACKING_PDF_TLA", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_SUCESOS_FILTROResult> GetSucesos_filtro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUCESOS_FILTROResult> result = new List<PROC_OBT_SUCESOS_FILTROResult>();
                try
                {
                    result = context.PROC_OBT_SUCESOS_FILTRO(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUCESOS_FILTRO", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<VIEW_OBTENER_COBRADOR> Obtener_Lista_Cobrador(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_COBRADOR> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_COBRADOR>(cia, "VIEW_OBTENER_COBRADOR");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_COBRADORs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }
    }
}
