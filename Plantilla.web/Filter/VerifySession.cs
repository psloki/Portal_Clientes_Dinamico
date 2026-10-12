using Plantilla.web.Controllers;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Filter
{
    public class VerifySession : ActionFilterAttribute
    {
        /*Revisa si tiene sesión no deja entrar al controlador de login y sino tiene sesión no solamente puede entrar al controlador de login*/
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string oUser = (string)HttpContext.Current.Session["Usuario"];


            if (filterContext.Controller is LoginController == false &&
                (oUser == null || string.IsNullOrWhiteSpace(HttpContext.Current.Session["CIA"] as string)))
            {
                filterContext.Result = new RedirectResult("~/Login");
                return;
            }
            else if (filterContext.Controller is LoginController && oUser != null)
            {

                int gestionPedidos = (int)HttpContext.Current.Session["GESTION_DE_PEDIDOS"];
                int rolusuario = (int)HttpContext.Current.Session["Rol"];

                if (gestionPedidos != 3 && rolusuario == 2)
                {
                    filterContext.HttpContext.Response.Redirect("~/Home");
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("~/Home");
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }
}