using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class ExpedienteDigitalManager : Manager
    {
        public List<PROC_OBT_EXPEDIENTE_DIGITAL_PPResult> GetExpedienteDigitalPP(string fechaInicio, string fechaFin, string tipo, string modulo, string proveedor, string referencia, string usuario, int rol, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_EXPEDIENTE_DIGITAL_PPResult> result = new List<PROC_OBT_EXPEDIENTE_DIGITAL_PPResult>();
                try
                {
                    result = context.PROC_OBT_EXPEDIENTE_DIGITAL_PP(fechaInicio, fechaFin, tipo, modulo, proveedor, referencia, usuario, rol, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_EXPEDIENTE_DIGITAL_PP", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }

        public List<PROC_OBT_TIPOS_EXPEDIENTE_DIGITAL_PPResult> GetTiposExpedienteDigitalPP(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TIPOS_EXPEDIENTE_DIGITAL_PPResult> result = new List<PROC_OBT_TIPOS_EXPEDIENTE_DIGITAL_PPResult>();
                try
                {
                    result = context.PROC_OBT_TIPOS_EXPEDIENTE_DIGITAL_PP(cia).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_USUARIOS_TEF", e);
                    return null;
                }
            }
        }

        public List<PROC_OBT_SELECT_TIPO_EXPEDIENTEResult> GetTiposExpedienteFiltro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SELECT_TIPO_EXPEDIENTEResult> result = new List<PROC_OBT_SELECT_TIPO_EXPEDIENTEResult>();
                try
                {
                    result = context.PROC_OBT_SELECT_TIPO_EXPEDIENTE(cia).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SELECT_TIPO_EXPEDIENTE", e);
                    return null;
                }
            }
        }

        public int InsertartExpediente(string documento, string tipo, string referencia, int referenciaLinea, string proveedor, string nombre, string adjunto, string extension, string usuario, int subtipo,string descripcion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                int result = 0;
                try
                {
                    result = context.PROC_INS_EXPEDIENTE_DIGITAL_PP(documento, tipo, referencia, referenciaLinea, proveedor, nombre, adjunto, extension, usuario, subtipo,descripcion, cia);
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_INS_EXPEDIENTE_DIGITAL_PP", e);
                    return 0;
                }
                context.Connection.Close();
                return result;
            }
        }

        public int EliminarDocumento(string documento, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                int result = 0;
                try
                {
                    result = context.PROC_DEL_EXPEDIENTE_DIGITAL_PP(Convert.ToInt32(documento), cia);
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_DEL_EXPEDIENTE_DIGITAL_PP", e);
                    return 0;
                }
                context.Connection.Close();
                return result;
            }
        }

        //public List<PROC_OBT_PROVEEDOR_LINEA_SOLICITUDResult> GetProveedorSolicitudLinea(string solicitud, int linea, string cia)
        //{
        //    using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
        //    {
        //        List<PROC_OBT_PROVEEDOR_LINEA_SOLICITUDResult> result;
        //        try
        //        {
        //            result = context.PROC_OBT_PROVEEDOR_LINEA_SOLICITUD(solicitud, linea, cia).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            string e = ex.Message;
        //            e = e.Replace(Environment.NewLine, " ");
        //            e = e.Replace('"', ' ');
        //            e = e.Replace("'", " ");
        //            WriteLog("PROC_OBT_PROVEEDOR_LINEA_SOLICITUD", e);
        //            return null;
        //        }
        //        context.Connection.Close();
        //        return result;
        //    }

        //}

        public List<PROC_OBT_PROVEEDOR_EXPEDIENTEResult> GetProveedorExpediente(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_PROVEEDOR_EXPEDIENTEResult> result;
                try
                {
                    result = context.PROC_OBT_PROVEEDOR_EXPEDIENTE(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_PROVEEDOR_EXPEDIENTE", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }
        public List<PROC_OBT_DOC_CXC_EXPEDIENTE_DIGITALResult> GetDocCXC_ExpedienteDigital(string provCliente, string fechaInicio, string fechaFin, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_DOC_CXC_EXPEDIENTE_DIGITALResult> result = new List<PROC_OBT_DOC_CXC_EXPEDIENTE_DIGITALResult>();
                try
                {
                    result = context.PROC_OBT_DOC_CXC_EXPEDIENTE_DIGITAL(provCliente, fechaInicio, fechaFin, cia).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_USUARIOS_TEF", e);
                    return null;
                }
            }
        }


        public List<PROC_OBT_SUBTIPO_MODULO_EXPResult> GetSubTipoModulo(string modulo, string tipoDoc, int subtipo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTIPO_MODULO_EXPResult> result;
                try
                {
                    result = context.PROC_OBT_SUBTIPO_MODULO_EXP(modulo, tipoDoc, subtipo, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPO_MODULO_EXP", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }


        public List<PROC_OBT_PROVEEDORES_CLIENTE_FILTROResult> GetProveedorCliente(string opcion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_PROVEEDORES_CLIENTE_FILTROResult> result;
                try
                {
                    result = context.PROC_OBT_PROVEEDORES_CLIENTE_FILTRO(opcion, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_PROVEEDORES_CLIENTE_FILTRO", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }


        public List<PROC_OBT_PROVEEDORES_CLIENTE_POR_DOCUMENTOResult> GetProveedorPorTipoDocumento(string opcion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_PROVEEDORES_CLIENTE_POR_DOCUMENTOResult> result;
                try
                {
                    result = context.PROC_OBT_PROVEEDORES_CLIENTE_POR_DOCUMENTO(opcion, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_PROVEEDORES_CLIENTE_POR_DOCUMENTO", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }

        public List<PROC_OBT_NUM_DOC_EXPEDIENTE_DIGITALResult> Doc_ExpedienteDigital(string modulo, string proveedor_cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_NUM_DOC_EXPEDIENTE_DIGITALResult> result = new List<PROC_OBT_NUM_DOC_EXPEDIENTE_DIGITALResult>();
                try
                {
                    result = context.PROC_OBT_NUM_DOC_EXPEDIENTE_DIGITAL(modulo, proveedor_cliente, cia).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_USUARIOS_TEF", e);
                    return null;
                }
            }
        }

        public List<PROC_OBT_DOCUMENTOS_CLIENT_PROVEEDOR_EXPEDIENTE_DIGITALResult> GetDocCXC_ClienteExpediente(string provCliente, string fechaInicio, string fechaFin, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_DOCUMENTOS_CLIENT_PROVEEDOR_EXPEDIENTE_DIGITALResult> result = new List<PROC_OBT_DOCUMENTOS_CLIENT_PROVEEDOR_EXPEDIENTE_DIGITALResult>();
                try
                {
                    result = context.PROC_OBT_DOCUMENTOS_CLIENT_PROVEEDOR_EXPEDIENTE_DIGITAL(provCliente, fechaInicio, fechaFin, cia).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_DOCUMENTOS_CLIENT_PROVEEDOR_EXPEDIENTE_DIGITAL", e);
                    return null;
                }
            }
        }


    }
}
