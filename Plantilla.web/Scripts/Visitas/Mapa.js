

var fechaVisita = $('#fechavisitaMapa').val().split('/').reverse().join('-');
var latitud = null;
var longitud = null;

const ubicacionDefault = {
    latitud: 9.9281,
    longitud: -84.0907
};

function obtenerNumeroCoordenada(valor) {
    const numero = Number(valor);
    return Number.isFinite(numero) ? numero : null;
}

function obtenerCentroMapa() {
    const lat = obtenerNumeroCoordenada(latitud);
    const lng = obtenerNumeroCoordenada(longitud);

    return [
        lat !== null ? lat : ubicacionDefault.latitud,
        lng !== null ? lng : ubicacionDefault.longitud
    ];
}

function llenarCamposUbicacionActual() {
    if (latitud !== null) {
        $('#latitud, #latitudvisita').val(latitud);
    }

    if (longitud !== null) {
        $('#longitud, #longitudvisita').val(longitud);
    }
}

function obtenerUbicacionActual() {
    return new Promise(resolve => {
        if (!navigator.geolocation) {
            resolve();
            return;
        }

        navigator.geolocation.getCurrentPosition(function (position) {
            latitud = position.coords.latitude;
            longitud = position.coords.longitude;
            llenarCamposUbicacionActual();
            resolve();
        }, function () {
            resolve();
        }, {
            enableHighAccuracy: true,
            timeout: 10000,
            maximumAge: 60000
        });
    });
}

function reiniciarMapa() {
    $("#kt_leaflet_3").remove()
    $("#contenedorMapa").append(`<div id="kt_leaflet_3" style="height: 700px;"></div>`)
    KTLeaflet.init();
}


const KTLeaflet = function () {
    let demo3 = function () {
        
        let leafletIcon1 = L.divIcon({
            html: `<i class="fas fa-map-marker-alt icon-10x text-primary" style="font-size: 3rem !important;"></i>`,
            bgPos: [10, 10],
            iconAnchor: [20, 37],
            popupAnchor: [0, -37],
            className: 'leaflet-marker'
        });
        const urlParams = new URLSearchParams(window.location.search);

        let vendedorVisita = urlParams.get('vendedorvisita');
        let clienteVisita = urlParams.get('cliente');

        if (vendedorVisita == 'null' || vendedorVisita == null  || clienteVisita =='null' || clienteVisita == null) {
            vendedorVisita = vendedor;
            clienteVisita = cliente;
        }
        

        const centroMapa = obtenerCentroMapa();
        let mymap = L.map('kt_leaflet_3').setView(centroMapa, 8);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, ' +
                '<a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
                'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
            maxZoom: 18,
            id: 'mapbox/streets-v11',
            tileSize: 512,
            zoomOffset: -1
        }).addTo(mymap);

        let fd = new FormData()
        fd.set('vendedor', vendedorVisita)
        fd.set('fecha', fechaVisita)
        fd.set('cliente', clienteVisita)
        const url = `${vmurl}/DatosUbicacionCliente`
        fetch(url,
            {
                method: 'POST',
                body: fd
            })
            .then(response => response.json())
            .then(data => {
                const markerCoords = [];
                data.forEach(item => {
                    var lat = obtenerNumeroCoordenada(item.LATITUD);
                    var lng = obtenerNumeroCoordenada(item.LONGITUD);
                    var client = item.COD_CLIENTE;
                    var clientName = item.NOM_CLIENTE;
                    var venta = item.TOTAL_A_FACTURAR.toLocaleString();
                    var hora = item.HORA

                    if (lat === null || lng === null) {
                        return;
                    }

                    var marker = L.marker([lat, lng], { icon: leafletIcon1 }).addTo(mymap);
                    marker.bindPopup(`${client}-${clientName} | ${hora}`);
                    
                    markerCoords.push([lat, lng]);
                    
                });

                if (markerCoords.length) {
                    const bounds = L.latLngBounds(markerCoords);
                    mymap.fitBounds(bounds);
                } else {
                    mymap.setView(centroMapa, 13);
                }
            });
        
    }
    
    return {
        
        init: function () {
            
            demo3();
        }
    };
}();

function cambiarFecha() {
    fechaVisita = $('#fechavisitaMapa').val().split('/').reverse().join('-');
    console.log('onchange fecha: ' + fechaVisita)

    reiniciarMapa();
}


function cambiarFiltros() {
    $('#clientesFiltro').on('change', function () {
        let valorSeleccionado = $(this).val();
        cliente = valorSeleccionado.toString();
        reiniciarMapa();
    });


}




jQuery(document).ready(function () {

    $("#fechavisitaMapa").daterangepicker({
        "singleDatePicker": true,
        "locale": {
            "format": "DD/MM/YYYY"
        }
    }).on('apply.daterangepicker', function (ev, picker) {
        let fechaSeleccionada = picker.startDate.format('DD/MM/YYYY');
        cambiarFecha();
    });

    fechaVisita = $('#fechavisitaMapa').val().split('/').reverse().join('-');
    obtenerUbicacionActual().then(() => {
        KTLeaflet.init();
    });

    cambiarFiltros();

});
