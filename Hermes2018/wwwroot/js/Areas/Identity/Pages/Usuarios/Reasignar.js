var objp = { urln: "", metodo: "", datos: {} };
var objr = { id: "" };
var objs = { arrRolUsuario: new Array("Titular", "Usuario") };
var objTipoHttp = { get: 1, post: 2 };

var objrol = { valor: "" };
var objcheck = { valor: false };
var objAutocompletado = { urln: "", usuario: "" };
var objInfoUsuario = { userName: "", fullName: "", email: "" };
//--
var objAutocompletadoTitular = { urln: "", usuario: "" };
var objInfoUsuarioTitular = { userName: "", fullName: "", email: "" };
//--
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };

var objAreas = { info: Object.keys(new Object()), indexSeleccionado: 0, objSeleccionado: Object.keys(new Object()) };
var objArea = { id: 0, actual: Object.keys(new Object()) };

//FUNCION PRINCIPAL
$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#userLogueado").text();

    ObtenerAreas();
    ControlArea();
    ControlAutocompletado();
    ControlAutocompletadoTitular();
    ControlEsTitular();

    $('input#Reasignar_Nombre').val('');
    $('input#Reasignar_NombreUsuario').val('');
    $('input#Reasignar_Correo').val('');
    return false;
});

function ObtenerAreas() {
    $('#Reasignar_RegionId').change(function () {
        if ($('#Reasignar_RegionId').val() !== "") {

            objr.id = $('#Reasignar_RegionId').val();
            objp.urln = objWebRoot.route + "api/v1/areas/" + objUser.username + "/" + objr.id;
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
                    //---
                    $("#Reasignar_AreaId").empty();
                    $('#Reasignar_AreaId').append($('<option></option>').val("").html("[Seleccione un área]"));

                    $.each(objAreas.info, function (index, area) {
                        $('#Reasignar_AreaId').append($('<option></option>').val(area.HER_AreaId).html(area.HER_Nombre));
                    });
                },
                error: function () {
                    console.log("No se puede recuperar la información");
                }
            });
            //--
        }
        else {
            $("#Reasignar_AreaId").empty();
            $('#Reasignar_AreaId').append($('<option></option>').val("").html("[Seleccione una región]"));
        }
    });
}
function ControlArea() {
    $('#Reasignar_AreaId').change(function () {
        if ($('#Reasignar_AreaId').val() !== "") {
            objArea.id = $('#Reasignar_AreaId').val();
            objArea.actual = objAreas.info.find(x => x.HER_AreaId == objArea.id);

            $("#Reasignar_Direccion").val(objArea.actual.HER_Direccion);
            $("#Reasignar_Telefono").val(objArea.actual.HER_Telefono);
        } else {
            $("#Reasignar_Direccion").val("");
            $("#Reasignar_Telefono").val("");
        }
    });
}
function ControlAutocompletado() {

    $('input#Reasignar_NombreUsuario').flexdatalist({
        cache: false,
        multiple: true,
        minLength: 3,
        searchContain: false,
        searchByWord: true,
        maxShownResults: 20,
        selectionRequired: true,
        focusFirstResult: true,
        searchIn: ["HER_NombreCompleto", "HER_UserName"],
        searchDelay: 400,
        noResultsText: 'No se han encontrado resultados para "{keyword}"',
        visibleProperties: ["HER_NombreCompleto"],
        textProperty: "{HER_UserName}",
        valueProperty: "HER_UserName",
        removeOnBackspace: true,
        valuesSeparator: ',',
        limitOfValues: 1,
        url: objWebRoot.route + "api/v1/users/reasignacion",
        requestHeaders: { 'Authorization': "Bearer " + objWebRoot.token }
    });

    $('input#Reasignar_NombreUsuario').on('select:flexdatalist', function (event, set, options) {
        $('#Reasignar_Nombre').val(set.HER_NombreCompleto);
        $('#Reasignar_Correo').val(set.HER_Correo);
    });

    $('input#Reasignar_NombreUsuario').on('after:flexdatalist.remove', function (event, set, options) {
        $('#control-busqueda ul > li.input-container').show();
        $('#Reasignar_Nombre').val('');
        $('#Reasignar_Correo').val('');
    });

}
function ControlAutocompletadoTitular() {

    $('input#Reasignar_Titular').flexdatalist({
        cache: false,
        multiple: true,
        minLength: 3,
        searchContain: false,
        searchByWord: true,
        maxShownResults: 20,
        selectionRequired: true,
        focusFirstResult: true,
        searchIn: ["HER_Usuario_NombreCompleto", "HER_Usuario_Username"],
        searchDelay: 400,
        noResultsText: 'No se han encontrado resultados para "{keyword}"',
        visibleProperties: ["HER_Usuario_NombreCompleto"],
        textProperty: "{HER_Usuario_Username}",
        valueProperty: "HER_Usuario_Username",
        removeOnBackspace: true,
        valuesSeparator: ',',
        limitOfValues: 1,
        url: objWebRoot.route + "api/v1/users/listactive",
        requestHeaders: { 'Authorization': "Bearer " + objWebRoot.token }
    });

    $('input#Reasignar_Titular').on('select:flexdatalist', function (event, set, options) {
        $('#Reasignar_Nombre').val(set.HER_Usuario_NombreCompleto);
        $('#Reasignar_Correo').val(set.HER_Usuario_Correo);
    });

    $('input#Reasignar_Titular').on('after:flexdatalist.remove', function (event, set, options) {
        $('#contiene-titular ul > li.input-container').show();
        $('#Reasignar_Nombre').val('');
        $('#Reasignar_Correo').val('');
    });

    //var options = {
    //    url: function (phrase) {

    //        objAutocompletadoTitular.usuario = phrase;
    //        objAutocompletadoTitular.urln = objWebRoot.route + "api/v1/users/ldap/" + objAutocompletadoTitular.usuario;

    //        return objAutocompletadoTitular.urln;
    //    },
    //    getValue: "HER_Usuario_Username",
    //    list: {
    //        onSelectItemEvent: function () {
    //            objInfoUsuarioTitular.fullName = $("#Titular").getSelectedItemData().HER_Usuario_NombreCompleto;
    //            objInfoUsuarioTitular.email = $("#Titular").getSelectedItemData().HER_Usuario_Correo;
    //            objInfoUsuarioTitular.userName = $("#Titular").getSelectedItemData().HER_Usuario_Username;

    //            $("#Nombre").val(objInfoUsuarioTitular.fullName);
    //        },
    //        match: {
    //            enabled: true
    //        }
    //    },
    //    template: {
    //        type: "description",
    //        fields: {
    //            description: "HER_Usuario_NombreCompleto"
    //        }
    //    },
    //    requestDelay: 300,
    //};
    //$('#Titular').easyAutocomplete(options);
}
function ControlEsTitular() {
    $("#Reasignar_EsTitular").change(function () {
        if ($(this).is(":checked")) {
            $("#Reasignar_Titular").val($("#Reasignar_NombreUsuario").val());
            $('#contiene-titular').removeClass("d-none");
            $('#contiene-titular').addClass("d-none");
            $("#Reasignar_Titular").val($("#Reasignar_NombreUsuario").val());
        } else {
            $("#Reasignar_Titular").val("");
            $('#contiene-titular').removeClass("d-none");
        }
    });
}
//-----------------------------------------

//Se agrega el método de evaluación condición
$.validator.addMethod('condicion',
    function (value, element, params) {
        //Valor del campo en el que se depende
        objcheck.valor = $('input[type=checkbox]#Reasignar_' + params[0]).is(':checked');

        if (!objcheck.valor) {
            //Valida para volver requeridos estos campos
            if ($.trim(value).length === 0 || $('input#Reasignar_' + params[1]).val() === value) {
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

$.validator.unobtrusive.adapters.add('condicion',
    ['dependencia', 'referencia'],
    function (options) {
        options.rules['condicion'] = [options.params['dependencia'], options.params['referencia']];
        options.messages['condicion'] = options.message;
    });
//-----------------------------------------