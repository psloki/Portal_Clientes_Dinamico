using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class KardexManager : Manager
    {
        public static KardexPagedResult ObtenerIndexKardex(
            string fechaInicio,
            string fechaFin,
            string cliente,
            string tipo,
            string bodega,
            string articulo,
            string cia,
            int start,
            int length,
            string search,
            string orderColumn,
            string orderDirection)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                KardexPagedResult result = new KardexPagedResult
                {
                    Data = new List<PROC_OBT_INDEX_KARDEXResult>()
                };

                try
                {
                    string cnn = context.Connection.ConnectionString;

                    using (SqlConnection cn = new SqlConnection(cnn))
                    using (SqlCommand cmd = new SqlCommand("GLOBAL.PROC_OBT_INDEX_KARDEX", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros (ajusta nombres y tipos según el SP)
                        cmd.Parameters.AddWithValue("@FECHA_INICIO", fechaInicio);
                        cmd.Parameters.AddWithValue("@FECHA_FIN", fechaFin);
                        cmd.Parameters.AddWithValue("@CLIENTE", string.IsNullOrEmpty(cliente) ? (object)DBNull.Value : cliente);
                        cmd.Parameters.AddWithValue("@TIPO", string.IsNullOrEmpty(tipo) ? (object)DBNull.Value : tipo);
                        cmd.Parameters.AddWithValue("@BODEGA", string.IsNullOrEmpty(bodega) ? (object)DBNull.Value : bodega);
                        cmd.Parameters.AddWithValue("@ARTICULO", string.IsNullOrEmpty(articulo) ? (object)DBNull.Value : articulo);
                        cmd.Parameters.AddWithValue("@CIA", cia);
                        cmd.Parameters.AddWithValue("@START", start < 0 ? 0 : start);
                        cmd.Parameters.AddWithValue("@LENGTH", length <= 0 ? 20 : length);
                        cmd.Parameters.AddWithValue("@SEARCH", string.IsNullOrWhiteSpace(search) ? (object)DBNull.Value : search.Trim());
                        cmd.Parameters.AddWithValue("@ORDER_COLUMN", string.IsNullOrWhiteSpace(orderColumn) ? "FECHA" : orderColumn);
                        cmd.Parameters.AddWithValue("@ORDER_DIR", string.IsNullOrWhiteSpace(orderDirection) ? "DESC" : orderDirection);

                        cn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                result.Data.Add(ToAuditTransInv(dr));
                            }

                            if (dr.NextResult() && dr.Read())
                            {
                                result.RecordsTotal = dr["TOTAL_REGISTROS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TOTAL_REGISTROS"]);
                                result.RecordsFiltered = dr["REGISTROS_FILTRADOS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["REGISTROS_FILTRADOS"]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_INDEX_KARDEX", e);
                }

                return result;
            }
        }
        
        public static List<PROC_OBT_ARTICULOS_TODOSResult> GetArticulosTodos(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_ARTICULOS_TODOSResult> result = new List<PROC_OBT_ARTICULOS_TODOSResult>();

                try
                {
                    string cnn = context.Connection.ConnectionString;

                    using (SqlConnection cn = new SqlConnection(cnn))
                    using (SqlCommand cmd = new SqlCommand("GLOBAL.PROC_OBT_ARTICULOS_TODOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CIA", cia);

                        cn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                result.Add(ToArticulo(dr));
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
                    WriteLog("PROC_OBT_ARTICULOS_TODOS", e);

                    return result;
                }
            }
        }
        
        public static List<PROC_OBT_TIPOSINVResult> GetTiposInv(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TIPOSINVResult> result = new List<PROC_OBT_TIPOSINVResult>();
                try
                {
                    string cnn = context.Connection.ConnectionString;

                    using (SqlConnection cn = new SqlConnection(cnn))
                    using (SqlCommand cmd = new SqlCommand("GLOBAL.PROC_OBT_TIPOSINV", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CIA", cia);

                        cn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                result.Add(ToTipoInv(dr));
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
                    WriteLog("PROC_OBT_TIPOSINV", e);

                    return result;
                }
            }
        }

        public static List<PROC_OBT_BODEGAS_TODASResult> Obtener_Lista_Bodegas(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_BODEGAS_TODASResult> result = new List<PROC_OBT_BODEGAS_TODASResult>();
                try
                {
                    string cnn = context.Connection.ConnectionString;

                    using (SqlConnection cn = new SqlConnection(cnn))
                    using (SqlCommand cmd = new SqlCommand("GLOBAL.PROC_OBT_BODEGAS_TODAS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CIA", cia);

                        cn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                result.Add(ToBodega(dr));
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
                    WriteLog("PROC_OBT_BODEGAS_TODAS", e);
                    
                    return result;
                }
            }
        }

        public static PROC_OBT_TIPOSINVResult ToTipoInv(SqlDataReader dr)
        {
            return new PROC_OBT_TIPOSINVResult
            {
                TIPO = dr["TIPO"]?.ToString(),
                DESCRIPCION = dr["DESCRIPCION"]?.ToString()
            };
        }

        public static PROC_OBT_BODEGAS_TODASResult ToBodega(SqlDataReader dr)
        {
            return new PROC_OBT_BODEGAS_TODASResult
            {
                BODEGA = dr["BODEGA"]?.ToString(),
                NOMBRE = dr["NOMBRE"]?.ToString()
            };
        }

        public static PROC_OBT_INDEX_KARDEXResult ToAuditTransInv(SqlDataReader dr)
        {
            return new PROC_OBT_INDEX_KARDEXResult
            {
                AUDIT_TRANS_INV = dr["AUDIT_TRANS_INV"]?.ToString(),
                APLICACION = dr["APLICACION"]?.ToString(),
                ASIENTO = dr["ASIENTO"]?.ToString(),
                REFERENCIA = dr["REFERENCIA"]?.ToString(),

                CENTRO_COSTO = dr["CENTRO_COSTO"]?.ToString(),
                CENTRO_COSTO_COMPLETO = dr["CENTRO_COSTO_COMPLETO"]?.ToString(),

                ARTICULO = dr["ARTICULO"]?.ToString(),
                ARTICULO_COMPLETO = dr["ARTICULO_COMPLETO"]?.ToString(),

                COSTO_TOT_FISC_LOC = dr["COSTO_TOT_FISC_LOC"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(dr["COSTO_TOT_FISC_LOC"]),

                COSTO_TOT_FISC_LOC_STR = dr["COSTO_TOT_FISC_LOC_STR"]?.ToString(),

                COSTO_UNITARIO_LOCAL = dr["COSTO_UNITARIO_LOCAL"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(dr["COSTO_UNITARIO_LOCAL"]),

                COSTO_UNITARIO_LOCAL_STR = dr["COSTO_UNITARIO_LOCAL_STR"]?.ToString(),

                COSTO_UNITARIO_DOLAR = dr["COSTO_UNITARIO_DOLAR"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(dr["COSTO_UNITARIO_DOLAR"]),

                COSTO_UNITARIO_DOLAR_STR = dr["COSTO_UNITARIO_DOLAR_STR"]?.ToString(),

                COSTO_TOT_FISC_DOL = dr["COSTO_TOT_FISC_DOL"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(dr["COSTO_TOT_FISC_DOL"]),

                COSTO_TOT_FISC_DOL_STR = dr["COSTO_TOT_FISC_DOL_STR"]?.ToString(),

                NATURALEZA = dr["NATURALEZA"]?.ToString(),

                CANTIDAD = dr["CANTIDAD"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(dr["CANTIDAD"]),

                CANTIDAD_INT = dr["CANTIDAD_INT"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(dr["CANTIDAD_INT"]),

                FECHA = dr["FECHA"] == DBNull.Value
                    ? DateTime.MinValue
                    : Convert.ToDateTime(dr["FECHA"]),

                FECHA_STR = dr["FECHA_STR"]?.ToString(),

                TIPO = dr["TIPO"]?.ToString(),
                BODEGA = dr["BODEGA"]?.ToString(),
                CLIENTE = dr["CLIENTE"]?.ToString(),
                CLIENTE_NOMBRE = dr["CLIENTE_NOMBRE"]?.ToString(),

                PRECIO_UNITARIO = dr["PRECIO_UNITARIO"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(dr["PRECIO_UNITARIO"]),
                PRECIO_TOTAL = dr["PRECIO_TOTAL"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(dr["PRECIO_TOTAL"]),
            };
        }

        public static PROC_OBT_ARTICULOS_TODOSResult ToArticulo(SqlDataReader dr)
        {
            return new PROC_OBT_ARTICULOS_TODOSResult
            {
                ARTICULO = dr["ARTICULO"]?.ToString(),
                DESCRIPCION = dr["DESCRIPCION"]?.ToString(),
                UNIDAD_ALMACEN = dr["UNIDAD_ALMACEN"]?.ToString(),
                COMPLETO = dr["COMPLETO"]?.ToString()
            };
        }

        public class PROC_OBT_TIPOSINVResult
        {
            public string TIPO { get; set; }
            public string DESCRIPCION { get; set; }
        }

        public class PROC_OBT_BODEGAS_TODASResult
        {
            public string BODEGA { get; set; }
            public string NOMBRE { get; set; }
        }

        public class PROC_OBT_ARTICULOS_TODOSResult
        {
            public string ARTICULO { get; set; }
            public string DESCRIPCION { get; set; }
            public string UNIDAD_ALMACEN { get; set; }
            public string COMPLETO { get; set; }
        }

        public class PROC_OBT_INDEX_KARDEXResult
        {
            public string AUDIT_TRANS_INV { get; set; }
            public string APLICACION { get; set; }
            public string ASIENTO { get; set; }
            public string REFERENCIA { get; set; }

            public string CENTRO_COSTO { get; set; }
            public string CENTRO_COSTO_COMPLETO { get; set; }

            public string ARTICULO { get; set; }
            public string ARTICULO_COMPLETO { get; set; }

            public decimal COSTO_TOT_FISC_LOC { get; set; }
            public string COSTO_TOT_FISC_LOC_STR { get; set; }

            public decimal COSTO_UNITARIO_LOCAL { get; set; }
            public string COSTO_UNITARIO_LOCAL_STR { get; set; }

            public decimal COSTO_UNITARIO_DOLAR { get; set; }
            public string COSTO_UNITARIO_DOLAR_STR { get; set; }

            public decimal COSTO_TOT_FISC_DOL { get; set; }
            public string COSTO_TOT_FISC_DOL_STR { get; set; }

            public string NATURALEZA { get; set; }

            public decimal CANTIDAD { get; set; }
            public int CANTIDAD_INT { get; set; }

            public DateTime FECHA { get; set; }
            public string FECHA_STR { get; set; }

            public string TIPO { get; set; }
            public string BODEGA { get; set; }

            public string CLIENTE { get; set; }
            public string CLIENTE_NOMBRE { get; set; }
            public decimal PRECIO_UNITARIO { get; set; }
            public decimal PRECIO_TOTAL { get; set; }
        }

        public class KardexPagedResult
        {
            public List<PROC_OBT_INDEX_KARDEXResult> Data { get; set; }
            public int RecordsTotal { get; set; }
            public int RecordsFiltered { get; set; }
        }
    }
}
