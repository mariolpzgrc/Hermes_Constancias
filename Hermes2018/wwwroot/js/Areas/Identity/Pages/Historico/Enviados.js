var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usuarioId: 0 };
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4 };
var objPagina = { tipoHistorico: 1, bandeja: 1 };

var objPaginacion = {
    selector: $('#paginacion-enviados'),
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
        onPageClick: function (event, page) {
            ObtenerInfo(page);
        }
    }
};

var objEnviados = { urln: "", info: Object.keys(new Object()) };
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
    objEnviados.urln = objWebRoot.route + 'api/v1/historico/bandeja/enviados/' + objUser.usuarioId + '/' + pagina;

    //--
    $.ajax({
        url: objEnviados.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objEnviados.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#historico-enviados tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            objEnviados.info = Object.keys(new Object());
            objEnviados.info = data;
        },
        error: function (xhr, status, error) {
            objEnviados.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if ((objEnviados.info.Datos).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objEnviados.info.Total_Paginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-enviados').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-enviados').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-enviados').twbsPagination('destroy');
            }

            ProcesaBandeja();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#historico-enviados tbody').fadeIn(500, "linear");
        }
    });
    //--
}

function ProcesaBandeja()
{
    if (objEnviados.info.Datos.length > 0) {
        $('#historico-enviados > tbody').empty();

        $.each(objEnviados.info.Datos, function (indice, valor) {
            objtr.elementobase = Object.keys(new Object());

            objtr.elementobase = $('<tr>').append(
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Historico/Lectura/' + objUser.usuarioId + '/' + objPagina.tipoHistorico + '/' + objPagina.bandeja + '/' + valor.EnvioId + '/' + valor.TipoEnvio + '">' + valor.Folio + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Historico/Lectura/' + objUser.usuarioId + '/' + objPagina.tipoHistorico + '/' + objPagina.bandeja + '/' + valor.EnvioId + '/' + valor.TipoEnvio + '">' + valor.Destinatario + (valor.Destinatarios_Extras > 0 ? ' (+' + valor.Destinatarios_Extras + ')' : '') + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Historico/Lectura/' + objUser.usuarioId + '/' + objPagina.tipoHistorico + '/' + objPagina.bandeja + '/' + valor.EnvioId + '/' + valor.TipoEnvio + '">' + valor.Asunto + '</a>')
                ),
                $('<td>' + moment(valor.FechaEnvio).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</td>'),
                $('<td>' + (valor.Importancia === true ? '<i title="Importancia Alta" class="fas fa-exclamation font-red" ></i>' : '') + '</td>'),
                $('<td>' + (valor.Adjuntos === true ? '<i title="Archivos Adjuntos Disponibles" class="fas fa-paperclip font-gray"></i>' : '') + '</td>'),
                $('<td>' +
                    (valor.TipoEnvio === objTipoEnvio.envio ?
                        '<i title="Doc. Enviado" class="fas fa-long-arrow-alt-right font-gray">'
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
                $('<td>' +
                    (valor.Estado === "No requiere respuesta" ?
                        '<i class="fas fa-circle font-gray" title="No requiere respuesta"></i>' :
                        valor.Estado === "Contestado parcialmente" ?
                            '<i class="fas fa-circle font-orange" title="Contestado parcialmente"></i>' :
                        valor.Estado === "Contestado completamente" ?
                                '<i class="fas fa-circle font-green" title="Contestado completamente"></i>' :
                                ''
                    )
                    + '</td>')
            );

            $('#historico-enviados > tbody').append(objtr.elementobase);
        });
    } else {
        objtr.elementobase = Object.keys(new Object());
        objtr.elementobase = $('<tr class="table-light">').append(
            $('<td colspan="8" class="text-center">').text("Sin documentos para mostrar")
        );

        $('#historico-enviados > tbody').append(objtr.elementobase);
    }
}