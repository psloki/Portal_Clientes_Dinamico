using System;

namespace Plantilla.core.DTOs
{
    public class InteresMoraInsertResultDto
    {
        public string DOCUMENTO { get; set; }
        public string TIPO { get; set; }
        public string CLIENTE { get; set; }
        public decimal INTERES_TOTAL { get; set; }
        public string DETALLE_INTERESES { get; set; }
        public DateTime? FECHA_ULT_MORA { get; set; }
    }
}
