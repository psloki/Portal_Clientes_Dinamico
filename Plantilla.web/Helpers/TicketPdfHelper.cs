using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.IO.Font.Constants;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.IO;
using System.Web;

namespace Plantilla.web.Helpers
{
    public static class TicketPdfHelper
    {
        public static byte[] RenderTicketPdf(string logoPath, string tiquete, int? anchoCaracteres = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);

                const float defaultPageWidth = 226f;
                const float defaultFontSize = 9f;
                const float marginHorizontal = 10f;
                var fontSize = defaultFontSize;
                var pageWidth = defaultPageWidth;

                if (anchoCaracteres.HasValue && anchoCaracteres.Value > 0)
                {
                    var contentWidth = anchoCaracteres.Value * fontSize * 0.60f;
                    pageWidth = Math.Max(defaultPageWidth, contentWidth + (marginHorizontal * 2));
                }

                PageSize pageSize = new PageSize(pageWidth, 1000);

                Document document = new Document(pdf, pageSize);
                document.SetMargins(10, marginHorizontal, 10, marginHorizontal);

                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER);
                AgregarLogo(document, logoPath);
                AgregarTextoTiquete(document, tiquete, font, fontSize);

                document.Close();

                return ms.ToArray();
            }
        }

        private static void AgregarLogo(Document document, string logoPath)
        {
            var rutaLogo = ResolverRutaLogo(logoPath);
            if (string.IsNullOrWhiteSpace(rutaLogo) || !File.Exists(rutaLogo))
                return;

            ImageData logoData = ImageDataFactory.Create(rutaLogo);

            Image logo = new Image(logoData)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .ScaleToFit(180, 80);

            document.Add(logo);
        }

        private static string ResolverRutaLogo(string logoPath)
        {
            if (string.IsNullOrWhiteSpace(logoPath))
                return null;

            if (System.IO.Path.IsPathRooted(logoPath))
                return logoPath;

            if (HttpContext.Current?.Server == null)
                return logoPath;

            return HttpContext.Current.Server.MapPath(logoPath);
        }

        private static void AgregarTextoTiquete(Document document, string tiquete, PdfFont font, float fontSize)
        {
            var lineas = (tiquete ?? string.Empty).Replace("\r\n", "\n").Split('\n');

            foreach (var lineaOriginal in lineas)
            {
                var texto = lineaOriginal ?? string.Empty;
                var izquierda = texto.StartsWith("<CC>", StringComparison.Ordinal);
                var centrado = !izquierda && texto.Length > texto.TrimStart(' ').Length;
                texto = texto.Replace("<CC>", "");

                if (string.IsNullOrEmpty(texto))
                {
                    texto = "\u00A0";
                }
                else if (centrado)
                {
                    texto = texto.Trim();
                }

                Paragraph p = new Paragraph(texto)
                    .SetFont(font)
                    .SetFontSize(fontSize)
                    .SetFixedLeading(fontSize + 2)
                    .SetMargin(0)
                    .SetTextAlignment(centrado ? TextAlignment.CENTER : TextAlignment.LEFT);

                if (izquierda)
                {
                    p.SetMarginLeft(15);
                }

                document.Add(p);
            }
        }
    }
}
