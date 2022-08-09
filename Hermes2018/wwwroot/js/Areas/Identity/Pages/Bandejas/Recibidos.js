/****************************************************************************************************************************************
*                                                   Start script datatable recibidos                                                    *
*****************************************************************************************************************************************/
var objWebRoot = { route: "", token: ""};
var objUser = { username: "" };
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4 };
var objTipoPara = { para: 1, ccp: 2 };

var objRecibidos = { urln: "", info: Object.keys(new Object()), exportar: new Array(), descarga: Object.keys(new Object()), temporalId: -1, auxiliar: -1, temporalBool: false, caracteristicas: { merges: new Array(), cols: new Array(), rows: new Array() } };
var objtr = { elementobase: Object.keys(new Object())};
var objSeleccion = { elementos: Object.keys(new Object()), indice: 0, total: 0 };
var objElementos = { elementospaginaactual: 0 };
var objBusqueda = { cadena: '', seBusco: false, inCarpeta: false};
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
        onPageClick: function (event, page)
        {
            ObtenerBandeja(page);
        }
    }
};
var objIndicador = { total: 0, proporcional: 0, porcentaje: 0.4, estado: false, fechaInicio: moment().locale('es'), fechaFin: moment().locale('es'), fechaAlerta: moment().locale('es'), fechaActual: moment().locale('es') };

var objProximosVencer = { proximoVencerId: 0 };
var objCategorias = { urln: "", info: Object.keys(new Object()), categoriaId: 0 };
var objTramites = { urln: "", info: Object.keys(new Object()), tramiteId: 0 };
var objEstados = { urln: "", info: Object.keys(new Object()), estadoId: 0 };
var objTipos = { urln: "", info: Object.keys(new Object()), tipoId: 0 };
var objFechas = { inicio: '', fin: '', seBusco: false };

var objCarpetas = { urln: "", info: Object.keys(new Object()), respuesta: Object.keys(new Object())  };
var objBandejas = { Recibidos: 1 };
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

var objMoverOficios = { totalCarpetas: 0 };

$(document).ready(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    //--
    objDelegar.estaActiva = ($('#InfoDelegar_EstaActiva').val() === 'True');
    objDelegar.tipoPermiso = (objDelegar.estaActiva) ? parseInt($('#InfoDelegar_TipoPermiso').val()) : 0;
    //--
    ControlBusquedaEnCarpetas();
    ObtenerBandeja(1);
    ControlSeleccion();
    ControlBusqueda();
    ControlProximosVencer();
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
    setInterval(ControlRecibidos, 180000);
});

function ControlRecibidos() {
    ObtenerBandeja(1);
}

function ObtenerBandeja(pagina)
{
         if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
             objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
                 + 'tipo=' + objTipos.tipoId + '&'
                 + 'estado=' + objEstados.estadoId + '&'
                 + 'busqueda=' + objBusqueda.cadena + '&'
                 + 'categoria=' + objCategorias.categoriaId + '&'
                 + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
                 + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
                 + 'tramite=' + objTramites.tramiteId + '&'
                 + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
             objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
                 + 'tipo=' + objTipos.tipoId + '&'
                 + 'estado=' + objEstados.estadoId + '&'
                 + 'busqueda=' + objBusqueda.cadena + '&'
                 + 'categoria=' + objCategorias.categoriaId + '&'
                 + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
                 + 'tramite=' + objTramites.tramiteId + '&'
                 + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
             objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
                 + 'tipo=' + objTipos.tipoId + '&'
                 + 'estado=' + objEstados.estadoId + '&'
                 + 'busqueda=' + objBusqueda.cadena + '&'
                 + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
                 + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
                 + 'tramite=' + objTramites.tramiteId + '&'
                 + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)){
            objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
                + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
                + 'tramite=' + objTramites.tramiteId + '&'
                + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId > 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tramite=' + objTramites.tramiteId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId > 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'proximovencer=' + objProximosVencer.proximoVencerId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'estado=' + objEstados.estadoId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId > 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'tipo=' + objTipos.tipoId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'categoria=' + objCategorias.categoriaId;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId > 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'estado=' + objEstados.estadoId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'busqueda=' + objBusqueda.cadena + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
            + 'categoria=' + objCategorias.categoriaId + '&'
            + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    else if (objTramites.tramiteId === 0 && objProximosVencer.proximoVencerId === 0 && objTipos.tipoId === 0 && objEstados.estadoId === 0 && objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
             objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?'
                 + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin + '&'
                 + 'enCarpeta=' + objBusqueda.inCarpeta;
         }
    else {
             objRecibidos.urln = objWebRoot.route + 'api/v1/documentos/recibidos/' + objUser.username + '/' + pagina + '?' + 'enCarpeta=' + objBusqueda.inCarpeta;
    }
    //--
    $.ajax({
        url: objRecibidos.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objRecibidos.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#Recibidos tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            objRecibidos.info = Object.keys(new Object());
            objRecibidos.info = data;
        },
        error: function (xhr, status, error) {
            objRecibidos.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if (objBusqueda.inCarpeta) {
                $('#carpetas').text("Carpeta");
            }
            if ((objRecibidos.info.Datos).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objRecibidos.info.Total_Paginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-recibidos').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-recibidos').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-recibidos').twbsPagination('destroy');
            }

            ProcesaBandeja();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#Recibidos tbody').fadeIn(500, "linear");
        }
    });
    //--
}

function ProcesaBandeja()
{
    $('#Recibidos tbody').empty();
    //--

    if ((objRecibidos.info.Datos).length > 0) {

        //Elementos de la página actual
        objElementos.elementospaginaactual = objRecibidos.info.Elementos_Pagina_Actual;
        
        $.each(objRecibidos.info.Datos, function (index, value)
        {
            objtr.elementobase = Object.keys(new Object());
            objSeleccion.indice = objSeleccion.elementos.findIndex(x => x.Id === parseInt(value.EnvioId) && x.Tipo === parseInt(value.TipoEnvio));

            //-----
            objIndicador.estado = false;

            if ((value.TipoEnvio == objTipoEnvio.envio || value.TipoEnvio == objTipoEnvio.turnar) && (value.Estado === "En proceso"))
            {
                objIndicador.fechaActual = moment().locale('es');
                objIndicador.fechaInicio = moment(value.FechaRecepcion).locale('es');
                objIndicador.fechaFin = moment(value.FechaCompromiso).locale('es');

                if (objIndicador.fechaActual.isSameOrBefore(objIndicador.fechaFin))
                {
                    objIndicador.total = objIndicador.fechaFin.diff(objIndicador.fechaInicio, 'days');
                    objIndicador.proporcional = Math.round(objIndicador.total * objIndicador.porcentaje);
                    objIndicador.fechaAlerta = moment(objIndicador.fechaFin.subtract(objIndicador.proporcional, 'd').locale('es').format('YYYY-MM-DDTHH:mm:ss'));
                    objIndicador.estado = objIndicador.fechaAlerta.isSameOrBefore(objIndicador.fechaActual);
                } else {
                    objIndicador.estado = false;
                }
            }

            //-----
            //Recibidos
 
             objtr.elementobase = $('<tr>').append(
                    $('<th scope="row">').append(
                        $('<input type="checkbox" class="checkbox_id" value="' + value.EnvioId + '" data-tipo="' + value.TipoEnvio + '">').prop('checked', (objSeleccion.indice === -1) ? false : true)
                    ),
                    $('<td>').append(
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Folio + '</a>')
                    ),
                    $('<td>').append(
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '" title="' + value.Area + ' (' + value.Region + ')' + '">' + value.Remitente + '</a>')
                    ),
                    $('<td>').append(
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Asunto + '</a>')
                    ),
                    $('<td>').append(
                        (value.NoInterno) ? $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.NoInterno + '</a>') : ''
                    ),
                    $('<td>').append(
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + moment(value.FechaRecepcion).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</a>')
                    ),
                    $('<td>').append(
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + ((value.TipoEnvio == objTipoEnvio.envio || value.TipoEnvio == objTipoEnvio.turnar) ? (value.RequiereRespuesta) ? moment(value.FechaCompromiso).locale('es').format('DD[/]MM[/]YYYY') : "" : "") + '</a>')
                    ),
                    $('<td>').append(
                        (value.Carpeta) ?
                            $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Carpeta + '</a>') :
                            (objBusqueda.inCarpeta && (objBusqueda.seBusco || objFechas.seBusco)) ?
                                $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">Recibidos</a>'): ''
                    ),
                    $('<td>').append(
                        (value.Importancia) ? $('<div data-toggle="tooltip" data-placement="top" title="Importancia alta"><i class="fas fa-exclamation font-red"></i></div>') : ''
                    ),
                    $('<td>').append(
                        (value.Adjuntos) ? $('<div data-toggle="tooltip" data-placement="top" title="Archivos adjuntos disponibles"><i class="fas fa-paperclip font-gray"></i></div>') : ''
                    ),
                    $('<td>').append(
                        (value.TipoEnvioParaIcono === objTipoEnvio.envio) ?
                            $('<div data-toggle="tooltip" data-placement="top" title="Doc. Enviado"><i class="fas fa-long-arrow-alt-left font-gray"></i></div>')
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
                        (value.TipoPara === objTipoPara.ccp) ?
                            $('<div data-toggle="tooltip" data-placement="top" title="Con copia"><i class="far fa-closed-captioning font-gray"></i></div>')
                            : ''
                    ),
                    $('<td>').append(
                        (value.Estado === "No requiere respuesta") ?
                            $('<i class="fas fa-circle font-gray" title="No requiere respuesta"></i>')
                            :
                            (value.Estado === "En proceso") ?
                                $('<i class="fas fa-circle font-yellow" title="En proceso"></i>')
                                :
                                (value.Estado === "Atendido") ?
                                    $('<i class="fas fa-circle font-green" title="Atendido"></i>')
                                    :
                                    (value.Estado === "Extemporáneo") ?
                                        $('<i class="fas fa-circle font-orange" title="Extemporáneo"></i>')
                                        :
                                        (value.Estado === "Vencido") ?
                                            $('<i class="fas fa-circle font-red" title="Vencido"></i>') : ''
                    ),
                    $('<td>').append(
                        (objIndicador.estado) ?
                            $('<i class="fas fa-hourglass-half font-gray" title="Próximo a vencer"></i>') : ''
                    ),
                    $('<td>').append(
                        $('<a class="font-dark link-externo" title="Abrir el documento en una ventana externa" href="' + objWebRoot.route + 'Correspondencia/Visualizacion/' + value.EnvioId + '/' + value.TipoEnvio + '/' + objUser.username + '"><i class="fas fa-external-link-alt font-gray"></i></a>')
                        //$('<a class="font-dark link-externo" title="Abrir el documento en una ventana externa" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '"><i class="fas fa-external-link-alt font-gray"></i></a>')
                    )
                    ).addClass((!value.Leido) ? 'sin-leer' : '').addClass((objSeleccion.indice === -1) ? '' : 'table-active');
            $('#Recibidos tbody').append(objtr.elementobase);
        });

        objSeleccion.total = $('#Recibidos tbody tr th input:checked').length;
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
            $('<td colspan="15" class="text-center">').text("Sin documentos para mostrar")
        );
        $('#Recibidos tbody').append(objtr.elementobase);
    }
}

function ControlSeleccion()
{
    $('#checkbox_all').click(function () {
        if ($(this).is(":checked")) {
            
            $('input.checkbox_id:checkbox').not(this).prop('checked', true).each(function ()
            {
                if ($(this).is(":checked"))
                {
                    $(this).parent().parent().removeClass('table-active').addClass('table-active');
                    objSeleccion.elementos.push({ Id: parseInt($(this).val()), Tipo: parseInt($(this).data('tipo')) });
                }
            });
            
        } else {
            $('input.checkbox_id').not(this).prop('checked', false).each(function ()
            {
                if (!$(this).is(":checked"))
                {
                    $(this).parent().parent().removeClass('table-active');
                    objSeleccion.indice = objSeleccion.elementos.findIndex(x => x.Id === parseInt($(this).val()) && x.Tipo === parseInt($(this).data('tipo')));
                    if (objSeleccion.indice !== -1)
                    {
                        objSeleccion.elementos.splice(objSeleccion.indice, 1);
                    }
                }
            });
        }
        //--
        objSeleccion.total = $('#Recibidos tbody tr th input:checked').length;
        //-
        if (objElementos.elementospaginaactual === objSeleccion.total && objElementos.elementospaginaactual > 0)
        {
            $('#checkbox_all').prop('checked', true);
        }
        else if (objElementos.elementospaginaactual > objSeleccion.total)
        {
            $('#checkbox_all').prop('checked', false);
        }
    });

    $('tbody').on("click", " tr .checkbox_id", function () {
        if ($(this).is(":checked"))
        {
            $(this).parent().parent().removeClass('table-active').addClass('table-active');
            objSeleccion.elementos.push({ Id: parseInt($(this).val()), Tipo: parseInt($(this).data('tipo')) });
        }
        else
        {
            $(this).parent().parent().removeClass('table-active');
            objSeleccion.indice = objSeleccion.elementos.findIndex(x => x.Id === parseInt($(this).val()) && x.Tipo === parseInt($(this).data('tipo')));
            if (objSeleccion.indice !== -1)
            {
                objSeleccion.elementos.splice(objSeleccion.indice, 1);
            }
        }
        //--
        objSeleccion.total = $('#Recibidos tbody tr th input:checked').length;
        //--
        if (objElementos.elementospaginaactual === objSeleccion.total && objElementos.elementospaginaactual > 0)
        {
            $('#checkbox_all').prop('checked', true);
        }
        else if (objElementos.elementospaginaactual > objSeleccion.total )
        {
            $('#checkbox_all').prop('checked', false);
        }
    });
}

function ControlBusqueda()
{
    jQuery(document).on('keyup contextmenu input', 'input#searchbox', function (ev) {
        objBusqueda.cadena = $(this).val().trim();

        if (ev.which === 13) {
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
            if (objBusqueda.seBusco)
            {
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
            $('#carpetas').text("");
            ObtenerBandeja(1);
        }
    })
}
/**** ***************** ****/
function ControlProximosVencer()
{
    $("#select-proximo-vencer").change(function () {
        objProximosVencer.proximoVencerId = parseInt($("#select-proximo-vencer option:selected").val());
        objBusqueda.seBusco = true;
        ObtenerBandeja(1);
    });
}
function ObtenerEstados() {
    objEstados.urln = objWebRoot.route + 'api/v1/estados/bandeja/recibidos';
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

function ObtenerCategorias()
{
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
    objRecibidos.exportar = Object.keys(new Object());

    //--Cambia el orden de la lista
    objRecibidos.info.Datos.sort(function (a, b) {
        if (a.TramiteId > b.TramiteId || a.FechaRecepcion > b.FechaRecepcion) {
            return 1;
        }

        if (a.TramiteId < b.TramiteId || a.FechaRecepcion < b.FechaRecepcion) {
            return -1;
        }

        return 0;
    });
    //--
    $.each(objRecibidos.info.Datos, function (index, value) {

        objRecibidos.auxiliar = (value.TramiteId == null) ? 0 : value.TramiteId;
        objRecibidos.temporalBool = value.Proximo != null;

        if (value.Tramite != null) {
            if (objRecibidos.auxiliar != objRecibidos.temporalId) {
                objRecibidos.exportar.push(new Array({
                    content: "TIPO DE TRÁMITE: " + value.Tramite.toUpperCase(), rowSpan: 0, colSpan: 16, styles: { halign: 'center', valign: 'middle', fillColor: [153, 206, 212] }
                    })
                );
            }
        }

        objRecibidos.exportar.push(
            new Array(
                { content: value.Folio, rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: value.NoInterno != null ? value.NoInterno : "", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: moment(value.FechaRecepcion).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]'), rowSpan: 0, colSpan: 0},
                { content: (value.TipoEnvio === objTipoEnvio.envio || value.TipoEnvio === objTipoEnvio.turnar) ? (value.RequiereRespuesta) ? moment(value.FechaCompromiso).locale('es').format('DD[/]MM[/]YYYY') : "" : "", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: (value.Importancia) ? "Alta" : "Normal", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: (value.Adjuntos) ? "Si" : "No", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: (value.TipoEnvioParaIcono === objTipoEnvio.envio) ? "Envio" : (value.TipoEnvioParaIcono === objTipoEnvio.turnar) ? "Turnado" : (value.TipoEnvioParaIcono === objTipoEnvio.respuestaParcial) ? "Respuesta parcial" : (value.TipoEnvioParaIcono === objTipoEnvio.respuesta) ? "Respuesta" : "", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: (value.RequiereRespuesta) ? "Si" : "No", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' }},
                { content: value.Remitente, rowSpan: 0, colSpan: 0 },
                { content: value.Area, rowSpan: 0, colSpan: 0 },
                { content: value.Region, rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: value.Asunto, rowSpan: 0, colSpan: 0 },
                { content: objRecibidos.temporalBool ? moment(value.Proximo.FechaEnvio).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') : "-", rowSpan: 0, colSpan: 0 },
                { content: objRecibidos.temporalBool ? value.Proximo.Destinatarios[0].Destinatario : "No ha sido Turnado", rowSpan: 0, colSpan: 0 },
                { content: objRecibidos.temporalBool ? value.Proximo.Destinatarios[0].Area : "-", rowSpan: 0, colSpan: 0, },
                { content: objRecibidos.temporalBool ? value.Proximo.Destinatarios[0].Region : "-", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } }
            )
        );

        objRecibidos.temporalId = (value.TramiteId == null) ? 0 : value.TramiteId;
    });

    objRecibidos.descarga = Object.keys(new Object());
    objRecibidos.descarga = new jsPDF({
        orientation: "landscape",
        format: [623.62, 1190.55]
    });

    objRecibidos.descarga.autoTable({
        head: [
            [
                { content: 'Folio', rowSpan: 2, colSpan: 0 },
                { content: 'Número Of./Memo', rowSpan: 2, colSpan: 0 },
                { content: 'Fecha Recepción', rowSpan: 2, colSpan: 0 },
                { content: 'Fecha Compromiso', rowSpan: 2, colSpan: 0 },
                { content: 'Importancia', rowSpan: 2, colSpan: 0 },
                { content: 'Adjuntos', rowSpan: 2, colSpan: 0 },
                { content: 'Tipo Envío', rowSpan: 2, colSpan: 0 },
                { content: 'Requiere Respuesta', rowSpan: 2, colSpan: 0 },
                { content: 'Solicita', rowSpan: 0, colSpan: 3 },
                { content: 'Asunto', rowSpan: 2, colSpan: 0 },
                { content: 'Atiende', rowSpan: 0, colSpan: 4 }
            ],
            [
                { content: 'De', rowSpan: 0, colSpan: 0 },
                { content: 'Entidad/Dependencia', rowSpan: 0, colSpan: 0 },
                { content: 'Región', rowSpan: 0, colSpan: 0 },
                { content: 'Fecha Envío', rowSpan: 0, colSpan: 0 },
                { content: 'Para', rowSpan: 0, colSpan: 0 },
                { content: 'Entidad/Dependencia', rowSpan: 0 },
                { content: 'Región', rowSpan: 0, colSpan: 0 },
            ]
        ],
        body: objRecibidos.exportar,
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

    objRecibidos.descarga.save('ListadoRecibidos.pdf');
}

function ControlExportarExcel() {
    objRecibidos.exportar = Object.keys(new Object());

    //--Cambia el orden de la lista
    objRecibidos.info.Datos.sort(function (a, b) {
        if (a.TramiteId > b.TramiteId || a.FechaRecepcion > b.FechaRecepcion) {
            return 1;
        }

        if (a.TramiteId < b.TramiteId || a.FechaRecepcion < b.FechaRecepcion) {
            return -1;
        }

        return 0;
    });
    //--
    objRecibidos.exportar.push(
        { "Folio": "Folio", "Número Of./Memo": "Número Of./Memo", "Fecha Recepción": "Fecha Recepción", "Fecha Compromiso": "Fecha Compromiso", "Importancia": "Importancia", "Adjuntos": "Adjuntos", "Tipo Envío": "Tipo Envío", "Requiere Respuesta": "Requiere Respuesta", "De": "Solicita", "Entidad/Dependencia1": "", "Región1": "", "Asunto": "Asunto", "Fecha Envío": "Atiende", "Para": "", "Entidad/Dependencia2": "","Región2": ""},
        { "Folio": "", "Número Of./Memo": "", "Fecha Recepción": "", "Fecha Compromiso": "", "Importancia": "", "Adjuntos": "", "Tipo Envío": "", "Requiere Respuesta": "", "De": "De", "Entidad/Dependencia1": "Entidad/Dependencia", "Región1": "Región", "Asunto": "", "Fecha Envío": "Fecha Envío", "Para": "Para", "Entidad/Dependencia2": "Entidad/Dependencia", "Región2": "Región"}
    );
    //--
    //--
    objRecibidos.caracteristicas.merges.push(
        {s: { r: 0, c: 8 },e: { r: 0, c: 10 }},
        {s: { r: 0, c: 11 },e: { r: 1, c: 11 }},
        {s: { r: 0, c: 12 },e: { r: 0, c: 15 }},
        {s: { r: 0, c: 7 },e: { r: 1, c: 7 }},
        {s: { r: 0, c: 6 },e: { r: 1, c: 6 }},
        {s: { r: 0, c: 5 },e: { r: 1, c: 5 }},
        {s: { r: 0, c: 4 },e: { r: 1, c: 4 }},
        {s: { r: 0, c: 3 },e: { r: 1, c: 3 }},
        {s: { r: 0, c: 2 },e: { r: 1, c: 2 }},
        {s: { r: 0, c: 1 },e: { r: 1, c: 1 }},
        {s: { r: 0, c: 0 },e: { r: 1, c: 0 }}
    );
    //--
    $.each(objRecibidos.info.Datos, function (index, value) {

        objRecibidos.auxiliar = (value.TramiteId == null) ? 0 : value.TramiteId;
        objRecibidos.temporalBool = value.Proximo != null;

        if (value.Tramite != null) {
            if (objRecibidos.auxiliar != objRecibidos.temporalId) {

                objRecibidos.exportar.push(
                    { "Folio": "TIPO DE TRÁMITE: " + value.Tramite.toUpperCase(), "Número Of./Memo": "", "Fecha Recepción": "", "Fecha Compromiso": "", "Importancia": "", "Adjuntos": "", "Tipo Envío": "", "Requiere Respuesta": "", "De": "", "Entidad/Dependencia1": "", "Región1": "", "Asunto": "", "Fecha Envío": "", "Para": "", "Entidad/Dependencia2": "","Región2": ""}
                );

                objRecibidos.caracteristicas.merges.push(
                    {s: { r: (objRecibidos.exportar.length - 1), c: 0 },e: { r: (objRecibidos.exportar.length - 1), c: 13 }}
                );
            }
        }

        objRecibidos.exportar.push( {
             "Folio": value.Folio,
             "Número Of./Memo": value.NoInterno != null ? value.NoInterno : "" ,
             "Fecha Recepción": moment(value.FechaRecepcion).locale('es').format('DD[/]MM[/]YYYY') ,
             "Fecha Compromiso": (value.TipoEnvio === objTipoEnvio.envio || value.TipoEnvio === objTipoEnvio.turnar) ? (value.RequiereRespuesta) ? moment(value.FechaCompromiso).locale('es').format('DD[/]MM[/]YYYY') : "" : "" ,
             "Importancia": (value.Importancia) ? "Alta" : "Normal" ,
             "Adjuntos": (value.Adjuntos) ? "Si" : "No" ,
             "Tipo Envío": (value.TipoEnvioParaIcono === objTipoEnvio.envio) ? "Envio" : (value.TipoEnvioParaIcono === objTipoEnvio.turnar) ? "Turnado" : (value.TipoEnvioParaIcono === objTipoEnvio.respuestaParcial) ? "Respuesta parcial" : (value.TipoEnvioParaIcono === objTipoEnvio.respuesta) ? "Respuesta" : "" ,
             "Requiere Respuesta": (value.RequiereRespuesta) ? "Si" : "No" ,
             "De": value.Remitente,
             "Entidad/Dependencia1": value.Area,
             "Región1": value.Region,
             "Asunto": value.Asunto ,
             "Fecha Envío": objRecibidos.temporalBool ? moment(value.Proximo.FechaEnvio).locale('es').format('DD[/]MM[/]YYYY') : "-" ,
             "Para": objRecibidos.temporalBool ? value.Proximo.Destinatarios[0].Destinatario : "No ha sido Turnado" ,
             "Entidad/Dependencia2": objRecibidos.temporalBool ? value.Proximo.Destinatarios[0].Area : "-",
             "Región2": objRecibidos.temporalBool ? value.Proximo.Destinatarios[0].Region : "-"
            });

        objRecibidos.temporalId = (value.TramiteId == null) ? 0 : value.TramiteId;
    });

    //---
    var wb = XLSX.utils.book_new();
    var ws = XLSX.utils.json_to_sheet(objRecibidos.exportar, { skipHeader: true });
    wb.Props = {
        Title: "Documentos Recibidos",
        Subject: "HERMES",
        Author: "UV",
    };
    wb.SheetNames.push("Recibidos (" + objUser.username + ")");
    ws['!merges'] = objRecibidos.caracteristicas.merges;
    wb.Sheets["Recibidos (" + objUser.username + ")"] = ws;

    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length); //convert s to arrayBuffer
        var view = new Uint8Array(buf);  //create uint8array as viewer
        for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF; //convert to octet
        return buf;
    }

    saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'ListadoRecibidos.xlsx');
}

function ControlExportar()
{
    $('#exportar-pdf').click(function ()
    {
        ControlExportarPDF();
    });

    $('#exportar-excel').click(function ()
    {
        ControlExportarExcel();
    });
}
/**************************/
function ObtenerCarpetas()
{
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
            //--
            objMoverOficios.totalCarpetas = data.length;
        },
        error: function (xhr, status, error) {
            objCarpetas.info = Object.keys(new Object());
        },
        complete: function (xhr, status) {
            if (objMoverOficios.totalCarpetas > 0) {
                $("#carpetasDropdown").removeClass('disabled');
            } else {
                $("#carpetasDropdown").removeClass('disabled');
                $("#carpetasDropdown").addClass('disabled');
            }
            //--
            ProcesaCarpetas();
        }
    });
    //--
}

function ProcesaCarpetas()
{
    $.each(objCarpetas.info, function (index, value) {
        $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 bg-light" data-id="' + value.CarpetaId +'"><div>' + value.Nombre + '</div></button>') );
        $('#contiene-carpetas').append( $('<div class="dropdown-divider sin-margen"></div>') );
        
        $.each(value.Subcarpetas, function (i, v) {
            $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 con-padding-l-14 font-dark-gray" data-id="' + v.SubcarpetaId +'"><div>' + v.Nombre + '</div></button>'));
            $('#contiene-carpetas').append($('<div class="dropdown-divider sin-margen"></div>'));
        });
    });
}

function ControlCarpetas()
{
    $('#contiene-carpetas').on("click", "button.carpeta-seleccion", function ()
    {
        if (objSeleccion.elementos.length > 0)
        {
            //--
            $.ajax({
                url: objWebRoot.route + "api/v1/carpetas/mover/documentos/" + objBandejas.Recibidos,
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
    $('#contiene-tabla table tbody').on("click", "tr a.link-externo",function () {
        window.open(this.href, "", "width=960,height=980,menubar=no,resizable=no,status=no", true);
        return false;
    });
}