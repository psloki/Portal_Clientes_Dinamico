using ClosedXML.Excel;
using Plantilla.core.Manager;
using Plantilla.web.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    [PermisosModulo(Modulo = 8, SubModulo = 22,
                    SesKey = "PERMISO_MOD_COBRADOR",
                    PermisoNecesario = "4",
                    CampoSesionForzar = "Cod_Cobrador")]
    [PermisosModulo(Modulo = 8, SubModulo = 22,
                    SesKey = "PERMISO_MOD_VENDEDOR",
                    PermisoNecesario = "5",
                    CampoSesionForzar = "Vendedor")]
    [PermisosModulo(Modulo = 8, SubModulo = 22,
                    SesKey = "PERMISO_MOD_CLIENTE",
                    PermisoNecesario = "6")]
    public class DescuentoClienteController : Controller
    {
        readonly DescuentoClienteManager DescCliente = new DescuentoClienteManager();
        // GET: DescuentoCliente
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("57"))
            {
                return RedirectToAction("Index", "Login");
            }


            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            ViewBag.ClienteDesc = DescCliente.Obtener_Lista_Clientes(Convert.ToString(Session["CIA"]));
            ViewBag.ArticuloDesc = DescCliente.Obtener_Lista_ArtciuloDesc(cia);
            ViewBag.MarcaArtDesc = DescCliente.Obtener_Lista_MarcaDesc(cia);
            ViewBag.CasaArtDesc = DescCliente.Obtener_Lista_CasaDesc(cia);
            ViewBag.SubCasaArtDesc = DescCliente.Obtener_Lista_SubCasaDesc(cia);
            ViewBag.CategoriaDesc = DescCliente.Obtener_Lista_CategoriaCLienteDesc(cia);
            return View();
        }

        public JsonResult obtenerDescuentoClienteIndex(string cliente, string categoria, string articulo, string casa, string subcasa)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            if (articulo == "" || articulo == null) { articulo = "0"; }
            //if (marca == "") { marca = "0"; }
            if (casa == "") { casa = "0"; }
            if (subcasa == "") { subcasa = "0"; }
            var resultado = new JsonResult();
            var resul = DescCliente.ObtenerLista_Descuento_Cliente(cliente, categoria, articulo, casa, subcasa, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult EditarDescuentoCliente(string cliente, string articulo, int escala, int cantidadMin, int cantidadMax, DateTime fechaIni, DateTime fechaFin, int porc_descuento)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            var resultado = new JsonResult();
            var resul = DescCliente.Editar_Porcentaje_Descuento_Cliente(cliente, articulo, escala, cantidadMin, cantidadMax, fechaIni, fechaFin, porc_descuento, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }
        [HttpPost]
        public JsonResult EditarDescuentoClienteMasivo(
            string cliente,
            string articulosCsv,
            int escala,
            int cantidadMin,
            int cantidadMax,
            DateTime fechaIni,
            DateTime fechaFin,
            int porc_descuento)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            var resul = DescCliente.Editar_Porcentaje_Descuento_Cliente_Masivo(
                cliente,
                articulosCsv,
                escala,
                cantidadMin,
                cantidadMax,
                fechaIni,
                fechaFin,
                porc_descuento,
                cia
            );

            return Json(resul, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadExcelDesc(HttpPostedFileBase excelFile)
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
            var resul = ActualizarDescuentoCliente(dataTable);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
            //return Content("Archivo subido y procesado exitosamente.");
        }

        [HttpPost]
        public JsonResult ActualizarDescuentoCliente(DataTable dataTable)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            var resultado = new JsonResult();
            var resul = "";
            foreach (DataRow row in dataTable.Rows)
            {
                // Obtener los valores de la fila
                string cliente = row["CLIENTE"].ToString();
                string articulo = row["ARTICULO"].ToString();
                int escala = Convert.ToInt32(row["ESCALA_DCTO"]);
                int cantidad_min = Convert.ToInt32(row["CANTIDAD_INICIAL"]);
                int cantidad_max = Convert.ToInt32(row["CANTIDAD_FINAL"]);
                DateTime fechaIni = Convert.ToDateTime(row["FECHA_INICIAL"]);
                DateTime fechaFin = Convert.ToDateTime(row["FECHA_FINAL"]);
                int porc_descuento = Convert.ToInt32(row["PORC_DCTO"]);

                // Llamar al método para editar el artículo de bonificación
                resul = DescCliente.Actualizar_Descuento_Excel_Cliente(cliente, articulo, escala, cantidad_min,cantidad_max, fechaIni, fechaFin, porc_descuento, cia);
            }
            // Asignar el resultado a la propiedad Data
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }
    }
}