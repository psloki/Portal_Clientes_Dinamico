using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Plantilla.web.Util
{
    public sealed class ArchivoGuardado
    {
        public string VirtualPath { get; set; }
        public string PhysicalPath { get; set; }
        public string FileName { get; set; }
        public long SizeBytes { get; set; }
    }

    public static class ComprobantePagoUtil
    {
        private static readonly string[] Allowed = { ".jpg", ".jpeg", ".png", ".pdf" };
        private const long MaxBytes = 10L * 1024 * 1024; // 10MB

        /// <summary>
        /// Guarda N archivos en:
        /// ~/Uploads/Comprobantes/{CIA}/{yyyyMM}/{Referencia}/comprobante_pago_{Referencia}.ext
        /// Si existe, agrega sufijo -2, -3, ...
        /// </summary>
        public static IList<ArchivoGuardado> Guardar(
            HttpServerUtilityBase server,
            IEnumerable<HttpPostedFileBase> archivos,
            DateTime fechaDoc,
            string referencia,
            string cia = "DIS86")
        {
            var result = new List<ArchivoGuardado>();

            var files = (archivos ?? Enumerable.Empty<HttpPostedFileBase>())
                .Where(f => f != null && f.ContentLength > 0)
                .Where(f =>
                {
                    var ext = (Path.GetExtension(f.FileName) ?? string.Empty).ToLowerInvariant();
                    return Allowed.Contains(ext) && f.ContentLength <= MaxBytes;
                })
                .ToList();

            if (files.Count == 0) return result;

            var rawRef = (referencia ?? string.Empty).Trim();
            var safeRef = Regex.Replace(rawRef, @"[^A-Za-z0-9_\-]", string.Empty);
            if (string.IsNullOrEmpty(safeRef)) safeRef = "SIN_DOC";

            var ciaSafe = string.IsNullOrWhiteSpace(cia) ? "DIS86" : cia.Trim();

            // Esquema corto por mes (yyyyMM) + referencia
            var baseVirtual = $"~/Uploads/Comprobantes/{ciaSafe}/{fechaDoc:yyyyMM}/{safeRef}";
            var basePhysical = server.MapPath(baseVirtual);
            Directory.CreateDirectory(basePhysical);

            foreach (var file in files)
            {
                var ext = (Path.GetExtension(file.FileName) ?? string.Empty).ToLowerInvariant();
                var baseName = $"comprobante_pago_{safeRef}";
                var finalName = baseName + ext;
                var phys = Path.Combine(basePhysical, finalName);

                int n = 2;
                while (System.IO.File.Exists(phys))
                {
                    finalName = $"{baseName}-{n}{ext}";
                    phys = Path.Combine(basePhysical, finalName);
                    n++;
                }

                file.SaveAs(phys);

                result.Add(new ArchivoGuardado
                {
                    FileName = finalName,
                    PhysicalPath = phys,
                    VirtualPath = VirtualPathUtility.ToAbsolute($"{baseVirtual}/{finalName}"),
                    SizeBytes = file.ContentLength
                });
            }

            return result;
        }
    }
}
