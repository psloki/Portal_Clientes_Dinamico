using System;
using System.Web.Mvc;
using System.Web.Routing;
using Plantilla.web.Controllers;
using Plantilla.web.Util;

namespace Plantilla.web.Filter
{
    public class VerifyLicense : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            string action = filterContext.ActionDescriptor.ActionName;
            bool isLogin = filterContext.Controller is LoginController;

            // Index muestra el formulario; su POST valida dentro de la acción.
            // Logout no necesita consultar la base para cerrar la sesión.
            if (isLogin &&
                (action.Equals("Index", StringComparison.OrdinalIgnoreCase) ||
                 action.Equals("Logout", StringComparison.OrdinalIgnoreCase)))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            int status = new LicenseValidation().GetStatus();
            if (status == 0)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var http = filterContext.HttpContext;
            if (http.Session != null)
            {
                http.Session.Clear();
                http.Session.Abandon();
            }

            // fetch no necesariamente incluye X-Requested-With.
            // Las peticiones de escritura siempre reciben un error JSON.
            if (!http.Request.HttpMethod.Equals("GET",
                    StringComparison.OrdinalIgnoreCase) ||
                http.Request.IsAjaxRequest())
            {
                http.Response.StatusCode = 403;
                http.Response.TrySkipIisCustomErrors = true;
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        message = "Error",
                        error = "No hay una licencia válida. Contacte a soporte.",
                        data = status
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                return;
            }

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new
                {
                    controller = "Login",
                    action = "Index",
                    licencia = status
                }));
        }
    }
}