using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace Plantilla.core.Entities
{
    /*
        Extensiones manuales del DataContext únicamente para el módulo Descuentos.

        Este archivo evita modificar Plantilla1.designer.cs, porque ese archivo es
        autogenerado y contiene mappings de muchos módulos que no deben sustituirse.
    */
    public partial class PlantillaDataContext
    {
        [Function(Name = "DIS86.PROC_EDI_PORCENTAJE_DESCUENTO")]
        public int PROC_EDI_PORCENTAJE_DESCUENTO(
            [Parameter(Name = "NIVEL_PRECIO", DbType = "VarChar(12)")] string nivelPrecio,
            [Parameter(Name = "MONEDA", DbType = "VarChar(1)")] string moneda,
            [Parameter(Name = "VERSION", DbType = "Int")] int? version,
            [Parameter(Name = "ARTICULO", DbType = "VarChar(20)")] string articulo,
            [Parameter(Name = "ESCALA", DbType = "Int")] int? escala,
            [Parameter(Name = "VERSION_DCTO", DbType = "Int")] int? versionDcto,
            [Parameter(Name = "CANTIDAD_MIN", DbType = "Int")] int? cantidadMin,
            [Parameter(Name = "CANTIDAD_MAX", DbType = "Int")] int? cantidadMax,
            [Parameter(Name = "PORC_DESCUENTO", DbType = "Decimal(28,8)")] decimal? porcentajeDescuento,
            [Parameter(Name = "FECHA_INI", DbType = "DateTime")] DateTime? fechaInicio,
            [Parameter(Name = "FECHA_FIN", DbType = "DateTime")] DateTime? fechaFin,
            [Parameter(Name = "USUARIO", DbType = "VarChar(50)")] string usuario,
            [Parameter(Name = "CIA", DbType = "VarChar(20)")] string cia)
        {
            IExecuteResult resultado = ExecuteMethodCall(
                this,
                (MethodInfo)MethodInfo.GetCurrentMethod(),
                nivelPrecio,
                moneda,
                version,
                articulo,
                escala,
                versionDcto,
                cantidadMin,
                cantidadMax,
                porcentajeDescuento,
                fechaInicio,
                fechaFin,
                usuario,
                cia);

            return (int)resultado.ReturnValue;
        }

        [Function(Name = "DIS86.PROC_EDI_PORCENTAJE_DESCUENTO_MASIVO")]
        public int PROC_EDI_PORCENTAJE_DESCUENTO_MASIVO(
            [Parameter(Name = "NIVEL_PRECIO", DbType = "VarChar(12)")] string nivelPrecio,
            [Parameter(Name = "LISTA_REGISTROS", DbType = "NVarChar(MAX)")] string listaRegistros,
            [Parameter(Name = "CANTIDAD_MIN", DbType = "Int")] int? cantidadMin,
            [Parameter(Name = "CANTIDAD_MAX", DbType = "Int")] int? cantidadMax,
            [Parameter(Name = "PORC_DESCUENTO", DbType = "Decimal(28,8)")] decimal? porcentajeDescuento,
            [Parameter(Name = "FECHA_INI", DbType = "DateTime")] DateTime? fechaInicio,
            [Parameter(Name = "FECHA_FIN", DbType = "DateTime")] DateTime? fechaFin,
            [Parameter(Name = "USUARIO", DbType = "VarChar(50)")] string usuario,
            [Parameter(Name = "CIA", DbType = "VarChar(20)")] string cia)
        {
            IExecuteResult resultado = ExecuteMethodCall(
                this,
                (MethodInfo)MethodInfo.GetCurrentMethod(),
                nivelPrecio,
                listaRegistros,
                cantidadMin,
                cantidadMax,
                porcentajeDescuento,
                fechaInicio,
                fechaFin,
                usuario,
                cia);

            return (int)resultado.ReturnValue;
        }

        [Function(Name = "DIS86.PROC_EDI_DESCUENTO_ARTICULO_PRECIO")]
        public int PROC_EDI_DESCUENTO_ARTICULO_PRECIO(
            [Parameter(Name = "NIVEL_PRECIO", DbType = "VarChar(12)")] string nivelPrecio,
            [Parameter(Name = "MONEDA", DbType = "VarChar(1)")] string moneda,
            [Parameter(Name = "VERSION", DbType = "Int")] int? version,
            [Parameter(Name = "ARTICULO", DbType = "VarChar(20)")] string articulo,
            [Parameter(Name = "ESCALA", DbType = "Int")] int? escala,
            [Parameter(Name = "VERSION_DCTO", DbType = "Int")] int? versionDcto,
            [Parameter(Name = "CANTIDAD_MIN", DbType = "Int")] int? cantidadMin,
            [Parameter(Name = "CANTIDAD_MAX", DbType = "Int")] int? cantidadMax,
            [Parameter(Name = "PORC_DESCUENTO", DbType = "Decimal(28,8)")] decimal? porcentajeDescuento,
            [Parameter(Name = "FECHA_INI", DbType = "DateTime")] DateTime? fechaInicio,
            [Parameter(Name = "FECHA_FIN", DbType = "DateTime")] DateTime? fechaFin,
            [Parameter(Name = "USUARIO", DbType = "VarChar(50)")] string usuario,
            [Parameter(Name = "CIA", DbType = "VarChar(20)")] string cia)
        {
            IExecuteResult resultado = ExecuteMethodCall(
                this,
                (MethodInfo)MethodInfo.GetCurrentMethod(),
                nivelPrecio,
                moneda,
                version,
                articulo,
                escala,
                versionDcto,
                cantidadMin,
                cantidadMax,
                porcentajeDescuento,
                fechaInicio,
                fechaFin,
                usuario,
                cia);

            return (int)resultado.ReturnValue;
        }

        [Function(Name = "DIS86.PROC_ELI_DESCUENTO_ARTICULO")]
        public int PROC_ELI_DESCUENTO_ARTICULO(
            [Parameter(Name = "NIVEL_PRECIO", DbType = "VarChar(12)")] string nivelPrecio,
            [Parameter(Name = "MONEDA", DbType = "VarChar(1)")] string moneda,
            [Parameter(Name = "VERSION", DbType = "Int")] int? version,
            [Parameter(Name = "ARTICULO", DbType = "VarChar(20)")] string articulo,
            [Parameter(Name = "ESCALA_DCTO", DbType = "Int")] int? escalaDcto,
            [Parameter(Name = "VERSION_DCTO", DbType = "Int")] int? versionDcto,
            [Parameter(Name = "CIA", DbType = "VarChar(20)")] string cia)
        {
            IExecuteResult resultado = ExecuteMethodCall(
                this,
                (MethodInfo)MethodInfo.GetCurrentMethod(),
                nivelPrecio,
                moneda,
                version,
                articulo,
                escalaDcto,
                versionDcto,
                cia);

            return (int)resultado.ReturnValue;
        }
    }

    public partial class PROC_OBT_NIVEL_ARTICULO_DESCUENTO_INDEXResult
    {
        [Column(DbType = "Int")]
        public int? VERSION_DCTO { get; set; }

        [Column(DbType = "Decimal(28,8)")]
        public decimal? U_PS_PORC_DCTO_ANT { get; set; }

        [Column(DbType = "VarChar(20)")]
        public string U_PS_FECHA_INICIO_PROY { get; set; }

        [Column(DbType = "VarChar(20)")]
        public string U_PS_FECHA_FIN_PROY { get; set; }

        [Column(DbType = "Decimal(28,8)")]
        public decimal? PRECIO_PROYECTADO { get; set; }

        [Column(DbType = "VarChar(20)")]
        public string ESTADO_PROYECCION { get; set; }

        [Column(DbType = "Bit")]
        public bool? TIENE_REGISTRO { get; set; }

        [Column(DbType = "VarChar(1)")]
        public string MONEDA { get; set; }

        [Column(DbType = "Int")]
        public int? VERSION_PRECIO { get; set; }
    }
}
