var objWebRoot = { route: "", token: "" },
    objTipoPersonal = { urln: "", info: Object.keys(new Object()), Id: 0 },
    objTipoPersonalByUser = { urln: "", info: Object.keys(new Object()), Id: 0 },
    objTipoConstancia = { urln: "", info: Object.keys(new Object()), Id: 0 },
    objUser = { username: "", usersession: "", userTipo: "", numeroPersonal: 0 },
    objtr = { elementobase: Object.keys(new Object()) },
    objSeleccion = { elementos: Object.keys(new Object()), indice: 0, total: 0 },
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
                ObtenerSeguimientosConstancia(page);
            }
        }
    },
    objConstanciasSeguimiento = { urln: "", info: Object.keys(new Object()), exportar: new Array(), descarga: Object.keys(new Object()) };


$(document).ready(function () {
    objUser.username = $("#user").text();
    objUser.usersession = $('#FolioSession').val();
    ObtenerTipodePersonalByUser(objUser.username);
    ObtenerSeguimientosConstancia(1);
});

function ObtenerTipodePersonalByUser(username) {
    objTipoPersonalByUser.urln = objWebRoot.route + 'api/constancias/ObtieneCveLogin_TP'
    $.ajax({
        url: objTipoPersonalByUser.urln,
        data: JSON.stringify({ sCveLogin: username }),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (data) {
                objUser.numeroPersonal = data.oLoginTP.iNumPer;
                objTipoPersonalByUser.info = Object.keys(new Object());
                objTipoPersonalByUser.info = data.oLoginTP.TipoPersonal;
            }
        },
        error: function (xhr, estatus, error) {
            objTipoPersonalByUser.info = Object.keys(new Object());
        },
        complete: function (xhr, estatus) {
            if ((objTipoPersonalByUser.info).length > 0) {
                $('#select-tipos-personal').empty();
                $('#select-tipos-personal').append($('<option></option>').val("0").html("Seleccione el tipo de personal."));
                $.each(objTipoPersonalByUser.info, function (index, tipoPersonal) {
                    $('#select-tipos-personal').append($('<option></option>').val(tipoPersonal.Id).html(tipoPersonal.Id + " - " + tipoPersonal.TipoPersonal));
                });
            } else {
                $('#select-tipos-personal').empty();
                $('#select-tipos-personal').append($('<option></option>').val("0").html("Seleccione el tipo del tipo de personal."));
            }
        }
    });

    $("#select-tipos-personal").change(function () {
        objTipoPersonalByUser.Id = parseInt($("#select-tipos-personal option:selected").val());
        ObtenerConstancias(objTipoPersonalByUser.Id);
    });
}


function ObtenerConstancias(tipoPersonal) {
    var urlservicio = objWebRoot.route + 'api/constancias/Get_HER_Constancias';
    var listaConstancias = document.getElementById("lista-tipo-Constancia")
    $.ajax({
        url: urlservicio,
        data: JSON.stringify({ TipoPersonal: tipoPersonal, UserId: objUser.username}),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (xhr, status, error) {
            objTipoConstancia.info = Object.key(new Object());

        },
        success: function (data) {
            if (data) {
                objTipoConstancia.info = Object.keys(new Object());
                objTipoConstancia.info = data;
            } else {
                $('#lista-tipo-Constancia').empty();
                $('#lista-tipo-Constancia').append(
                    //$('<h3>Constancias no disponibles</h3>')
                    $('<label class="list-group-item border-0 bg-light text-center font-weight-bold">Error al cargar las constancias</label>')
                );
            }
        },
        complete: function (xhr, estatus) {
            if ((objTipoConstancia.info).length > 0) {
                $('#lista-tipo-Constancia').empty();
                $('#lista-tipo-Constancia').append($('<label class="list-group-item border-0 bg-light text-center font-weight-bold">Constancias</label>'));
                $.each(objTipoConstancia.info, function (index, tipo) {
                    if (!tipo.SolicitudActiva) {
                        $('#lista-tipo-Constancia').append('<label class="list-group-item border-0 list-group-item-action"><input class="form-check-input me-1" type="radio" name="constancia" value="' + tipo.Id + '" >' + tipo.Nombre + '</label>');
                    } else {
                        $('#lista-tipo-Constancia').append('<label class="list-group-item border-0 list-group-item-action disabled"><input class="form-check-input me-1" type="radio" name="constancia" value="' + tipo.Id + '" disabled>' + tipo.Nombre + '</label>');
                    }
                })
            } else {
                $('#lista-tipo-Constancia').empty();
                $('#lista-tipo-Constancia').append(
                    //$('<h3>Constancias no disponibles</h3>')
                    $('<label class="list-group-item border-0 bg-light text-center font-weight-bold">Constancias no disponibles</label>')
                );
            }

        }
    });
}

function validarConstancia() {
    let idTipoConstancia = $('input[name="constancia"]:checked').val();

    var radiocheck = "";

    for (i = 0; i < document.getElementsByName('constancia').length; i++) {
        if (document.getElementsByName('constancia')[i].checked) {
            radiocheck = document.getElementsByName('constancia')[i].value;
        }
    }

    if (radiocheck !== "") {
        var queryParams = {
            numeroPersonal: objUser.numeroPersonal, tipoContancia: idTipoConstancia, tipoPersonal: objTipoPersonalByUser.Id
        }
        localStorage.setItem('queryParams', JSON.stringify(queryParams));
        window.location.href = "Resumen/" + idTipoConstancia + "/" + objTipoPersonalByUser.Id + "/";
    } else {
        alert("Debe escoger un tipo de constancia.")
    }
}

function ObtenerSeguimientosConstancia(pagina) {
    console.log("entro aqui,", pagina);
    objConstanciasSeguimiento.urln = objWebRoot.route + 'api/constancias/GET_HER_SolicitudConstancia';

    $.ajax({
        url: objConstanciasSeguimiento.urln,
        type: 'POST',
        data: JSON.stringify({ UsuarioId: objUser.username, NumPagina: pagina }),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
            //--
            objConstanciasSeguimiento.info = Object.keys(new Object());
            $('#contiene-cargador .loader').removeClass('d-none');
            $('#Constacias-historico tbody').fadeOut(100, "linear");
        },
        success: function (data) {
            console.log(data);
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
                $('#paginacion-costancias').twbsPagination('destroy');
                //--Crea la paginación
                $('#paginacion-costancias').twbsPagination(objPaginacion.opciones);
            } else {
                $('#contenedor-paginacion ul').empty();
                $('#paginacion-costancias').twbsPagination('destroy');
            }

            ProcesaSeguimiento();

            $('#contiene-cargador .loader').removeClass('d-none').addClass('d-none');
            $('#Constacias-historico tbody').fadeIn(500, "linear");
        }
    });
}

function ProcesaSeguimiento() {
    $('#Constacias-historico tbody').empty();

    if ((objConstanciasSeguimiento.info.Elementos).length > 0) {
        objElementos.elementospaginaactual = objConstanciasSeguimiento.info.PaginaActual;

        $.each(objConstanciasSeguimiento.info.Elementos, function (index, value) {
            objtr.elementobase = Object.keys(new Object());

            objtr.elementobase = $('<tr>').append(
                $('<td>').append(
                    $('<a class="font-dark">' + value.Folio.toUpperCase() + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark">' + value.NombreConstancia + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark">' + value.NombreEstado + '</a>')
                ),
                $('<td>').append(
                    $('<a class="font-dark">' + moment(value.FechaSolicitud).locale('es').format('DD[/]MM[/]YYYY') + '</a>')
                ),
                /*$('<td>').append(
                    $('<a class="font-dark">' +  value.Mensaje  + '</a>')
                ),*/
                $('<td>').append(
                    (value.FechaEntrega) ? $('<a class="font-dark">' + moment(value.FechaEntrega).locale('es').format('DD[/]MM[/]YYYY') + '</a>') : ''
                ),
            )

            $('#Constacias-historico tbody').append(objtr.elementobase);
        });

        objSeleccion.total = $('#Constacias-historico tbody tr th input:checked').length;
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
            $('<td colspan="8" class="text-center">').text("Sin constancias para mostrar")
        );
        $('#Constacias-historico tbody').append(objtr.elementobase);
    }
}

/***
 * Funciones de utileria
 * ***/