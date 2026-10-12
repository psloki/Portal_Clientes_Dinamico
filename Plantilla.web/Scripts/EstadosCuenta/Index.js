//let ec_semanamulti = $("#dia :selected").map((_, e) => e.value).get();//`(${$("#semana").val().map(item => `'${item}'`).join(',')})`;
//let concatenarsemanmulti = "";

let cocatenar_diasemana = "";
/*let ec_ruta = $('#ruta').val();*/
//let ec_semanamulti = ($("#semana :selected").map((_, e) => {
//    if ($(e).push('selected')) {
//        concatenarsemanmulti += e.value + ',';
//    }
//}));

let ec_dia_semana = ($("#dia :selected").map((_, e) => {
    if ($(e).push('selected')) {
        cocatenar_diasemana += e.value + ',';
    }
}));
//let ec_dia_semana = $("#dia :selected").map((_, e) => e.value).get();
let ec_saldo = $('#saldo').val();
let ec_condicion_pago = $('#condpago').val();
let ec_estado_visita = $('#EstadoVisita').val();
let ec_vendedor = $('#vendedorFiltro').val();
let ec_cobrador = $('#cobradorFiltro').val();
//let ec_cliente = $('#clientesFiltro').val();
let semanamulti = "";
let enviarcorreoec = "";
let clienteeccorreo = "";
let clientecorreoFE = "";
let correosCompleto = "";
let fecha_aplicacion_ini = "";
let fecha_aplicacion_fin = "";
let clientereporteAuxiliar = "";
let clientecorreoreporteauxiliar = "";
let correoaux = "";
let correoextraaux = "";
let correosCompletosAux = "";
let _correos_marcados = [];
let correos_seleccionados = ""
let cliente_visita = "";
var id_visita_actual = null;
let realizarPago = null;
let direccionLatLong = "";
let validSC = true;



var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#miTabla").DataTable({
            responsive: isMobile,
            scrollX: true,         // Habilita scroll horizontal
            autoWidth: false,
            processing: true,
            deferRender: true,
            orderable: false,
            destroy: true,
            searchDelay: 500,
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },
            //data: cotizaciones,
            order: [],

            destroy: true,
            //serverSide: true,
            ajax: {
                url: `${CURL}/Obtener_Estado_Cuenta_CLiente`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                    d.semanamulti = concatenarsemanmulti;
                    d.cocatenar_diasemana = cocatenar_diasemana;
                    d.ec_saldo = ec_saldo;
                    d.ec_condicion_pago = ec_condicion_pago;
                    d.ec_estado_visita = ec_estado_visita;

                    d.ec_vendedor = (ec_vendedor === 'VND') ? '0' : ec_vendedor;
                    d.ec_cobrador = (ec_cobrador === 'CND') ? '0' : ec_cobrador;

                    d.ec_cliente = ec_cliente;
                    d.ec_fechas = verFechas();
                }
            },
            columns: [

                {
                    "data": "U_ORDEN_VISITA",
                    "defaultContent": ``,
                    render: function (data, type, row) {
                        return `
                            <div class="form-check form-check-sm form-check-custom form-check-solid">
                                <input class="form-check-input" type="checkbox" value="${row.CLIENTE}" correo="${row.EMAIL}" />
                            </div>`;
                    }
                },
                {
                    "data": null,
                    render: function (data, type, row) {
                        return `<a href="javascript:;"><i class="fa fa-caret-right verSubtabla" style="width: 10px;"></i></a>`;
                    }
                },


                {
                    "data": "ESTADO",
                    render: function (data, type, row) {
                        return `<span class="badge ${row.COLOR_HEXA.replace("label", "badge")} badge-lg">${row.ESTADO}</span>`;
                    }
                },

                { "data": "CLI_NOMBRE" },
                { "data": "ALIAS" },
                { "data": "VEN_NOMBRE", },
                { "data": "EMAIL", },
                { "data": "COD_NOMBRE_COBRADOR", },
                { "data": "TELEFONO", },
                { "data": "CONDICION_PAGO", },
                {
                    "data": "SALDO",
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return `₡${row.SALDO.toLocaleString('en-US')}`;
                    }
                },
                {
                    "data": "DOC_VENC",
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.DOC_VENC}`;
                    }
                },
                {
                    "data": "ID_VISITA",
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.ID_VISITA;
                    }
                },
                {
                    "data": "FECHA_ULTIMA_VISITA",
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.FECHA_ULTIMA_VISITA;
                    }
                },
                {
                    "data": "SEMANA",
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.SEMANA;
                    }
                },
                {
                    "data": null,

                    orderable: false,
                    render: function (data, type, row) {
                        let accionesClientes = "";
                        const tablaColapsada = $('#miTabla').hasClass('collapsed');

                        if (PERMISOS_GESTION_PEDIDOS.split(',').includes("18")) {
                            accionesClientes += `<a href="javascript:;" class="btn btn-sm btn-clean btn-icon me-2" title="Crear visita" onclick="AbrirModalVisitas('${row.CLIENTE}','${row.CLI_NOMBRE}','${row.COBRADOR}','${row.COD_NOMBRE_COBRADOR}','${row.EMAIL}')">
	                                             <i class="la la-map-marker fs-2"></i>\
	                                             </a>`
                        }

                        if (PERMISOS_MENU.split(',').includes("46")) {
                            accionesClientes += `<a onclick="irPedido('${row.CLIENTE}','${row.ID_VISITA}')" class="btn btn-sm btn-clean btn-icon me-2" title="Ver pedidos">
                            <i class="la la-shopping-cart fs-2"></i>
                         </a>`;
                        }

                        if (PERMISOS_MENU.split(',').includes("48")) {
                            accionesClientes += `<a onclick ="irPago('${row.CLIENTE}','${row.ID_VISITA}','${row.COD_NOMBRE_COBRADOR}','${row.ALIAS}',  '${row.COD_NOMBRE_VENDEDOR}')"  id="irPagos" class="btn btn-sm btn-clean btn-icon me-2" title="Ir pagos" >
	                                             <i class="la la-copy fs-2"></i>\
	                                             </a>`
                        }

                        if (PERMISOS_GESTION_PEDIDOS.split(',').includes("21")) {
                            if (tablaColapsada) {
                                accionesClientes += `<a href="javascript:void(0);" onclick="handlePrint('${ecurl}/printEstadoCuenta?cliente=${row.CLIENTE}')" class="btn btn-sm btn-clean btn-icon me-2" title="Imprimir estado de cuenta">
                                    <i class="la la-print fs-2"></i>
                                </a>
                                <a href="javascript:;" onclick="abrirmodalCorreos('${row.EMAIL}','${row.CLIENTE}');return false;" class="btn btn-sm btn-clean btn-icon me-2" title="Enviar estado de cuenta">
                                    <i class="la la-envelope fs-2"></i>
                                </a>`;
                            } else {
                                accionesClientes += ` <div style="display:inline-flex">
                                    <button class="btn btn-sm btn-clean btn-icon me-2"  title="Estado de cuenta"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-overflow="true">
                                        <i class="la la-print fs-2"></i>
                                    </button>
                                    <!--begin::Menu-->
                                    <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4"  data-kt-menu="true" >
                                        <!--begin::Menu item-->
                                        <div class="menu-item px-3">
                                            <label>Estado de cuenta</label>
                                        </div>
                                        <!--end::Menu item--><!--begin::Menu item-->
                                        <div class="menu-item px-3">
                                            <a href="javascript:void(0);"
                                                onclick="handlePrint('${ecurl}/printEstadoCuenta?cliente=${row.CLIENTE}')"
                                                class="menu-link px-3">
                                                Imprimir
                                            </a>
                                        </div>

                                        <!--end::Menu item-->

                                        <!--begin::Menu item-->
                                        <div class="menu-item px-3">
                                            <a href="javascript:;" onclick="abrirmodalCorreos('${row.EMAIL}','${row.CLIENTE}');return false;" class="menu-link px-3">
                                                Enviar
                                            </a>
                                        </div>
                                        <!--end::Menu item-->
                                    </div>
                                </div>`
                            }
                        }

                        if (PERMISOS_GESTION_PEDIDOS.split(',').includes("22")) {
                            if (tablaColapsada) {
                                accionesClientes += `<a href="javascript:;" onclick="modalAuxiliar('${row.CLIENTE}');return false;" class="btn btn-sm btn-clean btn-icon me-2" title="Imprimir reporte de aplicación">
                                    <i class="la la-file-invoice-dollar fs-2"></i>
                                </a>
                                <a href="javascript:;" onclick="abrirmodalCorreosAux('${row.CLIENTE}','${row.EMAIL}');return false;" class="btn btn-sm btn-clean btn-icon me-2" title="Enviar reporte de aplicación">
                                    <i class="la la-paper-plane fs-2"></i>
                                </a>`;
                            } else {
                                accionesClientes += `<div style="display:inline-flex">
                                    <button class="btn btn-sm btn-clean btn-icon me-2"  title="Reporte de Aplicaciones"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-overflow="true">
                                        <i class="la la-file-invoice-dollar fs-2"></i>
                                    </button>
                                    <!--begin::Menu-->
                                    <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4"  data-kt-menu="true" >
                                        <!--begin::Menu item-->
                                        <div class="menu-item px-3">
                                            <label>Reporte de Aplicaciones</label>
                                        </div>
                                        <!--end::Menu item--><!--begin::Menu item-->
                                        <div class="menu-item px-3">
                                            <a href="javascript:;"  onclick="modalAuxiliar('${row.CLIENTE}');return false;"  class="menu-link px-3">
                                                Descargar a Excel
                                            </a>
                                        </div>
                                        <!--end::Menu item-->

                                        <!--begin::Menu item-->
                                        <div class="menu-item px-3">
                                            <a href="javascript:;" onclick="abrirmodalCorreosAux('${row.CLIENTE}','${row.EMAIL}');return false;"  class="menu-link px-3">
                                                Enviar
                                            </a>
                                        </div>
                                        <!--end::Menu item-->
                                    </div>
                                    </div>`
                            }
                        }

                        if (PERMISOS_MENU.split(',').includes("47")) {
                            accionesClientes += `<a onclick ="irVisita('${row.CLIENTE}','${row.ID_VISITA}')"  id="irVisita" class="btn btn-sm btn-clean btn-icon me-2" title="Ir a Visita" >
	                                             <i class="la la-map fs-2"></i>\
	                                             </a>`
                        }
                        return accionesClientes;
                    }
                },
            ],
            columnDefs: [

                // Responsive priorities
                { responsivePriority: 1, targets: 0 },  // Checkbox
                { responsivePriority: 1, targets: 1 },  // caret-right
                { responsivePriority: 2, targets: 2 },  // ESTADO
                { responsivePriority: 1, targets: 3 },  // CLIENTE
                { responsivePriority: 100, targets: [4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15] } // resto
            ]

        });
        table = dt.$;

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        dt.on('draw', function () {
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

            KTMenu.init();
        });
    }
    var resetTable = function () {
        if (dt) {
            dt.ajax.reload();

        }
    };


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
        var allChecked = dt.column(0).nodes().to$().find('input[type="checkbox"]:checked').length === dt.data().length;
        $('#checkAll').prop('checked', allChecked);

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
                    <table class="table  table-row-dashed fs-6 gy-4" id="subtabla${datos.CLIENTE}">
                        <thead>
                            <tr class="text-start text-gray-600 fw-bold fs-7 text-uppercase gs-0">
                                <th class="text-left min-w-25px">Tipo</th>
                                <th class="text-left min-w-150px">Documento</th>
                                <th class="text-left min-w-150px">Subtipo</th>
                                <th class="text-left min-w-150px">Referencia</th>
                                <th class="text-left min-w-80px">Fecha</th>
                                <th class="text-left min-w-80px">Vence</th>
                                <th class="text-left min-w-25px">DV</th>
                                <th class="text-left min-w-100px">Monto</th>
                                <th class="text-left min-w-100px">Abonado</th>
                                <th class="text-left min-w-100px">Saldo</th>
                                <th class="text-left min-w-125px">Usuario</th>
                                <th class="text-left min-w-80px">Creación</th>
                                <th class="text-left min-w-80px">Acciones</th>
                            </tr>
                        </thead>
                        <tbody class="text-gray-600 fw-semibold">
                        </tbody>
                    </table>
                `).show();
                $(`#subtabla${datos.CLIENTE}`).DataTable({
                    ajax: {
                        url: `${CURL}/Obtener_SubTablaDocumentos?cliente=${datos.CLIENTE}`,
                        dataSrc: ""
                    },
                    destroy: true,
                    columns: [


                        { "data": "TIPO" },
                        { "data": "DOCUMENTO" },
                        { "data": "SUBTIPO" },
                        { "data": "REFERENCIA_DEBITO" },
                        { "data": "FECHA_DOCUMENTO" },
                        { "data": "FECHA_VENCE" },
                        //{ "data": "VENDEDOR_DOC" },
                        //{ "data": "COBRADOR_DOC" },
                        { "data": "DIAS_NETO" },
                        { "data": "MONTO" },
                        { "data": "MONTO_PAGO" },
                        { "data": "SALDO" },
                        { "data": "USUARIO_CREACION" },
                        { "data": "FECHA_CREACION_DEBITO" },
                        {
                            "data": null,
                        },
                    ],
                    columnDefs: [
                        {
                            targets: 0,
                            orderable: false,
                            render: function (data, type, row) {
                                var status = {
                                    "FAC": { 'title': 'FAC', 'class': ' badge-light-success' },
                                    "REC": { 'title': 'REC', 'class': ' badge-light-warning' },
                                    "TEF": { 'title': 'TEF', 'class': ' badge-light-info' },
                                    "N/C": { 'title': 'N/C', 'class': ' badge-light-primary' },
                                    "N/D": { 'title': 'N/D', 'class': ' badge-light-success' },
                                    "O/C": { 'title': 'O/C', 'class': ' badge-light-danger' },
                                    "O/D": { 'title': 'O/D', 'class': ' badge-light-warning' },
                                    "DEP": { 'title': 'DEP', 'class': ' badge-light-warning' },
                                    "INT": { 'title': "INT", 'class': ' badge-light-success' }
                                };
                                return `<span class="badge ${status[row.TIPO].class} badge-lg">${status[row.TIPO].title}</span>`;
                            }
                        },
                        {
                            targets: 6,
                            className: "dt-body-right",
                            orderable: false,
                            render: function (data, type, row) {
                                return `${row.DIAS_NETO}`;
                            }
                        },
                        {
                            targets: 7,
                            className: "dt-body-right",
                            orderable: false,
                            render: function (data, type, row) {
                                return `₡${row.MONTO.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
                            }
                        },
                        {
                            targets: 8,
                            className: "dt-body-right",
                            orderable: false,
                            render: function (data, type, row) {
                                return `₡${row.MONTO_PAGO.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
                            }
                        },
                        {
                            targets: 9,
                            className: "dt-body-right",
                            orderable: false,
                            render: function (data, type, row) {
                                const fechaActual = new Date(); // Obtener la fecha actual
                                // Convertir la cadena de fecha en un objeto de fecha
                                const fechaVence = new Date(row.FECHA_VENCE);

                                if (fechaVence < fechaActual) {
                                    let data = `₡${row.SALDO.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`
                                    return `<span class="text-danger">${data}</span>`;
                                }
                                else {
                                    let data = `₡${row.SALDO.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`
                                    return `<span class="text-primary">${data}</span>`;
                                }
                            }
                        },
                        {
                            targets: 12,
                            orderable: false,
                            render: function (data, type, row) {
                                let accionessubtablaCliente = "";
                                //if (PERMISOS_GESTION_PEDIDOS.split(',').includes("24")) {
                                //    accionessubtablaCliente += `${row.TIPO == 'O/C' ?
                                //                               '<a title="Ver aplicaciones" onclick="AbrirModalRegistroEdit()" class="btn btn-sm btn-clean btn-icon me-2">\
                                //                                <i class="la la-file fs-2"></i>\
                                //                                </a>' :
                                //                                '<a title="Reenviar documento" onclick="abrirmodalCorreosPDF(\'' + datos.EMAIL + '\', \'' + datos.CLIENTE + '\',\'' + row.DOCUMENTO + '\')" class="btn btn-sm btn-clean btn-icon me-2">\
                                //                                 <i class="la la-envelope fs-2x"></i>\
                                //                                 </a>'}`
                                //}

                                if (["DEP", "REC", "TEF"].includes(row.TIPO) && isMobile) {
                                    accionessubtablaCliente = '<a title="Imprimir comprobante" onclick="reimprimirTiquete(\'' + row.DOCUMENTO + '\')" class="btn btn-sm btn-clean btn-icon me-2">\
                                                                    <i class="la la-print fs-2x"></i>\
                                                                </a>';
                                }

                                return accionessubtablaCliente
                            }
                        },
                    ],

                });

                $(`#subtabla${datos.CLIENTE}_wrapper`).addClass("ms-20")
                //row.child(construirsubtabla(row.data().LINEAS)).show();
                tr.addClass('shown');

                /*abrirModalPDF*/
            }
        });
    }

    // Public methods
    return {
        init: function () {
            initDatatable();
            subtabla()
            Filtros();
            //setTimeout(() => { ListenerCheckboxes() }, 5000)

        },
        reset: function () {
            resetTable();

        }

    }
}();

function ActualizarFiltros() {

    const semSel = $('#semana').val();
    concatenarsemanmulti = Array.isArray(semSel) ? semSel.join(',') : (semSel || '');

    const diaSel = $('#dia').val();
    cocatenar_diasemana = Array.isArray(diaSel)
        ? diaSel.join(',')
        : (diaSel || '');

    ec_saldo = $('#saldo').val();
    ec_condicion_pago = $('#condpago').val();
    ec_estado_visita = $('#EstadoVisita').val();
    ec_vendedor = $('#vendedorFiltro').val();
    ec_cobrador = $('#cobradorFiltro').val();
    ec_cliente = $('#clientesFiltro').val();
}

function ManejarFiltros() {

    $('.filtro').not('#vendedorFiltro, #cobradorFiltro')
        .on('change', () => {
            ActualizarFiltros();
            KTDatatablesServerSide.reset();
        });

    $('.filtro_multiple').on('change', function () {
        const $select = $(this);

        // Esperar a que select2 actualice internamente su estado
        setTimeout(() => {
            let valores = $select.val() || [];

            // Caso: se seleccionó "0" (Todas) junto con otros -> deja solo "0"
            if (valores.includes('0') && valores.length > 1) {
                $select.val(['0']).trigger('change.select2');
                return;
            }

            // Caso: está solo "0", y se agrega otra -> quita "0"
            if (!valores.includes('0') && valores.length > 0) {
                $select.find('option[value="0"]').prop('selected', false);
            }

            ActualizarFiltros();
            KTDatatablesServerSide.reset();
        }, 50); // Retry corto (puede aumentarse a 100ms si aún falla en algunos casos)
    });


    $(document).on('filtros-actualizados', (_, data) => {
        ec_vendedor = data.vendedor;
        ec_cobrador = data.cobrador;
        KTDatatablesServerSide.reset();
    });
    ActualizarFiltros();
}

//function AbrirModalVisitas(cliente, NOMBRE) {
//    $("#modalvisita").modal("show");
//    $("#clientevisita").val(NOMBRE); // Establece el valor del campo de entrada "#clientevisita" como el valor de "cliente"
//    cliente_visita = cliente
//    let direccion = "";
//    let latitud = "";
//    let longitud = "";
//    let txtLatitud = "";
//    navigator.geolocation.getCurrentPosition(function (position) {
//         latitud = position.coords.latitude;
//         longitud = position.coords.longitude;
//         console.log(latitud, longitud)
//         txtLatitud = document.getElementById("latitudvisita");
//         txtLongitud = document.getElementById("longitudvisita");
//         console.log('carga datos a texbox: ' + txtLatitud, txtLongitud)

//        txtLatitud.value = latitud;
//        txtLongitud.value = longitud;

//        ObtenerDireccionCoordenadas(latitud, longitud)
//            .then(direccion => {
//                direccionLatLong = direccion;
//                console.log(direccionLatLong);
//                // Aquí puedes hacer lo que necesites con la dirección obtenida
//            })
//            .catch(error => {
//                console.error('Error al obtener la dirección:', error);
//            });
//    });
//    documentosVencidos(cliente);
//}



//function obtenerUbicacionPorIP() {
//    return fetch("https://ipapi.co/json/")
//        .then(response => {
//            if (!response.ok) {
//                throw new Error("Error al obtener la ubicación por IP.");
//            }
//            return response.json();
//        })
//        .then(data => {
//            return {
//                latitude: data.latitude,
//                longitude: data.longitude
//            };
//        });
//}

//// Uso de la función obtenerUbicacionPorIP
//obtenerUbicacionPorIP()
//    .then(coords => {
//        console.log("Latitud:", coords.latitude);
//        console.log("Longitud:", coords.longitude);
//    })
//    .catch(error => {
//        console.error("Error al obtener la ubicación por IP:", error);
//    });



//function obtenerDireccion(latitud, longitud) {
//    var geocoder = Maps.newGeocoder().reverseGeocode(latitud, longitud);
//    var resultado = geocoder.results[0];

//    if (resultado) {
//        var direccion = resultado.formatted_address;
//        Logger.log('Dirección:', direccion);
//        return direccion;
//    } else {
//        Logger.log('No se encontró ninguna dirección para las coordenadas:', latitud, longitud);
//        return null;
//    }
//}




function AbrirModalVisitas(cliente, NOMBRE, COBRADOR, COD_NOMBRE_COBRADOR, EMAIL) {
    console.log(EMAIL);

    if (EMAIL === "" || EMAIL === null || EMAIL === "nd@nd.nd") {
        toastr.warning(`El cliente no tiene un correo asignado`, "Advertencia", toastrOptions);
    }

    $("#modalvisita").modal("show");
    $("#clientevisita").val(NOMBRE);
    $("#cobradorVisita").val(COD_NOMBRE_COBRADOR);

    cliente_visita = cliente;

    navigator.geolocation.getCurrentPosition(function (position) {
        let { latitude, longitude } = position.coords;
        console.log('Latitud:', latitude, 'Longitud:', longitude);

        let txtLatitud = document.getElementById("latitudvisita"),
            txtLongitud = document.getElementById("longitudvisita");

        txtLatitud.value = latitude;
        txtLongitud.value = longitude;

        ObtenerDireccionCoordenadas(latitude, longitude)
            .then(direccion => {

                direccionLatLong = direccion;
            })
            .catch(error => {
                console.error('Error al obtener la dirección:', error);
            });
    }, error => {
        console.error('Error al obtener la ubicación:', error);
    });

    documentosVencidos(cliente);
}



function ObtenerDireccionCoordenadas(latitud, longitud) {
    return new Promise((resolve, reject) => {
        fetch(`https://nominatim.openstreetmap.org/reverse?lat=${latitud}&lon=${longitud}&format=json`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Hubo un problema al obtener la dirección.');
                }
                return response.json();
            })
            .then(data => {
                if (data && data.display_name) {
                    resolve(data.display_name);
                } else {
                    throw new Error('No se encontró una dirección válida en la respuesta.');
                }
            })
            .catch(error => {
                reject(error.message);
            });
    });
}





function padNumber(number) {
    return (number < 10 ? '0' : '') + number;
}


function guardarVisita() {

    let id_visita = 0;
    let _tipo = $("#tipovisita").val()
    let fecha = $('#fechavisita').val();
    // Usa moment.js para parsear la cadena de fecha y hora
    let momentFecha = moment(fecha, 'DD/MM/YYYY hh:mm:ss A');
    // Formatea la fecha y hora según tus necesidades
    let _fecha_hora_visita = momentFecha.format('YYYY-MM-DD HH:mm:ss');
    let _latitud = $("#latitudvisita").val()
    let _longitud = $("#longitudvisita").val()
    let _estado_visita = "";
    let _doc_venc_descripcion = $("#Doc_Vencido").val()
    if (_tipo == '0001') {
        _estado_visita = 'P' // PEDIDO 
    } else if (_tipo == '0002') {
        _estado_visita = 'R' // RECIBO
    } else if (_tipo == '0003') {
        _estado_visita = 'T' // TELEFONO
    } else if (_tipo == '0004') {
        _estado_visita = 'VP' // PRESENCIAL
    }
    let _comentario = $("#comentariovisita").val();
    let _vendedor = $("#vendedorFiltro").val();
    let _usuario = $("#usuariovisita").val();

    if (_tipo === "" || _tipo === null) {
        toastr.warning(`Por favor seleccione un tipo de visita`, "Advertencia", toastrOptions)
    } else {
        Dajax(`${CURL}/RegistrarVisita`,
            "POST", {
            cliente: cliente_visita, tipo_visita: _tipo, fecha_visita: _fecha_hora_visita, latitud: _latitud, longitud: _longitud,
            estado_visita: _estado_visita, comentario: _comentario, vendedor: _vendedor, usuario: _usuario, doc_venc_descripcion: _doc_venc_descripcion, direccion: direccionLatLong
        }, function (response) {
            if (response.status == 200) {
                if (response.responseJSON != "0") {
                    id_visita_actual_temp = response.responseJSON;
                    // Almacenar pedido actual en localStorage
                    localStorage.setItem('id_visita_actual', id_visita_actual_temp);

                    toastr.success(`Visita registrada con éxito`, "Éxito", toastrOptions);
                    //KTDataTableClientes.reset();
                    /*  KTDatatablesServerSide.reset();*/
                    /*setTimeout(() => {*/
                    if ((_tipo == '0001' || _tipo == '0003' || _tipo == '0004') && realizarPago !== true) {
                        window.location.href =
                            `${CURL}/IrEcommerce?cliente=${encodeURIComponent(cliente_visita)}`;

                    } else if (realizarPago == true) {
                        let id_visita = localStorage.getItem('id_visita_actual');
                        /*$('#clientesFiltro').val(cliente_visita).trigger("change");*/
                        CambiarClienteSesion(cliente_visita);
                        //var url = `${RURL}?cliente=${cliente_visita}&idvisita=${id_visita}`;
                        //window.open(url, '_blank');

                        var url = `${RURL}?cliente=${cliente_visita}&idvisita=${id_visita}`;
                        window.location.href = url;
                    }

                    /*  }, 1500)*/


                } else {
                    toastr.warning(`Hubo un error al registrar la visita, inténtelo de nuevo más tarde`, "Error", toastrOptions)
                    setTimeout(function () {
                        location.reload();
                    }, 1500);
                }
            }
        });
        // mostrarTipoVisita();
        $("#modalvisita").modal("hide")
    }

}


function mostrarTipoVisita() {
    setTimeout(function () {
        location.reload();
    }, 1500);

}


function abrirmodalCorreos(EMAIL, CLIENTE) {
    $("#modalenviocorreo").modal("show")
    if (EMAIL != '') {
        $("#correos").val(EMAIL)
    }
    clienteeccorreo = CLIENTE
}


/// envia correo de estado de cuenta para cliente

function guardarEntrega() {

    if (!validSC) {
        toastr.error(`Por favor edite un correo valido`, 'Error')
        $("#correosextra").val("");
        return
    }

    // Obtener los valores de los campos de texto
    var correo = document.getElementById("correos").value;
    var correoextra = document.getElementById("correosextra").value;
    // Juntar los correos y dividirlos por punto y coma
    correosCompleto = correo + ";" + correoextra;
    Dajax(`${CURL}/EnviarEstadoCuenta`,
        "POST", { cliente: clienteeccorreo, correo: correosCompleto }, function (response) {
            if (response.responseJSON == "1") {
                toastr.success('Documento enviado con éxito', 'Éxito', toastrOptions)
            } else {
                toastr.warning('Hubo un error al hacer el llamado!', 'Enviar Reporte', toastrOptions)
            }
        });

    $("#modalenviocorreo").modal("hide")
    //setTimeout(() => { window.location.reload(); }, 2000)
}

function abrirmodalCorreosPDF(EMAIL, CLIENTE, DOCUMENTO) {
    if (EMAIL != '') {
        $("#correosFE").val(EMAIL)
    }
    $("#NumFactura").val(DOCUMENTO)
    $("#modalenviarFE").modal("show")
    clientecorreoFE = CLIENTE
}

function EnviarCorreoFE() {

    if (!validSC) {
        toastr.error(`Por favor edite un correo valido`, 'Error')
        $("#correosextraFE").val("");
        return
    }

    let factura = document.getElementById("NumFactura").value;
    let correo = document.getElementById("correosFE").value;
    let correoextra = document.getElementById("correosextraFE").value;
    let correosCompletoFE = correo + ";" + correoextra

    Dajax(`${CURL}/Enviar_FE_Cliente`,
        "POST", { cliente: clientecorreoFE, correos: correosCompletoFE, documento: factura }, function (response) {
            if (response.responseJSON == "1") {
                toastr.success('Documento enviado con éxito', 'Éxito', toastrOptions)
            } else {
                toastr.warning('Hubo un error al hacer el llamado!', 'Enviar Reporte', toastrOptions)
            }
        });

    $("#modalenviarFE").modal("hide")
}



function modalAuxiliar(cliente) {
    $("#kt_daterangepicker_modal").modal("show")
    clientereporteAuxiliar = cliente;
}


// Descargar excel para reporte de auxiliar
let descargandoExcelAuxiliar = false;

function mostrarLoadingExcelAuxiliar() {
    descargandoExcelAuxiliar = true;
    const $modal = $('#kt_daterangepicker_modal');
    const $btn = $('#BtnDescargarAuxiliar');

    if ($btn.length) {
        $btn.data('texto-original', $btn.html());
        $btn.prop('disabled', true).html('<span class="spinner-border spinner-border-sm align-middle me-2"></span>Descargando...');
    }

    $modal.find('[data-bs-dismiss="modal"], [data-dismiss="modal"]').prop('disabled', true);

    if (typeof KTApp !== 'undefined' && typeof KTApp.block === 'function') {
        KTApp.block('#kt_daterangepicker_modal .modal-content', {
            overlayColor: '#000000',
            state: 'primary',
            message: 'Generando Excel...'
        });
    }
}

function ocultarLoadingExcelAuxiliar() {
    descargandoExcelAuxiliar = false;
    const $modal = $('#kt_daterangepicker_modal');
    const $btn = $('#BtnDescargarAuxiliar');

    if ($btn.length) {
        $btn.prop('disabled', false).html($btn.data('texto-original') || 'Descargar');
    }

    $modal.find('[data-bs-dismiss="modal"], [data-dismiss="modal"]').prop('disabled', false);

    if (typeof KTApp !== 'undefined' && typeof KTApp.unblock === 'function') {
        KTApp.unblock('#kt_daterangepicker_modal .modal-content');
    }
}

function DescargarExcel() {
    if (descargandoExcelAuxiliar) return;

    // obtener el valor del datetimepicker
    var dateRange = $('#kt_daterangepicker_1_modal').val();

    // dividir el valor en dos fechas utilizando un guión
    var dates = dateRange.split(' - ');

    if (dates.length !== 2) {
        toastr.warning("Seleccione un rango de fechas válido.", "Reporte Auxiliar", toastrOptions);
        return;
    }

    // construir las fechas en el formato deseado "yyyy-mm-dd"
    var formattedStartDate = dates[0].split('/')[2] + '-' + dates[0].split('/')[1] + '-' + dates[0].split('/')[0];
    var formattedEndDate = dates[1].split('/')[2] + '-' + dates[1].split('/')[1] + '-' + dates[1].split('/')[0];

    mostrarLoadingExcelAuxiliar();
    const url = `${CURL}/DescargarExcelCuadreAuxiliar?cliente=${encodeURIComponent(clientereporteAuxiliar)}&fecha_doc_ini=${encodeURIComponent(formattedStartDate)}&fecha_doc_fin=${encodeURIComponent(formattedEndDate)}`;
    window.location.href = url;

    setTimeout(() => {
        ocultarLoadingExcelAuxiliar();
        $("#kt_daterangepicker_modal").modal("hide");
        toastr.success("Descargando Reporte Cuadre Auxiliar!", "Reporte Auxiliar", toastrOptions);
    }, 3000);

    //// Construir la URL con las fechas formateadas como parámetros
    //var url = `${ecurl}/printReporteAuxiliar?cliente=${clientereporteAuxiliar}&fecha_apli_ini=${formattedStartDate}&fecha_apli_fin=${formattedEndDate}`;

    //// Abrir la URL en una nueva ventana
    //window.open(url, "_blank");
    //// ocultar el modal

}

/// envia reporte de auxiliar para el cliente 

function abrirmodalCorreosAux(CLIENTE, EMAIL) {
    // Cerrar dropdowns abiertos antes de mostrar modal
    $('.menu-sub-dropdown.show').removeClass('show');
    $('body').removeAttr('data-kt-menu');
    // Abrir modal con API de Bootstrap 5 para evitar backdrops huérfanos
    const modalEl = document.getElementById('modalenviocorreoAux');
    if (modalEl) {
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();
    }

    if (EMAIL != '') {
        $("#correosAux").val(EMAIL)
    }

    clientecorreoreporteauxiliar = CLIENTE;

}

function EnviarCorreoReporteAuxiliar() {

    if (!validSC) {
        toastr.error('Por favor edite un correo válido', 'Error');
        $('#correosextraAux').val('');
        return;
    }

    const correoaux = document.getElementById('correosAux').value;
    const correoextraaux = document.getElementById('correosextraAux').value;
    const correosCompletosAux = correoaux + ';' + correoextraaux;

    const dateRange2 = $('#kt_daterangepicker_2_modal').val();
    const dates2 = dateRange2.split(' - ');
    const formattedStartDate2 = dates2[0].split('/')[2] + '-' + dates2[0].split('/')[1] + '-' + dates2[0].split('/')[0];
    const formattedEndDate2 = dates2[1].split('/')[2] + '-' + dates2[1].split('/')[1] + '-' + dates2[1].split('/')[0];

    Dajax(`${CURL}/EnviarReportesAuxiliar`,
        'POST',
        {
            cliente: clientecorreoreporteauxiliar,
            correo: correosCompletosAux,
            fecha_doc_ini: formattedStartDate2,
            fecha_doc_fin: formattedEndDate2
        },
        function (response) {

            switch (response.responseJSON) {

                case '1':
                    toastr.success('Se ha enviado el documento con éxito', 'Éxito', toastrOptions);
                    break;

                case '2':
                    toastr.info(
                        'El cliente no tiene documentos; se envió un archivo con las columnas vacías.',
                        'Envío realizado',
                        toastrOptions
                    );
                    break;

                default:
                    toastr.warning('Hubo un error enviando el archivo', 'Enviar Reporte', toastrOptions);
            }
        });

    $('#modalenviocorreoAux').modal('hide');

}


function abrirModalPDF(DOCUMENTO) {
    $('#pdf_preview').empty();
    Dajax(`${CURL}/ObtenerPDF`,
        "POST", { factura: DOCUMENTO }, function (response) {
            if (response.responseText != "") {
                $('#pdf_preview').removeClass('d-none');
                var cuerpo = '<embed type="application/pdf" id="pdf" src="" style="width:100%; height:900px" />';
                $("#pdf_preview").append(cuerpo);
                $("#pdf").attr("src", response.responseJSON.FACTURA);
                $("#Modal_PDF").modal("show");
            } else {
                toastr.warning("No se encontró el PDF de esta factura");
            }
        });
}

//function abrirmodalCorreosBatch() {
//    $("#modalenviocorreoBatch").modal("show")

//    let correosTemp = []
//    _correos_marcados = []
//    $("#miTabla input:checkbox:checked").each(function () {

//        if ($(this).attr('id') !== 'checkAll') {

//            correosTemp.push($(this).attr('correo'))
//            _correos_marcados.push({
//                cliente: $(this).val(),
//                correo: $(this).attr('correo')
//            })
//        }

//    });
//    correos_seleccionados = correosTemp.join(';');
//    $("#correosBatch").val(correos_seleccionados)
//}


function abrirmodalCorreosBatch() {
    $("#modalenviocorreoBatch").modal("show");

    let correosTemp = [];
    _correos_marcados = [];

    $("#miTabla input:checkbox:checked").each(function () {
        // Verificar si el checkbox actual no es el 'checkAll'
        if ($(this).attr('id') !== 'checkAll') {
            let correo = $(this).attr('correo');

            // Verificar si la columna "correo" está presente
            if (correo) {
                correosTemp.push(correo);

                _correos_marcados.push({
                    cliente: $(this).val(),
                    correo: correo
                });
            }
        }
    });

    correos_seleccionados = correosTemp.join(';');
    $("#correosBatch").val(correos_seleccionados);
}



function EnviarEstadoCuentaBatch() {

    if (!validSC) {
        toastr.error(`Por favor edite un correo valido`, 'Error')
        $("#correosextraBatch").val("");
        return
    }

    let fd = new FormData()
    fd.set('Lineas', JSON.stringify(_correos_marcados))
    fd.set('correoextra', $('#correosextraBatch').val())
    const url = `${CURL}/EnviarCorreoEstadoCuentaBatch`
    fetch(url, { method: 'POST', body: fd })
        .then(res => res.json())
        .then(data => {
            toastr.success(`Enviando estados de cuenta!`, "Éxito", toastrOptions)
        })
        .catch(err => {
            console.error(err)
            toastr.error('Error enviando los reportes', 'Error')
        })

    $('#modalenviocorreoBatch').modal('hide')
}

function DescargarExcel_Index() {

    let valoresCheck = [];
    $("input:checkbox:checked").each(function () {
        valoresCheck.push(this.value);
    });
    let numerosClientes = valoresCheck.filter(valor => valor.trim() !== "on");
    const str = numerosClientes.join(',');

    Dajax(`${CURL}/GenerarExcel_EstadoCuenta_Batch`,
        "POST", { cliente: str }, function (response) {
            let ruta = `${CURL}/DescargarArchivo_Excel_Index?ruta=${response.responseJSON}`
            if (response.responseJSON != 2) {
                toastr.success("Descargando Reporte Excel!", "Reporte Excel", toastrOptions);
            } else {
                toastr.warning("Error descargando el documento!", "Reporte Excel", toastrOptions);
            }
            setTimeout(() => { window.location.href = ruta; }, 1000)

        });
}

function verFechas() {
    //const datos = [
    //    "2 | 08-10-2023 | 15-10-2023",
    //    "3 | 16-10-2023 | 23-10-2023"
    //];
    //const datos= []
    let datosFechas = $("#semana option:selected").text().split('S')
    datosFechas.splice(0, 1);
    const semanasFechas = [];

    for (const dato of datosFechas) {
        const partes = dato.split('|');
        const semanaf = partes[0].trim();
        const fechaIniciof = partes[1].trim();
        const fechaFinf = partes[2].trim();

        const fechaSemana = `${semanaf}|${fechaIniciof}|${fechaFinf}`;
        semanasFechas.push(fechaSemana);
    }

    let txtfechas = ``
    semanasFechas.forEach(element => {
        txtfechas += element += ","
    })
    txtfechas = txtfechas.slice(0, -1);
    return txtfechas
}

function documentosVencidos(cod_cliente) {
    //toastr.success(`CARGA TEXTBOX CON DATOS DE FACTURAS ${cod_cliente} `, 'Éxito', toastrOptions)

    let fd = new FormData()
    fd.set('cliente', cod_cliente)
    const url = `${CURL}/Obtener_Documentos_VencidosCliente`
    fetch(url,
        {
            method: 'POST',
            body: fd
        })
        .then(res => res.json())
        .then(data => {
            // Verifica si hay al menos un elemento en el array
            if (data.length > 0) {
                // Accede al valor de la propiedad DOC_VENC_DESCRIPCION del primer objeto en el array
                let docVencDescripcion = data[0].DOC_VENC_DESCRIPCION;
                $('#Doc_Vencido').val(docVencDescripcion);
            }
        })
        .catch(err => {
            console.error(err)
            toastr.error('Error cargando documentos vencidos', 'Error')
        })
}


function ManejarBotones() {

    $("#Pagos").click(function () {
        // Tu lógica para el evento click aquí
        realizarPago = true
        guardarVisita();
        /*alert("Se hizo clic en el botón Pagos " + realizarPago);*/
    });


}


//function irPedido(cliente, fecha_ultima_visita, ID_VISITA) {
//    // Verificar si id_visita está definido y no está vacío
//    console.log(cliente, fecha_ultima_visita, id_visita);
//    if (id_visita && id_visita !== 'null') {
//        localStorage.setItem('id_visita_actual', id_visita);
//    }

//    // Cambiar el cliente en la sesión
//    CambiarClienteSesion(cliente);

//    // Construir la URL utilizando template literals
//    const url = `${PPURL}?cliente=${cliente}&fecha_visita=${fecha_ultima_visita}&idvisita=${id_visita}`;
//     window.location.href = url;
//}

// function irPedido(CLIENTE, ID_VISITA) {

//     let accion = "actualizar";
//     if (ID_VISITA === null || ID_VISITA === 'null') {
//         toastr.error("No puede crear un pedido sin una visita. Por favor, asegúrese de crear una visita antes de continuar. ", 'Error');
//     } else {
//         CambiarIdVisita(ID_VISITA);
//         const url = `${urlEcomercePedido}/Pedido/Historial?Cliente=${CLIENTE}`;
//         CambiarClienteSesion(CLIENTE);


//         setTimeout(() => { window.location.href = url; }, 1000);
//     }
// }

// CORRECTA
//--------------------------------------------------------------------------------------------------------------
// function irPedido(cliente, idVisita) {
//     if (!idVisita || idVisita === 'null') {
//         toastr.error(
//             "No puede crear un pedido sin una visita. Por favor, asegúrese de crear una visita antes de continuar.",
//             "Error"
//         );
//         return;
//     }

//     CambiarIdVisita(idVisita);

//     const url =
//         `${urlEcomercePedido}/Pedido/Historial` +
//         `?cliente=${encodeURIComponent(cliente)}`;

//     window.location.href = url;
// }
//-------------------------------------------------------------------------------------------------------------
// Se modificó la funcion para que usara "cliente" en vez de "Cliente" y se le quitó el setTimeout(...) ya que eso hacia 
//que la redirección ocurriera antes de que el cliente pueda actualizarse, cuando llegaba a Historial, el parametro ya habia desparecido, como en el video.
// function irPedido(cliente, idVisita) {
//     if (!idVisita || idVisita === 'null') {
//         toastr.error(
//             "No puede crear un pedido sin una visita. Por favor, asegúrese de crear una visita antes de continuar.",
//             "Error"
//         );
//         return;
//     }

//     CambiarIdVisita(idVisita);

//     const baseEcommerce = urlEcomercePedido.replace(/\/+$/, '');

//     const url =
//         `${baseEcommerce}/` +
//         `?cliente=${encodeURIComponent(cliente)}`;

//     window.location.href = url;
// }
function irPedido(cliente, idVisita) {
    if (!idVisita || idVisita === 'null') {
        toastr.error(
            "No puede crear un pedido sin una visita. Por favor, asegúrese de crear una visita antes de continuar.",
            "Error"
        );
        return;
    }

    CambiarIdVisita(idVisita);

    window.location.href =
        `${CURL}/IrEcommerce?cliente=${encodeURIComponent(cliente)}`;
}

function irPago(CLIENTE, ID_VISITA, COD_NOMBRE_COBRADOR, ALIAS, COD_NOMBRE_VENDEDOR) {

    if (!ID_VISITA || ID_VISITA === 'null') {
        toastr.error("No puede crear un pago sin una visita.");
        return;
    }

    if (!PERMISOS_MENU.split(',').includes("48")) {
        toastr.info("No tiene permiso para acceder a la pantalla de pagos.");
        return;
    }

    const [codCob, nomCob] = COD_NOMBRE_COBRADOR.split(' - ');

    if (!COD_NOMBRE_VENDEDOR) {
        COD_NOMBRE_VENDEDOR = $('#vendedorFiltro option:selected').text();
    }
    const [codVen, nomVen] = COD_NOMBRE_VENDEDOR.split(' - ');

    localStorage.setItem('id_visita_actual', ID_VISITA);
    CambiarIdVisita(ID_VISITA);

    CambiarCobradorSesion(codCob, nomCob);
    CambiarVendedorSesion(codVen, nomVen);
    CambiarClienteSesion(CLIENTE);

    const url = `${RURL}?cliente=${CLIENTE}&alias=${ALIAS}`
        + `&idvisita=${ID_VISITA}&cobrador=${codCob}&accion=actualizar`;

    setTimeout(() => location.href = url, 600);
}

function irVisita(CLIENTE, ID_VISITA) {

    if (!PERMISOS_MENU.split(',').includes("47")) {
        toastr.info("No tiene permiso para acceder  a la pantalla de visitas.");
        return;
    }

    if (ID_VISITA === null || ID_VISITA === 'null') {
        toastr.error("No hay visitas para este cliente. Por favor, asegúrese de crear una visita antes de continuar.", 'Error');
    } else {
        localStorage.setItem('id_visita_actual', ID_VISITA);
        CambiarClienteSesion(CLIENTE);

        const url = `${vurl}?cliente=${CLIENTE}&idvisita=${ID_VISITA}&accion=actualizar`;
        window.location.href = url;
    }
}



function CambiarClienteSesion(cliente) {
    if (cliente != "" && cliente != null) {
        $.ajax({
            url: `${HOME_URL}/CambiarCliente`,
            data: { clienteinfo: cliente },
            async: true,
            type: "POST",
            success: function (data) {
            },
            error: function (data) {
                toastr.error(data.der);
            }
        });
    } else {
        toastr.warning("Debe seleccionar un vendedor.");
    }
}



function CambiarIdVisita(idvisita) {
    $.ajax({
        url: `${HOME_URL}/CambiarVisita`,
        data: { idvisita: idvisita },
        async: true,
        type: "POST",
        success: function (data) {
            console.log(data)
        },
        error: function (data) {
            toastr.error(data.der);
        }
    });

}

function CambiarVendedorSesion(codVendedor, nomVendedor) {
    if (!codVendedor) {                                 // ”Todos” o vacío
        toastr.warning("Debe seleccionar un vendedor.");
        return;
    } 
    $.ajax({
        url: `${HOME_URL}/CambiarVenedorSession`,
        type: 'POST',
        data: { vendedor: codVendedor, nombrevendedor: nomVendedor },
        success: d => { if (d !== "1") toastr.error("No se pudo cambiar vendedor."); },
        error: () => toastr.error("Error de red al cambiar vendedor.")
    });
}

/** Cambia el cobrador en la sesión */
function CambiarCobradorSesion(codCobrador, nomCobrador) {
    if (!codCobrador) {
        toastr.warning("Debe seleccionar un cobrador.");
        return;
    }
    $.ajax({
        url: `${HOME_URL}/CambiarCobradorSession`,
        type: 'POST',
        data: { cobrador: codCobrador, nombrecobrador: nomCobrador },
        success: d => { if (d !== "1") toastr.error("No se pudo cambiar cobrador."); },
        error: () => toastr.error("Error de red al cambiar cobrador.")
    });
}

function CambiarCobradorPago(COD_NOMBRE_COBRADOR) {
    console.log(COD_NOMBRE_COBRADOR);
    // Obtén el texto seleccionado en el combobox
    var textoSeleccionado = COD_NOMBRE_COBRADOR;

    // Divide el texto en código y nombre utilizando el delimitador " - "
    var partes = textoSeleccionado.split(" | ");

    // Si hay al menos dos partes, la primera será el código y la segunda el nombre
    if (partes.length >= 2) {
        var codigoVendedor = partes[0].trim(); // Elimina espacios en blanco alrededor del código
        var nombreVendedor = partes[1].trim(); // Elimina espacios en blanco alrededor del nombre
    }

    let fd = new FormData()
    fd.set('cobrador', codigoVendedor)
    fd.set('nombrecobrador', nombreVendedor)
    fetch(`${HOME_URL}/CambiarCobradorSession`,
        {
            method: 'POST',
            body: fd
        })
        .then(res => res.json())
        .then(data => {
            if (data == 1) {

                //window.location.reload()

            } else {
                toastr.warning('Vendedor no válida', 'Alerta', toastrOptions)
            }
        })
        .catch(err => {
            console.error(err)
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions)
        })

}


function actualizaPagina() {
    const parametros = new URLSearchParams(window.location.search);
    const accion = parametros.get('accion');
    if (accion === 'actualizar') {
        console.log("La página se ha actualizado.");
        // Eliminar el parámetro "accion" de la URL
        parametros.delete('accion');
        const nuevaURL = window.location.pathname + '?' + parametros.toString();
        history.replaceState({}, '', nuevaURL);
        // Actualizar la página después de un breve retraso
        location.reload(true); // Recarga la página desde el servidor
    }
    else {
        KTDatatablesServerSide.init();
    }

};



function handlePrint(url) {
    // Detección simple del dispositivo
    const isTablet = /iPad|Android|Touch/.test(navigator.userAgent);

    if (isTablet) {
        // Para tabletas, forzar la descarga
        window.location.href = url + '&download=true';
    } else {
        // Para computadoras, abrir en nueva pestaña
        window.open(url, '_blank');
    }
}


function validarCorreos(campoCorreosID) {
    const input = document.getElementById(campoCorreosID).value;
    const correos = input.split(';').map(correo => correo.trim());
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    validSC = true;
    mensajesDeError = [];

    correos.forEach(correo => {
        if (correo.length > 0 && !emailRegex.test(correo)) {
            validSC = false;
            mensajesDeError.push(`"${correo}" no es un correo electrónico válido. Por favor, ingrese un correo válido.`);
        }
    });

    if (validSC) {
        //toastr.success('Todos los correos son válidos.', 'Éxito');
    } else {
        toastr.error(mensajesDeError.join(' '), 'Error');

    }
} function preseleccionarSemanaActual() {
    // 1) Fecha de hoy (sin hora)
    const hoy = new Date();
    hoy.setHours(0, 0, 0, 0);

    // 2) Calculamos todas las opciones que caen en el rango de hoy
    const seleccion = [];
    $("#semana option").each(function () {
        const valor = this.value;
        if (!valor || valor === "0") return;   // salto placeholder y "Todas"

        // texto: "S3 | 15-06-2025 | 21-06-2025"
        const partes = $(this).text().split("|").map(s => s.trim());
        if (partes.length < 3) return;

        const inicio = partes[1], fin = partes[2];
        const [dIni, mIni, yIni] = inicio.split("-").map(n => parseInt(n, 10));
        const [dFin, mFin, yFin] = fin.split("-").map(n => parseInt(n, 10));

        const dateInicio = new Date(yIni, mIni - 1, dIni);
        const dateFin = new Date(yFin, mFin - 1, dFin);

        if (hoy >= dateInicio && hoy <= dateFin) {
            seleccion.push(valor);
        }
    });

    if (seleccion.length) {
        // 3) Seteamos sólo la UI de Select2, sin disparar el change nativo
        $("#semana").val(seleccion)
            .trigger("change.select2");
        console.log("Semana actual preseleccionada:", seleccion);
    }
}

function reimprimirTiquete(documento) {
    $.getJSON(RURL + '/ObtenerComprobanteAplicacion?documento=' + documento, (json) => {
        const encoded = encodeURIComponent(json.tiquete);

        const rawbtIntent = `intent://${encoded}#Intent;scheme=rawbt;package=ru.a402d.rawbtprinter;end`;

        window.location.href = rawbtIntent;
    });
}

$(document).ready(function () {
    // 1) Inicializa tu select2 (con tu helper)
    inicializarSelectMultipleConTodos('#semana');

    // 2) Preselecciona la semana que cae hoy
    //preseleccionarSemanaActual();
    ManejarFiltros();
    ManejarBotones();
    $("#kt_daterangepicker_1_modal").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });
    $("#kt_daterangepicker_2_modal").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });

    ///*setTimeout(() => {*/
    //KTDatatablesServerSide.init();
    ///*}, 100)*/
    actualizaPagina();
});
