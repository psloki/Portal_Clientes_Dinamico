////let FECHAINI = $('#FechaInicio').val().split('/').reverse().join('-');
////let FECHAFIN = $('#FechaInicio').val().split('/').reverse().join('-');
let FECHAINI = $('#FechaInicio').val().split(" - ")[0].split('/').reverse().join('-');
let FECHAFIN = $('#FechaInicio').val().split(" - ")[1].split('/').reverse().join('-');
let PEDIDO = $('#pedido').val();
let ESTADO_PEDIDO = $('#estado').val();
let ESTADO_FACTURA = $('#factura').val();
//let CLIENTE = "A" + $('#cliente').val();
//let CLIENTE_SIN_A = $('#cliente').val();
let PED_CLIENTE_FILTRO = $('#clientesFiltro').val();
let ped_vendedor = $('#vendedorFiltro').val();
let ped_cliente = $('#clientesFiltro').val();

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
                url: `${URL_PEDIDO}/Obtener_Pedido_Porta_CLiente`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                    d.FECHAINI = FECHAINI,
                        d.FECHAFIN = FECHAFIN,
                        d.PEDIDO = PEDIDO,
                        d.ESTADO_PEDIDO = ESTADO_PEDIDO,
                        d.ESTADO_FACTURA = ESTADO_FACTURA,
                        d.ped_vendedor = ped_vendedor,
                        d.ped_cliente = ped_cliente
                }
            },
            columns: [

                {
                    "data": null,
                    "defaultContent": ``
                },
                {
                    "data": null,
                    "defaultContent": `<a href="javascript:;"><i class="fa fa-caret-right verSubtabla" style="width: 10px;"></i></a>`
                },
                { "data": "PEDIDO" },

                { "data": "ESTADO_PEDIDO_DESCRIPCION" },
                {
                    "data": "AUTORIZADO"
                },
                {
                    "data": "FECHA_PEDIDO",
                },

                {
                    "data": "FACTURA",
                },
                {
                    "data": "CLIENTE",
                },
                {
                    "data": "SEGUIR_FAC_DESCRIPCION",
                },
                {
                    "data": "DIAS",
                },
                {
                    "data": "SUBTOTAL",
                },
                {
                    "data": "TOTAL",
                },
                {
                    "data": null,
                },
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: function (data, type, row) {
                        return `
                            <div class="form-check form-check-sm form-check-custom form-check-solid">
                                <input class="form-check-input" type="checkbox" value="${row.PEDIDO}" />
                            </div>`;
                    }
                },
                {
                    targets: 3,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<span class="badge ${row.COLOR_HEXA_PED.replace("label", "badge")} badge-lg">${row.ESTADO_PEDIDO_DESCRIPCION}</span>`;
                    }
                },
                {
                    targets: 4,
                    orderable: false,
                    render: function (data, type, row) {
                        var status = {
                            "DOC VENCIDO": { 'title': 'DOC VENCIDO', 'class': ' badge-light-warning' },
                            "LIMITE CREDITO": { 'title': 'LIMITE CREDITO', 'class': ' badge-light-danger' },
                            "LIMITE CREDITO | DOC VENCIDO": { 'title': 'LIMT CRED | DOC VENC', 'class': ' badge-light-danger' },
                            "DOC VENCIDO | DIAS ATRASO": { 'title': 'DOC VENC | DIAS ATRASO', 'class': ' badge-light-danger' },
                            "ND": { 'title': '', 'class': '' },
                            "": { 'title': '', 'class': '' },
                        };
                        if (row.AUTORIZADO == "ND" || row.AUTORIZADO == "") {
                            return ``
                        } else {
                            return `<span class="badge ${status[row.AUTORIZADO].class} badge-lg">${status[row.AUTORIZADO].title}</span>`;
                            //return '<span class="label font-weight-bold label-lg ' + row.COLOR_HEXA_PED + ' label-inline">' + row.AUTORIZADO + '</span>';
                        }
                    }
                },
                {
                    targets: 8,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<span class="badge ${row.COLOR_HEXA_FAC.replace("label", "badge")} badge-lg">${row.SEGUIR_FAC_DESCRIPCION}</span>`;
                    }
                },
                {
                    targets: 10,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return `₡${row.SUBTOTAL.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
                    }
                },
                {
                    targets: 11,
                    className: "dt-body-right",
                    orderable: false,
                    render: function (data, type, row) {
                        return `₡${row.TOTAL.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
                    }
                },
                {
                    targets: 12,
                    orderable: false,
                    render: function (data, type, row) {

                        let accionesPedidos = "";
                        console.log(row.FACTURA);
                        if (PERMISOS_GESTION_PEDIDOS.split(',').includes("26") && (row.FACTURA !== null && row.FACTURA !== "")) {
                            accionesPedidos += `<a href="javascript:;" onclick="abrirModalPDF('${row.FACTURA}')" class=" verPDF btn btn-sm btn-clean btn-icon me-2" title="Imprimir PDF Factura" >
							<span class="svg-icon svg-icon-primary svg-icon-2x"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-05-14-112058/theme/html/demo1/dist/../src/media/svg/icons/Files/Deleted-file.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
								<g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
									<rect x="0" y="0" width="24" height="24"/>
									<path d="M16,17 L16,21 C16,21.5522847 15.5522847,22 15,22 L9,22 C8.44771525,22 8,21.5522847 8,21 L8,17 L5,17 C3.8954305,17 3,16.1045695 3,15 L3,8 C3,6.8954305 3.8954305,6 5,6 L19,6 C20.1045695,6 21,6.8954305 21,8 L21,15 C21,16.1045695 20.1045695,17 19,17 L16,17 Z M17.5,11 C18.3284271,11 19,10.3284271 19,9.5 C19,8.67157288 18.3284271,8 17.5,8 C16.6715729,8 16,8.67157288 16,9.5 C16,10.3284271 16.6715729,11 17.5,11 Z M10,14 L10,20 L14,20 L14,14 L10,14 Z" fill="currentColor"/>
									<rect fill="currentColor" opacity="0.3" x="8" y="2" width="8" height="2" rx="1"/>
								</g>
							</svg><!--end::Svg Icon--></span>

						     </a>`
                        }

                        if (PERMISOS_GESTION_PEDIDOS.split(',').includes("27") && row.ESTADO_PEDIDO_DESCRIPCION === "Sin Asignar") {
                            // Crear el enlace, pero sin parámetros visibles en la URL
                            accionesPedidos += `<a href="javascript:void(0);" class="btn btn-sm btn-clean btn-icon me-2" title="Editar pedido" onclick="sendForm('${row.PEDIDO}', '${row.COD_CLIENTE}')">
                                                <span class="svg-icon svg-icon-warning svg-icon-2x">
                                                    <!-- Aquí va tu SVG Icon -->
                                                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                            <rect x="0" y="0" width="24" height="24"/>
                                                            <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
                                                            <rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>
                                                        </g>
                                                    </svg>
                                                </span>
                                            </a>`;
                        }


                        if (PERMISOS_GESTION_PEDIDOS.split(',').includes("25") && row.ESTADO_PEDIDO_DESCRIPCION !== "Sin Asignar") {
                            accionesPedidos += `<a href="javascript:void(0);" onclick="VerPedidoTracking('${row.PEDIDO}')" class="btn btn-clean btn-icon me-2" title="Ver tracking">
                                                <i class="las la-truck fs-2 text-primary"></i>
                                                </a>`;
                                                 }


                        return accionesPedidos
                    }
                },

            ],

        });
        table = dt.$;

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        dt.on('draw', function () {
            const container = document.querySelector('#miTabla');
            const checkboxes = container.querySelectorAll('[type="checkbox"]');
            // Toggle delete selected toolbar
            checkboxes.forEach(c => {
                // Checkbox on click event
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
        //dt = $("#miTabla").DataTable();

        setTimeout(() => {

            dt.ajax.reload();
        }, 100)

        dt.clear().draw()

    }

    // Add a click event handler to the "checkAll" checkbox in the header
    $('#checkAll').on('click', function () {

        var isChecked = $("#checkAll").prop('checked');
        if (isChecked) {
            $(".multi").removeClass("d-none")
        } else {
            $(".multi").addClass("d-none")
        }
    });

    // Add a click event handler to the checkboxes in the body (tbody)
    $('#miTabla').on('click', 'input[type="checkbox"]', function () {
        // Check or uncheck the "checkAll" checkbox in the header based on the checkboxes in the body
        var allChecked = dt.column(0).nodes().to$().find('input[type="checkbox"]:checked').length === dt.data().length;
        $('#checkAll').prop('checked', allChecked);

    });
    var Filtros = function () {

        $('#kt_datatable_search_query').on('keyup', function () {
            dt.search($(this).val()).draw();
        });
    }
    var ManejarCantidadCheckboxes = () => {
        // Select all delete buttons

        // Select refreshed checkbox DOM elements

        const container = document.querySelector('#miTabla');
        const allCheckboxes = container.querySelectorAll('[type="checkbox"]');

        // Detect checkboxes state & count
        let checkedState = false;
        let count = 0;

        // Count checked boxes
        allCheckboxes.forEach(c => {
            if (c.checked) {
                checkedState = true;
                count++;
            }
        });
        if (checkedState) {

            //$("#cantselect").text(count)
            $(".multi").removeClass("d-none")
        } else {
            $(".multi").addClass("d-none")
        }

    }

    var subtabla = function () {
        $('#miTabla tbody').on('click', '.verSubtabla', function () {
            var tr = $(this).closest('tr');
            var row = dt.row(tr);
            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                //construirsubtabla(subtabladatos);
                let datos = row.data()
                row.child(`
                    <table class="table  table-row-dashed fs-6 gy-4" id="subtabla${datos.PEDIDO.replace('|', '')}">
                        <thead>
                            <tr class="text-start text-gray-600 fw-bold fs-7 text-uppercase gs-0">
                                <th class="text-left min-w-50px">Línea</th>
                                <th class="text-left min-w-175px">Producto</th>
                                <th class="min-w-50px" style="text-align:right">Uni</th>
                                <th class="min-w-100px" style="text-align:right">Precio</th>
                                <th class="min-w-100px" style="text-align:right">Desc (%)</th>
                                <th class="min-w-100px" style="text-align:right">Subtotal</th>
                                <th class="min-w-600px" style="background-color: white;"></th>
                            </tr>
                        </thead>
                        <tbody class="text-gray-600 fw-semibold">
                        </tbody>
                    </table>
                `).show();
                $(`#subtabla${datos.PEDIDO.replace('|', '')}`).DataTable({
                    ajax: {
                        url: `${URL_PEDIDO}/Obtener_SubTablaLineaPedido?pedido=${datos.PEDIDO}`,
                        dataSrc: ""
                    },
                    destroy: true,
                    columns: [


                        { "data": "PEDIDO_LINEA" },
                        { "data": "ARTICULO" },
                        { "data": "CANTIDAD" },
                        { "data": "PRECIO_UNITARIO" },
                        { "data": "PORC_DESCUENTO" },
                        { "data": "SUBTOTAL" },
                    ],
                    columnDefs: [
                        {
                            targets: 1,
                            orderable: false,
                            render: function (data, type, row) {
                                return '<span>' + row.ARTICULO + '-' + row.DESCRIPCION + '</span>';
                            }
                        },
                        {
                            targets: 2,
                            className: "dt-body-right",
                            orderable: false,
                            render: function (data, type, row) {
                                return `${row.CANTIDAD.toLocaleString('en-US')}`;
                            }
                        },
                        {
                            targets: 3,
                            className: "dt-body-right",
                            orderable: false,
                            render: function (data, type, row) {
                                return `₡${row.PRECIO_UNITARIO.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
                            }
                        },
                        {
                            targets: 4,
                            className: "dt-body-right",
                            orderable: false,
                            render: function (data, type, row) {
                                return `${row.PORC_DESCUENTO.toLocaleString('en-US')}%`;
                            }
                        },
                        {
                            targets: 5,
                            className: "dt-body-right",
                            orderable: false,
                            render: function (data, type, row) {
                                return `₡${row.SUBTOTAL.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
                            }
                        },
                    ],

                });

                $(`#subtabla${datos.PEDIDO.replace('|', '')}_wrapper`).addClass("ms-20")
                //row.child(construirsubtabla(row.data().LINEAS)).show();
                tr.addClass('shown');
            }
        });
    }

    // Public methods
    return {
        init: function () {
            initDatatable();
            subtabla()
            Filtros();
            //setTimeout(() => { ListenerCheckboxes() }, 5000)

        },
        reset: function () {
            resetTable();

        }

    }
}();


//function abrirModalPDF(FACTURA) {
//    $('#pdf_preview').empty();
//    Dajax(`${URL_PEDIDO}/ObtenerPDF`, "POST", { factura: FACTURA }, function (response) {
//        // Verifica si la respuesta tiene un valor válido para la factura (base64)
//        if (response.responseText != "") {
//            $('#pdf_preview').removeClass('m--hidden');  // Asegúrate de eliminar la clase 'd-none' correctamente

//            var cuerpo = '<embed type="application/pdf" id="pdf" src="" style="width:100%; height:900px" />';
//            $("#pdf_preview").append(cuerpo);

//            // Asignamos la cadena base64 al src del embed
//            var pdfData = response.responseJSON.FACTURA;  // Suponiendo que el PDF base64 está en response.responseJSON.FACTURA
//            var pdfSrc = "data:application/pdf;base64," + pdfData;  // Prefijo base64
//            $("#pdf").attr("src", pdfSrc);  // Asignar la cadena base64 al src del embed

//            // Abre el modal
//            $("#Modal_PDF").modal("show");  // Usa el método correcto de Bootstrap 5 para abrir el modal
//        } else {
//            toastr.warning("No se encontró el PDF de esta factura");
//        }
//    });
//}


function InicializarSelect() {
    $('#pedido').select2({
        language: "es",
        placeholder: 'Filtrar por un pedido',
        minimumInputLength: 4,
        ajax: {
            url: `${URL_PEDIDO}/Buscar_Codigo_Pedido`,
            dataType: 'json',
            type: 'GET',
            processResults(data) {
                const todos = [{ text: "Todos", id: "0" }]
                const results = $.map(data, function (item) {
                    return {
                        text: item.PEDIDO,
                        id: item.PEDIDO
                    }
                })
                const options = todos.concat(results)
                return {
                    results: options
                }
            }
        }
    })
}


function ManejarFiltros() {
    $('.filtro').on('change', () => {
        ActualizarFiltros();
        setTimeout(() => {
            KTDatatablesServerSide.reset()
        }, 300)

    });





}





function ActualizarFiltros() {

    FECHAINI = $('#FechaInicio').val().split(" - ")[0].split('/').reverse().join('-');
    FECHAFIN = $('#FechaInicio').val().split(" - ")[1].split('/').reverse().join('-');
    PEDIDO = $('#pedido').val();
    ESTADO_PEDIDO = $('#estado').val();
    ESTADO_FACTURA = $('#factura').val();
    //CLIENTE = "A" + $('#cliente').val();
    //CLIENTE_SIN_A = CLIENTE.slice(1);
    ped_vendedor = $('#vendedorFiltro').val();
    ped_cliente = $('#clientesFiltro').val();
}
/// DESCERGAR EXCEL CON COLUMNAS DEL INDEX 

function DescargarExcelPedido_Index() {

    let valoresCheck = [];

    $("input:checkbox:checked").each(function () {
        valoresCheck.push(this.value);
    });
    // Filtrar el valor 'on' del array
    valoresCheck = valoresCheck.filter(value => value !== 'on');

    const str = valoresCheck.join(',');
    console.log(str)
    Dajax(`${URL_PEDIDO}/GenerarExcel_Pedido_Batch`,
        "POST", { pedido: str }, function (response) {
            let ruta = `${URL_PEDIDO}/DescargarArchivo_ExcelPedido_Index?ruta=${response.responseJSON}`
            if (response.responseJSON != 2) {
                toastr.success("Descargando Reporte Excel!", "Reporte Excel", toastrOptions);
            } else {
                toastr.warning("Error descargando el documento!", "Reporte Excel", toastrOptions);
            }
            setTimeout(() => { window.location.href = ruta; }, 1000)

        });
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






// Función para validar la existencia del ID de visita antes de crear un pedido
function validarCrearPedido() {
    // Obtener el ID de visita desde algún lugar de tu página
    let _id_visita = localStorage.getItem('id_visita_actual');
    
    // Verificar si el ID de visita está definido y no es nulo o una cadena vacía
    if (_id_visita && _id_visita.trim() !== '' && _id_visita.trim()!=='null') {
        // Si el ID de visita existe, redirigir a la página de creación de pedidos
        window.location.href = '/Pedidos/Detalles';
    } else {
        // Si el ID de visita no existe, mostrar un mensaje de error
        toastr.error("No puede crear un pedido sin una visita. Por favor, asegúrese de crear una visita antes de continuar o haga clic en el botón 'Volver'. ", 'Error');
    }
    localStorage.removeItem('comentario');
    document.getElementById("observaciones-textarea").value = "";

}


//document.addEventListener("DOMContentLoaded", function () {
//    const parametros = new URLSearchParams(window.location.search);
//    const accion = parametros.get('accion');
//    console.log(accion);
//    if (accion === 'actualizar') {
//        console.log("La página se ha actualizado.");
//        // Eliminar el parámetro "accion" de la URL
//        parametros.delete('accion');
//        const nuevaURL = window.location.pathname + '?' + parametros.toString();
//        history.replaceState({}, '', nuevaURL);

//        // Actualizar la página después de un breve retraso
//        setTimeout(function () {
//            location.reload(true); // Recarga la página desde el servidor
//        }, 2000);
//    }
//});



function sendForm(pedido, cliente) {
    // Crear un formulario oculto
    let form = document.createElement('form');
    form.method = 'POST';  
    form.action = `${URL_PEDIDO}/Detalles`;  
    form.target = "_blank";  

    // Crear un campo oculto para el parámetro 'pedido'
    let pedidoField = document.createElement('input');
    pedidoField.type = 'hidden';
    pedidoField.name = 'pedido';  
    pedidoField.value = pedido; 
    form.appendChild(pedidoField);

    // Crear un campo oculto para el parámetro 'cliente'
    let clienteField = document.createElement('input');
    clienteField.type = 'hidden';
    clienteField.name = 'cliente';  
    clienteField.value = cliente; 
    form.appendChild(clienteField);

    // Agregar el formulario al cuerpo del documento (no será visible)
    document.body.appendChild(form);

    // Enviar el formulario
    form.submit();
}


//function VerPedidoTracking(pedido) {

//    const url = `${turl}?pedido=${pedido}`;
//    setTimeout(() => { window.location.href = url; }, 1000)

//}



function VerPedidoTracking(pedido) {

    localStorage.setItem('pedido', pedido);
    console.log("Pedido almacenado en localStorage:", pedido);
    // URL donde se va a enviar el formulario
    const url = `${turl}`;  // Cambia 'turl' a la URL de destino

    // Crear un formulario oculto
    const form = document.createElement("form");
    form.method = "POST";
    form.action = url;
    form.target = "_blank";

    // Crear un input oculto para el parámetro "pedido"
    const input = document.createElement("input");
    input.type = "hidden";
    input.name = "pedido";  // El nombre del parámetro que se envía
    input.value = pedido;   // El valor del parámetro

    // Añadir el input al formulario
    form.appendChild(input);

    // Añadir el formulario al cuerpo del documento
    document.body.appendChild(form);

    // Enviar el formulario
    form.submit();

    // Opcional: Eliminar el formulario después de enviarlo
    document.body.removeChild(form);
}



$(document).ready(function () {
    KTDatatablesServerSide.init()
    InicializarSelect()
    $("#FechaInicio").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });
    ManejarFiltros();
   
});