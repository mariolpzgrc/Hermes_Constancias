var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objEstado = {
    semaforo: new Array(
        '<i class="fas fa-circle font-yellow pr-1" title="En proceso"></i>',
        '<i class="fas fa-circle font-green pr-1" title="Atendido"></i>',
        '<i class="fas fa-circle font-orange pr-1" title="Extemporáneo"></i>',
        '<i class="fas fa-circle font-red pr-1" title="Vencido"></i>',
        '<i class="fas fa-circle font-blue pr-1" title="Contestado parcialmente"></i>',
        '<i class="fas fa-circle font-dark-blue pr-1" title="Contestado completamente"></i>',
        '<i class="fas fa-circle font-gray pr-1" title="No requiere respuesta"></i>'
    )
};
var objTipoEnvio = {
    Indicador: new Array(
        '<i class="fas fa-long-arrow-alt-right pl-1 pr-1 font-gray" title="Envio"></i>',
        '<i class="fas fa-redo-alt pl-1 pr-1 font-gray" title="Turnado"></i>',
        '<i class="fab fa-stack-exchange pl-1 pr-1 font-gray" title="Respuesta parcial"></i>',
        '<i class="fas fa-exchange-alt pl-1 pr-1 font-gray" title="Respuesta"></i>'
    ),
    Textos: new Array(
        'envió documento a:',
        'turnó documento a:',
        'respondió (parcialmente) a:',
        'respondió a:'
    ),
    TextosReenvio: new Array(
        'reenvió un documento (envío) a:',
        '',
        '',
        'reenvió un documento (respuesta) a:'
    ),
    envio: 1,
    turnar: 2,
    respuestaParcial: 3,
    respuesta: 4
};
var objTipoPara = { para: 1, ccp:2 };
//--
var objSeguimientoEnvios = { urln: "", paramEnvioId: 0, paramDocumentoId: 0, paramTipoEnvioId: 0, paramRecepcionId: 0, paramUsuario: '', paramFolio: '', info: Object.keys(new Object()) };
var objSeguimientoPlantilla = { base: Object.keys(new Object()) };
var objControl = { envio: { fecha: '' }, arrTemp: new Array(), arrTempCCP: new Array(), cadena: '', cadenaCCP: '' };
//--
var objPaginacion = {
    opciones: {
        totalPages: 1,
        visiblePages: 3,
        startPage: 1,
        initiateStartPageClick: false,
        pageVariable: '{{pagina}}',
        first: 'Primero',
        prev: 'Anterior',
        next: 'Siguiente',
        last: 'Último',
        onPageClick: function (event, page)
        {
            ObtenerEnvios(page);
        }
    }
};
var objArchivos = { tieneAdjunto: false, urln: "", datos: Object.keys(new Object()) };
/****************************************************************************************************************************************
*                                                            Función: Main                                                              *
*****************************************************************************************************************************************/
$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    //--
    objSeguimientoEnvios.paramEnvioId = parseInt($('#Encabezado_EnvioId').val());
    objSeguimientoEnvios.paramDocumentoId = parseInt($('#Encabezado_DocumentoId').val());
    objSeguimientoEnvios.paramFolio = $("#encabezado-folio").text();
    objSeguimientoEnvios.paramTipoEnvioId = parseInt($('#Encabezado_TipoEnvioId').val());
    objSeguimientoEnvios.paramRecepcionId = parseInt($('#Encabezado_RecepcionId').val());
    objSeguimientoEnvios.paramUsuario = $('#Encabezado_Usuario').val();
    //--
    objArchivos.tieneAdjunto = ($('#Encabezado_TieneAdjuntos').val().toLowerCase() === 'true') ? true : false;

    if (objArchivos.tieneAdjunto)
        LecturaArchivos();

    ObtenerEnvios(1);
    ControlToggle();
    ControlVisualizacion();
});

function ObtenerEnvios(pagina) {
    objSeguimientoEnvios.urln = objWebRoot.route + "api/v1/documentos/seguimiento/envios" + "/" + pagina;
    //--
    objSeguimientoEnvios.info = Object.keys(new Object());

    //--
    $.ajax({
        url: objSeguimientoEnvios.urln,
        type: "POST",
        data: JSON.stringify({ EnvioId: objSeguimientoEnvios.paramEnvioId, DocumentoId: objSeguimientoEnvios.paramDocumentoId, Folio: objSeguimientoEnvios.paramFolio, TipoEnvioId: objSeguimientoEnvios.paramTipoEnvioId, RecepcionId: objSeguimientoEnvios.paramRecepcionId, Usuario: objSeguimientoEnvios.paramUsuario }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objSeguimientoEnvios.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#contiene-seguimiento').fadeOut(100, "linear");
        },
        success: function (data) {
            objSeguimientoEnvios.info = Object.keys(new Object());
            objSeguimientoEnvios.info = data;
        },
        error: function (xhr, status, error) {
            objSeguimientoEnvios.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if ((objSeguimientoEnvios.info.Datos1).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objSeguimientoEnvios.info.Total_Paginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-seguimiento').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-seguimiento').twbsPagination(objPaginacion.opciones);
            }
            else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-seguimiento').twbsPagination('destroy');
            }

            ProcesaSeguimiento();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#contiene-seguimiento').fadeIn(500, "linear");
        }
    });
    //--
}
function ProcesaSeguimiento()
{
    $('#contiene-seguimiento').empty();
    objControl.envio.fecha = '';
    objControl.cadena = '';

    //Recorre envios
    $.each(objSeguimientoEnvios.info.Datos1, function (i, item) {
        //--
        //Fecha
        if (objControl.envio.fecha !== item.Fecha)
        {
            objControl.envio.fecha = item.Fecha;
            $('#contiene-seguimiento').append($('<div class="col-12"><div class="border-0 bg-secondary p-1 pl-2 mb-1"><strong>' + item.Fecha + '</strong></div></div>'));
        }
        //--
        objSeguimientoPlantilla.base = Object.keys(new Object());
        objSeguimientoPlantilla.base = $('#plantilla > div').clone();
        objSeguimientoPlantilla.base.find('.nombre-de').text(item.De);
        objSeguimientoPlantilla.base.find('.tipo-envio').text((item.EsReenvio == true) ? objTipoEnvio.TextosReenvio[item.TipoEnvio - 1] : objTipoEnvio.Textos[item.TipoEnvio - 1] );
        //--
        objSeguimientoPlantilla.base.find('.titulo-envio').prepend($('<i class="indica-mas-o-menos font-dark-gray far fa-plus-square" title="Expandir/Contraer"></i>'));
        objSeguimientoPlantilla.base.find('.titulo-envio').append(item.EsPublico === true ? $('<i class="far fa-eye float-right font-dark-gray pr-2 pt-1" title="Público">') : $('<i class="far fa-eye-slash float-right font-dark-gray pr-2 pt-1" title="Privado">'));
        objSeguimientoPlantilla.base.find('.titulo-envio').attr({ 'data-toggle': "collapse", href: '#resumen-' + item.EnvioId, role: "button", 'aria-expanded': "true", 'aria-controls': 'resumen-' + item.EnvioId });
        objSeguimientoPlantilla.base.find('.titulo-envio').addClass("principal mb-1");
        //--
        objSeguimientoPlantilla.base.find('.contiene-resumen').attr({ 'id': 'resumen-' + item.EnvioId }).addClass("collapse show");
        //--
        objSeguimientoPlantilla.base.find('.titulo-envio').on('click', function (e) {
            if ($(this).find("i.indica-mas-o-menos").hasClass("far fa-plus-square")) {
                $(this).find("i.indica-mas-o-menos").removeClass("far fa-plus-square");
                $(this).find("i.indica-mas-o-menos").removeClass("far fa-minus-square");
                $(this).find("i.indica-mas-o-menos").addClass("far fa-minus-square");
            }
            else if ($(this).find("i.indica-mas-o-menos").hasClass("far fa-minus-square")) {
                $(this).find("i.indica-mas-o-menos").removeClass("far fa-plus-square");
                $(this).find("i.indica-mas-o-menos").removeClass("far fa-minus-square");
                $(this).find("i.indica-mas-o-menos").addClass("far fa-plus-square");
            }
        });
        //---
        if (item.Actual)
        {
            objSeguimientoPlantilla.base.find('.titulo-envio').append($('<i class="far fa-bookmark float-right font-dark-gray pr-2 pt-1" title="Actual">'));
        }
        //--
        $.each(item.Destinatarios, function (j, elem) {

            objSeguimientoPlantilla.base.find('.contiene-resumen').append(
                $('<div class="m-0 destinatario-inicial" data-usuario="'+ elem.UsuarioPara + '">').append(
                    $(objTipoEnvio.Indicador[item.TipoEnvio - 1]),
                    $(objEstado.semaforo[elem.Estado - 1]),
                    $('<span class="pr-1">').text(elem.Fecha),
                    $('<span class="pr-1 small font-dark">').text(elem.TipoPara === objTipoPara.para ? "[PARA]" : "[CC]"),
                    (item.EsPublico === true )?
                        $('<a class="pr-1 enlace-seguimiento">').text(elem.Para).attr({ "href": objWebRoot.route + 'Correspondencia/Visualizacion/' + item.EnvioId + '/' + item.VisualizacionTipoEnvio + '/' + elem.UsuarioPara })
                    :
                        $('<span class="pr-1 ">').text(elem.Para)
                )
            );

        });
        //--
        objSeguimientoPlantilla.base.find('.titulo-envio').addClass("bg-light pl-1");
        //--
        //--
        $('#contiene-seguimiento').append(objSeguimientoPlantilla.base);
    });

    //Recorre respuestas
    objControl.cadena = '';
    //--
    $.each(objSeguimientoEnvios.info.Datos2, function (i, item) {
        //--
        objSeguimientoPlantilla.base = Object.keys(new Object());
        objSeguimientoPlantilla.base = $('#plantilla > div').clone();
        objSeguimientoPlantilla.base.find('.base .contiene-resumen ').addClass("border-right-plus border-left-plus");
        objSeguimientoPlantilla.base.find('.nombre-de').text(item.De);
        objSeguimientoPlantilla.base.find('.tipo-envio').text(objTipoEnvio.Textos[item.TipoEnvio - 1]);
        //--
        objSeguimientoPlantilla.base.find('.titulo-envio').append(item.EsPublico === true ? $('<i class="far fa-eye float-right font-dark-gray pr-2 pt-1" title="Público">') : $('<i class="far fa-eye-slash float-right font-dark-gray pr-2 pt-1" title="Privado">'));
        //--
        if (item.Actual) {
            objSeguimientoPlantilla.base.find('.titulo-envio').append($('<i class="far fa-bookmark float-right font-dark-gray pr-2 pt-1">'));
        }
        //--
        objControl.cadena = '';
        objControl.cadena = objControl.cadena + '#contiene-seguimiento #resumen-' + item.EnvioPadre + ' .destinatario-inicial[data-usuario*="' + item.UsuarioOrigen + '"]';
        //--
        $.each(item.Destinatarios, function (j, elem) {

            objSeguimientoPlantilla.base.find('.contiene-resumen').append(
                $('<div class="m-0 ">').append(
                    $(objTipoEnvio.Indicador[item.TipoEnvio - 1]),
                    $(objEstado.semaforo[elem.Estado - 1]),
                    $('<span class="pr-1">').text(elem.Fecha),
                    $('<span class="pr-1 small font-dark">').text(elem.TipoPara === objTipoPara.para ? "[PARA]" : "[CC]"),
                    (item.EsPublico === true) ?
                        $('<a class="pr-1 enlace-seguimiento">').text(elem.Para).attr({ "href": objWebRoot.route + 'Correspondencia/Visualizacion/' + item.EnvioId + '/' + item.TipoEnvio + '/' + elem.UsuarioPara })
                    :
                        $('<span class="pr-1 ">').text(elem.Para)
                )
            );

        });
        //--
        objSeguimientoPlantilla.base.find('.titulo-envio').addClass("bg-light-plus pl-1");
        //--
        if ($(objControl.cadena).length > 0) {
            objSeguimientoPlantilla.base.first().removeClass("col-12").addClass("pl-4");
            //--
            $(objControl.cadena).append(objSeguimientoPlantilla.base);
        }  
        else
            $('#contiene-seguimiento').append(objSeguimientoPlantilla.base);

        //---
    });
}
function ControlToggle()
{
    $("#control-toggle").click(function ()
    {
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
/****************************************************************************************************************************************
*                                                Control Visualización                                                                  *
****************************************************************************************************************************************/
function ControlVisualizacion() {
    $('#contiene-seguimiento').on("click", "a.enlace-seguimiento" ,function () {
        window.open(this.href, "", "width=960,height=980,menubar=no,resizable=no,status=no", true);
        return false;
    });
}

function LecturaArchivos() {
    objArchivos.urln = objWebRoot.route + "api/v1/anexos/descarga/envios";
    objArchivos.datos = Object.keys(new Object());

    //--
    $.ajax({
        url: objArchivos.urln,
        type: "POST",
        data: JSON.stringify({ Folio: objSeguimientoEnvios.paramFolio, TipoEnvioId: objSeguimientoEnvios.paramTipoEnvioId, EnvioId: objSeguimientoEnvios.paramEnvioId }),
        datatype: "application/json",
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (data !== '') {
                //--
                objArchivos.datos = Object.keys(new Object());
                objArchivos.datos = data;
                //--
                //Recorre
                $.each(objArchivos.datos, function (i, item) {
                    $('#containerArchivos').append($('<span class="tm-tag cm-tag">' + '<a class="cmt-titulo" href="' + objWebRoot.route + item.Ruta + '" download>' + item.Nombre + '</a>' + '</span>'));
                });
                //--
            }
        }
    });
    //--
}