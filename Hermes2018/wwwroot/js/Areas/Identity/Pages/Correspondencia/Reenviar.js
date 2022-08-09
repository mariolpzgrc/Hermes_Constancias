/**
 * 
 * 
 **/
var objWebRoot = { route: "", token: "" };
var tagifyEmails;
var tagifyEmailsCPP;
var objUser = { username: "", usersession: "" };
var objExtensiones = { valores: "image/*,application/pdf,text/plain,.doc,.docx,.xlsx,.xls,.ppt" };
var objListadoArchivos = { uri: "", paramFolio: "", datos: Object.keys(new Object()) };
var objParam = { envioId: 0, tipoEnvio: 0 };
var objAdjuntos = { existen: false };
var objFolio = { folio: "" };

//----Editor
var objEditor = { froala: null };

//---Calendario 
var objCalendario = { datePicker: null, dias: "", datos: new Array(), inicio: "", limite: "" };
/****************************************************************************************************************************************
*                                                            Función: Main                                                              *
*****************************************************************************************************************************************/
$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    objUser.usersession = $('#Reenviar_FolioSession').val();
    objExtensiones.valores = $("#Reenviar_Extensiones").val();
    objFolio.folio = $("#folioInput").val();
    objParam.envioId = parseInt($('#envio_identifica').text());
    objParam.tipoEnvio = parseInt($('#tipo_envio').text());
    objAdjuntos.existen = ($('#Reenviar_ExisteAdjuntos').val().toLowerCase() === 'true') ? true : false;
    //--
    objCalendario.dias = $('#Inhabiles_Dias').val();
    objCalendario.datos = objCalendario.dias.split(',');
    objCalendario.inicio = $('#Inhabiles_Inicio').val();
    objCalendario.limite = $('#Inhabiles_Limite').val();
    //--
    ControlAutocompletarPara();
    ControlAutocompletarConCopia();
    ControlDropzone();
    CargaArchivosSoloLectura();

    //-------
    objEditor.froala = new FroalaEditor('#Reenviar_Indicaciones', {
        key: "UBB7jE6D5G3I3A2A6aIVLEABVAYFKc1Ce1MYGD1c1NYVMiB3B9B6B5C2B4C3H3I3I3==",
        toolbarButtons: [
            'fullscreen',
            'bold',
            'italic',
            'fontSize',
            'underline',
            '|',
            'align',
            'formatOL',
            'formatUL',
            '|',
            'insertLink',
            '|',
            'clearFormatting',
            'selectAll',
            '|',
            'undo',
            'redo'
        ],

        attribution: false,
        //charCounterCount: false,
        scaytAutoload: true,
        language: 'es',
        iframe: true,
        //height: 400,
        heightMin: 300,
        heightMax: 400,
        theme: 'gray',
        //zIndex: 2001,
        placeholderText: 'Escriba el contenido',
        wordPasteModal: true,
        pastePlain: true,
        wordDeniedAttrs: ['style', 'width', 'height'],
        quickInsertTags: [''],
        fontSizeSelection: true,
        fontSize: ['8', '10', '12', '14', '16', '18'],
        fontSizeDefaultSelection: '16',
        listAdvancedTypes: false,
    });

    //---
    objCalendario.datePicker = flatpickr('#fechaCompromiso', {
        dateFormat: "d/m/Y",
        minDate: "today", //objCalendario.inicio
        locale: {
            firstDayOfWeek: 7,
            weekdays: {
                shorthand: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                longhand: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            },
            months: {
                shorthand: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Оct', 'Nov', 'Dic'],
                longhand: ['Enero', 'Febrero', 'Мarzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            },
        },
        disable: objCalendario.datos,
        maxDate: objCalendario.limite
    });
    //--
    $("#Reenviar_RequiereRespuesta").change(function () {
        if ($("#Reenviar_RequiereRespuesta option:selected").val() == 'false') {
            $('#fechaCompromiso').val('');
            $('#contieneFechapc').hide();
        } else {
            $('#contieneFechapc').show();
        }
    });
});
$.validator.setDefaults({ ignore: [] });
Dropzone.autoDiscover = false;
/****************************************************************************************************************************************
*                                                   Start scripts for autocomplete input                                                  *
*****************************************************************************************************************************************/
var objp = { urln: "" };
var dominioCorreo = "@uv.mx";
function getRoute(inputValue) {
    return objWebRoot.route + "api/v1/users/local/" + inputValue;
}
function filterProperties(whiteList) {
    var newList = [];
    var obj;
    for (var i = 0; i < whiteList.length; i++) {
        obj = {
            "value": $.trim(whiteList[i].HER_NombreCompleto),
            "userName": $.trim(whiteList[i].HER_UserName),
            "searchBy": $.trim(whiteList[i].HER_UserName) + " " + $.trim(whiteList[i].HER_Area),
            "puesto": $.trim(whiteList[i].HER_Puesto),
            "area": $.trim(whiteList[i].HER_Area)
        };
        newList.push(obj);
    }
    return newList;
}
function checkUndefined(value) {
    return value || '';
}
function checkPuestoyArea(data) {
    var puestoyArea = "";
    if (data.puesto) {
        puestoyArea = "<br><strong>Puesto: </strong>" + data.puesto;
    }
    if (data.area) {
        puestoyArea += "<br><strong>Area: </strong>" + data.area;
    }
    return puestoyArea;
}
function getEmailDetails(data) {
    var details;
    if (data.miembros) {
        details = "<ul type='circle'>";
        data.miembros.forEach(function (item) {
            details += "<li>" + checkUndefined(item.nombre) + " - <i>" + item.userName + dominioCorreo + "</i></li>";
        });
        details += "</ul>"
    } else {
        details = data.userName + dominioCorreo;
    }
    return details;
}
function toPlainString(listString) {
    var plainString = "";
    listString.forEach(function (element, index, array) {
        if (array.length - 1 == index) {
            plainString += element;
        } else {
            plainString += element + ",";
        }
    });
    return plainString;
}
function getEmailsParaToSend() {
    var finalList = [];
    var values = $('#form-autocomplete-para').val();
    if (values) {
        var emailList = JSON.parse(values);
        emailList.forEach(function (element) {
            if (element.miembros) {
                element.miembros.forEach(function (miembro) {
                    if (!finalList.includes(miembro.userName)) {//Value is the reference to Nombre by tag component
                        finalList.push(miembro.userName);
                    }
                });
            } else {
                if (!finalList.includes(element.userName)) {
                    finalList.push(element.userName);
                }
            }
        });
        return toPlainString(finalList);
    }
}
function getEmailsCCPToSend() {
    var finalList = [];
    var values = $('#form-autocomplete-ccp').val();
    if (values) {
        var emailList = JSON.parse(values);
        emailList.forEach(function (element) {
            if (!finalList.includes(element.userName)) {//Value is the reference to Nombre by tag component
                finalList.push(element.userName);
            }
        });
        return toPlainString(finalList);
    }
}
function updateHelperDataEmails() {
    $("#Reenviar_Para").val(getEmailsParaToSend());//Set up the new For's values to asp helper
    $("#Reenviar_CCP").val(getEmailsCCPToSend());//Set up the new CCP's values to asp helper  
}
/****************************************************************************************************************************************
*                                                   Script autocomplete input To/Para                                                   *
*****************************************************************************************************************************************/

function ControlAutocompletarPara() {
    const inputFor = document.getElementById("form-autocomplete-para");

    function notifyFacadeFor(title, text, time) {
        $.getScript(objWebRoot.route + "lib/bootstrap-notify/bootstrap-notify.js", function () {
            $.notify({
                title: title,
                message: text
            }, {
                type: "info",
                element: "#tagsEmailPara",
                delay: time,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                }
            });
        });
    }
    function uniqueTagsIncludingCCP(data, dataPersona) {
        if (data) {
            var arrayData = JSON.parse(data);
            arrayData.forEach(function (elementTag) {
                if (dataPersona.userName == elementTag.userName) {
                    tagifyEmailsCCP.removeTag(elementTag.value);
                }
            });
        }
    }
    function checkOnInput(e) {
        var valueInput = e.detail;
        if (valueInput.length > 2) {
            var route;
            var listEmails = [];
            tagifyEmails.settings.whitelist.length = 0; // reset the whitelist
            controller && controller.abort();
            controller = new AbortController();
            route = getRoute(valueInput);
            fetch(route, {
                //signal: controller.signal,
                headers: {
                    'Authorization': "Bearer " + objWebRoot.token
                }
            })
                .then(RES => RES.json())
                .then(function (whitelist) {
                    //Remove current user
                    if (whitelist.findIndex(i => i.HER_UserName === objUser.username) > -1) {
                        whitelist.splice(whitelist.findIndex(i => i.HER_UserName === objUser.username), 1);
                    }
                    //--
                    listEmails = filterProperties(whitelist);
                    tagifyEmails.settings.whitelist = listEmails;
                    tagifyEmails.dropdown.show.call(tagifyEmails, valueInput); // render the suggestions dropdown
                })
                .catch(error => {
                    //console.log("Petición cancelada")
                });
        }
    }
    function checkAddTag(input) {
        var data = JSON.parse($('#form-autocomplete-para').val());
        var dataPersona = input.detail.data;
        data = $('#form-autocomplete-ccp').val();
        uniqueTagsIncludingCCP(data, dataPersona)

        updateHelperDataEmails();
    }
    function checkRemovedTag() {
        updateHelperDataEmails();
    }
    function showDetails(input) {
        var data = input.detail.data;
        var informacion;
        informacion = "<br><strong>Nombre: </strong>" + data.value +
            checkPuestoyArea(data) +
            "<br><strong>Correo electrónico:</strong><br><i>" + getEmailDetails(data) + "</i>";
        notifyFacadeFor("Detalles del contacto", informacion, 5000);
    }
    tagifyEmails = new Tagify(inputFor, {
        whitelist: [],
        enforceWhitelist: true,
        dropdown: {
            enabled: 3,
            fuzzySearch: true,
            itemTemplate: function (tagData) {
                return `<div class=tagify__dropdown__item>
                    <span style="font-weight: bold">${tagData.value}</span>
                    <span style="font-size: small;"> (${tagData.userName}) </span>
                    <span style="font-size: small; font-style: italic;"> - ${tagData.puesto} - </span>
                    <span style="font-size: small;">${tagData.area}</span> 
                </div>`
            }
        }
    });
    var controller;
    tagifyEmails.on('input', checkOnInput);
    tagifyEmails.on('add', checkAddTag);
    tagifyEmails.on('remove', checkRemovedTag);
    tagifyEmails.on('click', showDetails);
}
/****************************************************************************************************************************************
*                                                   Script autocomplete input CCP                                                   *
*****************************************************************************************************************************************/

function ControlAutocompletarConCopia() {
    const inputCCP = document.getElementById("form-autocomplete-ccp");

    function notifyFacadeCCP(title, text, time) {
        $.getScript(objWebRoot.route + "lib/bootstrap-notify/bootstrap-notify.js", function () {
            $.notify({
                title: title,
                message: text
            }, {
                type: "info",
                element: "#tagsEmailCCP",
                delay: time,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                }
            });
        });
    }
    function uniqueTagsIncludingPara(data, dataPersona) {
        if (data) {
            var arrayData = JSON.parse(data);
            arrayData.forEach(function (elementTag) {
                if (dataPersona.userName == elementTag.userName) {
                    tagifyEmailsCCP.removeTag(dataPersona.value);
                    $('#labelPara').removeClass().addClass("shake" + ' animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
                        $(this).removeClass();
                    });
                }
            });
        }
    }
    function checkOnInputCCP(e) {
        var valueInput = e.detail;
        if (valueInput.length > 2) {
            var route;
            var listEmails = [];
            tagifyEmailsCCP.settings.whitelist.length = 0; // reset the whitelist
            controllerCCP && controllerCCP.abort();
            controllerCCP = new AbortController();
            route = getRoute(valueInput);
            fetch(route, {
                signal: controllerCCP.signal,
                headers: {
                    'Authorization': "Bearer " + objWebRoot.token
                }
            })
                .then(RES => RES.json())
                .then(function (whitelist) {
                    //Remove current user
                    if (whitelist.findIndex(i => i.HER_UserName === objUser.username) > -1) {
                        whitelist.splice(whitelist.findIndex(i => i.HER_UserName === objUser.username), 1);
                    }
                    //--
                    listEmails = filterProperties(whitelist);
                    tagifyEmailsCCP.settings.whitelist = listEmails;
                    tagifyEmailsCCP.dropdown.show.call(tagifyEmailsCCP, valueInput); // render the suggestions dropdown
                })
                .catch(error => {
                    //console.log("Petición cancelada")
                });
        }
    }
    function checkAddTagCCP(input) {
        var dataRaw = $('#form-autocomplete-para').val();
        var dataPersona = input.detail.data;
        uniqueTagsIncludingPara(dataRaw, dataPersona);
        updateHelperDataEmails();
    }
    function checkRemoveTagCCP() {
        updateHelperDataEmails();
    }
    function showDetailsCCP(input) {
        var data = input.detail.data;
        var informacion;
        informacion = "<br><strong>Nombre: </strong>" + data.value +
            checkPuestoyArea(data) +
            "<br><strong>Correo electrónico: </strong><br><i>" + getEmailDetails(data) + "</i>";
        notifyFacadeCCP("Detalles del contacto", informacion, 5000);
    }
    tagifyEmailsCCP = new Tagify(inputCCP, {
        whitelist: [],
        enforceWhitelist: true,
        dropdown: {
            enabled: 3,
            fuzzySearch: true,
            itemTemplate: function (tagData) {
                return `<div class=tagify__dropdown__item>
                    <span style="font-weight: bold">${tagData.value}</span>
                    <span style="font-size: small;"> (${tagData.userName}) </span>
                    <span style="font-size: small; font-style: italic;"> - ${tagData.puesto} - </span>
                    <span style="font-size: small;">${tagData.area}</span> 
                </div>`
            }
        }
    });
    var controllerCCP;
    tagifyEmailsCCP.on('input', checkOnInputCCP);
    tagifyEmailsCCP.on('add', checkAddTagCCP);
    tagifyEmailsCCP.on('remove', checkRemoveTagCCP);
    tagifyEmailsCCP.on('click', showDetailsCCP);
}

/****************************************************************************************************************************************
*                                                            Dropzone                                                                   *
*****************************************************************************************************************************************/
function ControlDropzone() {
    var objarchivos = { listado: new Array(), cadena: "", indice: 0 };
    var optionsDropzone = {
        url: objWebRoot.route + "api/v1/anexos/subir/archivo/temporal/" + objUser.usersession,
        acceptedFiles: objExtensiones.valores,
        dictDefaultMessage: "Arraste sus archivos aquí o haga clic para abrir el explorador",
        dictFallbackMessage: "Su navegador no admite la carga de archivos por medio de arrastrar y soltar.",
        dictFallbackText: "Utilice el formulario de reserva para cargar sus archivos como en los viejos tiempos.",
        dictFileTooBig: "El archivo es demasiado grande ({{filesize}}MiB). Tamaño máximo de archivo: {{maxFilesize}}MiB.",
        dictInvalidFileType: "No puedes subir archivos de este tipo.",
        dictResponseError: "El servidor respondió con el código {{statusCode}}.",
        dictCancelUpload: "Cancelar carga",
        dictCancelUploadConfirmation: "¿Estás seguro de que deseas cancelar esta carga?",
        dictRemoveFile: "Remover archivo",
        dictMaxFilesExceeded: "No puedes subir más archivos.",
        dictRemoveFile: "x",
        createImageThumbnails: true,
        addRemoveLinks: true,
        maxFiles: 10,
        maxFilesize: 25,
        success: function (file, response) {

            if (response.trim().length > 0) {
                //Si ya existe un archivo con el mismo nombre, no agregar al arreglo
                if ($.inArray(response, objarchivos.listado) === -1) {
                    //Se agrega el nombre del archivo a un arreglo
                    objarchivos.listado.push(response);
                    //Se recorre el arrreglo de los archivos alamacenados
                    objarchivos.cadena = "";
                    $.each(objarchivos.listado, function (index, value) {
                        objarchivos.cadena = objarchivos.cadena + value;

                        if (index + 1 < objarchivos.listado.length) {
                            objarchivos.cadena = objarchivos.cadena + ",";
                        }
                    });
                    //Agrega la cadena
                    $("#Reenviar_Anexos").val(objarchivos.cadena);
                   
                }
            } else {
                //file.previewElement.classList.add("dz-error");
                this.removeFile(file);
                //--
                var _ref = file.previewElement;
                _ref !== null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
            }
        },
        renameFile: function (file) {
            // Rename uploaded files
            var x = file.name;
            if (x.includes(",")) {
                x = x.replace(/,/g, "-");
            }
            let newName = x;
            // Add new name to the file object:
            file.newName = newName;
            // As an object is handed over by reference it will persist
            return newName;
        },
        error: function (file, response) {
            alert(response);
            //file.previewElement.classList.add("dz-error");
            this.removeFile(file);
            //--
            var _ref = file.previewElement;
            _ref !== null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
        },
        removedfile: function (file) {
            //Borra los archivos que el usuario desea borrar
            //--
            $.ajax({
                url: objWebRoot.route + "api/v1/anexos/borrar/archivo/temporal",
                type: "POST",
                data: JSON.stringify({ Folio: objUser.usersession, NombreArchivo: file.name }),
                datatype: "application/json",
                contentType: 'application/json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
                },
                success: function (result) {

                    if (result.trim().length > 0) {
                        //Obtiene el indice donde se encuentre el nombre del archivo a borrar
                        objarchivos.indice = $.inArray(result, objarchivos.listado);
                        //Borra el archivo del arreglo
                        if (objarchivos.indice > -1) {
                            objarchivos.listado.splice(objarchivos.indice, 1);
                        }
                        //Actualiza los valores del input
                        objarchivos.cadena = "";
                        $.each(objarchivos.listado, function (index, value) {
                            objarchivos.cadena = objarchivos.cadena + value;

                            //if ((index + 1) < (objarchivos.listado.length)) {
                            if (index + 1 < objarchivos.listado.length) {
                                objarchivos.cadena = objarchivos.cadena + ",";
                            }
                        });

                        $("#Reenviar_Anexos").val(objarchivos.cadena);
                        

                        var _ref = file.previewElement;
                        //return (_ref !== null) ? _ref.parentNode.removeChild(file.previewElement) : void 0;
                        return _ref !== null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
                    }
                }
            });
            //--
        },
        headers: { "Authorization": 'Bearer ' + objWebRoot.token }
    };
    $("#uploader").dropzone(optionsDropzone);
}
/****************************************************************************************************************************************/
function CargaArchivosSoloLectura() {
    objListadoArchivos.uri = objWebRoot.route + "api/v1/anexos/descarga/envios";
    objListadoArchivos.paramFolio = objFolio.folio;
    objListadoArchivos.datos = Object.keys(new Object());
    //--
    $.ajax({
        url: objListadoArchivos.uri,
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
                    $('#containerArchivos').append($('<span class="tm-tag cm-tag">' + '<a class="cmt-titulo" href="' + objWebRoot.route + item.Ruta + '" download>' + item.Nombre + '</a></span>'));
                });
            }
        }
    });
    //--
}

/****************************************************************************************************************************************/
//-----------------------------------------
var objRequiereRespuesta = { valor: false };
//Se agrega el método de evaluación condición
$.validator.addMethod('condicionbolean',
    function (value, element, params) {
        //Valor del campo en el que se depende
        objRequiereRespuesta.valor = ($('#Reenviar_' + params[0] + ' option:selected').val() == 'true');

        if (objRequiereRespuesta.valor) {
            //Valida para volver requeridos estos campos
            if ($.trim(value).length === 0 || value === 0) {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            return true;
        }
    });

$.validator.unobtrusive.adapters.add('condicionbolean',
    ['dependencia'],
    function (options) {
        options.rules['condicionbolean'] = [options.params['dependencia']];
        options.messages['condicionbolean'] = options.message;
    });
//-----------------------------------------