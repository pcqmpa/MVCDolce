$(function () {

    var mensaje = $("#mensaje").val();

    var mensajeError = $("#Mensaje_error").val();

    if (mensaje != null || mensaje != '')
    {
        Materialize.toast(mensaje, 4000);
    }  

    if (mensajeError != null || mensajeError != '')
    {
        Materialize.toast(mensajeError, 4000);
       
    }

});