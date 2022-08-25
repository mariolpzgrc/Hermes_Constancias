/**
 * JS
 **/
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objCarpetasMenu = { result: null, urln: "", info: Object.keys(new Object()), carpetasArray: new Array(), subcarpetasArray: new Array(), subcarpetasN3Array: new Array(), subcarpetasN4Array: new Array(), subcarpetasN5Array: new Array(), carpetaId: 0, carpetaTipo: 0, band: false };

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

function ObtenerCarpetasMenu() {
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
function ControlCarpetasMenu() {
    if (typeof (objCarpetasMenu.carpetaId) !== "undefined" && typeof (objCarpetasMenu.carpetaTipo) !== "undefined") {
        objCarpetasMenu.carpetaId = parseInt($('#CarpetaId').val());
        objCarpetasMenu.carpetaTipo = parseInt($('#TipoBandeja').val());
        //--
        objCarpetasMenu.band = true;
    } else {
        objCarpetasMenu.band = false;
    }
}
function ProcesaCarpetasMenu() {
    objCarpetasMenu.carpetasArray = new Array();
    objCarpetasMenu.subcarpetasArray = new Array();
    //--
    $.each(objCarpetasMenu.info, function (index, valor) {
        if (valor.Subcarpetas.length > 0) {
            objCarpetasMenu.subcarpetasArray = new Array();
            //Subcarpetas N2
            $.each(valor.Subcarpetas, function (ind, val) {
                if (val.Subcarpetas.length > 0) {
                    objCarpetasMenu.subcarpetasN3Array = new Array();
                    //Subcarpetas N3
                    $.each(val.Subcarpetas, function (ind3, val3) {
                        if (val3.Subcarpetas.length > 0) {
                            objCarpetasMenu.subcarpetasN4Array = new Array();
                            //Subcarpetas N4
                            $.each(val3.Subcarpetas, function (ind4, val4) {
                                if (val4.Subcarpetas.length > 0) {
                                    objCarpetasMenu.subcarpetasN5Array = new Array();
                                    //Subcarpetas N5
                                    $.each(val4.Subcarpetas, function (ind5, val5) {
                                        objCarpetasMenu.subcarpetasN5Array.push({
                                            'text': val5.Nombre,
                                            'state':
                                            {
                                                'opened': objCarpetasMenu.carpetaId === val5.SubcarpetaId,
                                                'selected': objCarpetasMenu.carpetaId === val5.SubcarpetaId
                                            },
                                            'icon': 'far fa-circle',
                                            "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + val5.SubcarpetaId + "/" + 1 }
                                        });
                                    });
                                    objCarpetasMenu.subcarpetasN4Array.push({
                                        'text': val4.Nombre,
                                        'state':
                                        {
                                            'opened': objCarpetasMenu.carpetaId === val4.SubcarpetaId,
                                            'selected': objCarpetasMenu.carpetaId === val4.SubcarpetaId
                                        },
                                        'icon': 'far fa-circle',
                                        "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + val4.SubcarpetaId + "/" + 1 },
                                         "children": objCarpetasMenu.subcarpetasN5Array
                                    });
                                }
                                else {
                                    objCarpetasMenu.subcarpetasN4Array.push({
                                        'text': val4.Nombre,
                                        'state':
                                        {
                                            'opened': false,
                                            'selected': objCarpetasMenu.carpetaId === val4.SubcarpetaId
                                        },
                                        'icon': 'far fa-circle',
                                        "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + val4.SubcarpetaId + "/" + 1 }
                                    });
                                }
                            });
                            objCarpetasMenu.subcarpetasN3Array.push({
                                'text': val3.Nombre,
                                'state':
                                {
                                    'opened': objCarpetasMenu.carpetaId === val3.SubcarpetaId,
                                    'selected': objCarpetasMenu.carpetaId === val3.SubcarpetaId
                                },
                                'icon': 'far fa-circle',
                                "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + val3.SubcarpetaId + "/" + 1 },
                                "children": objCarpetasMenu.subcarpetasN4Array
                            });
                        }
                        else {
                            objCarpetasMenu.subcarpetasN3Array.push({
                                'text': val3.Nombre,
                                'state':
                                {
                                    'opened': false,
                                    'selected': objCarpetasMenu.carpetaId === val3.SubcarpetaId
                                },
                                'icon': 'far fa-circle',
                                "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + val3.SubcarpetaId + "/" + 1 },
                            });
                        }
                    });
                    objCarpetasMenu.subcarpetasArray.push({
                        'text': val.Nombre,
                        'state':
                        {
                            'opened': objCarpetasMenu.carpetaId === val.SubcarpetaId,
                            'selected': objCarpetasMenu.carpetaId === val.SubcarpetaId
                        },
                        'icon': 'far fa-circle',
                        "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + val.SubcarpetaId + "/" + 1 },
                        'children': objCarpetasMenu.subcarpetasN3Array
                    });
                } else {
                    //SubCarpetas de primer nivel
                    objCarpetasMenu.subcarpetasArray.push({
                        'text': val.Nombre,
                        'state':
                        {
                            'opened': false,
                            'selected': objCarpetasMenu.carpetaId === val.SubcarpetaId
                        },
                        'icon': 'fas fa-circle',
                        "a_attr": { "href": objWebRoot.route + 'Bandejas/Personales/' + val.SubcarpetaId + "/" + 1 }
                    });
                }

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