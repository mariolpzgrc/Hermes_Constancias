var objWebRoot = { route: "", token: "" };
var ligaArea;

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();

    ControlAutocompletado();

    return false;
});

function ControlAutocompletado() {
    $('input#Info_HER_UserName').flexdatalist({
        cache: false,
        multiple: true,
        minLength: 3,
        searchContain: false,
        searchByWord: true,
        maxShownResults: 10,
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
        url: objWebRoot.route + "api/v1/users/buscar",
        requestHeaders: { 'Authorization': "Bearer " + objWebRoot.token }
    });
    $('input#Info_HER_UserName').on('select:flexdatalist', function (event, set, options) {
        $('#Info_HER_NombreCompleto').text(set.HER_NombreCompleto);
        $('#Info_HER_Correo').text(set.HER_Correo);
        $('#Info_HER_NombreUsuario').text(set.HER_UserName);
        $('#Info_HER_Tipo').text(set.HER_Tipo);
        $('#Info_HER_Puesto').text(set.HER_Puesto);
        $('#Info_HER_EsUnico').text(set.HER_EsUnico);
        $('#Info_HER_Area').text(set.HER_Area);
        $('#idArea').val(set.HER_LigaArea);
        ligaArea = $('#idArea').val();
        $('#ligaArea').attr('href', objWebRoot.route + ligaArea);
        $('#Info_HER_Region').text(set.HER_Region);
        $('#Info_HER_Aprobado').text(set.HER_Aprobado);
        $('#Info_HER_FechaAprobacion').text(set.HER_FechaAprobacion);
        $('#Info_HER_Titular').text(set.HER_Titular);
        $('#Info_HER_Estado').text(set.HER_Estado);
        $('#Info_HER_AceptoTerminos').text(set.HER_AceptoTerminos);
        $('#Info_HER_PermisoAdministradorArea').text(set.HER_PermisoAdministradorArea);
        $('#Info_HER_FechaTerminos').text(set.HER_FechaTerminos);
        $('#Info_HER_FechaRegistro').text(set.HER_FechaRegistro);
        $('#Info_HER_FechaActualizacion').text(set.HER_FechaActualizacion);
    });
    
    $('input#Info_HER_UserName').on('after:flexdatalist.remove', function (event, set, options) {
        $('#control-busqueda ul > li.input-container').show();
        $('#Info_HER_NombreCompleto').val('');
        $('#Info_HER_Correo').val('');
        $('#Info_HER_NombreUsuario').val('');
        $('#Info_HER_Tipo').val('');
        $('#Info_HER_Puesto').val('');
        $('#Info_HER_EsUnico').val('');
        $('#Info_HER_Area').val('');
        $('#Info_HER_Region').val('');
        $('#Info_HER_Aprobado').val('');
        $('#Info_HER_FechaAprobacion').val('');
        $('#Info_HER_Titular').val('');
        $('#Info_HER_Estado').val('');
        $('#Info_HER_AceptoTerminos').val('');
        $('#Info_HER_PermisoAdministradorArea').val('');
        $('#Info_HER_FechaTerminos').val('');
        $('#Info_HER_FechaRegistro').val('');
        $('#Info_HER_FechaActualizacion').val('');
    });
}