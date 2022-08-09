var objp = { urln: "" };
var objd = { urln: "" };
var objc = { urln: "" };
var obja = { urln: "" };

var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usersession: "" };
//--
var objParam = { envioId: 0, tipoVisualizacion: 0, tipoEnvio: 0, esTurnado: false, existeAdjuntos: false, tieneRespuesta: false, fechaPropuesta: "", esReenvio: false };
var objParamOrigen = { envioId: 0, tipoEnvio: 0, existeAdjuntos: false };
var objDelegar = { estaActiva: false, tipoPermiso: 0 };
//--
var objTipoVisualizacion = { recepcion: 1, envio: 2 };
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4, reenvio: 5};
//--
var objDocumento = { envioId: 0, tipoEnvioId: 0, recepcionId: 0};

//---Calendario 
var objCalendario = {
    datePicker: null, dias: "", datos: new Array(), inicio: "", limite: "", esVigente: 0, configuracion: null };

//--Tramite
var objTramite = {
    tipo: 0, dias: 0, const: { general: 1 }
};

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
    objParam.fechaPropuesta = $('#fecha_propuesta').text();
    
    //--Origen
    objParamOrigen.envioId = parseInt($('#origen_envio_identifica').text());
    objParamOrigen.tipoEnvio = parseInt($('#origen_tipo_envio').text());
    objParamOrigen.existeAdjuntos = ($('#origen_existe_adjuntos').text().toLowerCase() === 'true') ? true : false;
    
    //--
    objDelegar.estaActiva = ($('#InfoDelegar_EstaActiva').val() == 'True');
    objDelegar.tipoPermiso = (objDelegar.estaActiva) ? parseInt($('#InfoDelegar_TipoPermiso').val()) : 0;

    objDocumento.envioId = $('#NuevoCompromiso_EnvioId').val();
    objDocumento.tipoEnvioId = $('#NuevoCompromiso_TipoEnvioId').val();
    objDocumento.recepcionId = $('#NuevoCompromiso_RecepcionId').val();
    //--
    objCalendario.configuracion = {
        dateFormat: "d/m/Y",
        clickOpens: true,
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
        disable: "", //objCalendario.datos
        minDate: "",//objCalendario.inicio
        maxDate: "", //objCalendario.limite
        defaultDate : ""
    };

    //Funciones
    if (objParam.tipoVisualizacion === objTipoVisualizacion.envio)
    {
        ObtenerCategoriasEnvio();      
    }
    else if (objParam.tipoVisualizacion === objTipoVisualizacion.recepcion)
    {
        if (objDelegar.estaActiva === false || (objDelegar.estaActiva === true && objDelegar.tipoPermiso === 1)) {
            ObtenerCategoriasRecepcion();
            //--
            $('#buttonCategoria').click(function () {
                ControlCategoria();
            });
            //--
            $('#buttonActualizar').click(function () {
                ActualizarOficio();
            });
            //--

            if (objParam.tieneRespuesta == false) {
                //Control Fecha
            }
        }
        else {
            ObtenerCategoriasRecepcionSoloLectura();  
        }
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

    if (objDocumento.tipoEnvioId == objTipoEnvio.envio) {
        //Tramites
        ControlTramites();
    } else if (objDocumento.tipoEnvioId == objTipoEnvio.turnar) {
        Control2Tramites();
    }

    //---
    ControlAutocompletarPara();

    console.log($('#containerMailsFor').html());

    var texto = document.getElementById("containerMailsFor").innerHTML;
    console.log(texto);
});

$.validator.setDefaults({ ignore: [] });
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

function LecturaArchivosOrigen()
{
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
function ObtenerCategoriasRecepcionSoloLectura() {
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
                    $('#tagsSaver').append('<span class="ml-1 badge badge-light">' + categoria.Nombre + '</span>');
                    //--
                    objCategoriasRecepcion.seleccionadas.push(parseInt(categoria.CategoriaId));
                }
            });
            //Poner las categorias en el input del formulario
            $('#catSeleccionadas').text(objCategoriasRecepcion.seleccionadas.join(','));
        }
    });
    //--
}
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
    
    $('#tagsSaver input[type=checkbox]').on('change', function ()
    {
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
*                                      Ocultar botones para el delegado Revisor                                                         *
****************************************************************************************************************************************/
//var userPermiso = document.getElementById('user-permiso').innerHTML;
//var btnEnviar = document.getElementById('responderEnviar');

//function checarPermisosUsuario() {
//	if (userPermiso === '2') {
//		btnEnviar.style.display = "none";
//		turnarBtn.style.display = "none";
//	} else {
//		btnEnviar.style.display = "block";
//	}
//}
//checarPermisosUsuario();
//element.addEventListener("keyup", checarPermisosUsuario);

/****************************************************************************************************************************************
*                                                Control Visualización                                                                  *
****************************************************************************************************************************************/
function ControlVisualizacion()
{
    $('#contenedor-destiantarios a').click(function () {
        window.open(this.href, "", "width=960,height=980,menubar=no,resizable=no,status=no", true);
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
/****************************************************************************************************************************************
*                                                Control Fecha compromiso                                                                  *
****************************************************************************************************************************************/

function ControlFecha() {
    $('#btnCambiar').click(function () {
        $('#contieneAgregarFecha').removeClass("d-none");
        $('#btnCambiar').prop("disabled", true);
    });
    //--
    $("#btnAgregarFecha").click(function () {
        if ($("#fechaCompromiso").val() != "") {
            //--
            $.ajax({
                url: objWebRoot.route + "api/v1/documentos/compromiso/cambiar",
                type: "POST",
                data: JSON.stringify({ RecepcionId: objDocumento.recepcionId, Fecha: $('#fechaCompromiso').val() }),
                datatype: "application/json",
                contentType: 'application/json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
                },
                success: function (result) {
                    if (result.Estado == 1) {
                        $('#contieneAgregarFecha').removeClass("d-none");
                        $('#contieneAgregarFecha').addClass("d-none");
                        $('#fechaCompromisoActual').text($('#fechaCompromiso').val());
                        $('#fechaCompromiso').val("");
                        $('#btnCambiar').prop("disabled", false);
                        //--
                    }
                }
            });
            //--
        }
    });
    //--
    $("#btnCancelarFecha").click(function () {
        $('#contieneAgregarFecha').removeClass("d-none");
        $('#contieneAgregarFecha').addClass("d-none");
        $('#fechaCompromiso').val("");
        $('#btnCambiar').prop("disabled", false);
    });
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

function ControlAutocompletarPara(){
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


/****************************************************************************************************************************************
*                                                Control Tramites y Calendarios                                                         *
****************************************************************************************************************************************/

function ControlTramites() {
    //Por defecto
    objTramite.tipo = parseInt($("select#NuevoCompromiso_TramiteId option:selected").val());
    objTramite.dias = parseInt($("select#NuevoCompromiso_TramiteId option:selected").data("tdias"));
    //--
    $.ajax({
        url: objWebRoot.route + "api/v1/calendario/info/" + objTramite.dias + "/" + objDocumento.recepcionId,
        type: "GET",
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            if ($.trim(result) != null || $.trim(result) != undefined)
            {
                objCalendario.dias = result.Dias;
                objCalendario.datos = objCalendario.dias.split(',');
                objCalendario.inicio = result.Inicio;
                objCalendario.limite = result.Limite;
                objCalendario.esVigente = result.EsVigente;

                objCalendario.configuracion.disable = objCalendario.datos;
                objCalendario.configuracion.minDate = objCalendario.inicio;
                objCalendario.configuracion.maxDate = objCalendario.limite;

                if (objTramite.tipo == objTramite.const.general) {
                    objCalendario.configuracion.defaultDate = objParam.fechaPropuesta;
                    objCalendario.configuracion.clickOpens = true;
                } else {
                    objCalendario.configuracion.defaultDate = objCalendario.limite;
                    objCalendario.configuracion.clickOpens = false;
                }

                objCalendario.datePicker = flatpickr('#NuevoCompromiso_FechaCompromiso', objCalendario.configuracion);

                if (objCalendario.esVigente < 0) {
                    $("#NuevoCompromiso_FechaCompromiso").removeClass("text-danger");
                    $("#NuevoCompromiso_FechaCompromiso").addClass("text-danger");
                } else {
                    $("#NuevoCompromiso_FechaCompromiso").removeClass("text-danger");
                }
            }
        }
    });
    //--

    //--Al cambiar de opción
    $("select#NuevoCompromiso_TramiteId").change(function () {

        objTramite.tipo = parseInt($("select#NuevoCompromiso_TramiteId option:selected").val());
        objTramite.dias = parseInt($("select#NuevoCompromiso_TramiteId option:selected").data("tdias"));
        //--
        $.ajax({
            url: objWebRoot.route + "api/v1/calendario/info/" + objTramite.dias + "/" + objDocumento.recepcionId,
            type: "GET",
            datatype: "application/json",
            contentType: 'application/json',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            },
            success: function (result) {
                if ($.trim(result) != null || $.trim(result) != undefined)
                {
                    objCalendario.dias = result.Dias;
                    objCalendario.datos = objCalendario.dias.split(',');
                    objCalendario.inicio = result.Inicio;
                    objCalendario.limite = result.Limite;
                    objCalendario.esVigente = result.EsVigente;

                    objCalendario.configuracion.disable = objCalendario.datos;
                    objCalendario.configuracion.minDate = objCalendario.inicio;
                    objCalendario.configuracion.maxDate = objCalendario.limite;

                    if (objTramite.tipo == objTramite.const.general) {
                        objCalendario.configuracion.defaultDate = objParam.fechaPropuesta;
                        objCalendario.configuracion.clickOpens = true;
                    } else {
                        objCalendario.configuracion.defaultDate = objCalendario.limite;
                        objCalendario.configuracion.clickOpens = false;
                    }

                    objCalendario.datePicker.destroy();
                    objCalendario.datePicker = flatpickr('#NuevoCompromiso_FechaCompromiso', objCalendario.configuracion);

                    if (objCalendario.esVigente < 0) {
                        $("#NuevoCompromiso_FechaCompromiso").removeClass("text-danger");
                        $("#NuevoCompromiso_FechaCompromiso").addClass("text-danger");
                    } else {
                        $("#NuevoCompromiso_FechaCompromiso").removeClass("text-danger");
                    }
                }
            }
        });
        //--
    });
}

function Control2Tramites() {
    //--
    objCalendario.dias = $("#Inhabiles_Dias").val();
    objCalendario.datos = objCalendario.dias.split(',');
    objCalendario.inicio = $("#Inhabiles_Inicio").val();
    objCalendario.limite = $("#Inhabiles_Limite").val();
    objCalendario.esVigente = parseInt($("#Inhabiles_EsVigente").val());

    objCalendario.configuracion.disable = objCalendario.datos;
    objCalendario.configuracion.minDate = objCalendario.inicio;
    objCalendario.configuracion.maxDate = objCalendario.limite;
    //--
    objCalendario.configuracion.defaultDate = $.trim($('#fechaCompromisoActual').text());

    objCalendario.datePicker = flatpickr('#NuevoCompromiso_FechaCompromiso', objCalendario.configuracion);
    //--
    if (objCalendario.esVigente < 0) {
        $("#NuevoCompromiso_FechaCompromiso").removeClass("text-danger");
        $("#NuevoCompromiso_FechaCompromiso").addClass("text-danger");
    } else {
        $("#NuevoCompromiso_FechaCompromiso").removeClass("text-danger");
    }
}