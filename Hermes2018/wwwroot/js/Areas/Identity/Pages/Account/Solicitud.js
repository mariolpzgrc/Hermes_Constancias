var objWebRoot = { route: "", token: "" };
var objUsuario = { anonimo: "" };
var objAreas = { info: "" }; //Object.keys(new Object())

//FUNCION PRINCIPAL
$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objUsuario.anonimo = $('#usuarioAnonimo').val();
    //--
    ControlAreas();

    return false;
});

function ControlAreas() {

    $('#Solicitud_AreaId').change(function () {
        if ($('#Solicitud_AreaId').val() !== "") {
            objAreas.info = "";
            objAreas.info = $("#Solicitud_AreaId option:selected").text().replace('-', ' ').trim();
            $("#Solicitud_Area").val(objAreas.info);
        }
        else {
            objAreas.info = "";
            $("#Solicitud_Area").val("");
        }
    });
}