
let articuloBoni = $('#articulosBoni').val();
let nivelPrecioBoni = $('#nivelprecioBoni').val();

let casaApBoni = $("#casaBoni :selected").map((_, e) => e.value).get();
let concasaBoni = "";
let subCasaBoni = $("#SubCasaBoni :selected").map((_, e) => e.value).get();
let consubCasaBoni = "";
let currentNivel = '';
let Edi_Cliente_Boni = "";
let Edi_Articulo_Boni = "";
let Edi_Escala_Boni = "";
let Edi_Moneda_Boni = "";
let Edi_Version_Bonif = "";
let selArt = "";

var KTDatatablesBonificaciones = function () {
    var dt;

    var initDatatable = function () {
        dt = $("#miTabla").DataTable({
            searchDelay: 500,
            processing: true,
            scrollX: true,
            lengthMenu: [
                [10, 100, 200, 500, -1],
                [10, 100, 200, 500, 'Todos']
            ],
            pageLength: 100,
        
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },
            order: [],
            "bDestroy": true,
            destroy: true,
         
            ajax: {
                url: `${_urlBoni}/obtenerArticuloClienteBonificadosIndex`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                        d.articulo = articuloBoni,
                        d.nivelPrecio = nivelPrecioBoni,
                   
                        d.casa = concasaBoni,
                        d.subcasa = consubCasaBoni

                }
            },
            columns: [
           
                {
                    "data": null,
                    "defaultContent": ``
                },
                { "data": "NIVEL_PRECIO" },
                { "data": "ARTICULO_NORMAL" },
                { "data": "CASA" },
                { "data": "SUBCASA" },
                { "data": "CATEGORIA_ART" },
                { "data": "ARTICULO_BONIFICADO" },
                { "data": "ESCALA_BONIF" },
                { "data": "CANT_MINIMA" },
                { "data": "CANT_MAXIMA" },
                { "data": "FECHA_INICIAL" },
                { "data": "FECHA_FINAL" },
                { "data": "FACTOR_BONIF" },
                { "data": "UNIDADES_BONIF" },
                { "data": "Acciones" },
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: (data, type, row) => {
                        const key = `${row.NIVEL_PRECIO}|${row.COD_ARTICULO_NORMAL}`;
                                return `
                <div class="form-check form-check-sm form-check-custom form-check-solid">
                  <input class="form-check-input row-checkbox" type="checkbox" value="${key}" />
                </div>`;
                            }
                },
                {
                    targets: 2,
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.COD_ARTICULO_NORMAL} | ${row.ARTICULO_NORMAL}`
                    }
                },
                {
                    targets: 6,
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.COD_ART_BONIFICADO} | ${row.ARTICULO_BONIFICADO}`
                    }
                },
                {
                    targets: 7,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.ESCALA_BONIF}`
                    }
                },
                {
                    targets: 8,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.CANT_MINIMA.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 9,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.CANT_MAXIMA.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 12,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.FACTOR_BONIF.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 13,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.UNIDADES_BONIF.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 14,
                    orderable: false,
                    render: function (data, type, row) {
                        const permisos = PERMISOS_MOD_DESCUENTOS.split(',');
                        const tienePermiso72 = permisos.includes("74");

                        if (!tienePermiso72) return '';

                        return `<a href="javascript:;"  onclick="abrirModalBonificacion('${row.NIVEL_PRECIO}','${row.COD_ARTICULO_NORMAL}','${row.ARTICULO_NORMAL}','${row.CANT_MINIMA}','${row.CANT_MAXIMA}','${row.UNIDADES_BONIF}','${row.FACTOR_BONIF}','${row.FECHA_INICIAL}','${row.FECHA_FINAL}','${row.ESCALA_BONIF}','${row.MONEDA}','${row.VERSION_BONIF}', '${row.COD_ART_BONIFICADO}','${row.ARTICULO_BONIFICADO}')" class="btn btn-sm btn-clean btn-icon me-2" title="Editar Precio">
								<span class="svg-icon svg-icon-warning svg-icon-2x"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-05-14-112058/theme/html/demo1/dist/../src/media/svg/icons/General/Settings-2.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
									<g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
										<rect x="0" y="0" width="24" height="24"/>
										<path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
										<rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>
									</g>
								</svg><!--end::Svg Icon--></span>
							</a>
                            `
                    }
                },
            ],
           
        });
        selArt = wireMultiSelect(dt, {
            checkAllSelector: '#checkAll',
            rowCheckboxSelector: '.form-check-input.row-checkbox',
            btnMultiSelector: '.app-engage-btn.multi',
            rowKeyFn: row => `${row.NIVEL_PRECIO}|${row.COD_ARTICULO_NORMAL}`
        });
        KTDatatablesBonificaciones.dt = dt;


      
    }



    var Filtros = function () {
        $('#kt_datatable_search_query_Boni').on('keyup', function () {
            dt.search($(this).val()).draw();
        });
    }

    var ListenerCheckboxes = () => {
        const container = document.querySelector('#miTabla');
        const checkboxes = container.querySelectorAll('[type="checkbox"]');

        checkboxes.forEach(c => {
            c.addEventListener('click', function () {
                setTimeout(function () {
                    ManejarCantidadCheckboxes();
                }, 50);
            });
        });
    }
    function rowKey(row) {
        return `${row.NIVEL_PRECIO}|${row.COD_ARTICULO_NORMAL}`;
    }
   
    const reloadData = () => {
        selArt.clear();                    

        if (KTDatatablesBonificaciones.dt) {
            KTDatatablesBonificaciones.dt.ajax.reload(null, false);
        }
    };

    return {
        init: function () {
            initDatatable();
            Filtros();
            ListenerCheckboxes();
        },
        reload: reloadData
    };
}();

function abrirModalBonificacion(
    NIVEL_PRECIO, COD_ARTICULO_NORMAL, ARTICULO_NORMAL,
    CANT_MINIMA, CANT_MAXIMA,
    UNIDADES_BONIF, FACTOR_BONIF,
    FECHA_INICIAL, FECHA_FINAL,
    ESCALA_BONIF, MONEDA, VERSION_BONIF,
    COD_ART_BONIFICADO, ARTICULO_BONIFICADO) {
    const modoNuevo = (arguments.length === 0);

    if (modoNuevo) {
        // valores por defecto
        const hoy = moment().format('YYYY-MM-DD');
        const maniana = moment().add(1, 'day').format('YYYY-MM-DD');

        // nivel seleccionado en el filtro
        NIVEL_PRECIO = $('#nivelprecioBoni').val() || '';

        // contamos las claves seleccionadas
        const cnt = selArt.getSelectedKeys().length;

        // construimos el texto
        ARTICULO_NORMAL = `${cnt} artículo${cnt === 1 ? '' : 's'}`;
        COD_ARTICULO_NORMAL = '';
        COD_ART_BONIFICADO = '';
        CANT_MINIMA = 1;
        CANT_MAXIMA = 2;
        UNIDADES_BONIF = 1;
        FACTOR_BONIF = 1;
        FECHA_INICIAL = hoy;
        FECHA_FINAL = maniana;
        ESCALA_BONIF = 1;
        MONEDA = '';
        VERSION_BONIF = 1;
    }

    $('#NivelPrecioBonificacion').val(NIVEL_PRECIO);
    $('#ArticuloBonificacion').val(
        modoNuevo ? `Artículos a afectar (${ARTICULO_NORMAL})`
            : `${COD_ARTICULO_NORMAL} | ${ARTICULO_NORMAL}`
    );
    $('#minimoBoni').val(CANT_MINIMA);
    $('#maximoBoni').val(CANT_MAXIMA);
    $('#abonificarBoni').val(UNIDADES_BONIF);
    $('#factorBoni').val(FACTOR_BONIF);
    $('#fechainiBoni').val(FECHA_INICIAL);
    $('#fechafinBoni').val(FECHA_FINAL);

    Edi_Moneda_Boni = MONEDA || '';
    Edi_Version_Bonif = VERSION_BONIF || 1;
    Edi_Articulo_Boni = COD_ARTICULO_NORMAL || '';  
    Edi_Escala_Boni = ESCALA_BONIF || 1;
    currentNivel = NIVEL_PRECIO;                

    const $sel = $('#articuloBonifSelect');
    $sel.val(null).trigger('change');                
    $sel.attr('data-placeholder', 'Seleccione artículo a bonificar');

    if (!modoNuevo) {     
        const codigoSel = COD_ART_BONIFICADO || COD_ARTICULO_NORMAL;
        const textoSel = (COD_ART_BONIFICADO)
            ? `${COD_ART_BONIFICADO} | ${ARTICULO_BONIFICADO}`
            : `${COD_ARTICULO_NORMAL} | ${ARTICULO_NORMAL}`;

        if (!$sel.find(`option[value='${codigoSel}']`).length) {
            $sel.append(new Option(textoSel, codigoSel, true, true));
        }
        $sel.val(codigoSel).trigger('change');
    }

    $("#Edi_ModalBonificacion").modal("show");
}


function guardarBonificacion() {

    const nivelPrecio = $('#NivelPrecioBonificacion').val().trim();
    const cantMin = +$('#minimoBoni').val();
    const cantMax = +$('#maximoBoni').val();
    const undBonificar = +$('#abonificarBoni').val();
    const factor = +$('#factorBoni').val();
    const articuloBonif = $('#articuloBonifSelect').val();

    const fechaIni = moment($('#fechainiBoni').val().trim(), ['YYYY-MM-DD', 'DD/MM/YYYY'], true);
    const fechaFin = moment($('#fechafinBoni').val().trim(), ['YYYY-MM-DD', 'DD/MM/YYYY'], true);

    if (!articuloBonif) { toastr.warning('Debe seleccionar el artículo a bonificar', 'Campo requerido', toastrOptions); return; }
    if (!fechaIni.isValid() || !fechaFin.isValid() || fechaIni.isAfter(fechaFin)) {
        toastr.warning('La fecha de inicio no puede ser mayor que la fecha final', 'Error', toastrOptions); return;
    }
    if (!validarEnteroEnRango(cantMin, 'cantidad mínima', 1, Number.MAX_SAFE_INTEGER)) return;
    if (!validarEnteroEnRango(cantMax, 'cantidad máxima', 1, Number.MAX_SAFE_INTEGER)) return;
    if (!validarRangoCantidades(cantMin, cantMax)) return;
    if (!validarEnteroEnRango(undBonificar, 'unidades a bonificar', 1, Number.MAX_SAFE_INTEGER)) return;
    if (!validarEnteroEnRango(factor, 'factor', 1, Number.MAX_SAFE_INTEGER)) return;

    const common = {
        nivelPrecio: nivelPrecio,
        articuloBonif: articuloBonif,
        cantidaMin: cantMin,
        cantidaMax: cantMax,
        UndBonificar: undBonificar,
        factor: factor,
        fechaIni: fechaIni.format('YYYY-MM-DD'),
        fechaFin: fechaFin.format('YYYY-MM-DD'),
        escala: Edi_Escala_Boni,
        moneda: 'L',
        version_bonif: 1
    };
    const keys = selArt.getSelectedKeys();
    const esMasivo = keys.length > 1;
    let url, data;
    if (esMasivo) {
        const listaArticulos = keys
            .map(k => k.split('|')[1])
            .join(',');

        url = `${_urlBoni}/EditarBonificacionMasiva`;
        data = { ...common, listaArticulos };
    } else {
        url = `${_urlBoni}/EditarArticuloBonificacion`;
        data = { ...common, articulo: Edi_Articulo_Boni };
    }

    const ejecutarPeticion = () => {
        Dajax(url, 'POST', data, function (resp) {
            if (resp.responseJSON == "1") {
                toastr.success('Bonificación actualizada con éxito', 'Éxito', toastrOptions);
            } else {
                toastr.warning('Hubo un error actualizando la Bonificación, inténtelo nuevamente', 'Error', toastrOptions);
            }
            selArt.clear();
         
            $('#miTabla').DataTable().ajax.reload(null, false);
        });
        $('#Edi_ModalBonificacion').modal('hide');
    };

    if (esMasivo) {
        Swal.fire({
            title: '¿Continuar?',
            html: `Vas a actualizar <b>${keys.length} artículos</b>.<br>¿Deseas continuar?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, actualizar',
            cancelButtonText: 'Cancelar'
        }).then(r => { if (r.isConfirmed) ejecutarPeticion(); });
    } else {
        ejecutarPeticion();
    }
    selArt.clear();
}


function openUploadModal() {
    $("#uploadModal").modal("show")
}

function closeUploadModal() {
    $("#uploadModal").modal("hide")
    let botonProcesar = document.getElementById('bt_ProcesarBonificacion');
    botonProcesar.disabled = false;

    var inputFile = document.getElementById('excelFile');
    inputFile.value = '';

    var descripcionField = document.getElementById('descripcionFile');
    descripcionField.value = ''; 

}





function ManejarFiltros() {
    

    $('#articulosBoni').on('change', function () {
        ActualizarFiltros();
    });
    $('#nivelprecioBoni').on('change', function () {
        ActualizarFiltros();
    });

    $('#nivelprecioBoni').on('change', function () {
        const nivel = $(this).val();
        cargarArticulosPorNivel(nivel);
    });
    $('#casaBoni').on('change', function () {
        var selectedValues = $(this).val();
        ActualizarFiltros();
    });
    $('#SubCasaBoni').on('change', function () {
        var selectedValues = $(this).val();
        ActualizarFiltros();
    });
}
function ActualizarFiltros() {
    concasaBoni = $("#casaBoni :selected").map((_, e) => e.value).get().join(',');
    consubCasaBoni = $("#SubCasaBoni :selected").map((_, e) => e.value).get().join(',');
    nivelPrecioBoni = $('#nivelprecioBoni').val();
    articuloBoni = $('#articulosBoni').val();
    selArt.clear();  
    KTDatatablesBonificaciones.reload();
}

function cargarArticulosBonifSelect(nivel) {
    $('#articuloBonifSelect').empty();               
    $('#articuloBonifSelect').append(
        $('<option>', { value: '', text: 'Cargando…' })
    );

    $.getJSON(`${_urlAp}/ObtenerArticulosPorNivel`, { nivelPrecio: nivel })
        .done(function (data) {
            $('#articuloBonifSelect').empty();
            $('#articuloBonifSelect').append(
                $('<option>', { value: '', text: 'Seleccione un artículo…' })
            );
            data.forEach(it => {
                $('#articuloBonifSelect').append(
                    $('<option>', {
                        value: it.ARTICULO,
                        text: `${it.ARTICULO} | ${it.DESCRIPCION}`
                    })
                );
            });
        });
}


function IniciarFechas() {
    createDatePicker("fechainiBoni");
    createDatePicker("fechafinBoni");
}
function cargarArticulosPorNivel(nivelPrecio = '0') {
    cargarSelectPorAjax({
        selector: '#articulosBoni',
        url: `${_urlAp}/ObtenerArticulosPorNivel`,
        params: { nivelPrecio }
    });
}

function initSelectArticuloBonif() {
    $('#articuloBonifSelect').select2({
        dropdownParent: $('#Edi_ModalBonificacion'),
        placeholder: 'Buscar artículo a bonificar',
        allowClear: false,
        minimumInputLength: 1,

        language: {
            inputTooShort: args => {
                const falta = 1 - args.input.length;
                return `Ingrese ${falta} caracter${falta === 1 ? '' : 'es'} más`;
            },
            noResults: () => 'Sin resultados',
            searching: () => 'Buscando…',
            errorLoading: () => 'No se pudieron cargar los resultados'
        },

        ajax: {
            url: `${_urlAp}/ObtenerArticulosPorNivel`,
            delay: 300,
            dataType: 'json',
            data: params => ({
                nivelPrecio: currentNivel,
                term: params.term || ''
            }),
            cache: true,
            processResults: (data, params) => {
                const term = (params.term || '').toLowerCase();
                const filtrados = !term
                    ? data
                    : data.filter(it =>
                        it.ARTICULO.toLowerCase().includes(term) ||
                        it.DESCRIPCION.toLowerCase().includes(term)
                    );

                return {
                    results: filtrados.map(it => ({
                        id: it.ARTICULO,
                        text: `${it.ARTICULO} | ${it.DESCRIPCION}`
                    }))
                };
            }
        }
    });
}


$(document).ready(function () {
    IniciarFechas();
    ManejarFiltros();
    const nivelInicial = $('#nivelprecioBoni').val();
    cargarArticulosPorNivel(nivelInicial);
    inicializarSelectConBusquedaYTodos('#articulosBoni', 'Filtrar por un artículo');
    inicializarSelectMultipleConTodos('#casaBoni', 'Filtrar por Casa');
    inicializarSelectMultipleConTodos('#SubCasaBoni', 'Filtrar por SubCasa');
    initSelectArticuloBonif();
    KTDatatablesBonificaciones.init()
});