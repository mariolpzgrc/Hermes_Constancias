/**
 * Archivo JS para eventos y acciones en vista de respuesta a una correspondencia del sistema Hermes
 * Últimos cambios el día 02/05/2019
 **/
var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usersession: "" };
var objExtensiones = { valores: "" };  //image/*,application/pdf,text/plain,.doc,.docx,.xlsx,.xls,.ppt
var objFolio = { folio: "" };

var objTipoDocumento = { oficio: 1, memorandum: 2, seleccionado: 0 };
var objParam = { envioId: 0, Folio: '', tipoEnvio: 0, recepcionId: 0, existeAdjuntos: false };
var objListadoArchivos = { urln: "", paramFolio: "", datos: Object.keys(new Object()) };

var tagifyEmails;
var tagifyEmailsCCP;
var objEditor = { froala: null };
/****************************************************************************************************************************************
*                                                            Función: Main                                                              *
*****************************************************************************************************************************************/
$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    //--
    objParam.envioId = parseInt($('#envio_identifica').text());
    objParam.tipoEnvio = parseInt($('#tipo_envio_identifica').text());
    objParam.existeAdjuntos = ($('#existe_adjuntos').text().toLowerCase() === 'true') ? true : false;
    //--
    objUser.usersession = $('#Responder_FolioSession').val();
    objExtensiones.valores = $("#Responder_Extensiones").val();
    objFolio.folio = $("#folioInput").val();
    //--

    objEditor.froala = new FroalaEditor('#Responder_Cuerpo', {
        key: "UBB7jE6D5G3I3A2A6aIVLEABVAYFKc1Ce1MYGD1c1NYVMiB3B9B6B5C2B4C3H3I3I3==",
        toolbarButtons: [
            'fullscreen',
            'bold',
            'italic',
            'textColor',
            'fontSize',
            'underline',
            'subscript',
            'superscript',
            '|',
            'align',
            'formatOL',
            'formatUL',
            'outdent',
            'indent',
            '|',
            'insertLink',
            //'insertImage',
            //'insertVideo',
            'insertTable',
            '|',
            'clearFormatting',
            'selectAll',
            '|',
            'undo',
            'redo'
        ],
        colorsText: [
            '#000000', '#2E64FE', '#088A29', '#FF0000', '#585858', '#FFFF00', '#8A084B'
        ],
        attribution: false,
        language: 'es',
        //height: 400,
        heightMin: 300,
        heightMax: 400,
        theme: 'gray',
        //zIndex: 2001,
        placeholderText: 'Escriba su documento',
        wordPasteModal: true,
        pastePlain: true,
        wordDeniedAttrs: ['style', 'width', 'height'],
        quickInsertTags: [''],
        //videoUpload: false,
        //imageUpload: true,
        imageMaxSize: 1024 * 1024 * 3,
        fontSizeSelection: true,
        fontSize: ['8', '10', '12', '14', '18'],
        fontSizeDefaultSelection: '16',
        listAdvancedTypes: false,
        imageUploadURL: objWebRoot.route + "api/v1/CargaImagenes/" + objUser.username + "/" + objFolio.folio,
        requestHeaders: {
            Authorization: "Bearer " + objWebRoot.token
        },
        events: {
            'image.removed': function ($img) {
                $.ajax({
                    // Request method.
                    method: 'POST',
                    // Request URL.
                    url: objWebRoot.route + "api/v1/EliminaImagenes",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
                    },
                    // Request params.
                    data: {
                        src: $img.attr('src')
                    }
                })
                .done(function (data) {
                    console.log('Image was deleted');
                })
                .fail(function (err) {
                    console.log('Image delete problem: ' + JSON.stringify(err));
                });
            }
        }
    });

    $('form').each(function () {
        if ($(this).data('validator'))
            $(this).data('validator').settings.ignore = ".fr-box *";
    });
    //--
    GetCategorias();
    ControlCategorias();
    ControlDropzone();
    ControlAutocompletarCCP();
    ControlTemplate();
    RecuperarDatosAutoCompletarCCP();
    ControlTipoDocumento();

    $("#plantillaSelect").change(function () {
        AplicarPlantilla();
    });

    if (objParam.existeAdjuntos) {
        LecturaArchivosOrigen();
    }
});
$.validator.setDefaults({ ignore: [] });
Dropzone.autoDiscover = false;

function ControlTipoDocumento() {

    objTipoDocumento.seleccionado = parseInt($('#Responder_TipoId option:selected').val());

    if (objTipoDocumento.seleccionado === objTipoDocumento.memorandum) {
        $("#leyenda-tipo-documento").text("Memorándum");
    } else {
        $("#leyenda-tipo-documento").text("");
    }
    //---
    $("#Responder_TipoId").change(function () {
        objTipoDocumento.seleccionado = parseInt($('#Responder_TipoId option:selected').val());

        if (objTipoDocumento.seleccionado === objTipoDocumento.memorandum) {
            $("#leyenda-tipo-documento").text("Memorándum");
        } else {
            $("#leyenda-tipo-documento").text("");
        }
    });
}
/****************************************************************************************************************************************
*                                                            Dropzone                                                                   *
*****************************************************************************************************************************************/
var objarchivos = { listado: new Array(), cadena: "", indice: 0 };

function ControlDropzone() {
    
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
                    $("#Responder_Anexos").val(objarchivos.cadena);
                    //console.log("Agregado: " + $("#Responder_Anexos").val());
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

                        $("#Responder_Anexos").val(objarchivos.cadena);
                        //console.log("Borrado: " + $("#Responder_Anexos").val());

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

/****************************************************************************************************************************************
*                                                   Categorias Scripts                                                                  *
*****************************************************************************************************************************************/
var objCategorias = { urln: "" , seleccionadas: new Array() };
function GetCategorias() {
    objCategorias.urln = objWebRoot.route + "api/v1/categorias/" + objUser.username;

    //--
    $.ajax({
        url: objCategorias.urln,
        type: "GET",
        datatype: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            $.each(result, function (i, categoria) {
                if (categoria.Nombre === "General") {
                    $('#tagsSaver').append('<input type="checkbox" class="categorias" id="catGeneral" value="' + categoria.CategoriaId + '" checked="true" /><small> ' + categoria.Nombre + '</small><br/>');
                    objCategorias.seleccionadas.push(parseInt(categoria.CategoriaId));
                } else {
                    $('#tagsSaver').append('<input type="checkbox" class="categorias" value="' + categoria.CategoriaId + '"/><small> ' + categoria.Nombre + '</small><br/>');
                }
            });
            //Poner las categorias en el input del formulario
            $('#Responder_Categorias').val(objCategorias.seleccionadas.join(','));
            //--
            SeleccionCategoria();
        }
    });
    //--
}

function SeleccionCategoria() {
    $('#tagsSaver input[type=checkbox]').on('change', function () {
        //
        objCategorias.seleccionadas = new Array();
        //
        if ($('#tagsSaver input[type=checkbox]:checked').length === 0) {
            $('input#catGeneral').prop('checked', true);
        }
        //
        $('#tagsSaver input[type=checkbox]:checked').each(function () {
            objCategorias.seleccionadas.push(parseInt($(this).val()));
        });
        //
        $('#Responder_Categorias').val(objCategorias.seleccionadas.join(','));
        //
    });
}

function ControlCategorias() {
    $('#buttonTag').click(function () {
        NuevaCategoria();
    });

    $(document).on('click', "#agregarCategoria", function () {
        if ($("#nameTag").val() !== "") {
            AgregarCategoria();
        }
    });
}

var objc = { urln: "" };
function AgregarCategoria() {
    objc.urln = objWebRoot.route + "api/v1/categorias/agregar";

    //--
    $.ajax({
        url: objc.urln,
        type: "POST",
        data: JSON.stringify({ Usuario: objUser.username, Categoria: $('#nameTag').val() }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            if (result.Estado === 1) {
                $('#tagsSaver').append('<input type="checkbox" class="categorias" value="' + result.CategoriaId + '"/><small> ' + result.Nombre + '</small><br/>');
                $('#checkboxAdd').empty();
                $('#checkboxAdd').remove();
                $('#buttonTag').prop("disabled", false);

                SeleccionCategoria();
            } else if (result.Estado === 3) {
                Notifica("No se ha puede guardar la categoría, ya existe una con el mismo nombre.");
            } else if (result.Estado === 2 || result.Estado === 4) {
                Notifica("No se ha puede guardar la categoría, inténtelo más tarde.");
            }
        }
    });
    //--
}

function CancelarNuevaCategoria() {
    $('#checkboxAdd').empty();
    $('#checkboxAdd').remove();
    $('#buttonTag').prop("disabled", false);
}

function NuevaCategoria() {
    $('#tagsSaver').append(
        '<div id="checkboxAdd"><small><div class=""><input class="form-control form-control-sm font-regular-10" id="nameTag" placeholder="Nombre categoría" required maxlength="100"/></div><div class="mb-2 text-center"><button id="agregarCategoria" type="button" class="btn btn-primary btn-sm pr-2 pl-2 mr-1"><i class="fa fa-check"></i></button><button class="btn btn-light btn-sm pr-2 pl-2" type="button" onclick="CancelarNuevaCategoria()"><i class="fas fa-times"></i></button></div></small></div>'
    );
    $('#buttonTag').prop("disabled", true);
}

/****************************************************************************************************************************************
*                                                   Start autocomplete-tag Scripts                                                                  *
*****************************************************************************************************************************************/
var objp = { urln: "" };
var dominioCorreo = "@uv.mx";
function getRoute(inputValue) {
    console.log(inputValue);
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
    details = data.userName + dominioCorreo;
    return details;
}
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
    $("#Responder_CCP").val(getEmailsCCPToSend());//Set up the new CCP's values to asp helper  
}

/****************************************************************************************************************************************
*                                                   Start script autocomplete-tag CCP Recuperacion                                                                  *
*****************************************************************************************************************************************/
function RecuperarDatosAutoCompletarCCP() {
    var userNamesMail = $("#Responder_CCP").val();
    CargarUsuarioCCP(userNamesMail);
}

function CargarUsuarioCCP(usuarios) {
    var listEmails = [];
    var route = objWebRoot.route + "api/v1/users/local/coleccion";

    fetch(route, {
        method: 'POST',
        headers: {
            Authorization: "Bearer " + objWebRoot.token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ "usuarios": usuarios }),
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (rawWhiteList) {
            listEmails = filterProperties(rawWhiteList);
            CargarTagAutocompletarCCP(listEmails);
        })
        .catch(error => {
            console.log("Peticion cancelada" + error);
        });
}

function CargarTagAutocompletarCCP(listEmails) {
    tagifyEmailsCCP.settings.whitelist = listEmails;
    tagifyEmailsCCP.addTags(listEmails);
}

/****************************************************************************************************************************************
*                                                   Start script autocomplete-tag CCP                                                   *
*****************************************************************************************************************************************/

function ControlAutocompletarCCP() {

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
    function uniqueTagPara(dataPersona) {
        var dataPara = $('#labelfor').text().trim();
        if (dataPersona.value === dataPara) {
            tagifyEmailsCCP.removeTag(dataPersona.value);
            $('#labelfor').removeClass().addClass("shake" + ' animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
                $(this).removeClass();
            });
        }
    }
    function checkOnInputCCP(e) {
        console.log(e, e.detail);
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
                if (whitelist.findIndex(i => i.HER_UserName === $('#user').text()) > -1) {
                    whitelist.splice(whitelist.findIndex(i => i.HER_UserName === $('#user').text()), 1);
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
        console.log(input);
        var dataRaw = $('#form-autocomplete-para').val();
        var dataPersona = input.detail.data;
        uniqueTagPara(dataPersona);
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
        notifyFacadeCCP("Detalles del contacto", informacion, 10000);
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
*                                                   Start Template scripts                                                              *
*****************************************************************************************************************************************/
var objrefPlantilla = { inputTextNombre: null, inputTextAreaCuerpo: $("#Responder_Cuerpo") };

function ControlTemplate() {
    $('#SaveAsTemplate').click(function () {
        AddTemplate();
    });

    $(document).on('click', "#BtnSaveTemplate", function () {
        objrefPlantilla.inputTextNombre = $('#nameTemplate');
        //--
        if (objrefPlantilla.inputTextNombre.val() !== "") {
            if (objrefPlantilla.inputTextAreaCuerpo.val() !== "") //objrefPlantilla.inputTextSaludo.val() !== "" &&
            { 
                SaveTemplate();
            } else {
                $("#msg-validate-textarea").text("El dato cuerpo es requerido."); //Los campos saludo y cuerpo son requeridos.
            }
        } else {
            $("#msg-validate-textarea").text("El dato Nombre de plantilla es requerido.");
        }
    });

    $(document).on('click', "#disposeTemplate", function () {
        $("#msg-validate-textarea").empty();
        DisposeTemplate();
    });
}

function AddTemplate() {
    $("#msg-validate-template").empty();
    $('#ButtonsOficio').append('<div id="TemplateForm" class="templateForm"><div><small><input id="nameTemplate" class="form-control form-control-sm text-center font-regular-10 mt-1 mb-1"  placeholder="Nombre de plantilla" required maxlength="100" autocomplete="off"></small></div><div><small id="msg-validate-textarea" class="text-danger"></small></div><div><button id="BtnSaveTemplate" class="btn btn-primary btn-sm pr-2 pl-2" type="button"><i class="fa fa-check"></i></button> <button id="disposeTemplate" class="btn btn-light btn-sm pr-2 pl-2" type="button"><i class="fas fa-times"></i></button></div></div>');
    $('#SaveAsTemplate').prop("disabled", true);
}

function SaveTemplate() {
    var objSaveTemplate = { cuerpo: "", nombre: "", urln: "" }; //saludo: "",

    objSaveTemplate.cuerpo = $('#Responder_Cuerpo').val();
    objSaveTemplate.nombre = $('#nameTemplate').val();
    objSaveTemplate.urln = objWebRoot.route + "api/v1/plantillas/" + objUser.username;

    //--
    $.ajax({
        url: objSaveTemplate.urln,
        type: "POST",
        data: JSON.stringify({ HER_Texto: objSaveTemplate.cuerpo, HER_Nombre: objSaveTemplate.nombre }), //HER_Saludo: objSaveTemplate.saludo,
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (data === 0) {
                Notifica("No se ha puede guardar la plantilla, inténtelo más tarde.");
            } else if (data === 2) {
                Notifica("No se ha puede guardar la plantilla, ya existe una con el mismo nombre.");
            }
            GetTemplates();
        }
    });
    //--

    $('#TemplateForm').empty();
    $('#TemplateForm').remove();
    $('#SaveAsTemplate').prop("disabled", false);
    $("#msg-validate-template").text("Plantilla guardada");
    $("#msg-validate-template").addClass("text-success");
}

function DisposeTemplate() {
    $('#TemplateForm').empty();
    $('#TemplateForm').remove();
    $('#SaveAsTemplate').prop("disabled", false);
    flagTemplateValidation = false;
}

function AplicarPlantilla() {
    var objAplicarPlantilla = { urln: "", nombre: "" };

    objAplicarPlantilla.nombre = $('#plantillaSelect option:selected').val();

    if (objAplicarPlantilla.nombre !== "")
    {
        objAplicarPlantilla.urln = objWebRoot.route + "api/v1/plantillas/" + objAplicarPlantilla.nombre + ":" + objUser.username;

        //--
        $.ajax({
            url: objAplicarPlantilla.urln,
            type: "GET",
            datatype: "application/json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            },
            success: function (result) {
                //$("#Responder_Cuerpo").froalaEditor('html.set', result.HER_Texto);
                (objEditor.froala).html.set(result.HER_Texto);
            }
        });
        //--
    }
}

var objObtenerPlantillas = { urln: "" };
function GetTemplates() {
    objObtenerPlantillas.urln = objWebRoot.route + "api/v1/plantillas/" + objUser.username;

    $("#plantillaSelect").empty();
    $("#plantillaSelect").append("<option selected>Seleccione una plantilla </option>");

    //--
    $.ajax({
        url: objObtenerPlantillas.urln,
        type: "GET",
        datatype: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            $.each(result, function (i, template) {
                $('<option>', {
                    value: template.HER_Nombre
                }).html(template.HER_Nombre).appendTo($('#plantillaSelect'));
            });
        }
    });
    //--
}

function Notifica(mensaje) {
    $.notify({
        message: mensaje,
    }, {
            type: "info",
            newest_on_top: true,
            delay: 3000,
            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            }
        });
}
/****************************************************************************************************************************************/

function LecturaArchivosOrigen() {
    objListadoArchivos.urln = objWebRoot.route + "api/v1/anexos/descarga/enviosOrigen";
    objParam.Folio = $('#folio').text();
    objListadoArchivos.datos = Object.keys(new Object());
    //--
    $.ajax({
        url: objListadoArchivos.urln,
        type: "POST",
        data: JSON.stringify({ Folio: objParam.Folio, TipoEnvioId: objParam.tipoEnvio, EnvioId: objParam.envioId }),
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