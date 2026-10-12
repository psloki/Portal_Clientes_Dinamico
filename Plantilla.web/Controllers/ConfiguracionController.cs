using Plantilla.core.Entities;
using Plantilla.core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    public class ConfiguracionController : Controller
    {
        // GET: Configuracion
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("59"))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Segui_Pedido = ConfiguracionManager.GetSegui_Ped(Convert.ToString(Session["CIA"]));
            return View();
        }



        [HttpGet]
        public JsonResult GetEstadoPed()
        {
            string cia = Session["Cia"].ToString();
            List<PROC_OBT_ESTADO_PEDIDOResult> result = ConfiguracionManager.GetEstado_Pedido(Convert.ToString(Session["CIA"]));
            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }



        [HttpPost]
        public JsonResult ActualizarEstadoPedidos(int codigo, string det_direccion, string color_hexa)
        {
            string cia = Session["Cia"].ToString();
            string result = ConfiguracionManager.Actualiza_estado_ped(codigo, det_direccion, color_hexa, cia);
            int.TryParse(result, out int res);
            return Json(res);
        }



        [HttpPost]
        public JsonResult GuardarEstadoPedidos(string estado, string det_direccion, string color_hexa)
        {
            string cia = Session["Cia"].ToString();
            string result = ConfiguracionManager.Guardar_estado_ped(estado, det_direccion, color_hexa, cia);
            int.TryParse(result, out int res);
            return Json(res);
        }



        [HttpGet]
        public JsonResult GetSeguimientoFac()
        {
            string cia = Session["Cia"].ToString();
            List<PROC_OBT_SEGUI_FACTURAResult> result = ConfiguracionManager.GetSeguimineto_Factura(Convert.ToString(Session["CIA"]));
            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }


        [HttpPost]
        public JsonResult ActualizarSeguimientoFacturas(int codigo, string det_direccion, string color_hexa)
        {
            string cia = Session["Cia"].ToString();
            string result = ConfiguracionManager.Actualiza_seguimiento_fac(codigo, det_direccion, color_hexa, cia);
            int.TryParse(result, out int res);
            return Json(res);
        }


        [HttpPost]
        public JsonResult GuardarSeguimientoFactura(string estado, string det_direccion, string color_hexa)
        {
            string cia = Session["Cia"].ToString();
            string result = ConfiguracionManager.Guardar_seguimiento_fac(estado, det_direccion, color_hexa, cia);
            int.TryParse(result, out int res);
            return Json(res);
        }


        [HttpGet]
        public JsonResult GetSucesos()
        {
            string cia = Session["Cia"].ToString();
            List<PROC_OBT_SUCESOSResult> result = ConfiguracionManager.Get_Sucesos(Convert.ToString(Session["CIA"]));
            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }


        [HttpPost]
        public JsonResult ActualizarSucesos(int codigo, string descripcion)
        {
            string cia = Session["Cia"].ToString();
            string result = ConfiguracionManager.Actualiza_suceso(codigo, descripcion, cia);
            int.TryParse(result, out int res);
            return Json(res);
        }

        [HttpPost]
        public JsonResult GuardarSucesos(string descripcion, string det_descripcion)
        {
            string cia = Session["Cia"].ToString();
            string result = ConfiguracionManager.Guardar_suceso(descripcion, det_descripcion, cia);
            int.TryParse(result, out int res);
            return Json(res);
        }


    }
}