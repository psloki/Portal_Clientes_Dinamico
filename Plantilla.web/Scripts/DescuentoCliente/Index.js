
let clienteDesc = $('#descCliente').val();
let nivelPrecioDesc = $('#nivelprecioDesc').val();
let articuloDesc = $('#descArticulo').val();
let categoriaDesc = $('#descCategoria').val();

let casaDesc = $("#descCasa :selected").map((_, e) => e.value).get();
let concasaDesc = "";
let subCasaDesc = $("#descSubCasa :selected").map((_, e) => e.value).get();
let consubCasaDesc = "";

let clienteEdi = "";
let articuloEdi = "";
let porcentajeEdi = "";
let fecchaIniEdi= null
let fechaFinEdi = null
let escalaEdi = "";

function bloquearBotonDescuentoCliente() {
    const boton = document.getElementById('btnGuardarDescuentoCliente');
    if (!boton || boton.dataset.accionBloqueada === 'true') return null;
    boton.dataset.accionBloqueada = 'true';
    boton.disabled = true;
    return function () {
        delete boton.dataset.accionBloqueada;
        boton.disabled = false;
    };
}


let articuloscincodescuento = [
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-006",
        "ARTICULO_NOMBRE": "CARRITO NIÑO DE EMPUJAR TIPO TRACTOR 4 LLANTAS 107608",
        "ID_CASA": "C02-77",
        "CASA": "JUGUETERIA",
        "ID_SUBCASA": "UEN2",
        "SUBCASA": "BICI ARMADA",
        "ID_CATEGORIA_ART": "C03-203",
        "CATEGORIA_ART": "JUGUETE",
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    },
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-040",
        "ARTICULO_NOMBRE": "CANDADO LLAVE ESPIRAL CABLE 6X1000MM NEGRO",
        "ID_CASA": "C02-257",
        "CASA": "CANDADOS Y AFINES",
        "ID_SUBCASA": "UEN3",
        "SUBCASA": "BICI ACCESORIOS",
        "ID_CATEGORIA_ART": "C03-45",
        "CATEGORIA_ART": "UNIVERSAL BICICLETAS",
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    },
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-055",
        "ARTICULO_NOMBRE": "BUSHING CONVERTIDOR P/MARCO BMX DE 14M A 3/8 PARES",
        "ID_CASA": "C02-02",
        "CASA": "ACCESORIO BICI",
        "ID_SUBCASA": "UEN3",
        "SUBCASA": "BICI ACCESORIOS",
        "ID_CATEGORIA_ART": null,
        "CATEGORIA_ART": null,
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    },
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-090",
        "ARTICULO_NOMBRE": "CARAMAÑOLA BOTELLA 600C H-W11 (D)",
        "ID_CASA": "C02-176",
        "CASA": "CARAMAÑOLAS",
        "ID_SUBCASA": "UEN3",
        "SUBCASA": "BICI ACCESORIOS",
        "ID_CATEGORIA_ART": "C03-45",
        "CATEGORIA_ART": "UNIVERSAL BICICLETAS",
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    },
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-152",
        "ARTICULO_NOMBRE": "LENTES X-RACE CAMBIABLES POLARIZADO UV400 5 REPUESTOS",
        "ID_CASA": "C02-264",
        "CASA": "LENTES",
        "ID_SUBCASA": "UEN3",
        "SUBCASA": "BICI ACCESORIOS",
        "ID_CATEGORIA_ART": "C03-130",
        "CATEGORIA_ART": "GAFAS O ANTEOJOS",
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    },
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-250",
        "ARTICULO_NOMBRE": "PROTECTOR DE CADENA P/EL MARCO CB-0920 ACOLCHADO",
        "ID_CASA": "C02-02",
        "CASA": "ACCESORIO BICI",
        "ID_SUBCASA": "UEN3",
        "SUBCASA": "BICI ACCESORIOS",
        "ID_CATEGORIA_ART": null,
        "CATEGORIA_ART": null,
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    },
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-255",
        "ARTICULO_NOMBRE": "PROTECTOR CADENA/MARCO CP-01 SILVER LABRADO (D)",
        "ID_CASA": "C02-02",
        "CASA": "ACCESORIO BICI",
        "ID_SUBCASA": "UEN3",
        "SUBCASA": "BICI ACCESORIOS",
        "ID_CATEGORIA_ART": null,
        "CATEGORIA_ART": null,
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    },
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-271",
        "ARTICULO_NOMBRE": "CANDADO LLAVE ESPIRAL C/CIERRE SEGURIDAD 12X1000MM NEGRO",
        "ID_CASA": "C02-257",
        "CASA": "CANDADOS Y AFINES",
        "ID_SUBCASA": "UEN3",
        "SUBCASA": "BICI ACCESORIOS",
        "ID_CATEGORIA_ART": "C03-45",
        "CATEGORIA_ART": "UNIVERSAL BICICLETAS",
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    },
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-276",
        "ARTICULO_NOMBRE": "LUZ SIRENA 3 SONIDOS CON CLAXON P/POLICIAL",
        "ID_CASA": "C02-83",
        "CASA": "LUCES",
        "ID_SUBCASA": "UEN3",
        "SUBCASA": "BICI ACCESORIOS",
        "ID_CATEGORIA_ART": "C03-45",
        "CATEGORIA_ART": "UNIVERSAL BICICLETAS",
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    },
    {
        "CLIENTE": "005338",
        "NOMBRE": "CLIENTE WEB",
        "CATEGORIA": "CI",
        "COD_ARTICULO": "BA01000-303",
        "ARTICULO_NOMBRE": "CICLOMETRO VDO M5 WL INALAMBRICO C/MONITOR CARDIACO",
        "ID_CASA": "C02-258",
        "CASA": "CICLOMETROS Y AFINES",
        "ID_SUBCASA": "UEN3",
        "SUBCASA": "BICI ACCESORIOS",
        "ID_CATEGORIA_ART": "C03-45",
        "CATEGORIA_ART": "UNIVERSAL BICICLETAS",
        "ESCALA_DCTO": 1,
        "CANT_MINIMA": 1,
        "CANT_MAXIMA": 999,
        "FECHA_INICIAL": "2025-05-08",
        "FECHA_FINAL": "2025-05-09",
        "PORC_DESCUENTO": 10
    }
]
var KTDatatablesDescuentoCliente = function () {
    var dt;

    var initDatatable = function () {

        console.log('Cliente Init: ' + clienteDesc + ' Articulo Init: ' + articuloDesc + ' Descuento Init: ' + nivelPrecioDesc + ' Categoria Init: ' + categoriaDesc);

        dt = $("#miTabla").DataTable({
            processing: true,
            // ↓ Menos filas por página + render diferido = menos trabajo de pintura
            pageLength: 50,
            lengthMenu: [25, 50, 100],
            deferRender: true,
            stateSave: true,
            scrollX: true,

            // Mantienes tu selección múltiple
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },

            // Si ordenar por % no es crucial, puedes desactivarlo para más alivio
            order: [[7, 'desc']],

            // ↓ No re-inicialices a cada rato; con destroy basta si realmente necesitas
            destroy: true,

            ajax: {
                url: `${_urlDescCli}/obtenerDescuentoClienteIndex`,
                type: 'GET',
                cache: false,
                // ⚠️ Quita contentType aquí; DataTables hace GET con querystring
                dataSrc: '',
                data: function (d) {
                    d.cliente = clienteDesc;
                    d.articulo = articuloDesc;
                    d.categoria = categoriaDesc;
                    d.casa = concasaDesc;
                    d.subcasa = consubCasaDesc;
                }
            },

            columns: [
                { data: "CLIENTE", defaultContent: `` },
                { data: "CATEGORIA" },
                { data: "CLIENTE" },
                { data: "COD_ARTICULO" },
                { data: "CASA" },
                { data: "SUBCASA" },
                { data: "CATEGORIA_ART" },
                { data: "PORC_DESCUENTO" },
                { data: "FECHA_INICIAL" },
                { data: "FECHA_FINAL" },
                { data: "Acciones" },
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
            <input class="form-check-input row-checkbox" type="checkbox" value="${key}" />
          </div>`;
                    }
                },
                {
                    targets: 2,
                    orderable: false,
                    render: (data, type, row) => `${row.CLIENTE} | ${row.NOMBRE}`
                },
                {
                    targets: 3,
                    orderable: false,
                    render: (data, type, row) => `${row.COD_ARTICULO} | ${row.ARTICULO_NOMBRE}`
                },
                {
                    // ↓ Evita toLocaleString en celdas masivas; concatena solo el símbolo
                    targets: 7,
                    className: "dt-body-right",
                    orderable: true,
                    render: (data, type, row) => `${row.PORC_DESCUENTO}%`
                },
                {
                    targets: 8,
                    className: "dt-body-right",
                    orderable: false,
                    render: (data, type, row) => row.FECHA_INICIAL
                },
                {
                    targets: 9,
                    className: "dt-body-right",
                    orderable: false,
                    render: (data, type, row) => row.FECHA_FINAL
                },
                {
                    targets: 10,
                    orderable: false,
                    render: function (data, type, row) {
                        const permisos = PERMISOS_MOD_DESCUENTOS.split(',');
                        const tienePermiso = permisos.includes("84");
                        if (!tienePermiso) return '';
                        return `
          <a href="javascript:;" onclick="abrirModalEdiDescuento('${row.CLIENTE}','${row.NOMBRE}','${row.COD_ARTICULO}','${row.ARTICULO_NOMBRE}','${row.CANT_MINIMA}','${row.CANT_MAXIMA}','${row.PORC_DESCUENTO}','${row.FECHA_INICIAL}','${row.FECHA_FINAL}','${row.ESCALA_DCTO}')" class="btn btn-sm btn-clean btn-icon me-2" title="Editar Descuento">
            <span class="svg-icon svg-icon-warning svg-icon-2x">...</span>
          </a>`;
                    }
                }
            ]
        });

        selDesc = wireMultiSelect(dt, {
            checkAllSelector: '#checkAll',              
            rowCheckboxSelector: '.row-checkbox',
            btnMultiSelector: '.app-engage-btn.multi',  
            rowKeyFn: row => `${row.CLIENTE}|${row.COD_ARTICULO}`
        });
        KTDatatablesDescuentoCliente.dt = dt;
      

    }


    var Filtros = function () {

   
        $('#kt_datatable_search_queryDescCli').on('keyup', function () {
            dt.search($(this).val()).draw();
        });
   
    }

  
    return {
        init: function () {
            initDatatable();
            Filtros();
        },
        reload: function () {
            console.log(KTDatatablesDescuentoCliente.dt);
            if (KTDatatablesDescuentoCliente.dt) {
                KTDatatablesDescuentoCliente.dt.ajax.reload(null, false);
            }
        }
    };
}();
var KTDatatablesCincoDescuento = function () {
    var dt;

    var initDatatable = function () {
        dt = $("#tabledesccinco").DataTable({
            searchDelay: 500,
            processing: true,
            scrollX: true,
            pageLength: 100,
           
            "bDestroy": true,
            destroy: true,
            responsive: true,
            data: articuloscincodescuento,
            //ajax: {
            //    url: `${_urlDescCli}/obtenerDescuentoClienteIndex`,
            //    dataSrc: "",
            //    contentType: 'application/json',
            //    data: function (d) {
            //            d.cliente = clienteDesc,
            //            d.articulo = articuloDesc,
            //            d.categoria = categoriaDesc,
            //            d.casa = concasaDesc
            //            d.subcasa = consubCasaDesc
            //    }
            //},
            columns: [
                
                
                {
                    "data": "COD_ARTICULO",
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.COD_ARTICULO} | ${row.ARTICULO_NOMBRE}`
                    }
                },
                { "data": "CASA" },
                { "data": "SUBCASA" },
                { "data": "CATEGORIA_ART" }, 
                {
                    "data": "PORC_DESCUENTO",
                    className: "dt-body-right",
                    orderable: true,
                    render: function (data, type, row) {
                        return (row.PORC_DESCUENTO).toLocaleString('en-US') + "%";
                    }
                },
                {
                    "data": "FECHA_INICIAL",
                    className: "dt-body-right",
                    orderable: false,                },
                {
                    "data": "FECHA_FINAL",
                    className: "dt-body-right",
                    orderable: false,                },
               
            ],
            columnDefs: [
                

            ],
           
        });
        //selDesc = wireMultiSelect(dt, {
        //    checkAllSelector: '#checkAll',              
        //    rowCheckboxSelector: '.row-checkbox',
        //    btnMultiSelector: '.app-engage-btn.multi',  
        //    rowKeyFn: row => `${row.CLIENTE}|${row.COD_ARTICULO}`
        //});
        //KTDatatablesDescuentoCliente.dt = dt;
      

    }


    var Filtros = function () {

   
        //$('#kt_datatable_search_queryDescCli').on('keyup', function () {
        //    dt.search($(this).val()).draw();
        //});
   
    }

    var ajustarcolumnas = function () {
        setTimeout(function () {

            dt.columns.adjust().responsive.recalc();
        }, 1000);
    }
  
    return {
        init: function () {
            initDatatable();
            Filtros();
            ajustarcolumnas()
        },
        reload: function () {
            if (KTDatatablesCincoDescuento.dt) {
                KTDatatablesCincoDescuento.dt.ajax.reload(null, false);
            }
        }
    };
}();

function openModalDescCinco() {
    $("#ModalDescuentoCinco").modal("show");
    if ($.fn.DataTable.isDataTable('#tabledesccinco')) {
        $('#tabledesccinco').DataTable().clear().rows.add(articuloscincodescuento).draw(false);
    } else {
        KTDatatablesCincoDescuento.init();
    }
}

function guardarCincoDescuento() {
    $("#ModalDescuentoCinco").modal("hide")

    toastr.success("Descuento editado con éxito", "Éxito", toastrOptions);
}
function abrirModalEdiDescuento(
    CLIENTE, NOMBRE,
    COD_ARTICULO, ARTICULO_NOMBRE,
    CANT_MINIMA, CANT_MAXIMA,
    PORC_DESCUENTO, FECHA_INICIAL_IGNORADA, FECHA_FINAL_IGNORADA,
    ESCALA_DCTO
) {
    const cantMax = (parseInt(CANT_MAXIMA, 10) === 0) ? 1 : CANT_MAXIMA;

    const hoy = new Date();
    const man = new Date(hoy);
    man.setDate(hoy.getDate() + 1);

    const pad = x => String(x).padStart(2, '0');
    const fmt = d => `${pad(d.getDate())}/${pad(d.getMonth() + 1)}/${pad(d.getFullYear())}`;

    const fechaFormateadaInicial = fmt(hoy);
    const fechaFormateadaFinal = fmt(man);

    document.getElementById("descuentoCliente").value = `${CLIENTE} | ${NOMBRE}`;
    document.getElementById("descuentoArticulo").value = `${COD_ARTICULO} | ${ARTICULO_NOMBRE}`;
    document.getElementById("CantidadMinCli").value = CANT_MINIMA;
    document.getElementById("CantidadMaxCli").value = cantMax;
    document.getElementById("PorDescuentoCli").value = PORC_DESCUENTO;
    document.getElementById("fechaIniDesc").value = fechaFormateadaInicial;
    document.getElementById("fechaFinalDesc").value = fechaFormateadaFinal;

    clienteEdi = CLIENTE;
    articuloEdi = COD_ARTICULO;
    escalaEdi = ESCALA_DCTO;

    $("#ModalEdiDescuento").modal("show");
}


function guardarDescuento() {
    let _cantidad_min = $('#CantidadMinCli').val();
    let _cantidad_max = $('#CantidadMaxCli').val();
    let _porcDescuento = $('#PorDescuentoCli').val();
    let _fechaIni = $('#fechaIniDesc').val();
    let _fechaFin = $('#fechaFinalDesc').val();

    if (!validarRangoFechasGenerico(_fechaIni, _fechaFin)) {
        toastr.warning("La fecha de inicio no puede ser mayor que la fecha final", "Error", toastrOptions);
        return;
    }


    if (!validarCantidad(_cantidad_min, "cantidad mínima")) {
        return;
    }
    if (!validarCantidad(_cantidad_max, "cantidad máxima")) {
        return;
    }

    if (!validarRangoCantidades(_cantidad_min, _cantidad_max)) {
        return;
    }

    if (!validarEnteroEnRango(_porcDescuento, "porcentaje", 0, 100)) {
        return;
    }

    const desbloquear = bloquearBotonDescuentoCliente();
    if (!desbloquear) return;

    Dajax(`${_urlDescCli}/EditarDescuentoCliente`,
        "POST", {
        cliente: clienteEdi,
        articulo: articuloEdi,
        escala: escalaEdi,
        cantidadMin: _cantidad_min,
        cantidadMax: _cantidad_max,
        fechaIni: _fechaIni,
        fechaFin: _fechaFin,
        porc_descuento: _porcDescuento
    }, function (response) {
        try {
            if (response.responseJSON == "1") {
                toastr.success("Descuento editado con éxito", "Éxito", toastrOptions);
            } else {
                toastr.warning("Hubo un error actualizando el precio, inténtelo nuevamente", "Error", toastrOptions);
            }
            $("#ModalEdiDescuento").modal("hide");
            KTDatatablesDescuentoCliente.dt.ajax.reload(null, false);
        } finally {
            desbloquear();
        }
    });
}

function abrirModalDescuentoMasivo() {
    const $sel = $('#descCliente option:selected');
    const cli = $sel.val() || '0';
    const alias = cli === '0'
        ? 'Todos'
        : ($sel.text().split('|')[1] || $sel.text()).trim();

    $('#ModalDescuento').find('#descuentoCliente').val(alias);

    const n = selDesc.getSelectedKeys().length;
    $('#ModalDescuento').find('#descuentoArticulo').val(
        `${n} ${n === 1 ? 'artículo' : 'artículos'}`
    );

    const hoy = new Date();
    const man = new Date(hoy); man.setDate(hoy.getDate() + 1);
    const pad = x => String(x).padStart(2, '0');
    const fmt = d => `${pad(d.getDate())}/${pad(d.getMonth() + 1)}/${pad(d.getFullYear())}`;

    $('#ModalDescuento').find('#fechaIniDesc').val(fmt(hoy));
    $('#ModalDescuento').find('#fechaFinalDesc').val(fmt(man));
    $('#ModalDescuento').find('#CantidadMinCli').val('1');
    $('#ModalDescuento').find('#CantidadMaxCli').val('1');
    $('#ModalDescuento').find('#PorDescuentoCli').val('1');

    clienteEdi = cli;
    articuloEdi = '';
    escalaEdi = parseInt($('#nivelprecioDesc').val(), 10) || 1;

    $("#ModalDescuento").modal("show");
}

function guardarDescuentoMasivo() {
    const cantidadMin = $('#ModalDescuento').find('#CantidadMinCli').val().trim();
    const cantidadMax = $('#ModalDescuento').find('#CantidadMaxCli').val().trim();
    const porcDescuento = $('#ModalDescuento').find('#PorDescuentoCli').val().trim();

    let _fechaIni = $('#ModalDescuento').find('#fechaIniDesc').val().trim();
    let _fechaFin = $('#ModalDescuento').find('#fechaFinalDesc').val().trim();


    if (!validarRangoFechasGenerico(_fechaIni, _fechaFin)) {
        toastr.warning("La fecha de inicio no puede ser mayor que la fecha final", "Error", toastrOptions);
        return;
    }

    if (!validarCantidad(cantidadMin, "cantidad mínima") ||
        !validarCantidad(cantidadMax, "cantidad máxima") ||
        !validarRangoCantidades(cantidadMin, cantidadMax) ||
        !validarEnteroEnRango(porcDescuento, "porcentaje", 0, 100)) {
        return;
    }

    const keys = selDesc.getSelectedKeys();
    if (keys.length === 0) {
        toastr.warning("Selecciona al menos un registro", "Error", toastrOptions);
        return;
    }
    const articulosCsv = keys.map(k => k.split('|')[1]).join(',');

    const txt = `${keys.length} ${keys.length === 1 ? 'artículo' : 'artículos'}`;
    Swal.fire({
        title: '¿Continuar?',
        html: `Vas a actualizar <b>${txt}</b>.<br>¿Deseas continuar?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, actualizar',
        cancelButtonText: 'Cancelar'
    }).then(result => {
        if (!result.isConfirmed) return;

        Dajax(`${_urlDescCli}/EditarDescuentoClienteMasivo`, "POST", {
            cliente: clienteEdi,
            articulosCsv,
            escala: escalaEdi,
            cantidadMin,
            cantidadMax,
            fechaIni: _fechaIni,
            fechaFin: _fechaFin,
            porc_descuento: porcDescuento
        }, response => {
            if (response.responseJSON == "1") {
                toastr.success("Descuentos aplicados con éxito", "Éxito", toastrOptions);
                selDesc.clear();
                KTDatatablesDescuentoCliente.dt.ajax.reload(null, false);
                $("#ModalDescuento").modal("hide");
            } else {
                toastr.error("Error aplicando descuentos", "Error", toastrOptions);
            }
        });
    });
}


function IniciarFechas() {
    createDatePicker("fechaexp");
    createDatePicker("fechaIniDesc");
    createDatePicker("fechaFinalDesc");
}
function ManejarFiltros() {
    $('#descCliente, #nivelprecioDesc, #descArticulo, #descCategoria, #descMarca, #descCasa, #descSubCasa')
        .on('change', ActualizarFiltros);
}

function ActualizarFiltros() {
    concasaDesc = $("#descCasa :selected").map((_, e) => e.value).get().join(',');
    consubCasaDesc = $("#descSubCasa :selected").map((_, e) => e.value).get().join(',');

    clienteDesc = $('#descCliente').val();
    nivelPrecioDesc = $('#nivelprecioDesc').val();
    articuloDesc = $('#descArticulo').val();
    categoriaDesc = $('#descCategoria').val();

    console.log('Cliente: ' + clienteDesc + ' Articulo: ' + articuloDesc + ' Descuento: ' + nivelPrecioDesc + ' Categoria: ' + categoriaDesc);

    if (KTDatatablesCincoDescuento.dt) {
        console.log('Antes del reload');
        KTDatatablesDescuentoCliente.reload();
    } else
    {
        console.log('Antes del init');
        KTDatatablesDescuentoCliente.init()
    }



  
}

function openUploadModalDesc() {
    $("#EdiModalDescuento").modal("show")
}

function closeUploadModalDesc() {

    $("#EdiModalDescuento").modal("hide")
    let botonProcesar = document.getElementById('bt_ProcesarDescuentoCli');
    botonProcesar.disabled = false;

    var inputFile = document.getElementById('excelFile');
    inputFile.value = '';

    var descripcionField = document.getElementById('descripcionFileDesc');
    descripcionField.value = ''; 
}


$(document).ready(function () {
    IniciarFechas();
    ManejarFiltros();
    inicializarSelectConBusquedaYTodos('#descArticulo', 'Filtrar por un artículo');
    inicializarSelectConBusquedaYTodos('#descCliente', 'Filtrar por un cliente');
    $('#descCliente')
        .select2('destroy')
        .select2({
            placeholder: 'Filtrar por un cliente',
            allowClear: false,
        });
    inicializarSelectConBusquedaYTodos('#descCategoria', 'Filtrar por categoria');
    inicializarSelectMultipleConTodos('#descCasa', 'Filtrar por Casa');
    inicializarSelectMultipleConTodos('#descSubCasa', 'Filtrar por SubCasa');
    //$('#descCliente').val('005338').trigger('change'); 

    KTDatatablesDescuentoCliente.dt && KTDatatablesDescuentoCliente.dt.ajax.reload(null, false);

});
