$(document).ready(function () {
    console.log("ready!");

    var date = FechaActual();
    $('#fechaNacimiento').val(date);

});


function FechaActual() {

    var today = new Date();
    var year = today.getFullYear();
    var moth = (today.getMonth() + 1) <= 9 ? "0" + (today.getMonth() + 1) : (today.getMonth() + 1);
    var day = today.getDate() <= 9 ? "0" + today.getDate() : today.getDate();
    var date = year + '-' + moth + '-' + day;

    return date;

}

function GetBearerValue() {
   var miStorage = window.localStorage;
    var bearer = localStorage.getItem("accessToken")
    var bearer2 = localStorage.getItem("i18nextLng")
    console.log(bearer);
    console.log(bearer2);
    console.log(miStorage);
    console.log(Window.sessionStorage);
    console.log(Window.localStorage);
}

