

    let fecha_ini = $('#FechaInicioV').val().split(' - ')[0].split('/').reverse().join('-');
    let fecha_fin = $('#FechaInicioV').val().split(' - ')[1].split('/').reverse().join('-');
    let semanamulti = $("#semanaV :selected").map((_, e) => e.value).get();
    let dia_semana = $('#diaV').val();
    let tipo_visita = $('#TipoV').val();
    let vis_vendedor = $('#vendedorFiltro').val() || '0';

    //let vis_cliente = $('#clientesFiltro').val();
    let concatenarsemanmulti = "";


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
                responsive: isMobile,
                scrollX: true,         // Habilita scroll horizontal
                autoWidth: false,
                deferRender: true, 
                select: {
                    style: 'multi',
                    selector: 'td:first-child input[type="checkbox"]',
                    className: 'row-selected'
                },

                "bDestroy": true,
                destroy: true,
             
                ajax: {
                    url: `${vurl}/Obtener_Visistas`,
                    dataSrc: "",
                    contentType: 'application/json',
                    data: function (d) {
                        d.fecha_ini = fecha_ini;
                        d.fecha_fin = fecha_fin;
                        const semanas = $("#semanaV").val();
                        d.semanamulti = Array.isArray(semanas)
                            ? semanas.join(',')
                            : '0';
                        d.dia_semana = dia_semana;
                        d.tipo_visita = tipo_visita;
                        d.vis_vendedor = vis_vendedor;
                        d.vis_cliente = vis_cliente;
                    }
                },

                columns: [

                
               
                    { "data": "COD_CLIENTE" },

                    { "data": "COD_VENDEDOR" },
                    {
                        "data": "FECHA_VISITA"
                    },
                    {
                        "data": "RUTA",
                    },
                    {
                        "data": "PROVINCIA",
                    },
                    {
                        "data": "DIRECCION",
                    },
                    {
                        "data": "ESTADO_VISITA",
                    },
                    {
                        "data": "TOTAL_A_FACTURAR",
                    },
                    {
                        "data": "COMENTARIO",
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
                            return `${row.COD_CLIENTE} - ${row.NOM_CLIENTE}`;
                        }
                    },
                    {
                        targets: 1,
                        orderable: false,
                        render: function (data, type, row) {
                            return `${row.COD_VENDEDOR} - ${row.NOM_VENDEDOR}`;
                        }
                    },
                    {
                        targets: 3,
                        orderable: false,
                        render: function (data, type, row) {
                            return `${row.RUTA} - ${row.DESCRIPCION}`;
                        }
                    },
                    {
                        targets: 6,
                        orderable: false,
                        render: function (data, type, row) {
                            return `<span class="badge ${row.COLOR_HEXA.replace("label", "badge")} badge-lg">${row.ESTADO_VISITA}</span>`;
                        }
                    },
                    {
                        targets: 7,
                        className: "dt-body-right",
                        orderable: false,
                        render: function (data, type, row) {
                            return `${row.TOTAL_A_FACTURAR.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',')}`;
                        }
                    },
               
                    {
                        targets: 9,
                        orderable: false,
                        render: function (data, type, row) {

                            let fechaUTC = new Date(row.FECHA_VISITA + 'T00:00:00Z');
                            let diaFor = fechaUTC.getUTCDate().toString().padStart(2, '0');
                            let mesFor = (fechaUTC.getUTCMonth() + 1).toString().padStart(2, '0');
                            let anioFor = fechaUTC.getUTCFullYear();
                            let fechaFormateadaFor = diaFor + '/' + mesFor + '/' + anioFor;
                            let acciones = ''

                            if (PERMISOS_MENU.split(',').includes("41")) {
                                acciones += `<a href="${encodeURIComponent(mapaUrl)}?latitud=${encodeURIComponent(row.LATITUD)}&longitud=${encodeURIComponent(row.LONGITUD)}&vendedorvisita=${encodeURIComponent(row.COD_VENDEDOR)}&cliente=${encodeURIComponent(row.COD_CLIENTE)}&fecha_visita=${encodeURIComponent(fechaFormateadaFor)}"
                                              title="Ver en mapa" id="${row.DOCUMENTO}" target="_blank" data-bloqueo-navegacion="true" class="btn btn-sm btn-clean btn-icon mr-2 ml-1">
                                              <span class="svg-icon svg-icon-primary svg-icon-2x">
                                              <i class="fas fa-map-marked-alt text-primary"></i>
                                              </span>
                                               </a>`;

                                             }


                            if (PERMISOS_GESTION_PEDIDOS.split(',').includes("19") && row.ESTADO_VISITA == 'P' && row.TOTAL_A_FACTURAR > 0) {
                                var fechaVisita = new Date(row.FECHA_VISITA);
                                fechaVisita.setHours(0, 0, 0, 0); // Establecer la hora en 00:00:00

                                var dia = fechaVisita.getUTCDate() + 1;
                                var mes = fechaVisita.getMonth() + 1; // Los meses empiezan en 0, por lo que se suma 1
                                var anio = fechaVisita.getFullYear();
                                var fechaFormateada = dia.toString().padStart(2, '0') + '/' + mes.toString().padStart(2, '0') + '/' + anio;

                                acciones += '<a href="' + eurlP + '?cliente=' + row.COD_CLIENTE + '&fecha_visita=' + fechaFormateada + '" target="_blank" data-bloqueo-navegacion="true" title="Ver pedidos" id=' + row.DOCUMENTO + ' class="btn btn-sm btn-clean btn-icon mr-2 ml-1" >\
								    <span class="svg-icon svg-icon-primary svg-icon-2x">\
                                        <i class="las la-shopping-cart icon-xl text-primary"></i>\
											    </span <\
                                    </a>'
                            }
                            return acciones;
                        }
                    },
                    // Responsive priorities
                    { responsivePriority: 1, targets: 0 },  // Checkbox
                    { responsivePriority: 100, targets: [2,3,4, 5, 6, 7, 8, 9] } // resto
                ],

            });
            //table = dt.$;

        
        }
        var resetTable = function () {
            dt.ajax.reload(null, false);
        };

    
        var Filtros = function () {

            $('#kt_datatable_search_query').on('keyup', function () {
                dt.search($(this).val()).draw();
            });
        }


        // Public methods
        return {
            init: function () {
                initDatatable();
                Filtros();
                //setTimeout(() => { ListenerCheckboxes() }, 5000)

            },
            reset: function () {
                resetTable();

            }

        }
    }();
function ManejarFiltros() {
    $('.filtro').not('#vendedorFiltro, #cobradorFiltro, #clientesFiltro')
        .on('change', () => {
            ActualizarFiltros();
            KTDatatablesServerSide.reset();
        });

    $('.filtro_multiple').on('change', function () {
        ActualizarFiltros();
        KTDatatablesServerSide.reset();
    });
}


function ActualizarFiltros() {
    const rango = $('#FechaInicioV').val().split(' - ');
    fecha_ini = rango[0].split('/').reverse().join('-');
    fecha_fin = rango[1].split('/').reverse().join('-');

    const semanas = $('#semanaV').val() || ['0'];
    concatenarsemanmulti = semanas.join(',');

    dia_semana = $('#diaV').val() || '';
    tipo_visita = $('#TipoV').val() || '';

}

$(document).on('filtros-actualizados', (_, data) => {
    vis_vendedor = data.vendedor;
    vis_cobrador = data.cobrador;
    $("#spanvendedor").text($("#vendedorFiltro option:selected").text());
    $("#spancobrador").text($("#cobradorFiltro option:selected").text());

    KTDatatablesServerSide.reset();
});

$('#clientesFiltro').on('change', () => {
    vis_cliente = $('#clientesFiltro').val() || '0';
    $("#spancliente").text($("#clientesFiltro option:selected").text());
  
    ActualizarFiltros();
    KTDatatablesServerSide.reset();
});

$(document).ready(function () {
    $("#FechaInicioV").daterangepicker({ locale: { format: "DD/MM/YYYY" } });

    inicializarSelectMultipleConTodos('#semanaV', 'Seleccione semanas');
   

    setTimeout(() => { KTDatatablesServerSide.init(); }, 100);
    ManejarFiltros();
});

$(document)
    .off('click.visitasAntiRepetido', 'a[data-bloqueo-navegacion="true"]')
    .on('click.visitasAntiRepetido', 'a[data-bloqueo-navegacion="true"]', function (event) {
        const enlace = this;
        if (enlace.dataset.accionBloqueada === 'true') {
            event.preventDefault();
            return;
        }

        enlace.dataset.accionBloqueada = 'true';
        enlace.setAttribute('aria-disabled', 'true');
        enlace.style.pointerEvents = 'none';

        // Estos enlaces abren otra página/pestaña; la vista actual no recibe
        // una señal de finalización. La ventana corta bloquea solo el doble clic.
        window.setTimeout(function () {
            delete enlace.dataset.accionBloqueada;
            enlace.removeAttribute('aria-disabled');
            enlace.style.pointerEvents = '';
        }, 1200);
    });
