using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class SeguimientoManager : Manager
    {

        public List<PROC_SUBTABLA_SEGUIMIENTO_TRACKING_COLOR_HEXAResult> Obtener_SubTabla_SeguimientoTracking(string pedido, string cia)

        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_SUBTABLA_SEGUIMIENTO_TRACKING_COLOR_HEXAResult> result;
                try
                {
                    result = context.PROC_SUBTABLA_SEGUIMIENTO_TRACKING_COLOR_HEXA(pedido, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_SUBTABLA_SEGUIMIENTO_TRACKING_COLOR_HEXA", e);
                    result = null;
                }
                context.Connection.Close();
                return result;


            }

        }


        public List<PROC_SEGUIMIENTO_COLOR_HEXAResult> Obtener_Seguimiento(string fechaInicio, string fechaFin, int estado_pedido, string fecha_prometida, int estado_factura, string rutamulti, string fpedido, string ffactura,
                                              string fcliente, string agente, string usuario, string suceso, string cobrador, string FModEstadoPedido, string FModEstadoFactura, string FModSeguiFactura_rango_ini, string FModSeguiFactura_rango_fin, string cia)

        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_SEGUIMIENTO_COLOR_HEXAResult> result;
                try
                {
                    result = context.PROC_SEGUIMIENTO_COLOR_HEXA(fechaInicio, fechaFin, estado_pedido, fecha_prometida, estado_factura, rutamulti, fpedido, ffactura, fcliente, agente, usuario, suceso, cobrador, FModEstadoPedido, FModEstadoFactura, FModSeguiFactura_rango_ini, FModSeguiFactura_rango_fin, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_SEGUIMIENTO_COLOR_HEXA", e);
                    result = null;
                }
                context.Connection.Close();
                return result;


            }

        }


        //public List<VIEW_OBTENER_ESTADO_PEDIDO> Obtener_Estado_Pedido_Seguimiento()
        //{
        //    using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
        //    {
        //        List<VIEW_OBTENER_ESTADO_PEDIDO> result;
        //        try
        //        {

        //            result = context.VIEW_OBTENER_ESTADO_PEDIDOs.ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            string e = ex.Message;
        //            e = e.Replace(Environment.NewLine, " ");
        //            e = e.Replace('"', ' ');
        //            e = e.Replace("'", " ");
        //            WriteLog("VIEW_OBTENER_ESTADO_PEDIDOs", e);
        //            result = null;
        //        }
        //        context.Connection.Close();
        //        return result;
        //    }

        //}


        //public List<VIEW_OBTENER_SEGUIMIENTO_FACTURA> ObtenerEstado_Factura()
        //{
        //    using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
        //    {
        //        List<VIEW_OBTENER_SEGUIMIENTO_FACTURA> result;
        //        try
        //        {

        //            result = context.VIEW_OBTENER_SEGUIMIENTO_FACTURAs.ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            string e = ex.Message;
        //            e = e.Replace(Environment.NewLine, " ");
        //            e = e.Replace('"', ' ');
        //            e = e.Replace("'", " ");
        //            WriteLog("VIEW_OBTENER_SEGUIMIENTO_FACTURAs", e);
        //            result = null;
        //        }
        //        context.Connection.Close();
        //        return result;
        //    }

        //}


        //public List<PROC_OBTENER_COD_PEDIDO_SEGUIMIENTOResult> ObtenerCod_Pedido_Seg()
        //{
        //    using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
        //    {
        //        List<PROC_OBTENER_COD_PEDIDO_SEGUIMIENTOResult> result;
        //        try
        //        {

        //            result = context.PROC_OBTENER_COD_PEDIDO_SEGUIMIENTO().ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            string e = ex.Message;
        //            e = e.Replace(Environment.NewLine, " ");
        //            e = e.Replace('"', ' ');
        //            e = e.Replace("'", " ");
        //            WriteLog("PROC_OBTENER_COD_PEDIDO_SEGUIMIENTO", e);
        //            result = null;
        //        }
        //        context.Connection.Close();
        //        return result;
        //    }

        //}


        //public List<PROC_OBTENER_COD_FACTURA_SEGUIMIENTOResult> ObtenerCod_Factura_Seg()
        //{
        //    using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
        //    {
        //        List<PROC_OBTENER_COD_FACTURA_SEGUIMIENTOResult> result;
        //        try
        //        {

        //            result = context.PROC_OBTENER_COD_FACTURA_SEGUIMIENTO().ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            string e = ex.Message;
        //            e = e.Replace(Environment.NewLine, " ");
        //            e = e.Replace('"', ' ');
        //            e = e.Replace("'", " ");
        //            WriteLog("PROC_OBTENER_COD_FACTURA_SEGUIMIENTO", e);
        //            result = null;
        //        }
        //        context.Connection.Close();
        //        return result;
        //    }

        //}





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


        public string ActualizaObservacionSeguimiento(int id_seguimiento, string pedido, string observacion, string usuario, string cia)
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


        public string ActualizaSucesoSeguimiento(string pedido, string suceso, string correo, string usuario, string fecha, string observacion, string cia)
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


        /// <summary>
        ///  parte nueva 
        /// </summary>
        /// <returns></returns>
        /// 
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


        public List<PROC_OBTENER_COD_PEDIDO_SEGUIMIENTO_BUSQUEDAResult> ObtenerCod_Pedido_Segui_Buscar(string term, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBTENER_COD_PEDIDO_SEGUIMIENTO_BUSQUEDAResult> result;
                try
                {

                    result = context.PROC_OBTENER_COD_PEDIDO_SEGUIMIENTO_BUSQUEDA(term, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBTENER_COD_PEDIDO_SEGUIMIENTO_BUSQUEDA", e);
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

        public List<PROC_OBTENER_COD_FACTURA_SEGUIMIENTO_BUSQUEDAResult> ObtenerCod_Factura_Seg_Busqueda(string term, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBTENER_COD_FACTURA_SEGUIMIENTO_BUSQUEDAResult> result;
                try
                {

                    result = context.PROC_OBTENER_COD_FACTURA_SEGUIMIENTO_BUSQUEDA(term, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBTENER_COD_FACTURA_SEGUIMIENTO_BUSQUEDA", e);
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

        public List<VIEW_OBTENER_USUARIO> Obtener_Lista_Usuario()
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_USUARIO> result;
                try
                {

                    result = context.VIEW_OBTENER_USUARIOs.ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_USUARIOs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<VIEW_OBTENER_SUCESOS_SEGUIMIENTO> Obtener_Lista_Suceso(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_SUCESOS_SEGUIMIENTO> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_SUCESOS_SEGUIMIENTO>(cia, "VIEW_OBTENER_SUCESOS_SEGUIMIENTO");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_SUCESOS_SEGUIMIENTOs", e);
                    result = null;
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

        public List<PROC_REPORTE_SEGUIMIENTO_FACTURA_PEDIDOResult> GenerarExcel_Reporte_Seguimientos(string pedido, int estado_pedido, int estado_factura, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_REPORTE_SEGUIMIENTO_FACTURA_PEDIDOResult> result;
                try
                {
                    result = context.PROC_REPORTE_SEGUIMIENTO_FACTURA_PEDIDO(pedido, estado_pedido, estado_factura, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_REPORTE_SEGUIMIENTO_FACTURA_PEDIDO", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }


        public List<PROC_REPORTE_SUGIMIENTO_RUTAResult> Enviar_Reporte_Ruta_TLA(string fechaPrometida, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_REPORTE_SUGIMIENTO_RUTAResult> result;
                try
                {
                    result = context.PROC_REPORTE_SUGIMIENTO_RUTA(fechaPrometida, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_REPORTE_SUGIMIENTO_RUTA", e);
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
                    result = context.PCL_PROC_ENVIAR_REPORTE_RUTAS_SEGUIMIENTO(fechaPrometida, correo, nombreReporte, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_ENVIAR_REPORTE_RUTAS_SEGUIMIENTO", e);
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
    }
}
