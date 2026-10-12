using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plantilla.core.Entities.PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE_V2Result;


namespace Plantilla.core.Manager
{
   public class DescuentoClienteManager:Manager
    {

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

        public List<PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTEResult> Obtener_Lista_ArtciuloDesc(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTEResult> result = new List<PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTEResult>();
                try
                {
                    result = context.PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_ARTICULO_NIVEL_PRECIO_CLIENTE", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_MARCAResult> Obtener_Lista_MarcaDesc(string cia)
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
        public List<PROC_OBT_CASAResult> Obtener_Lista_CasaDesc(string cia)
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
        public List<PROC_OBT_SUBCASAResult> Obtener_Lista_SubCasaDesc(string cia)
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
        public List<PROC_OBT_CATEGORIA_CLIENTE_NIVEL_PRECIOResult> Obtener_Lista_CategoriaCLienteDesc(string cia)
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

        public List<PROC_OBT_DCTO_ART_CLIENTE_INDEX_V2Result> ObtenerLista_Descuento_Cliente(string cliente, string categoria, string articulo,  string casa, string subcasa, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_DCTO_ART_CLIENTE_INDEX_V2Result> result = new List<PROC_OBT_DCTO_ART_CLIENTE_INDEX_V2Result>();
                try
                {
                    result = context.PROC_OBT_DCTO_ART_CLIENTE_INDEX_V2(cliente,categoria, articulo, casa, subcasa, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_DCTO_ART_CLIENTE_INDEX_V2", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public string Editar_Porcentaje_Descuento_Cliente(string cliente, string articulo, int escala, int cantidadMin, int cantidadMax, DateTime fechaIni, DateTime fechaFin, int porc_descuento, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_DESCUENTO_CLIENTE_ARTICULO(cliente, articulo, escala, cantidadMin, cantidadMax, fechaIni, fechaFin, porc_descuento, cia).ToString();
                    
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_DESCUENTO_CLIENTE_ARTICULO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }
        public string Editar_Porcentaje_Descuento_Cliente_Masivo(
            string cliente,
            string articulosCsv,
            int escala,
            int cantidadMin,
            int cantidadMax,
            DateTime fechaIni,
            DateTime fechaFin,
            int porc_descuento,
            string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context
                        .PROC_EDI_DESCUENTO_CLIENTE_ARTICULO_MASIVO(
                            cliente,
                            articulosCsv,
                            escala,
                            cantidadMin,
                            cantidadMax,
                            fechaIni,
                            fechaFin,
                            porc_descuento,
                            cia
                        )
                        .ToString();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message
                                .Replace(Environment.NewLine, " ")
                                .Replace("\"", " ")
                                .Replace("'", " ");
                    WriteLog("PROC_EDI_DESCUENTO_CLIENTE_ARTICULO_MASIVO", msg);
                    result = "0";
                }
                finally
                {
                    context.Connection.Close();
                }
                return result;
            }
        }

        public string Actualizar_Descuento_Excel_Cliente(string cliente, string articulo, int escala, int cantidad_min, int cantidad_max, DateTime fechaIni, DateTime fechaFin, int porc_descuento, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_DESCUENTO_CLIENTE_ARTICULO(cliente, articulo, escala, cantidad_min, cantidad_max, fechaIni, fechaFin, porc_descuento, cia).ToString();
                    //result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_DESCUENTO_CLIENTE_ARTICULO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }

    }
}
