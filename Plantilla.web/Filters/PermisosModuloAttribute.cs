using System;
using System.Linq;
using System.Web.Mvc;
using Plantilla.core.Manager;

namespace Plantilla.web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class PermisosModuloAttribute : ActionFilterAttribute
    {
        public int Modulo { get; set; }
        public int SubModulo { get; set; }
        public string SesKey { get; set; }          
        public string PermisoNecesario { get; set; } = "4"; 
        public string CampoSesionForzar { get; set; } = null;  

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            var ses = ctx.HttpContext.Session;

            int rol = Convert.ToInt32(ses["Rol"] ?? ses["ROL"] ?? 0);
            string cia = ses["CIA"]?.ToString() ?? "DIS86";

            if (rol != 0)
            {
                string lista = new EstadoCuentaManager()
                                   .ObtenerPermisosRolModuloSub(rol, cia)
                                   ?.FirstOrDefault(p => p.MODULO == Modulo &&
                                                         p.SUBMODULO == SubModulo)
                                   ?.PERMISOS ?? string.Empty;

                ses[SesKey] = lista;   

                bool tienePermiso = lista.Split(',')
                                         .Any(p => p == PermisoNecesario);

                if (!tienePermiso && !string.IsNullOrWhiteSpace(CampoSesionForzar))
                {
                    string usuario = Convert.ToString(ses["Usuario"]);
                    if (!string.IsNullOrWhiteSpace(usuario))
                    {
                        var datos = LoginManager.ValidarUsuarioActivo(usuario, cia);
                        if (datos != null)
                        {
                            if (CampoSesionForzar == "Cod_Cobrador")
                            {
                                string cob = datos.COBRADOR?.Trim().ToUpperInvariant();
                                ses["Cod_Cobrador"] = (cob == "CND" || string.IsNullOrEmpty(cob)) ? "0" : cob;
                            }

                            if (CampoSesionForzar == "Vendedor")
                            {
                                string ven = datos.VENDEDOR?.Trim().ToUpperInvariant();
                                ses["Vendedor"] = (ven == "VND" || string.IsNullOrEmpty(ven)) ? "0" : ven;
                            }
                        }


                    }
                }
            }

            base.OnActionExecuting(ctx);
        }
    }
}
