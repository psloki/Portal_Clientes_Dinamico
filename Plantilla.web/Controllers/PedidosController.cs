using DocumentFormat.OpenXml.Spreadsheet;
using Plantilla.core.Manager;
using Plantilla.web.Models;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    public class PedidosController : Controller
    {
        readonly PedidoManager PedidoM = new PedidoManager();
        // GET: Pedidos
        public ActionResult Index()
        {
            ViewBag.EstadoPedido = PedidoM.Obtener_Estado_Pedido(Convert.ToString(Session["CIA"]));
            ViewBag.EstadoFactura = PedidoM.Obtener_Estado_Factura(Convert.ToString(Session["CIA"]));

            // ViewBag.Clientes = PedidoM.Obtener_Lista_Clientes();
            return View();
        }


        // GET: Pedidos
        public ActionResult Detalles()
        {

            ViewBag.Clientes = PedidoM.Obtener_Lista_Clientes(Convert.ToString(Session["CIA"]));
            ViewBag.Consecutivo_Ped = PedidoM.Obtener_Consecutivo_Ped(Convert.ToString(Session["CIA"]));
            ViewBag.Lista_Articulos = PedidoM.Obtener_Lista_Articulo(Convert.ToString(Session["CIA"]));
            return View();
        }


        [HttpGet]
        public JsonResult Buscar_Codigo_Pedido(string term)
        {
            var resultado = new JsonResult();
            var resul = PedidoM.BuscarCodPedido(term, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpGet]
        public JsonResult Obtener_Pedido_Porta_CLiente(string FECHAINI, string FECHAFIN, string PEDIDO, int ESTADO_PEDIDO, int ESTADO_FACTURA, string ped_vendedor, string ped_cliente)
        {

            // Convertir las cadenas de fecha a formato YYYY-MM-DD
            DateTime fechaInicio, fechaFin;
            DateTime fechaActual = DateTime.Now;

            fechaInicio = DateTime.TryParseExact(FECHAINI, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaInicio)
                ? fechaInicio
                : fechaActual;

            fechaFin = DateTime.TryParseExact(FECHAFIN, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaFin)
                ? fechaFin
                : new DateTime(fechaActual.Year, fechaActual.Month, DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month));

            // Convertir las fechas a formato "YYYY-MM-DD"
            string fechaInicioFormato = fechaInicio.ToString("yyyy-MM-dd");
            string fechaFinFormato = fechaFin.ToString("yyyy-MM-dd");


            if (ped_vendedor == null || ped_vendedor == "") { ped_vendedor = Session["Vendedor"].ToString(); }
            if (ped_cliente == null || ped_cliente == "") { ped_cliente = Session["cliente"].ToString(); }

            var resultado = new JsonResult();
            var resul = PedidoM.ObtenerPed(fechaInicioFormato, fechaFinFormato, PEDIDO, ESTADO_PEDIDO, ESTADO_FACTURA, ped_vendedor, ped_cliente, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpGet]
        public JsonResult Obtener_SubTablaLineaPedido(string pedido)
        {
            var resultado = new JsonResult();
            var resul = PedidoM.Obtener_SubTabla_LineaPedido(pedido, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        /// GENERAR EXCEL INDEX PEDIDO 

        [HttpPost]
        public ActionResult GenerarExcel_Pedido_Batch(string pedido)
        {
            JsonResult json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                string nombreReporte = string.Concat("ReporteExcelPedidoBatch.xlsx");
                string path = Path.Combine(Server.MapPath("~/Temp"), nombreReporte);
                SLDocument document = new SLDocument();
                SLStyle style = new SLStyle();
                SLStyle estiloTitulo = new SLStyle();
                SLStyle estiloEncabezado = new SLStyle();
                SLStyle estiloEncabezado2 = new SLStyle();
                SLStyle estiloPiePagina = new SLStyle();
                SLStyle stylePiePagina = new SLStyle();


                //**** TITULO CENTRADO EN LA HOJA DE CALCULO ****//
                //estiloTitulo.Border.BottomBorder.BorderStyle = BorderStyleValues.Medium;
                //estiloTitulo.Border.LeftBorder.BorderStyle = BorderStyleValues.Medium;
                //estiloTitulo.Border.RightBorder.BorderStyle = BorderStyleValues.Medium;
                //estiloTitulo.Border.TopBorder.BorderStyle = BorderStyleValues.Medium;
                estiloTitulo.Font.Bold = true;
                estiloTitulo.Font.FontName = "Calibri";
                estiloTitulo.Font.FontSize = 16;
                estiloTitulo.Alignment.JustifyLastLine = true;
                estiloTitulo.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightGray, System.Drawing.Color.Black);
                estiloTitulo.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
                //****Bordes del titulo (TERMINO)****//

                //****Bordes del titulo (INICIO - ESTILO ENCABEZADO)****//
                estiloEncabezado.Border.BottomBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado.Border.LeftBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado.Border.RightBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado.Border.TopBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Yellow, System.Drawing.Color.Black);
                estiloEncabezado.Font.Bold = true;
                estiloEncabezado.Font.FontName = "Calibri";
                estiloEncabezado.Font.FontSize = 12;

                estiloEncabezado2.Border.BottomBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado2.Border.LeftBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado2.Border.RightBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado2.Border.TopBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado2.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightSteelBlue, System.Drawing.Color.Black);
                estiloEncabezado2.Font.Bold = true;
                estiloEncabezado2.Font.FontName = "Calibri";
                estiloEncabezado2.Font.FontSize = 12;
                //****Bordes del titulo (TERMINO - ESTILO ENCABEZADO)****//

                //****Bordes de la tabla principal (INICIO)****//
                style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                style.Font.FontName = "Calibri";
                style.Font.FontSize = 10;
                document.CreatePatternFill();

                var resul = PedidoM.GenerarReporteExcelPedidoBatch(pedido, Convert.ToString(Session["CIA"]));

                document.MergeWorksheetCells("A1", "K1");
                //document.MergeWorksheetCells("A2", "D2");
                document.SetCellStyle("A2", estiloEncabezado2);
                document.SetCellStyle("B2", estiloEncabezado2);
                document.SetCellStyle("C2", estiloEncabezado2);
                document.SetCellStyle("D2", estiloEncabezado2);
                document.SetCellStyle("E2", estiloEncabezado2);
                document.SetCellStyle("F2", estiloEncabezado2);
                document.SetCellStyle("G2", estiloEncabezado2);
                document.SetCellStyle("H2", estiloEncabezado2);
                document.SetCellStyle("I2", estiloEncabezado2);
                document.SetCellStyle("J2", estiloEncabezado2);
                document.SetCellStyle("K2", estiloEncabezado2);



                document.RenameWorksheet("Sheet1", "REPORTE EXCEL PEDIDO EN BATCH");

                document.SetCellValue(2, 1, "PEDIDO");
                document.SetCellValue(2, 2, "CLIENTE");
                document.SetCellValue(2, 3, "ESTADO PEDIDO");
                document.SetCellValue(2, 4, "FECHA PEDIDO");
                document.SetCellValue(2, 5, "FACTURA");
                document.SetCellValue(2, 6, "SEGUIR FAC");
                document.SetCellValue(2, 7, "DIAS");
                document.SetCellValue(2, 8, "AUTORIZADO");
                document.SetCellValue(2, 9, "SUBTOTAL");
                document.SetCellValue(2, 10, "TOTAL");
                document.SetCellValue(2, 11, "FECHA CREACION");



                int cont = 3;
                foreach (var item in resul)
                {

                    document.SetCellValue(cont, 1, item.PEDIDO.ToString());
                    document.SetCellValue(cont, 2, item.CLIENTE.ToString());
                    document.SetCellValue(cont, 3, item.ESTADO_PEDIDO_DESCRIPCION.ToString());
                    document.SetCellValue(cont, 4, item.FECHA_PEDIDO.ToString());
                    document.SetCellValue(cont, 5, item.FACTURA);
                    document.SetCellValue(cont, 6, item.SEGUIR_FAC_DESCRIPCION.ToString());
                    document.SetCellValue(cont, 7, item.DIAS.ToString());
                    document.SetCellValue(cont, 8, item.AUTORIZADO.ToString());
                    //document.SetCellValue(cont, 9, Convert.ToDouble(item.SUBTOTAL).ToString("N", new CultureInfo("en-US")));
                    document.SetCellValue(cont, 9, Convert.ToDouble(item.SUBTOTAL));
                    document.SetCellStyle(cont, 9, new SLStyle() { FormatCode = "#,##0.00" });
                    //document.SetCellValue(cont, 10, Convert.ToDouble(item.TOTAL).ToString("N", new CultureInfo("en-US")));
                    document.SetCellValue(cont, 10, Convert.ToDouble(item.TOTAL));
                    document.SetCellStyle(cont, 10, new SLStyle() { FormatCode = "#,##0.00" });


                    document.SetCellValue(cont, 11, item.CreateDate.ToString());

                    document.AutoFitColumn(cont, 1);
                    document.AutoFitColumn(cont, 2);
                    document.AutoFitColumn(cont, 3);
                    document.AutoFitColumn(cont, 4);
                    document.AutoFitColumn(cont, 5);
                    document.AutoFitColumn(cont, 6);
                    document.AutoFitColumn(cont, 7);
                    document.AutoFitColumn(cont, 8);
                    document.AutoFitColumn(cont, 9);
                    document.AutoFitColumn(cont, 10);
                    document.AutoFitColumn(cont, 11);

                    //****Estilos en las celdas (INICIO)****//
                    document.SetCellStyle(cont, 1, style);
                    document.SetCellStyle(cont, 2, style);
                    document.SetCellStyle(cont, 3, style);
                    document.SetCellStyle(cont, 4, style);
                    document.SetCellStyle(cont, 5, style);
                    document.SetCellStyle(cont, 6, style);
                    document.SetCellStyle(cont, 7, style);
                    document.SetCellStyle(cont, 8, style);
                    document.SetCellStyle(cont, 9, style);
                    document.SetCellStyle(cont, 10, style);
                    document.SetCellStyle(cont, 11, style);



                    //****Estilos en las celdas (TERMINO)****//
                    cont++;
                }

                document.SaveAs(path);
                var ubicacion = $"Temp/{nombreReporte}"; //colocar el nombre de la publicacion para que funcione 
                json.Data = ubicacion;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return Json(2);
                //respuesta = "0";
                //throw ex;
            }

            return json;
        }

        [HttpGet]
        public ActionResult DescargarArchivo_ExcelPedido_Index(string ruta)
        {
            try
            {
                ruta = Path.Combine(Server.MapPath("~/"), ruta);
                string nombreArchivo = string.Concat("Reporte Excel Batch", DateTime.Now.ToString("yyyy_MM_dd_HHmm"), ".xlsx");
                return File(ruta, "application/vnd.ms-excel", nombreArchivo);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }


        }


        public JsonResult MigrarApedido(string consecutivo, string cliente, int tipo, string vendedor, string observacion, string bodega)
        {
            string cia;
            string usuario = Session["Usuario"].ToString();
            int idPedido = 0;
            if (Session["CIA"] == null)
            {
                throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            }
            else
            {
                cia = Session["CIA"].ToString();
            }

            var JsonResult = new JsonResult();
            idPedido = IngresaPedido(consecutivo, cliente, tipo, vendedor, observacion, usuario,bodega, cia);
            JsonResult = Json(idPedido, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
            // Cambiar el valor de la sesión

        }

        private int IngresaPedido(string consecutivo, string cliente, int tipo, string vendedor, string observacion, string usuario,string bodega ,string cia)
        {
            var consePedido = PedidoM.InsertarPedido(consecutivo, cliente, tipo, vendedor, observacion, usuario,bodega ,cia);
            
            return consePedido; // si consecutivo es 0 es porque dio error.
        }


        [HttpGet]
        public JsonResult Obtener_Info_Articulo(string articulo,string cliente)
        {
            string cia;
            cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = PedidoM.ObtenerInformacion_Articulo(articulo,cliente, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpGet]
        public JsonResult Obtener_Info_Articulo_DCTO(string articulo, string cliente,int cantidad_min)
        {
            string cia;
            cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = PedidoM.ObtenerInformacion_Articulo_Dcto(articulo, cliente, cantidad_min, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult Ingresar_Linea_Pedido(int pedido, string articulo, int cantidad, int existencia, decimal porc_descuento)
        {
            string cia;
            if (Session["CIA"] == null)
            {
                throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            }
            else
            {
                cia = Session["CIA"].ToString();
            }
            var resultado = PedidoM.InsertarPedidoLinea(pedido, articulo, cantidad, existencia, porc_descuento, cia);

            if (resultado != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }


        [HttpGet]
        public JsonResult Obtener_Linea_Pedido(int pedidoIndex)
        {
            var resultado = new JsonResult();
            var resul = PedidoM.ObtenerLineaPedido(pedidoIndex, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }



        public JsonResult Actualizar_Linea_Pedido(int pedido, int pedido_linea, int cantidad)
        {
            string cia;
            if (Session["CIA"] == null)
            {
                throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            }
            else
            {
                cia = Session["CIA"].ToString();
            }
            var resultado = PedidoM.ActualizarPedidoLinea(pedido, pedido_linea, cantidad, cia);

            if (resultado != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }

        public JsonResult ObtenerPDF(string factura)
        {
            var resultado = PedidoM.ObtenerPDF(factura, Convert.ToString(Session["CIA"]));
            var JsonResult = Json(resultado, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;
        }

        public JsonResult Eliminar_Linea_Pedido(int pedido, int pedido_linea,string articulo)
        {
            string cia;
            if (Session["CIA"] == null)
            {
                throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            }
            else
            {
                cia = Session["CIA"].ToString();
            }
            var resultado = PedidoM.EliminarPedidoLinea(pedido, pedido_linea, articulo, cia);

            if (resultado != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }


        public JsonResult CargarPedidoASoftland(int pedido, string observaciones, string cliente, int id_visita)
        {
            string cia;
            string TipoFacturacion = "0";

            if (Session["CIA"] == null)
            {
                throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            }
            else
            {
                cia = Session["CIA"].ToString();
            }

            var result = new JsonResult();
           
            var rol = int.Parse((Session["ROL"] != null ? Session["ROL"].ToString() : "3"));
           
            var resultado = "0";

           
            if (TipoFacturacion == "0" || TipoFacturacion == null)
            {
                TipoFacturacion = "POSTEMPORADA";
            }

            

            resultado = PedidoM.CargarPedidoASoftland(pedido, rol, observaciones, TipoFacturacion, "NO", cliente, id_visita, cia);
            

            var auxresult = new
            {
                resp = resultado,
                //bloq = bloqueado
            };
            result = Json(auxresult, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        public ActionResult VerTracking(string pedido)
        {

            TrackingToken trackingToken = new TrackingToken();
            trackingToken.pedido = pedido;
            trackingToken.usuario = Session["Usuario"].ToString();
            trackingToken.cia = Session["CIA"].ToString();
            trackingToken.GenerarToken();
            if (trackingToken.token == null)
            {
                return Redirect("~/Pedidos?mensaje=No se puede ver trancking");
            }
            string tracking = ConfigurationManager.AppSettings["tracking"].ToString();
            string url = $"{tracking}?code={trackingToken.token}";
            return Redirect(url);

        }
    }
}