using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Plantilla.core.Entities;
using Plantilla.core.Manager;
using SpreadsheetLight;
using DocumentFormat.OpenXml;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace Plantilla.web.Controllers
{

    public class TrackingController : Controller
    {

        readonly TrackingManager TrackingM = new TrackingManager();

        // GET: Tracking
        public ActionResult Index()
        {
            ViewBag.Rutas = TrackingM.ListaRutas(Convert.ToString(Session["CIA"]));
            ViewBag.HoraCorte = TrackingM.Obtener_HoraCorte(Convert.ToString(Session["CIA"]));
            ViewBag.EstadoFactura = TrackingM.ObtenerSeguimiento_Factura(Convert.ToString(Session["CIA"]));
            ViewBag.EstadoPedido = TrackingM.Obtener_Estado_Pedido(Convert.ToString(Session["CIA"]));
            ViewBag.Clientes = TrackingM.Obtener_Lista_Clientes(Convert.ToString(Session["CIA"]));
            ViewBag.TLA_Estado = TrackingM.ObtenerTLA_Estado(Convert.ToString(Session["CIA"]));
            ViewBag.Vendedor = TrackingM.Obtener_Lista_Agente(Convert.ToString(Session["CIA"]));
            ViewBag.Sucesos = TrackingM.GetSucesos_filtro(Convert.ToString(Session["CIA"]));
            ViewBag.Cobrador = TrackingM.Obtener_Lista_Cobrador(Convert.ToString(Session["CIA"]));
            return View();
        }



        [HttpGet]
        public JsonResult Obtener_PedidoTracking(string fechaInicio, string fechaFin, string estado_pedido, string fecha_prometida, string fecha_prometidaFin, string estado_factura,
                                                 string concatenar, string fpedido, string ffactura, string fcliente, string cocatenar_tla, string corte_tla,
                                                 string agente, string cobrador, string cocatenar_diasemana, int horaencortefuera, string fecha_CxcRecibe_Ini, string fecha_CxcRecibe_Fin)
        {
            int.TryParse(estado_pedido, out int EstadoPedido);
            int.TryParse(estado_factura, out int EstadoFactura);
            if (concatenar == "") { concatenar = "0"; }
            if (cocatenar_tla == "") { cocatenar_tla = "0"; }
            if (cocatenar_diasemana == "" || cocatenar_diasemana == null) { cocatenar_diasemana = "0"; }
            if (fecha_prometida == "Todas") { fecha_prometida = "0"; }
            if (fecha_prometidaFin == "Todas") { fecha_prometidaFin = "0"; }
            if (fecha_CxcRecibe_Ini == "Todas") { fecha_CxcRecibe_Ini = "0"; }
            if (fecha_CxcRecibe_Fin == "Todas") { fecha_CxcRecibe_Fin = "0"; }
            var resultado = new JsonResult();
            var resul = TrackingM.ObtenerPedTracking(fechaInicio, fechaFin, EstadoPedido, fecha_prometida, fecha_prometidaFin, EstadoFactura, concatenar, fpedido, ffactura, fcliente,
                                                     cocatenar_tla, corte_tla, agente, cobrador, cocatenar_diasemana, horaencortefuera, fecha_CxcRecibe_Ini, fecha_CxcRecibe_Fin, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;

        }





        [HttpGet]
        public JsonResult Obtener_Cod_Pedido_Buscar(string term)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ObtenerCod_Pedido_buscar(term, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpGet]
        public JsonResult Obtener_Cod_Factura_Buscar(string term)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ObtenerCod_Factura_Buscar(term, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }





        [HttpPost]
        public JsonResult ActualizaEstadoPedido(string pedido, int estado, DateTime fecha, string observacion, string usuario, string correo)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ActualizaEstadoPedido(pedido, estado, fecha, observacion, usuario, correo, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult DeleteSeguimiento(int id, string numpedido)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.DELETE_SEGUIMIENTO(id, numpedido, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult ActualizaEstadoPedidoMultiple(string pedido, int estado, DateTime fecha_Pedido, string observacion, string usuario, string correo)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ActualizaEstadoPedido_Multiple(pedido, estado, fecha_Pedido, observacion, usuario, correo, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }




        [HttpPost]
        public JsonResult ActualizaEstado_Factura(string pedido, int estado)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ActualizaEstadoFactura(pedido, estado, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }



        [HttpPost]
        public JsonResult ActualizaEstadoFacturaMultiple(string pedido, int estado, string observacion, string usuario)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ActualizaEstadoFactura_Multiple(pedido, estado, observacion, usuario, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult Actualiza_Observacion(string pedido, string observacion, string usuario)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ActualizaObservacion(pedido, observacion, usuario, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult Actualiza_Observacions_SubTabla(int id_seguimiento, string pedido, string observacion, string usuario)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ActualizaObservacion_SubTabla(id_seguimiento, pedido, observacion, usuario, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult Actualiza_Suceso_SubTabla(string pedido, string suceso, string correo, string usuario, string fecha, string observacion)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ActualizaSuceso_SubTabla(pedido, suceso, correo, usuario, fecha, observacion, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult Actualiza_Fecha_Ruta(string pedido, DateTime fecha, string usuario)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ActualizaFechaRuta(pedido, fecha, usuario, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        public JsonResult Actualiza_Fecha_Ruta_Multiple(string pedido, DateTime fecha, string usuario)
        {
            var resultado = new JsonResult();
            var resul = TrackingM.ActualizaFechaRuta_Multiple(pedido, fecha, usuario, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }



        [HttpPost]
        public ActionResult GenerarExcelRutas(string pedido)
        {
            JsonResult json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                string nombreReporte = string.Concat("Reporte Rutas.xlsx");
                string path = Path.Combine(Server.MapPath("~/Temp"), nombreReporte);
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

                var resul = TrackingM.GenerarExcel_Reporte_Ruta(pedido, Convert.ToString(Session["CIA"]));

                document.MergeWorksheetCells("A1", "M1");
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
                document.SetCellStyle("M2", estiloEncabezado);
                document.RenameWorksheet("Sheet1", "REPORTE DE RUTAS");
                //document.SetCellValue(1, 1, "REPORTE DE RUTAS");
                //document.SetCellValue(2, 1, "");
                document.SetCellValue(2, 1, "PEDIDO");
                document.SetCellValue(2, 2, "FACTURA");
                document.SetCellValue(2, 3, "OBSERVACIONES");
                document.SetCellValue(2, 4, "ESTADO PEDIDO DESCRIPCION");
                document.SetCellValue(2, 5, "FECHA PROYECTADA");
                document.SetCellValue(2, 6, "DIAS");
                document.SetCellValue(2, 7, "SEGUIR FAC DESCRIPCION");
                document.SetCellValue(2, 8, "CLIENTE");
                document.SetCellValue(2, 9, "AGENTE");
                document.SetCellValue(2, 10, "RUTA");
                document.SetCellValue(2, 11, "TLA ESTADO");
                document.SetCellValue(2, 12, "FECHA HORA RECIBIDO");
                document.SetCellValue(2, 13, "TIEMPO");

                int cont = 3;
                foreach (var item in resul)
                {

                    document.SetCellValue(cont, 1, item.PEDIDO.ToString());
                    document.SetCellValue(cont, 2, item.FACTURA.ToString());
                    document.SetCellValue(cont, 3, item.OBSERVACIONES.ToString());
                    document.SetCellValue(cont, 4, item.ESTADO_PEDIDO_DESCRIPCION.ToString());
                    document.SetCellValue(cont, 5, item.FECHA_PROYECTADA.ToString());
                    document.SetCellValue(cont, 6, item.DIAS.ToString());
                    document.SetCellValue(cont, 7, item.SEGUIR_FAC_DESCRIPCION.ToString());
                    document.SetCellValue(cont, 8, item.CLIENTE.ToString());
                    document.SetCellValue(cont, 9, item.AGENTE.ToString());
                    document.SetCellValue(cont, 10, item.RUTA.ToString());
                    document.SetCellValue(cont, 11, item.TLA_ESTADO.ToString());
                    document.SetCellValue(cont, 12, item.FECHA_HORA_RECIBIDO.ToString());
                    document.SetCellValue(cont, 13, item.TIEMPO.ToString());
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
                string nombreArchivo = string.Concat("Reporte Rutas ", DateTime.Now.ToString("yyyy_MM_dd_HHmm"), ".xlsx");//new FileInfo(ruta).Name;
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
                string nombreReporte = string.Concat("Reporte Rutas", DateTime.Now.ToString("yyyy_MM_dd_HHmm"), ".xlsx");

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

                var resul = TrackingM.Enviar_Reporte_Ruta_TLA(fechaPrometida, Convert.ToString(Session["CIA"]));

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
                respuesta = TrackingM.EnviaCorreo_ReporteRutas(fechaPrometida, correo, nombreReporte, Convert.ToString(Session["CIA"]));
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




        public JsonResult EnviarReportePDF_TLA(string ESTADO_FAC, string USUARIO, string PEDIDO)
        {
            //string path = "";
            var resultado = new JsonResult();
            // funcion que envia reporte rutas.
            var respuesta = "1";
            //Manager.WriteLog("*** EnviarReportePDF_TLA ***", " LOG 1 VALOR ESTADO: " + ESTADO_FAC);
            if (ESTADO_FAC == "2")
            {
                //Manager.WriteLog("*** EnviarReportePDF_TLA ***", "LOG 2 SI ENTRA AL IF DE ESTADO: " + ESTADO_FAC);
                this.HttpContext.Session["ReportName"] = "ReporteFacturas_ANCLA_A_TLA.rpt";
                try
                {
                    bool isValid = true;
                    string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
                   // Manager.WriteLog("*** EnviarReportePDF_TLA ***", " LOG 3 Nombre reporte: " + strReportName);
                    if (string.IsNullOrEmpty(strReportName))
                    {
                        isValid = false;
                    }
                    if (isValid)
                    {

                     // Manager.WriteLog("*** EnviarReportePDF_TLA ***", " LOG 4 Entra al isValid: " + isValid);

                        using (ReportClass rptH = new ReportClass())
                        {
                            //Manager.WriteLog("*** EnviarReportePDF_TLA ***", "LOG 5 Entra al ReportClass: " + Server.MapPath("~/") + "Rpts//" + strReportName);
                            rptH.FileName = Server.MapPath("~/") + "Rpts//" + strReportName;
                            rptH.Load();
                            //Manager.WriteLog("*** EnviarReportePDF_TLA ***", "LOG 6 Informe cargado correctamente.");
                            rptH.SetDatabaseLogon("SA", "@NcL@@");
                            if (!string.IsNullOrEmpty(ESTADO_FAC))
                            {
                                string numReporte = "ReporteFacturas_ANCLA_A_TLA";//lo puede cambiar por el nombre que quiera tenga el pdf
                                string nombrePDF = numReporte;
                                nombrePDF = nombrePDF.Replace(" ", "");
                                nombrePDF = nombrePDF.Replace(":", "");
                                nombrePDF = nombrePDF.Replace(".", "");
                                rptH.SetParameterValue("@PEDIDO", PEDIDO);
                                rptH.SetParameterValue("@ESTADO_FAC", ESTADO_FAC);
                                rptH.SetParameterValue("@USUARIO", USUARIO);
                                // path = Path.Combine(Path1, nombrePDF);//espacio para ambiente local.
                                //rptH.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/PDFTRACK/") + nombrePDF + ".pdf");//esto genera el pdf en la ruta descomentar cuando se realice la publicacion.
                                //rptH.ExportToDisk(ExportFormatType.PortableDocFormat, path + ".pdf");//esto genera el pdf en la ruta comentar cuando se realice la publicacion. 

                               // Manager.WriteLog("*** EnviarReportePDF_TLA ***", " LOG 7 Ruta del archivo PDF: " + Server.MapPath("~/PDFTRACK/") + nombrePDF + ".pdf");
                                rptH.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/PDFTRACK/") + nombrePDF + ".pdf");


                                respuesta = TrackingM.EnviaCorreo_ReporteRutas_PDF_TLA(ESTADO_FAC, nombrePDF, Convert.ToString(Session["CIA"]));

                            }
                            //rptH.SetParameterValue("CLIENTE", cliente); // y aqui las variables se asignan a las de crystal y ahora SI se deben llamar igual
                            var ruta = rptH.FileName;
                            Session["ReportName"] = null;
                            rptH.Close();
                            rptH.Dispose();
                        }
                    }
                    else
                    {
                        respuesta = "0";
                    }

                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    Manager.WriteLog("*** EnviarReportePDF_TLA ***", "ERROR: " + ex.Message);
                    respuesta = "0";
                }
            }

            else
            {
                if (ESTADO_FAC == "3")
                {
                    this.HttpContext.Session["ReportName"] = "ReporteFacturas_TLA_A_ANCLA.rpt";
                    try
                    {
                        bool isValid = true;
                        string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();

                        if (string.IsNullOrEmpty(strReportName))
                        {
                            isValid = false;
                        }
                        if (isValid)
                        {
                            using (ReportClass rptH = new ReportClass())
                            {
                                rptH.FileName = Server.MapPath("~/") + "Rpts//" + strReportName;
                                rptH.Load();
                                rptH.SetDatabaseLogon("SA", "@NcL@@");
                                if (!string.IsNullOrEmpty(ESTADO_FAC))
                                {
                                    string numReporte = "ReporteFacturas_TLA_A_ANCLA";//lo puede cambiar por el nombre que quiera tenga el pdf
                                    string nombrePDF = numReporte;
                                    nombrePDF = nombrePDF.Replace(" ", "");
                                    nombrePDF = nombrePDF.Replace(":", "");
                                    nombrePDF = nombrePDF.Replace(".", "");
                                    rptH.SetParameterValue("@PEDIDO", PEDIDO);
                                    rptH.SetParameterValue("@ESTADO_FAC", ESTADO_FAC);
                                    rptH.SetParameterValue("@USUARIO", USUARIO);
                                    //path = Path.Combine(Path1, nombrePDF);
                                    rptH.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/PDFTRACK/") + nombrePDF + ".pdf");//esto genera el pdf en la ruta descomentar cuando se realice la publicacion.
                                    //rptH.ExportToDisk(ExportFormatType.PortableDocFormat, path + ".pdf");//esto genera el pdf en la ruta comentar cuando se realice la publicacion. 

                                    respuesta = TrackingM.EnviaCorreo_ReporteRutas_PDF_TLA(ESTADO_FAC, nombrePDF, Convert.ToString(Session["CIA"]));

                                }
                                //rptH.SetParameterValue("CLIENTE", cliente); // y aqui las variables se asignan a las de crystal y ahora SI se deben llamar igual
                                var ruta = rptH.FileName;
                                Session["ReportName"] = null;
                                rptH.Close();
                                rptH.Dispose();
                            }
                        }
                        else
                        {
                            respuesta = "0";
                        }

                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                        Manager.WriteLog("*** EnviarReportePDF_TLA ***", "ERROR: " + ex.Message);
                        respuesta = "0";
                    }
                }
                else
                {
                    if (ESTADO_FAC == "4")
                    {
                        this.HttpContext.Session["ReportName"] = "ReporteFacturas_ANCLA_A_CXC.rpt";
                        try
                        {
                            bool isValid = true;
                            string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();

                            if (string.IsNullOrEmpty(strReportName))
                            {
                                isValid = false;
                            }
                            if (isValid)
                            {
                                using (ReportClass rptH = new ReportClass())
                                {
                                    rptH.FileName = Server.MapPath("~/") + "Rpts//" + strReportName;
                                    rptH.Load();
                                    rptH.SetDatabaseLogon("SA", "@NcL@@");
                                    if (!string.IsNullOrEmpty(ESTADO_FAC))
                                    {
                                        string numReporte = "ReporteFacturas_ANCLA_A_CXC";//lo puede cambiar por el nombre que quiera tenga el pdf
                                        string nombrePDF = numReporte; //+ "-" + ESTADO_FAC;
                                        nombrePDF = nombrePDF.Replace(" ", "");
                                        nombrePDF = nombrePDF.Replace(":", "");
                                        nombrePDF = nombrePDF.Replace(".", "");
                                        rptH.SetParameterValue("@PEDIDO", PEDIDO);
                                        rptH.SetParameterValue("@ESTADO_FAC", ESTADO_FAC);
                                        rptH.SetParameterValue("@USUARIO", USUARIO);
                                        // path = Path.Combine(Path1, nombrePDF);
                                        rptH.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/PDFTRACK/") + nombrePDF + ".pdf");//esto genera el pdf en la ruta descomentar cuando se realice la publicacion.
                                        //rptH.ExportToDisk(ExportFormatType.PortableDocFormat, path + ".pdf");//esto genera el pdf en la ruta comentar cuando se realice la publicacion. 

                                        respuesta = TrackingM.EnviaCorreo_ReporteRutas_PDF_TLA(ESTADO_FAC, nombrePDF, Convert.ToString(Session["CIA"]));

                                    }
                                    //rptH.SetParameterValue("CLIENTE", cliente); // y aqui las variables se asignan a las de crystal y ahora SI se deben llamar igual
                                    var ruta = rptH.FileName;
                                    Session["ReportName"] = null;
                                    rptH.Close();
                                    rptH.Dispose();
                                }
                            }
                            else
                            {
                                respuesta = "0";
                            }

                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex.Message);
                            Manager.WriteLog("*** EnviarReportePDF_TLA ***", "ERROR: " + ex.Message);
                            respuesta = "0";
                        }
                    }

                }

            }

            resultado = Json(respuesta, JsonRequestBehavior.AllowGet);
            resultado.MaxJsonLength = int.MaxValue;
            return resultado;
        }

        [HttpPost]
        public JsonResult PurgarBloqueo()
        {
            var resultado = new JsonResult();
            var resul = TrackingM.PURGAR_BLOQUEOS(Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }




    }
}