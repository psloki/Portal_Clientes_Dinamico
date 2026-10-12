
let clienteBoni = $('#boniCli').val();
let articuloBoni = $('#boniArticuloCli').val();
let categoriaBoni = $('#boniCategoriaCli').val();

let casaApBoni = $("#boniCasaCli :selected").map((_, e) => e.value).get();
let concasaBoniCli = "";
let subCasaBoni = $("#boniSubCasaCli :selected").map((_, e) => e.value).get();
let consubCasaBoniCli = "";

let Edi_Cliente_Boni = "";
let Edi_Articulo_Boni = "";
let Edi_Escala_Boni = "";
let Edi_Articulo_BoniCli = "";
let Edi_Version_Bonif = "";



var KTDatatablesBonificacionesCliente = function () {
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
                url: `${_urlBoniCli}/obtenerArticuloBonificacionesClienteIndex`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                        d.cliente = clienteBoni,
                        d.articulo = articuloBoni,
                        d.categoria = categoriaBoni,
                        d.casa = concasaBoniCli,
                        d.subcasa = consubCasaBoniCli
                }
            },
            columns: [
                {
                    "data": "CLIENTE",
                    "defaultContent": ``
                },
                { "data": "CATEGORIA" },
                { "data": "CLIENTE" },
                { "data": "COD_ARTICULO" },
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
                    visible: true,
                    render: (data, type, row) => {
                        const key = `${row.CLIENTE}|${row.COD_ARTICULO}`;
                        return `
                      <div class="form-check form-check-sm form-check-custom form-check-solid">
                        <input class="form-check-input row-checkbox" 
                               type="checkbox" 
                               value="${key}" />
                      </div>`;
                    }
                },
                {
                    targets: 2,
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.CLIENTE} | ${row.NOMBRE}`
                    }
                },
                {
                    targets: 3,
                    className: "dt-body-left",
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.COD_ARTICULO} | ${row.ARTICULO_NOMBRE}`
                    }
                },
                {
                    targets: 8,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.ESCALA_BONIF.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 9,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.CANT_MINIMA.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 10,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.CANT_MAXIMA.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 11,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.FECHA_INICIAL;
                    }
                },
                {
                    targets: 12,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.FECHA_FINAL;
                    }
                },
                {
                    targets: 13,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.FACTOR_BONIF.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 14,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.UNIDADES_BONIF.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },

                {
                    targets: 15,
                    orderable: false,
                    render: function (data, type, row) {
                        const permisos = PERMISOS_MOD_DESCUENTOS.split(',');
                        const tienePermiso = permisos.includes("78");
                        function b64EncodeUnicode(str) {
                            return btoa(
                                encodeURIComponent(str).replace(/%([0-9A-F]{2})/g,
                                    (match, p1) => String.fromCharCode(parseInt(p1, 16))
                                )
                            );
                        }

                        const codArt = row.COD_ARTICULO;
                        const artNom = row.ARTICULO_NOMBRE;
                        const codBoni = row.COD_ART_BONIFICADO || codArt;
                        const artBoniRaw = row.ARTICULO_BONIFICADO;
                        const artBoniNom = artBoniRaw
                            ? artBoniRaw.split(' | ').slice(1).join(' | ')
                            : artNom;

                        const codArtEnc = b64EncodeUnicode(codArt);
                        const artNomEnc = b64EncodeUnicode(artNom);
                        const codBoniEnc = b64EncodeUnicode(codBoni);
                        const artBoniEnc = b64EncodeUnicode(artBoniNom);
                     
                        if (!tienePermiso) return '';

                      
                        return `
                          <a href="javascript:;"
                             onclick="abrirModalBonificacion(
                               '${row.CLIENTE}',
                               '${row.NOMBRE}',
                               '${codArtEnc}',
                               '${artNomEnc}',
                               ${row.CANT_MINIMA},
                               ${row.CANT_MAXIMA},
                               ${row.UNIDADES_BONIF},
                               ${row.FACTOR_BONIF},
                               '${row.FECHA_INICIAL}',
                               '${row.FECHA_FINAL}',
                               ${row.ESCALA_BONIF},
                               '${codBoniEnc}',
                               '${artBoniEnc}'
                             )" class="btn btn-sm btn-clean btn-icon me-2" title="Editar Precio">
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
        selArtCli = wireMultiSelect(dt, {
            checkAllSelector: '#checkAll',             
            rowCheckboxSelector: '.row-checkbox',
            btnMultiSelector: '.app-engage-btn.multi',
            rowKeyFn: row => `${row.CLIENTE}|${row.COD_ARTICULO}`
        });
   
        KTDatatablesBonificacionesCliente.dt = dt;

    }


    var Filtros = function () {
        $('#kt_datatable_search_query_BoniCli').on('keyup', function () {
            dt.search($(this).val()).draw();
        });
    }


    return {
        init: function () {
            initDatatable();
            Filtros();
        },
        reload: function () {
            if (KTDatatablesBonificacionesCliente.dt) {
                KTDatatablesBonificacionesCliente.dt.ajax.reload(null, false);
            }
        }
    };
}(); function abrirModalBonificacion(
    CLIENTE, NOMBRE,
    COD_ARTICULO_ENC, ARTICULO_NOMBRE_ENC,
    CANT_MINIMA, CANT_MAXIMA,
    UNIDADES_BONIF, FACTOR_BONIF,
    FECHA_INICIAL, FECHA_FINAL,
    ESCALA_BONIF,
    COD_ART_BONIFICADO_ENC, ARTICULO_BONIFICADO_ENC
) {
    const COD_ARTICULO = safeDecode(COD_ARTICULO_ENC);
    const ARTICULO_NOMBRE = safeDecode(ARTICULO_NOMBRE_ENC);
    const COD_ART_BON = safeDecode(COD_ART_BONIFICADO_ENC) || '';
    const ARTICULO_BON = safeDecode(ARTICULO_BONIFICADO_ENC) || '';

    const minVenta = Number(CANT_MINIMA) || 1;
    const maxVenta = Number(CANT_MAXIMA) || (minVenta + 1);
    const unidades = Number(UNIDADES_BONIF) || 1;
    const factorVal = Number(FACTOR_BONIF) || 1;

    const hoy = new Date();
    const man = new Date(hoy);
    man.setDate(hoy.getDate() + 1);

    const parseOrDefault = (str, defDate) => {
        const d = moment(str, ['YYYY-MM-DD', 'DD/MM/YYYY'], true);
        return d.isValid() ? d.toDate() : defDate;
    };
    const iniDate = parseOrDefault(FECHA_INICIAL, hoy);
    const finDate = parseOrDefault(FECHA_FINAL, man);

    const pad = x => String(x).padStart(2, '0');
    const fmt = d => `${pad(d.getDate())}/${pad(d.getMonth() + 1)}/${pad(d.getFullYear())}`;

    $('#bonificacionCli').val(`${CLIENTE} | ${NOMBRE}`);
    $('#ArticuloBonificacionCli').val(`${COD_ARTICULO} | ${ARTICULO_NOMBRE}`);
    $('#minimoBoniCli').val(minVenta);
    $('#maximoBoniCli').val(maxVenta);
    $('#abonificarBoniCli').val(unidades);
    $('#factorBoniCli').val(factorVal);
    $('#fechainiBoniCli').val(fmt(iniDate));
    $('#fechafinBoniCli').val(fmt(finDate));

    const $sel = $('#articulosBoniCli').val(null).trigger('change');
    const valToSelect = COD_ART_BON || COD_ARTICULO;
    const textToSelect = COD_ART_BON
        ? `${COD_ART_BON} | ${ARTICULO_BON}`
        : `${COD_ARTICULO} | ${ARTICULO_NOMBRE}`;

    if (!$sel.find(`option[value="${valToSelect}"]`).length) {
        $sel.append(new Option(textToSelect, valToSelect, true, true));
    }
    $sel.val(valToSelect).trigger('change');

    Edi_Cliente_Boni = CLIENTE;
    Edi_Articulo_Boni = COD_ARTICULO;
    Edi_Escala_Boni = ESCALA_BONIF;
    Edi_Articulo_BoniCli = valToSelect;

    $('#Edi_ModalBonificacion').modal('show');
}


function validarCantidad(valor, nombreCampo) {
    if (valor === "" || valor === null || parseFloat(valor) <= 0) {
        toastr.warning(`Por favor llenar el campo ${nombreCampo} con un valor numérico válido`, "Error", toastrOptions);
        return false;
    }
    return true;
}



function guardarBonificacion() {
    const articuloBoniSeleccionado = $('#articulosBoniCli').val();
    let Edi_Cant_Min = document.getElementById("minimoBoniCli").value;
    let Edi_Cant_Max = document.getElementById("maximoBoniCli").value;
    let Edi_Und_Boni = document.getElementById("abonificarBoniCli").value;
    let Edi_Factor_Boni = document.getElementById("factorBoniCli").value;
    let Edi_FechaIni_Boni = $('#fechainiBoniCli').val().split(" - ")[0].split('/').reverse().join('-');
    let Edi_FechaFin_Boni = $('#fechafinBoniCli').val().split(" - ")[0].split('/').reverse().join('-');
    const escalaEnviar = (Edi_Escala_Boni === 0 ? 1 : Edi_Escala_Boni);

    const payload = {
        cliente: Edi_Cliente_Boni,
        articulo: Edi_Articulo_Boni,
        escala: escalaEnviar,
        cantidadMin: +$('#minimoBoniCli').val(),
        cantidadMax: +$('#maximoBoniCli').val(),
        fechaIni: $('#fechainiBoniCli').val().split(" - ")[0].split('/').reverse().join('-'),
        FechaFin: $('#fechafinBoniCli').val().split(" - ")[0].split('/').reverse().join('-'),
        factor: +$('#factorBoniCli').val(),
        Edi_articuloBoniCli: articuloBoniSeleccionado,
        UndBonificar: +$('#abonificarBoniCli').val()
    };

    if (!validarEnteroEnRango(Edi_Cant_Min, "mínimo de venta", 1, Number.MAX_SAFE_INTEGER)) {
        return;
    }
    if (!validarEnteroEnRango(Edi_Cant_Max, "máximo de venta", 1, Number.MAX_SAFE_INTEGER)) {
        return;
    }
    if (!validarRangoCantidades(Edi_Cant_Min, Edi_Cant_Max)) {
        return;
    }
    if (!validarEnteroEnRango(Edi_Und_Boni, "unidades a bonificar", 1, Number.MAX_SAFE_INTEGER)) {
        return;
    }
    if (!validarEnteroEnRango(Edi_Factor_Boni, "factor", 1, Number.MAX_SAFE_INTEGER)) {
        return;
    }

    let dateIni = new Date(Edi_FechaIni_Boni);
    let dateFin = new Date(Edi_FechaFin_Boni);
    if (dateIni > dateFin) {
        toastr.warning("La fecha de inicio no puede ser mayor que la fecha final", "Error", toastrOptions);
        return;
    }
    Dajax(`${_urlBoniCli}/EditarBonificacionCliente`, "POST", payload, function (response) {
        if (response.responseJSON == "1") {
            toastr.success("Bonificación cliente editada con éxito", "Éxito", toastrOptions);
        } else {
            toastr.warning("Hubo un error editando la bonificación, inténtelo nuevamente", "Error", toastrOptions);
        }
    });



    $("#Edi_ModalBonificacion").modal("hide");
    KTDatatablesBonificacionesCliente.dt.ajax.reload(null, false);

}

function openUploadModal() {
    $("#uploadModal").modal("show")
}

function closeUploadModal() {
    $("#uploadModal").modal("hide")
    let botonProcesar = document.getElementById('bt_ProcesarBonificacionCli');
    botonProcesar.disabled = false; 

    var inputFile = document.getElementById('excelFile');
    inputFile.value = '';

    var descripcionField = document.getElementById('descripcionFile');
    descripcionField.value = '';

}


function ManejarFiltros() {
    $('#boniCli, #boniArticuloCli, #boniCategoriaCli, #boniMarcaCli, #boniCasaCli, #boniSubCasaCli')
        .on('change', ActualizarFiltros);
}

function ActualizarFiltros() {
    concasaBoniCli = $("#boniCasaCli :selected").map((_, e) => e.value).get().join(',');
    consubCasaBoniCli = $("#boniSubCasaCli :selected").map((_, e) => e.value).get().join(',');

    clienteBoni = $('#boniCli').val();
    articuloBoni = $('#boniArticuloCli').val();
    categoriaBoni = $('#boniCategoriaCli').val();

    KTDatatablesBonificacionesCliente.reload();
}

function IniciarFechas() {
    createDatePicker("fechainiBoniCli");
    createDatePicker("fechafinBoniCli");
}


function cargarArticulosBonifSelect() {
    $('#articuloBonifSelect').empty();
    $('#articuloBonifSelect').append(
        $('<option>', { value: '', text: 'Cargando…' })
    );

    $.getJSON(`${_urlBoniCli}/ObtenerArticulos`)
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
function cargarArticulos() {
    cargarSelectPorAjax({
        selector: '#articulosBoni',
        url: `${_urlBoniCli}/ObtenerArticulos`
    });
}
function initSelectArticulosBoniCli() {
    $('#articulosBoniCli').select2({
        dropdownParent: $('#Edi_ModalBonificacion'),
        placeholder: 'Filtrar por un artículo',
        allowClear: false,
        minimumInputLength: 1,
        ajax: {
            url: `${_urlBoniCli}/ObtenerArticulos`,
            dataType: 'json',
            delay: 250,
            data: params => ({
                term: params.term  
            }),
            processResults: data => data   
        },
        language: {
            inputTooShort: args => `Ingrese ${1 - args.input.length} caracter${1 - args.input.length === 1 ? '' : 'es'} más`,
            noResults: () => 'Sin resultados',
            searching: () => 'Buscando…'
        }
    });
}
function abrirModalBonificacionMasivo() {
    const cli = $('#boniCli').val() || '0';
    const alias = cli === '0'
        ? 'Todos'
        : ($('#boniCli option:selected').text().split('|')[1] || $('#boniCli option:selected').text()).trim();
    $('#ModalBonificacionMasivo').find('#bonificacionMasivoCliente').val(alias);

    const keys = selArtCli.getSelectedKeys();
    const n = keys.length;
    $('#ModalBonificacionMasivo').find('#bonificacionMasivoArticulos').val(
        `${n} ${n === 1 ? 'artículo' : 'artículos'}`
    );

    const hoy = new Date();
    const man = new Date(hoy); man.setDate(hoy.getDate() + 1);
    const pad = x => String(x).padStart(2, '0');
    const fmt = d => `${pad(d.getDate())}/${pad(d.getMonth() + 1)}/${pad(d.getFullYear())}`;
    $('#ModalBonificacionMasivo').find('#fechaIniBoniMasivo').val(fmt(hoy));
    $('#ModalBonificacionMasivo').find('#fechaFinBoniMasivo').val(fmt(man));

    $('#ModalBonificacionMasivo').find('#minimoBoniMasivo').val('1');
    $('#ModalBonificacionMasivo').find('#maximoBoniMasivo').val('1');
    $('#ModalBonificacionMasivo').find('#factorBoniMasivo').val('1');
    $('#ModalBonificacionMasivo').find('#unidadesBoniMasivo').val('1');

    clienteEdi = cli;
    articulosEdiMas = keys.map(k => k.split('|')[1]).join(',');
    escalaEdiMas = parseInt($('#nivelprecioBoniMasivo').val(), 10) || 1;

    $('#bonificacionMasivoArticuloBoni')
        .val(null)          
        .trigger('change');  

    $('#ModalBonificacionMasivo').modal('show');
}

function guardarBonificacionMasiva() {
    const cliente = clienteEdi;
    const articulosCsv = articulosEdiMas;
    const escala = escalaEdiMas;
    const cantidadMin = parseInt($('#minimoBoniMasivo').val(), 10) || 1;
    const cantidadMax = parseInt($('#maximoBoniMasivo').val(), 10) || cantidadMin;
    const fechaIni = $('#fechaIniBoniMasivo').val().split('/').reverse().join('-');
    const fechaFin = $('#fechaFinBoniMasivo').val().split('/').reverse().join('-');
    const factor = parseInt($('#factorBoniMasivo').val(), 10) || 1;
    const undBonificar = parseInt($('#unidadesBoniMasivo').val(), 10) || 1;
    const ediArticuloBoni = $('#bonificacionMasivoArticuloBoni').val() || '';

    if (!ediArticuloBoni) {
        toastr.warning('Debes seleccionar un artículo a bonificar', 'Error', toastrOptions);
        return;
    }

    if (cantidadMin <= 0 || cantidadMax < cantidadMin) {
        toastr.warning('Revisa los valores de cantidad mínima y máxima', 'Error', toastrOptions);
        return;
    }
    if (new Date(fechaIni) > new Date(fechaFin)) {
        toastr.warning('La fecha de inicio no puede ser posterior a la fecha final', 'Error', toastrOptions);
        return;
    }

    const txt = `${articulosCsv.split(',').length} artículo(s) para el cliente ${cliente}`;

    Swal.fire({
        title: '¿Continuar?',
        html: `Vas a actualizar <b>${txt}</b>.<br>¿Deseas continuar?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, actualizar',
        cancelButtonText: 'Cancelar'
    }).then(result => {
        if (!result.isConfirmed) return;

        const payload = {
            cliente: cliente,
            articulosCsv: articulosCsv,
            escala: escala,
            cantidadMin: cantidadMin,
            cantidadMax: cantidadMax,
            fechaIni: fechaIni,
            fechaFin: fechaFin,
            factor: factor,
            edi_articuloBoniCli: ediArticuloBoni,
            undBonificar: undBonificar
        };

        Dajax(
            `${_urlBoniCli}/EditarBonificacionClienteMasivo`,
            'POST',
            payload,
            function (response) {
                const res = response.responseJSON;
                if (res === '1') {
                    toastr.success('Bonificación masiva aplicada con éxito', 'Éxito', toastrOptions);
                    $('#ModalBonificacionMasivo').modal('hide');
                    KTDatatablesBonificacionesCliente.dt.ajax.reload(null, false);
                } else {
                    toastr.error('Error al aplicar la bonificación masiva', 'Error', toastrOptions);
                }
            }
        );
    });
}


function initSelectBonifMasivoArticuloBoni() {
    $('#bonificacionMasivoArticuloBoni').select2({
        dropdownParent: $('#ModalBonificacionMasivo'),
        placeholder: 'Filtrar por un artículo',
        allowClear: false,
        minimumInputLength: 1,
        ajax: {
            url: `${_urlBoniCli}/ObtenerArticulos`,
            dataType: 'json',
            delay: 250,
            data: params => ({ term: params.term }),
            processResults: data => data   
        },
        language: {
            inputTooShort: args =>
                `Ingrese ${1 - args.input.length} caracter${1 - args.input.length === 1 ? '' : 'es'} más`,
            noResults: () => 'Sin resultados',
            searching: () => 'Buscando…'
        }
    });
}


$(document).ready(function () {
    IniciarFechas();
    ManejarFiltros();
    $('#boniCli').val('005338').trigger('change'); 
    initSelectArticulosBoniCli();
    initSelectBonifMasivoArticuloBoni();
    inicializarSelectConBusquedaYTodos('#boniArticuloCli', 'Filtrar por un artículo');
    inicializarSelectConBusquedaYTodos('#boniCli', 'Filtrar por un cliente');
    inicializarSelectConBusquedaYTodos('#boniCategoriaCli', 'Filtrar por categoria');
    inicializarSelectMultipleConTodos('#boniCasaCli', 'Filtrar por Casa');
    inicializarSelectMultipleConTodos('#boniSubCasaCli', 'Filtrar por SubCasa');
    inicializarSelectConBusquedaYTodos('#articulosBoni', 'Filtrar por un artículo');

    KTDatatablesBonificacionesCliente.init()
}); 