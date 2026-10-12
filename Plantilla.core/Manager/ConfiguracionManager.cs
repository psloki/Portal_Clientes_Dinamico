using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class ConfiguracionManager : Manager
    {
        public static List<PROC_OBT_ESTADO_PEDIDOResult> GetEstado_Pedido(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_ESTADO_PEDIDOResult> result = new List<PROC_OBT_ESTADO_PEDIDOResult>();
                try
                {
                    result = context.PROC_OBT_ESTADO_PEDIDO(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_ESTADO_PEDIDO", e);
                }
                context.Connection.Close();
                return result;
            }

        }



        public static List<PROC_OBT_COLOR_HEXAResult> GetSegui_Ped(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_COLOR_HEXAResult> result = new List<PROC_OBT_COLOR_HEXAResult>();
                try
                {
                    result = context.PROC_OBT_COLOR_HEXA(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_COLOR_HEXA", e);
                }
                context.Connection.Close();
                return result;
            }

        }


        public static string Actualiza_estado_ped(int codigo, string det_direccion, string color_hexa, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_SEGUI_PEDIDO(codigo, det_direccion, color_hexa, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_SEGUI_PEDIDO", e);
                }
                return result;
            }
        }

        public static string Guardar_estado_ped(string codigo, string det_direccion, string color_hexa, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_SEGUI_PEDIDO(codigo, det_direccion, color_hexa, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_SEGUI_PEDIDO", e);
                }
                return result;
            }
        }


        public static List<PROC_OBT_SEGUI_FACTURAResult> GetSeguimineto_Factura(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SEGUI_FACTURAResult> result = new List<PROC_OBT_SEGUI_FACTURAResult>();
                try
                {
                    result = context.PROC_OBT_SEGUI_FACTURA(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SEGUI_FACTURA", e);
                }
                context.Connection.Close();
                return result;
            }

        }


        public static string Actualiza_seguimiento_fac(int codigo, string det_direccion, string color_hexa, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_SEGUI_FACTURA(codigo, det_direccion, color_hexa, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_SEGUI_FACTURA", e);
                }
                return result;
            }
        }


        public static string Guardar_seguimiento_fac(string codigo, string det_direccion, string color_hexa, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_SEGUI_FACTURA(codigo, det_direccion, color_hexa, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_SEGUI_FACTURA", e);
                }
                return result;
            }
        }

        public static List<PROC_OBT_SUCESOSResult> Get_Sucesos(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUCESOSResult> result = new List<PROC_OBT_SUCESOSResult>();
                try
                {
                    result = context.PROC_OBT_SUCESOS(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUCESOS", e);
                }
                context.Connection.Close();
                return result;
            }

        }


        public static string Actualiza_suceso(int codigo, string descripcion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_SUCESO(codigo, descripcion, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_SUCESO", e);
                }
                return result;
            }
        }

        public static string Guardar_suceso(string descripcion, string det_descripcion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_SUCESO(descripcion, det_descripcion, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_SUCESO", e);
                }
                return result;
            }
        }
    }
}
