using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class PedidoManager : Manager
    {
        public List<PROC_OBT_COD_PEDIDO_BUSQUEDA_PTCResult> BuscarCodPedido(string pedido, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_COD_PEDIDO_BUSQUEDA_PTCResult> result;
                try
                {

                    result = context.PROC_OBT_COD_PEDIDO_BUSQUEDA_PTC(pedido, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_COD_PEDIDO_BUSQUEDA_PTC", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<VIEW_OBT_ESTADO_PEDIDO_PTC> Obtener_Estado_Pedido(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBT_ESTADO_PEDIDO_PTC> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBT_ESTADO_PEDIDO_PTC>(cia, "VIEW_OBT_ESTADO_PEDIDO_PTC");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_ESTADO_PEDIDO_PTCs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<VIEW_OBT_ESTADO_FACTURA_PTC> Obtener_Estado_Factura(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBT_ESTADO_FACTURA_PTC> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBT_ESTADO_FACTURA_PTC>(cia, "VIEW_OBT_ESTADO_FACTURA_PTC");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_ESTADO_FACTURA_PTCs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<PROC_OBT_CLIENTE_PEDResult> Obtener_Lista_Clientes(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTE_PEDResult> result = new List<PROC_OBT_CLIENTE_PEDResult>();
                try
                {
                    result = context.PROC_OBT_CLIENTE_PED(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_PED", e);
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<PROC_OBT_CLIENTE_PEDResult> Obtener_Lista_Clientes_Filtro(string cliente, string fecha, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTE_PEDResult> result = new List<PROC_OBT_CLIENTE_PEDResult>();
                try
                {
                    result = context.PROC_OBT_CLIENTE_PED(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_PED", e);
                }
                context.Connection.Close();
                return result;
            }

        }



        public List<PROC_OBT_PEDIDO_PTCResult> ObtenerPed(string FECHAINI, string FECHAFIN, string PEDIDO, int ESTADO_PEDIDO, int ESTADO_FACTURA, string ped_vendedor, string ped_cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_PEDIDO_PTCResult> result;
                try
                {
                    result = context.PROC_OBT_PEDIDO_PTC(FECHAINI, FECHAFIN, PEDIDO, ESTADO_PEDIDO, ESTADO_FACTURA, ped_vendedor, ped_cliente, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_PEDIDO_PTC", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }

        public List<PROC_OBT_SUBTABLA_LINEA_PEDIDOResult> Obtener_SubTabla_LineaPedido(string pedido, string cia)

        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTABLA_LINEA_PEDIDOResult> result;
                try
                {
                    result = context.PROC_OBT_SUBTABLA_LINEA_PEDIDO(pedido, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTABLA_LINEA_PEDIDO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;


            }

        }

        public List<PROC_REPORTE_EXCEL_PEDIDO_INDEX_BATCHResult> GenerarReporteExcelPedidoBatch(string pedido, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_REPORTE_EXCEL_PEDIDO_INDEX_BATCHResult> result;
                try
                {
                    result = context.PROC_REPORTE_EXCEL_PEDIDO_INDEX_BATCH(pedido, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_REPORTE_EXCEL_PEDIDO_INDEX_BATCH", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }




        public PROC_OBT_CONSECUTIVO_PEDIDOResult Obtener_Consecutivo_Ped(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                try
                {
                    var result = context.PROC_OBT_CONSECUTIVO_PEDIDO(cia).FirstOrDefault();

                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CONSECUTIVO_PEDIDO", e);
                    return null;
                }

            }
        }

        public int InsertarPedido(string consecutivo, string cliente, int tipo, string vendedor, string observacion, string usuario,string bodega ,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                int resultado;
                try
                {
                    string pedidoId = string.Empty;
                    var result = context.PROC_INSERTAR_PEDIDO(consecutivo, cliente, tipo, vendedor, observacion, usuario,bodega ,cia, ref pedidoId).ToString();
                    resultado = Convert.ToInt32(pedidoId);
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_INSERTAR_PEDIDO", e);
                    resultado = 0; // CERO IMPLICA QUE NO SE CREO.
                }
                context.Connection.Close();
                return resultado;
            }

        }

        public List<VIEW_OBT_LISTA_ARTICULO> Obtener_Lista_Articulo(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBT_LISTA_ARTICULO> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBT_LISTA_ARTICULO>(cia, "VIEW_OBT_LISTA_ARTICULOS");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_LISTA_ARTICULOs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_INFORMACION_ARTICULOResult> ObtenerInformacion_Articulo(string articulo,string cliente,string cia)

        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_INFORMACION_ARTICULOResult> result;
                try
                {
                    result = context.PROC_OBT_INFORMACION_ARTICULO(articulo,cliente,cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_INFORMACION_ARTICULO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;


            }

        }

        public List<PROC_OBT_INFORMACION_ARTICULO_DCTOResult> ObtenerInformacion_Articulo_Dcto(string articulo, string cliente,int cantidad_min, string cia)

        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_INFORMACION_ARTICULO_DCTOResult> result;
                try
                {
                    result = context.PROC_OBT_INFORMACION_ARTICULO_DCTO(articulo, cliente, cantidad_min, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_INFORMACION_ARTICULO_DCTO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;


            }

        }



        public string InsertarPedidoLinea(int pedido, string articulo, int cantidad, int existencia, decimal porc_descuento, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_PEDIDO_LINEA(pedido, articulo, cantidad, existencia, porc_descuento, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_INS_PEDIDO_LINEA", e);
                    result = null;

                }
                context.Connection.Close();
                return result;


            }

        }


        public List<PROC_OBT_TOTAL_LINEA_PEDIDO_PTCResult> ObtenerLineaPedido(int pedido, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TOTAL_LINEA_PEDIDO_PTCResult> result;
                try
                {
                    result = context.PROC_OBT_TOTAL_LINEA_PEDIDO_PTC(pedido, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_TOTAL_LINEA_PEDIDO_PTC", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }

        public string ActualizarPedidoLinea(int pedido, int pedido_linea, int cantidad, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_PEDIDO_LINEA(pedido, pedido_linea, cantidad, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_PEDIDO_LINEA", e);
                    result = null;

                }
                context.Connection.Close();
                return result;


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


        public string EliminarPedidoLinea(int pedido, int pedido_linea,string articulo ,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_DEL_PEDIDO_LINEA(pedido, pedido_linea, articulo, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_DEL_PEDIDO_LINEA", e);
                    result = null;

                }
                context.Connection.Close();
                return result;
            }
        }


        public string ActualizaClientePedido(string cliente, int pedido, string usuario, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    //context.GrabarUsuarioActual(usuario);
                    result = context.PROC_EDI_CLIENTE_PEDIDO(cliente, pedido, cia).ToString();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_CLIENTE_PEDIDO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }



        public string ActualizaConFacturaPedido(int pedido, string usuario, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    ///context.GrabarUsuarioActual(usuario);
                    result = context.PROC_EDI_OCUPA_FACTURA_PEDIDO(pedido, cia).ToString();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_OCUPA_FACTURA_PEDIDO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public string CargarPedidoASoftland(int pedido, int rol, string observaciones, string TipoFacturacion, string tipo_pago, string cliente, int id_visita, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    context.PROC_PEDIDO_MIGRAR_SOFTLAND(pedido, rol, observaciones, TipoFacturacion, tipo_pago, cliente, id_visita, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_PEDIDO_MIGRAR_SOFTLAND", e);
                    result = null;
                }
                context.Connection.Close();
                return result;


            }

        }


    }
}
