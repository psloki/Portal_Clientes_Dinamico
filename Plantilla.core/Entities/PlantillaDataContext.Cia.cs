using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Plantilla.core.Entities
{
    public partial class PlantillaDataContext
    {
        // Stable entry point; the SP chooses the schema from the authenticated session's CIA.
        [Function(Name = "DIS86.PROC_OBT_CATALOGO_CIA")]
        [ResultType(typeof(VIEW_OBTENER_AGENTE))]
        [ResultType(typeof(VIEW_OBTENER_BODEGA))]
        [ResultType(typeof(VIEW_OBTENER_CLIENTE))]
        [ResultType(typeof(VIEW_OBTENER_COBRADOR))]
        [ResultType(typeof(VIEW_OBTENER_DIA_RUTA))]
        [ResultType(typeof(VIEW_OBTENER_ESTADO_PEDIDO))]
        [ResultType(typeof(VIEW_OBTENER_HORA_CORTE))]
        [ResultType(typeof(VIEW_OBTENER_RUTA))]
        [ResultType(typeof(VIEW_OBTENER_SEGUIMIENTO_FACTURA))]
        [ResultType(typeof(VIEW_OBTENER_SUCESOS_SEGUIMIENTO))]
        [ResultType(typeof(VIEW_OBTENER_TLA_ESTADO))]
        [ResultType(typeof(VIEW_OBTENER_VENDEDOR))]
        [ResultType(typeof(VIEW_OBT_BANCO))]
        [ResultType(typeof(VIEW_OBT_CLIENTES_PTC))]
        [ResultType(typeof(VIEW_OBT_ESTADO_FACTURA_PTC))]
        [ResultType(typeof(VIEW_OBT_ESTADO_LIQ))]
        [ResultType(typeof(VIEW_OBT_ESTADO_PEDIDO_PTC))]
        [ResultType(typeof(VIEW_OBT_LISTA_ARTICULO))]
        [ResultType(typeof(VIEW_OBT_LISTA_ARTICULOS_BONIFICACION))]
        [ResultType(typeof(VIEW_OBT_RUTA))]
        [ResultType(typeof(VIEW_OBT_TIPO_DOC_UNICO))]
        public IMultipleResults PROC_OBT_CATALOGO_CIA(
            [Parameter(Name = "CIA", DbType = "VarChar(20)")] string cia,
            [Parameter(Name = "CATALOGO", DbType = "NVarChar(128)")] string catalogo)
        {
            IExecuteResult result = ExecuteMethodCall(this,
                (MethodInfo)MethodInfo.GetCurrentMethod(), cia, catalogo);
            return (IMultipleResults)result.ReturnValue;
        }

        public List<T> ObtenerCatalogoCia<T>(string cia, string catalogo)
        {
            if (string.IsNullOrWhiteSpace(cia))
                throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            using (IMultipleResults result = PROC_OBT_CATALOGO_CIA(cia, catalogo))
                return result.GetResult<T>().ToList();
        }

        public DataTable PROC_OBT_COMPROBANTE_PAGO_CIA(string documento, string cia)
        {
            if (string.IsNullOrWhiteSpace(cia))
                throw new InvalidOperationException("La sesión no tiene compañía. Inicie sesión nuevamente.");
            using (SqlCommand command = new SqlCommand("DIS86.PROC_OBT_COMPROBANTE_PAGO_CIA", (SqlConnection)Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@DOCUMENTO", SqlDbType.VarChar, 50).Value = documento;
                command.Parameters.Add("@CIA", SqlDbType.VarChar, 20).Value = cia;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable result = new DataTable();
                    adapter.Fill(result);
                    return result;
                }
            }
        }
    }
}
