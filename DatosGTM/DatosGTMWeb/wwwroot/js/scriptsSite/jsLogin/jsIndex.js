$(document).ready(function () {
    console.log("ready!");
});

function Login() {

    var userMail = $('#userMail').val();
    var password = $('#password').val();
    var confirmar = document.getElementById('confirmar').checked;

    if (userMail === '' || password === '' || !confirmar) {
        toastr.warning("Todos los campos son requeridos");
        return false;
    }


    $.ajax({
        type: "GET",
        url: urlLogin,
        data: { userMail: userMail, password: password },
        datatype: "json",
        success: function (data) {
            if (data.estado) {
                toastr.success(data.mensaje);
                setTimeout(IrHome, 2000);
            }
            else
                toastr.warning(data.mensaje);
        }
    });
    return false;
}

function IrHome() {
    window.location.href = urlHome;
}
function EmailValido(mail) {
    const regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(mail);
}

