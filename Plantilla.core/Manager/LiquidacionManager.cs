using Plantilla.core.DTOs;
using Plantilla.core.Entities;
using Plantilla.core.ViewModels;
using Portal.core.GenericRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Plantilla.core.Manager
{
    public class LiquidacionManager : Manager
    {




        public List<VIEW_OBTENER_CLIENTE> ListaClientes(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBTENER_CLIENTE> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBTENER_CLIENTE>(cia, "VIEW_OBTENER_CLIENTES");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBTENER_CLIENTEs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public static PROC_OBT_MONEDA_X_CUENTA_FILTROResult ObtenerMonedaXCuenta(string banco, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                PROC_OBT_MONEDA_X_CUENTA_FILTROResult result = new PROC_OBT_MONEDA_X_CUENTA_FILTROResult();
                try
                {
                    result = context.PROC_OBT_MONEDA_X_CUENTA_FILTRO(banco, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_OBT_MONEDA_X_CUENTA_FILTRO", e);
                }
                return result;
            }
        }

        public static PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE_DEPResult ValidarSiExisteNumBancoTEF_DEP(string numero, string banco, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE_DEPResult result = new PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE_DEPResult();
                try
                {
                    result = context.PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE_DEP(numero, banco, cia).First();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE_DEP", e);
                }
                return result;
            }
        }

        public List<VIEW_OBTENER_COBRADOR> ListaCobrador(string cia)
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
        public List<VIEW_OBT_BANCO> ListaBancos(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBT_BANCO> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBT_BANCO>(cia, "VIEW_OBT_BANCOS");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_BANCOs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<VIEW_OBT_TIPO_DOC_UNICO> ListaTipoDoc(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBT_TIPO_DOC_UNICO> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBT_TIPO_DOC_UNICO>(cia, "VIEW_OBT_TIPO_DOC_UNICOS");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_TIPO_DOC_UNICOs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_TIPO_DOC_DETALLEResult> Obtener_FormaPago_Detalle(string tipo, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TIPO_DOC_DETALLEResult> result;
                try
                {
                    result = context.PROC_OBT_TIPO_DOC_DETALLE(tipo, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_TIPO_DOC_DETALLE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<VIEW_OBT_ESTADO_LIQ> ListaEstadoLiq(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<VIEW_OBT_ESTADO_LIQ> result;
                try
                {

                    result = context.ObtenerCatalogoCia<VIEW_OBT_ESTADO_LIQ>(cia, "VIEW_OBT_ESTADO_LIQ");
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("VIEW_OBT_ESTADO_LIQs", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CCResult> Obtener_Documentos_Recibos(string fechaInicio, string fechaFin, string cliente, string cobrador,
                                                                                         string moneda, string tipoDoc, int subTipoDoc, int estado, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CCResult> result;
                try
                {
                    result = context.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC(fechaInicio, fechaFin, cliente, cobrador, moneda, tipoDoc, subTipoDoc, estado, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CCResult", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public static PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_EFECTIVOResult Obtener_Documentos_Recibos_FILTRO_EFECTIVO(string fechaInicio, string fechaFin, string cliente, string cobrador,
                                                                                        string moneda, string tipoDoc, int subTipoDoc, int estado, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_EFECTIVOResult result = new PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_EFECTIVOResult();
                try
                {
                    result = context.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_EFECTIVO(fechaInicio, fechaFin, cliente, cobrador, moneda, tipoDoc, subTipoDoc, estado, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_EFECTIVO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public static PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUEResult Obtener_Documentos_Recibos_FILTRO_CHEQUE(string fechaInicio, string fechaFin, string cliente, string cobrador,
                                                                                       string moneda, string tipoDoc, int subTipoDoc, int estado, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUEResult result = new PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUEResult();
                try
                {
                    result = context.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUE(fechaInicio, fechaFin, cliente, cobrador, moneda, tipoDoc, subTipoDoc, estado, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public static PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUE_POSTFECHADOResult Obtener_Documentos_Recibos_FILTRO_CHEQUE_POSTFECHADO(string fechaInicio, string fechaFin, string cliente, string cobrador,
                                                                                       string moneda, string tipoDoc, int subTipoDoc, int estado, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUE_POSTFECHADOResult result = new PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUE_POSTFECHADOResult();
                try
                {
                    result = context.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUE_POSTFECHADO(fechaInicio, fechaFin, cliente, cobrador, moneda, tipoDoc, subTipoDoc, estado, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_FILTRO_CHEQUE_POSTFECHADO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_SUB_INDEXResult> Obtener_debitos_Recibos_SubIndex(string documento, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_SUB_INDEXResult> result;
                try
                {
                    result = context.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_SUB_INDEX(documento, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC_SUB_INDEX", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CCResult> GenerarExcel_RecibosPorIds(
     string ids,
     string fechaIni,
     string fechaFin,
     string cliente,
     string cobrador,
     string moneda,
     string tipoDoc,
     int subTipo,
     int estado,
     string cia
 )
        {
            var idList = ids
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x))
                .ToList();

            using (var ctx = new PlantillaDataContext())
            {
                var todos = ctx.PCL_PROC_OBT_RECIBOS_DOCUMENTOS_CC(
                    fechaIni, fechaFin,
                    cliente, cobrador,
                    moneda, tipoDoc,
                    subTipo, estado,
                    cia
                ).ToList();

                return todos
                    .Where(r => r.ID.HasValue && idList.Contains(r.ID.Value))
                    .ToList();
            }
        }


        public static List<PROC_OBT_DOCUMENTOS_INDEX_CCResult> GetDocumentosCC(
          string fechaInicio,
          string fechaFinal,
          string vendedor,
          string cobrador,
          string cliente,
          string subtipo,
          string deposito,
          string depositado,
          string cia = null)   // parámetro opcional
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                // ⚠️ En producción guarda el connection string en config/secret, no hardcodeado.
                string cnn = context.Connection.ConnectionString;

                var repo = new GenericRepositorySqlClient<PROC_OBT_DOCUMENTOS_INDEX_CCResult>(cnn);

                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@FECHA_INICIO", SqlDbType.VarChar, 25){ Value = (object)fechaInicio   ?? DBNull.Value },
                    new SqlParameter("@FECHA_FINAL",  SqlDbType.VarChar, 25){ Value = (object)fechaFinal    ?? DBNull.Value },
                    new SqlParameter("@COBRADOR",     SqlDbType.VarChar, 20) { Value = (object)cobrador     ?? "0" },
                    new SqlParameter("@VENDEDOR",     SqlDbType.VarChar, 20) { Value =(object)vendedor      ?? "0" },
                    new SqlParameter("@CLIENTE",      SqlDbType.VarChar, 20){ Value = (object)cliente       ?? "0" },
                    new SqlParameter("@SUBTIPO",      SqlDbType.VarChar, 20) { Value = (object)subtipo      ?? "0" },
                    new SqlParameter("@DEPOSITO",     SqlDbType.VarChar, 50) { Value = (object)deposito     ?? "0" },
                    new SqlParameter("@DEPOSITADO",   SqlDbType.VarChar, 1) { Value = (object)depositado    ?? "0" },
                    new SqlParameter("@CIA",          SqlDbType.VarChar, 20){ Value = cia }
                };

                var lista = repo.ExecuteProcedureSQL("GLOBAL.PROC_OBT_DOCUMENTOS_INDEX_CC_V3", parametros);

                return lista.ToList();
            }
        }

        public static List<PROC_OBT_DOCUMENTOS_TOTAL_CCResult> GetDocumentosCCTotal(
          string fechaInicio,
          string fechaFinal,
          string vendedor,
          string cobrador,
          string cliente,
          string subtipo,
          string deposito,
          string depositado,
          string cia = null)   // parámetro opcional
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                // ⚠️ En producción guarda el connection string en config/secret, no hardcodeado.
                string cnn = context.Connection.ConnectionString;

                var repo = new GenericRepositorySqlClient<PROC_OBT_DOCUMENTOS_TOTAL_CCResult>(cnn);

                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@FECHA_INICIO", SqlDbType.VarChar, 25){ Value = (object)fechaInicio   ?? DBNull.Value },
                    new SqlParameter("@FECHA_FINAL",  SqlDbType.VarChar, 25){ Value = (object)fechaFinal    ?? DBNull.Value },
                    new SqlParameter("@COBRADOR",     SqlDbType.VarChar, 20) { Value = (object)cobrador     ?? "0" },
                    new SqlParameter("@VENDEDOR",     SqlDbType.VarChar, 20) { Value =(object)vendedor      ?? "0" },
                    new SqlParameter("@CLIENTE",      SqlDbType.VarChar, 20){ Value = (object)cliente       ?? "0" },
                    new SqlParameter("@SUBTIPO",      SqlDbType.VarChar, 20) { Value = (object)subtipo      ?? "0" },
                    new SqlParameter("@DEPOSITO",     SqlDbType.VarChar, 50) { Value = (object)deposito     ?? "0" },
                    new SqlParameter("@DEPOSITADO",   SqlDbType.VarChar, 1) { Value = (object)depositado    ?? "0" },
                    new SqlParameter("@CIA",          SqlDbType.VarChar, 20){ Value = cia }
                };

                var lista = repo.ExecuteProcedureSQL("GLOBAL.PROC_OBT_DOCUMENTOS_TOTAL_CC", parametros);

                return lista.ToList();
            }
        }


        public static List<PROC_OBT_DOCUMENTOS_INDEX_CC_V2_Result> GetDocumentosCC_V2(
    string fechaInicio,
    string fechaFinal,
    string cliente,
    string moneda,
    string estado = "P",         // 'P' pendientes (igual que v1), 'L' liquidados, "" o null = todos
    string cia = null)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string cnn = context.Connection.ConnectionString;

                var repo = new GenericRepositorySqlClient<PROC_OBT_DOCUMENTOS_INDEX_CC_V2_Result>(cnn);

                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@FECHA_INICIO", SqlDbType.VarChar, 25){ Value = (object)fechaInicio ?? DBNull.Value },
                    new SqlParameter("@FECHA_FINAL",  SqlDbType.VarChar, 25){ Value = (object)fechaFinal  ?? DBNull.Value },
                    new SqlParameter("@CLIENTE",      SqlDbType.VarChar, 20){ Value = (object)cliente     ?? DBNull.Value },
                    new SqlParameter("@MONEDA",       SqlDbType.VarChar, 4) { Value = (object)moneda      ?? DBNull.Value },
                    new SqlParameter("@CIA",          SqlDbType.VarChar, 20){ Value = cia },
                };

                var lista = repo.ExecuteProcedureSQL("GLOBAL.PROC_OBT_DOCUMENTOS_INDEX_CC_V2", parametros);
                return lista.ToList();
            }
        }


        /// <summary>
        /// Inserta un movimiento bancario (nuevo documento) en Exactus via SP GLOBAL.PROC_INS_DOCUMENTO_MOV_BANCOS_NUEVO_DOC.
        /// Retorna: 1 = OK, 0 = no insertó, lanza excepción/log en error.
        /// </summary>
        /// <param name="cuentaBancaria">22 chars máx</param>
        /// <param name="usuario">50 chars máx</param>
        /// <param name="documento">50 chars máx (p.ej. número depósito/TEF)</param>
        /// <param name="monto">DEC(28,8)</param>
        /// <param name="tipo">3 chars (T/C, N/C, T/D, N/D, etc.)</param>
        /// <param name="subtipo">int (p.ej. 0, 1, etc.)</param>
        /// <param name="fechaDoc">Fecha del documento (se enviará como dd/MM/yyyy)</param>
        /// <param name="concepto">240 chars máx</param>
        /// <param name="beneficiario">150 chars máx (cliente/tercero)</param>
        /// <param name="moneda">4 chars (CRC, USD, …)</param>
        /// <param name="tipoCambioOrig">DEC(28,8) (si CRC usualmente 1)</param>
        /// <param name="montoOriginal">DEC(28,8) (monto en moneda original)</param>
        /// <param name="cia">Sigla de compañía (p.ej. DIS86)</param>
        public static int InsertarMovimientoBancarioNuevoDoc(
          string cuentaBancaria,
          string usuario,
          string documento,
          decimal monto,
          string tipo,
          int subtipo,
          DateTime fechaDoc,
          string concepto,
          string beneficiario,
          string moneda,
          decimal? tipoCambioOrig,
          decimal? montoOriginal,
          string cia)
        {
            tipo = string.IsNullOrWhiteSpace(tipo) ? "T/C" : tipo.Trim();
            moneda = (moneda ?? "CRC").Trim().ToUpperInvariant();
            var tc = tipoCambioOrig ?? (moneda == "CRC" ? 1m : 1m); // ajusta si quieres otro default para USD
            var monOrig = montoOriginal ?? monto;

            string Trunc(string s, int max) => string.IsNullOrEmpty(s) ? "" : (s.Length <= max ? s : s.Substring(0, max));

            cuentaBancaria = Trunc(cuentaBancaria, 22);
            usuario = Trunc(usuario, 50);
            documento = Trunc(documento, 50);
            tipo = Trunc(tipo, 3);
            concepto = Trunc(concepto, 240);
            beneficiario = Trunc(beneficiario, 150);
            moneda = Trunc(moneda, 4);
            cia = Trunc(cia, 30);

            var fechaStr = fechaDoc.ToString("dd/MM/yyyy"); // el SP la espera como VARCHAR dd/MM/yyyy

            string cnn = GetConnectionString();

            try
            {
                using (var conn = new SqlConnection(cnn))
                using (var cmd = new SqlCommand("GLOBAL.PROC_INS_DOCUMENTO_MOV_BANCOS_NUEVO_DOC", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 60;

                    cmd.Parameters.Add(new SqlParameter("@CUENTA_BANCARIA", SqlDbType.VarChar, 22) { Value = cuentaBancaria });
                    cmd.Parameters.Add(new SqlParameter("@USUARIO", SqlDbType.VarChar, 50) { Value = usuario });
                    cmd.Parameters.Add(new SqlParameter("@DOCUMENTO", SqlDbType.VarChar, 50) { Value = documento });

                    var pMonto = new SqlParameter("@MONTO", SqlDbType.Decimal) { Precision = 28, Scale = 8, Value = monto };
                    cmd.Parameters.Add(pMonto);

                    cmd.Parameters.Add(new SqlParameter("@TIPO", SqlDbType.VarChar, 3) { Value = tipo });
                    cmd.Parameters.Add(new SqlParameter("@SUBTIPO", SqlDbType.Int) { Value = subtipo });
                    cmd.Parameters.Add(new SqlParameter("@FECHA", SqlDbType.VarChar, 10) { Value = fechaStr });
                    cmd.Parameters.Add(new SqlParameter("@CONCEPTO", SqlDbType.VarChar, 240) { Value = concepto });
                    cmd.Parameters.Add(new SqlParameter("@BENEFICIARIO", SqlDbType.VarChar, 150) { Value = beneficiario ?? "" });
                    cmd.Parameters.Add(new SqlParameter("@MONEDA", SqlDbType.VarChar, 4) { Value = moneda });

                    var pTc = new SqlParameter("@TIPO_CAMBIO_ORIG", SqlDbType.Decimal) { Precision = 28, Scale = 8, Value = tc };
                    cmd.Parameters.Add(pTc);

                    var pMonOrig = new SqlParameter("@MONTO_ORIGINAL", SqlDbType.Decimal) { Precision = 28, Scale = 8, Value = monOrig };
                    cmd.Parameters.Add(pMonOrig);

                    cmd.Parameters.Add(new SqlParameter("@CIA", SqlDbType.VarChar, 30) { Value = cia });

                    var pReturn = new SqlParameter("@RETURN_VALUE", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(pReturn);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return (pReturn.Value is int rv) ? rv : 0; // 1=OK, 0=no insertó
                }
            }
            catch (Exception ex)
            {
                // Log y propagación
                try
                {
                    var msg = ex.Message.Replace(Environment.NewLine, " ").Replace('"', ' ').Replace("'", " ");
                    WriteLog("PROC_INS_DOCUMENTO_MOV_BANCOS_NUEVO_DOC", msg);
                }
                catch { }
                throw;
            }
        }

        private const string Cnn =
        "Data Source=192.168.12.8;" +
        "Initial Catalog=PS_PORTAL_CLIENTE;" +
        "Persist Security Info=True;" +
        "User ID=SA;" +
        "Password=B1@dm1n;" +
        "Encrypt=True;" +
        "TrustServerCertificate=True";

        public static bool CrearReciboYMovimientoBancario(
            string cliente, string documentoCC, string tipoDoc, int subtipo,
            string cuentaBancaria, string fecha_ddMMyyyy, string referencia,
            string moneda, decimal totalAbonado, string concepto,
            string img1, string img2, int idVisita, string vendedor, string cobrador,
            string usuario, string comentarioCob, string cia,
            // MOV_BANCOS
            string mov_TipoDocumento, int mov_Subtipo, string mov_Beneficiario,
            decimal? mov_TipoCambioOrig, decimal? mov_MontoOriginal,
            // JSON con las aplicaciones a facturas (opcional)
            string detalleAplicacionJson = null
        )
        {
            if (string.IsNullOrWhiteSpace(fecha_ddMMyyyy))
                throw new ArgumentException("La fecha no puede venir vacía.", nameof(fecha_ddMMyyyy));

            // 👉 Parseo seguro de la fecha que viene como dd/MM/yyyy
            var fecha = DateTime.ParseExact(fecha_ddMMyyyy, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            // Valores por defecto si vienen nulos
            if (referencia == null) referencia = string.Empty;
            if (concepto == null) concepto = string.Empty;
            if (usuario == null) usuario = "web";
            if (mov_Beneficiario == null) mov_Beneficiario = "N/D";

            moneda = (moneda ?? "CRC").Trim().ToUpperInvariant();

            // Defaults para movimiento bancario
            var tc = mov_TipoCambioOrig ?? 1m;
            var monOrig = mov_MontoOriginal ?? totalAbonado;

            // Evitar PK duplicada en MOV_BANCOS (usa referencia si hay)
            var numeroBanco = string.IsNullOrWhiteSpace(referencia) ? documentoCC : referencia;

            using (var conn = new SqlConnection(Cnn))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        // Paso 1: Insertar movimiento bancario
                        using (var cmd2 = new SqlCommand("GLOBAL.PROC_INS_DOCUMENTO_MOV_BANCOS_NUEVO_DOC", conn, tx))
                        {
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.Add(new SqlParameter("@CUENTA_BANCARIA", SqlDbType.VarChar, 22) { Value = cuentaBancaria });
                            cmd2.Parameters.Add(new SqlParameter("@USUARIO", SqlDbType.VarChar, 50) { Value = usuario });
                            cmd2.Parameters.Add(new SqlParameter("@DOCUMENTO", SqlDbType.VarChar, 50) { Value = numeroBanco });
                            cmd2.Parameters.Add(new SqlParameter("@MONTO", SqlDbType.Decimal) { Precision = 28, Scale = 8, Value = totalAbonado });
                            cmd2.Parameters.Add(new SqlParameter("@TIPO", SqlDbType.VarChar, 3) { Value = mov_TipoDocumento ?? "T/C" });
                            cmd2.Parameters.Add(new SqlParameter("@SUBTIPO", SqlDbType.Int) { Value = mov_Subtipo });

                            // 👉 La misma fecha convertida arriba
                            cmd2.Parameters.Add(new SqlParameter("@FECHA", SqlDbType.Date) { Value = fecha });

                            cmd2.Parameters.Add(new SqlParameter("@CONCEPTO", SqlDbType.VarChar, 240) { Value = concepto });
                            cmd2.Parameters.Add(new SqlParameter("@BENEFICIARIO", SqlDbType.VarChar, 150) { Value = mov_Beneficiario });
                            cmd2.Parameters.Add(new SqlParameter("@MONEDA", SqlDbType.VarChar, 4) { Value = moneda });
                            cmd2.Parameters.Add(new SqlParameter("@TIPO_CAMBIO_ORIG", SqlDbType.Decimal) { Precision = 28, Scale = 8, Value = tc });
                            cmd2.Parameters.Add(new SqlParameter("@MONTO_ORIGINAL", SqlDbType.Decimal) { Precision = 28, Scale = 8, Value = monOrig });
                            cmd2.Parameters.Add(new SqlParameter("@CIA", SqlDbType.VarChar, 30) { Value = cia });

                            var ret = new SqlParameter("@RETURN_VALUE", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
                            cmd2.Parameters.Add(ret);

                            cmd2.ExecuteNonQuery();

                            var returned = (ret.Value is int i) ? i : Convert.ToInt32(ret.Value);
                            if (returned != 1)
                                throw new Exception("PROC_INS_DOCUMENTO_MOV_BANCOS_NUEVO_DOC no devolvió 1.");
                        }

                        // Paso 2: Actualizar en DOCUMENTOS_CC vía SP V3 multi-cliente
                        using (var cmd1 = new SqlCommand("GLOBAL.PROC_INS_DOCUMENTOS_V3_MULTI", conn, tx))
                        {
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.Add(new SqlParameter("@CLIENTE", SqlDbType.VarChar, 20) { Value = (object)cliente ?? "0" });
                            cmd1.Parameters.Add(new SqlParameter("@DOCUMENTO", SqlDbType.VarChar, 50) { Value = documentoCC });
                            cmd1.Parameters.Add(new SqlParameter("@TIPO", SqlDbType.VarChar, 3) { Value = tipoDoc });
                            cmd1.Parameters.Add(new SqlParameter("@SUBTIPO", SqlDbType.Int) { Value = subtipo });
                            cmd1.Parameters.Add(new SqlParameter("@CUENTA_BANCARIA", SqlDbType.VarChar, 22) { Value = cuentaBancaria });

                            // 👉 Ahora la fecha se pasa como Date
                            cmd1.Parameters.Add(new SqlParameter("@FECHA", SqlDbType.Date) { Value = fecha });

                            cmd1.Parameters.Add(new SqlParameter("@REFERENCIA", SqlDbType.VarChar, 50) { Value = referencia });
                            cmd1.Parameters.Add(new SqlParameter("@MONEDA", SqlDbType.VarChar, 4) { Value = moneda });
                            cmd1.Parameters.Add(new SqlParameter("@TOTAL_ABONADO", SqlDbType.Decimal) { Precision = 28, Scale = 8, Value = totalAbonado });
                            cmd1.Parameters.Add(new SqlParameter("@CONCEPTO", SqlDbType.VarChar, 240) { Value = concepto });
                            cmd1.Parameters.Add(new SqlParameter("@IMG_COMPROBANTE1", SqlDbType.VarChar, 300) { Value = (object)img1 ?? DBNull.Value });
                            cmd1.Parameters.Add(new SqlParameter("@IMG_COMPROBANTE2", SqlDbType.VarChar, 300) { Value = (object)img2 ?? DBNull.Value });
                            cmd1.Parameters.Add(new SqlParameter("@ID_VISITA", SqlDbType.Int) { Value = idVisita });
                            cmd1.Parameters.Add(new SqlParameter("@VENDEDOR", SqlDbType.VarChar, 20) { Value = (object)vendedor ?? string.Empty });
                            cmd1.Parameters.Add(new SqlParameter("@COBRADOR", SqlDbType.VarChar, 20) { Value = (object)cobrador ?? string.Empty });
                            cmd1.Parameters.Add(new SqlParameter("@USUARIO", SqlDbType.VarChar, 50) { Value = usuario });
                            cmd1.Parameters.Add(new SqlParameter("@COMENTARIOCOB", SqlDbType.VarChar, 250) { Value = (object)comentarioCob ?? string.Empty });
                            cmd1.Parameters.Add(new SqlParameter("@CIA", SqlDbType.VarChar, 30) { Value = cia });

                            // JSON como varchar(max)
                            cmd1.Parameters.Add(new SqlParameter("@DETALLE_APLICACION_JSON", SqlDbType.VarChar, -1)
                            { Value = (object)detalleAplicacionJson ?? DBNull.Value });

                            cmd1.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }


    }
}
