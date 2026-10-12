using System.Collections.Generic;

namespace Plantilla.web.Models
{
    public class KardexDataTableRequest
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string cliente { get; set; }
        public string tipo { get; set; }
        public string bodega { get; set; }
        public string articulo { get; set; }
        public DataTableSearch search { get; set; }
        public List<DataTableOrder> order { get; set; }
    }

    public class DataTableSearch
    {
        public string value { get; set; }
    }

    public class DataTableOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}
