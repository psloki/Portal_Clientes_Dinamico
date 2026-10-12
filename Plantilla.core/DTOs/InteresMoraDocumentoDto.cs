using System;

namespace Plantilla.core.DTOs
{
    public class InteresMoraDocumentoDto
    {
        public string CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public DateTime? FECHA_ULT_MORA { get; set; }
        public decimal TASA_INTERES_MORA { get; set; }
        public string TIPO { get; set; }
        public string DOCUMENTO { get; set; }
        public DateTime? FECHA { get; set; }
        public DateTime? FECHA_VENCE { get; set; }
        public decimal MONTO { get; set; }
        public decimal SALDO { get; set; }
        public string MONEDA { get; set; }
        public int DIAS_VENCIDO { get; set; }
        public decimal INTERESES { get; set; }
    }
}
