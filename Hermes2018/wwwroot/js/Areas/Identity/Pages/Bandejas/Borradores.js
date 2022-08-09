/****************************************************************************************************************************************
*                                                   Start script datatable recibidos                                                    *
*****************************************************************************************************************************************/
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };
var objTipoEnvio = { envio: 1, turnar: 2, respuestaParcial: 3, respuesta: 4 };

var objBorradores = { urln: "", info: Object.keys(new Object()), exportar: new Array(), descarga: Object.keys(new Object()) };
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

$(function () {
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
        objBorradores.urln = objWebRoot.route + 'api/v1/documentos/borradores/' + objUser.username + '/' + pagina + '?' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objBorradores.urln = objWebRoot.route + 'api/v1/documentos/borradores/' + objUser.username + '/' + pagina + '?' + 'busqueda=' + objBusqueda.cadena;
    } else if (objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objBorradores.urln = objWebRoot.route + 'api/v1/documentos/borradores/' + objUser.username + '/' + pagina + '?' + 'categoria=' + objCategorias.categoriaId;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length === 0 && objFechas.fin.length === 0)) {
        objBorradores.urln = objWebRoot.route + 'api/v1/documentos/borradores/' + objUser.username + '/' + pagina + '?' + 'busqueda=' + objBusqueda.cadena + '&' + 'categoria=' + objCategorias.categoriaId;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId === 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objBorradores.urln = objWebRoot.route + 'api/v1/documentos/borradores/' + objUser.username + '/' + pagina + '?' + 'busqueda=' + objBusqueda.cadena + '&' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else if (objBusqueda.cadena.length === 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objBorradores.urln = objWebRoot.route + 'api/v1/documentos/borradores/' + objUser.username + '/' + pagina + '?' + 'categoria=' + objCategorias.categoriaId + '&' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else if (objBusqueda.cadena.length > 0 && objCategorias.categoriaId > 0 && (objFechas.inicio.length > 0 && objFechas.fin.length > 0)) {
        objBorradores.urln = objWebRoot.route + 'api/v1/documentos/borradores/' + objUser.username + '/' + pagina + '?' + 'busqueda=' + objBusqueda.cadena + '&' + 'categoria=' + objCategorias.categoriaId + '&' + 'fechaini=' + objFechas.inicio + '&' + 'fechafin=' + objFechas.fin;
    } else {
        objBorradores.urln = objWebRoot.route + 'api/v1/documentos/borradores/' + objUser.username + '/' + pagina;
    }

    //--
    $.ajax({
        url: objBorradores.urln,
        type: 'GET',
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objBorradores.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#Borradores tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            objBorradores.info = Object.keys(new Object());
            objBorradores.info = data;
        },
        error: function (xhr, status, error) {
            objBorradores.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
        },
        complete: function (xhr, status) {
            if ((objBorradores.info.Datos).length > 0) {
                //Rehace la paginación
                objPaginacion.opciones.startPage = pagina;
                objPaginacion.opciones.totalPages = objBorradores.info.Total_Paginas;
                //--Limpia la paginación 
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-borradores').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-borradores').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-borradores').twbsPagination('destroy');
            }

            ProcesaBandeja();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#Borradores tbody').fadeIn(500, "linear");
        }
    });
    //---
}

function ProcesaBandeja() {
    $('#Borradores tbody').empty();
    //--
    if ((objBorradores.info.Datos).length > 0) {

        //Elementos de la página actual
        objElementos.elementospaginaactual = objBorradores.info.Elementos_Pagina_Actual;
        
        $.each(objBorradores.info.Datos, function (index, value) {
            objtr.elementobase = Object.keys(new Object());

            objtr.elementobase = $('<tr>').append(
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Editar/' + value.DocumentoId + '">' + value.Folio + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Editar/' + value.DocumentoId + '">' + value.Destinatario + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Editar/' + value.DocumentoId + '">' + value.Asunto + '</a>')
                ),
                $('<td>').append(
                    (value.NoInterno) ? $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Editar/' + value.DocumentoId + '">' + value.NoInterno + '</a>') : ''
                ),
                $('<td>').append(
                    $('<a class="font-dark" href="' + objWebRoot.route + 'Correspondencia/Editar/' + value.DocumentoId + '">' + moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]') + '</a>')
                ),
                $('<td>').append(
                    (value.Importancia) ? $('<div data-toggle="tooltip" data-placement="top" title="Importancia alta"><i class="fas fa-exclamation font-red"></i></div>') : ''
                ),
                $('<td>').append(
                    (value.Adjuntos) ? $('<div data-toggle="tooltip" data-placement="top" title="Archivos adjuntos disponibles"><i class="fas fa-paperclip font-gray"></i></div>') : ''
                ),
                $('<td>').append(
                    $('<a class="font-dark" title="Eliminar Borrador" href="' + objWebRoot.route + 'Correspondencia/Borrar/' + value.DocumentoId + '"><i class="far fa-trash-alt font-gray"></i></a>')
                )
            );

            $('#Borradores tbody').append(objtr.elementobase);
        });

    } else {
        objtr.elementobase = Object.keys(new Object());
        objtr.elementobase = $('<tr class="table-light">').append(
            $('<td colspan="8" class="text-center">').text("Sin documentos para mostrar")
        );
        $('#Borradores tbody').append(objtr.elementobase);
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
    objBorradores.exportar = Object.keys(new Object());

    $.each(objBorradores.info.Datos, function (index, value) {
        objBorradores.exportar.push({
            folio: value.Folio,
            destinatario: value.Destinatario,
            asunto: value.Asunto,
            fecha: moment(value.Fecha).locale('es').format('DD[/]MM[/]YYYY HH:mm [hrs.]'),
            importancia: (value.Importancia) ? "Alta" : "Normal",
            adjuntos: (value.Adjuntos) ? "Si" : "No"
        });
    });

    objBorradores.descarga = Object.keys(new Object());
    objBorradores.descarga = new jsPDF();

    objBorradores.descarga.autoTable({
        columns: [
            { header: 'Folio', dataKey: 'folio' },
            { header: 'Destinatario', dataKey: 'destinatario' },
            { header: 'Asunto', dataKey: 'asunto' },
            { header: 'Fecha', dataKey: 'fecha' },
            { header: 'Importancia', dataKey: 'importancia' },
            { header: 'Adjuntos', dataKey: 'adjuntos' }],
        body: objBorradores.exportar,
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

    objBorradores.descarga.save('ListadoBorradores.pdf');
}

function ControlExportarExcel() {
    objBorradores.exportar = Object.keys(new Object());

    objBorradores.exportar.push(new Array(
        { text: "Folio" },
        { text: "Destinatario" },
        { text: "Asunto" },
        { text: "Fecha" },
        { text: "Importancia" },
        { text: "Adjuntos" }
    ));

    $.each(objBorradores.info.Datos, function (index, value) {
        objBorradores.exportar.push(new Array(
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
            sheetName: "Borradores",
            data: objBorradores.exportar
        }], {
            fileName: "ListadoBorradores",
            extension: ".xlsx",
            sheetName: "Hoja1",
            fileFullName: "ListadoBorradores.xlsx",
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