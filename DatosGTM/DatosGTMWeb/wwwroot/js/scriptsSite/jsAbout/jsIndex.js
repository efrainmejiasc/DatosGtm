$(document).ready(function () {
    console.log("ready!");
    $('#loading').hide();
    GetTerceros();
});

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

    $('#loading').show();

    $.ajax({
            url: urlUploadFile,
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            timeout: 10000000000000, 
            success: function (data)
            {
                console.log(data);
                if (data.estado) {
                    console.log(data.tercero);
                    toastr.success(data.mensaje);
                    $('#theFile').val('');
                    $('#tablaTercero tbody tr').remove();
                    $.each(data.tercero, function (index, item) {
                        let tr = `<tr>
                      <td> ${item.numero} </td>
                      <td> ${item.nit} </td>
                      <td> ${item.nombre} </td>
                      <td> ${item.fechaInicio} </td>
                      </tr>`;
                       $('#tablaTercero tbody').append(tr);
                    });
                    //setTimeout(InicializarDataTable, 1000);
                    $('#loading').hide();
                }
                else {
                    toastr.error(data.mensaje);
                    $('#loading').hide();
                }
            }, error: function (jqXHR, textStatus, errorThrown)
               {
                 $('#tablaTercero tbody tr').remove();
                 $('#loading').hide();
                 toastr.error('ERROR INESPERADO: ' + textStatus + ' ' + jqXHR + ' ' + errorThrown);
            }
    });

    return false;
}


function GetTerceros() {

    $('#loading').show();

    $.ajax(
        {
            url: urlGetTerceros,
            type: "GET",
            success: function (data)
            {
                if (data != null) {
                    $('#tablaTercero tbody tr').remove();
                    console.log(data);
                    $.each(data, function (index, item) {
                        let tr = `<tr>
                      <td> ${item.numero} </td>
                      <td> ${item.nit} </td>
                      <td> ${item.nombre} </td>
                      <td> ${item.fechaInicio} </td>
                      </tr>`;
                        $('#tablaTercero tbody').append(tr);
                    });
                    setTimeout(InicializarDataTable, 1000);
                    $('#loading').hide();
                }
                else {
                    $('#tablaTercero tbody tr').remove();
                    toastr.error(data.mensaje);
                    $('#loading').hide();
                }
            }, error: function (jqXHR, textStatus, errorThrown) {
                $('#tablaTercero tbody tr').remove();
            }
        }
    );

    return false;
}

function InicializarDataTable() {
    var init = $('#initDataTable').val();
   // $.noConflict();
    try {
        if (init === 'no') {
            $('#tablaTercero').DataTable({
                language: {
                    "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
                },
                "bInfo": false,
                "lengthChange": false,
                pagingType: "simple"
            });
            $('#initDataTable').val('si');
        } else {
            $('#tablaTercero').DataTable().fnDestroy({
                language: {
                    "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
                },
                "bInfo": false,
                "lengthChange": false,
                pagingType: "simple"
            });
        }
        $("#tablaTercero").addClass("display compact dt-center"); }
    catch { console.log(''); }
}





