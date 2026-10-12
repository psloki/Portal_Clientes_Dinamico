let fechaInicio = $('#FechaInicioFin').val().split(" - ")[0].split('/').reverse().join('-');
let fechaFin = $('#FechaInicioFin').val().split(" - ")[1].split('/').reverse().join('-');

let fecha_prometida = "";
let fecha_prometidaFin = "";
var isChecked = $("#checkboxfecha").prop('checked');
if (isChecked) {
    try {

        fecha_prometida = $('#FechaRuta').val().split(" - ")[0].split('/').reverse().join('-');
        fecha_prometidaFin = $('#FechaRuta').val().split(" - ")[1].split('/').reverse().join('-');
    } catch (error) {

        fecha_prometida = "Todas";
        fecha_prometidaFin = "Todas";
    }
} else {
    fecha_prometida = "Todas";
    fecha_prometidaFin = "Todas";
}

let fecha_CxcRecibe_Ini = "";
let fecha_CxcRecibe_Fin = "";
var isCheckedcxc = $("#checkboxfechaCxcRecibe").prop('checked');
if (isCheckedcxc) {
    try {

        fecha_CxcRecibe_Ini = $('#FechaCxcRecibe').val().split(" - ")[0].split('/').reverse().join('-');
        fecha_CxcRecibe_Fin = $('#FechaCxcRecibe').val().split(" - ")[1].split('/').reverse().join('-');
    } catch (error) {

        fecha_CxcRecibe_Ini = "Todas";
        fecha_CxcRecibe_Fin = "Todas";
    }
} else {
    fecha_CxcRecibe_Ini = "Todas";
    fecha_CxcRecibe_Fin = "Todas";
}
let estado_pedido = $('#filtrestadopedido').val();
let estado_factura = $('#estado').val();
let rutamulti = `(${$("#ruta").val().map(item => `'${item}'`).join(',')})`;
let fpedido = $('#pedido').val();
let ffactura = $('#factura').val();
let fcliente = $('#cliente').val();
let tla_estado = $("#tlaestado :selected").map((_, e) => e.value).get();
let concatenar = "";
let cocatenar_tla = "";
let corte_tla = $('#horascorte').val();
let agente = $('#vendedor').val();
let dia_semana = $("#kt_select2_9 :selected").map((_, e) => e.value).get();
let horaencortefuera = $('#HORA_FUERA_ENCORTE').val();
let cocatenar_diasemana = "";
let _pedido = "";
let hora_recibido = "";
let hora_corte = "";
let opciones_tla = "";
let _subIdSeguimiento = "";
let _subIdPedido = "";
let _IdPedSuceso = "";
let _IdSuceso = "";
let contador = 0;
let cobrador = $('#CobradorTracking').val();



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
            lengthMenu: [
                [10, 100, 200, 500, -1],
                [10, 100, 200, 500, 'Todos']
            ],
            pageLength: 500,
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },
            //data: cotizaciones,

            "bDestroy": true,
            destroy: true,
            //serverSide: true,
            ajax: {
                url: `${URL}/Obtener_PedidoTracking`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                        d.fechaInicio = fechaInicio,
                        d.fechaFin = fechaFin,
                        d.estado_pedido = estado_pedido,
                        d.fecha_prometida = fecha_prometida,
                        d.fecha_prometidaFin = fecha_prometidaFin,
                        d.estado_factura = estado_factura,
                        d.concatenar = rutamulti,
                        d.fpedido = fpedido,
                        d.ffactura = ffactura,
                        d.fcliente = fcliente,
                        d.cocatenar_tla = cocatenar_tla,
                        d.corte_tla = corte_tla,
                        d.agente = agente,
                        d.cobrador = cobrador,
                        d.cocatenar_diasemana = cocatenar_diasemana,
                        d.horaencortefuera = horaencortefuera,
                        d.fecha_CxcRecibe_Ini = fecha_CxcRecibe_Ini,
                        d.fecha_CxcRecibe_Fin = fecha_CxcRecibe_Fin
                }
            },
            columns: [

                {
                    "data": null,
                    "defaultContent": ``
                },
                {
                    "data": null,
                    "defaultContent": `<a href="javascript:;"><i class="fa fa-caret-right verSubtabla" style="width: 10px;"></i></a>`
                },
                { "data": "PEDIDO" },

                { "data": "FACTURA" },
                {
                    "data": "OBSERVACIONES"
                },
                {
                    "data": "TIPO_CORTE",
                },

                {
                    "data": "FECHA_PEDIDO",
                },
                {
                    "data": "FECHA_HORA_RECIBIDO",
                },
                {
                    "data": "ESTADO_PEDIDO_DESCRIPCION",
                },
                {
                    "data": "DIA_RUTA",
                },
                {
                    "data": "SEGUIR_FAC_DESCRIPCION",
                },

                {
                    "data": "CLIENTE",
                },
                {
                    "data": null,
                },
                {
                    "data": "RUTA",
                },
                {
                    "data": "AGENTE",
                },
                {
                    "data": "DIAS",
                },
                {
                    "data": "TLA_ESTADO",
                },
                {
                    "data": "TIEMPO",
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
                    targets: 2,
                    orderable: true,
                    render: function (data, type, row) {
                        if (row.SUCESOS == 0) {
                            return `<span class="badge badge-light-warning badge-lg">${row.PEDIDO}</span>`
                        } else {
                            return row.PEDIDO
                        }
                    }
                },
                {
                    targets: 7,
                    orderable: false,
                    render: function (data, type, row) {
                        var status = ""
                        if (row.HORA_FUERA_ENCORTE == "1") {
                            status = '<span class="badge badge-light-success badge-lg">' + row.FECHA_HORA_RECIBIDO + '</span>';
                        }
                        else {
                            status = '<span class="badge badge-light-warning badge-lg">' + row.FECHA_HORA_RECIBIDO + '</span>';
                        }
                        return status
                        
                    }
                },
                {
                    targets: 8,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<span class="badge ${row.COLOR_HEXA_PED.replace("label", "badge")} badge-lg">${row.ESTADO_PEDIDO_DESCRIPCION}</span>`;
                    }
                },
                {
                    targets: 10,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<span class="badge ${row.COLOR_HEXA_FAC.replace("label", "badge")} badge-lg">${row.SEGUIR_FAC_DESCRIPCION}</span>`;
                    }
                },
                {
                    targets: 12,
                    orderable: false,
                    render: function (data, type, row) {
                        let accionesTracking = "";
                        if (PERMISOS_MOD_TRACKING_IN.split(',').includes("12")) {
                            accionesTracking += `<a href="javascript:;" class="btn btn-sm btn-clean btn-icon me-2" title="Editar observaciones"  onclick="modalAccion('${row.PEDIDO}', '${row.RUTA}', '${row.OBSERVACIONES}');">
	                         <span class="svg-icon svg-icon-md svg-icon-warning svg-icon-2x">
							<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
								<g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
									<rect x="0" y="0" width="24" height="24" />
									<path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero"\ transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
					                <rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1" />
								</g>
							</svg>
						</span>
	                        </a>`
                        }

                        if (PERMISOS_MOD_TRACKING_IN.split(',').includes("13")) {
                            accionesTracking += `<a href="javascript:;" class="btn btn-sm btn-clean btn-icon me-2" title="Asignar suceso" onclick="abrirModalSucesos('${row.PEDIDO}','${row.SUCESO}');">
						<span class="svg-icon svg-icon-md svg-icon-warning svg-icon-2x">
							<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
								 <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
									<rect x="0" y="0" width="24" height="24"/>
									<path d="M11.1669899,4.49941818 L2.82535718,19.5143571 C2.557144,19.9971408 2.7310878,20.6059441 3.21387153,20.8741573 C3.36242953,20.9566895 3.52957021,21 3.69951446,21 L21.2169432,21 C21.7692279,21 22.2169432,20.5522847 22.2169432,20 C22.2169432,19.8159952 22.1661743,19.6355579 22.070225,19.47855 L12.894429,4.4636111 C12.6064401,3.99235656 11.9909517,3.84379039 11.5196972,4.13177928 C11.3723594,4.22181902 11.2508468,4.34847583 11.1669899,4.49941818 Z" fill="currentColor" opacity="0.3"/>
									<rect fill="currentColor" x="11" y="9" width="2" height="7" rx="1"/>
									<rect fill="currentColor" x="11" y="17" width="2" height="2" rx="1"/>
								</g>
							</svg>
						</span>
					</a>`
                        }
                        return accionesTracking ;
                    }
                },

            ],
        });
        table = dt.$;
        dt.on('draw', function () {
            $('#totallineas').html(`Cant. Pedidos: ${dt.data().length}`);
            const container = document.querySelector('#miTabla');
            const checkboxes = container.querySelectorAll('[type="checkbox"]');
            checkboxes.forEach(c => {
                
                c.addEventListener('click', function () {
                    setTimeout(function () {
                        ManejarCantidadCheckboxes();
                    }, 50);
                });
            });
        });
    }
    var resetTable = function () {
        setTimeout(() => {

            dt.ajax.reload();
        }, 100)

        dt.clear().draw()
    }
    $('#checkAll').on('click', function () {

        var isChecked = $("#checkAll").prop('checked');
        if (isChecked) {
            $(".multi").removeClass("d-none")
        } else {
            $(".multi").addClass("d-none")
        }
    });
    
    $('#miTabla').on('click', 'input[type="checkbox"]', function () {
        var allChecked = dt.column(0).nodes().to$().find('input[type="checkbox"]:checked').length === dt.data().length;
        $('#checkAll').prop('checked', allChecked);
    });
    var Filtros = function () {
        $('#kt_datatable_search_query').on('keyup', function () {
            dt.search($(this).val()).draw();
        });
        $('#kt_select2_10').on('change', function () {
            var val = $(this).val()
            var arraybusqueda = []
            var searchString = ''
            try {
                val = JSON.parse(val)
                val.forEach(element => { arraybusqueda.push(element.value) })
                searchString = arraybusqueda.join('|');
            } catch (error) {
                searchString = ""
            }
            
            dt.search(searchString, true, false).draw();
        });
    }
    var ManejarCantidadCheckboxes = () => {
        const container = document.querySelector('#miTabla');
        const allCheckboxes = container.querySelectorAll('[type="checkbox"]');
        let checkedState = false;
        let count = 0;
        allCheckboxes.forEach(c => {
            if (c.checked) {
                checkedState = true;
                count++;
            }
        });
        if (checkedState) {
            $(".multi").removeClass("d-none")
        } else {
            $(".multi").addClass("d-none")
        }
    }


    var subtabla = function () {
        $('#miTabla tbody').on('click', '.verSubtabla', function () {
            var tr = $(this).closest('tr');
            var row = dt.row(tr);
            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                //construirsubtabla(subtabladatos);
                let datos = row.data()
                row.child(`
                    <table class="table  table-row-dashed fs-6 gy-4" id="subtabla${datos.PEDIDO.replace('|','')}">
                        <thead>
                            <tr class="text-start text-gray-600 fw-bold fs-7 text-uppercase gs-0">
                                <th class="text-left min-w-100px">Id segumiento</th>
                                <th class="text-left min-w-150px">Estado PED</th>
                                <th class="text-left min-w-150px">Seguir FAC</th>

                                <th class="text-left min-w-75px">TLA</th>
                                <th class="text-left min-w-75px">Usuario</th>
                                <th class="text-left min-w-100px">Modifica Ped.</th>

                                <th class="text-left min-w-100px">Modifica Seg.</th>
                                <th class="text-left min-w-75px">Suceso</th>
                                <th class="text-left min-w-75px">Observaciones</th>
                                <th class="text-left min-w-900px">Acciones</th>
                            </tr>
                        </thead>
                        <tbody class="text-gray-600 fw-semibold">
                        </tbody>
                    </table>
                `).show();
                $(`#subtabla${datos.PEDIDO.replace('|', '')}`).DataTable({
                    ajax: {
                        url: `${URL_SEGUIMIENTO}/Obtener_SubTablaSeguimiento_Tracking?Pedido=${datos.PEDIDO}`,
                        dataSrc: ""
                    },
                    destroy: true,
                    columns: [


                        { "data": "SEGUIMIENTO" },
                        { "data": "ESTADO_PEDIDO_DESCRIPCION" },
                        { "data": "SEGUIR_FAC_DESCRIPCION" },
                        { "data": "TLAESTADO" },
                        { "data": "USUARIO" },
                        { "data": "FECHA_CAMBIA_ESTADO_PED" },
                        { "data": "FECHA" },
                        { "data": "SUCESO" },
                        { "data": "OBSERVACIONES" },
                        {
                            "data": null,
                        },
                    ],
                    columnDefs: [
                        {
                            targets: 1,
                            orderable: false,
                            render: function (data, type, row) {
                                return `<span class="badge ${row.COLOR_HEXA_PED.replace("label", "badge")} badge-lg">${row.ESTADO_PEDIDO_DESCRIPCION}</span>`;
                            }
                        },
                        {
                            targets: 2,
                            orderable: false,
                            render: function (data, type, row) {
                                return `<span class="badge ${row.COLOR_HEXA_FAC.replace("label", "badge")} badge-lg">${row.SEGUIR_FAC_DESCRIPCION}</span>`;
                            }
                        },
                        {
                            targets: 9,
                            orderable: false,
                            render: function (data, type, row) {
                                if (!TRACKING_IN) {
                                    return ''
                                }
                                return `
					                    <a href="javascript:;" class="btn btn-sm btn-clean btn-icon mr-2" title="Eliminar" onclick="DeleteSeguimiento('${row.SEGUIMIENTO}', '${row.PEDIDO}');">
						                    <span class="svg-icon svg-icon-md svg-icon-danger svg-icon-2x">
							                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
								                     <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24"/>
                                                             <circle fill="currentColor" opacity="0.3" cx="12" cy="12" r="10"/>
                                                            <path d="M12.0355339,10.6213203 L14.863961,7.79289322 C15.2544853,7.40236893 15.8876503,7.40236893 16.2781746,7.79289322 C16.6686989,8.18341751 16.6686989,8.81658249 16.2781746,9.20710678 L13.4497475,12.0355339 L16.2781746,14.863961 C16.6686989,15.2544853 16.6686989,15.8876503 16.2781746,16.2781746 C15.8876503,16.6686989 15.2544853,16.6686989 14.863961,16.2781746 L12.0355339,13.4497475 L9.20710678,16.2781746 C8.81658249,16.6686989 8.18341751,16.6686989 7.79289322,16.2781746 C7.40236893,15.8876503 7.40236893,15.2544853 7.79289322,14.863961 L10.6213203,12.0355339 L7.79289322,9.20710678 C7.40236893,8.81658249 7.40236893,8.18341751 7.79289322,7.79289322 C8.18341751,7.40236893 8.81658249,7.40236893 9.20710678,7.79289322 L12.0355339,10.6213203 Z" fill="currentColor"/>
                                                         </g>
							                    </svg>
						                    </span>
					                    </a>`
                            }
                        },
                    ],

                });

                $(`#subtabla${datos.PEDIDO.replace('|', '')}_wrapper`).addClass("ms-20")
                //row.child(construirsubtabla(row.data().LINEAS)).show();
                tr.addClass('shown');
            }
        });
    }

    // Public methods
    return {
        init: function () {
            initDatatable();
            Filtros();
            subtabla()
            //setTimeout(() => { ListenerCheckboxes() }, 5000)

        },
        reset: function () {
            resetTable();

        }

    }
}();

function ActualizarFiltros() {
    concatenar = "";
    cocatenar_tla = "";
    cocatenar_diasemana = "";
     fechaInicio = $('#FechaInicioFin').val().split(" - ")[0].split('/').reverse().join('-');
     fechaFin = $('#FechaInicioFin').val().split(" - ")[1].split('/').reverse().join('-');

     fecha_prometida = "";
     fecha_prometidaFin = "";
     isChecked = $("#checkboxfecha").prop('checked');
    if (isChecked) {
        try {

            fecha_prometida = $('#FechaRuta').val().split(" - ")[0].split('/').reverse().join('-');
            fecha_prometidaFin = $('#FechaRuta').val().split(" - ")[1].split('/').reverse().join('-');
        } catch (error) {

            fecha_prometida = "Todas";
            fecha_prometidaFin = "Todas";
        }
    } else {
        fecha_prometida = "Todas";
        fecha_prometidaFin = "Todas";
    }

     fecha_CxcRecibe_Ini = "";
     fecha_CxcRecibe_Fin = "";
     isCheckedcxc = $("#checkboxfechaCxcRecibe").prop('checked');
    if (isCheckedcxc) {
        try {

            fecha_CxcRecibe_Ini = $('#FechaCxcRecibe').val().split(" - ")[0].split('/').reverse().join('-');
            fecha_CxcRecibe_Fin = $('#FechaCxcRecibe').val().split(" - ")[1].split('/').reverse().join('-');
        } catch (error) {

            fecha_CxcRecibe_Ini = "Todas";
            fecha_CxcRecibe_Fin = "Todas";
        }
    } else {
        fecha_CxcRecibe_Ini = "Todas";
        fecha_CxcRecibe_Fin = "Todas";
    }
     estado_pedido = $('#filtrestadopedido').val();
     estado_factura = $('#estado').val();
     rutamulti = `(${$("#ruta").val().map(item => `'${item}'`).join(',')})`;
     fpedido = $('#pedido').val();
     ffactura = $('#factura').val();
     fcliente = $('#cliente').val();
     cobrador = $('#CobradorTracking').val();
     tla_estado = ($("#tlaestado :selected").map((_, e) => {
        if ($(e).push('selected')) {
            cocatenar_tla += e.value + ',';
        }
    }));
    corte_tla = $('#horascorte').val();
    agente = $('#vendedor').val();
    dia_semana = ($("#kt_select2_9 :selected").map((_, e) => {
        if ($(e).push('selected')) {
            cocatenar_diasemana += e.value + ',';
        }
    }));
     horaencortefuera = $('#HORA_FUERA_ENCORTE').val();
}


function ManejarFiltros() {
    ActualizarFiltros()
    setTimeout(() => {

        KTDatatablesServerSide.reset()
    }, 300)
}

function LimpiarFiltros() {
    $('#pedido').val(0);
    $('#factura').val(0);
    $('#cliente').val(0);
    $('#filtrestadopedido').val(0);
    $('#estado').val(0);
    $('#tlaestado').val(0);
    $('#horascorte').val(0);
    $('#vendedor').val(0);
    $('#CobradorTracking').val(0);
    $('#ruta').val(0);
    $('#kt_select2_9').val(0);
    $('#HORA_FUERA_ENCORTE').val(0);
    window.location.reload();
}

$("#checkboxfecha").change(function (value) {
    var checked = $("#checkboxfecha").is(':checked');
    if (checked) {
        $("#FechaRuta").attr("disabled", false);
        $("#FechaRuta").val("");

    } else {
        $("#FechaRuta").attr("disabled", true);
        $("#FechaRuta").val("Todas");

        //setTimeout(() => { window.location.
        //setTimeout(() => { window.location.reload(); }, 100)
    }

    ManejarFiltros();
})


$("#checkboxfechaCxcRecibe").change(function (value) {
    var checked = $("#checkboxfechaCxcRecibe").is(':checked');
    if (checked) {
        $("#FechaCxcRecibe").attr("disabled", false);
        //$("#FechaCxcRecibe").val("");

    } else {
        $("#FechaCxcRecibe").attr("disabled", true);
        //$("#FechaCxcRecibe").val("Todas");

        //setTimeout(() => { window.location.
        //setTimeout(() => { window.location.reload(); }, 100)
    }

    ManejarFiltros();
})

function cambioestadoped(PEDIDO, OBSERVACIONES, ESTADO_PEDIDO_DESCRIPCION) {
    
    $("#estadoPedido").text(`Cambiar estado PED ${PEDIDO}`)
    $('#asignarestadopedido').val(ESTADO_PEDIDO_DESCRIPCION).trigger("change")
    console.log(ESTADO_PEDIDO_DESCRIPCION)
    $('#Observacionesseges2').val(OBSERVACIONES).trigger("change")
    $("#CambiarestadoPedido").modal("show")
    _pedido = PEDIDO;
}

function guardarEstadoPedido() {
    let _estado = $("#asignarestadopedido").val();
    let _Observacion_Estado_Pedido = $("#Observacionesseges2").val();
    let _usuario = $("#entregando2").val();
    let _fechaCambiaEstadoPedido = $('#FechaPedidoIndividual').val().split('/').reverse().join('-');
    let __correoCEPIndividual = $("#correosPedidoIndividual").val();

    Dajax(`${URL}/ActualizaEstadoPedido`,
        "POST", { PEDIDO: _pedido, ESTADO: _estado, FECHA: _fechaCambiaEstadoPedido, OBSERVACION: _Observacion_Estado_Pedido, USUArio: _usuario, CORREO: __correoCEPIndividual }, function (response) {
            if (response.responseJSON == "1") {
                toastr.success('Se ha cambiado el estado con éxito', 'Éxito', toastrOptions);
            } else {
                toastr.warning('Ha ocurrido un error, por favor inténtelo más tarde', 'Error', toastrOptions)
            }
        });

    $("#CambiarestadoPedido").modal("hide")
    setTimeout(() => { window.location.reload(); }, 2000)

    /*KTDataTable.reset();*/

}

function modalAccion(PEDIDO, RUTA, OBSERVACIONES) {

    if (PEDIDO == '') {
        $("#acciontitulo").text(`Seguir FAC para múltiples pedidos`)
    } else {
        $("#acciontitulo").text(`Observaciones del pedido ${PEDIDO}`)
        $('#Observacionessege').val(OBSERVACIONES).trigger("change")

    }

    $("#haceraccion").modal("show")
    _pedido = PEDIDO;
}

function Guardar_ObservacionPed() {
    var _Observacion = $("#Observacionessege").val();
    var _usuario = $("#na").val();

    Dajax(`${URL}/Actualiza_Observacion`,
        "POST", { PEDIDO: _pedido, OBSERVACION: _Observacion, USUARIO: _usuario }, function (response) {
            if (response.responseJSON == "1") {
                toastr.success("Se han actualizado las observaciones del pedido con éxito", "Éxito", toastrOptions);
            } else {
                toastr.warning("Hubo un error actualizando las observaciones, inténtelo más tarde", "Error", toastrOptions);
            }
        });

    $("#haceraccion").modal("hide")
    setTimeout(() => { window.location.reload(); }, 2000)
    //KTDataTable.reset();
}

function asigRuta(PEDIDO, DIA_RUTA, RUTA) {
    if (PEDIDO == '') {
        $("#rutaunica").addClass('d-none')
    } else {
        $("#rutaunica").removeClass('d-none')
    }
    $("#asignaruta").text(`Fecha Prometida del Pedido: ${PEDIDO}`)
    $("#rutaunica").val(RUTA).trigger("change")
    $("#fecharutaPrometida").val(DIA_RUTA).trigger("change")
    $("#AsignarFechaUnica").modal("show")
    _pedido = PEDIDO;
}

function guardar_FechaPrometidaUnica() {
    var _Fecha = $('#fecharutaPrometida').val().split('/').reverse().join('-');
    var _usuario = $("#entregando2").val();
    //$("input:checkbox:checked").each(function () {
    //	var _pedido = $(this).val();

    Dajax(`${URL}/Actualiza_Fecha_Ruta`,
        "POST", { PEDIDO: _pedido, FECHA: _Fecha, USUARIO: _usuario }, function (response) {
            if (response.responseJSON == "1") {

                toastr.success("Se ha asignado el pedido al día y la ruta seleccionada", "Éxito", toastrOptions);
            } else {
                toastr.warning("Hubo un error al asignar el pedido al día y la ruta, ", "Error", toastrOptions);
            }
        });
    /*});*/

    $("#AsignarFechaUnica").modal("hide")
    setTimeout(() => { window.location.reload(); }, 2000)
    //KTDataTable.reset();
}

function abrirModalSucesos(PEDIDO, SUCESOS) {

    $("#SucesosTitulo").text(`Editar suceso del Pedido: ${PEDIDO}`)
    $('#sucesos').val(SUCESOS).trigger("change")
    $("#modalsucesos").modal("show")
    _IdPedSuceso = PEDIDO;
    /*_IdSuceso = SEGUIMIENTO;*/
}

function InsertarSuceso() {
    let _sucesos = $("#sucesos").val();
    let _correo = $("#correosSuceso").val();
    let _usuario = $("#entregando").val();
    let _fechaSuceso = $('#FechaIngresaSuceso').val().split('/').reverse().join('-');
    let _observacionSuceso = $("#ObservacioSuceso").val();
    Dajax(`${URL}/Actualiza_Suceso_SubTabla`,
        "POST", { PEDIDO: _IdPedSuceso, SUCESO: _sucesos, CORREO: _correo, USUARIO: _usuario, FECHA: _fechaSuceso, OBSERVACION: _observacionSuceso }, function (response) {
            if (response.responseJSON == "1") {
                toastr.success('Suceso agregado con éxito', 'Éxito', toastrOptions);
            } else {
                toastr.warning("Hubo un error al agregar el suceso, inténtelo de nuevo más tarde", "Error", toastrOptions);
            }
        });
    $("#modalsucesos").modal("hide")
    setTimeout(() => { window.location.reload(); }, 2000)
    //KTDataTable.reset();

}

function DeleteSeguimiento(SEGUIMIENTO, PEDIDO) {

    Swal.fire({
        text: `¿Desea eliminar los seguimientos del pedido ${PEDIDO}?`,
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
            Dajax(`${URL}/DeleteSeguimiento`,
                "POST", { ID: SEGUIMIENTO, NUMPEDIDO: PEDIDO }, function (response) {
                    if (response.responseJSON == "1") {
                        toastr.success('Se elimino el seguimiento con éxito', 'Éxito', toastrOptions);
                    } else {
                        toastr.warning('Hubo un error al eliminar el seguimiento', 'Error', toastrOptions)
                    }
                    //setTimeout(() => { window.location.reload(); }, 2000)
                    //KTDataTable.reset();
                    KTDatatablesServerSide.reset();
                });
        }
    });

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
            Dajax(`${URL}/PurgarBloqueo`,
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
function asigRutaMultiple(PEDIDO, RUTA, DIA_RUTA) {
    if (PEDIDO == '') {
        $("#divruta").addClass('d-none')
    } else {
        $("#divruta").removeClass('d-none')
    }
    $("#fechayruta").text(`Asignar Fecha Prometida del Pedido: ${PEDIDO}`)
    $('#fecharuta').val(DIA_RUTA).trigger("change")
    $('#divruta').val(RUTA).trigger("change")
    $("#asignarfechayruta").modal("show")

}

function guardar_FechaPrometidaMultiple() {
    var _Fecha = $('#fecharuta').val().split('/').reverse().join('-');
    var _usuario = $("#entregando").val();

    $("#miTabla input:checkbox:checked").each(function () {
        var _pedido = $(this).val();

        Dajax(`${URL}/Actualiza_Fecha_Ruta_Multiple`,
            "POST", { PEDIDO: _pedido, FECHA: _Fecha, USUARIO: _usuario }, function (response) {
                if (response.responseJSON == "1") {

                    toastr.success("Se ha actualizado la fecha y ruta de los pedidos seleccionados", "Éxito", toastrOptions);
                } else {
                    toastr.warning("Hubo un error al actualiar, inténtelo de nuevo más tarde", "Error", toastrOptions);
                }
            });
    });

    $("#asignarfechayruta").modal("hide")
    setTimeout(() => { window.location.reload(); }, 2000)
    //KTDataTable.reset();
}
function openmodalfact() {

    $("#facturas").modal("show")
}
function guardarEstadoPedidoMulti() {
    let _estadoM = $("#cambioestado").val();
    let _Fecha_Ped = $("#FechaCambioEstado").val();
    console.log(_Fecha_Ped)
    let momentFecha = moment(_Fecha_Ped, 'DD/MM/YYYY hh:mm:ss A');
    //// Formatea la fecha y hora según tus necesidades
    let _Fecha_Pedido = momentFecha.format('YYYY-MM-DD HH:mm:ss');
    let _Observacion_Estado_Pedido = $("#Observacionescambioestado").val();
    let _usuario = $("#entregando").val();
    let _correoPedidos = $("#correoPedidos").val();


    $("#miTabla input:checkbox:checked").each(function () {
        var _pedido = $(this).val();

        if (_pedido!=="1") {
            Dajax(`${URL}/ActualizaEstadoPedidoMultiple`,
                "POST", { PEDIDO: _pedido, ESTADO: _estadoM, FECHA_PEDIDO: _Fecha_Pedido, OBSERVACION: _Observacion_Estado_Pedido, USUARIO: _usuario, CORREO: _correoPedidos }, function (response) {
                    if (response.responseJSON == "1") {
                        toastr.success('Se han cambiado los estados de los pedidos', 'Éxito', toastrOptions);
                    } else {
                        toastr.warning('Ha ocurrido un error, por favor inténtelo más tarde', 'Error', toastrOptions);
                    }
                });
        }
       
    });

    $("#facturas").modal("hide")
    setTimeout(() => {
        window.location.reload();
    }, 1000)
    
    //KTDatatablesServerSide.reset();
    // Desmarcar el checkbox "Seleccionar Todos" después de cambiar el estado
    $("#checkAll").prop('checked', false);
}

function Entregar2(PEDIDO, OBSERVACIONES, SEGUIR_FAC_DESCRIPCION) {

    $("#entregotitulo2").text(`Cambiar Seguir FAC ${PEDIDO}`)
    $("#entregar2").modal("show")
    $('#segasignar').val(SEGUIR_FAC_DESCRIPCION).trigger("change")
    $('#Observacionesseges').val(OBSERVACIONES).trigger("change")

}

function guardarEstadoFacturaMulti() {
    var _estadoF = $("#segasignar").val();
    var _Observacion_Estado_Pedido = $("#Observacionesseges").val();
    var _usuario = $("#entregando").val();


    $("#miTabla input:checkbox:checked").each(function () {
        var _pedido = $(this).val();
        Dajax(`${URL}/ActualizaEstadoFacturaMultiple`,
            "POST", { PEDIDO: _pedido, ESTADO: _estadoF, OBSERVACION: _Observacion_Estado_Pedido, USUARIO: _usuario }, function (response) {
                if (response.responseJSON == "1") {
                    toastr.success('Se ha cambiado el seguimiento de factura con éxito', 'Éxito', toastrOptions);
                } else {
                    toastr.error('Hubo un error al cambiar el seguimiento de las facturas, por favor inténtelo más tarde', 'Error', toastrOptions);
                }
            });
    });

    if (_estadoF == 2 || _estadoF == 3 || _estadoF == 4) {
        EnviarCorreo_TLA_PDF();
    }
    $("#entregar2").modal("hide");
    //setTimeout(() => {
    //    window.location.reload();
    //}, 1000)
    
    KTDatatablesServerSide.reset();
    $("#checkAll").prop('checked', false);

}


function EnviarCorreo_TLA_PDF() {
    var _usuario = $("#entregando").val();
    var _estado = $("#segasignar").val();
    let valoresCheck = [];

    $("#miTabla input:checkbox:checked").each(function () {
        valoresCheck.push(this.value);
    });
    var str = valoresCheck.join(',');

    if (_estado == "") {
        toastr.warning("Debe seleccionar una opcion para generar el reporte", "Reporte Rutas", toastrOptions);
    }
    else {

        switch (_estado) {
            case "Sin Asignar": _estado = "1";
                break;
            case "Ancla a TLA": _estado = "2";
                break;
            case "TLA a Ancla": _estado = "3";
                break;
            case "Ancla a CxC": _estado = "4";
                break;
            case "CxC Recibe": _estado = "5";
                break;
            case "Cobrador Recibe": _estado = "6";
                break;
            case "Refacturada": _estado = "7";
                break;
            case "Trámite": _estado = "8";
                break;
            case "Factura Posterior": _estado = "9";
                break;
            case "Anulada con NC": _estado = "10";
                break;
            /*default: "Seleccione un estado "*/
        }
        if (_estado == 1 || _estado == 5 || _estado == 6 || _estado == 7 || _estado == 8 || _estado == 9 || _estado == 10) {
            toastr.warning("Esta opcion no estan valorada para reporte, seleccione Ancla a TLA, TLA a Ancla o Ancla a CXC!", "Reporte Rutas", toastrOptions);
        } else {
            Dajax(`${URL}/EnviarReportePDF_TLA`,
                "POST", { ESTADO_FAC: _estado, USUARIO: _usuario, PEDIDO: str }, function (response) {
                    if (response.responseJSON == 1) {
                        toastr.success("Reporte en formato PDF enviado con éxito!", "Reporte Rutas", toastrOptions);
                    } else {
                        toastr.warning("Error enviando el documento en formato PDF!", "Reporte Rutas", toastrOptions);
                    }
                });
        }
    }
    $("#entregar2").modal("hide")

}

function InicializarFechas() {
    
    new tempusDominus.TempusDominus(document.getElementById("FechaPedidoIndividual"), {
        localization: {
            locale: 'es',
            format: "DD/MM/YYYY"
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

    new tempusDominus.TempusDominus(document.getElementById("fecharutaPrometida"), {
        localization: {
            locale: 'es',
            format: "DD/MM/YYYY"
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
    new tempusDominus.TempusDominus(document.getElementById("FechaIngresaSuceso"), {
        localization: {
            locale: 'es',
            format: "DD/MM/YYYY"
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
    new tempusDominus.TempusDominus(document.getElementById("fecharuta"), {
        localization: {
            locale: 'es',
            format: "DD/MM/YYYY"
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
    $("#FechaRuta").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });
    $("#FechaCxcRecibe").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });
    $("#FechaInicioFin").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });

    //$("#FechaCambioEstado").daterangepicker({
    //    "locale": {
    //        "format": "DD/MM/YYYY"
    //    }
    //});


    new tempusDominus.TempusDominus(document.getElementById("FechaCambioEstado"), {
        localization: {
            locale: 'es',
            format: "DD/MM/YYYY"
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


    

}

function getParameterByName(name) {
    const url = window.location.href;
    name = name.replace(/[[]]/g, '\\$&'); // Escapa los corchetes de nombre
    const regex = new RegExp('[?&]' + name + '=([^&#]*)');
    const results = regex.exec(url);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
}

function InicializarSelects() {

    // Inicialización del select2 para 'pedido'
    $('#pedido').select2({
        language: "es",
        placeholder: 'Filtrar por un pedido',
        minimumInputLength: 4,
        ajax: {
            url: `${URL}/Obtener_Cod_Pedido_Buscar`,
            dataType: 'json',
            type: 'GET',
            processResults(data) {
                const todos = [{ text: "Todos", id: "0" }];
                const results = $.map(data, function (item) {
                    return {
                        text: item.PEDIDO,
                        id: item.PEDIDO
                    };
                });
                const options = todos.concat(results);
                return {
                    results: options
                };
            }
        }
    });

    // Inicialización del select2 para 'factura'
    $('#factura').select2({
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
                ];
                const results = $.map(data, function (item) {
                    return {
                        text: item.FACTURA,
                        id: item.FACTURA
                    };
                });
                const options = todos.concat(results);
                return {
                    results: options
                };
            }
        }
    });

    
}


//function InicializarSelects() {

//    $('#pedido').select2({
//        language: "es",
//        placeholder: 'Filtrar por un pedido',
//        minimumInputLength: 4,
//        ajax: {
//            url: `${URL}/Obtener_Cod_Pedido_Buscar`,
//            dataType: 'json',
//            type: 'GET',
//            processResults(data) {
//                const todos = [{ text: "Todos", id: "0" }]
//                const results = $.map(data, function (item) {
//                    return {
//                        text: item.PEDIDO,
//                        id: item.PEDIDO
//                    }
//                })
//                const options = todos.concat(results)
//                return {
//                    results: options
//                }
//            }
//        }

//    })

//    $('#factura').select2({
//        language: "es",
//        placeholder: 'Filtrar por una factura',
//        minimumInputLength: 4,
//        ajax: {
//            url: `${URL_SEGUIMIENTO}/Obtener_Cod_FacturaSeg_Busqueda`,
//            dataType: 'json',
//            type: 'GET',
//            processResults(data) {
//                const todos = [
//                    { text: "Todos", id: "0" },
//                    { text: "Con facturas", id: "99" }
//                ]
//                const results = $.map(data, function (item) {
//                    return {
//                        text: item.FACTURA,
//                        id: item.FACTURA
//                    }
//                })
//                const options = todos.concat(results)
//                return {
//                    results: options
//                }
//            }
//        }
//    })

//    const pedidoUrl = localStorage.getItem('pedido');
//    console.log("Pedido recibido desde localStorage:", pedidoUrl); // Verifica que se recuperó correctamente
//    if (pedidoUrl) {
//        // Establecer el valor del combobox
//        $('#pedido').val(pedidoUrl).trigger('change');
//    }
//    // Limpiar el localStorage después de usar el valor
//    localStorage.removeItem('pedido');
//}


function abrirmodalCorreos() {
    $("#modalenviocorreo").modal("show");
    new tempusDominus.TempusDominus(document.getElementById("FechaEnvio"), {
        localization: {
            locale: 'es', // Establece el idioma en español
            format: "DD/MM/YYYY", // Formato de fecha en español
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




function DescargarExcel() {
    let valoresCheck = [];
    $("#miTabla input:checkbox:checked").each(function () {
        valoresCheck.push(this.value);
    });
    const str = valoresCheck.join(',');
    Dajax(`${URL}/GenerarExcelRutas`,
        "POST", { PEDIDO: str }, function (response) {
            let ruta = `${URL}/DescargarArchivo?ruta=${response.responseJSON}`
            if (response.responseJSON != 2) {
                toastr.success("Reporte de Rutas se ha enviado con éxito!", "Reporte Rutas", toastrOptions);
            } else {
                toastr.warning("Error enviando el documento Reporte de Ruta!", "Reporte Rutas", toastrOptions);
            }
            setTimeout(() => { window.location.href = ruta; }, 1000)
        });
}

function enviarCorreo() {
    var _Fecha = $('#FechaEnvio').val().split('/').reverse().join('-')
    var _Email = $('#correos').val()
    Dajax(`${URL}/EnviarReportesRutas`,
        "POST", { fechaPrometida: _Fecha, CORREO: _Email }, function (response) {
            if (response.responseJSON == 1) {
                toastr.success("¡Se ha enviado el reporte de seguimientos con éxito!", "Enviado", toastrOptions);
            } else {
                toastr.warning("Error al enviar el reporte de seguimientos, inténtelo de nuevo más tarde.", "Error", toastrOptions);
            }
        });
    $("#modalenviocorreo").modal("hide")
}




function cargarParamtroPedido() {
    // Recuperar el valor de 'pedido' desde localStorage
    const pedidoUrl = localStorage.getItem('pedido');
    console.log("Pedido recibido desde localStorage:", pedidoUrl); // Verifica que se recuperó correctamente

    if (pedidoUrl) {
        // Esperar a que el 'select2' haya cargado las opciones
        $('#pedido').on('select2:open', function () {
            // Intentar setear el valor después de que las opciones se hayan cargado
            setTimeout(function () {
                // Verifica si el valor ya existe en las opciones cargadas
                const options = $('#pedido').select2('data');
                const exists = options.some(option => option.id === pedidoUrl);

                if (exists) {
                    // Si el valor existe en las opciones, seleccionarlo
                    $('#pedido').val(pedidoUrl).trigger('change');
                } else {
                    // Si no existe, hacer una búsqueda manual para encontrarlo
                    $('#pedido').select2('open');
                    // Puedes forzar la búsqueda escribiendo el valor directamente en el campo de búsqueda
                    $('#pedido').val(pedidoUrl).trigger('input');
                }
            }, 300); // Ajusta el tiempo según sea necesario
        });

        // Si ya está cargado, forzar la apertura del dropdown
        $('#pedido').select2('open');
    }

    // Limpiar el localStorage después de usar el valor
    // localStorage.removeItem('pedido');
}


$(document).ready(function () {
    InicializarSelects()
    InicializarFechas()
    new Tagify(document.querySelector("#kt_select2_10"));
    cargarParamtroPedido();
    KTDatatablesServerSide.init()


    //ManejarFiltros();
});