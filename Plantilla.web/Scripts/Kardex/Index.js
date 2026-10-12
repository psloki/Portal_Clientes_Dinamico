$(document).ready(function () {
    const params = new URLSearchParams(window.location.search);
    const articulo = params.get("articulo");
    const descripcion = params.get("descripcion");
    const bodega = params.get("bodega");

    if (articulo) {
        setTimeout(() => {
            let existe = $(`#FiltroArticulo option[value="${articulo}"]`).length > 0;

            if (!existe) {
                $('#FiltroArticulo').append(
                    `<option value="${articulo}">${articulo} - ${decodeURIComponent(descripcion || '')}</option>`
                );
                $('#FiltroBogeda').append(
                    `<option value="${bodega}">${bodega}</option>`
                );
            }

            $('#FiltroArticulo').val(articulo).trigger('change.select2');
            $('#FiltroBodega').val(bodega).trigger('change.select2');

            if (KTDataTable && typeof KTDataTable.reset === 'function') {
                KTDataTable.reset();
            }

            if (!$('#filtroAuto').length) {
                $('.card-body').first().prepend(info);
            }

        }, 500);
    }
});


var KTDataTable = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;
    var loadingOverlay = null;

    var toggleFullscreenLoader = function (show) {
        if (!loadingOverlay) {
            loadingOverlay = $('#kardex-loading-overlay');
        }

        if (!loadingOverlay.length) return;

        if (show) {
            loadingOverlay.removeClass('d-none');
        } else {
            loadingOverlay.addClass('d-none');
        }
    };

    // Private functions
    var initDatatable = function () {
        dt = $("#kt_datatable").DataTable({
            searchDelay: 1000,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
            
            processing: true,
            stateSave: true,
            serverSide: true,
            scrollX: true,
            order: [[3, 'desc']],
            //data: tefs,
            ajax: {
                url: `${KARDEX_URL}/ObtenerIndexKardex`,
                type: 'POST',
                data: function (d) {
                    d.fechaInicio = $("#FechaInicio").val().split(' - ')[0].split('/').reverse().join('-'),
                        d.fechaFin = $("#FechaInicio").val().split(' - ')[1].split('/').reverse().join('-'),
                        d.tipo = $('#FiltroTipo').val(),
                        d.bodega = $('#FiltroBodega').val(),
                        d.articulo = $('#FiltroArticulo').val(),
                        d.cliente = $('#cliente_cxc').val()

                },

                dataSrc: 'data'

            },
            columns: [
                {
                    "data": null,
                    orderable: false,
                    visible: false,
                    render: function (data, type, row) {
                        return ''
                    }
                },
                {
                    data: 'AUDIT_TRANS_INV',
                    width: 100
                },
                {
                    data: 'ARTICULO_COMPLETO',
                    width: 150
                },
                {
                    data: 'FECHA_STR',
                    width: 100
                },
                {
                    data: 'APLICACION',
                    width: 100
                },
                {
                    data: 'TIPO',
                    width: 50
                },
                {
                    data: 'NATURALEZA',
                    width: 50
                },
                {
                    data: 'ASIENTO',
                    width: 100
                },
                {
                    data: 'REFERENCIA',
                    width: 100
                },
                {
                    data: 'CLIENTE_NOMBRE',
                    width: 100
                },
                {
                    data: 'BODEGA',
                    width: 50
                },
                {
                    data: 'CANTIDAD_INT',
                    width: 50
                },
                //{
                //    data: 'COSTO_TOT_FISC_LOC',
                //    width: 50,
                //    render: (data) => {
                //        return formatearNumero2Digitos(data)
                //    }
                //},
                {
                    data: 'PRECIO_UNITARIO',
                    width: 50,
                    render: (data) => {
                        return formatearNumero2Digitos(data)
                    }
                }
            ],

            columnDefs: [


            ],

        });
        table = dt.$;

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        dt.on('draw', function () {

        });

        $('#kt_datatable').on('processing.dt', function (e, settings, processing) {
            toggleFullscreenLoader(processing);
        });

        $('#kt_datatable').on('xhr.dt error.dt', function () {
            toggleFullscreenLoader(false);
        });


    }
    // Add a click event handler to the "checkAll" checkbox in the header

    var resetTable = function () {
        //dt = $("#miTabla").DataTable();
        setTimeout(() => {
            dt.ajax.reload(null, true);
        }, 100)
    }

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


    // Public methods
    return {
        init: function () {
            initDatatable();

            Filtros();
        },
        reset: function () {
            resetTable();
        }
    }
}();



function ManejarFiltrosBotones() {
    $('#FechaInicio').on('change', () => {
        KTDataTable.reset()
    })
    $('#FiltroTipo').on('change', () => {
        KTDataTable.reset()
    })
    $('#FiltroBodega').on('change', () => {
        KTDataTable.reset()
    })
    $('#FiltroArticulo').on('change', () => {
        KTDataTable.reset()
    })
    $('#cliente_cxc').on('change', () => {
        KTDataTable.reset()
    })
}

$(document).ready(() => {
    $("#FechaInicio").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });
    KTDataTable.init()
    ManejarFiltrosBotones()

})
