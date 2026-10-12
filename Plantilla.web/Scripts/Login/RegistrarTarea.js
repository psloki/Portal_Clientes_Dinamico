function cargarReloj() {

    // Haciendo uso del objeto Date() obtenemos la hora, minuto y segundo 
    let fechahora = new Date();
    let dia = fechahora.getDate();
    let mes = fechahora.getMonth();
    let anio = fechahora.getFullYear();
    let hora = fechahora.getHours();
    let minuto = fechahora.getMinutes();
    let segundo = fechahora.getSeconds();

    // Variable meridiano con el valor 'AM' 
    let meridiano = "AM";


    // Si la hora es igual a 0, declaramos la hora con el valor 12 
    if (hora == 0) {

        hora = 12;

    }

    // Si la hora es mayor a 12, restamos la hora - 12 y mostramos la variable meridiano con el valor 'PM' 
    if (hora > 12) {

        hora = hora - 12;

        // Variable meridiano con el valor 'PM' 
        meridiano = "PM";

    }

    if (dia < 10) {
        dia = `0${dia}`
    }

    mes++
    if (mes < 10) {
        mes = `0${mes}`
    }

    // Formateamos los ceros '0' del reloj 
    hora = (hora < 10) ? "0" + hora : hora;
    minuto = (minuto < 10) ? "0" + minuto : minuto;
    segundo = (segundo < 10) ? "0" + segundo : segundo;

    // Enviamos la hora a la vista HTML 
    let tiempo = `${dia}/${mes}/${anio} ${hora}:${minuto}:${segundo} ${meridiano}`//hora + ":" + minuto + ":" + segundo + " " + meridiano;
    document.getElementById("relojnumerico").innerText = tiempo;
    document.getElementById("relojnumerico").textContent = tiempo;

    // Cargamos el reloj a los 500 milisegundos 
    setTimeout(cargarReloj, 500);

}

var KTKBootstrapTouchspin = function () {

    // Private functions
    var demos = function () {
        // minimum setup
        $('#horas').TouchSpin({
            buttondown_class: 'btn btn-secondary',
            buttonup_class: 'btn btn-secondary',

            min: 0,
            max: 24,
            step: 1,
            decimals: 0,
            boostat: 5,
            maxboostedstep: 10,
        });
        // minimum setup
        $('#minutos').TouchSpin({
            buttondown_class: 'btn btn-secondary',
            buttonup_class: 'btn btn-secondary',

            min: 0,
            max: 59,
            step: 1,
        });

    }

    return {
        // public functions
        init: function () {
            demos();
        }
    };
}();

var KTBootstrapDatepicker = function () {

    var arrows;
    if (KTUtil.isRTL()) {
        arrows = {
            leftArrow: '<i class="la la-angle-right"></i>',
            rightArrow: '<i class="la la-angle-left"></i>'
        }
    } else {
        arrows = {
            leftArrow: '<i class="la la-angle-left"></i>',
            rightArrow: '<i class="la la-angle-right"></i>'
        }
    }

    // Private functions
    var demos = function () {
        // minimum setup
        $('.fecha').datepicker({
            rtl: KTUtil.isRTL(),
            todayHighlight: true,
            orientation: "bottom left",
            templates: arrows
        });

    }

    return {
        // public functions
        init: function () {
            demos();
        }
    };
}();
$(document).ready(() => {
    // Ejecutamos la función 'CargarReloj' 
    KTKBootstrapTouchspin.init()
    KTBootstrapDatepicker.init()
    cargarReloj();
    $('#TipoMarca').select2({
        placeholder: 'Seleccione un tipo de marca'
    })
    $('#Trabajo').select2({
        placeholder: 'Seleccione una cotización'
    })
    $('#tipotrabajo').select2({
        placeholder: 'Seleccione un tipo de trabajo'
    })
    $('#cliente').select2({
        placeholder: 'Seleccione un cliente'
    })
    $('#linea').select2({
        placeholder: 'Seleccione una línea de trabajo'
    })

    $('#regresar').on('click', e => {
        e.preventDefault()
        window.location.href = `${rurl}`
    })

    $('#registrar_marca').on('click', e => {
        e.preventDefault()
        Swal.fire({
            title: 'Horas registradas con éxito',
            text: '',
            icon: 'success',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: "Cancelar",
            confirmButtonText: 'Aceptar'
        }).then((result) => {
            if (result.value) {
                window.location.reload()
            }
        })
    })
    $('#registrar_marcasalir').on('click', e => {
        e.preventDefault()
        Swal.fire({
            title: 'Horas registradas con éxito',
            text: '',
            icon: 'success',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: "Cancelar",
            confirmButtonText: 'Aceptar'
        }).then((result) => {
            if (result.value) {
                window.location.href = `${rurl}`
            }
        })
    })
})