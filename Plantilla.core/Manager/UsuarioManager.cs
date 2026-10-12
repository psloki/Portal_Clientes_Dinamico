using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.Manager
{
    public class UsuarioManager : Manager
    {
        public static List<PROC_OBT_ROLESResult> GetRoles(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_ROLESResult> result = new List<PROC_OBT_ROLESResult>();
                try
                {
                    result = context.PROC_OBT_ROLES(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_ROLES", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        //public static List<PROC_OBT_SELECT_EMPLEADOS_JEFATURAResult> GetEmpleadosSelectJefatura(string cia)
        //{
        //    using (Portal_EntitiesDataContext context = new Portal_EntitiesDataContext())
        //    {
        //        List<PROC_OBT_SELECT_EMPLEADOS_JEFATURAResult> result = new List<PROC_OBT_SELECT_EMPLEADOS_JEFATURAResult>();
        //        try
        //        {
        //            result = context.PROC_OBT_SELECT_EMPLEADOS_JEFATURA(cia).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            string e = ex.Message;
        //            e = e.Replace(Environment.NewLine, " ");
        //            e = e.Replace('"', ' ');
        //            e = e.Replace("'", " ");
        //            WriteLog("PROC_OBT_SELECT_EMPLEADOS_JEFATURA", e);
        //        }
        //        context.Connection.Close();
        //        return result;
        //    }

        //}

        public static List<VIEW_OBTENER_ROLES_MATENIMIENTO> ObtenerRoles_Matenimiento()
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_ROLES_MATENIMIENTO> result = new List<VIEW_OBTENER_ROLES_MATENIMIENTO>();
                try
                {
                    result = context.VIEW_OBTENER_ROLES_MATENIMIENTOs.ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_ROLES_MATENIMIENTOs", e);
                }
                context.Connection.Close();
                return result;
            }

        }




        public static List<PROC_OBT_USUARIO_MANTENIMIENTOResult> GetUsuarios(string cia, int tipo_rol)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_USUARIO_MANTENIMIENTOResult> result = new List<PROC_OBT_USUARIO_MANTENIMIENTOResult>();
                try
                {
                    result = context.PROC_OBT_USUARIO_MANTENIMIENTO(cia, tipo_rol).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_USUARIO_MANTENIMIENTO", e);
                }
                context.Connection.Close();
                return result;
            }

        }



        public static string UpdateUsuario(string usuario, int rol, string vendedor, string cobrador, string bodega, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_USUARIO_MANTENIMIENTO(usuario, rol, vendedor, cobrador, bodega, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_UPDATE_USUARIO_MANTENIMIENTO", e);
                }
                context.Connection.Close();
                return result;
            }

        }


        public static string CambiarEstado(string usuario, string estado, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_UPDATE_ESTADO_ACTIVO_USUARIO(usuario, estado, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_UPDATE_ESTADO_ACTIVO_USUARIO", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public static PROC_VALIDA_EXISTENCIA_USUARIOResult ValidaExistenciaUsuario(string usuario, string correo, string identificacion, string cia)
        {

            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                PROC_VALIDA_EXISTENCIA_USUARIOResult result;
                try
                {

                    result = context.PROC_VALIDA_EXISTENCIA_USUARIO(usuario, correo, identificacion, cia).First();


                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_VALIDA_EXISTENCIA_USUARIO", e);

                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public static string RegistrarUsuario(string usuario, string pass, string email, string cedula, string nombre, string vendedor, string cobrador, string bodega,int rol, string cia)
        {

            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INSERTAR_USUARIO(usuario, pass, email, cedula, nombre, vendedor, cobrador, bodega,rol,cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_INSERTAR_USUARIO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }
        }


        public static string InsertarRol(string descripcion, string comentario, string usuarioEjecucion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    var respuesta = context.PROC_INS_ROL(descripcion, comentario, usuarioEjecucion, cia).FirstOrDefault();
                    result = respuesta?.CODIGO.ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_INS_ROL", e);
                }
                context.Connection.Close();
                return result;
            }

        }


        public static string EditarRol(string rol, string descripcion, string comentario, string usuarioEjecucion, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    var respuesta = context.PROC_EDI_ROL(Convert.ToInt32(rol), descripcion, comentario, usuarioEjecucion, cia).FirstOrDefault();
                    result = respuesta?.CODIGO.ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_ROL", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public static List<PROC_OBT_PERMISOS_V2Result> GetPermisos(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_PERMISOS_V2Result> result = new List<PROC_OBT_PERMISOS_V2Result>();
                try
                {
                    result = context.PROC_OBT_PERMISOS_V2(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_PERMISOS_V2", e);
                }
                context.Connection.Close();
                return result;
            }

        }

        public static string EditarPermisos(int rol, int seguimiento, int tracking, int estadistica, int usuario, int configuracion, int pedidos, int portal,int descuento, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_PERMISOS_V2(rol, seguimiento, tracking, estadistica, usuario, configuracion,pedidos, portal, descuento, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_PERMISOS", e);
                }
                return result;
            }
        }


        public static List<VIEW_OBTENER_COBRADOR> Obtener_Lista_Cobrador(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_COBRADOR> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_COBRADOR>(cia, "VIEW_OBTENER_COBRADOR");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_COBRADORs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public static List<VIEW_OBTENER_VENDEDOR> Obtener_Lista_Vendedor(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_VENDEDOR> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_VENDEDOR>(cia, "VIEW_OBTENER_VENDEDOR");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_VENDEDORs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public static List<VIEW_OBTENER_BODEGA> Obtener_Lista_Bodega(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_BODEGA> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_BODEGA>(cia, "VIEW_OBTENER_BODEGA");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_BODEGAs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public static  List<PROC_OBT_SELECT_USUARIOSResult> GetUsuariosSelectInsertar(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                List<PROC_OBT_SELECT_USUARIOSResult> result = new List<PROC_OBT_SELECT_USUARIOSResult>();
                try
                {
                    result = context.PROC_OBT_SELECT_USUARIOS(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SELECT_USUARIOS", e);
                }
                context.Connection.Close();
                return result;
            }
        }

        public static List<PROC_OBT_INFORMACION_USUARIOResult> ObtenerInformacion_Usuario(string usuario, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                List<PROC_OBT_INFORMACION_USUARIOResult> result = new List<PROC_OBT_INFORMACION_USUARIOResult>();
                try
                {
                    result = context.PROC_OBT_INFORMACION_USUARIO(usuario,cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_INFORMACION_USUARIO", e);
                }
                context.Connection.Close();
                return result;
            }
        }

        public static List<PROC_OBT_MODULOSResult> GetModulos(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_MODULOSResult> result = new List<PROC_OBT_MODULOSResult>();
                try
                {
                    result = context.PROC_OBT_MODULOS(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_MODULOS", e);
                }
                return result;
            }
        }

        public static List<PROC_OBT_SUBMODULOSResult> GetSubmodulos(int modulo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBMODULOSResult> result = new List<PROC_OBT_SUBMODULOSResult>();
                try
                {
                    result = context.PROC_OBT_SUBMODULOS(modulo, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBMODULOS", e);
                }
                return result;
            }
        }

        public static List<PROC_OBT_PERMISOS_X_SUBMODULOResult> GetPermisosSubModulos(int modulo, int rol, int tab, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_PERMISOS_X_SUBMODULOResult> result = new List<PROC_OBT_PERMISOS_X_SUBMODULOResult>();
                try
                {
                    result = context.PROC_OBT_PERMISOS_X_SUBMODULO(modulo, rol, tab, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_PERMISOS_X_SUBMODULO", e);
                }
                return result;
            }
        }

        

        public static string EditarPermisosV2(int rol, int modulo, int submodulo, string permisos, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_EDI_PERMISOS_VERSION_NUEVA(rol, modulo, submodulo, permisos, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_EDI_PERMISOS_VERSION_NUEVA", e);
                }
                return result;
            }
        }

    }
}
