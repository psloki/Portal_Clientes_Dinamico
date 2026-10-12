    
let nivelPrecioAPF = $('#nivelprecioNP').val();
const selectedRows = new Set();
let articuloAPF = $('#articulosNP').val();

let casaApAPF = $("#casaAp :selected").map((_, e) => e.value).get();
let concasaApAPF = "";
let subCasaAPF = $("#subCasaAp :selected").map((_, e) => e.value).get();
let consubCasaAPF = "";
let Edi_Articulo = "";
let Edi_Precio = "";
let Edi_Nivel_Precio = "";

function bloquearBotonGuardarPrecioArticulo() {
    const boton = document.getElementById('btnGuardarPrecioArticulo');
    if (!boton || boton.dataset.accionBloqueada === 'true') return null;
    boton.dataset.accionBloqueada = 'true';
    boton.disabled = true;
    return function () {
        delete boton.dataset.accionBloqueada;
        boton.disabled = false;
    };
}

let nivelPrecioOrigenSeleccionado = $('#nivelPrecioOrigen').val();

var KTDatatablesArticuloPrecio = function () {
    var dt;


    var initDatatable = function () {
        dt = $("#miTabla").DataTable({
            searchDelay: 500,
            processing: true,
            scrollX: true,
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
                url: `${_urlAp}/obtenerClienteArticuloPrecioIndex`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                        d.nivelPrecioAP = nivelPrecioAPF,
                        d.articuloAP = articuloAPF,
                      
                        d.casaAP = concasaApAPF,
                        d.subcasaAp = consubCasaAPF
                    
                }
            },
            columns: [

                {
                    "data": "NIVEL_PRECIO",
                    "defaultContent": ``
                },
              
                { "data": "NIVEL_PRECIO" },
                { "data": "COD_NOMBRE_ARTICULO" },
                { "data": "CASA" },
                { "data": "SUBCASA" },
                { "data": "CATEGORIA_ART" },
                { "data": "PRECIO" },
                { "data": "FECHA_INICIAL" },
                { "data": "FECHA_FIN" },
                
                { "data": "Acciones" },
            ],
            columnDefs: [

                {
                createdRow: function (row, data) {
                    $(row).attr('data-id', data.ARTICULO); 
                }
                 },
                {
                    targets: 0,
                    orderable: false,
                    visible: true,
                    render: function (data, type, row) {
                        return `
                            <div class="form-check form-check-sm form-check-custom form-check-solid">
                               <input class="form-check-input" type="checkbox" value="${row.ARTICULO}"  />
                            </div>`;
                    }
                },
                {
                    targets: 2,
                    className: "dt-body-left",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.COD_NOMBRE_ARTICULO;
                    }
                },
                {
                    targets: 3,
                    className: "dt-body-left",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.CASA;
                    }
                },
                {
                    targets: 6,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return formatearNumero(row.PRECIO)
                    }
                },
                {
                    targets: 7,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.FECHA_INICIAL;
                    }
                },
                {
                    targets: 8,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.FECHA_FIN;
                    }
                },
                {
                    targets: 9,
                    orderable: false,
                    render: function (data, type, row) {
                        const permisos = PERMISOS_MOD_DESCUENTOS.split(',');
                        const tienePermiso72 = permisos.includes("72");

                        if (!tienePermiso72) return '';

                        return `
                                <a href="javascript:;"  onclick="abrirModalPrecio('${row.ARTICULO}','${row.PRECIO}','${row.NIVEL_PRECIO}')" class="btn btn-sm btn-clean btn-icon me-2" title="Editar Precio">
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
        KTDatatablesArticuloPrecio.dt = dt;
        dt.on('draw', function () {
            dt.rows({ page: 'current' }).every(function () {
                const $row = $(this.node());
                const id = $row.data('id');
                $row.find('input.form-check-input')
                    .prop('checked', selectedRows.has(id));
            });
            const vis = dt.rows({ page: 'current' }).nodes();
            const total = vis.length;
            const marc = vis.to$().find('input:checked').length;
            $('#checkAll').prop('checked', total && total === marc);
            updateMultiBtn();
        });

    }
        $('#checkAll').on('click', function () {
                   const checked = this.checked;
               dt.rows({ page: 'all' }).every(function () {   
                       const id = this.data().ARTICULO;  
                                checked ? selectedRows.add(id) : selectedRows.delete(id);
                           });
                        dt.rows({ page: 'current' }).nodes()
                         .to$().find('input.form-check-input').prop('checked', checked);
                    updateMultiBtn();
               });

    var Filtros = function () {
        
        $('#kt_datatable_search_queryNP').on('keyup', function () {
            dt.search($(this).val()).draw();
        });
        
    }
    
        function updateMultiBtn() {
                    selectedRows.size ? $('.multi').removeClass('d-none')
                                         : $('.multi').addClass('d-none');
                }

            $('#miTabla').on('click', 'input.form-check-input', function () {
                     const id = this.value; 
                   this.checked ? selectedRows.add(id) : selectedRows.delete(id);
                    updateMultiBtn();
               });


    return {
        init: function () {
            initDatatable();
            Filtros();
           
        }
    }
}();
function abrirModalPrecio(ARTICULO, PRECIO, NIVEL_PRECIO) {
    $('#precioactual').val(formatearNumero(PRECIO));
    $('#precionuevo').val('');
    Edi_Articulo = ARTICULO;
    Edi_Nivel_Precio = NIVEL_PRECIO;
    $("#ModalPrecio").modal("show");
}

function validarPrecioDecimal(valor) {
    const regex = /^\d+(\.\d{1,8})?$/;
    return regex.test(valor.trim());
}
$('.currency-mask').on('input', function () {
    let value = $(this).val();

    value = value.replace(/[^\d.]/g, '');

    let parts = value.split('.');
    let entero = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',').substring(0,18);
    let decimal = '';
    if (parts.length > 1) {
        decimal = '.' + parts[1].substring(0, 8);
    }

    $(this).val(entero + decimal);
});

$('.currency-mask').on('blur', function () {
    if ($(this).val().trim() === '') {
        $(this).val('0');
    }
});
function guardarPrecio() {
    let precioStr = $('#precionuevo').val().trim();

    precioStr = precioStr.replace(/,/g, '');

    const decimalRegex = /^(?:\d{1,18})(?:\.\d{1,8})?$/;  
    if (!decimalRegex.test(precioStr)) {
        toastr.warning("Por favor, ingrese un número válido (máx. 18 enteros y 8 decimales)", "Error", toastrOptions);
        return;
    }

    const desbloquear = bloquearBotonGuardarPrecioArticulo();
    if (!desbloquear) return;

    Dajax(
        `${_urlAp}/EditarArticuloPrecio`,
        "POST",
        {
            articulo: Edi_Articulo,
            precio: precioStr,
            nivelPrecio: Edi_Nivel_Precio
        },
        function (response) {
            try {
                if (response.responseJSON === "1") {
                    toastr.success("Precio actualizado con éxito", "Éxito", toastrOptions);
                } else {
                    toastr.warning("Hubo un error actualizando el precio, inténtelo nuevamente", "Error", toastrOptions);
                }
                $('#miTabla').DataTable().ajax.reload(null, false);
            } finally {
                desbloquear();
            }
        }
    );

    $("#ModalPrecio").modal("hide");
}

function abrirModalPrecioMasivo() {
    const cantidad = selectedRows.size;
    if (!cantidad) return;

    const texto = cantidad === 1
        ? '1 Artículo'
        : `${cantidad} Artículos`;

    $('#cantArticulosMasivo').val(texto);
    $('#precioNuevoMasivo').val('0');
    $('#ModalPrecioMasivo').modal('show');
}

function validaNivelOrigenSeleccionado() {
    let value = $('#nivelPrecioOrigen').val();

    if (value != nivelPrecioOrigenSeleccionado) {
        //Deshabilitamos la nueva seleccion
        $("#nivelPrecioDestino option[value='" + value + "']").prop("disabled", true);
        //Habilitamos la seleccion anterior
        $("#nivelPrecioDestino option[value='" + nivelPrecioOrigenSeleccionado + "']").prop("disabled", false);
        //Aguardamos la seleccion como la nueva;
    } else {
        $("#nivelPrecioDestino option[value='" + value + "']").prop("disabled", true);
    }
    
    nivelPrecioOrigenSeleccionado = value;

    $('#nivelPrecioDestino').val("").trigger("change");
}

function abrirModalCopiarNivelPrecio() {
    $('#nivelPrecioOrigen, #nivelPrecioDestino').select2();

    $('#nivelPrecioOrigen').on('change', function () {
        validaNivelOrigenSeleccionado();
    });

    $('#ModalCopiarNivelPrecio').on('hidden.bs.modal', function () {
        $('#nivelPrecioOrigen').val("").trigger("change");
        $('#nivelPrecioDestino').val("").trigger("change");
        $("#porcentaje").val(0);
    });

    $('#ModalCopiarNivelPrecio').modal('show');
}

function copiarNivelPrecio() {
    const nivelPrecioOrigen = $("#nivelPrecioOrigen").val();
    const nivelPrecioDestino = $("#nivelPrecioDestino").val();
    const porcentaje = $("#porcentaje").val();

    if (nivelPrecioOrigen == "") {
        toastr.warning("Nivel de precio origen invalido.", "Error", toastrOptions);
        return;
    }

    if (nivelPrecioDestino == "") {
        toastr.warning("Nivel de precio destino invalido.", "Error", toastrOptions);
        return;
    }

    if (parseInt(porcentaje) < 0) {
        toastr.warning("El porcentaje proporcionado debe ser mayor a 0", "Error", toastrOptions);
        return;
    }

    Dajax(
        `${_urlAp}/CopiarNivelPrecio`,
        "POST",
        {
            nivelPrecioOrigen: nivelPrecioOrigen,
            nivelPrecioDestino: nivelPrecioDestino,
            porcentaje: porcentaje
        },
        function (response) {
            if (response.responseJSON != "0") {
                toastr.success("Nivel de precio copiado con éxito", "Éxito", toastrOptions);
            } else {
                toastr.warning("Hubo un error copiando el nivel de precio, inténtelo nuevamente", "Error", toastrOptions);
            }

            $('#ModalCopiarNivelPrecio').modal('hide');
        }
    );
}

function guardarPrecioMasivo() {
    const precioStr = $('#precioNuevoMasivo').val().replace(/,/g, '').trim();

    if (!/^(?:\d{1,18})(?:\.\d{1,8})?$/.test(precioStr)) {
        toastr.warning('Número inválido (máx 18 enteros y 8 decimales)', 'Error', toastrOptions);
        return;
    }
    const articulos = Array.from(selectedRows);
    const cant = articulos.length;
    const txt = cant === 1 ? '1 artículo' : `${cant} artículos`;
 
    Swal.fire({
        title: '¿Continuar?',
        html: `Vas a actualizar <b>${txt}</b>.<br>¿Deseas continuar?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, actualizar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (!result.isConfirmed) return;        

   

        Dajax(`${_urlAp}/EditarPrecioMasivo`,

            "POST",
            { articulos, nuevoPrecio: precioStr, nivelPrecio: $('#nivelprecioNP').val() },
            function (resp) {
                if (resp.responseJSON === '1') {
                    toastr.success('Precios actualizados', 'Éxito', toastrOptions);
                    selectedRows.clear();
                    KTDatatablesArticuloPrecio.dt.ajax.reload(null, false);
                    $('.multi').addClass('d-none');
                } else {
                    toastr.warning('Error al actualizar', 'Error', toastrOptions);
                }
            });

        $('#ModalPrecioMasivo').modal('hide');
    });
}

function ManejarFiltros() {
    $('#nivelprecioNP, #articulosNP, #marcaAp, #casaAp, #subCasaAp').on('change', ActualizarFiltros);
    $('#nivelprecioNP').on('change', function () {
        const nivel = $(this).val();
        cargarArticulosPorNivel(nivel);
    });

}

function ActualizarFiltros() {
    concasaApAPF = $("#casaAp :selected").map((_, e) => e.value).get().join(',');
    consubCasaAPF = $("#subCasaAp :selected").map((_, e) => e.value).get().join(',');
    nivelPrecioAPF = $('#nivelprecioNP').val();
    articuloAPF = $('#articulosNP').val();
    KTDatatablesArticuloPrecio.init();
}



function IniciarFechas() {

    new tempusDominus.TempusDominus(document.getElementById("fechaexp"), {
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
}


function openUploadModalAP() {
    $("#EdiModalPrecioArticulo").modal("show")
}

function closeUploadModalAP() {
    $("#EdiModalPrecioArticulo").modal("hide")
    let botonProcesar = document.getElementById('bt_ProcesarPrecioArticulo');
    botonProcesar.disabled = false; 

    var inputFile = document.getElementById('excelFile');
    inputFile.value = '';

    var descripcionField = document.getElementById('descripcionFileAP');
    descripcionField.value = ''; 

}

function cargarArticulosPorNivel(nivelPrecio = '0') {
    cargarSelectPorAjax({
        selector: '#articulosNP',
        url: `${_urlAp}/ObtenerArticulosPorNivel`,
        params: { nivelPrecio }
    });
}

$(document).ready(function () {
    IniciarFechas();
    ManejarFiltros();

    const nivelInicial = $('#nivelprecioNP').val();
    cargarArticulosPorNivel(nivelInicial);

    inicializarSelectConBusquedaYTodos('#articulosNP', 'Filtrar por un artículo');
    inicializarSelectMultipleConTodos('#casaAp', 'Filtrar por Casa');
    inicializarSelectMultipleConTodos('#subCasaAp', 'Filtrar por SubCasa');

    KTDatatablesArticuloPrecio.init();
});
