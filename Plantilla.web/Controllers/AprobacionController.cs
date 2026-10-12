using Plantilla.core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    public class AprobacionController : Controller
    {
        // GET: Aprobacion
        public ActionResult Index()
        {

            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("50"))
            {
                return RedirectToAction("Index", "Login");
            }

            string cia = (Session["CIA"] as string) ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");


            ViewBag.ListaVendedores = AprobacionManager.ObtenerVendedoresFiltro(cia);
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerCuentasXCobrar(
         string fechaInicio, string fechaFin, string documento, string tipo, string subtipo, string vendedor
    )
        {
            documento = string.IsNullOrEmpty(documento) || documento == "0" ? null : documento;
            tipo = string.IsNullOrEmpty(tipo) || tipo == "0" ? null : tipo;
            subtipo = string.IsNullOrEmpty(subtipo) || subtipo == "00" ? null : subtipo;
            vendedor = string.IsNullOrEmpty(vendedor) || vendedor == "0" ? null : vendedor;

            string cia = Session["CIA"].ToString();

            var result = AprobacionManager.ObtenerCuentasXCobrar(
                 fechaInicio, fechaFin, documento, tipo, subtipo, vendedor, cia
            );

            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    

     // === NUEVO ===
        // Endpoint que tu front llama en cargarTiposDocumento()
        [HttpGet]
        public JsonResult ObtenerTiposDocumento(string tipo = null)
        {
            var cia = (Session["CIA"] as string) ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            var rows = AprobacionManager.ObtenerTiposDocumento(cia, tipo)
                       ?? new System.Collections.Generic.List<Plantilla.core.Entities.PROC_OBT_TIPOS_DOCUMENTOS_CCResult>();

            var payload = rows.Select(t => new
            {
                TIPO = (t.TIPO ?? "").Trim().ToUpper(),
                BADGE_TITLE = string.IsNullOrWhiteSpace(t.BADGE_TITLE) ? (t.TIPO ?? "").Trim() : t.BADGE_TITLE,
                BADGE_CLASS = string.IsNullOrWhiteSpace(t.BADGE_CLASS) ? "badge-light-secondary" : t.BADGE_CLASS
            });

            return Json(payload, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerSubtiposDocumento(string tipo = null, string subtipo = null)
        {
            var cia = (Session["CIA"] as string) ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            var rows = AprobacionManager.ObtenerSubtiposDocumento(cia, tipo, subtipo)
                      ?? new System.Collections.Generic.List<Plantilla.core.Entities.PROC_OBT_SUBTIPOS_DOCUMENTOS_CCResult>();

            var payload = rows.Select(x => new
            {
                subtipo = (x.SUBTIPO ?? "").Trim(),
                descripcion = (x.DESCRIPCION ?? "").Trim(),
                label = string.IsNullOrWhiteSpace(x.LABEL)
                    ? (!string.IsNullOrWhiteSpace(x.DESCRIPCION) ? x.DESCRIPCION.Trim()
                                                                 : (x.SUBTIPO ?? "").Trim())
                    : x.LABEL.Trim()
            });

            return Json(payload, JsonRequestBehavior.AllowGet);
        }

    }
}