var objUser = { username: "", usersession: "" };
var objWebRoot = { route: "", token: "" };
var objDelegar = { estadoDelegar: false };
//--------------------------------------------------------------------------------------------------------------------------------------
var objVisualizaRevision = { remitente: 1, destinatario: 2, actual: 0 };
var objEstadoRemitente = { estado1: 1, estado2: 2, actual: 0 };
var objEstadoDestinatario = {estado1: 3, estado2: 4, actual: 0 };

var objExtensiones = { valores: "" }; //"image/*,application/pdf,text/plain,.doc,.docx,.xlsx,.xls,.ppt"
var objFolio = { folio: '' };
var objTipoDocumento = { oficio: 1, memorandum: 2, seleccionado: 0 };
var objEditor = { froala: null };
/****************************************************************************************************************************************
*                                                            Función: Main                                                              *
*****************************************************************************************************************************************/
$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();

    objUser.usersession = $('#RevisarDocumento_FolioSession').val();
    objExtensiones.valores = $("#RevisarDocumento_Extensiones").val();
    objFolio.folio = $("#folioInput").val();
    //--
    objVisualizaRevision.actual = parseInt($('#RevisarDocumento_TipoVisualizacion').val());
    objEstadoRemitente.actual = parseInt($('#RevisarDocumento_EstadoRemitente').val());
    objEstadoDestinatario.actual = parseInt($('#RevisarDocumento_EstadoDestinatario').val());
    //--
    objDelegar.estadoDelegar = ($("#EstadoDelegar").val() === 'True');
    //--
    if (objVisualizaRevision.actual === objVisualizaRevision.remitente)
    {
        if (objEstadoRemitente.actual === objEstadoRemitente.estado1)
        {
            LecturaCategorias();
            LecturaArchivos();
            LecturaTipoDocumento();
        }
        else if (objEstadoRemitente.actual === objEstadoRemitente.estado2)
        {
            //--
            $('#contiene-cuerpo').removeClass('d-none');

            if (objDelegar.estadoDelegar)
            {
                //--
                objEditor.froala = new FroalaEditor('#RevisarDocumento_Cuerpo', {
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
                //--
                GetCategorias();
                ControlArchivos();
                CargaArchivos();
                ControlPlantilla();
                ControlTipoDocumento();
            }
            else {
                LecturaCategorias();
                LecturaArchivos();
                LecturaTipoDocumento();
            }
        }
    }
    else if (objVisualizaRevision.actual === objVisualizaRevision.destinatario)
    {
        $('#contiene-cuerpo').removeClass('d-none');

        if (objDelegar.estadoDelegar)
        {
            objEditor.froala = new FroalaEditor('#RevisarDocumento_Cuerpo', {
                key: "UBB7jE6D5G3I3A2A6aIVLEABVAYFKc1Ce1MYGD1c1NYVMiB3B9B6B5C2B4C3H3I3I3==",
                toolbarButtons: [
                    'fullscreen',
                    'bold',
                    'italic',
                    'textColor',
                    'underline',
                    'strikeThrough',
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
                    'insertHR',
                    'clearFormatting',
                    'selectAll',
                    '|',
                    'undo',
                    'redo'
                ],
                colorsText: [
                    '#000000', '#2E64FE', '#088A29', '#FF0000', '#585858', '#FFFF00', '#8A084B'
                ],
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
                videoUpload: false,
                imageUpload: true,
                imageMaxSize: 1024 * 1024 * 3,
                fontSizeSelection: true,
                fontSize: ['8', '10', '12', '14', '18'],
                fontSizeDefaultSelection: '16',
                listAdvancedTypes: false,
                imageUploadURL: objWebRoot.route + "api/v1/CargaImagenes/" + objUser.username + "/" + objFolio.folio,
                requestHeaders: {
                    Authorization: "Bearer " + objWebRoot.token
                }
                /*imageInsertButtons: ['imageBack', '|', 'imageByURL', 'imageManager']*/
            });
            //--
            GetCategorias();
            ControlArchivos();
            CargaArchivos();
            ControlPlantilla();
            LecturaTipoDocumento();
        }
        else {
            LecturaCategorias();
            LecturaArchivos();
            LecturaTipoDocumento();
        }
    }
});

function ControlTipoDocumento()
{
    objTipoDocumento.seleccionado = parseInt($('#RevisarDocumento_TipoId option:selected').val());

    if (objTipoDocumento.seleccionado === objTipoDocumento.memorandum)
        $("#leyenda-tipo-documento").text("Memorándum");
    else
        $("#leyenda-tipo-documento").text("");

    //---
    $("#RevisarDocumento_TipoId").change(function ()
    {
        objTipoDocumento.seleccionado = parseInt($('#RevisarDocumento_TipoId option:selected').val());

        if (objTipoDocumento.seleccionado === objTipoDocumento.memorandum)
            $("#leyenda-tipo-documento").text("Memorándum");
        else
            $("#leyenda-tipo-documento").text("");

    });
}

function LecturaTipoDocumento()
{
    objTipoDocumento.seleccionado = parseInt($('#RevisarDocumento_TipoId').val());

    if (objTipoDocumento.seleccionado === objTipoDocumento.memorandum)
        $("#leyenda-tipo-documento").text("Memorándum");
    else
        $("#leyenda-tipo-documento").text("");
}

/****************************************************************************************************************************************
*                                                       Función: Plantillas                                                             *
*****************************************************************************************************************************************/
var objPlantilla = { urln: "", nombre: "" };

function ControlPlantilla() {

    $("#plantillaSelect").change(function () {
        objPlantilla.nombre = $('#plantillaSelect option:selected').val();
        objPlantilla.urln = objWebRoot.route + "api/v1/plantillas/" + objPlantilla.nombre + ":" + objUser.username;

        if (objPlantilla.nombre !== "") {
            //--
            $.ajax({
                url: objPlantilla.urln,
                type: "GET",
                datatype: "application/json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
                },
                success: function (result) {
                    (objEditor.froala).html.set(result.HER_Texto);
                }
            });
            //--
        }
    });
}

/****************************************************************************************************************************************
*                                                       Función: Categoria                                                              *
*****************************************************************************************************************************************/
var objCategorias = { urln: "", seleccionadas: new Array() };

function GetCategorias() {
    objCategorias.seleccionadas = new Array();
    objCategorias.urln = objWebRoot.route + "api/v1/categorias/seleccionadas";

    $.ajax({
        url: objCategorias.urln,
        type: "POST",
        data: JSON.stringify({ Usuario: objUser.username, Folio: objFolio.folio }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            $.each(result, function (i, categoria) {
                if (categoria.Estado) {
                    $('#tagsSaver').append('<input type="checkbox" class="categorias" value="' + categoria.CategoriaId + '" checked="true"/><small> ' + categoria.Nombre + '</small><br/>');
                    objCategorias.seleccionadas.push(parseInt(categoria.CategoriaId));
                }
                else {
                    $('#tagsSaver').append('<input type="checkbox" class="categorias" value="' + categoria.CategoriaId + '"/><small> ' + categoria.Nombre + '</small><br/>');
                }
            });
            //Poner las categorias en el input del formulario
            $('#RevisarDocumento_Categorias').val(objCategorias.seleccionadas.join(','));
            //--
            SeleccionCategoria();
        }
    });
}

function SeleccionCategoria() {
    $('#tagsSaver input[type=checkbox]').on('change', function () {

        objCategorias.seleccionadas = new Array();
        //--
        if ($('#tagsSaver input[type=checkbox]:checked').length === 0)
        {
            $('input#catGeneral').prop('checked', true);
        }
        //
        $('#tagsSaver input[type=checkbox]:checked').each(function () {
            objCategorias.seleccionadas.push(parseInt($(this).val()));
        });
        //
        $('#RevisarDocumento_Categorias').val(objCategorias.seleccionadas.join(','));
        //
    });
}

function LecturaCategorias() {
    objCategorias.seleccionadas = new Array();
    objCategorias.urln = objWebRoot.route + "api/v1/categorias/seleccionadas";

    //--
    $.ajax({
        url: objCategorias.urln,
        type: "POST",
        data: JSON.stringify({ Usuario: objUser.username, Folio: objFolio.folio }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            $.each(result, function (i, categoria) {
                if (categoria.Estado) {
                    $('#tagsSaver').append('<span class="ml-1 badge badge-light">' + categoria.Nombre + '</span>');
                    objCategorias.seleccionadas.push(parseInt(categoria.CategoriaId));
                }
            });
            //Poner las categorias en el input del formulario
            $('#RevisarDocumento_Categorias').val(objCategorias.seleccionadas.join(','));
        }
    });
    //--
}
/****************************************************************************************************************************************
*                                                   Start Dropzone Script                                                               *
*****************************************************************************************************************************************/
var objArchivosTemp = { listado: new Array(), cadena: "", indice: 0, urlnCargar: "", urlnEliminar: "" };
Dropzone.autoDiscover = false;

function ControlArchivos()
{
    objArchivosTemp.urlnCargar = objWebRoot.route + "api/v1/anexos/subir/archivo/temporal/" + objUser.usersession;
    objArchivosTemp.urlnEliminar = objWebRoot.route + "api/v1/anexos/borrar/archivo/temporal";

    var optionsDropzone = {
        url: objArchivosTemp.urlnCargar,
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
                if ($.inArray(response, objArchivosTemp.listado) === -1) {
                    //Se agrega el nombre del archivo a un arreglo
                    objArchivosTemp.listado.push(response);
                    //Se recorre el arrreglo de los archivos alamacenados
                    objArchivosTemp.cadena = "";
                    $.each(objArchivosTemp.listado, function (index, value) {
                        objArchivosTemp.cadena = objArchivosTemp.cadena + value;

                        if (index + 1 < objArchivosTemp.listado.length) {
                            objArchivosTemp.cadena = objArchivosTemp.cadena + ",";
                        }
                    });
                    //Agrega la cadena
                    $("#RevisarDocumento_Anexos").val(objArchivosTemp.cadena);
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
                url: objArchivosTemp.urlnEliminar,
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
                        objArchivosTemp.indice = $.inArray(result, objArchivosTemp.listado);
                        //Borra el archivo del arreglo
                        if (objArchivosTemp.indice > -1) {
                            objArchivosTemp.listado.splice(objArchivosTemp.indice, 1);
                        }
                        //Actualiza los valores del input
                        objArchivosTemp.cadena = "";
                        $.each(objArchivosTemp.listado, function (index, value) {
                            objArchivosTemp.cadena = objArchivosTemp.cadena + value;

                            if (index + 1 < objArchivosTemp.listado.length) {
                                objArchivosTemp.cadena = objArchivosTemp.cadena + ",";
                            }
                        });

                        $("#RevisarDocumento_Anexos").val(objArchivosTemp.cadena);

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
*                                                       Función: Carga de archivos existentes                                            *
*****************************************************************************************************************************************/
// Cargar archivos en TagsManager
var objListadoArchivos = { urln: "", datos: Object.keys(new Object()) };
var objEliminarArchivo = { urln: "", estado: false, bandera: null, seleccion: null };
//--
function CargaArchivos() {
    objListadoArchivos.urln = objWebRoot.route + "api/v1/anexos/descarga/archivos/documento";
    objListadoArchivos.datos = Object.keys(new Object());

    //--
    $.ajax({
        url: objListadoArchivos.urln,
        type: "POST",
        data: JSON.stringify({ Folio: objFolio.folio }),
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
                    $('#containerArchivos').append($('<span class="tm-tag cm-tag">' + '<a class="cmt-titulo" href="' + objWebRoot.route + item.Ruta + '" download>' + item.Nombre + '</a>' + '<span class="cmt-eliminar tm-tag-remove btn btn-sm pt-0 pr-0 pb-0">x</span>' + '</span>'));
                });
                //--
                $('.cm-tag').on("click", ".cmt-eliminar", function () {
                    objEliminarArchivo.seleccion = null;
                    objEliminarArchivo.seleccion = $(this);
                    eliminaAnexo(objEliminarArchivo.seleccion.siblings("a").text());
                    //--
                    $.when(objEliminarArchivo.bandera).done(function () {
                        if (objEliminarArchivo.estado) {
                            objEliminarArchivo.seleccion.parent().remove();
                        }
                        //--Re-Inicializa
                        objEliminarArchivo.bandera = null;
                    });
                });
            }
        }
    });
    //--
}
//--
function eliminaAnexo(file)
{
    objEliminarArchivo.urln = objWebRoot.route + "api/v1/anexos/borrar/archivo/documento";

    //--
    objEliminarArchivo.bandera = $.ajax({
        url: objEliminarArchivo.urln,
        type: "POST",
        data: JSON.stringify({ Folio: objFolio.folio, NombreArchivo: file }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            if (result.trim().length > 0) {
                objEliminarArchivo.estado = true;
            } else {
                objEliminarArchivo.estado = false;
            }
        },
        error: function () {
            objEliminarArchivo.estado = false;
            //console.log("No se han podido eliminar el anexo.");
        }
    });
    //--
}

function LecturaArchivos()
{
    objListadoArchivos.urln = objWebRoot.route + "api/v1/anexos/descarga/archivos/documento";

    //--
    $.ajax({
        url: objListadoArchivos.urln,
        type: "POST",
        data: JSON.stringify({ Folio: objFolio.folio }),
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