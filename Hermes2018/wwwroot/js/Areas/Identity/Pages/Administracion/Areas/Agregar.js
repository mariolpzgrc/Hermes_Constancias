var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objp = { urln: "", metodo: "", datos: {}, id: "" };
var objAreas = { info: Object.keys(new Object()) };
var objcheck = { valor: false };

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    //---
    ControlAgregarLogo();
    ControlDias();
});
$.validator.setDefaults({ ignore: '' });

function ControlDias() {
    $("input#Agregar_Dias_Compromiso").TouchSpin({
        initval: '',
        min: 1,
        max: 40,
        step: 1,
        boostat: 5,
        buttondown_class: 'btn',
        buttonup_class: 'btn'
    });
}

function ControlAgregarLogo() {
    $("#Agregar_AgregarLogo").change(function () {

        if ($("#Agregar_AgregarLogo").is(":checked")) {
            $(".contiene-logo").removeClass("d-none");
        } else {
            $(".contiene-logo").removeClass("d-none");
            $(".contiene-logo").addClass("d-none");
        }
    });
}

//-----------------------------------------
$.validator.addMethod("condicionarchivo",
    function (value, element, params) {
        var checkvalor = $('input[type=checkbox]#Agregar_' + params[0]).is(':checked');
        if (checkvalor) {
            var tipos = params[1].split(',');

            if (element.files.length === 0) {
                return false;
            } else {
                for (var i = 0; i < element.files.length; i++) {
                    var extension = getFileExtension(element.files[i].name);
                    if ($.inArray(extension, tipos) === -1) {
                        return false;
                    }
                }
            }
        }

        return true;
    });

$.validator.unobtrusive.adapters.add('condicionarchivo',
    ['dependencia', 'tipos'],
    function (options) {
        options.rules['condicionarchivo'] = [options.params['dependencia'], options.params['tipos']];
        options.messages['condicionarchivo'] = options.message;
    });

function getFileExtension(fileName) {
    if (/[.]/.exec(fileName)) {
        return /[^.]+$/.exec(fileName)[0].toLowerCase();
    }
    return null;
}