/******************************************************
*                     Global var                      *
******************************************************/
var userAdd = [];
var objusuarioBusqueda = { nombre: "", username: "", email: ""};
var objusuarioActual = { nombre: "", username: "", email: "" };
var objusuariosEnSeleccion = { listado: new Array()};
var objprecarga = { conteo: 0 };
//--
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
/*************************************************************Main function***********************************************************************/
//FUNCION PRINCIPAL
$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();

    ControlAutocompletado();
    return false;
});
/**************************************************************************************************************************************************/

function ControlAutocompletado()
{
    $('input#Agregar_Usuario').flexdatalist({
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
        url: objWebRoot.route + "api/v1/users/busqueda/local/" + objUser.username,
        requestHeaders: { 'Authorization': "Bearer " + objWebRoot.token }
    });

    $('input#Agregar_Usuario').on('select:flexdatalist', function (event, set, options) {
        $('#Agregar_Nombre').val(set.HER_NombreCompleto);
        $('#Agregar_Correo').val(set.HER_Correo);
    });

    $('input#Agregar_Usuario').on('after:flexdatalist.remove', function (event, set, options) {
        $('#control-busqueda ul > li.input-container').show();
        $('#Agregar_Nombre').val('');
        $('#Agregar_Correo').val('');
    });

    $('input#Agregar_Usuario').on('after:flexdatalist.search', function (event, key, data, match) {

    });
    //
}