using ClosedXML.Excel;
using Plantilla.core.Manager;
using Plantilla.web.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    [PermisosModulo(Modulo = 8, SubModulo = 18,
                    SesKey = "PERMISO_MOD_COBRADOR",
                    PermisoNecesario = "4",
                    CampoSesionForzar = "Cod_Cobrador")]
    [PermisosModulo(Modulo = 8, SubModulo = 18,
                    SesKey = "PERMISO_MOD_VENDEDOR",
                    PermisoNecesario = "5",
                    CampoSesionForzar = "Vendedor")]
    [PermisosModulo(Modulo = 8, SubModulo = 18,
                    SesKey = "PERMISO_MOD_CLIENTE",
                    PermisoNecesario = "6")]
    public class ArticuloPrecioController : Controller
    {
        readonly ArticuloPrecioManager ArtPrecio = new ArticuloPrecioManager();
        // GET: ArticuloPrecio
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("53"))
            {
                return RedirectToAction("Index", "Login");
            }

            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            ViewBag.MarcaArtPrecio = ArtPrecio.Obtener_Lista_Marca(cia);
            ViewBag.CasaArtPrecio = ArtPrecio.Obtener_Lista_Casa(cia);
            ViewBag.SubCasaArtPrecio = ArtPrecio.Obtener_Lista_SubCasa(cia);
            ViewBag.ClienteArtPrecio = ArtPrecio.Obtener_Lista_Clientes(Convert.ToString(Session["CIA"]));
            ViewBag.NivelPrecioAP = ArtPrecio.Obtener_Lista_nivelPrecio(cia);
            //ViewBag.ArticuloAP = ArtPrecio.Obtener_Lista_ArtciuloNP(cia);
            ViewBag.CategoriaAP = ArtPrecio.Obtener_Lista_CategoriaCLienteNP(cia);
            return View();
        }


        //public JsonResult obtener_Cliente()
        //{
        //    string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : "COONATRAMA");
        //    var resultado = new JsonResult();
        //    var resul = ArtPrecio.Obtener_Lista_Clientes(Convert.ToString(Session["CIA"]));
        //    resultado.Data = resul;
        //    resultado.MaxJsonLength = int.MaxValue;
        //    resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return resultado;
        //}
        //public JsonResult obtener_nivelPrecio(string cliente)
        //{
        //    string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : "COONATRAMA");
        //    var resultado = new JsonResult();
        //    var resul = ArtPrecio.Obtener_Lista_nivelPrecio(cliente, cia);
        //    resultado.Data = resul;
        //    resultado.MaxJsonLength = int.MaxValue;
        //    resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return resultado;
        //}

        //public JsonResult obtener_Articulo_nivelPrecio(string nivel_precio)
        //{
        //    string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : "COONATRAMA");
        //    var resultado = new JsonResult();
        //    var resul = ArtPrecio.Obtener_Lista_ArtciuloNP(nivel_precio, cia);
        //    resultado.Data = resul;
        //    resultado.MaxJsonLength = int.MaxValue;
        //    resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return resultado;
        //}

        //public JsonResult obtener_categoriaCliente_nivelPrecio(string cliente)
        //{
        //    string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : "COONATRAMA");
        //    var resultado = new JsonResult();
        //    var resul = ArtPrecio.Obtener_Lista_CategoriaCLienteNP(cliente, cia);
        //    resultado.Data = resul;
        //    resultado.MaxJsonLength = int.MaxValue;
        //    resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return resultado;
        //}
        public JsonResult ObtenerArticulosPorNivel(string nivelPrecio)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            var articulos = ArtPrecio.Obtener_Lista_ArtciuloNP(cia, nivelPrecio);
            return Json(articulos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult obtenerClienteArticuloPrecioIndex(string nivelPrecioAP, string articuloAP, string casaAP, string subcasaAp)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            
            if (nivelPrecioAP == "" || nivelPrecioAP == null) { nivelPrecioAP = "0"; }
            if (articuloAP == "" || articuloAP == null) { articuloAP = "0"; }
            //if (marcaAP == "") { marcaAP = "0"; }
            if (casaAP == "") { casaAP = "0"; }
            if (subcasaAp == "") { subcasaAp = "0"; }
            var resultado = new JsonResult();
            var resul = ArtPrecio.ObtenerListaClienteArticuloNivelPrecio(nivelPrecioAP, articuloAP, casaAP, subcasaAp, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }
    
        [HttpPost]
        public JsonResult EditarArticuloPrecio(string articulo, string precio, string nivelPrecio)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            if (!decimal.TryParse(precio, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precioDec))
            {
                return Json("0", JsonRequestBehavior.AllowGet); 
            }

            string resul = ArtPrecio.Editar_PrecioArticulo(articulo, precioDec, nivelPrecio, cia);
            return Json(resul, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditarPrecioMasivo(string[] articulos,
                                     string nuevoPrecio,
                                     string nivelPrecio)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            if (articulos == null || articulos.Length == 0)
                return Json("0", JsonRequestBehavior.AllowGet);

            if (!decimal.TryParse(nuevoPrecio,
                                  NumberStyles.Any,
                                  CultureInfo.InvariantCulture,
                                  out decimal precioDec))
                return Json("0", JsonRequestBehavior.AllowGet);

            string csv = string.Join(",", articulos);

            string res = ArtPrecio.Editar_PrecioMasivo(csv,
                                                       precioDec,
                                                       nivelPrecio,
                                                       cia);
            System.Diagnostics.Debug.WriteLine(
        $"⇢ SP result = {res} | csv = {csv} | precio = {precioDec}");
                return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadExcelAP(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se ha subido ningún archivo.");

            var dataTable = new DataTable();
            using (var stream = new MemoryStream())
            {
                excelFile.InputStream.CopyTo(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed();

                    foreach (var row in rows)
                    {
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (var cell in row.Cells())
                            {
                                dataTable.Columns.Add(cell.Value.ToString());
                            }
                        }
                        else
                        {
                            dataTable.Rows.Add();
                            int i = 0;
                            foreach (var cell in row.Cells())
                            {
                                dataTable.Rows[dataTable.Rows.Count - 1][i++] = cell.Value;
                            }
                        }
                    }
                }
            }
            var resultado = new JsonResult();
            var resul = ActualizarArticuloPrecio(dataTable);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
            //return Content("Archivo subido y procesado exitosamente.");
        }


        [HttpPost]
        public JsonResult ActualizarArticuloPrecio(DataTable dataTable)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            var resultado = new JsonResult();
            var resul = "";
            foreach (DataRow row in dataTable.Rows)
            {
                // Obtener los valores de la fila
                string nivel_precio = row["NIVEL_PRECIO"].ToString();
                string moneda = row["MONEDA"].ToString();
                //int version = Convert.ToInt32(row["VERSION"]);
                string articulo = row["ARTICULO"].ToString();
                int version_articulo = Convert.ToInt32(row["VERSION_ARTICULO"]);
                decimal precio = Convert.ToDecimal(row["PRECIO"]);
                string esquema_trabajo = row["ESQUEMA_TRABAJO"].ToString();
                decimal margen_utilidad = Convert.ToDecimal(row["MARGEN_UTILIDAD"]);
                //DateTime fechaIni = Convert.ToDateTime(row["FECHA_INICIO"]);
                //DateTime fechaFin = Convert.ToDateTime(row["FECHA_FIN"]);
                
                // Llamar al método para editar el artículo de bonificación
                resul = ArtPrecio.Actualizar_ArticuloPrecio_Excel(nivel_precio, moneda, 1, articulo, version_articulo, precio, esquema_trabajo,
                                                                  margen_utilidad, DateTime.Now, DateTime.Now, cia);
            }
            // Asignar el resultado a la propiedad Data
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult CopiarNivelPrecio(string nivelPrecioOrigen, string nivelPrecioDestino, int porcentaje)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            string respuesta = ArtPrecio.Copiar_NivelPrecio(cia, nivelPrecioOrigen, nivelPrecioDestino, porcentaje);

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}