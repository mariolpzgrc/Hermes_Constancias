var objp = { urln: "" };
var objd = { urln: "" };
var objc = { urln: "" };
var obja = { urln: "" };

var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usersession: "" };

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

    ControlAutocompletarPara();

    console.log($('#containerMailsFor').html());

    var texto = document.getElementById("containerMailsFor").innerHTML;
    console.log(texto);
    $.validator.setDefaults({ ignore: [] });
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
        data: JSON.stringify({ Folio: objListadoArchivos.paramFolio, TipoEnvioId: objParam.tipoEnvio, EnvioId: objParam.envioId }),
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
    objListadoArchivos.urln = objWebRoot.route + "api/v1/anexos/descarga/enviosOrigen";
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


/****************************************************************************************************************************************
*                                                       Función: Enviar por correo                                                      *
*****************************************************************************************************************************************/
function toPlainString(listString) {
    var plainString = "";
    listString.forEach(function (element, index, array) {
        if (array.length - 1 === index) {
            plainString += element;
        } else {
            plainString += element + ",";
        }
    });
    return plainString;

}

function getEmailsParaToSend() {

    var TagArray = []
    var values = $('#form-autocomplete-para').val();

    if (values) {
        var TagValues = JSON.parse(values);
        TagValues.forEach(function (element) {
            if (!TagArray.includes(element.value)) {
                TagArray.push(element.value);
            }
        });
        return toPlainString(TagArray);
    }

}
function updateHelperDataEmails() {
    //var num = 2;
    $("#EnvioCorreo_Correo").val(getEmailsParaToSend());
}

function ControlAutocompletarPara() {
    // The DOM element you wish to replace with Tagify
    //var input = document.querySelector('input[id=form-autocomplete-para]');
    const inputPara = document.getElementById("form-autocomplete-para");

    updateHelperDataEmails();

    function checkAddTag() {
        updateHelperDataEmails();
        //alert($("#EnvioCorreo_Correo").val());
    }


    // initialize Tagify on the above input node reference
    //new Tagify(input)

    tagifyEmailsPara = new Tagify(inputPara, {
        maxTags: 20,
        pattern: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    });

    //updateHelperDataEmails();
    tagifyEmailsPara.on('add', checkAddTag);
}
