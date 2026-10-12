using Plantilla.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plantilla.core.Manager
{
    public class LoginManager : Manager
    {
        public static string ValidarUsuarioSoftland(string usuario, string contrasena, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
                try
                {
                    string result = context.PROC_VAL_USUARIO_SOFTLAND(usuario, contrasena, cia).First().VALIDO.ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_VAL_USUARIO_SOFTLAND", e);
                    return null;
                }
        }

        public static string ValidarUsuarioSistema(string usuario, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
                try
                {
                    string result = context.PROC_OBT_USUARIO(usuario, cia).First().ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_USUARIO", e);
                    return null;
                }
        }


        public static PROC_OBT_USUARIOResult ValidarUsuarioActivo(string usuario, string cia)
        {
            PROC_OBT_USUARIOResult result = new PROC_OBT_USUARIOResult();
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
                try
            {
                result = context.PROC_OBT_USUARIO(usuario, cia).First();
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                e = e.Replace(Environment.NewLine, " ");
                e = e.Replace('"', ' ');
                e = e.Replace("'", " ");
                WriteLog("PROC_OBT_USUARIO", e);
            }
            return result;
        }


        public string RegistrarUsuario(string cedula, string nombre, string usuario, string email, string pass, string vendedor, string cobrador, string bodega,int rol, string cia)
        {

            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {

                    result = context.PROC_INSERTAR_USUARIO(cedula, nombre, usuario, email, pass, vendedor, cobrador, bodega,rol,cia).ToString();
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


        public PROC_VALIDA_EXISTENCIA_USUARIOResult ValidaExistenciaUsuario(string usuario, string correo, string identificacion, string cia)
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

        public static PROC_OBT_PERMISOS_ROL_V2Result ValidarPermisosRol(int rol, string cia)
        {
            PROC_OBT_PERMISOS_ROL_V2Result result = new PROC_OBT_PERMISOS_ROL_V2Result();
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
                try
            {
                result = context.PROC_OBT_PERMISOS_ROL_V2(rol, cia).First();
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                e = e.Replace(Environment.NewLine, " ");
                e = e.Replace('"', ' ');
                e = e.Replace("'", " ");
                WriteLog("PROC_OBT_PERMISOS_ROL", e);
            }
            return result;
        }

        public static List<PROC_OBT_PERMISOS_ROL_MODULOResult> ValidarPermisosRol_Modulo(int rol, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_PERMISOS_ROL_MODULOResult> result;
                try
                {
                    result = context.PROC_OBT_PERMISOS_ROL_MODULO(rol, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_PERMISOS_ROL_MODULO", e);
                    return null;
                }
                context.Connection.Close();
                return result;
            }
        }


    }
}
