using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Plantilla.core.Manager;
using Plantilla.web.Models;

namespace Plantilla.web.Controllers
{
    public class KardexController : Controller
    {
        readonly DescuentoClienteManager DescCliente = new DescuentoClienteManager();

        public ActionResult Index()
        {
            string Cia = Session["Cia"].ToString();

            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("91"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Bodegas = KardexManager.Obtener_Lista_Bodegas(Cia);
            ViewBag.Articulos = KardexManager.GetArticulosTodos(Cia);
            ViewBag.Tipo = KardexManager.GetTiposInv(Cia);
            ViewBag.Clientes = DescCliente.Obtener_Lista_Clientes(Convert.ToString(Session["CIA"]));

            return View();
        }

        [HttpPost]
        public JsonResult ObtenerIndexKardex(KardexDataTableRequest request)
        {
            string cia = Session["Cia"].ToString();
            request = request ?? new KardexDataTableRequest();

            int draw = request.draw > 0 ? request.draw : 1;
            int start = request.start >= 0 ? request.start : 0;
            int length = request.length > 0 ? request.length : 10;
            string search = request.search != null ? request.search.value : null;
            int orderIndex = request.order != null && request.order.Count > 0 ? request.order[0].column : 3;
            string orderColumn = MapOrderColumn(orderIndex);
            string orderDirection = request.order != null && request.order.Count > 0 ? request.order[0].dir : "desc";

            string tipo = string.IsNullOrEmpty(request.tipo) || request.tipo == "0" ? null : request.tipo;
            string cliente = string.IsNullOrEmpty(request.cliente) || request.cliente == "0" ? null : request.cliente;
            string bodega = string.IsNullOrEmpty(request.bodega) || request.bodega == "0" ? null : request.bodega;
            string articulo = string.IsNullOrEmpty(request.articulo) || request.articulo == "0" ? null : request.articulo;

            KardexManager.KardexPagedResult result = KardexManager.ObtenerIndexKardex(
                request.fechaInicio, request.fechaFin, cliente, tipo, bodega, articulo, cia, start, length, search, orderColumn, orderDirection);

            JsonResult json = new JsonResult();
            json.Data = new
            {
                draw,
                recordsTotal = result.RecordsTotal,
                recordsFiltered = result.RecordsFiltered,
                data = result.Data
            };
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        private static string MapOrderColumn(int columnIndex)
        {
            switch (columnIndex)
            {
                case 1: return "AUDIT_TRANS_INV";
                case 2: return "ARTICULO_COMPLETO";
                case 3: return "FECHA";
                case 4: return "APLICACION";
                case 5: return "TIPO";
                case 6: return "NATURALEZA";
                case 7: return "ASIENTO";
                case 8: return "REFERENCIA";
                case 9: return "CLIENTE_NOMBRE";
                case 10: return "BODEGA";
                case 11: return "CANTIDAD_INT";
                case 12: return "COSTO_TOT_FISC_LOC";
                case 13: return "PRECIO_UNITARIO";
                default: return "FECHA";
            }
        }
    }
}
