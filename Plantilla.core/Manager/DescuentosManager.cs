using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class DescuentosManager : Manager
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

        public List<PROC_OBT_NIVEL_PRECIO_CLIENTEResult> Obtener_Lista_nivelPrecioDesc(string cia)
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

        public List<PROC_OBT_NIVEL_ARTICULO_DESCUENTO_INDEXResult> ObtenerListaClienteArticuloDcto(string articulo, string nivelPrecio, string casa, string subcasa, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_NIVEL_ARTICULO_DESCUENTO_INDEXResult> result = new List<PROC_OBT_NIVEL_ARTICULO_DESCUENTO_INDEXResult>();
                try
                {
                    result = context.PROC_OBT_NIVEL_ARTICULO_DESCUENTO_INDEX(articulo, nivelPrecio, casa, subcasa, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_NIVEL_ARTICULO_DESCUENTO_INDEX", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public int Editar_Porcentaje_Descuento(
            string nivelPrecio,
            string moneda,
            int version,
            string articulo,
            int escala,
            int versionDcto,
            int cantidadMin,
            int cantidadMax,
            decimal porcentajeProyectado,
            DateTime fechaInicioProyectada,
            DateTime fechaFinProyectada,
            string usuario,
            string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                int result;
                try
                {
                    result = context.PROC_EDI_PORCENTAJE_DESCUENTO(
                        nivelPrecio,
                        moneda,
                        version,
                        articulo,
                        escala,
                        versionDcto,
                        cantidadMin,
                        cantidadMax,
                        porcentajeProyectado,
                        fechaInicioProyectada,
                        fechaFinProyectada,
                        usuario,
                        cia);
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_PORCENTAJE_DESCUENTO", e);
                    result = 0;
                }
                context.Connection.Close();
                return result;
            }

        }
        public int Editar_Porcentaje_Descuento_Masivo(
            string nivelPrecio,
            string listaRegistros,
            int cantidadMin,
            int cantidadMax,
            decimal porcentajeProyectado,
            DateTime fechaInicioProyectada,
            DateTime fechaFinProyectada,
            string usuario,
            string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                int result;
                try
                {
                    result = context.PROC_EDI_PORCENTAJE_DESCUENTO_MASIVO(
                        nivelPrecio,
                        listaRegistros,
                        cantidadMin,
                        cantidadMax,
                        porcentajeProyectado,
                        fechaInicioProyectada,
                        fechaFinProyectada,
                        usuario,
                        cia);
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_PORCENTAJE_DESCUENTO_MASIVO", e);
                    result = 0;
                }
                context.Connection.Close();
                return result;
            }
        }

        public int Actualizar_Descuento_Excel(
            string nivelPrecio,
            string moneda,
            int version,
            string articulo,
            int escala,
            int versionDcto,
            int cantidadMin,
            int cantidadMax,
            decimal porcentajeProyectado,
            DateTime fechaInicioProyectada,
            DateTime fechaFinProyectada,
            string usuario,
            string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                int result;
                try
                {
                    result = context.PROC_EDI_DESCUENTO_ARTICULO_PRECIO(
                        nivelPrecio,
                        moneda,
                        version,
                        articulo,
                        escala,
                        versionDcto,
                        cantidadMin,
                        cantidadMax,
                        porcentajeProyectado,
                        fechaInicioProyectada,
                        fechaFinProyectada,
                        usuario,
                        cia);
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_EDI_DESCUENTO_ARTICULO_PRECIO", e);
                    result = 0;
                }
                context.Connection.Close();
                return result;
            }

        }

        public int Eliminar_Descuento(
            string nivelPrecio,
            string moneda,
            int version,
            string articulo,
            int escalaDcto,
            int versionDcto,
            string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                int result;
                try
                {
                    result = context.PROC_ELI_DESCUENTO_ARTICULO(
                        nivelPrecio,
                        moneda,
                        version,
                        articulo,
                        escalaDcto,
                        versionDcto,
                        cia);
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_ELI_DESCUENTO_ARTICULO", e);
                    result = 0;
                }

                context.Connection.Close();
                return result;
            }
        }
    }
}
