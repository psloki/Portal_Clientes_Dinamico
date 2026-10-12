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
    [PermisosModulo(Modulo = 8, SubModulo = 20,
                    SesKey = "PERMISO_MOD_COBRADOR",
                    PermisoNecesario = "4",
                    CampoSesionForzar = "Cod_Cobrador")]
    [PermisosModulo(Modulo = 8, SubModulo = 20,
                    SesKey = "PERMISO_MOD_VENDEDOR",
                    PermisoNecesario = "5",
                    CampoSesionForzar = "Vendedor")]
    [PermisosModulo(Modulo = 8, SubModulo = 20,
                    SesKey = "PERMISO_MOD_CLIENTE",
                    PermisoNecesario = "6")]
    public class BonificacionesClienteController : Controller
    {
        readonly BonificacionesClienteManager BoniCliente = new BonificacionesClienteManager();
        // GET: BonificacionesCliente
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("55"))
            {
                return RedirectToAction("Index", "Login");
            }

            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            ViewBag.ClienteBoni = BoniCliente.Obtener_Lista_Clientes_Boni(Convert.ToString(Session["CIA"]));
            ViewBag.ArticuloBoni = BoniCliente.Obtener_Lista_ArticuloBoni(Convert.ToString(Session["CIA"]));
            //ViewBag.NivelPrecioBoni = BoniCliente.Obtener_Lista_NivelPrecioBoni(cia);
            ViewBag.CategoriaCliBoni = BoniCliente.Obtener_Lista_CategoriaCLienteBoni(cia);
            ViewBag.MarcaArtBoni = BoniCliente.Obtener_Lista_Marca(cia);
            ViewBag.CasaArtBoni = BoniCliente.Obtener_Lista_Casa(cia);
            ViewBag.SubCasaArtBoni = BoniCliente.Obtener_Lista_SubCasa(cia);

            return View();
        }

        public JsonResult obtenerArticuloBonificacionesClienteIndex(string cliente, string categoria, string articulo, string casa, string subcasa)
       {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            if (cliente == "" || cliente == null) { cliente = "0"; }
            if (categoria == "" || categoria == null) { categoria = "0"; }
            if (articulo == "" || articulo == null) { articulo = "0"; }
            //if (marca == "") { marca = "0"; }
            if (casa == "") { casa = "0"; }
            if (subcasa == "") { subcasa = "0"; }
            var resultado = new JsonResult();
            var resul = BoniCliente.ObtenerListaBonificacionesCliente(cliente, categoria, articulo, casa, subcasa, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }
        public JsonResult ObtenerArticulos(string term = "")
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            var articulos = BoniCliente.Obtener_Lista_Articulos("0", "0", "0", cia);

            if (!string.IsNullOrWhiteSpace(term))
            {
                term = term.ToLower();
                articulos = articulos
                    .Where(a => a.ARTICULO.ToLower().Contains(term)
                             || a.COD_NOMBRE_ARTICULO.ToLower().Contains(term))
                    .ToList();
            }

            var resultado = articulos
                .Select(a => new {
                    id = a.ARTICULO,
                    text = $"{a.ARTICULO} | {a.COD_NOMBRE_ARTICULO}"
                });

            return Json(new { results = resultado }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EditarBonificacionCliente(string cliente, string articulo,int escala, int cantidadMin, int cantidadMax, 
                                                    DateTime fechaIni, DateTime FechaFin, int factor, string Edi_articuloBoniCli, int UndBonificar)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            var resultado = new JsonResult();
            var resul = BoniCliente.Editar_Bonificacion_Cliente(cliente, articulo, escala, cantidadMin, cantidadMax,fechaIni, FechaFin, factor, Edi_articuloBoniCli, UndBonificar, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }
        [HttpPost]
                public JsonResult EditarBonificacionClienteMasivo(
                string cliente,
                string articulosCsv,
                int escala,
                int cantidadMin,
                int cantidadMax,
                DateTime fechaIni,
                DateTime fechaFin,
                int factor,
                string edi_articuloBoniCli,
                int undBonificar
        )
                {
                    string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
                    var resultado = new JsonResult
                    {
                        MaxJsonLength = int.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };

                    resultado.Data = BoniCliente.Editar_Bonificacion_Cliente_Masivo(
                        cliente,
                        articulosCsv,
                        escala,
                        cantidadMin,
                        cantidadMax,
                        fechaIni,
                        fechaFin,
                        factor,
                        edi_articuloBoniCli,
                        undBonificar,
                        cia
                    );

                    return resultado;
                }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase excelFile)
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
            var resul = ActualizarClienteBonificacion(dataTable);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
            //return Content("Archivo subido y procesado exitosamente.");
        }


        [HttpPost]
        public JsonResult ActualizarClienteBonificacion(DataTable dataTable)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            var resultado = new JsonResult();
            var resul = "";
            foreach (DataRow row in dataTable.Rows)
            {
                // Obtener los valores de la fila
                string cliente = row["CLIENTE"].ToString();
                string articulo = row["ARTICULO"].ToString();
                int escala = Convert.ToInt32(row["ESCALA_BONIF"]);
                int cantidad_min = Convert.ToInt32(row["CANTIDAD_INICIAL"]);
                int cantidad_max = Convert.ToInt32(row["CANTIDAD_FINAL"]);
                DateTime fechaIni = Convert.ToDateTime(row["FECHA_INICIAL"]);
                DateTime fechaFin = Convert.ToDateTime(row["FECHA_FINAL"]);
                int factor_boni = Convert.ToInt32(row["FACTOR_BONIF"]);
                string articuloBoni = row["ARTICULO_BONIF"].ToString();
                int unidadesBoni = Convert.ToInt32(row["UNIDADES_BONIF"]);
                // Llamar al método para editar el artículo de bonificación
                resul = BoniCliente.Actualizar_Bonificacion_Excel_Cliente(cliente, articulo, escala, cantidad_min,cantidad_max, fechaIni, fechaFin, factor_boni,articuloBoni, unidadesBoni, cia);
            }
            // Asignar el resultado a la propiedad Data
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


    }
}