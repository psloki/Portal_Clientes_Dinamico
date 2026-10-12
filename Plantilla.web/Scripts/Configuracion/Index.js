
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ESTADOS PED++++++++++++++++++++++++++++++++++++++++
var KTTablaEstados = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#tablaEstados").DataTable({
            searchDelay: 500,
            processing: true,
            
            scrollX: true,
            
            //data: cotizaciones,

            "bDestroy": true,
            destroy: true,
            //serverSide: true,
            ajax: {
                url: `${CURL}/GetEstadoPed`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                    
                }
            },
            columns: [

                
                { "data": "ESTADO_PEDIDO" },

                { "data": "DESCRIPCION" },
                {
                    "data": "DET_DESCRIPCION"
                },
                {
                    "data": "COLOR_HEXA",
                },

                {
                    "data": null,
                },
            ],
            columnDefs: [
                
                
                {
                    targets: 3,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<span class="badge ${row.COLOR_HEXA.replace("label", "badge")} badge-lg">${row.COLOR_HEXA.replace("label", "badge")}</span>`;
                    }
                },
                {
                    targets: 4,
                    orderable: false,
                    render: function (data, type, row) {
                        let accionesEstadoPedido = "";

                        if (PERMISOS_MOD_CONFIGURACION.split(',').includes("68")) {
                            accionesEstadoPedido += `<a href="javascript:;" class="btn btn-sm btn-clean btn-icon me-2" title="Editar" onclick="EditarRol(${row.ESTADO_PEDIDO}, '${row.DESCRIPCION}','${row.DET_DESCRIPCION}')">
						        <span class="svg-icon svg-icon-warning svg-icon-2x"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-05-14-112058/theme/html/demo1/dist/../src/media/svg/icons/General/Settings-2.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
							          <svg xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" width = "24px" height = "24px" viewBox = "0 0 24 24" version = "1.1" >
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                <rect x="0" y="0" width="24" height="24"/>
                                                <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
                                                <rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>
                                            </g>
                                        </svg>
						        </svg><!--end::Svg Icon--></span>
				            </a>`
                        }
                        return accionesEstadoPedido ;
                    }
                },
            ],
        });
        table = dt.$;
        dt.on('draw', function () {
        });
    }
    var resetTable = function () {
        setTimeout(() => {
            dt.ajax.reload();
        }, 100)
        dt.clear().draw()
    }
    
    return {
        init: function () {
            initDatatable();
        },
        reset: function () {
            resetTable();
        }

    }
}();

function formatState(state) {
    if (!state.id) { return state.text; }
    var $state = $(
        '<div class="' + state.element.value.toLowerCase() + '">' + state.text + '</div>'
    );
    return $state;
};
function EditarRol(ESTADO_PEDIDO, DESCRIPCION, DET_DESCRIPCION) {
    $('#EditarRolCodigo').val(ESTADO_PEDIDO)
    $("#EditarRolCodigo").attr("disabled", "disabled")
    $('#EditarRolDescripcion').val(DESCRIPCION)
    $("#EditarRolDescripcion").attr("disabled", "disabled")
    $('#EditarRolDescripcion_Det').val(DET_DESCRIPCION)
    $('#modalEditarRol').modal('show')
}

function ActualizarEstadoPedidos() {
    var formData = new FormData();
    var _codigo = $('#EditarRolCodigo').val();
    var _det_descripcion = $('#EditarRolDescripcion_Det').val();
    var _color_hexa = $('#colores').val();



    formData.append('codigo', _codigo);
    formData.append('det_direccion', _det_descripcion);
    formData.append('color_hexa', _color_hexa);


    const url = `${CURL}/ActualizarEstadoPedidos`
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(res => res.json())
        .then(data => {
            if (data == "1") {
                toastr.success('Estado actualizado con éxito', 'Éxito')
                $('#modalEditarRol').modal('hide')
                RecargarTabla(1)

            } else {
                toastr.warning('Ha ocurrido un error actualizando el estado, por favor intenta más tarde', 'Alerta', toastrOptions)
                RecargarTabla(1)
                $('#modalEditarRol').modal('hide')
            }
        })
        .catch(err => {
            toastr.warning('Ha ocurrido un error actualizando el estado, por favor intenta más tarde', 'Error')
            RecargarTabla(1)
        })

}

function NuevoEstadoPedido() {
    $("#EstadoIns").val('')
    $("#DescripcionIns").val('')
    $("#NuevoSeguimientoModalLabel").html("Agregar Estado Pedido")
    $("#NuevoSeguimientoModal").modal("show")

}

function GuardarEstadoPedidos() {
    var formData = new FormData();
    var _estado = $('#EstadoIns').val();
    var _det_descripcion = $('#DescripcionIns').val();
    var _color_hexa = $('#coloresIns').val();



    formData.append('estado', _estado);
    formData.append('det_direccion', _det_descripcion);
    formData.append('color_hexa', _color_hexa);


    const url = `${CURL}/GuardarEstadoPedidos`
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(res => res.json())
        .then(data => {
            if (data == "1") {
                toastr.success('Estado creado con éxito', 'Éxito')
                $("#NuevoSeguimientoModal").modal("hide")
                RecargarTabla(1)

            } else {
                toastr.warning('Ha ocurrido un error creando el estado, por favor intentar más tarde', 'Alerta', toastrOptions)
                RecargarTabla(1)
                $("#NuevoSeguimientoModal").modal("hide")
            }
        })
        .catch(err => {
            toastr.warning('Error en la petición! recargue e intente de nuevo', 'Error')
            RecargarTabla(1)
        })

}
//++++++++++++++++++++++++++++++++++++++ SUCESOS ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
var KTTablaSucesos = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#tablaSucesos").DataTable({
            searchDelay: 500,
            processing: true,
            
            scrollX: true,

            //data: cotizaciones,

            "bDestroy": true,
            destroy: true,
            //serverSide: true,
            ajax: {
                url: `${CURL}/GetSucesos`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {

                }
            },
            columns: [


                { "data": "CONSECUTIVO" },

                { "data": "SUCESO" },
                {
                    "data": "DESCRIPCION"
                },
               
                {
                    "data": null,
                },
            ],
            columnDefs: [

                {
                    targets: 3,
                    orderable: false,
                    render: function (data, type, row) {
                        let accionesSucesos = "";
                        if (PERMISOS_MOD_CONFIGURACION.split(',').includes("69")) {
                            accionesSucesos += `<a href="javascript:;" class="btn btn-sm btn-clean btn-icon me-2" title="Editar"  onclick="EditarSuceso(${row.CONSECUTIVO}, '${row.SUCESO}','${row.DESCRIPCION}')">
						        <span class="svg-icon svg-icon-warning svg-icon-2x"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-05-14-112058/theme/html/demo1/dist/../src/media/svg/icons/General/Settings-2.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
							          <svg xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" width = "24px" height = "24px" viewBox = "0 0 24 24" version = "1.1" >
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                <rect x="0" y="0" width="24" height="24"/>
                                                <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
                                                <rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>
                                            </g>
                                        </svg>
						        </svg><!--end::Svg Icon--></span>
				            </a>`
                        }

                        return accionesSucesos;
                    }
                },
            ],
        });
        table = dt.$;

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        dt.on('draw', function () {

        });
    }
    var resetTable = function () {
        //dt = $("#miTabla").DataTable();

        setTimeout(() => {

            dt.ajax.reload();
        }, 100)

        dt.clear().draw()
    }





    // Public methods
    return {
        init: function () {
            initDatatable();
            //setTimeout(() => { ListenerCheckboxes() }, 5000)

        },
        reset: function () {
            resetTable();

        }

    }
}();



function EditarSuceso(CONSECUTIVO, SUCESO, DESCRIPCION) {
    $('#EditarSucesoId_Fac').val(CONSECUTIVO)
    $("#EditarSucesoId_Fac").attr("disabled", "disabled")
    $('#EditarSucesoEstado').val(SUCESO)
    $("#EditarSucesoEstado").attr("disabled", "disabled")
    $('#EditarSucesoDescripcion').val(DESCRIPCION)
    $('#modalEditar_Suceso').modal('show')
}
function ActualizarSuceso() {
    var formData = new FormData();
    var _codigo_suceso = $('#EditarSucesoId_Fac').val();
    var _descripcion_suceso = $('#EditarSucesoDescripcion').val();

    formData.append('codigo', _codigo_suceso);
    formData.append('descripcion', _descripcion_suceso);

    const url = `${CURL}/ActualizarSucesos`
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(res => res.json())
        .then(data => {
            if (data == "1") {
                toastr.success('Suceso editado éxitosamente', 'Éxito');
                /*toastr.success('Datos seguimiento factura actualizados', 'Éxito')*/
                $('#modalEditar_Suceso').modal('hide')
                RecargarTabla(2)

            } else {
                toastr.warning('Ha ocurrido un error editando el suceso, por favor intentar más tarde', 'Alerta')
                $('#modalEditar_Suceso').modal('hide')
                RecargarTabla(2)
            }
        })
        .catch(err => {
            toastr.warning('Ha ocurrido un error editando el suceso, por favor intentar más tarde', 'Error')
            $('#modalEditar_Suceso').modal('hide')
            RecargarTabla(2)

        })

}

function NuevoSuceso() {
    $("#InsertSucesoEstado").val('')
    $("#InsertSucesoDescripcion").val('')
    /*$("#modalInsertarrSucesoLabel").html("Agrega suceso")*/
    $('#modalInsert_Suceso').modal('show')
}

function GuardarSuceso() {
    var formData = new FormData();
    var _descripcion = $('#InsertSucesoEstado').val();
    var _det_descripcion = $('#InsertSucesoDescripcion').val();

    formData.append('descripcion', _descripcion);
    formData.append('det_descripcion', _det_descripcion);


    const url = `${CURL}/GuardarSucesos`
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(res => res.json())
        .then(data => {
            if (data == "1") {
                toastr.success('Suceso ingresado éxitosamente', 'Éxito')
                RecargarTabla(2)
                $("#modalInsert_Suceso").modal("hide")


            } else {
                toastr.warning('Ha ocurrido un error ingresando el suceso, por favor intentar más tarde', 'Alerta', toastrOptions)
                RecargarTabla(2)
                $("#modalInsert_Suceso").modal("hide")

            }
        })
        .catch(err => {
            toastr.warning('Ha ocurrido un error ingresando el suceso, por favor intentar más tarde', 'Error')
            RecargarTabla(2)
            $("#modalInsert_Suceso").modal("hide")
        })

}
//++++++++++++++++++++++++++++++++++++++ SEGUIR FAC ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
var KTTablaSeguirFact = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#tablaFac").DataTable({
            searchDelay: 500,
            processing: true,
            
            scrollX: true,

            //data: cotizaciones,

            "bDestroy": true,
            destroy: true,
            //serverSide: true,
            ajax: {
                url: `${CURL}/GetSeguimientoFac`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {

                }
            },
            columns: [


                { "data": "CONSECUTIVO" },

                { "data": "ESTADO" },
                {
                    "data": "DESCRIPCION"
                },
                {
                    "data": "COLOR_HEXA_SEG_FAC",
                },
                {
                    "data": null,
                },
            ],
            columnDefs: [
                {
                    targets: 3,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<span class="badge ${row.COLOR_HEXA_SEG_FAC.replace("label", "badge")} badge-lg">${row.COLOR_HEXA_SEG_FAC.replace("label", "badge")}</span>`;
                    }
                },
                {
                    targets: 4,
                    orderable: false,
                    render: function (data, type, row) {
                        let accionesSeguimientoFac = "";
                        if (PERMISOS_MOD_CONFIGURACION.split(',').includes("70")) {
                            accionesSeguimientoFac += `<a href="javascript:;" class="btn btn-sm btn-clean btn-icon me-2" title="Editar" onclick="EditarSeguimiento_Fac(${row.CONSECUTIVO}, '${row.ESTADO}','${row.DESCRIPCION}')">
						        <span class="svg-icon svg-icon-warning svg-icon-2x"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-05-14-112058/theme/html/demo1/dist/../src/media/svg/icons/General/Settings-2.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
							          <svg xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" width = "24px" height = "24px" viewBox = "0 0 24 24" version = "1.1" >
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                <rect x="0" y="0" width="24" height="24"/>
                                                <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
                                                <rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>
                                            </g>
                                        </svg>
						        </svg><!--end::Svg Icon--></span>
				            </a>`
                        }
                        return accionesSeguimientoFac;
                    }
                },

            ],
            
        });
        table = dt.$;
        dt.on('draw', function () {

        });
    }
    var resetTable = function () {
        setTimeout(() => {

            dt.ajax.reload();
        }, 100)

        dt.clear().draw()
    }
    
    return {
        init: function () {
            initDatatable();
        },
        reset: function () {
            resetTable();

        }

    }
}();


function EditarSeguimiento_Fac(CONSECUTIVO, ESTADO, DESCRIPCION) {
    $('#EditarRolCodigo_Fac').val(CONSECUTIVO)
    $("#EditarRolCodigo_Fac").attr("disabled", "disabled")
    $('#EditarRolDescripcion_Fac').val(ESTADO)
    $("#EditarRolDescripcion_Fac").attr("disabled", "disabled")
    $('#EditarRolDescripcion_Det_Fac').val(DESCRIPCION)
    $('#modalEditar_Fac').modal('show')
}

function ActualizarSeguimientoFactura() {
    var formData = new FormData();
    var _codigo_SF = $('#EditarRolCodigo_Fac').val();
    var _det_descripcion_SF = $('#EditarRolDescripcion_Det_Fac').val();
    var _color_hexa_SF = $('#colores_edit_fac').val();



    formData.append('codigo', _codigo_SF);
    formData.append('det_direccion', _det_descripcion_SF);
    formData.append('color_hexa', _color_hexa_SF);


    const url = `${CURL}/ActualizarSeguimientoFacturas`
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(res => res.json())
        .then(data => {
            if (data == "1") {
                toastr.success('Seguimiento de factura guardado éxitosamente', 'Éxito');
                /*toastr.success('Datos seguimiento factura actualizados', 'Éxito')*/
                $('#modalEditar_Fac').modal('hide')
                RecargarTabla(3)

            } else {
                toastr.warning('Ha ocurrido un error guardando el seguimiento de factura, por favor intentar más tarde', 'Alerta')
                $('#modalEditar_Fac').modal('hide')
                RecargarTabla(3)
            }
        })
        .catch(err => {
            toastr.warning('Ha ocurrido un error guardando el seguimiento de factura, por favor intentar más tarde', 'Error')
            $('#modalEditar_Fac').modal('hide')
            RecargarTabla(3)

        })

}

function NuevoSeguimiento_FAC() {
    $("#EstadoIns_Fac").val('')
    $("#DescripcionIns_Fac").val('')
    $("#NuevoSeguimientoModalLabel_Fac").html("Agregar Seguimiento Factura")
    $("#NuevoSeguimientoModal_Fac").modal("show")

}


function GuardarSeguimientoFactura() {
    var formData = new FormData();
    var _estado = $('#EstadoIns_Fac').val();
    var _det_descripcion = $('#DescripcionIns_Fac').val();
    var _color_hexa = $('#coloresIns_Fac').val();



    formData.append('estado', _estado);
    formData.append('det_direccion', _det_descripcion);
    formData.append('color_hexa', _color_hexa);


    const url = `${CURL}/GuardarSeguimientoFactura`
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(res => res.json())
        .then(data => {
            if (data == "1") {
                toastr.success('Seguimiento de factura creado con éxito', 'Éxito')
                RecargarTabla(3)
                $("#NuevoSeguimientoModal_Fac").modal("hide")


            } else {
                toastr.warning('Ha ocurrido un error creando el seguimiento de factura, por favor intentar más tarde', 'Alerta', toastrOptions)
                RecargarTabla(3)
                $("#NuevoSeguimientoModal_Fac").modal("hide")

            }
        })
        .catch(err => {
            toastr.warning('Ha ocurrido un error creando el seguimiento de factura, por favor intentar más tarde', 'Error')
            RecargarTabla(3)
            $("#NuevoSeguimientoModal_Fac").modal("hide")
        })

}
function RecargarTabla(index) {
    if (index == 1) {
        try {
            KTTablaEstados.init()
        } catch (error) {
            KTTablaEstados.reset()
        }
    }
    if (index == 2) {
        try {
            KTTablaSucesos.init()
        } catch (error) {
            KTTablaSucesos.reset()
        }
    }
    if (index == 3) {
        try {
            KTTablaSeguirFact.init()
        } catch (error) {
            KTTablaSeguirFact.reset()
        }
    }
}
$(document).ready(function () {
 
    KTTablaEstados.init()
    //ManejarFiltros();
});