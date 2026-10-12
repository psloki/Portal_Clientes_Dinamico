using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plantilla.web.Models
{
    public class TrackingToken : IToken
    {
        private readonly string pantalla = "Tracking";
        public string usuario { get; set; }
        public string pedido { get; set; }
        public string token { get; set; }
        public string cia { get; set; }
        public TrackingToken()
        {

        }



        public void GenerarToken()
        {
            try
            {
                string valor = $"{usuario},{cia},{pedido}";
                token = Helpers.Helper.Encrypt(valor);
            }
            catch (Exception)
            {

                token = null;

            }

        }

    }
}