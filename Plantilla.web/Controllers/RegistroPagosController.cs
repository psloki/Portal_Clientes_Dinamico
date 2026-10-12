using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json.Linq;
using Plantilla.core.Entities;
using Plantilla.core.Manager;
using Plantilla.core.ViewModels;
using Plantilla.web.Filters;
using Plantilla.web.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Unosquare.Swan;

namespace Plantilla.web.Controllers
{
    [PermisosModulo(Modulo = 7, SubModulo = 14,
                    SesKey = "PERMISO_MOD_COBRADOR",
                    PermisoNecesario = "4",
                    CampoSesionForzar = "Cod_Cobrador")]
    [PermisosModulo(Modulo = 7, SubModulo = 14,
                    SesKey = "PERMISO_MOD_VENDEDOR",
                    PermisoNecesario = "5",
                    CampoSesionForzar = "Vendedor")]
    [PermisosModulo(Modulo = 7, SubModulo = 14,
                    SesKey = "PERMISO_MOD_CLIENTE",
                    PermisoNecesario = "6")]
    public class RegistroPagosController : Controller
    {
        readonly RegistroPagoManager RegistroPagoM = new RegistroPagoManager();
        // GET: RegistroPagos
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("48"))
            {
                return RedirectToAction("Index", "Login");
            }

            string cia = Session["CIA"].ToString();
            string cobrador = Session["Cod_Cobrador"]?.ToString() ?? "0";
            ViewBag.ConseDocc = RegistroPagoM.Obtener_Consecutivo_Documentos(cobrador, cia);
            ViewBag.SubTipoDoc = RegistroPagoM.Obtener_Tipo_Doc(cia);
            ViewBag.BancosDoc = RegistroPagoM.Obtener_Cuenta_Bancaria(cia);
            ViewBag.TipoDoc = RegistroPagoM.Obtener_Tipo(cia);
            ViewBag.ListaCuentas = TransferenciasManager.ObtenerCuentasFiltro(cia);
            ViewBag.ListaClientes = TransferenciasManager.ObtenerClientesFiltro(cia);
            ViewBag.Documentos = RegistroPagoM.GetTiposExpedienteFiltro(cia);
            ViewBag.Tipos = RegistroPagoM.GetTiposExpedienteDigitalPP(cia);

            return View();
        }

        [HttpGet]
        public JsonResult ObtenerInteresesMoraCliente(string cliente, string fechaCorte)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cliente) || cliente == "0")
                {
                    cliente = Session?["cliente"]?.ToString();
                }

                if (string.IsNullOrWhiteSpace(cliente) || cliente == "0")
                {
                    return Json(new { ok = false, mensaje = "Debe seleccionar un cliente.", data = new object[0], total = 0 }, JsonRequestBehavior.AllowGet);
                }

                if (!DateTime.TryParseExact(fechaCorte, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
                {
                    return Json(new { ok = false, mensaje = "La fecha indicada no es válida.", data = new object[0], total = 0 }, JsonRequestBehavior.AllowGet);
                }

                if (fecha.Date < DateTime.Today)
                {
                    return Json(new { ok = false, mensaje = "La fecha no puede ser menor a la fecha actual.", data = new object[0], total = 0 }, JsonRequestBehavior.AllowGet);
                }

                string cia = Session["CIA"].ToString();
                var datos = RegistroPagoM.ObtenerInteresesMoraCliente(cliente, cia, fecha);
                var total = datos.Sum(x => x.INTERESES);

                if (total <= 0)
                {
                    return Json(new { ok = false, mensaje = "El total de intereses es cero. No se puede generar el documento.", data = datos, total }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { ok = true, data = datos, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Plantilla.core.Manager.Manager.WriteLog("*** RegistroPagosController.ObtenerInteresesMoraCliente ***", "ERROR: " + ex);
                return Json(new { ok = false, mensaje = "No se pudieron obtener los intereses de mora.", data = new object[0], total = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GenerarInteresesMoraCliente(string cliente, string fechaCorte)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cliente) || cliente == "0")
                {
                    cliente = Session?["cliente"]?.ToString();
                }

                if (string.IsNullOrWhiteSpace(cliente) || cliente == "0")
                {
                    return Json(new { ok = false, mensaje = "Debe seleccionar un cliente." }, JsonRequestBehavior.AllowGet);
                }

                if (!DateTime.TryParseExact(fechaCorte, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
                {
                    return Json(new { ok = false, mensaje = "La fecha indicada no es válida." }, JsonRequestBehavior.AllowGet);
                }

                if (fecha.Date < DateTime.Today)
                {
                    return Json(new { ok = false, mensaje = "La fecha no puede ser menor a la fecha actual." }, JsonRequestBehavior.AllowGet);
                }

                string cia = Session["CIA"].ToString();
                string usuario = Session["Usuario"]?.ToString();
                var resultado = RegistroPagoM.InsertarInteresesMoraCliente(cliente, cia, fecha, usuario);

                if (resultado == null)
                {
                    return Json(new { ok = false, mensaje = "No se pudo generar el documento de intereses." }, JsonRequestBehavior.AllowGet);
                }

                if (resultado.INTERES_TOTAL <= 0)
                {
                    return Json(new { ok = false, mensaje = "El total de intereses es cero. No se puede generar el documento." }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { ok = true, data = resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Plantilla.core.Manager.Manager.WriteLog("*** RegistroPagosController.GenerarInteresesMoraCliente ***", "ERROR: " + ex);
                return Json(new { ok = false, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Obtener_documentos_clientes_fac(
            string tipoDoc = "0",
            string vendedor = "0",
            string cobrador = "0",
            string cliente = "0",
            int pagina = 1,
            int tamPagina = 20,
            int draw = 1,
            string searchTxt = ""
        )
        {
            if (string.IsNullOrEmpty(cliente) || cliente == "0")
            {
                if (Session?["cliente"] == null || Session?["cliente"].ToString() == "0")
                {
                    return Json(new
                    {
                        draw = draw,
                        recordsTotal = 0,
                        recordsFiltered = 0,
                        data = new List<PCL_PROC_OBT_DOCUMENTO_POR_CLIENTE_V2_SQLResult>()
                    });
                }

                cliente = Session?["cliente"].ToString();
            }

            Func<string, string> norm = v =>
            {
                v = (v ?? "").Trim().ToUpperInvariant();
                return (v == "" || v == "VND" || v == "CND") ? "0" : v;
            };
            vendedor = norm(vendedor);
            cobrador = norm(cobrador);
            cliente = string.IsNullOrWhiteSpace(cliente) ? "0" : cliente.Trim();

            if (vendedor == "0" && Session?["Vendedor"] is string sesVend) vendedor = sesVend;
            if (cobrador == "0" && Session?["Cobrador"] is string sesCob) cobrador = sesCob;
            if (cliente == "0" && Session?["cliente"] is string sesCli) cliente = sesCli;

            /* ---- obtenemos el bloque paginado desde el SP ---- */
            int total;
            var datos = RegistroPagoM.Obtener_Documentos_Cliente(
                            tipoDoc, vendedor, cliente, 1 /*traemos todo*/,
                            int.MaxValue, out total, Convert.ToString(Session["CIA"]));

            if (!string.IsNullOrWhiteSpace(searchTxt))
            {
                datos = datos.Where(d =>
                ((d.DOCUMENTO ?? "")
                     .IndexOf(searchTxt, StringComparison.OrdinalIgnoreCase) >= 0) ||
                ((d.CLIENTE ?? "")
                     .IndexOf(searchTxt, StringComparison.OrdinalIgnoreCase) >= 0) ||
                ((d.CLIENTE_NOMBRE ?? "")
                     .IndexOf(searchTxt, StringComparison.OrdinalIgnoreCase) >= 0) ||
                ((d.COMENTARIO_DEBITO ?? "")
                     .IndexOf(searchTxt, StringComparison.OrdinalIgnoreCase) >= 0)
             ).ToList();
            }

            //int totalFiltrado = datos.Count;
            //var page = datos.Skip((pagina - 1) * tamPagina)
            //                .Take(tamPagina)
            //                .ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = total,
                recordsFiltered = datos.Count,
                data = datos
            });
        }

        public JsonResult InsertarDocumentoCC(string cuenta, string documento, string tipo, int subtipo, string fecha, string concepto,
            string monto, string moneda, string idLinea, string cliente)
        {
            string cia = Session["CIA"].ToString();

            string usuario = Session["Usuario"].ToString();

            var result = TransferenciasManager.InsertarDocumentoCC(cuenta, usuario, documento, tipo, subtipo, fecha, concepto,
            monto, moneda, idLinea, cliente, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;
        }

        public JsonResult ObtenerDataEstadoCuentaBancario(string cuenta_banco, string numero)
        {

            string cia = Session["CIA"].ToString();

            var result = RegistroPagoManager.ObtenerDataEstadoCuentaBancario(cuenta_banco, numero, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ValidarSiExisteNumBancoTEF(string numero, string banco)
        {
            string cia = Session["CIA"].ToString();

            var result = RegistroPagoManager.ValidarSiExisteNumBancoTEF(numero, banco, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        [HttpPost]
        public JsonResult ObtenerTotal_Saldo(string tipoDoc, string vendedor, string cobrador, string cliente)
        {
            string cia = Session["CIA"].ToString();
            tipoDoc = tipoDoc ?? "0";
            vendedor = string.IsNullOrEmpty(vendedor) ? Session["Vendedor"].ToString() : vendedor;
            cliente = string.IsNullOrEmpty(cliente) ? Session["cliente"].ToString() : cliente;

            var resultado = new JsonResult();
            var resul = RegistroPagoM.SaldoTotal(tipoDoc, vendedor, cobrador, cliente, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult obtenerCreditosCliente(string vendedor, string cobrador, string cliente)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Obtener_credito_cliente(vendedor, cobrador, cliente, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult obtenerCobradorCliente(string cliente)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Obtener_cobrador_cliente(cliente, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult obtenerVendedorCliente(string cliente)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Obtener_vendedor_cliente(cliente, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult obtenerTipoDoc()
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Obtener_Tipo_Doc(cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult MostrarBanco()
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Obtener_Cuenta_Bancaria(cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult RegistrarDocumentoSinFactura(string cliente, string documentoCC, string tipo_doc, int subtipo, string cuenta_bancaria, string fecha, string referencia,
                                             string moneda, decimal total_abonado, string concepto, int id_visita, string vendedor, string cobrador, string comentariocob)
        {
            try
            {
                string cia = Session["CIA"].ToString();
                string usuario = Session["Usuario"].ToString();
                cuenta_bancaria = (cuenta_bancaria == "null") ? null : cuenta_bancaria;
                referencia = (referencia == "null") ? null : referencia;
                concepto = (concepto == "null" || concepto == "undefined") ? null : concepto;
                concepto = NormalizarConcepto(concepto);

                var resultado = new JsonResult();
                var resul = RegistroPagoM.Agregar_Documentos(cliente, documentoCC, tipo_doc, subtipo, cuenta_bancaria, fecha, referencia, moneda, total_abonado,
                                                             concepto, null, null, id_visita, vendedor, cobrador, usuario, comentariocob, cia);
                resultado.Data = resul;
                resultado.MaxJsonLength = int.MaxValue;
                resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return resultado;
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult RegistrarDocumento(string cliente, string documentoCC, string tipo_doc, int subtipo, string cuenta_bancaria, string fecha, string referencia,
                                             string moneda, string total_abonado, string concepto, int id_visita, string vendedor, string cobrador, string comentariocob)
        {
            try
            {
                string cia = Session["CIA"].ToString();
                string usuario = Session["Usuario"].ToString();
                string rutaComprobantePrincipal = subirComprantes(Request.Files[0], documentoCC.Replace("|", ""));
                string rutaComprobanteOpcional = subirComprantes(Request.Files[1], documentoCC.Replace("|", ""));
                cuenta_bancaria = (cuenta_bancaria == "null") ? null : cuenta_bancaria;
                referencia = (referencia == "null") ? null : referencia;
                concepto = NormalizarConcepto(concepto);

                var resultado = new JsonResult();
                var resul = RegistroPagoM.Agregar_Documentos(cliente, documentoCC, tipo_doc, subtipo, cuenta_bancaria, fecha, referencia, moneda, Convert.ToDecimal(total_abonado),
                                                             concepto, rutaComprobantePrincipal, rutaComprobanteOpcional, id_visita, vendedor, cobrador, usuario, comentariocob, cia);
                resultado.Data = resul;
                resultado.MaxJsonLength = int.MaxValue;
                resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return resultado;
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private static string NormalizarConcepto(string concepto)
        {
            if (string.IsNullOrWhiteSpace(concepto)) return null;
            const int maxLen = 240;
            var texto = concepto.Trim();
            if (texto.Length <= maxLen) return texto;
            return texto.Substring(0, maxLen - 3) + "...";
        }

        private string subirComprantes(HttpPostedFileBase httpPostedFileBase, string consecutivo)
        {
            string subruta = "~/Files/Recibos";
            Helper helper = new Helper();
            return helper.SubirAdjuntos(httpPostedFileBase, subruta, consecutivo);
        }

        [HttpPost]
        public JsonResult RegistrarDoc_Auxiliar(string Lineas, string doc_credito, string tipo_credito, int subtipo_credito)
        {
            try
            {
                JArray lineas = JArray.Parse(Lineas);
                foreach (JObject item in lineas)
                {
                    ProcesarDocc_Auxiliar(
                                             doc_credito,
                                             item["DEBITO"].ToString(),
                                             tipo_credito,
                                             item["TIPO_DEBITO"].ToString(),
                                             subtipo_credito,
                                             Convert.ToInt32(item["SUBTIPO_DEBITO"]),
                                             DateTime.Parse(item["FECHA_DEBITO"].ToString()).ToString("yyyy-MM-dd"),
                                             Convert.ToDecimal(item["MONTO_PAGO"])
                                         );

                }
                return Json(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult ObtenerCorreoClientesPorCobrador(string cobrador)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            var json = new JsonResult
            {
                Data = RegistroPagoM.ObtenerEmailsClientesPorCobrador(cobrador, cia),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };

            return json;
        }
        [HttpPost]
        public JsonResult ProcesarDocc_Auxiliar(string doc_credito, string documento_debito, string tipo_credito, string tipo, int subtipo_credito, int subtipo_debito, string fecha, decimal monto_pago)
        {
            InsertarDoccAuxiliar(doc_credito, documento_debito, tipo_credito, tipo, subtipo_credito, subtipo_debito, fecha, monto_pago);
            return Json(1);
        }

        [HttpPost]
        public JsonResult InsertarDoccAuxiliar(string doc_credito, string documento_debito, string tipo_credito, string tipo, int subtipo_credito, int subtipo_debito, string fecha, decimal monto_pago)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Agregar_doc_Auxiliar(doc_credito, documento_debito, tipo_credito, tipo, subtipo_credito, subtipo_debito, fecha, monto_pago, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        public void printReporteComprobante12(string doc_debito)
        {
            try
            {
                bool isValid = true;
                this.HttpContext.Session["ReportName"] = "ReporteComprobantePago.rpt";
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
                    rd.SetDatabaseLogon("SA", "@NcL@@");
                    if (!string.IsNullOrEmpty(doc_debito))
                    {
                        rd.SetParameterValue("@DOC_DEBITO", doc_debito);

                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ReporteComprobantePago");
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

        public JsonResult CargarReporteRdl(string doc_debito)
        {
            var resultado = new JsonResult();
            var respuesta = "";
            var Error = "";
            var ConE = "N";

            try
            {
                string usuario = ConfigurationManager.AppSettings["RepUsuario"];
                string contrasena = ConfigurationManager.AppSettings["RepContrasena"];
                string DataSource = ConfigurationManager.AppSettings["DataSource"];
                string connectionString = $"Data Source={DataSource};Initial Catalog=PS_PORTAL_CLIENTE;User ID={usuario};Password={contrasena}";

                string rutaArchivoExportado = Server.MapPath("~\\RPservices\\ReporteComprobantePago.rdl");
                ReportViewer rv = new ReportViewer
                {
                    LocalReport = { ReportPath = rutaArchivoExportado }
                };

                DataTable datos = RegistroPagoM.ObtenerComprobantePago(
                    doc_debito, Convert.ToString(Session["CIA"]));
                ReportDataSource source = new ReportDataSource("DataSet1", datos);
                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(source);
                string rutaLogo = ConfigurationManager.AppSettings["LogoComprobante." + Convert.ToString(Session["CIA"])];
                string logo = string.IsNullOrWhiteSpace(rutaLogo) ? string.Empty
                    : Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath(rutaLogo)));
                rv.LocalReport.SetParameters(new[] {
                    new ReportParameter("CIA", Convert.ToString(Session["CIA"])),
                    new ReportParameter("Documento", doc_debito),
                    new ReportParameter("LogoEmpresa", logo)
                });
                rv.LocalReport.Refresh();

                string mimeType, encoding, extension;
                string[] streams;
                Warning[] warnings;
                byte[] Bytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streams, out warnings);

                respuesta = Convert.ToBase64String(Bytes);
            }
            catch (Exception ex)
            {
                ConE = "S";
                Error = ex.Message; // Consider logging this
            }

            var resultadoFinal = new
            {
                Datos = respuesta,
                Errrores = Error,
                ConError = ConE,
            };
            resultado = Json(resultadoFinal, JsonRequestBehavior.AllowGet);
            resultado.MaxJsonLength = int.MaxValue;
            return resultado;
        }


        [HttpPost]
        public JsonResult GenerarReporteAplicacionPdf(string doc_debito)
        {

            var json = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            json.MaxJsonLength = int.MaxValue;

            string errores = "";
            string conError = "N";
            string nombrePdf = $"ReciboAplicacion_{doc_debito}.pdf";

            string rptPath = Server.MapPath("~/Rpts/ReporteComprobanteAplicacion.rpt");
            string carpeta = @"C:\inetpub\wwwroot\PortalClientes\DIS86rdp\rpdf";
            string fullPath = Path.Combine(carpeta, nombrePdf);

            try
            {
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                using (ReportDocument rd = new ReportDocument())
                {
                    rd.Load(rptPath);

                    rd.SetDatabaseLogon("SA", "B1@dm1n", "192.168.12.8", "PS_PORTAL_CLIENTE");

                    rd.SetParameterValue("@DOC_DEBITO", doc_debito);

                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, fullPath);
                }
            }
            catch (Exception ex)
            {
                conError = "S";
                errores = ex.Message;
            }

            json.Data = new
            {
                ConError = conError,
                Errores = errores,
                RutaPdf = (conError == "N" ? fullPath : null)
            };
            return json;
        }

        [HttpGet]
        public ActionResult ObtenerComprobanteAplicacion(string documento)
        {
            string cia = Session["CIA"].ToString();

            var tiqueteOriginal = RegistroPagoM.ObtenerTiqueteAplicacionTextoCSharp(documento, cia) ?? string.Empty;
            var tiqueteSanitizado = SanitizarTextoImpresora(tiqueteOriginal);
            var tiqueteBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(tiqueteSanitizado));

            return Json(new { tiqueteBase64 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarComprobantePagoPdf(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
            {
                return new HttpStatusCodeResult(400, "Documento requerido.");
            }

            var pdf = GenerarPdfComprobantePago(documento);
            if (pdf == null)
            {
                return HttpNotFound("No se encontro el comprobante de pago.");
            }

            var nombreArchivo = $"ComprobantePago_{documento.Replace("|", "").Replace("/", "-")}.pdf";

            return File(pdf, "application/pdf", nombreArchivo);
        }

        [HttpGet]
        public ActionResult VerComprobantePagoPdf(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
            {
                return new HttpStatusCodeResult(400, "Documento requerido.");
            }

            try
            {
                var pdf = GenerarPdfComprobantePago(documento);
                if (pdf == null)
                {
                    return HttpNotFound("No se encontro el comprobante de pago.");
                }

                Response.AddHeader("Content-Disposition", "inline; filename=\"ComprobantePago.pdf\"");
                return File(pdf, "application/pdf");
            }
            catch
            {
                return new HttpStatusCodeResult(500, "No se pudo generar el comprobante de pago.");
            }
        }

        [HttpPost]
        public JsonResult CargarComprobantePagoTicketPdf(string documento)
        {
            string base64Pdf = string.Empty;
            string errores = string.Empty;
            string conError = "N";

            try
            {
                var pdf = GenerarPdfComprobantePago(documento);
                if (pdf == null)
                {
                    conError = "S";
                    errores = "No se encontro el comprobante de pago.";
                }
                else
                {
                    base64Pdf = Convert.ToBase64String(pdf);
                }
            }
            catch (Exception ex)
            {
                conError = "S";
                errores = ex.Message;
            }

            var json = Json(new
            {
                Datos = base64Pdf,
                Errrores = errores,
                ConError = conError
            }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        private byte[] GenerarPdfComprobantePago(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
            {
                return null;
            }

            if (Session["CIA"] == null)
            {
                throw new InvalidOperationException("La sesión de compañía expiró.");
            }

            string cia = Session["CIA"].ToString();
            var tiqueteOriginal = RegistroPagoM.ObtenerTiqueteAplicacionTextoCSharp(documento, cia) ?? string.Empty;
            var tiqueteSanitizado = SanitizarTextoImpresora(tiqueteOriginal);

            if (string.IsNullOrWhiteSpace(tiqueteSanitizado))
            {
                return null;
            }

            return TicketPdfHelper.RenderTicketPdf("~/img/Cliente/cliente.png", tiqueteSanitizado, 42);
        }

        private static string SanitizarTextoImpresora(string texto)
        {
            var valor = (texto ?? string.Empty).Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(valor.Length);

            foreach (var ch in valor)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            valor = sb.ToString()
                .Replace("“", "\"")
                .Replace("”", "\"")
                .Replace("‘", "'")
                .Replace("’", "'")
                .Replace("–", "-")
                .Replace("—", "-")
                .Replace('\u00A0', ' ');

            sb.Clear();
            foreach (var ch in valor)
            {
                if (ch == '\t' || ch == '\n' || ch == '\r' || (ch >= ' ' && ch <= '~'))
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString();
        }

        [HttpPost]
        public JsonResult CargarReporteAplicacionRpt(string doc_debito)
        {
            // Preparamos el JsonResult
            var json = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            json.MaxJsonLength = int.MaxValue;

            string base64Pdf = "";
            string errores = "";
            string conError = "N";

            try
            {
                // 1) Ruta física del .rpt
                string rptPath = Server.MapPath("~/Rpts/ReporteComprobanteAplicacion.rpt");

                // 2) Creamos y configuramos el ReportDocument
                using (ReportDocument rd = new ReportDocument())
                {
                    rd.Load(rptPath);

                    string usuario = "SA";
                    string contrasena = "B1@dm1n";
                    string servidor = "192.168.12.8";
                    string baseDatos = "PS_PORTAL_CLIENTE";

                    rd.SetDatabaseLogon(usuario, contrasena, servidor, baseDatos);

                    // 4) Parámetro de tu SP/vista
                    rd.SetParameterValue("@DOC_DEBITO", doc_debito);

                    // 5) Exportamos a PDF a un stream
                    using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
                    {
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        base64Pdf = Convert.ToBase64String(bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                conError = "S";
                errores = ex.Message;
            }

            // 6) Armamos la respuesta JSON
            json.Data = new
            {
                Datos = base64Pdf,
                Errrores = errores,
                ConError = conError
            };

            return json;
        }

        //public JsonResult CargarReporte(string factura)
        //{
        //    var resultado = new JsonResult();
        //    var respuesta = "1";
        //    var Error = "";
        //    var ConE = "N";
        //    try
        //    {
        //        //  bool isValid = true;
        //        string usuario, contrasena, DataSource;
        //        usuario = ConfigurationManager.AppSettings["RepUsuario"];
        //        contrasena = ConfigurationManager.AppSettings["RepContrasena"];
        //        DataSource = ConfigurationManager.AppSettings["DataSource"];
        //        string connectionString = "Data Source=" + DataSource + ";Initial Catalog=PS_TRACKING;User ID=" + usuario + ";Password=" + contrasena;

        //        string rutaArchivoExportado;
        //        string numReporte = "ReciboTicket";
        //        string nombrePDF = numReporte + "_" + factura;
        //        nombrePDF = nombrePDF.Replace(" ", "");
        //        nombrePDF = nombrePDF.Replace(":", "");
        //        nombrePDF = nombrePDF.Replace(".", "");
        //        nombrePDF = nombrePDF + ".pdf";

        //        var ruta_doc = "";
        //        ruta_doc = Server.MapPath("~\\RPservices\\ReporteComprobantePago.rdl");
        //        rutaArchivoExportado = ruta_doc;
        //        ReportViewer rv = new ReportViewer();

        //        rv.LocalReport.ReportPath = rutaArchivoExportado;
        //        rv.LocalReport.ReportEmbeddedResource = ruta_doc;
        //        int Counter = 0;
        //        List<ReportParameter> paramList = new List<ReportParameter>();
        //        ReportParameterInfoCollection paramInfo = rv.LocalReport.GetParameters();
        //        ReportParameter[] Param = new ReportParameter[paramInfo.Count];
        //        Param[Counter++] = new ReportParameter("DOCUMENTO", factura);

        //        rv.LocalReport.ReportEmbeddedResource = ruta_doc;
        //        rv.LocalReport.SetParameters(Param);

        //        SqlConnection connection = new SqlConnection(connectionString);
        //        SqlCommand command = new SqlCommand("SELECT * FROM [ANCLA].[VIEW_REPORTE_COMPROBANTE_PAGO]", connection);
        //        command.Parameters.AddWithValue("@DOCUMENTO", factura);

        //        command.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter adapter = new SqlDataAdapter(command);
        //        DataSet dataSet = new DataSet();
        //        adapter.Fill(dataSet);
        //        ReportDataSource source = new ReportDataSource("DataSet1", dataSet.Tables[0]);
        //        rv.LocalReport.DataSources.Clear();
        //        rv.LocalReport.DataSources.Add(source);
        //        rv.LocalReport.Refresh();
        //        string mimeType;
        //        string encoding;
        //        string extension;
        //        string[] streams;
        //        Warning[] warnings;
        //        byte[] Bytes = rv.LocalReport.Render("PDF", string.Empty, out mimeType, out encoding, out extension, out streams, out warnings);
        //        respuesta = Convert.ToBase64String(Bytes);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        ConE = "S";
        //        Error = ex.Message;
        //    }
        //    var restulado = new
        //    {
        //        Datos = respuesta,
        //        Errrores = Error,
        //        ConError = ConE,
        //    };
        //    resultado = Json(restulado, JsonRequestBehavior.AllowGet);
        //    resultado.MaxJsonLength = int.MaxValue;
        //    return resultado;
        //}




        public void printReporteComprobanteAplicacion(string doc_debito)
        {
            try
            {
                bool isValid = true;
                this.HttpContext.Session["ReportName"] = "ReporteComprobanteAplicacion.rpt";
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
                    rd.SetDatabaseLogon("SA", "123");
                    if (!string.IsNullOrEmpty(doc_debito))
                    {
                        rd.SetParameterValue("@DOC_DEBITO", doc_debito);

                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ReporteComprobanteAplicacion");
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

        [HttpPost]
        public JsonResult EnviarAplicacionNC(string doc_aplicacion,
                                     string cliente,
                                     string correos)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            var resultado = new JsonResult();
            string resul = RegistroPagoM.Enviar_Aplicacion_NC(doc_aplicacion,
                                                               cliente,
                                                               correos,
                                                               cia);

            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        public JsonResult ObtenerPDF(string factura)
        {
            var resultado = RegistroPagoM.ObtenerPDF(factura, Convert.ToString(Session["CIA"]));
            var JsonResult = Json(resultado, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;
        }

        [HttpPost]
        public JsonResult ObtenerCorreo_Cuentas_Pagar(string cliente)
        {
            string cia = Session["CIA"].ToString();
            cliente = string.IsNullOrEmpty(cliente) ? Session["cliente"].ToString() : cliente;

            var resultado = new JsonResult();
            var resul = RegistroPagoM.ObtenerCorreoCliente(cliente, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpPost]
        public JsonResult CargarSubtipo(string tipo_Doc)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Obtener_SubtipoDoc(tipo_Doc, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult obtenerCliente(string cliente)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Obtener_cliente(cliente, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }
        [HttpPost]
        public JsonResult ObtenerCorreoCobrador(string cobrador)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            if (!string.IsNullOrWhiteSpace(cobrador))
            {
                var match = System.Text.RegularExpressions
                             .Regex.Match(cobrador, @"^\s*(\d+)");
                if (match.Success)
                    cobrador = match.Groups[1].Value;
            }

            var correo = RegistroPagoM.Obtener_Email_Cobrador(cobrador, cia);
            return Json(correo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult obtenerConsecutivoCobrador(string cobrador)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Obtener_Consecutivo_Documentos(cobrador, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult Enviar_FE_Cliente(string cliente, string correos, string documento)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            var resultado = new JsonResult();
            var resul = RegistroPagoM.Enviar_Correo_CLiente_FE(cliente, correos, documento, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

    }
}
