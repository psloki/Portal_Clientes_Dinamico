using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
   public class BonificacionesManager : Manager
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

        public List<PROC_OBT_CLIENTE_ARTICULO_BONIFICADO_INDEX_V2Result> ObtenerListaClienteArticuloBonificados(string articulo, string nivelPrecio, string casa, string subcasa, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTE_ARTICULO_BONIFICADO_INDEX_V2Result> result = new List<PROC_OBT_CLIENTE_ARTICULO_BONIFICADO_INDEX_V2Result>();
                try
                {
                    result = context.PROC_OBT_CLIENTE_ARTICULO_BONIFICADO_INDEX_V2(articulo, nivelPrecio, casa, subcasa, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_ARTICULO_BONIFICADO_INDEX_V2", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public string Editar_Articulo_Bonificacion(string nivelPrecio, string articulo, string articuloBonif, int cantidaMin, int cantidaMax, int UndBonificar,
                                                     int factor, string fechaIni, string FechaFin, int escala, string moneda, int version_bonif, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_ARTICULO_CLIENTE_BONIFICADO(nivelPrecio, articulo, articuloBonif, cantidaMin, cantidaMax,
                                                                          UndBonificar, factor, fechaIni, FechaFin, escala, moneda, version_bonif, cia).ToString();
                    //result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_ARTICULO_CLIENTE_BONIFICADO", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }
        public string Editar_Bonificacion_Masiva(string nivel, string listaArt,
                                         string artBonif, int min, int max,
                                         int und, int factor,
                                         string fIni, string fFin,
                                         int escala, char moneda,
                                         int verBonif, string cia)
        {
            using (PlantillaDataContext ctx = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    return ctx.PROC_EDI_ARTICULO_CLIENTE_BONIFICADO_MASIVO(
                               nivel, listaArt, artBonif, min, max, und, factor,
                               fIni, fFin, escala, moneda, verBonif, cia).ToString();
                }
                catch (Exception ex)
                {
                    WriteLog("PROC_EDI_ARTICULO_CLIENTE_BONIFICADO_MASIVO", ex.Message);
                    return "0";
                }
            }
        }
        public string Actualizar_Bonificacion_Excel(string nivelPrecio, string moneda, int version, string articulo, int escala, int versionBonif, string articuloBoni,
                                                    int cantidad_min, int cantidad_max, int unidadesBoni, DateTime fechaIni, DateTime fechaFin, int factor_boni, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_ESCALA_BONIFICACION(nivelPrecio, moneda, version, articulo, escala, versionBonif, articuloBoni,
                                                                            cantidad_min, cantidad_max, unidadesBoni, fechaIni, fechaFin, factor_boni, cia).ToString();
                    //result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_ESCALA_BONIFICACION", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }

    }
}
