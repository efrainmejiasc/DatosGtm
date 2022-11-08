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

function UploadFileMethod() {
    var input = document.getElementById('theFile');
    var files = input.files;
  

    if (files.length == 0) {
        toastr.warning("Debe elegir un archivo");
        return false;
     }
    else if (files.length > 1) {
        toastr.warning("Solo puede subir un archivo a la vez");
        return false;
    }

    var formData = new FormData();
    for (var i = 0; i < files.length; i++) {
        formData.append('file', files[i]);
    }

    $.ajax(
        {
            url: urlUploadFile,
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (data) {
                if (data.estado) {
                    toastr.success(data.mensaje);
                    $('#theFile').val(''); 
                }
                else 
                    toastr.error(data.mensaje)
            }
        }
    );
}


function LeerArchivo() {
 
    $.ajax(
        {
            url: urlLeerArchivo,
            type: "POST",
            success: function (data) {
                if (data.estado) {
                    toastr.success(data.mensaje);
                }
                else
                    toastr.error(data.mensaje)
            }
        }
    );
}
