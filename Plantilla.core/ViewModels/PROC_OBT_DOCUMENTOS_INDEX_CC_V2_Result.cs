using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla.core.ViewModels
{
    public class PROC_OBT_DOCUMENTOS_INDEX_CC_V2_Result
    {
        public string TIPO { get; set; }
        public string DOCUMENTO { get; set; }
        public decimal? TIPO_CAMBIO { get; set; }
        public string FECHA { get; set; }
        public string CONDICION_PAGO { get; set; }
        public string U_ORDEN_COMPRA { get; set; }
        public string MONEDA { get; set; }

        public decimal? MONTO_ORIGINAL { get; set; }
        public decimal? MONTO_LIQUIDADO { get; set; }   // ← NUEVO
        public decimal? SALDO { get; set; }             // (restante)
        public decimal? MONTO_A_LIQUIDAR { get; set; }  // (por defecto = SALDO)

        // opcionales legados
        public string MONTO_MILES { get; set; }
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
    }



}
