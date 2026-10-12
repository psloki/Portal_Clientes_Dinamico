using System;

namespace Plantilla.core.ViewModels
{
    public class PCL_PROC_OBT_DOCUMENTO_POR_CLIENTE_V2_SQLResult
    {
        public long? ID { get; set; }
        public string CLIENTE { get; set; }
        public string CLIENTE_NOMBRE { get; set; }
        public string DOCUMENTO { get; set; }
        public string TIPO { get; set; }
        public string SUBTIPO { get; set; }
        public short COD_SUBTIPO_DEBITO { get; set; }
        public DateTime? FECHA_DOCUMENTO { get; set; }
        public DateTime? FECHA_VENCE { get; set; }
        public int? DIAS_NETO { get; set; }
        public decimal MONTO { get; set; }
        public decimal SALDO { get; set; }
        public decimal? MONTO_PAGO { get; set; }
        public string COD_VENDEDOR_CLI { get; set; }
        public string VENDEDOR_DEBITO { get; set; }
        public string COBRADOR_DEBITO { get; set; }
        public string NOMBRE_COBRADOR_DEBITO { get; set; }
        public string MONEDA_DEBITO { get; set; }
        public string COMENTARIO_DEBITO { get; set; }

        public string CONDICION_PAGO_DESC_DEB { get; set; }
        public string CONDICION_PAGO_DEB { get; set; }
        public string CONDICION_PAGO_DESC_CRE { get; set; }
        public string CONDICION_PAGO_CRE { get; set; }
    }
}
