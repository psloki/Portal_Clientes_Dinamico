using Plantilla.core.Entities;
using Plantilla.core.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plantilla.core.ViewModels;

namespace Plantilla.core.Manager
{
  public class RegistroPagoManager: Manager
    {
        public DataTable ObtenerComprobantePago(string documento, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    return context.PROC_OBT_COMPROBANTE_PAGO_CIA(documento, cia);
                }
                finally
                {
                    context.Connection.Close();
                }
            }
        }

        public List<PCL_PROC_OBT_DOCUMENTO_POR_CLIENTE_V2_SQLResult> Obtener_Documentos_Cliente(
            string tipoDoc,
            string vendedor,
            string cliente,
            int pagina,
            int tamPagina,
            out int totalFilas, string cia)
        {
            string Norm(string v)
            {
                v = (v ?? "").Trim().ToUpperInvariant();
                return (v == "" || v == "VND" || v == "CND") ? "0" : v;
            }

            vendedor = Norm(vendedor);
            cliente = string.IsNullOrWhiteSpace(cliente) ? "0" : cliente.Trim();

            var result = new List<PCL_PROC_OBT_DOCUMENTO_POR_CLIENTE_V2_SQLResult>();
            totalFilas = 0;

            string connectionString = GetConnectionString();
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (var cmd = new SqlCommand("GLOBAL.PCL_PROC_OBT_DOCUMENTO_POR_CLIENTE_V2", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CIA", SqlDbType.VarChar, 20).Value = cia;
                        cmd.CommandTimeout = 120;

                        cmd.Parameters.Add("@TIPO_DOC", SqlDbType.VarChar, 100).Value = (object)(tipoDoc ?? "0");
                        cmd.Parameters.Add("@VENDEDOR", SqlDbType.VarChar, 4).Value = (object)(vendedor ?? "0");
                        cmd.Parameters.Add("@CLIENTE", SqlDbType.VarChar, 20).Value = (object)(cliente ?? "0");
                        cmd.Parameters.Add("@PAGINA", SqlDbType.Int).Value = pagina;
                        cmd.Parameters.Add("@TAM_PAGINA", SqlDbType.Int).Value = tamPagina;

                        var pTotal = new SqlParameter("@TOTAL_FILAS", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.InputOutput,
                            Value = 0
                        };
                        cmd.Parameters.Add(pTotal);

                        conn.Open();

                        using (var dr = cmd.ExecuteReader())
                        {
                            string GetString(string col)
                            {
                                try
                                {
                                    int i = dr.GetOrdinal(col);
                                    return dr.IsDBNull(i) ? null : dr.GetValue(i)?.ToString();
                                }
                                catch (IndexOutOfRangeException) { return null; }
                            }
                            long? GetLong(string col)
                            {
                                try
                                {
                                    int i = dr.GetOrdinal(col);
                                    return dr.IsDBNull(i) ? (long?)null : Convert.ToInt64(dr.GetValue(i));
                                }
                                catch (IndexOutOfRangeException) { return null; }
                            }
                            short GetShort(string col)
                            {
                                try
                                {
                                    int i = dr.GetOrdinal(col);
                                    return dr.IsDBNull(i) ? (short)0 : Convert.ToInt16(dr.GetValue(i));
                                }
                                catch (IndexOutOfRangeException) { return 0; }
                            }
                            int? GetInt(string col)
                            {
                                try
                                {
                                    int i = dr.GetOrdinal(col);
                                    return dr.IsDBNull(i) ? (int?)null : Convert.ToInt32(dr.GetValue(i));
                                }
                                catch (IndexOutOfRangeException) { return null; }
                            }
                            DateTime? GetDate(string col)
                            {
                                try
                                {
                                    int i = dr.GetOrdinal(col);
                                    return dr.IsDBNull(i) ? (DateTime?)null : Convert.ToDateTime(dr.GetValue(i));
                                }
                                catch (IndexOutOfRangeException) { return null; }
                            }
                            decimal GetDecimal(string col)
                            {
                                try
                                {
                                    int i = dr.GetOrdinal(col);
                                    return dr.IsDBNull(i) ? 0m : Convert.ToDecimal(dr.GetValue(i));
                                }
                                catch (IndexOutOfRangeException) { return 0m; }
                            }
                            decimal? GetNullableDecimal(string col)
                            {
                                try
                                {
                                    int i = dr.GetOrdinal(col);
                                    return dr.IsDBNull(i) ? (decimal?)null : Convert.ToDecimal(dr.GetValue(i));
                                }
                                catch (IndexOutOfRangeException) { return null; }
                            }

                            while (dr.Read())
                            {
                                result.Add(new PCL_PROC_OBT_DOCUMENTO_POR_CLIENTE_V2_SQLResult
                                {
                                    ID = GetLong("ID"),
                                    CLIENTE = GetString("CLIENTE"),
                                    CLIENTE_NOMBRE = GetString("CLIENTE_NOMBRE"),
                                    DOCUMENTO = GetString("DOCUMENTO"),
                                    TIPO = GetString("TIPO"),
                                    SUBTIPO = GetString("SUBTIPO"),
                                    COD_SUBTIPO_DEBITO = GetShort("COD_SUBTIPO_DEBITO"),
                                    FECHA_DOCUMENTO = GetDate("FECHA_DOCUMENTO"),
                                    FECHA_VENCE = GetDate("FECHA_VENCE"),
                                    DIAS_NETO = GetInt("DIAS_NETO"),
                                    MONTO = GetDecimal("MONTO"),
                                    SALDO = GetDecimal("SALDO"),
                                    MONTO_PAGO = GetNullableDecimal("MONTO_PAGO"),
                                    COD_VENDEDOR_CLI = GetString("COD_VENDEDOR_CLI"),
                                    VENDEDOR_DEBITO = GetString("VENDEDOR_DEBITO"),
                                    COBRADOR_DEBITO = GetString("COBRADOR_DEBITO"),
                                    NOMBRE_COBRADOR_DEBITO = GetString("NOMBRE_COBRADOR_DEBITO"),
                                    MONEDA_DEBITO = GetString("MONEDA_DEBITO"),
                                    COMENTARIO_DEBITO = GetString("COMENTARIO_DEBITO"),
                                    CONDICION_PAGO_DESC_DEB = GetString("CONDICION_PAGO_DESC_DEB"),
                                    CONDICION_PAGO_DEB = GetString("CONDICION_PAGO_DEB"),
                                    CONDICION_PAGO_DESC_CRE = GetString("CONDICION_PAGO_DESC_CRE"),
                                    CONDICION_PAGO_CRE = GetString("CONDICION_PAGO_CRE")
                                });
                            }
                        }

                        totalFilas = (pTotal.Value == DBNull.Value || pTotal.Value == null)
                            ? 0
                            : Convert.ToInt32(pTotal.Value);
                    }
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("GLOBAL.PCL_PROC_OBT_DOCUMENTO_POR_CLIENTE_V2", e);
                    result = new List<PCL_PROC_OBT_DOCUMENTO_POR_CLIENTE_V2_SQLResult>();
                    totalFilas = 0;
                }
            }

            return result;
        }


        public List<PROC_OBT_TIPOS_EXPEDIENTE_DIGITAL_PPResult> GetTiposExpedienteDigitalPP(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TIPOS_EXPEDIENTE_DIGITAL_PPResult> result = new List<PROC_OBT_TIPOS_EXPEDIENTE_DIGITAL_PPResult>();
                try
                {
                    result = context.PROC_OBT_TIPOS_EXPEDIENTE_DIGITAL_PP(cia).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_USUARIOS_TEF", e);
                    return null;
                }
            }
        }
        public static PROC_OBT_DOCUMENTO_ESTADO_CUENTA_BANCARIOResult ObtenerDataEstadoCuentaBancario(string cuenta_banco, string numero, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                PROC_OBT_DOCUMENTO_ESTADO_CUENTA_BANCARIOResult result = new PROC_OBT_DOCUMENTO_ESTADO_CUENTA_BANCARIOResult();
                try
                {
                    result = context.PROC_OBT_DOCUMENTO_ESTADO_CUENTA_BANCARIO(cuenta_banco, numero, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    result = null;
                    WriteLog("PROC_OBT_DOCUMENTO_ESTADO_CUENTA_BANCARIO", e);
                }
                return result;
            }
        }


        public static PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTEResult ValidarSiExisteNumBancoTEF(string numero, string banco, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTEResult result = new PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTEResult();
                try
                {
                    result = context.PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE(numero, banco, cia).First();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_VERIFICAR_SI_NUM_BANCO_TEF_EXISTE", e);
                }
                return result;
            }
        }

        public PCL_PROC_OBT_SALDO_DOC_CLIENTEResult SaldoTotal(
            string tipoDoc,
            string vendedor,
            string cobrador,
            string cliente,
            string cia)
        {
       
            vendedor = string.Equals(vendedor, "VND", StringComparison.OrdinalIgnoreCase)
                       ? "0"
                       : vendedor;

            cobrador = string.Equals(cobrador, "CND", StringComparison.OrdinalIgnoreCase)
                       ? "0"
                       : cobrador;

            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    return context.PCL_PROC_OBT_SALDO_DOC_CLIENTE(
             tipoDoc,
             vendedor,  
             cobrador,  
             cliente,
             cia)
           .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message
                                 .Replace(Environment.NewLine, " ")
                                 .Replace('"', ' ')
                                 .Replace('\'', ' ');
                    WriteLog("PCL_PROC_OBT_SALDO_DOC_CLIENTE", msg);
                    return null;
                }
            }
        }


        public List<PROC_OBT_SELECT_TIPO_EXPEDIENTEResult> GetTiposExpedienteFiltro(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SELECT_TIPO_EXPEDIENTEResult> result = new List<PROC_OBT_SELECT_TIPO_EXPEDIENTEResult>();
                try
                {
                    result = context.PROC_OBT_SELECT_TIPO_EXPEDIENTE(cia).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SELECT_TIPO_EXPEDIENTE", e);
                    return null;
                }
            }
        }

        public PROC_OBT_CONSECUTIVO_DOCUMENTOS_CCResult Obtener_Consecutivo_Documentos(string cobrador,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    var result = context.PROC_OBT_CONSECUTIVO_DOCUMENTOS_CC(cobrador,cia).FirstOrDefault();
                    context.Connection.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CONSECUTIVO_DOCUMENTOS_CC", e);
                    return null;
                }


            }
        }

        public List<PROC_OBT_CREDITOS_CLIENTEResult> Obtener_credito_cliente(
      string vendedor,
      string cobrador,
      string cliente,
      string cia)
        {
            
            if (string.Equals(vendedor, "VND", StringComparison.OrdinalIgnoreCase))
                vendedor = "0";

            if (string.Equals(cobrador, "CND", StringComparison.OrdinalIgnoreCase))
                cobrador = "0";

            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    return context
                           .PROC_OBT_CREDITOS_CLIENTE(vendedor, cobrador, cliente, cia)
                           .ToList();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message
                                 .Replace(Environment.NewLine, " ")
                                 .Replace('"', ' ')
                                 .Replace("'", " ");

                    WriteLog("PROC_OBT_CREDITOS_CLIENTE", msg);
                    return null;
                }
                finally
                {
                    context.Connection.Close();
                }
            }
        }


        public List<PROC_OBT_COBRADOR_CLIENTEResult> Obtener_cobrador_cliente(string cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_COBRADOR_CLIENTEResult> result;
                try
                {
                    result = context.PROC_OBT_COBRADOR_CLIENTE(cliente, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_COBRADOR_CLIENTE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }


        public List<PROC_OBT_VENDEDOR_CLIENTEResult> Obtener_vendedor_cliente(string cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_VENDEDOR_CLIENTEResult> result;
                try
                {
                    result = context.PROC_OBT_VENDEDOR_CLIENTE(cliente, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_VENDEDOR_CLIENTEResult", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_TIPO_DOCResult> Obtener_Tipo_Doc(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TIPO_DOCResult> result;
                try
                {

                    result = context.PROC_OBT_TIPO_DOC(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_TIPO_DOC", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_BANCOResult> Obtener_Cuenta_Bancaria(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_BANCOResult> result;
                try
                {

                    result = context.PROC_OBT_BANCO(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_BANCO", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }
        public string Agregar_Documentos(
            string cliente, string documentoCC, string tipo_doc, int subtipo,
            string cuenta_bancaria, string fecha, string referencia,
            string moneda, decimal total_abonado, string concepto,
            string img_comprobante1, string img_comprante2, int id_visita,
            string vendedor, string cobrador, string usuario,
            string comentariocob, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    // Aquí obtienes el resultado real del SP
                    var result = context.PROC_INS_DOCUMENTOS(
                        cliente, documentoCC, tipo_doc, subtipo,
                        cuenta_bancaria, fecha, referencia,
                        moneda, total_abonado, concepto,
                        img_comprobante1, img_comprante2,
                        id_visita, vendedor, cobrador,
                        usuario, comentariocob, cia
                    );

                    return "1"; //result.ToString() ?? "0";
                }
                catch (Exception ex)
                {
                    var e = ex.Message
                                .Replace(Environment.NewLine, " ")
                                .Replace('"', ' ')
                                .Replace("'", " ");

                    WriteLog("PROC_INS_DOCUMENTOS", e);

                    // Re-lanza para que tu Controller pueda mostrar el error crudo
                    throw;
                }
                finally
                {
                    context.Connection.Close();
                }
            }
        }
        


        public string ObtenerTiqueteAplicacionTexto(string documento, string cia)
        {
            string resultado = string.Empty;

            string connectionString = GetConnectionString();
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("global.PROC_OBT_COMPROBANTE_APLICACION", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // 2. Parámetros del SP
                        cmd.Parameters.AddWithValue("@DOCUMENTO", documento);
                        cmd.Parameters.AddWithValue("@CIA", cia);

                        conn.Open();

                        // 3. Ejecutar y leer resultado
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                // Ajusta el nombre EXACTO según tu SP
                                resultado = dr["TEXTO_IMPRESION"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var e = ex.Message.Replace(Environment.NewLine, " ").Replace('"', ' ').Replace("'", " ");
                    WriteLog("PROC_OBT_COMPROBANTE_APLICACION", e);

                    resultado = ex.Message;
                }
            }

            return resultado;
        }


        public string ObtenerTiqueteAplicacionTextoCSharp(string documento, string cia)
        {
            var datos = ObtenerDatosComprobanteAplicacion(documento, cia);
            return ConstruirTiqueteAplicacionEscPos(datos);
        }

        private List<ComprobanteAplicacionDato> ObtenerDatosComprobanteAplicacion(string documento, string cia)
        {
            var resultado = new List<ComprobanteAplicacionDato>();
            string connectionString = GetConnectionString();
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("GLOBAL.PROC_OBT_COMPROBANTE_APLICACION_DATOS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DOCUMENTO", documento);
                        cmd.Parameters.AddWithValue("@CIA", cia);

                        conn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                resultado.Add(new ComprobanteAplicacionDato
                                {
                                    NIT = LeerString(dr, "NIT"),
                                    NOMBRE_CIA = LeerString(dr, "NOMBRE_CIA"),
                                    DIREC1 = LeerString(dr, "DIREC1"),
                                    TELEFONO = LeerString(dr, "TELEFONO"),
                                    REFERENCIA = LeerString(dr, "REFERENCIA"),
                                    FECHA_MOV = LeerString(dr, "FECHA_MOV"),
                                    HORA = LeerString(dr, "HORA"),
                                    MONEDA = LeerString(dr, "MONEDA"),
                                    METODO_PAGO_DESC = LeerString(dr, "METODO_PAGO_DESC"),
                                    CLIENTE = LeerString(dr, "CLIENTE"),
                                    NOM_CLIENTE = LeerString(dr, "NOM_CLIENTE"),
                                    DETALLE = LeerString(dr, "DETALLE"),
                                    MONTO_DEBITO = LeerDecimal(dr, "MONTO_DEBITO") ?? 0,
                                    NOTAS = LeerString(dr, "NOTAS"),
                                    MONTO_CREDITO = LeerDecimal(dr, "MONTO_CREDITO") ?? 0,
                                    DEBITO = LeerString(dr, "DEBITO"),
                                    MONTO_ABONADO = LeerDecimal(dr, "MONTO_ABONADO"),
                                    SALDO = LeerDecimal(dr, "SALDO")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var e = ex.Message.Replace(Environment.NewLine, " ").Replace('"', ' ').Replace("'", " ");
                    WriteLog("PROC_OBT_COMPROBANTE_APLICACION_DATOS", e);
                    throw;
                }
            }

            return resultado;
        }


        public List<InteresMoraDocumentoDto> ObtenerInteresesMoraCliente(string cliente, string cia, DateTime fechaCorte)
        {
            var resultado = new List<InteresMoraDocumentoDto>();
            string connectionString = GetConnectionString();
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("GLOBAL.PROC_OBT_INTERESES_MORA_CLIENTE", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 120;
                        cmd.Parameters.AddWithValue("@CLIENTE", cliente);
                        cmd.Parameters.AddWithValue("@CIA", cia);
                        cmd.Parameters.Add("@FECHA_CORTE", SqlDbType.Date).Value = fechaCorte.Date;

                        conn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                resultado.Add(new InteresMoraDocumentoDto
                                {
                                    CLIENTE = LeerString(dr, "CLIENTE"),
                                    NOMBRE = LeerString(dr, "NOMBRE"),
                                    FECHA_ULT_MORA = LeerDate(dr, "FECHA_ULT_MORA"),
                                    TASA_INTERES_MORA = LeerDecimal(dr, "TASA_INTERES_MORA") ?? 0,
                                    TIPO = LeerString(dr, "TIPO"),
                                    DOCUMENTO = LeerString(dr, "DOCUMENTO"),
                                    FECHA = LeerDate(dr, "FECHA"),
                                    FECHA_VENCE = LeerDate(dr, "FECHA_VENCE"),
                                    MONTO = LeerDecimal(dr, "MONTO") ?? 0,
                                    SALDO = LeerDecimal(dr, "SALDO") ?? 0,
                                    MONEDA = LeerString(dr, "MONEDA"),
                                    DIAS_VENCIDO = Convert.ToInt32(LeerDecimal(dr, "DIAS_VENCIDO") ?? 0),
                                    INTERESES = LeerDecimal(dr, "INTERESES") ?? 0
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var e = ex.Message.Replace(Environment.NewLine, " ").Replace('"', ' ').Replace("'", " ");
                    WriteLog("PROC_OBT_INTERESES_MORA_CLIENTE", e);
                    throw;
                }
            }

            return resultado;
        }
        public InteresMoraInsertResultDto InsertarInteresesMoraCliente(string cliente, string cia, DateTime fechaCorte, string usuario)
        {
            InteresMoraInsertResultDto resultado = null;
            string connectionString = GetConnectionString();
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("GLOBAL.PROC_INS_INTERESES_MORA_CLIENTE", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("@CLIENTE", cliente);
                        cmd.Parameters.AddWithValue("@CIA", cia);
                        cmd.Parameters.Add("@FECHA_CORTE", SqlDbType.Date).Value = fechaCorte.Date;
                        cmd.Parameters.AddWithValue("@USUARIO", string.IsNullOrWhiteSpace(usuario) ? (object)DBNull.Value : usuario);

                        conn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                resultado = new InteresMoraInsertResultDto
                                {
                                    DOCUMENTO = LeerString(dr, "DOCUMENTO"),
                                    TIPO = LeerString(dr, "TIPO"),
                                    CLIENTE = LeerString(dr, "CLIENTE"),
                                    INTERES_TOTAL = LeerDecimal(dr, "INTERES_TOTAL") ?? 0,
                                    DETALLE_INTERESES = LeerString(dr, "DETALLE_INTERESES"),
                                    FECHA_ULT_MORA = LeerDate(dr, "FECHA_ULT_MORA")
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var e = ex.Message.Replace(Environment.NewLine, " ").Replace('"', ' ').Replace("'", " ");
                    WriteLog("PROC_INS_INTERESES_MORA_CLIENTE", e);
                    throw;
                }
            }

            return resultado;
        }
        private static string ConstruirTiqueteAplicacionEscPos(List<ComprobanteAplicacionDato> datos)
        {
            if (datos == null || datos.Count == 0) return string.Empty;

            const int ancho = 42;
            var separador = new string('-', ancho);
            var d = datos[0];
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine(Centrar(d.NOMBRE_CIA, ancho));
            sb.AppendLine(Centrar("Cedula Juridica No." + d.NIT, ancho));
            sb.AppendLine(Centrar(d.DIREC1, ancho));
            sb.AppendLine(Centrar(d.TELEFONO, ancho));
            sb.AppendLine(separador);
            sb.AppendLine();
            sb.AppendLine(Centrar("Recibo por Dinero", ancho));
            sb.AppendLine(Centrar("Original", ancho));
            sb.AppendLine(separador);
            sb.AppendLine();
            // Los datos generales se imprimen inmediatamente después de su etiqueta.
            // Antes se completaba cada línea hasta 42 caracteres con espacios,
            // haciendo que impresoras con una fuente o ancho distinto cortaran
            // o desplazaran los valores. Este formato es más estable en impresoras térmicas.
            sb.AppendLine("Referencia: " + d.REFERENCIA);
            sb.AppendLine("Fecha: " + d.FECHA_MOV);
            sb.AppendLine("Hora: " + d.HORA);
            sb.Append(WrapText("Moneda: " + d.MONEDA, ancho));
            sb.AppendLine();
            sb.Append(WrapText("Metodo Pago: " + d.METODO_PAGO_DESC, ancho));
            sb.AppendLine();
            sb.Append(WrapText("Cliente: " + d.CLIENTE + " - " + d.NOM_CLIENTE, ancho));
            sb.AppendLine();
            sb.AppendLine(separador);
            sb.AppendLine();

            if (!string.Equals(d.DETALLE, "ADELANTO", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine("Numero Documento       Abonado    Saldo");

                foreach (var item in datos.Where(x => !string.IsNullOrWhiteSpace(x.DEBITO)))
                {
                    var debito = PadRightLimit(item.DEBITO, 23);
                    var abonado = PadRightLimit(FormatoDecimalSinMiles(item.MONTO_ABONADO ?? 0), 10);
                    var saldo = FormatoDecimalSinMiles(item.SALDO ?? 0);
                    sb.AppendLine(debito + abonado + saldo);
                }
            }

            sb.AppendLine();
            sb.AppendLine(EtiquetaValor("Total Documentos: ", FormatoDecimal(d.MONTO_DEBITO), ancho));
            sb.AppendLine(separador);
            sb.Append(WrapText(d.NOTAS, ancho));
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(Centrar("RESUMEN DE MONTOS TOTALES", ancho));
            sb.AppendLine(EtiquetaValor("Monto: ", FormatoDecimal(d.MONTO_CREDITO), ancho));
            sb.AppendLine(EtiquetaValor("Recibo: ", FormatoDecimal(d.MONTO_CREDITO), ancho));
            sb.AppendLine(separador);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(Centrar("Firma ____________________", ancho));
            sb.AppendLine();
            sb.AppendLine();

            return QuitarAcentos(sb.ToString());
        }

        private static string EtiquetaValor(string etiqueta, string valor, int ancho)
        {
            valor = valor ?? string.Empty;
            var texto = etiqueta + valor;
            var espacios = Math.Max(ancho - texto.Length, 0);
            return etiqueta + new string(' ', espacios) + valor;
        }

        private static string Centrar(string valor, int ancho)
        {
            valor = (valor ?? string.Empty).Trim();
            var espacios = Math.Max((ancho - valor.Length) / 2, 0);
            return new string(' ', espacios) + valor;
        }

        private static string WrapText(string texto, int ancho)
        {
            texto = texto ?? string.Empty;
            var palabras = texto.Replace("\r\n", "\n").Split(new[] { ' ', '\n' }, StringSplitOptions.None);
            var sb = new StringBuilder();
            var linea = string.Empty;

            foreach (var palabra in palabras)
            {
                if (palabra == string.Empty) continue;

                if (linea.Length == 0)
                {
                    linea = palabra;
                    continue;
                }

                if (linea.Length + 1 + palabra.Length <= ancho)
                {
                    linea += " " + palabra;
                }
                else
                {
                    sb.AppendLine(linea);
                    linea = palabra;
                }
            }

            if (linea.Length > 0) sb.Append(linea);
            return sb.ToString();
        }

        private static string PadRightLimit(string valor, int ancho)
        {
            valor = valor ?? string.Empty;
            return (valor + new string(' ', ancho)).Substring(0, ancho);
        }

        private static string FormatoDecimal(decimal valor)
        {
            return valor.ToString("N2", CultureInfo.GetCultureInfo("en-US"));
        }

        private static string FormatoDecimalSinMiles(decimal valor)
        {
            return valor.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static string LeerString(SqlDataReader dr, string columna)
        {
            var ordinal = dr.GetOrdinal(columna);
            return dr.IsDBNull(ordinal) ? string.Empty : Convert.ToString(dr.GetValue(ordinal));
        }

        private static decimal? LeerDecimal(SqlDataReader dr, string columna)
        {
            var ordinal = dr.GetOrdinal(columna);
            return dr.IsDBNull(ordinal) ? (decimal?)null : Convert.ToDecimal(dr.GetValue(ordinal));
        }

        private static DateTime? LeerDate(SqlDataReader dr, string columna)
        {
            var ordinal = dr.GetOrdinal(columna);
            return dr.IsDBNull(ordinal) ? (DateTime?)null : Convert.ToDateTime(dr.GetValue(ordinal));
        }

        private static string QuitarAcentos(string texto)
        {
            var normalizado = (texto ?? string.Empty).Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalizado.Length);

            foreach (var ch in normalizado)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString()
                .Replace("“", "\"")
                .Replace("”", "\"")
                .Replace("‘", "'")
                .Replace("’", "'")
                .Replace("–", "-")
                .Replace("—", "-")
                .Replace('\u00A0', ' ');
        }

        public string Agregar_doc_Auxiliar(string credito_doc, string documento_debito, string tipo_doc, string tipo, int subtipo, int subtipo_debito, string fecha, decimal monto_pago, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PROC_INS_DOCUMENTOS_DET(credito_doc, documento_debito, tipo_doc, tipo, subtipo, subtipo_debito, fecha, monto_pago, cia).ToString();
                    result = "1";
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_INS_DOCUMENTOS_DET", e);
                    result = "0";
                }
                context.Connection.Close();
                return result;
            }

        }


        public OBTENER_PDF_BASE64Result ObtenerPDF(string factura, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                OBTENER_PDF_BASE64Result resultado;
                try
                {
                    //Context.Connection.ConnectionString = getConexion();
                    resultado = context.OBTENER_PDF_BASE64(factura, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("OBTENER_PDF_BASE64", e);
                    resultado = null;
                }
                context.Connection.Close();
                return resultado;
            }
        }

        public PCL_PROC_OBT_CORREO_CP_CLIENTEResult ObtenerCorreoCliente(string cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {

                PCL_PROC_OBT_CORREO_CP_CLIENTEResult result = new PCL_PROC_OBT_CORREO_CP_CLIENTEResult();
                try
                {
                    result = context.PCL_PROC_OBT_CORREO_CP_CLIENTE(cliente, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_OBT_CORREO_CP_CLIENTE", e);
                    return null;
                }
                return result;
            }
        }


        public List<PROC_OBT_SUBTIPO_DOC_CCResult> Obtener_SubtipoDoc(string tipoDoc,string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_SUBTIPO_DOC_CCResult> result;
                try
                {

                    result = context.PROC_OBT_SUBTIPO_DOC_CC(tipoDoc, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_SUBTIPO_DOC_CC", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_TIPO_DOC_CCResult> Obtener_Tipo(string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_TIPO_DOC_CCResult> result;
                try
                {

                    result = context.PROC_OBT_TIPO_DOC_CC(cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_TIPO_DOC", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

        public List<PROC_OBT_CLIENTE_CCResult> Obtener_cliente(string cliente, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                List<PROC_OBT_CLIENTE_CCResult> result;
                try
                {
                    result = context.PROC_OBT_CLIENTE_CC(cliente, cia).ToList();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PROC_OBT_CLIENTE_CC", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }
        public string Enviar_Aplicacion_NC(string docAplicacion,
                                   string cliente,
                                   string correos,
                                   string cia)
        {
            using (PlantillaDataContext ctx = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    ctx.PCL_PROC_ENVIAR_APLICACION_NC(docAplicacion, cliente, correos, cia);
                    return "1";

                }
                catch (Exception ex)
                {
                    string e = ex.Message
                                   .Replace(Environment.NewLine, " ")
                                   .Replace('"', ' ')
                                   .Replace("'", " ");
                    WriteLog("PCL_PROC_ENVIAR_APLICACION_NC", e);
                    return "0";
                }
                finally
                {
                    ctx.Connection.Close();
                }
            }
        }
        public PROC_OBT_EMAIL_COBRADORResult Obtener_Email_Cobrador(string cobrador, string cia)
        {
          
            using (PlantillaDataContext ctx = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    return ctx.PROC_OBT_EMAIL_COBRADOR(cobrador, cia).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string e = ex.Message.Replace(Environment.NewLine, " ")
                                         .Replace('"', ' ')
                                         .Replace("'", " ");
                    WriteLog("PROC_OBT_EMAIL_COBRADOR", e);
                    return null;
                }
                finally
                {
                    ctx.Connection.Close();
                }
            }
        }
        public List<PROC_OBT_EMAIL_CLIENTE_POR_COBRADORResult>
        ObtenerEmailsClientesPorCobrador(string cobrador, string cia)
        {
            using (PlantillaDataContext ctx = new PlantillaDataContext(GetConnectionString()))
            {
                try
                {
                    return ctx.PROC_OBT_EMAIL_CLIENTE_POR_COBRADOR(cobrador, cia)
                              .ToList();
                }
                catch (Exception ex)
                {
                    var e = ex.Message
                                .Replace(Environment.NewLine, " ")
                                .Replace('"', ' ')
                                .Replace("'", " ");
                    WriteLog("PROC_OBT_EMAIL_CLIENTE_POR_COBRADOR", e);
                    return null;
                }
                finally
                {
                    ctx.Connection.Close();
                }
            }
        }
        public string Enviar_Correo_CLiente_FE(string cliente, string correos, string documento, string cia)
        {
            using (PlantillaDataContext context = new PlantillaDataContext(GetConnectionString()))
            {
                string result;
                try
                {
                    result = context.PCL_PROC_ENVIAR_FACTURA_AL_CLIENTE(cliente, correos, documento, cia).ToString();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e = e.Replace(Environment.NewLine, " ");
                    e = e.Replace('"', ' ');
                    e = e.Replace("'", " ");
                    WriteLog("PCL_PROC_ENVIAR_FACTURA_AL_CLIENTE", e);
                    result = null;
                }
                context.Connection.Close();
                return result;
            }

        }

    }
}


