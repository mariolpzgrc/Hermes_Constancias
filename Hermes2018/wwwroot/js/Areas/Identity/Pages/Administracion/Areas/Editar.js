var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objp = { urln: "", metodo: "", datos: {} };
var objAreas = { info: Object.keys(new Object()) };
var objcheck = { valor: false };

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    //--
    ControlDias();

});
$.validator.setDefaults({ ignore: [] });

function ControlDias() {
    $("input#Editar_Dias_Compromiso").TouchSpin({
        initval: "",
        min: 1,
        max: 40,
        step: 1,
        boostat: 5,
        buttondown_class: 'btn',
        buttonup_class: 'btn'
    });
}