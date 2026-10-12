using OfficeOpenXml;
using Plantilla.core.Entities;
using Plantilla.core.Manager;
using Plantilla.web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    public class TransferenciasController : Controller
    {
        //private readonly string path = ConfigurationManager.AppSettings["RutaSaveTXT"].ToString();
        // GET: Transferencias
        public ActionResult Index()
        {
            string cia = Session["CIA"].ToString();

            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("52"))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.ListaClientes = TransferenciasManager.ObtenerClientesFiltro(cia);
            ViewBag.ListaProveedores = TransferenciasManager.ObtenerProveedoresFiltro(cia);
            ViewBag.ListaCuentas = TransferenciasManager.ObtenerCuentasFiltro(cia);
            ViewBag.ListaDocumentos = TransferenciasManager.ObtenerDocumentosFiltro(cia);
            ViewBag.ListaProveedores = TransferenciasManager.ObtenerProveedoresFiltro(cia);
            ViewBag.ListaSubtiposCC = TransferenciasManager.ObtenerSubtiposCC_Filtro(cia);
            ViewBag.ListaSubtiposCP = TransferenciasManager.ObtenerSubtiposCP_Filtro(cia);
            ViewBag.ListaSubtiposTODOS = TransferenciasManager.ObtenerSubtiposTODOS_Filtro(cia);
            ViewBag.ListaSubtiposControlBancario = TransferenciasManager.ObtenerSubtiposControlBancario_Filtro(cia);
            ViewBag.ListaConciliaciones = TransferenciasManager.ObtenerConciliacionesFiltro(null, cia);

            return View();
        }

        public ActionResult DescargarExcel()
        {
            try
            {
                int row = 1, cell = 1;
                var lista_t = Session["RE_RV2"] as List<PROC_OBT_ESTADO_CUENTA_BANCARIOResult>;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage workbook = new ExcelPackage();
                workbook.Workbook.Worksheets.Add("Hoja1");
                var hojaActual = workbook.Workbook.Worksheets["hoja1"];

                //********************************** CREACION DEL ENCABEZADO ***********************//
                hojaActual.Cells[row, cell].Value = "# BANCO";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "CUENTA ORIGEN";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "# SFTL";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "TIPO";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "SUBTIPO";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "ESTADO";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "# FUSIÓN";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "CC | CP | CB";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "FECHA";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "CONCEPTO";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "DEBITO";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "CREDITO";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "PROVEEDOR";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "CLIENTE";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                cell++;

                hojaActual.Cells[row, cell].Value = "ASIENTO";
                hojaActual.Cells[row, cell].Style.Font.Bold = true;
                hojaActual.Cells[row, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaActual.Cells[row, cell].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                //********************************** CREACION DEL CONTENIDO ***********************//
                bool isGray = false; // Bandera para alternar colores
                foreach (var item in lista_t)
                {
                    row++;
                    cell = 1;

                    // Alternar entre blanco y gris claro
                    if (isGray)
                    {
                        hojaActual.Cells[row, cell, row, cell + 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        hojaActual.Cells[row, cell, row, cell + 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3")); // Gris claro
                    }
                    else
                    {
                        hojaActual.Cells[row, cell, row, cell + 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        hojaActual.Cells[row, cell, row, cell + 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White); // Blanco
                    }

                    hojaActual.Cells[row, cell].Value = item.REFERENCIA; cell++;
                    hojaActual.Cells[row, cell].Value = item.CUENTA_BANCO; cell++;
                    hojaActual.Cells[row, cell].Value = item.DOCUMENTO; cell++;
                    hojaActual.Cells[row, cell].Value = item.TIPO; cell++;
                    hojaActual.Cells[row, cell].Value = item.SUBTIPO; cell++;

                    // Modificación del estado según la lógica solicitada
                    string estadoModificado;
                    switch (item.ESTADO)
                    {
                        case "D":
                            estadoModificado = "NUM DUPLICADO";
                            break;
                        case "N":
                            estadoModificado = "SIN ERRORES";
                            break;
                        case "CC":
                            estadoModificado = "CONS CAMBIADO";
                            break;
                        case "F":
                            estadoModificado = "FUSIONADO";
                            break;
                        default:
                            estadoModificado = item.ESTADO; // Valor normal
                            break;
                    }
                    hojaActual.Cells[row, cell].Value = estadoModificado; cell++;

                    hojaActual.Cells[row, cell].Value = item.DOCUMENTO_FUSION; cell++;
                    hojaActual.Cells[row, cell].Value = item.CC_CP; cell++;
                    hojaActual.Cells[row, cell].Value = item.FECHA_TEF_STR; cell++;
                    hojaActual.Cells[row, cell].Value = item.PS_86_REFERENCIA; cell++;
                    hojaActual.Cells[row, cell].Value = item.DEBITO_STR; cell++;
                    hojaActual.Cells[row, cell].Value = item.CREDITO_STR; cell++;
                    hojaActual.Cells[row, cell].Value = item.PROVEEDOR; cell++;
                    hojaActual.Cells[row, cell].Value = item.CLIENTE; cell++;
                    hojaActual.Cells[row, cell].Value = item.ASIENTO;

                    isGray = !isGray; // Cambiar el estado de la bandera
                }

                var Name = Session["CIA"].ToString() + "_EXTRACTO_BANCARIO_" + DateTime.Now.ToString("yyyy_MM_dd_HHmm") + ".xlsx";
                string handle = Guid.NewGuid().ToString();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }
                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = Name }
                };
            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = new { FileGuid = 0, FileName = "sin nombre", TieneErrores = "1", Prove = ex.Message }
                };
            }
        }

        [HttpGet]
        public virtual ActionResult DEXCELRV(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public JsonResult ValidarFusionDocs(int seleccion, string numero, string cuenta_banco, string fecha)
        {
            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.ValidarFusionDocs(seleccion, numero, cuenta_banco, fecha, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ValidarNuevoConsecutivo(string numero)
        {
            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.ValidarNuevoConsecutivo(numero, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        [HttpGet]
        public JsonResult ObtenerTransferencias(
          string fechaInicio, string fechaFin, string cuenta, string tipo, string subtipo, string documento,
          string proveedor, string cliente, string debitoXcredito, string cxc_cxp, string cargadoBancos
     )
        {
            cuenta = string.IsNullOrEmpty(cuenta) || cuenta == "0" ? null : cuenta;
            tipo = string.IsNullOrEmpty(tipo) || tipo == "0" ? null : tipo;
            subtipo = string.IsNullOrEmpty(subtipo) || subtipo == "00" ? null : subtipo;
            documento = string.IsNullOrEmpty(documento) || documento == "0" ? null : documento;
            proveedor = string.IsNullOrEmpty(proveedor) || proveedor == "0" ? null : proveedor;
            cliente = string.IsNullOrEmpty(cliente) || cliente == "0" ? null : cliente;
            debitoXcredito = string.IsNullOrEmpty(debitoXcredito) || debitoXcredito == "0" ? null : debitoXcredito;
            cxc_cxp = string.IsNullOrEmpty(cxc_cxp) || cxc_cxp == "0" ? null : cxc_cxp;
            cargadoBancos = string.IsNullOrEmpty(cargadoBancos) || cargadoBancos == "0" ? null : cargadoBancos;

            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.ObtenerTransferencias(
                 fechaInicio, fechaFin, cuenta, tipo, subtipo, documento,
                          proveedor, cliente, debitoXcredito, cxc_cxp, cargadoBancos, cia
            );
            Session["RE_RV2"] = result;
            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        public JsonResult ObtenerTipoCambioXFecha(string moneda, string fecha)
        {

            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.ObtenerTipoCambioXFecha(moneda, fecha, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ObtenerConciliacion(string cuenta, string fecha)
        {

            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.ObtenerConciliaciones(cuenta, fecha, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult EditarDocumentoBanco(int idLinea, string doc_viejo, string doc_nuevo)
        {

            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.EditarDocumentoBanco(idLinea, doc_viejo, doc_nuevo, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

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


        public JsonResult EditarNumBanco(string idLinea, string numNuevo)
        {
            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.EditarNumBanco(idLinea, numNuevo, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;
        }

        //public JsonResult GuardarTXT(string cuenta, string usuario, string contrasena, string token)
        //{
        //    // Verificar y crear el directorio si no existe
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    string nombreArchivo = cuenta + "_" + usuario;

        //    var result = 0;

        //    try
        //    {
        //        // Usamos StreamWriter para escribir en el archivo
        //        using (StreamWriter writer = new StreamWriter(path + nombreArchivo + ".txt"))
        //        {
        //            writer.WriteLine(cuenta);
        //            writer.WriteLine(usuario);
        //            writer.WriteLine(contrasena);
        //            writer.WriteLine(token);
        //        }

        //        result = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = 0;
        //    }

        //    var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;
        //}

        public JsonResult InsertarDocumentoCP(string cuenta, string documento, string tipo, int subtipo, string fecha, string concepto,
           string monto, string moneda, string idLinea, string proveedor)
        {
            string cia = Session["CIA"].ToString();

            string usuario = Session["Usuario"].ToString();

            var result = TransferenciasManager.InsertarDocumentoCP(cuenta, usuario, documento, tipo, subtipo, fecha, concepto,
            monto, moneda, idLinea, proveedor, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;
        }

        public JsonResult InsertarMovBancos(string cuenta, string documento, string monto, string moneda, string tipo, int subtipo, string fecha, string concepto, string idLinea)
        {
            string cia = Session["CIA"].ToString();

            string usuario = Session["Usuario"].ToString();

            var result = TransferenciasManager.InsertarMovBancos(cuenta, usuario, documento, monto, moneda, tipo, subtipo, fecha, concepto, idLinea, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;
        }

        public JsonResult InsertarDocumentoMovReportados(string cuenta, string documento, string tipo, string fecha, string concepto,
        string monto, string idLinea)
        {
            string cia = Session["CIA"].ToString();

            string usuario = Session["Usuario"].ToString();

            var result = TransferenciasManager.InsertarDocumentoMovReportados(cuenta, documento, tipo, fecha, concepto, monto, usuario, idLinea, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;
        }

        private class docprove
        {
            string cuentabanco, documento, tipo, subtipo, fecha, concepto, montoC, montoD;

            public string CUENTA_BANCO { get => cuentabanco; set => cuentabanco = value; }
            public string DOCUMENTO { get => documento; set => documento = value; }
            public string TIPO { get => tipo; set => tipo = value; }
            public string SUBTIPO { get => subtipo; set => subtipo = value; }
            public string FECHA { get => fecha; set => fecha = value; }
            public string CONCEPTO { get => concepto; set => concepto = value; }
            public string MONTO_C { get => montoC; set => montoC = value; }
            public string MONTO_D { get => montoD; set => montoD = value; }


        }

        public JsonResult CargarExcel(HttpPostedFileBase[] file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // DECLARACION DE VARIABLES
            string error = "N";
            List<docprove> lista = new List<docprove>();
            MemoryStream memoryStream = new MemoryStream();

            // SI HAY ARCHIVO
            if (file != null && file.Length > 0)
            {
                // PASAMOS EL ARCHIVO A MEMORIA
                foreach (var f in file)
                {
                    byte[] bytes = new byte[f.ContentLength];
                    f.InputStream.Read(bytes, 0, f.ContentLength);
                    memoryStream.Write(bytes, 0, f.ContentLength);
                }
                try
                {
                    // COMENZAMOS A LEER EL ARCHIVO DE MEMORIA
                    using (MemoryStream stream = new MemoryStream(memoryStream.ToArray())) // Create a new memory stream for reading
                    {
                        // LO CONVERTIMOS A UN ARCHIVO DE EXCEL
                        using (ExcelPackage paquete = new ExcelPackage(stream))
                        {
                            // LEEMOS LA PRIMER HOJA
                            ExcelWorksheet hoja = paquete.Workbook.Worksheets[0];
                            docprove linea;
                            int i = 2; // Assuming the data starts from row 2
                                       // COMENZAMOS A LEER LAS LINEAS
                            while (hoja.Cells[i, 1].Value != null)
                            {
                                // INICIALIZAMOS LA LINEA.
                                linea = new docprove();
                                linea.CUENTA_BANCO = hoja.Cells[i, 1].Value.ToString();
                                linea.DOCUMENTO = hoja.Cells[i, 2].Value.ToString();
                                linea.TIPO = hoja.Cells[i, 3].Value.ToString();
                                linea.SUBTIPO = string.IsNullOrEmpty(hoja.Cells[i, 4].Value?.ToString()) ? "0" : hoja.Cells[i, 4].Value.ToString();
                                linea.FECHA = hoja.Cells[i, 5].Value.ToString();
                                linea.CONCEPTO = hoja.Cells[i, 6].Value.ToString();
                                string montoCValue = hoja.Cells[i, 7].Value?.ToString() ?? "";
                                string montoDValue = hoja.Cells[i, 8].Value?.ToString() ?? "";

                                // APPLICAMOS LAS REGLAS DE MONTO_C Y MONTO_D
                                if (!string.IsNullOrEmpty(montoCValue) && string.IsNullOrEmpty(montoDValue))
                                {
                                    linea.MONTO_C = montoCValue;
                                    linea.MONTO_D = "0";
                                }
                                else if (string.IsNullOrEmpty(montoCValue) && !string.IsNullOrEmpty(montoDValue))
                                {
                                    linea.MONTO_C = "0";
                                    linea.MONTO_D = montoDValue;
                                }
                                else
                                {
                                    // Manejo opcional para el caso en que ambos campos tengan valores no vacíos
                                    linea.MONTO_C = montoCValue;
                                    linea.MONTO_D = montoDValue;
                                }

                                // CARGAMOS LA LINEA A LA LISTA
                                lista.Add(linea);
                                i++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = "S";
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                error = "S";
            }

            var response = new
            {
                error = error,
                Lista = lista
            };
            var JsonResult = Json(response, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
        }

        [HttpPost]
        public JsonResult CargarDatosExcel(List<ModelCargaExcel> lista)
        {
            if (lista.Count == 0)
            {
                return Json(2);
            }

            string cia = Session["CIA"].ToString();
            string usuario = Session["Usuario"].ToString();
            string result;

            // Contar la cantidad de ocurrencias de cada DOCUMENTO
            var documentoCounts = lista.GroupBy(x => x.DOCUMENTO)
                                        .ToDictionary(g => g.Key, g => g.Count());

            foreach (var item in lista)
            {
                // Validar y ajustar el tipo de documento basado en DEBITO y CREDITO
                string tipoDocumento;
                decimal montoFinal;

                if (item.MONTO_D > 0 && item.MONTO_C == 0)
                {
                    montoFinal = item.MONTO_D;
                    tipoDocumento = "D";
                }
                else if (item.MONTO_D == 0 && item.MONTO_C > 0)
                {
                    montoFinal = item.MONTO_C;
                    tipoDocumento = "C";
                }
                else
                {
                    montoFinal = item.MONTO_C;
                    tipoDocumento = item.TIPO; // Mantener el valor original si no se cumple ninguna de las condiciones anteriores
                }

                // Convertir montoFinal a string
                string montoFinalString = montoFinal.ToString();

                // Convertir SUBTIPO de string a int
                int subTipoInt;
                if (!int.TryParse(item.SUBTIPO, out subTipoInt))
                {
                    subTipoInt = 0; // Valor predeterminado en caso de error
                }

                // Convertir la fecha de formato d/M/yyyy h:mm:ss tt a yyyy-MM-dd
                string fechaFormatoOriginal = item.FECHA;
                DateTime fecha;
                string fechaFormatoDeseado;

                if (DateTime.TryParse(fechaFormatoOriginal, out fecha))
                {
                    fechaFormatoDeseado = fecha.ToString("yyyy-MM-dd");
                }
                else
                {
                    fechaFormatoDeseado = "0001-01-01"; // Valor predeterminado en caso de error
                }

                // Determinar el valor de INS_MOV_REPORTADOS
                string insMovReportados = documentoCounts[item.DOCUMENTO] > 1 ? "N" : "S";

                result = TransferenciasManager.InsertarDocumentoExcel(
                    item.CUENTA_BANCO,
                    item.DOCUMENTO,
                    item.TIPO,
                    subTipoInt,
                    fechaFormatoDeseado,
                    item.CONCEPTO,
                    montoFinalString,
                    tipoDocumento,
                    usuario,
                    insMovReportados, // Añadir INS_MOV_REPORTADOS
                    cia
                   
                );
            }

            return Json(1);
        }





        public JsonResult InsertarConciliacionBancaria(string cuenta, string conciliacion, string documento, string fecha, string concepto, string monto, string tipo, string idLinea)
        {
            // Extraer el primer valor de la cadena conciliacion
            if (!string.IsNullOrEmpty(conciliacion) && conciliacion.Contains("//"))
            {
                conciliacion = conciliacion.Split(new[] { "//" }, StringSplitOptions.None)[0];
            }
            else
            {
                conciliacion = string.IsNullOrEmpty(conciliacion) || conciliacion == "N" ? null : conciliacion;
            }

            string cia = Session["CIA"].ToString();
            string usuario = Session["Usuario"].ToString();

            var result = TransferenciasManager.InsertarConciliacionBancaria(cuenta, conciliacion, usuario, documento, fecha, concepto,
                monto, tipo, idLinea, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
        }


        public JsonResult ObtenerSubtiposCC_XTipoFiltro(string tipo)
        {
            tipo = string.IsNullOrEmpty(tipo) || tipo == "" ? null : tipo;

            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.ObtenerSubtiposCC_XTipoFiltro(tipo, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ObtenerConciliacionXBanco(string cuenta)
        {
            cuenta = string.IsNullOrEmpty(cuenta) || cuenta == "" ? null : cuenta;

            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.ObtenerConciliacionesFiltro(cuenta, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ObtenerSubtiposCP_XTipoFiltro(string tipo)
        {
            tipo = string.IsNullOrEmpty(tipo) || tipo == "" ? null : tipo;

            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.ObtenerSubtiposCP_XTipoFiltro(tipo, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        public JsonResult ObtenerSubtiposControlBancario_XTipoFiltro(string tipo)
        {
            tipo = string.IsNullOrEmpty(tipo) || tipo == "" ? null : tipo;

            string cia = Session["CIA"].ToString();

            var result = TransferenciasManager.ObtenerSubtiposControlBancario_X_TIPO_Filtro(tipo, cia);

            var JsonResult = Json(result, JsonRequestBehavior.AllowGet); JsonResult.MaxJsonLength = int.MaxValue; return JsonResult;

        }

        [HttpPost]
        public JsonResult ConciliarBatch(
      List<ModelParaBatch> documentosParaBatch, string conciliacion
  )
        {
            if (documentosParaBatch.Count == 0)
            {
                return Json(2);
            }

            conciliacion = string.IsNullOrEmpty(conciliacion) || conciliacion == "N" ? null : conciliacion;
            string cia = Session["CIA"].ToString();
            string usuario = Session["Usuario"].ToString();

            bool allSuccess = true;
            bool allFailure = true;

            foreach (var item in documentosParaBatch)
            {
                var datosList = TransferenciasManager.ObtenerTransferenciasParaAcciones(item.linea, cia);

                foreach (var datos in datosList)
                {
                    // Validar y ajustar el tipo de documento basado en DEBITO y CREDITO
                    string tipoDocumento;
                    if (datos.DEBITO > 0 && datos.CREDITO == 0)
                    {
                        tipoDocumento = "T/D";
                    }
                    else if (datos.DEBITO == 0 && datos.CREDITO > 0)
                    {
                        tipoDocumento = "T/C";
                    }
                    else
                    {
                        tipoDocumento = datos.TIPO; // Mantener el valor original si no se cumple ninguna de las condiciones anteriores
                    }

                    // Seleccionar el valor adecuado para DOCUMENTO
                    string documento = datos.DOCUMENTO ?? datos.REFERENCIA;

                    string result = TransferenciasManager.InsertarConciliacionBancaria(
                        datos.CUENTA_BANCO,
                        conciliacion,
                        usuario,
                        documento,
                        datos.FECHA_TEF_STR,
                        datos.PS_86_REFERENCIA,
                        datos.MONTO,
                        tipoDocumento,
                        datos.ID_LINEA,
                        cia
                    );

                    if (result == "1")
                    {
                        allFailure = false; // Hubo al menos un éxito
                    }
                    else if (result == "2")
                    {
                        allSuccess = false; // Hubo al menos un fracaso
                    }
                }
            }

            // Determinar el resultado final
            if (allSuccess)
            {
                return Json(1);
            }
            else if (allFailure)
            {
                return Json(2);
            }
            else
            {
                return Json(3);
            }
        }

        [HttpPost]
        public JsonResult FusionBatch(
          List<ModelParaBatch> documentosParaBatch, string documento_fusion
          )
        {
            if (documentosParaBatch.Count == 0)
            {
                return Json(2);
            }
            string cia = Session["CIA"].ToString();
            string result;
            string result2;
            foreach (var item in documentosParaBatch)
            {
                result = TransferenciasManager.EditarDocumentoFusion(
                    item.linea,
                    documento_fusion,
                    cia
                );
            }
            result2 = TransferenciasManager.InsertarDocumentoFusion(
                   documento_fusion,
                   cia
               );
            return Json(1);
        }



        [HttpPost]
        public JsonResult ProveedorBatch(
          List<ModelParaBatch> documentosParaBatch, string tipo, int subtipo, string proveedor
          )
        {
            if (documentosParaBatch.Count == 0)
            {
                return Json(2);
            }
            string cia = Session["CIA"].ToString();
            string usuario = Session["Usuario"].ToString();
            string result;
            foreach (var item in documentosParaBatch)
            {
                var datosList = TransferenciasManager.ObtenerTransferenciasParaAcciones(item.linea, cia);

                foreach (var datos in datosList)
                {
                    // Seleccionar el valor adecuado para DOCUMENTO
                    string documento = datos.DOCUMENTO ?? datos.REFERENCIA;

                    result = TransferenciasManager.InsertarDocumentoCP(
                        datos.CUENTA_BANCO,
                        usuario,
                        documento,
                        tipo,
                        subtipo,
                        datos.FECHA_TEF_STR,
                        datos.PS_86_REFERENCIA,
                        datos.MONTO,
                        datos.MONEDA,
                        datos.ID_LINEA,
                        proveedor,
                        cia
                    );
                }
            }
            return Json(1);
        }


        [HttpPost]
        public JsonResult ClienteBatch(
          List<ModelParaBatch> documentosParaBatch, string tipo, int subtipo, string cliente
          )
        {
            if (documentosParaBatch.Count == 0)
            {
                return Json(2);
            }
            string cia = Session["CIA"].ToString();
            string usuario = Session["Usuario"].ToString();
            string result;
            foreach (var item in documentosParaBatch)
            {
                var datosList = TransferenciasManager.ObtenerTransferenciasParaAcciones(item.linea, cia);

                foreach (var datos in datosList)
                {
                    // Seleccionar el valor adecuado para DOCUMENTO
                    string documento = datos.DOCUMENTO ?? datos.REFERENCIA;

                    result = TransferenciasManager.InsertarDocumentoCC(
                        datos.CUENTA_BANCO,
                        usuario,
                        documento,
                        tipo,
                        subtipo,
                        datos.FECHA_TEF_STR,
                        datos.PS_86_REFERENCIA,
                        datos.MONTO,
                        datos.MONEDA,
                        datos.ID_LINEA,
                        cliente,
                        cia
                    );
                }
            }
            return Json(1);
        }

        [HttpPost]
        public JsonResult BancoBatch(
         List<ModelParaBatch> documentosParaBatch
         )
        {
            if (documentosParaBatch.Count == 0)
            {
                return Json(2);
            }
            string cia = Session["CIA"].ToString();
            string usuario = Session["Usuario"].ToString();
            string result;
            foreach (var item in documentosParaBatch)
            {
                var datosList = TransferenciasManager.ObtenerTransferenciasParaAcciones(item.linea, cia);

                foreach (var datos in datosList)
                {
                    // Seleccionar el valor adecuado para DOCUMENTO
                    string documento = datos.DOCUMENTO ?? datos.REFERENCIA;

                    // Intentar convertir datos.SUBTIPO de string a int
                    int subTipoInt;
                    bool conversionExitosa = int.TryParse(datos.SUBTIPO, out subTipoInt);

                    // Si la conversión falla, puedes decidir cómo manejar el error, por ejemplo:
                    if (!conversionExitosa)
                    {
                        // Manejar el caso en que la conversión falla, por ejemplo, asignar un valor predeterminado
                        subTipoInt = 0; // O cualquier valor predeterminado que tenga sentido en tu contexto
                    }

                    result = TransferenciasManager.InsertarMovBancos(
                        datos.CUENTA_BANCO,
                        usuario,
                        documento,
                        datos.MONTO,
                        datos.MONEDA,
                        datos.TIPO,
                        subTipoInt, // Aquí usas la versión convertida a int
                        datos.FECHA_TEF_STR,
                        datos.PS_86_REFERENCIA,
                        datos.ID_LINEA,
                        cia
                    );
                }

            }
            return Json(1);
        }

        [HttpPost]
        public JsonResult LibroBatch(
        List<ModelParaBatch> documentosParaBatch
        )
        {
            if (documentosParaBatch.Count == 0)
            {
                return Json(2);
            }
            string cia = Session["CIA"].ToString();
            string usuario = Session["Usuario"].ToString();
            string result;
            foreach (var item in documentosParaBatch)
            {
                var datosList = TransferenciasManager.ObtenerTransferenciasParaAcciones(item.linea, cia);

                foreach (var datos in datosList)
                {

                    result = TransferenciasManager.InsertarDocumentoMovReportados(
                        datos.CUENTA_BANCO,
                        datos.REFERENCIA,
                        datos.TIPO,
                        datos.FECHA_TEF_STR,
                        datos.PS_86_REFERENCIA,
                        datos.MONTO,
                        usuario,
                        datos.ID_LINEA,
                        cia
                    );
                }

            }
            return Json(1);
        }
    }
}