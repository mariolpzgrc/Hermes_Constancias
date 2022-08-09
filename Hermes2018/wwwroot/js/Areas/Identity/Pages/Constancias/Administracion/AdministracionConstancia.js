var objFechas = { inicio: '', fin: '', seBusco: false },
    objWebRoot = { route: "", tokem: "" },
    objBusqueda = { cadena: '', seBusco: false },
    objTipoConstancia = { urln: "", info: Object.keys(new Object()), Id: 0 },
    objEstadoConstancia = { urln: "", info: Object.keys(new Object()), Id: 0 },
    objRegion = { urln: "", info: Object.keys(new Object()), Id: 0 },
    objEntidadDependencia = { urln: "", info: Object.keys(new Object()), Id: 0 },
    objSolicitantes = { urln: "", info: Object.keys(new Object()), Id: 0 },
    objtr = { elementobase: Object.keys(new Object()) },
    objSeleccion = { elementos: Object.keys(new Object()), indice: 0, total: 0 },
    objConstanciasSeguimiento = { urln: "", info: Object.keys(new Object()), exportar: new Array(), descarga: Object.keys(new Object()), temporalId: -1, auxiliar: -1, temporalBool: false, caracteristicas: { merges: new Array(), cols: new Array(), rows: new Array() }  },
    objFiltro = {estadoConstancia: 0, fechaInicio: "", fechaFin: "", idConstancia: 0, Busqueda: "", idCampus: 0, noPersonal: 0, folio: "", depedencia: "", tipoPersonal: 0}
    objElementos = { elementospaginaactual: 0 },
    objPaginacion = {
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
                ObtenerConstanciasSolicitadas(page);
            }
        }
    },
    objFechas = { inicio: '', fin: '', seBusco: '' },
    tiposConstancia = { urln: "", info: Object };


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
    ObtenerConstanciasSolicitadas(1);
    ControlFechasConstancias();
    ControlRegion();
    ControlEstadosConstancia();
    ControlTipoConstancia();
    ObtenerTipoConstancias();
    ControlTipoPersonal();
    ControlExportar();
    ControlBusqueda();
});

function ObtenerConstanciasSolicitadas(pagina) {
    objConstanciasSeguimiento.urln = objWebRoot.route + 'api/constancias/GET_FiltersConstancias';

    var filtro = {
        ConstanciaId: objFiltro.idConstancia,
        FechaInicio: objFiltro.fechaInicio,
        FechaFin: objFiltro.fechaFin,
        EstadoId: objFiltro.estadoConstancia,
        Busqueda: objFiltro.Busqueda,
        Pagina: pagina,
        CampusId: objFiltro.idCampus,
        NoPersonal: objFiltro.noPersonal,
        Folio: objFiltro.folio,
        Dependencia: objFiltro.depedencia,
        TipoPersonal: objFiltro.tipoPersonal
    }

    $.ajax({
        url: objConstanciasSeguimiento.urln,
        type: 'POST',
        data: JSON.stringify(filtro),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objConstanciasSeguimiento.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#SolicitudesConstancias tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            objConstanciasSeguimiento.info = Object.keys(new Object());
            objConstanciasSeguimiento.info = data;
        },
        error: function (xhr, estatus, error) {
            objConstanciasSeguimiento.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if ((objConstanciasSeguimiento.info.Elementos).length > 0) {
                //Rehaciendo la paginacion
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objConstanciasSeguimiento.info.TotalPaginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-solicitadas').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-solicitadas').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-solicitadas').twbsPagination('destroy');
            }
            ProcesaSolicitudes();
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#SolicitudesConstancias tbody').fadeIn(500, "linear");
        }
    });
}

function ProcesaSolicitudes() {
    $('#SolicitudesConstancias tbody').empty();

    if ((objConstanciasSeguimiento.info.Elementos).length > 0) {
        objElementos.elementospaginaactual = objConstanciasSeguimiento.info.ElementosPagina;
        $.each(objConstanciasSeguimiento.info.Elementos, function (index, value) {
            objtr.elementobase = Object.keys(new Object());
            objtr.elementobase = $('<tr>').append(
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Constancias/Seguimiento/' + value.Id + '">' + value.Folio.toUpperCase() + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Constancias/Seguimiento/' + value.Id + '">' + value.NoPersonal + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Constancias/Seguimiento/' + value.Id + '">' + value.NombreUsuario.toUpperCase() + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Constancias/Seguimiento/' + value.Id + '">' + value.Dependencia + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Constancias/Seguimiento/' + value.Id + '">' + value.NombreCampus + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Constancias/Seguimiento/' + value.Id + '">' + value.NombreConstancia + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Constancias/Seguimiento/' + value.Id + '">' + moment(value.FechaSolicitud).locale('es').format('DD[/]MM[/]YYYY') + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Constancias/Seguimiento/' + value.Id + '">' + value.NombreEstado + '</a>')
                ),
                $('<td>').append(
                    $('<button data-toggle="tooltip" data-placement="top" title="Ver Vista Previa" type="button" class="btn btn-link btn-sm" onclick="iraLectura(' + value.Id + ',' + value.ConstanciaId + ')"><i class="fas fa-eye font-gray"></i></button>')
                ),
            )
            $('#SolicitudesConstancias tbody').append(objtr.elementobase);
        });

    } else {
        objtr.elementobase = Object.keys(new Object());
        objtr.elementobase = $('<tr class="table-light">').append(
            $('<td colspan="8" class="text-center">').text("Sin constancias para mostrar")
        );
        $('#SolicitudesConstancias tbody').append(objtr.elementobase);
    }
}

function ControlFechasConstancias() {
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

        //--Fechas para mostrar en el dialogo
        objFechas.inicio = picker.startDate.format('DD/MM/YYYY');
        objFechas.fin = picker.endDate.format('DD/MM/YYYY');

        //--Fechas para mandar al servicio
        objFiltro.fechaInicio = picker.startDate.format('YYYY-MM-DD');
        objFiltro.fechaFin = picker.endDate.format('YYYY-MM-DD');
        //--
        objFechas.seBusco = true;
        ObtenerConstanciasSolicitadas(1);
    });

    $('input[name="datepicker1"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');

        //--Fecha para limpiar el dialogo
        objFechas.inicio = '';
        objFechas.fin = '';
        //--Fecha para limpiar el servicio
        objFiltro.fechaInicio = "";
        objFiltro.fechaFin = "";
        //--
        if (objFechas.seBusco) {
            objFechas.seBusco = false;
            ObtenerConstanciasSolicitadas(1);
        }
    });
}

function ControlFiltros() {
    $("#select-region-constancia").change(function () {
        objFiltro.idCampus = parseInt($("#select-region-constancia option:selected").val());
    });
    $("#select-estados-constancia").change(function () {
        objFiltro.estadoConstancia = parseInt($("#select-estados-constancia option:selected").val());
    });
    $("#select-tipos-constancia").change(function () {
        objFiltro.idConstancia = parseInt($("#select-tipos-constancia option:selected").val());
    });
    ObtenerConstanciasSolicitadas(1);
}

function ControlRegion() {
    $("#select-region-constancia").change(function () {
        objFiltro.idCampus = parseInt($("#select-region-constancia option:selected").val());
        ObtenerConstanciasSolicitadas(1)
    });
}

function ControlEstadosConstancia() {
    $("#select-estados-constancia").change(function () {
        objFiltro.estadoConstancia = parseInt($("#select-estados-constancia option:selected").val());
        ObtenerConstanciasSolicitadas(1)
    });
}

function ControlTipoConstancia() {
    $("#select-tipos-constancia").change(function () {
        objFiltro.idConstancia = parseInt($("#select-tipos-constancia option:selected").val());
        ObtenerConstanciasSolicitadas(1)
    });
}

function ControlTipoPersonal(){
    $("#select-tipo-personal").change(function () {
        objFiltro.tipoPersonal = parseInt($("#select-tipo-personal option:selected").val());
        ObtenerConstanciasSolicitadas(1);
    });
}

function LimpiarBusqueda() {
    objFiltro.estadoConstancia = 0; objFiltro.fechaInicio = ""; objFiltro.fechaFin = "", objFiltro.idConstancia = 0, objFiltro.Busqueda = "", objFiltro.idCampus = 0; objFiltro.tipoPersonal = 0;
    $("#select-region-constancia").val(0);
    $("#select-estados-constancia").val(0);
    $("#select-tipos-constancia").val(0);
    $("#select-tipo-personal").val(0);
    $('input[name="datepicker1"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        //--Fecha para limpiar el dialogo
        objFechas.inicio = '';
        objFechas.fin = '';
        //--Fecha para limpiar el servicio
        objFiltro.fechaInicio = "";
        objFiltro.fechaFin = "";
        //--
        if (objFechas.seBusco) {
            objFechas.seBusco = false;
        }
    });
    ObtenerConstanciasSolicitadas(1)
}

function ControlBusqueda() {
    jQuery(document).on('keyup', 'input#searchbox', function (ev) {
        objBusqueda.cadena = $(this).val().trim();
        if (ev.which === 13) {
            if (objBusqueda.cadena.length === 5) {
                objFiltro.noPersonal = parseInt(objBusqueda.cadena);
                objBusqueda.seBusco = true;
                ObtenerConstanciasSolicitadas(1)
            } else if (objBusqueda.cadena.length === 11) {
                objFiltro.folio = objBusqueda.cadena.toLowerCase();
                objBusqueda.seBusco = true;
                ObtenerConstanciasSolicitadas(1)
            } else {
                objFiltro.depedencia = objBusqueda.cadena;
                objBusqueda.seBusco = true;
                ObtenerConstanciasSolicitadas(1)
            }
            return false;
        }

        if (objBusqueda.cadena.length > 0) {
            $('#btn-x-search').removeClass('d-none');
        } else {
            $('#btn-x-search').removeClass('d-none').addClass('d-none');

            objBusqueda.cadena = "";
            if (objBusqueda.seBusco) {
                objBusqueda.seBusco = false;
                objFiltro.depedencia = ""; objFiltro.noPersonal = 0; objFiltro.folio = "";
                ObtenerConstanciasSolicitadas(1)
            }
        }
    });

    $('#btn-x-search').on("click", function () {
        $('input#searchbox').val("");
        $('#btn-x-search').removeClass('d-none').addClass('d-none');
        objBusqueda.cadena = "";
        if (objBusqueda.seBusco) {
            objBusqueda.seBusco = false;
            objFiltro.depedencia = ""; objFiltro.noPersonal = 0; objFiltro.folio = "";
            ObtenerConstanciasSolicitadas(1);
        }
    });
}

function iraLectura(idConstanciaSolcitada, idConstancia) {
    let lecturaParams = {
        ConstanciaSolicitadaId: idConstanciaSolcitada, ConstanciaId: idConstancia
    }
    localStorage.setItem('lecturaParams', JSON.stringify(lecturaParams));
    window.location.href = "Lectura/" + idConstanciaSolcitada + "/" + idConstancia + "/";
}

function ObtenerTipoConstancias() {
    objTipoConstancia.urln = objWebRoot.route + 'api/constancias/Get_HER_Constancias';
    $.ajax({
        url: objTipoConstancia.urln,
        data: JSON.stringify({ TipoPersonal: 1 }),
        type: 'POST',
        contentType: "application/json; charset = utf-8",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            objTipoConstancia.info = Object.keys(new Object());
            objTipoConstancia.info = data;
        },
        error: function (xhr, status, error) {
            objTipoConstancia.info = Object.keys(new Object());
        },
        complete: function (xhr, estatus) {
            if ((objTipoConstancia.info).length > 0) {
                $('#select-tipos-constancia').empty();
                $('#select-tipos-constancia').append($('<option></option>').val("0").html("[Todos los tipos de constancia]"));
                $.each(objTipoConstancia.info, function (index, constancia) {
                    $('#select-tipos-constancia').append($('<option></option>').val(constancia.Id).html(constancia.Nombre));
                });
            } else {
                $('#select-tipos-constancia').empty();
                $('#select-tipos-constancia').append($('<option></option>').val("0").html("[Todos los tipos de constancia]"));
            }
        }
    });
}

function ObtenerEntidadDependencia() {
    objEntidadDependencia.urln = objWebRoot.route + '';
    $.ajax({
        url: objEntidadDependencia.urln,
        data: JSON.stringify(),
        type: 'POST',
        contentType: "application/json; charset = utf-8",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            objEntidadDependencia.info = Object.keys(new Object());
            objRobjEntidadDependenciaegion.info = data;
        },
        error: function (xhr, estatus) {
            objEntidadDependencia.info = Object.keys(new Object());
        },
        complete: function (xhr, estatus) {
            if ((objEntidadDependencia.info).length > 0) {
                $('#select-ed-constancia').empty();
                $('#select-ed-constancia').append($('<option></option>').val(0).html("[Todos los estados de constancia]"));
            } else {
                $('#select-ed-constancia').empty();
                $('#select-ed-constancia').append($('<option></option>').val(0).html("[Todos los estados de constancia]"));
            }
        }
    });
}

function ControlExportar() {
    $('#exportar-pdf').click(function () {
        ControlExportarPDF();
    });

    $('#exportar-excel').click(function () {
        ControlExportarExcel();
    });
}

function ControlExportarPDF() {
    objConstanciasSeguimiento.exportar = Object.keys(new Object());
    objConstanciasSeguimiento.info.Elementos.sort(function (a, b) {
        if (a.Id > b.Id || a.FechaSolicitud > b.FechaSolicitud) {
            return 1
        }
        if (a.Id < b.Id || a.FechaSolicitud < b.FechaSolicitud) {
            return -1
        }
        return 0;
    });

    $.each(objConstanciasSeguimiento.info.Elementos, function (index, value) {
        objConstanciasSeguimiento.auxiliar = (value.Id == null) ? 0 : value.Id;

        if (value.NombreConstancia != null) {
            if (objConstanciasSeguimiento.auxiliar != objConstanciasSeguimiento.auxiliar) {
                objConstanciasSeguimiento.exportar.push(
                    new Array({
                        content: "TIPO DE CONSTANCIA: " + value.NombreConstancia.toUpperCase(), rowSpan: 0, colSpan: 11, styles: {halign: 'center', valign: 'middle', fillColor: [153,206,212]}
                    })
                )
            }
        }

        objConstanciasSeguimiento.exportar.push(
            new Array(
                { content: value.NombreConstancia.toUpperCase(), rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle', fillColor: [153, 206, 212] } },
                { content: value.NombreEstado, rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: value.Folio.toUpperCase(), rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: value.NombreUsuario.toUpperCase(), rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: value.Dependencia, rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: value.NombreCampus, rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: moment(value.FechaSolicitud).locale('es').format('DD[/]MM[/]YYYY'), rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } },
                { content: (value.FechaEntrega) ? moment(value.FechaEntrega).locale('es').format('DD[/]MM[/]YYYY') : "  ", rowSpan: 0, colSpan: 0, styles: { halign: 'center', valign: 'middle' } }
            )
        );

        objConstanciasSeguimiento.temporalId = (value.Id == null) ? 0 : value.Id;

        objConstanciasSeguimiento.descarga = Object.keys(new Object());
        objConstanciasSeguimiento.descarga = new jsPDF({
            orientation: "landscape",
            format: 'letter'
        });

        objConstanciasSeguimiento.descarga.autoTable({
            head: [
                [
                    { content: 'Tipo de constancia', rowSpan: 0, colSpan: 0 },
                    { content: 'Estado', rowSpan: 0, colSpan: 0 },
                    { content: 'Folio', rowSpan: 0, colSpan: 0 },
                    { content: 'Nombre del Personal', rowSpan: 0, colSpan: 0 },                  
                    { content: 'Entidad/Depedencia', rowSpan: 0, colSpan: 0 },
                    { content: 'Región', rowSpan: 0, colSpan: 0 },
                    { content: 'Fecha de solicitud', rowSpan: 0, colSpan: 0 },
                    { content: 'Fecha de entrega', rowSpan: 0, colSpan: 0 }
                ]
            ],
            body: objConstanciasSeguimiento.exportar,
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
    });

    var currentTimeStamp = Date.parse(new Date());

    objConstanciasSeguimiento.descarga.save('Constancias' + currentTimeStamp + '.pdf');
}

function ControlExportarExcel() {
    objConstanciasSeguimiento.exportar = Object.keys(new Object());

    objConstanciasSeguimiento.info.Elementos.sort(function (a, b) {
        if (a.Id > b.Id || a.FechaSolicitud > b.FechaSolicitud) {
            return 1
        }

        if (a.Id < b.Id || a.FechaSolicitud < b.FechaSolicitud) {
            return -1
        }

        return 0;
    });

    objConstanciasSeguimiento.exportar.push(
        {
            "Constancia": "Constancia",
            "Estado": "Estado",
            "Folio": "Folio",
            "Nombre del Personal": "Nombre del Personal",
            "Entidad/Depedencia": "Entidad/Depedencia",
            "Región": "Región",
            "Fecha de solicitud": "Fecha de solicitud",
            "Fecha de entrega" : "Fecha de entrega"
        }
    );

    objConstanciasSeguimiento.caracteristicas.merges.push(
        { s: { r: 0, c: 8 }, e: { r: 1, c: 8 } },
        { s: { r: 0, c: 7 }, e: { r: 1, c: 7 } },
        { s: { r: 0, c: 6 }, e: { r: 1, c: 6 } },
        { s: { r: 0, c: 5 }, e: { r: 1, c: 5 } },
        { s: { r: 0, c: 4 }, e: { r: 1, c: 4 } },
        { s: { r: 0, c: 3 }, e: { r: 1, c: 3 } },
        { s: { r: 0, c: 2 }, e: { r: 1, c: 2 } },
        { s: { r: 0, c: 1 }, e: { r: 1, c: 1 } },
        { s: { r: 0, c: 0 }, e: { r: 1, c: 0 } }
    );

    $.each(objConstanciasSeguimiento.info.Elementos, function (index, value) {
        objConstanciasSeguimiento.auxiliar = (value.Id) ? 0 : value.Id;


        objConstanciasSeguimiento.exportar.push({
            "Constancia": value.NombreConstancia.toUpperCase(),
            "Estado": value.NombreEstado,
            "Folio": value.Folio.toUpperCase(),
            "Nombre del Personal": value.NombreUsuario.toUpperCase(),
            "Entidad/Depedencia": value.Dependencia,
            "Región": value.NombreCampus,
            "Fecha de solicitud": moment(value.FechaSolicitud).locale('es').format('DD[/]MM[/]YYYY'),
            "Fecha de entrega": (value.FechaEntrega) ? moment(value.FechaEntrega).locale('es').format('DD[/]MM[/]YYYY') : "  "
        });

        objConstanciasSeguimiento.temporalId = (value.Id) ? 0 : value.Id;
    });

    console.log(objConstanciasSeguimiento.exportar);

    var wb = XLSX.utils.book_new();
    var ws = XLSX.utils.json_to_sheet(objConstanciasSeguimiento.exportar, { skipHeader: false });
    console.log(ws);
    wb.Props = {
        Title: "Constancias",
        Subject: "HERMES",
        Author: "UV"
    };
    wb.SheetNames.push("Constancias solicitadas");
    ws['!merges'] = objConstanciasSeguimiento.caracteristicas.merges;
    wb.Sheets["Constancias solicitadas"] = ws;

    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
    var currentTimeStamp = Date.parse(new Date());

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'Constancias' + currentTimeStamp + '.xlsx');
}