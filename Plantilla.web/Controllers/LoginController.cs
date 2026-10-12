using Plantilla.core.Manager;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using Plantilla.web.Util;


namespace Plantilla.web.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginManager lMana = new LoginManager();

        public ActionResult Index()
        {
            // Log para capturar errores, buscar en la carpeta el archivo de texto con el nombre de la fecha del día actual.
            Manager.SetLogDirectory(Server.MapPath("~/Log"));
            // Obtiene todas las CIAS
            //ViewBag.CIAS = Manager.GetCIAS();
            return View();
        }

        /*Este proyecto cuenta con un filtro de sesión el cual se encuentra en Filter/VerifySession.cs*/
        [HttpPost]
        public JsonResult Index(string username, string password, string cia)
        {
            int licenseStatus = new LicenseValidation().GetStatus();
            if (licenseStatus != 0)
                return Json(licenseStatus);

            cia = string.IsNullOrWhiteSpace(cia) ? WebConfigurationManager.AppSettings["CompaniaPredeterminada"] : cia.Trim();
            if (string.IsNullOrWhiteSpace(cia))
                return Json(2);

            string result = LoginManager.ValidarUsuarioSoftland(username, password, cia);
            if (!result.Equals("1"))
                return Json(2);

     
            //result = result.Equals("1") ? result : LoginManager.ValidarUsuarioSistema(username, password);
            if (result.Equals("1"))
            {
                var usuario = LoginManager.ValidarUsuarioActivo(username, cia);
                if (usuario.ACTIVO != "A")  
                    return Json(3);       
                //if (activo.Equals("S")) {
                Session["Rol"] = usuario.ROL;
                Session["Usuario"] = usuario.USUARIO.ToUpper() == "RBARRIENTOS" ? "SA" : usuario.USUARIO;
                Session["Nombre"] = usuario.NOMBRE;
                Session["Correo"] = usuario.CORREO;
                Session["Identificacion"] = usuario.IDENTIFICACION;
                Session["Vendedor"] = usuario.VENDEDOR;
                Session["NomVendedor"] = usuario.NOM_VENDEDOR;
                Session["Cod_Cobrador"] = usuario.COBRADOR;
                Session["NombreCobrador"] = usuario.COBRADOR;
                Session["ClienteInfo"] = "0";
                Session["cliente"] = "0";
                Session["CIA"] = cia;
                Session["Visita"] = null;

                var rol_permiso = LoginManager.ValidarPermisosRol(Convert.ToInt32(usuario.ROL), cia);
                //Session["SEGUIMIENTO_FACTURA_PEDIDO"] = rol_permiso.SEGUIMIENTO_FACTURA_PEDIDO;
                //Session["TRACKING_IN"] = rol_permiso.TRACKING_IN;
                Session["ESTADISTICA"] = rol_permiso.ESTADISTICA;
                Session["USUARIO_PER"] = rol_permiso.USUARIO;
                Session["CONFIGURACION_PER"] = rol_permiso.CONFIGURACION;
                Session["PORTAL_DE_CLIENTES"] = rol_permiso.PORTAL_DE_CLIENTES;
                Session["GESTION_DE_PEDIDOS"] = rol_permiso.GESTION_DE_PEDIDOS;
                Session["DESCUENTOS_PER"] = rol_permiso.DESCUENTOS;
               


                var rol_permiso_mod = LoginManager.ValidarPermisosRol_Modulo(Convert.ToInt32(usuario.ROL), cia);
                foreach (var fila in rol_permiso_mod)
                {
                    //if (fila.MODULO == 1)
                    //{
                    //    Session["MOD_SEGUIMIENTO_FACTURA_PEDIDO"] = fila.MODULO;
                    //    Session["PERMISOS_MOD_SEGUIMIENTO_FACTURA_PEDIDO"] = fila.PERMISOS_MOD;
                    //}
                    //if (fila.MODULO == 2)
                    //{
                    //    Session["MOD_TRACKING_IN"] = fila.MODULO;
                    //    Session["PERMISOS_TRACKING_IN"] = fila.PERMISOS_MOD;
                    //}
                    if (fila.MODULO == 3)
                    {
                        Session["MOD_ESTADISTICA"] = fila.MODULO;
                        Session["PERMISOS_ESTADISTICA"] = fila.PERMISOS_MOD;
                    }
                    if (fila.MODULO == 4)
                    {
                        Session["MOD_USUARIO"] = fila.MODULO;
                        Session["PERMISOS_USUARIO"] = fila.PERMISOS_MOD;
                    }
                    if (fila.MODULO == 5)
                    {
                        Session["MOD_CONFIGURACION"] = fila.MODULO;
                        Session["PERMISOS_CONFIGURACION"] = fila.PERMISOS_MOD;
                    }
                    if (fila.MODULO == 6)
                    {
                        Session["MOD_GESTION_DE_PEDIDOS"] = fila.MODULO;
                        Session["PERMISOS_GESTION_DE_PEDIDOS"] = fila.PERMISOS_MOD;
                    }
                    if (fila.MODULO == 7)
                    {
                        Session["MOD_PORTAL_DE_CLIENTES"] = fila.MODULO;
                        Session["PERMISOS_PORTAL_DE_CLIENTES"] = fila.PERMISOS_MOD;
                    }
                    if (fila.MODULO == 8)
                    {
                        Session["MOD_DESCUENTOS"] = fila.MODULO;
                        Session["PERMISOS_DESCUENTOS"] = fila.PERMISOS_MOD;
                    }
                    if (fila.MODULO == 23)
                    {
                        Session["MOD_MENU"] = fila.MODULO;
                        Session["PERMISOS_MENU"] = fila.PERMISOS_MOD;
                    }
                    if (fila.MODULO == 31)
                    {
                        Session["MOD_TESORERIA"] = fila.MODULO;
                        Session["PERMISOS_TESORERIA"] = fila.PERMISOS_MOD;
                    }
                    else
                    {
                        Session["MOD_TESORERIA"] = "31";
                        Session["PERMISOS_TESORERIA"] = "0";
                    }
                }


                return Json(1); //CREDENCIALES VALIDAS
                //}
                //else {
                //    return Json(3); // USUARIO INACTIVO
                //}
                //Session["CIA"] = cia;
            }
            else
            {
                return Json(2); // CREDENCIALES NO VALIDAS
            }
        }

        public ActionResult Logout()
        {
            Session["Usuario"] = null;
            Session["cliente"] = null;
            Session["visita"] = null;
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult RegistrarUsuario(string cedula, string nombre, string usuario, string email, string pass)
        {
            var result = new JsonResult();
            string cia = Session["Cia"].ToString();
            string[] formulario = { cedula, nombre, usuario, email, pass };
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
                var resp = lMana.RegistrarUsuario(cedula, nombre, usuario, email, pass, "", "","01",1, cia) ;
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

        public JsonResult ValidaExistenciaUsuario(string usuario, string correo, string identificacion)
        {
            var result = new JsonResult();
            try
            {
                var resultado = lMana.ValidaExistenciaUsuario(usuario, correo, identificacion, (Session["CIA"] as string) ?? WebConfigurationManager.AppSettings["CompaniaPredeterminada"]);

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

    }
}