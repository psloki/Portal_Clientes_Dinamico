
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Usuarios++++++++++++++++++++++++++++++++++++++++
let tipo_rol = $('#FiltroRol').val();
var KTTablaUsuarios = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#tablaUsuarios").DataTable({
            searchDelay: 500,
            processing: true,
            responsive: isMobile,
            scrollX: true,         // Habilita scroll horizontal
            autoWidth: false,
            pageLength: 50,
            //data: cotizaciones,

            "bDestroy": true,
            destroy: true,
            //serverSide: true,
            ajax: {
                url: `${rurl}/GetUsuarios`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                    d.tipo_rol = tipo_rol
                }
            },
            columns: [

               
                { "data": "USUARIO" },

                { "data": "IDENTIFICACION" },
                {
                    "data": "NOMBRE"
                },
                {
                    "data": "VENDEDOR"
                },
                {
                    "data": "COBRADOR"
                },
                {
                    "data": "COD_BODEGA"
                },
                {
                    "data": "DESCRIPCION"
                },
                {
                    "data": "ACTIVO",
                },

                {
                    "data": null,
                },
            ],
            columnDefs: [
               
                {
                    targets: 3,
                    orderable: false,
                    render: function (data, type, row) {
                            return `${row.VENDEDOR} | ${row.NOM_VENDEDOR}`
                    }
                },
                {
                    targets: 4,
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.COBRADOR} | ${row.NOM_COBRADOR}`
                    }
                },
                {
                    targets: 5,
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.COD_BODEGA} | ${row.NOM_BODEGA}`
                    }
                },
                {
                    targets: 7,
                    orderable: false,
                    render: function (data, type, row) {
                        if (row.ACTIVO == 'A') {
                            return `<span class="badge badge-lg  badge-light-primary">Activo</span>`
                        } else {
                            return `<span class="badge badge-lg  badge-light-danger">Inactivo</span>`
                        }
                    }
                },
                {
                    targets: 8,
                    orderable: false,
                    render: function (data, type, row) {

                        let accionesUsuario = "";
                        if (PERMISOS_MOD_USUARIO.split(',').includes("62")) {
                            accionesUsuario += `<a href="javascript:;" class="btn btn-sm btn-clean btn-icon me-2" title="Editar usuario"  onclick="EditarUsuario('${row.USUARIO}', '${row.NOMBRE}','${row.ROL}','${row.VENDEDOR}','${row.COBRADOR}')">
						        <span class="svg-icon svg-icon-warning svg-icon-2x"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-05-14-112058/theme/html/demo1/dist/../src/media/svg/icons/General/Settings-2.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
							          <svg xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" width = "24px" height = "24px" viewBox = "0 0 24 24" version = "1.1" >
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                <rect x="0" y="0" width="24" height="24"/>
                                                <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
                                                <rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>
                                            </g>
                                        </svg>
						        </svg><!--end::Svg Icon--></span>
				            </a>`
                        }
                        if (PERMISOS_MOD_USUARIO.split(',').includes("16") && row.ACTIVO === "A") {
                            accionesUsuario +=
                                `<a onclick="ConfirmarCambioEstado('${row.USUARIO}','N')" class="btn btn-sm btn-clean btn-icon me-2" title="Desactivar">
	                                <span class="svg-icon svg-icon-danger svg-icon-2x">
                                        <svg xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" width = "24px" height = "24px" viewBox = "0 0 24 24" version = "1.1" >
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                <rect x="0" y="0" width="24" height="24"/>
                                                <circle fill="currentColor" opacity="0.3" cx="12" cy="12" r="10"/>
                                                <path d="M12.0355339,10.6213203 L14.863961,7.79289322 C15.2544853,7.40236893 15.8876503,7.40236893 16.2781746,7.79289322 C16.6686989,8.18341751 16.6686989,8.81658249 16.2781746,9.20710678 L13.4497475,12.0355339 L16.2781746,14.863961 C16.6686989,15.2544853 16.6686989,15.8876503 16.2781746,16.2781746 C15.8876503,16.6686989 15.2544853,16.6686989 14.863961,16.2781746 L12.0355339,13.4497475 L9.20710678,16.2781746 C8.81658249,16.6686989 8.18341751,16.6686989 7.79289322,16.2781746 C7.40236893,15.8876503 7.40236893,15.2544853 7.79289322,14.863961 L10.6213203,12.0355339 L7.79289322,9.20710678 C7.40236893,8.81658249 7.40236893,8.18341751 7.79289322,7.79289322 C8.18341751,7.40236893 8.81658249,7.40236893 9.20710678,7.79289322 L12.0355339,10.6213203 Z" fill="currentColor"/>
                                            </g>
                                        </svg>
                                    </span>
							    </a>`
                            
                        } else if (PERMISOS_MOD_USUARIO.split(',').includes("16")){
                            accionesUsuario +=
                                `<a onclick="ConfirmarCambioEstado('${row.USUARIO}','A')" class="btn btn-sm btn-clean btn-icon mr-2" title="Activar">
	                            <span class="svg-icon svg-icon-success svg-icon-2x">
                                    <svg xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" width = "24px" height = "24px" viewBox = "0 0 24 24" version = "1.1" >
                                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                            <rect x="0" y="0" width="24" height="24"/>
                                            <circle fill="currentColor" opacity="0.3" cx="12" cy="12" r="10"/>
                                            <path d="M16.7689447,7.81768175 C17.1457787,7.41393107 17.7785676,7.39211077 18.1823183,7.76894473 C18.5860689,8.1457787 18.6078892,8.77856757 18.2310553,9.18231825 L11.2310553,16.6823183 C10.8654446,17.0740439 10.2560456,17.107974 9.84920863,16.7592566 L6.34920863,13.7592566 C5.92988278,13.3998345 5.88132125,12.7685345 6.2407434,12.3492086 C6.60016555,11.9298828 7.23146553,11.8813212 7.65079137,12.2407434 L10.4229928,14.616916 L16.7689447,7.81768175 Z" fill="currentColor" fill-rule="nonzero"/>
                                        </g>
                                    </svg>
                                </span>
							</a>`
                        }
                        return accionesUsuario
                    }
                },
                { responsivePriority: 1, targets: [0] },  // Checkbox
                { responsivePriority: 100, targets: [1,2,3, 4, 5, 6, 7, 8] } // resto

            ],
            
        });
        table = dt.$;
        dt.on('draw', function () {

        });
    }
    var Filtros = function () {

        
        $('#kt_datatable_search_query').on('keyup', function () {
            dt.search($(this).val()).draw();
        });
    }
    var resetTable = function () {
        setTimeout(() => {

            dt.ajax.reload();
        }, 100)

        dt.clear().draw()
    }
    
    return {
        init: function () {
            initDatatable();
            Filtros()
        },
        reset: function () {
            resetTable();

        }

    }
}();

function EditarUsuario(USUARIO, NOMBRE, ROL, VENDEDOR, COBRADOR) {
    
    $('#UsuarioEditar').val(USUARIO).trigger('change')
    //$('#UsuarioEditarInput').val(usuario)
    //$("#UsuarioEditar").prop("disabled", true);
    $('#EditarColaborador').val(NOMBRE).trigger('change')
    $('#RolUsuarioEditar').val(ROL).trigger('change')
    $('#VendedorUsuario').val(VENDEDOR).trigger('change')
    $('#CobradorUsuario').val(COBRADOR).trigger('change')
    $('#modalEditarUsuario').modal('show')
}
function ValidarGuardarUsuarioCrear() {
    const nombre = $('#nombreMantenimientoUsuario').val().trim();
    const usuario = $('#idusuarioMantenimientoUsuario').val().trim();
    const email = $('#correo_electronicoMantenimientoUsuario').val().trim();
    const pass = $('#passwordMantenimientoUsuario').val();
    const rpass = $('#rpasswordMantenimientoUsuario').val();
    const vendedor = $('#VendedorRegistrar').val();
    const cobrador = $('#CobradorRegistrar').val();
    const bodega = $('#BodegaRegistrar').val();
    const _iden = $('#identificacionMantenimientoUsuario').val().trim();

    if (!nombre || !usuario || !email || !pass || !rpass || vendedor === "" || cobrador === "" || bodega === "") {
        toastr.warning('Debe completar todos los campos obligatorios.', '¡Atención!');
        return false;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        toastr.warning('Formato de correo electrónico inválido.', '¡Atención!');
        return false;
    }

    if (pass !== rpass) {
        toastr.warning('Las contraseñas no coinciden.', '¡Atención!');
        return false;
    }

    if (_iden.length > 15) {
        toastr.warning('Identificación inválida. Máximo 15 dígitos.', '¡Alerta!', toastrOptions);
        return false;
    }

    if (pass.length < 3) {
        toastr.warning('La contraseña debe tener al menos 3 caracteres.', '¡Atención!');
        return false;
    }

    return true;
}
function ValidarGuardarUsuarioEditar(form) {
    let fd = new FormData(form)
    let result = true

    if (fd.get('Usuario') == '') {
        toastr.warning('Debe seleccionar un usuario', 'Alerta', toastrOptions)
        result = false
    } else if (fd.get('Rol') == '') {
        toastr.warning('Debe seleccionar un rol', 'Alerta', toastrOptions)
        result = false
    } else if (fd.get('Bodega') == "0") {
        toastr.warning('Debe seleccionar una bodega', 'Alerta', toastrOptions)
        result = false
    }
    return result
}


function GuardarUsuario(form) {
    let fd = new FormData(form)


    fd.set('Usuario', $('#UsuarioEditar').val())
    fd.set('Rol', $('#RolUsuarioEditar').val())
    fd.set('Vendedor', $('#VendedorUsuario').val())
    fd.set('Cobrador', $('#CobradorUsuario').val())
    fd.set('Bodega', $('#BodegaUsuario').val())

    if ($('#BodegaUsuario').val() == "0") {
        toastr.warning('Debe seleccionar una bodega', 'Advertencia', toastrOptions);
        return;
    }
        const url = `${rurl}/ActualizarUsuario`
        fetch(url, {
            method: 'POST',
            body: fd
        })
            .then(res => res.json())
            .then(data => {
                if (data == 1) {
                    toastr.success('Usuario modificado correctamente', 'Éxito', toastrOptions)
                    RecargarTabla(1)
                    $('#modalEditarUsuario').modal('hide')
                }
                else {
                    toastr.warning('No se pudo actualizar el usuario', 'Alerta', toastrOptions)
                }
            })
            .catch(err => {
                console.error(err)
                toastr.warning('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions)
            })
    
}

function ConfirmarCambioEstado(usuario, nuevoEstado) {
    let titulo = '¿Desea activar este usuario?'
    if (nuevoEstado == 'N') {
        titulo = '¿Desea desactivar este usuario?'
    }
    Swal.fire({
        title: titulo,
        text: '',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: "Cancelar",
        confirmButtonText: 'Aceptar'
    }).then((result) => {
        if (result.value) {
            CambiarEstado(usuario, nuevoEstado)
        }
    })
}

function CambiarEstado(usuario, estado) {
    let fd = new FormData()
    fd.set('Usuario', usuario)
    fd.set('Estado', estado)
    const url = `${rurl}/CambiarEstado`
    const exitoString = estado == 'A' ? 'Usuario activado correctamente' : 'Usuario desactivado correctamente'
    const errorString = estado == 'A' ? 'No se pudo activar el usuario' : 'No se pudo desactivar el usuario'
    fetch(url, {
        method: 'POST',
        body: fd
    })
        .then(res => res.json())
        .then(data => {
            if (data == 1) {
                toastr.success(exitoString, 'Éxito', toastrOptions)
                RecargarTabla(1)
            } else {
                toastr.warning(errorString, 'Alerta', toastrOptions)
            }
        })
        .catch(err => {
            console.error(err)
            toastr.error('Ha ocurrido un error, por favor inténtelo de nuevo más tarde', 'Error', toastrOptions)
        })
}

function CrearUsuario() {
    //$('#UsuarioInsertar').val(null).trigger('change')
    $('#EmpleadoInsertar').val(null).trigger('change')
    $('#RolUsuarioInsertar').val(null).trigger('change')
    $('#modalInsertarUsuario').modal('show')
}

function RegistrarNuevoUsuario() {

    let bandera = false
    const _iden = $('#identificacionMantenimientoUsuario').val().trim();
    const _nombre = $('#nombreMantenimientoUsuario').val();
    const _usuario = $('#idusuarioMantenimientoUsuario').val();
    const _email = $('#correo_electronicoMantenimientoUsuario').val();
    const _pass = $('#passwordMantenimientoUsuario').val();
    const _rpass = $('#rpasswordMantenimientoUsuario').val();
    let _vendedor = $('#VendedorRegistrar').val();
    let _cobrador = $('#CobradorRegistrar').val();
    let _bodega = $('#BodegaRegistrar').val();
    let _rol = $('#RolUsuarioIns').val()
    
    const formulario = [_iden, _nombre, _usuario, _email, _pass, _vendedor, _cobrador, _bodega,_rol]
    
    if (formulario.includes('')) {
        toastr.warning('Se deben completar todos los campos.', '¡Alerta!', toastrOptions)
        return
    }
    if (_pass != _rpass) {
        toastr.warning('Las contraseñas no coinciden.', '¡Alerta!', toastrOptions)
        return
    }

    Dajax(`${rurl}/ValidaExistenciaUsuarioMantenimiento`, 
        "POST", { usuario: _usuario, correo: _email, identificacion: _iden }, function (response) {
            if (response.responseJSON) {
                bandera = false;
                if (response.responseJSON.message == "Éxito") {
                    if (response.responseJSON.data.EXISTE == 1) {
                        toastr.warning('El usuario ya existe.', 'Ingresar Usuario', toastrOptions)
                        usuarioExistente = false;
                        bandera = false

                    } else {
                        if (response.responseJSON.data.EXISTE == 2) {
                            toastr.warning("El correo electr\u00F3nico ya existe, intente otro usuario.", "Correo electr\u00F3nico no v\u00E1lido!",);
                            usuarioExistente = false;
                            bandera = false
                        } else {
                            if (response.responseJSON.data.EXISTE == 3) {
                                toastr.warning("El n\u00FAmero de identificiaci\u00F3n ya existe, intente con otro.", "N\u00FAmero de identificaci\u00F3n no v\u00E1lido!");
                                usuarioExistente = false;
                                bandera = false

                            } else {
                                usuarioExistente = true;
                                bandera = true
                            }
                        }
                    }
                    //bandera = true;
                } else {
                    usuarioExistente = false;
                    bandera = true;
                }
            }

            if (bandera) {
                Dajax(`${rurl}/RegistrarUsuarioMantenimiento`,
                "POST",
                    {
                        USUARIO: _usuario,
                        PASS: _pass,
                        EMAIL: _email,
                        CEDULA: _iden,
                        NOMBRE: _nombre,
                        VENDEDOR: _vendedor,
                        COBRADOR: _cobrador,
                        BODEGA: _bodega,
                        ROL: _rol
                    }, function (response) {
                    if (response.responseJSON.message == "Exito") {
                        toastr.success('El usuario se a registrado correctamente', 'Ingresar Usuario', toastrOptions);
                    } else {
                        toastr.warning('No se ha podido registrar el usuario.', 'Ingresar Usuario', toastrOptions)
                    }
                });
            }

        });
    $("#modalInsertarUsuario").modal('hide')
    RecargarTabla(1)
}

var KTTablaRoles = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#tablaRoles").DataTable({
            searchDelay: 500,
            processing: true,
            
            scrollX: true,
            pageLength: 50,
            //data: cotizaciones,

            "bDestroy": true,
            destroy: true,
            //serverSide: true,
            ajax: {
                url: `${rurl}/ObtenerRolesMantenimiento`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                }
            },
            columns: [


                { "data": "ROL" },

                { "data": "DESCRIPCION" },
                {
                    "data": "COMENTARIO"
                },
                

                {
                    "data": null,
                },
            ],
            columnDefs: [
                {
                    targets: 3,
                    orderable: false,
                    render: function (data, type, row) {
                        let accionesRoles = "";
                        if (PERMISOS_MOD_USUARIO.split(',').includes("63")) {
                            accionesRoles += `<a onclick="EditarRol(${row.ROL},'${row.DESCRIPCION}', '${row.COMENTARIO}')" class="btn btn-sm btn-clean btn-icon mr-2" title="Editar">
	                                <span class="svg-icon svg-icon-warning svg-icon-2x">
                                        <svg xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" width = "24px" height = "24px" viewBox = "0 0 24 24" version = "1.1" >
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                <rect x="0" y="0" width="24" height="24"/>
                                                <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
                                                <rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>
                                            </g>
                                        </svg>
                                    </span>
							    </a>`
                        }
                        return accionesRoles;
                    }
                },

            ],
            
        });
        table = dt.$;
        dt.on('draw', function () {

        });
    }
    var resetTable = function () {
        setTimeout(() => {

            dt.ajax.reload();
        }, 100)

        dt.clear().draw()
    }
    return {
        init: function () {
            initDatatable();
        },
        reset: function () {
            resetTable();

        }

    }
}();

function EditarRol(ROL, DESCRIPCION, COMENTARIO) {
    /*console.log(ROL,DESCRIPCION,COMENTARIO)*/
    $('#EditarRolId').val(ROL)
    $('#EditarRolDescripcion').val(DESCRIPCION)
    /* $("#EditarRolDescripcion").attr("disabled", "disabled")*/
    $('#EditarRolComentario').val(COMENTARIO)
    $('#modalEditarRol2').modal('show')
}

function ValidarGuardarRol(form) {
    let fd = new FormData(form)
    let result = true
    if (fd.get('Descripcion') == '') {
        toastr.warning('Debe ingresar un nombre para el rol', 'Alerta', toastrOptions)
        result = false
    }
    return result
}

function ActualizarRol(form) {
    let fd = new FormData(form)
    const url = `${rurl}/GuardarRol`
    fetch(url, {
        method: 'POST',
        body: fd
    })
        .then(res => res.json())
        .then(data => {
            if (data == 1) {
                toastr.success('Rol modificado correctamente', 'Éxito', toastrOptions)
                     $('#modalInsertarRol').modal('hide')
                     $("#modalEditarRol2").modal('hide')
                    form.reset()
                               setTimeout(() => { location.reload(); }, 300)

                $('#modalInsertarRol').modal('hide')
                $("#modalEditarRol2").modal('hide')
                form.reset()
            } else {
                toastr.warning('El rol no es válido', 'Alerta', toastrOptions)
            }
        })
        .catch(err => {
            console.error(err)
            toastr.error('Ha ocurrido un error, por favor inténtelo más tarde', 'Error', toastrOptions)
        })
}

function CrearRol() {
    $('#modalInsertarRol').modal('show')
}

var KTTablaPermisos = function () {
    // Shared variables
    var table;
    var dt;
    var filterPayment;

    // Private functions
    var initDatatable = function () {
        dt = $("#tablaPermisos").DataTable({
            searchDelay: 500,
            processing: true,
            
            scrollX: true,
            pageLength: 50,
            //data: cotizaciones,

            "bDestroy": true,
            destroy: true,
            //serverSide: true,
            ajax: {
                url: `${rurl}/GetPermisos`,
                dataSrc: "",
                contentType: 'application/json',
                data: function (d) {
                }
            },
            columns: [


                { "data": "ROL" },

                //{ "data": "SEGUIMIENTO_FACTURA_PEDIDO" },
                //{ "data": "TRACKING_IN" },
                { "data": "USUARIO" },
                { "data": "CONFIGURACION" },
                { "data": "GESTION_DE_PEDIDOS" },
                { "data": "PORTAL_DE_CLIENTES" },
                { "data": "DESCUENTOS" },
                { "data": "MENU" },
                {
                    "data": null,
                },
            ],
            columnDefs: [


                {
                    targets: 0,
                    orderable: false,
                    render: function (data, type, row) {
                        return `${row.DESCRIPCION}`
                        
                    }
                },
                //{
                //    targets: 1,
                //    orderable: false,
                //    render: function (data, type, row) {
                //        return SetIcono(row.SEGUIMIENTO_FACTURA_PEDIDO)
                        
                //    }
                //},
                //{
                //    targets: 2,
                //    orderable: false,
                //    render: function (data, type, row) {
                //        return SetIcono(row.TRACKING_IN)
                        
                //    }
                //},
               
                {
                    targets: 1,
                    orderable: false,
                    render: function (data, type, row) {
                        return SetIcono(row.USUARIO)
                        
                    }
                },
                {
                    targets: 2,
                    orderable: false,
                    render: function (data, type, row) {
                        return SetIcono(row.CONFIGURACION)
                        
                    }
                },
                {
                    targets: 3,
                    orderable: false,
                    render: function (data, type, row) {
                        return SetIcono(row.GESTION_DE_PEDIDOS)
                        
                    }
                },
                {
                    targets: 4,
                    orderable: false,
                    render: function (data, type, row) {
                        return SetIcono(row.PORTAL_DE_CLIENTES)
                        
                    }
                },
                {
                    targets: 5,
                    orderable: false,
                    render: function (data, type, row) {
                        return SetIcono(row.DESCUENTOS)

                    }
                },
                {
                    targets: 6,
                    orderable: false,
                    render: function (data, type, row) {
                        return SetIcono(row.MENU)

                    }
                },
                {
                    targets: 7,
                    orderable: false,
                    render: function (data, type, row) {
                        let accionesUsuario = "";

                        if (PERMISOS_MOD_USUARIO.split(',').includes("64")) {
                            accionesUsuario += `     <a onclick="EditarPermisos('${row.ROL}')" class="btn btn-sm btn-clean btn-icon mr-2" title="Editar">
	                                <span class="svg-icon svg-icon-warning svg-icon-2x">
                                        <svg xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" width = "24px" height = "24px" viewBox = "0 0 24 24" version = "1.1" >
                                            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                                <rect x="0" y="0" width="24" height="24"/>
                                                <path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="currentColor" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/>
                                                <rect fill="currentColor" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/>
                                            </g>
                                        </svg>
                                    </span>
							    </a>`
                        }
                        return accionesUsuario;
	                      
                    }
                },

            ],
            
        });
        table = dt.$;
        dt.on('draw', function () {

        });
    }
    var resetTable = function () {
        setTimeout(() => {

            dt.ajax.reload();
        }, 100)

        dt.clear().draw()
    }

    
    return {
        init: function () {
            initDatatable();
        },
        reset: function () {
            resetTable();
        }

    }
}();

function SetIcono(permiso) {
    let html = ''
    if (parseInt(permiso) === 1) {
        html = `<span class="svg-icon svg-icon-primary svg-icon-2x" title="Administrar">
                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                            <rect x="0" y="0" width="24" height="24"/>
                            <path d="M5,8.6862915 L5,5 L8.6862915,5 L11.5857864,2.10050506 L14.4852814,5 L19,5 L19,9.51471863 L21.4852814,12 L19,14.4852814 L19,19 L14.4852814,19 L11.5857864,21.8994949 L8.6862915,19 L5,19 L5,15.3137085 L1.6862915,12 L5,8.6862915 Z M12,15 C13.6568542,15 15,13.6568542 15,12 C15,10.3431458 13.6568542,9 12,9 C10.3431458,9 9,10.3431458 9,12 C9,13.6568542 10.3431458,15 12,15 Z" fill="currentColor"/>
                        </g>
                    </svg>
                </span>`
    } else if (parseInt(permiso) === 2) {
        html = `<span class="svg-icon svg-icon-success svg-icon-2x" title="Listar">
                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                            <rect x="0" y="0" width="24" height="24"/>
                            <path d="M10.5,5 L19.5,5 C20.3284271,5 21,5.67157288 21,6.5 C21,7.32842712 20.3284271,8 19.5,8 L10.5,8 C9.67157288,8 9,7.32842712 9,6.5 C9,5.67157288 9.67157288,5 10.5,5 Z M10.5,10 L19.5,10 C20.3284271,10 21,10.6715729 21,11.5 C21,12.3284271 20.3284271,13 19.5,13 L10.5,13 C9.67157288,13 9,12.3284271 9,11.5 C9,10.6715729 9.67157288,10 10.5,10 Z M10.5,15 L19.5,15 C20.3284271,15 21,15.6715729 21,16.5 C21,17.3284271 20.3284271,18 19.5,18 L10.5,18 C9.67157288,18 9,17.3284271 9,16.5 C9,15.6715729 9.67157288,15 10.5,15 Z" fill="currentColor"/>
                            <path d="M5.5,8 C4.67157288,8 4,7.32842712 4,6.5 C4,5.67157288 4.67157288,5 5.5,5 C6.32842712,5 7,5.67157288 7,6.5 C7,7.32842712 6.32842712,8 5.5,8 Z M5.5,13 C4.67157288,13 4,12.3284271 4,11.5 C4,10.6715729 4.67157288,10 5.5,10 C6.32842712,10 7,10.6715729 7,11.5 C7,12.3284271 6.32842712,13 5.5,13 Z M5.5,18 C4.67157288,18 4,17.3284271 4,16.5 C4,15.6715729 4.67157288,15 5.5,15 C6.32842712,15 7,15.6715729 7,16.5 C7,17.3284271 6.32842712,18 5.5,18 Z" fill="currentColor" opacity="0.3"/>
                        </g>
                    </svg>
                </span>`
    } else {
        html = `<span class="svg-icon svg-icon-danger svg-icon-2x" title="Sin permisos">
                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                        <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                            <rect x="0" y="0" width="24" height="24"/>
                            <path d="M12,22 C6.4771525,22 2,17.5228475 2,12 C2,6.4771525 6.4771525,2 12,2 C17.5228475,2 22,6.4771525 22,12 C22,17.5228475 17.5228475,22 12,22 Z M12,20 C16.418278,20 20,16.418278 20,12 C20,7.581722 16.418278,4 12,4 C7.581722,4 4,7.581722 4,12 C4,16.418278 7.581722,20 12,20 Z M19.0710678,4.92893219 L19.0710678,4.92893219 C19.4615921,5.31945648 19.4615921,5.95262146 19.0710678,6.34314575 L6.34314575,19.0710678 C5.95262146,19.4615921 5.31945648,19.4615921 4.92893219,19.0710678 L4.92893219,19.0710678 C4.5384079,18.6805435 4.5384079,18.0473785 4.92893219,17.6568542 L17.6568542,4.92893219 C18.0473785,4.5384079 18.6805435,4.5384079 19.0710678,4.92893219 Z" fill="currentColor" fill-rule="nonzero" opacity="0.3"/>
                        </g>
                    </svg>
                </span>`
    }
    return html
}

function EditarPermisosvv(permisos) {
    permisos = JSON.parse(permisos)
    console.log(permisos)

    $(`#rd-seguimiento-${permisos[0]}`).prop("checked", true)
    $(`#rd-tracking-${permisos[1]}`).prop("checked", true)
    $(`#rd-estadistica-${permisos[2]}`).prop("checked", true)
    $(`#rd-usuario-${permisos[3]}`).prop("checked", true)
    $(`#rd-configuracion-${permisos[4]}`).prop("checked", true)
    $(`#rd-pedidos-${permisos[5]}`).prop("checked", true)
    $(`#rd-portal-${permisos[6]}`).prop("checked", true)
    $(`#rd-descuento-${permisos[7]}`).prop("checked", true)
    $('#p-rol').val(permisos[8])
    $('#modalEditarPermisos').modal('show')
}

function EditarPermisos(ROL) {
    $('#permisoRol').val(ROL)
    $('#modulo').val('').trigger('change')
    $('#submodulo').val('').trigger('change')
    $('#ContenedorPermisos').html('')
    $('#modalEditarPermisos').modal('show')
}

function RecargarTabla(index) {
    if (index == 1) {
        try {
            KTTablaUsuarios.init()
        } catch (error) {
            KTTablaUsuarios.reset()
        }
    }
    if (index == 2) {
        try {
            KTTablaRoles.init()
        } catch (error) {
            KTTablaRoles.reset()
        }
    }
    if (index == 3) {
        try {
            KTTablaPermisos.init()
        } catch (error) {
            KTTablaPermisos.reset()
        }
    }
}

function GuardarPermisos() {
    var formData = new FormData();

    var _rol = $('#p-rol').val();
    var _seguimiento_factura_pedido = $('input[name="seguimiento"]:checked').val();
    var _tracking = $('input[name="tracking"]:checked').val();
    var _estadistica = $('input[name="estadistica"]:checked').val();
    var _usuario = $('input[name="usuario"]:checked').val();
    var _configuracion = $('input[name="configuracion"]:checked').val();
    var _pedidos = $('input[name="pedidos"]:checked').val();
    var _portal = $('input[name="portal"]:checked').val();
    var _descuento = $('input[name="descuento"]:checked').val();


    formData.append('rol', _rol);
    formData.append('seguimiento', _seguimiento_factura_pedido);
    formData.append('tracking', _tracking);
    formData.append('estadistica', _estadistica);
    formData.append('usuario', _usuario);
    formData.append('configuracion', _configuracion);
    formData.append('pedidos', _pedidos);
    formData.append('portal', _portal);
    formData.append('descuento', _descuento);


    const url = `${rurl}/GuardarPermisos`
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(res => res.json())
        .then(data => {
            if (data == "1") {
                toastr.success('Se cambiaron los permisos del rol', 'Éxito')
                /* BuscarPermisosRol()*/
                RecargarTabla(3)
                $('#modalEditarPermisos').modal('hide')
            } else {
                toastr.warning('El permiso no es válido', 'Alerta')
            }
        })
        .catch(err => {
            /*console.error(err)*/
            toastr.warning('Error en la petición! recargue e intente de nuevo', 'Error')
        })
}

function GuardarPermisosV2() {
    const form = document.querySelector('#frm-editar-permisosV2')
    let fd = new FormData(form)
    const url = `${rurl}/GuardarPermisosV2`
    fetch(url, {
        method: 'POST',
        body: fd
    })
        .then(res => res.json())
        .then(data => {
            if (data == 1) {
                toastr.success("Permisos Editados correctamente", 'Éxito', toastrOptions)
                RecargarTabla(3)
                $('#modalEditarPermisos').modal('hide')
            } else {
                toastr.warning('No se pudieron guardar los permisos', 'Alerta', toastrOptions)
            }
        })
        .catch(err => {
            console.error(err)
            toastr.error('Error en la petición! recargue e intente de nuevo', 'Error', toastrOptions)
        })
}

function ManejarFiltros() {
    $('.filtro').on('change', () => {
        ActualizarFiltros()
        RecargarTabla(1)


    });
}

function ActualizarFiltros() {
    tipo_rol = $('#FiltroRol').val();

}

function ManejarBotones() {
    $('#btn-editar-usuario').on('click', (e) => {
        e.preventDefault()
        const form = document.querySelector('#frm-editar-usuario')
        if (ValidarGuardarUsuarioEditar(form)) {
            GuardarUsuario(form)
        }
    })

    $('#btn-insertar-usuario').on('click', function (e) {
        e.preventDefault();
        if (ValidarGuardarUsuarioCrear()) {
            RegistrarNuevoUsuario();
        }
    });
    $('#btn-editar-rol').on('click', (e) => {
        e.preventDefault()
        const form = document.querySelector('#frm-editar-rol')
        if (ValidarGuardarRol(form)) {
            ActualizarRol(form)
        }
        /* toastr.success(`Rol editado con éxito`, "Éxito", toastrOptions)*/
        $("#modalEditarRol2").modal('hide')
    })
    $('#btn-insertar-rol').on('click', (e) => {
        e.preventDefault()
        const form = document.querySelector('#frm-insertar-rol')
        if (ValidarGuardarRol(form)) {
            ActualizarRol(form)
        }
    })

    $('#btn-guardar-permisos').on('click', (e) => {
        e.preventDefault()
        GuardarPermisos()
        //toastr.success('Se cambiaron los permisos del rol', 'Éxito', toastrOptions)
        $('#modalEditarPermisos').modal('hide')

    })

    $('#btn-guardar-permisosV2').on('click', (e) => {
        e.preventDefault()
        GuardarPermisosV2();
        //toastr.success('Se cambiaron los permisos del rol', 'Éxito', toastrOptions)
        $('#modalEditarPermisos').modal('hide')
    })

    $('#modulo').on('change', async function (e) {
        $('#ContenedorPermisos').html('')

        if (e.target.value) {
            //Crear combo de bubmodulos
            ManejadorModulos(e.target.value)
        }
        else {
            //Quitar combo de submodulos
            $('#ContenedorSubModulo').addClass('d-none')
            $('#submodulo').html("")
        }
    })

    $('#submodulo').on('change', function (e) {
        if (e.target.value) {
            ObtenerPermisos($('#modulo').val(), e.target.value)

        }
        else {
            $('#ContenedorPermisos').html('')

        }
    })
}

function CargarDatosUsuario( usuario) {

    $.ajax({
        url: `${rurl}/Obtener_Info_Usuario`,
        type: "GET",
        data: { usuario:usuario},
        success: function (data) {
            $.each(data, function (index, value) {
                $("#nombreMantenimientoUsuario").val(value.NOMBRE);
                $("#correo_electronicoMantenimientoUsuario").val(value.E_MAIL);
            });
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });
}
async function ManejadorModulos(modulo) {
    const url = `${rurl}/SubModulos?modulo=${modulo}`;

    try {
        const response = await fetch(url);
        const result = await response.json();

        if (result.length !== 0) {
            let html = '<option></option>';

            for (const submodulo of result) {
                // --- FILTRO: excluir EXPEDIENTE DIGITAL --------------------
                const desc = (submodulo.DESCRIPCION || '').trim().toUpperCase();
                if (desc === 'EXPEDIENTE DIGITAL') continue;

                html += `<option value="${submodulo.SUBMODULO}">
                            ${submodulo.DESCRIPCION}
                         </option>`;
            }

            $('#submodulo').html(html);
            $('#ContenedorSubModulo').removeClass('d-none');
        } else {
            $('#ContenedorSubModulo').addClass('d-none');
            $('#submodulo').html('');
            // Consultar permisos
            ObtenerPermisos(modulo, 0);
        }
    } catch (ex) {
        console.error(ex);
        $('#ContenedorPermisos').html('');
        $('#submodulo').html('');
    }
}
function ValidarGuardarUsuario() {
    const nombre = $('#nombreMantenimientoUsuario').val().trim();
    const usuario = $('#idusuarioMantenimientoUsuario').val().trim();
    const email = $('#correo_electronicoMantenimientoUsuario').val().trim();
    const pass = $('#passwordMantenimientoUsuario').val();
    const rpass = $('#rpasswordMantenimientoUsuario').val();
    const vendedor = $('#VendedorRegistrar').val();
    const cobrador = $('#CobradorRegistrar').val();
    const bodega = $('#BodegaRegistrar').val();
    const _iden = $('#identificacionMantenimientoUsuario').val().trim();

    if (!nombre || !usuario || !email || !pass || !rpass || vendedor === "" || cobrador === "" || bodega === "") {
        toastr.warning('Debe completar todos los campos obligatorios.', '¡Atención!');
        return false;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        toastr.warning('Formato de correo electrónico inválido.', '¡Atención!');
        return false;
    }

    if (pass !== rpass) {
        toastr.warning('Las contraseñas no coinciden.', '¡Atención!');
        return false;
    }
    if (_iden.length > 15) {
        toastr.warning('Identificación inválida. Máximo 15 dígitos.', '¡Alerta!', toastrOptions);
        return;
    }
    if (pass.length < 3) {
        toastr.warning('La contraseña debe tener al menos 3 caracteres.', '¡Atención!');
        toastr.warning('La contraseña debe tener al menos 3 caracteres.', '¡Atención!');
        return false;
    }

    return true;
}

async function ObtenerPermisos(modulo, tab) {
    const rol = $('#permisoRol').val()
    let url = `${rurl}/GetPermisosSubmodulo?modulo=${modulo}&rol=${rol}&tab=${tab}&&`
    try {
        const response = await fetch(url)
        const result = await response.json()
        let HTML = ""
        for (const permiso of result) {
            HTML += `<div class="col-4 col-form-label">
                            <label class="checkbox checkbox-primary">
                                <input type="checkbox" id="permiso-${permiso.PERMISO}" name="${permiso.PERMISO}" ${permiso.ACTIVO == 'S' ? 'checked' : ''} /*${rol == 1 ? 'disabled' : ''}*//>
                                <span></span>
                                ${permiso.DESCRIPCION}
                            </label>
                        </div>`
        }
        $('#ContenedorPermisos').html(HTML)


    }
    catch (ex) {
        console.error(ex)
        $('#ContenedorPermisos').html('')

    }
}


$(document).ready(function () {

    KTTablaUsuarios.init()
    ManejarBotones();
    ManejarFiltros();
});