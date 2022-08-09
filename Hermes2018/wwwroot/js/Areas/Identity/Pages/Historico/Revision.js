var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usuarioId: 0};
var objPaginacion = {
    selector: $('#paginacion-revision'),
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
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4 };

var objRevision = { urln: "", info: Object.keys(new Object()) };
var objtr = { elementobase: Object.keys(new Object()) };

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#userLogueado").text();
    objUser.usuarioId = $('#InfoUsuarioId').val();

    ObtenerInfo(1);

    return false;
});



function ObtenerInfo(pagina)
{
    objRevision.urln = objWebRoot.route + 'api/v1/historico/bandeja/revision/' + objUser.usuarioId + '/' + pagina;

    //--
    $.ajax({
        url: objRevision.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objRevision.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#historico-revision tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            objRevision.info = Object.keys(new Object());
            objRevision.info = data;
        },
        error: function (xhr, status, error) {
            objRevision.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if ((objRevision.info.Datos).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objRevision.info.Total_Paginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-revision').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-revision').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-revision').twbsPagination('destroy');
            }

            ProcesaBandeja();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#historico-revision tbody').fadeIn(500, "linear");
        }
    });
    //--
}

function ProcesaBandeja()
{
    if (objRevision.info.Datos.length > 0) {
        $('#historico-revision > tbody').empty();

        $.each(objRevision.info.Datos, function (indice, valor) {
            objtr.elementobase = Object.keys(new Object());
            objtr.elementobase = $('<tr>').append(
                $('<td>' + valor.Folio + '</td>'),
                $('<td>' + valor.Destinatario + '</td>'),
                $('<td>' + valor.Asunto + '</td>'),
                $('<td>' + moment(valor.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</td>'),
                $('<td>' + (valor.Importancia === true ? '<i title="Importancia Alta" class="fas fa-exclamation font-red" ></i>' : '') + '</td>'),
                $('<td>' + (valor.Adjuntos === true ? '<i title="Archivos Adjuntos Disponibles" class="fas fa-paperclip font-gray"></i>' : '') + '</td>'),
                $('<td>' +
                    (valor.TipoEnvio === objTipoEnvio.turnar ?
                    '<i title="Doc. Reenviado" class="fas fa-share font-gray">'
                    :
                        valor.TipoEnvio === objTipoEnvio.respuestaParcial ?
                        '<i title="Respuesta parcial" class="fab fa-stack-exchange font-gray"></i>'
                        :
                        valor.TipoEnvio === objTipoEnvio.respuesta ?
                            '<i title="Respuesta" class="fas fa-reply font-gray"></i>' : '')
                    + '</td>')
            );

            $('#historico-revision > tbody').append(objtr.elementobase);
        });
    } else {
        objtr.elementobase = Object.keys(new Object());
        objtr.elementobase = $('<tr class="table-light">').append(
            $('<td colspan="7" class="text-center">').text("Sin documentos para mostrar")
        );

        $('#historico-revision > tbody').append(objtr.elementobase);
    }
}