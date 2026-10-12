using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Para IEnumerable
using System.Web; // Para HttpPostedFileBase

  public class CrearDepositoVM // O LiqMovimientoBancoVM si no usas herencia para el archivo
{
    [Required(ErrorMessage = "El cliente es obligatorio.")]
    public string Cliente { get; set; }

    [Required(ErrorMessage = "La cuenta bancaria es obligatoria.")]
    public string CuentaBancaria { get; set; }

    [Required(ErrorMessage = "El número de documento/referencia es obligatorio.")]
    [StringLength(50)]
    public string Documento { get; set; }

    // En CrearDepositoVM.cs o LiqMovimientoBancoVM.cs

    [Required(ErrorMessage = "El monto es obligatorio.")]
    public string Monto { get; set; } // <-- Ahora el Model Binder simplemente acepta el texto "200.75"

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [RegularExpression(@"^\d{2}\/\d{2}\/\d{4}$", ErrorMessage = "La fecha debe tener el formato dd/MM/yyyy.")]
    public string Fecha { get; set; }

    // Propiedades que ya tenías...
    [Required]
    public string Moneda { get; set; }
    [Required]
    public int Subtipo { get; set; }
    public string TipoCC { get; set; }
    public string TipoBanco { get; set; }
    public string Concepto { get; set; }
    public string Beneficiario { get; set; }
    public string Referencia { get; set; }
    public string Comentario { get; set; }
    public decimal? TipoCambioOrig { get; set; }
    public decimal? MontoOriginal { get; set; }

    //Los archivos son opcionales.
    //[Required(ErrorMessage = "Debe adjuntar al menos un comprobante.")]
    public IEnumerable<HttpPostedFileBase> ComprobantePago { get; set; }


    public string DetalleAplicacionJson { get; set; }

}
