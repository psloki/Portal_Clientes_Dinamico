using Plantilla.core.Manager;
using Plantilla.web.Filters;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    [PermisosModulo(Modulo = 6, SubModulo = 11,
                    SesKey = "PERMISO_MOD_COBRADOR",
                    PermisoNecesario = "4",
                    CampoSesionForzar = "Cod_Cobrador")]
    [PermisosModulo(Modulo = 6, SubModulo = 11,
                    SesKey = "PERMISO_MOD_VENDEDOR",
                    PermisoNecesario = "5",
                    CampoSesionForzar = "Vendedor")]
    [PermisosModulo(Modulo = 6, SubModulo = 11,
                    SesKey = "PERMISO_MOD_CLIENTE",
                    PermisoNecesario = "6")]
    public class VisitasController : Controller
    {
        readonly VisitaManager VisitaM = new VisitaManager();
        readonly RegistroPagoManager RegistroPagoM = new RegistroPagoManager();
        // GET: Visitas
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("47"))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.SemanaVisita = VisitaM.ObtenerSemana_Anio(Convert.ToString(Session["CIA"]));
            //ViewBag.VendedorVisita = VisitaM.ObtenerVendedor();
            ViewBag.TipoVisita = VisitaM.ObtenerTipoVisita(Convert.ToString(Session["CIA"]));

            //// view para filtros 
            //ViewBag.ClientesFiltros = LoginManager.Obtener_Lista_Clientes(Session["Vendedor"].ToString());
            //ViewBag.VendedorFiltros = LoginManager.ObtenerVendedor();

            return View();
        }
       

        [PermisosModulo(Modulo = 6, SubModulo = 12, SesKey = "PERMISO_MOD_COBRADOR")]

        public ActionResult Mapa()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("41"))
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpGet]
        public JsonResult Obtener_Visistas(string fecha_ini, string fecha_fin, string semanamulti, string dia_semana, string tipo_visita, string vis_vendedor, string vis_cliente)
        {

            if (semanamulti == "") { semanamulti = "0"; }
            if (vis_vendedor == null || vis_vendedor == "") { vis_vendedor = Session["Vendedor"].ToString(); }
            if (vis_cliente == null || vis_cliente == "") { vis_cliente = Session["cliente"].ToString(); };
            var resultado = new JsonResult();
            var resul = VisitaM.ObtenerVistas(fecha_ini, fecha_fin, semanamulti, dia_semana, tipo_visita, vis_vendedor, vis_cliente, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;

        }

        [HttpPost]
        public JsonResult DatosUbicacionCliente(string vendedor, string fecha,string cliente)
        {
            var resultado = new JsonResult();
            var resul = VisitaM.Obtener_Datos_Ubicacion(vendedor, fecha,cliente, Convert.ToString(Session["CIA"]));
            resultado.Data = resul;//Json(resul, JsonRequestBehavior.AllowGet);
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        public ActionResult Documento()
        {

            return View();
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
        public JsonResult Obtener_TablaDocumentos(string cliente, string fechaInicio, string fechaFin, string tipoDocumento)
        {
            if (string.IsNullOrEmpty(cliente))
            {
                return Json(new { code = -2, message = "Favor seleccione un cliente." }, JsonRequestBehavior.AllowGet);
            }

            string cia = Session["CIA"] as string ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            string filtroTipoDoc = string.IsNullOrEmpty(tipoDocumento) ? "" : tipoDocumento;

            DateTime? fechaInicioDt = null;
            DateTime? fechaFinDt = null;

            if (!string.IsNullOrEmpty(fechaInicio))
            {
                if (DateTime.TryParse(fechaInicio, out DateTime fi))
                    fechaInicioDt = fi;
                else
                    return Json(new { code = -3, message = "Fecha inicio inválida." }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(fechaFin))
            {
                if (DateTime.TryParse(fechaFin, out DateTime ff))
                    fechaFinDt = ff;
                else
                    return Json(new { code = -4, message = "Fecha fin inválida." }, JsonRequestBehavior.AllowGet);
            }

            EstadoCuentaManager EstadoCuentasM = new EstadoCuentaManager();
            var resultado = new JsonResult();
            var resul = EstadoCuentasM.Obtener_Tabla_Documentos(cia,cliente,fechaInicioDt,fechaFinDt,tipoDocumento);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        //public ActionResult ObtenerComprobanteAplicacion(string documento)
        //{
        //    string cia = Session["CIA"] as string ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

        //    var tiqueteOriginal = RegistroPagoM.ObtenerTiqueteAplicacionTexto(documento, cia) ?? string.Empty;
        //    var tiqueteSanitizado = SanitizarTextoImpresora(tiqueteOriginal);
        //    var tiqueteBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(tiqueteSanitizado));

        //    return Json(new { tiqueteBase64 }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ObtenerComprobanteAplicacion(string documento)
        {
            string cia = Session["CIA"] as string ?? throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");

            // Utilizar el mismo generador de tiquete que Registro de Pagos.
            // Este formato se construye en C# con un ancho controlado y evita
            // desplazamientos causados por diferencias de fuente en impresoras térmicas.
            var tiqueteOriginal = RegistroPagoM.ObtenerTiqueteAplicacionTextoCSharp(documento, cia) ?? string.Empty;

            var tiqueteSanitizado = SanitizarTextoImpresora(tiqueteOriginal);
            var tiqueteBase64 = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(tiqueteSanitizado)
            );

            return Json(new { tiqueteBase64 }, JsonRequestBehavior.AllowGet);
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
    }
}


