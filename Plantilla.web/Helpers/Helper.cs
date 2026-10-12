using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Configuration;
using System.Net.Mime;

namespace Plantilla.web.Helpers
{
    public class Helper
    {
        public string SubirAdjuntos(HttpPostedFileBase archivo, string subRuta, string identificador)
        {

            if (archivo.ContentLength == 0)
            {
                return null;
            }

            string ruta;
            string directorio = HttpContext.Current.Server.MapPath(subRuta);
            try
            {
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }
                ruta = Path.Combine(directorio, Path.GetFileName($"{DateTime.Now.ToString("yyyyMMddHHmmss")}-{identificador}_{archivo.FileName}"));
                archivo.SaveAs(ruta);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }

            return ruta;
        }


        public static string Encrypt(string texto)
        {
            try
            {
                string result;
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("ddMMyyyy"));
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyyMMdd"));
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(texto);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    result = Convert.ToBase64String(ms.ToArray());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public static string Decrypt(string texto)
        {
            try
            {
                string result;
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("ddMMyyyy"));
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyyMMdd"));
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[texto.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(texto.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    result = encoding.GetString(ms.ToArray());
                }
                return result;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }


    }

    public class SmtpMailHelper
    {
        public static bool EnviarCorreo(string correosDestino, string asunto, string cuerpoHtml, string rutaAdjunto, out string error)
        {
            error = string.Empty;
            try
            {
                var emailFrom = ConfigurationManager.AppSettings["SmtpEmailFrom"] ?? string.Empty;
                var password = ConfigurationManager.AppSettings["SmtpPassword"] ?? string.Empty;
                var displayName = ConfigurationManager.AppSettings["SmtpDisplayName"] ?? string.Empty;
                var host = ConfigurationManager.AppSettings["SmtpHost"] ?? string.Empty;
                var portText = ConfigurationManager.AppSettings["SmtpPort"] ?? "25";
                var sslText = ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true";

                int port;
                if (!int.TryParse(portText, out port))
                {
                    port = 25;
                }

                bool enableSsl;
                if (!bool.TryParse(sslText, out enableSsl))
                {
                    enableSsl = true;
                }

                using (var msg = new MailMessage())
                {
                    msg.From = new MailAddress(emailFrom, displayName);
                    foreach (var correo in (correosDestino ?? string.Empty).Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        msg.To.Add(correo.Trim());
                    }

                    msg.Subject = asunto ?? string.Empty;
                    msg.SubjectEncoding = Encoding.UTF8;
                    msg.HeadersEncoding = Encoding.UTF8;
                    msg.BodyEncoding = Encoding.UTF8;
                    msg.Body = cuerpoHtml ?? string.Empty;
                    msg.IsBodyHtml = true;
                    msg.AlternateViews.Add(
                        AlternateView.CreateAlternateViewFromString(
                            cuerpoHtml ?? string.Empty,
                            Encoding.UTF8,
                            MediaTypeNames.Text.Html));

                    if (string.IsNullOrWhiteSpace(rutaAdjunto))
                    {
                        error = "Ruta de adjunto vacía.";
                        return false;
                    }

                    if (!File.Exists(rutaAdjunto))
                    {
                        error = "No se encontró el adjunto en ruta: " + rutaAdjunto;
                        return false;
                    }

                    var attachment = new Attachment(rutaAdjunto, MediaTypeNames.Application.Pdf);
                    attachment.Name = Path.GetFileName(rutaAdjunto);
                    msg.Attachments.Add(attachment);

                    using (var smtp = new SmtpClient(host, port))
                    {
                        smtp.EnableSsl = enableSsl;
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.Send(msg);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return false;
            }
        }
    }
}
