var objWebRoot = { route: "", token: "" },
    objUser = { username: "", usersession: "" },
    objtr = { elementobase: Object.keys(new Object()) },
    objGetInfoConstancia = { Id: 0, info: Object.keys(new Object()), urln: "" };

$(document).ready(function () {
    getInfoConstancia();
    ControlToggle();
})

function getInfoConstancia() {
    var url = window.location.pathname;
    var id = parseInt(url.substring(url.lastIndexOf('/') + 1));
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/GET_ConstanciaSolicitadaId';
    $.ajax({
        url: objGetInfoConstancia.urln,
        type: 'POST',
        data: JSON.stringify({ Id: id }),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            objGetInfoConstancia.info = Object.keys(new Object());
            objGetInfoConstancia.info = data;
            $('#folio').text(Mayuscula(objGetInfoConstancia.info.Folio));
            $('#tipoConstancia').text(objGetInfoConstancia.info.NombreConstancia);
            $('#idtipoConstancia').text(objGetInfoConstancia.info.ConstanciaId)
            $('#tituloConstancia').text(objGetInfoConstancia.info.NombreConstancia);
            $('#constanciaPara').text(objGetInfoConstancia.info.NombreUsuario.toUpperCase());
            $('#dependencia').text(objGetInfoConstancia.info.NombreDependencia);
        },
        error: function () {
            alert('No pudó terminar la operación');
        },
        complete: function (xhr, estatus) {
            console.log('paso aqui');
            if ((objGetInfoConstancia.info.EstadosConstancia).length > 0) {
                $.each(objGetInfoConstancia.info.EstadosConstancia, function (index, value) {
                    objtr.elementobase = Object.keys(new Object());
                    if (value.EstadoId === 1) {
                        objtr.elementobase = $('#infoEstado').append(

                            $('<div class="info-border col-12 pb-1">').append(
                                $('<div class="p-2 mb-1 border-top border-bottom-0 bg-light"><strong> El trámite se realizo el: ' + moment(value.FechaHora).locale('es').format('DD[/]MM[/]YYYY') + '</strong><button type="button" class="btn btn-link float-right btn-sm" id="control-toggle"><i class="fas fa-angle-down"></i></button></div>')
                            ),
                        )
                    } else if (value.EstadoId === 2) {
                        objtr.elementobase = $('#infoEstado').append(

                            $('<div class="info-border col-12 pb-1">').append(
                                $('<div class="p-2 mb-1 border-top border-bottom-0 bg-light"><strong> El trámite se autorizo el: ' + moment(value.FechaHora).locale('es').format('DD[/]MM[/]YYYY') + '</strong><button type="button" class="btn btn-link float-right btn-sm" id="control-toggle"><i class="fas fa-angle-down"></i></button></div>')
                            ),
                        )
                    } else if (value.EstadoId === 3) {
                        objtr.elementobase = $('#infoEstado').append(

                            $('<div class="info-border col-12 pb-1">').append(
                                $('<div class="p-2 mb-1 border-top border-bottom-0 bg-light"><strong> El trámite se imprimió el: ' + moment(value.FechaHora).locale('es').format('DD[/]MM[/]YYYY') + '</strong><button type="button" class="btn btn-link float-right btn-sm" id="control-toggle"><i class="fas fa-angle-down"></i></button></div>')
                            ),
                        )
                    } else if (value.EstadoId === 4) {
                        objtr.elementobase = $('#infoEstado').append(

                            $('<div class="info-border col-12 pb-1">').append(
                                $('<div class="p-2 mb-1 border-top border-bottom-0 bg-light"><strong> El trámite se re-imprimió el: ' + moment(value.FechaHora).locale('es').format('DD[/]MM[/]YYYY') + '</strong><button type="button" class="btn btn-link float-right btn-sm" id="control-toggle"><i class="fas fa-angle-down"></i></button></div>')
                            ),
                        )
                    } else if (value.EstadoId === 5) {
                        objtr.elementobase = $('#infoEstado').append(

                            $('<div class="info-border col-12 pb-1">').append(
                                $('<div class="p-2 mb-1 border-top border-bottom-0 bg-light"><strong> El trámite se entregó el: ' + moment(value.FechaHora).locale('es').format('DD[/]MM[/]YYYY') + '</strong><button type="button" class="btn btn-link float-right btn-sm" id="control-toggle"><i class="fas fa-angle-down"></i></button></div>')
                            ),
                        )
                    } else if (value.EstadoId === 2) {
                        objtr.elementobase = $('#infoEstado').append(

                            $('<div class="info-border col-12 pb-1">').append(
                                $('<div class="p-2 mb-1 border-top border-bottom-0 bg-light"><strong> El trámite se canceló el: ' + moment(value.FechaHora).locale('es').format('DD[/]MM[/]YYYY') + '</strong><button type="button" class="btn btn-link float-right btn-sm" id="control-toggle"><i class="fas fa-angle-down"></i></button></div>')
                            ),
                        )
                    }

                });
            }
        }
    });
}

/**    Control de Toggle **/
function ControlToggle() {
    $("#control-toggle").click(function () {
        $("#contenido-info").toggle({
            duration: 'fast',
            complete: function () {
                if ($('#contenido-info:visible').length === 0) {
                    $("#control-toggle i").removeClass('fas fa-angle-down').addClass('fas fa-angle-right');
                } else {
                    $("#control-toggle i").removeClass('fas fa-angle-right').addClass('fas fa-angle-down');
                }
            }
        });
    });
}

function Mayuscula(string) {
    return string.toUpperCase();
}

function Estado(id) {
    let Estado = "";
    switch (id) {
        case 1: Estado = "En trámite";  break;
        case 2: Estado = "Autorizada"; break;
        case 3: Estado = "Impresa"; break;
        case 4: Estado = "Re-Impresa"; break;
        case 5: Estado = "Entregada"; break;
        case 6: Estado = "Cancelada"; break;
    }
    return Estado;
}
