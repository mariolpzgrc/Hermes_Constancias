var objp = { urln: "" };
var objd = { urln: "" };
var objc = { urln: "" };
var obja = { urln: "" };

var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usersession: "" };

var objParam = { envioId: 0, tipoVisualizacion: 0, tipoEnvio: 0, esTurnado: false, existeAdjuntos: false, tieneRespuesta: false, esReenvio: false };
var objParamOrigen = { envioId: 0, tipoEnvio: 0, existeAdjuntos: false };
var objDelegar = { estaActiva: false, tipoPermiso: 0 };

var objTipoVisualizacion = { recepcion: 1, envio: 2 };
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4 };
//--
var objDocumento = { recepcionId: 0 };
/****************************************************************************************************************************************
*                                                            Función: Main                                                              *
*****************************************************************************************************************************************/
$(function () {
    objUser.username = $("#user").text();
    objUser.usersession = $('#FolioSession').val();
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();

    //--Actual
    objParam.envioId = parseInt($('#envio_identifica').text());
    objParam.tipoVisualizacion = parseInt($('#tipo_visualizacion').text());
    objParam.tipoEnvio = parseInt($('#tipo_envio').text());
    objParam.esTurnado = ($('#es_turnado').text().toLowerCase() === 'true') ? true : false;
    objParam.esReenvio = ($('#es_reenvio').text().toLowerCase() === 'true') ? true : false;
    objParam.existeAdjuntos = ($('#existe_adjuntos').text().toLowerCase() === 'true') ? true : false;
    objParam.tieneRespuesta = ($('#tiene_respuesta').text().toLowerCase() === 'true') ? true : false;

    //--Origen
    objParamOrigen.envioId = parseInt($('#origen_identifica').text());
    objParamOrigen.tipoEnvio = parseInt($('#origen_tipo_envio').text());
    objParamOrigen.existeAdjuntos = ($('#origen_existe_adjuntos').text().toLowerCase() === 'true') ? true : false;

    //--
    objDelegar.estaActiva = ($('#InfoDelegar_EstaActiva').val() == 'True');
    objDelegar.tipoPermiso = (objDelegar.estaActiva) ? parseInt($('#InfoDelegar_TipoPermiso').val()) : 0;

    objDocumento.recepcionId = $('#Envio_Actual_RecepcionId').val();
    //--
    //Funciones
    if (objParam.tipoVisualizacion === objTipoVisualizacion.envio)
    {
        ObtenerCategoriasEnvio();      
    }
    else if (objParam.tipoVisualizacion === objTipoVisualizacion.recepcion)
    {
        ObtenerCategoriasRecepcion();
        //--
        $('#buttonCategoria').click(function () {
            ControlCategoria();
        });
        //--
        $('#buttonActualizar').click(function () {
            ActualizarOficio();
        });
    }

    //Archivos adjuntos
    if (objParam.existeAdjuntos)
    {
        LecturaArchivos();
    }

    //Origen solo en caso de turnados
    if ((objParam.esTurnado || (objParam.esReenvio && objParam.tipoEnvio === 1)) && objParamOrigen.existeAdjuntos) {
        LecturaArchivosOrigen();
    }

    //Visualización
    ControlVisualizacion();
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
*                                                   Categorias Scripts                                                                  *
*****************************************************************************************************************************************/
var objCategoriasRecepcion = { seleccionadas: new Array(), urln: "" };
function ObtenerCategoriasRecepcion() {
    objCategoriasRecepcion.urln = objWebRoot.route + "api/v1/categorias/recepcion/seleccionadas";

    //--
    $.ajax({
        url: objCategoriasRecepcion.urln,
        type: "POST",
        data: JSON.stringify({ Usuario: objUser.username, Folio: $('#folioInput').text(), EnvioId: objParam.envioId, TipoEnvio: objParam.tipoEnvio }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            objCategoriasRecepcion.seleccionadas = new Array();
            //--
            $.each(result, function (i, categoria) {
                if (categoria.Estado) {
                    if (categoria.Nombre === 'General') {
                        $('#tagsSaver').append('<input type="checkbox" class="categorias" id="catGeneral" value="' + categoria.CategoriaId + '" checked="true"/><small> ' + categoria.Nombre + '</small><br/>');
                    } else {
                        $('#tagsSaver').append('<input type="checkbox" class="categorias" value="' + categoria.CategoriaId + '" checked="true"/><small> ' + categoria.Nombre + '</small><br/>');
                    }
                    //--
                    objCategoriasRecepcion.seleccionadas.push(parseInt(categoria.CategoriaId));
                }
                else {
                    if (categoria.Nombre === 'General') {
                        $('#tagsSaver').append('<input type="checkbox" class="categorias" id="catGeneral" value="' + categoria.CategoriaId + '"/><small> ' + categoria.Nombre + '</small><br/>');
                    } else {
                        $('#tagsSaver').append('<input type="checkbox" class="categorias" value="' + categoria.CategoriaId + '"/><small> ' + categoria.Nombre + '</small><br/>');
                    }
                }
            });
            //Poner las categorias en el input del formulario
            $('#catSeleccionadas').text(objCategoriasRecepcion.seleccionadas.join(','));
            //--
            SeleccionCategoria();
        }
    });
    //--
}
//---
var objCategoriasEnvio = { seleccionadas: new Array(), urln: "" };
function ObtenerCategoriasEnvio() {
    objCategoriasEnvio.urln = objWebRoot.route + "api/v1/categorias/envio/seleccionadas";

    //--
    $.ajax({
        url: objCategoriasEnvio.urln,
        type: "POST",
        data: JSON.stringify({ Usuario: objUser.username, Folio: $('#folioInput').text(), EnvioId: objParam.envioId, TipoEnvio: objParam.tipoEnvio }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            objCategoriasEnvio.seleccionadas = new Array();
            //--
            $.each(result, function (i, categoria) {
                if (categoria.Estado) {
                    //$('#tagsSaver').append('<input type="checkbox" class="categorias" value="' + categoria.CategoriaId + '" checked="true"/><small> ' + categoria.Nombre + '</small><br/>');
                    $('#tagsSaver').append('<span class="ml-1 badge badge-light">' + categoria.Nombre + '</span>'); //<br/>
                    objCategoriasEnvio.seleccionadas.push(parseInt(categoria.CategoriaId));
                }
                else {
                    //$('#tagsSaver').append('<input type="checkbox" class="categorias" value="' + categoria.CategoriaId + '"/><small> ' + categoria.Nombre + '</small><br/>');
                }
            });
            //Poner las categorias en el input del formulario
            //$('#Categorias').val(objCategoriasEnvio.seleccionadas.join(','));
            //--
        }
    });
    //--
}
//---
/**************************************************************************************************************************************
*------------------------------------------- Agregar Categoria Scripts ---------------------------------------------------------------*
***************************************************************************************************************************************/
var objCategoria = { urln: "" };
function ControlCategoria() {
    $('#tagsSaver').append(
        '<div id="checkboxAdd"><small><div class=""><input class="form-control form-control-sm font-regular-10" id="nameTag" placeholder="Nombre categoría" required maxlength="100"/></div><div class="mb-2 text-center"><button id="agregarCategoria" type="button" class="btn btn-primary btn-sm pr-2 pl-2 mr-1"><i class="fa fa-check"></i></button><button class="btn btn-light btn-sm pr-2 pl-2" type="button" onclick="CancelarNuevaCategoria()"><i class="fas fa-times"></i></button></div></small></div>'
    );
    $('#buttonCategoria').prop("disabled", true);

    AgregarCategoria();
}
function AgregarCategoria() {
    objCategoria.urln = objWebRoot.route + "api/v1/categorias/agregar";

    $('#agregarCategoria').click(function () {

        if ($('#nameTag').val().length > 0) {
            $('#txtAviso').removeClass('d-none').addClass('d-none').text('');

            //--
            $.ajax({
                url: objCategoria.urln,
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
                        $('#buttonCategoria').prop("disabled", false);

                        //--
                        SeleccionCategoria();
                    } else if (result.Estado === 3) {
                        Notifica("No se puede guardar la categoría, ya existe una con el mismo nombre.");
                    } else if (result.Estado === 2 || result.Estado === 4) {
                        Notifica("No se puede guardar la categoría, inténtelo más tarde.");
                    }
                }
            });
            //--
        } else {
            $('#txtAviso').removeClass('d-none').text('El nombre de la categoría es requerido.');
        }

    });
}
function CancelarNuevaCategoria() {
    $('#checkboxAdd').empty();
    $('#checkboxAdd').remove();
    $('#buttonCategoria').prop("disabled", false);
}
/**************************************************************************************************************************************
*------------------------------------------- Seleccion Categoria Scripts --------------------------------------------------------------*
***************************************************************************************************************************************/
var objActualizarOficio = { categoriasSeleccionadas: new Array(), categorias: '', urln: '' };
function SeleccionCategoria() {

    $('#tagsSaver input[type=checkbox]').on('change', function () {
        objActualizarOficio.categoriasSeleccionadas = new Array();
        //
        if ($('#tagsSaver input[type=checkbox]:checked').length === 0) {
            $('input#catGeneral').prop('checked', true);
        }
        //
        $('#tagsSaver input[type=checkbox]:checked').each(function () {
            objActualizarOficio.categoriasSeleccionadas.push(parseInt($(this).val()));
        });
        //
        $('#catSeleccionadas').text(objActualizarOficio.categoriasSeleccionadas.join(','));
        //
    });
}
function ActualizarOficio() {
    objActualizarOficio.categorias = $('#catSeleccionadas').text();
    objActualizarOficio.urln = objWebRoot.route + "api/v1/categorias/recepcion/actualizar";

    //--
    $.ajax({
        url: objActualizarOficio.urln,
        type: "POST",
        data: JSON.stringify({ EnvioId: objParam.envioId, Usuario: objUser.username, Categorias: objActualizarOficio.categorias, TipoEnvio: objParam.tipoEnvio }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            if (parseInt(result.Estado) === 1) {
                //$('#txtAviso').removeClass('d-none').addClass('d-none').text('');
                Notifica("Se ha guardado correctamente la(s) categoría(s) del documento");
            } else {
                //$('#txtAviso').removeClass('d-none').text('En este momento, no se puede actualizar las categorías del oficio, inténtelo más tarde.');
                Notifica("En este momento, no se puede guardar la(s) categoría(s) al oficio, inténtelo más tarde.");
            }
        }
    });
    //--
}
/****************************************************************************************************************************************/

/****************************************************************************************************************************************
*                                          Script para generar pdf                                                                      *
*****************************************************************************************************************************************/
$("#download-pdf").click(function (e) {
	e.preventDefault();
	var idOficio = window.location.pathname.split("/").pop();

    //--
    $.ajax({
        url: $('#routeWebRoot').val() + "api/v1/PdfOficios/" + parseInt(idOficio),
        type: "GET",
        datatype: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function () {
            redirectUrl = $('#routeWebRoot').val() + "api/v1/PdfOficios/" + parseInt(idOficio);
            window.open(redirectUrl, '_blank');
        }
    });
    //--
});

/****************************************************************************************************************************************
*                                                Control Visualización                                                                  *
****************************************************************************************************************************************/
function ControlVisualizacion()
{
    $('#contenedor-destiantarios a').click(function () {
        window.open(this.href, "_visualiza", "width=960,height=980,menubar=no,resizable=no,status=no", true);
        return false;
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