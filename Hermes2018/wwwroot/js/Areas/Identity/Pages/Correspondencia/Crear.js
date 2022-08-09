
/**
 * Archivo JS para eventos y acciones en vista de creación de nueva correspondencia del sistema Hermes
 * Últimos cambios el día 23/05/2019
 **/
var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usersession: "" };
var objTipoSubmit = { guardar: 1, enviar: 2 };
var objresumen = { de: "", para: "", ccp: "", asunto: "" };

var objPlantilla = { nombre: "", cuerpo: "" };
var objExtensiones = { valores: "" }; //image/*,application/pdf,text/plain,.doc,.docx,.xlsx,.xls,.ppt
var objFolio = { folio: "" };
var objTipoDocumento = { oficio: 1, memorandum: 2, seleccionado: 0 };

var tagifyEmails;
var tagifyEmailsCPP;
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
    //--
    objUser.usersession = $('#CrearDocumento_FolioSession').val();
    objUser.area = $("#user-area-id").text();
    objExtensiones.valores = $("#CrearDocumento_Extensiones").val();
    objFolio.folio = $("#folioInput").val();
    //--
    $("#checkRevision").val = false;
    //--
    objCalendario.dias = $('#Inhabiles_Dias').val();
    console.log(objCalendario.dias);
    objCalendario.datos = objCalendario.dias.split(',');
    objCalendario.inicio = $('#Inhabiles_Inicio').val();
    objCalendario.limite = $('#Inhabiles_Limite').val();
    //--
    objEditor.froala = new FroalaEditor('#CrearDocumento_Cuerpo', {
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
        tableEditButtons: [
            'tableHeader',
            'tableRemove',
            ' | ',
            'tableRows',
            'tableColumns',

            ' | ',
            'tableCells',
            'tableCellVerticalAlign',
            'tableCellHorizontalAlign',
        ],
        colorsText: [
            '#000000', '#2E64FE', '#088A29', '#FF0000', '#585858', '#FFFF00', '#8A084B'
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
        placeholderText: 'Escriba su documento',
        wordPasteModal: true,
        pastePlain: true,
        wordDeniedAttrs: ['style', 'width', 'height'],
        quickInsertTags: [''],
        //videoUpload: false,
        //imageUpload: true,
        imageMaxSize: 1024 * 1024 * 3,
        fontSizeSelection: true,
        fontSize: ['8', '10', '12', '14', '16', '18'],
        fontSizeDefaultSelection: '16',
        listAdvancedTypes: false,
        imageUploadURL: objWebRoot.route + "api/v1/CargaImagenes/" + objUser.username + "/" + objFolio.folio ,
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
    //--
    $('form').each(function () {
        if ($(this).data('validator'))
            $(this).data('validator').settings.ignore = ".fr-box *";
    });
    //--
    ControlAutocompletarPara();
    ControlAutocompletarConCopia();

    GetGrupos();
    GetServicios();
    GetCategorias();

    ControlDropzone();
    ControlTags();
    ControlTemplate();
    ControlValidacion();
    ControlCategoria();
    ControlAlerta();
    ControSubmit();
    ControlToggleEncabezado();
    ControlTipoDocumento();

    $("#checkRevision").click(function () {
        ControlRevisionCheck();
    });

    $("#serviciosSelect").change(function () {
        AplicarServicio();
    });

    $("#grupoSelect").change(function () {
        AplicarGrupo();
    });

    $("#plantillaSelect").change(function () {
        AplicarPlantilla();
    });

    $("#CrearDocumento_RequiereRespuesta").change(function () {
        if ($("#CrearDocumento_RequiereRespuesta option:selected").val() == 'false') {
            $('#fechaCompromiso').val('');
            $('#contieneFechapc').hide();
        } else {
            $('#contieneFechapc').show();
        }
    });

    //--fecha de Hoy
    
    //---
    objCalendario.datePicker = flatpickr('#fechaCompromiso', {
        dateFormat: "d/m/Y",
        minDate: objCalendario.inicio,
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
    
});
$.validator.setDefaults({ ignore: [] });
Dropzone.autoDiscover = false;

function ControlTipoDocumento() {

    objTipoDocumento.seleccionado = parseInt($('#CrearDocumento_TipoId option:selected').val());

    if (objTipoDocumento.seleccionado === objTipoDocumento.memorandum) {
        $("#leyenda-tipo-documento").text("Memorándum");
    } else {
        $("#leyenda-tipo-documento").text("");
    }
    //---
    $("#CrearDocumento_TipoId").change(function () {
        objTipoDocumento.seleccionado = parseInt($('#CrearDocumento_TipoId option:selected').val());

        if (objTipoDocumento.seleccionado === objTipoDocumento.memorandum) {
            $("#leyenda-tipo-documento").text("Memorándum");
        } else {
            $("#leyenda-tipo-documento").text("");
        }
    });
}

/****************************************************************************************************************************************
*                                                   Start script revision checkbox                                                       *
*****************************************************************************************************************************************/

function ControlRevisionCheck() {
    var check = $("#checkRevision");
    var gruposElement = document.getElementById("grupoSelect");
    var serviciosElement = document.getElementById('serviciosSelect');
    var inputCCP = $("#tagsEmailCCP");
    
    if (check.prop("checked")) {
        gruposElement.disabled = true;
        serviciosElement.disabled = true;
        inputCCP.hide();
        tagifyEmails.settings.maxTags = 1;
    } else {
        gruposElement.disabled = false;
        serviciosElement.disabled = false;
        inputCCP.show();
        tagifyEmails.settings.maxTags = Infinity;
    }
}
/****************************************************************************************************************************************
*                                                   Start scripts for autocomplete input                                                  *
*****************************************************************************************************************************************/
var objp = { urln: "" };
var dominioCorreo = "@uv.mx";
function getRoute(inputValue) {
    return objWebRoot.route + "api/v1/users/local/" + inputValue;
}
function getRouteRevision(inputValue) {
    return objWebRoot.route + "api/v1/users/revision/" + objUser.username + "/" + objUser.area + "/" + inputValue;
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
            details += "<li>" + checkUndefined(item.nombre) + " - <i>" + item.userName + dominioCorreo +"</i></li>";
        });
        details += "</ul>";
    } else {
        details = data.userName + dominioCorreo;
    }
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
    $("#CrearDocumento_Para").val(getEmailsParaToSend());//Set up the new For's values to asp helper
    $("#CrearDocumento_CCP").val(getEmailsCCPToSend());//Set up the new CCP's values to asp helper  
}
/****************************************************************************************************************************************
*                                                   Script autocomplete input To/Para                                                   *
*****************************************************************************************************************************************/
function ControlAutocompletarPara()
{
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
    function uniqueTagsIncludingGroup(miembros, data) {
        miembros.forEach(function (miembroItem) {
            for (var j = 0; j < data.length; j++) {
                if (data[j].userName === miembroItem.userName) {
                    tagifyEmails.removeTag(data[j].value); //remove by name, after match by email  
                }
            }
        });
    }
    function uniqueTagsIncludingGroupCCP(miembros, data) {
        if (data) {
            var arrayData = JSON.parse(data);
            arrayData.forEach(function (element) {
                miembros.forEach(function (miembro) {
                    if (miembro.userName === element.userName) {
                        tagifyEmailsCCP.removeTag(element.value); //remove by name, after match by email
                    }
                }); 
            });
        }
    }
    function avoidEmailImplicitInGroup(data, dataPersona) {
        data.forEach(function (elementTag) {
            if (elementTag.miembros) {
                var miembros = elementTag.miembros;
                for (j = 0; j < miembros.length; j++) {
                    if (miembros[j].userName === dataPersona.userName) {
                        tagifyEmails.removeTag(dataPersona.value);
                        tagifyEmailsCCP.removeTag(dataPersona.value);
                        notifyFacadeFor("<strong>Duplicado</strong><br>", "Destinatario incluido en grupo o servicio", 3000);
                        break;
                    }
                }
            }
        });
    }
    function uniqueTagsIncludingCCP(data, dataPersona) {
        if (data) {
            var arrayData = JSON.parse(data);
            arrayData.forEach(function (elementTag) {
                if (dataPersona.userName === elementTag.userName) {
                    tagifyEmailsCCP.removeTag(elementTag.value);
                }
            });
        }  
    }
    function checkOnInput(e) {
        //--
        var valueInput = e.detail;
        if (valueInput.length > 2) {
            var check = $("#checkRevision");
            var route;
            var listEmails = [];

            tagifyEmails.settings.whitelist.length = 0; // reset the whitelist
            controller && controller.abort();
            controller = new AbortController();

            if (check.prop("checked")) { //Check element for revisión
                route = getRouteRevision(valueInput);
            } else {
                route = getRoute(valueInput);
            }
            //--------------
            fetch(route, {
                //signal: controller.signal,
                headers: {
                    'Authorization': "Bearer " + objWebRoot.token
                }
            })
            .then(RES => RES.json())
                .then(function (whitelist) {
                    //console.log(whitelist);
                //Remove current user
                if (whitelist.findIndex(i => i.HER_UserName === objUser.username) > -1) {
                    whitelist.splice(whitelist.findIndex(i => i.HER_UserName === objUser.username), 1);
                }
                listEmails = filterProperties(whitelist);
                    //console.log(listEmails);
                tagifyEmails.settings.whitelist = listEmails;
                tagifyEmails.dropdown.show.call(tagifyEmails, valueInput); // render the suggestions dropdown
            })
            .catch(error => {
                //console.log("Petición cancelada") 
                //Peticiones no esperadas y canceladas pueden generar problemas
            });
            //--------------
        }
        //--
    }
    function checkAddTag(input) {
        var data = JSON.parse($('#form-autocomplete-para').val());
        //console.log(data);
        if (input.detail.data.miembros) {
            var miembros = input.detail.data.miembros;
            uniqueTagsIncludingGroup(miembros, data);
            data = $('#form-autocomplete-ccp').val();
            uniqueTagsIncludingGroupCCP(miembros, data);
        } else {
            var dataPersona = input.detail.data;
            avoidEmailImplicitInGroup(data, dataPersona);
            data = $('#form-autocomplete-ccp').val();
            uniqueTagsIncludingCCP(data, dataPersona);
        } 
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
                    <span style="font-size: small;font-style: italic;"> - ${tagData.puesto} - </span>
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

    document.querySelector('#checkRevision')
        .addEventListener('click', tagifyEmails.removeAllTags.bind(tagifyEmails));
    
}
/****************************************************************************************************************************************
*                                                   Script autocomplete input CCP                                                   *
*****************************************************************************************************************************************/
function ControlAutocompletarConCopia()
{
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
                if (dataPersona.userName === elementTag.userName) {
                    tagifyEmailsCCP.removeTag(dataPersona.value);
                    $('#labelPara').removeClass().addClass("shake" + ' animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
                        $(this).removeClass();
                    });
                }
            });
        }
    }
    function avoidEmailImplicitInGroup(data, dataPersona) {
        if (data) {
            var arrayData = JSON.parse(data);
            arrayData.forEach(function (elementTag) {
                if (elementTag.miembros) {
                    var miembros = elementTag.miembros;
                    for (j = 0; j < miembros.length; j++) {
                        if (miembros[j].userName === dataPersona.userName) {
                            tagifyEmailsCCP.removeTag(dataPersona.value);
                            notifyFacadeCCP("<strong>Duplicado</strong><br>", "Destinatario incluido en grupo o servicio", 3000);
                            break;
                        }
                    }
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

            //--------------
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
            //---------
        }
    }
    function checkAddTagCCP(input) {
        var dataRaw = $('#form-autocomplete-para').val();
        var dataPersona = input.detail.data;
        avoidEmailImplicitInGroup(dataRaw, dataPersona);
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
                </div>`;
            }
        }
    });
    var controllerCCP;
    tagifyEmailsCCP.on('input', checkOnInputCCP);
    tagifyEmailsCCP.on('add', checkAddTagCCP);
    tagifyEmailsCCP.on('remove', checkRemoveTagCCP);
    tagifyEmailsCCP.on('click', showDetailsCCP);
    document.querySelector('#checkRevision')
        .addEventListener('click', tagifyEmailsCCP.removeAllTags.bind(tagifyEmailsCCP)); 
}
/****************************************************************************************************************************************
*                                                   Start scripts Grupos                                                                *
*****************************************************************************************************************************************/
function GetGrupos() {
    var objGrupos = { urln: "" };
    objGrupos.urln = objWebRoot.route + "api/v1/grupos/" + objUser.username;
    var selectGrupo = document.getElementById("grupoSelect");

    //--
    $.ajax({
        url: objGrupos.urln,
        type: "GET",
        datatype: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            $.each(result, function (i, item) {
                selectGrupo.options[selectGrupo.options.length] = new Option(item.HER_Nombre, item.HER_GrupoId);
            });
        }
    });
}
function getMembersGroup(idGroup) {
    var arrayResultsMembers = [];
    var obj;
    var objGrupoIntegrantes = { urln: "" };   
    objGrupoIntegrantes.urln = objWebRoot.route + "api/v1/grupos/usuarios/" + idGroup;

    //--
    $.ajax({
        url: objGrupoIntegrantes.urln,
        type: "GET",
        datatype: "application/json",
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            $.each(result, function (i, item) {
                obj = {
                    userName: item.HER_UserName,
                    nombre: item.HER_NombreCompleto
                };
                arrayResultsMembers.push(obj);
            });
        }
    });
    //--
    
    return arrayResultsMembers;
}
function AplicarGrupo() {
    var selectGrupo = document.getElementById("grupoSelect");
    var grupo = selectGrupo.options[selectGrupo.selectedIndex];
    var arrayMembers = getMembersGroup(grupo.value);
    var obj = [
        {
            miembros: arrayMembers,
            value: grupo.text   
        }
    ];
    tagifyEmails.settings.whitelist = obj;
    tagifyEmails.addTags(obj);
    $("#grupoSelect").val($("#grupoSelect").data("default-value"));
}
function CambiarFlecha() {
    if ($('#flecha1').attr('class') === 'fas fa-angle-down') {
        $('#flecha1').attr('class', 'fas fa-angle-up');
    } else {
        $('#flecha1').attr('class', 'fas fa-angle-down');
    }
}
/****************************************************************************************************************************************
*                                                   Start scripts Servicios                                                             *
*****************************************************************************************************************************************/
var objServicios = { urln: "", datos: Object.keys(new Object()) };
function GetServicios() {
    var selectServicio = document.getElementById("serviciosSelect");
    objServicios.urln = objWebRoot.route + "api/v1/servicios";
    //--
    $.ajax({
        url: objServicios.urln,
        type: "GET",
        datatype: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            objServicios.datos = result;
            $.each(result, function (i, item) {
                var array = [item.HER_Integrantes[0].HER_UserName, item.HER_Integrantes[0].HER_NombreCompleto]
                selectServicio.options[selectServicio.options.length] = new Option(item.HER_Servicio_Nombre, array);
            });
        }
    });
    //--
}
function AplicarServicio() {
    var serviciosSelect = document.getElementById("serviciosSelect");
    var servicio = serviciosSelect.options[serviciosSelect.selectedIndex];
    var arrayValue = servicio.value.split(',');
    var obj = [
        {
            miembros: [{ userName: arrayValue[0], nombre: arrayValue[1] }],
            value: servicio.text
        }
    ];
    tagifyEmails.settings.whitelist = obj;
    tagifyEmails.addTags(obj);
    $("#serviciosSelect").val($("#serviciosSelect").data("default-value"));
}
/****************************************************************************************************************************************
*                                                   Start Dropzone Script                                                               *
*****************************************************************************************************************************************/
var objarchivos = { listado: new Array(), cadena: "", indice: 0 };
function ControlDropzone() {
    var optionsDropzone = {
        url: objWebRoot.route + "api/v1/anexos/subir/archivo/temporal/" + objUser.usersession,
        acceptedFiles: objExtensiones.valores,
        dictDefaultMessage: "Arrastre sus archivos aquí o haga clic para abrir el explorador",
        dictFallbackMessage : "Su navegador no admite la carga de archivos por medio de arrastrar y soltar.",
        dictFallbackText : "Utilice el formulario de reserva para cargar sus archivos como en los viejos tiempos.",
        dictFileTooBig : "El archivo es demasiado grande ({{filesize}}MiB). Tamaño máximo de archivo: {{maxFilesize}}MiB.",
        dictInvalidFileType : "No puedes subir archivos de este tipo.",
        dictResponseError : "El servidor respondió con el código {{statusCode}}.",
        dictCancelUpload : "Cancelar carga",
        dictCancelUploadConfirmation : "¿Estás seguro de que deseas cancelar esta carga?",
        dictRemoveFile : "Remover archivo",
        dictMaxFilesExceeded : "No puedes subir más archivos.",
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
                    $("#CrearDocumento_Anexos").val(objarchivos.cadena);
                    //console.log("Agregado: " + $("#CrearDocumento_Anexos").val());
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

                        $("#CrearDocumento_Anexos").val(objarchivos.cadena);

                        var _ref = file.previewElement;
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
function ControlTags() {
    $('#buttonTag').click(function () {
        AddTag();
    });
}
function AddTag() {
    $('#tagsSaver').append(
        '<div id="checkboxAdd"><small><div class=""><input class="form-control form-control-sm font-regular-10" id="nameTag" placeholder="Nombre categoría" required maxlength="100"/></div><div class="mb-2 text-center"><button id="agregarCategoria" type="button" class="btn btn-primary btn-sm pr-2 pl-2 mr-1"><i class="fa fa-check"></i></button><button class="btn btn-light btn-sm pr-2 pl-2" type="button" onclick="CancelarNuevaCategoria()"><i class="fas fa-times"></i></button></div></small></div>'
    );
    $('#buttonTag').prop("disabled", true);
}

var objCategorias = { urln: "", seleccionadas: new Array() };
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
            $('#CrearDocumento_Categorias').val(objCategorias.seleccionadas.join(','));
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
        $('#CrearDocumento_Categorias').val(objCategorias.seleccionadas.join(','));
		//
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

var objrefCategoria = { inputCheckText: "" };
function ControlCategoria() {
    $(document).on('click', "#agregarCategoria", function () {
        objrefCategoria.inputCheckText = $("#nameTag").val();
        if (objrefCategoria.inputCheckText !== "") {
            AgregarCategoria();
        }
    });
}
/****************************************************************************************************************************************
*                                                   Start Template scripts                                                              *
*****************************************************************************************************************************************/
function AddTemplate() {
	$("#msg-validate-template").empty();
    $('#ButtonsOficio').append('<div id="TemplateForm" class="templateForm"><div><input type="text" id="nameTemplate" class="form-control form-control-sm text-center font-regular-10" placeholder="Nombre de plantilla" required maxlength="100" autocomplete="off" value=""></div><div><small id="msg-validate-textarea" class="text-danger"></small></div><div><button type="button" id="BtnSaveTemplate" class="btn btn-primary btn-sm pr-2 pl-2" type="button"><i class="fa fa-check"></i></button> <button type="button" id="disposeTemplate" class="btn btn-light btn-sm pr-2 pl-2" type="button"><i class="fas fa-times"></i></button></div></div>');
	$('#SaveAsTemplate').prop("disabled", true);
}
function SaveTemplate() {
    var objsavetemplate = { cuerpo: "", nombre: "" };

    objsavetemplate.cuerpo = $('#CrearDocumento_Cuerpo').val();
	objsavetemplate.nombre = $('#nameTemplate').val();

	var objp1 = { urln: "" };
    objp1.urln = objWebRoot.route + "api/v1/plantillas/" + objUser.username;

    //--
    $.ajax({
        url: objp1.urln,
        type: "POST",
        data: JSON.stringify({ HER_Texto: objsavetemplate.cuerpo, HER_Nombre: objsavetemplate.nombre }),
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
function AplicarPlantilla() {
    var objplan2 = { urln: "", nombre: "" };

    objplan2.nombre = $('#plantillaSelect option:selected').val();

    if (objplan2.nombre !== "")
    {
        objplan2.urln = objWebRoot.route + "api/v1/plantillas/" + objplan2.nombre + ":" + objUser.username;
        //--
        $.ajax({
            url: objplan2.urln,
            type: "GET",
            datatype: "application/json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            },
            success: function (result) {
                //$("#CrearDocumento_Cuerpo").froalaEditor('html.set', result.HER_Texto);
                (objEditor.froala).html.set(result.HER_Texto);
            }
        });
        //--
    }
}
function GetTemplates() {
	$("#plantillaSelect").empty();
	$("#plantillaSelect").append("<option selected>Seleccione una plantilla </option>");

	var objplan3 = { urln: "" };
    objplan3.urln = objWebRoot.route + "api/v1/plantillas/" + objUser.username;

    //--
    $.ajax({
        url: objplan3.urln,
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
function DisposeTemplate() {
	$('#TemplateForm').empty();
	$('#TemplateForm').remove();
	$('#SaveAsTemplate').prop("disabled", false);
	flagTemplateValidation = false;
}
function ControlTemplate() {
    $(document).on('click', "#BtnSaveTemplate", function ()
    {
        objPlantilla.nombre = $('#nameTemplate').val();
        objPlantilla.cuerpo = $("#CrearDocumento_Cuerpo").val();
        
        if (objPlantilla.nombre !== "")
        {
            if (objPlantilla.cuerpo !== "")
                SaveTemplate();
            else 
                $("#msg-validate-textarea").text("El dato cuerpo es requerido.");

        } else {
            $("#msg-validate-textarea").text("El dato Nombre de plantilla es requerido.");
        }
    });

    $('#SaveAsTemplate').click(function () {
        AddTemplate();
    });

    $(document).on('click', "#disposeTemplate", function () {
        $("#msg-validate-textarea").empty();
        DisposeTemplate();
    });
}
/****************************************************************************************************************************************
*                            Mensaje de alerta en caso que el usuario salga sin guardar                                                 *
****************************************************************************************************************************************/
function ControlAlerta()
{
    var btn_clicked = false;
    document.querySelector('#send-new-ofi').addEventListener("click", function () { // put a class="send" in submit button
        btn_clicked = true; // if it is click onbeforeunload will not add dialog anymore
    });

    document.querySelector('#savetest').addEventListener("click", function () {
        btn_clicked = true;
    });
    window.onbeforeunload = function () {
        if (btn_clicked) {
            return undefined;
        } else {
            return "Are you sure to leave this page?";
        }
    };
}
/****************************************************************************************************************************************
 *    Validación para los diferentes tipos de submit
 ****************************************************************************************************************************************/
var objSubmitActual = { valor: "" };
function ControlValidacion() {
    $.validator.addMethod('simple',
        function (value, element, params) {
            //Valor del campo en el que se depende
            objSubmitActual.valor = $('#CrearDocumento_' + params[0]).val();

            if (objSubmitActual.valor === params[1]) {
                //Valida para volver requeridos estos campos
                if ($.trim(value).length === 0) {
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

    $.validator.unobtrusive.adapters.add('simple',
        ['dependencia', 'valor'],
        function (options) {
            options.rules['simple'] = [options.params['dependencia'], options.params['valor']];
            options.messages['simple'] = options.message;
        });
}
/****************************************************************************************************************************************
*                                                               Submit                                                                  *
****************************************************************************************************************************************/
function ControSubmit() {
    $("#send-new-ofi").click(function (e) {
        //Tipo de submit
        $('#CrearDocumento_TipoSubmit').val(objTipoSubmit.enviar);
    });

    $("#savetest").click(function () {
        //Tipo de submit
        $('#CrearDocumento_TipoSubmit').val(objTipoSubmit.guardar);
    });
}
function ControlToggleEncabezado(){
    $('#btn-control-encabezado').click(function () {

        $('#control-toggle-encab').toggle(190, "linear", function ()
        {
            if (!$('#control-toggle-encab').is(":visible"))
            {
                $('#btn-control-encabezado').empty();
                $('#btn-control-encabezado').append($('<i class="fas fa-angle-double-down"></i>'));

                objresumen.de = $('#labelfrom').text();
                objresumen.asunto = $('#asunto').val();
                objresumen.para = '';
                objresumen.ccp = '';
                //---
                $('#tagsEmailPara label.label-fill tags tag').each(function ()
                {
                    if (objresumen.para.length > 0)
                    {
                        objresumen.para = objresumen.para + ', ' + $(this).attr('value');
                    }
                    else
                    {
                        objresumen.para = $(this).attr('value');
                    }
                });

                $('#tagsEmailCCP label.label-fill tags tag').each(function ()
                {
                    if (objresumen.ccp.length > 0)
                    {
                        objresumen.ccp = objresumen.ccp + ', ' + $(this).attr('value');
                    }
                    else {
                        objresumen.ccp = $(this).attr('value');
                    }
                });
                $('#resumen').removeClass('d-none');
                $('#resumen').removeClass('d-inline-block');
                $('#resumen').addClass('d-inline-block');

                $('#resumen .resumen-de').text(objresumen.de);
                $('#resumen .resumen-para').text(objresumen.para);
                $('#resumen .resumen-CCP').text(objresumen.ccp);
                $('#resumen .resumen-asunto').text(objresumen.asunto);
            }
            else
            {
                $('#btn-control-encabezado').empty();
                $('#btn-control-encabezado').append($('<i class="fas fa-angle-double-up"></i>'));

                $('#resumen').removeClass('d-none');
                $('#resumen').addClass('d-none');
                $('#resumen').removeClass('d-inline-block');
            }
        });
    });
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

/***************************************************************************************************************************************/

//-----------------------------------------
var objRequiereRespuesta = { valor: false };
//Se agrega el método de evaluación condición
$.validator.addMethod('condicionbolean',
    function (value, element, params) {
        //Valor del campo en el que se depende
        objRequiereRespuesta.valor = ($('#CrearDocumento_' + params[0] +' option:selected').val() == 'true');

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