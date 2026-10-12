

const Login = (fd, btn) => {
    const url = `${RURL}Index`;
    btn.attr('data-kt-indicator', 'on');
    btn.attr('disabled', true);

    fetch(url, { method: 'POST', body: fd })
        .then(res => res.json())
        .then(data => {
            if (data === 1) {
                toastr.success('Acceso exitoso', 'Bienvenido', toastrOptions);
                localStorage.removeItem('id_visita_actual');
                setTimeout(() => window.location.reload(), 1000);
                return;
            }

            const messages = {
                2: 'Usuario o contraseña invalido',
                3: 'El usuario no tiene permisos en esta compañía',
                4: 'El usuario no tiene permisos para ingresar al sistema',
                5: 'El sistema no tiene una licencia configurada.',
                6: 'No se pudo validar la licencia. Contacte a soporte.',
                7: 'La licencia ha vencido. Contacte a Produsoft para renovarla.'
            };

            if (!messages[data]) {
                toastr.error('Respuesta inesperada del servidor.',
                    'Error', toastrOptions);
                return;
            }

            if (data === 6) {
                toastr.error(messages[data], 'Licencia', toastrOptions);
            } else {
                toastr.warning(messages[data],
                    data >= 5 ? 'Licencia' : 'Alerta', toastrOptions);
            }
        })
        .catch(err => {
            toastr.error('Error en la petición! recargue e intente de nuevo',
                'Error', toastrOptions);
            console.error(err);
        })
        .finally(() => {
            btn.removeAttr('data-kt-indicator');
            btn.attr('disabled', false);
        });
};


const HandleFormSubmits = () => {
    $('#login-form').on('submit', (e) => {
        e.preventDefault();
        const btn = $('#kt_sign_in_submit');
        if (btn.prop('disabled')) return;

        const form = document.querySelector('#login-form');
        const fd = new FormData(form);
        if (fd.get('username') == '') {
            toastr.warning('Debe ingresar su usuario', 'Alerta', toastrOptions);
            $('#usuario').trigger('focus');
        } else if (fd.get('password') == '') {
            toastr.warning('Debe ingresar su contraseña', 'Alerta', toastrOptions);
            $('#passwordL').trigger('focus');
        } else if (fd.get('cia') == '') {
            toastr.warning('Debe seleccionar una compañía', 'Alerta', toastrOptions);
        } else {
            Login(fd, btn);
        }
    });
};


function guardarUsuario() {

    let bandera = false
    const _iden = "999999999";
    const _nombre = $('#nomb').val();
    const _usuario = $('#idusuario').val();
    const _email = $('#correo_electronico').val();
    const _pass = $('#password').val();
    const _rpass = $('#rpasssword').val();
    const _terminos = $('#Terminos').is(':checked');
    const formulario = [_iden, _nombre, _usuario, _email, _pass]
    /*console.log(_terminos);*/
    if (formulario.includes('')) {
        toastr.warning('Se deben completar todos los campos.', 'Alerta!', toastrOptions)
        return
    }
    if (_pass != _rpass) {
        toastr.warning('Las contraseñas no coinciden.', 'Alerta!', toastrOptions)
        return
    }

    //if (!_terminos) {
    //    toastr.warning('Debe aceptar los terminos y condiciones', 'Alerta!', toastrOptions)
    //    return
    //}

    Dajax(`${RURL}ValidaExistenciaUsuario`, //_url + _sistema + "Login/ValidaExistenciaUsuario",
        "POST", { usuario: _usuario, correo: _email, identificacion: _iden }, function (response) {
            if (response.responseJSON) {
                bandera = false;
                if (response.responseJSON.message == "Éxito") {
                    if (response.responseJSON.data.EXISTE == 1) {
                        toastr.warning('El usuario ya existe.', 'Error', toastrOptions)
                        usuarioExistente = false;
                        bandera = false

                    } else {
                        if (response.responseJSON.data.EXISTE == 2) {
                            toastr.warning("El correo electrónico ya está en uso, intente otro correo electrónico.", "Error", toastrOptions);
                            usuarioExistente = false;
                            bandera = false
                        } else {
                            if (response.responseJSON.data.EXISTE == 3) {
                                toastr.warning("El número de identificiación ya está en uso, intente con otro.", "Error", toastrOptions);
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
                Dajax(`${RURL}RegistrarUsuario`,
                    "POST", { CEDULA: _iden, NOMBRE: _nombre, USUARIO: _usuario, EMAIL: _email, PASS: _pass }, function (response) {
                        if (response.responseJSON.message == "Exito") {
                            toastr.success('El usuario se ha registrado correctamente', 'Bienvenido', toastrOptions);
                        } else {
                            toastr.warning('No se ha podido registrar el usuario debido a un error', 'Error', toastrOptions)
                        }
                    });
            }

        });
    setTimeout(() => { window.location.reload(); }, 2000)
    // $("#CambiarestadoPedido").modal("hide")
    /*KTDataTable.reset();*/
}
function registrar() {

    $('#login-signin').addClass("d-none")
    $('#register-confirm').addClass("d-none")
    $('#login-signup').removeClass("d-none")
    $('#login-signup').addClass("d-flex")
    $('#tipoide').select2({
        placeholder: "Seleccione un tipo",
    })
};

function forgot() {
    $('#login-signin, #login-signup').addClass('d-none');
    $('#register-confirm').removeClass('d-none').addClass('d-flex');
    $('#kt_app_root').attr('aria-labelledby', 'recovery-title');
    $('#recovery-title').trigger('focus');
};

function volverAlLogin() {
    $('#register-confirm, #login-signup').addClass('d-none');
    $('#login-signin').removeClass('d-none');
    $('#kt_app_root').attr('aria-labelledby', 'login-title');
    $('#kt_login_forgot').trigger('focus');
};

function reg() {
    toastr.success(`Usuario registrado con éxito`, "Éxito", toastrOptions)
    setTimeout(() => { window.location.reload(); }, 3000)
};
$('#basic-addon2').on('click', (e) => {
    e.preventDefault();
    const mostrar = $('#passwordL').attr('type') === 'password';
    $('#passwordL').attr('type', mostrar ? 'text' : 'password');
    $('#icono').toggleClass('la-eye-slash', !mostrar).toggleClass('la-eye', mostrar);
    $('#basic-addon2')
        .attr('aria-label', mostrar ? 'Ocultar contraseña' : 'Mostrar contraseña')
        .attr('aria-pressed', String(mostrar));
});
$(document).ready(() => {
    $('#CIA').select2({
        placeholder: "Seleccione una compañía",
    })
    HandleFormSubmits()
})
