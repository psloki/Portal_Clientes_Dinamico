using Plantilla.core.Entities;
using Plantilla.core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    public class UsuarioController : Controller
    {
        //readonly UsuarioManager TrackingM = new UsuarioManager();

        // GET: Usaurio
        public ActionResult Index()
        {
            //Si el usuario no tiene permiso para ver el index no puede accerde por link
            if (!Session["PERMISOS_MENU"].ToString().Contains("58"))
            {
                return RedirectToAction("Index", "Login");
            }

            string cia = Session["CIA"].ToString();

            ViewBag.Roles = UsuarioManager.GetRoles(cia);
            ViewBag.Cobrador_Usuario = UsuarioManager.Obtener_Lista_Cobrador(Convert.ToString(Session["CIA"]));
            ViewBag.Vendedor_Usuario = UsuarioManager.Obtener_Lista_Vendedor(Convert.ToString(Session["CIA"]));
            ViewBag.Bodega_Usuario = UsuarioManager.Obtener_Lista_Bodega(Convert.ToString(Session["CIA"]));
            ViewBag.UsuariosInsertar = UsuarioManager.GetUsuariosSelectInsertar(cia);
            ViewBag.Modulos = UsuarioManager.GetModulos(Convert.ToString(Session["CIA"]));
            // ViewBag.Jefes = UsuarioManager.GetEmpleadosSelectJefatura(cia);
            return View();
        }
        // GET: Usaurio
        public ActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerRolesMantenimiento()
        {

            List<VIEW_OBTENER_ROLES_MATENIMIENTO> result = UsuarioManager.ObtenerRoles_Matenimiento();
            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }



        [HttpGet]
        public JsonResult GetUsuarios(int tipo_rol)
        {
            string Cia = Session["CIA"].ToString();
            List<PROC_OBT_USUARIO_MANTENIMIENTOResult> result = UsuarioManager.GetUsuarios(Cia, tipo_rol);
            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }


        [HttpPost]
        public JsonResult ActualizarUsuario(string Usuario, int Rol,string vendedor,string cobrador, string bodega)
        {

            string Cia = Session["Cia"].ToString();
            string result;
            result = UsuarioManager.UpdateUsuario(Usuario, Rol, vendedor, cobrador, bodega, Convert.ToString(Session["CIA"]));

            int.TryParse(result, out int res);
            return Json(res);
        }


        [HttpPost]
        public JsonResult CambiarEstado(string Usuario, string Estado)
        {
            string Cia = Session["Cia"].ToString();
            string result = UsuarioManager.CambiarEstado(Usuario, Estado, Convert.ToString(Session["CIA"]));
            int.TryParse(result, out int res);
            return Json(res);
        }


        public JsonResult ValidaExistenciaUsuarioMantenimiento(string usuario, string correo, string identificacion)
        {

            var result = new JsonResult();
            try
            {
                var resultado = UsuarioManager.ValidaExistenciaUsuario(usuario, correo, identificacion, Convert.ToString(Session["CIA"]));

                if (resultado != null)
                {
                    result.Data = new
                    { message = "Éxito", error = "No", data = resultado };
                }
                else
                {
                    result.Data = new
                    { message = "Error", error = "No se pudo realizar el proceso de validacion", data = resultado };
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Data = new
                { message = "Exito", error = ex.Message, data = "" };
                return result;

            }
        }


        [HttpPost]
        public JsonResult RegistrarUsuarioMantenimiento(string usuario, string pass, string email, string cedula, string nombre,string vendedor,string cobrador, string bodega,int rol)
        {
            var result = new JsonResult();
            string cia = Session["Cia"].ToString();
            string[] formulario = { cedula, nombre, usuario, email, pass,vendedor,cobrador,bodega};
            formulario = formulario.Where(item => item == "").ToArray();
            // Valida si algun campo del formulario viene en blanco
            if (formulario.Length != 0)
            {
                result.Data = new
                { message = "Error", error = "Se deben completar todos los campos.", data = "0" };
                return result;
            }

            try
            {
                vendedor = (vendedor == "Todos" || vendedor == "") ? "0" : vendedor;
                cobrador = (cobrador == "Todos" || cobrador == "") ? "0" : cobrador;
                var resp = UsuarioManager.RegistrarUsuario(usuario, pass, email, cedula, nombre ,vendedor,cobrador,bodega,rol, cia);
                // Si la funcion no se ejecuto correctamente 
                if (resp == null)
                {
                    result.Data = new
                    { message = "Error", error = "No se pudo registrar el usuario", data = resp };
                    return result;
                }
                // Al pasar todas la validaciones
                result.Data = new
                { message = "Exito", error = "No", data = resp };

                return result;
            }
            catch (Exception ex)
            {
                result.Data = new
                { message = "Error", error = ex.Message, data = "" };
                return result;

            }
        }


        [HttpPost]
        public JsonResult GuardarRol(string Rol, string Descripcion, string Comentario)
        {
            string cia = Session["CIA"].ToString();
            //string usuario = Session["Usuario"].ToString();
            int.TryParse(Rol, out int rol);
            string result;
            if (rol == 0)
            {
                result = UsuarioManager.InsertarRol(Descripcion, Comentario, Convert.ToString(Session["Usuario"]), Convert.ToString(Session["CIA"]));
            }
            else
            {
                result = UsuarioManager.EditarRol(Rol, Descripcion, Comentario, Convert.ToString(Session["Usuario"]), Convert.ToString(Session["CIA"]));
            }
            int.TryParse(result, out int res);
            return Json(res);
        }

        [HttpGet]
        public JsonResult GetPermisos()
        {
            string Cia = Session["Cia"].ToString();
            List<PROC_OBT_PERMISOS_V2Result> result = UsuarioManager.GetPermisos(Cia);
            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpPost]
        public JsonResult GuardarPermisos(int rol, int seguimiento, int tracking, int estadistica, int usuario, int configuracion, int pedidos, int portal,int descuento)
        {
            string cia = Session["Cia"].ToString();

            string result = UsuarioManager.EditarPermisos(rol, seguimiento, tracking, estadistica, usuario, configuracion, pedidos, portal, descuento, cia);
            int.TryParse(result, out int res);
            return Json(res);
        }

        [HttpGet]
        public JsonResult Obtener_Info_Usuario(string usuario)
        {
            string cia;
            cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = UsuarioManager.ObtenerInformacion_Usuario(usuario,cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        [HttpGet]
        public JsonResult SubModulos(string modulo)
        {
            string Cia = Session["CIA"].ToString();
            var result = UsuarioManager.GetSubmodulos(int.Parse(modulo), Cia);
            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }


        [HttpGet]
        public JsonResult GetPermisosSubmodulo(string modulo, string rol, string tab)
        {
            string Cia = Session["CIA"].ToString();
            var result = UsuarioManager.GetPermisosSubModulos(int.Parse(modulo), int.Parse(rol), int.Parse(tab), Cia);
            JsonResult json = new JsonResult();
            json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }


        [HttpPost]
        public JsonResult GuardarPermisosV2(FormCollection collection)
        {
            string Cia = Session["CIA"].ToString();
            List<string> llaves = collection.AllKeys.Where(item => item.All(char.IsNumber)).ToList();
            string permisos = String.Join(",", llaves);
            int.TryParse(collection["rol"], out int rol);
            int.TryParse(collection["modulo"], out int modulo);
            int.TryParse(collection["submodulo"], out int submodulo);
            string result = UsuarioManager.EditarPermisosV2(rol, modulo, submodulo, permisos, Cia);
            int.TryParse(result, out int res);
            return Json(res);
        }


    }
}