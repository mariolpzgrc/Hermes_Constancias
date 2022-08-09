/****************************************************************************************************************************************
*                                                   Start script datatable recibidos                                                    *
*****************************************************************************************************************************************/
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };

var objRevision = { urln: "", info: Object.keys(new Object()), exportar: new Array(), descarga: Object.keys(new Object()) };
var objtr = { elementobase: Object.keys(new Object()) };
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

$(document).ready(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    //--
    ObtenerBandeja(1);
    ControlBusqueda();

    ObtenerCategorias();
    ControlFechas();
    ControlExportar();
    //--
});

function ObtenerBandeja(pagina) {

    if (objBusqueda.cadena.length === 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRevision.urln = objWebRoot.route + 'api/v1/documentos/revision/' + objUser.username + '/' + pagina + '?' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRevision.urln = objWebRoot.route + 'api/v1/documentos/revision/' + objUser.username + '/' + pagina + '?' + 'busqueda=' + objBusqueda.cadena;
    } else if (objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRevision.urln = objWebRoot.route + 'api/v1/documentos/revision/' + objUser.username + '/' + pagina + '?' + 'categoria=' + objCategorias.categoriaId;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objRevision.urln = objWebRoot.route + 'api/v1/documentos/revision/' + objUser.username + '/' + pagina + '?' + 'busqueda=' + objBusqueda.cadena + '&' + 'categoria=' + objCategorias.categoriaId;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRevision.urln = objWebRoot.route + 'api/v1/documentos/revision/' + objUser.username + '/' + pagina + '?' + 'busqueda=' + objBusqueda.cadena + '&' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else if (objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRevision.urln = objWebRoot.route + 'api/v1/documentos/revision/' + objUser.username + '/' + pagina + '?' + 'categoria=' + objCategorias.categoriaId + '&' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objRevision.urln = objWebRoot.route + 'api/v1/documentos/revision/' + objUser.username + '/' + pagina + '?' + 'busqueda=' + objBusqueda.cadena + '&' + 'categoria=' + objCategorias.categoriaId + '&' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else {
        objRevision.urln = objWebRoot.route + 'api/v1/documentos/revision/' + objUser.username + '/' + pagina;
    }
    
    //--
    $.ajax({
        url: objRevision.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objRevision.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#Revision tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            objRevision.info = Object.keys(new Object());
            objRevision.info = data;
        },
        error: function (xhr, status, error) {
            objRevision.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if ((objRevision.info.Datos).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objRevision.info.Total_Paginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-revision').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-revision').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-revision').twbsPagination('destroy');
            }

            ProcesaBandeja();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#Revision tbody').fadeIn(500, "linear");
        }
    });
    //--
}

function ProcesaBandeja() {
    $('#Revision tbody').empty();
    //--
    if ((objRevision.info.Datos).length > 0) {

        //Elementos de la página actual
        objElementos.elementospaginaactual = objRevision.info.Elementos_Pagina_Actual;
        
        $.each(objRevision.info.Datos, function (index, value) {
            objtr.elementobase = Object.keys(new Object());

            objtr.elementobase = $('<tr>').append(
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Revision/' + value.RevisionId + '">' + value.Folio + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Revision/' + value.RevisionId + '">' + value.Destinatario + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Revision/' + value.RevisionId + '">' + value.Asunto + '</a>')
                ),
                $('<td>').append(
                    (value.NoInternol) ? $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Revision/' + value.EnvioId + '/' + value.TipoEnvio + '">' + value.NoInterno + '</a>') : ''
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Revision/' + value.RevisionId + '">' + moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</a>')
                ),
                $('<td>').append(
                    (value.Importancia) ? $('<div data-toggle="tooltip" data-placement="top" title="Importancia alta"><i class="fas fa-exclamation font-red"></i></div>') : ''
                ),
                $('<td>').append(
                    (value.Adjuntos) ? $('<div data-toggle="tooltip" data-placement="top" title="Archivos adjuntos disponibles"><i class="fas fa-paperclip font-gray"></i></div>') : ''
                )
            );

            $('#Revision tbody').append(objtr.elementobase);
        });

    } else {
        objtr.elementobase = Object.keys(new Object());
        objtr.elementobase = $('<tr class="table-light">').append(
            $('<td colspan="6" class="text-center">').text("Sin documentos para mostrar")
        );
        $('#Revision tbody').append(objtr.elementobase);
    }
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
/**** ***************** ****/

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

function ControlExportarPDF() {
    objRevision.exportar = Object.keys(new Object());

    $.each(objRevision.info.Datos, function (index, value) {
        objRevision.exportar.push({
            folio: value.Folio,
            destinatario: value.Destinatario,
            asunto: value.Asunto,
            fecha: moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]'),
            importancia: (value.Importancia) ? "Alta" : "Normal",
            adjuntos: (value.Adjuntos) ? "Si" : "No"
        });
    });

    objRevision.descarga = Object.keys(new Object());
    objRevision.descarga = new jsPDF();

    objRevision.descarga.autoTable({
        columns: [
            { header: 'Folio', dataKey: 'folio' },
            { header: 'Destinatario', dataKey: 'destinatario' },
            { header: 'Asunto', dataKey: 'asunto' },
            { header: 'Fecha', dataKey: 'fecha' },
            { header: 'Importancia', dataKey: 'importancia' },
            { header: 'Adjuntos', dataKey: 'adjuntos' }],
        body: objRevision.exportar,
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

    objRevision.descarga.save('ListadoRevision.pdf');
}

function ControlExportarExcel() {
    objRevision.exportar = Object.keys(new Object());

    objRevision.exportar.push(new Array(
        { text: "Folio" },
        { text: "Destinatario" },
        { text: "Asunto" },
        { text: "Fecha" },
        { text: "Importancia" },
        { text: "Adjuntos" }
    ));

    $.each(objRevision.info.Datos, function (index, value) {
        objRevision.exportar.push(new Array(
            { text: value.Folio },
            { text: value.Destinatario },
            { text: value.Asunto },
            { text: moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') },
            { text: (value.Importancia) ? "Alta" : "Normal" },
            { text: (value.Adjuntos) ? "Si" : "No" }
        ));
    });

    Jhxlsx.export(
        [{
            sheetName: "Revisión",
            data: objRevision.exportar
        }], {
            fileName: "ListadoRevision",
            extension: ".xlsx",
            sheetName: "Hoja1",
            fileFullName: "ListadoRevision.xlsx",
            header: true,
            maxCellWidth: 30
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