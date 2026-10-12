let totalPedido = 0
let parametros_pedido_cliente = "";
var pedido_actual = null;
let pedidoIndex = 0
let cantidadnueva = 0;
let _articulo_marcados = [];
let inputAnterior = null; // Variable para almacenar el input de la línea anterior
let id_visita_pedido = 0



var KTDatatablesServerSide = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#miTabla").DataTable({
            searchDelay: 500,
            processing: true,
            
            scrollX: true,
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },
            //data: cotizaciones,

            "bDestroy": true,
            destroy: true,
            //serverSide: true,
            ajax: {
                url: `${dped}/Obtener_Linea_Pedido`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                    d.pedidoIndex = localStorage.getItem('pedido_actual')
                }
            },
            columns: [

               
                {"data": "PEDIDO_LINEA" },
                {"data": "ARTICULO_SOFTLAND" },
                {"data": "CANTIDAD"},
                //{"data": "EXISTENCIA"},
                {"data": "PRECIO_UNITARIO"},
                {"data": "PORC_DESCUENTO" },
                {"data": "MONTO_DCTO"},
                {"data": "IVA"},
                {"data": "MONTO_IVA"},
                {"data": "SUBTOTAL"},
                {"data": "TOTAL"},
                {"data": null},
            ],
            columnDefs: [
                
                {
                    targets: 1,
                    orderable: false,
                    render: function (data, type, row) {
                        return '<span>' + row.ARTICULO_SOFTLAND + '-' + row.DESCRIPCION + '</span>';
                    }
                },
                {
                    targets: 2,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<input type="number" class="form-control" id="cantidadlin${row.ID}" value="${row.CANTIDAD}" disabled  onchange="ActualizarLinea(${row.ID},${row.PEDIDO_LINEA})" />`;
                    }
                },
                //{
                //    targets: 3,
                //    className: "dt-body-right",
                //    orderable: false,
                //    render: function (data, type, row) {
                //        return row.EXISTENCIA.toLocaleString('en-US');
                //    }
                //},
                {
                    targets: 3,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.PRECIO_UNITARIO.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 4,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return (row.PORC_DESCUENTO*100).toLocaleString('en-US');
                    }
                },
                {
                    targets: 5,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.MONTO_DCTO.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 6,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.IVA.toLocaleString('en-US');
                    }
                },
                {
                    targets: 7,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.MONTO_IVA.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 8,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.SUBTOTAL.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                    }
                },
                {
                    targets: 9,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return row.TOTAL.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');

                    }
                },
                {
                    targets: 10,
                    orderable: false,
                    render: function (data, type, row) {
                        var btns = ``

                        btns += `<a href="javascript:;" onclick="editarCantidad(${row.ID})" class="btn btn-sm btn-clean btn-icon me-2" title="Editar cantidad" >
                           <span class="svg-icon svg-icon-warning svg-icon-2x"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-05-14-112058/theme/html/demo1/dist/../src/media/svg/icons/Navigation/Arrow-up.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                              <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                    <rect x="0" y="0" width="24" height="24"/>
                                    <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
                                    <rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>
                                </g>
                            </svg><!--end::Svg Icon--></span>
                        </a>`
                        btns += `<a href="javascript:;" onclick="EliminarLinea(${row.PEDIDO_LINEA},'${row.ARTICULO_SOFTLAND}')" class="btn btn-sm btn-clean btn-icon me-2" title="Eliminar línea">
                          <span class="svg-icon svg-icon-danger svg-icon-2x"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-05-14-112058/theme/html/demo1/dist/../src/media/svg/icons/Navigation/Arrow-down.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                               <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                    <rect x="0" y="0" width="24" height="24"/>
                                    <path d="M6,8 L18,8 L17.106535,19.6150447 C17.04642,20.3965405 16.3947578,21 15.6109533,21 L8.38904671,21 C7.60524225,21 6.95358004,20.3965405 6.89346498,19.6150447 L6,8 Z M8,10 L8.45438229,14.0894406 L15.5517885,14.0339036 L16,10 L8,10 Z" fill="currentColor" fill-rule="nonzero"/>
                                    <path d="M14,4.5 L14,3.5 C14,3.22385763 13.7761424,3 13.5,3 L10.5,3 C10.2238576,3 10,3.22385763 10,3.5 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="currentColor" opacity="0.3"/>
                                </g>
                            </svg><!--end::Svg Icon--></span>
                        </a>`
                        return btns
                    }
                },

            ],
            //}
        });
        table = dt.$;

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        dt.on('draw', function () {
            let totalLineas = dt.data().length
            let totaldescuento = dt.data().reduce((previousValue, currentValue) => previousValue + parseFloat(currentValue.MONTO_DCTO), 0)
            let totaliva = dt.data().reduce((previousValue, currentValue) => previousValue + parseFloat(currentValue.MONTO_IVA), 0)
            let subtotalgeneral = dt.data().reduce((previousValue, currentValue) => previousValue + parseFloat(currentValue.SUBTOTAL), 0)
            let total = dt.data().reduce((previousValue, currentValue) => previousValue + parseFloat(currentValue.TOTAL), 0)
            setSumaEncabezado('lineas', totalLineas, true)
            setSumaEncabezado('Descuento', totaldescuento)
            setSumaEncabezado('IVA', totaliva)
            setSumaEncabezado('Subtotal', subtotalgeneral)
            setSumaEncabezado('Total', total)

            $('#pretot').val(subtotalgeneral.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ','));
            $('#tot').val(total.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ','));
            
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
            //localStorage.setItem('pedido_actual')
            initDatatable();
            //setTimeout(() => { ListenerCheckboxes() }, 5000)

        },
        reset: function () {
            resetTable();

        }

    }
}();

function Guardar() {
    var res = 0;
    let _consecutivo = $('#ConsecutivoPed').val()
    let _cliente = $('#clienteDetalle').val();
    let _tipo = 1;
    let _vendedor = $('#vendedorFiltro').val();
    let _observacion = document.getElementById("observaciones-textarea").value;
    let _bodega = document.getElementById('BodegaEncabezado').value;
    console.log('El valor del input es: ' + _bodega);

    localStorage.setItem('comentario', _observacion);
    if (_cliente == null || _cliente == "") {
        toastr.warning('Seleccione un cliente')
    } else {

        Dajax(`${dped}/MigrarApedido`,
            "POST", { consecutivo: _consecutivo, cliente: _cliente, tipo: _tipo, vendedor: _vendedor, observacion: _observacion, bodega: _bodega }, function (response) {
                if (response.status == 200) {
                    if (response.responseJSON != "0") {
                        pedido_actual_temp = response.responseJSON;
                        // Almacenar pedido actual en localStorage
                        localStorage.setItem('pedido_actual', pedido_actual_temp);

                        if (res == 0) {
                            toastr.success("Pedido encabezado ingresado con exitó.");

                            // Obtener valores de cliente y número de pedido
                            let cliente = $('#clienteDetalle').val();
                            let pedido = $('#ConsecutivoPed').val();

                            // Construir la URL de redirección con los parámetros de cliente y pedido
                            let url = window.location.href.split('?')[0];
                            let parametros = [`cliente=${cliente}`, `pedido=${pedido}`].filter(Boolean).join('&');
                            let urlRedireccion = `${url}?${parametros}`;

                            // Redirigir a la página con la URL construida
                            setTimeout(function () {
                                window.location.href = urlRedireccion;
                            }, 2500); // Redirigir después de 1 segundo (1000 milisegundos)

                        }

                    } else {
                        res = 1;
                        toastr.warning("Hubo un erro al realizar la acción");
                        setTimeout(function () {
                            location.reload();
                        }, 1500);
                    }
                }
            });
    }

    console.log(id_visita)
}


function EnviarSoftland() {

    let _pedido = localStorage.getItem('pedido_actual');
    let _observacion = $("#observaciones-textarea").val()
    let _cliente = $("#clienteDetalle").val()
    let res = 0
    let _id_visita = localStorage.getItem('id_visita_actual');

    if (_pedido != "0") {

        Dajax(`${dped}/CargarPedidoASoftland`,
            "POST", { pedido: _pedido, observaciones: _observacion, cliente: _cliente, id_visita: _id_visita }, function (response) {
                if (response.status == 200) {
                    if (response.responseJSON != "0") {
                        if (res == 0) {
                            toastr.success("Pedido enviado a softland exitó.");
                            localStorage.removeItem('comentario');
                            setTimeout(function () {
                                window.location.href = `${dped}`;
                            }, 1500);
                        }

                    } else {
                        res = 1;
                        toastr.warning("Hubo un error al realizar la acción");
                        setTimeout(function () {
                            location.reload();
                        }, 1500);
                    }
                }
            });
    }
}

function setSumaEncabezado(id, total, sonHoras) {
    if (sonHoras) {
        $(`#${id}Encabezado`).html(`${Number(total.toFixed(2)).toLocaleString('en-US')}`)
    } else {
        $(`#${id}Encabezado`).html(`${Number(total.toFixed(2)).toLocaleString('en-US')}`)
    }
}

function editarCantidad(ID) {

    const input = document.getElementById(`cantidadlin${ID}`);
    if (input) {
        if (inputAnterior && inputAnterior !== input) {
            inputAnterior.setAttribute('disabled', true);
        }
        input.removeAttribute('disabled');
        inputAnterior = input;
    }
}


function EliminarLinea(PEDIDO_LINEA, ARTICULO_SOFTLAND) {
    var res = 0;
    let _pedido = localStorage.getItem('pedido_actual');
    let _pedido_linea = PEDIDO_LINEA
    if (_pedido_linea == null || _pedido_linea == "") {
        toastr.warning('No hay un pedido para editar')
    } else {

        Dajax(`${dped}/Eliminar_Linea_Pedido`,
            "POST", { pedido: _pedido, pedido_linea: _pedido_linea, articulo: ARTICULO_SOFTLAND }, function (response) {
                if (response.status == 200) {
                    if (response.responseJSON != "0") {
                        if (res == 0) {
                            toastr.success("Linea eliminada con exitó.");
                        }

                    } else {
                        res = 1;
                        toastr.warning("Hubo un error al realizar la acción");
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    }
                }
            });
    }
    pedidoIndex = localStorage.getItem('pedido_actual');
    KTDatatablesServerSide.reset();
}

function EliminarLineav1(PEDIDO_LINEA, ARTICULO_SOFTLAND) {
    var res = 0;
    let _pedido = localStorage.getItem('pedido_actual');
    let _pedido_linea = PEDIDO_LINEA
    if (_pedido_linea == null || _pedido_linea == "") {
        toastr.warning('No hay un pedido para editar')
    } else {

        Dajax(`${dped}/Eliminar_Linea_Pedido`,
            "POST", { pedido: _pedido, pedido_linea: _pedido_linea, articulo: ARTICULO_SOFTLAND  }, function (response) {
                if (response.status == 200) {
                    if (response.responseJSON != "0") {
                        if (res == 0) {
                            toastr.success("Linea eliminada con exitó.");
                        }

                    } else {
                        res = 1;
                        toastr.warning("Hubo un error al realizar la acción");
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    }
                }
            });
    }
    pedidoIndex = localStorage.getItem('pedido_actual');
    KTDataTableLineas.reset();
}

function calctotal() {
    let cantidad = $("#cantidaddet").val()
    let precio = $("#preciodet").val()
    let descuento = $("#descuentodet").val()
    descuento = descuento.replace("%", "")
    descuento = descuento / 100
    let montosindescuento = (cantidad * precio)
    let subtotal = (montosindescuento - (montosindescuento * (descuento)))
    $("#montodesc").val((montosindescuento * descuento).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ','))
    $("#montoiva").val((subtotal * 0.13).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ','))
    $("#subtotaldet").val(subtotal.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ','))
    $("#total").val((subtotal + (subtotal * 0.13)).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ','))
}

function AgregarLinea() {
    var res = 0;
    let _pedido = localStorage.getItem('pedido_actual');
    let precio = $('#preciodet').val();
    console.log(precio);
    let _articulo = $('#articulodet').val();
    let _cantidad = $('#cantidaddet').val();
    let _existencia = $('#existenciadet').val();
    let _total_existencia = _existencia - _cantidad
    let _porc_desc = parseFloat($('#descuentodet').val());
    _porc_desc = (_porc_desc === "" || _porc_desc === null) ? 0 : _porc_desc;

    if (_articulo == null || _articulo == "") {
        toastr.warning('Seleccione un articulo')
    }
    else {

        if (parseInt(_cantidad) > parseInt(_existencia)) {
            toastr.warning("Alerta: La cantidad pedida es mayor que la cantidad en stock");
            //alert(".");
        }

        if (precio === "0.00" || precio ==="") {
            toastr.warning("No se se pueden agregar lineas con precios en cero","Alerta");
            return
        }

        if (_cantidad === "0.00" || _cantidad === "" || _cantidad == null || _cantidad <= 0) {
            toastr.warning("No se pueden agregar líneas con cantidad en cero, nulos, negativos o letras", "Alerta");
            return;
        }


        Dajax(`${dped}/Ingresar_Linea_Pedido`,
            "POST", { pedido: parseInt(_pedido), articulo: _articulo, cantidad: parseInt(_cantidad), existencia: parseInt(_total_existencia), porc_descuento: parseFloat(_porc_desc) }, function (response) {
                if (response.status == 200) {
                    if (response.responseJSON != "0") {
                        if (res == 0) {
                            toastr.success("Linea agregada con éxito.");
                        }

                    } else {
                        res = 1;
                        toastr.warning("Hubo un error al realizar la acción");
                        setTimeout(function () {
                            location.reload();
                        }, 1500);
                    }
                }
            });
    }
    pedidoIndex = localStorage.getItem('pedido_actual');

    KTDatatablesServerSide.reset();
}

function ActualizarLinea(ID, PEDIDO_LINEA) {
    var res = 0;
    let _pedido = localStorage.getItem('pedido_actual');
    let _pedido_linea = PEDIDO_LINEA
    let input = document.getElementById(`cantidadlin${ID}`);
    let _cantidad = input.value;
    //let _cantidad = $('#cantidadlin').val();
    if (_pedido_linea == null || _pedido_linea == "") {
        toastr.warning('Editar cantidad')
    } else if (_cantidad === "0.00" || _cantidad === "" || _cantidad == null || _cantidad <= 0) {
        toastr.warning("No se pueden agregar líneas con cantidad en cero, nulos, negativos o letras", "Alerta");
        return;
    }else {

        Dajax(`${dped}/Actualizar_Linea_Pedido`,
            "POST", { pedido: _pedido, pedido_linea: _pedido_linea, cantidad: _cantidad }, function (response) {
                if (response.status == 200) {
                    if (response.responseJSON != "0") {
                        if (res == 0) {
                            toastr.success("Linea actualizada con exitó.");
                        }

                    } else {
                        res = 1;
                        toastr.warning("Hubo un erro al realizar la acción");
                        setTimeout(function () {
                            location.reload();
                        }, 1500);
                    }
                }
            });
    }
    pedidoIndex = localStorage.getItem('pedido_actual');
    KTDatatablesServerSide.reset();
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

function ManejarFiltros() {

    $("#articulodet").change(function () {
        var seleccionado = $(this).val();
        var _cliente = document.getElementById('clientesFiltro').value;

        $.ajax({
            url: `${dped}/Obtener_Info_Articulo`,
            type: "GET",
            data: { articulo: seleccionado, cliente: _cliente },
            success: function (data) {
                $.each(data, function (index, value) {
                    $("#existenciadet").val(value.CANT_DISPONIBLE);
                    $("#preciodet").val(value.PRECIO.toFixed(2));
                    $("#descuentodet").val(value.PORC_DCTO);

                });
                $("#cantidaddet").val(1).trigger('change');

                calctotal();
            },
            error: function (xhr) {
                console.log(xhr.responseText);
            }
        });
    });


    $("#cantidaddet").change(function () {
        var _cantidad_min = $(this).val();
        var _cliente = document.getElementById('clientesFiltro').value;
        let _articulo_selecionado = document.getElementById('articulodet').value;

        $.ajax({
            url: `${dped}/Obtener_Info_Articulo_DCTO`,
            type: "GET",
            data: { articulo: _articulo_selecionado, cliente: _cliente, cantidad_min: _cantidad_min },
            success: function (data) {
                $.each(data, function (index, value) {
                    $("#existenciadet").val(value.CANT_DISPONIBLE);
                    $("#preciodet").val(value.PRECIO.toFixed(2));
                    $("#descuentodet").val(value.PORC_DCTO);
                });
                calctotal();
            },
            error: function (xhr) {
                console.log(xhr.responseText);
            }
        });
    });

}

$(document).ready(function () {
        KTDatatablesServerSide.init()
        $("#observaciones-textarea").val(localStorage.getItem('comentario'))
        ManejarFiltros();
});
    
