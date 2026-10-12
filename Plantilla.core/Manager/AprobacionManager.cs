using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class AprobacionManager : Manager
    {
        public static List<PROC_OBT_CUENTAS_X_COBRARResult> ObtenerCuentasXCobrar(
          string fechaInicio, string fechaFin, string documento, string tipo, string subtipo, string vendedor, string cia
     )
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CUENTAS_X_COBRARResult> result = new List<PROC_OBT_CUENTAS_X_COBRARResult>();
                try
                {
                    result = context.PROC_OBT_CUENTAS_X_COBRAR(
                          fechaInicio, fechaFin, documento, tipo, subtipo, vendedor, cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CUENTAS_X_COBRAR", e);
                    return null;
                }
            }
        }
        public static List<PROC_OBT_TIPOS_DOCUMENTOS_CCResult> ObtenerTiposDocumento(
           string cia, string tipo = null)
        {
            using (PlantillaDataContext ctx = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    return ctx.PROC_OBT_TIPOS_DOCUMENTOS_CC(cia, tipo).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message
                                 .Replace(Environment.NewLine, " ")
                                 .Replace('"', ' ')
                                 .Replace("'", " ");
                    WriteLog("PROC_OBT_TIPOS_DOCUMENTOS_CC", e);
                    return null;
                }
            }
        }
        public static List<PROC_OBT_SUBTIPOS_DOCUMENTOS_CCResult>
        ObtenerSubtiposDocumento(string cia,
                                 string tipo = null,
                                 string subtipo = null)
        {
            using (PlantillaDataContext ctx = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    return ctx.PROC_OBT_SUBTIPOS_DOCUMENTOS_CC(cia, tipo, subtipo)
                              .ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message
                                 .Replace(Environment.NewLine, " ")
                                 .Replace('"', ' ')
                                 .Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPOS_DOCUMENTOS_CC", e);
                    return null;
                }
            }
        }

        public static List<PROC_OBT_VENDEDORResult> ObtenerVendedoresFiltro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_VENDEDORResult> result = new List<PROC_OBT_VENDEDORResult>();
                try
                {
                    result = context.PROC_OBT_VENDEDOR(
                         cia
                    ).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_VENDEDOR", e);
                    return null;
                }
            }
        }
    }
}
