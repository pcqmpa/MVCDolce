$(function () {

    var mensaje = $("#mensaje").val();

    var mensajeError = $("#Mensaje_error").val();

    if (mensaje != null || mensaje != '') {
        Materialize.toast(mensaje, 4000);
    }

    if (mensajeError != null || mensajeError != '') {
        Materialize.toast(mensajeError, 4000);

    }


    $("#btn-consultaAsesora-motivacion").click(function () {

        var url = $("#urlConsulta").val();
        var documento = $("#txt-cedula").val();

        $.ajax({
            url: url,
            data: { strDocumento: documento },
            type: "POST"
        }).done(function (res) {

            if (res.Error != undefined) {
                Materialize.toast(res.Error, 4000);
            }
            else 
            {
                $("#txt-puntos").val(res.datos.NumPuntos);
                $("#StrNombre").val(res.datos.StrNombre);
                $("#txt-ultimacampana").val(res.datos.StrUltimaCampaña);
            }
            

        }).fail(function (error) {
            alert(error.error);
        });




    });

});