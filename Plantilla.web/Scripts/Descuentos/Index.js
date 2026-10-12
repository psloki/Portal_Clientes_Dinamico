let nivelPrecioDesc = $('#nivelprecioDesc').val() || '0';
let articuloDesc = $('#articulosDesc').val() || '0';
let concasaDesc = '0';
let consubCasaDesc = '0';
let selDescCli;
let descuentoSeleccionado = null;

const permisosDescuentos = (PERMISOS_MOD_DESCUENTOS || '')
    .split(',')
    .map(p => p.trim());

const puedeEditarDescuento = permisosDescuentos.includes('81');
const puedeEliminarDescuento = permisosDescuentos.includes('83');

function bloquearControlDescuentoNivel(control) {
    if (!control || control.dataset.accionBloqueada === 'true') return null;
    control.dataset.accionBloqueada = 'true';
    control.disabled = true;
    return function () {
        delete control.dataset.accionBloqueada;
        control.disabled = false;
    };
}

const KTDatatablesDescuento = function () {
    let dt;

    const initDatatable = function () {
        dt = $('#miTabla').DataTable({
            searchDelay: 500,
            processing: true,
            scrollX: true,
            pageLength: 100,
            select: {
                style: 'multi',
                selector: 'td:first-child input[type="checkbox"]',
                className: 'row-selected'
            },
            order: [],
            bDestroy: true,
            destroy: true,
            ajax: {
                url: `${_urlDesc}/obtenerClientArticuloDctoIndex`,
                dataSrc: '',
                contentType: 'application/json',
                data: function (d) {
                    d.articulo = articuloDesc;
                    d.nivelPrecio = nivelPrecioDesc;
                    d.casa = concasaDesc;
                    d.subcasa = consubCasaDesc;
                }
            },
            columns: [
                { data: 'NIVEL_PRECIO', defaultContent: '' },
                { data: 'NIVEL_PRECIO', defaultContent: '' },
                { data: 'COD_ARTICULO', defaultContent: '' },
                { data: 'CASA', defaultContent: '' },
                { data: 'SUBCASA', defaultContent: '' },
                { data: 'CATEGORIA_ART', defaultContent: '' },
                { data: 'PRECIO', defaultContent: 0 },
                { data: 'PORC_DESCUENTO', defaultContent: 0 },
                { data: 'FECHA_INICIAL', defaultContent: '' },
                { data: 'FECHA_FINAL', defaultContent: '' },
                { data: 'PRECIO_FINAL', defaultContent: 0 },
                { data: 'U_PS_PORC_DCTO_ANT', defaultContent: 0 },
                { data: 'U_PS_FECHA_INICIO_PROY', defaultContent: '' },
                { data: 'U_PS_FECHA_FIN_PROY', defaultContent: '' },
                { data: 'PRECIO_PROYECTADO', defaultContent: null },
                { data: 'ESTADO_PROYECCION', defaultContent: '' },
                { data: null, defaultContent: '' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: function (_data, _type, row) {
                        if (!tieneRegistro(row)) return '';

                        const key = construirClaveRegistro(row);
                        return `
                            <div class="form-check form-check-sm form-check-custom form-check-solid">
                                <input class="form-check-input row-checkbox"
                                       type="checkbox"
                                       value="${key}" />
                            </div>`;
                    }
                },
                {
                    targets: [6, 10, 14],
                    className: 'dt-body-right',
                    orderable: false,
                    render: function (data) {
                        return data === null || data === undefined || data === ''
                            ? '-'
                            : formatearNumero(Number(data));
                    }
                },
                {
                    targets: [7, 11],
                    className: 'dt-body-right',
                    orderable: false,
                    render: function (data) {
                        const valor = Number(data || 0);
                        return `${valor.toLocaleString('en-US', { maximumFractionDigits: 2 })}%`;
                    }
                },
                {
                    targets: [8, 9, 12, 13],
                    render: function (data) {
                        return formatearFechaPantalla(data) || '-';
                    }
                },
                {
                    targets: 15,
                    orderable: false,
                    render: function (data) {
                        return badgeEstadoProyeccion(data);
                    }
                },
                {
                    targets: 16,
                    orderable: false,
                    render: function (_data, _type, row) {
                        if (!tieneRegistro(row)) return '';

                        const estado = row.ESTADO_PROYECCION || 'SIN_PROYECCION';
                        const puedeModificar = puedeEditarDescuento
                            && estado !== 'ACTIVA'
                            && estado !== 'INCONSISTENTE';

                        const editar = puedeModificar
                            ? `<button type="button"
                                       class="btn btn-sm btn-clean btn-icon me-2 btn-editar-descuento"
                                       title="Programar o editar descuento">
                                   <i class="fas fa-edit text-warning"></i>
                               </button>`
                            : '';

                        const eliminar = puedeEliminarDescuento
                            ? `<button type="button"
                                       class="btn btn-sm btn-clean btn-icon btn-eliminar-descuento"
                                       title="Eliminar descuento">
                                   <i class="fas fa-trash-alt text-danger"></i>
                               </button>`
                            : '';

                        return editar + eliminar;
                    }
                }
            ]
        });

        selDescCli = wireMultiSelect(dt, {
            checkAllSelector: '#checkAll',
            rowCheckboxSelector: '.row-checkbox',
            btnMultiSelector: '.app-engage-btn.multi',
            rowKeyFn: construirClaveRegistro
        });

        $('#miTabla tbody')
            .off('click.descuentos', '.btn-editar-descuento')
            .on('click.descuentos', '.btn-editar-descuento', function () {
                const row = dt.row($(this).closest('tr')).data();
                if (row) abrirModalEdiDescuento(row);
            });

        $('#miTabla tbody')
            .off('click.eliminarDescuento', '.btn-eliminar-descuento')
            .on('click.eliminarDescuento', '.btn-eliminar-descuento', function () {
                const row = dt.row($(this).closest('tr')).data();
                if (row) confirmarEliminarDescuento(row, this);
            });

        KTDatatablesDescuento.dt = dt;
    };

    const filtros = function () {
        $('#kt_datatable_search_queryDesc')
            .off('keyup.descuentos')
            .on('keyup.descuentos', function () {
                dt.search($(this).val()).draw();
            });
    };

    return {
        init: function () {
            initDatatable();
            filtros();
        },
        reload: function () {
            if (KTDatatablesDescuento.dt) {
                KTDatatablesDescuento.dt.ajax.reload(null, false);
            }
        }
    };
}();

function tieneRegistro(row) {
    return row && (row.TIENE_REGISTRO === true || Number(row.TIENE_REGISTRO) === 1);
}

function construirClaveRegistro(row) {
    return [
        row.NIVEL_PRECIO,
        row.MONEDA,
        row.VERSION_PRECIO,
        row.COD_ARTICULO,
        row.ESCALA_DCTO,
        row.VERSION_DCTO
    ].map(valor => encodeURIComponent(valor ?? '')).join('|');
}

function leerClaveRegistro(key) {
    const valores = String(key).split('|').map(decodeURIComponent);
    if (valores.length !== 6) return null;

    return {
        NIVEL_PRECIO: valores[0],
        MONEDA: valores[1],
        VERSION: Number(valores[2]),
        ARTICULO: valores[3],
        ESCALA_DCTO: Number(valores[4]),
        VERSION_DCTO: Number(valores[5])
    };
}

function badgeEstadoProyeccion(estado) {
    const estilos = {
        SIN_REGISTRO: ['badge-light-secondary', 'Sin registro'],
        SIN_PROYECCION: ['badge-light-secondary', 'Sin proyección'],
        PENDIENTE: ['badge-light-info', 'Pendiente'],
        ACTIVA: ['badge-light-success', 'Activa'],
        INCONSISTENTE: ['badge-light-danger', 'Inconsistente']
    };

    const configuracion = estilos[estado] || estilos.INCONSISTENTE;
    return `<span class="badge ${configuracion[0]}">${configuracion[1]}</span>`;
}

function abrirModalEdiDescuento(row) {
    if (!tieneRegistro(row)) {
        toastr.warning('El artículo no tiene un registro en ESCALA_DCTO.', 'Advertencia', toastrOptions);
        return;
    }

    if (row.ESTADO_PROYECCION === 'ACTIVA' || row.ESTADO_PROYECCION === 'INCONSISTENTE') {
        toastr.warning(
            'La proyección está activa o presenta datos inconsistentes y no puede modificarse.',
            'Advertencia',
            toastrOptions);
        return;
    }

    descuentoSeleccionado = row;

    $('#nivelPrecioDescuento').val(row.NIVEL_PRECIO || '');
    $('#ArticuloDescuento').val(row.ARTICULO || row.COD_ARTICULO || '');
    $('#CantidadMin').val(Number(row.CANT_MINIMA || 1));
    $('#CantidadMax').val(Number(row.CANT_MAXIMA || 1));
    $('#PorDescuentoActual').val(Number(row.PORC_DESCUENTO || 0));
    $('#PrecioActual').val(formatearNumero(Number(row.PRECIO_FINAL || 0)));
    $('#fechaIniDescActual').val(formatearFechaPantalla(row.FECHA_INICIAL));
    $('#fechaFinalDescActual').val(formatearFechaPantalla(row.FECHA_FINAL));

    const pendiente = row.ESTADO_PROYECCION === 'PENDIENTE';
    const manana = sumarDias(new Date(), 1);
    const pasadoManana = sumarDias(new Date(), 2);

    $('#PorDescuentoProyectado').val(pendiente ? Number(row.U_PS_PORC_DCTO_ANT || 0) : 1);
    $('#fechaIniDescProyectada').val(
        pendiente ? formatearFechaPantalla(row.U_PS_FECHA_INICIO_PROY) : formatearFechaObjeto(manana));
    $('#fechaFinalDescProyectada').val(
        pendiente ? formatearFechaPantalla(row.U_PS_FECHA_FIN_PROY) : formatearFechaObjeto(pasadoManana));

    $('#estadoProyeccionModal')
        .attr('class', pendiente ? 'badge badge-light-info' : 'badge badge-light-secondary')
        .text(pendiente ? 'Pendiente' : 'Sin proyección');

    $('#mensajeProyeccionActiva').addClass('d-none');
    $('#PorDescuentoProyectado, #fechaIniDescProyectada, #fechaFinalDescProyectada, #btnGuardarDescuento')
        .prop('disabled', false);

    actualizarPrecioProyectado();
    $('#ModalEdiDescuento').modal('show');
}

function actualizarPrecioProyectado() {
    const precioBase = Number(descuentoSeleccionado?.PRECIO || 0);
    const porcentaje = Number($('#PorDescuentoProyectado').val() || 0);

    if (!Number.isFinite(precioBase) || !Number.isFinite(porcentaje) || porcentaje < 0 || porcentaje > 100) {
        $('#PrecioProyectado').val('');
        return;
    }

    const precioProyectado = precioBase - (precioBase * porcentaje / 100);
    $('#PrecioProyectado').val(formatearNumero(precioProyectado));
}

function guardarDescuento() {
    if (!descuentoSeleccionado) return;

    const cantidadMin = Number($('#CantidadMin').val());
    const cantidadMax = Number($('#CantidadMax').val());
    const porcentajeProyectado = Number($('#PorDescuentoProyectado').val());
    const fechaInicio = $('#fechaIniDescProyectada').val();
    const fechaFin = $('#fechaFinalDescProyectada').val();

    if (!validarDatosProyeccion(cantidadMin, cantidadMax, porcentajeProyectado, fechaInicio, fechaFin)) {
        return;
    }

    const desbloquear = bloquearControlDescuentoNivel(document.getElementById('btnGuardarDescuento'));
    if (!desbloquear) return;

    Dajax(`${_urlDesc}/EditarPorcentajeDescuento`, 'POST', {
        nivelPrecio: descuentoSeleccionado.NIVEL_PRECIO,
        moneda: descuentoSeleccionado.MONEDA,
        version: descuentoSeleccionado.VERSION_PRECIO,
        articulo: descuentoSeleccionado.COD_ARTICULO,
        escala: descuentoSeleccionado.ESCALA_DCTO,
        versionDcto: descuentoSeleccionado.VERSION_DCTO,
        cantidadMin,
        cantidadMax,
        porcentajeProyectado,
        fechaInicioProyectada: fechaPantallaAISO(fechaInicio),
        fechaFinProyectada: fechaPantallaAISO(fechaFin)
    }, function (response) {
        try {
            const codigo = obtenerCodigoRespuesta(response);

            if (codigo === 1) {
                toastr.success('Descuento proyectado guardado con éxito.', 'Éxito', toastrOptions);
                $('#ModalEdiDescuento').modal('hide');
                KTDatatablesDescuento.reload();
                return;
            }

            mostrarErrorOperacion(codigo);
        } finally {
            desbloquear();
        }
    });
}

function confirmarEliminarDescuento(row, boton) {
    Swal.fire({
        title: '¿Eliminar descuento?',
        html: `Se eliminará el registro de <b>${row.COD_ARTICULO}</b> en el nivel <b>${row.NIVEL_PRECIO}</b>.<br>Esta acción no se puede deshacer.`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then(result => {
        if (!result.isConfirmed) return;

        const desbloquear = bloquearControlDescuentoNivel(boton);
        if (!desbloquear) return;

        Dajax(`${_urlDesc}/EliminarDescuento`, 'POST', {
            nivelPrecio: row.NIVEL_PRECIO,
            moneda: row.MONEDA,
            version: row.VERSION_PRECIO,
            articulo: row.COD_ARTICULO,
            escalaDcto: row.ESCALA_DCTO,
            versionDcto: row.VERSION_DCTO
        }, function (response) {
            try {
                const codigo = obtenerCodigoRespuesta(response);
                if (codigo === 1) {
                    toastr.success('Descuento eliminado con éxito.', 'Éxito', toastrOptions);
                    KTDatatablesDescuento.reload();
                } else if (codigo === 2) {
                    toastr.warning('El registro ya no existe.', 'Advertencia', toastrOptions);
                    KTDatatablesDescuento.reload();
                } else if (codigo === -1) {
                    toastr.error('No posee permiso para eliminar descuentos.', 'Permiso denegado', toastrOptions);
                } else {
                    toastr.error('No se pudo eliminar el descuento.', 'Error', toastrOptions);
                }
            } finally {
                desbloquear();
            }
        });
    });
}

function abrirModalDescuento() {
    const claves = selDescCli.getSelectedKeys();
    if (!claves.length) {
        toastr.warning('Debe seleccionar al menos un registro.', 'Advertencia', toastrOptions);
        return;
    }

    $('#nivelPrecioDescuentoMultiple').val($('#nivelprecioDesc').val() || '');
    $('#ArticuloDescuentoMultiple').val(
        claves.length <= 10
            ? claves.map(k => leerClaveRegistro(k)?.ARTICULO).filter(Boolean).join(', ')
            : `${claves.length} artículos`);

    $('#fechaIniDescMasivo').val(formatearFechaObjeto(sumarDias(new Date(), 1)));
    $('#fechaFinalDescMasivo').val(formatearFechaObjeto(sumarDias(new Date(), 2)));
    $('#CantidadMinMasivo').val('1');
    $('#CantidadMaxMasivo').val('1');
    $('#PorDescuentoMasivo').val('1');
    $('#ModalDescuento').modal('show');
}

function guardarDescuentoNivelMasivo() {
    const nivelPrecio = $('#nivelPrecioDescuentoMultiple').val();
    const cantidadMin = Number($('#CantidadMinMasivo').val());
    const cantidadMax = Number($('#CantidadMaxMasivo').val());
    const porcentajeProyectado = Number($('#PorDescuentoMasivo').val());
    const fechaInicio = $('#fechaIniDescMasivo').val();
    const fechaFin = $('#fechaFinalDescMasivo').val();
    const registros = selDescCli.getSelectedKeys().map(leerClaveRegistro).filter(Boolean);

    if (!registros.length) {
        toastr.warning('Debe seleccionar al menos un registro.', 'Advertencia', toastrOptions);
        return;
    }

    if (!validarDatosProyeccion(cantidadMin, cantidadMax, porcentajeProyectado, fechaInicio, fechaFin)) {
        return;
    }

    Swal.fire({
        title: '¿Programar descuentos?',
        html: `Se programará el descuento para <b>${registros.length}</b> registros.<br>Los registros que ya tengan una proyección harán que se rechace toda la operación.`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, programar',
        cancelButtonText: 'Cancelar'
    }).then(result => {
        if (!result.isConfirmed) return;

        Dajax(`${_urlDesc}/EditarPorcentajeDescuentoMasivo`, 'POST', {
            nivelPrecio,
            listaRegistros: JSON.stringify(registros),
            cantidadMin,
            cantidadMax,
            porcentajeProyectado,
            fechaInicioProyectada: fechaPantallaAISO(fechaInicio),
            fechaFinProyectada: fechaPantallaAISO(fechaFin)
        }, function (response) {
            const codigo = obtenerCodigoRespuesta(response);

            if (codigo === 1) {
                toastr.success('Descuentos proyectados guardados con éxito.', 'Éxito', toastrOptions);
                $('#ModalDescuento').modal('hide');
                KTDatatablesDescuento.reload();
                return;
            }

            mostrarErrorOperacion(codigo);
        });
    });
}

function validarDatosProyeccion(cantidadMin, cantidadMax, porcentaje, fechaInicioTexto, fechaFinTexto) {
    if (!Number.isInteger(cantidadMin) || cantidadMin <= 0) {
        toastr.warning('La cantidad mínima debe ser un entero mayor que cero.', 'Advertencia', toastrOptions);
        return false;
    }

    if (!Number.isInteger(cantidadMax) || cantidadMax < cantidadMin) {
        toastr.warning('La cantidad máxima debe ser un entero mayor o igual que la mínima.', 'Advertencia', toastrOptions);
        return false;
    }

    if (!Number.isFinite(porcentaje) || porcentaje < 0 || porcentaje > 100) {
        toastr.warning('El porcentaje proyectado debe estar entre 0 y 100.', 'Advertencia', toastrOptions);
        return false;
    }

    const fechaInicio = parsearFecha(fechaInicioTexto);
    const fechaFin = parsearFecha(fechaFinTexto);
    const hoy = inicioDelDia(new Date());

    if (!fechaInicio || !fechaFin) {
        toastr.warning('Debe ingresar fechas válidas.', 'Advertencia', toastrOptions);
        return false;
    }

    if (fechaInicio <= hoy) {
        toastr.warning('La fecha de inicio proyectada debe ser posterior a hoy.', 'Advertencia', toastrOptions);
        return false;
    }

    if (fechaFin <= fechaInicio) {
        toastr.warning('La fecha final proyectada debe ser posterior a la fecha de inicio.', 'Advertencia', toastrOptions);
        return false;
    }

    return true;
}

function obtenerCodigoRespuesta(response) {
    const valor = response && response.responseJSON !== undefined
        ? response.responseJSON
        : response;
    return Number(valor);
}

function mostrarErrorOperacion(codigo) {
    const mensajes = {
        '-1': 'No posee permiso para realizar esta operación.',
        '0': 'Ocurrió un error procesando el descuento.',
        '2': 'Uno o más registros no existen en ESCALA_DCTO.',
        '3': 'Uno o más registros ya tienen una proyección activa o no modificable.',
        '4': 'Los datos o las fechas de la proyección no son válidos.'
    };

    toastr.warning(mensajes[String(codigo)] || mensajes['0'], 'Advertencia', toastrOptions);
}

function parsearFecha(valor) {
    if (!valor) return null;
    const texto = String(valor).trim();
    let partes;

    if (/^\d{4}-\d{2}-\d{2}/.test(texto)) {
        partes = texto.substring(0, 10).split('-').map(Number);
        return crearFechaValida(partes[0], partes[1], partes[2]);
    }

    if (/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(texto)) {
        partes = texto.split('/').map(Number);
        return crearFechaValida(partes[2], partes[1], partes[0]);
    }

    return null;
}

function crearFechaValida(anio, mes, dia) {
    const fecha = new Date(anio, mes - 1, dia);
    if (fecha.getFullYear() !== anio || fecha.getMonth() !== mes - 1 || fecha.getDate() !== dia) {
        return null;
    }
    return inicioDelDia(fecha);
}

function inicioDelDia(fecha) {
    return new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate());
}

function sumarDias(fecha, dias) {
    const copia = inicioDelDia(fecha);
    copia.setDate(copia.getDate() + dias);
    return copia;
}

function formatearFechaObjeto(fecha) {
    const pad = n => String(n).padStart(2, '0');
    return `${pad(fecha.getDate())}/${pad(fecha.getMonth() + 1)}/${fecha.getFullYear()}`;
}

function formatearFechaPantalla(valor) {
    const fecha = parsearFecha(valor);
    return fecha ? formatearFechaObjeto(fecha) : '';
}

function fechaPantallaAISO(valor) {
    const fecha = parsearFecha(valor);
    if (!fecha) return '';
    const pad = n => String(n).padStart(2, '0');
    return `${fecha.getFullYear()}-${pad(fecha.getMonth() + 1)}-${pad(fecha.getDate())}`;
}

function IniciarFechas() {
    createDatePicker('fechaIniDescProyectada');
    createDatePicker('fechaFinalDescProyectada');
    createDatePicker('fechaIniDescMasivo');
    createDatePicker('fechaFinalDescMasivo');
}

function ManejarFiltros() {
    $('#nivelprecioDesc, #articulosDesc, #casaDesc, #SubCasaDesc')
        .off('change.descuentos')
        .on('change.descuentos', ActualizarFiltros);

    $('#nivelprecioDesc')
        .off('change.cargarArticulos')
        .on('change.cargarArticulos', function () {
            cargarArticulosPorNivel($(this).val());
        });
}

function ActualizarFiltros() {
    concasaDesc = valoresFiltroMultiple('#casaDesc');
    consubCasaDesc = valoresFiltroMultiple('#SubCasaDesc');
    nivelPrecioDesc = $('#nivelprecioDesc').val() || '0';
    articuloDesc = $('#articulosDesc').val() || '0';
    KTDatatablesDescuento.reload();
}

function valoresFiltroMultiple(selector) {
    const valores = $(`${selector} :selected`).map((_indice, elemento) => elemento.value).get();
    return valores.length ? valores.join(',') : '0';
}

function cargarArticulosPorNivel(nivelPrecio = '0') {
    cargarSelectPorAjax({
        selector: '#articulosDesc',
        url: `${_urlAp}/ObtenerArticulosPorNivel`,
        params: { nivelPrecio }
    });
}

function openUploadModalDesc() {
    $('#EdiModalDescuento').modal('show');
}

function closeUploadModalDesc() {
    $('#EdiModalDescuento').modal('hide');
    $('#bt_ProcesarDescuento').prop('disabled', false);
    $('#excelFile').val('');
    $('#descripcionFileDesc').val('');
}

function updateFileNameDesc(input) {
    const fileName = input && input.files && input.files.length
        ? input.files[0].name
        : '';
    $('#descripcionFileDesc').val(fileName);
}

async function uploadExcelDesc() {
    const botonProcesar = $('#bt_ProcesarDescuento');
    const fileInput = document.getElementById('excelFile');

    if (!fileInput || !fileInput.files.length) {
        toastr.warning('Por favor seleccione un archivo.', 'Advertencia', toastrOptions);
        return;
    }

    const archivo = fileInput.files[0];
    if (!archivo.name.toLowerCase().endsWith('.xlsx')) {
        toastr.warning('El cargador debe ser un archivo .xlsx.', 'Advertencia', toastrOptions);
        return;
    }

    const formData = new FormData();
    formData.append('excelFile', archivo);
    botonProcesar.prop('disabled', true);

    try {
        const response = await fetch(`${_urlDesc}/UploadExcelDesc`, {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            const detalle = await response.text();
            throw new Error(detalle || 'Error procesando el archivo Excel.');
        }

        const data = await response.json();
        const errores = Array.isArray(data.errores) ? data.errores : [];
        const resumen = `Procesados: ${data.procesados || 0}. Omitidos: ${data.omitidos || 0}.`;

        if (data.success) {
            toastr.success(resumen, 'Archivo procesado', toastrOptions);
        } else {
            const detalle = errores.length ? `<br>${errores.join('<br>')}` : '';
            Swal.fire({
                title: 'El archivo terminó con observaciones',
                html: `${resumen}${detalle}`,
                icon: 'warning'
            });
        }

        closeUploadModalDesc();
        KTDatatablesDescuento.reload();
    } catch (error) {
        toastr.error(error.message || 'Error procesando el archivo Excel.', 'Error', toastrOptions);
    } finally {
        botonProcesar.prop('disabled', false);
    }
}

$(document).ready(function () {
    IniciarFechas();
    ManejarFiltros();
    cargarArticulosPorNivel($('#nivelprecioDesc').val() || '0');
    inicializarSelectConBusquedaYTodos('#articulosDesc', 'Filtrar por un artículo');
    inicializarSelectMultipleConTodos('#casaDesc', 'Filtrar por Casa');
    inicializarSelectMultipleConTodos('#SubCasaDesc', 'Filtrar por SubCasa');

    $('#PorDescuentoProyectado')
        .off('input.precioProyectado')
        .on('input.precioProyectado', actualizarPrecioProyectado);

    ActualizarFiltros();
    KTDatatablesDescuento.init();
});
