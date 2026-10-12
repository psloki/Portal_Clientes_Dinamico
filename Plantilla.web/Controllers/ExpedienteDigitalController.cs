using Plantilla.core.Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    public class ExpedienteDigitalController : Controller
    {
        readonly ExpedienteDigitalManager ExpedienteM = new ExpedienteDigitalManager();
        // GET: ExpedienteDigital
        public ActionResult Index(string solicitud)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            string usuario = (Session["Rol"].ToString() == "1") ? "0" : (Session["Usuario"]?.ToString() ?? ConfigurationManager.AppSettings["UsuarioDefault"].ToString());
            string solicitud_oc = solicitud != null ? solicitud : "0";
            int rol = Convert.ToInt32(Session["Rol"]?.ToString());
            ViewBag.Tipos = ExpedienteM.GetTiposExpedienteDigitalPP(cia);
            ViewBag.Proveedores_Cliente = null;//ExpedienteM.GetProveedorCliente("0", cia);
            ViewBag.Ordenes = null; //OrdenCompra2Manager.ObtenerOrdenesCompraFiltro(cia);
            ViewBag.Cotizaciones = null; //CotizacionM.GetCotizacionesFiltro(usuario,rol,cia);
            ViewBag.CotizacionesLineas = null; // CotizacionM.GetCotizacionesLineaExp("0",cia);
            ViewBag.Solicitudes = null; //SolicitudCM.GetFiltroSolicitud(usuario, rol, cia);
            ViewBag.Documentos = ExpedienteM.GetTiposExpedienteFiltro(cia);
            ViewBag.SolicitudesLinea = null; //SolicitudCM.GetFiltroSolicitudLinea(solicitud_oc, cia);
            //ViewBag.DocumentosCXC = ExpedienteM.GetDocCXC_ExpedienteDigital("0", cia);


            return View();
        }


        [HttpGet]
        public JsonResult Obtener_Cod_Cliente_Proveedor_Buscar(string term)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            var resultado = new JsonResult();
            var resul = ExpedienteM.GetProveedorCliente(term,cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        public JsonResult GetExpedienteDigitalPP(string fechaInicio, string fechaFin, string tipo, string modulo, string proveedor, string referencia)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            string usuario = (Session["Rol"]?.ToString() == "1") ? null : (Session["Usuario"]?.ToString() ?? ConfigurationManager.AppSettings["UsuarioDefault"].ToString());
            fechaInicio = string.IsNullOrEmpty(fechaInicio) || fechaInicio == "0" ? null : fechaInicio;
            fechaFin = string.IsNullOrEmpty(fechaFin) || fechaFin == "0" ? null : fechaFin;
            tipo = string.IsNullOrEmpty(tipo) || tipo == "0" ? null : tipo;
            proveedor = string.IsNullOrEmpty(proveedor) || proveedor == "0" ? null : proveedor;
            referencia = string.IsNullOrEmpty(referencia) || referencia == "0" ? null : referencia;
            modulo = string.IsNullOrEmpty(modulo) || modulo == "0" ? null : modulo;
            int rol = Convert.ToInt32(Session["Rol"].ToString());
            var resultado = new JsonResult();
            var resul = ExpedienteM.GetExpedienteDigitalPP(fechaInicio, fechaFin, tipo, modulo, proveedor, referencia, usuario, rol, cia);
            resultado.Data = resul;//Json(resul, JsonRequestBehavior.AllowGet);
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        [HttpPost]
        public JsonResult GuardarAdjuntos(string tipo, string proveedor, string documento, string descripcion, string adjunto, string referencia, string referenciaLinea, int subtipo)
        {
            string[] formulario = new string[5] { tipo, null, proveedor, referencia, referenciaLinea };
            int lineaReferencia = referenciaLinea != null || referenciaLinea != "" ? Convert.ToInt32(referenciaLinea) : 0;
            if (formulario.Contains(""))
            {
                return Json(2); //Todos los campos son obligatorios
            }
            if (adjunto == "")
            {
                return Json(3); //Debe adjutar un archivo
            }

            string directorioTemporal = $"~/Doc/Expediente/Temp/{adjunto}";
            string directorio = $"~/Doc/Expediente/{proveedor}/{referencia}/";
            directorioTemporal = Server.MapPath(directorioTemporal);
            directorio = Server.MapPath(directorio);

            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            string usuario = Session["Usuario"].ToString();
            int result;
            try
            {
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }
                string archivoTemporal = Directory.GetFiles(directorioTemporal)[0];
                FileInfo fi = new FileInfo(archivoTemporal);
                string nuevoArchivo = $"{directorio}{fi.Name}";

                if (System.IO.File.Exists(nuevoArchivo))
                {
                    // Si el archivo existe, renómbralo con un GUID o timestamp
                    string nombreSinExtension = Path.GetFileNameWithoutExtension(fi.Name);
                    string extension = fi.Extension;
                    string nombreConTimestamp = $"{nombreSinExtension}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";
                    nuevoArchivo = Path.Combine(directorio, nombreConTimestamp);
                }
                string rutaAdjunto = $"/Doc/Expediente/{proveedor}/{referencia}/{Path.GetFileName(nuevoArchivo)}";
                //string rutaAdjunto = $"/Doc/Expediente/{proveedor}/{referencia}/{fi.Name}";
                // Verifica si el archivo ya existe en el directorio destino
                //if (System.IO.File.Exists(nuevoArchivo))
                //{
                //    // Si existe,
                //    System.IO.File.Delete(nuevoArchivo); // Elimina el archivo existente
                //}
                System.IO.File.Move(archivoTemporal, nuevoArchivo);
                fi = new FileInfo(nuevoArchivo);
                result = ExpedienteM.InsertartExpediente(documento, tipo, referencia, lineaReferencia, proveedor, fi.Name, rutaAdjunto, fi.Extension, usuario, subtipo, descripcion, cia);
            }
            catch (Exception ex)
            {
                Manager.WriteLog("GuardarAdjuntos", ex.Message);
                result = 0;
            }
            return Json(result);
        }

        public JsonResult obtenerlineaSolicitudExp(string solicitud)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            var resultado = new JsonResult();
            var resul = "";//SolicitudCM.GetFiltroSolicitudLinea(solicitud, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        public JsonResult obtenerlineaOrdenCompraExp(string ordencompra)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            var resultado = new JsonResult();
            var resul = "";//CotizacionM.GetOrdenCompraLineaExp(ordencompra, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        public JsonResult obtenerlineaCotizacionExp(string cotizacion)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            var resultado = new JsonResult();
            var resul = ""; // CotizacionM.GetCotizacionesLineaExp(cotizacion, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        //public JsonResult obtenerProveedorlineaExp(string solicitud, string linea)
        //{
        //    string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
        //    int lineasoicitud = (linea != null || linea != "" ? Convert.ToInt32(linea) : 0);
        //    var resultado = new JsonResult();
        //    var resul = ExpedienteM.GetProveedorSolicitudLinea(solicitud, lineasoicitud, cia);
        //    resultado.Data = resul;
        //    resultado.MaxJsonLength = int.MaxValue;
        //    resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return resultado;
        //}

        public JsonResult obtenerProveedorExp()
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            var resultado = new JsonResult();
            var resul = ExpedienteM.GetProveedorExpediente(cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        public JsonResult obtenerProveedorlineaCotizacionExp(string cotizacion, string linea)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            int lineacotizacion = linea != null || linea != "" ? Convert.ToInt32(linea) : 0;
            var resultado = new JsonResult();
            var resul = "";//ExpedienteM.GetProveedorCotizacionLinea(cotizacion, lineacotizacion, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        public JsonResult EliminarDocumento(string documento)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            //TODO: Eliminar el archivo
            int result = ExpedienteM.EliminarDocumento(documento, cia);
            return Json(result);
        }

        [HttpPost]
        public JsonResult SubirArchivos(string id)
        {
            SubirArchivoTemporal(id);
            return Json(id);
        }

        private string SubirArchivoTemporal(string directorio)
        {
            string ruta = null;
            // Se crea la ruta para la solicitud
            string subRuta = string.Format("~/Doc/Expediente/Temp/{0}", directorio);
            directorio = Server.MapPath(subRuta);
            // Se verifica si se enviaron archivos
            if (Request.Files != null && Request.Files.Count > 0)
            {
                // Se obtiene el archivo
                HttpPostedFileBase archivo = Request.Files[0];
                try
                {
                    // Valida si la carpeta ya existe
                    if (!Directory.Exists(directorio))
                    {
                        //se crea una nueva carpeta
                        Directory.CreateDirectory(directorio);
                    }
                    //se crea la ruta para el archivo
                    ruta = Path.Combine(directorio, Path.GetFileName(archivo.FileName));
                    //Se guarda el archivo
                    archivo.SaveAs(ruta);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);

                    Manager.WriteLog("SubirArchivoTemporal", ex.Message);
                    return null;
                }
            }
            return ruta;
        }

        [HttpPost]
        public JsonResult obtenerSubtipoModulo(string modulo, string tipoDoc, int subtipo)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            var resultado = new JsonResult();
            var resul = ExpedienteM.GetSubTipoModulo(modulo, tipoDoc, subtipo, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        public JsonResult obtenerProveedor_ClienteExp(string opcion)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            var resultado = new JsonResult();
            var resul = ExpedienteM.GetProveedorCliente(opcion, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        public JsonResult obtenerProveedor_PorTipoModulo(string opcion)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            var resultado = new JsonResult();
            var resul = ExpedienteM.GetProveedorPorTipoDocumento(opcion, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

        public JsonResult obtenerDocProvCLiente(string provCliente, string fechaInicio, string fechaFin)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            var resultado = new JsonResult();
            var resul = ExpedienteM.GetDocCXC_ExpedienteDigital(provCliente, fechaInicio, fechaFin, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }


        public JsonResult obtenerDocProvCLiente_TipoDoc(string provCliente, string fechaInicio, string fechaFin)
        {
            string cia = (Session["CIA"] != null ? Session["CIA"].ToString() : ConfigurationManager.AppSettings["NombreCia"].ToString());
            provCliente = (provCliente == "") ? "0" : provCliente;
            
            var resultado = new JsonResult();
            var resul = ExpedienteM.GetDocCXC_ClienteExpediente(provCliente, fechaInicio, fechaFin, cia);
            resultado.Data = resul;
            resultado.MaxJsonLength = int.MaxValue;
            resultado.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resultado;
        }

    }
}