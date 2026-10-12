let arrayejemplo = []
let existeNumBancoTEF = 'N'
let comprobantePdfActual = '';

function toNumber(val) {
    return Number(String(val).replace(/[^\d.-]/g, '')) || 0;
}
var totaltef = 0
var checkedNodes = [];
var cantparciales = 0;
var factscobrar = []
var montoselec = 0
var montoyaproyectado = 0
var saldodeNC = 0
let MontodeNC = 0
let saldoNCDistribuido = 0;
let comentarioTesoreria = "";

// VARIABLES DE DESARROLLO 
let fechaInicioFiltro = '';
let fechaFinFiltro = '';
let tipoDocFiltro = $('#tipodoc').val();
let vendedorFiltro = ($('#vendedorFiltro').val() || '0').trim();
let cobradorFiltro = ($('#cobradorFiltro').val() || '0').trim();
let clienteFiltro = ($('#clientesFiltro').val() || '0').trim();

// Declara un array para almacenar los saldos seleccionados
let saldosSeleccionados = [];
let montoSeleccionados = []
let _correos_marcados = [];
let conceptosArray = [];
let comentarioTesoreriaArray = [];
let conceptoText = '';
let notaSeleccionada = '';
let subtipo_doc = '';
let filaDatos = [];
let tipoCredito = '';
let sumaMontoAplicar = 0;
let saldorest = 0;
let montoAplicarDebito = 0
let clientecorreoFERP = "";
let docsInteres = [];
let interesesCargados = false;
const pagosSeleccionMap = new Map();
const MAX_CONCEPTO_RECIBO = 240;
// Identifica si el sistema se está ejecutando desde un dispositivo Android.
// RawBT únicamente debe utilizarse en este tipo de dispositivo.
const esAndroidRegistroPagos = /Android/i.test(navigator.userAgent || '');

function bloquearBotonesRegistroPagos(selector) {
    const $botones = $(selector);
    if (!$botones.length || $botones.is('[data-accion-bloqueada="true"]')) return null;

    $botones.attr('data-accion-bloqueada', 'true').prop('disabled', true);
    return function () {
        $botones.removeAttr('data-accion-bloqueada').prop('disabled', false);
    };
}

function construirRowKeyDesdeDatos(row) {
    const id = String(row && row.ID != null ? row.ID : '').trim();
    if (id && id.toLowerCase() !== 'undefined' && id.toLowerCase() !== 'null') return id;

    const tipo = String((row && row.TIPO) || '').trim();
    const doc = String((row && row.DOCUMENTO) || '').trim();
    const cli = String((row && row.CLIENTE) || '').trim();
    const fecha = String((row && row.FECHA_DOCUMENTO) || '').trim();
    return [tipo, doc, cli, fecha].join('|');
}

function construirRowKeyDesdeCheckbox($checkbox) {
    const dataKey = String($checkbox.attr('data-rowkey') || '').trim();
    if (dataKey) return dataKey;

    const id = String($checkbox.val() || $checkbox.attr('value') || '').trim();
    if (id && id.toLowerCase() !== 'undefined' && id.toLowerCase() !== 'null') return id;

    const tipo = String($checkbox.attr('tipodoc') || '').trim();
    const doc = String($checkbox.attr('documento') || '').trim();
    const cli = String($checkbox.attr('cliente') || '').trim();
    const fecha = String($checkbox.attr('fecha_documento') || '').trim();
    return [tipo, doc, cli, fecha].join('|');
}

function construirConceptoRecibo(conceptos) {
    const base = `Pago de facturas: ${conceptos.join(', ')}`;
    if (base.length <= MAX_CONCEPTO_RECIBO) return base;
    return `${base.substring(0, MAX_CONCEPTO_RECIBO - 3)}...`;
}

function obtenerEstadoPago(rowId) {
    return pagosSeleccionMap.get(String(rowId)) || null;
}

function guardarEstadoPago(rowId, patch) {
    const key = String(rowId);
    const current = pagosSeleccionMap.get(key) || {};
    pagosSeleccionMap.set(key, Object.assign({}, current, patch, { id: key }));
}

function sincronizarFilaPago($checkbox) {
    const rowId = construirRowKeyDesdeCheckbox($checkbox);
    if (!rowId) return;

    const $input = $checkbox.closest('tr').find('.montopagos');
    const montoInput = toNumber($input.val());
    const saldo = toNumber($checkbox.attr('saldo'));

    guardarEstadoPago(rowId, {
        checked: $checkbox.prop('checked'),
        monto: montoInput > 0 ? montoInput : saldo,
        saldo: saldo,
        tipoDoc: ($checkbox.attr('tipodoc') || '').trim(),
        documento: ($checkbox.attr('documento') || '').trim(),
        cliente: ($checkbox.attr('cliente') || '').trim(),
        fecha: ($checkbox.attr('fecha_documento') || '').trim(),
        subtipo: ($checkbox.attr('subtipo') || '').trim(),
        comentarioTes: ($checkbox.attr('comentarioTes') || '').trim()
    });
}

// function restaurarSeleccionPaginaActual() {
//     $("#miTabla input[type='checkbox']").each(function () {
//         const $cb = $(this);
//         const rowId = construirRowKeyDesdeCheckbox($cb);
//         if (!rowId) return;

//         const estado = obtenerEstadoPago(rowId);
//         const saldo = toNumber($cb.attr("saldo"));
//         const $input = $cb.closest('tr').find('.montopagos');

//         if (estado) {
//             $cb.prop('checked', !!estado.checked);
//             const monto = toNumber(estado.monto) > 0 ? toNumber(estado.monto) : saldo;
//             $input.val(formatearMiles(monto.toFixed(2)));
//             $input.prop('disabled', !estado.checked);
//             sincronizarFilaPago($cb);
//         } else {
//             $input.val(formatearMiles(saldo.toFixed(2)));
//             $input.prop('disabled', true);
//             sincronizarFilaPago($cb);
//         }
//     });
// }

// Al redibujar la tabla se toma siempre como límite el saldo actualizado
// devuelto por el servidor. Antes, si pagosSeleccionMap conservaba un monto
// anterior, ese monto tenía prioridad aunque fuera mayor que el nuevo saldo.
// Ahora un valor almacenado nunca puede restaurarse por encima del saldo actual.
function restaurarSeleccionPaginaActual() {

    $("#miTabla input[type='checkbox']").each(function () {

        const $cb = $(this);
        const rowId = construirRowKeyDesdeCheckbox($cb);

        if (!rowId) return;

        const estado = obtenerEstadoPago(rowId);

        // Este saldo pertenece a la fila recién devuelta por el servidor.
        const saldoActual = toNumber($cb.attr("saldo"));

        const $input = $cb.closest('tr').find('.montopagos');

        if (estado) {

            const checked = !!estado.checked;
            const montoGuardado = toNumber(estado.monto);

            // Nunca restaurar un monto superior al saldo que actualmente
            // tiene el documento.
            const monto = Math.min(
                montoGuardado > 0 ? montoGuardado : saldoActual,
                saldoActual
            );

            $cb.prop('checked', checked);

            $input.val(
                formatearMiles(monto.toFixed(2))
            );

            $input.prop('disabled', !checked);

            // Sincroniza nuevamente el estado utilizando tanto el monto
            // corregido como el saldo recién recibido.
            sincronizarFilaPago($cb);

        } else {

            $input.val(
                formatearMiles(saldoActual.toFixed(2))
            );

            $input.prop('disabled', true);

            sincronizarFilaPago($cb);
        }
    });
}


const GenerarIdTemporal = () => {
    // Genera una cadena aleatoria como semilla
    const seed = Math.random().toString(36).substring(2);

    // Genera una cadena aleatoria como valor del token
    const token = Math.random().toString(36).substring(2);

    // Calcula el hash SHA256 de la semilla y el valor del token
    //const hash = crypto.subtle.digest("SHA-256", new TextEncoder().encode(seed + token));

    //// Convierte el hash a una cadena hexadecimale
    //const hashArray = Array.from(new Uint8Array(hash));
    //const hashHex = hashArray.map(b => b.toString(16).padStart(2, "0")).join("");
    const hashHex = '';
    // Combina la semilla y el valor del token en un formato legible
    const finalToken = `${seed}${token}${hashHex}`;

    return finalToken;
}


var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $('#miTabla').DataTable({
            processing: true,
            //serverSide: true, 
            //responsive: isMobile,
            scrollX: true,         // Habilita scroll horizontal
            autoWidth: false,
            stateSave: false,
            pageLength: 25,
            order: [[2, 'asc']],
            ajax: {
                url: `${URLRP}/Obtener_documentos_clientes_fac`,
                type: 'POST',
                data: function (d) {


                    d.pagina = (d.start / d.length) + 1;
                    d.tamPagina = d.length;

                    d.tipoDoc = ($('#tipodoc').val() || '0').trim();
                    d.cliente = ($('#clientesFiltro').val() || '0').trim();

                    const vend = ($('#vendedorFiltro').val() || '0').trim().toUpperCase();
                    d.vendedor = (vend === 'ND' || vend === 'ND') ? '0' : vend;

                    d.cobrador = ($('#cobradorFiltro').val() || '0').trim();
                    d.searchTxt = d.search && d.search.value ? d.search.value.trim() : '';

                },

                dataSrc: 'data'
            },
            columns: [
                {
                    "data": "ID",
                    "defaultContent": ``
                },
                { "data": "CLIENTE" },
                { "data": "TIPO" },
                { "data": "DOCUMENTO" },
                { "data": "FECHA_DOCUMENTO" },
                { "data": "FECHA_VENCE" },
                { "data": "DIAS_NETO" },
                {
                    "data": null,
                    orderable: true,
                    render: function (data, type, row) {
                        const condicionPago =
                            row.CONDICION_PAGO_DESC_DEB ||
                            row.CONDICION_PAGO_DEB ||
                            row.CONDICION_PAGO_DESC_CRE ||
                            row.CONDICION_PAGO_CRE ||
                            row.CONDICION_PAGO ||
                            '';
                        const diasNeto = row.DIAS_NETO ?? '';

                        if (type === 'sort') {
                            return condicionPago || diasNeto;
                        }

                        if (condicionPago) {
                            return condicionPago;
                        }

                        if (diasNeto !== '') {
                            return `${diasNeto} días`;
                        }

                        return 'N/D';
                    }
                },
                { "data": "MONEDA_DEBITO" },
                { "data": "MONTO" },
                { "data": "SALDO" },
                { "data": "MONTO_PAGO" },
                { "data": "COD_VENDEDOR_CLI" },
                { "data": "COBRADOR_DEBITO" },
                { "data": "COMENTARIO_DEBITO" },
                /*               { "data": null },*/
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: function (data, type, row) {
                        const rowKey = construirRowKeyDesdeDatos(row);
                        const estado = obtenerEstadoPago(rowKey);
                        const checkedAttr = estado && estado.checked ? 'checked' : '';
                        return `
                            <div class="form-check form-check-sm form-check-custom form-check-solid">
                                <input class="form-check-input" type="checkbox" value="${row.ID}" data-rowkey="${rowKey}" saldo="${row.SALDO}" tipodoc="${row.TIPO}" monto=${row.MONTO_DEBITO} documento="${row.DOCUMENTO}" cliente ="${row.CLIENTE}"  fecha_documento ="${row.FECHA_DOCUMENTO}" subtipo ="${row.COD_SUBTIPO_DEBITO}" comentarioTes ="${row.COMENTARIO}" ${checkedAttr}/>
                            </div>`;
                    }
                },
                {
                    targets: 1,
                    orderable: true,
                    render: function (data, type, row) {
                        if (type === 'sort') {
                            return row.CLIENTE;
                        }
                        return `${row.CLIENTE} - ${row.CLIENTE_NOMBRE}`
                    }
                },
                {
                    targets: 2,
                    orderable: true,
                    render: function (data, type, row) {

                        /* * Lógica de Ordenamiento 
                         * -------------------------------------------------------------------------
                         * Prioridad de orden: 
                         * 1. Tipo Documento (INT primero via prefijo '0')
                         * 2. Fecha Vencimiento (Cronológico ASC via normalización YYYYMMDD)
                         * 3. Monto (Numérico DESC via inversión de valor base y padding de ceros)
                         * * Se utiliza 'type === sort' para generar una llave de comparación única
                         * sin afectar la representación visual de la celda (Badges Metronic).
                         */

                        if (type === 'sort') {
                            // 1. Prioridad Tipo documento
                            let p = (data === 'INT') ? '0' : '1';

                            // 2. Prioridad Fecha vencimiento
                            let f = '99999999';
                            if (row.FECHA_VENCE) {
                                // Limpieza de posibles espacios y split
                                let fClean = row.FECHA_VENCE.trim();
                                let fArr = fClean.split('/');
                                if (fArr.length === 3) {
                                    let dia = fArr[0].padStart(2, '0');
                                    let mes = fArr[1].padStart(2, '0');
                                    let anio = fArr[2];
                                    f = anio + mes + dia;
                                }
                            }

                            // 3. Prioridad Monto 
                            let montoLimpio = 0;
                            if (row.MONTO) {
                                let strMonto = row.MONTO.toString().replace(/[^0-9.-]+/g, "");
                                montoLimpio = parseFloat(strMonto) || 0;
                            }
                            let m = (1000000000 - montoLimpio).toFixed(2).padStart(15, '0');

                            let sortKey = `${p}_${f}_${m}`;

                            return sortKey;
                        }

                        var status = {
                            "FAC": { 'title': 'FAC', 'class': 'badge-light-success' },
                            "REC": { 'title': 'REC', 'class': ' badge-light-warning' },
                            "TEF": { 'title': 'TEF', 'class': ' badge-light-info' },
                            "N/C": { 'title': 'N/C', 'class': ' badge-light-primary' },
                            "N/D": { 'title': 'N/D', 'class': ' badge-light-success' },
                            "O/C": { 'title': 'O/C', 'class': ' badge-light-danger' },
                            "O/D": { 'title': 'O/D', 'class': ' badge-light-warning' },
                            "INT": { 'title': 'INT', 'class': 'badge-light-success' },
                        };


                        return '<span class="badge badge-lg ' + status[row.TIPO].class + '  " >' + status[row.TIPO].title + '</span>';
                    }
                },
                {
                    targets: 4,
                    orderable: true,
                    render: function (data, type, row) {
                        if (type === 'sort') {
                            return row.FECHA_DOCUMENTO;
                        }
                        const ticks = parseInt(data.substr(6));
                        return new Date(ticks).toLocaleDateString('es-CR');

                    }
                },
                /* FECHA_VENCE */
                {
                    targets: 5,
                    orderable: true,
                    render: function (data, type, row) {
                        if (type === 'sort') {
                            return row.FECHA_VENCE;
                        }
                        const ticks = parseInt(data.substr(6));
                        return new Date(ticks).toLocaleDateString('es-CR');

                    }
                },
                {
                    targets: 8,
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.MONEDA_DEBITO}`;
                    }
                },
                {
                    targets: 9,
                    orderable: true,
                    className: 'dt-right',
                    render: function (data, type, row) {
                        if (type === 'display') {
                            return `₡${parseFloat(row.MONTO).toLocaleString('en-US')}`;
                        }
                        return data;
                    }
                },
                {
                    targets: 10,
                    orderable: true,
                    className: 'dt-right',
                    render: function (data, type, row) {
                        if (type === 'display') {
                            return `<span class="text-danger">₡${toNumber(row.SALDO).toLocaleString('en-US')}</span>`
                        }
                        return data;
                    }
                },
                {
                    targets: 11,
                    orderable: false,
                    className: 'dt-right',
                    render: function (data, type, row) {
                        const rowKey = construirRowKeyDesdeDatos(row);
                        const estado = obtenerEstadoPago(rowKey);
                        const montoInicial = estado && toNumber(estado.monto) > 0
                            ? toNumber(estado.monto)
                            : toNumber(row.SALDO);
                        const disabledAttr = estado && estado.checked ? '' : 'disabled';
                        return `
        <input  type="text"
                class="form-control montopagos text-end"
                data-rowid="${row.ID}"         
                data-saldo="${row.SALDO}"     
                value="${montoInicial.toLocaleString('en-US')}"
                ${disabledAttr}
                onchange="validarMontoAbono(this)">
    `

                    }
                },
                {
                    targets: 12,
                    orderable: false,
                    render: function (data, type, row) {
                        return row.COD_VENDEDOR_CLI + '-' + row.VENDEDOR_DEBITO
                    }
                },
                {
                    targets: 13,
                    orderable: false,
                    render: function (data, type, row) {
                        return row.COBRADOR_DEBITO + '-' + row.NOMBRE_COBRADOR_DEBITO
                    }
                },
                {
                    targets: 14,
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.COMENTARIO_DEBITO}`
                    }
                },
                //{
                //    targets: 14,
                //    orderable: false,
                //    render: function (data, type, row) {
                //        let accionesRegistroPagos = "";

                //        if (PERMISOS_PORTAL_DE_CLIENTES.split(',').includes("24")) {
                //            accionesRegistroPagos += `<a onclick="abrirmodalCorreosPDF('${'ND'}','${row.CLIENTE}', '${row.DOCUMENTO}')" title="Enviar por correo" class="btn btn-sm btn-clean btn-icon me-2 mb-2">
                //                                 <i class="la la-envelope fs-2x"></i>
                //                                 </a>`
                //        }
                //        return accionesRegistroPagos;

                //    }
                /*  },*/

                // Responsive priorities
                //{ responsivePriority: 1, targets: 0 }, 
                //{ responsivePriority: 1, targets: 1 },  // Checkbox
                //{ responsivePriority: 1, targets: 2 },  // CLIENTE
                //{ responsivePriority: 1, targets: 10 },  // CLIENTE
                //{ responsivePriority: 100, targets: [3,4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14] } // resto


            ],
            // Add data-filter attribute
            //createdRow: function (row, data, dataIndex) {
            //    $(row).find('td:eq(4)').attr('data-filter', data.CreditCardType);
            //}
        });
        table = dt.$;

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        dt.on('draw', function () {
            restaurarSeleccionPaginaActual();
            seleccionarCreditos();

        });

        $(document)
            .off('click.registroPagos', '#miTabla input.form-check-input[type="checkbox"]')
            .on('click.registroPagos', '#miTabla input.form-check-input[type="checkbox"]', function (e) {
                e.stopPropagation();
            });

        $(document)
            .off('change.registroPagos', '#miTabla input.form-check-input[type="checkbox"]')
            .on('change.registroPagos', '#miTabla input.form-check-input[type="checkbox"]', function () {
                sincronizarFilaPago($(this));
                seleccionarCreditos();
            });
    }

    $('#checkAll').on('click', function () {
        setTimeout(function () {
            ManejarCantidadCheckboxes();
        }, 50);
    });

    var Filtros = function () {
        $('#kt_datatable_search_query').on('keyup', function () {
            //const val = $(this).val() == '0' ? '' : $(this).val()
            dt.search($(this).val()).draw();
        });
        //const filterSearch = document.querySelector('[data-kt-docs-table-filter="search"]');
        //filterSearch.addEventListener('keyup', function (e) {
        //    dt.search(e.target.value).draw();
        //});
    }

    var ListenerCheckboxes = () => { }
    // Public methods
    return {
        init: function () {
            initDatatable();
            Filtros();
            ListenerCheckboxes();
        },
        reload: function () {
            if (dt) { dt.ajax.reload(null, false); }
        }
    }
}();
$(document).on('filtros-actualizados', function (e, filtros) {
    vendedorFiltro = filtros.vendedor;
    cobradorFiltro = filtros.cobrador;
    clienteFiltro = filtros.cliente;
    pagosSeleccionMap.clear();
    $('#miTabla').DataTable().ajax.reload(null, false);
});

function validarMontoAbono(inputEl) {
    const $checkbox = $(inputEl).closest('tr').find('input[type="checkbox"]');
    const rowId = construirRowKeyDesdeCheckbox($checkbox);

    const saldo = toNumber($checkbox.attr('saldo'));
    let valor = toNumber(inputEl.value);

    // Si está vacío o menor o igual a cero
    if (!valor || valor <= 0) {
        toastr.warning("El monto del abono no puede ser cero o vacío.", toastrOptions);
        inputEl.value = formatearMiles(saldo.toFixed(2));
        return;
    }

    // Si el monto es mayor al saldo permitido
    if (valor > saldo) {
        toastr.warning("El monto del abono no puede ser mayor al saldo.", toastrOptions);
        inputEl.value = formatearMiles(saldo.toFixed(2));
        return;
    }

    // Si pasa validaciones, actualiza el atributo del checkbox para que lo lea correctamente
    $checkbox.attr("monto", valor);
    inputEl.value = formatearMiles(valor.toFixed(2));
    guardarEstadoPago(rowId, { monto: valor, checked: $checkbox.prop("checked") });

    // Recalcula totales
    seleccionarCreditos();
}


var ManejarCantidadCheckboxes = () => {
    $("#miTabla input[type='checkbox']").each(function () {
        sincronizarFilaPago($(this));
    });
    seleccionarCreditos();
}

function updateTotal(inputEl) {
    const numero = toNumber(inputEl.value);
    inputEl.value = formatearMiles(numero.toFixed(2));

    // ← NUEVA LÍNEA: guarda el nuevo monto en el checkbox
    $(inputEl)
        .closest('tr')
        .find('input[type="checkbox"]')
        .attr('monto', numero);            // ahora seleccionarCreditos lo lee bien

    seleccionarCreditos();                // recalcula todo
}
function seleccionarCreditos() {

    const nc = $("#ncsf").val();
    const saldoNC = nc !== "0" ? toNumber($("#ncsf option:selected").attr("saldo")) : 0;

    totaltef = 0;
    let suma = 0,
        contDoc = 0,
        checked = false;

    saldosSeleccionados.length = 0;
    montoSeleccionados.length = 0;
    conceptosArray.length = 0;
    filaDatos.length = 0;
    _correos_marcados.length = 0;
    $("#notasaplicar").empty();

    const visibles = Array.from($("#miTabla input[type='checkbox']"));
    const inputsVisiblesPorRowId = new Map();
    visibles.forEach(function (e) {
        const $cb = $(e);
        const rowId = construirRowKeyDesdeCheckbox($cb);
        const estado = obtenerEstadoPago(rowId);
        const $input = $cb.closest('tr').find('.montopagos');
        const checked = estado ? !!estado.checked : $cb.prop("checked");
        const monto = estado ? toNumber(estado.monto) : toNumber($input.val());
        const saldo = estado ? toNumber(estado.saldo) : toNumber($cb.attr("saldo"));
        const montoMostrar = monto > 0 ? monto : saldo;

        inputsVisiblesPorRowId.set(rowId, $input);
        $cb.prop("checked", checked);
        $input.prop("disabled", !checked);
        if (montoMostrar > 0) {
            $input.val(formatearMiles(montoMostrar.toFixed(2)));
        }
    });

    let seleccionados = Array.from(pagosSeleccionMap.values())
        .filter(x => x && x.checked)
        .sort((a, b) => {
            const aIsINT = (a.tipoDoc || "") === "INT";
            const bIsINT = (b.tipoDoc || "") === "INT";
            if (aIsINT && !bIsINT) return -1;
            if (!aIsINT && bIsINT) return 1;
            return 0;
        });

    seleccionados.forEach(function (item) {
        checked = true;

        const tipoDoc = item.tipoDoc;
        const saldo = toNumber(item.saldo);

        if (
            !["FAC", "INT", "O/D", "N/D"].includes(tipoDoc) ||
            saldo <= 0
        ) {
            return;
        }

        const montoSolicitado =
            toNumber(item.monto) > 0
                ? toNumber(item.monto)
                : saldo;

        const documento = item.documento;
        const cliente = item.cliente;
        const fecha = item.fecha;
        const subtipo = item.subtipo;
        const rowId = item.id;

        const $input = inputsVisiblesPorRowId.get(rowId);

        /*
         * Validación final antes de construir filaDatos.
         * Antes, para recibos normales se utilizaba directamente el monto almacenado
         * en pagosSeleccionMap, aunque fuera mayor que el saldo actual del documento.
         * Ahora el monto aplicado queda limitado al saldo disponible, evitando que un
         * valor desactualizado pueda producir una aplicación mayor al saldo.
         */
        let montoAplicado = Math.min(montoSolicitado, saldo);

        // Si encontramos un monto desactualizado, también corregimos
        // el estado almacenado y el valor mostrado en pantalla.
        if (montoSolicitado > saldo) {

            guardarEstadoPago(rowId, {
                monto: saldo,
                saldo: saldo
            });

            if ($input && $input.length) {
                $input.val(
                    formatearMiles(saldo.toFixed(2))
                );
            }
        }

        if (nc !== "0") {

            const montoDisponible =
                Math.max(saldoNC - totaltef, 0);

            if (montoDisponible <= 0) {

                if ($input && $input.length) {

                    $input
                        .closest('tr')
                        .find('input[type="checkbox"]')
                        .prop("checked", false);

                    $input.prop("disabled", true);
                }

                guardarEstadoPago(rowId, {
                    checked: false
                });

                $("#saldonc").text(
                    formatDollar(0, '')
                );

                return;
            }

            const montoAntesDeAplicarNC = montoAplicado;

            montoAplicado = Math.min(
                montoAplicado,
                montoDisponible
            );

            if (
                $input &&
                $input.length &&
                montoAplicado !== montoAntesDeAplicarNC
            ) {

                $input.val(
                    formatearMiles(
                        montoAplicado.toFixed(2)
                    )
                );
            }

            guardarEstadoPago(rowId, {
                monto: montoAplicado,
                saldo: saldo,
                checked: true
            });
        }

        saldosSeleccionados.push(saldo);
        montoSeleccionados.push(montoAplicado);
        conceptosArray.push(`${tipoDoc} | ${documento} | ${cliente} | Monto: ₡${formatearMiles(montoAplicado.toFixed(2))}`);
        filaDatos.push({
            DEBITO: documento,
            TIPO_DEBITO: tipoDoc,
            FECHA_DEBITO: fecha,
            SUBTIPO_DEBITO: subtipo,
            MONTO_PAGO: montoAplicado
        });
        contDoc++;

        if (nc !== "0") {
            const resto = saldoNC - totaltef - montoAplicado;

            const reg = { id: rowId, monto_aplicado: montoAplicado, tipoDoc, documento, saldo };
            _correos_marcados.push(reg);
            AgregarAAplicar(reg);
            $("#saldonc").text(formatDollar(Math.max(resto, 0), '₡'));
        }

        totaltef += montoAplicado;
        suma += montoAplicado;
    });

    comentarioTesoreriaArray = seleccionados
        .map(x => (x.comentarioTes || '').trim())
        .filter(x => x);
    comentarioTesoreria = comentarioTesoreriaArray.join(', ');
    $("#comentariotesoreria").val(comentarioTesoreria);

    if (checked) {
        $("#concepto").val(construirConceptoRecibo(conceptosArray));
        $("#MontoSelectFac").text(`${contDoc} documentos`);
        $("#montoFacturaSelect").text(formatDollar(suma, '₡'));
        $("#total, #montoexacto").val(formatDollar(totaltef, '₡'));
        $("#totalmodal").val(formatDollar(totaltef, '₡'));
        $("#crearRecibo").removeClass('d-none');
        $("#CrearTeffBanco").addClass('d-none');
        $("#CrearIntereses").addClass('d-none');
        $("#CrearAdelantoBtn").addClass('d-none');

        if (nc !== "0") {
            AplicarNC(nc);
            $("#crearRecibo").addClass('d-none');
        } else {
            $("#crearRecibo").removeClass('d-none');
        }
    } else {
        $("#MontoSelectFac").text(`0 documentos`);
        $("#montoFacturaSelect").text(formatDollar(0, '₡'));
        $("#total, #montoexacto").val(formatDollar(0, '₡'));
        $("#crearRecibo").addClass('d-none');
        $("#comentariotesoreria").val("");
        $(".notnc").removeClass("d-none");
        $(".nc").addClass("d-none");
        $(".col-xl-4").addClass("col-xl-3");
        $(".col-xl-3").removeClass("col-xl-4");
        $("#CrearTeffBanco").removeClass('d-none');
        $("#CrearIntereses").removeClass('d-none');
        $("#CrearAdelantoBtn").removeClass('d-none');
    }
}

function AplicarNC(nc) {
    if (nc !== "0") {

        $("#NotaCredito").val(notaSeleccionada);
        $("#NotaCredito").text(notaSeleccionada);
        $(".notnc").addClass("d-none")
        $(".nc").removeClass("d-none")
        $("#aplicarNota").removeClass("d-none")
        $("#crearRecibo").addClass("d-none")
        $("#textPorVencerUSD").text(nc.split("SALDO")[0])

        $("#textnotascredito").text("Documento con saldo a favor a aplicar " + nc.split("SALDO")[0])
        saldodeNC = parseFloat($("#ncsf").find('option:selected').attr('saldo').replace(/[^\d.-]/g, ''));
        $("#ncselec").text(formatDollar(saldodeNC, '₡'))
        $("#originalmodal").val(formatDollar(saldodeNC, '₡'))
        $("#saldomodal").val(formatDollar(saldodeNC, '₡'))

        $(".col-xl-3").addClass("col-xl-4")
        $(".col-xl-4").removeClass("col-xl-3")
    } else {
        saldodeNC = 0

        $("#textnotascredito").text("Documentos con saldo a favor a aplicar")
        $(".notnc").removeClass("d-none")
        $(".nc").addClass("d-none")
        $("#aplicarNota").addClass("d-none")
        $("#crearRecibo").removeClass("d-none")


        $(".col-xl-4").addClass("col-xl-3")
        $(".col-xl-3").removeClass("col-xl-4")
    }
}

function TipoPago(opt) {

    var selectedOption = $("#formpago :selected");
    var text = selectedOption.text();
    // Utilizando expresión regular para extraer el número entre las barras verticales
    var numeroExtraido = text.match(/\| (\d+) \|/);
    if (numeroExtraido && numeroExtraido.length > 1) {
        var numero = parseInt(numeroExtraido[1], 10);
        subtipo_doc = numero;
    } else {
        subtipo_doc = 0;
    }

    $("#labelfmp").text("Fecha " + opt)
    $("#labelfmp2").text("# Referencia " + opt)
    if (opt == "REC") {
        $("#bancor").addClass("d-none")
        $("#numtefr").addClass("d-none")
        $("#fechaposr").addClass("d-none")
        $("#numeroReciboFisico").removeClass("d-none")
    } else
        if (opt == "TEF" || opt == "DEP") {
            $("#bancor").removeClass("d-none")
            $("#numtefr").removeClass("d-none")
            $("#fechaposr").removeClass("d-none")
            $("#numeroReciboFisico").addClass("d-none")
        }
}
function ModalCrearSeg() {
    seleccionarCreditos()

    if (!checkClienteEnUrl()) return;


    const visita = $('#spanvisita').text();
    if (!visita || visita === "Sin visita") {
        toastr.warning('Debe realizar una visita para registrar el pago', 'Alerta!', toastrOptions);
        return;
    }

    const clienteVal = ($('#clientesFiltro').val() || '').trim();
    if (!clienteVal || clienteVal === '0') {
        toastr.warning('Debe seleccionar un cliente', 'Alerta!', toastrOptions);
        return;
    }
    clienteFiltro = clienteVal;

    const cobradorVal = ($('#cobradorFiltro').val() || '').trim();
    if (!cobradorVal || cobradorVal === '0') {
        toastr.warning('Debe seleccionar un cobrador', 'Alerta!', toastrOptions);
        return;
    }
    cobradorFiltro = cobradorVal;

    ObtenerUltimoConsecutivoCobrador()
        .done(function (data) {
            const consec = data.CONSECUTIVO || "";
            if (!consec) {
                toastr.warning('El cobrador no tiene un consecutivo asignado', 'Alerta!', toastrOptions);
                return;
            }

            CargaCobradorClienteRecibo();
            CargaVendedorClienteRecibo();
            $("#CrearTefModal").modal("show");
        })
        .fail(function () {
            toastr.info('No se pudo obtener el consecutivo del cobrador', 'Error', toastrOptions);
        });
}
function OpcionTipoAbono() {
    var tipoDoc = $("#tipoabono").val();

    if (tipoDoc == "REC") {
        $("#referenciaAbono").text("Número de referencia " + tipoDoc)
        $("#cuentaAdelanto").addClass("d-none")

    } else
        if (tipoDoc == "TEF" || tipoDoc == "DEP") {
            $("#referenciaAbono").text("Número de referencia " + tipoDoc)
            $("#cuentaAdelanto").removeClass("d-none")
        }
}


//function GuardarAsignar() {
//    $("#modalAbono").modal("hide")
//    toastr.success(`Abono creado con éxito`, "Éxito", toastrOptions)
//}

function CuentaOrigenCambio() {
    $('#referencia_cxc').val("")
    $('#concepto_cxc').val("")
    $('#fechadoc_cxc').val("")
    $('#moneda_cxc').val("")
    $('#monto_cxc').val("")

    var _cuenta = $('#cuentaorigen').val().length
    if (_cuenta > 0) {
        $('#referencia_cxc').attr("disabled", false);
        $('#referencia_cxc').attr("placeholder", "Ingrese una referencia bancaria");
    } else {
        $('#referencia_cxc').attr("disabled", true);
        $('#referencia_cxc').attr("placeholder", "Seleccione una cuenta origen");
    }
}

function ValidarSiExisteNumBancoTEF() {
    var formData = new FormData();
    var _numero = $('#referencia_cxc').val()
    var _banco = $('#cuentaorigen').val()

    formData.append('numero', _numero);
    formData.append('banco', _banco);

    const url = `${URLRP}/ValidarSiExisteNumBancoTEF`
    fetch(url, {
        method: 'POST',
        body: formData    //JSON.stringify(fd)
    })
        .then(res => res.json())
        .then(data => {
            if (data.EXISTE == 0) {
                toastr.warning('La referencia bancaria no existe o ya ha sido cargada a bancos', 'Alerta', toastrOptions)
                existeNumBancoTEF = 'N'
            } else {
                toastr.info('Referencia bancaria encontrada con éxito', 'Info', toastrOptions)
                ObtenerDataEstadoCuentaBancario(_banco, _numero)
                existeNumBancoTEF = 'S'
            }
        })
        .catch(err => {
            console.error(err)
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions)
        })
}

function ObtenerDataEstadoCuentaBancario(cuenta, numero) {
    let fd = new FormData()
    fd.set('cuenta_banco', cuenta);
    fd.set('numero', numero);
    const url2 = `${URLRP}/ObtenerDataEstadoCuentaBancario`
    fetch(url2, {
        method: 'POST',
        body: fd
    })
        .then(res => res.json())
        .then(data => {
            if (data != null) {
                $('#concepto_cxc').val(data.PS_86_REFERENCIA)
                $('#fechadoc_cxc').val(data.FECHA_TEF_STR)
                $('#moneda_cxc').val(data.MONEDA)
                $('#monto_cxc').val(data.MONTO)
                $('#idlinea_cxc').val(data.ID_LINEA)
            }
        })
        .catch(err => {
            console.error(err)
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions)
        })
}

async function guardarRec_Teff(accionPosterior) {
    accionPosterior = accionPosterior || 'imprimir';
    const form = document.querySelector('#formAdelanto');
    let fd = new FormData(form);

    var visita_id = document.getElementById('spanvisita').innerText;
    console.log(visita_id);

    if (visita_id === null || visita_id === "" || visita_id === "Sin visita") {
        toastr.warning('Debe realizar una visita para registrar el pago', 'Alerta!', toastrOptions);
        return;
    }


    if (form.elements.tipoabono.value === "0" || form.elements.tipoabono.value === "") {
        toastr.warning('Debe seleccionar un tipo de documento', 'Alerta!', toastrOptions);
        return;
    }

    if (form.elements.tipoabono.value !== "0" && form.elements.subtipoAbono.value === "0") {
        toastr.warning('Debe seleccionar un Subtipo de documento', 'Alerta!', toastrOptions);
        return;
    }

    if (form.elements.cliabono.value === "0" || form.elements.cliabono.value === "") {
        toastr.warning('Debe seleccionar un Cliente', 'Alerta!', toastrOptions);
        return;
    }


    if ((form.elements.tipoabono.value === 'TEF' || form.elements.tipoabono.value === 'DEP') && (form.elements.cuentaabono.value === "" || form.elements.cuentaabono.value === "0")) {
        toastr.warning('Debe seleccionar una cuenta bancaria', 'Alerta!', toastrOptions);
        return;
    }


    if (form.elements.montoabono.value === "" || isNaN(form.elements.montoabono.value) || form.elements.montoabono.value <= 0) {
        toastr.warning('Por favor, ingrese un monto válido (número positivo mayor a cero).', 'Alerta!', toastrOptions);
        return;

    }
    if (vendedorFiltro === '0' || vendedorFiltro === 'ND' || vendedorFiltro === 'ND') {
        toastr.warning('Debe seleccionar un vendedor válido en el filtro', 'Alerta!', toastrOptions);
        return;
    }
    if (cobradorFiltro === '0' || cobradorFiltro === 'ND' || cobradorFiltro === 'ND') {
        toastr.warning('Debe seleccionar un cobrador válido en el filtro', 'Alerta!', toastrOptions);
        return;
    }
    const desbloquear = bloquearBotonesRegistroPagos('#btnGuardarAdelantoImprimir, #btnGuardarAdelantoDescargar');
    if (!desbloquear) return;

    try {
        await guardarReciboTransferenciasSinFactura(accionPosterior);
    } finally {
        desbloquear();
    }
}

async function guardarReciboTransferenciasSinFactura(accionPosterior) {
    accionPosterior = accionPosterior || 'imprimir';
    const form = document.querySelector('#formAdelanto');
    let tipoDocumento = $('#tipoabono').val();
    let referencia = null;
    let cuenta_banco = null;
    if (tipoDocumento === "TEF" || tipoDocumento === "DEP") {
        referencia = $('#ReferenciaNumAbono').val();
        cuenta_banco = $('#cuentaabono').val();
    }
    if (tipoDocumento === "REC") {
        referencia = $('#numrec').val();
        cuenta_banco = "";
    }
    let numero = document.getElementById('spanvisita').innerText;
    let visita_id = numero.split(': ')[1]; // Divide la cadena y toma la segunda parte


    let fd = new FormData(form);
    let valorOriginal = $('#montoabono').val().replaceAll(',', '');
    let cantidadSinColones = valorOriginal.replace('₡', '');
    let totalAbonadoDecimal = parseFloat(cantidadSinColones);
    fd.set('cliente', $('#cliabono').val());
    const documentoComprobante = $('#numreciboAbono').val();
    fd.set('documentoCC', $('#numreciboAbono').val());
    fd.set('tipo_doc', $('#tipoabono').val());
    fd.set('subtipo', $('#subtipoAbono').val());
    fd.set('cuenta_bancaria', cuenta_banco);
    fd.set('fecha', $('#fechaabono').val().split('/').reverse().join('-'));
    fd.set('referencia', referencia);
    fd.set('moneda', "CRC");
    fd.set('total_abonado', parseFloat(totalAbonadoDecimal));
    fd.set('concepto', $('#conceptoabono').val());
    fd.set('id_visita', parseInt(visita_id));
    fd.set('vendedor', vendedorFiltro);
    fd.set('cobrador', cobradorFiltro);
    fd.set('comentariocob', "");
    try {
        const url = `${URLRP}/RegistrarDocumentoSinFactura`
        const request = await fetch(url, {
            method: 'POST',
            body: fd
        })
        const response = await request.json();
        if (response == "1") {
            toastr.success(`Documento generado con éxito!`, "Éxito", toastrOptions)

            $("#modalAbono").modal("hide")
            actualizarDatosDespuesDePago();
            //procesarComprobantePagoGuardado(referencia, accionPosterior);
            procesarComprobantePagoGuardado(documentoComprobante, accionPosterior);

        } else {
            toastr.warning('Hubo un error al hacer el llamado!', 'Error', toastrOptions)
        }

    } catch (e) {
        console.error(e)
    }

}

function advertencia_Monto_Menor() {
    //ESTA PARTE ES DEMO DE MONTO EXACTO MENOR AL MONTO SELECCIONADO
    Swal.fire({
        title: 'Alerta',
        text: 'El monto exacto no puede ser menor al monto seleccionado. Debe corregir el monto a aplicar en las facturas',
        icon: 'warning',
        showCancelButton: false,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: "Cancelar",
        confirmButtonText: 'Aceptar'
    }).then((result) => {
        $("#CrearTefModal").modal("hide")
        if (result.value) {
            //CambiarEstado(usuario, nuevoEstado)
        }
    })
    return;
    //FIN


}

async function guardarDoc(accionPosterior) {
    accionPosterior = accionPosterior || 'imprimir';

    const form = document.querySelector('#formRecibos');
    let fd = new FormData(form);
    var visita_id = document.getElementById('spanvisita').innerText;
    console.log(visita_id);

    // Eliminar el símbolo de moneda y las comas de los miles
    let montoExactoNum = parseFloat(form.elements.montoexacto.value.replace(/[^0-9.-]+/g, ""));
    let totalNum = parseFloat(form.elements.total.value.replace(/[^0-9.-]+/g, ""));

    if (montoExactoNum < totalNum) {
        advertencia_Monto_Menor()
        return;
    }

    if (visita_id === null || visita_id === "" || visita_id === "Sin visita") {
        toastr.warning('Debe realizar una visita para registrar el pago', 'Alerta!', toastrOptions);
        return;
    }

    if (form.elements.formpago.value === 'TEF' && (form.elements.banco.value === '' || form.elements.banco.value === "0")) {
        toastr.warning('Debe seleccionar una cuenta bancaria', 'Alerta!', toastrOptions);
        return;
    }

    if (form.elements.formpago.value === 'DEP' || form.elements.formpago.value === 'TEF') {
        const documentoValue = form.elements.numero_transferencia.value.trim();
        // Verifica si el valor contiene solo dígitos y opcionalmente un punto decimal
        const isValidNumber = /^\d+(\.\d+)?$/.test(documentoValue);
        if (!isValidNumber) {
            toastr.warning('Debe digitar un valor numérico para la referencia.', 'Alerta!', toastrOptions);
            return;
        }
    } else if (form.elements.numero_transferencia.value === "0" || form.elements.numero_transferencia.value === "" && (form.elements.formpago.value === 'DEP' || form.elements.formpago.value === 'TEF')) {
        toastr.warning('Debe digitar un valor para la referencia.', 'Alerta!', toastrOptions);
        return;
    }

    //if (form.elements.formpago.value === 'REC' && form.elements.NumDocFisico.value === '') {
    //    toastr.warning('Digite un número de documento fisico', 'Alerta!', toastrOptions);
    //    return;
    //}

    if (form.elements.montoexacto.value === '0' || form.elements.montoexacto.value === '') {
        toastr.warning('Editar un monto para crear el documento.', 'Alerta!', toastrOptions);
        return;
    }

    //if (form.elements.comentarioscob.value === null || form.elements.comentarioscob.value === '') {
    //    toastr.warning('Debe digitar un comentario.', 'Alerta!', toastrOptions);
    //    return;
    //}

    if (form.elements.formpago.value === '0' || form.elements.formpago.value === '') {
        toastr.warning('Debe seleccionar un tipo de documento.', 'Alerta!', toastrOptions);
        return;
    }


    //const idTemporales = KTUppy.getIdTemporales();
    //if (!idTemporales.length) {
    //    toastr.warning('Debe cargar al menos un documento antes de continuar.', 'Alerta!', toastrOptions);
    //    return;
    //}

    //Se agregan validaciones de vendedor-cobrador-cliente antes de guardar
    if (vendedorFiltro === '0' || vendedorFiltro === 'ND' || vendedorFiltro === 'ND') {
        toastr.warning('Debe seleccionar un vendedor válido en el filtro', 'Alerta!', toastrOptions);
        return;
    }
    if (cobradorFiltro === '0' || cobradorFiltro === 'ND' || cobradorFiltro === 'ND') {
        toastr.warning('Debe seleccionar un cobrador válido en el filtro', 'Alerta!', toastrOptions);
        return;
    }
    if (clienteFiltro === "0" || clienteFiltro === null) {
        toastr.warning('Debe seleccionar un cliente valido del filtro', 'Alerta!', toastrOptions);
        return;
    }
    const desbloquear = bloquearBotonesRegistroPagos('#btnGuardarReciboImprimir, #btnGuardarReciboDescargar');
    if (!desbloquear) return;

    try {
        await guardarRecibo(accionPosterior);
    } finally {
        desbloquear();
    }

}

async function guardarRecibo(accionPosterior) {
    accionPosterior = accionPosterior || 'imprimir';
    const form = document.querySelector('#formRecibos');
    let tipoDocumento = $('#formpago').val();

    let referencia = null;
    let cuenta_banco = null;

    if (tipoDocumento === "TEF" || tipoDocumento === "DEP") {
        referencia = $('#numero_transferencia').val();
        cuenta_banco = $('#banco').val();
    }
    if (tipoDocumento === "REC") {
        referencia = $('#numrecibo').val();
        cuenta_banco = "";
    }

    let numero = document.getElementById('spanvisita').innerText;
    let visita_id = numero.split(': ')[1]; // Divide la cadena y toma la segunda parte
    console.log(visita_id);


    let fd = new FormData(form);
    let valorOriginal = $('#montoexacto').val().replace(/,/g, '').replace('₡', '');
    let totalAbonadoDecimal = parseFloat(valorOriginal) || 0;

    let totalEnFormatoLocal = totalAbonadoDecimal
        .toFixed(2)
        .replace('.', ',');

    console.log(totalAbonadoDecimal);
    fd.set('cliente', clienteFiltro);
    fd.set('documentoCC', $('#numrecibo').val());
    fd.set('tipo_doc', $('#formpago').val());
    fd.set('subtipo', subtipo_doc);
    fd.set('cuenta_bancaria', cuenta_banco);
    fd.set('fecha', $('#fechapago').val().split('/').reverse().join('-'));
    fd.set('referencia', referencia);
    fd.set('moneda', $('#moneda').val());
    fd.set('total_abonado', totalEnFormatoLocal);
    const conceptoSeguro = construirConceptoRecibo(conceptosArray);
    fd.set('concepto', conceptoSeguro);
    $('#concepto').val(conceptoSeguro);
    fd.set('id_visita', visita_id);
    fd.set('vendedor', $('#VendedorRecibo').val())
    fd.set('cobrador', cobradorFiltro)
    fd.set('comentariocob', $('#comentarioscob').val())

    try {
        const url = `${URLRP}/RegistrarDocumento`
        const request = await fetch(url, {
            method: 'POST',
            body: fd
        })
        const response = await request.json();
        if (response == "1") {
            toastr.success(`Documento generado con éxito!`, "Éxito", toastrOptions)

            await GuardarDoccDet(accionPosterior);
            // $("#CrearTefModal").modal("hide")
        } else if (response && response.ok === false) {
            toastr.error(response.mensaje || 'No se pudo generar el documento.', 'Error', toastrOptions);
        } else {
            toastr.warning('Hubo un error al hacer el llamado!', 'Error', toastrOptions)
        }

    } catch (e) {
        console.error(e)
        toastr.error('No se pudo generar el documento. Revise el concepto o intente de nuevo.', 'Error', toastrOptions);
    }

}

async function GuardarDoccDet(accionPosterior) {
    accionPosterior = accionPosterior || 'imprimir';
    const form = document.querySelector('#formRecibos');
    let fd = new FormData(form);
    fd.set('Lineas', JSON.stringify(filaDatos))
    console.log(filaDatos);

    fd.set('doc_credito', $('#numrecibo').val());
    fd.set('tipo_credito', $('#formpago').val());
    fd.set('subtipo_credito', subtipo_doc);

    const url = `${URLRP}/RegistrarDoc_Auxiliar`;
    try {
        const response = await fetch(url, { method: 'POST', body: fd });
        const data = await response.json();
        toastr.success(`Registro de detalle realizado con éxito!`, "Éxito", toastrOptions);
        filaDatos.splice(0, filaDatos.length);
        $("#CrearTefModal").modal("hide");
        actualizarDatosDespuesDePago();
        procesarComprobantePagoGuardado($('#numrecibo').val(), accionPosterior);
    } catch (err) {
        console.error(err);
        toastr.error('Error enviando los reportes', 'Error');
    }
}

// Define qué método utilizar después de guardar el comprobante.
// Android utiliza RawBT para impresión térmica.
// En otros dispositivos se utiliza la visualización PDF existente del sistema.
// function procesarComprobantePagoGuardado(documento, accionPosterior) {

//     if (!documento) {
//         toastr.warning(
//             'Documento generado, pero no se pudo obtener el número para el comprobante.',
//             'Alerta!',
//             toastrOptions
//         );
//         return;
//     }

//     // Guardar y Descargar mantiene el mismo comportamiento
//     // independientemente del dispositivo.
//     if (accionPosterior === 'descargar') {
//         descargarComprobantePagoPdf(documento);
//         return;
//     }

//     // Guardar e Imprimir
//     if (esAndroidRegistroPagos) {

//         // Tablet Android -> RawBT -> impresora térmica
//         reimprimirTiquete(documento);

//     } else {

//         // PC -> método de visualización/impresión existente
//         VisualizarRecibo(documento);
//     }
// }
function procesarComprobantePagoGuardado(documento, accionPosterior) {
    if (!documento) {
        toastr.warning(
            'Documento generado, pero no se pudo obtener el número para el comprobante.',
            'Alerta!',
            toastrOptions
        );
        return;
    }

    if (accionPosterior === 'descargar') {
        descargarComprobantePagoPdf(documento);
        return;
    }

    VisualizarRecibo(documento);
}

function descargarComprobantePagoPdf(documento) {
    mostrarLoadingDescargaComprobante();
    const url = `${URLRP}/DescargarComprobantePagoPdf?documento=${encodeURIComponent(documento)}`;
    window.location.href = url;
    setTimeout(ocultarLoadingDescargaComprobante, 3000);
}

function mostrarLoadingDescargaComprobante() {
    if (typeof KTApp !== 'undefined' && typeof KTApp.block === 'function') {
        KTApp.block('body', {
            overlayColor: '#000000',
            state: 'primary',
            message: 'Generando documento...'
        });
    }
}

function ocultarLoadingDescargaComprobante() {
    if (typeof KTApp !== 'undefined' && typeof KTApp.unblock === 'function') {
        KTApp.unblock('body');
    }
}

//function reimprimirTiquete(documento) {
//    $.getJSON(URLRP + '/ObtenerComprobanteAplicacion?documento=' + documento, (json) => {
//        const encoded = encodeURIComponent(json.tiquete);

//        const rawbtIntent = `intent://${encoded}#Intent;scheme=rawbt;package=ru.a402d.rawbtprinter;end`;

//        window.location.href = rawbtIntent;
//    });
//}

// Se inicializa explícitamente la impresora antes de cada copia.
// Anteriormente el tiquete dependía del estado previo de la impresora
// (fuente, tamaño o alineación), lo que podía provocar que un mismo
// comprobante se imprimiera con diferentes anchos y saltos de línea.
function reimprimirTiquete(documento) {

    $.getJSON(
        URLRP + '/ObtenerComprobanteAplicacion?documento=' + documento,
        (json) => {

            let ticketLimpio = '';

            if (json && json.tiqueteBase64) {
                try {
                    ticketLimpio =
                        decodeURIComponent(
                            escape(window.atob(json.tiqueteBase64))
                        );
                } catch (e) {
                    ticketLimpio = '';
                }
            }

            if (!ticketLimpio) {
                ticketLimpio =
                    sanitizarTextoImpresora(
                        (json && json.tiquete) || ''
                    );
            }

            // Normalizar saltos de línea para RawBT / ESC-POS.
            ticketLimpio =
                ticketLimpio.replace(/\r\n/g, '\n');

            /*
             * ESC @     = Inicializar impresora.
             * ESC M 1   = Fuente B (más angosta).
             * GS ! 0    = Tamaño normal.
             * ESC a 0   = Alineación izquierda.
             */
            const inicializar =
                "\x1B\x40" +
                "\x1B\x4D\x01" +
                "\x1D\x21\x00" +
                "\x1B\x61\x00";

            const corte =
                "\n\n\n\x1D\x56\x00";

            // Se vuelve a inicializar antes de la segunda copia para
            // evitar que el corte o el estado interno de la impresora
            // alteren el formato de la siguiente impresión.
            // const contenidoFinal =
            //     inicializar +
            //     ticketLimpio +
            //     corte +
            //     inicializar +
            //     ticketLimpio +
            //     corte;
            const contenidoFinal =
                inicializar +
                ticketLimpio +
                corte;

            const encoded =
                encodeURIComponent(contenidoFinal);

            const rawbtIntent =
                `intent://${encoded}` +
                `#Intent;scheme=rawbt;` +
                `package=ru.a402d.rawbtprinter;end`;

            window.location.href = rawbtIntent;
        }
    );
}

function sanitizarTextoImpresora(texto) {
    let valor = String(texto || '');

    if (typeof valor.normalize === 'function') {
        valor = valor.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
    }

    valor = valor
        .replace(/[“”]/g, '"')
        .replace(/[‘’]/g, "'")
        .replace(/[–—]/g, '-')
        .replace(/\u00A0/g, ' ');

    return valor.replace(/[^\x09\x0A\x0D\x20-\x7E]/g, '');
}


function ReciboImpreso() {

    let documento_credito = $('#numrecibo').val();
    /*  setTimeout(() => {*/
    const url = `${URLRP}/printReporteComprobante?doc_debito=${documento_credito}`;
    window.open(url, "_blank");
    var urlnew = `${URLRP}`;
    window.location.href = urlnew;
    //KTDatatablesServerSide.reset();
    //location.reload();
    /*}, 1500);*/
}

function obtenerMontoCreditoSeleccionado() {
    const nc = $("#ncsf").val();
    if (!nc || nc === "0") return 0;
    return toNumber($("#ncsf option:selected").attr("saldo"));
}

function obtenerMontoDistribuidoCredito() {
    return filaDatos.reduce((total, item) => total + toNumber(item.MONTO_PAGO), 0);
}

function validarDistribucionCredito() {
    const nc = $("#ncsf").val();
    if (!nc || nc === "0") return true;

    const montoCredito = obtenerMontoCreditoSeleccionado();
    const montoDistribuido = obtenerMontoDistribuidoCredito();

    if (montoCredito <= 0) {
        toastr.warning('Debe seleccionar un crédito con saldo disponible.', 'Advertencia', toastrOptions);
        return false;
    }

    if (montoDistribuido <= 0) {
        toastr.warning('El monto a aplicar debe ser mayor a 0.', 'Advertencia', toastrOptions);
        return false;
    }

    if (montoDistribuido - montoCredito > 0.01) {
        toastr.warning(
            `La distribución (${formatDollar(montoDistribuido, '₡')}) no puede superar el monto del crédito seleccionado (${formatDollar(montoCredito, '₡')}).`,
            'Advertencia',
            toastrOptions
        );
        return false;
    }

    return true;
}


function AbrirModalNotas() {
    seleccionarCreditos()
    if (!checkClienteEnUrl()) return;
    const creditoSeleccionado = $("#ncsf").val();
    const montoCreditoSeleccionado = obtenerMontoCreditoSeleccionado();
    if (!creditoSeleccionado || creditoSeleccionado === "0" || montoCreditoSeleccionado <= 0) {
        toastr.warning('Debe seleccionar un crédito con saldo mayor a cero.', 'Advertencia', toastrOptions);
        return;
    }

    let tieneSaldoCero = false;

    $("#miTabla input[type='checkbox']:checked").each(function () {
        const $cb = $(this);
        const $input = $cb.closest('tr').find('.montopagos');

        if ($input.val() <= 0) {
            tieneSaldoCero = true;

            return false; // rompe el each
        }
    });

    if (tieneSaldoCero) {
        toastr.warning("Existe al menos un registro con saldo en cero.", 'Advertencia!', toastrOptions);
        return;
    }

    const clienteActual = ($('#clientesFiltro').val() || '0').trim();
    const montoAplicar = totaltef || 0;
    if (clienteActual === '0') {
        console.log(clienteActual + "F");
        toastr.warning("Por favor seleccione un cliente para procesar los documentos", 'Advertencia!', toastrOptions);
        return;
    }
    if (!cobradorFiltro || cobradorFiltro === '0') {
        toastr.warning('Por favor seleccione un cobrador antes de continuar', 'Advertencia!', toastrOptions);
        return;
    }
    if (montoAplicar <= 0) {
        toastr.warning('El monto a aplicar debe ser mayor a 0.', 'Advertencia', toastrOptions);
        return;
    }
    if (!validarDistribucionCredito()) return;

    CargaCobradorClienteCreditos();
    $("#totalmodal").val(formatDollar(totaltef, '₡'));
    $("#saldomodal").val(formatDollar(Math.max(obtenerMontoCreditoSeleccionado() - totaltef, 0), '₡'));

    $("#AplicarNota").modal("show");
}


function abrirmodalCorreosBatch() {
    $("#modalenviocorreoBatch").modal("show")
    $("#correosBatch").val(correos_seleccionados)
    var correosTemp = []
    _correos_marcados = []
    $("#miTabla input:checkbox:checked").each(function () {
        correosTemp.push($(this).attr('correo'))
        _correos_marcados.push({
            cliente: $(this).val(),
            correo: $(this).attr('correo')
        })
    });
    correos_seleccionados = correosTemp.join(';');
}

function AgregarAAplicar(obj) {
    console.log(obj);
    let nc = $("#ncsf").val().split("SALDO")[0]
    var selectedOption = $("#ncsf option:selected");
    tipoCredito = selectedOption.attr("tipo_credito"); // asumiendo que agregaste data-tipo al elemento option
    let textadd = `<tr>
                    <td>${tipoCredito}</td>
                    <td>${nc}</td>
                    <td>${obj.tipoDoc}</td>
                    <td>${obj.documento}</td>
                    <td class="text-right">${formatDollar(obj.monto_aplicado, '₡')}
                    </td>
                    <td class="text-right">${formatDollar(obj.saldo, '₡')}</td>
                    <td></td>
                </tr>`

    $("#notasaplicar").append(textadd)

}
async function guardarApl() {
    seleccionarCreditos();

    const numeroNota = $('#NotaCredito').val().trim();
    if (!numeroNota) {
        toastr.warning('Debe seleccionar una nota de crédito antes de aplicar.', 'Advertencia', toastrOptions);
        return;
    }
    if (!validarDistribucionCredito()) return;

    const fd = new FormData();
    fd.set('Lineas', JSON.stringify(filaDatos));
    fd.set('doc_credito', numeroNota);
    fd.set('tipo_credito', tipoCredito);
    fd.set('subtipo_credito', 0);

    const desbloquear = bloquearBotonesRegistroPagos('#btnConfirmarAplicacion');
    if (!desbloquear) return;

    try {
        const response = await fetch(`${URLRP}/RegistrarDoc_Auxiliar`, { method: 'POST', body: fd });
        const data = await response.json();
        if (data === 1 || data === '1') {
            toastr.success('Crédito aplicado con éxito!', 'Éxito', toastrOptions);
            actualizarDatosDespuesDePago();
            filaDatos = [];
            $('#AplicarNota').modal('hide');

            if (esAndroidRegistroPagos) {
                reimprimirTiquete(numeroNota);
            } else {
                VisualizarReciboAplicado(numeroNota);
            }
        } else {
            toastr.error('Ocurrió un error al aplicar la nota.', 'Error', toastrOptions);
        }
    } catch (err) {
        console.error(err);
        toastr.error('No se pudo conectar con el servidor.', 'Error', toastrOptions);
    } finally {
        desbloquear();
    }
}

// function actualizarDatosDespuesDePago() {
//     pagosSeleccionMap.clear();
//     SaldoTotal();

//     if (typeof KTDatatablesServerSide?.reload === 'function') {
//         KTDatatablesServerSide.reload(); //Ese reload es asíncrono. El JavaScript no espera a que SQL responda.
//     } else if ($.fn.DataTable && $.fn.DataTable.isDataTable('#miTabla')) {
//         $('#miTabla').DataTable().ajax.reload(null, false);
//     }

//     $('#miTabla input[type="checkbox"]').prop('checked', false);
//     ManejarCantidadCheckboxes();
//     $('#ncsf').val('0').trigger('change');
// }

// Después de registrar un pago se elimina completamente el estado de la
// selección anterior antes de solicitar los saldos actualizados.
// Anteriormente se ejecutaba ManejarCantidadCheckboxes() mientras el reload
// AJAX todavía no había terminado, por lo que se volvían a guardar en memoria
// los montos del recibo anterior. Además, el trigger('change') de #ncsf podía
// volver a ejecutar esa sincronización. Ahora se limpia el estado sin volver
// a leer la tabla vieja y posteriormente se recargan los datos desde el servidor.
function actualizarDatosDespuesDePago() {

    // El pago anterior ya terminó; ningún documento debe conservar
    // su selección o monto en memoria.
    pagosSeleccionMap.clear();

    filaDatos.length = 0;
    saldosSeleccionados.length = 0;
    montoSeleccionados.length = 0;
    conceptosArray.length = 0;

    // Limpiar visualmente las filas que todavía están en pantalla.
    // Estas filas pueden contener temporalmente los saldos anteriores
    // hasta que finalice la recarga AJAX.
    $('#miTabla input[type="checkbox"]').prop('checked', false);
    $('#miTabla .montopagos').prop('disabled', true);

    // Restablecer la nota de crédito sin disparar el evento change.
    // El evento change ejecuta ManejarCantidadCheckboxes() cuando el valor
    // es 0 y podría volver a guardar los valores viejos de la tabla.
    $('#ncsf').val('0');
    AplicarNC('0');

    // Recalcular la interfaz con el mapa ya vacío.
    seleccionarCreditos();

    SaldoTotal();

    // Finalmente solicitar nuevamente los saldos reales al servidor.
    if (typeof KTDatatablesServerSide?.reload === 'function') {
        KTDatatablesServerSide.reload();
    } else if (
        $.fn.DataTable &&
        $.fn.DataTable.isDataTable('#miTabla')
    ) {
        $('#miTabla').DataTable().ajax.reload(null, false);
    }
}

function VisualizarReciboAplicado(docDebito) {
    $.ajax({
        url: `${URLRP}/CargarReporteAplicacionRpt`,
        type: 'POST',
        dataType: 'json',
        data: { doc_debito: docDebito },
        success: function (res) {
            if (res.ConError === 'N') {
                PDFObject.embed(
                    "data:application/pdf;base64," + res.Datos,
                    "#pdf_previewApli"
                );
                $('#modalverarchivoAplicacion').modal('show');
            } else {
                toastr.error(res.Errrores || 'Error generando el PDF.', '¡Error!');
            }
        },
        error: function () {
            toastr.error('No se pudo obtener el PDF de aplicación.', '¡Error!');
        }
    });
}

const getVal = sel => {
    const $el = $(sel);
    return ($el.length ? ($el.val() ?? '') : '').toString().trim();
};

async function guardarApl_ConCorreo() {
    seleccionarCreditos();

    const numeroNota = getVal('#NotaCredito');
    const cliente = getVal('#clientesFiltro');
    const correoCP = getVal('#CorreoCuentasPagar');
    const correoCobrador = getVal('#correocobrador');
    const correosDest = [correoCP, correoCobrador].filter(Boolean).join(';');



    if (!numeroNota) {
        toastr.warning('Debe seleccionar una nota de crédito antes de aplicar.', 'Advertencia', toastrOptions);
        return;
    }
    if (!validarDistribucionCredito()) return;

    if (!correosDest) {
        toastr.warning('No se encontró correo de destino.', 'Advertencia', toastrOptions);
        return;
    }

    try {

        const fd = new FormData();
        fd.set('Lineas', JSON.stringify(filaDatos));
        fd.set('doc_credito', numeroNota);
        fd.set('tipo_credito', tipoCredito);
        fd.set('subtipo_credito', 0);

        const regRes = await fetch(`${URLRP}/RegistrarDoc_Auxiliar`, { method: 'POST', body: fd });
        const regData = await regRes.json();

        if (regData !== 1 && regData !== '1') throw new Error('Error al registrar doc auxiliar');

        //const pdfBody = `doc_debito=${encodeURIComponent(numeroNota)}`;
        //const pdfRes = await fetch(`${URLRP}/GenerarReporteAplicacionPdf`, {
        //    method: 'POST',
        //    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        //    body: pdfBody
        //});
        //const pdfData = await pdfRes.json();

        //if (pdfData.ConError === 'S') {
        //    toastr.error('Aplicación guardada, pero ocurrió un problema al generar el PDF.',
        //        'Advertencia', toastrOptions);

        //    console.error('[PDF-ERROR]', pdfData.Errores);
        //}

        if (esAndroidRegistroPagos) {
            reimprimirTiquete(numeroNota);
        }

        const mailBody = `doc_aplicacion=${encodeURIComponent(numeroNota)}`
            + `&cliente=${encodeURIComponent(cliente)}`
            + `&correos=${encodeURIComponent(correosDest)}`;

        const mailRes = await fetch(`${URLRP}/EnviarAplicacionNC`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: mailBody
        });
        const mailData = await mailRes.json();

        if (mailData !== 1 && mailData !== '1') {
            toastr.error('PDF generado, pero el correo no pudo enviarse.', 'Advertencia', toastrOptions);
        } else {
            toastr.success('Crédito aplicado, PDF generado y correo enviado.', 'Éxito', toastrOptions);
        }

        actualizarDatosDespuesDePago();
        filaDatos = [];
        $('#AplicarNota').modal('hide');

    } catch (err) {
        console.error('[ERROR]', err);
        toastr.error(err.message || 'Error inesperado', 'Error', toastrOptions);
    }
}

window.guardarApl_ConCorreo = guardarApl_ConCorreo;

// function VisualizarRecibo(documento_credito) {

//     // Mantiene compatibilidad con llamadas antiguas que no envían el documento.
//     documento_credito = documento_credito || $('#numrecibo').val();

//     $.ajax({
//         url: `${URLRP}/CargarReporteRdl`,
//         data: {
//             doc_debito: documento_credito
//         },
//         async: true,
//         type: "POST",
//         success: function (data) {
//             if (typeof (data) === "object") {
//                 if (data.ConError == "N") {
//                     PDFObject.embed(
//                         "data:application/pdf;base64," + data.Datos,
//                         "#pdf_preview"
//                     );

//                     $("#pdf_preview").removeClass("FHIDE");
//                     $("#modalverarchivo").modal("show");
//                 }
//             }
//         },
//         error: function (jqXHR, exception) {
//             var error = "";

//             if (jqXHR.status === 0) {
//                 error = 'No conectado.\nVerifique su conexión de red.';
//             } else if (jqXHR.status == 404) {
//                 error = 'La página solicitada no fue encontrada. [404]';
//             } else if (jqXHR.status == 500) {
//                 error = 'Error interno del servidor [500].';
//             } else if (exception === 'parsererror') {
//                 error = 'Fallo la conversión a JSON.';
//             } else if (exception === 'timeout') {
//                 error = 'Error de tiempo de espera.';
//             } else if (exception === 'abort') {
//                 error = 'Solicitud Ajax abortada.';
//             } else {
//                 error = 'Error no detectado.\n' + jqXHR.responseText;
//             }

//             toastr.error(error, "¡Error!");
//         }
//     });
// }

function VisualizarRecibo(documento_credito) {
    documento_credito = documento_credito || $('#numrecibo').val();

    if (!documento_credito) {
        toastr.warning('No se pudo obtener el número del recibo.', 'Aviso');
        return;
    }

    comprobantePdfActual = documento_credito;

    $.ajax({
        url: `${URLRP}/CargarComprobantePagoTicketPdf`,
        data: {
            documento: documento_credito
        },
        async: true,
        type: 'POST',
        success: function (data) {
            if (data && data.ConError === 'N' && data.Datos) {
                PDFObject.embed(
                    'data:application/pdf;base64,' + data.Datos,
                    '#pdf_preview'
                );

                $('#pdf_preview').removeClass('FHIDE');
                $('#modalverarchivo').modal('show');
            } else {
                toastr.error(
                    (data && data.Errrores) ||
                    'No se pudo generar la vista previa del recibo.',
                    'Error'
                );
            }
        },
        error: function (jqXHR, exception) {
            var error = '';

            if (jqXHR.status === 0) {
                error = 'No conectado.\nVerifique su conexión de red.';
            } else if (jqXHR.status == 404) {
                error = 'La página solicitada no fue encontrada. [404]';
            } else if (jqXHR.status == 500) {
                error = 'Error interno del servidor [500].';
            } else if (exception === 'parsererror') {
                error = 'Falló la conversión a JSON.';
            } else if (exception === 'timeout') {
                error = 'Error de tiempo de espera.';
            } else if (exception === 'abort') {
                error = 'Solicitud Ajax abortada.';
            } else {
                error = 'Error no detectado.\n' + jqXHR.responseText;
            }

            toastr.error(error, '¡Error!');
        }
    });
}

function AbrirComprobanteParaImprimir() {
    if (!comprobantePdfActual) {
        toastr.warning('No hay un recibo cargado para imprimir.', 'Aviso');
        return;
    }

    const url =
        `${URLRP}/VerComprobantePagoPdf?documento=` +
        encodeURIComponent(comprobantePdfActual);

    const ventana = window.open(url, '_blank');

    if (!ventana) {
        window.location.href = url;
    }
}

async function cargarCorreosClientePorCobrador() {

    const codCobrador = ($('#Creditocobrador').val() || '')
        .split('–')[0]
        .split('-')[0]
        .trim();

    if (!codCobrador) {
        $('#correocliente').val('');
        return;
    }

    try {
        const res = await fetch(`${URLRP}/ObtenerCorreoClientesPorCobrador`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `cobrador=${encodeURIComponent(codCobrador)}`
        });

        const data = await res.json();
        const listaCorreos = (Array.isArray(data) ? data : [])
            .map(r => r.E_MAIL)
            .filter(m => m)
            .join(';');

        $('#correocliente').val(listaCorreos);
    } catch (err) {
        console.error(err);
        toastr.error('No se pudo obtener el correo del cliente',
            'Error', toastrOptions);
    }
}

$('#AplicarNota').on('shown.bs.modal', cargarCorreosClientePorCobrador);
function guardar_EnviarCorreo() {
    $("#modalenviocorreo").modal("show")
}
async function cargarCorreoCobrador() {

    const rawValue = $('#Creditocobrador').val().trim();
    const codCobrador = rawValue.includes(' - ')
        ? rawValue.split(' - ')[0].trim()
        : rawValue;

    if (!codCobrador) { $('#correocobrador').val(''); return; }

    const fd = new FormData();
    fd.set('cobrador', getCodCobrador());
    const res = await fetch(`${URLRP}/ObtenerCorreoCobrador`, {
        method: 'POST',
        body: fd
    });
    const data = await res.json();
    $('#correocobrador').val(data?.E_MAIL ?? '');
}

$('#AplicarNota').on('shown.bs.modal', cargarCorreoCobrador);

function getCodCobrador() {
    const raw = $('#Creditocobrador').val().trim();


    const m = raw.match(/^\d+/);
    return m ? m[0] : raw;
}

function ReciboImpresoAplicacion() {

    let documento_credito = $('#NotaCredito').val();
    /*  setTimeout(() => {*/
    const url = `${URLRP}/printReporteComprobanteAplicacion?doc_debito=${documento_credito}`;
    window.open(url, "_blank");
    var urlnew = `${URLRP}`;
    window.location.href = urlnew;
    //KTDatatablesServerSide.reset();
    // location.reload();
    /*}, 1500);*/
}

//function formatDollar(num, symbol) {
//    var isNegative = false;
//    if (num < 0) {
//        isNegative = true;
//        num = Math.abs(num);
//    }

//    var p = num.toString().split(".");
//    var formattedNum = symbol + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
//        return num === "-" ? acc : num + (i && !(i % 3) ? "," : "") + acc;
//    }, "") + (p[1] === undefined || p[1] === null ? "" : "." + p[1]);

//    return isNegative ? "-" + formattedNum : formattedNum;
//}

function formatDollar(value, symbol) {
    if (value === null || value === undefined || isNaN(value)) {
        return symbol + "0.00";
    }

    var isNegative = value < 0;
    value = Math.abs(value);

    var formatted = new Intl.NumberFormat("en-US", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }).format(value);

    return (isNegative ? "-" : "") + symbol + formatted;
}

function obtenerRangoFechas() {

    // Obtener el elemento input por su ID
    let fechaInput = document.getElementById("FechaInicioV");
    // Obtener el valor del atributo value
    let valor = fechaInput.value;
    // Dividir el valor para obtener las fechas de inicio y fin
    let fechas = valor.split(" - ");
    // Convertir las cadenas de fecha a objetos de fecha de JavaScript
    fechaInicioFiltro = fechas[0].split('/').reverse().join('-');
    fechaFinFiltro = fechas[1].split('/').reverse().join('-');

}

function InicializarSelect() {

    $("#FechaInicioV").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });


    new tempusDominus.TempusDominus(document.getElementById("fechaabono"), {
        localization: {
            locale: 'es',
            format: "DD/MM/YYYY" // Formato ajustado a Día/Mes/Año
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


    $('#formpago').select2({
        placeholder: "Seleccione un tipo de documento"
    })

    $('#banco').select2({
        placeholder: "Seleccione un banco"
    })

    $("#formpago").val("REC").trigger("change");

}

function ManaejarBotones() {
    $('#vendedorFiltro').on('change', function () {
        vendedorFiltro = $(this).val();
        // dispara el evento para que tu listener recargue la tabla
        $(document).trigger('filtros-actualizados', {
            vendedor: vendedorFiltro,
            cobrador: cobradorFiltro,
            cliente: clienteFiltro,
        });
    });

    $('#cobradorFiltro').on('change', function () {
        cobradorFiltro = $(this).val();

        ObtenerUltimoConsecutivoCobrador();

        $(document).trigger('filtros-actualizados', {
            vendedor: vendedorFiltro,
            cobrador: cobradorFiltro,
            cliente: clienteFiltro,
        });
    });


    $('#FechaInicioV').on('change', function () {
        let valor = this.value;
        let fechas = valor.split(" - ");

        fechaInicioFiltro = fechas[0].split('/').reverse().join('-');
        fechaFinFiltro = fechas[1].split('/').reverse().join('-');
        SaldoTotal();
        CargaCreditosCliente();
        KTDatatablesServerSide.reload();
    });

    $('#tipodoc').on('change', function () {
        tipoDocFiltro = this.value;
        KTDatatablesServerSide.reload();
        SaldoTotal();
    });

    $('#clientesFiltro').on('change', function () {
        clienteFiltro = this.value.trim();
        $('#ncselec').text('₡0.00');
        CargaCreditosCliente();

        $(document).trigger('filtros-actualizados', {
            vendedor: vendedorFiltro,
            cobrador: cobradorFiltro,
            cliente: clienteFiltro,
        });
        let nc = $("#ncsf").val()
        SaldoTotal();
        Correo_Cuentas_Pagar_Cliente();
        if (nc != "0") {
            CargaCobradorClienteRecibo();

        } else {
            CargaCobradorClienteCreditos();
        }
        KTDatatablesServerSide.reload();
        CargaClienteAbono();
    });

    $('#ncsf').on('change', function () {
        let valorCreditos = this.value;
        let numeroDevolucion = valorCreditos.split(" ")[0];
        notaSeleccionada = numeroDevolucion
        const container = document.querySelector('#miTabla');
        const allCheckboxes = container.querySelectorAll('[type="checkbox"]');
        let hayCheckMarcados = false;

        allCheckboxes.forEach(checkbox => {
            if (checkbox.checked) {
                hayCheckMarcados = true;
            }
        });

        if (hayCheckMarcados) {
            seleccionarCreditos(0);
        } else {
            if (valorCreditos != "0") {
                toastr.warning("Debe seleccionar documentos a los cuales aplicar las notas", 'Advertencia', toastrOptions)
            } else {
                ManejarCantidadCheckboxes();
            }
        }

    });
    vendedorFiltro = _vendedorFiltro
    cobradorFiltro = _cobradorFiltro
    clienteFiltro = _clienteFiltro
}

function formatearMiles(valor) {
    return new Intl.NumberFormat("en-US", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }).format(valor);
}

function SaldoTotal() {
    $.ajax({
        url: `${URLRP}/ObtenerTotal_Saldo`,
        data: { tipoDoc: tipoDocFiltro, vendedor: vendedorFiltro, cobrador: cobradorFiltro, cliente: clienteFiltro },
        async: true,
        type: "POST",
        dataType: "json",
        success: function (data) {
            // Función para formatear el saldo y el número de documentos
            const formatData = (saldo, numDoc) => ({
                saldo: saldo != null ? formatDollar(saldo, '₡') : formatDollar(0.00, '₡'),
                numDoc: `${numDoc} documentos`,
            });

            // Saldo total
            const saldoTotalData = formatData(data.SALDO, data.CONTADOR);
            $("#SaldoTotalSum").text(saldoTotalData.saldo);
            $("#SaldoTotalCountDoc").text(saldoTotalData.numDoc);

            // Saldo vencido
            const saldoVencidoData = formatData(data.SALDO_VENCIDO, data.CONTADOR_DOC_VENCIDO);
            $("#SaldoVencidoDoc").text(saldoVencidoData.saldo);
            $("#SaldoVencidoCountDoc").text(saldoVencidoData.numDoc);
        }
    });
}

function CargaCreditosCliente() {
    fechaInicioFiltro



    if (vendedorFiltro) {
        $.ajax({
            url: `${URLRP}/obtenerCreditosCliente`,
            data: { vendedor: vendedorFiltro, cobrador: cobradorFiltro, cliente: clienteFiltro },
            type: "POST",
            success: function (data) {
                const $selectCreditos = $("select[name='notasfiltro']");
                let creditoscliente = "";
                if (data && data.length > 0) {
                    creditoscliente += "<option value='0'>Seleccione un credito</option>";
                    data.forEach((prov) => {
                        const saldo_number = formatearMiles(prov.SALDO);
                        const saldo_money = formatDollar(prov.SALDO, "₡");
                        const monto_number = formatearMiles(prov.MONTO);
                        const monto_money = formatDollar(prov.MONTO, "₡");
                        //creditoscliente += `<option value='${prov.DOCUMENTO}'>${prov.TIPO} | ${prov.DOCUMENTO} | ${prov.APLICACION}| SALDO: ${prov.SALDO.toLocaleString()} | MONTO: ${prov.MONTO.toLocaleString()} | FAC: ${prov.FACTURA} |BOLETA: ${prov.BOLETA} | COMENTARIO: ${prov.COMENTARIO}</option>`;
                        creditoscliente += `<option value="${prov.DOCUMENTO} SALDO:${saldo_money}" saldo="${saldo_number}" MONTO:${monto_money}" monto="${monto_number}" tipo_credito="${prov.TIPO}">${prov.TIPO} ${prov.DOCUMENTO} | SALDO: ${saldo_money} | MONTO: ${monto_money} | ${prov.APLICACION} | BOLETA: ${prov.BOLETA}-${prov.COMENTARIO} | CLIENTE: ${prov.CLIENTE} | COBRADOR: ${prov.COBRADOR} </option>`;
                    });
                } else {
                    creditoscliente += "<option value='0'>No aplicar</option>";
                }
                $selectCreditos.html(creditoscliente);
                $selectCreditos.trigger("change");
            },
            error: function () {
                setTimeout(function () {
                    toastr.warning("Debe seleccionar un cliente.");
                }, 3000);
            }
        });
    }
}

function CargaCobradorClienteRecibo() {
    let cliente = clienteFiltro;
    if (cliente === "") {
        cliente = 0;
    }
    if (cliente) {
        $.ajax({
            url: `${URLRP}/obtenerCobradorCliente`,
            data: { cliente: cliente },
            type: "POST",
            success: function (data) {
                const $selectCobrador = $("select[name='cobradorRecibo']");
                let cobradorcliente = "";
                if (data && data.length > 0) {
                    data.forEach((prov) => {
                        cobradorcliente += `<option selected value='${prov.COBRADOR}'>${prov.COBRADOR} - ${prov.NOMBRE}</option>`;
                    });
                } else {
                    cobradorcliente += "<option value='0'>Sin Cobrador</option>";
                }
                $selectCobrador.html(cobradorcliente);
                //$selectCobrador.trigger("change");
            },
            error: function () {
                setTimeout(function () {
                    toastr.warning("Debe seleccionar un cliente.");
                }, 3000);
            }
        });
    }
}
function CargaCobradorClienteCreditos() {
    // Leer cliente directamente desde el filtro
    const cliente = $('#clientesFiltro').val();
    if (!cliente || cliente === '0') {
        // Si no hay cliente, limpio el campo y salgo
        $('#Creditocobrador').val('');
        return;
    }

    // Llamada al controller, que ya monta @CIA desde sesión
    $.ajax({
        url: `${URLRP}/obtenerCobradorCliente`,
        method: "POST",
        dataType: "json",
        data: { cliente },
        success: function (data) {
            if (data && data.length > 0) {
                // Tomo el primer registro (suponiendo un solo cobrador por cliente)
                const { COBRADOR, NOMBRE } = data[0];
                $('#Creditocobrador').val(`${COBRADOR} – ${NOMBRE}`);
            } else {
                // Si no trajo datos, limpio el campo
                $('#Creditocobrador').val('');
            }
        },
        error: function () {
            // En caso de fallo, limpio el campo
            $('#Creditocobrador').val('');
        }
    });
}



function CargaVendedorClienteRecibo() {
    let cliente = clienteFiltro;
    if (cliente === "") {
        cliente = 0;
    }
    if (cliente) {
        $.ajax({
            url: `${URLRP}/obtenerVendedorCliente`,
            data: { cliente: cliente },
            type: "POST",
            success: function (data) {
                const $selectVendedor = $("select[name='VendedorRecibo']");
                let vendedorcliente = "";
                if (data && data.length > 0) {
                    data.forEach((prov) => {
                        vendedorcliente += `<option selected value='${prov.COD_VENDEDOR}'>${prov.COD_VENDEDOR} - ${prov.NOM_VENDEDOR}</option>`;
                    });
                } else {
                    vendedorcliente += "<option value='0'>Sin Vendedor</option>";
                }
                $selectVendedor.html(vendedorcliente);
                //$selectCobrador.trigger("change");
            },
            error: function () {
                setTimeout(function () {
                    toastr.warning("Debe seleccionar un cliente.");
                }, 3000);
            }
        });
    }
}

function CargaTipo_Doc() {
    $.ajax({
        url: `${URLRP}/obtenerTipoDoc`,
        //data: { cliente: cliente },
        type: "POST",
        success: function (data) {
            const $selectTipoDoc = $("select[name='formpago']");
            let TipoDoc = "";
            if (data && data.length > 0) {
                data.forEach((prov) => {
                    TipoDoc += `<option value='${prov.TIPO}'>${prov.TIPO} | ${prov.SUBTIPO} | ${prov.DESCRIPCION}</option>`;
                });
            } else {
                TipoDoc += "<option value='0'>Sin Tipo Doc</option>";
            }
            $selectTipoDoc.html(TipoDoc);
            $selectTipoDoc.trigger("change");
        },
        error: function () {
            setTimeout(function () {
                toastr.warning("Debe seleccionar un tipo.");
            }, 3000);
        }
    });
}

function CargaBancos() {
    $.ajax({
        url: `${URLRP}/MostrarBanco`,
        //data: { cliente: cliente },
        type: "POST",
        success: function (data) {
            const $selectBanco = $("select[name='banco']");
            let TipoBanco = "";
            if (data && data.length > 0) {
                data.forEach((prov) => {
                    TipoBanco += `<option selected value='${prov.CUENTA_BANCO}'>${prov.BANCO} - ${prov.NOMBRE}</option>`;
                });
            } else {
                TipoBanco += "<option value='0'>Sin Banco</option>";
            }
            $selectBanco.html(TipoBanco);
            $selectBanco.trigger("change");
        },
        error: function () {
            setTimeout(function () {
                toastr.warning("Debe seleccionar un banco.");
            }, 3000);
        }
    });
}


function abrirModalPDF(DOCUMENTO) {
    $('#pdf_preview').empty();
    Dajax(`${URLRP}/ObtenerPDF`,
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

function Correo_Cuentas_Pagar_Cliente() {
    $.ajax({
        url: `${URLRP}/ObtenerCorreo_Cuentas_Pagar`,
        data: { cliente: clienteFiltro },
        async: true,
        type: "POST",
        dataType: "json",
        success: function (data) {
            //$("#correo_cuenta_pagar").text(data.CORREO);
            $("#correo_cuenta_pagar").val(data.CORREO);
        }
    });
}




function irRuta() {
    let accion = "actualizar";
    CambiarIdVisita(null);
    // Cambiar el cliente en la sesión
    CambiarClienteSesion("0");
    // Formatear la fecha a YYYY-MM-DD
    // Construir la URL utilizando template literals
    const url = `${URLRTA}?accion=${accion}`;
    window.location.href = url;
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

function AgregarReferencia() {
    var visita_id = document.getElementById('spanvisita').innerText;
    if (visita_id === null || visita_id === "" || visita_id === "Sin visita") {
        toastr.warning('Debe realizar una visita para registrar el pago', 'Alerta!', toastrOptions);
        return; InsertarDocumentoCC
    }

    if (clienteFiltro === "0" || clienteFiltro === null) {
        toastr.warning('Debe seleccionar un cliente', 'Alerta!', toastrOptions);
        return;
    }

    if ($('#clientesFiltro').val() != 0) {
        $('#cliente_cxc').val($('#clientesFiltro').val()).trigger('change')
    }

    $("#agregarRef").modal("show")
}

function CargarRef(value) {
    if (value == "12345") {
        $("#concepto_cxc").val("Pago xyz")
        //$("#cuentabancaria_cxc").val("CR43011400007355535690")
        $("#fechadoc_cxc").val("31/07/2024")
        $("#monto_cxc").val(992051, 86)

        $("#moneda_cxc").val("CRC")
    }
}

var KTDatatablesInteres = function () {
    // Shared variables
    var table;
    var dt;

    var asegurarEncabezadoInteres = function () {
        const $thead = $('#tablainteres thead');
        const totalColumnas = $thead.find('tr:first th').length;
        if (totalColumnas === 7) return;

        $thead.html(`
            <tr class="text-start text-gray-600 fw-bold fs-7 text-uppercase gs-0">
                <th class="text-left min-w-100px">Documento</th>
                <th class="text-left min-w-120px">Fecha Vence</th>
                <th class="text-left min-w-120px">Fecha ult. pago</th>
                <th class="text-left min-w-80px">DV</th>
                <th class="text-left min-w-100px">Monto</th>
                <th class="text-left min-w-100px">Saldo</th>
                <th class="text-left min-w-100px">Intereses</th>
            </tr>
        `);
    };

    // Private functions
    var initDatatable = function () {
        asegurarEncabezadoInteres();
        dt = $("#tablainteres").DataTable({
            searchDelay: 500,
            processing: true,
            scrollX: true,
            data: docsInteres,
            order: [],
            "bDestroy": true,
            destroy: true,

            columns: [
                { "data": "DOCUMENTO" },
                { "data": "FECHA_VENCE" },
                { "data": "FECHA_ULT_MORA" },
                { "data": "DIAS_VENCIDO" },
                { "data": "MONTO" },
                { "data": "SALDO" },
                { "data": "INTERESES" },
            ],
            columnDefs: [
                {
                    targets: 1,
                    orderable: false,
                    render: function (data, type, row) {
                        return formatearFechaInteres(row.FECHA_VENCE);
                    }
                },
                {
                    targets: 2,
                    orderable: false,
                    render: function (data, type, row) {
                        return formatearFechaInteres(row.FECHA_ULT_MORA);
                    }
                },
                {
                    targets: [4, 5, 6],
                    className: 'text-end',
                    orderable: false,
                    render: function (data) {
                        return formatDollar(data || 0, '₡');
                    }
                },
            ],
        });
        table = dt.$;
    }

    var resetDatatable = function () {
        if (dt) {
            dt.clear();
            dt.rows.add(docsInteres);
            dt.draw();
        } else {
            initDatatable();
        }
    }

    // Public methods
    return {
        init: function () {
            initDatatable();
        },
        reset: function () {
            resetDatatable();
        }
    }
}();

function AbrirGenerarInteres() {
    if (!checkClienteEnUrl()) return;

    prepararFechaInteresMora();
    docsInteres = [];
    interesesCargados = false;
    actualizarTotalIntereses(0);
    $("#intereses").modal("show");
    setTimeout(function () { KTDatatablesInteres.reset() }, 200);
}

function prepararFechaInteresMora() {
    const hoy = obtenerFechaActualInput();
    $('#fechaInteresMora').attr('min', hoy);
    if (!$('#fechaInteresMora').val() || $('#fechaInteresMora').val() < hoy) {
        $('#fechaInteresMora').val(hoy);
    }
}

function obtenerFechaActualInput() {
    const fecha = new Date();
    const mes = String(fecha.getMonth() + 1).padStart(2, '0');
    const dia = String(fecha.getDate()).padStart(2, '0');
    return `${fecha.getFullYear()}-${mes}-${dia}`;
}

function formatearFechaInteres(value) {
    if (!value) return '';

    const match = /\/Date\((\d+)\)\//.exec(String(value));
    const fecha = match ? new Date(parseInt(match[1], 10)) : new Date(value);
    if (Number.isNaN(fecha.getTime())) return '';

    const dia = String(fecha.getDate()).padStart(2, '0');
    const mes = String(fecha.getMonth() + 1).padStart(2, '0');
    return `${dia}/${mes}/${fecha.getFullYear()}`;
}

function actualizarTotalIntereses(total) {
    $('#totalInteresesGenerar').text(formatDollar(total || 0, '₡'));
}

function mostrarLoadingIntereses() {
    if (typeof KTApp !== 'undefined' && typeof KTApp.block === 'function') {
        KTApp.block('#intereses .modal-content', {
            overlayColor: '#000000',
            state: 'primary',
            message: 'Generando intereses...'
        });
    }
}

function ocultarLoadingIntereses() {
    if (typeof KTApp !== 'undefined' && typeof KTApp.unblock === 'function') {
        KTApp.unblock('#intereses .modal-content');
    }
}

async function cargarInteresesMora() {
    if (!checkClienteEnUrl()) return;

    const fecha = $('#fechaInteresMora').val();
    const hoy = obtenerFechaActualInput();
    const clienteActual = ($('#clientesFiltro').val() || clienteFiltro || '0').trim();

    if (!fecha) {
        toastr.warning('Debe seleccionar una fecha.', 'Alerta', toastrOptions);
        return;
    }

    if (fecha < hoy) {
        toastr.warning('La fecha no puede ser menor a la fecha actual.', 'Alerta', toastrOptions);
        return;
    }

    const desbloquear = bloquearBotonesRegistroPagos('#btnCargarInteresesMora');
    if (!desbloquear) return;

    mostrarLoadingIntereses();

    try {
        const url = `${URLRP}/ObtenerInteresesMoraCliente?cliente=${encodeURIComponent(clienteActual)}&fechaCorte=${encodeURIComponent(fecha)}`;
        const response = await fetch(url, { method: 'GET', credentials: 'same-origin' });
        const json = await response.json();

        if (!json.ok) {
            docsInteres = [];
            interesesCargados = false;
            actualizarTotalIntereses(0);
            KTDatatablesInteres.reset();
            toastr.warning(json.mensaje || 'No se pudieron obtener los intereses.', 'Alerta', toastrOptions);
            return;
        }

        docsInteres = json.data || [];
        const totalIntereses = json.total || docsInteres.reduce((sum, item) => sum + toNumber(item.INTERESES), 0);
        interesesCargados = totalIntereses > 0;
        actualizarTotalIntereses(totalIntereses);
        KTDatatablesInteres.reset();

        if (!docsInteres.length) {
            toastr.info('No se encontraron documentos para generar intereses.', 'Intereses', toastrOptions);
        } else if (totalIntereses <= 0) {
            toastr.warning('El total de intereses es cero. No se puede generar el documento.', 'Alerta', toastrOptions);
        }
    } catch (e) {
        console.error(e);
        toastr.error('Error obteniendo intereses de mora.', 'Error', toastrOptions);
    } finally {
        ocultarLoadingIntereses();
        desbloquear();
    }
}

async function continuarGeneracionInteres() {
    if (!interesesCargados) {
        toastr.warning('Debe generar los intereses antes de continuar.', 'Alerta', toastrOptions);
        return;
    }

    if (!docsInteres.length) {
        toastr.warning('No hay documentos para generar intereses.', 'Alerta', toastrOptions);
        return;
    }

    const fecha = $('#fechaInteresMora').val();
    const hoy = obtenerFechaActualInput();
    const clienteActual = ($('#clientesFiltro').val() || clienteFiltro || '0').trim();

    if (fecha < hoy) {
        toastr.warning('La fecha no puede ser menor a la fecha actual.', 'Alerta', toastrOptions);
        return;
    }

    const totalIntereses = docsInteres.reduce((sum, item) => sum + toNumber(item.INTERESES), 0);
    if (totalIntereses <= 0) {
        toastr.warning('El total de intereses es cero. No se puede generar el documento.', 'Alerta', toastrOptions);
        return;
    }

    const desbloquear = bloquearBotonesRegistroPagos('#btnContinuarGeneracionInteres');
    if (!desbloquear) return;

    const fd = new FormData();
    fd.set('cliente', clienteActual);
    fd.set('fechaCorte', fecha);

    mostrarLoadingIntereses();

    try {
        const response = await fetch(`${URLRP}/GenerarInteresesMoraCliente`, {
            method: 'POST',
            body: fd,
            credentials: 'same-origin'
        });
        const json = await response.json();

        if (!json.ok) {
            toastr.warning(json.mensaje || 'No se pudo generar el documento de intereses.', 'Alerta', toastrOptions);
            return;
        }

        const documento = json.data && json.data.DOCUMENTO ? json.data.DOCUMENTO : '';
        toastr.success(`Documento de intereses generado${documento ? ': ' + documento : ''}.`, 'Éxito', toastrOptions);
        $('#intereses').modal('hide');
        docsInteres = [];
        interesesCargados = false;
        actualizarTotalIntereses(0);
        KTDatatablesInteres.reset();
        actualizarDatosDespuesDePago();
    } catch (e) {
        console.error(e);
        toastr.error('Error generando el documento de intereses.', 'Error', toastrOptions);
    } finally {
        ocultarLoadingIntereses();
        desbloquear();
    }
}
function getParamUrl(k) {
    return new URLSearchParams(location.search).get(k) || '';
}

function CrearAbono() {
    if (!checkClienteEnUrl()) return;

    var visita_id = document.getElementById('spanvisita').innerText;
    console.log(visita_id);

    if (visita_id === null || visita_id === "" || visita_id === "Sin visita") {
        toastr.warning('Debe realizar una visita para registrar el pago', 'Alerta!', toastrOptions);
        return;
    }

    ObtenerUltimoConsecutivoCobrador();

    $("#tipoabono").val("REC").trigger("change")
    $("#subtipoAbono").val("00").trigger("change");
    $("#ReferenciaNumAbono").val("")
    $("#conceptoabono").val("")
    $("#montoabono").val(0)
    $("#cuentaabono").val(0)
    const cli = getParamUrl('cliente');
    const alias = getParamUrl('alias');

    const textoCliente = alias ? `${cli} | ${alias}` : cli;
    $('#cliabono').val(textoCliente);
    $("#modalAbono").modal("show")
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
        // Limpiar el campo de correos
        document.getElementById(campoCorreosID).value = '';
    }
}

function cargarSubtipo() {
    let tipoabono = $("#tipoabono").val();
    OpcionTipoAbono();
    console.log(tipoabono);
    $.ajax({
        url: `${URLRP}/CargarSubtipo`,
        data: { tipo_Doc: tipoabono },
        type: "POST",
        success: function (data) {
            const $selectSubtipo = $("select[name='subtipoAbono']");
            let SubTipo = "";
            if (data && data.length > 0) {
                // Opcional: reiniciar el select de subtipo antes de cargar nuevos datos
                $selectSubtipo.empty();
                SubTipo += "<option value='0'>Seleccione un subtipo</option>"; // Opción inicial
                data.forEach((prov) => {
                    SubTipo += `<option value='${prov.SUBTIPO}'>${prov.SUBTIPO} | ${prov.DESCRIPCION}</option>`;
                });
            } else {
                SubTipo += "<option value='0'>Sin Subtipo</option>";
            }
            $selectSubtipo.html(SubTipo);
        },
        error: function () {
            setTimeout(function () {
                toastr.warning("Error al cargar los subtipos. Intente nuevamente.");
            }, 3000);
        }
    });
}

function CargaClienteAbono() {
    //let cliente = clienteFiltro;
    //console.log(cliente);
    //if (cliente === "") {
    //    cliente = 0;
    //}
    let cliente = clienteFiltro || "0"; // Asignar 0 si clienteFiltro es vacío

    if (cliente) {
        $.ajax({
            url: `${URLRP}/obtenerCliente`,
            data: { cliente: cliente },
            type: "POST",
            success: function (data) {
                const $selectCLiente = $("select[name='cliabono']");
                let cobradorcliente = "";
                if (data && data.length > 0) {

                    if (cliente === "0") {
                        cobradorcliente += "<option value='0'>Seleccione un Cliente</option>"; // Opción inicial
                        data.forEach((prov) => {
                            cobradorcliente += `<option  value='${prov.CLIENTE}'>${prov.CLIENTE} - ${prov.NOMBRE}</option>`;
                        });
                    }
                    else if (cliente !== "0") {
                        data.forEach((prov) => {
                            cobradorcliente += `<option selected value='${prov.CLIENTE}'>${prov.CLIENTE} - ${prov.NOMBRE}</option>`;
                        });
                    }

                } else {
                    cobradorcliente += "<option value='0'>Sin Cliente</option>";
                }
                $selectCLiente.html(cobradorcliente);
                $selectCLiente.trigger("change");
            },
            error: function () {
                setTimeout(function () {
                    toastr.warning("Debe seleccionar un cliente.");
                }, 3000);
            }
        });
    }
}

function GuardarAsignarCC() {
    if ($("#cuentaorigen").val().length == 0) {
        toastr.warning(`Debe seleccionar una cuenta origen`, "Alerta", toastrOptions);
        return
    }

    if (existeNumBancoTEF == 'N') {
        toastr.warning('La referencia bancaria no existe', 'Alerta', toastrOptions)
        return
    }
    return AsignarCC();
}

async function AsignarCC() {
    if ($("#cliente_cxc").val().length == 0) {
        toastr.warning(`Debe seleccionar un cliente`, "Alerta", toastrOptions);
        return
    }

    var formData = new FormData();
    var _cuenta = $('#cuentaorigen').val();
    var _documento = $('#referencia_cxc').val();
    var _tipo = $('#tipo_cxc').val();
    var _subtipo = 0//$('#subtipo_cxc').val();
    var _fecha = $('#fechadoc_cxc').val();
    var _concepto = $('#concepto_cxc').val();
    var _monto = $('#monto_cxc').val().replace(/\./g, ',');
    var _idLinea = $('#idlinea_cxc').val();
    var _moneda = $('#moneda_cxc').val();
    var _cliente = $('#cliente_cxc').val();

    formData.append('cuenta', _cuenta);
    formData.append('documento', _documento);
    formData.append('tipo', _tipo);
    formData.append('subtipo', _subtipo);
    formData.append('fecha', _fecha);
    formData.append('concepto', _concepto);
    formData.append('monto', _monto);
    formData.append('idLinea', _idLinea);
    formData.append('moneda', _moneda);
    formData.append('cliente', _cliente);

    const url = `${URLRP}/InsertarDocumentoCC`
    const desbloquear = bloquearBotonesRegistroPagos('#btnGuardarReferenciaPago');
    if (!desbloquear) return;

    try {
        const response = await fetch(url, {
            method: 'POST',
            body: formData
        });
        const data = await response.json();
        if (data === "1") {
            toastr.success('Documento creado correctamente', 'Éxito', toastrOptions);
            $("#agregarRef").modal("hide");
        } else {
            toastr.warning('Ocurrió un error', 'Alerta', toastrOptions);
        }
    } catch (err) {
        console.error(err);
        toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions);
    } finally {
        desbloquear();
    }
}


function ObtenerUltimoConsecutivoCobrador() {
    return $.ajax({
        url: `${URLRP}/obtenerConsecutivoCobrador`,
        data: { cobrador: cobradorFiltro },
        type: "POST",
        dataType: "json",
        success: function (data) {
            const c = data.CONSECUTIVO || "";
            $("#numreciboAbono, #numrec, #numrecibo, #NotaCredito").val(c);
        },
        error: function () {
            $("#numreciboAbono, #numrec, #numrecibo, #NotaCredito").val("");
        }
    });
}

function checkClienteEnUrl(msgSinUrl = 'Debe abrir el registro desde la agenda del cliente.',
    msgDistinto = 'El cliente del filtro no coincide con el de la visita.') {

    const cliUrl = getParamUrl('cliente');
    const cliFiltro = ($('#clientesFiltro').val() || '').trim();

    if (!cliUrl) {
        toastr.warning(msgSinUrl, 'Alerta', toastrOptions);
        return false;
    }

    if (cliFiltro && cliFiltro !== '0' && cliFiltro !== cliUrl) {
        toastr.warning(msgDistinto, 'Alerta', toastrOptions);
        return false;
    }

    return true;
}

function abrirmodalCorreosPDF(EMAIL, CLIENTE, DOCUMENTO) {
    if (EMAIL === 'ND') {
        EMAIL = 'nd@nd.nd'
        $("#correosFE").val(EMAIL)
    }
    $("#NumFactura").val(DOCUMENTO)
    $("#modalenviarFE").modal("show")
    clientecorreoFERP = CLIENTE
}


function EnviarCorreoFE() {

    let factura = document.getElementById("NumFactura").value;
    let correo = document.getElementById("correosFE").value;
    let correoextra = document.getElementById("correosextraFE").value;
    let correosCompletoFE = correo + ";" + correoextra

    const desbloquear = bloquearBotonesRegistroPagos('#EnviarFacturaElectronica');
    if (!desbloquear) return;

    Dajax(`${URLRP}/Enviar_FE_Cliente`,
        "POST", { cliente: clientecorreoFERP, correos: correosCompletoFE, documento: factura }, function (response) {
            try {
                if (response.responseJSON == "1") {
                    toastr.success('Documento enviado con éxito', 'Éxito', toastrOptions)
                } else {
                    toastr.warning('Hubo un error al hacer el llamado!', 'Enviar Reporte', toastrOptions)
                }
            } finally {
                desbloquear();
            }
        });

    $("#modalenviarFE").modal("hide")
}


function AdjuntarArchivos() {
    solicitudExpediente = $('#Solicitud').val();
    lineaExpediente = 0;
    console.log(solicitudExpediente, lineaExpediente);

    KTUppy.init()

    if (solicitudExpediente && solicitudExpediente.trim() !== '') {
        $('#documento').val('SC').trigger('change')
        $('#solicitud').val(solicitudExpediente).trigger('change')
        $('#solicitudLinea').val(0).trigger('change')
        $('#proveedor').val(proveedorExpediente)
        $('#tipoDocumento').val('EC').trigger('change')
        $('#descripcion').val('Solicitud de compra')
        $('#tipoDocumento').prop('disabled', true);
        $('#proveedor').prop('disabled', true);
        $('#modalAdjuntarArchivos').modal('show')

    }
    else {
        $('#tipoDocumento').val('').trigger('change')
        $('#documento').val('').trigger('change')
        $('#proveedor').val('').trigger('change')
        $('#descripcion').val('')
        $('#modalAdjuntarArchivos').modal('show')
    }
}


const KTUppy = function () {
    const Dashboard = Uppy.Dashboard;
    const XHRUpload = Uppy.XHRUpload;
    let idTemporales = [];

    let initUppy1 = function () {
        let id = '#kt_uppy';
        if (document.querySelector(id)) {
            let UppyOptions = {
                proudlyDisplayPoweredByUppy: false,
                target: id,
                inline: true,
                replaceTargetContent: true,
                showProgressDetails: true,
                height: 470,
                metaFields: [
                    { id: 'name', name: 'Name', placeholder: 'file name' },
                    { id: 'caption', name: 'Caption', placeholder: 'describe what the image is about' }
                ],
                browserBackButtonClose: true,
                hideUploadButton: true
            };

            let uppyDashboard = Uppy.Core({
                autoProceed: false, // Cambia a false para evitar la carga automática
                restrictions: {
                    maxFileSize: 25 * 1024 * 1024, // 25 MB
                    maxNumberOfFiles: 5,
                    minNumberOfFiles: 1,
                }
            });

            uppyDashboard.use(Dashboard, UppyOptions);

            uppyDashboard.on('file-added', async (file) => {
                const convertToJPEGUsingCanvas = async (blob, fileName) => {
                    return new Promise((resolve, reject) => {
                        const img = new Image();
                        img.onload = () => {
                            const canvas = document.createElement('canvas');
                            canvas.width = img.width;
                            canvas.height = img.height;
                            const ctx = canvas.getContext('2d');
                            ctx.drawImage(img, 0, 0);

                            canvas.toBlob((jpegBlob) => {
                                if (jpegBlob) {
                                    const newFile = new File([jpegBlob], fileName.replace(/\.\w+$/, '.jpeg'), { type: 'image/jpeg' });
                                    resolve(newFile);
                                } else {
                                    reject(new Error('Canvas conversion failed'));
                                }
                            }, 'image/jpeg', 0.92); // Calidad del JPEG (0-1)
                        };

                        img.onerror = (error) => reject(error);
                        img.src = URL.createObjectURL(blob);
                    });
                };

                if (['heif', 'HEIF', 'png', 'PNG', 'img', 'IMG', 'jpg', 'JPG'].includes(file.extension)) {
                    try {
                        const newFile = await convertToJPEGUsingCanvas(file.data, file.name);
                        uppyDashboard.removeFile(file.id); // Elimina el archivo original
                        uppyDashboard.addFile({
                            name: newFile.name,
                            type: newFile.type,
                            data: newFile,
                            source: file.source,
                            id: file.id + '-converted',
                            meta: file.meta
                        });

                        uppyDashboard.upload(); // Inicia la carga solo del archivo convertido
                    } catch (error) {
                        console.log(`Error al convertir ${file.extension} a .jpeg:`, error);
                        toastr.error(`Error al convertir ${file.extension} a .jpeg`, 'Error', toastrOptions);
                    }
                } else {
                    uppyDashboard.upload(); // Si no es un archivo que necesita conversión, lo carga directamente
                }
            });

            uppyDashboard.use(XHRUpload, {
                endpoint: `${uppyUrlExp}/${GenerarIdTemporal()}`,
                getResponseData(responseText) {
                    let idTemporal = responseText.replaceAll('"', '');
                    idTemporales.push(idTemporal);
                    console.log(idTemporales);
                    return {
                        url: `${window.location.hostname}:${location.port}`
                    };
                }
            });
        }
    };

    return {
        init: function () {
            initUppy1();
        },
        getIdTemporales: function () {
            return idTemporales;
        }
    };
}();

async function GuardarAdjuntos() {

    const EXPEDIENTE_DIGITAL_URL = uppyUrlExp.replace(/\/SubirArchivos$/, '');
    const numrecibo = $('#numrecibo').val() || '';

    const proveedor = $('#clientesFiltro').val() || 'ND';
    const tipo = $('#documento').val();
    const documento = $('#tipoDocumento').val();
    const ordenCompra = $('#ordenCompra').val();
    const cotizacion = $('#cotizacion').val();
    const solicitud = $('#solicitud').val();
    const documento_cxc = $('#documentoCXC').val();
    const linea_solicitud = $('#solicitudLinea').val();
    const linea_cotizacion = $('#cotizacionLinea').val();
    const linea_ordencompra = $('#ordenCompraLinea').val();
    const descripcion = $('#descripcion').val();
    const subtipo = $('#tipoDocumento option:selected')
        .text().split('||')[1]?.trim();

    const documentoCxcLimpio = documento_cxc?.replace('|', '') ?? '';
    const referencia = [ordenCompra, cotizacion, solicitud, documentoCxcLimpio]
        .find(Boolean) ?? proveedor;
    const referencia_linea = [linea_solicitud, linea_cotizacion, linea_ordencompra]
        .find(Boolean) ?? 0;

    if ([tipo, documento, referencia].some(v => !v)) {
        toastr.warning('Todos los campos son obligatorios', 'Alerta', toastrOptions);
        return;
    }

    const idTemporales = KTUppy.getIdTemporales();
    if (!idTemporales.length) {
        toastr.warning('Debe adjuntar un archivo', 'Alerta', toastrOptions);
        return;
    }

    const desbloquear = bloquearBotonesRegistroPagos('#btnGuardarAdjuntosPago');
    if (!desbloquear) return;

    try {
        for (const idTemporal of idTemporales) {
            const fd = new FormData();
            fd.set('tipo', tipo);
            fd.set('referencia', numrecibo);
            fd.set('referenciaLinea', referencia_linea);
            fd.set('proveedor', proveedor);
            fd.set('documento', documento);
            fd.set('descripcion', descripcion);
            fd.set('adjunto', idTemporal);
            fd.set('subtipo', subtipo ?? '0');


            const resp = await fetch(`${EXPEDIENTE_DIGITAL_URL}/GuardarAdjuntos`, {
                method: 'POST',
                body: fd
            });
            const result = await resp.json();

            switch (result) {
                case 1:
                    toastr.success('Documento guardado', 'Éxito', toastrOptions);
                    $("#modalAdjuntarArchivos").modal("hide");
                    KTDatatablesServerSide.reset?.();
                    break;

                case 2:
                    toastr.warning('Todos los campos son obligatorios',
                        'Alerta', toastrOptions);
                    break;

                case 3:
                    toastr.warning('Debe adjuntar un archivo',
                        'Alerta', toastrOptions);
                    break;

                default:
                    toastr.warning('No se pudo guardar el documento',
                        'Alerta', toastrOptions);
            }
        }
    } catch (err) {
        console.error(err);
        toastr.error('Ha ocurrido un error', 'Error', toastrOptions);
    } finally {
        desbloquear();
    }
}

window.GuardarAdjuntos = GuardarAdjuntos;

$(document).ready(function () {
    vendedorFiltro = _vendedorFiltro;
    cobradorFiltro = _cobradorFiltro;
    tipoDocFiltro = $('#tipodoc').val() || '0';
    clienteFiltro = _clienteFiltro;


    InicializarSelect();

    setTimeout(function () {
        const cliIni = ($('#clientesFiltro').val() || '').trim();

        if (cliIni && cliIni !== '0') {
            clienteFiltro = cliIni;
            $('#clientesFiltro').trigger('change');
        }
    }, 0);

    obtenerRangoFechas();
    ManaejarBotones();
    CargaCreditosCliente();
    Correo_Cuentas_Pagar_Cliente();
    CargaClienteAbono();
    SaldoTotal();
    KTDatatablesServerSide.init();

});
