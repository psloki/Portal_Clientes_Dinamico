using ClosedXML.Excel;
using Plantilla.core.Manager;
using Plantilla.web.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Plantilla.web.Controllers
{
    [PermisosModulo(Modulo = 8, SubModulo = 21,
                    SesKey = "PERMISO_MOD_COBRADOR",
                    PermisoNecesario = "4",
                    CampoSesionForzar = "Cod_Cobrador")]
    [PermisosModulo(Modulo = 8, SubModulo = 21,
                    SesKey = "PERMISO_MOD_VENDEDOR",
                    PermisoNecesario = "5",
                    CampoSesionForzar = "Vendedor")]
    [PermisosModulo(Modulo = 8, SubModulo = 21,
                    SesKey = "PERMISO_MOD_CLIENTE",
                    PermisoNecesario = "6")]
    public class DescuentosController : Controller
    {
        private const string PermisoCargarExcel = "80";
        private const string PermisoEditar = "81";
        private const string PermisoEditarMasivo = "82";
        private const string PermisoEliminar = "83";

        private static readonly string[] ColumnasExcelRequeridas =
        {
            "NIVEL_PRECIO",
            "ARTICULO",
            "ESCALA_DCTO",
            "CANTIDAD_MIN",
            "CANTIDAD_MAX"
        };

        private static readonly string[] ColumnasPorcentajeExcel =
        {
            "PORC_DCTO",
            "U_PS_PORC_DCTO_ANT"
        };

        private static readonly string[] ColumnasFechaInicioExcel =
        {
            "FECHA_INICIO",
            "U_PS_FECHA_INICIO_PROY"
        };

        private static readonly string[] ColumnasFechaFinExcel =
        {
            "FECHA_FIN",
            "U_PS_FECHA_FIN_PROY"
        };

        private readonly DescuentosManager descuento = new DescuentosManager();

        public ActionResult Index()
        {
            string permisosMenu = Convert.ToString(Session["PERMISOS_MENU"]);
            if (string.IsNullOrWhiteSpace(permisosMenu) || !permisosMenu.Contains("56"))
            {
                return RedirectToAction("Index", "Login");
            }

            string cia = ObtenerCia();
            ViewBag.NivelPrecioDesc = descuento.Obtener_Lista_nivelPrecioDesc(cia);
            ViewBag.MarcaArtDesc = descuento.Obtener_Lista_MarcaDesc(cia);
            ViewBag.CasaArtDesc = descuento.Obtener_Lista_CasaDesc(cia);
            ViewBag.SubCasaArtDesc = descuento.Obtener_Lista_SubCasaDesc(cia);
            return View();
        }

        [HttpGet]
        public JsonResult obtenerClientArticuloDctoIndex(
            string nivelPrecio,
            string articulo,
            string casa,
            string subcasa)
        {
            string cia = ObtenerCia();
            nivelPrecio = NormalizarFiltro(nivelPrecio);
            articulo = NormalizarFiltro(articulo);
            casa = NormalizarFiltro(casa);
            subcasa = NormalizarFiltro(subcasa);

            var resultado = new JsonResult
            {
                Data = descuento.ObtenerListaClienteArticuloDcto(
                    articulo,
                    nivelPrecio,
                    casa,
                    subcasa,
                    cia),
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return resultado;
        }

        [HttpPost]
        public JsonResult EditarPorcentajeDescuento(
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
            DateTime fechaFinProyectada)
        {
            if (!TienePermiso(PermisoEditar))
            {
                return Json(-1);
            }

            if (!EsProyeccionValida(
                    cantidadMin,
                    cantidadMax,
                    porcentajeProyectado,
                    fechaInicioProyectada,
                    fechaFinProyectada))
            {
                return Json(4);
            }

            int resultado = descuento.Editar_Porcentaje_Descuento(
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
                ObtenerUsuarioActual(),
                ObtenerCia());

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult EditarPorcentajeDescuentoMasivo(
            string nivelPrecio,
            string listaRegistros,
            int cantidadMin,
            int cantidadMax,
            decimal porcentajeProyectado,
            DateTime fechaInicioProyectada,
            DateTime fechaFinProyectada)
        {
            if (!TienePermiso(PermisoEditarMasivo))
            {
                return Json(-1);
            }

            if (string.IsNullOrWhiteSpace(listaRegistros)
                || !EsProyeccionValida(
                    cantidadMin,
                    cantidadMax,
                    porcentajeProyectado,
                    fechaInicioProyectada,
                    fechaFinProyectada))
            {
                return Json(4);
            }

            int resultado = descuento.Editar_Porcentaje_Descuento_Masivo(
                nivelPrecio,
                listaRegistros,
                cantidadMin,
                cantidadMax,
                porcentajeProyectado,
                fechaInicioProyectada,
                fechaFinProyectada,
                ObtenerUsuarioActual(),
                ObtenerCia());

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult EliminarDescuento(
            string nivelPrecio,
            string moneda,
            int version,
            string articulo,
            int escalaDcto,
            int versionDcto)
        {
            if (!TienePermiso(PermisoEliminar))
            {
                return Json(-1);
            }

            int resultado = descuento.Eliminar_Descuento(
                nivelPrecio,
                moneda,
                version,
                articulo,
                escalaDcto,
                versionDcto,
                ObtenerCia());

            return Json(resultado);
        }

        [HttpPost]
        public ActionResult UploadExcelDesc(HttpPostedFileBase excelFile)
        {
            if (!TienePermiso(PermisoCargarExcel))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "No posee permiso para cargar descuentos.");
            }

            if (excelFile == null || excelFile.ContentLength == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se ha subido ningún archivo.");
            }

            string extension = Path.GetExtension(excelFile.FileName);
            if (!string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "El cargador debe ser un archivo .xlsx.");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    excelFile.InputStream.CopyTo(stream);
                    stream.Position = 0;

                    using (var workbook = new XLWorkbook(stream))
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(1);
                        IXLRow filaEncabezado = worksheet.FirstRowUsed();

                        if (filaEncabezado == null)
                        {
                            return Json(new
                            {
                                success = false,
                                procesados = 0,
                                omitidos = 0,
                                errores = new[] { "El archivo no contiene encabezados." }
                            });
                        }

                        Dictionary<string, int> columnas = filaEncabezado.CellsUsed()
                            .GroupBy(c => c.Value.ToString().Trim().ToUpperInvariant())
                            .ToDictionary(g => g.Key, g => g.First().Address.ColumnNumber);

                        string[] faltantes = ColumnasExcelRequeridas
                            .Where(c => !columnas.ContainsKey(c))
                            .ToArray();

                        var gruposFaltantes = new List<string>();
                        if (!ContieneAlgunaColumna(columnas, ColumnasPorcentajeExcel))
                        {
                            gruposFaltantes.Add("PORC_DCTO");
                        }

                        if (!ContieneAlgunaColumna(columnas, ColumnasFechaInicioExcel))
                        {
                            gruposFaltantes.Add("FECHA_INICIO");
                        }

                        if (!ContieneAlgunaColumna(columnas, ColumnasFechaFinExcel))
                        {
                            gruposFaltantes.Add("FECHA_FIN");
                        }

                        if (faltantes.Any() || gruposFaltantes.Any())
                        {
                            string[] columnasFaltantes = faltantes
                                .Concat(gruposFaltantes)
                                .ToArray();

                            return Json(new
                            {
                                success = false,
                                procesados = 0,
                                omitidos = 0,
                                errores = new[]
                                {
                                    "Faltan las columnas: " + string.Join(", ", columnasFaltantes)
                                }
                            });
                        }

                        return ProcesarFilasExcel(
                            worksheet,
                            filaEncabezado.RowNumber(),
                            columnas,
                            ObtenerCia(),
                            ObtenerUsuarioActual());
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    procesados = 0,
                    omitidos = 0,
                    errores = new[] { "No se pudo procesar el archivo: " + ex.Message }
                });
            }
        }

        private JsonResult ProcesarFilasExcel(
            IXLWorksheet worksheet,
            int numeroFilaEncabezado,
            IDictionary<string, int> columnas,
            string cia,
            string usuario)
        {
            var errores = new List<string>();
            int procesados = 0;
            int omitidos = 0;

            foreach (IXLRow row in worksheet.RowsUsed().Where(r => r.RowNumber() > numeroFilaEncabezado))
            {
                int numeroFila = row.RowNumber();
                string porcentajeTexto = ObtenerTextoPrimeroDisponible(
                    row,
                    columnas,
                    ColumnasPorcentajeExcel);
                string fechaInicioTexto = ObtenerTextoPrimeroDisponible(
                    row,
                    columnas,
                    ColumnasFechaInicioExcel);
                string fechaFinTexto = ObtenerTextoPrimeroDisponible(
                    row,
                    columnas,
                    ColumnasFechaFinExcel);

                if (string.IsNullOrWhiteSpace(porcentajeTexto)
                    && string.IsNullOrWhiteSpace(fechaInicioTexto)
                    && string.IsNullOrWhiteSpace(fechaFinTexto))
                {
                    omitidos++;
                    continue;
                }

                string nivelPrecio = ObtenerTexto(row, columnas, "NIVEL_PRECIO");
                string articulo = ObtenerTexto(row, columnas, "ARTICULO");

                if (!TryObtenerEnteroOpcional(row, columnas, "VERSION", 1, out int version)
                    || !TryObtenerEntero(row, columnas, "ESCALA_DCTO", out int escala)
                    || !TryObtenerEnteroOpcional(row, columnas, "VERSION_DCTO", 1, out int versionDcto)
                    || !TryObtenerEntero(row, columnas, "CANTIDAD_MIN", out int cantidadMin)
                    || !TryObtenerEntero(row, columnas, "CANTIDAD_MAX", out int cantidadMax)
                    || !TryObtenerDecimal(porcentajeTexto, out decimal porcentajeProyectado)
                    || !TryObtenerFecha(fechaInicioTexto, out DateTime fechaInicioProyectada)
                    || !TryObtenerFecha(fechaFinTexto, out DateTime fechaFinProyectada)
                    || string.IsNullOrWhiteSpace(nivelPrecio)
                    || string.IsNullOrWhiteSpace(articulo)
                    || version <= 0
                    || escala <= 0
                    || versionDcto <= 0
                    || !EsProyeccionValida(
                        cantidadMin,
                        cantidadMax,
                        porcentajeProyectado,
                        fechaInicioProyectada,
                        fechaFinProyectada))
                {
                    errores.Add("Fila " + numeroFila + ": contiene datos inválidos o incompletos.");
                    continue;
                }

                string moneda = ObtenerTextoOpcional(row, columnas, "MONEDA", "L");
                if (string.IsNullOrWhiteSpace(moneda))
                {
                    errores.Add("Fila " + numeroFila + ": la moneda no puede estar vacía.");
                    continue;
                }

                int resultado = descuento.Actualizar_Descuento_Excel(
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

                switch (resultado)
                {
                    case 1:
                        procesados++;
                        break;
                    case 2:
                        errores.Add("Fila " + numeroFila + ": el registro no existe en ESCALA_DCTO.");
                        break;
                    case 3:
                        errores.Add("Fila " + numeroFila + ": el registro ya tiene una proyección pendiente o activa.");
                        break;
                    case 4:
                        errores.Add("Fila " + numeroFila + ": la proyección no cumple las validaciones.");
                        break;
                    default:
                        errores.Add("Fila " + numeroFila + ": ocurrió un error al actualizar el registro.");
                        break;
                }
            }

            return Json(new
            {
                success = errores.Count == 0,
                procesados,
                omitidos,
                errores = errores.Take(50).ToArray(),
                totalErrores = errores.Count
            });
        }

        private static string ObtenerTexto(
            IXLRow row,
            IDictionary<string, int> columnas,
            string nombreColumna)
        {
            return row.Cell(columnas[nombreColumna]).Value.ToString().Trim();
        }

        private static string ObtenerTextoOpcional(
            IXLRow row,
            IDictionary<string, int> columnas,
            string nombreColumna,
            string valorPredeterminado)
        {
            if (!columnas.ContainsKey(nombreColumna))
            {
                return valorPredeterminado;
            }

            string valor = ObtenerTexto(row, columnas, nombreColumna);
            return string.IsNullOrWhiteSpace(valor) ? valorPredeterminado : valor;
        }

        private static string ObtenerTextoPrimeroDisponible(
            IXLRow row,
            IDictionary<string, int> columnas,
            IEnumerable<string> nombresColumnas)
        {
            string nombre = nombresColumnas.First(columnas.ContainsKey);
            return ObtenerTexto(row, columnas, nombre);
        }

        private static bool ContieneAlgunaColumna(
            IDictionary<string, int> columnas,
            IEnumerable<string> nombresColumnas)
        {
            return nombresColumnas.Any(columnas.ContainsKey);
        }

        private static bool TryObtenerEntero(
            IXLRow row,
            IDictionary<string, int> columnas,
            string nombreColumna,
            out int valor)
        {
            string texto = ObtenerTexto(row, columnas, nombreColumna);
            return int.TryParse(texto, NumberStyles.Integer, CultureInfo.InvariantCulture, out valor)
                || int.TryParse(texto, NumberStyles.Integer, CultureInfo.CurrentCulture, out valor);
        }

        private static bool TryObtenerEnteroOpcional(
            IXLRow row,
            IDictionary<string, int> columnas,
            string nombreColumna,
            int valorPredeterminado,
            out int valor)
        {
            if (!columnas.ContainsKey(nombreColumna))
            {
                valor = valorPredeterminado;
                return true;
            }

            string texto = ObtenerTexto(row, columnas, nombreColumna);
            if (string.IsNullOrWhiteSpace(texto))
            {
                valor = valorPredeterminado;
                return true;
            }

            return int.TryParse(texto, NumberStyles.Integer, CultureInfo.InvariantCulture, out valor)
                || int.TryParse(texto, NumberStyles.Integer, CultureInfo.CurrentCulture, out valor);
        }

        private static bool TryObtenerDecimal(string texto, out decimal valor)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                valor = 0;
                return false;
            }

            texto = texto.Trim();

            // Evita interpretar "33,33" como 3333 bajo la cultura invariante.
            if (texto.Contains(",") && !texto.Contains("."))
            {
                return decimal.TryParse(
                    texto.Replace(',', '.'),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out valor);
            }

            return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out valor)
                || decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out valor);
        }

        private static bool TryObtenerFecha(string texto, out DateTime fecha)
        {
            string[] formatos =
            {
                "yyyy-MM-dd",
                "yyyy-MM-dd HH:mm:ss",
                "dd/MM/yyyy",
                "d/M/yyyy",
                "MM/dd/yyyy",
                "M/d/yyyy"
            };

            if (DateTime.TryParseExact(
                    texto,
                    formatos,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AllowWhiteSpaces,
                    out fecha)
                || DateTime.TryParse(texto, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out fecha)
                || DateTime.TryParse(texto, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out fecha))
            {
                fecha = fecha.Date;
                return true;
            }

            if (double.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out double serialExcel))
            {
                try
                {
                    fecha = DateTime.FromOADate(serialExcel).Date;
                    return true;
                }
                catch (ArgumentException)
                {
                    // Se reportará la fila como inválida.
                }
            }

            fecha = default(DateTime);
            return false;
        }

        private static bool EsProyeccionValida(
            int cantidadMin,
            int cantidadMax,
            decimal porcentajeProyectado,
            DateTime fechaInicioProyectada,
            DateTime fechaFinProyectada)
        {
            return cantidadMin > 0
                && cantidadMax >= cantidadMin
                && porcentajeProyectado >= 0
                && porcentajeProyectado <= 100
                && fechaInicioProyectada.Date > DateTime.Today
                && fechaFinProyectada.Date > fechaInicioProyectada.Date;
        }

        private string ObtenerCia()
        {
            string cia = Convert.ToString(Session["CIA"]);
            if (string.IsNullOrWhiteSpace(cia))
                throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            return cia.Trim();
        }

        private string ObtenerUsuarioActual()
        {
            string usuario = Convert.ToString(Session["USUARIO"]);
            if (string.IsNullOrWhiteSpace(usuario))
            {
                usuario = Convert.ToString(Session["USER"]);
            }

            if (string.IsNullOrWhiteSpace(usuario)
                && User != null
                && User.Identity != null)
            {
                usuario = User.Identity.Name;
            }

            usuario = string.IsNullOrWhiteSpace(usuario) ? "sa" : usuario.Trim();
            return usuario.Length <= 50 ? usuario : usuario.Substring(0, 50);
        }

        private bool TienePermiso(string permiso)
        {
            string permisos = Convert.ToString(Session["PERMISOS_DESCUENTOS"]);
            return permisos.Split(',')
                .Select(p => p.Trim())
                .Any(p => string.Equals(p, permiso, StringComparison.Ordinal));
        }

        private static string NormalizarFiltro(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? "0" : valor.Trim();
        }
    }
}
