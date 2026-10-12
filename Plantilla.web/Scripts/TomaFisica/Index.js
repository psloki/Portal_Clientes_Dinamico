const ESTADO_TF = {
    "0": { 'titulo': 'Activa', 'class': ' label-light-success' },
    "1": { 'titulo': 'Inactiva', 'class': ' label-light-warning' },
    "2": { 'titulo': 'Finalizada', 'class': ' label-light-primary' },
}
let tomas = [

    {
        "CONSECUTIVO": "TF-0000155",
        "ESTADO": "Activa",
        "FECHA_FINAL": "06/12/2021",
        "FECHA_INICIAL": "06/12/2021",
        "RESPONSABLE": "CRC00130",
        "RESPONSABLE_DESCRIPCION": "CRC00130 - BENAVIDEZ COTO JULIO ALBERTO1",
        "UBICACION": "ADMINIST",
        "UBICACION_DESCRIPCION": "ADMINIST - DEPTO. ADMINISTRATIVO",
        "COMITE": "01-01 Metropolitano",
        "AREA": "REDUCCI\u00D3N DE RIESGO LOCAL",
    },
    {
        "CONSECUTIVO": "TF-0000153",
        "ESTADO": "Finalizada",
        "FECHA_FINAL": "03/12/2021",
        "FECHA_INICIAL": "01/12/2021",
        "RESPONSABLE": "CRC00130,CRC00339",
        "RESPONSABLE_DESCRIPCION": "CRC00130 - BENAVIDEZ COTO JULIO ALBERTO1, CRC00339 - ESQUIVEL SANCHEZ JEFFERSON GERARDO",
        "UBICACION": "FINANC",
        "UBICACION_DESCRIPCION": "FINANC - FINANCIERO",
        "COMITE": "01-01 Metropolitano",
        "AREA": "REDUCCI\u00D3N DE RIESGO LOCAL",
    },
    {
        "CONSECUTIVO": "TF-0000148",
        "ESTADO": "Activa",
        "FECHA_FINAL": "29/09/2020",
        "FECHA_INICIAL": "29/09/2020",
        "RESPONSABLE": "CRC00235",
        "RESPONSABLE_DESCRIPCION": "CRC00235 - CASTRO ZUÑIGA LUIS FABIAN",
        "UBICACION": "FINANC",
        "UBICACION_DESCRIPCION": "FINANC - FINANCIERO",
        "COMITE": "01-01 Metropolitano",
        "AREA": "REDUCCI\u00D3N DE RIESGO LOCAL",
    },
    {
        "CONSECUTIVO": "TF-0000145",
        "ESTADO": "Finalizada",
        "FECHA_FINAL": "07/03/2019",
        "FECHA_INICIAL": "06/03/2019",
        "RESPONSABLE": "CRC00381",
        "RESPONSABLE_DESCRIPCION": "CRC00381 - GARCIA GARCIA ANTONIO",
        "UBICACION": "ADMINIST",
        "UBICACION_DESCRIPCION": "ADMINIST - DEPTO. ADMINISTRATIVO",
        "COMITE": "01-01 Metropolitano",
        "AREA": "REDUCCI\u00D3N DE RIESGO LOCAL",
    }
]



var template;

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
            stateSave: true,
            scrollX: true,
            data: tomas,

            //serverSide: true,
            //ajax: {
            //    url: "https://preview.keenthemes.com/api/datatables.php",
            //},
            columns: [

                { "data": "CONSECUTIVO" },
                { "data": "RESPONSABLE_DESCRIPCION" },
                { "data": "UBICACION_DESCRIPCION" },
                { "data": "FECHA_INICIAL" },
                { "data": "FECHA_FINAL" },
                { "data": "ESTADO" },
                {
                    "data": null,
                },
            ],
            columnDefs: [
                
                {
                    targets: 5,
                    orderable: false,
                    render: function (data) {
                        const ESTADO = {
                            "Activa": { 'titulo': 'Activa', 'class': ' badge-light-success ' },
                            "Inactiva": { 'titulo': 'Inactiva', 'class': ' badge-light-warning ' },
                            "Finalizada": { 'titulo': 'Finalizada', 'class': ' badge-light-primary ' },
                        };
                        return `<span class="badge ${ESTADO[data].class} badge-lg">${ESTADO[data].titulo}</span>`;
                    }
                },
                {
                    targets: 6,
                    orderable: false,
                    render: function (data, type, row) {
                        let btns = `<a href="${opurl}/Conteo" class="btn btn-sm btn-clean btn-icon mr-2" title="Ver conteo" >
                            <!--begin::Svg Icon | path: /var/www/preview.keenthemes.com/kt-products/docs/metronic/html/releases/2023-01-26-051612/core/html/src/media/icons/duotune/art/art005.svg-->
                                <span class="svg-icon svg-icon-primary svg-icon-1">
                                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path opacity="0.3" d="M3 6C2.4 6 2 5.6 2 5V3C2 2.4 2.4 2 3 2H5C5.6 2 6 2.4 6 3C6 3.6 5.6 4 5 4H4V5C4 5.6 3.6 6 3 6ZM22 5V3C22 2.4 21.6 2 21 2H19C18.4 2 18 2.4 18 3C18 3.6 18.4 4 19 4H20V5C20 5.6 20.4 6 21 6C21.6 6 22 5.6 22 5ZM6 21C6 20.4 5.6 20 5 20H4V19C4 18.4 3.6 18 3 18C2.4 18 2 18.4 2 19V21C2 21.6 2.4 22 3 22H5C5.6 22 6 21.6 6 21ZM22 21V19C22 18.4 21.6 18 21 18C20.4 18 20 18.4 20 19V20H19C18.4 20 18 20.4 18 21C18 21.6 18.4 22 19 22H21C21.6 22 22 21.6 22 21Z" fill="currentColor"/>
                                <path d="M3 16C2.4 16 2 15.6 2 15V9C2 8.4 2.4 8 3 8C3.6 8 4 8.4 4 9V15C4 15.6 3.6 16 3 16ZM13 15V9C13 8.4 12.6 8 12 8C11.4 8 11 8.4 11 9V15C11 15.6 11.4 16 12 16C12.6 16 13 15.6 13 15ZM17 15V9C17 8.4 16.6 8 16 8C15.4 8 15 8.4 15 9V15C15 15.6 15.4 16 16 16C16.6 16 17 15.6 17 15ZM9 15V9C9 8.4 8.6 8 8 8H7C6.4 8 6 8.4 6 9V15C6 15.6 6.4 16 7 16H8C8.6 16 9 15.6 9 15ZM22 15V9C22 8.4 21.6 8 21 8H20C19.4 8 19 8.4 19 9V15C19 15.6 19.4 16 20 16H21C21.6 16 22 15.6 22 15Z" fill="currentColor"/>
                                </svg>
                            </span>
                            <!--end::Svg Icon-->
                         </a>`
                        if (row.ESTADO == "Activa") {
                            btns += `<a href="javascript:;" data-bs-toggle="modal" data-bs-target="#modalTomaFisica" class="btn btn-sm btn-clean btn-icon mr-2" title="Editar toma fisica" >
	                           <!--begin::Svg Icon | path: /var/www/preview.keenthemes.com/kt-products/docs/metronic/html/releases/2023-01-26-051612/core/html/src/media/icons/duotune/art/art005.svg-->
                                <span class="svg-icon svg-icon-warning svg-icon-1">
                                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path opacity="0.3" d="M21.4 8.35303L19.241 10.511L13.485 4.755L15.643 2.59595C16.0248 2.21423 16.5426 1.99988 17.0825 1.99988C17.6224 1.99988 18.1402 2.21423 18.522 2.59595L21.4 5.474C21.7817 5.85581 21.9962 6.37355 21.9962 6.91345C21.9962 7.45335 21.7817 7.97122 21.4 8.35303ZM3.68699 21.932L9.88699 19.865L4.13099 14.109L2.06399 20.309C1.98815 20.5354 1.97703 20.7787 2.03189 21.0111C2.08674 21.2436 2.2054 21.4561 2.37449 21.6248C2.54359 21.7934 2.75641 21.9115 2.989 21.9658C3.22158 22.0201 3.4647 22.0084 3.69099 21.932H3.68699Z" fill="currentColor"/>
                                    <path d="M5.574 21.3L3.692 21.928C3.46591 22.0032 3.22334 22.0141 2.99144 21.9594C2.75954 21.9046 2.54744 21.7864 2.3789 21.6179C2.21036 21.4495 2.09202 21.2375 2.03711 21.0056C1.9822 20.7737 1.99289 20.5312 2.06799 20.3051L2.696 18.422L5.574 21.3ZM4.13499 14.105L9.891 19.861L19.245 10.507L13.489 4.75098L4.13499 14.105Z" fill="currentColor"/>
                                </svg>
                                </span>
                                <!--end::Svg Icon-->
	                        </a>
                            <a href="javascript:;" onclick="EliminarToma('${row.CONSECUTIVO}')" class="btn btn-sm btn-clean btn-icon mr-2" title="Eliminar toma física" >
	                           <!--begin::Svg Icon | path: /var/www/preview.keenthemes.com/kt-products/docs/metronic/html/releases/2023-01-26-051612/core/html/src/media/icons/duotune/art/art005.svg-->
                                <span class="svg-icon svg-icon-danger svg-icon-1">
                                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M5 9C5 8.44772 5.44772 8 6 8H18C18.5523 8 19 8.44772 19 9V18C19 19.6569 17.6569 21 16 21H8C6.34315 21 5 19.6569 5 18V9Z" fill="currentColor"/>
                                        <path opacity="0.5" d="M5 5C5 4.44772 5.44772 4 6 4H18C18.5523 4 19 4.44772 19 5V5C19 5.55228 18.5523 6 18 6H6C5.44772 6 5 5.55228 5 5V5Z" fill="currentColor"/>
                                        <path opacity="0.5" d="M9 4C9 3.44772 9.44772 3 10 3H14C14.5523 3 15 3.44772 15 4V4H9V4Z" fill="currentColor"/>
                                    </svg>
                                </span>
                                <!--end::Svg Icon-->
	                        </a>`
                        }
                        return btns;


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
            //initToggleToolbar();
            //toggleToolbars();
            //handleDeleteRows();
            //KTMenu.createInstances();
        });
    }

    var Filtros = function () {

        
        $('#responsable').on('change', function () {
            const val = $(this).val() == '0' ? '' : $(this).val()
            dt.columns(1).search(val).draw();
        });
        $('#ubicacion').on('change', function () {
            const val = $(this).val() == '0' ? '' : $(this).val()
            dt.columns(2).search(val).draw();
        });
        $('#estado').on('change', function () {
            const val = $(this).val() == 'Todos' ? '' : $(this).val()
            dt.columns(5).search(val).draw();
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



    // Public methods
    return {
        init: function () {
            initDatatable();
            Filtros();
        }
    }
}();

function EliminarToma(toma) {
    Swal.fire({
        text: `¿Desea eliminar la toma física ${toma}?`,
        icon: "warning",
        showCancelButton: true,
        buttonsStyling: false,
        confirmButtonText: "Eliminar",
        cancelButtonText: "Cancelar",
        customClass: {
            confirmButton: "btn fw-bold btn-danger",
            cancelButton: "btn fw-bold btn-active-secondary"
        }
    }).then(function (result) {
        if (result.value) {
            // Simulate delete request -- for demo purpose only

        } else if (result.dismiss === 'cancel') {

        }
    });
}
function GuardarToma() {

    $("#modalTomaFisica").modal("hide")

    toastr.success('Toma física creada con éxito', 'Éxito', toastrOptions)
}
$(document).ready(function () {
    new tempusDominus.TempusDominus(document.getElementById("fecharealizacion"), {
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
    new tempusDominus.TempusDominus(document.getElementById("fechafinaliza"), {
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
    $("#kt_daterangepicker_1").daterangepicker();
    //initDropzone()
    KTDatatablesServerSide.init()

});