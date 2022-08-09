/******************************************************
*                     Global var                      *
******************************************************/
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };

/*************************************************************Main function***********************************************************************/
//FUNCION PRINCIPAL
$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#userLogueado").text();

    ControlAutocompletado();

    return false;
});
/**************************************************************************************************************************************************/

function ControlAutocompletado()
{
    $('input#Crear_Usuario').flexdatalist({
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

    $('input#Crear_Usuario').on('select:flexdatalist', function (event, set, options) {
        $('#Crear_Nombre').val(set.HER_NombreCompleto);
        $('#Crear_Correo').val(set.HER_Correo);
    });

    $('input#Crear_Usuario').on('after:flexdatalist.remove', function (event, set, options) {
        $('#control-busqueda ul > li.input-container').show();
        $('#Crear_Nombre').val('');
        $('#Crear_Correo').val('');
    });
}