var objWebRoot = { route: "" };
var objUser = { username: "", usersession: "" };
var objConstConfigGeneral = { TipoColeccionN1: $('#TipoColeccionN1').val(), TipoColeccionN2: $("#TipoColeccionN2").val() };
var objVariables = { Tipo: $("#Tipo").val() };

var objListado = { datos: new Array(), actual: new Array(), filtro: new Array(), recordado: new Array() };
var objNuevo = { valor: "" };

$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objUser.username = $("#user").text();
    objUser.usersession = $('#FolioSession').val();

    CargaInicial();
    AgregarNuevoElemento();
    EliminarElemento();
});
function AgregarNuevoElemento() {
    $('#btn-agregar').on("click", function () {
        if (objVariables.Tipo === objConstConfigGeneral.TipoColeccionN2) {
            objNuevo.valor = $("#text-valor").val();

            objListado.filtro = objListado.datos.filter(function (value, index, arr) {
                return value.valor === objNuevo.valor;
            }).length;

            objListado.filtro += (objListado.actual.filter(function (value, index, arr) {
                return value === objNuevo.valor;
            }).length);

            if (objListado.filtro === 0) {
                if (objNuevo.valor.length > 0) {
                    objListado.datos.push({ valor: objNuevo.valor });
                    PintarElementos();
                    $("#text-valor").val("");
                } else {
                    console.log("falta2");
                }
            }
        }
    });
}
function PintarElementos(){
    $("#contiene-listado").empty();
    $.each(objListado.datos, function (key, value) {
        if (objVariables.Tipo === objConstConfigGeneral.TipoColeccionN2){
            $("#contiene-listado").append(
                $('<div class="form-check">'
                    + '<button type="button" class="btn btn-link text-primary btn-borrar" data-index="' + key +'" title="Eliminar"><i class="far fa-trash-alt"></i></button>'
                    + '<input type="hidden" id="Nuevo_' + key + '__Estado" name="Nuevo[' + key + '].Estado" value="True">'
                    + '<input type="hidden" id="Nuevo_' + key + '__Id" name="Nuevo[' + key + '].Id" value="0">'
                    + '<input type="hidden" id="Nuevo_' + key + '__Valor" name="Nuevo[' + key + '].Valor" value="' + value.valor + '" class="valor-nuevo">'
                    + '<label class="form-check-label">' + value.valor + '</label>'
                    + '</div>'
                ));
        }
    });
}
function EliminarElemento() {
    $("#contiene-listado").on("click", ".btn-borrar", function () {
        objListado.datos.splice($(this).data("index"), 1);
        //--
        PintarElementos();
    });
}
function CargaInicial()
{
    objListado.actual = new Array();
    $('#contiene-listado-actual .form-check input.valor-actual').each(function (index, element) {
        objListado.actual.push($(this).val());
    });
    //---
    if (objVariables.Tipo === objConstConfigGeneral.TipoColeccionN2){
        $('#contiene-listado .form-check input.valor-nuevo').each(function (index, element) {
            objListado.recordado = $(this).val();
            objListado.datos.push({ valor: objListado.recordado });
        });
    }
}