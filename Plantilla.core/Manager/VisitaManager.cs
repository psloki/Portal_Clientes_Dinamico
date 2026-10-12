using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class VisitaManager : Manager
    {

        public List<PROC_OBT_SEMANAS_POR_ANIOResult> ObtenerSemana_Anio(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SEMANAS_POR_ANIOResult> result;
                try
                {

                    result = context.PROC_OBT_SEMANAS_POR_ANIO(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SEMANAS_POR_ANIO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<PROC_OBT_VENDEDORResult> ObtenerVendedor(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_VENDEDORResult> result;
                try
                {

                    result = context.PROC_OBT_VENDEDOR(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_VENDEDOR", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_TIPO_VISITAResult> ObtenerTipoVisita(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TIPO_VISITAResult> result;
                try
                {

                    result = context.PROC_OBT_TIPO_VISITA(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_TIPO_VISITA", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_VISITASResult> ObtenerVistas(string fecha_ini, string fecha_fin, string semanamulti, string dia_semana, string tipo_visita, string vis_vendedor, string vis_cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_VISITASResult> result;
                try
                {
                    result = context.PROC_OBT_VISITAS(fecha_ini, fecha_fin, semanamulti, dia_semana, tipo_visita, vis_vendedor, vis_cliente, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_VISITAS", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }

        public List<PROC_OBT_UBICACION_CLIENTEResult> Obtener_Datos_Ubicacion(string vendedor, string fechavisita,string cliente, string cia)

        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_UBICACION_CLIENTEResult> result;
                try
                {
                    result = context.PROC_OBT_UBICACION_CLIENTE(vendedor, fechavisita, cliente, cia).ToList();

                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_UBICACION_CLIENTE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }
        }
    }
}
