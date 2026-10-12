let fechaInicio = $('#FechaSeguimiento').val().split(" - ")[0].split('/').reverse().join('-');
let fechaFin = $('#FechaSeguimiento').val().split(" - ")[1].split('/').reverse().join('-');
let fecha_prometida = ""
var checked = $("#checkboxfecha").prop('checked');
if (checked) {

    fecha_prometida = $('#FechaInicio2Seguimiento').val().split('/').reverse().join('-');
} else {
    fecha_prometida = "Todas"
}
let fpedido = $('#pedidoSeguimiento').val();
let ffactura = $('#facturaSeguimiento').val();
let fcliente = $('#clienteSeguimiento').val();
let estado_pedido = $('#filtrestadopedidoSeguimiento').val();
let estado_factura = $('#estadoSeguimiento').val();
let rutamulti = `(${$("#rutaSeguimiento").val().map(item => `'${item}'`).join(',')})`;
let agente = $('#vendedorSeguimiento').val();
let usuario = $('#UsuarioSeguimiento').val();
let suceso = $('#SucesoSeguimiento').val();
let cobrador = $('#CobradorSeguimiento').val();
let FModEstadoPedido = $('#FechaModEstadoPedido').val().split('/').reverse().join('-');
let FModEstadoFactura = $('#FechaModEstadoFactura').val().split('/').reverse().join('-');
let FModSeguiFactura_rango_ini = "";
let FModSeguiFactura_rango_fin = "";

var isChecked = $("#checkboxfecha_sf").prop('checked');
if (isChecked) {
    try {

        FModSeguiFactura_rango_ini = $('#FechaRuta_sf').val().split(" - ")[0].split('/').reverse().join('-');
        FModSeguiFactura_rango_fin = $('#FechaRuta_sf').val().split(" - ")[1].split('/').reverse().join('-');
    } catch (error) {

        FModSeguiFactura_rango_ini = "Todas";
        FModSeguiFactura_rango_fin = "Todas";
    }
} else {
    FModSeguiFactura_rango_ini = "Todas";
    FModSeguiFactura_rango_fin = "Todas";
}





var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#miTabla").DataTable({
            searchDelay: 500,
            processing: true,
            
            scrollX: true,
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },
            //data: cotizaciones,

            "bDestroy": true,
            destroy : true,
            //serverSide: true,
            ajax: {
                url: `${URL_SEGUIMIENTO}/Obtener_Seguimiento`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d){
                        d.fechaInicio = fechaInicio,
                        d.fechaFin = fechaFin,
                        d.estado_pedido = estado_pedido,
                        d.fecha_prometida = fecha_prometida,
                        d.estado_factura = estado_factura,
                        d.rutamulti = rutamulti,
                        d.fpedido = fpedido,
                        d.ffactura = ffactura,
                        d.fcliente = fcliente,
                        d.agente = agente,
                        d.usuario = usuario,
                        d.suceso = suceso,
                        d.cobrador = cobrador,
                        d.FModEstadoPedido = FModEstadoPedido,
                        d.FModEstadoFactura = FModEstadoFactura,
                        d.FModSeguiFactura_rango_ini = FModSeguiFactura_rango_ini,
                        d.FModSeguiFactura_rango_fin = FModSeguiFactura_rango_fin
                }
            },
            columns: [

                {
                    "data": null,
                    "defaultContent": ``
                },
                { "data": "PEDIDO" },

                { "data": "FACTURA" },
                {
                    "data": "FECHA_FACTURA"
                },
                {
                    "data": "ESTADO_PEDIDO_DESCRIPCION",
                },

                {
                    "data": "FECHA_CAMBIA_ESTADO_PED",
                },
                {
                    "data": "SEGUIR_FAC_DESCRIPCION",
                },
                {
                    "data": "FECHA_CAMBIA_ESTADO_FACTURA",
                },
                {
                    "data": "SUCESO",
                },
                {
                    "data": "USUARIO",
                },
                {
                    "data": "OBSERVACIONES",
                },
                {
                    "data": "FECHA_HORA_RECIBIDO",
                },
                {
                    "data": "FECHA_PEDIDO",
                },
                {
                    "data": "DIA_RUTA",
                },
                {
                    "data": "RUTA",
                },
                {
                    "data": "CLIENTE",
                },
                {
                    "data": "AGENTE",
                },
                {
                    "data": "COBRADOR",
                },
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: function (data, type, row) {
                        return `
                            <div class="form-check form-check-sm form-check-custom form-check-solid">
                                <input class="form-check-input" type="checkbox" value="${row.PEDIDO}" />
                            </div>`;
                    }
                },
                {
                    targets: 4,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<span class="badge ${row.COLOR_HEXA_PED.replace("label", "badge")} badge-lg">${row.ESTADO_PEDIDO_DESCRIPCION}</span>`;
                    }
                },
                {
                    targets: 6,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<span class="badge ${row.COLOR_HEXA_FAC.replace("label", "badge")} badge-lg">${row.SEGUIR_FAC_DESCRIPCION}</span>`;
                    }
                },
               
            ],
            //}
        });
        table = dt.$;

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        dt.on('draw', function () {
            $('#totallineas').html(`Cant. Seg: ${dt.data().length}`);
            const container = document.querySelector('#miTabla');
            const checkboxes = container.querySelectorAll('[type="checkbox"]');
            // Toggle delete selected toolbar
            checkboxes.forEach(c => {
                // Checkbox on click event
                c.addEventListener('click', function () {
                    setTimeout(function () {
                        ManejarCantidadCheckboxes();
                    }, 50);
                });
            });
        });
    }
    var resetTable = function () {
        //dt = $("#miTabla").DataTable();

        setTimeout(() => {
           
            dt.ajax.reload();
        }, 100)

        dt.clear().draw()
    }

    // Add a click event handler to the "checkAll" checkbox in the header
    $('#checkAll').on('click', function () {

        var isChecked = $("#checkAll").prop('checked');
        if (isChecked) {
            $(".multi").removeClass("d-none")
        } else {
            $(".multi").addClass("d-none")
        }
    });

    // Add a click event handler to the checkboxes in the body (tbody)
    $('#miTabla').on('click', 'input[type="checkbox"]', function () {
        // Check or uncheck the "checkAll" checkbox in the header based on the checkboxes in the body
        //var allChecked = dataTable.column(0).nodes().to$().find('input[type="checkbox"]:checked').length === dataTable.data().length;
        //$('#checkAll').prop('checked', allChecked);
    });
    var Filtros = function () {

        $('#kt_datatable_search_query').on('keyup', function () {
            dt.search($(this).val()).draw();
        });
    }
    var ManejarCantidadCheckboxes = () => {
        // Select all delete buttons

        // Select refreshed checkbox DOM elements

        const container = document.querySelector('#miTabla');
        const allCheckboxes = container.querySelectorAll('[type="checkbox"]');

        // Detect checkboxes state & count
        let checkedState = false;
        let count = 0;

        // Count checked boxes
        allCheckboxes.forEach(c => {
            if (c.checked) {
                checkedState = true;
                count++;
            }
        });
        if (checkedState) {

            //$("#cantselect").text(count)
            $(".multi").removeClass("d-none")
        } else {
            $(".multi").addClass("d-none")
        }

    }

    // Public methods
    return {
        init: function () {
            initDatatable();
            Filtros();
            //setTimeout(() => { ListenerCheckboxes() }, 5000)
            
        },
        reset: function () {
            resetTable();

        }

    }
}();


$("#checkboxfecha_sf").change(function (value) {
    var checked = $("#checkboxfecha_sf").is(':checked');
    if (checked) {
        $("#FechaRuta_sf").attr("disabled", false);
        //$("#FechaRuta_sf").val("");

    } else {
        $("#FechaRuta_sf").attr("disabled", true);
        //$("#FechaRuta_sf").val("Todas");


        //setTimeout(() => { window.location.
        //setTimeout(() => { window.location.reload(); }, 100)
    }

    ManejarFiltros();
})

$("#checkboxfecha").change(function (value) {
    var checked = $("#checkboxfecha").is(':checked');
    if (checked) {
        $("#FechaInicio2Seguimiento").attr("disabled", false);
        $("#FechaInicio2Seguimiento").val("");

    } else {
        $("#FechaInicio2Seguimiento").attr("disabled", true);
        $("#FechaInicio2Seguimiento").val("Todas");
    }

    ManejarFiltros();
})
function InicializarFechas() {
    new tempusDominus.TempusDominus(document.getElementById("FechaInicio2Seguimiento"), {
        localization: {
            locale: 'default',

            format: "dd/MM/yyyy"
        },
        display: {
            viewMode: "calendar",
            components: {
                decades: true,
                year: true,
                month: true,
                date: true,
                hours: false,
                minutes: false,
                seconds: false
            }
        },
        
    });
    new tempusDominus.TempusDominus(document.getElementById("FechaModEstadoPedido"), {
        localization: {
            locale: 'default',

            format: "dd/MM/yyyy"
        },
        display: {
            viewMode: "calendar",
            components: {
                decades: true,
                year: true,
                month: true,
                date: true,
                hours: false,
                minutes: false,
                seconds: false
            }
        }
    });
    new tempusDominus.TempusDominus(document.getElementById("FechaModEstadoFactura"), {
        localization: {
            locale: 'default',

            format: "dd/MM/yyyy"
        },
        display: {
            viewMode: "calendar",
            components: {
                decades: true,
                year: true,
                month: true,
                date: true,
                hours: false,
                minutes: false,
                seconds: false
            }
        }
    });
    
    $("#FechaSeguimiento").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });
    $("#FechaRuta_sf").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });
}



function ManejarFiltros() {
    ActualizarFiltros()
    //KTDataTable.reset()
    //$('#miTabla tbody').remove()
    setTimeout(() => {

        KTDatatablesServerSide.reset()
    }, 300)

}
function DescargarExcel() {
    let _estado_pedido = $('#filtrestadopedidoSeguimiento').val();
    let _estado_factura = $('#estadoSeguimiento').val();

    let valoresCheck = [];
    $("#miTabla input:checkbox:checked").each(function () {
        valoresCheck.push(this.value);
    });
    const str = valoresCheck.join(',');
    console.log(str)
    /*	var _Fecha = $('#FechaInicio2').val().split('/').reverse().join('-')*/
    Dajax(`${URL_SEGUIMIENTO}/GenerarExcelSeguimientos`,
        "POST", { PEDIDO: str, ESTADO_PEDIDO: _estado_pedido, ESTADO_FACTURA: _estado_factura }, function (response) {
            let ruta = `${URL_SEGUIMIENTO}/DescargarArchivo?ruta=${response.responseJSON}`
            if (response.responseJSON != 2) {
                toastr.success("¡Reporte de seguimientos descargado con éxito!", "Éxito", toastrOptions);
            } else {
                toastr.warning("Error al descargar el reporte de seguimientos, inténtelo de nuevo más tarde.", "Error", toastrOptions);
            }
            setTimeout(() => { window.location.href = ruta; }, 1000)
            //      toastr.success("Excel generado con éxito!", "Excel", toastrOptions);
        });
}

function abrirmodalCorreos() {
    $("#modalenviocorreoSucesos").modal("show")
    new tempusDominus.TempusDominus(document.getElementById("FechaEnvio"), {
        localization: {
            locale: 'default',

            format: "dd/MM/yyyy"
        },
        useCurrent: true,
        display: {
            viewMode: "calendar",
            components: {
                decades: true,
                year: true,
                month: true,
                date: true,
                hours: false,
                minutes: false,
                seconds: false
            }
        }
    });
}
function enviarCorreo() {
    var _Fecha = $('#FechaEnvio').val().split('/').reverse().join('-')
    var _Email = $('#correos').val()
    Dajax(`${URL_SEGUIMIENTO}/EnviarReportesRutas`,//_url + _sistema + "EstadoCuenta/EnviarEstadoCuenta",
        "POST", { fechaPrometida: _Fecha, CORREO: _Email }, function (response) {
            if (response.responseJSON == 1) {
                toastr.success("¡Se ha enviado el reporte de seguimientos con éxito!", "Enviado", toastrOptions);
                //setTimeout(function () { ir_a("Pedido", "Index"); }, 2000);
            } else {
                toastr.warning("Error al enviar el reporte de seguimientos, inténtelo de nuevo más tarde.", "Error", toastrOptions);
            }
        });
    $("#modalenviocorreoSucesos").modal("hide")
}
function ActualizarFiltros() {
     fechaInicio = $('#FechaSeguimiento').val().split(" - ")[0].split('/').reverse().join('-');
    fechaFin = $('#FechaSeguimiento').val().split(" - ")[1].split('/').reverse().join('-');
    var checked = $("#checkboxfecha").prop('checked');
    if (checked) {

        fecha_prometida = $('#FechaInicio2Seguimiento').val().split('/').reverse().join('-');
    } else {
        fecha_prometida = "Todas"
    }
     fpedido = $('#pedidoSeguimiento').val();
     ffactura = $('#facturaSeguimiento').val();
     fcliente = $('#clienteSeguimiento').val();
    estado_pedido = $('#filtrestadopedidoSeguimiento').val();
    estado_factura = $('#estadoSeguimiento').val();
    rutamulti = `(${$("#rutaSeguimiento").val().map(item => `'${item}'`).join(',')})`;
     agente = $('#vendedorSeguimiento').val();
     usuario = $('#UsuarioSeguimiento').val();
     suceso = $('#SucesoSeguimiento').val();
     cobrador = $('#CobradorSeguimiento').val();
     FModEstadoPedido = $('#FechaModEstadoPedido').val().split('/').reverse().join('-');
    FModEstadoFactura = $('#FechaModEstadoFactura').val().split('/').reverse().join('-');
    var isChecked = $("#checkboxfecha_sf").prop('checked');
    if (isChecked) {
        try {

            FModSeguiFactura_rango_ini = $('#FechaRuta_sf').val().split(" - ")[0].split('/').reverse().join('-');
            FModSeguiFactura_rango_fin = $('#FechaRuta_sf').val().split(" - ")[1].split('/').reverse().join('-');
        } catch (error) {

            FModSeguiFactura_rango_ini = "Todas";
            FModSeguiFactura_rango_fin = "Todas";
        }
    } else {
        FModSeguiFactura_rango_ini = "Todas";
        FModSeguiFactura_rango_fin = "Todas";
    }

}
function LimpiarFiltros() {
    $('#pedidoSeguimiento').val(0);
    $('#facturaSeguimiento').val(0);
    $('#clienteSeguimiento').val(0);
    $('#filtrestadopedidoSeguimiento').val(0);
    $('#estadoSeguimiento').val(0);
    $('#vendedorSeguimiento').val(0);
    $('#rutaSeguimiento').val(0);
    $('#UsuarioSeguimiento').val(0);
    $('#SucesoSeguimiento').val(0);
    window.location.reload();
}

function InicializarSelects() {
    $('#pedidoSeguimiento').select2({
        language: "es",
        placeholder: 'Filtrar por un pedido',
        minimumInputLength: 4,
        ajax: {
            url: `${URL_SEGUIMIENTO}/Obtener_Cod_PedidoSeg_Busqueda`,
            dataType: 'json',
            type: 'GET',
            processResults(data) {
                const todos = [{ text: "Todos", id: "0" }]
                const results = $.map(data, function (item) {
                    return {
                        text: item.PEDIDO,
                        id: item.PEDIDO
                    }
                })
                const options = todos.concat(results)
                return {
                    results: options
                }
            }
        }
    })


    $('#facturaSeguimiento').select2({
        language: "es",
        placeholder: 'Filtrar por una factura',
        minimumInputLength: 4,
        ajax: {
            url: `${URL_SEGUIMIENTO}/Obtener_Cod_FacturaSeg_Busqueda`,
            dataType: 'json',
            type: 'GET',
            processResults(data) {
                const todos = [
                    { text: "Todos", id: "0" },
                    { text: "Con facturas", id: "99" }
                ]
                const results = $.map(data, function (item) {
                    return {
                        text: item.FACTURA,
                        id: item.FACTURA
                    }
                })
                const options = todos.concat(results)
                return {
                    results: options
                }
            }
        }
    })
}


function PurgarBloqueos() {

    Swal.fire({
        text: `¿Desea purgar los bloqueos?`,
        icon: "warning",
        showCancelButton: true,
        buttonsStyling: false,
        confirmButtonText: "Aceptar",
        cancelButtonText: "Cancelar",
        customClass: {
            confirmButton: "btn font-weight-bold btn-primary",
            cancelButton: "btn font-weight-bold btn-default"
        }
    }).then(function (result) {
        if (result.value) {
            Dajax(`${URL_SEGUIMIENTO}/PurgarBloqueo`,
                "POST", {}, function (response) {
                    if (response.responseJSON == "1") {
                        toastr.success('¡Purga de bloqueos realizada con éxito!', 'Éxito', toastrOptions);
                    } else {
                        toastr.warning('Hubo un error al purgar los bloqueos, inténtelo de nuevo más tarde.', 'Error', toastrOptions)
                    }
                    /*setTimeout(() => { window.location.reload(); }, 2000)*/
                    //KTDataTable.reset();
                });
        }
    });

}
$(document).ready(function () {
    
    InicializarSelects()
    InicializarFechas()
    KTDatatablesServerSide.init()

    //ManejarFiltros();
});