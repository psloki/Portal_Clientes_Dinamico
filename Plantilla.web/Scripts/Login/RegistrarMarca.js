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



$(document).ready(() => {
    // Ejecutamos la función 'CargarReloj' 
    cargarReloj();
    $('#TipoMarca').select2({
        placeholder: 'Seleccione un tipo de marca'
    })
    $('#Trabajo').select2({
        placeholder: 'Seleccione una cotización'
    })

    $('#regresar').on('click', e => {
        e.preventDefault()
        window.location.href = `${rurl}`
    })

    $('#registrar_marca').on('click', e => {
        e.preventDefault()
        Swal.fire({
            title: 'Marca registrada',
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