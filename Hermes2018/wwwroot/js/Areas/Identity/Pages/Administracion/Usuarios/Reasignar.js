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
    objr.id = parseInt($("#usuario_areaId").text());

    ControlAutocompletado();
    ObtenerAreas();
    ControlArea();
    ControlAutocompletadoTitular();
    ControlEsTitular();

    $('input#ViewModel_Nombre').val('');
    $('input#ViewModel_NombreUsuario').val('');
    $('input#ViewModel_Correo').val('');
    return false;
});

function ControlAutocompletado() {

    $('input#ViewModel_NombreUsuario').flexdatalist({
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

    $('input#ViewModel_NombreUsuario').on('select:flexdatalist', function (event, set, options) {
        $('#ViewModel_Nombre').val(set.HER_NombreCompleto);
        $('#ViewModel_Correo').val(set.HER_Correo);
    });

    $('input#ViewModel_NombreUsuario').on('after:flexdatalist.remove', function (event, set, options) {
        $('#control-busqueda ul > li.input-container').show();
        $('#ViewModel_Nombre').val('');
        $('#ViewModel_Correo').val('');
    });

}
function ControlAutocompletadoTitular() {

    $('input#ViewModel_Titular').flexdatalist({
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

    $('input#ViewModel_Titular').on('select:flexdatalist', function (event, set, options) {
        $('#ViewModel_Nombre').val(set.HER_Usuario_NombreCompleto);
        $('#ViewModel_Correo').val(set.HER_Usuario_Correo);
    });

    $('input#ViewModel_Titular').on('after:flexdatalist.remove', function (event, set, options) {
        $('#contiene-titular ul > li.input-container').show();
        $('#ViewModel_Nombre').val('');
        $('#ViewModel_Correo').val('');
    });
}
function ControlEsTitular() {
    $("#ViewModel_EsTitular").change(function () {
        if ($(this).is(":checked")) {
            //$("#ViewModel_Titular").val($("#ViewModel_NombreUsuario").val());
            $('#contiene-titular').removeClass("d-none");
            $('#contiene-titular').addClass("d-none");
            //$("#ViewModel_Titular").val($("#ViewModel_NombreUsuario").val());
        } else {
            //$("#ViewModel_Titular").val("");
            $('#contiene-titular').removeClass("d-none");
        }
    });
}
//-----------------------------------------

//Se agrega el método de evaluación condición
$.validator.addMethod('condicion',
    function (value, element, params) {
        //Valor del campo en el que se depende
        objcheck.valor = $('input[type=checkbox]#ViewModel_' + params[0]).is(':checked');

        if (!objcheck.valor) {
            //Valida para volver requeridos estos campos
            if ($.trim(value).length === 0 || $('input#ViewModel_' + params[1]).val() === value) {
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

function ObtenerAreas() {
    objp.urln = objWebRoot.route + "api/v1/areas/padresConHijas/" + objr.id;
    objp.metodo = 'GET';

    $.ajax({
        url: objp.urln,
        type: objp.metodo,
        contentType: "application/json; charset=utf-8",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (result) {
            objAreas.info = Object.keys(new Object());
            objAreas.info = result;
            $("#ViewModel_AreaId").empty();
            $('#ViewModel_AreaId').append($('<option></option>').val("").html("[Seleccione un área]"));

            $.each(objAreas.info, function (index, area) {
                $('#ViewModel_AreaId').append($('<option></option>').val(area.HER_AreaId).html(area.HER_Nombre));
            });
        },
        error: function () {
            console.log("No se puede recuperar la informacion");
            $("#ViewModel_AreaId").empty();
            $('#ViewModel_AreaId').append($('<option></option>').val("").html("[Seleccione una región]"));
        }
    });
}

function ControlArea() {
    $('#ViewModel_AreaId').change(function () {
        if ($('#ViewModel_AreaId').val() !== "") {
            objArea.id = $('#ViewModel_AreaId').val();
            objArea.actual = objAreas.info.find(x => x.HER_AreaId == objArea.id);

            $("#ViewModel_Direccion").val(objArea.actual.HER_Direccion);
            $("#ViewModel_Telefono").val(objArea.actual.HER_Telefono);
        } else {
            $("#ViewModel_Direccion").val("");
            $("#ViewModel_Telefono").val("");
        }
    });
}