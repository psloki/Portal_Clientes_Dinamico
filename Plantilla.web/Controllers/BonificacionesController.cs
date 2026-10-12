using ClosedXML.Excel;
using Plantilla.core.Entities;
using Plantilla.core.Manager;
using Plantilla.web.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{

    [PermisosModulo(Modulo = 8, SubModulo = 19,
                    SesKey = "PERMISO_MOD_COBRADOR",
                    PermisoNecesario = "4",
                    CampoSesionForzar = "Cod_Cobrador")]
    [PermisosModulo(Modulo = 8, SubModulo = 19,
                    SesKey = "PERMISO_MOD_VENDEDOR",
                    PermisoNecesario = "5",
                    CampoSesionForzar = "Vendedor")]
    [PermisosModulo(Modulo = 8, SubModulo = 19,
                    SesKey = "PERMISO_MOD_CLIENTE",
                    PermisoNecesario = "6")]
    public class BonificacionesController : Controller
    {
        readonly BonificacionesManager Bonificacion = new BonificacionesManager();
        // GET: Bonificaciones
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("54"))
            {
                return RedirectToAction("Index", "Login");
            }

            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
           // ViewBag.ClienteBoni = Bonificacion.Obtener_Lista_Clientes_Boni(Convert.ToString(Session["CIA"]));
            //ViewBag.ArticuloBoni = Bonificacion.Obtener_Lista_ArticuloBoni(Convert.ToString(Session["CIA"]));
            ViewBag.NivelPrecioBoni = Bonificacion.Obtener_Lista_NivelPrecioBoni(cia);
            //ViewBag.CategoriaCliBoni = Bonificacion.Obtener_Lista_CategoriaCLienteBoni(cia);
            ViewBag.MarcaArtBoni = Bonificacion.Obtener_Lista_Marca(cia);
            ViewBag.CasaArtBoni = Bonificacion.Obtener_Lista_Casa(cia);
            ViewBag.SubCasaArtBoni = Bonificacion.Obtener_Lista_SubCasa(cia);
            return View();
        }

        public JsonResult obtenerArticuloClienteBonificadosIndex(string articulo,string nivelPrecio, string casa, string subcasa)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            if (nivelPrecio == "" || nivelPrecio == null) { nivelPrecio = "0"; }
            if (articulo == "" || articulo == null) { articulo = "0"; }
            //if (marca == "") { marca = "0"; }
            if (casa == "") { casa = "0"; }
            if (subcasa == "") { subcasa = "0"; }
            var resultado = new JsonResult();
            var resul = Bonificacion.ObtenerListaClienteArticuloBonificados(articulo, nivelPrecio, casa, subcasa, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult EditarArticuloBonificacion(string nivelPrecio, string articulo, string articuloBonif, int cantidaMin, int cantidaMax, int UndBonificar,
                                                     int factor,string fechaIni, string FechaFin, int escala,string moneda,int version_bonif)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente."));
            var resultado = new JsonResult();
            var resul = Bonificacion.Editar_Articulo_Bonificacion(nivelPrecio, articulo, articuloBonif, cantidaMin, cantidaMax, 
                                                                  UndBonificar, factor, fechaIni, FechaFin, escala, moneda, version_bonif,cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }
             
        [HttpPost]
        public JsonResult EditarBonificacionMasiva(string nivelPrecio,
                                           string listaArticulos,  
                                           string articuloBonif,
                                           int cantidaMin, int cantidaMax, int UndBonificar,
                                           int factor, string fechaIni, string fechaFin,
                                           int escala, char moneda, int version_bonif)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            var res = Bonificacion.Editar_Bonificacion_Masiva(
                          nivelPrecio, listaArticulos, articuloBonif,
                          cantidaMin, cantidaMax, UndBonificar, factor,
                          fechaIni, fechaFin, escala, moneda, version_bonif, cia);

            return Json(res, JsonRequestBehavior.AllowGet);
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
            var resul = ActualizarArticuloBonificacion(dataTable);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
            //return Content("Archivo subido y procesado exitosamente.");
        }

        [HttpPost]
        public JsonResult ActualizarArticuloBonificacion(DataTable dataTable)
        {
            string cia = Session["CIA"]?.ToString() ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            const string MONEDA_FIJA = "L";
            const int VERSION_FIJA = 1;
            const int VERSION_BONIF_FIJA = 1;

            string resultadoSP = "0";   

            foreach (DataRow row in dataTable.Rows)
            {
                string nivelPrecio = row["NIVEL_PRECIO"].ToString();
                string articulo = row["ARTICULO"].ToString();
                int escala = Convert.ToInt32(row["ESCALA_BONIF"]);
                string articuloBoni = row["ARTICULO_BONIF"].ToString();

                int cantidadMin = Convert.ToInt32(row["CANTIDAD_MIN"]);
                int cantidadMax = Convert.ToInt32(row["CANTIDAD_MAX"]);
                int unidadesBoni = Convert.ToInt32(row["UNIDADES_BONIF"]);
                int factorBoni = Convert.ToInt32(row["FACTOR_BONIF"]);

                DateTime fechaIni = Convert.ToDateTime(row["FECHA_INICIO"]);
                DateTime fechaFin = Convert.ToDateTime(row["FECHA_FIN"]);

                resultadoSP = Bonificacion.Actualizar_Bonificacion_Excel(
                    nivelPrecio,
                    MONEDA_FIJA,          
                    VERSION_FIJA,        
                    articulo,
                    escala,
                    VERSION_BONIF_FIJA,   
                    articuloBoni,
                    cantidadMin,
                    cantidadMax,
                    unidadesBoni,
                    fechaIni,
                    fechaFin,
                    factorBoni,
                    cia
                );
            }

            return new JsonResult
            {
                Data = resultadoSP,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }




    }


}