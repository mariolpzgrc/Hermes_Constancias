var objWebRoot = { route: "", token: "" };
var objUser = { username: "", usuarioId: 0};
var objPaginacion = {
    selector: $('#paginacion-borradores'),
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

var objBorradores = { urln: "", info: Object.keys(new Object()) };
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
    objBorradores.urln = objWebRoot.route + 'api/v1/historico/bandeja/borradores/' + objUser.usuarioId + '/' + pagina;

    //--
    $.ajax({
        url: objBorradores.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objBorradores.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#historico-borradores tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            objBorradores.info = Object.keys(new Object());
            objBorradores.info = data;
        },
        error: function (xhr, status, error) {
            objBorradores.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if ((objBorradores.info.Datos).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objBorradores.info.Total_Paginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-borradores').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-borradores').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-borradores').twbsPagination('destroy');
            }

            ProcesaBandeja();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#historico-borradores tbody').fadeIn(500, "linear");
        }
    });
    //--
}

function ProcesaBandeja()
{
    if (objBorradores.info.Datos.length > 0) {
        $('#historico-borradores > tbody').empty();
        $.each(objBorradores.info.Datos, function (indice, valor) {
            objtr.elementobase = Object.keys(new Object());
            objtr.elementobase = $('<tr>').append(
                $('<td>' + valor.Folio + '</td>'),
                $('<td>' + valor.Destinatario + (valor.Destinatarios_Extras > 0 ? '(+' + valor.Destinatarios_Extras + ')' : '') + ' </td>'),
                $('<td>' + valor.Asunto + '</td>'),
                $('<td>' + moment(valor.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</td>'),
                $('<td>' + (valor.Importancia === true ? '<i title="Importancia Alta" class="fas fa-exclamation font-red" ></i>' : '') + '</td>'),
                $('<td>' + (valor.Adjuntos === true ? '<i title="Archivos Adjuntos Disponibles" class="fas fa-paperclip font-gray"></i>' : '') + '</td>')
            );

            $('#historico-borradores > tbody').append(objtr.elementobase);
        });
    } else {
        objtr.elementobase = Object.keys(new Object());
        objtr.elementobase = $('<tr class="table-light">').append(
            $('<td colspan="6" class="text-center">').text("Sin documentos para mostrar")
        );
        $('#historico-borradores > tbody').append(objtr.elementobase);
    }
}