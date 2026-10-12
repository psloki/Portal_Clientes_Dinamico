using System.Collections.Generic;
using System.Linq;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Web;
using Plantilla.core.Manager;
using ProdusoftLicenseValidation;

namespace Plantilla.web.Util
{
    public class LicenseValidation : Manager
    {
        // 0 = válida; 5 = falta configuración/archivo;
        // 6 = no se pudo validar; 7 = vencida.
        public int GetStatus()
        {
            try
            {
                string licenseSetting =
                    ConfigurationManager.AppSettings["license"];
                string certificateSetting =
                    ConfigurationManager.AppSettings["certificate"];
                string key =
                    ConfigurationManager.AppSettings["licenseKey"];

                if (string.IsNullOrWhiteSpace(licenseSetting) ||
                    string.IsNullOrWhiteSpace(certificateSetting) ||
                    string.IsNullOrWhiteSpace(key))
                {
                    SetConnectionString(null);
                    return 5;
                }

                string licensePath =
                    HttpContext.Current.Server.MapPath(licenseSetting);
                string certificatePath =
                    HttpContext.Current.Server.MapPath(certificateSetting);

                if (!File.Exists(licensePath) ||
                    !File.Exists(certificatePath))
                {
                    SetConnectionString(null);
                    return 5;
                }

                var license = Validator.Validate(
                    certificatePath, key, licensePath);

                if (license == null)
                    throw new InvalidDataException("Licencia vacía.");

                // El código del video interpreta timeToLive como
                // milisegundos desde 1970-01-01 UTC.
                DateTime expiresUtc = new DateTime(
                    1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .AddMilliseconds(license.timeToLive);

                // El video agrega 14 días de gracia.
                if (expiresUtc.AddDays(14) < DateTime.UtcNow)
                {
                    SetConnectionString(null);
                    return 7;
                }

                if (string.IsNullOrWhiteSpace(license.connectionString))
                    throw new InvalidDataException(
                        "La licencia no contiene conexión.");

                SetConnectionString(license.connectionString);
                return 0;
            }
            catch (Exception ex)
            {
                SetConnectionString(null);
                // No registrar la llave ni la conexión con contraseña.
                Trace.TraceError("Validación de licencia: {0}",
                    ex.GetType().FullName);
                return 6;
            }
        }
    }
}