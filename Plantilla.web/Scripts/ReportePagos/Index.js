const status = {
    "S": { 'class': ' label-light-success', 'text': 'Activo' },
    "N": { 'class': ' label-light-danger', 'text': 'Inactivo' },
};

let aprobaciones = [
    {
        "ID": 0,
        "SEGUIMIENTO": "SEG00001",
        "NOMBRE_CLIENTE": "Macrobiotica Gotitas Salud",
        "VENDEDOR": "7009",
        "VENDEDOR_NOMBRE": "GRACE QUESADA",
        "comentario": "null",
        "ESTADO": "Aprobado",
        "TIPO": "REC",
        "TEF": "123122",
        "DOCUMENTO": "5178",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Efectivo", "BANCO": "BAC",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 5650.00000000,
        "NOTAS": "",
        "FACTURAS": [{
            "ID": 0,
            "CODIGO": "3101090101",
            "VENDEDOR": "7024",
            "DOCUMENTO": "004344",
            "CLIENTE": "Farmacia El Parque",
            "ALIAS": "HDTFC",
            "VENDEDOR_NOMBRE": "ANA YANCY SALAS",
            "TIPO": "FAC",
            "FECHA_INICIO": "2016-06-03",
            "FECHA_FINAL": "2016-06-03",
            "ANTIGUEDAD": "12",
            "MONTO_LOCAL": 588073.50000000,
            "SALDO_LOCAL": 588073.50000000,
            "MONTO_PROYECTADO": 588073.50000000,
            "DESCUENTO": 0.00000000,
            "MONTO_CON_DESCUENTO": 588073.50000000
        },
        {
            "ID": 1,
            "CODIGO": "801190243",
            "VENDEDOR": "7014",
            "DOCUMENTO": "004345",
            "CLIENTE": "Macrobiotica Gotitas Salud",
            "ALIAS": "CDSN",
            "VENDEDOR_NOMBRE": "STELLA CHAVES",
            "TIPO": "FAC",
            "FECHA_INICIO": "2016-06-03",
            "FECHA_FINAL": "2016-06-03",
            "ANTIGUEDAD": "12",
            "MONTO_LOCAL": 545770.16000000,
            "SALDO_LOCAL": 545770.16000000,
            "MONTO_PROYECTADO": 545770.16000000,
            "DESCUENTO": 0.00000000,
            "MONTO_CON_DESCUENTO": 545770.16000000
        },
        {
            "ID": 2,
            "CODIGO": "3102007223",
            "VENDEDOR": "7014",
            "DOCUMENTO": "004346",
            "CLIENTE": "Corporacion de Supermercados Unidos S.R.L",
            "ALIAS": "HNN",
            "VENDEDOR_NOMBRE": "STELLA CHAVES",
            "TIPO": "FAC",
            "FECHA_INICIO": "2016-06-03",
            "FECHA_FINAL": "2016-06-03",
            "ANTIGUEDAD": "12",
            "MONTO_LOCAL": 474420.80000000,
            "SALDO_LOCAL": 474420.80000000,
            "MONTO_PROYECTADO": 474420.80000000,
            "DESCUENTO": 0.00000000,
            "MONTO_CON_DESCUENTO": 474420.80000000
        },]
    },
    {
        "ID": 1,
        "SEGUIMIENTO": "SEG00001",
        "NOMBRE_CLIENTE": "Farmacia El Parque",
        "VENDEDOR": "7008",
        "VENDEDOR_NOMBRE": "HELLEN ALFARO",
        "comentario": "null",
        "ESTADO": "Aprobado",
        "TIPO": "REC",
        "TEF": "123124",
        "DOCUMENTO": "2979",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Cheque", "BANCO": "BAC",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 944036.75000000,
        "NOTAS": "",
        "FACTURAS": [{
            "ID": 3,
            "CODIGO": "3102007223",
            "VENDEDOR": "7014",
            "DOCUMENTO": "004347",
            "CLIENTE": "Corporacion de Supermercados Unidos S.R.L",
            "ALIAS": "HNN",
            "VENDEDOR_NOMBRE": "STELLA CHAVES",
            "TIPO": "FAC",
            "FECHA_INICIO": "2016-06-03",
            "FECHA_FINAL": "2016-06-03",
            "ANTIGUEDAD": "12",
            "MONTO_LOCAL": 474420.80000000,
            "SALDO_LOCAL": 474420.80000000,
            "MONTO_PROYECTADO": 474420.80000000,
            "DESCUENTO": 0.00000000,
            "MONTO_CON_DESCUENTO": 474420.80000000
        },
        {
            "ID": 4,
            "CODIGO": "701990273",
            "VENDEDOR": "7006",
            "DOCUMENTO": "PF000621",
            "CLIENTE": "Super Bribri",
            "ALIAS": "OBANDO MARIN RAFAEL",
            "VENDEDOR_NOMBRE": "ANA YANCY SALAS (CARTAGO)",
            "TIPO": "FAC",
            "FECHA_INICIO": "2016-06-06",
            "FECHA_FINAL": "2016-06-06",
            "ANTIGUEDAD": "12",
            "MONTO_LOCAL": 23000.00000000,
            "SALDO_LOCAL": 23000.00000000,
            "MONTO_PROYECTADO": 23000.00000000,
            "DESCUENTO": 0.00000000,
            "MONTO_CON_DESCUENTO": 23000.00000000
        },
        {
            "ID": 6,
            "CODIGO": "900630945",
            "VENDEDOR": "7004",
            "DOCUMENTO": "7004F003427",
            "CLIENTE": "Almacen San Isidro",
            "ALIAS": "SALINAS MOLINA BRAYAN STIVEN",
            "VENDEDOR_NOMBRE": "BRENDA OLIVER",
            "TIPO": "FAC",
            "FECHA_INICIO": "2016-06-06",
            "FECHA_FINAL": "2016-06-06",
            "ANTIGUEDAD": "12",
            "MONTO_LOCAL": 22000.00000000,
            "SALDO_LOCAL": 22000.00000000,
            "MONTO_PROYECTADO": 22000.00000000,
            "DESCUENTO": 0.00000000,
            "MONTO_CON_DESCUENTO": 22000.00000000
        },]
    },
    {
        "ID": 2,
        "SEGUIMIENTO": "SEG00001",

        "NOMBRE_CLIENTE": "Macrobiotica Gotitas Salud",
        "VENDEDOR": "7009",
        "VENDEDOR_NOMBRE": "GRACE QUESADA",
        "comentario": "null",
        "ESTADO": "Sin aprobar",
        "TIPO": "REC",
        "TEF": "342321",
        "DOCUMENTO": "5177",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Efectivo", "BANCO": "BAC",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 588073.50000000,
        "NOTAS": "",
        "FACTURAS": [{
            "ID": 7,
            "CODIGO": "3101221977",
            "VENDEDOR": "7014",
            "DOCUMENTO": "PF000627",
            "CLIENTE": "Farmacia Economica - Plaza Quepos",
            "ALIAS": "ROJAS CERDAS MYRIAM",
            "VENDEDOR_NOMBRE": "STELLA CHAVES",
            "TIPO": "FAC",
            "FECHA_INICIO": "2016-06-08",
            "FECHA_FINAL": "2016-06-08",
            "ANTIGUEDAD": "12",
            "MONTO_LOCAL": 4000.00000000,
            "SALDO_LOCAL": 4000.00000000,
            "MONTO_PROYECTADO": 4000.00000000,
            "DESCUENTO": 0.00000000,
            "MONTO_CON_DESCUENTO": 4000.00000000
        },
        {
            "ID": 8,
            "CODIGO": "3101691000",
            "VENDEDOR": "7004",
            "DOCUMENTO": "004357",
            "CLIENTE": "Botica Internacional de Costa Rica BICR S.A.",
            "ALIAS": "HSJD",
            "VENDEDOR_NOMBRE": "BRENDA OLIVER",
            "TIPO": "FAC",
            "FECHA_INICIO": "2016-06-08",
            "FECHA_FINAL": "2016-06-08",
            "ANTIGUEDAD": "12",
            "MONTO_LOCAL": 588073.50000000,
            "SALDO_LOCAL": 588073.50000000,
            "MONTO_PROYECTADO": 588073.50000000,
            "DESCUENTO": 0.00000000,
            "MONTO_CON_DESCUENTO": 588073.50000000
        },
        {
            "ID": 9,
            "CODIGO": "3105670569",
            "VENDEDOR": "7002",
            "DOCUMENTO": "PF000628",
            "CLIENTE": "Panadería Angelicus",
            "ALIAS": "GUTIERREZ RODRIGUEZ LUIS ARMANDO",
            "VENDEDOR_NOMBRE": "HEILEN DURAN HERNANDEZ",
            "TIPO": "FAC",
            "FECHA_INICIO": "2016-06-08",
            "FECHA_FINAL": "2016-06-08",
            "ANTIGUEDAD": "12",
            "MONTO_LOCAL": 1786405.70000000,
            "SALDO_LOCAL": 1786405.70000000,
            "MONTO_PROYECTADO": 1786405.70000000,
            "DESCUENTO": 0.00000000,
            "MONTO_CON_DESCUENTO": 1786405.70000000
        }]
    },
    {
        "ID": 3,
        "SEGUIMIENTO": "SEG00001",

        "NOMBRE_CLIENTE": "Macrobiotica Gotitas Salud",
        "VENDEDOR": "7009",
        "VENDEDOR_NOMBRE": "GRACE QUESADA",
        "comentario": "null",
        "ESTADO": "Sin aprobar",
        "TIPO": "REC",
        "TEF": "564755",
        "DOCUMENTO": "5179",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Sinpe móvil", "BANCO": "BAC",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 474420.80000000,
        "NOTAS": "",
        "FACTURAS": []
    },
    {
        "ID": 5,
        "SEGUIMIENTO": "SEG00001",
        "NOMBRE_CLIENTE": "Corporacion de Supermercados Unidos S.R.L",
        "VENDEDOR": "7009",
        "VENDEDOR_NOMBRE": "GRACE QUESADA",
        "comentario": "null",
        "ESTADO": "No depositado",
        "TIPO": "REC",
        "TEF": "",
        "DOCUMENTO": "5180",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Cheque", "BANCO": "",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 237210.40000000,
        "NOTAS": "",
        "FACTURAS": []
    },
    {
        "ID": 4,
        "SEGUIMIENTO": "SEG00001",
        "NOMBRE_CLIENTE": "Macrobiotica Gotitas Salud",
        "VENDEDOR": "7009",
        "VENDEDOR_NOMBRE": "GRACE QUESADA",
        "comentario": "null",
        "ESTADO": "No depositado",
        "TIPO": "REC",
        "TEF": "",
        "DOCUMENTO": "5181",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Efectivo", "BANCO": "",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 100000.00000000,
        "NOTAS": "",
        "FACTURAS": []
    },
    {
        "ID": 5,
        "SEGUIMIENTO": "SEG00001",
        "NOMBRE_CLIENTE": "Botica Internacional de Costa Rica BICR S.A.",
        "VENDEDOR": "7009",
        "VENDEDOR_NOMBRE": "GRACE QUESADA",
        "comentario": "null",
        "ESTADO": "Sin aprobar",
        "TIPO": "REC",
        "TEF": "657765",
        "DOCUMENTO": "3251",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Transferencia", "BANCO": "BAC",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 586943.50000000,
        "NOTAS": "",
        "FACTURAS": []
    },
    {
        "ID": 6,
        "SEGUIMIENTO": "SEG00001",
        "NOMBRE_CLIENTE": "Macrobiotica Gotitas Salud",
        "VENDEDOR": "7009",
        "VENDEDOR_NOMBRE": "GRACE QUESADA",
        "comentario": "null",
        "ESTADO": "Sin aprobar",
        "TIPO": "REC",
        "TEF": "434221",
        "DOCUMENTO": "3252",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Efectivo", "BANCO": "BAC",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 258947.10000000,
        "NOTAS": "",
        "FACTURAS": []
    },
    {
        "ID": 7,
        "SEGUIMIENTO": "SEG00001",
        "NOMBRE_CLIENTE": "Macrobiotica Gotitas Salud",
        "VENDEDOR": "7010",
        "VENDEDOR_NOMBRE": "LIZ RODRIGUEZ BADILLA",
        "comentario": "null",
        "ESTADO": "Sin aprobar",
        "TIPO": "REC",
        "TEF": "231222",
        "DOCUMENTO": "0521",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Efectivo", "BANCO": "BAC",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 781500.00000000,
        "NOTAS": "",
        "FACTURAS": []
    },
    {
        "ID": 8,
        "SEGUIMIENTO": "SEG00001",
        "NOMBRE_CLIENTE": "Macrobiotica Gotitas Salud",
        "VENDEDOR": "7009",
        "VENDEDOR_NOMBRE": "GRACE QUESADA",
        "comentario": "null",
        "ESTADO": "No depositado",
        "TIPO": "REC",
        "TEF": "",
        "DOCUMENTO": "5182",
        "FECHA_SEGUIMIENTO": "2021-03-15",
        "FORMAPAGO": "Cheque", "BANCO": "",
        "MONEDA": "CRC",
        "MONTO_LOCAL": 100000.00000000,
        "NOTAS": "",
        "FACTURAS": []
    }
];


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
            //fixedColumns: {
            //    left: 2
            //},
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },
            data: aprobaciones,

            //serverSide: true,
            //ajax: {
            //    url: "https://preview.keenthemes.com/api/datatables.php",
            //},
            columns: [

                {
                    "data": null,
                    "defaultContent": ``
                },
                {
                    "data": null,
                    "defaultContent": `<a href="javascript:;"><i class="fa fa-caret-right verSubtabla" style="width: 10px;"></i></a>`
                },
                { "data": "NOMBRE_CLIENTE" },
                { "data": "TIPO" },
                { "data": "FORMAPAGO" },
                { "data": "DOCUMENTO" },
                { "data": "MONEDA" },
                { "data": "MONTO_LOCAL" },
                { "data": "MONTOPAGO" },
                { "data": "ESTADO" },
                { "data": "BANCO" },
                { "data": "TEF" },
                { "data": "FECHA_SEGUIMIENTO" },
                { "data": "comentario" },
                { "data": "VENDEDOR" },
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: function (data, type, row) {
                        return `
                            <div class="form-check form-check-sm form-check-custom form-check-solid">
                                <input class="form-check-input" type="checkbox" value="${row.ID}" saldo="${row.MONTO_LOCAL}" />
                            </div>`;
                    }
                },
                {
                    targets: 4,
                    orderable: false,
                    render: function (data, type, row) {
                        var formas = {
                            "Efectivo": { 'title': 'Efectivo', 'class': 'badge-light-dark' },
                            "Sinpe móvil": { 'title': 'Sinpe móvil', 'class': ' badge-light-primary' },
                            "Cheque": { 'title': 'Cheque', 'class': 'badge-light-warning' },
                            "Cheque postfechado": { 'title': 'Cheque postfechado', 'class': ' badge-light-warning' },
                            "Transferencia": { 'title': 'Transferencia', 'class': 'badge-light-success' },
                        };
                        return '<span class="badge badge-lg ' + formas[row.FORMAPAGO].class + '">' + row.FORMAPAGO + '</span>';
                    }
                },
                {
                    targets: 7,
                    orderable: false,
                    render: function (data, type, row) {
                        return `₡${parseFloat(row.MONTO_LOCAL).toLocaleString('en-US')}`;
                    }
                },
                {
                    targets: 8,
                    orderable: false,
                    render: function (data, type, row) {
                        return `<input disabled class="form-control montopagos" type="Number" value="${row.MONTO_LOCAL}" id="montopago${row.ID}" onchange="updateTotal()" id="row${row.DOCUMENTO}"></>`
                    }
                },
                {
                    targets: 9,
                    orderable: false,
                    render: function (data, type, row) {
                        var estado = {
                            "Aprobado": { 'title': 'Aprobado', 'class': ' badge-info' },
                            "Sin aprobar": { 'title': 'Sin aprobar', 'class': ' badge-light-info' },
                            "Rechazado": { 'title': 'Rechazado', 'class': ' badge-light-danger ' },
                            "No depositado": { 'title': 'No depositado', 'class': ' badge-light-danger text-danger ' },
                        };
                        return '<span class="badge font-weight-bold badge-lg ' + estado[row.ESTADO].class + ' label-inline">' + row.ESTADO + '</span>';
                    }
                },
                {
                    targets: 14,
                    orderable: false,
                    render: function (data, type, row) {
                        return row.VENDEDOR + '-' + row.VENDEDOR_NOMBRE
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
            //console.log("Hola")
            ////$('#totallineas').html(`Cant. Seg: ${dt.data().length}`);
            //const container = document.querySelector('#miTabla');
            //const checkboxes = container.querySelectorAll('[type="checkbox"]');
            //// Toggle delete selected toolbar
            //checkboxes.forEach(c => {
            //    // Checkbox on click event
            //    c.addEventListener('click', function () {
            //        setTimeout(function () {
            //            ManejarCantidadCheckboxes();
            //        }, 50);
            //    });
            //});
        });
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

    $('#miTabla').on('click', 'input[type="checkbox"]', function () {
        // Check or uncheck the "checkAll" checkbox in the header based on the checkboxes in the body
        var allChecked = dt.column(0).nodes().to$().find('input[type="checkbox"]:checked').length === dt.data().length;
        $('#checkAll').prop('checked', allChecked);
    });


    var Filtros = function () {

        $('#estado').on('change', function () {
            const val = $(this).val() == '0' ? '' : $(this).val()
            dt.search(val).draw();
        });

        $('#FiltroEstado').on('change', function () {
            const val = $(this).val() == '0' ? '' : $(this).val()
            dt.search(val).draw();
        });
        $('#tipo').on('change', function () {
            const val = $(this).val() == '0' ? '' : $(this).val()
            dt.search(val).draw();
        });
        $('#colaborador').on('change', function () {
            const val = $(this).val() == '0' ? '' : $(this).val()
            dt.search(val).draw();
        });
        $('#kt_datatable_search_query').on('keyup', function () {
            //const val = $(this).val() == '0' ? '' : $(this).val()
            dt.search($(this).val()).draw();
        });
        //const filterSearch = document.querySelector('[data-kt-docs-table-filter="search"]');
        //filterSearch.addEventListener('keyup', function (e) {
        //    dt.search(e.target.value).draw();
        //});
    }

    var ListenerCheckboxes = () => {
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
                    <table class="table  table-row-dashed fs-6 gy-4" id="subtabla${datos.ID}">
                        <thead>
                            <tr class="text-start text-gray-600 fw-bold fs-7 text-uppercase gs-0">
                                 <th class="text-left min-w-50px">Tipo</th>
                                <th class="text-left min-w-10px ">Documento</th>
                                <th class="text-left min-w-10px ">Fecha doc</th>
                                <th class="text-left min-w-10px">Fecha ven</th>
                                <th class="text-left min-w-50px ">Venc</th>
                                <th class="text-left min-w-10px">Moneda</th>
                                <th class="text-left min-w-10px">Monto</th>
                                <th class="text-left min-w-20px">Monto aplicado</th>
                                <th class="text-left min-w-400px">Saldo</th>
                            </tr>
                        </thead>
                        <tbody class="text-gray-600 fw-semibold">
                        </tbody>
                    </table>
                `).show();
                $(`#subtabla${datos.ID}`).DataTable({
                    data: datos.FACTURAS,
                    destroy: true,
                    columns: [


                        { "data": "TIPO" },
                        { "data": "DOCUMENTO" },
                        { "data": "FECHA_INICIO" },
                        { "data": "FECHA_FINAL" },
                        { "data": "ANTIGUEDAD" },
                        { "data": null },
                        { "data": "MONTO_LOCAL" },
                        { "data": "MONTOPAGO" },
                        { "data": "SALDO_LOCAL" },
                    ],
                    columnDefs: [

                        {
                            targets: 1,
                            orderable: false,
                            render: function (data, type, row) {
                                var status = {
                                    "FAC": { 'title': 'FAC', 'class': 'badge-light-success' },
                                    "REC": { 'title': 'REC', 'class': ' badge-light-warning' },
                                    "TEF": { 'title': 'TEF', 'class': ' badge-light-info' },
                                    "N/C": { 'title': 'N/C', 'class': ' badge-light-primary' },
                                    "N/D": { 'title': 'N/D', 'class': ' badge-light-success' },
                                    "O/C": { 'title': 'O/C', 'class': ' badge-light-danger' },
                                    "O/D": { 'title': 'O/D', 'class': ' badge-light-warning' },
                                };


                                return '<span class="badge badge-lg ' + status[row.TIPO].class + '  " >' + status[row.TIPO].title + '</span>';
                            }
                        },
                        {
                            targets: 5,
                            orderable: false,
                            render: function (data, type, row) {
                                return `CRC`;
                            }
                        },
                        {
                            targets: 6,
                            orderable: false,
                            render: function (data, type, row) {
                                return `₡${parseFloat(row.MONTO_LOCAL).toLocaleString('en-US')}`;
                            }
                        },
                        {
                            targets: 7,
                            orderable: false,
                            render: function (data, type, row) {
                                return `₡0`
                            }
                        },
                        {
                            targets: 8,
                            orderable: false,
                            render: function (data, type, row) {
                                return `<span class="text-danger">₡${parseFloat(row.SALDO_LOCAL).toLocaleString('en-US')}</span>`
                            }
                        },

                    ],

                });

                $(`#subtabla${datos.ID}_wrapper`).addClass("ms-20")
                //row.child(construirsubtabla(row.data().LINEAS)).show();
                tr.addClass('shown');
            }
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
        $(".montopagos").attr("disabled", true)
        if (checkedState) {

            var suma = 0.0
            $("#notasaplicar").empty()
            $("#miTabla input:checkbox:checked").each(function () {
                //correosTemp.push($(this).attr('correo'))
                //_correos_marcados.push({
                //    cliente: $(this).val(),
                //    correo: $(this).attr('correo')
                //})
                //let found = aprobaciones.find(element => element.ID == $(this).val())
                //console.log(found)
                //totaltef = parseFloat(totaltef) + parseFloat(found.MONTO_LOCAL)
                //arrayejemplo.push(found)
                //AgregarAAplicar(found)
                $("#montopago" + $(this).val()).attr("disabled", false)
                suma += parseFloat($(this).attr('saldo'))

            });
            console.log(suma)
            //let saldorest = 0
            //saldorest = saldodeNC - totaltef
            //$("#saldonc").text(formatDollar(saldorest.toFixed(2), '₡'))
            //$("#saldomodal").val(formatDollar(saldorest.toFixed(2), '₡'))
            $("#textPorVencerCRC").text(`${count} documentos`)
            $("#montoPorVencerCRC").text(formatDollar(suma.toFixed(2), '₡'))
            $("#total").val(formatDollar(suma.toFixed(2), '₡'))

            //$("#montoexacto").val(formatDollar(suma.toFixed(2), '₡'))
            //$("#totalmodal").val(formatDollar(totaltef.toFixed(2), '₡'))
            //$("#cantselect").text(count)
            $(".multi").removeClass("d-none")
        } else {
            $(".multi").addClass("d-none")
        }

    }

    // Public methods
    return {
        init: function () {
            initDatatable();
            Filtros();
            ListenerCheckboxes();
            subtabla()
        }
    }
}();
function aprobar() {
    $("#CrearTefModal").modal("show")
}
function guardarcambios() {
    $("#CrearTefModal").modal("hide")
    toastr.success(`Cambios guardados con éxito`, "Éxito", toastrOptions)
}
function formatDollar(num, symbol) {
    var isNegative = false;
    if (num < 0) {
        isNegative = true;
        num = Math.abs(num);
    }

    var p = num.toString().split(".");
    var formattedNum = symbol + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
        return num === "-" ? acc : num + (i && !(i % 3) ? "," : "") + acc;
    }, "") + (p[1] === undefined || p[1] === null ? "" : "." + p[1]);

    return isNegative ? "-" + formattedNum : formattedNum;
}
$(document).ready(function () {
    $("#FechaInicioV").daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY"
        }
    });
    new tempusDominus.TempusDominus(document.getElementById("FechaInicio2Seguimiento"), {
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
        },

    });
    KTDatatablesServerSide.init()

});