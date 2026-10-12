using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Plantilla.core.Manager;
using SpreadsheetLight;
using DocumentFormat.OpenXml;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;

namespace Plantilla.web.Controllers
{
    public class SeguimientoController : Controller
    {
        readonly SeguimientoManager SeguimentoM = new SeguimientoManager();
        // GET: Seguimiento
        public ActionResult Index()
        {

            ViewBag.Rutas = SeguimentoM.ListaRutas(Convert.ToString(Session["CIA"]));
            ViewBag.EstadoFactura = SeguimentoM.ObtenerSeguimiento_Factura(Convert.ToString(Session["CIA"]));
            ViewBag.EstadoPedido = SeguimentoM.Obtener_Estado_Pedido(Convert.ToString(Session["CIA"]));
            ViewBag.Clientes = SeguimentoM.Obtener_Lista_Clientes(Convert.ToString(Session["CIA"]));
            ViewBag.Vendedor = SeguimentoM.Obtener_Lista_Agente(Convert.ToString(Session["CIA"]));
            ViewBag.Usuario = SeguimentoM.Obtener_Lista_Usuario();
            ViewBag.Cobrador = SeguimentoM.Obtener_Lista_Cobrador(Convert.ToString(Session["CIA"]));
            ViewBag.Sucesos = SeguimentoM.GetSucesos_filtro(Convert.ToString(Session["CIA"]));
            return View();
        }

        /// dejar esta parte para pantalla de traxking.
        /// 
        [HttpGet]
        public JsonResult Obtener_SubTablaSeguimiento_Tracking(string pedido)
        {
            var resultado = new JsonResult();
            var resul = SeguimentoM.Obtener_SubTabla_SeguimientoTracking(pedido, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;//Json(resul, JsonRequestBehavior.AllowGet);
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpGet]
        public JsonResult Obtener_Seguimiento(string fechaInicio, string fechaFin, string estado_pedido, string fecha_prometida, string estado_factura, string rutamulti, string fpedido, string ffactura,
                                              string fcliente, string agente, string usuario, string suceso, string cobrador, string FModEstadoPedido, string FModEstadoFactura, string FModSeguiFactura_rango_ini, string FModSeguiFactura_rango_fin)
        {
            int.TryParse(estado_pedido, out int EstadoPedido);
            int.TryParse(estado_factura, out int EstadoFactura);
            if (ffactura == "ND") { ffactura = "0"; }
            if (fecha_prometida == "Todas") { fecha_prometida = "0"; }
            if (FModEstadoPedido == "Todas") { FModEstadoPedido = "0"; }
            if (FModEstadoFactura == "Todas") { FModEstadoFactura = "0"; }
            if (FModSeguiFactura_rango_ini == "Todas") { FModSeguiFactura_rango_ini = "0"; }
            if (FModSeguiFactura_rango_fin == "Todas") { FModSeguiFactura_rango_fin = "0"; }

            var resultado = new JsonResult();
            var resul = SeguimentoM.Obtener_Seguimiento(fechaInicio, fechaFin, EstadoPedido, fecha_prometida, EstadoFactura, rutamulti, fpedido, ffactura, fcliente, agente, usuario, suceso, cobrador, FModEstadoPedido, FModEstadoFactura, FModSeguiFactura_rango_ini, FModSeguiFactura_rango_fin, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;//Json(resul, JsonRequestBehavior.AllowGet);
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        //[HttpGet]
        //public JsonResult Obtener_Estado_PedidoSeguimiento()
        //{
        //    var resultado = new JsonResult();
        //    var resul = SeguimentoM.Obtener_Estado_Pedido_Seguimiento();
        //    resultado.Data = resul;
        //    resultado.MaxJsonLength = int.MaxValue;
        //    resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return resultado;
        //}


        //[HttpGet]
        //public JsonResult Obtener_Estado_FacturaSeguimiento()
        //{
        //    var resultado = new JsonResult();
        //    var resul = SeguimentoM.ObtenerEstado_Factura();
        //    resultado.Data = resul;
        //    resultado.MaxJsonLength = int.MaxValue;
        //    resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return resultado;
        //}


        [HttpGet]
        public JsonResult Obtener_Cod_PedidoSeg_Busqueda(string term)
        {
            var resultado = new JsonResult();
            var resul = SeguimentoM.ObtenerCod_Pedido_Segui_Buscar(term, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }



        [HttpGet]
        public JsonResult Obtener_Cod_FacturaSeg_Busqueda(string term)
        {
            var resultado = new JsonResult();
            var resul = SeguimentoM.ObtenerCod_Factura_Seg_Busqueda(term, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        //[HttpGet]
        //public JsonResult Obtener_ListaAgente()
        //{
        //    var resultado = new JsonResult();
        //    var resul = SeguimentoM.Obtener_Lista_Agente(Convert.ToString(Session["CIA"]));
        //    resultado.Data = resul;
        //    resultado.MaxJsonLength = int.MaxValue;
        //    resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return resultado;
        //}



        [HttpPost]
        public JsonResult DeleteSeguimiento(int id, string numpedido)
        {
            var resultado = new JsonResult();
            var resul = SeguimentoM.DELETE_SEGUIMIENTO(id, numpedido, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult Actualiza_Observacion_Seg(int id_seguimiento, string pedido, string observacion, string usuario)
        {
            var resultado = new JsonResult();
            var resul = SeguimentoM.ActualizaObservacionSeguimiento(id_seguimiento, pedido, observacion, usuario, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult Actualiza_Suceso_Seguimiento(string pedido, string suceso, string correo, string usuario, string fecha, string observacion)
        {
            var resultado = new JsonResult();
            var resul = SeguimentoM.ActualizaSucesoSeguimiento(pedido, suceso, correo, usuario, fecha, observacion, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public ActionResult GenerarExcelSeguimientos(string pedido, int estado_pedido, int estado_factura)
        {
            JsonResult json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                string nombreReporte = string.Concat("Reporte Seguimientos.xlsx");
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

                var resul = SeguimentoM.GenerarExcel_Reporte_Seguimientos(pedido, estado_pedido, estado_factura, Convert.ToString(Session["CIA"]));

                document.MergeWorksheetCells("A1", "T1");
                //document.MergeWorksheetCells("A2", "D2");
                document.SetCellStyle("A2", estiloEncabezado);
                document.SetCellStyle("B2", estiloEncabezado);
                document.SetCellStyle("C2", estiloEncabezado);
                document.SetCellStyle("D2", estiloEncabezado);
                document.SetCellStyle("E2", estiloEncabezado);
                document.SetCellStyle("F2", estiloEncabezado);
                document.SetCellStyle("G2", estiloEncabezado);
                document.SetCellStyle("H2", estiloEncabezado);
                document.SetCellStyle("I2", estiloEncabezado);
                document.SetCellStyle("J2", estiloEncabezado);
                document.SetCellStyle("K2", estiloEncabezado);
                document.SetCellStyle("L2", estiloEncabezado);
                document.SetCellStyle("M2", estiloEncabezado2);
                document.SetCellStyle("N2", estiloEncabezado2);
                document.SetCellStyle("O2", estiloEncabezado2);
                document.SetCellStyle("P2", estiloEncabezado2);
                document.SetCellStyle("Q2", estiloEncabezado2);
                document.SetCellStyle("R2", estiloEncabezado2);
                document.SetCellStyle("S2", estiloEncabezado2);
                document.SetCellStyle("T2", estiloEncabezado2);

                document.RenameWorksheet("Sheet1", "REPORTE DE SEGUIMIENTOS");
                //document.SetCellValue(1, 1, "REPORTE DE RUTAS");
                //document.SetCellValue(2, 1, "");
                document.SetCellValue(2, 1, "PEDIDO");
                document.SetCellValue(2, 2, "FECHA PEDIDO");
                document.SetCellValue(2, 3, "FACTURA");
                document.SetCellValue(2, 4, "FECHA PROYECTADA");
                document.SetCellValue(2, 5, "ESTADO PEDIDO DESCRIPCION");
                document.SetCellValue(2, 6, "OBSERVACIONES");
                document.SetCellValue(2, 7, "FECHA CAMBIA ESTADO PED");
                document.SetCellValue(2, 8, "SEGUIR FAC DESCRIPCION");
                document.SetCellValue(2, 9, "FECHA CAMBIA ESTADO FACTURA");
                document.SetCellValue(2, 10, "SUCESO");
                document.SetCellValue(2, 11, "FECHA SUCESO");
                document.SetCellValue(2, 12, "RUTA");
                document.SetCellValue(2, 13, "CLIENTE");
                document.SetCellValue(2, 14, "AGENTE");
                document.SetCellValue(2, 15, "COBRADOR");
                document.SetCellValue(2, 16, "FECHA FACTURA");
                document.SetCellValue(2, 17, "FECHA VENCE");
                document.SetCellValue(2, 18, "CONDICION PAGO");
                document.SetCellValue(2, 19, "TOTAL FACTURA");
                document.SetCellValue(2, 20, "SALDO");

                int cont = 3;
                foreach (var item in resul)
                {

                    document.SetCellValue(cont, 1, item.PEDIDO.ToString());
                    document.SetCellValue(cont, 2, item.FECHA_PEDIDO.ToString());
                    document.SetCellValue(cont, 3, item.FACTURA.ToString());
                    document.SetCellValue(cont, 4, item.FECHA_PROYECTADA.ToString());
                    document.SetCellValue(cont, 5, item.ESTADO_PEDIDO_DESCRIPCION.ToString());
                    document.SetCellValue(cont, 6, item.OBSERVACIONES.ToString());
                    document.SetCellValue(cont, 7, item.FECHA_CAMBIA_ESTADO_PED.ToString());
                    document.SetCellValue(cont, 8, item.SEGUIR_FAC_DESCRIPCION.ToString());
                    document.SetCellValue(cont, 9, item.FECHA_CAMBIA_ESTADO_FACTURA.ToString());
                    document.SetCellValue(cont, 10, item.SUCESO.ToString());
                    document.SetCellValue(cont, 11, item.FECHA_SUCESO.ToString());
                    document.SetCellValue(cont, 12, item.RUTA.ToString());
                    document.SetCellValue(cont, 13, item.CLIENTE.ToString());
                    document.SetCellValue(cont, 14, item.AGENTE.ToString());
                    document.SetCellValue(cont, 15, item.COBRADOR.ToString());
                    document.SetCellValue(cont, 16, item.FECHA_FACTURA.ToString());
                    document.SetCellValue(cont, 17, item.FECHA_VENCE.ToString());
                    document.SetCellValue(cont, 18, item.CONDICION_PAGO.ToString());
                    document.SetCellValue(cont, 19, Convert.ToDouble(item.TOTAL_FACTURA).ToString("N", new CultureInfo("en-US")));
                    document.SetCellValue(cont, 20, Convert.ToDouble(item.SALDO).ToString("N", new CultureInfo("en-US")));

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
                    document.AutoFitColumn(cont, 12);
                    document.AutoFitColumn(cont, 13);
                    document.AutoFitColumn(cont, 14);
                    document.AutoFitColumn(cont, 15);
                    document.AutoFitColumn(cont, 16);
                    document.AutoFitColumn(cont, 17);
                    document.AutoFitColumn(cont, 18);
                    document.AutoFitColumn(cont, 19);
                    document.AutoFitColumn(cont, 20);

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
                    document.SetCellStyle(cont, 12, style);
                    document.SetCellStyle(cont, 13, style);
                    document.SetCellStyle(cont, 14, style);
                    document.SetCellStyle(cont, 15, style);
                    document.SetCellStyle(cont, 16, style);
                    document.SetCellStyle(cont, 17, style);
                    document.SetCellStyle(cont, 18, style);
                    document.SetCellStyle(cont, 19, style);
                    document.SetCellStyle(cont, 20, style);
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
        public ActionResult DescargarArchivo(string ruta)
        {
            try
            {
                ruta = Path.Combine(Server.MapPath("~/"), ruta);
                string nombreArchivo = string.Concat("Reporte Seguimientos", DateTime.Now.ToString("yyyy_MM_dd_HHmm"), ".xlsx");//new FileInfo(ruta).Name;
                return File(ruta, "application/vnd.ms-excel", nombreArchivo);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }


        }



        public JsonResult EnviarReportesRutas(string fechaPrometida, string correo)
        {

            var resultado = new JsonResult();
            var respuesta = "1";
            try
            {
                string nombreReporte = string.Concat("Reporte Seguimientos Rutas", DateTime.Now.ToString("yyyy_MM_dd_HHmm"), ".xlsx");

                string path = Path.Combine(Server.MapPath("~/Temp"), nombreReporte); //Path.Combine(Server.MapPath("~/Temp"), nombreReporte); //Path.Combine("C:/Reportes", nombreReporte); agregar cuando se haga la publicacion
                SLDocument document = new SLDocument();
                SLStyle style = new SLStyle();
                SLStyle estiloTitulo = new SLStyle();
                SLStyle estiloEncabezado = new SLStyle();
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
                estiloEncabezado.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightGray, System.Drawing.Color.Black);
                estiloEncabezado.Font.Bold = true;
                estiloEncabezado.Font.FontName = "Calibri";
                estiloEncabezado.Font.FontSize = 12;
                //****Bordes del titulo (TERMINO - ESTILO ENCABEZADO)****//

                //****Bordes de la tabla principal (INICIO)****//
                style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                style.Font.FontName = "Calibri";
                style.Font.FontSize = 10;
                document.CreatePatternFill();

                var resul = SeguimentoM.Enviar_Reporte_Ruta_TLA(fechaPrometida, Convert.ToString(Session["CIA"]));

                document.MergeWorksheetCells("A1", "D1");
                //document.MergeWorksheetCells("A2", "D2");
                document.SetCellStyle("A2", estiloEncabezado);
                document.SetCellStyle("B2", estiloEncabezado);
                document.SetCellStyle("C2", estiloEncabezado);
                document.SetCellStyle("D2", estiloEncabezado);
                document.SetCellStyle("E2", estiloEncabezado);
                document.RenameWorksheet("Sheet1", "REPORTE DE RUTAS");
                //document.SetCellValue(1, 1, "REPORTE DE RUTAS");
                //document.SetCellValue(2, 1, "");
                document.SetCellValue(2, 1, "RUTA");
                document.SetCellValue(2, 2, "CLIENTE");
                document.SetCellValue(2, 3, "PEDIDO");
                document.SetCellValue(2, 4, "CONDICION PAGO ");
                document.SetCellValue(2, 5, "OBSERVACIONES");
                int cont = 3;
                foreach (var item in resul)
                {

                    document.SetCellValue(cont, 1, item.RUTA.ToString());
                    document.SetCellValue(cont, 2, item.CLIENTE.ToString());
                    document.SetCellValue(cont, 3, item.PEDIDO.ToString());
                    document.SetCellValue(cont, 4, item.CONDICION_PAGO.ToString());
                    document.SetCellValue(cont, 5, item.OBSERVACIONES.ToString());
                    //document.SetCellValue(cont, 5, item.CONDICION_PAGO.ToString());
                    document.AutoFitColumn(cont, 1);
                    document.AutoFitColumn(cont, 2);
                    document.AutoFitColumn(cont, 3);
                    document.AutoFitColumn(cont, 4);
                    document.AutoFitColumn(cont, 5);



                    //****Estilos en las celdas (INICIO)****//
                    document.SetCellStyle(cont, 1, style);
                    document.SetCellStyle(cont, 2, style);
                    document.SetCellStyle(cont, 3, style);
                    document.SetCellStyle(cont, 4, style);
                    document.SetCellStyle(cont, 5, style);
                    //****Estilos en las celdas (TERMINO)****//
                    cont++;
                }

                document.SaveAs(path);
                respuesta = SeguimentoM.EnviaCorreo_ReporteRutas(fechaPrometida, correo, nombreReporte, Convert.ToString(Session["CIA"]));
                respuesta = "1";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = "0";
            }

            resultado.Data = respuesta;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }



        [HttpPost]
        public JsonResult PurgarBloqueo()
        {
            var resultado = new JsonResult();
            var resul = SeguimentoM.PURGAR_BLOQUEOS(Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }






    }
}