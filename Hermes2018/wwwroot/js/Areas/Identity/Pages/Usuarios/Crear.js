var objp = { urln: "", metodo: "", datos: {} };
var objr = { id: "" };
var objs = {
    arrRolAdmin: new Array("Administrador General", "Administrador Xalapa", "Administrador Veracruz", "Administrador Orizaba-Córdoba", "Administrador Poza Rica-Tuxpan", "Administrador Coatzacoalcos-Minatitlán"),
    arrRolUsuario: new Array("Titular", "Usuario"),
    arrRolDelegado: new Array("Delegado", "Delegado Revisor")
};
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
    ControlRol();
    ControlAutocompletado();
    ControlAutocompletadoTitular();
    ControlEsTitular();

    return false;
});

function ObtenerAreas() {
    $('#Crear_RegionId').change(function () {
        if ($('#Crear_RegionId').val() !== "") {

            objr.id = $('#Crear_RegionId').val();
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
                    //--
                    $("#Crear_AreaId").empty();
                    $('#Crear_AreaId').append($('<option></option>').val("").html("[Seleccione un área]"));

                    $.each(objAreas.info, function (index, area) {
                        $('#Crear_AreaId').append($('<option></option>').val(area.HER_AreaId).html(area.HER_Nombre));
                    });
                },
                error: function () {
                    console.log("No se puede recuperar la información");
                }
            });
            //--
        }
        else {
            $("#Crear_AreaId").empty();
            $('#Crear_AreaId').append($('<option></option>').val("").html("[Seleccione una área]"));
        }
    });
}
function ControlArea() {
    $('#Crear_AreaId').change(function () {
        if ($('#Crear_AreaId').val() !== "") {
            objArea.id = $('#Crear_AreaId').val();
            objArea.actual = objAreas.info.find(x => x.HER_AreaId == objArea.id);

            $("#Crear_Direccion").val(objArea.actual.HER_Direccion);
            $("#Crear_Telefono").val(objArea.actual.HER_Telefono);
        } else {
            $("#Crear_Direccion").val("");
            $("#Crear_Telefono").val("");
        }
    });
}
function ControlRol() {
    $('select#Crear_Rol').change(function () {

        if (objs.arrRolUsuario.includes($(this).val())) {
            //$('#contiene-region, #contiene-area, #contiene-es-titular').removeClass("d-none");
            //--
            if (!$("#Crear_EsTitular").is(":checked")) {
                $('#contiene-titular').removeClass("d-none");
            }
        } else {
            //$('#contiene-region, #contiene-area, #contiene-es-titular').removeClass("d-none");
            //$('#contiene-region, #contiene-area, #contiene-es-titular').addClass("d-none");
            $('#Crear_RegionId, #Crear_AreaId').val("");
            //--
            if (!$("#Crear_EsTitular").is(":checked")) {
                $('#contiene-titular').removeClass("d-none");
                $('#contiene-titular').addClass("d-none");
            }
        }
    });
}
function ControlAutocompletado() {
    $('input#Crear_NombreUsuario').flexdatalist({
        cache: false,
        multiple: true,
        minLength: 3,
        searchContain: false,
        searchByWord: true,
        maxShownResults: 10,
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

    $('input#Crear_NombreUsuario').on('select:flexdatalist', function (event, set, options) {
        $('#Crear_Nombre').val(set.HER_Usuario_NombreCompleto);
        $('#Crear_Correo').val(set.HER_Usuario_Correo);
    });

    $('input#Crear_NombreUsuario').on('after:flexdatalist.remove', function (event, set, options) {
        $('#control-busqueda ul > li.input-container').show();
        $('#Crear_Nombre').val('');
        $('#Crear_Correo').val('');
    });


    //var options = {
    //    url: function (phrase) {

    //        objAutocompletado.usuario = phrase;
    //        objAutocompletado.urln = objWebRoot.route + "api/v1/users/ldap/" + objAutocompletado.usuario;

    //        return objAutocompletado.urln;
    //    },
    //    getValue: "HER_Usuario_Username",
    //    list: {
    //        onSelectItemEvent: function () {
    //            objInfoUsuario.fullName = $("#NombreUsuario").getSelectedItemData().HER_Usuario_NombreCompleto;
    //            objInfoUsuario.email = $("#NombreUsuario").getSelectedItemData().HER_Usuario_Correo;
    //            objInfoUsuario.userName = $("#NombreUsuario").getSelectedItemData().HER_Usuario_Username;

    //            $("#Nombre").val(objInfoUsuario.fullName);
    //            $("#Correo").val(objInfoUsuario.email);
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
    //$('#NombreUsuario').easyAutocomplete(options);
}
function ControlAutocompletadoTitular() {
    $('input#Crear_Titular').flexdatalist({
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

    $('input#Crear_Titular').on('select:flexdatalist', function (event, set, options) {
        $('#Crear_Nombre').val(set.HER_Usuario_NombreCompleto);
        //$('#Correo').val(set.HER_Usuario_Correo);
    });

    $('input#Crear_Titular').on('after:flexdatalist.remove', function (event, set, options) {
        $('#contiene-titular ul > li.input-container').show();
        $('#Crear_Nombre').val('');
        //$('#Correo').val('');
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
    $("#Crear_EsTitular").change(function () {
        if ($(this).is(":checked")) {
            $("#Crear_Titular").val($("#Crear_NombreUsuario").val());
            $('#contiene-titular').removeClass("d-none");
            $('#contiene-titular').addClass("d-none");
            $("#Crear_Titular").val($("#Crear_NombreUsuario").val());
        } else {
            $("#Crear_Titular").val("");
            $('#contiene-titular').removeClass("d-none");
        }
    });
}

//Se agregan el método de evaluación dependiente
$.validator.addMethod('dependiente',
    function (value, element, params) {
        //Valor del campo en el que se depende
        objrol.valor = $('select#Crear_' + params[0]).val();

        //Son necesarios los elementos region, area
        if (objs.arrRolUsuario.includes(objrol.valor)) {
            //Valida si que se seleccione algo para volver requeridos estos campos
            if (value === '') {
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

$.validator.unobtrusive.adapters.add('dependiente',
    ['dependencia'],
    function (options) {
        options.rules['dependiente'] = [options.params['dependencia']];
        options.messages['dependiente'] = options.message;
    });
//-----------------------------------------

//Se agrega el método de evaluación condición
$.validator.addMethod('condicion',
    function (value, element, params) {
        //Valor del campo en el que se depende
        objcheck.valor = $('input[type=checkbox]#Crear_' + params[0]).is(':checked');

        if (!objcheck.valor) {
            //Valida para volver requeridos estos campos
            if ($.trim(value).length === 0 || $('input#Crear_' + params[1]).val() === value) {
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