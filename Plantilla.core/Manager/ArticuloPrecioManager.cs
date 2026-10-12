using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class ArticuloPrecioManager:Manager
    {

        public List<PROC_OBT_MARCAResult> Obtener_Lista_Marca(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_MARCAResult> result = new List<PROC_OBT_MARCAResult>();
                try
                {
                    result = context.PROC_OBT_MARCA(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_MARCA", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_CASAResult> Obtener_Lista_Casa(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CASAResult> result = new List<PROC_OBT_CASAResult>();
                try
                {
                    result = context.PROC_OBT_CASA(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CASA", e);
                }
                context.Connection.Close();
                return result;
            }

        }
        public List<PROC_OBT_SUBCASAResult> Obtener_Lista_SubCasa(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBCASAResult> result = new List<PROC_OBT_SUBCASAResult>();
                try
                {
                    result = context.PROC_OBT_SUBCASA(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBCASA", e);
                }
                context.Connection.Close();
                return result;
            }

        }
        public List<VIEW_OBT_CLIENTES_PTC> Obtener_Lista_Clientes(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBT_CLIENTES_PTC> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBT_CLIENTES_PTC>(cia, "VIEW_OBT_CLIENTES_PTC");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_CLIENTES_PTCs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_NIVEL_PRECIO_CLIENTEResult> Obtener_Lista_nivelPrecio(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_NIVEL_PRECIO_CLIENTEResult> result = new List<PROC_OBT_NIVEL_PRECIO_CLIENTEResult>();
                try
                {
                    result = context.PROC_OBT_NIVEL_PRECIO_CLIENTE(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_NIVEL_PRECIO_CLIENTE", e);
                }
                context.Connection.Close();
                return result;
            }

        }
        //public List<PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTEResult> Obtener_Lista_ArtciuloNP(string cia)
        //{
        //    using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
        //    {
        //        List<PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTEResult> result = new List<PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTEResult>();
        //        try
        //        {
        //            result = context.PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE(cia).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            string e = ex.Message;
        //            e = e.Replace(Environment.NewLine, " ");
        //            e = e.Replace('"', ' ');
        //            e = e.Replace("'", " ");
        //            WriteLog("PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE", e);
        //        }
        //        context.Connection.Close();
        //        return result;
        //    }

        //}
        public List<PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE_V2Result> Obtener_Lista_ArtciuloNP(string cia, string nivelPrecio)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE_V2Result> result = new List<PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE_V2Result>();
                try
                {
                    result = context.PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE_V2(nivelPrecio, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE_V2", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_CATEGORIA_CLIENTE_NIVEL_PRECIOResult> Obtener_Lista_CategoriaCLienteNP(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CATEGORIA_CLIENTE_NIVEL_PRECIOResult> result = new List<PROC_OBT_CATEGORIA_CLIENTE_NIVEL_PRECIOResult>();
                try
                {
                    result = context.PROC_OBT_CATEGORIA_CLIENTE_NIVEL_PRECIO(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CATEGORIA_CLIENTE_NIVEL_PRECIO", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEXResult> ObtenerListaClienteArticuloNivelPrecio(string nivelPrecioAP, string articuloAP, string casaAP, string subcasaAp,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEXResult> result = new List<PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEXResult>();
                try
                {
                    result = context.PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEX(nivelPrecioAP, articuloAP, casaAP, subcasaAp, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEX", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public string Editar_PrecioArticulo(string articulo, decimal precio, string nivelPrecio,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_ARTICULO_PRECIO(articulo, precio, nivelPrecio,cia).ToString();
                    //result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_ARTICULO_PRECIO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }
        public string Editar_PrecioMasivo(string articulosCsv,
                               decimal precio,
                               string nivelPrecio,
                               string cia)
        {
            using (PlantillaDataContext ctx = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                

                   
                    int? r = ctx.PROC_EDI_ARTICULO_PRECIO_MASIVO(
                         articulosCsv,
                         precio,
                         nivelPrecio,
                         cia);

                    return (r.HasValue && r.Value == 1) ? "1" : "0";
                }
                catch (Exception ex)
                {
                    WriteLog("PROC_EDI_ARTICULO_PRECIO_MASIVO",
                             ex.Message.Replace(Environment.NewLine, " "));
                    return "0";
                }
            }
        }
        public string Actualizar_ArticuloPrecio_Excel(string nivel_precio, string moneda, int version, string articulo, int version_articulo, 
                                                     decimal precio, string esquema_trabajo,decimal margen_utilidad, DateTime fechaIni, DateTime fechaFin,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_ARTICULO_PRECIO_CLIENTE_ARTICULO(nivel_precio, moneda, version, articulo, 
                                                                               version_articulo,precio, esquema_trabajo,
                                                                               margen_utilidad,fechaIni, fechaFin, cia).ToString();
                    //result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_ARTICULO_PRECIO_CLIENTE_ARTICULO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }

        public string Copiar_NivelPrecio(string cia, string nivelPrecioOrigen, string nivelPrecioDestino, int porcentaje)
        {
            try
            {
                using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
                using (var connection = new SqlConnection(context.Connection.ConnectionString))
                using (var command = new SqlCommand("GLOBAL.PROC_EDI_NIVEL_PRECIO_DESTINO", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NIVEL_PRECIO_ORIGEN", nivelPrecioOrigen);
                    command.Parameters.AddWithValue("@NIVEL_PRECIO_DESTINO", nivelPrecioDestino);
                    command.Parameters.AddWithValue("@PORCENTAJE", porcentaje);
                    command.Parameters.AddWithValue("@CIA", cia);

                    var returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    command.ExecuteNonQuery();

                    return returnParameter.Value.ToString();
                }
            }
            catch (SqlException ex)
            {
                string mensaje = ex.Message
                    .Replace(Environment.NewLine, " ")
                    .Replace("\"", " ")
                    .Replace("'", " ");

                WriteLog("GLOBAL.PROC_EDI_NIVEL_PRECIO_DESTINO", mensaje);
                return "0";
            }
        }
    }
}
