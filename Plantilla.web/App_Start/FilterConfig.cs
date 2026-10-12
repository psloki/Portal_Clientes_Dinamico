using System.Web.Mvc;

namespace Plantilla.web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filter.VerifySession());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filter.VerifyLicense { Order = 0 });
            filters.Add(new Filter.VerifySession { Order = 1 });
        }
    }
}
