/**
 * JS
 **/
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objCarpetasMenu = { result: null, urln: "", info: Object.keys(new Object()), carpetasArray: new Array(), subcarpetasArray: new Array(), carpetaId: 0, carpetaTipo: 0, band: false };

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();

    objCarpetasMenu.carpetaId = $('#CarpetaId').val();
    objCarpetasMenu.carpetaTipo = $('#TipoBandeja').val();
    
    ObtenerCarpetasMenu();
    ControlCarpetasMenu();

    return false;
});

function ObtenerCarpetasMenu()
{
    objCarpetasMenu.urln = objWebRoot.route + 'api/v1/carpetas/' + objUser.username;

    $.ajax({
        url: objCarpetasMenu.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            objCarpetasMenu.info = Object.keys(new Object());
            objCarpetasMenu.info = data;
        },
        error: function (xhr, status, error) {
            objCarpetasMenu.info = Object.keys(new Object());
        },
        complete: function (xhr, status) {
            ProcesaCarpetasMenu();
        }
    });
}
function ControlCarpetasMenu()
{
    if (typeof (objCarpetasMenu.carpetaId) !== "undefined" && typeof (objCarpetasMenu.carpetaTipo) !== "undefined")
    {
        objCarpetasMenu.carpetaId = parseInt($('#CarpetaId').val());
        objCarpetasMenu.carpetaTipo = parseInt($('#TipoBandeja').val());
        //--
        objCarpetasMenu.band = true;
    } else {
        objCarpetasMenu.band = false;
    }
}
function ProcesaCarpetasMenu()
{
    objCarpetasMenu.carpetasArray = new Array();
    objCarpetasMenu.subcarpetasArray = new Array();
    //--
    $.each(objCarpetasMenu.info, function (index, valor) {
        if (valor.Subcarpetas.length > 0) {
            objCarpetasMenu.subcarpetasArray = new Array();
            //Subcarpetas
            $.each(valor.Subcarpetas, function (ind, val) {
                objCarpetasMenu.subcarpetasArray.push({
                    'text': val.Nombre,
                    'state':
                    {
                        'opened': objCarpetasMenu.carpetaId === val.SubcarpetaId,
                        'selected': objCarpetasMenu.carpetaId === val.SubcarpetaId
                    },
                    'icon': 'far fa-circle',
                    "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + val.SubcarpetaId + "/" + 1 }
                });
            });
            //Carpetas
            objCarpetasMenu.carpetasArray.push({
                'text': valor.Nombre,
                'state':
                {
                    'opened': false,
                    'selected': objCarpetasMenu.carpetaId === valor.CarpetaId
                },
                'icon': 'fas fa-circle',
                "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + valor.CarpetaId + "/" + 1 },
                'children': objCarpetasMenu.subcarpetasArray
            });
        } else {
            //Carpetas
            objCarpetasMenu.carpetasArray.push({
                'text': valor.Nombre,
                'state':
                {
                    'opened': false,
                    'selected': objCarpetasMenu.carpetaId === valor.CarpetaId
                },
                'icon': 'fas fa-circle',
                "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + valor.CarpetaId + "/" + 1 }
            });
        }
    });

    objCarpetasMenu.result = $('#contiene-arbol-carpetas').jstree({
        'core': {
            'strings': { 'Loading ...': 'Cargando ...' },
            'data': objCarpetasMenu.carpetasArray
        }
    }).bind("select_node.jstree", function (e, data) {
        document.location.href = data.node.a_attr.href;
    });
}