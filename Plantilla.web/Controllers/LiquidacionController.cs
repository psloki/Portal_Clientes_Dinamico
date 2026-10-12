using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Plantilla.core.DTOs;
using Plantilla.core.Manager;
using Plantilla.core.ViewModels;
using Plantilla.web.Filters;
using Plantilla.web.Util;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Plantilla.core.Manager.LiquidacionManager;
using static Plantilla.web.Util.ComprobantePagoUtil;

namespace Plantilla.web.Controllers
{
    [PermisosModulo(Modulo = 7, SubModulo = 15,
                    SesKey = "PERMISO_MOD_COBRADOR",
                    PermisoNecesario = "4",
                    CampoSesionForzar = "Cod_Cobrador")]
    [PermisosModulo(Modulo = 7, SubModulo = 15,
                    SesKey = "PERMISO_MOD_VENDEDOR",
                    PermisoNecesario = "5",
                    CampoSesionForzar = "Vendedor")]
    [PermisosModulo(Modulo = 7, SubModulo = 15,
                    SesKey = "PERMISO_MOD_CLIENTE",
                    PermisoNecesario = "6")]
    public class LiquidacionController : Controller
    {
        readonly LiquidacionManager LiquidacionM = new LiquidacionManager();
        readonly RegistroPagoManager RegistroPagoM = new RegistroPagoManager();
        // GET: Liquidacion
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("49"))
            {
                return RedirectToAction("Index", "Login");
            }

            string cia = Session["CIA"].ToString();
            ViewBag.TipoDoc = RegistroPagoM.Obtener_SubtipoDoc("REC", cia);
            //ViewBag.Clientes = LiquidacionM.ListaClientes(Convert.ToString(Session["CIA"]));
            //ViewBag.Cobrador = LiquidacionM.ListaCobrador(Convert.ToString(Session["CIA"]));
            ViewBag.Bancos = LiquidacionM.ListaBancos(Convert.ToString(Session["CIA"]));
            //ViewBag.Tipo = LiquidacionM.ListaTipoDoc(Convert.ToString(Session["CIA"]));
            //ViewBag.EstadoLiquidacion = LiquidacionM.ListaEstadoLiq(Convert.ToString(Session["CIA"]));
            ViewBag.ListaSubtiposTODOS = TransferenciasManager.ObtenerSubtiposTODOS_Filtro(cia);
            //ViewBag.ListaCuentas = TransferenciasManager.ObtenerCuentasFiltro(cia);

            return View();
        }
        private string GetUsuarioERP()
        {
            // 1) Intenta con Session
            var u = (Session["Usuario"] as string);

            // 2) Si está vacío o es "web", usa un usuario que sí exista en ERPADMIN.USUARIO
            if (string.IsNullOrWhiteSpace(u) || u.Equals("web", StringComparison.OrdinalIgnoreCase))
                u = System.Configuration.ConfigurationManager.AppSettings["RepUsuario"] ?? "SA";

            return u.Trim();
        }


        [HttpPost]
        public JsonResult obtenerFormaPagoDetalle(string tipo)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = LiquidacionM.Obtener_FormaPago_Detalle(tipo, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        public JsonResult ObtenerMonedaXCuenta(string cuenta)
        {

            string cia = Session["CIA"].ToString();

            var result = LiquidacionManager.ObtenerMonedaXCuenta(cuenta, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ValidarSiExisteNumBancoTEF_DEP(string numero, string banco)
        {
            string cia = Session["CIA"].ToString();

            var result = LiquidacionManager.ValidarSiExisteNumBancoTEF_DEP(numero, banco, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ObtenerDataEstadoCuentaBancario(string cuenta_banco, string numero)
        {

            string cia = Session["CIA"].ToString();

            var result = RegistroPagoManager.ObtenerDataEstadoCuentaBancario(cuenta_banco, numero, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        [HttpGet]
        public JsonResult Obtener_recibos(string fechaInicio, string fechaFin, string cliente, string cobrador,
                                          string moneda, string tipoDoc, int subTipoDoc, int estado)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = LiquidacionM.Obtener_Documentos_Recibos(fechaInicio, fechaFin, cliente, cobrador, moneda, tipoDoc, subTipoDoc, estado, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;

        }

        public JsonResult ObtenerSaldosEfectivo(string fechaInicio, string fechaFin, string cliente, string cobrador,
                                          string moneda, string tipoDoc, int subTipoDoc, int estado)
        {
            string cia = Session["CIA"].ToString();

            var result = LiquidacionManager.Obtener_Documentos_Recibos_FILTRO_EFECTIVO(fechaInicio, fechaFin, cliente, cobrador, moneda, tipoDoc, subTipoDoc, estado, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ObtenerSaldosCheque(string fechaInicio, string fechaFin, string cliente, string cobrador,
                                         string moneda, string tipoDoc, int subTipoDoc, int estado)
        {
            string cia = Session["CIA"].ToString();

            var result = LiquidacionManager.Obtener_Documentos_Recibos_FILTRO_CHEQUE(fechaInicio, fechaFin, cliente, cobrador, moneda, tipoDoc, subTipoDoc, estado, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ObtenerSaldosChequePostfechado(string fechaInicio, string fechaFin, string cliente, string cobrador,
                                         string moneda, string tipoDoc, int subTipoDoc, int estado)
        {
            string cia = Session["CIA"].ToString();

            var result = LiquidacionManager.Obtener_Documentos_Recibos_FILTRO_CHEQUE_POSTFECHADO(fechaInicio, fechaFin, cliente, cobrador, moneda, tipoDoc, subTipoDoc, estado, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        [HttpGet]
        public JsonResult Obtener_debitos_recibos(string documento)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = LiquidacionM.Obtener_debitos_Recibos_SubIndex(documento, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;

        }
        [HttpPost]
        public ActionResult GenerarExcelRecibosSeleccionados(
                 string ids,
                 string fechaIni,
                 string fechaFin,
                 string cliente,
                 string cobrador,
                 string moneda,
                 string tipoDoc,
                 int subTipo,
                 int estado
             )
        {
            var json = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            try
            {
                // 1) Nombre + ruta física
                string nombreReporte = $"LIQUIDACION_SEL_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string fullPath = Path.Combine(Server.MapPath("~/Temp"), nombreReporte);

                // 2) Obtener datos (solo los que coinciden con el SP + filtros, y luego filtramos por IDs)
                string cia = Session["CIA"].ToString();
                var lista = LiquidacionM.GenerarExcel_RecibosPorIds(
                    ids,
                    fechaIni, fechaFin,
                    cliente, cobrador,
                    moneda, tipoDoc,
                    subTipo, estado,
                    cia
                );

                // 3) Crear documento y estilos
                var doc = new SLDocument();

                var estiloEnc = new SLStyle
                {
                    Font = { Bold = true, FontName = "Calibri", FontSize = 12 }
                };
                estiloEnc.Fill.SetPattern(
                    PatternValues.Solid,
                    System.Drawing.Color.LightSteelBlue,
                    System.Drawing.Color.Black
                );
                estiloEnc.Border.TopBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEnc.Border.BottomBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEnc.Border.LeftBorder.BorderStyle = BorderStyleValues.Medium;
                estiloEnc.Border.RightBorder.BorderStyle = BorderStyleValues.Medium;

                var estiloCelda = new SLStyle
                {
                    Font = { FontName = "Calibri", FontSize = 10 }
                };
                estiloCelda.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
                estiloCelda.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
                estiloCelda.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
                estiloCelda.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;

                doc.CreatePatternFill();

                // 4) Encabezados
                string[] headers = {
                  "Cliente","Nombre Cliente","Tipo","Subtipo",
                  "Forma de Pago","Documento","Moneda","Monto",
                  "Estado","Fecha Documento"
                };
                for (int i = 0; i < headers.Length; i++)
                {
                    int col = i + 1;
                    doc.SetCellValue(1, col, headers[i]);
                    doc.SetCellStyle(1, col, estiloEnc);
                    doc.AutoFitColumn(1, col);
                }

                // 5) Datos
                int fila = 2;
                foreach (var r in lista)
                {
                    doc.SetCellValue(fila, 1, r.CLIENTE);
                    doc.SetCellValue(fila, 2, r.NOMBRE_CLIENTE);
                    doc.SetCellValue(fila, 3, r.TIPO);
                    doc.SetCellValue(fila, 4, r.SUBTIPO ?? 0);
                    doc.SetCellValue(fila, 5, r.FORMA_PAGO);
                    doc.SetCellValue(fila, 6, r.DOCUMENTO);
                    doc.SetCellValue(fila, 7, r.MONEDA);

                    // Monto
                    doc.SetCellValue(fila, 8, Convert.ToDouble(r.MONTO));
                    var estiloMonto = estiloCelda.Clone();
                    estiloMonto.FormatCode = "#,##0.00";
                    doc.SetCellStyle(fila, 8, estiloMonto);

                    doc.SetCellValue(fila, 9, r.ESTADO_DESCRIPCION);
                    if (DateTime.TryParse(r.FECHA_DOCUMENTO, out DateTime dt))
                        doc.SetCellValue(fila, 10, dt);
                    else
                        doc.SetCellValue(fila, 10, r.FECHA_DOCUMENTO);

                    // Bordes resto de celdas
                    for (int c = 1; c <= headers.Length; c++)
                        if (c != 8) doc.SetCellStyle(fila, c, estiloCelda);

                    fila++;
                }

                // 6) Guardar y devolver ruta
                doc.SaveAs(fullPath);
                json.Data = $"Temp/{nombreReporte}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return json;
        }

        [HttpGet]
        public ActionResult DescargarArchivo(string ruta)
        {
            try
            {
                string fullPath = Path.Combine(Server.MapPath("~"), ruta);
                string nombreArchivo = Path.GetFileName(fullPath);
                return File(
                    fullPath,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    nombreArchivo
                );
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult GetDocumentosCC(string fechaInicio, string fechaFin, string subtipo, string deposito, string depositado)
        {
            string cia = (Session["CIA"] as string)?.Trim();

            if (string.IsNullOrWhiteSpace(cia))
            {
                return Json(new { error = "No hay valor de CIA en la sesión." }, JsonRequestBehavior.AllowGet);
            }

            string vendedor = (Session["Vendedor"] as string)?.Trim();
            string cobrador = (Session["Cod_Cobrador"] as string)?.Trim();
            string cliente = (Session["cliente"] as string)?.Trim();

            vendedor = vendedor == "VND" ? "0" : vendedor;
            cobrador = cobrador == "CND" ? "0" : cobrador;

            List<PROC_OBT_DOCUMENTOS_INDEX_CCResult> result =
                LiquidacionManager.GetDocumentosCC(fechaInicio, fechaFin, vendedor, cobrador, cliente, subtipo, deposito, depositado, cia);

            JsonResult json = new JsonResult
            {
                Data = result,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return json;
        }

        [HttpGet]
        public JsonResult GetDocumentosCCTotales(string fechaInicio, string fechaFin, string subtipo, string deposito, string depositado)
        {
            string cia = (Session["CIA"] as string)?.Trim();

            if (string.IsNullOrWhiteSpace(cia))
            {
                return Json(new { error = "No hay valor de CIA en la sesión." }, JsonRequestBehavior.AllowGet);
            }

            string vendedor = (Session["Vendedor"] as string)?.Trim();
            string cobrador = (Session["Cod_Cobrador"] as string)?.Trim();
            string cliente = (Session["cliente"] as string)?.Trim();

            vendedor = vendedor == "VND" ? "0" : vendedor;
            cobrador = cobrador == "CND" ? "0" : cobrador;

            var totales = LiquidacionManager.GetDocumentosCCTotal(fechaInicio, fechaFin, vendedor, cobrador, cliente, subtipo, deposito, depositado, cia);

            JsonResult json = new JsonResult
            {
                Data = totales,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return json;
        }

        [HttpGet]
        public JsonResult GetDocumentosCCV2(string fechaInicio, string fechaFin, string cliente, string moneda, string estado = "P")
        {
            cliente = string.IsNullOrEmpty(cliente) || cliente == "0" ? null : cliente;
            moneda = string.IsNullOrEmpty(moneda) || moneda == "0" ? null : moneda;
            estado = string.IsNullOrEmpty(estado) ? "P" : estado;   // default pendientes

            string cia = (Session["CIA"] as string)?.Trim();
            if (string.IsNullOrWhiteSpace(cia))
            {
                return Json(new { error = "No hay valor de CIA en la sesión." }, JsonRequestBehavior.AllowGet);
            }

            List<PROC_OBT_DOCUMENTOS_INDEX_CC_V2_Result> result =
                LiquidacionManager.GetDocumentosCC_V2(fechaInicio, fechaFin, cliente, moneda, estado, cia);

            JsonResult json = new JsonResult
            {
                Data = result,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return json;
        }


        [HttpPost]
        public ActionResult CrearDeposito(CrearDepositoVM vm)
        {
            // Valida los campos requeridos del ViewModel (ej. que Monto no venga vacío)
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return Json(new { ok = false, msg = string.Join(" ", errores) });
            }

            // --- Inicio de la nueva lógica para el Monto ---

            // 1. Convertimos el Monto (que ahora es un string) a decimal de forma segura
            decimal montoConvertido;
            if (!decimal.TryParse(vm.Monto, NumberStyles.Any, CultureInfo.InvariantCulture, out montoConvertido))
            {
                return Json(new { ok = false, msg = "El formato del monto no es válido. Asegúrese de usar '.' como separador decimal." });
            }

            // 2. Validamos que el monto sea positivo
            if (montoConvertido <= 0)
            {
                return Json(new { ok = false, msg = "El monto debe ser mayor a 0." });
            }

            // --- Fin de la nueva lógica para el Monto ---

            var cia = (Session["CIA"] as string)?.Trim();
            if (string.IsNullOrWhiteSpace(cia))
                return Json(new { ok = false, msg = "No hay CIA en la sesión." });

            string cobrador = (Session["Cod_Cobrador"] as string ?? "").Trim();
            if (string.IsNullOrWhiteSpace(cobrador) || cobrador == "0")
                return Json(new { ok = false, msg = "Su sesión no tiene cobrador válido. Vuelva a iniciar sesión." });

            try
            {
                var fechaDoc = DateTime.ParseExact(vm.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var fechaStr = fechaDoc.ToString("dd/MM/yyyy");

                var usuarioErp = GetUsuarioERP();
                var moneda = vm.Moneda.Trim().ToUpperInvariant();

                var guardados = ComprobantePagoUtil.Guardar(Server, vm.ComprobantePago, fechaDoc, vm.Documento, cia)
                                                  .Take(2).ToList();
                var img1 = guardados.ElementAtOrDefault(0)?.VirtualPath;
                var img2 = guardados.ElementAtOrDefault(1)?.VirtualPath;

                var tipoDocCC = (vm.TipoCC ?? "DEP").Trim().ToUpperInvariant();
                var tipoMovBanco = (vm.TipoBanco ?? "T/C").Trim().ToUpperInvariant();

                var tiposQueRestan = new HashSet<string> { "REC", "DEP", "TEF", "N/C", "O/C" };
                if (!tiposQueRestan.Contains(tipoDocCC))
                {
                    return Json(new { ok = false, msg = $"Tipo de documento no permitido para CC: {tipoDocCC}." });
                }

                var beneficiario = string.IsNullOrWhiteSpace(vm.Beneficiario) ? "N/D" : vm.Beneficiario.Trim();
                string vendedor = (Session["Vendedor"] as string ?? "").Trim();
                string comentario = vm.Comentario ?? "";

                var ok = LiquidacionManager.CrearReciboYMovimientoBancario(
     cliente: string.IsNullOrWhiteSpace(vm.Cliente) ? "0" : vm.Cliente,
     documentoCC: vm.Documento,
     tipoDoc: tipoDocCC,
     subtipo: vm.Subtipo,
     cuentaBancaria: vm.CuentaBancaria,
     fecha_ddMMyyyy: fechaStr,
     referencia: vm.Referencia,
     moneda: moneda,
     totalAbonado: montoConvertido,
     concepto: vm.Concepto,
     img1: img1, img2: img2,
     idVisita: 0,
     vendedor: vendedor,
     cobrador: cobrador,
     usuario: usuarioErp,
     comentarioCob: comentario,
     cia: cia,
     mov_TipoDocumento: tipoMovBanco,
     mov_Subtipo: vm.Subtipo,
     mov_Beneficiario: beneficiario,
     mov_TipoCambioOrig: vm.TipoCambioOrig ?? (moneda == "CRC" ? 1m : 1m),
     mov_MontoOriginal: vm.MontoOriginal ?? montoConvertido,
     detalleAplicacionJson: vm.DetalleAplicacionJson   // 🔹 nuevo
 );


                return Json(new
                {
                    ok,
                    msg = ok ? "Depósito creado correctamente." : "No se pudo registrar el depósito."
                });
            }
            catch (Exception ex)
            {
                var msg = (ex.GetBaseException()?.Message ?? ex.Message).Replace("\r", " ").Replace("\n", " ");
                return Json(new { ok = false, msg = $"Error en el servidor: {msg}" });
            }
        }
    }

    }
