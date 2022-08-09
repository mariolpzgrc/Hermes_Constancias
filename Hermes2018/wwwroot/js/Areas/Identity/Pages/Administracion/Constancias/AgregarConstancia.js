var objWebRoot = { route: "", toke: "" }
var objUser = { username: "" }

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('TokenWebApi').val();
    objUser.username = $('#user').text();

    ControlDias();
});

function ControlDias() {
    $("input[name='dias-atencion']").TouchSpin({
        initval: "",
        min: 1,
        max: 40,
        step: 1,
        boostat: 5,
        buttondown_class: 'btn',
        buttonup_class: 'btn'
    });
}