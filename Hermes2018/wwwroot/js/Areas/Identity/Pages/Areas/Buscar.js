var objWebRoot = { route: "", token: "" };

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();

    ControlAutocompletado();

    return false;
});

function ControlAutocompletado() {
    $('input#Buscar').flexdatalist({
        cache: false,
        multiple: true,
        minLength: 3,
        searchContain: false,
        searchByWord: true,
        maxShownResults: 10,
        selectionRequired: true,
        focusFirstResult: true,
        searchIn: ["Nombre", "Clave", "Direccion"],
        searchDelay: 400,
        noResultsText: 'No se han encontrado resultados para "{keyword}"',
        visibleProperties: ["Nombre"],
        textProperty: "{Nombre}",
        valueProperty: "Nombre",
        removeOnBackspace: true,
        valuesSeparator: ',',
        limitOfValues: 1,
        url: objWebRoot.route + "api/v1/areas/buscar",
        requestHeaders: { 'Authorization': "Bearer " + objWebRoot.token }
    });
    $('input#Buscar').on('select:flexdatalist', function (event, set, options) {
        $('#Nombre').text(set.Nombre);
        $('#Clave').text(set.Clave);
        $('#DiasCompromiso').text(set.DiasCompromiso);
        $('#Direccion').text(set.Direccion);
        $('#Telefono').text(set.Telefono);
        $('#Region').text(set.Region);
        $('#AreaPadre').text(set.AreaPadre);

        $('.lbl-info').removeClass("d-none");
    });
    $('input#Buscar').on('after:flexdatalist.remove', function (event, set, options) {
        $('#control-busqueda ul > li.input-container').show();
        $('#Nombre').text('');
        $('#Clave').text('');
        $('#DiasCompromiso').text('');
        $('#Direccion').text('');
        $('#Telefono').text('');
        $('#Region').text('');
        $('#AreaPadre').text('');

        $('.lbl-info').removeClass("d-none");
        $('.lbl-info').addClass("d-none");
    });
}