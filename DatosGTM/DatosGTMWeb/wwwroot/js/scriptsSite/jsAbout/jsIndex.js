$(document).ready(function () {
    console.log("ready!");
});

function Request() {

    $.ajax({
        type: "GET",
        url: urlRequest,
        datatype: "json",
        success: function (data) {
            console.log(data);
            if (data.estado) {
                toastr.success(data.mensaje)
            }
            else
                toastr.warning(data.mensaje);
        }
    });

    return false;
}