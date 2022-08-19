/****************************************************************************************************************************************
*                                                   Start script datatable enviados                                                     *
*****************************************************************************************************************************************/
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4 };

var objEnviados = { urln: "", info: Object.keys(new Object()), exportar: new Array(), descarga: Object.keys(new Object()), temporalId: -1, auxiliar: -1, temporalBool: false, caracteristicas: { merges: new Array(), cols: new Array(), rows: new Array() }  };
var objtr = { elementobase: Object.keys(new Object()) };
var objSeleccion = { elementos: Object.keys(new Object()), indice: 0, total: 0 };
var objElementos = { elementospaginaactual: 0 };
var objBusqueda = { cadena: '', seBusco: false, inCarpeta: false };
var objPaginacion = {
    opciones: {
        totalPages: 1,
        visiblePages: 3,
        startPage: 1,
        initiateStartPageClick: false,
        pageVariable: '{{pagina}}',
        first: 'Primero',
        prev: 'Anterior',
        next: 'Siguiente',
        last: 'Último',
        onPageClick: function (event, page) {
            ObtenerBandeja(page);
        }
    }
};

var objCategorias = { urln: "", info: Object.keys(new Object()), categoriaId: 0 };
var objTramites = { urln: "", info: Object.keys(new Object()), tramiteId: 0 };
var objFechas = { inicio: '', fin: '', seBusco: false };

var objCarpetas = { urln: "", info: Object.keys(new Object()) };
var objEstados = { urln: "", info: Object.keys(new Object()), estadoId: 0 };
var objTipos = { urln: "", info: Object.keys(new Object()), tipoId: 0 };
var objBandejas = { Enviados: 2 };
var objDelegar = { estaActiva: false, tipoPermiso: 0 };
var objEstado = {
    clase: new Array(
        'font-yellow',
        'font-green',
        'font-orange',
        'font-red',
        'font-orange',
        'font-green',
        'font-gray'
    )
};

$(document).ready(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    //--
    ControlBusquedaEnCarpetas();
    ObtenerBandeja(1);
    ControlSeleccion();
    ControlBusquedaEnCarpetas();
    ControlBusqueda();
    //--
    ObtenerEstados();
    ObtenerTipos();
    ObtenerCategorias();
    ObtenerTramites();
    ControlFechas();
    ControlExportar();
    //--
    if (objDelegar.estaActiva === false || (objDelegar.estaActiva === true && objDelegar.tipoPermiso === 1)) {
        ObtenerCarpetas();
        ControlCarpetas();
    }
    //--
    ControlVisualizacion();
    //--
});

function ObtenerBandeja(pagina) {

         if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
         }
    else if (objTramites.tramiteId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
             objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?'
                 + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
                 + 'enCarpeta=' + objBusqueda.inCarpeta;
         }
    else {
             objEnviados.urln = objWebRoot.route + 'api/v1/documentos/enviados/' + objUser.username + '/' + pagina + '?' + 'enCarpeta=' + objBusqueda.inCarpeta;
    }

    //--
    $.ajax({
        url: objEnviados.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objEnviados.info = Object.keys(new Object());

            $('#contiene-cargador .loader').removeClass('d-none');
            $('#Enviados tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            objEnviados.info = Object.keys(new Object());
            objEnviados.info = data;
        },
        error: function (xhr, status, error) {
            objEnviados.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if (objBusqueda.inCarpeta) {
                $('#carpetas').text("Carpeta");
            }

            if ((objEnviados.info.Datos).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objEnviados.info.Total_Paginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-enviados').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-enviados').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-enviados').twbsPagination('destroy');
            }

            ProcesaBandeja();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#Enviados tbody').fadeIn(500, "linear");
        }
    });
    //--
}

function ProcesaBandeja() {
    $('#Enviados tbody').empty();
    //--
    if ((objEnviados.info.Datos).length > 0) {

        //Elementos de la página actual
        objElementos.elementospaginaactual = objEnviados.info.Elementos_Pagina_Actual;

        $.each(objEnviados.info.Datos, function (index, value) {
            objtr.elementobase = Object.keys(new Object());
            objSeleccion.indice = objSeleccion.elementos.findIndex(x => x.Id === parseInt(value.EnvioId) && x.Tipo === parseInt(value.TipoEnvio));

            objtr.elementobase = $('<tr>').append(
                $('<th scope="row">').append(
                    $('<input type="checkbox" class="checkbox_id" value="' + value.EnvioId + '" data-tipo="' + value.TipoEnvio + '">').prop('checked', (objSeleccion.indice === -1) ? false : true)
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Folio + '</a>')
                ),
                $('<td>').append(
                    (value.Destinatarios_Extras > 0) ? 
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '" title="'+ value.Area + ' (' + value.Region +')' + '">' + value.Destinatario + ' (+' + value.Destinatarios_Extras + ')' + '</a>')
                    :
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '" title="'+ value.Area + ' (' + value.Region +')' + '">' + value.Destinatario + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Asunto + '</a>')
                ),
                $('<td>').append(
                    (value.NoInterno) ?  $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.NoInterno  + '</a>') : ''
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + moment(value.FechaEnvio).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</a>')
                ),
                $('<td>').append(
                    (value.Carpeta && objBusqueda.inCarpeta) ?
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Carpeta + '</a>') :
                        (objBusqueda.inCarpeta && (objBusqueda.seBusco || objFechas.seBusco)) ?
                            $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">Enviados</a>') : ''
                ),
                $('<td>').append(
                    (value.Importancia) ? $('<div data-toggle="tooltip" data-placement="top" title="Importancia alta"><i class="fas fa-exclamation font-red"></i></div>') : ''
                ),
                $('<td>').append(
                    (value.Adjuntos) ? $('<div data-toggle="tooltip" data-placement="top" title="Archivos adjuntos disponibles"><i class="fas fa-paperclip font-gray"></i></div>') : ''
                ),
                $('<td>').append(
                    (parseInt(value.TipoEnvio) == objTipoEnvio.envio) ?
                        (value.ConFechaCompromiso) ? $('<div data-toggle="tooltip" data-placement="top" title="Se ha registrado la fecha compromiso"><i class="far fa-calendar-check font-gray"></i></div>') : $('<div data-toggle="tooltip" data-placement="top" title="No se ha registrado la fecha compromiso"><i class="far fa-calendar font-gray"></i></div>')
                        : ''
                ),
                $('<td>').append(
                    (value.TipoEnvioParaIcono === objTipoEnvio.envio) ?
                        $('<div data-toggle="tooltip" data-placement="top" title="Doc. Enviado"><i class="fas fa-long-arrow-alt-right font-gray"></i></div>')
                        :
                        (value.TipoEnvioParaIcono === objTipoEnvio.turnar) ?
                            $('<div data-toggle="tooltip" data-placement="top" title="Doc. Turnado"><i class="fas fa-redo-alt font-gray"></i></div>')
                            :
                            (value.TipoEnvioParaIcono === objTipoEnvio.respuestaParcial) ?
                                $('<div data-toggle="tooltip" data-placement="top" title="Respuesta parcial"><i class="fab fa-stack-exchange font-gray"></i></div>')
                                : 
                                (value.TipoEnvioParaIcono === objTipoEnvio.respuesta) ?
                                    $('<div data-toggle="tooltip" data-placement="top" title="Respuesta"><i class="fas fa-exchange-alt font-gray"></i></div>') : ''
                ),
                $('<td>').append(
                    (value.Estado === "No requiere respuesta") ?
                        $('<i class="fas fa-circle font-gray" title="No requiere respuesta"></i>')
                        :
                        (value.Estado === "Contestado parcialmente") ?
                            $('<i class="fas fa-circle font-orange" title="Contestado parcialmente"></i>')
                            :
                            (value.Estado === "Contestado completamente") ?
                                $('<i class="fas fa-circle font-green" title="Contestado completamente"></i>')
                                : ''
                ),
                $('<td>').append(
                    $('<a class="font-dark link-externo" title="Abrir el documento en una ventana externa" href="' + objWebRoot.route + 'Correspondencia/Visualizacion/' + value.EnvioId + '/' + value.TipoEnvio + '/' + objUser.username + '"><i class="fas fa-external-link-alt font-gray"></i></a>')
                )
            ).addClass((objSeleccion.indice === -1) ? '' : 'table-active');

            $('#Enviados tbody').append(objtr.elementobase);
        });

        objSeleccion.total = $('#Enviados tbody tr th input:checked').length;
        //--
        if (objElementos.elementospaginaactual === objSeleccion.total && objElementos.elementospaginaactual > 0) {
            $('#checkbox_all').prop('checked', true);
        }
        else if (objElementos.elementospaginaactual > objSeleccion.total) {
            $('#checkbox_all').prop('checked', false);
        }

    } else {
        objtr.elementobase = Object.keys(new Object());
        objtr.elementobase = $('<tr class="table-light">').append(
            $('<td colspan="13" class="text-center">').text("Sin documentos para mostrar")
        );
        $('#Enviados tbody').append(objtr.elementobase);
    }
}

function ControlSeleccion() {
    $('#checkbox_all').click(function () {
        if ($(this).is(":checked")) {

            $('input.checkbox_id:checkbox').not(this).prop('checked', true).each(function () {
                if ($(this).is(":checked")) {
                    $(this).parent().parent().removeClass('table-active').addClass('table-active');
                    objSeleccion.elementos.push({ Id: parseInt($(this).val()), Tipo: parseInt($(this).data('tipo')) });
                }
            });

        } else {
            $('input.checkbox_id').not(this).prop('checked', false).each(function () {
                if (!$(this).is(":checked")) {
                    $(this).parent().parent().removeClass('table-active');
                    objSeleccion.indice = objSeleccion.elementos.findIndex(x => x.Id === parseInt($(this).val()) && x.Tipo === parseInt($(this).data('tipo')));
                    if (objSeleccion.indice !== -1) {
                        objSeleccion.elementos.splice(objSeleccion.indice, 1);
                    }
                }
            });
        }
        //--
        objSeleccion.total = $('#Enviados tbody tr th input:checked').length;
        //-
        if (objElementos.elementospaginaactual === objSeleccion.total && objElementos.elementospaginaactual > 0) {
            $('#checkbox_all').prop('checked', true);
        }
        else if (objElementos.elementospaginaactual > objSeleccion.total) {
            $('#checkbox_all').prop('checked', false);
        }
    });

    $('tbody').on("click", " tr .checkbox_id", function () {
        if ($(this).is(":checked")) {
            $(this).parent().parent().removeClass('table-active').addClass('table-active');
            objSeleccion.elementos.push({ Id: parseInt($(this).val()), Tipo: parseInt($(this).data('tipo')) });
        }
        else {
            $(this).parent().parent().removeClass('table-active');
            objSeleccion.indice = objSeleccion.elementos.findIndex(x => x.Id === parseInt($(this).val()) && x.Tipo === parseInt($(this).data('tipo')));
            if (objSeleccion.indice !== -1) {
                objSeleccion.elementos.splice(objSeleccion.indice, 1);
            }
        }
        //--
        objSeleccion.total = $('#Enviados tbody tr th input:checked').length;
        //--
        if (objElementos.elementospaginaactual === objSeleccion.total && objElementos.elementospaginaactual > 0) {
            $('#checkbox_all').prop('checked', true);
        }
        else if (objElementos.elementospaginaactual > objSeleccion.total) {
            $('#checkbox_all').prop('checked', false);
        }
    });
}

function ControlBusqueda() {
    jQuery(document).on('keyup contextmenu input', 'input#searchbox', function (ev) {
        objBusqueda.cadena = $(this).val().trim();

        if (ev.which === 13){   
            objBusqueda.cadena = objBusqueda.cadena.replace(/ /g, '+');
            objBusqueda.seBusco = true;

            ObtenerBandeja(1);

            return false;
        }

        if (objBusqueda.cadena.length > 0) {
            $('#btn-x-search').removeClass('d-none');
        } else {
            $('#btn-x-search').removeClass('d-none').addClass('d-none');

            objBusqueda.cadena = "";

            if (objBusqueda.seBusco) {
                objBusqueda.seBusco = false;
                ObtenerBandeja(1);
            }
        }
    });

    $('#btn-x-search').on("click", function () {
        $('input#searchbox').val("");
        $('#btn-x-search').removeClass('d-none').addClass('d-none');

        objBusqueda.cadena = "";

        if (objBusqueda.seBusco) {
            objBusqueda.seBusco = false;
            objBusqueda.inCarpeta = false;
            $('#inCarpetas').prop("checked", false);
            $('#carpetas').text("");
            ObtenerBandeja(1);
        }
    });
}

function ControlBusquedaEnCarpetas() {
    $('#inCarpetas').click(function () {
        if ($(this).is(":checked")) {
            objBusqueda.inCarpeta = true;
        } else {
            objBusqueda.inCarpeta = false;
            objBusqueda.inCarpeta = false;
            $('#carpetas').text("");
        }
    })
}
/*************************/
function ObtenerEstados() {
    objEstados.urln = objWebRoot.route + 'api/v1/estados/bandeja/enviados';
    objBusqueda.seBusco = false;

    //--
    $.ajax({
        url: objEstados.urln,
        type: 'GET',
        dataType: 'json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            objEstados.info = Object.keys(new Object());
            objEstados.info = data;
        },
        error: function (xhr, status, error) {
            objEstados.info = Object.keys(new Object());
        },
        complete: function (xhr, status) {
            if ((objEstados.info).length > 0) {
                $("#select-estado").empty();
                $('#select-estado').append($('<option></option>').val("0").html("[Todos los estados]"));

                $.each(objEstados.info, function (index, estado) {
                    $('#select-estado').append($('<option></option>').val(estado.HER_EstadoEnvioId).html(estado.HER_Nombre).addClass(objEstado.clase[estado.HER_EstadoEnvioId - 1]));
                });
            } else {
                $("#select-estado").empty();
                $('#select-estado').append($('<option></option>').val("0").html("[Todos los estados]"));
            }
        }
    });
    //--
    $("#select-estado").change(function () {
        objEstados.estadoId = parseInt($("#select-estado option:selected").val());
        objBusqueda.seBusco = true;
        ObtenerBandeja(1);
    });
}

function ObtenerTipos() {
    objTipos.urln = objWebRoot.route + 'api/v1/tipos/envio';
    objBusqueda.seBusco = false;
    //--
    $.ajax({
        url: objTipos.urln,
        type: 'GET',
        dataType: 'json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            objTipos.info = Object.keys(new Object());
            objTipos.info = data;
        },
        error: function (xhr, status, error) {
            objTipos.info = Object.keys(new Object());
        },
        complete: function (xhr, status) {
            if ((objTipos.info).length > 0) {
                $("#select-tipo").empty();
                $('#select-tipo').append($('<option></option>').val("0").html("[Todos los tipos]"));

                $.each(objTipos.info, function (index, tipo) {
                    $('#select-tipo').append($('<option></option>').val(tipo.TipoEnvioId).html(tipo.Nombre));
                });
            } else {
                $("#select-tipo").empty();
                $('#select-tipo').append($('<option></option>').val("0").html("[Todos los tipos]"));
            }
        }
    });
    //--
    $("#select-tipo").change(function () {
        objTipos.tipoId = parseInt($("#select-tipo option:selected").val());
        objBusqueda.seBusco = true;
        ObtenerBandeja(1);
    });
}

function ObtenerCategorias() {
    objCategorias.urln = objWebRoot.route + 'api/v1/categorias/' + objUser.username;
    objBusqueda.seBusco = false;

    //--
    $.ajax({
        url: objCategorias.urln,
        type: 'GET',
        dataType: 'json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            objCategorias.info = Object.keys(new Object());
            objCategorias.info = data;
        },
        error: function (xhr, status, error) {
            objCategorias.info = Object.keys(new Object());
        },
        complete: function (xhr, status) {
            if ((objCategorias.info).length > 0) {
                $("#select-categorias").empty();
                $('#select-categorias').append($('<option></option>').val("0").html("[Todas las categorías]"));

                $.each(objCategorias.info, function (index, area) {
                    $('#select-categorias').append($('<option></option>').val(area.CategoriaId).html(area.Nombre));
                });
            } else {
                $("#select-categorias").empty();
                $('#select-categorias').append($('<option></option>').val("0").html("[Todas las categorías]"));
            }
        }
    });
    //--

    $("#select-categorias").change(function () {
        objCategorias.categoriaId = parseInt($("#select-categorias option:selected").val());
        objBusqueda.seBusco = true;
        ObtenerBandeja(1);
    });
}

function ObtenerTramites() {
    objTramites.urln = objWebRoot.route + 'api/v1/tramites';
    objBusqueda.seBusco = false;
    fetch(objTramites.urln, {
        method: 'GET',
        headers: {
            'Authorization': "Bearer " + objWebRoot.token
        }
    })
        .then(response => response.json())
        .then(function (data) {
            objTramites.info = Object.keys(new Object());
            objTramites.info = data;
            if ((objTramites.info).length > 0) {
                $("#select-tramites").empty();
                $('#select-tramites').append($('<option></option>').val("0").html("[Todos los trámites]"));

                $.each(objTramites.info, function (index, tramite) {
                    if (tramite.TramiteId == 1)
                        $('#select-tramites').append($('<option></option>').val(tramite.TramiteId).html(tramite.Nombre));
                    else
                        $('#select-tramites').append($('<option></option>').val(tramite.TramiteId).html(tramite.Nombre + " (" + tramite.Dias + " días)"));
                });
            } else {
                $("#select-tramites").empty();
                $('#select-tramites').append($('<option></option>').val("0").html("[Todos los trámites]"));
            }
        })
        .catch(error => {
            objTramites.info = Object.keys(new Object());
        });

    $("#select-tramites").change(function () {
        objTramites.tramiteId = parseInt($("#select-tramites option:selected").val());
        objBusqueda.seBusco = true;
        ObtenerBandeja(1);
    });
}

function ControlFechas() {

    $('input[name="datepicker1"]').daterangepicker({
        opens: 'left',
        autoUpdateInput: false,
        locale: {
            format: 'DD/MM/YYYY',
            cancelLabel: 'Limpiar',
            applyLabel: "Aplicar",
            fromLabel: "De",
            toLabel: "a",
            daysOfWeek: [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            monthNames: [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ]
        }
    });

    $('input[name="datepicker1"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));

        objFechas.inicio = picker.startDate.format('DD/MM/YYYY');
        objFechas.fin = picker.endDate.format('DD/MM/YYYY');
        //--
        objFechas.seBusco = true;
        ObtenerBandeja(1);
    });

    $('input[name="datepicker1"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');

        objFechas.inicio = '';
        objFechas.fin = '';
        //--
        if (objFechas.seBusco) {
            objFechas.seBusco = false;
            objBusqueda.inCarpeta = false;
            $('#inCarpetas').prop("checked", false);
            $('#carpetas').text("");
            ObtenerBandeja(1);
        }

    });
}

function ControlExportarPDF() {
    objEnviados.exportar = Object.keys(new Object());

    //--Cambia el orden de la lista
    objEnviados.info.Datos.sort(function (a, b) {
        if (a.TramiteId > b.TramiteId || a.FechaEnvio > b.FechaEnvio) {
            return 1;
        }

        if (a.TramiteId < b.TramiteId || a.FechaEnvio < b.FechaEnvio) {
            return -1;
        }

        return 0;
    });
    //--
    $.each(objEnviados.info.Datos, function (index, value) {

        objEnviados.auxiliar = (value.TramiteId == null) ? 0 : value.TramiteId;
        objEnviados.temporalBool = value.Proximo != null;

        if (value.Tramite != null) {
            if (objEnviados.auxiliar != objEnviados.temporalId) {

                objEnviados.exportar.push(
                    new Array({
                        content: "TIPO DE TRÁMITE: " + value.Tramite.toUpperCase(), rowSpan: 0, colSpan: 11, styles: { halign: 'center', valign: 'middle', fillColor: [153, 206, 212] }
                    })
                );
            }
        }

        objEnviados.exportar.push(
            new Array(
                { content: value.Folio, rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: value.NoInterno != null ? value.NoInterno : "", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: moment(value.FechaEnvio).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]'), rowSpan: 0, colSpan: 0 },
                { content: (value.Importancia) ? "Alta" : "Normal", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: (value.Adjuntos) ? "Si" : "No", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: (value.TipoEnvioParaIcono === objTipoEnvio.envio) ? "Envio" : (value.TipoEnvioParaIcono === objTipoEnvio.turnar) ? "Turnado" : (value.TipoEnvioParaIcono === objTipoEnvio.respuestaParcial) ? "Respuesta parcial" : (value.TipoEnvioParaIcono === objTipoEnvio.respuesta) ? "Respuesta" : "", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: (value.RequiereRespuesta) ? "Si" : "No", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: value.Destinatario, rowSpan: 0, colSpan: 0 },
                { content: value.Area, rowSpan: 0, colSpan: 0 },
                { content: value.Region, rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: value.Asunto, rowSpan: 0, colSpan: 0},
            )
        );

        objEnviados.temporalId = (value.TramiteId == null) ? 0 : value.TramiteId;
    });

    objEnviados.descarga = Object.keys(new Object());
    objEnviados.descarga = new jsPDF({
        orientation: "landscape",
        format: 'a4'
    });

    objEnviados.descarga.autoTable({
        head: [
            [
                { content: 'Folio', rowSpan: 2, colSpan: 0 },
                { content: 'Número Of./Memo', rowSpan: 2, colSpan: 0 },
                { content: 'Fecha Envío', rowSpan: 2, colSpan: 0 },
                { content: 'Importancia', rowSpan: 2, colSpan: 0 },
                { content: 'Adjuntos', rowSpan: 2, colSpan: 0 },
                { content: 'Tipo Envío', rowSpan: 2, colSpan: 0 },
                { content: 'Requiere Respuesta', rowSpan: 2, colSpan: 0 },
                { content: 'Atiende', rowSpan: 0, colSpan: 3 },
                { content: 'Asunto', rowSpan: 2, colSpan: 0 }
            ],
            [
                { content: 'Para', rowSpan: 0, colSpan: 0 },
                { content: 'Entidad/Dependencia', rowSpan: 0, colSpan: 0 },
                { content: 'Región', rowSpan: 0, colSpan: 0 }
            ]
        ],
        body: objEnviados.exportar,
        margin: { top: 5, bottom: 5, right: 5, left: 5 },
        theme: 'striped',
        headStyles: {
            fillColor: [0, 131, 143],
            halign: 'center',
            valign: 'middle',
            overflow: 'linebreak',
            cellWidth: 'auto',
            minCellWidth: 18,
            cellPadding: 1,
            lineColor: [13, 71, 161],
            lineWidth: 0.1,
            fontSize: 7
        },
        bodyStyles: {
            cellPadding: 1,
            halign: 'left',
            valign: 'middle',
            overflow: 'linebreak',
            lineColor: 10,
            lineWidth: 0.1,
            fontSize: 7
        },
        styles: {
            font: 'helvetica',
            fontStyle: 'normal'
        }
    });

    objEnviados.descarga.save('ListadoEnviados.pdf');
}

function ControlExportarExcel() {
    objEnviados.exportar = Object.keys(new Object());

    //--Cambia el orden de la lista
    objEnviados.info.Datos.sort(function (a, b) {
        if (a.TramiteId > b.TramiteId || a.FechaEnvio > b.FechaEnvio) {
            return 1;
        }

        if (a.TramiteId < b.TramiteId || a.FechaEnvio < b.FechaEnvio) {
            return -1;
        }

        return 0;
    });
    //--
    objEnviados.exportar.push(
        {
            "Folio": "Folio",
            "Número Of./Memo": "Número Of./Memo",
            "Fecha Envío": "Fecha Envío",
            "Importancia": "Importancia",
            "Adjuntos": "Adjuntos",
            "Tipo Envío": "Tipo Envío",
            "Requiere Respuesta": "Requiere Respuesta",
            "Para": "Atiende",
            "Entidad/Dependencia": "",
            "Región": "",
            "Asunto": "Asunto"
        },
        {
            "Folio": "",
            "Número Of./Memo": "",
            "Fecha Envío": "",
            "Importancia": "",
            "Adjuntos": "",
            "Tipo Envío": "",
            "Requiere Respuesta": "",
            "Para": "Para",
            "Entidad/Dependencia": "Entidad/Dependencia",
            "Región": "Región",
            "Asunto": ""
        }
    );
    //--
    objEnviados.caracteristicas.merges.push(
        { s: { r: 0, c: 7 }, e: { r: 0, c: 9 } },
        { s: { r: 0, c: 10 }, e: { r: 1, c: 10 } },
        { s: { r: 0, c: 6 }, e: { r: 1, c: 6 } },
        { s: { r: 0, c: 5 }, e: { r: 1, c: 5 } },
        { s: { r: 0, c: 4 }, e: { r: 1, c: 4 } },
        { s: { r: 0, c: 3 }, e: { r: 1, c: 3 } },
        { s: { r: 0, c: 2 }, e: { r: 1, c: 2 } },
        { s: { r: 0, c: 1 }, e: { r: 1, c: 1 } },
        { s: { r: 0, c: 0 }, e: { r: 1, c: 0 } }
    );
    //--
    $.each(objEnviados.info.Datos, function (index, value) {

        objEnviados.auxiliar = (value.TramiteId == null) ? 0 : value.TramiteId;
        objEnviados.temporalBool = value.Proximo != null;

        if (value.Tramite != null) {
            if (objEnviados.auxiliar != objEnviados.temporalId) {

                objEnviados.exportar.push(
                    { "Folio": "TIPO DE TRÁMITE: " + value.Tramite.toUpperCase(), "Número Of./Memo": "", "Fecha Envío": "", "Importancia": "", "Adjuntos": "", "Tipo Envío": "", "Requiere Respuesta": "", "Para": "", "Entidad/Dependencia": "", "Región": "", "Asunto": "" }
                );

                objEnviados.caracteristicas.merges.push(
                    { s: { r: (objEnviados.exportar.length - 1), c: 0 }, e: { r: (objEnviados.exportar.length - 1), c: 10 } }
                );
            }
        }

        objEnviados.exportar.push({
            "Folio": value.Folio,
            "Número Of./Memo": value.NoInterno != null ? value.NoInterno : "",
            "Fecha Envío": moment(value.FechaEnvio).locale('es').format('DD[/]MM[/]YYYY'),
            "Importancia": (value.Importancia) ? "Alta" : "Normal",
            "Adjuntos": (value.Adjuntos) ? "Si" : "No",
            "Tipo Envío": (value.TipoEnvioParaIcono === objTipoEnvio.envio) ? "Envio" : (value.TipoEnvioParaIcono === objTipoEnvio.turnar) ? "Turnado" : (value.TipoEnvioParaIcono === objTipoEnvio.respuestaParcial) ? "Respuesta parcial" : (value.TipoEnvioParaIcono === objTipoEnvio.respuesta) ? "Respuesta" : "",
            "Requiere Respuesta": (value.RequiereRespuesta) ? "Si" : "No",
            "Para": value.Destinatario,
            "Entidad/Dependencia": value.Area,
            "Región": value.Region,
            "Asunto": value.Asunto
        });

        objEnviados.temporalId = (value.TramiteId == null) ? 0 : value.TramiteId;
    });

    //---
    var wb = XLSX.utils.book_new();
    var ws = XLSX.utils.json_to_sheet(objEnviados.exportar, { skipHeader: true });
    wb.Props = {
        Title: "Documentos Enviados",
        Subject: "HERMES",
        Author: "UV",
    };
    wb.SheetNames.push("Enviados (" + objUser.username + ")");
    ws['!merges'] = objEnviados.caracteristicas.merges;
    wb.Sheets["Enviados (" + objUser.username + ")"] = ws;

    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length); 
        var view = new Uint8Array(buf);  
        for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'ListadoEnviados.xlsx');
}

function ControlExportar() {
    $('#exportar-pdf').click(function () {
        ControlExportarPDF();
    });

    $('#exportar-excel').click(function () {
        ControlExportarExcel();
    });
}
/**************************/

function ObtenerCarpetas() {
    objCarpetas.urln = objWebRoot.route + 'api/v1/carpetas/' + objUser.username;

    //--
    $.ajax({
        url: objCarpetas.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            objCarpetas.info = Object.keys(new Object());
            objCarpetas.info = data;
        },
        error: function (xhr, status, error) {
            objCarpetas.info = Object.keys(new Object());
        },
        complete: function (xhr, status) {
            ProcesaCarpetas();
        }
    });
    //--
}

function ProcesaCarpetas() {
    $.each(objCarpetas.info, function (index, value) {
        $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 bg-light" data-id="' + value.CarpetaId + '"><div>' + value.Nombre + '</div></button>'));
        $('#contiene-carpetas').append($('<div class="dropdown-divider sin-margen"></div>'));

        $.each(value.Subcarpetas, function (i, v) {
            $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 con-padding-l-14 font-dark-gray" data-id="' + v.SubcarpetaId + '"><div>' + v.Nombre + '</div></button>'));
            $('#contiene-carpetas').append($('<div class="dropdown-divider sin-margen"></div>'));

            $.each(v.Subcarpetas, function (i3, v3) {
                $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 con-padding-l-22 font-dark-gray" data-id="' + v3.SubcarpetaId + '"><div>' + v3.Nombre + '</div></button>'));
                $('#contiene-carpetas').append($('<div class="dropdown-divider sin-margen"></div>'));

                $.each(v3.Subcarpetas, function (i4, v4) {
                    $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 con-padding-l-30 font-dark-gray" data-id="' + v4.SubcarpetaId + '"><div>' + v4.Nombre + '</div></button>'));
                    $('#contiene-carpetas').append($('<div class="dropdown-divider sin-margen"></div>'));

                    $.each(v4.Subcarpetas, function (i5, v5) {
                        $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 con-padding-l-38 font-dark-gray" data-id="' + v5.SubcarpetaId + '"><div>' + v5.Nombre + '</div></button>'));
                        $('#contiene-carpetas').append($('<div class="dropdown-divider sin-margen"></div>'));
                    });
                });
            });
        });
    });
}

function ControlCarpetas() {
    $('#contiene-carpetas').on("click", "button.carpeta-seleccion", function ()
    {
        if (objSeleccion.elementos.length > 0)
        {
            //--
            $.ajax({
                url: objWebRoot.route + "api/v1/carpetas/mover/documentos/" + objBandejas.Enviados,
                type: "POST",
                data: JSON.stringify({ Usuario: objUser.username, Carpeta: $(this).data("id"), Valores: objSeleccion.elementos }),
                contentType: "application/json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
                },
                success: function (data) {
                    objCarpetas.respuesta = data;
                },
                error: function (xhr, status, error) {
                    objCarpetas.respuesta = { estado: false };
                },
                complete: function (xhr, status) {
                    if (objCarpetas.respuesta.estado) {
                        objSeleccion.elementos = [];
                        ObtenerBandeja(1);
                    }
                }
            });
            //--
        }
    });
}

function ControlVisualizacion() {
    $('#contiene-tabla table tbody').on("click", "tr a.link-externo", function () {
        window.open(this.href, "","width=960,height=980,menubar=no,resizable=no,status=no", true);
        return false;
    });
}