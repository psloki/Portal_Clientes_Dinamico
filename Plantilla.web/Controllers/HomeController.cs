using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Plantilla.core.Entities;
using Plantilla.core.Manager;

namespace Taller.web.Controllers
{
    public class HomeController : Controller
    {
        readonly HomeManager HomeM = new HomeManager();
        public ActionResult Index()
        {
            if (Session["PERMISOS_MENU"].ToString().Contains("45"))
            {
                return Redirect("~/EstadosCuenta");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("47"))
            {
                return Redirect("~/Visitas");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("41"))
            {
                return Redirect("~/Visitas/Mapa");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("48"))
            {
                return Redirect("~/RegistroPagos");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("49"))
            {
                return Redirect("~/Liquidacion");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("50"))
            {
                return Redirect("~/Aprobacion");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("52"))
            {
                return Redirect("~/Transferencias");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("53"))
            {
                return Redirect("~/ArticuloPrecio");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("54"))
            {
                return Redirect("~/Bonificaciones");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("55"))
            {
                return Redirect("~/BonificacionesCliente");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("56"))
            {
                return Redirect("~/Descuentos");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("57"))
            {
                return Redirect("~/DescuentoCliente");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("58"))
            {
                return Redirect("~/Usuario");
            }
            else if (Session["PERMISOS_MENU"].ToString().Contains("59"))
            {
                return Redirect("~/Configuracion");
            }
            else
            {
                Session["Usuario"] = null;
                return Redirect("~/Login");
            }
        }

        

        public JsonResult GetCias()
        {
            //List<PROC_OBT_CIASResult> result = Manager.GetCIAS();
            JsonResult json = new JsonResult();
            // json.Data = result;
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        public JsonResult CambiarCIA(string cia, string vendedor, string nomVendedor, string clienteinfo)
        {
            JsonResult json = new JsonResult();
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (string.IsNullOrEmpty(cia) || string.IsNullOrEmpty(vendedor))
            {
                json.Data = "0";
            }
            else
            {
                Session["Vendedor"] = vendedor;
                Session["NomVendedor"] = nomVendedor;
                Session["ClienteInfo"] = clienteinfo;
                Session["CIA"] = cia;
                json.Data = "1";
            }
            return json;
        }


        public JsonResult CambiarVenedorSession(string vendedor, string nombrevendedor)
        {
            JsonResult json = new JsonResult();
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (string.IsNullOrEmpty(vendedor) || string.IsNullOrEmpty(vendedor))
            {
                json.Data = "0";
            }
            else
            {
                Session["Vendedor"] = vendedor;
                Session["NomVendedor"] = nombrevendedor;
                json.Data = "1";
            }
            return json;
        }

        public JsonResult CambiarCobradorSession(string cobrador, string nombrecobrador)
        {
            JsonResult json = new JsonResult();
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (string.IsNullOrEmpty(cobrador) || string.IsNullOrEmpty(nombrecobrador))
            {
                json.Data = "0";
            }
            else
            {
                Session["Cod_Cobrador"] = cobrador;
                Session["NombreCobrador"] = nombrecobrador;
                json.Data = "1";
            }
            return json;
        }






        public JsonResult CambiarCliente(string clienteinfo)
        {
            bool esAdministrador =
                Session["Rol"] != null &&
                Session["Rol"].ToString() == "2";

            string vendedor = esAdministrador
                ? "0"
                : Session["Vendedor"] != null
                    ? Session["Vendedor"].ToString()
                    : "0";

            string cobrador = esAdministrador
                ? "0"
                : Session["Cod_Cobrador"] != null
                    ? Session["Cod_Cobrador"].ToString()
                    : "0";

            string cia = Session["CIA"].ToString();

            if (string.IsNullOrEmpty(clienteinfo))
            {
                return Json(
                    new { status = "0", vendedor, cobrador },
                    JsonRequestBehavior.AllowGet
                );
            }

            Session["cliente"] = clienteinfo;

            if (!string.IsNullOrEmpty(clienteinfo) && clienteinfo != "0")
            {
                var cliente = HomeManager.GetClienteById(clienteinfo, cia);

                // Solo modificar cobrador/vendedor si NO es administrador
                if (!esAdministrador)
                {
                    if (Session["PERMISO_MOD_COBRADOR"] != null &&
                        Array.IndexOf(
                            Session["PERMISO_MOD_COBRADOR"].ToString().Split(','),
                            "4"
                        ) != -1)
                    {
                        if (Session["Cod_Cobrador"] == null ||
                            Session["Cod_Cobrador"].ToString() != cliente.COBRADOR)
                        {
                            cobrador = cliente.COBRADOR;
                            Session["Cod_Cobrador"] = cobrador;
                            Session["NombreCobrador"] = cliente.NOMBRE_COBRADOR;
                        }
                    }

                    if (Session["PERMISO_MOD_VENDEDOR"] != null &&
                        Array.IndexOf(
                            Session["PERMISO_MOD_VENDEDOR"].ToString().Split(','),
                            "5"
                        ) != -1)
                    {
                        if (Session["Vendedor"] == null ||
                            Session["Vendedor"].ToString() != cliente.VENDEDOR)
                        {
                            vendedor = cliente.VENDEDOR;
                            Session["Vendedor"] = vendedor;
                            Session["NomVendedor"] = cliente.NOMBRE_VENDEDOR;
                        }
                    }
                }
            }

            // Administrador: traer TODOS los clientes
            if (esAdministrador)
            {
                var resul = HomeManager.Obtener_Clientes_Cobrador_Vendedor(
                    "0",
                    "0",
                    cia
                );

                return Json(
                    new
                    {
                        status = "1",
                        vendedor = "0",
                        cobrador = "0",
                        clientes = resul
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            // Usuario normal: mantener comportamiento actual
            if (cobrador != "0" && vendedor != "0")
            {
                var resul = HomeManager.Obtener_Clientes_Cobrador_Vendedor(
                    cobrador,
                    vendedor,
                    cia
                );

                return Json(
                    new
                    {
                        status = "1",
                        vendedor,
                        cobrador,
                        clientes = resul
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            return Json(
                new { status = "1", vendedor, cobrador },
                JsonRequestBehavior.AllowGet
            );
        }


        public JsonResult CambiarVisita(string idvisita)
        {
            JsonResult json = new JsonResult();
            json.MaxJsonLength = int.MaxValue;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (string.IsNullOrEmpty(idvisita) || idvisita=="null")
            {
                Session["Visita"] = null;
                json.Data = "0";
            }
            else
            {
                Session["Visita"] = idvisita;
                json.Data = "1";
            }
            return json;
        }

        public JsonResult MostrarVendedor()
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = HomeManager.ObtenerVendedor(cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        public JsonResult MostrarCobrador()
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = HomeManager.Obtener_Comprador(cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }



        //[HttpPost]
        //public JsonResult MostrarClienteCobrador_Vendedor(string cobrador,string vendedor)
        //{
        //    string cia = Session["CIA"].ToString();
        //    var resultado = new JsonResult();
        //    var resul = HomeManager.Obtener_Clientes_Cobrador_Vendedor(cobrador, vendedor, cia);
        //    resultado.Data = resul;
        //    resultado.MaxJsonLength = int.MaxValue;
        //    resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return resultado;
        //}
        [HttpPost]
        public JsonResult MostrarClienteCobrador_Vendedor(string cobrador, string vendedor)
        {
            string cia = Session["CIA"].ToString();

            // Si es administrador, no aplicar filtros
            if (Session["Rol"] != null && Session["Rol"].ToString() == "2")
            {
                vendedor = "0";
                cobrador = "0";
            }
            else
            {
                // Si no viene vendedor desde pantalla,
                // utilizar el vendedor que ya tiene el usuario en sesión
                if (string.IsNullOrEmpty(vendedor) ||
                    vendedor == "0" ||
                    vendedor == "VND")
                {
                    if (Session["Vendedor"] != null)
                    {
                        vendedor = Session["Vendedor"].ToString();
                    }
                }
            }

            var resultado = new JsonResult();

            var resul = HomeManager.Obtener_Clientes_Cobrador_Vendedor(
                cobrador,
                vendedor,
                cia
            );

            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return resultado;
        }


        [HttpPost]
        public JsonResult MostrarClienteVendedor(string vendedor)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = HomeManager.Obtener_Lista_Clientes(vendedor,cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult MostrarCliente_Cobrador(string cobrador)
        {
            string cia = Session["CIA"].ToString();
            var resultado = new JsonResult();
            var resul = HomeManager.Obtener_Lista_Clientes_Cobrador(cobrador, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }



    }
}