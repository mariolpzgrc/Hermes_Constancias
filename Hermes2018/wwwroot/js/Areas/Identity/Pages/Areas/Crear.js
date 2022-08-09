var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objp = { urln: "", metodo: "", datos: {}, id : "" };
var objAreas = { info: Object.keys(new Object()) };
var objcheck = { valor: false };

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();

    ObtenerAreas();
    //---
    ControlAreaPadre();
    ControlAgregarLogo();
    ControlDias();
});
$.validator.setDefaults({ ignore: '' });

function ControlDias()
{
    $("input#Crear_Dias_Compromiso").TouchSpin({
        initval: "",
        min: 1,
        max: 40,
        step: 1,
        boostat: 5,
        buttondown_class: 'btn',
        buttonup_class: 'btn'
    });
}

function ControlAreaPadre()
{
    $("#Crear_AsignarAreaPadre").change(function () {

        if ($("#Crear_AsignarAreaPadre").is(":checked")) {
            $(".contiene-area-padre").removeClass("d-none");
        } else {
            $(".contiene-area-padre").removeClass("d-none");
            $(".contiene-area-padre").addClass("d-none");
        }
    });
}

function ControlAgregarLogo()
{
    $("#Crear_AgregarLogo").change(function () {

        if ($("#Crear_AgregarLogo").is(":checked")) {
            $(".contiene-logo").removeClass("d-none");
        } else {
            $(".contiene-logo").removeClass("d-none");
            $(".contiene-logo").addClass("d-none");
        }
    });
}

function ObtenerAreas() {
    $('#Crear_RegionId').change(function () {
        if ($('#Crear_RegionId').val() !== "") {

            objp.id = $('#Crear_RegionId').val();
            objp.urln = objWebRoot.route + "api/v1/areas/" + objUser.username + "/" + objp.id;
            objp.metodo = "GET";

            //--
            $.ajax({
                url: objp.urln,
                type: objp.metodo,
                datatype: "application/json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
                },
                success: function (result) {
                    objAreas.info = Object.keys(new Object());
                    objAreas.info = result;
                    //--
                    $("#Crear_Area_PadreId").empty();
                    $('#Crear_Area_PadreId').append($('<option></option>').val("").html("[Seleccione un área]"));

                    $.each(objAreas.info, function (index, area) {
                        $('#Crear_Area_PadreId').append($('<option></option>').val(area.HER_AreaId).html(area.HER_Nombre));
                    });
                },
                error: function () {
                    console.log("No se puede recuperar la información");
                }
            });
            //--
        }
        else {
            $("#Crear_Area_PadreId").empty();
            $('#Crear_Area_PadreId').append($('<option></option>').val("").html("[Seleccione una región]"));
        }
    });     
}

//-----------------------------------------
//Se agrega el método de evaluación condición
$.validator.addMethod('condicionbolean',
    function (value, element, params) {
        //console.log(params);
        //Valor del campo en el que se depende
        objcheck.valor = $('input[type=checkbox]#Crear_' + params[0]).is(':checked');

        if (objcheck.valor) {
            //Valida para volver requeridos estos campos
            if ($.trim(value).length === 0 || value === 0) {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            return true;
        }
    });

$.validator.unobtrusive.adapters.add('condicionbolean',
    ['dependencia'],
    function (options) {
        options.rules['condicionbolean'] = [options.params['dependencia']];
        options.messages['condicionbolean'] = options.message;
    });
//-----------------------------------------
$.validator.addMethod("condicionarchivo",
    function (value, element, params) {
        var checkvalor = $('input[type=checkbox]#Crear_' + params[0]).is(':checked');
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