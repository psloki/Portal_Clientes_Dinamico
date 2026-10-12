var totaltef = 0;
var checkedNodes = [];
var cantparciales = 0;
var factscobrar = [];
var montoselec = 0;
var montoyaproyectado = 0;
var saldodeNC = 0;
let BADGE_TIPO = {};

function formatDollar(num, symbol) {
    var isNegative = false;
    if (num < 0) {
        isNegative = true;
        num = Math.abs(num);
    }
    var p = num.toString().split(".");
    var formattedNum = symbol + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
        return num === "-" ? acc : num + (i && !(i % 3) ? "," : "") + acc;
    }, "") + (p[1] === undefined || p[1] === null ? "" : "." + p[1]);
    return isNegative ? "-" + formattedNum : formattedNum;
}

function parseDMY(s) {
    if (!s) return null;
    var parts = s.split('/');
    if (parts.length !== 3) return null;
    var d = parts[0], m = parts[1], y = parts[2];
    return new Date(y + '-' + m + '-' + d + 'T00:00:00');
}

var KTDatatablesServerSide = (function () {
    // Shared variables
    var table;
    var dt; // instancia de DataTable
    var isReady = false;

    // Private functions
    var initDatatable = function () {
        dt = $("#miTabla").DataTable({
            searchDelay: 500,
            processing: true,
            stateSave: false,
            scrollX: true,
            responsive: false,
            pageLength: 100,
            language: {
                lengthMenu: "Mostrando _MENU_ registros por página",
                zeroRecords: "No se encontraron registros",
                info: "Mostrando página _PAGE_ de _PAGES_",
                infoEmpty: "No se encontraron registros",
                infoFiltered: "(de un total de _MAX_ registros)"
            },
            ajax: {
                url: `${Aprobacion_URL}/ObtenerCuentasXCobrar`,
                type: 'GET',
                dataType: 'json',
                dataSrc: '',
                // contentType: 'application/json', // GET sin body
                data: () => {
                    const tiposSel = [].concat($('#tipo_cxc').val() || []);
                    const subtiposSel = [].concat($('#subtipo_cxc').val() || []);

                    const tipoArr = tiposSel.filter(v => v !== '0');
                    const subtipoArr = subtiposSel.filter(v => v !== '0');

                    const tipo = tipoArr.length ? tipoArr.join(',') : null;
                    const subtipo = subtipoArr.length ? subtipoArr.join(',') : null;

                    const fechas = $('#rangoFechas').val().split(' - ');
                    const [ini, fin] = fechas.length === 2
                        ? fechas.map(d => d.split('/').reverse().join('-'))
                        : [null, null];

                    const docVal = $('#documento_cxc').val(); // puede ser "0"

                    return {
                        fechaInicio: ini,
                        fechaFin: fin,
                        documento: (docVal && docVal !== '0') ? docVal : null,
                        tipo,
                        subtipo,
                        vendedor: $('#vendedorFiltro').val() || null
                    };
                }
            },
            columns: [
                { data: "TIPO" },
                { data: "DOCUMENTO_DEBITO" },
                { data: "SUBTIPO" },
                { data: "FECHA_DOCUMENTO_STR" },
                { data: "FECHA_VENCE_STR" },
                { data: "NOMBRE_VENDEDOR" },
                { data: "DIAS_NETO" },
                { data: "MONTO_STR" },
                { data: "MONTO_PAGO_STR" },
                { data: "SALDO_STR" }
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: (_, __, row) => {
                        const key = (row.TIPO || '').trim().toUpperCase();
                        const info = BADGE_TIPO[key];
                        const badge = info ?? { title: key, class: 'badge-light-secondary' };
                        return `<span class="badge ${badge.class} badge-lg">${badge.title}</span>`;
                    }
                },
                {
                    targets: 2,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<span class="badge badge-secondary badge-lg">${row.SUBTIPO}</span>`;
                    }
                },
                {
                    targets: [7, 8],
                    className: "dt-body-right"
                },
                {
                    targets: 9,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        const hoy = new Date();
                        const vence = row.FECHA_VENCE
                            ? new Date(row.FECHA_VENCE)
                            : parseDMY(row.FECHA_VENCE_STR);
                        const vencido = vence && !isNaN(vence) && (vence < hoy);
                        const cls = vencido ? 'text-danger' : 'text-primary';
                        return `<span class="${cls}">${row.SALDO_STR}</span>`;
                    }
                }
            ]
        });

        table = dt.$;
        isReady = true;

        $('#kt_datatable_search_query').off('keyup').on('keyup', function () {
            if (!dt) return;
            dt.search(this.value).draw();
        });

        dt.on('draw', function () {
            // hook draw si lo necesitas
        });
    };

    // Checkbox "checkAll"
    $('#checkAll').off('click').on('click', function () {
        var isChecked = $("#checkAll").prop('checked');
        if (isChecked) {
            $(".multi").removeClass("d-none");
        } else {
            $(".multi").addClass("d-none");
        }
    });

    // Clicks dentro de la tabla (proteger si dt no está aún)
    $('#miTabla').off('click', 'input[type="checkbox"]').on('click', 'input[type="checkbox"]', function () {
        if (!dt) return;
        var allChecked = dt.column(0).nodes().to$().find('input[type="checkbox"]:checked').length === dt.data().length;
        $('#checkAll').prop('checked', allChecked);
    });

    // Public methods
    return {
        init() { initDatatable(); },
        reload() {
            if (!dt || !isReady) return; // evita errores si aún no está listo
            try {
                dt.ajax.reload(null, false);
            } catch (e) {
                // fallback ultra seguro
                try { dt.draw(false); } catch (_) { }
            }
        }
        // fullReload opcional:
        // fullReload() { if (dt) { dt.destroy(); } isReady = false; this.init(); }
    };
})();

// ÚNICO hook de cambios (usa #vendedorFiltro, no #vendedor_cxc)
$('#rangoFechas, #vendedorFiltro, #tipo_cxc, #subtipo_cxc, #documento_cxc')
    .off('change')
    .on('change', () => KTDatatablesServerSide.reload());

function ManejarFiltrosBotones() {
    $('#rangoFechas').off('change').on('change', () => {
        KTDatatablesServerSide.reload();
    });
    // Si tu HTML tenía #vendedor_cxc, cámbialo por #vendedorFiltro en la vista.
    $('#vendedorFiltro').off('change').on('change', () => {
        KTDatatablesServerSide.reload();
    });
}

function comentario() {
    $("#modalcomentario").modal("show");
}

function GuardarComentario() {
    $("#modalcomentario").modal("hide");
    toastr.success(`Comentario guardado con éxito`, "Éxito", toastrOptions);
}

function ModalCorreos() {
    $("#modalcorreos").modal("show");
}

function EnviarCorreo() {
    toastr.success("Comprobante enviado con éxito con éxito", 'Éxito', toastrOptions);
    $("#modalcorreos").modal("hide");
}

function inicializarSelectDocumento() {
    $('#documento_cxc')
        .empty()
        .append('<option></option>')
        .select2({
            placeholder: 'Filtrar por documento',
            allowClear: true,
            width: '100%'
        });
}

function cargarTiposDocumento() {
    return $.getJSON(`${Aprobacion_URL}/ObtenerTiposDocumento`, function (filas) {
        const $sel = $('#tipo_cxc').empty()
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
    }).fail(function (xhr) {
        // Si 404/500, no rompas la UI
        console.warn('ObtenerTiposDocumento falló:', xhr.status, xhr.statusText);
        $('#tipo_cxc').empty()
            .append('<option value="0" selected>Todos</option>')
            .trigger('change.select2');
        BADGE_TIPO = {};
    });
}

function cargarSubtiposDocumento(tipoParam) {
    const qs = tipoParam ? `?tipo=${encodeURIComponent(tipoParam)}` : "";
    return $.getJSON(`${Aprobacion_URL}/ObtenerSubtiposDocumento${qs}`, function (payload) {
        const $sub = $("#subtipo_cxc")
            .empty()
            .append('<option value="0" selected>Todos</option>');

        (payload || []).forEach(item => {
            const text = item.label || item.descripcion || item.subtipo;
            $sub.append(`<option value="${item.subtipo}">${text}</option>`);
        });

        $sub.trigger("change.select2");
    }).fail(function (xhr) {
        console.warn('ObtenerSubtiposDocumento falló:', xhr.status, xhr.statusText);
        $("#subtipo_cxc")
            .empty()
            .append('<option value="0" selected>Todos</option>')
            .trigger("change.select2");
    });
}

function cargarDocumentosPorFecha() {
    const rango = $('#rangoFechas').val() || '';
    const parts = rango.split(' - ');
    const [ini, fin] = (parts.length === 2)
        ? parts.map(d => d.split('/').reverse().join('-'))
        : [null, null];

    const params = {
        fechaInicio: ini,
        fechaFin: fin,
        documento: '',
        tipo: '',
        subtipo: '',
        vendedor: ''
    };

    $.ajax({
        url: `${Aprobacion_URL}/ObtenerCuentasXCobrar`,
        method: 'GET',
        data: params,
        cache: false,
        success: function (res) {
            const docs = [...new Set((res || []).map(r => r.DOCUMENTO_DEBITO))].sort();
            const $sel = $('#documento_cxc')
                .empty()
                .append('<option></option>')
                .append('<option value="0">Todos</option>');
            docs.forEach(doc => {
                $sel.append(`<option value="${doc}">${doc}</option>`);
            });
            $sel.trigger('change.select2');
        },
        error: function (xhr) {
            console.warn('ObtenerCuentasXCobrar (doc por fecha) falló:', xhr.status, xhr.statusText);
            const $sel = $('#documento_cxc')
                .empty()
                .append('<option></option>')
                .append('<option value="0">Todos</option>');
            $sel.trigger('change.select2');
        }
    });
}

$('#rangoFechas').off('apply.daterangepicker').on('apply.daterangepicker', function () {
    cargarDocumentosPorFecha();
    KTDatatablesServerSide.reload();
});

$(document).ready(function () {
    inicializarSelectMultipleConTodos('#tipo_cxc', 'Tipo de documento');
    inicializarSelectMultipleConTodos('#subtipo_cxc', 'Forma de pago | Subtipo');

    $('#tipo_cxc').on('change', function () {
        const selected = $(this).val() || [];
        const tipoParam = selected.filter(v => v !== '0').join(',') || null;
        cargarSubtiposDocumento(tipoParam).done(() => KTDatatablesServerSide.reload());
    });

    $('#subtipo_cxc, #rangoFechas, #documento_cxc').on('change', () => KTDatatablesServerSide.reload());
    $('#vendedorFiltro').on('change', () => KTDatatablesServerSide.reload());

    $(document).off('vendedores-actualizados').on('vendedores-actualizados', () => KTDatatablesServerSide.reload());

    ManejarFiltrosBotones();

    cargarDocumentosPorFecha();
    cargarTiposDocumento()
        .done(() => cargarSubtiposDocumento(null))
        .always(() => {
            KTDatatablesServerSide.init();
        });

    $('#rangoFechas').daterangepicker({
        locale: {
            format: 'DD/MM/YYYY',
            applyLabel: 'Aplicar',
            cancelLabel: 'Cancelar',
            fromLabel: 'Desde',
            toLabel: 'Hasta',
            customRangeLabel: 'Rango personalizado',
            daysOfWeek: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            firstDay: 1
        }
    });
});
