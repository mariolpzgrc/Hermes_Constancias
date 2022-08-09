var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objp = { urln: "", metodo: "", datos: {}, regionId : 0, areaId: 0 };
var objAreas = { info: Object.keys(new Object()) };
var objcheck = { valor: false };
var objVal = { tieneLogoInicial: false };

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    //--
    objp.regionId = $('#RegionId').val();
    objp.areaId = $('#AreaId').val();
    //--
    objVal.tieneLogoInicial = $("#Editar_TieneLogo").prop('checked');

    ObtenerAreas();
    ControlAreaPadre();
    ControlActualizarNombre();
    ControlDias();
    ControlLogo();

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

function ControlAreaPadre()
{
    //$("#Editar_AsignarAreaPadre").change(function () {
        
    //    if ($("#Editar_AsignarAreaPadre").is(":checked")) {
    //        $(".contiene-area-padre").removeClass("d-none");
    //    } else {
    //        $(".contiene-area-padre").removeClass("d-none");
    //        $(".contiene-area-padre").addClass("d-none");
    //    }
    //});
}

function ControlActualizarNombre()
{
    $('#Editar_ActualizarNombre').change(function () {
        if ($("input#Editar_ActualizarNombre[type='checkbox']").is(":checked")) {
            $("#Editar_Nombre").removeClass("disabled");
            $("#Editar_Nombre").removeClass("bg-light");
            $("#Editar_Nombre").addClass("disabled");
            $("#Editar_Nombre").addClass("bg-light");

        } else {
            $("#Editar_Nombre").removeClass("bg-light");
            $("#Editar_Nombre").removeClass("disabled");
        }
    });
}

function ObtenerAreas() {
    $('#Editar_RegionId').change(function () {
        if ($('#Editar_RegionId').val() !== "") {

            objp.urln = objWebRoot.route + "api/v1/areas/sinorigen/" + objp.regionId + "/" + objp.areaId;
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
                    $("#Editar_Area_PadreId").empty();
                    $('#Editar_Area_PadreId').append($('<option></option>').val("").html("[Seleccione un área]"));

                    $.each(objAreas.info, function (index, area) {
                        $('#Editar_Area_PadreId').append($('<option></option>').val(area.HER_AreaId).html(area.HER_Nombre));
                    });
                },
                error: function () {
                    console.log("No se puede recuperar la información");
                }
            });
            //--
        }
        else {
            $("#Editar_Area_PadreId").empty();
            $('#Editar_Area_PadreId').append($('<option></option>').val("").html("[Seleccione una región]"));
        }
    });     
}

function ControlLogo() {
    $("#Editar_TieneLogo").change(function () {
        if ($("#Editar_TieneLogo").is(":checked")) {
            $("#contiene-logo-actual").removeClass("d-none");
            $("#contiene-elementos-actualizador").removeClass("d-none");
            //--
            if (objVal.tieneLogoInicial === false) {
                console.log("pasaN2");
                $("#Editar_ActualizarLogo").bootstrapToggle('enable');
                $("#Editar_ActualizarLogo").prop('checked', true).change();
                //$("#Editar_ActualizarLogo").bootstrapToggle('disable');
            }
        } else {
            $("#contiene-logo-actual").removeClass("d-none");
            $("#contiene-logo-actual").addClass("d-none");
            $("#contiene-elementos-actualizador").removeClass("d-none");
            $("#contiene-elementos-actualizador").addClass("d-none");
            //--
            if (objVal.tieneLogoInicial === false) {
                console.log("pasaN3");
                $("#Editar_ActualizarLogo").bootstrapToggle('enable');
                $("#Editar_ActualizarLogo").prop('checked', false).change();
                //$("#Editar_ActualizarLogo").bootstrapToggle('disable');
            }
        }
    });

    $("#Editar_ActualizarLogo").change(function () {
        if ($("#Editar_ActualizarLogo").is(":checked")) {
            $("#contiene-logo").removeClass("d-none");
        } else {
            $("#contiene-logo").removeClass("d-none");
            $("#contiene-logo").addClass("d-none");
        }
    });
}

//-----------------------------------------
//Se agrega el método de evaluación condición
$.validator.addMethod('condicionbolean',
    function (value, element, params) {
        //Valor del campo en el que se depende
        objcheck.valor = $('input[type=checkbox]#Editar_' + params[0]).is(':checked');

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
        var checkvalor = $('input[type=checkbox]#Editar_' + params[0]).is(':checked');
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
//-------------------------------------------
$.validator.addMethod('condicionantebool',
    function (value, element, params) {
        var enbase = $('input#Editar_' + params[0]).val() === "True"? true : false ;
        var tieneLogo = $('input[type=checkbox]#Editar_' + params[1]).is(':checked');
        var actualizarLogo = $('input[type=checkbox]#Editar_' + params[2]).is(':checked');

        if (enbase === false) {
            if (tieneLogo === true) {
                //Valida para volver requeridos estos campos
                if (actualizarLogo === false) {
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        } else {
            return true;
        }
        
    });

$.validator.unobtrusive.adapters.add('condicionantebool',
    ['enbase', 'dependencia', 'actual'],
    function (options) {
        options.rules['condicionantebool'] = [options.params['enbase'], options.params['dependencia'], options.params['actual']];
        options.messages['condicionantebool'] = options.message;
    });