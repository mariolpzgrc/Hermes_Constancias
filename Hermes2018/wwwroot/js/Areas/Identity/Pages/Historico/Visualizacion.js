var objp = { urln: "" };
var objd = { urln: "" };
var objc = { urln: "" };
var obja = { urln: "" };

var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usersession: "" };
//--
var objParam = { envioId: 0, tipoEnvio: 0, existeAdjuntos: false };
var objParamOrigen = { envioId: 0, tipoEnvio: 0, existeAdjuntos: false };
//--
var objTipoVisualizacion = { envio: 1, recepcion: 2 };
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4 };
/****************************************************************************************************************************************
*                                                            Función: Main                                                              *
*****************************************************************************************************************************************/
$(function () {
    objUser.username = $("#user").text();
    objUser.usersession = $('#FolioSession').val();
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();

    objParam.envioId = parseInt($('#envio_identifica').text());
    objParam.tipoEnvio = parseInt($('#tipo_envio').text());
    objParam.existeAdjuntos = ($('#existe_adjuntos').text().toLowerCase() === 'true') ? true : false;

    objParamOrigen.envioId = parseInt($('#origen_envio_identifica').text());
    objParamOrigen.tipoEnvio = parseInt($('#origen_tipo_envio').text());
    objParamOrigen.existeAdjuntos = ($('#origen_existe_adjuntos').text().toLowerCase() === 'true') ? true : false;

    //Archivos adjuntos
    if (objParam.existeAdjuntos)
        LecturaArchivos();

    if (objParamOrigen.existeAdjuntos)
        LecturaArchivosOrigen();
});

/****************************************************************************************************************************************
*                                                       Función: Carga de archivos existentes                                            *
*****************************************************************************************************************************************/
var objListadoArchivos = { urln: "", paramFolio: "", datos: Object.keys(new Object()) };
function LecturaArchivos() {
    objListadoArchivos.urln = objWebRoot.route + "api/v1/anexos/descarga/envios";
    objListadoArchivos.paramFolio = $('#folioInput').text();
    objListadoArchivos.datos = Object.keys(new Object());

    //--
    $.ajax({
        url: objListadoArchivos.urln,
        type: "POST",
        data: JSON.stringify({ Folio: objListadoArchivos.paramFolio, TipoEnvioId: objParam.tipoEnvio, EnvioId: objEnvio.envioId }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (data !== '') {
                //--
                objListadoArchivos.datos = Object.keys(new Object());
                objListadoArchivos.datos = data;
                //--
                //Recorre
                $.each(objListadoArchivos.datos, function (i, item) {
                    $('#containerArchivos').append($('<span class="tm-tag cm-tag">' + '<a class="cmt-titulo" href="' + objWebRoot.route + item.Ruta + '" download>' + item.Nombre + '</a>' + '</span>'));
                });
                //--
            }
        }
    });
    //--
}

function LecturaArchivosOrigen() {
    objListadoArchivos.urln = objWebRoot.route + "api/v1/anexos/descarga/envios";
    objListadoArchivos.paramFolio = $('#folioInput').text();
    objListadoArchivos.datos = Object.keys(new Object());

    //--
    $.ajax({
        url: objListadoArchivos.urln,
        type: "POST",
        data: JSON.stringify({ Folio: objListadoArchivos.paramFolio, TipoEnvioId: objParamOrigen.tipoEnvio, EnvioId: objParamOrigen.envioId }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (data !== '') {
                //--
                objListadoArchivos.datos = Object.keys(new Object());
                objListadoArchivos.datos = data;
                //--
                //Recorre
                $.each(objListadoArchivos.datos, function (i, item) {
                    $('#containerArchivosOrigen').append($('<div class="tm-tag cm-tag">' + '<a class="cmt-titulo" href="' + objWebRoot.route + item.Ruta + '" download>' + item.Nombre + '</a>' + '</div>'));
                });
                //--
            }
        }
    });
    //--
}