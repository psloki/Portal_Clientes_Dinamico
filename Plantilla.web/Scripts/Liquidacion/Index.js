/* =======================
   Liquidación de valores
   Index.js (versión completa mejorada)
   ======================= */

'use strict';

/* -------------------------------------------
 * 0) Helper de recarga inmediata de la DT
 * ------------------------------------------- */
function reloadDocs() {
    // Si la DataTable ya existe, limpia selección y recarga
    if ($.fn.dataTable.isDataTable('#kt_datatable_documentos')) {
        try {
            // limpiar selección y totales
            Object.keys(selectedIds).forEach(k => delete selectedIds[k]);
            Object.keys(selectedState).forEach(k => delete selectedState[k]);
            documentosParaBatch = [];
            $(".multi").addClass("d-none");
            $("#montoSeleccionadoCount").text(`0 documentos`);
            $("#montoSeleccionado").text('₡0.00');
            $("#TotalMontoSeleccionado").val('₡0.00');
        } catch (_) { }

        $('#kt_datatable_documentos').DataTable().ajax.reload(null, false);
    }
}

/* -------------------------------------------
 * 1) Sincroniza selects del layout -> hidden
 *    (cárgalo en todas las páginas)
 * ------------------------------------------- */

/* 1.b) Listeners del layout -> hidden (con namespace para no duplicar) */
$(document)
    .off('change.liq select2:select.liq select2:clear.liq', '#clientesFiltro, #clientesFiltroMovil')
    .on('change.liq select2:select.liq select2:clear.liq', '#clientesFiltro, #clientesFiltroMovil', syncClienteHidden);

$(document)
    .off('change.liq select2:select.liq select2:clear.liq', '#cobradorFiltro, #cobradorFiltroMovil')
    .on('change.liq select2:select.liq select2:clear.liq', '#cobradorFiltro, #cobradorFiltroMovil', syncCobradorHidden);

$(document)
    .off('change.liq select2:select.liq select2:clear.liq', '#vendedorFiltro, #vendedorFiltroMovil')
    .on('change.liq select2:select.liq select2:clear.liq', '#vendedorFiltro, #vendedorFiltroMovil', syncVendedorHidden);


function pickLayoutVal(...selectors) {
    for (const s of selectors) {
        const $el = $(s);
        if ($el.length) {
            const v = ($el.val() || '').trim();
            if (v) return v;              // solo si hay valor real
            return null;                  // si existe el control pero está vacío, NO tocar
        }
    }
    return null; // no existe ningún control en esta página
}

function syncClienteHidden() {
    const v = pickLayoutVal('#clientesFiltro', '#clientesFiltroMovil');
    if (v == null) return;                                   // <- CLAVE: no sobreescribas
    const prev = ($('#clienteHeader').val() || '').trim();
    if (prev !== v) {
        $('#clienteHeader').val(v);
        console.log('[SYNC] clienteHeader <-', v);
        reloadDocs();
    }
}
function syncCobradorHidden() {
    const v = pickLayoutVal('#cobradorFiltro', '#cobradorFiltroMovil');
    if (v == null) return;
    const prev = ($('#cobradorHeader').val() || '').trim();
    if (prev !== v) {
        $('#cobradorHeader').val(v);
        console.log('[SYNC] cobradorHeader <-', v);
        reloadDocs();
    }
    // Mostrar/ocultar “Liquidar” según regla (docs + cobrador)
    refrescarBotonLiquidar();

    // Si el modal está abierto, actualizá el input al vuelo
    if ($('#CrearTefModal').is(':visible')) {
        $('#cobrador').val(codigoCorto(getCobrador() || ''));
    }
}

function syncVendedorHidden() {
    const v = pickLayoutVal('#vendedorFiltro', '#vendedorFiltroMovil');
    if (v == null) return;                                   // <- CLAVE
    const prev = ($('#vendedorHeader').val() || '').trim();
    if (prev !== v) {
        $('#vendedorHeader').val(v);
        console.log('[SYNC] vendedorHeader <-', v);
        // reloadDocs(); // si lo necesitas
    }
}

// Antes tenías: setTimeout(syncClienteHidden, 300) ...
setTimeout(() => { if ($('#clientesFiltro, #clientesFiltroMovil').length) syncClienteHidden(); }, 300);
setTimeout(() => { if ($('#cobradorFiltro, #cobradorFiltroMovil').length) syncCobradorHidden(); }, 300);
setTimeout(() => { if ($('#vendedorFiltro, #vendedorFiltroMovil').length) syncVendedorHidden(); }, 300);




/* -------------------------------------------
 * 2) Helpers
 * ------------------------------------------- */
const status = {
    "S": { 'class': ' label-light-success', 'text': 'Activo' },
    "N": { 'class': ' label-light-danger', 'text': 'Inactivo' },
};

// Siempre leer del hidden sincronizado:
const getCliente  = () => ($('#clienteHeader').val()  || '').trim();
const getCobrador = () => ($('#cobradorHeader').val() || '').trim();
const getVendedor = () => ($('#vendedorHeader').val() || '').trim();

let fechaInicioFiltro = '';
let fechaFinFiltro = '';
let bancoFiltro = $('#bancoliquidacion').val();
let monedaFiltro = $('#monedaLiquidacion').val();
let tipDocFiltro = $('#tipoliquidacion').val();
let subtipoFiltro = parseInt($('#formapagoliquidacion').val() || '0', 10);
let estadoFiltro = $('#estadoliq').val();
let montosCuadrosTextoEfectivo = 0;
let montosCuadrosTextoCheque = 0;
let montosCuadrosTextoChequePostfechado = 0;
let cuentaBancoGlobal = '';
let existeNumBancoTEF_DEP = 'N';
var documentosParaBatch = [];

let montoOriginalSeleccionados = [];
let montoSeleccionados = [];
let _correos_marcados = [];
let conceptosArray = [];
let conceptoText = '';
let notaSeleccionada = '';
let subtipo_doc = '';
let filaDatos = [];
let tipoCredito = '';

const formas = {
    EFECTIVO: { title: 'Efectivo', class: 'badge-light-dark' },
    "Sinpe móvil": { title: 'Sinpe móvil', class: 'badge-light-primary' },
    CHEQUE: { title: 'Cheque', class: 'badge-light-warning' },
    "CHEQUE POSFECHADO": { title: 'Cheque postfechado', class: 'badge-light-warning' },
    Transferencia: { title: 'Transferencia', class: 'badge-light-success' },
    "MESES ANTERIORES": { title: 'Meses Anteriores', class: 'badge-light-success' }
};

const selectedIds = {}; // { rowKey: true/false }
const selectedState = {}; // { rowKey: { checked, monto, documento, cliente, moneda, tipo } }

function upsertSelectedState(rowKey, patch) {
    selectedState[rowKey] = Object.assign({}, selectedState[rowKey] || {}, patch);
}

function syncRowStateFromDom(rowKey) {
    const $chk = $(`#kt_datatable_documentos input.chk-doc[value="${rowKey}"]`);
    const $inp = $(`#montopago_${rowKey}`);
    if (!$chk.length || !$inp.length) return;

    const monto = parseFloat(($inp.val() || '0').toString().replace(/,/g, '')) || 0;
    upsertSelectedState(rowKey, {
        checked: $chk.prop('checked'),
        monto,
        documento: $chk.data('doc'),
        cliente: $chk.data('cliente'),
        moneda: $chk.data('moneda') || 'CRC',
        tipo: $chk.data('tipo') || ''
    });
    selectedIds[rowKey] = !!$chk.prop('checked');
}

function syncAllVisibleRowsState() {
    $('#kt_datatable_documentos input.chk-doc').each(function () {
        syncRowStateFromDom(this.value);
    });
}

function puedeLiquidar() {
    return documentosParaBatch.length > 0 && !!getCobrador(); // requiere documentos y cobrador header
}
refrescarBotonLiquidar();

if ($('#CrearTefModal').is(':visible')) {
    const { code, name } = getCobradorInfo();
    $('#cobrador').val(formatCobradorDisplay(code, name));
}



function toISO(dmy) { const [d, m, y] = dmy.split('/'); return `${y}-${m}-${d}`; }
function addCommas(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) x1 = x1.replace(rgx, '$1' + ',' + '$2');
    return x1 + x2;
}
function formatDollar(num, symbol) {
    var isNegative = false;
    if (num < 0) { isNegative = true; num = Math.abs(num); }
    var p = num.toString().split(".");
    var formattedNum = symbol + p[0].split("").reverse().reduce(function (acc, num, i) {
        return num === "-" ? acc : num + (i && !(i % 3) ? "," : "") + acc;
    }, "") + (p[1] === undefined || p[1] === null ? "" : "." + p[1]);
    return isNegative ? "-" + formattedNum : formattedNum;
}
function formatearMiles(valor) {
    return valor.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function codigoCorto(txt) {
    const left = (String(txt || '')).split('-')[0].split('|')[0].trim();
    return left.replace(/[^\dA-Za-z]/g, '');
}


function splitCodeNameFromLabel(label) {
    if (!label) return { code: '', name: '' };
    const m = label.match(/^\s*([A-Za-z0-9]+)\s*(?:-|\|\|?|\u2013|\u2014)\s*(.+)$/);
    if (m) return { code: m[1].trim(), name: m[2].trim() };
    return { code: codigoCorto(label), name: '' };
}

function getCobradorInfo() {
    let code = codigoCorto(getCobrador() || '');
    let name = '';

    // Nombre desde el select del layout (si existe)
    const $sel = $('#cobradorFiltro').length ? $('#cobradorFiltro') : $('#cobradorFiltroMovil');
    if ($sel.length && $sel.val() != null && $sel.find('option:selected').length) {
        const parsed = splitCodeNameFromLabel(($sel.find('option:selected').text() || '').trim());
        if (parsed.code) code = code || parsed.code;
        if (parsed.name) name = parsed.name;
    }
    return { code, name };
}

function formatCobradorDisplay(code, name) {
    return [code, name].filter(Boolean).join(' - ');
}

/* -------------------------------------------
 * 3) DataTable (server-side)
 * ------------------------------------------- */
var KTDatatablesServerSide = (function () {
    var dt;

    function makeKey(r) {
        const safe = s => String(s ?? '').replace(/[^A-Za-z0-9_-]/g, '_');
        return [safe(r.TIPO), safe(r.DOCUMENTO), safe(r.COD_PROVEEDOR || '')].join('__');
    }
    const currencySymbol = (m) => (m === 'USD' ? '$' : m === 'CRC' ? '₡' : '₡');

    var initDatatable = function () {
        dt = $("#kt_datatable_documentos").DataTable({
            searchDelay: 500,
            processing: true,
            responsive: false,       // evita que Responsive te “reparta” columnas
            scrollX: true,
            autoWidth: false,
            order: [],
            destroy: true,

            ajax: {
                url: `${URLP}/GetDocumentosCC`,
                type: 'GET',
                data: function (d) {
                    const raw = ($('#FechaInicioV').val() || '').trim();
                    let fi, ff;
                    if (raw && raw.includes(' - ')) {
                        const [a, b] = raw.split(' - ');
                        fi = toISO(a); ff = toISO(b);
                    } else {
                        const today = new Date();
                        const past = new Date(); past.setDate(today.getDate() - 60);
                        const pad = n => String(n).padStart(2, '0');
                        const toIso = x => `${x.getFullYear()}-${pad(x.getMonth() + 1)}-${pad(x.getDate())}`;
                        fi = toIso(past); ff = toIso(today);
                    }
                    d.fechaInicio = fi;
                    d.fechaFin = ff;
                    d.subtipo = $("#subtipoliquidacion").val();
                    d.deposito = $("#Depositoliquidacion").val();
                    d.depositado = $("#Depositadoliquidacion").val();
                },
                // 👉 Aquí mapeamos MONTO_A_LIQUIDAR sin tocar el SP
                dataSrc: function (rows) {
                    return (rows || []).map(r => {
                        const saldo = Number(r.SALDO ?? 0);
                        const monto = Number(r.MONTO ?? 0);
                        return {
                            ...r,
                            MONTO_A_LIQUIDAR: Number(r.MONTO_A_LIQUIDAR ?? (saldo || monto))
                        };
                    });
                },
                error: function (xhr, status, err) {
                    if (status === 'abort') return;
                    console.error('Error DataTables AJAX:', status, err, xhr?.responseText);
                    toastr.error('No se pudieron cargar los documentos.', 'Error');
                }
            },

            columns: [
                {
                    data: 'DEPOSITADO', defaultContent: '',
                    orderable: false, width: 30,
                    render: function (_d, _t, row) {
                        const k = makeKey(row);
                        const max = Number(row.SALDO ?? row.MONTO ?? 0);

                        if (_d) {
                            return '';
                        }

                        return `
                                <div class="form-check form-check-sm form-check-custom form-check-solid">
                                  <input class="form-check-input chk-doc" type="checkbox"
                                         value="${k}"
                                         data-doc="${row.DOCUMENTO}"
                                         data-cliente="${row.COD_PROVEEDOR || ''}"
                                         data-monto="${max}"
                                         data-moneda="${row.MONEDA}"
                                         data-tipo="${row.TIPO}" />
                                </div>`;
                    }
                },         // 0 checkbox
                { data: 'TIPO' },                           // 1
                { data: 'SUBTIPO_DESC' },
                { data: 'DOCUMENTO' },                      // 2
                { data: 'DEPOSITADO' },
                { data: 'VENDEDOR_NOMBRE' },
                { data: 'CLIENTE_CONTRIBUYENTE' },          // 3
                { data: 'FECHA' },                          // 4
                { data: 'MONEDA' },                         // 5
                {
                    data: 'MONTO', defaultContent: 0,
                    className: 'text-end text-nowrap',
                    render: (d, _t, row) =>
                        `${currencySymbol(row.MONEDA)}${Number(d || 0).toLocaleString('en-US', { minimumFractionDigits: 2 })}`
                },       // 6  monto original
                {
                    data: 'SALDO',
                    defaultContent: 0,
                    className: 'text-end text-nowrap',
                    render: (d, _t, row) =>
                        `${currencySymbol(row.MONEDA)}${Number(d || 0).toLocaleString('en-US', { minimumFractionDigits: 2 })}`
                },       
                {
                    data: 'MONTO_A_LIQUIDAR',
                    defaultContent: 0,
                    className: 'text-end text-nowrap',
                    render: (d, _t, row) =>
                        `${currencySymbol(row.MONEDA)}${Number(d || 0).toLocaleString('en-US', { minimumFractionDigits: 2 })}`
                }, // 7  input editable
                {
                    data: 'TIPO_CAMBIO', defaultContent: 0,
                    orderable: false, className: 'text-end text-nowrap', width: 160,
                    render: function (_d, _t, row) {
                        const k = makeKey(row);
                        const max = Number(row.SALDO ?? row.MONTO ?? 0);
                        const estado = selectedState[k];
                        const val = Number(estado?.monto ?? row.MONTO_A_LIQUIDAR ?? row.SALDO ?? 0);

                        return `<input class="form-control form-control-sm montopagos"
                                style="max-width:140px"
                                disabled
                                id="montopago_${k}"
                                type="text"
                                value="${val.toLocaleString('en-US', { minimumFractionDigits: 2 })}"
                                max="${max}" min="0"
                                onchange="updateTotal(this)">`;
                    }
                }, // 8
                {
                    data: 'USUARIO',
                    className: 'text-end text-nowrap',
                    //render: d => Number(d || 0).toLocaleString('en-US', { minimumFractionDigits: 2 })
                } // 9
            ],

            columnDefs: []
        });

        // Sincroniza checks e inputs cuando se redibuja
        dt.on('draw', () => {
            $('#kt_datatable_documentos input.chk-doc').each(function () {
                const id = this.value;
                const estado = selectedState[id];
                const checked = !!(estado ? estado.checked : selectedIds[id]);
                $(this).prop('checked', checked);
                const $inp = $(`[id="montopago_${id}"]`);
                $inp.prop('disabled', !checked);
                if (estado && estado.monto != null) {
                    $inp.val(Number(estado.monto).toLocaleString('en-US', { minimumFractionDigits: 2 }));
                }
                syncRowStateFromDom(id);
            });
            const allOnPage = dt.rows({ search: 'applied' }).data().toArray()
                .map(r => makeKey(r))
                .every(k => selectedIds[k]);
            $('#checkAll').prop('checked', allOnPage);
        });

        // Click individual
        $('#kt_datatable_documentos').on('change', 'input.chk-doc', function () {
            const id = this.value;
            selectedIds[id] = this.checked;
            $(`[id="montopago_${id}"]`).prop('disabled', !this.checked);
            syncRowStateFromDom(id);
            ObtenerSeleccion();
        });

        // Seleccionar todos (solo página visible)
        $('#checkAll').on('change', function () {
            const check = this.checked;
            const keys = dt.rows({ search: 'applied' }).data().toArray().map(r => makeKey(r));
            keys.forEach(k => {
                selectedIds[k] = check;
                upsertSelectedState(k, { checked: check });
            });
            dt.draw(false);
            ObtenerSeleccion();
        });

        // Rango fechas -> recarga
        $('#FechaInicioV').on('apply.daterangepicker change', () => dt.ajax.reload(null, false));
        $("#subtipoliquidacion, #Depositoliquidacion, #Depositadoliquidacion").on("change", () => {
            dt.ajax.reload(null, false);
            ObtenerDocumentosTotales();
        });
    };

    var resetTable = function () {
        if (dt && dt.settings()[0].oInit.ajax) {
            dt.ajax.reload(null, false);
        } else if (dt) {
            dt.draw(false);
        }
    };

    return {
        init: function () {
            initDatatable();
            bindSearchInput();
            $(".multi").addClass("d-none");
        },
        reset: function () {
            resetTable();
            $(".multi").addClass("d-none");
        }
    };
})();

function ObtenerDocumentosTotales() {
    const raw = ($('#FechaInicioV').val() || '').trim();
    let fi, ff;
    if (raw && raw.includes(' - ')) {
        const [a, b] = raw.split(' - ');
        fi = toISO(a); ff = toISO(b);
    } else {
        const today = new Date();
        const past = new Date(); past.setDate(today.getDate() - 60);
        const pad = n => String(n).padStart(2, '0');
        const toIso = x => `${x.getFullYear()}-${pad(x.getMonth() + 1)}-${pad(x.getDate())}`;
        fi = toIso(past); ff = toIso(today);
    }
    
    const subtipo = $("#subtipoliquidacion").val();
    const deposito = $("#Depositoliquidacion").val();
    const depositado = $("#Depositadoliquidacion").val();

    $.ajax({
        url: URLP + '/GetDocumentosCCTotales',
        type: 'GET',
        data: { fechaInicio: fi, fechaFin: ff, subtipo, deposito, depositado },
        success: function (json) {
            let monto = 0;
            let total = 0;

            $("#monto_17").text("₡ 0");
            $("#monto_count_17").text("0");
            $("#monto_18").text("₡ 0");
            $("#monto_count_18").text("0");
            $("#monto_19").text("₡ 0");
            $("#monto_count_19").text("0");

            json.map(item => {
                monto += item.MONTO;
                total += item.TOTAL;
                console.log(item)
                $("#monto_" + item.SUBTIPO).text('₡' + formatearMiles(item.MONTO));
                $("#monto_count_" + item.SUBTIPO).text(item.TOTAL);
            });

            if ($("#Depositadoliquidacion").val() === 'S') {
                $("#monto_no_depositado").text("₡ 0");
                $("#monto_count_no_depositado").text("0");
            }

            if ($("#Depositadoliquidacion").val() === 'N') {
                $("#monto_no_depositado").text('₡' + formatearMiles(monto));
                $("#monto_count_no_depositado").text(total);
            }
            
            $("#monto_total").text('₡' + formatearMiles(monto));
            $("#monto_count_total").text(total);
        }
    });
}

function formatearMiles(valor) {
    return new Intl.NumberFormat("en-US", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }).format(valor);
}

function bindSearchInput() {
    const t = $('#kt_datatable_documentos').DataTable();
    $('#kt_datatable_search_query').on('keyup', function () {
        t.search(this.value).draw();
    });
}


/* -------------------------------------------
 * 4) Selección y totales
 * ------------------------------------------- */
function ocultarBotonesFlotantes() {
    $(".multi").addClass('d-none');
}

function ObtenerSeleccion() {
    syncAllVisibleRowsState();
    documentosParaBatch = [];

    Object.keys(selectedState).forEach(rowKey => {
        const s = selectedState[rowKey];
        if (!s || !s.checked) return;
        documentosParaBatch.push({
            linea: rowKey,
            documento: s.documento,
            cliente: s.cliente,
            moneda: s.moneda || 'CRC',
            tipo: s.tipo || '',
            monto: Number(s.monto || 0)
        });
    });

    // 👉 Mostrar / ocultar botón flotante "Liquidar" según selección + cobrador
    refrescarBotonLiquidar();

    const totalMonto = documentosParaBatch.reduce((t, d) => t + d.monto, 0);
    const totalFmt = '₡ ' + addCommas(totalMonto.toFixed(2));
    $("#montoSeleccionado").text(totalFmt);
    $("#montoSeleccionadoCount").text(`${documentosParaBatch.length} documentos`);
    $("#TotalMontoSeleccionado").val(totalFmt);

    const conceptos = documentosParaBatch.map(d =>
        `${d.tipo || ''} | ${d.documento} | ${d.cliente} | Monto: ₡${addCommas(d.monto.toFixed(2))}`
    );
    $("#concepto").val(`Liquidación valores: ${conceptos.join(', ')}`);
}


function updateTotal(inputElement) {
    const idAttr = $(inputElement).attr('id');   // ej: montopago_FAC__123__C001
    const rowKey = idAttr.replace('montopago_', '');

    const max = parseFloat($(inputElement).attr('max')) || 0;
    let montoIngresado = parseFloat($(inputElement).val().replace(/,/g, '')) || 0;

    if (montoIngresado > max) {
        montoIngresado = max;
        $(inputElement).val(max.toLocaleString('en-US'));
        toastr.warning(`El monto no puede ser mayor a ₡${max.toLocaleString('en-US')}`, 'Alerta');
    }
    if (montoIngresado < 0) {
        montoIngresado = 0;
        $(inputElement).val('0');
        toastr.warning(`El monto no puede ser menor a ₡0`, 'Alerta');
    }

    const doc = documentosParaBatch.find(d => d.linea === rowKey);
    if (doc) doc.monto = montoIngresado;
    upsertSelectedState(rowKey, { monto: montoIngresado });

    ObtenerSeleccion();
}


/* -------------------------------------------
 * 5) Modal: abrir y guardar
 * ------------------------------------------- */
function limpiarModalCreacion() {
    const $m = $('#CrearTefModal');

    // Campos del modal
    $m.find('#bancoliquidacion').val('0').trigger('change');  // vuelve a "Todos"
    $m.find('#numtefr').val('');         // referencia
    $m.find('#FechaInicio2Seguimiento').val(moment().format('DD/MM/YYYY')); // fecha = hoy
    $m.find('#subtipo').val('00').trigger('change');          // subtipo por defecto
    $m.find('#comentarioscob').val('');                       // comentario
    // Derivados (los repoblamos al abrir el modal)
    $m.find('#TotalMontoSeleccionado').val('₡0.00');
    $m.find('#concepto').val('');

    // Hidden
    $m.find('#clienteModalHidden, #docIdsHidden').val('');

    //Los archivo adjuntos no estan disponibles para este proceso, por lo que se omite su limpieza
    // Archivos / preview (KT image-input)
    //$m.find('#ComprobantePago1,#ComprobantePago2').val('');
    //$m.find('[data-kt-image-input="true"] .image-input-wrapper')
    //    .css('background-image', 'none');
}


function asegurarHiddenModal() {
    if (!$('#clienteModalHidden').length) $('<input type="hidden" id="clienteModalHidden">').appendTo('#CrearTefModal .modal-body');
    if (!$('#docIdsHidden').length) $('<input type="hidden" id="docIdsHidden">').appendTo('#CrearTefModal .modal-body');
}

function aprobar() {
    asegurarHiddenModal();

    // No abras el modal si no hay cobrador en el header
    const cobCode = codigoCorto(getCobrador() || '');
    if (!cobCode) {
        toastr.warning('Seleccione un cobrador válido en el header.', 'Alerta');
        return;
    }

    $('#cobrador').val(cobCode);

    // Multi-cliente permitido: el cliente se resuelve por cada línea seleccionada en backend.
    $('#clienteModalHidden').val('0');

    const ids = documentosParaBatch.map(d => `${d.tipo}|${d.documento}|${d.cliente}`);
    $('#docIdsHidden').val(ids.join(','));

    $('#CrearTefModal').on('show.bs.modal', function () {
        const { code, name } = getCobradorInfo();
        $('#cobrador').val(formatCobradorDisplay(code, name));
    });

}

let guardandoDepositoLiquidacion = false;

function obtenerBotonGuardarDeposito() {
    return $('#CrearTefModal').find('.modal-footer button[onclick*="guardarcambios"]');
}

function mostrarLoadingGuardarDeposito() {
    guardandoDepositoLiquidacion = true;

    const $btn = obtenerBotonGuardarDeposito();
    if ($btn.length) {
        $btn.data('texto-original', $btn.html());
        $btn.prop('disabled', true).html('<span class="spinner-border spinner-border-sm align-middle me-2"></span>Guardando...');
    }

    $('#CrearTefModal').find('[data-bs-dismiss="modal"], [data-dismiss="modal"]').prop('disabled', true);

    if (typeof KTApp !== 'undefined' && typeof KTApp.block === 'function') {
        KTApp.block('#CrearTefModal .modal-content', {
            overlayColor: '#000000',
            state: 'primary',
            message: 'Guardando depósito...'
        });
    }
}

function ocultarLoadingGuardarDeposito() {
    guardandoDepositoLiquidacion = false;

    const $btn = obtenerBotonGuardarDeposito();
    if ($btn.length) {
        $btn.prop('disabled', false).html($btn.data('texto-original') || 'Guardar');
    }

    $('#CrearTefModal').find('[data-bs-dismiss="modal"], [data-dismiss="modal"]').prop('disabled', false);

    if (typeof KTApp !== 'undefined' && typeof KTApp.unblock === 'function') {
        KTApp.unblock('#CrearTefModal .modal-content');
    }
}


async function guardarcambios() {
    if (guardandoDepositoLiquidacion) return;

    try {
        const $modal = $('#CrearTefModal');
        const fd = new FormData();

        const $optBanco = $modal.find('#bancoliquidacion option:selected');
        const cuenta = ($optBanco.data('cuenta') || '').toString().trim();

        const documento = ($modal.find('#numtefr').val() || '').toString().trim();
        const montoSelTxt = ($modal.find('#TotalMontoSeleccionado').val() || '0').toString().replace(/[₡,\s]/g, '');
        const montoSelNum = Number(montoSelTxt);

        const fecha = ($modal.find('#FechaInicio2Seguimiento').val() || '').trim(); // dd/MM/yyyy
        const subtipo = parseInt($modal.find('#subtipo').val() || '0', 10);
        const moneda = ($modal.find('#moneda').val() || 'CRC').trim();
        const beneficiarioNombre = ($modal.find('#cobrador').val() || '').trim();
        const concepto = ($modal.find('#concepto').val() || '').trim();

        const comentario = ($modal.find('#comentarioscob').val() || '').trim();

        // reales (FK) desde hidden/layout
        const cliente = '0';
        const cobrador = codigoCorto(getCobrador() || '');
        const vendedor = codigoCorto(getVendedor() || '');

        if (!cobrador) { toastr.warning('Seleccione un cobrador válido en el header.', 'Alerta', toastrOptions); return; }
        if (!vendedor) { toastr.warning('Seleccione un vendedor válido en el header.', 'Alerta', toastrOptions); return; }
        if (!cuenta) { toastr.warning('Seleccione un Banco válido.', 'Alerta', toastrOptions); return; }
        if (!documento) { toastr.warning('Ingrese el # de referencia.', 'Alerta', toastrOptions); return; }
        if (!montoSelNum || isNaN(montoSelNum) || montoSelNum <= 0) { toastr.warning('El monto debe ser mayor a 0.', 'Alerta', toastrOptions); return; }
        if (!fecha || !/^\d{2}\/\d{2}\/\d{4}$/.test(fecha)) { toastr.warning('La fecha debe tener formato dd/MM/yyyy.', 'Alerta', toastrOptions); return; }
        if (isNaN(subtipo)) { toastr.warning('Seleccione un Subtipo válido.', 'Alerta', toastrOptions); return; }

        const tipoDocCC = 'DEP'; // documento que insertas en DOCUMENTOS_CC para el depósito
        const tipoBanco = 'DEP'; // movimiento en MOV_BANCOS

        const detalleAplicacion = (documentosParaBatch || []).map(d => ({
            CLIENTE: (d.cliente || '').toString().trim(),
            TIPO_DOC: (d.tipo || '').toString().trim(),        // debería venir 'REC'
            DOCUMENTO: (d.documento || '').toString().trim(),  // número del REC
            MONTO_APLICADO: Number(d.monto || 0)               // lo que digitaste en "Monto a liquidar" por línea
        }));

        const clientePrincipal = (detalleAplicacion.find(x => x.CLIENTE) || {}).CLIENTE || '0';

        const sumaDetalle = detalleAplicacion.reduce((acc, x) => acc + (Number(x.MONTO_APLICADO) || 0), 0);

        if (detalleAplicacion.length === 0) {
            toastr.warning('Seleccione al menos un recibo (REC) y un monto a liquidar por línea.', 'Alerta', toastrOptions);
            return;
        }
        if (sumaDetalle > montoSelNum + 1e-6) {
            toastr.warning('La suma del detalle excede el monto del depósito.', 'Alerta', toastrOptions);
            return;
        }
        // Si quieres que el depósito SIEMPRE sea exactamente la suma de los REC seleccionados:
        // if (Math.abs(sumaDetalle - montoSelNum) > 1e-6) {
        //     toastr.warning('El monto del depósito debe ser igual a la suma de los recibos seleccionados.', 'Alerta', toastrOptions);
        //     return;
        // }

        // --- payload para tu VM/Controller ---
        fd.append('Cliente', clientePrincipal);
        fd.append('CuentaBancaria', cuenta);
        fd.append('Documento', documento);
        fd.append('Monto', montoSelNum.toFixed(2));      
        fd.append('Fecha', fecha);
        fd.append('TipoCC', tipoDocCC);
        fd.append('TipoBanco', tipoBanco);
        fd.append('Subtipo', String(subtipo));
        fd.append('Moneda', moneda);
        fd.append('Concepto', concepto);
        fd.append('Beneficiario', beneficiarioNombre);
        fd.append('Referencia', documento);
        fd.append('DetalleAplicacionJson', JSON.stringify(detalleAplicacion));
        fd.append('Comentario', comentario);

        //--->cambio 02-24-26: se omite la subida de archivos
        //const f1 = document.getElementById('ComprobantePago1')?.files?.[0];
        //const f2 = document.getElementById('ComprobantePago2')?.files?.[0];

        //if (!f1 && !f2) {
        //    toastr.warning('Debe adjuntar al menos un comprobante de pago.', 'Alerta', toastrOptions);
        //    return;
        //}

        //if (f1) fd.append('ComprobantePago', f1);
        //if (f2) fd.append('ComprobantePago', f2);
        //--->fin cambio 02-04-26

        const cobradorNombre = ($('#cobrador').val() || '').trim();
        fd.append('Cobrador', `${cobrador} - ${cobradorNombre}`);
        fd.append('Vendedor', vendedor);

        mostrarLoadingGuardarDeposito();

        // --- POST ---
        const resp = await fetch(`${URLP}/CrearDeposito`, {
            method: 'POST',
            body: fd,
            credentials: 'same-origin'
        });
        if (!resp.ok) throw new Error(await resp.text() || ('HTTP ' + resp.status));

        const data = await resp.json();
        $modal.modal('hide');

        if (data.ok) {
            toastr.success(data.msg || 'Depósito creado y recibos liquidados.', 'Éxito', toastrOptions);
            limpiarModalCreacion();
            reloadDocs(); // verás los SALDO de los REC actualizados
        } else {
            if (data.errors) {
                Object.keys(data.errors).forEach(k => {
                    const arr = data.errors[k];
                    if (arr && arr.length) toastr.error(`${k}: ${arr.join(', ')}`, 'Error', toastrOptions);
                });
            }
            toastr.error(data.msg || 'No se pudo crear el depósito.', 'Error', toastrOptions);
        }
    } catch (e) {
        $('#CrearTefModal').modal('hide');
        toastr.error('Error: ' + (e?.message || e), 'Error', toastrOptions);
        console.error(e);
    } finally {
        ocultarLoadingGuardarDeposito();
    }
}



/* -------------------------------------------
 * 6) Otras funciones (bancos, saldos, etc.)
 * ------------------------------------------- */
function CargaFormaPagoLiquidacion() {
    let tipo_docLiquidacion = $('#tipoliquidacion').val();
    if (tipo_docLiquidacion === "") tipo_docLiquidacion = 0;

    if (tipo_docLiquidacion !== undefined && tipo_docLiquidacion !== null) {
        $.ajax({
            url: `${URLP}/obtenerFormaPagoDetalle`,
            data: { tipo: tipo_docLiquidacion },
            type: "POST",
            success: function (data) {
                const $selectFormaPago = $("select[name='formapagoliquidacion']");
                let formadePagoLiq = "";
                if (tipo_docLiquidacion === "0") {
                    formadePagoLiq += "<option selected value='0'>Todos</option>";
                } else if (data && data.length > 0) {
                    data.forEach((prov) => {
                        formadePagoLiq += `<option selected value='${prov.SUBTIPO}'>${prov.SUBTIPO_DET} | ${prov.DESCRIPCION}</option>`;
                    });
                } else {
                    formadePagoLiq += "<option value='0'>Sin Forma de Pago</option>";
                }
                $selectFormaPago.html(formadePagoLiq);
                $selectFormaPago.trigger("change");
            },
            error: function () {
                setTimeout(function () {
                    toastr.warning("Debe seleccionar un tipo de documento.");
                }, 3000);
            }
        });
    }
}

function ManaejarBotones() {
    $('#FechaInicioV').on('change', function () {
        let valor = this.value;
        let fechas = valor.split(" - ");
        fechaInicioFiltro = fechas[0].split('/').reverse().join('-');
        fechaFinFiltro = fechas[1].split('/').reverse().join('-');
        KTDatatablesServerSide.reset();
    });

    $('#clienteliquidacion').on('change', function () {
        KTDatatablesServerSide.reset();
    });

    $('#monedaLiquidacion').on('change', function () {
        monedaFiltro = this.value;
        KTDatatablesServerSide.reset();
    });

    $('#tipoliquidacion').on('change', function () {
        tipDocFiltro = this.value;
        CargaFormaPagoLiquidacion();
        KTDatatablesServerSide.reset();
    });

    $('#formapagoliquidacion').on('change', function () {
        subtipoFiltro = this.value;
        KTDatatablesServerSide.reset();
    });

    if ($('#tipoliquidacion').val() === "") {
        $('#tipoliquidacion').val("0").trigger("change");
    }
}

function GuardarAsignarCC() {
    if ($("#cuentaorigen").val().length == 0) {
        toastr.warning(`Debe seleccionar una cuenta origen`, "Alerta", toastrOptions);
        return;
    }
    if (existeNumBancoTEF_DEP == 'N') {
        toastr.warning('La referencia bancaria no existe', 'Alerta', toastrOptions);
        return;
    }
    AsignarCC();
}

function AsignarCC() {
    if ($("#cliente_cxc").val().length == 0) {
        toastr.warning(`Debe seleccionar un cliente`, "Alerta", toastrOptions);
        return;
    }

    var formData = new FormData();
    formData.append('cuenta', $('#cuentaorigen').val());
    formData.append('documento', $('#referencia_cxc').val());
    formData.append('tipo', $('#tipo_cxc').val());
    formData.append('subtipo', 0);
    formData.append('fecha', $('#fechadoc_cxc').val());
    formData.append('concepto', $('#concepto_cxc').val());
    formData.append('monto', $('#monto_cxc').val().replace(/\./g, ','));
    formData.append('idLinea', $('#idlinea_cxc').val());
    formData.append('moneda', $('#moneda_cxc').val());
    formData.append('cliente', $('#cliente_cxc').val());

    const url = `${URLRP}/InsertarDocumentoCC`;
    fetch(url, { method: 'POST', body: formData })
        .then(res => res.json())
        .then(data => {
            if (data === "1") {
                toastr.success('Documento creado correctamente', 'Éxito', toastrOptions);
            } else {
                toastr.warning('Ocurrió un error', 'Alerta', toastrOptions);
            }
        })
        .catch(err => {
            console.error(err);
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions);
        });

    $("#agregarRef").modal("hide");
}

function obtenerRangoFechas() {
    let valor = (document.getElementById("FechaInicioV") || {}).value || '';
    let fechas = valor.split(" - ");
    fechaInicioFiltro = (fechas[0] || '').split('/').reverse().join('-');
    fechaFinFiltro = (fechas[1] || '').split('/').reverse().join('-');
}

function InicializarSelect() {
    const raw = ($('#FechaInicioV').val() || '').trim();

    let start, end;
    if (raw && raw.includes(' - ')) {
        const [a, b] = raw.split(' - ');
        start = moment(a, 'DD/MM/YYYY', true);
        end = moment(b, 'DD/MM/YYYY', true);
    }
    if (!start?.isValid() || !end?.isValid()) {
        start = moment().subtract(59, 'days');
        end = moment();
        $('#FechaInicioV').val(`${start.format('DD/MM/YYYY')} - ${end.format('DD/MM/YYYY')}`);
    }

    $('#FechaInicioV').daterangepicker({
        startDate: start,
        endDate: end,
        autoUpdateInput: true,
        locale: { format: 'DD/MM/YYYY', applyLabel: 'Aplicar', cancelLabel: 'Cancelar' }
    });

    $('#FechaInicioV').on('apply.daterangepicker change', () => {
        const t = $('#kt_datatable_documentos').DataTable();
        if (t) t.ajax.reload(null, false);
    });
}

function checkAll_activos() {
    $('#checkAll').on('click', function () {
        var isChecked = $("#checkAll").prop('checked');
        if (isChecked) $(".multi").removeClass("d-none");
        else $(".multi").addClass("d-none");
    });
}

function obtenerMoneda(cuenta) {
    const fd = new FormData();
    fd.set('cuenta', cuenta);
    fetch(`${URLP}/ObtenerMonedaXCuenta`, { method: 'POST', body: fd })
        .then(res => res.json())
        .then(data => {
            if (data != null) $('#monedaLiquidacion').val(data.MONEDA);
        })
        .catch(err => {
            console.error(err);
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions);
        });
}

function obtenerSaldosEfectivo() {
    let fd = new FormData();
    fd.set('fechaInicio', fechaInicioFiltro);
    fd.set('fechaFin', fechaFinFiltro);
    fd.set('cliente', getCliente());
    fd.set('cobrador', getCobrador());
    fd.set('moneda', monedaFiltro);
    fd.set('tipoDoc', tipDocFiltro);
    fd.set('subTipoDoc', subtipoFiltro);
    fd.set('estado', estadoFiltro);

    const url = `${URLP}/ObtenerSaldosEfectivo`;
    fetch(url, { method: 'POST', body: fd })
        .then(res => res.json())
        .then(data => {
            if (data?.MONTO != null) {
                $('#montoEfectivo').text("₡ " + addCommas(data.MONTO.toFixed(2).toString()));
                montosCuadrosTextoEfectivo = data.MONTO;
            } else {
                $('#montoEfectivo').text("₡ 0.00");
                montosCuadrosTextoEfectivo = 0;
            }
            obtenerSaldosNoDepositado();
        })
        .catch(err => {
            console.error(err);
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions);
        });
}

function obtenerSaldosCheque() {
    let fd = new FormData();
    fd.set('fechaInicio', fechaInicioFiltro);
    fd.set('fechaFin', fechaFinFiltro);
    fd.set('cliente', getCliente());
    fd.set('cobrador', getCobrador());
    fd.set('moneda', monedaFiltro);
    fd.set('tipoDoc', tipDocFiltro);
    fd.set('subTipoDoc', subtipoFiltro);
    fd.set('estado', estadoFiltro);

    const url = `${URLP}/ObtenerSaldosCheque`;
    fetch(url, { method: 'POST', body: fd })
        .then(res => res.json())
        .then(data => {
            if (data?.MONTO != null) {
                $('#montoCheque').text("₡ " + addCommas(data.MONTO.toFixed(2).toString()));
                montosCuadrosTextoCheque = data.MONTO;
            } else {
                $('#montoCheque').text("₡ 0.00");
                montosCuadrosTextoCheque = 0;
            }
            obtenerSaldosNoDepositado();
        })
        .catch(err => {
            console.error(err);
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions);
        });
}

function obtenerSaldosChequePostfechado() {
    let fd = new FormData();
    fd.set('fechaInicio', fechaInicioFiltro);
    fd.set('fechaFin', fechaFinFiltro);
    fd.set('cliente', getCliente());
    fd.set('cobrador', getCobrador());
    fd.set('moneda', monedaFiltro);
    fd.set('tipoDoc', tipDocFiltro);
    fd.set('subTipoDoc', subtipoFiltro);
    fd.set('estado', estadoFiltro);

    const url = `${URLP}/ObtenerSaldosChequePostfechado`;
    fetch(url, { method: 'POST', body: fd })
        .then(res => res.json())
        .then(data => {
            if (data?.MONTO != null) {
                $('#monto_chequeposfechado').text("₡ " + addCommas(data.MONTO.toFixed(2).toString()));
                montosCuadrosTextoChequePostfechado = data.MONTO;
            } else {
                $('#monto_chequeposfechado').text("₡ 0.00");
                montosCuadrosTextoChequePostfechado = 0;
            }
            obtenerSaldosNoDepositado();
        })
        .catch(err => {
            console.error(err);
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions);
        });
}

function obtenerSaldosNoDepositado() {
    var montoNoDepositado = montosCuadrosTextoEfectivo + montosCuadrosTextoCheque + montosCuadrosTextoChequePostfechado;
    $('#montoNo_depositado').text("₡ " + addCommas(montoNoDepositado.toFixed(2).toString()));
}

function exportarExcelSeleccionados() {
    const idsArray = Object.keys(selectedIds).filter(id => selectedIds[id]);
    if (!idsArray.length) {
        toastr.warning('Seleccione al menos un registro para exportar.', 'Alerta');
        return;
    }
    const ids = idsArray.join(',');

    const data = {
        ids,
        fechaIni: fechaInicioFiltro,
        fechaFin: fechaFinFiltro,
        cliente: getCliente(),
        cobrador: getCobrador(),
        moneda: monedaFiltro,
        tipoDoc: tipDocFiltro,
        subTipo: subtipoFiltro,
        estado: estadoFiltro
    };

    $.post(`${URLP}/GenerarExcelRecibosSeleccionados`, data, function (ruta) {
        window.location = `${URLP}/DescargarArchivo?ruta=${encodeURIComponent(ruta)}`;
    });
}


// Abrir modal al pulsar "Liquidar"
$(document).on('click', '#btnLiquidar', function (e) {
    e.preventDefault();

    // Reglas mínimas
    if (!puedeLiquidar()) {
        toastr.warning('Seleccione al menos un documento y un cobrador.', 'Alerta');
        return;
    }

    // Prepara hidden/validaciones previas
    aprobar();

    // Mostrar el modal
    const el = document.getElementById('CrearTefModal');
    if (!el) {
        toastr.error('No se encontró el modal #CrearTefModal en el DOM.', 'Error');
        return;
    }
    try {
        if (window.bootstrap?.Modal) {                           // Bootstrap 5
            const modal = bootstrap.Modal.getOrCreateInstance(el);
            modal.show();
        } else {                                                 // Bootstrap 4
            $('#CrearTefModal').modal('show');
        }
    } catch (err) {
        console.error(err);
        toastr.error('No se pudo abrir el modal.', 'Error');
    }
});


/* Muestra/oculta el botón flotante de “Liquidar” según la selección y el cobrador */
function refrescarBotonLiquidar() {
    // Reglas: al menos 1 documento seleccionado y cobrador elegido en el header
    const puede = puedeLiquidar && typeof puedeLiquidar === 'function' ? puedeLiquidar() : false;

    // Botones flotantes (clase .multi) y, si existen, botones específicos
    const $multi = $('.multi');
    const $btnLiquidar = $('#btnLiquidar');      // opcional, si lo tienes
    const $btnAprobar = $('#btnAprobar');       // opcional, si lo tienes

    if (puede) {
        if ($multi.length) $multi.removeClass('d-none');
        if ($btnLiquidar.length) $btnLiquidar.prop('disabled', false).removeClass('disabled');
        if ($btnAprobar.length) $btnAprobar.prop('disabled', false).removeClass('disabled');
    } else {
        if ($multi.length) $multi.addClass('d-none');
        if ($btnLiquidar.length) $btnLiquidar.prop('disabled', true).addClass('disabled');
        if ($btnAprobar.length) $btnAprobar.prop('disabled', true).addClass('disabled');
    }
}



/* -------------------------------------------
 * 7) Ready
 * ------------------------------------------- */
$(document).ready(function () {
    // Si no existieran los hidden en el DOM, créalos (defensivo)
    if (!$('#clienteHeader').length)  $('<input type="hidden" id="clienteHeader">').appendTo('body');
    if (!$('#cobradorHeader').length) $('<input type="hidden" id="cobradorHeader">').appendTo('body');
    if (!$('#vendedorHeader').length) $('<input type="hidden" id="vendedorHeader">').appendTo('body');

    // CORREGIDO: escuchar el select real del modal de depósito
    $('#bancoliquidacion').on('change', function () {
        const cuentaBanco = $(this).find('option:selected').data('cuenta');
        cuentaBancoGlobal = cuentaBanco || '';
        if (cuentaBanco) obtenerMoneda(cuentaBanco);
        else $("#monedaLiquidacion").val("");
    });

    refrescarBotonLiquidar();
    InicializarSelect();
    obtenerRangoFechas();
    ManaejarBotones();
    CargaFormaPagoLiquidacion();
    KTDatatablesServerSide.init();
    ObtenerDocumentosTotales();

    // Forzar una primera sync después de init (por si el layout rellenó más tarde)
    syncClienteHidden();
    syncCobradorHidden();
    syncVendedorHidden();

    $('.filtro').not('#vendedorFiltro, #cobradorFiltro')
        .on('change', () => {
            KTDatatablesServerSide.reset();
        });
});
