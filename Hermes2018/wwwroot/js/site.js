var objResultado = { nombreCompuesto: "", claves: new Array(), clave: "" };
var tmpFecha = { referencia: new Date(), fecha: "" };
var tmpCookie = { cadena: "", listado: new Array() };

function LeerMenuCookie(nombre)
{
    objResultado.nombreCompuesto = nombre + "=";
    objResultado.claves = document.cookie.split(';');
    objResultado.clave = "";

    for (var i = 0; i < objResultado.claves.length; i++)
    {
        objResultado.clave = objResultado.claves[i];
        while (objResultado.clave.charAt(0) === ' ') objResultado.clave = objResultado.clave.substring(1, objResultado.clave.length);

        if (objResultado.clave.indexOf(objResultado.nombreCompuesto) === 0) {
            return decodeURIComponent(objResultado.clave.substring(objResultado.nombreCompuesto.length, objResultado.clave.length));
        }
    }
    return null;
}
function AgregarMenuCookie(nombre)
{
    tmpFecha.fecha = new Date(tmpFecha.referencia.getFullYear() + 10, tmpFecha.referencia.getMonth(), tmpFecha.referencia.getDate()).toUTCString();
    tmpCookie.cadena = nombre + "=yes; expires=" + tmpFecha.fecha + "; path=/; secure; samesite=strict;";
    //--
    document.cookie = tmpCookie.cadena;
}
function ActivarValorMenuCookie(nombre)
{
    tmpFecha.fecha = new Date(tmpFecha.referencia.getFullYear() + 10, tmpFecha.referencia.getMonth(), tmpFecha.referencia.getDate()).toUTCString();
    tmpCookie.cadena = nombre + "=yes; expires=" + tmpFecha.fecha + "; path=/; secure; samesite=strict;";
    //--
    document.cookie = tmpCookie.cadena;
}
function DesactivarValorMenuCookie(nombre) {
    tmpFecha.fecha = new Date(tmpFecha.referencia.getFullYear() + 10, tmpFecha.referencia.getMonth(), tmpFecha.referencia.getDate()).toUTCString();
    tmpCookie.cadena = nombre + "=no; expires=" + tmpFecha.fecha + "; path=/; secure; samesite=strict;";
    //--
    document.cookie = tmpCookie.cadena;
}
function EliminarMenuCookie(nombre) {
    tmpCookie.cadena = nombre + "=; max-age=0;";
    //--
    document.cookie = tmpCookie.cadena;
}

$(document).ready(function () {
    $('[data-val-required]:not(.sin-indicador)').parent().before('<span class="indicador">*</span>');
});