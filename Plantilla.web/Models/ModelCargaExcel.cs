using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plantilla.web.Models
{
    public class ModelCargaExcel
    {
        public string CUENTA_BANCO { get; set; }
        public string DOCUMENTO { get; set; }
        public string TIPO { get; set; }
        public string SUBTIPO { get; set; }
        public string FECHA { get; set; }
        public string CONCEPTO { get; set; }
        public decimal MONTO_C { get; set; }
        public decimal MONTO_D { get; set; }
        public string INS_MOV_REPORTADOS { get; set; }
    }
}