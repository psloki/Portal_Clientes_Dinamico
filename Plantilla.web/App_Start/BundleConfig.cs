using System.Web;
using System.Web.Optimization;

namespace Plantilla.web
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new Bundle("~/bundles/KardexIndex").Include(
                "~/Scripts/Kardex/Index.js"));
            bundles.Add(new Bundle("~/bundles/AprobacionIndex").Include(
                       "~/Scripts/Aprobacion/Index.js"
                       ));
            bundles.Add(new Bundle("~/bundles/AprobacionRecibosIndex").Include(
                      "~/Scripts/AprobacionRecibos/Index.js"
                      ));
            bundles.Add(new Bundle("~/bundles/ArticuloPrecioIndex").Include(
                   "~/Scripts/ArticuloPrecio/Index.js"
                   ));
            bundles.Add(new Bundle("~/bundles/BonificacionesIndex").Include(
                 "~/Scripts/Bonificaciones/Index.js"
                 ));
            bundles.Add(new Bundle("~/bundles/BonificacionesClienteIndex").Include(
              "~/Scripts/BonificacionesCliente/Index.js"
              ));
            bundles.Add(new Bundle("~/bundles/ConfiguracionIndex").Include(
             "~/Scripts/Configuracion/Index.js"
             ));
            bundles.Add(new Bundle("~/bundles/DashboardIndex").Include(
           "~/Scripts/Dashboard/Index.js"
           ));
            bundles.Add(new Bundle("~/bundles/DescuentoClienteIndex").Include(
              "~/Scripts/DescuentoCliente/Index.js"
              ));
            bundles.Add(new Bundle("~/bundles/DescuentosIndex").Include(
             "~/Scripts/Descuentos/Index.js"
             ));
            bundles.Add(new Bundle("~/bundles/DocumentosHistoricosIndex").Include(
         "~/Scripts/DocumentosHistoricos/Index.js"
         ));
            bundles.Add(new Bundle("~/bundles/EstadosCuentaIndex").Include(
        "~/Scripts/EstadosCuenta/Index.js"
        ));
            bundles.Add(new Bundle("~/bundles/ExpedienteDigitalIndex").Include(
      "~/Scripts/ExpedienteDigital/Index.js"
      ));
            bundles.Add(new Bundle("~/bundles/LiquidacionIndex").Include(
     "~/Scripts/Liquidacion/Index.js"
     ));
            bundles.Add(new Bundle("~/bundles/PedidosIndex").Include(
     "~/Scripts/Pedidos/Index.js"
     ));
            bundles.Add(new Bundle("~/bundles/RegistroPagosIndex").Include(
     "~/Scripts/RegistroPagos/Index.js"
     ));
            bundles.Add(new Bundle("~/bundles/ReportePagosIndex").Include(
   "~/Scripts/ReportePagos/Index.js"
   ));
            bundles.Add(new Bundle("~/bundles/TransferenciasIndex").Include(
    "~/Scripts/Transferencias/Index.js"
    ));
    bundles.Add(new Bundle("~/bundles/UsuarioIndex").Include(
        "~/Scripts/Usuario/Index.js"
    ));

        }
    }
}
