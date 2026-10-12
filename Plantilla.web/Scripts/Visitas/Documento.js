'use strict';

$.fn.dataTable.ext.errMode = 'throw';

let BADGE_TIPO = {};

function obtenerRangoFechas() {
    const raw = ($('#rangoFechas').val() || '').trim();
    let fechaInicio, fechaFin;

    // 1. Definir el formateador una sola vez para que sea accesible en toda la funciÃ³n
    const formatToSQL = (dateObj) => {
        const pad = n => String(n).padStart(2, '0');
        return `${dateObj.getFullYear()}-${pad(dateObj.getMonth() + 1)}-${pad(dateObj.getDate())}`;
    };

    if (raw && raw.includes(' - ')) {
        const [a, b] = raw.split(' - ');

        // FunciÃ³n interna para parsear el string DD/MM/YYYY
        const parseDP = (s) => {
            const p = s.trim().split('/');
            return new Date(p[2], p[1] - 1, p[0]);
        };

        fechaInicio = formatToSQL(parseDP(a));
        fechaFin = formatToSQL(parseDP(b));
    } else {
        // Fallback robusto
        const today = new Date();
        const past = new Date();
        past.setDate(today.getDate() - 60);

        fechaInicio = formatToSQL(past);
        fechaFin = formatToSQL(today);
    }

    return { fechaInicio, fechaFin };
}

// Se inicializa explícitamente la impresora antes de cada copia.
// Anteriormente el tiquete dependía del estado previo de la impresora
// (fuente, tamaño o alineación), lo que podía provocar que un mismo
// comprobante se imprimiera con diferentes anchos y saltos de línea.
// function reimprimirTiquete(documento) {

//     const esMovil = KTUtil.isMobileDevice();
//     //const esAndroid = esMovil && /Android/i.test(navigator.userAgent);
//     const esAndroid = /Android/i.test(navigator.userAgent || '');

//     if (typeof KTApp !== 'undefined' && typeof KTApp.block === 'function') {
//         KTApp.block('body', {
//             overlayColor: '#000000',
//             state: 'primary',
//             message: 'Procesando tiquete...'
//         });
//     }

//     $.getJSON(
//         `${vurl}/ObtenerComprobanteAplicacion?documento=${encodeURIComponent(documento)}`,
//         (json) => {

//             if (typeof KTApp !== 'undefined' && typeof KTApp.unblock === 'function') {
//                 KTApp.unblock('body');
//             }

//             if (!json || (!json.tiqueteBase64 && !json.tiquete)) {
//                 toastr.warning(
//                     "No se pudo recuperar el contenido del tiquete.",
//                     "Aviso"
//                 );
//                 return;
//             }

//             let tiqueteLimpio = '';

//             if (json.tiqueteBase64) {
//                 try {
//                     tiqueteLimpio =
//                         decodeURIComponent(
//                             escape(window.atob(json.tiqueteBase64))
//                         );
//                 } catch (e) {
//                     tiqueteLimpio = '';
//                 }
//             }

//             if (!tiqueteLimpio) {
//                 tiqueteLimpio =
//                     sanitizarTextoImpresora(
//                         (json && json.tiquete) || ''
//                     );
//             }

//             /*
//              * RawBT trabaja directamente con comandos ESC/POS.
//              * Se normalizan los saltos de línea antes de enviar
//              * el contenido a la impresora.
//              */
//             tiqueteLimpio =
//                 tiqueteLimpio.replace(/\r\n/g, '\n');

//             if (esAndroid) {

//                 /*
//                  * ESC @   = Inicializar impresora.
//                  * ESC M 1 = Fuente B.
//                  * GS ! 0  = Tamaño normal.
//                  * ESC a 0 = Alineación izquierda.
//                  *
//                  * Se fuerza el mismo estado antes de cada copia para
//                  * evitar que la impresión dependa de la configuración
//                  * dejada por un trabajo anterior.
//                  */
//                 const inicializar =
//                     "\x1B\x40" +
//                     "\x1B\x4D\x01" +
//                     "\x1D\x21\x00" +
//                     "\x1B\x61\x00";

//                 const corte =
//                     "\n\n\n\x1D\x56\x00";

//                 const contenidoFinal =
//                     inicializar +
//                     tiqueteLimpio +
//                     corte +
//                     inicializar +
//                     tiqueteLimpio +
//                     corte;

//                 const encoded =
//                     encodeURIComponent(contenidoFinal);

//                 const rawbtIntent =
//                     `intent://${encoded}` +
//                     `#Intent;scheme=rawbt;` +
//                     `package=ru.a402d.rawbtprinter;end`;

//                 window.location.href = rawbtIntent;

//             } else {

//                 // PC / iOS: utilizar la ventana normal de impresión.
//                 imprimirEnVentana(tiqueteLimpio, documento);
//             }
//         }
//     ).fail(() => {

//         if (typeof KTApp !== 'undefined' && typeof KTApp.unblock === 'function') {
//             KTApp.unblock('body');
//         }

//         toastr.error(
//             "Error de comunicación con el servidor",
//             "Error"
//         );
//     });
// }

function bloquearEnlaceDocumento(enlace, duracionMs) {
    if (!enlace) return function () { };
    if (enlace.dataset.accionBloqueada === 'true') return null;

    enlace.dataset.accionBloqueada = 'true';
    enlace.setAttribute('aria-disabled', 'true');
    enlace.style.pointerEvents = 'none';

    let liberado = false;
    const timeout = window.setTimeout(liberar, duracionMs);
    function liberar() {
        if (liberado) return;
        liberado = true;
        window.clearTimeout(timeout);
        delete enlace.dataset.accionBloqueada;
        enlace.removeAttribute('aria-disabled');
        enlace.style.pointerEvents = '';
    }
    return liberar;
}

function reimprimirTiquete(documento, enlace) {
    if (!documento) {
        toastr.warning(
            'No se pudo obtener el número del comprobante.',
            'Aviso'
        );
        return;
    }

    const liberar = bloquearEnlaceDocumento(enlace, 1200);
    if (enlace && !liberar) return;

    const baseUrl =
        typeof registroPagosUrl !== 'undefined' && registroPagosUrl
            ? registroPagosUrl
            : '/RegistroPagos';

    const url =
        `${baseUrl}/VerComprobantePagoPdf?documento=` +
        encodeURIComponent(documento);

    const ventana = window.open(url, '_blank');

    if (!ventana) {
        window.location.href = url;
    }

}


// function reimprimirTiquete(documento) {
//     const esMovil = KTUtil.isMobileDevice();
//     const esAndroid = esMovil && /Android/i.test(navigator.userAgent);

//     // ValidaciÃ³n segura para el bloqueo de pantalla
//     if (typeof KTApp !== 'undefined' && typeof KTApp.block === 'function') {
//         KTApp.block('body', {
//             overlayColor: '#000000',
//             state: 'primary',
//             message: 'Procesando tiquete...'
//         });
//     }

//     $.getJSON(`${vurl}/ObtenerComprobanteAplicacion?documento=${documento}`, (json) => {
//         // Desbloqueo seguro
//         if (typeof KTApp !== 'undefined' && typeof KTApp.unblock === 'function') {
//             KTApp.unblock('body');
//         }

//         if (!json || (!json.tiqueteBase64 && !json.tiquete)) {
//             toastr.warning("No se pudo recuperar el contenido del tiquete.", "Aviso");
//             return;
//         }

//         let tiqueteLimpio = '';
//         if (json.tiqueteBase64) {
//             try {
//                 tiqueteLimpio = decodeURIComponent(escape(window.atob(json.tiqueteBase64)));
//             } catch (e) {
//                 tiqueteLimpio = '';
//             }
//         }
//         if (!tiqueteLimpio) {
//             tiqueteLimpio = sanitizarTextoImpresora(json.tiquete);
//         }

//         if (isMobile) {
//             tiqueteLimpio = tiqueteLimpio.replace(/\r\n/g, '\n');
//             const corte = "\n\n\n\x1D\x56\x00";
//             const contenidoFinal = `${tiqueteLimpio}${corte}${tiqueteLimpio}${corte}`;
//             const encoded = encodeURIComponent(contenidoFinal);
//             const rawbtIntent = `intent://${encoded}#Intent;scheme=rawbt;package=ru.a402d.rawbtprinter;end`;
//             window.location.href = rawbtIntent;
//         } else {
//             // LÃ³gica PC / iOS (Ventana de impresiÃ³n)
//             imprimirEnVentana(tiqueteLimpio, documento);
//         }
//     }).fail(() => {
//         if (typeof KTApp !== 'undefined' && typeof KTApp.unblock === 'function') {
//             KTApp.unblock('body');
//         }
//         toastr.error("Error de comunicaciÃ³n con el servidor", "Error");
//     });
// }

function descargarDocumentoPdf(documento, enlace) {
    if (!documento) {
        toastr.warning("No se pudo obtener el documento para descargar.", "Aviso");
        return;
    }

    const liberar = bloquearEnlaceDocumento(enlace, 3000);
    if (enlace && !liberar) return;

    mostrarLoadingDescargaDocumento();
    const baseUrl = typeof registroPagosUrl !== 'undefined' ? registroPagosUrl : '/RegistroPagos';
    window.location.href = `${baseUrl}/DescargarComprobantePagoPdf?documento=${encodeURIComponent(documento)}`;
    setTimeout(function () {
        ocultarLoadingDescargaDocumento();
    }, 3000);
}

function mostrarLoadingDescargaDocumento() {
    if (typeof KTApp !== 'undefined' && typeof KTApp.block === 'function') {
        KTApp.block('body', {
            overlayColor: '#000000',
            state: 'primary',
            message: 'Generando documento...'
        });
    }
}

function ocultarLoadingDescargaDocumento() {
    if (typeof KTApp !== 'undefined' && typeof KTApp.unblock === 'function') {
        KTApp.unblock('body');
    }
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
// FunciÃ³n auxiliar para la impresion y configuracion de la hoja
function imprimirEnVentana(contenido, doc) {
    const ventana = window.open('', '_blank', 'width=450,height=800');

    ventana.document.write(`
        <html>
        <head>
            <title>Comprobante - ${doc}</title>
            <style>
                /* 1. Eliminar encabezados y pies de pÃ¡gina del navegador */
                @page { 
                    margin: 0; 
                }
                
                body { 
                    font-family: 'Courier New', Courier, monospace; 
                    font-size: 12px; 
                    margin: 0;
                    padding: 20px; /* Margen interno para que no pegue al borde fÃ­sico */
                    width: 300px;  /* Ancho estÃ¡ndar de tiquete tÃ©rmico */
                }

                pre { 
                    white-space: pre-wrap; 
                    word-wrap: break-word; 
                    margin: 0; 
                }

                /* 2. Forzar que no se imprima nada extra */
                @media print {
                    header, footer, .no-print { display: none !important; }
                }
            </style>
        </head>
        <body>
            <pre>${contenido}</pre>
            <script>
                window.onload = function() { 
                    // PequeÃ±o delay para asegurar que el CSS se aplique
                    setTimeout(function() {
                        window.print(); 
                        window.close();
                    }, 500);
                };
            <\/script>
        </body>
        </html>
    `);
    ventana.document.close();
}
// mostrar o ocultar los filtros cuando no hay cliente seleccionado
function toggleFiltrosSecundarios(habilitar) {
    const $tipoDoc = $('#tipo_documento');
    const $rangoFechas = $('#rangoFechas');

    if (habilitar) {
        $tipoDoc.prop('disabled', false);
        $rangoFechas.prop('disabled', false).removeClass('bg-secondary').css('cursor', 'pointer');
    } else {
        $tipoDoc.prop('disabled', true);
        $rangoFechas.prop('disabled', true).addClass('bg-secondary').css('cursor', 'not-allowed');
    }

    // Select2 requiere trigger para actualizar el estado visual
    if ($tipoDoc.data('select2')) {
        $tipoDoc.trigger('change.select2');
    }
}

function reloadDocs() {
    const $table = $('#kt_datatabla_documentos');
    if ($.fn.dataTable.isDataTable('#kt_datatabla_documentos')) {
        const dt = $table.DataTable();

        // Limpiar selecciÃ³n batch si existe
        if (typeof selectedIds !== 'undefined') {
            Object.keys(selectedIds).forEach(k => delete selectedIds[k]);
        }

        // reload(callback, resetPaging)
        dt.ajax.reload(null, true);
    }
}

function pickLayoutVal(...selectors) {
    let fallback = null;
    for (const s of selectors) {
        const elements = Array.from(document.querySelectorAll(s));
        for (const el of elements) {
            const v = ($(el).val() || '').trim();
            if (v && v !== '0') return v;
            if (v && fallback === null) fallback = v;
        }
    }
    return fallback;
}

function syncClienteHidden() {
    const v = pickLayoutVal('#clientesFiltro', '#clientesFiltroMovil');
    const $hidden = $('#clienteHeader');
    const prev = ($hidden.val() || '').trim();

    // Si no hay valor y el anterior TAMPOCO tenÃ­a valor, no mandamos Toastr (evita spam al cargar)
    if (!v || v === '0') {
        if (prev !== '') { // Solo si antes habÃ­a un cliente y ahora no
            $hidden.val('');
            toggleFiltrosSecundarios(false);
            toastr.warning('Seleccione un cliente para consultar', 'Error', toastrOptions);
            if ($.fn.dataTable.isDataTable('#kt_datatabla_documentos')) {
                $('#kt_datatabla_documentos').DataTable().clear().draw();
            }
        }
        return;
    }

    if (prev !== v) {
        $hidden.val(v);
        toggleFiltrosSecundarios(true);
        if ($.fn.dataTable.isDataTable('#kt_datatabla_documentos')) {
            reloadDocs();
        } else {
           KTDatatablesServerSide.init();
        }
    }
}

const getCliente = () => ($('#clienteHeader').val() || '').trim();

let cliente = getCliente();

function cargarTiposDocumento() {
    return $.getJSON(`${vurl}/ObtenerTiposDocumento`, function (filas) {
        const $sel = $('#tipo_documento').empty()
            .append('<option value="0" selected>Todos</option>');

        BADGE_TIPO = {};

        (filas || []).forEach(f => {
            const key = (f.TIPO || '').trim().toUpperCase();
            if (key && !$sel.find(`option[value='${key}']`).length) {
                $sel.append(`<option value="${key}">${key}</option>`);
            }
            BADGE_TIPO[key] = { title: f.BADGE_TITLE, class: f.BADGE_CLASS };
        });

        $sel.trigger('change.select2');
    }).fail(function () {
        $('#tipo_documento').empty()
            .append('<option value="0" selected>Todos</option>')
            .trigger('change.select2');
        BADGE_TIPO = {};
    });
}

var KTDatatablesServerSide = function () {

    var dt;
    var initDatatable = function () {

        if ($.fn.DataTable.isDataTable('#kt_datatabla_documentos')) {
            dt.ajax.reload();
            return;
        }

        dt = $("#kt_datatabla_documentos").DataTable({

            responsive: isMobile,
            scrollX: true,
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
            order: [],
            ajax: {
                url: `${vurl}/Obtener_TablaDocumentos`,
                type: 'GET',
                data: function (d) {

                    const clienteHeader = $('#clienteHeader').val();
                    const { fechaInicio, fechaFin } = obtenerRangoFechas();
                    let arrayDoc = $('#tipo_documento').val() || ['0'];
                    const tipoDocumento = Array.isArray(arrayDoc) ? arrayDoc.join(',') : arrayDoc;

                    d.cliente = clienteHeader || '';
                    d.tipoDocumento = tipoDocumento || '0';
                    d.fechaInicio = fechaInicio || '';
                    d.fechaFin = fechaFin || '';

                    return d;
                },
                dataSrc: function (json) {
                    if (!json || (json.code && json.code < 0)) {
                        if (json.message) toastr.warning(json.message, "Advertencia", toastrOptions);
                        return [];
                    }
                    return json;
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error Obtener_TablaDocumentos:", error);
                }
            },
            columns: [
                { "data": null },
                { "data": "TIPO" },
                { "data": "DOCUMENTO" },
                { "data": "SUBTIPO" },
                { "data": "REFERENCIA_DEBITO" },
                { "data": "FECHA_DOCUMENTO" },
                { "data": "FECHA_VENCE" },
                { "data": "DIAS_NETO" },
                { "data": "MONTO" },
                { "data": "MONTO_PAGO" },
                { "data": "SALDO" },
                { "data": "USUARIO_CREACION" },
                { "data": "FECHA_CREACION_DEBITO" },
                { "data": null }
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: function () {
                        return `<div class="form-check form-check-sm form-check-custom form-check-solid">
                                    <input class="form-check-input" type="checkbox"/>
                                </div>`;
                    }
                },
                {
                    targets: 1,
                    orderable: false,
                    render: function (data, type, row) {
                        const status = {
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

                        const t = status[row.TIPO];

                        if (!t) {
                            return `<span class="badge badge-light badge-lg">${row.TIPO || ''}</span>`;
                        }

                        return `<span class="badge ${t.class} badge-lg">${t.title}</span>`;
                    }
                },
                {
                    targets: 7,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.DIAS_NETO}`;
                    }
                },
                {
                    targets: 8,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        const v = Number(row.MONTO || 0);
                        return `₡ ${v.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
                    }
                },
                {
                    targets: 9,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        const v = Number(row.MONTO_PAGO || 0);
                        return `₡ ${v.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
                    }
                },
                {
                    targets: 10,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        const fechaActual = new Date();
                        const fechaVence = new Date(row.FECHA_VENCE);
                        const v = Number(row.SALDO || 0);
                        const monto = `₡ ${v.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;

                        return fechaVence < fechaActual ? `<span class="text-danger">${monto}</span>` : `<span class="text-primary">${monto}</span>`;
                    }
                },
                {
                    targets: 13,
                    orderable: false,
                    render: function (data, type, row) {
                        let accionessubtablaCliente = "";

                        if (["DEP", "REC", "TEF"].includes(row.TIPO)) {
                            accionessubtablaCliente = `<a title="Imprimir comprobante" onclick="reimprimirTiquete('${row.DOCUMENTO}', this)" class="btn btn-sm btn-clean btn-icon me-2">
                                                            <i class="la la-print fs-2x"></i>
                                                        </a>
                                                        <a title="Descargar documento" onclick="descargarDocumentoPdf('${row.DOCUMENTO}', this)" class="btn btn-sm btn-clean btn-icon me-2">
                                                            <i class="la la-download fs-2x"></i>
                                                        </a>`;
                        }

                        return accionessubtablaCliente;
                    }
                }
            ]
        });

        dt.on('draw', function () {
            const container = document.querySelector('#kt_datatabla_documentos');
            const checkboxes = container.querySelectorAll('[type="checkbox"]');

            checkboxes.forEach(c => {
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
        if (dt) dt.ajax.reload();
    };

    return {
        init: function () {
            const val = $('#clienteHeader').val();
            if (val && val !== '') {
                // Si ya existe la tabla pero se llamÃ³ a init, hacemos reload
                if ($.fn.dataTable.isDataTable('#kt_datatabla_documentos')) {
                    dt.ajax.reload();
                } else {
                    initDatatable();
                }
            }
        },
        reset: function () {
            resetTable();
        }
    }

}();

// ==================== Event Listeners ====================
$(document).on('select2:select select2:clear', '#clientesFiltro, #clientesFiltroMovil', function () {
    syncClienteHidden();
});

// Mantener el change solo si no es provocado por select2
$(document).on('change', '#clientesFiltro, #clientesFiltroMovil', function (e) {
    if (e.originalEvent) { // Solo si es una interacciÃ³n real de usuario no-select2
        syncClienteHidden();
    }
});

// ==================== Document Ready ====================
// 1. Centralizar la obtenciÃ³n de valores de Layout
const getActiveLayoutValues = () => {
    syncClienteHidden();
    syncCobradorHidden();
    syncVendedorHidden();
};

$(document).ready(function () {
    if (!$('#clienteHeader').length) $('body').append('<input type="hidden" id="clienteHeader">');

    inicializarSelectMultipleConTodos('#tipo_documento', 'Tipo de documento');

    let tiposDocumentoListos = false;
    let cargaInicialFinalizada = false;

    function intentarCargaInicial() {
        if (!tiposDocumentoListos || cargaInicialFinalizada) return;

        const v = pickLayoutVal('#clientesFiltro', '#clientesFiltroMovil');
        if (v && v !== '0') {
            finalizarCargaInicial(v);
        } else {
            toggleFiltrosSecundarios(false);
        }
    }

    function finalizarCargaInicial(clienteId) {
        if (cargaInicialFinalizada) return;
        cargaInicialFinalizada = true;
        $('#clienteHeader').val(clienteId);
        toggleFiltrosSecundarios(true);
        KTDatatablesServerSide.init();
    }

    $(document).on('clientes-cargados-layout', function () {
        intentarCargaInicial();
    });

    cargarTiposDocumento().always(() => {
        tiposDocumentoListos = true;
        intentarCargaInicial();
    });

    // Filtro de Tipo de Documento (Select2)
    // Forzar la escucha en el componente de Tipo de Documento
    let filterTimeout;
    $(document).on('change', '#tipo_documento', function () {
        clearTimeout(filterTimeout);

        filterTimeout = setTimeout(() => {
            const clienteActivo = ($('#clienteHeader').val() || '').trim();
            const tiposSeleccionados = $(this).val();

            // 1. Validar Cliente Primero
            if (!clienteActivo || clienteActivo === '0') {
                toastr.warning('Seleccione un cliente para consultar antes de aplicar filtros', 'Error', toastrOptions);
                return;
            }

            // 2. Validar SelecciÃ³n de Tipos (MÃºltiple)
            if (!tiposSeleccionados || tiposSeleccionados.length === 0) {
                toastr.warning('Seleccione al menos un tipo de documento para filtrar', 'Error', toastrOptions);
                return;
            }

            // 3. Si ambos pasan, recargar DataTable
            if ($.fn.dataTable.isDataTable('#kt_datatabla_documentos')) {
                reloadDocs();
            }
        }, 250);
    });

    // Filtros de Fecha (DateRangePicker)
    // Nota: daterangepicker requiere el evento 'apply.daterangepicker'
    $('#rangoFechas').on('apply.daterangepicker', function (ev, picker) {
        const clienteActivo = ($('#clienteHeader').val() || '').trim();

        if (!clienteActivo || clienteActivo === '0') {
            toastr.warning('Seleccione un cliente para consultar antes de aplicar fechas', 'Error', toastrOptions);
            $(this).val('');
            return;
        }

        $('#fechaInicio').val(picker.startDate.format('YYYY-MM-DD'));
        $('#fechaFin').val(picker.endDate.format('YYYY-MM-DD'));
        reloadDocs();
    });

    $('#rangoFechas').on('cancel.daterangepicker', function () {
        $('#fechaInicio, #fechaFin').val('');
        $(this).val('');
        reloadDocs();
    });

    $('#rangoFechas').daterangepicker({
        locale: {
            format: 'DD/MM/YYYY',
            applyLabel: 'Aplicar',
            cancelLabel: 'Cancelar',
            customRangeLabel: 'Rango personalizado',
            daysOfWeek: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            firstDay: 1
        }
    });
});
