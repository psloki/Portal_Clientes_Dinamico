using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plantilla.core.Manager.KardexManager;

namespace Plantilla.core.Manager
{
    public class HomeManager : Manager
    {

        public static List<PROC_OBT_VENDEDORResult> ObtenerVendedor(string cia)
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


        public static List<PROC_OBT_COBRADOR_FILTROResult> Obtener_Comprador(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_COBRADOR_FILTROResult> result;
                try
                {
                    result = context.PROC_OBT_COBRADOR_FILTRO(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_COBRADOR_FILTRO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public static List<PROC_OBT_CLIENTE_FILTROSResult> Obtener_Lista_Clientes(string vendedor,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTE_FILTROSResult> result = new List<PROC_OBT_CLIENTE_FILTROSResult>();
                try
                {
                    result = context.PROC_OBT_CLIENTE_FILTROS(vendedor, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_FILTROS", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public static List<PROC_OBT_CLIENTE_COBRADOR_VENDEDORResult> Obtener_Clientes_Cobrador_Vendedor(string cobrador, string vendedor, string cia)
        {
            if (cobrador == "CND")
                cobrador = "0";

            if (vendedor == "VND")
                vendedor = "0";

            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTE_COBRADOR_VENDEDORResult> result = new List<PROC_OBT_CLIENTE_COBRADOR_VENDEDORResult>();
                try
                {
                    result = context.PROC_OBT_CLIENTE_COBRADOR_VENDEDOR(cobrador, vendedor, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_COBRADOR_VENDEDOR", e);
                }
                context.Connection.Close();
                return result;
            }
        }


        public static List<PROC_OBT_CLIENTE_COBRADORResult> Obtener_Lista_Clientes_Cobrador(string cobrador,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTE_COBRADORResult> result = new List<PROC_OBT_CLIENTE_COBRADORResult>();
                try
                {
                    result = context.PROC_OBT_CLIENTE_COBRADOR(cobrador,cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_COBRADOR", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public static PROC_OBT_CLIENTEResult GetClienteById(string cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                var result = new PROC_OBT_CLIENTEResult();

                try
                {
                    string cnn = context.Connection.ConnectionString;

                    using (SqlConnection cn = new SqlConnection(cnn))
                    using (SqlCommand cmd = new SqlCommand("GLOBAL.[PROC_OBT_CLIENTE_X_ID]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CLIENTE", cliente);
                        cmd.Parameters.AddWithValue("@CIA", cia);

                        cn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                result = ToCliente(dr);
                            }
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_X_ID", e);

                    return result;
                }
            }
        }

        public static PROC_OBT_CLIENTEResult ToCliente(SqlDataReader dr)
        {
            return new PROC_OBT_CLIENTEResult
            {
                CLIENTE = dr["CLIENTE"]?.ToString(),
                NOMBRE = dr["NOMBRE"]?.ToString(),
                VENDEDOR = dr["VENDEDOR"]?.ToString(),
                COBRADOR = dr["COBRADOR"]?.ToString(),
                NOMBRE_VENDEDOR = dr["NOMBRE_VENDEDOR"]?.ToString(),
                NOMBRE_COBRADOR = dr["NOMBRE_COBRADOR"]?.ToString(),
            };
        }
    }

    public class PROC_OBT_CLIENTEResult
    {
        public string CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public string VENDEDOR { get; set; }
        public string NOMBRE_VENDEDOR { get; set; }
        public string COBRADOR { get; set; }
        public string NOMBRE_COBRADOR { get; set; }
    }

}
