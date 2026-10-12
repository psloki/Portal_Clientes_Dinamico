using System;

namespace Plantilla.core.DTOs
{
    public class PROC_OBT_DOCUMENTOS_INDEX_CCResult
    {
        public string TIPO { get; set; }
        public string DOCUMENTO { get; set; }
        public decimal? TIPO_CAMBIO { get; set; }
        public string FECHA { get; set; }
        public string CONDICION_PAGO { get; set; }
        public string U_ORDEN_COMPRA { get; set; }
        public string MONEDA { get; set; }
        public decimal? MONTO { get; set; }
        public string MONTO_MILES { get; set; }
        public decimal? SALDO { get; set; }
        public string SALDO_MILES { get; set; }
        public decimal? IMPUESTO1 { get; set; }
        public string IMPUESTO_MILES { get; set; }
        public decimal? CODIGO_IMPUESTO { get; set; }
        public string USUARIO_APROBACION { get; set; }
        public string CLIENTE_CONTRIBUYENTE { get; set; }
        public decimal? TIPO_CAMBIO_MH { get; set; }
        public string USUARIO { get; set; }
        public string COD_PROVEEDOR { get; set; }
        public int? SUBTIPO { get; set; }
        public DateTime? FECHA_REAL { get; set; }
        public string SUBTIPO_DESC { get; set; }
        public string VENDEDOR { get; set; }
        public string COBRADOR { get; set; }
        public string VENDEDOR_NOMBRE { get; set; }
        public string COBRADOR_NOMBRE { get; set; }
        public string DEPOSITADO { get; set; }
    }

    public class PROC_OBT_DOCUMENTOS_TOTAL_CCResult
    {
        public string TIPO { get; set; }
        public decimal? MONTO { get; set; }
        public int? SUBTIPO { get; set; }
        public int TOTAL { get; set; }
    }
}
