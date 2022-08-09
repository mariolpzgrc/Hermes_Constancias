var objWebRoot = { route: "", token: "" },
    options = { year: 'numeric', month: 'long', day: 'numeric' },
    objBusqueda = { cadena: '', tipoBusqueda: '', seBusco: false },
    objRegsitro = {Id: 0, idConstancia: 0, fecha: '', numPerE: 0, numPerR: 0, }
    objGetInfoConstancia = { Id: 0, info: Object.keys(new Object()), urln: "" },
    objBusquedaPersonal = { Id: 0, info: Object.keys(new Object()), urln: "" },
    objEntrega = { Id: 0, info: Object.keys(new Object()), urln: "" };


$(document).ready(function () {
    getData();
    ObtenerInfoConstancia();
    ControlBusquedaPersonalAdmin();
    ControlBusquedaPersonalUser();
    ControlToggle();
});

function getData() {
    var getInfo = JSON.parse(localStorage.getItem('entregaParams'));
    if (localStorage.getItem('entregaParams') === null) {
        window.history.back();
    } else {
        console.log(getInfo.ConstanciaId);
        objGetInfoConstancia.Id = getInfo.ConstanciaId;
    }
}

function ObtenerInfoConstancia() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/GET_ConstanciaSolicitadaId';
    $.ajax({
        url: objGetInfoConstancia.urln,
        type: 'POST',
        data: JSON.stringify({ Id: objGetInfoConstancia.Id }),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            console.log(data);
            objGetInfoConstancia.info = Object.keys(new Object());
            objGetInfoConstancia.info = data;
            if (objGetInfoConstancia.info) {
                $('#numeroPersonal').text(objGetInfoConstancia.info.NoPersonal);
                $('#solicitante').text(objGetInfoConstancia.info.NombreUsuario.toUpperCase() + ' (' + objGetInfoConstancia.info.UsuarioId + ')');
                $('#folio').text(Mayuscula(objGetInfoConstancia.info.Folio));
                $('#tipoConstancia').text(objGetInfoConstancia.info.NombreConstancia);
                $('#fechaSolicitud').text(Mayuscula(new Date(objGetInfoConstancia.info.FechaSolicitud).toLocaleDateString("es-ES", options)));
            } else {
                alert("Error al obtener los datos");
            }
        },
        error: function () {
            alert('No pudó terminar la operación');
        },
        complete: function (xhr, estatus) {
            console.log(objGetInfoConstancia.info.EstadosConstancia);
            $.each(objGetInfoConstancia.info.EstadosConstancia, function (index, value) {
                if (value.EstadoId == 2) {   
                    $('#fechaGeneracion').text(Mayuscula(new Date(objGetInfoConstancia.info.EstadosConstancia[index].FechaHora).toLocaleDateString("es-ES", options)));
                }
            });
        }
    });
}

function ControlBusquedaPersonalAdmin() {
    jQuery(document).on('keyup', 'input#noper', function (event) {
        if (event.key === 'Enter' || event.keyCode === 13) {
            // Do something
            objBusqueda.cadena = $(this).val().trim();
            objBusqueda.tipoBusqueda = "Admin";
            console.log(objBusqueda.cadena);
            //BuscarPersonal(objBusqueda.cadena, objBusqueda.tipoBusqueda);
        }
    });
}

function ControlBusquedaPersonalUser() {
    jQuery(document).on('keyup', 'input#noper', function (event) {
        if (event.key === 'Enter' || event.keyCode === 13) {
            // Do something
            objBusqueda.cadena = $(this).val().trim();
            objBusqueda.tipoBusqueda = "User";
            console.log(objBusqueda.cadena);
            //BuscarPersonal(objBusqueda.cadena, objBusqueda.tipoBusqueda);
        }
    });
}

function BuscarPersonal(numpersonal, tipobusqueda) {
    objBusquedaPersonal.urln = objWebRoot.route + '';
    $.ajax({
        url: objBusquedaPersonal.urln,
        type: 'POST',
        data: JSON.stringify({ Id: numpersonal }),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (tipobusqueda === "Admin") {

            } else {

            }
        },
        error: function () {
            alert('No pudó terminar la operación');
        }
    });

}

function GuardarEntrega() {
    objEntrega.urln = objWebRoot.route + '';
    $.ajax({
        url: objEntrega.urln,
        type: 'POST',
        data: JSON.stringify({ Id: numpersonal }),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (data.CodeResponse === 0) {
                $('#entregaModal').modal('show');
            } else {

            }
        },
        error: function () {
            alert('No pudó terminar la operación');
        }
    });
}

function Mayuscula(string) {
    return string.toUpperCase();
}

/**    Control de Toggle **/
function ControlToggle() {
    $("#control-toggle").click(function () {
        $("#contenidoConstancia").toggle({
            duration: 'fast',
            complete: function () {
                if ($('#contenidoConstancia:visible').length === 0) {
                    $("#control-toggle i").removeClass('fas fa-angle-down').addClass('fas fa-angle-right');
                } else {
                    $("#control-toggle i").removeClass('fas fa-angle-right').addClass('fas fa-angle-down');
                }
            }
        });
    });
}