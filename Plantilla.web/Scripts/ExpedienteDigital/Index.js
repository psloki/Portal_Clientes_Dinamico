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
// NUEVA VARIABLES 

let solicitudExpediente = '';
let ordenExpediente = '';
let lineaExpediente = '';
let proveedorExpediente = '';
let cotizacionExpediente = '';
let documento = '';

function bloquearControlExpediente(control) {
    if (!control || control.dataset.accionBloqueada === 'true') return null;
    control.dataset.accionBloqueada = 'true';
    if ('disabled' in control) control.disabled = true;
    control.setAttribute('aria-disabled', 'true');
    control.classList.add('disabled');
    return function () {
        delete control.dataset.accionBloqueada;
        if ('disabled' in control) control.disabled = false;
        control.removeAttribute('aria-disabled');
        control.classList.remove('disabled');
    };
}
let subtipoDocExp = '';
let tipoExp = '';
let moduloExp = '';


let fechaInicio = $("#FechaInicioV").val().split(' - ')[0].split('/').reverse().join('-');
let fechaFin = $("#FechaInicioV").val().split(' - ')[1].split('/').reverse().join('-');
let tipo = $('#filtrotipo').val();
let proveedor = $('#filtroProveedor').val();
let ordenCompra = $('#ordenCompraFiltro').val();
let cotizacion = $('#cotizacionFiltro').val();
let solicitud = $('#solicitudFiltro').val();
let documentocxcFiltro = $('#documentocxcFiltro').val();
let moduloFiltro = $('#ModuloFiltro').val();
// FIN NUEVA VARIABLES

let inicio = ''
let fin = ''
let idTemporal = ''

var KtDataTableDocs = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#tabla").DataTable({
            searchDelay: 500,
            processing: true,

            scrollX: true,
            ajax: {
                url: `${EXPEDIENTE_DIGITAL_URL}/GetExpedienteDigitalPP`,
                contentType: 'application/json',
                data: function (d) {
                    d.fechaInicio = fechaInicio,//$("#FechaInicioV").val().split(' - ')[0].split('/').reverse().join('-'),
                        d.fechaFin = fechaFin,//$("#FechaInicioV").val().split(' - ')[1].split('/').reverse().join('-'),
                        d.tipo = tipo,
                        d.modulo = moduloFiltro,
                        d.proveedor = proveedor,
                        d.referencia = documentocxcFiltro ? documentocxcFiltro.replace('|', '') : '0';


                },
                dataSrc: ""

            },
            columns: [
                { "data": "TIPO" },
                { "data": "MODULO" },
                { "data": "PROVEEDOR" },
                { "data": "REFERENCIA" },
                { "data": "FECHA" },
                { "data": null },
            ],
            columnDefs: [

                {
                    targets: 0,
                    orderable: false,
                    className: "dt-body-left",
                    render: function (data, type, row) {
                        return `${row.TIPO} || ${row.DESCRIPCION}`;
                    }
                },
                {
                    targets: 1,
                    orderable: false,
                    className: "dt-body-left",
                    render: function (data, type, row) {
                        return `${row.MODULO} || ${row.NOMBRE}`;
                    }
                },
                {
                    targets: 2,
                    orderable: false,
                    className: "dt-body-left",
                    render: function (data, type, row) {
                        return `${row.PROVEEDOR} || ${row.NOMBRE_PROV_CLI}`;
                    }
                },
                {
                    targets: 3,
                    orderable: false,
                    className: "dt-body-right",
                    render: function (data, type, row) {
                        return `${row.REFERENCIA}`;
                    }
                },
                {
                    targets: 4,
                    orderable: false,
                    className: "dt-body-center",
                    render: function (data, type, row) {
                        return `${row.FECHA}`;
                    }
                },
                {
                    targets: 5,
                    orderable: false,
                    render: function (data, type, row) {
                        let accionesExpediente = "";
                        /*if (PERMISOS_MOD_EXPEDIENTE.split(',').includes("3")) {*/
                            accionesExpediente += `<a onclick="VisualizarComprobante('${row.ADJUNTO}','${row.EXTENSION}')" class="btn btn-sm btn-clean btn-icon me-2" title="Ver documento">
                                                            <i class="la la-search-plus text-primary mx-2 fs-2x"></i>
                                                            </a>`
                       /* }*/
                       /* if (PERMISOS_MOD_EXPEDIENTE.split(',').includes("3")) {*/
                            accionesExpediente += `<a href="${row.ADJUNTO}" target="_blank" class="btn btn-sm btn-clean btn-icon me-2" title="Descargar documento">
	                                                <i class="la la-download text-success mx-2 fs-2x"></i>
							                        </a>`
                        /*}*/
                       /* if (PERMISOS_MOD_EXPEDIENTE.split(',').includes("4")) {*/
                            accionesExpediente += `<a onclick="confirmarEliminarDocumento(${row.EXPEDIENTE_DIGITAL_PP}, this)"  class="btn btn-sm btn-clean btn-icon me-2" title="Eliminar documento">
	                                                <i class="la la-trash text-danger mx-2 fs-2x"></i>
							                        </a>`
                        /*}*/
                        return accionesExpediente
                    }
                },



            ],
            // Add data-filter attribute
            //createdRow: function (row, data, dataIndex) {
            //    $(row).find('td:eq(4)').attr('data-filter', data.CreditCardType);
            //}
        });
        table = dt.$;

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        dt.on('draw', function () {

        });
    }
    // Add a click event handler to the "checkAll" checkbox in the header

    var resetTable = function () {
        //dt = $("#miTabla").DataTable();
        setTimeout(() => {

            dt.ajax.reload();
        }, 100)

        dt.clear().draw()

    }

    var Filtros = function () {


        $('#kt_datatable_search_query').on('keyup', function () {
            //const val = $(this).val() == '0' ? '' : $(this).val()
            dt.search($(this).val()).draw();
        });
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

                if (['heif', 'HEIF', 'png', 'PNG', 'img', 'IMG'].includes(file.extension)) {
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
                endpoint: `${uppyUrl}/${GenerarIdTemporal()}`,
                getResponseData(responseText) {
                    let idTemporal = responseText.replaceAll('"', '');
                    idTemporales.push(idTemporal);

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
    proveedorExpediente = proveedorExpediente || ($('#proveedor').val() && $('#proveedor').val() !== "0" ? $('#proveedor').val() : "ND");
    let proveedor = $('#filtroProveedor').val()|| "ND";
    let tipo = $('#documento').val();
    let documento = $('#tipoDocumento').val();
    //let proveedor = proveedorExpediente === "" ? "ND" : proveedorExpediente;
    let ordenCompra = $('#ordenCompra').val();
    let cotizacion = $('#cotizacion').val();
    let solicitud = $('#solicitud').val();
    let documento_cxc = $('#documentoCXC').val();
    let linea_solicitud = $('#solicitudLinea').val();
    let linea_cotizacion = $('#cotizacionLinea').val();
    let linea_ordencompra = $('#ordenCompraLinea').val();
    let documentoCxcLimpio = documento_cxc?.replace('|', '') ?? '';
    let referencia = [ordenCompra, cotizacion, solicitud, documentoCxcLimpio].find(item => item) ?? proveedor;
    let referencia_linea = [linea_solicitud, linea_cotizacion, linea_ordencompra].find(item => item) ?? 0;
    let descripcion = $('#descripcion').val();
    let subtipo = $('#tipoDocumento option:selected').text().split('||')[1]?.trim();
    console.log(subtipo);
    let formulario = [tipo, documento, referencia];


    if (formulario.includes(null) || formulario.includes('')) {
        toastr.warning('Todos los campos son obligatorios', 'Alerta', toastrOptions);
        return;
    }

    if (referencia === "ND") {

        toastr.warning('El cliente no tiene un documento para asignar adjuntos', 'Alerta', toastrOptions);
        return;
    }


    const idTemporales = KTUppy.getIdTemporales(); // Obtén los identificadores temporales de los archivos cargados
    if (idTemporales.length === 0) {
        toastr.warning('Debe adjuntar un archivo', 'Alerta', toastrOptions);
        return;
    }

    const boton = document.getElementById('btnGuardarAdjuntosExpediente');
    const desbloquear = bloquearControlExpediente(boton);
    if (!desbloquear) return;

    try {
        for (const idTemporal of idTemporales) {

            const url = `${EXPEDIENTE_DIGITAL_URL}/GuardarAdjuntos`;
            const fd = new FormData();
            fd.set('tipo', tipo);
            fd.set('referencia', referencia);
            fd.set('referenciaLinea', referencia_linea);
            fd.set('proveedor', proveedor);
            fd.set('documento', documento);
            fd.set('descripcion', descripcion);
            fd.set('adjunto', idTemporal);
            fd.set('subtipo', subtipo);
            const response = await fetch(url, {
                method: 'POST',
                body: fd
            });

            const result = await response.json();

            switch (result) {
                case 1:
                    toastr.success('Documento guardado', 'Éxito', toastrOptions);
                    $("#modalArchivos").modal("hide")
                    // En la ventana C después de guardar el archivo adjunto
                    //localStorage.setItem('archivoGuardadoParaContador', 'true');
                    //localStorage.setItem('ValorLinea', referencia_linea);
                    KtDataTableDocs.reset();

                    break;
                case 0:
                    //toastr.warning('No se pudo guardar el documento', 'Alerta', toastrOptions);
                    break;
                case 2:
                    toastr.warning('Todos los campos son obligatorios', 'Alerta', toastrOptions);
                    break;
                case 3:
                    toastr.warning('Debe adjuntar un archivo', 'Alerta', toastrOptions);
                    break;
                default:
                    toastr.warning('No se pudo guardar el documento', 'Alerta', toastrOptions);
                    break;
            }
        }
    } catch (e) {
        console.error(e);
        toastr.error('Ha ocurrido un error', 'Error', toastrOptions);
    } finally {
        desbloquear();
    }
} 


function confirmarEliminarDocumento(documento, boton) {
    Swal.fire({
        text: `¿Desea eliminar este documento?`,
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
            eliminarDocumento(documento, boton)
        }
    })
};

async function eliminarDocumento(documento, boton) {
    const desbloquear = bloquearControlExpediente(boton);
    if (!desbloquear) return;

    try {
        const url = `${EXPEDIENTE_DIGITAL_URL}/EliminarDocumento`
        let fd = new FormData()
        fd.set('documento', documento)
        const request = await fetch(url, {
            method: 'POST',
            body: fd
        })
        const result = await request.json()
        if (result === 1) {
            toastr.success('Se ha eliminado el documento con éxito', 'Éxito', toastrOptions)
            KtDataTableDocs.reset()
        } else {
            toastr.warning('Se ha podido eliminar el documento', 'Alerta', toastrOptions)
        }
    } catch (e) {
        console.error(e)
        toastr.error('Ha ocurrido un error', 'Error', toastrOptions)
    } finally {
        desbloquear();
    }
};

function VisualizarComprobante(archivo, extension) {

    $("#modalverarchivo").modal("show")

    let extensiones = ['.heif', '.HEIF', '.png', '.PNG', '.img', '.IMG', '.jpg', '.JPG', '.jpeg', '.JPEG']

    document.querySelector('#spinner').classList.remove('d-none')
    document.querySelector('#spinner').classList.add('d-flex')
    if (extensiones.includes(extension)) {

        document.querySelector('#adjuntoimg').src = archivo
        $("#adjuntoimg").removeClass('d-none')
        $('#pdf').addClass('d-none')
    } else {

        $("#adjuntoimg").addClass('d-none')
        $('#pdf').removeClass('d-none')
        document.querySelector('#pdf').src = archivo
    }


    //document.querySelector('#mobile-pdf').classList.add('d-none')
    //document.querySelector('#spinner').classList.remove('d-none')
    //document.querySelector('#spinner').classList.add('d-flex')
    //document.querySelector('#pdf').src = archivo
    //document.querySelector('#pdf-link').href = archivo

    document.querySelector('#spinner').classList.add('d-none')
    document.querySelector('#spinner').classList.remove('d-flex')
}


function CloseModalVerArchivo() {
    $("#modalverarchivo").modal("hide")
}

const KTDatepicker = function () {

    let arrows;
    //if (KTUtil.isRTL()) {
    //    arrows = {
    //        leftArrow: '<i class="la la-angle-right"></i>',
    //        rightArrow: '<i class="la la-angle-left"></i>'
    //    }
    //} else {
    //    arrows = {
    //        leftArrow: '<i class="la la-angle-left"></i>',
    //        rightArrow: '<i class="la la-angle-right"></i>'
    //    }
    //}

    // Private functions
    const Range = function () {

        //// range picker
        //$('#kt_datepicker_5').datepicker({
        //    rtl: KTUtil.isRTL(),
        //    todayHighlight: true,
        //    orientation: "bottom auto",
        //    format: "dd/mm/yyyy",
        //    templates: arrows,
        //    language: 'es'
        //});
    }

    return {
        // public functions
        init: function () {
            Range();
        }
    };
}();

function ManejadorFormulario() {
    $('#documento').on('change', e => {
        const referencia = e.target.value;
        $('.documento').val('').trigger('change');
        $('.documento').parent('div').addClass('d-none');
        switch (referencia) {
            case 'CXP':
                $('#documentoCXC').parent('div').removeClass('d-none');
                $('#documentoCXCLinea').parent('div').removeClass('d-none');
                $('#tipoDocumento').val(tipoExp).trigger('change')
                $('#descripcion').val('')
                // $('#tipoDocumento').prop('disabled', true);
                break;

            case 'CXC':
                $('#documentoCXC').parent('div').removeClass('d-none');
                $('#documentoCXCLinea').parent('div').removeClass('d-none');
                $('#tipoDocumento').val(tipoExp).trigger('change')
                $('#descripcion').val('')
                // $('#tipoDocumento').prop('disabled', true);
                break;

            default:
                $('#tipoDocumento').val('').trigger('change')
                $('#tipoDocumento').prop('disabled', false);
                $('#descripcion').val('');
                $('#proveedor').val('').trigger('change')
                $('#proveedor').prop('disabled', false);




        }

    })

    $('#solicitud').on('change', e => {
        CargaLinea_Solicutd();
    })
    $('#solicitudLinea').on('change', e => {
        CargaProveedorLinea();
    })

    $('#cotizacion').on('change', e => {
        CargaLinea_Cotizacion();
    })
    $('#cotizacionLinea').on('change', e => {
        CargaProveedorLineaCoti();
    })
    $('#ordenCompra').on('change', e => {
        CargaLinea_OrdenComprar();
    })
    $('#documento').on('change', e => {
        //CargaProveedor();
    })
    $('#proveedor').on('change', e => {
       // DocumentosProvCLiente_TipoDoc();
    })

}


// Función genérica para validar un campo y mostrar error
function validarCampo(campo, valor, mensaje) {
    if (valor === "0") {
        toastr.error(mensaje + " Error");
        return false;
    }
    return true;
}


function AdjuntarArchivos() {
    KTUppy.init()
    let tipo = $('#filtrotipo').val();
    let filtroProveedor = $('#filtroProveedor').val();

    if (!validarCampo('#filtrotipo', tipo, 'Debe seleccionar un tipo para continuar') ||
        !validarCampo('#filtroProveedor', filtroProveedor, 'Debe seleccionar un proveedor')) {
        return;
    }

    if (documento && documento.trim() !== '') {
        $('#documento').val(moduloExp).trigger('change')
        CargaProveedor();
        $('#documentoCXC').val(documento).trigger('change')
        $('#documentoCXCLinea').val(lineaExpediente).trigger('change')
        $('#proveedor').val(proveedorExpediente).trigger('change')
        $('#tipoDocumento').val(tipoExp).trigger('change')
        $('#descripcion').val('')
        $('#tipoDocumento').prop('disabled', true);
        $('#proveedor').prop('disabled', true);
        $('#documento').prop('disabled', true);
        $('#documentoCXC').prop('disabled', true);
        $('#documentoCXCLinea').prop('disabled', true);

        $('#modalArchivos').modal('show')
    }
    else {
        $('#documento').val($('#filtrotipo').val()).trigger('change')
        $('#tipoDocumento').val($('#ModuloFiltro').val()).trigger('change')
        $('#documentoCXC').val($('#documentocxcFiltro').val()).trigger('change')
        $('#documentoCXCLinea').val(0).trigger('change')
        $('#proveedor').val($('#filtroProveedor').val()).trigger('change')
        $('#descripcion').val('')
        $('#modalArchivos').modal('show')
    }
}

function InicializarSelects() {
    $('#EstadoSolicitud').select2({
        placeholder: "Seleccione un estado",
    })
    $('#FiltroColaborador').select2({
        placeholder: "Seleccione un empleado",
    })
    $('#FiltroSupervisor').select2({
        placeholder: "Filtrar por autorización local",
    })
    $('#filtroadmin').select2({
        placeholder: "Filtrar por autorización administrativa",
    })
    $('#tipodocumento').select2({
        placeholder: "Seleccione un tipo",
    })
    $('#filtrotipo').select2({
        placeholder: "Filtrar por tipo",
    })

    $('#documento').select2({
        placeholder: "Seleccione una referencia",
    })
    $('#proveedor').select2({
        placeholder: "Seleccione un proveedor",
    })
    $('#filtroProveedor').select2({
        placeholder: "Seleccione un proveedor",
    })
    $('#ordenCompraFiltro').select2({
        placeholder: "Seleccione un oc",
    })
    $('#ordenCompra').select2({
        placeholder: "Seleccione un orden de compra",
    })
    $('#cotizacion').select2({
        placeholder: "Seleccione una cotización",
    })
    $('#cotizacionFiltro').select2({
        placeholder: "Seleccione una cotización",
    })
    $('#solicitudFiltro').select2({
        placeholder: "Seleccione una solicitud",
    })
    $('#solicitud').select2({
        placeholder: "Seleccione una solicitud",
    })
    $('#solicitudLinea').select2({
        placeholder: "Seleccione una línea de solicitud",
    })
    $('#documentocxcFiltro').select2({
        placeholder: "Seleccione un documento",
    })

    $('#filtroProveedor').select2({
        language: "es",
        placeholder: 'Filtrar por un Cliente o Proveedor',
        minimumInputLength: 4,
        ajax: {
            url: `${EXPEDIENTE_DIGITAL_URL}/Obtener_Cod_Cliente_Proveedor_Buscar`,
            dataType: 'json',
            type: 'GET',
            processResults(data) {
                const todos = [{ text: "Todos", id: "0" }];
                const results = $.map(data, function (item) {
                    return {
                        text: item.COMPLETO,
                        id: item.PROVEEDOR_CLIENTE
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


function GetRango() {
    fechaInicio = $("#FechaInicioV").val().split(' - ')[0].split('/').reverse().join('-');
    fechaFin = $("#FechaInicioV").val().split(' - ')[1].split('/').reverse().join('-');
    tipo = $('#filtrotipo').val();
    proveedor = $('#filtroProveedor').val();
    documentocxcFiltro = $('#documentocxcFiltro').val();
    moduloFiltro = $('#ModuloFiltro').val();
    

}

function ManejarFiltros() {
    $('.filtro').on('change', () => {
        GetRango();
        KtDataTableDocs.reset();

    })
}

function CargaLinea_Solicutd() {
    let solicitud = $('#solicitud').val();
    if (solicitud) {
        $.ajax({
            url: `${EXPEDIENTE_DIGITAL_URL}/obtenerlineaSolicitudExp`,
            data: { solicitud: solicitud },
            type: "POST",
            success: function (data) {
                const $selectLinea = $("select[name='solicitudLinea']");
                let solicitud_linea = "";
                if (data && data.length) {
                    console.log(lineaExpediente);
                    if (lineaExpediente == null || lineaExpediente == "") {
                        solicitud_linea += "<option value='0'>Seleccione una linea </option>"; // Agregar esta línea
                    }
                    data.forEach((prov) => {
                        if (prov.SOLICITUD_OC_LINEA === parseInt(lineaExpediente)) {

                            solicitud_linea += `<option value='${prov.SOLICITUD_OC_LINEA}' selected>${prov.SOLICITUD_OC_LINEA}</option>`;
                        } else {
                            solicitud_linea += `<option value='${prov.SOLICITUD_OC_LINEA}'>${prov.SOLICITUD_OC_LINEA}</option>`;
                        }
                        //solicitud_linea += `<option value='${prov.SOLICITUD_OC_LINEA}'>${prov.SOLICITUD_OC_LINEA}</option>`;
                    });
                }

                $selectLinea.html(solicitud_linea);
                $selectLinea.trigger("change");
            },
            error: function () {
                setTimeout(function () {
                    toastr.error("Lo sentimos, ocurrió un error al obtener la linea. Compruebe su conexión e intente de nuevo.");
                }, 3000);
            }
        });
    }
}

function CargaProveedorLineaCoti() {
    let cotizacion = $('#cotizacion').val();
    let cotizacion_linea = $('#cotizacionLinea').val();
    if (cotizacion && cotizacion_linea) {
        $.ajax({
            url: `${EXPEDIENTE_DIGITAL_URL}/obtenerProveedorlineaCotizacionExp`,
            data: { cotizacion: cotizacion, linea: cotizacion_linea },
            type: "POST",
            success: function (data) {
                const $selectProveedor = $("select[name='proveedor']");
                let proveedorCotilinea = "";
                if (data && data.length) {
                    data.forEach((prov) => {
                        proveedorCotilinea += `<option value='${prov.PROVEEDOR}' selected>${prov.PROVEEDOR}-${prov.NOMBRE_PROVEEDOR}</option>`;
                    });
                }
                $selectProveedor.html(proveedorCotilinea);
                $selectProveedor.trigger("change");
                $selectProveedor.prop('disabled', true);
            },
            error: function () {
                setTimeout(function () {
                    toastr.error("Lo sentimos, ocurrió un error al obtener el proveedor. Compruebe su conexión e intente de nuevo.");
                }, 3000);
            }
        });
    }
}

function CargaProveedorLinea() {
    let solicitud = $('#solicitud').val();
    let linea = $('#solicitudLinea').val();
    if (solicitud) {
        $.ajax({
            url: `${EXPEDIENTE_DIGITAL_URL}/obtenerProveedorlineaExp`,
            data: { solicitud: solicitud, linea: linea },
            type: "POST",
            success: function (data) {
                const $selectProveedor = $("select[name='proveedor']");
                let proveedorSolicitudlinea = "";
                if (data && data.length) {
                    data.forEach((prov) => {
                        if (prov.PROVEEDOR === proveedorExpediente) {
                            proveedorSolicitudlinea += `<option value='${prov.PROVEEDOR}' selected>${prov.PROVEEDOR}-${prov.NOMBRE_PROVEEDOR}</option>`;
                        } else {
                            proveedorSolicitudlinea += `<option value='${prov.PROVEEDOR}'>${prov.PROVEEDOR}-${prov.NOMBRE_PROVEEDOR}</option>`;
                        }

                    });
                }
                $selectProveedor.html(proveedorSolicitudlinea);
                $selectProveedor.trigger("change");
                $selectProveedor.prop('disabled', true);
            },
            error: function () {
                setTimeout(function () {
                    //toastr.error("Lo sentimos, ocurrió un error al obtener el proveedor. Compruebe su conexión e intente de nuevo.");
                }, 3000);
            }
        });
    }
}

function CargaProveedor() {
    let documento = $('#documento').val();
    let opcion = { 'CXC': 'C', 'CXP': 'P' }[documento] || '0';

    console.log(documento);

    $.ajax({
        url: `${EXPEDIENTE_DIGITAL_URL}/obtenerProveedor_PorTipoModulo`,
        data: { opcion: opcion },
        type: "POST",
        success: function (data) {
            const $selectProveedor = $("select[name='proveedor']");
            let proveedor = "";
            if (data && data.length) {
                proveedor += "<option value='0'>Seleccione un proveedor</option>";
                data.forEach((prov) => {
                    if (prov.PROVEEDOR_CLIENTE === proveedorExpediente) {
                        console.log(proveedorExpediente);
                        proveedor += `<option selected value='${prov.PROVEEDOR_CLIENTE}'>${prov.PROVEEDOR_CLIENTE}-${prov.NOMBRE}</option>`;
                    } else {
                        proveedor += `<option value='${prov.PROVEEDOR_CLIENTE}'>${prov.PROVEEDOR_CLIENTE}-${prov.NOMBRE}</option>`;
                    }

                });
            }
            $selectProveedor.html(proveedor); DocumentosProvCLiente
            $selectProveedor.trigger("change");
            //$selectProveedor.prop('disabled', false);
        },
        error: function () {
            setTimeout(function () {
                toastr.error("Lo sentimos, ocurrió un error al obtener el proveedor. Compruebe su conexión e intente de nuevo.");
            }, 3000);
        }
    });

}

function CargaLinea_Cotizacion() {
    let cotizacion = $('#cotizacion').val();
    if (cotizacion) {
        $.ajax({
            url: `${EXPEDIENTE_DIGITAL_URL}/obtenerlineaCotizacionExp`,
            data: { cotizacion: cotizacion },
            type: "POST",
            success: function (data) {
                const $selectLineaCoti = $("select[name='cotizacionLinea']");
                let cotizacion_linea = "";
                if (data && data.length) {
                    cotizacion_linea += "<option value='0'>Seleccione una linea </option>"; // Agregar esta línea

                    data.forEach((prov) => {
                        cotizacion_linea += `<option value='${prov.COTIZACION_LINEA}'>${prov.COTIZACION_LINEA}</option>`;
                    });
                }
                $selectLineaCoti.html(cotizacion_linea);
                $selectLineaCoti.trigger("change");
            },
            error: function () {
                setTimeout(function () {
                    toastr.error("Lo sentimos, ocurrió un error al obtener la linea. Compruebe su conexión e intente de nuevo.");
                }, 3000);
            }
        });
    }
}

function CargaLinea_OrdenComprar() {
    let ordencompra = $('#ordenCompra').val();
    if (ordencompra) {
        $.ajax({
            url: `${EXPEDIENTE_DIGITAL_URL}/obtenerlineaOrdenCompraExp`,
            data: { ordencompra: ordencompra },
            type: "POST",
            success: function (data) {
                const $selectLineaOrden = $("select[name='cotizacionLinea']");
                let ordencompra_linea = "";
                if (data && data.length) {
                    ordencompra_linea += "<option value='0'>Seleccione una linea </option>";
                    data.forEach((prov) => {
                        ordencompra_linea += `<option value='${prov.COTIZACION_LINEA}'>${prov.COTIZACION_LINEA}</option>`;
                    });
                }
                $selectLineaOrden.html(ordencompra_linea);
                $selectLineaOrden.trigger("change");
            },
            error: function () {
                setTimeout(function () {
                    toastr.error("Lo sentimos, ocurrió un error al obtener la linea. Compruebe su conexión e intente de nuevo.");
                }, 3000);
            }
        });
    }
}


function CargaSubTipoModulo() {
    let _modulo = $('#filtrotipo').val();
    documento = documento === "" ? "" : documento;
    let _tipoDoc = tipoExp === "" ? "0" : tipoExp;
    let _subtipo = subtipoDocExp === "" ? 1000 : subtipoDocExp;


    $.ajax({
        url: `${EXPEDIENTE_DIGITAL_URL}/obtenerSubtipoModulo`,
        data: { modulo: _modulo, tipoDoc: _tipoDoc, subtipo: _subtipo },
        type: "POST",
        success: function (data) {
            const $selectProveedor = $("select[name='ModuloFiltro']");
            let proveedor = "";
            if (data && data.length) {
                if (_tipoDoc === "0") {
                    console.log(_subtipo)
                    proveedor += "<option selected value='0'>Todos</option>";
                    data.forEach((prov) => {
                        proveedor += `<option  value='${prov.CODIGO}'>${prov.CODIGO} || ${prov.DESCRIPCION}</option>`;
                    });
                } else {
                    data.forEach((prov) => {
                        proveedor += `<option selected value='${prov.CODIGO}'>${prov.CODIGO} || ${prov.DESCRIPCION}</option>`;
                    });
                }
            }
            $selectProveedor.html(proveedor);
            $selectProveedor.trigger("change");

        },
        error: function () {
            setTimeout(function () {
                toastr.error("Lo sentimos, ocurrió un error al obtener el modulo. Compruebe su conexión e intente de nuevo.");
            }, 3000);
        }
    });


}

function CargaProveedor_Cliente() {
    let tipo = $('#filtrotipo').val();
    let opcion = { 'CXC': 'C', 'CXP': 'P' }[tipo] || '0';

    console.log(tipo, opcion);

    $.ajax({
        url: `${EXPEDIENTE_DIGITAL_URL}/obtenerProveedor_ClienteExp`,
        data: { opcion: opcion },
        type: "POST",
        success: function (data) {
            const $selectProveedor = $("select[name='filtroProveedor']");
            let proveedor = "";
            if (data && data.length) {
                proveedor += "<option value='0'>Todos</option>";
                data.forEach((prov) => {
                    proveedor += `<option value='${prov.PROVEEDOR_CLIENTE}'>${prov.PROVEEDOR_CLIENTE}-${prov.NOMBRE}</option>`;
                });
            }
            $selectProveedor.html(proveedor);
            $selectProveedor.trigger("change");
            //$selectProveedor.prop('disabled', false);
        },
        error: function () {
            setTimeout(function () {
                toastr.error("Lo sentimos, ocurrió un error al obtener el proveedor. Compruebe su conexión e intente de nuevo.");
            }, 3000);
        }
    });

}


function DocumentosProvCLiente() {
    let prov_cliente = $('#filtroProveedor').val();
    let tipoDoc = $('#ModuloFiltro').val()
    fechaInicio,
         fechaFin,
    $.ajax({
        url: `${EXPEDIENTE_DIGITAL_URL}/obtenerDocProvCLiente`,
        data: { provCliente: prov_cliente, fechaInicio: fechaInicio, fechaFin: fechaFin},
        type: "POST",
        success: function (data) {
            const $selectProveedor = $("select[name='documentocxcFiltro']");
            let proveedor = "";
            if (data && data.length) {
                proveedor += "<option value='0'>Todos</option>";
                data.forEach((prov) => {
                    proveedor += `<option value='${prov.DOCUMENTO}'>${prov.DOCUMENTO}</option>`;
                });
            }
            $selectProveedor.html(proveedor);
            $selectProveedor.trigger("change");
            DocumentosProvCLiente_TipoDoc();
        },
        error: function () {
            setTimeout(function () {
                toastr.error("Lo sentimos, ocurrió un error al obtener el proveedor. Compruebe su conexión e intente de nuevo.");
            }, 3000);
        }
    });

}



function DocumentosProvCLiente_TipoDoc() {
    let prov_cliente = $('#filtroProveedor').val();
    let tipo_DocumentoMod = $('#tipoDocumento').val();
    let documentoFiltro = $('#documentocxcFiltro').val();
    $.ajax({
        url: `${EXPEDIENTE_DIGITAL_URL}/obtenerDocProvCLiente_TipoDoc`,
        data: { provCliente: prov_cliente, fechaInicio: fechaInicio, fechaFin: fechaFin},
        type: "POST",
        success: function (data) {
            const $selectProveedor = $("select[name='documentoCXC']");
            let proveedor = "";
            if (data && data.length) {
                proveedor += "<option value='0'>Todos</option>";
                data.forEach((prov) => {
                    if (prov.DOCUMENTO === documentoFiltro) {
                        proveedor += `<option selected value='${prov.DOCUMENTO}'>${prov.DOCUMENTO}</option>`;
                    } else {
                        proveedor += `<option value='${prov.DOCUMENTO}'>${prov.DOCUMENTO}</option>`;
                    }

                });
            }
            $selectProveedor.html(proveedor);
            $selectProveedor.trigger("change");
            //$selectProveedor.prop('disabled', false);
        },
        error: function () {
            setTimeout(function () {
                toastr.error("Lo sentimos, ocurrió un error al obtener el proveedor. Compruebe su conexión e intente de nuevo.");
            }, 3000);
        }
    });

}

function onchageFiltros() {
    $('#filtrotipo').on('change', function () {
        let tipoSeleccionado = $(this).val();
        // Aquí puedes agregar la lógica que quieras ejecutar cuando cambie el valor
        console.log('Tipo seleccionado:', tipoSeleccionado);
        CargaProveedor_Cliente();

    });

    $('#filtroProveedor').on('change', function () {
        let tipoSeleccionado = $(this).val();
        // Aquí puedes agregar la lógica que quieras ejecutar cuando cambie el valor
        console.log('Proveedor_Cliente:', tipoSeleccionado);
        DocumentosProvCLiente();
        DocumentosProvCLiente_TipoDoc();
    });

}


$(document).ready(() => {
    KTDatepicker.init();
    KtDataTableDocs.init();
    //Filtrar();
    InicializarSelects();
    ManejadorFormulario();
    ManejarFiltros();

    $("#FechaInicioV").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });


    //// Obtener el valor del parámetro "solicitud" de la URL actual
    //var url = window.location.hre
    //var params = new URLSearchParams(new URL(url).search);
    //var solicitud = params.get('solicitud');
    //var orden = params.get('orden');
    //var linea = params.get('linea')
    //let documentoParam = params.get('documento');
    //var proveedorParam = params.get('proveedor')
    //var cotizacionParam = params.get('cotizacion');
    //var tipoParam = params.get('tipo');
    //var subtipoParam = params.get('subtipo');
    //var moduloParam = params.get('modulo');

    //if (documentoParam && documentoParam.trim() !== '') {
    //    documento = documentoParam
    //    lineaExpediente = 0;
    //    proveedorExpediente = proveedorParam;
    //    subtipoDocExp = subtipoParam;
    //    tipoExp = tipoParam;
    //    moduloExp = moduloParam;
    //    // Realizar una acción, como abrir un modal
    //    setTimeout(function () {
    //        AdjuntarArchivos();

    //    }, 100)

    //}

    $('#filtrotipo').trigger('change');

    onchageFiltros();

})



