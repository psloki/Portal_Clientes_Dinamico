using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json.Linq;
using Plantilla.core.Manager;
using Plantilla.web.Filters;  
using Plantilla.web.Helpers;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebGrease;

namespace Plantilla.web.Controllers
{


    [PermisosModulo(Modulo = 6, SubModulo = 9,
                    SesKey = "PERMISO_MOD_COBRADOR",
                    PermisoNecesario = "4",
                    CampoSesionForzar = "Cod_Cobrador")]
    [PermisosModulo(Modulo = 6, SubModulo = 9,
                    SesKey = "PERMISO_MOD_VENDEDOR",
                    PermisoNecesario = "5",
                    CampoSesionForzar = "Vendedor")]

    [PermisosModulo(Modulo = 6, SubModulo = 9,
                    SesKey = "PERMISO_MOD_CLIENTE",
                    PermisoNecesario = "6")]
    public class EstadosCuentaController : Controller
    {
        readonly EstadoCuentaManager EstadoCuentasM = new EstadoCuentaManager();
        private const string DocsElectronicosPathKey = "DocsElectronicosPath";
        private const string CrystalDbUserKey = "CrystalDbUser";
        private const string CrystalDbPasswordKey = "CrystalDbPassword";

        #region Metodos para el login automatico hacia el ecpmmerce
        private static string CrearFirmaSso(string texto, string secreto)
        {
            using (var hmac = new HMACSHA256(
                Encoding.UTF8.GetBytes(secreto)))
            {
                var bytes = hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(texto));

                return BitConverter
                    .ToString(bytes)
                    .Replace("-", "")
                    .ToLowerInvariant();
            }
        }


        [HttpGet]
        public ActionResult IrEcommerce(string cliente)
        {
            var usuario = Convert.ToString(Session["Usuario"]).Trim();

            if (string.IsNullOrWhiteSpace(usuario))
            {
                return RedirectToAction("Index", "Login");
            }

            cliente = (cliente ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(cliente))
            {
                return new HttpStatusCodeResult(
                    400,
                    "No se indicó el cliente."
                );
            }

            var ecommerce =
                ConfigurationManager
                    .AppSettings["EcomercePedido"];

            var secreto =
                ConfigurationManager
                    .AppSettings["EcommerceSsoSecret"];

            if (string.IsNullOrWhiteSpace(ecommerce) ||
                string.IsNullOrWhiteSpace(secreto))
            {
                return new HttpStatusCodeResult(
                    500,
                    "Configuración SSO incompleta."
                );
            }

            ecommerce = ecommerce.TrimEnd('/');

            // La firma durará solamente unos minutos.
            var ts = DateTime.UtcNow.Ticks.ToString(
                CultureInfo.InvariantCulture
            );

            var textoFirmar =
                usuario + "|" +
                cliente + "|" +
                ts;

            var firma =
                CrearFirmaSso(
                    textoFirmar,
                    secreto
                );

            var url =
                ecommerce +
                "/Login/Portal" +
                "?usuario=" +
                HttpUtility.UrlEncode(usuario) +
                "&cliente=" +
                HttpUtility.UrlEncode(cliente) +
                "&ts=" +
                HttpUtility.UrlEncode(ts) +
                "&sig=" +
                HttpUtility.UrlEncode(firma);

            return Redirect(url);
        }
        #endregion

        private static string ObtenerRutaDocsElectronicos()
        {
            var ruta = ConfigurationManager.AppSettings[DocsElectronicosPathKey];
            if (string.IsNullOrWhiteSpace(ruta))
            {
                ruta = ConfigurationManager.AppSettings["FileSystemProdusoft"];
            }
            return ruta;
        }

        private static string ObtenerCrystalDbUser()
        {
            var user = ConfigurationManager.AppSettings[CrystalDbUserKey];
            return string.IsNullOrWhiteSpace(user) ? "SA" : user;
        }

        private static string ObtenerCrystalDbPassword()
        {
            var pwd = ConfigurationManager.AppSettings[CrystalDbPasswordKey];
            return string.IsNullOrWhiteSpace(pwd) ? "B1@dm1n" : pwd;
        }
        
        // GET: EstadosCuenta
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("45"))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Semana = EstadoCuentasM.ObtenerSemana_Anio(Convert.ToString(Session["CIA"]));
            ViewBag.Rutas  = EstadoCuentasM.ListaRutas(Convert.ToString(Session["CIA"]));
            ViewBag.Condicion_Pago = EstadoCuentasM.Obtener_Condicion_Pago(Convert.ToString(Session["CIA"]));
            ViewBag.EstadoVisita = EstadoCuentasM.Obtener_Estado_Visita(Convert.ToString(Session["CIA"]));
            ViewBag.TipoVisita = EstadoCuentasM.ObtenerTipoVisita(Convert.ToString(Session["CIA"]));

            return View();
        }



        [HttpGet]
        public JsonResult Obtener_Estado_Cuenta_CLiente(string semanamulti, string cocatenar_diasemana, string ec_saldo, string ec_condicion_pago, string ec_estado_visita, string ec_cliente, string ec_fechas, string ec_vendedor, string ec_cobrador)
        {
            if (semanamulti == "") { semanamulti = "0"; }
            if (cocatenar_diasemana == "" || cocatenar_diasemana == null) { cocatenar_diasemana = "0"; }
            if (ec_fechas == "" || ec_fechas == null) { ec_fechas = "0"; } 
            if (ec_cliente == null || ec_cliente == "") { ec_cliente = Session["cliente"].ToString(); }

            if (ec_vendedor == null || ec_vendedor == "") { ec_vendedor = Session["Vendedor"].ToString(); }
            if (ec_cobrador == null || ec_cobrador == "") { ec_cobrador= Session["Cod_Cobrador"].ToString(); }

            
            var resultado = new JsonResult();
            var resul = EstadoCuentasM.ObtenerEstadoCuentaCLiente(semanamulti, cocatenar_diasemana, ec_saldo, ec_condicion_pago, ec_estado_visita, ec_vendedor, ec_cobrador, ec_cliente, ec_fechas, Convert.ToString(Session["CIA"]));

            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpGet]
        public JsonResult Obtener_SubTablaDocumentos(string cliente)
        {
            var resultado = new JsonResult();
            var resul = EstadoCuentasM.Obtener_SubTabla_Documentos(cliente, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }
        public void printEstadoCuentav1(string cliente)
        {

            string tipo = "S"; //Session["tipoCliente"].ToString();
            if (tipo == "S")
            {
                try
                {
                    bool isValid = true;
                    this.HttpContext.Session["ReportName"] = "EstadoCuentaCorporativoProdusoft.rpt";
                    string strReportName = System.Web.HttpContext.Current.Session["ReportName"] as string; // Usar "as" para evitar excepción si el valor es null
                    
                    if (string.IsNullOrEmpty(strReportName))
                    {
                        isValid = false;
                    }
                    if (isValid)
                    {
                        ReportDocument rd = new ReportDocument();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/Rpts//") + strReportName; // Combinar ruta con el nombre del archivo
                        rd.Load(strRptPath);
                        rd.SetDatabaseLogon(ObtenerCrystalDbUser(), ObtenerCrystalDbPassword());
                        if (!string.IsNullOrEmpty(cliente))
                        {
                            rd.SetParameterValue("CLIENTE", cliente);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "EstadoCuentaCorporativoProdusoft");
                        }
                        Session["ReportName"] = null;
                        Session["rptIdentificacion"] = null;
                    }
                    else
                    {
                        Response.Write("<H2>Nothing Found; No Report name found</H2>");
                    }
                }
                catch (Exception ex)
                {
                    // Capturar y manejar adecuadamente las excepciones
                    Response.Write(ex.ToString());
                }
            }

            else
            {
                try
                {
                    bool isValid = true;
                    this.HttpContext.Session["ReportName"] = "EstadoCuentaProdusoft.rpt";
                    string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();

                    if (string.IsNullOrEmpty(strReportName))
                    {
                        isValid = false;
                    }

                    if (isValid)
                    {
                        ReportDocument rd = new ReportDocument();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Rpts//" + strReportName;
                        rd.Load(strRptPath);
                        rd.SetDatabaseLogon(ObtenerCrystalDbUser(), ObtenerCrystalDbPassword());
                        if (!string.IsNullOrEmpty(cliente))
                        {
                            rd.SetParameterValue("CLIENTE", cliente);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "EstadoCuentaProdusoft");
                        }
                        Session["ReportName"] = null;
                        Session["rptIdentificacion"] = null;
                    }
                    else
                    {
                        Response.Write("<H2>Nothing Found; No Report name found</H2>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }

        public void printEstadoCuenta(string cliente)
        {
            string tipo = "S"; //Session["tipoCliente"].ToString();
            if (tipo == "S")
            {
                try
                {
                    bool isValid = true;
                    this.HttpContext.Session["ReportName"] = "EstadoCuentaCorporativoProdusoft.rpt";
                    string strReportName = System.Web.HttpContext.Current.Session["ReportName"] as string;

                    if (string.IsNullOrEmpty(strReportName))
                    {
                        isValid = false;
                    }
                    if (isValid)
                    {
                        ReportDocument rd = new ReportDocument();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/Rpts//") + strReportName;
                        rd.Load(strRptPath);
                        rd.SetDatabaseLogon(ObtenerCrystalDbUser(), ObtenerCrystalDbPassword());
                        if (!string.IsNullOrEmpty(cliente))
                        {
                            rd.SetParameterValue("CLIENTE", cliente);

                            string tempPath = Path.Combine(Server.MapPath("~/Temp/"), "EstadoCuentaCorporativoProdusoft.pdf");

                            if (System.Web.HttpContext.Current.Request.QueryString["download"] == "true")
                            {
                                // Forzar la descarga
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, tempPath);
                                // Llamar al método de descarga
                                System.Web.HttpContext.Current.Response.Redirect(Url.Action("DescargarArchivoPdf", new { ruta = "Temp/EstadoCuentaCorporativoProdusoft.pdf" }));
                            }
                            else
                            {
                                // Mostrar en el navegador
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "EstadoCuentaProdusoft");
                            }
                        }
                        Session["ReportName"] = null;
                        Session["rptIdentificacion"] = null;
                    }
                    else
                    {
                        Response.Write("<H2>Nothing Found; No Report name found</H2>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
        }


        [HttpGet]
        public ActionResult DescargarArchivoPdf(string ruta)
        {
            try
            {
                ruta = Path.Combine(Server.MapPath("~/"), ruta);
                string nombreArchivo = string.Concat("Reporte_Estados_Cuenta", DateTime.Now.ToString("yyyy_MM_dd_HHmm"), ".pdf");
                return File(ruta, "application/pdf", nombreArchivo);
            }
            catch (Exception ex)
            {
                Manager.WriteLog("*** EstadosCuentaController ***", "ERROR: " + ex.ToString());
                return null;
            }
        }


        [HttpPost]
        public JsonResult EnviarCorreoEstadoCuentaBatch(string Lineas, string correoextra)
        {

            try
            {
                JArray lineas = JArray.Parse(Lineas);
                foreach (JObject item in lineas)
                {
                    ProcesarEnvioEstadoCuentaBatch(item["cliente"].ToString(), item["correo"].ToString(), correoextra);
                }
            }
            catch (Exception ex)
            {

                Manager.WriteLog("*** ProcesarEnvioEstadoCuentaBatch ***", "ERROR: " + ex.Message);
            }
            
            
            return Json(1);
        }


        [HttpPost]
        public JsonResult ProcesarEnvioEstadoCuentaBatch(string cliente, string correo, string correoextra)
        {
            try
            {
               //int posicionGuion = cliente.IndexOf('-');
               // string numeroCliente = cliente.Substring(0, posicionGuion);
                string correosCompletos = correo + ";" + correoextra;
                EnviarEstadoCuenta(cliente, correosCompletos);
               
            }
            catch (Exception ex)
            {
                Manager.WriteLog("*** ProcesarEnvioEstadoCuentaBatch ***", "ERROR: " + ex.Message);
            }
            return Json(1);
        }


        public JsonResult EnviarEstadoCuenta(string cliente, string correo)
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
            var resultado = new JsonResult();
            // funcion que envia el estado de cuenta
            var respuesta = "1";
            //

            string tipo = "S"; //Session["tipoCliente"].ToString();

            //string tipo = "S";
            if (tipo == "S")
            {
                this.HttpContext.Session["Cliente"] = cliente; //en el session se pone cualquier nombre yo le puse los mismos del crystal para no perderme
                this.HttpContext.Session["ReportName"] = "EstadoCuentaCorporativoProdusoft.rpt";
                string rutaDestino = ObtenerRutaDocsElectronicos();
                try
                {
                    bool isValid = true;
                    System.Web.HttpContext.Current.Session["CLIENTE"] = cliente;
                    string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
                    string FileSystemProdusoft = rutaDestino;
                    
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
                            rptH.SetDatabaseLogon(ObtenerCrystalDbUser(), ObtenerCrystalDbPassword());
                            if (!string.IsNullOrEmpty(cliente))
                            {
                                string numReporte = "EstadoCuentaCorporativo_"+cliente;
                                string nombrePDF = numReporte;
                                nombrePDF = nombrePDF.Replace(" ", "");
                                nombrePDF = nombrePDF.Replace(":", "");
                                nombrePDF = nombrePDF.Replace(".", "");

                                rptH.SetParameterValue("CLIENTE", cliente);

                                rptH.ExportToDisk(ExportFormatType.PortableDocFormat, $"{FileSystemProdusoft}/" + nombrePDF + ".pdf");//esto genera el pdf en la ruta
                                if (correo.StartsWith(";"))
                                {
                                    correo = correo.Substring(1);
                                }

                                var correoData = EstadoCuentasM.ObtenerDataCorreoEstadoCuenta(cliente, tipo, nombrePDF, correo, cia);
                                if (correoData == null)
                                {
                                    respuesta = "0";
                                    Manager.WriteLog("*** EstadosCuentaController.EnviarEstadoCuenta-SQLDATA ***", "No se obtuvo asunto/cuerpo/correos desde SQL. Cliente: " + cliente);
                                }
                                else
                                {
                                    string errorSmtp;
                                    var rutaAdjunto = $"{FileSystemProdusoft}/" + nombrePDF + ".pdf";
                                    var enviado = SmtpMailHelper.EnviarCorreo(correoData.Correos, correoData.Asunto, correoData.Cuerpo, rutaAdjunto, out errorSmtp);
                                    if (enviado)
                                    {
                                        respuesta = "1";
                                    }
                                    else
                                    {
                                        respuesta = "0";
                                    }
                                    if (!enviado)
                                    {
                                        Manager.WriteLog("*** EstadosCuentaController.EnviarEstadoCuenta-SMTP ***", "RutaAdjunto: " + rutaAdjunto + " | ERROR: " + errorSmtp);
                                    }
                                }

                            }
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
                    Manager.WriteLog("*** EstadosCuentaController.EnviarEstadoCuenta(S) ***", "Ruta: " + rutaDestino + " | ERROR: " + ex.ToString());
                    respuesta = "0";
                }
            }
            else
            {
                this.HttpContext.Session["Cliente"] = cliente; //en el session se pone cualquier nombre yo le puse los mismos del crystal para no perderme
                this.HttpContext.Session["ReportName"] = "EstadoCuentaProdusoft.rpt";
                string rutaDestino = ObtenerRutaDocsElectronicos();
                try
                {
                    bool isValid = true;
                    System.Web.HttpContext.Current.Session["CLIENTE"] = cliente;
                    string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
                    string FileSystemProdusoft = rutaDestino;

                    if (string.IsNullOrEmpty(strReportName))
                    {
                        isValid = false;
                    }
                    if (isValid)
                    {
                        using (ReportClass rptH = new ReportClass())
                        {
                            rptH.FileName = Server.MapPath("~/") + "Rpts/" + strReportName;
                            rptH.Load();
                            rptH.SetDatabaseLogon(ObtenerCrystalDbUser(), ObtenerCrystalDbPassword());
                            if (!string.IsNullOrEmpty(cliente))
                            {
                                string numReporte = "EstadoCuenta_" + cliente;//lo puede cambiar por el nombre que quiera tenga el pdf
                                string nombrePDF = numReporte;
                                nombrePDF = nombrePDF.Replace(" ", "");
                                nombrePDF = nombrePDF.Replace(":", "");
                                nombrePDF = nombrePDF.Replace(".", "");

                                rptH.SetParameterValue("CLIENTE", cliente);
                                // rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "EstadoCuenta");
                                rptH.ExportToDisk(ExportFormatType.PortableDocFormat, $"{FileSystemProdusoft}/" + nombrePDF + ".pdf");//esto genera el pdf en la ruta
                                var correoData = EstadoCuentasM.ObtenerDataCorreoEstadoCuenta(cliente, tipo, nombrePDF, correo, cia);
                                if (correoData == null)
                                {
                                    respuesta = "0";
                                    Manager.WriteLog("*** EstadosCuentaController.EnviarEstadoCuenta-SQLDATA ***", "No se obtuvo asunto/cuerpo/correos desde SQL. Cliente: " + cliente);
                                }
                                else
                                {
                                    string errorSmtp;
                                    var rutaAdjunto = $"{FileSystemProdusoft}/" + nombrePDF + ".pdf";
                                    var enviado = SmtpMailHelper.EnviarCorreo(correoData.Correos, correoData.Asunto, correoData.Cuerpo, rutaAdjunto, out errorSmtp);
                                    if (enviado)
                                    {
                                        respuesta = "1";
                                    }
                                    else
                                    {
                                        respuesta = "0";
                                    }
                                    if (!enviado)
                                    {
                                        Manager.WriteLog("*** EstadosCuentaController.EnviarEstadoCuenta-SMTP ***", "RutaAdjunto: " + rutaAdjunto + " | ERROR: " + errorSmtp);
                                    }
                                }
                            }
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
                    Manager.WriteLog("*** EstadosCuentaController.EnviarEstadoCuenta(N) ***", "Ruta: " + rutaDestino + " | ERROR: " + ex.ToString());
                    respuesta = "0";
                }
            }

            resultado = Json(respuesta, JsonRequestBehavior.AllowGet);
            resultado.MaxJsonLength = int.MaxValue;
            return resultado;
        }




        [HttpPost]
        public ActionResult GenerarExcelCuadreAuxiliar(string cliente, string fecha_doc_ini, string fecha_doc_fin)
        {
            JsonResult json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            string rutaDestino = ObtenerRutaDocsElectronicos();
            string path = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(rutaDestino) || !Directory.Exists(rutaDestino))
                {
                    rutaDestino = Server.MapPath("~/Temp");
                }

                Directory.CreateDirectory(rutaDestino);
                string nombreReporte = $"ReporteAplicacion_{Guid.NewGuid():N}.xlsx";
                path = Path.Combine(rutaDestino, nombreReporte);

                SLDocument document = new SLDocument();
                SLStyle style = new SLStyle();
                SLStyle estiloTitulo = new SLStyle();
                SLStyle estiloEncabezado = new SLStyle();
                SLStyle estiloEncabezado2 = new SLStyle();
                SLStyle estiloEncabezado3 = new SLStyle();
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

                estiloEncabezado3.Border.BottomBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado3.Border.LeftBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado3.Border.RightBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado3.Border.TopBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEncabezado3.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightSalmon, System.Drawing.Color.Black);
                estiloEncabezado3.Font.Bold = true;
                estiloEncabezado3.Font.FontName = "Calibri";
                estiloEncabezado3.Font.FontSize = 12;
                //****Bordes del titulo (TERMINO - ESTILO ENCABEZADO)****//

                //****Bordes de la tabla principal (INICIO)****//
                style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                style.Font.FontName = "Calibri";
                style.Font.FontSize = 10;
                document.CreatePatternFill();

                var resul = EstadoCuentasM.GenerarExcel_Reporte_Auxiliar(cliente, fecha_doc_ini, fecha_doc_fin, Convert.ToString(Session["CIA"]));



                document.MergeWorksheetCells("A1", "Q1");
                //document.MergeWorksheetCells("A2", "D2");
                document.SetCellStyle("A2", estiloEncabezado3);
                document.SetCellStyle("B2", estiloEncabezado3);
                document.SetCellStyle("C2", estiloEncabezado);
                document.SetCellStyle("D2", estiloEncabezado);
                document.SetCellStyle("E2", estiloEncabezado);
                document.SetCellStyle("F2", estiloEncabezado);
                document.SetCellStyle("G2", estiloEncabezado);
                document.SetCellStyle("H2", estiloEncabezado);
                document.SetCellStyle("I2", estiloEncabezado2);
                document.SetCellStyle("J2", estiloEncabezado2);
                document.SetCellStyle("K2", estiloEncabezado2);
                document.SetCellStyle("L2", estiloEncabezado2);
                document.SetCellStyle("M2", estiloEncabezado2);
                document.SetCellStyle("N2", estiloEncabezado2);
                document.SetCellStyle("O2", estiloEncabezado3);
                document.SetCellStyle("P2", estiloEncabezado3);
                document.SetCellStyle("Q2", estiloEncabezado3);

                document.RenameWorksheet("Sheet1", "REPORTE DE APLICACIONES");
                document.SetCellValue(2, 1, "CLIENTE");
                document.SetCellValue(2, 2, "NOMBRE CLIENTE");
                document.SetCellValue(2, 3, "DOCUMENTO CRÉDITO");
                document.SetCellValue(2, 4, "REFERENCIA CRÉDITO");
                document.SetCellValue(2, 5, "FECHA DOC. CRÉDITO");
                document.SetCellValue(2, 6, "TIPO CRÉDITO");
                document.SetCellValue(2, 7, "MONTO CRÉDITO");
                document.SetCellValue(2, 8, "SALDO CRÉDITO");
                document.SetCellValue(2, 9, "DOCUMENTO DÉBITO");
                document.SetCellValue(2, 10, "REFERENCIA DÉBITO");
                document.SetCellValue(2, 11, "FECHA DOC. DÉBITO");
                document.SetCellValue(2, 12, "TIPO DÉBITO");
                document.SetCellValue(2, 13, "MONTO DÉBITO");
                document.SetCellValue(2, 14, "SALDO DÉBITO");
                document.SetCellValue(2, 15, "FECHA APLICACIÓN");
                document.SetCellValue(2, 16, "MONTO APLICADO");
                document.SetCellValue(2, 17, "USUARIO CREACIÓN");


                int cont = 3;
                foreach (var item in resul)
                {

                    document.SetCellValue(cont, 1, Convert.ToString(item.cliente));
                    document.SetCellValue(cont, 2, Convert.ToString(item.cliente_nombre));
                    document.SetCellValue(cont, 3, Convert.ToString(item.documento_credito));
                    document.SetCellValue(cont, 4, Convert.ToString(item.referencia_credito));
                    document.SetCellValue(cont, 5, Convert.ToString(item.fecha_documento_credito));
                    document.SetCellValue(cont, 6, Convert.ToString(item.tipo_credito));

                    //document.SetCellValue(cont, 6, Convert.ToDouble(item.monto_credito).ToString("N", new CultureInfo("en-US")));
                    document.SetCellValue(cont, 7, Convert.ToDouble(item.monto_credito));
                    document.SetCellStyle(cont, 7, new SLStyle() { FormatCode = "#,##0.00" });

                    //document.SetCellValue(cont, 7, Convert.ToDouble(item.saldo_credito).ToString("N", new CultureInfo("en-US")));
                    document.SetCellValue(cont, 8, Convert.ToDouble(item.saldo_credito));
                    document.SetCellStyle(cont, 8, new SLStyle() { FormatCode = "#,##0.00" });
                    document.SetCellValue(cont, 9, Convert.ToString(item.documento_debito));
                    document.SetCellValue(cont, 10, Convert.ToString(item.referencia_debito));
                    document.SetCellValue(cont, 11, Convert.ToString(item.fecha_documento_debito));
                    document.SetCellValue(cont, 12, Convert.ToString(item.tipo_debito));

                    //document.SetCellValue(cont, 11, Convert.ToDouble(item.monto_debito).ToString("N", new CultureInfo("en-US")));
                    document.SetCellValue(cont, 13, Convert.ToDouble(item.monto_debito));
                    document.SetCellStyle(cont, 13, new SLStyle() { FormatCode = "#,##0.00" });

                    //document.SetCellValue(cont, 12, Convert.ToDouble(item.saldo_debito).ToString("N", new CultureInfo("en-US")));
                    document.SetCellValue(cont, 14, Convert.ToDouble(item.saldo_debito));
                    document.SetCellStyle(cont, 14, new SLStyle() { FormatCode = "#,##0.00" });

                    document.SetCellValue(cont, 15, Convert.ToString(item.fecha_aplicacion));
                    //document.SetCellValue(cont, 14, Convert.ToDouble(item.monto_aplicado).ToString("N", new CultureInfo("en-US")));
                    document.SetCellValue(cont, 16, Convert.ToDouble(item.monto_aplicado));
                    document.SetCellStyle(cont, 16, new SLStyle() { FormatCode = "#,##0.00" });

                    document.SetCellValue(cont, 17, Convert.ToString(item.usuario_creacion));



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

                    //****Estilos en las celdas (TERMINO)****//
                    cont++;
                }


                document.SaveAs(path);
                var ubicacion = path; //colocar el nombre de la publicacion para que funcione 
                json.Data = ubicacion;
            }
            catch (Exception ex)
            {
                Manager.WriteLog("*** EstadosCuentaController.GenerarExcelCuadreAuxiliar ***", "RutaBase: " + rutaDestino + " | RutaArchivo: " + path + " | ERROR: " + ex.ToString());
                //return Json(2);
                //respuesta = "0";
                //throw ex;
            }

            return json;
        }

        [HttpGet]
        public ActionResult DescargarExcelCuadreAuxiliar(string cliente, string fecha_doc_ini, string fecha_doc_fin)
        {
            try
            {
                var generado = GenerarExcelCuadreAuxiliar(cliente, fecha_doc_ini, fecha_doc_fin) as JsonResult;
                var ruta = generado?.Data as string;

                if (string.IsNullOrWhiteSpace(ruta) || !System.IO.File.Exists(ruta))
                {
                    return HttpNotFound("No se pudo generar el reporte de auxiliares.");
                }

                string nombreArchivo = string.Concat("Reporte de Aplicaciones", DateTime.Now.ToString("yyyy_MM_dd_HHmm"), ".xlsx");
                return File(ruta, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
            catch (Exception ex)
            {
                Manager.WriteLog("*** EstadosCuentaController.DescargarExcelCuadreAuxiliar ***", "ERROR: " + ex.ToString());
                return new HttpStatusCodeResult(500, "Error generando el reporte de auxiliares.");
            }
        }
        [HttpGet]
        public ActionResult DescargarArchivo(string ruta)
        {
            try
            {
                ruta = Path.IsPathRooted(ruta) ? ruta : Path.Combine(Server.MapPath("~/"), ruta);
                if (!System.IO.File.Exists(ruta))
                {
                    return HttpNotFound("No se encontro el archivo solicitado.");
                }
                string nombreArchivo = string.Concat("Reporte de Aplicaciones", DateTime.Now.ToString("yyyy_MM_dd_HHmm"), ".xlsx");//new FileInfo(ruta).Name;
                return File(ruta, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
            catch (Exception ex)
            {

                Manager.WriteLog("*** EstadosCuentaController ***", "ERROR: " + ex.ToString());
                return null;
            }


        }

        [HttpPost]
        public JsonResult EnviarReportesAuxiliar(
     string cliente,
     string correo,
     string fecha_doc_ini,
     string fecha_doc_fin)
        {
            var resultado = new JsonResult();
            var respuesta = "1";
            string rutaDestino = ObtenerRutaDocsElectronicos();
            string path = string.Empty;

            // --- Variable que necesitamos fuera del using -----------------------------
            bool sinDatos = false;

            try
            {
                string FileSystemProdusoft = rutaDestino;
                string nombreReporte = $"CuadreAxiliar_{DateTime.Now:yyyy_MM_dd_HHmm}.xlsx";
                path = Path.Combine($"{FileSystemProdusoft}/", nombreReporte);

                using (var document = new SLDocument())
                {
                    // --- Estilos --------------------------------------------------------
                    var style = new SLStyle();
                    var estiloTitulo = new SLStyle();
                    var estiloEncabezado = new SLStyle();
                    var estiloEncabezado2 = new SLStyle();
                    var estiloEncabezado3 = new SLStyle();

                    estiloTitulo.Font.Bold = true;
                    estiloTitulo.Font.FontName = "Calibri";
                    estiloTitulo.Font.FontSize = 16;
                    estiloTitulo.Fill.SetPattern(PatternValues.Solid,
                                                 System.Drawing.Color.LightGray,
                                                 System.Drawing.Color.Black);
                    estiloTitulo.SetHorizontalAlignment(HorizontalAlignmentValues.Center);

                    Action<SLStyle, System.Drawing.Color> ConfiguraEncabezado = (e, color) =>
                    {
                        e.Border.BottomBorder.BorderStyle = BorderStyleValues.Medium;
                        e.Border.LeftBorder.BorderStyle = BorderStyleValues.Medium;
                        e.Border.RightBorder.BorderStyle = BorderStyleValues.Medium;
                        e.Border.TopBorder.BorderStyle = BorderStyleValues.Medium;
                        e.Font.Bold = true;
                        e.Font.FontName = "Calibri";
                        e.Font.FontSize = 12;
                        e.Fill.SetPattern(PatternValues.Solid, color, System.Drawing.Color.Black);
                    };

                    ConfiguraEncabezado(estiloEncabezado, System.Drawing.Color.LightGray);
                    ConfiguraEncabezado(estiloEncabezado2, System.Drawing.Color.LightSteelBlue);
                    ConfiguraEncabezado(estiloEncabezado3, System.Drawing.Color.LightSalmon);

                    style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                    style.Font.FontName = "Calibri";
                    style.Font.FontSize = 10;

                    // --- Consultar datos ------------------------------------------------
                    var resul = EstadoCuentasM.Enviar_Reporte_Cuadre_Auxiliar(
                                    cliente, fecha_doc_ini, fecha_doc_fin, Convert.ToString(Session["CIA"]));
                    sinDatos = resul == null || !resul.Any();

                    // --- Hoja y encabezados ---------------------------------------------
                    document.RenameWorksheet("Sheet1", "REPORTE DE APLICACIONES");

                    string[] headers = {
                "CLIENTE","NOMBRE CLIENTE","DOCUMENTO CRÉDITO","REFERENCIA CRÉDITO",
                "FECHA DOC. CRÉDITO","TIPO CRÉDITO","MONTO CRÉDITO","SALDO CRÉDITO",
                "DOCUMENTO DÉBITO","REFERENCIA DÉBITO","FECHA DOC. DÉBITO","TIPO DÉBITO",
                "MONTO DÉBITO","SALDO DÉBITO","FECHA APLICACIÓN","MONTO APLICADO",
                "USUARIO CREACIÓN"
            };

                    for (int c = 0; c < headers.Length; c++)
                    {
                        document.SetCellValue(2, c + 1, headers[c]);

                        SLStyle headerStyle;
                        if (c <= 1)
                            headerStyle = estiloEncabezado3;
                        else if (c >= 8 && c <= 13)
                            headerStyle = estiloEncabezado2;
                        else
                            headerStyle = estiloEncabezado;

                        document.SetCellStyle(2, c + 1, headerStyle);
                    }

                    // --- Cargar filas si hay datos -------------------------------------
                    int fila = 3;
                    if (!sinDatos)
                    {
                        foreach (var item in resul)
                        {
                            document.SetCellValue(fila, 1, item.cliente);
                            document.SetCellValue(fila, 2, item.cliente_nombre);
                            document.SetCellValue(fila, 3, item.documento_credito);
                            document.SetCellValue(fila, 4, item.referencia_credito);
                            document.SetCellValue(fila, 5, item.fecha_documento_credito);
                            document.SetCellValue(fila, 6, item.tipo_credito);

                            document.SetCellValue(fila, 7, Convert.ToDouble(item.monto_credito));
                            document.SetCellStyle(fila, 7, new SLStyle { FormatCode = "#,##0.00" });

                            document.SetCellValue(fila, 8, Convert.ToDouble(item.saldo_credito));
                            document.SetCellStyle(fila, 8, new SLStyle { FormatCode = "#,##0.00" });

                            document.SetCellValue(fila, 9, item.documento_debito);
                            document.SetCellValue(fila, 10, item.referencia_debito);
                            document.SetCellValue(fila, 11, item.fecha_documento_debito);
                            document.SetCellValue(fila, 12, item.tipo_debito);

                            document.SetCellValue(fila, 13, Convert.ToDouble(item.monto_debito));
                            document.SetCellStyle(fila, 13, new SLStyle { FormatCode = "#,##0.00" });

                            document.SetCellValue(fila, 14, Convert.ToDouble(item.saldo_debito));
                            document.SetCellStyle(fila, 14, new SLStyle { FormatCode = "#,##0.00" });

                            document.SetCellValue(fila, 15, item.fecha_aplicacion);
                            document.SetCellValue(fila, 16, Convert.ToDouble(item.monto_aplicado));
                            document.SetCellStyle(fila, 16, new SLStyle { FormatCode = "#,##0.00" });

                            document.SetCellValue(fila, 17, item.usuario_creacion);

                            for (int col = 1; col <= 17; col++)
                                document.SetCellStyle(fila, col, style);

                            fila++;
                        }
                    }

                    // --- Ajustar ancho de columnas --------------------------------------
                    for (int col = 1; col <= 17; col++)
                        document.AutoFitColumn(col);

                    // --- Guardar --------------------------------------------------------
                    document.SaveAs(path);
                }
                // --- Enviar correo con el adjunto --------------------------------------
                var correoData = EstadoCuentasM.ObtenerDataCorreoReporteAuxiliar(fecha_doc_ini, fecha_doc_fin, correo, nombreReporte, Convert.ToString(Session["CIA"]));
                if (correoData == null)
                {
                    respuesta = "0";
                    Manager.WriteLog("*** EstadosCuentaController.EnviarReportesAuxiliar-SQLDATA ***", "No se obtuvo asunto/cuerpo/correos desde SQL. Cliente: " + cliente);
                }
                else
                {
                    string errorSmtp;
                    var enviado = SmtpMailHelper.EnviarCorreo(correoData.Correos, correoData.Asunto, correoData.Cuerpo, path, out errorSmtp);
                    if (enviado)
                                    {
                                        respuesta = "1";
                                    }
                                    else
                                    {
                                        respuesta = "0";
                                    }
                    if (!enviado)
                    {
                        Manager.WriteLog("*** EstadosCuentaController.EnviarReportesAuxiliar-SMTP ***", "RutaAdjunto: " + path + " | ERROR: " + errorSmtp);
                    }
                }

                if (sinDatos) respuesta = "2";
            }
            catch (Exception ex)
            {
                Manager.WriteLog("*** EstadosCuentaController.EnviarReportesAuxiliar ***", "RutaBase: " + rutaDestino + " | RutaArchivo: " + path + " | ERROR: " + ex.ToString());
                respuesta = "0";               // Error
            }

            resultado.Data = respuesta;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        /// GENERAR EXCEL PARA REPORTE INDEX 
        [HttpPost]
        public ActionResult GenerarExcel_EstadoCuenta_Batch(string cliente)
        {
            JsonResult json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                string nombreReporte = string.Concat("ReporteExcelEstadoCuentaBatch.xlsx");
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

                var resul = EstadoCuentasM.GenerarReporteExcelBatch(cliente, Convert.ToString(Session["CIA"]));

                document.MergeWorksheetCells("A1", "N1");
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
                document.SetCellStyle("L2", estiloEncabezado2);
                document.SetCellStyle("M2", estiloEncabezado2);
                document.SetCellStyle("N2", estiloEncabezado2);


                document.RenameWorksheet("Sheet1", "REPORTE EXCEL ESTADO DE CUENTAS BATCH");

                document.SetCellValue(2, 1, "ESTADO");
                document.SetCellValue(2, 2, "CLIENTE");
                document.SetCellValue(2, 3, "NOMBRE CLIENTE");
                document.SetCellValue(2, 4, "ALIAS");
                document.SetCellValue(2, 5, "COD. VENDEDOR");
                document.SetCellValue(2, 6, "NOMBRE VENDEDOR");
                document.SetCellValue(2, 7, "NOMBRE VENDEDOR");
                document.SetCellValue(2, 8, "EMAIL");
                document.SetCellValue(2, 9, "TELEFONO");
                document.SetCellValue(2, 10, "CONDICION PAGO");
                document.SetCellValue(2, 11, "DIAS NETO");
                document.SetCellValue(2, 12, "SALDO");
                document.SetCellValue(2, 13, "FECHA ULTIMA VISITA");
                document.SetCellValue(2, 14, "SEMANA");


                int cont = 3;
                foreach (var item in resul)
                {

                    document.SetCellValue(cont, 1, item.ESTADO.ToString());
                    document.SetCellValue(cont, 2, item.CLIENTE.ToString());
                    document.SetCellValue(cont, 3, item.CLI_NOMBRE.ToString());
                    document.SetCellValue(cont, 4, item.ALIAS.ToString());
                    document.SetCellValue(cont, 5, item.COD_VENDEDOR.ToString());
                    document.SetCellValue(cont, 6, item.NOMBRE_VENDEDOR.ToString());
                    document.SetCellValue(cont, 7, item.VEN_NOMBRE.ToString());
                    document.SetCellValue(cont, 8, item.EMAIL.ToString());
                    document.SetCellValue(cont, 9, item.TELEFONO.ToString());
                    document.SetCellValue(cont, 10, item.CONDICION_PAGO.ToString());
                    document.SetCellValue(cont, 11, item.DIAS_NETO.ToString());

                    //document.SetCellValue(cont, 12, Convert.ToDouble(item.SALDO).ToString("N", new CultureInfo("en-US")));
                    document.SetCellValue(cont, 12, Convert.ToDouble(item.SALDO));
                    document.SetCellStyle(cont, 12, new SLStyle() { FormatCode = "#,##0.00" });

                    document.SetCellValue(cont, 13, item.FECHA_ULTIMA_VISITA.ToString());
                    document.SetCellValue(cont, 14, item.SEMANA.ToString());



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


                    //****Estilos en las celdas (TERMINO)****//
                    cont++;
                }

                document.SaveAs(path);
                var ubicacion = $"Temp/{nombreReporte}"; //colocar el nombre de la publicacion para que funcione 
                json.Data = ubicacion;
            }
            catch (Exception ex)
            {
                Manager.WriteLog("*** EstadosCuentaController ***", "ERROR: " + ex.ToString());
                //return Json(2);
                //respuesta = "0";
                //throw ex;
            }

            return json;
        }


        [HttpGet]
        public ActionResult DescargarArchivo_Excel_Index(string ruta)
        {
            try
            {
                ruta = Path.Combine(Server.MapPath("~/"), ruta);
                string nombreArchivo = string.Concat("Reporte Excel Batch", DateTime.Now.ToString("yyyy_MM_dd_HHmm"), ".xlsx");
                return File(ruta, "application/vnd.ms-excel", nombreArchivo);
            }
            catch (Exception ex)
            {

                Manager.WriteLog("*** EstadosCuentaController ***", "ERROR: " + ex.ToString());
                return null;
            }


        }

        public JsonResult RegistrarVisita(string cliente, string tipo_visita, DateTime fecha_visita, string latitud, string longitud, string estado_visita,
                                          string comentario, string vendedor, string usuario,string doc_venc_descripcion,string direccion)
        {
            string cia;
            int idVisita = 0;
            if (Session["CIA"] == null)
            {
                throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            }
            else
            {
                cia = Session["CIA"].ToString();
            }
            Session["cliente"] = cliente;
            var JsonResult = new JsonResult();
            idVisita = IngresarVisita(cliente, tipo_visita, fecha_visita, latitud, longitud, estado_visita, comentario, vendedor, usuario, doc_venc_descripcion, direccion, cia);
            Session["Visita"] = idVisita;
            JsonResult = Json(idVisita, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
            // Cambiar el valor de la sesión

        }


        private int IngresarVisita(string cliente, string tipo_visita, DateTime fecha_visita, string latitud, string longitud, string estado_visita,
                                          string comentario, string vendedor, string usuario,string doc_venc_descripcion,string direccion, string cia)
        {
            var consVisita = EstadoCuentasM.Insertar_Visita(cliente, tipo_visita, fecha_visita, latitud, longitud, estado_visita, comentario, vendedor, usuario, doc_venc_descripcion, direccion, cia);
            return consVisita; // si consecutivo es 0 es porque dio error.
        }

        public JsonResult ObtenerPDF(string factura)
        {
            var resultado = EstadoCuentasM.ObtenerPDF(factura, Convert.ToString(Session["CIA"]));
            var JsonResult = Json(resultado, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;
        }



        [HttpPost]
        public JsonResult Obtener_Documentos_VencidosCliente(string cliente)
        {
            var resultado = new JsonResult();
            var resul = EstadoCuentasM.Obtener_Documentos_Vencidos(cliente, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        public JsonResult ObtenerTipoVisita()
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            var resultado = new JsonResult();
            var resul = EstadoCuentasM.ObtenerTipoVisita(Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }



        [HttpPost]
        public JsonResult Enviar_FE_Cliente(string cliente,string correos, string documento)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            var resultado = new JsonResult();
            var resul = EstadoCuentasM.Enviar_Correo_CLiente_FE(cliente, correos, documento, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

    }

}


