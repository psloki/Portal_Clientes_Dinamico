using System;

namespace Plantilla.core.DTOs
{
    public class ComprobanteAplicacionDato
    {
        public string NIT { get; set; }
        public string NOMBRE_CIA { get; set; }
        public string DIREC1 { get; set; }
        public string TELEFONO { get; set; }
        public string REFERENCIA { get; set; }
        public string FECHA_MOV { get; set; }
        public string HORA { get; set; }
        public string MONEDA { get; set; }
        public string METODO_PAGO_DESC { get; set; }
        public string CLIENTE { get; set; }
        public string NOM_CLIENTE { get; set; }
        public string DETALLE { get; set; }
        public decimal MONTO_DEBITO { get; set; }
        public string NOTAS { get; set; }
        public decimal MONTO_CREDITO { get; set; }
        public string DEBITO { get; set; }
        public decimal? MONTO_ABONADO { get; set; }
        public decimal? SALDO { get; set; }
    }
}
