
function wireMultiSelect(table, opts = {}) {
    const {
        checkAllSelector = '#checkAll',
        rowCheckboxSelector = '.form-check-input.row-checkbox',
        btnMultiSelector = '.app-engage-btn.multi',
        rowKeyFn = row => row.id
    } = opts;

    const selected = new Set();
    let filteredTotal = 0;
    let filteredMarked = 0;

    // 1) Al redibujar (draw), restauramos checks y recalcualmos totales
    table.on('draw.dt', () => {
        // Restaurar sólo la página visible
        table.rows({ page: 'current' }).every(function () {
            const key = rowKeyFn(this.data());
            $(this.node())
                .find(rowCheckboxSelector)
                .prop('checked', selected.has(key));
        });


        // Recalcular cuántas filas cumplen el filtro y cuántas de ellas están marcadas
        filteredTotal = table.rows({ search: 'applied' }).count();
        filteredMarked = 0;
        table.rows({ search: 'applied' }).every(function () {
            if (selected.has(rowKeyFn(this.data()))) filteredMarked++;
        });

        // Actualizar header y botón
        $(checkAllSelector).prop(
            'checked',
            filteredTotal > 0 && filteredMarked === filteredTotal
        );
        $(btnMultiSelector).toggleClass('d-none', selected.size <= 1);
    });

    // 2) Click individual
    table.on('click', rowCheckboxSelector, function () {
        const key = this.value;
        if (this.checked) {
            selected.add(key);
            filteredMarked++;
        } else {
            selected.delete(key);
            filteredMarked--;
        }
        // Sólo actualizamos header y botón, sin recorrer nada más
        $(checkAllSelector).prop(
            'checked',
            filteredTotal > 0 && filteredMarked === filteredTotal
        );
        $(btnMultiSelector).toggleClass('d-none', selected.size <= 1);
    });

    // 3) Click “checkAll”
    $(checkAllSelector).on('click', function () {
        const checked = this.checked;
        selected.clear();

        if (checked) {
            // Seleccionar todas las filtradas
            table.rows({ search: 'applied' }).every(function () {
                selected.add(rowKeyFn(this.data()));
            });
            filteredMarked = filteredTotal;
        } else {
            filteredMarked = 0;
        }

        // Marcar sólo la página actual
        table.rows({ page: 'current' }).every(function () {
            $(this.node())
                .find(rowCheckboxSelector)
                .prop('checked', checked);
        });

        $(btnMultiSelector).toggleClass('d-none', selected.size <= 1);
    });

    return {
        getSelectedKeys: () => Array.from(selected),
        clear: () => {
            selected.clear();
            filteredTotal = filteredMarked = 0;
            table.rows().nodes().to$().find(rowCheckboxSelector).prop('checked', false);
            $(checkAllSelector).prop('checked', false);
            $(btnMultiSelector).addClass('d-none');
        }
    };
}


$(document).ready(() => {
    $('#CIA').select2({
        placeholder: "Seleccione una compañía",
    });
    $('#CIA').on('change', () => {
        CambiarCIA()
    })

    $('#vendedorFiltro').on('change', () => {
        //if (cambiarVendedor)
            CambiarVendedorSession();
    })

    $('#cobradorFiltro').on('change', () => {
        //if (cambiarCobrador)
        cambiarCobradorSession();
    })

    // CambiarCliente()
    $("#spancliente").text($("#clientesFiltro option:selected").text())

    //$('#vendedorFiltro').on('change', () => {
    //    $("#spanvendedor").text($("#vendedorFiltro option:selected").text())
    //})
})


const select2esp = {
    noResults: function () {
        return 'No hay resultados'
    },
    searching: function () {
        return 'Buscando...'
    },
    inputTooShort: function () {
        return 'Ingrese al menos 2 caracteres'
    }
}
function b64DecodeUnicode(str) {
    return decodeURIComponent(
        atob(str)
            .split('')
            .map(c => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
            .join('')
    );
}
function safeDecode(b64) {
    if (typeof b64 !== 'string' || !b64.trim()) {
        return '';         
    }
    try {
        if (!/^[A-Za-z0-9+/=]+$/.test(b64)) {
            throw new Error('Not valid Base64');
        }
        return b64DecodeUnicode(b64);
    } catch (e) {
        return b64;        
    }
}
const toastrOptions = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

function Volver() {
    if (window.history.length == 1) {
        window.close()
    } else {
        window.history.back()
    }
}

function CambiarCIA() {
    let fd = new FormData()
    fd.set('cia', $('#CIA').val())
    fetch(`${HOME_URL}/CambiarCIA`,
        {
            method: 'POST',
            body: fd
        })
        .then(res => res.json())
        .then(data => {
            if (data == 1) {
                window.location.reload()
            } else {
                toastr.warning('Compañía no válida', 'Alerta', toastrOptions)
            }
        })
        .catch(err => {
            console.error(err)
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions)
        })
}

function mostrarToast(tipo, texto) {

    const toast = bootstrap.Toast.getOrCreateInstance($("#kt_docs_toast_toggle"));
    $("#toasttype").text(tipo)
    $("#toasttext").text(texto)
    toast.show();
}

function scrollToTableRight() {
    const table = document.querySelector('.dataTables_scrollBody')
    if (table) {
        //table.scrollTo(table.scrollWidth, 0)
        table.scrollTo({
            left: 2500
        })
    }
}

function scrollToTableLeft() {
    const table = document.querySelector('.dataTables_scrollBody')
    if (table) {
        //table.scrollTo(0, table.scrollWidth)
        table.scrollTo({
            left: 0
        })
    }
}

function Volver() {
    if (window.history.length == 1) {
        window.close()
    } else {
        window.history.back()
    }
}

function Dajax(url, method, data, callback, typeSync = false) {
    $.ajax({
        url: url,
        method: method,
        async: typeSync,
        data: data,
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/x-www-form-urlencoded;",
        },
        beforeSend: function () {
            // habilita la imagen Cargando
            //LoadingOn();
        },
        error: function (jqXHR, exception) {
            var error = "";
            if (jqXHR.status === 0) {
                error = 'No conectado.\nVerifique su conexión de red.';
            } else if (jqXHR.status == 404) {
                error = 'La página solicitada no fue encontrada. [404]';
            } else if (jqXHR.status == 500) {
                error = 'Error interno del servidor [500].';
            } else if (exception === 'parsererror') {
                error = 'Fallo la conversión a JSON.';
            } else if (exception === 'timeout') {
                error = 'Error de tiempo de espera.';
            } else if (exception === 'abort') {
                error = 'Solicitud Ajax abortada.';
            } else {
                error = 'Error no detectado.\n' + jqXHR.responseText;
            }
            //MToasT("E", error, "¡Error!");
        },
        complete: function (response) {
            // quitar imagen cargando
            // LoadingOff();
            // regresa los datos obtenidos.
            callback(response);
        }
    });
}

function CambiarClienteSession() {
    const textoSel = $("#clientesFiltro option:selected").text();
    const [cod, nombre = 'Todos'] = textoSel.split(' - ').map(t => t?.trim());

    const fd = new FormData();
    fd.set('cliente', $('#clientesFiltro').val());
    fd.set('nombrevendedor', nombre || 'Todos');

    fetch(`${HOME_URL}/CambiarCliente`, { method: 'POST', body: fd })
        .then(r => r.json())
        .then(ok => {
            
            if (ok == 1) {
                $(document).trigger('filtros-actualizados', {
                    vendedor: $('#vendedorFiltro').val(),
                    cobrador: $('#cobradorFiltro').val(),
                    cliente: $('#clientesFiltro').val(),
                });
            } else {
                toastr.warning('Cliente no válido', 'Alerta', toastrOptions);
            }
        })
        .catch(() => toastr.error('Error en la petición', 'Error', toastrOptions));
}

function CambiarVendedorSession() {
    const textoSel = $("#vendedorFiltro option:selected").text();
    const [cod, nombre = 'Todos'] = textoSel.split(' - ').map(t => t?.trim());

    const fd = new FormData();
    fd.set('vendedor', $('#vendedorFiltro').val());
    fd.set('nombrevendedor', nombre || 'Todos');

    //if ($('#vendedorFiltro').val() === '0')
    //    $('#clientesFiltro').val('0').trigger("change.select2");

    console.log($('#vendedorFiltro').val())
    fetch(`${HOME_URL}/CambiarVenedorSession`, { method: 'POST', body: fd })
        .then(r => r.json())
        .then(ok => {
            if (ok == 1) {
                $(document).trigger('filtros-actualizados', {
                    vendedor: $('#vendedorFiltro').val(),
                    cobrador: $('#cobradorFiltro').val(),
                    cliente: $('#clientesFiltro').val()

                });
            } else {
                toastr.warning('Vendedor no válido', 'Alerta', toastrOptions);
            }
        })
        .catch(() => toastr.error('Error en la petición', 'Error', toastrOptions));
}

function CambiarVendedorSession2() {
    const textoSel = $("#vendedorFiltro option:selected").text();
    const [cod, nombre = 'Todos'] = textoSel.split(' - ').map(t => t?.trim());

    const fd = new FormData();
    fd.set('vendedor', $('#vendedorFiltro').val());
    fd.set('nombrevendedor', nombre || 'Todos');

    //if ($('#vendedorFiltro').val() === '0')
    //    $('#clientesFiltro').val('0').trigger("change.select2");

    console.log($('#vendedorFiltro').val())
    
    fetch(`${HOME_URL}/CambiarVenedorSession`, { method: 'POST', body: fd })
        .then(r => r.json())
        .then(ok => {
            if (ok == 1) {
                $(document).trigger('vendedores-actualizados', {
                    vendedor: $('#vendedorFiltro').val(),
                    cobrador: $('#cobradorFiltro').val()
                });
            } else {
                toastr.warning('Vendedor no válido', 'Alerta', toastrOptions);
            }
        })
        .catch(() => toastr.error('Error en la petición', 'Error', toastrOptions));
}

function cambiarCobradorSession() {
    const textoSel = $("#cobradorFiltro option:selected").text();
    const [cod, nombre = 'Todos'] = textoSel.split(' - ').map(t => t?.trim());

    const fd = new FormData();
    fd.set('cobrador', $('#cobradorFiltro').val());
    fd.set('nombrecobrador', nombre || 'Todos');

    //if ($('#cobradorFiltro').val() === '0')
    //    $('#clientesFiltro').val('0').trigger("change.select2");

    console.log($('#cobradorFiltro').val())
    fetch(`${HOME_URL}/CambiarCobradorSession`, { method: 'POST', body: fd })
        .then(r => r.json())
        .then(ok => {
            if (ok == 1) {
                $(document).trigger('filtros-actualizados', {
                    vendedor: $('#vendedorFiltro').val(),
                    cobrador: $('#cobradorFiltro').val(),
                    cliente: $('#clientesFiltro').val()
                });
            } else {
                toastr.warning('Cobrador no válido', 'Alerta', toastrOptions);
            }
        })
        .catch(() => toastr.error('Error en la petición', 'Error', toastrOptions));
}


function formatDate(dateStr) {
    const date = new Date(dateStr + 'T00:00:00');
    date.setHours(0, 0, 0, 0);
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}
function createDatePicker(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        new tempusDominus.TempusDominus(element, {
            localization: {
                locale: 'es',
                format: 'dd/MM/yyyy'
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
            }
        });
    } else {
        console.warn(`Element with id ${elementId} not found`);
    }
}
function validarRangoFechasGenerico(fechaIniStr, fechaFinStr) {
    if (!fechaIniStr || !fechaFinStr) {
        return false;
    }

    var partesIni = fechaIniStr.split('/');
    var partesFin = fechaFinStr.split('/');

    if (partesIni.length !== 3 || partesFin.length !== 3) {
        return false;
    }

    var diaIni = parseInt(partesIni[0], 10);
    var mesIni = parseInt(partesIni[1], 10) - 1; 
    var anioIni = parseInt(partesIni[2], 10);

    var diaFin = parseInt(partesFin[0], 10);
    var mesFin = parseInt(partesFin[1], 10) - 1;
    var anioFin = parseInt(partesFin[2], 10);

    var dateIni = new Date(anioIni, mesIni, diaIni);
    var dateFin = new Date(anioFin, mesFin, diaFin);

    return dateIni <= dateFin;
}
function validarCantidad(valor, nombreCampo) {
    if (valor === "" || valor === null || parseFloat(valor) < 0) {
        toastr.warning(`Por favor llenar el campo ${nombreCampo} con un valor numérico válido`, "Error", toastrOptions);
        return false;
    }
    return true;
}

function validarEnteroEnRango(valor, nombre, min, max) {
    let numero = Number(valor);

    if (!Number.isInteger(numero)) {
        toastr.warning(`Por favor, ingrese un valor entero válido para ${nombre}`, "Error", toastrOptions);
        return false;
    }

    if (numero < min || numero > max) {
        toastr.warning(`Por favor, ingrese un valor entero para ${nombre} entre ${min} y ${max}`, "Error", toastrOptions);
        return false;
    }

    return true;
}
function validarRangoCantidades(cantMin, cantMax) {
    var numMin = Number(cantMin);
    var numMax = Number(cantMax);

    if (!Number.isInteger(numMin) || !Number.isInteger(numMax)) {
        toastr.warning("Las cantidades deben ser números enteros.", "Error", toastrOptions);
        return false;
    }

    if (isNaN(numMin) || isNaN(numMax)) {
        toastr.warning("Las cantidades deben ser valores numéricos válidos.", "Error", toastrOptions);
        return false;
    }

    if (numMin > numMax) {
        toastr.warning("La cantidad mínima no puede ser mayor que la cantidad máxima.", "Error", toastrOptions);
        return false;
    }
    return true;
}
function validarPrecioEntero(valor) {
    let regex = /^\d+$/;
    return regex.test(valor);
}
function formatearNumero(valor) {
    return parseFloat(valor).toLocaleString('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 8
    });
}

function formatearNumero2Digitos(valor) {
    return parseFloat(valor).toLocaleString('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

function limpiarFormato(valor) {
    return valor.replace(/,/g, '');
}

function validarPrecioDecimal(valor) {
    return /^\d+(\.\d{1,8})?$/.test(valor.trim());
}

function AbrirFiltros() {
    setTimeout(() => {
        $("#kt_header_search_toggle").trigger("click")}, 50)
}

function inicializarSelectConBusquedaYTodos(selector, placeholder = 'Seleccione una opción') {
    $(selector).select2({
        matcher: function (params, data) {
            if ($.trim(params.term) === '') {
                return data;
            }

            const term = params.term.toLowerCase();
            const words = term.split(' ');
            const text = data.text.toLowerCase();

            const allMatch = words.every(function (word) {
                return text.includes(word);
            });

            return allMatch ? data : null;
        },
        placeholder: placeholder,
        allowClear: true
    });

    $(selector).on('select2:clear', function () {
        $(this).val('0').trigger('change');
    });
}

function inicializarSelectMultipleConTodos(selector,
    placeholder = 'Seleccione una opción') {

    const $sel = $(selector);

    /*── Init Select2 ───────────────────────────────────────────────*/
    $sel.select2({
        placeholder,
        allowClear: true,
        closeOnSelect: false,
        language: { noResults: () => 'No hay resultados' }
    });

    /*── Limpia la caja de búsqueda al seleccionar algo ────────────*/
    $sel.on('select2:select', () =>
        $('.select2-container--open .select2-search__field')
            .val('').trigger('input'));

    /*──────────────── Evento CLEAR (clic en la X) ─────────────────*/
    $sel.on('select2:clear', function () {
        // fuerza a "Todos" y notifica
        $(this).val(['0'])
            .trigger('change.select2')   // refresca la UI de select2
            .trigger('change');          // evento normal para tu lógica
    });

    /*── Lógica Todos/Individuales para select & unselect ──────────*/
    $sel.on('select2:select select2:unselect', function (e) {

        let vals = $(this).val() || [];      // estado actual

        // Si no queda nada -> vuelve a 0
        if (vals.length === 0) {
            $(this).val(['0'])
                .trigger('change.select2')
                .trigger('change');
            return;
        }

        // ¿Se acaba de seleccionar "0"? -> quitar los demás
        if (e.type === 'select2:select' && e.params.data.id === '0') {
            $(this).val(['0'])
                .trigger('change.select2')
                .trigger('change');
            return;
        }

        // Si ahora hay una mezcla 0 + otros -> quitar 0 y mantener los otros
        if (vals.includes('0') && vals.length > 1) {
            vals = vals.filter(v => v !== '0');
            $(this).val(vals)
                .trigger('change.select2')
                .trigger('change');
        } else {
            // No hay conflicto: sólo notifica el cambio normal
            $(this).trigger('change');
        }
    });

    /*── Valor inicial = Todos ──────────────────────────────────────*/
    $sel.val(['0']).trigger('change.select2');
}
function cargarSelectPorAjax({
    selector,
    url,
    params = {},
    includeTodos = true,
    todosText = 'Todos',
    todosValue = '0'
}) {
    $.ajax({
        url,
        method: 'GET',
        data: params,
        success: data => {
            const $select = $(selector);
            $select.empty();

            if (includeTodos) {
                $select.append(`<option value="${todosValue}" selected>${todosText}</option>`);
            }

            data.forEach(item => {
                const value = item.ARTICULO ?? item.ID ?? item.value;
                const text = item.DESCRIPCION ?? item.text ?? '';
                $select.append(`<option value="${value}">${value} | ${text}</option>`);
            });

            $select.trigger('change.select2');
        },
        error: () => toastr.warning('Error cargando datos', 'Error', toastrOptions)
    });



}
