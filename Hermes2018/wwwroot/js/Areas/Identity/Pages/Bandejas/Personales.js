/****************************************************************************************************************************************
*                                                   Start script datatable carpeta personal                                             *
*****************************************************************************************************************************************/
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objCarpeta = { id: 0};
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4 };
var objTipoPara = { para: 1, ccp: 2 };

var objCarpetaPersonal = { urln: "", info: Object.keys(new Object()), exportar: new Array(), descarga: Object.keys(new Object()) };
var objtr = { elementobase: Object.keys(new Object()) };
var objSeleccion = { elementos: Object.keys(new Object()), indice: 0, total: 0 };
var objElementos = { elementospaginaactual: 0 };
var objBusqueda = { cadena: '', seBusco: false };
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
var objFechas = { inicio: '', fin: '', seBusco: false };
var objCarpetas = { urln: "", info: Object.keys(new Object()), respuesta: Object.keys(new Object()) };

var objBandejas = {Recibidos: 1, Enviados: 2 };
var objBandejasSeleccion = {actual : 1};
var objDelegar = { estaActiva: false, tipoPermiso: 0 };

var objControl = { d1: $.Deferred() };

$(document).ready(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    objCarpeta.id = parseInt($('#CarpetaId').val());
    objBandejasSeleccion.actual = parseInt($('#TipoBandeja').val());
    //--
    objDelegar.estaActiva = ($('#InfoDelegar_EstaActiva').val() == 'True');
    objDelegar.tipoPermiso = (objDelegar.estaActiva) ? parseInt($('#InfoDelegar_TipoPermiso').val()) : 0;
    //---------------
    ObtenerBandeja(1);
    //--
    ControlSeleccionRecibidos();
    ControlSeleccionEnviados();
    //-
    ControlBusqueda();
    ObtenerCategorias();
    ControlFechas();
    ControlExportar();
    //--
    if (objDelegar.estaActiva === false || (objDelegar.estaActiva === true && objDelegar.tipoPermiso === 1)) {
        ObtenerCarpetas();
        ControlCarpetas();
    }
    //
    ControlTabs();
    //
    ControlVisualizacion();
    //--
});

function ObtenerBandeja(pagina) {
    if (objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objCarpetaPersonal.urln = objWebRoot.route + 'api/v1/documentos/carpeta/personal/' + objUser.username + '/' + pagina + '/' + objCarpeta.id + '/' + objBandejasSeleccion.actual + '?' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objCarpetaPersonal.urln = objWebRoot.route + 'api/v1/documentos/carpeta/personal/' + objUser.username + '/' + pagina + '/' + objCarpeta.id + '/' + objBandejasSeleccion.actual + '?' + 'busqueda=' + objBusqueda.cadena;
    } else if (objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objCarpetaPersonal.urln = objWebRoot.route + 'api/v1/documentos/carpeta/personal/' + objUser.username + '/' + pagina + '/' + objCarpeta.id + '/' + objBandejasSeleccion.actual + '?' + 'categoria=' + objCategorias.categoriaId;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objCarpetaPersonal.urln = objWebRoot.route + 'api/v1/documentos/carpeta/personal/' + objUser.username + '/' + pagina + '/' + objCarpeta.id + '/' + objBandejasSeleccion.actual + '?' + 'busqueda=' + objBusqueda.cadena + '&' + 'categoria=' + objCategorias.categoriaId;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objCarpetaPersonal.urln = objWebRoot.route + 'api/v1/documentos/carpeta/personal/' + objUser.username + '/' + pagina + '/' + objCarpeta.id + '/' + objBandejasSeleccion.actual + '?' + 'busqueda=' + objBusqueda.cadena + '&' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else if (objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objCarpetaPersonal.urln = objWebRoot.route + 'api/v1/documentos/carpeta/personal/' + objUser.username + '/' + pagina + '/' + objCarpeta.id + '/' + objBandejasSeleccion.actual + '?' + 'categoria=' + objCategorias.categoriaId + '&' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objCarpetaPersonal.urln = objWebRoot.route + 'api/v1/documentos/carpeta/personal/' + objUser.username + '/' + pagina + '/' + objCarpeta.id + '/' + objBandejasSeleccion.actual + '?' + 'busqueda=' + objBusqueda.cadena + '&' + 'categoria=' + objCategorias.categoriaId + '&' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else {
        objCarpetaPersonal.urln = objWebRoot.route + 'api/v1/documentos/carpeta/personal/' + objUser.username + '/' + pagina + '/' + objCarpeta.id + '/' + objBandejasSeleccion.actual;
    }

    //--
    $.ajax({
        url: objCarpetaPersonal.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objCarpetaPersonal.info = Object.keys(new Object());
            //--
            if (objBandejasSeleccion.actual === objBandejas.Recibidos) {
                $('#contiene-cargador-recibidos .loader').removeClass('d-none');
                $('#Recibidos tbody').fadeOut(100, "linear");
                //--
                $('#Enviados tbody').empty();
            }
            else if (objBandejasSeleccion.actual === objBandejas.Enviados) {
                $('#contiene-cargador-enviados .loader').removeClass('d-none');
                $('#Enviados tbody').fadeOut(100, "linear");
                //--
                $('#Recibidos tbody').empty();
            }
        },
        success: function (data) {
            objCarpetaPersonal.info = Object.keys(new Object());
            objCarpetaPersonal.info = data;
        },
        error: function (xhr, status, error) {
            objCarpetaPersonal.info = Object.keys(new Object());
            //--
            if (objBandejasSeleccion.actual === objBandejas.Recibidos) {
                $('#contiene-cargador-recibidos .loader').removeClass('d-none').addClass('d-none');
            }
            else if (objBandejasSeleccion.actual === objBandejas.Enviados) {
                $('#contiene-cargador-enviados .loader').removeClass('d-none').addClass('d-none');
            }
        },
        complete: function (xhr, status) {
            if ((objCarpetaPersonal.info.Datos).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objCarpetaPersonal.info.Total_Paginas;

                if (objBandejasSeleccion.actual === objBandejas.Recibidos) {
                    //--Limpia la paginación
                    $('#contenedor-paginacion-recibidos ul').empty();
                    $('#paginacion-recibidos').twbsPagination('destroy');
                    //--Crea la paginación
                    $('#paginacion-recibidos').twbsPagination(objPaginacion.opciones);
                }
                else if (objBandejasSeleccion.actual === objBandejas.Enviados) {
                    //--Limpia la paginación
                    $('#contenedor-paginacion-enviados ul').empty();
                    $('#paginacion-enviados').twbsPagination('destroy');
                    //--Crea la paginación
                    $('#paginacion-enviados').twbsPagination(objPaginacion.opciones);
                }
            } else {
                if (objBandejasSeleccion.actual === objBandejas.Recibidos) {
                    $('#contenedor-paginacion-recibidos ul').empty();
                    $('#paginacion-recibidos').twbsPagination('destroy');
                }
                else if (objBandejasSeleccion.actual === objBandejas.Enviados) {
                    $('#contenedor-paginacion-enviados ul').empty();
                    $('#paginacion-enviados').twbsPagination('destroy');
                }
            }

            if (objBandejasSeleccion.actual === objBandejas.Recibidos) {
                ProcesaBandejaRecibidos();
            }
            else if (objBandejasSeleccion.actual === objBandejas.Enviados) {
                ProcesaBandejaEnviados();
            }

            if (objBandejasSeleccion.actual === objBandejas.Recibidos) {
                $('#contiene-cargador-recibidos .loader').removeClass('d-none').addClass('d-none');
                $('#Recibidos tbody').fadeIn(500, "linear");
            }
            else if (objBandejasSeleccion.actual === objBandejas.Enviados) {
                $('#contiene-cargador-enviados .loader').removeClass('d-none').addClass('d-none');
                $('#Enviados tbody').fadeIn(500, "linear");
            }
        }
    });
    //--
}

function ProcesaBandejaRecibidos() {
    $('#Recibidos tbody').empty();
    //--
    if ((objCarpetaPersonal.info.Datos).length > 0) {

        //Elementos de la página actual
        objElementos.elementospaginaactual = objCarpetaPersonal.info.Elementos_Pagina_Actual;

        $.each(objCarpetaPersonal.info.Datos, function (index, value) {
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
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Remitente + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Asunto + '</a>')
                ),
                $('<td>').append(
                    (value.NoInterno) ? $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.NoInterno + '</a>') : ''
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</a>')
                ),
                $('<td>').append(
                    (value.Importancia) ? $('<div data-toggle="tooltip" data-placement="top" title="Importancia alta"><i class="fas fa-exclamation font-red"></i></div>') : ''
                ),
                $('<td>').append(
                    (value.Adjuntos) ? $('<div data-toggle="tooltip" data-placement="top" title="Archivos adjuntos disponibles"><i class="fas fa-paperclip font-gray"></i></div>') : ''
                ),
                $('<td>').append(
                    (value.TipoEnvio === objTipoEnvio.envio) ?
                        $('<div data-toggle="tooltip" data-placement="top" title="Doc. Enviado"><i class="fas fa-long-arrow-alt-left font-gray"></i></div>')
                        :
                        (value.TipoEnvio === objTipoEnvio.turnar) ?
                            $('<div data-toggle="tooltip" data-placement="top" title="Doc. Turnado"><i class="fas fa-redo-alt font-gray"></i></div>')
                            :
                            (value.TipoEnvio === objTipoEnvio.respuestaParcial) ?
                                $('<div data-toggle="tooltip" data-placement="top" title="Respuesta parcial"><i class="fab fa-stack-exchange font-gray"></i></div>')
                                :
                                (value.TipoEnvio === objTipoEnvio.respuesta) ?
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
                    $('<a class="font-dark link-externo" title="Abrir el documento en una ventana externa" href="' + objWebRoot.route + 'Correspondencia/Visualizacion/' + value.EnvioId + '/' + value.TipoEnvio + '/' + objUser.username + '"><i class="fas fa-external-link-alt font-gray"></i></a>')
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
            $('<td colspan="10" class="text-center">').text("Sin documentos para mostrar")
        );
        $('#Recibidos tbody').append(objtr.elementobase);
    }
}
function ProcesaBandejaEnviados() {
    $('#Enviados tbody').empty();
    //--
    if ((objCarpetaPersonal.info.Datos).length > 0) {

        //Elementos de la página actual
        objElementos.elementospaginaactual = objCarpetaPersonal.info.Elementos_Pagina_Actual;

        $.each(objCarpetaPersonal.info.Datos, function (index, value) {
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
                    (value.Destinatarios_Extra > 0) ?
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Destinatario + ' (' + value.Destinatarios_Extra + ')' + '</a>')
                        :
                        $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Destinatario + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.Asunto + '</a>')
                ),
                $('<td>').append(
                    (value.NoInterno) ? $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.NoInterno + '</a>') : ''
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Lectura/' + value.EnvioId + '/' + value.TipoEnvio + '">' + moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</a>')
                ),
                $('<td>').append(
                    (value.Importancia) ? $('<div data-toggle="tooltip" data-placement="top" title="Importancia alta"><i class="fas fa-exclamation font-red"></i></div>') : ''
                ),
                $('<td>').append(
                    (value.Adjuntos) ? $('<div data-toggle="tooltip" data-placement="top" title="Archivos adjuntos disponibles"><i class="fas fa-paperclip font-gray"></i></div>') : ''
                ),
                $('<td>').append(
                    (value.TipoEnvio === objTipoEnvio.envio) ?
                        $('<div data-toggle="tooltip" data-placement="top" title="Doc. Enviado"><i class="fas fa-long-arrow-alt-left font-gray"></i></div>')
                        :
                        (value.TipoEnvio === objTipoEnvio.turnar) ?
                            $('<div data-toggle="tooltip" data-placement="top" title="Doc. Turnado"><i class="fas fa-redo-alt font-gray"></i></div>')
                            :
                            (value.TipoEnvio === objTipoEnvio.respuestaParcial) ?
                                $('<div data-toggle="tooltip" data-placement="top" title="Respuesta parcial"><i class="fab fa-stack-exchange font-gray"></i></div>')
                                :
                                (value.TipoEnvio === objTipoEnvio.respuesta) ?
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
            $('<td colspan="10" class="text-center">').text("Sin documentos para mostrar")
        );
        $('#Enviados tbody').append(objtr.elementobase);
    }
}

function ControlSeleccionRecibidos() {
    $('#checkbox_all_recibidos').click(function () {
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
        objSeleccion.total = $('#Recibidos tbody tr th input:checked').length;
        //-
        if (objElementos.elementospaginaactual === objSeleccion.total && objElementos.elementospaginaactual > 0) {
            $('#checkbox_all_recibidos').prop('checked', true);
        }
        else if (objElementos.elementospaginaactual > objSeleccion.total) {
            $('#checkbox_all_recibidos').prop('checked', false);
        }
    });

    $('#Recibidos tbody').on("click", " tr .checkbox_id", function () {
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
        objSeleccion.total = $('#Recibidos tbody tr th input:checked').length;
        //--
        if (objElementos.elementospaginaactual === objSeleccion.total && objElementos.elementospaginaactual > 0) {
            $('#checkbox_all_recibidos').prop('checked', true);
        }
        else if (objElementos.elementospaginaactual > objSeleccion.total) {
            $('#checkbox_all_recibidos').prop('checked', false);
        }
    });
}
function ControlSeleccionEnviados() {
    $('#checkbox_all_enviados').click(function () {
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
            $('#checkbox_all_enviados').prop('checked', true);
        }
        else if (objElementos.elementospaginaactual > objSeleccion.total) {
            $('#checkbox_all_enviados').prop('checked', false);
        }
    });

    $('#Enviados tbody').on("click", " tr .checkbox_id", function () {
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
            $('#checkbox_all_enviados').prop('checked', true);
        }
        else if (objElementos.elementospaginaactual > objSeleccion.total) {
            $('#checkbox_all_enviados').prop('checked', false);
        }
    });
}

function ControlBusqueda() {
    jQuery(document).on('keyup', 'input#searchbox', function (ev) {
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
            ObtenerBandeja(1);
        }
    });
}
function ObtenerCategorias() {
    objCategorias.urln = objWebRoot.route + 'api/v1/categorias/' + objUser.username;

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
            ObtenerBandeja(1);
        }

    });
}
/************************/
function ControlExportar() {
    $('#exportar-pdf').click(function () {
        if (objBandejasSeleccion.actual === objBandejas.Recibidos)
        {
            ControlExportarPDFRecibidos();
        }
        else if (objBandejasSeleccion.actual === objBandejas.Enviados)
        {
            ControlExportarPDFEnviados();
        }
    });

    $('#exportar-excel').click(function () {
        if (objBandejasSeleccion.actual === objBandejas.Recibidos) {
            ControlExportarExcelRecibidos();
        }
        else if (objBandejasSeleccion.actual === objBandejas.Enviados) {
            ControlExportarExcelEnviados();
        }
    });
}

function ControlExportarPDFRecibidos() {
    objCarpetaPersonal.exportar = Object.keys(new Object());

    $.each(objCarpetaPersonal.info.Datos, function (index, value) {
        objCarpetaPersonal.exportar.push({
            origen: value.Bandeja_Origen,
            folio: value.Folio,
            remitente: value.Remitente,
            asunto: value.Asunto,
            fecha: moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]'),
            importancia: (value.Importancia) ? "Alta" : "Normal",
            adjuntos: (value.Adjuntos) ? "Si" : "No",
            tipoEnvio: (value.TipoEnvio === objTipoEnvio.envio) ? "Envio" : (value.TipoEnvio === objTipoEnvio.turnar) ? "Turnado" : (value.TipoEnvio === objTipoEnvio.respuestaParcial) ? "Respuesta parcial" : (value.TipoEnvio === objTipoEnvio.respuesta) ? "Respuesta" : "", 
            estado: (value.Estado !== "Respuesta") ? value.Estado : ""
        });
    });

    objCarpetaPersonal.descarga = Object.keys(new Object());
    objCarpetaPersonal.descarga = new jsPDF();

    objCarpetaPersonal.descarga.autoTable({
        columns: [
            { header: 'Origen', dataKey: 'origen' },
            { header: 'Folio', dataKey: 'folio' },
            { header: 'Remitente', dataKey: 'remitente' },
            { header: 'Asunto', dataKey: 'asunto' },
            { header: 'Fecha', dataKey: 'fecha' },
            { header: 'Importancia', dataKey: 'importancia' },
            { header: 'Adjuntos', dataKey: 'adjuntos' },
            { header: 'TipoEnvio', dataKey: 'tipoEnvio' },
            { header: 'Estado', dataKey: 'estado' }],
        body: objCarpetaPersonal.exportar,
        margin: { top: 5, bottom: 5, right: 5, left: 5 },
        theme: 'striped',
        headStyles: {
            fillColor: [0, 131, 143]
        },
        styles: {
            font: 'helvetica',
            fontStyle: 'normal',
            fontSize: 6,
            overflow: 'linebreak',
            cellWidth: 'auto'
        }
    });

    objCarpetaPersonal.descarga.save('ListadoCarpetaPersonalRecibidos.pdf');
}
function ControlExportarPDFEnviados() {
    objCarpetaPersonal.exportar = Object.keys(new Object());

    $.each(objCarpetaPersonal.info.Datos, function (index, value) {
        objCarpetaPersonal.exportar.push({
            origen: value.Bandeja_Origen,
            folio: value.Folio,
            destinatario: value.Destinatario,
            destinatariosExtras: value.Destinatarios_Extra,
            asunto: value.Asunto,
            fecha: moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]'),
            importancia: (value.Importancia) ? "Alta" : "Normal",
            adjuntos: (value.Adjuntos) ? "Si" : "No",
            tipoEnvio: (value.TipoEnvio === objTipoEnvio.envio) ? "Envio" : (value.TipoEnvio === objTipoEnvio.turnar) ? "Turnado" : (value.TipoEnvio === objTipoEnvio.respuestaParcial) ? "Respuesta parcial" : (value.TipoEnvio === objTipoEnvio.respuesta) ? "Respuesta" : "", 
            estado: (value.Estado !== "Respuesta") ? value.Estado : ""
        });
    });

    objCarpetaPersonal.descarga = Object.keys(new Object());
    objCarpetaPersonal.descarga = new jsPDF();

    objCarpetaPersonal.descarga.autoTable({
        columns: [
            { header: 'Origen', dataKey: 'origen' },
            { header: 'Folio', dataKey: 'folio' },
            { header: 'Destinatario', dataKey: 'destinatario' },
            { header: 'DestinatariosExtras', dataKey: 'destinatariosExtras' },
            { header: 'Asunto', dataKey: 'asunto' },
            { header: 'Fecha', dataKey: 'fecha' },
            { header: 'Importancia', dataKey: 'importancia' },
            { header: 'Adjuntos', dataKey: 'adjuntos' },
            { header: 'TipoEnvio', dataKey: 'tipoEnvio' },
            { header: 'Estado', dataKey: 'estado' }],
        body: objCarpetaPersonal.exportar,
        margin: { top: 5, bottom: 5, right: 5, left: 5 },
        theme: 'striped',
        headStyles: {
            fillColor: [0, 131, 143]
        },
        styles: {
            font: 'helvetica',
            fontStyle: 'normal',
            fontSize: 6,
            overflow: 'linebreak',
            cellWidth: 'auto'
        }
    });

    objCarpetaPersonal.descarga.save('ListadoEnviadosEnviados.pdf');
}

function ControlExportarExcelRecibidos() {
    objCarpetaPersonal.exportar = Object.keys(new Object());

    objCarpetaPersonal.exportar.push(new Array(
        { text: "Origen" },
        { text: "Folio" },
        { text: "Remitente" },
        { text: "Asunto" },
        { text: "Fecha" },
        { text: "Importancia" },
        { text: "Adjuntos" },
        { text: "TipoEnvio" },
        { text: "Estado" }
    ));

    $.each(objCarpetaPersonal.info.Datos, function (index, value) {
        objCarpetaPersonal.exportar.push(new Array(
            { text: value.Bandeja_Origen },
            { text: value.Folio },
            { text: value.Remitente },
            { text: value.Asunto },
            { text: moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') },
            { text: (value.Importancia) ? "Alta" : "Normal" },
            { text: (value.Adjuntos) ? "Si" : "No" },
            { text: (value.TipoEnvio === objTipoEnvio.envio) ? "Envio" : (value.TipoEnvio === objTipoEnvio.turnar) ? "Turnado" : (value.TipoEnvio === objTipoEnvio.respuestaParcial) ? "Respuesta parcial" : (value.TipoEnvio === objTipoEnvio.respuesta) ? "Respuesta" : "" },
            { text: (value.Estado !== "Respuesta") ? value.Estado : "" }
        ));
    });

    Jhxlsx.export(
        [{
            sheetName: "CarpetaPersonalRecibidos",
            data: objCarpetaPersonal.exportar
        }], {
            fileName: "ListadoCarpetaPersonalRecibidos",
            extension: ".xlsx",
            sheetName: "Hoja1",
            fileFullName: "ListadoCarpetaPersonalRecibidos.xlsx",
            header: true,
            maxCellWidth: 30
        });

}
function ControlExportarExcelEnviados() {
    objCarpetaPersonal.exportar = Object.keys(new Object());

    objCarpetaPersonal.exportar.push(new Array(
        { text: "Origen" },
        { text: "Folio" },
        { text: "Destinatario" },
        { text: "DestinatariosExtras" },
        { text: "Asunto" },
        { text: "Fecha" },
        { text: "Importancia" },
        { text: "Adjuntos" },
        { text: "TipoEnvio" },
        { text: "Estado" }
    ));

    $.each(objCarpetaPersonal.info.Datos, function (index, value) {
        objCarpetaPersonal.exportar.push(new Array(
            { text: value.Bandeja_Origen },
            { text: value.Folio },
            { text: value.Destinatario },
            { text: value.Destinatarios_Extra },
            { text: value.Asunto },
            { text: moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') },
            { text: (value.Importancia) ? "Alta" : "Normal" },
            { text: (value.Adjuntos) ? "Si" : "No" },
            { text: (value.TipoEnvio === objTipoEnvio.envio) ? "Envio" : (value.TipoEnvio === objTipoEnvio.turnar) ? "Turnado" : (value.TipoEnvio === objTipoEnvio.respuesta) ? "Respuesta" : "" },
            { text: (value.Estado !== "Respuesta") ? value.Estado : "" }
        ));
    });

    Jhxlsx.export(
        [{
            sheetName: "CarpetaPersonalEnviados",
            data: objCarpetaPersonal.exportar
        }], {
            fileName: "ListadoCarpetaPersonalEnviados",
            extension: ".xlsx",
            sheetName: "Hoja1",
            fileFullName: "ListadoCarpetaPersonalEnviados.xlsx",
            header: true,
            maxCellWidth: 30
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

    $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 bg-light" data-id="0"><div>Bandeja de origen</div></button>'));

    $.each(objCarpetas.info, function (index, value) {
        if (parseInt(value.CarpetaId) === objCarpeta.id) {
            $('#contiene-carpetas').append($('<div class="dropdown-item con-padding-rl-6 bg-light">' + value.Nombre + '</div>'));
        } else {
            $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 bg-light" data-id="' + value.CarpetaId + '"><div>' + value.Nombre + '</div></button>'));
        }
        $('#contiene-carpetas').append($('<div class="dropdown-divider sin-margen"></div>'));

        $.each(value.Subcarpetas, function (i, v) {
            if (parseInt(v.SubcarpetaId) === objCarpeta.id) {
                $('#contiene-carpetas').append($('<div class="dropdown-item con-padding-rl-6 con-padding-l-14 font-dark-gray">' + v.Nombre + '</div>'));
            } else {
                $('#contiene-carpetas').append($('<button type="button" class="carpeta-seleccion dropdown-item hover-bold btn btn-link con-padding-rl-6 con-padding-l-14 font-dark-gray" data-id="' + v.SubcarpetaId + '"><div>' + v.Nombre + '</div></button>'));
            }
            $('#contiene-carpetas').append($('<div class="dropdown-divider sin-margen"></div>'));
        });
    });
}
function ControlCarpetas() {
    $('#contiene-carpetas').on("click", "button.carpeta-seleccion", function () {
        if (objSeleccion.elementos.length > 0) {
            //--
            $.ajax({
                url: objWebRoot.route + "api/v1/carpetas/mover/documentos/" + objBandejasSeleccion.actual,
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
                        if (objBandejasSeleccion.actual === objBandejas.Recibidos) {
                            $('#checkbox_all_recibidos').prop('checked', false);
                        } else if (objBandejasSeleccion.actual === objBandejas.Enviados) {
                            $('#checkbox_all_enviados').prop('checked', false);
                        }
                        //--
                        objSeleccion.elementos = [];
                        ObtenerBandeja(1);
                    }
                }
            });
            //--
        }
    });
}

function ControlTabs() {
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        objBandejasSeleccion.actual = parseInt($(this).data("tipo"));
        ObtenerBandeja(1);
        //--
        objSeleccion.elementos = Object.keys(new Object());
    });
}

function ControlVisualizacion() {
    $('#contiene-tabla table tbody').on("click", "tr a.link-externo", function () {
        window.open(this.href, "_visualiza", "width=960,height=980,menubar=no,resizable=no,status=no", true);
        return false;
    });

    $('#contiene-tabla-enviados table tbody').on("click", "tr a.link-externo", function () {
        window.open(this.href, "_visualiza", "width=960,height=980,menubar=no,resizable=no,status=no", true);
        return false;
    });
}