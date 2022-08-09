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
    ControlAgregarUsuarios();
    Precarga();

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

    $('input#Crear_Usuario').on('after:flexdatalist.search', function (event, key, data, match) {

    });
    //
}

function ControlAgregarUsuarios()
{
    /*--------------*Add user array function*--------------*/
    $("#btn-add-users").click(function (event) {
        objusuarioActual.nombre = $('#Crear_Nombre').val();
        objusuarioActual.username = $('#Crear_Usuario').val();
        objusuarioActual.email = $('#Crear_Correo').val();

        if (objusuarioActual.nombre !== "" && objusuarioActual.username !== "" && objusuarioActual.email !== "") {
            //Valida que no exista en la lista
            if (objusuariosEnSeleccion.listado.findIndex(x => x.username === objusuarioActual.username) === -1) {
                //Se guardan los valores
                objusuariosEnSeleccion.listado.push({ nombre: objusuarioActual.nombre, username: objusuarioActual.username, email: objusuarioActual.email });
                //Mostrar
                MostrarEnTabla();
            }

            LimpiarInfo();
        }
    });
    /*--------------***********************--------------*/
}

function Precarga()
{
    objprecarga.conteo = $('#add-content-users').find('tr').length;
    if (objprecarga.conteo > 0)
    {
        //Limpiar el objeto principal
        LimpiarUsuarioActual();

        //Si hay elmentos cargados desde un inicio se agregan al arreglo principal
        $('#add-content-users').find('tr').each(function () {
            objusuarioActual.username = $(this).children(':nth-child(2)').find('Input').val();
            objusuarioActual.nombre = $(this).children(':nth-child(3)').find('Input').val();
            objusuarioActual.email = $(this).children(':nth-child(4)').find('Input').val();

            if (objusuariosEnSeleccion.listado.findIndex(x => x.username === objusuarioActual.username) === -1) {
                //Se guardan los valores
                objusuariosEnSeleccion.listado.push({ nombre: objusuarioActual.nombre, username: objusuarioActual.username, email: objusuarioActual.email });
            }
        });

        //Limpiar el objeto principal
        LimpiarUsuarioActual();
        //Actualiza el total elementos agregados
        $('#Crear_TotalIntegrantes').val(objusuariosEnSeleccion.listado.length);
    }
}

/*******************************************************Clean data function************************************************************************/
function LimpiarInfo()
{
    //Limpia los campos del DOM
    $('#Crear_Nombre').val("");
    $('#Crear_Correo').val("");
    $('#Crear_Usuario').flexdatalist('value', '');
    $('#control-busqueda ul > li.input-container').show();

    LimpiarUsuarioActual();
}

function LimpiarUsuarioActual() {
    //Limpia los valores del objeto objusuarioActual
    objusuarioActual.nombre = "";
    objusuarioActual.username = "";
    objusuarioActual.email = "";
}
/*******************************************************Clean data function************************************************************************/


/*******************************************************Show user array function************************************************************************/
function MostrarEnTabla() {
    //console.log(objusuariosEnSeleccion.listado);
    //Limpia la tabla
    $("#add-content-users > tr").remove();
    //Agrega los elementos guardados en el Arreglo de objetos
    $.each(objusuariosEnSeleccion.listado, function (i, field) {
        $("#add-content-users").append("<tr>" +
            "<td><span>" + (i + 1) + "</span>" + "</td>" +
            "<td><span>" + field.username + "</span>" + "<input type=\"hidden\" data-val=\"true\" data-val-required=\"El dato Usuario es requerido.\" id=\"Crear_Integrantes_" + i + "__Usuario\" name=\"Crear.Integrantes[" + i + "].Usuario\" value=\"" + field.username + "\">" + "</td>" +
            "<td><span>" + field.nombre + "</span>" + "<input type=\"hidden\" data-val=\"true\" data-val-required=\"El dato Nombre es requerido.\" id=\"Crear_Integrantes_" + i + "__Nombre\" name=\"Crear.Integrantes[" + i + "].Nombre\" value=\"" + field.nombre + "\">" + "</td>" +
            "<td><span>" + field.email + "</span>" + "<input type=\"hidden\" data-val=\"true\" data-val-required=\"El dato Correo es requerido.\" id=\"Crear_Integrantes_" + i + "__Correo\" name=\"Crear.Integrantes[" + i + "].Correo\" value=\"" + field.email + "\">" + "</td>" +
            "<td class=\"indicador text-center\">" + '<button class=\"btn btn-link\" onClick="Eliminar(' + "'" + field.username + "'" + "," + i + ')">' + '<i class="far fa-trash-alt font-gray"></i>' + '</button>' + "</td>" +
        "</tr>");
    });
    //Actualiza el total elementos agregados
    $('#Crear_TotalIntegrantes').val(objusuariosEnSeleccion.listado.length);
}
/*******************************************************Show user array function************************************************************************/

/*******************************************************Delete user array function*********************************************************************/

function Eliminar(username, posicion)
{
    //Si hay elementos en el arreglo procede a eliminar
    if (objusuariosEnSeleccion.listado.length > 0) {
        objusuariosEnSeleccion.listado.splice(posicion, 1);
        MostrarEnTabla();
        LimpiarInfo();

        //$("#msg").text('Usuario:' + username + ' se a quitado de la lista');

        //setTimeout(function () {
        //    $("#msg").text("");
        //}, 1500);
	}
}

/************************************************************Validacion personalizada para el listado de integrantes**********************************/
$.validator.setDefaults({ ignore: [] });
//Se agregan el método
$.validator.addMethod('minimoentero',
    function (value, element, params)
    {
        if (parseInt(value) < parseInt(params[0])) {
            return false;
        } else {
            return true;
        }
    });

$.validator.unobtrusive.adapters.add('minimoentero',
    ['valor'],
    function (options) {
        options.rules['minimoentero'] = [options.params['valor']];
        options.messages['minimoentero'] = options.message;
    });
//-----------------------------------------