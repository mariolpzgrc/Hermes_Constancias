var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usuarioId: 0 };
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4 };
var objPagina = { tipoHistorico :1, bandeja :1 };
var objTipoPara = { para: 1, ccp: 2 };

var objPaginacion = {
    selector: $('#paginacion-recibidos'),
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
            ObtenerInfo(page);
        }
    }
};

var objRecibidos = { urln: "", info: Object.keys(new Object()) };
var objtr = { elementobase: Object.keys(new Object()) };

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#userLogueado").text();

    objUser.usuarioId = $('#InfoUsuarioId').val();
    objPagina.tipoHistorico = $('#TipoHistorico').val();
    objPagina.bandeja = $('#Bandeja').val();

    ObtenerInfo(1);

    return false;
});

function ObtenerInfo(pagina)
{
    objRecibidos.urln = objWebRoot.route + 'api/v1/historico/bandeja/recibidos/' + objUser.usuarioId + '/' + pagina;

    //--
    $.ajax({
        url: objRecibidos.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objRecibidos.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#historico-recibidos tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            objRecibidos.info = Object.keys(new Object());
            objRecibidos.info = data;
        },
        error: function (xhr, status, error) {
            objRecibidos.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if ((objRecibidos.info.Datos).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objRecibidos.info.Total_Paginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-recibidos').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-recibidos').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-recibidos').twbsPagination('destroy');
            }

            ProcesaBandeja();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#historico-recibidos tbody').fadeIn(500, "linear");
        }
    });
    //--
}

function ProcesaBandeja()
{
    $('#historico-recibidos tbody').empty();

    if (objRecibidos.info.Datos.length > 0) {
        //console.log(objRecibidos.info.Datos);
        $.each(objRecibidos.info.Datos, function (indice, valor) {
            objtr.elementobase = Object.keys(new Object());

            objtr.elementobase = $('<tr>').append(
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Historico/Lectura/' + objUser.usuarioId + '/'+ objPagina.tipoHistorico + '/' + objPagina.bandeja + '/' + valor.EnvioId + '/' + valor.TipoEnvio + '"><span class="' + (valor.Leido === false ? 'font-weight-bold' : '') + '">' + valor.Folio + '</span></a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Historico/Lectura/' + objUser.usuarioId + '/'+ objPagina.tipoHistorico + '/' + objPagina.bandeja +  '/' + valor.EnvioId + '/' + valor.TipoEnvio + '"><span class="' + (valor.Leido === false ? 'font-weight-bold' : '') + '">' + valor.Remitente + '</span></a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Historico/Lectura/' + objUser.usuarioId + '/'+ objPagina.tipoHistorico + '/' + objPagina.bandeja +  '/' + valor.EnvioId + '/' + valor.TipoEnvio + '"><span class="' + (valor.Leido === false ? 'font-weight-bold' : '') + '">' + valor.Asunto + '</span></a>')
                ),
                $('<td>' + moment(valor.FechaRecepcion).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</td>'),
                $('<td>' + (valor.Importancia === true ? '<i title="Importancia Alta" class="fas fa-exclamation font-red" ></i>' : '') + '</td>'),
                $('<td>' + (valor.Adjuntos === true ? '<i title="Archivos Adjuntos Disponibles" class="fas fa-paperclip font-gray"></i>' : '') + '</td>'),
                $('<td>' +
                    (valor.TipoEnvio === objTipoEnvio.envio ?
                        '<i title="Doc. Enviado"  class="fas fa-long-arrow-alt-left font-gray"></i>'
                    :
                        valor.TipoEnvio === objTipoEnvio.turnar ?
                            '<i title="Doc. Turnado" class="fas fa-redo-alt font-gray">'
                        :
                            valor.TipoEnvio === objTipoEnvio.respuestaParcial ?
                            '<i title="Respuesta parcial" class="fab fa-stack-exchange font-gray"></i>'
                            : 
                                valor.TipoEnvio === objTipoEnvio.respuesta ?
                                '<i title="Respuesta" class="fas fa-exchange-alt font-gray"></i>' : '')
                    + '</td>'),
                $('<td>').append(
                    (valor.TipoPara === objTipoPara.ccp) ?
                        $('<div data-toggle="tooltip" data-placement="top" title="Con copia"><i class="far fa-closed-captioning font-gray"></i></div>')
                        : ''
                ),
                $('<td>' +
                    (valor.Estado === "No requiere respuesta" ?
                        '<i class="fas fa-circle font-gray" title="No requiere respuesta"></i>' :
                        valor.Estado === "En proceso" ?
                            '<i class="fas fa-circle font-yellow" title="En proceso"></i>' :
                            valor.Estado === "Atendido" ?
                                '<i class="fas fa-circle font-green" title="Atendido"></i>' :
                                valor.Estado === "Extemporáneo" ?
                                    '<i class="fas fa-circle font-orange" title="Extemporáneo"></i>' :
                                    valor.Estado === "Vencido" ?
                                        '<i class="fas fa-circle font-red" title="Vencido"></i>' :
                                        ''
                    )
                    + '</td>')
            );

            $('#historico-recibidos > tbody').append(objtr.elementobase);
        });

    } else {
        objtr.elementobase = Object.keys(new Object());
        objtr.elementobase = $('<tr class="table-light">').append(
            $('<td colspan="8" class="text-center">').text("Sin documentos para mostrar")
        );
        $('#historico-recibidos > tbody').append(objtr.elementobase);
    }
}
