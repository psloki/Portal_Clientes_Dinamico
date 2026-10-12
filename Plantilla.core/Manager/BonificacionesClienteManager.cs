using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
   public class BonificacionesClienteManager: Manager
    {
        public List<VIEW_OBT_CLIENTES_PTC> Obtener_Lista_Clientes_Boni(string cia)
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

        public List<VIEW_OBT_LISTA_ARTICULOS_BONIFICACION> Obtener_Lista_ArticuloBoni(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBT_LISTA_ARTICULOS_BONIFICACION> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBT_LISTA_ARTICULOS_BONIFICACION>(cia, "VIEW_OBT_LISTA_ARTICULOS_BONIFICACION");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_LISTA_ARTICULOS_BONIFICACIONs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }
        public List<PROC_OBT_NIVEL_PRECIO_CLIENTEResult> Obtener_Lista_NivelPrecioBoni(string cia)
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
        public List<PROC_OBT_CATEGORIA_CLIENTE_NIVEL_PRECIOResult> Obtener_Lista_CategoriaCLienteBoni(string cia)
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
        public List<PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEX_V2Result> Obtener_Lista_Articulos(string articulo, string casa,  string subcasa, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEX_V2Result> result = new List<PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEX_V2Result>();
                try
                {
                    result = context.PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEX_V2(articulo,"0","0", cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_ARTICULO_PRECIO_INDEX_V2", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_BONIF_ART_CLIENTE_INDEX_V2Result> ObtenerListaBonificacionesCliente(string cliente, string categoria, string articulo, string casa, string subcasa, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_BONIF_ART_CLIENTE_INDEX_V2Result> result = new List<PROC_OBT_BONIF_ART_CLIENTE_INDEX_V2Result>();
                try
                {
                    result = context.PROC_OBT_BONIF_ART_CLIENTE_INDEX_V2(cliente, categoria, articulo, casa, subcasa, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_BONIF_ART_CLIENTE_INDEX_V2", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public string Editar_Bonificacion_Cliente(string cliente, string articulo, int escala, int cantidadMin, int cantidadMax,
                                                  DateTime fechaIni, DateTime FechaFin, int factor, string Edi_articuloBoniCli, int UndBonificar, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_BONIFICACION_CLIENTE_ARTICULO(cliente, articulo, escala, cantidadMin, cantidadMax, fechaIni, FechaFin, factor, Edi_articuloBoniCli, UndBonificar, cia).ToString();
                    //result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_BONIFICACION_CLIENTE_ARTICULO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }
        public string Editar_Bonificacion_Cliente_Masivo(
            string cliente,
            string articulosCsv,
            int escala,
            int cantidadMin,
            int cantidadMax,
            DateTime fechaIni,
            DateTime fechaFin,
            int factor,
            string edi_articuloBoniCli,
            int undBonificar,
            string cia
        )
                {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context
                        .PROC_EDI_BONIFICACION_CLIENTE_ARTICULO_MASIVO(
                            cliente,
                            articulosCsv,
                            escala,
                            cantidadMin,
                            cantidadMax,
                            fechaIni,
                            fechaFin,
                            factor,
                            edi_articuloBoniCli,
                            undBonificar,
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
                    WriteLog("PROC_EDI_BONIFICACION_CLIENTE_ARTICULO_MASIVO", msg);
                    result = "0";
                }
                finally
                {
                    context.Connection.Close();
                }
                return result;
            }
        }

        public string Actualizar_Bonificacion_Excel_Cliente(string cliente, string articulo, int escala, int cantidad_min, int cantidad_max, DateTime fechaIni, DateTime fechaFin, int factor_boni,
                                                                            string  articuloBoni, int unidadesBoni,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_BONIFICACION_CLIENTE_ARTICULO(cliente, articulo, escala, cantidad_min, cantidad_max, fechaIni, fechaFin, factor_boni,
                                                                           articuloBoni, unidadesBoni, cia).ToString();
                    //result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_BONIFICACION_CLIENTE_ARTICULO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }

    }
}
