using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using Plantilla.core.Entities;
using Plantilla.core.Manager;
namespace Plantilla.web.Controllers

{
    public class ExcelController : Controller
    {

        
        // GET: Excel
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No se ha subido ningún archivo.");

            var dataTable = new DataTable();
            using (var stream = new MemoryStream())
            {
                excelFile.InputStream.CopyTo(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed();

                    foreach (var row in rows)
                    {
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (var cell in row.Cells())
                            {
                                dataTable.Columns.Add(cell.Value.ToString());
                            }
                        }
                        else
                        {
                            dataTable.Rows.Add();
                            int i = 0;
                            foreach (var cell in row.Cells())
                            {
                                dataTable.Rows[dataTable.Rows.Count - 1][i++] = cell.Value;
                            }
                        }
                    }
                }
            }

            _ = UpdateDatabase(dataTable);
            return Content("Archivo subido y procesado exitosamente.");
        }

        private async Task UpdateDatabase(DataTable dataTable)
        {
            using (var context =  new PlantillaDataContext(Manager.GetConnectionString()))
            {
                var connectionString = context.Connection.ConnectionString;

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var commandText = "UPDATE ArticulosPBA " +
                                           "SET Precio = @Precio " +
                                          "WHERE Articulo = @Articulo AND Articulo_Precio = @Articulo_Precio";
                        using (var command = new SqlCommand(commandText, connection))
                        {
                            command.Parameters.AddWithValue("@Articulo", row["Articulo"]);
                            command.Parameters.AddWithValue("@Articulo_Precio", row["Articulo_Precio"]);
                            command.Parameters.AddWithValue("@Precio", row["Precio"]);
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }



    }
}