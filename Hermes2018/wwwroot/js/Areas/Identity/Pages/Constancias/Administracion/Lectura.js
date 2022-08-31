var objWebRoot = { route: "", token: "" },
    objUser = { username: "", usersession: "" },
    objConstanciaParams = {IdConstanciaSolicitud: 0, IdConstanciaRec: 0 },
    objInfoUserConstancia = { NoPersonal: 0, tipoPersonal: 0, tipoConstancia: 0, username: "", estado: 0, fechaAutorizada: "", folio: ""},
    objSegConstancia = { Id: 0, info: Object.keys(new Object()), urln: "", CveDep: 0, NombreDep: "", Fecha: "", director: "" },
    objGetInfoConstancia = { Id: 0, info: Object.keys(new Object()), urln: "" },
    options = { year: 'numeric', month: 'long', day: 'numeric' },
    monedaMXN = { plural: "PESOS", singular: "PESO", centPlural: "CENTAVOS", centSingular: "CENTAVO" },
    objGetInfoUSerConstancia = { Id: 0, info: Object.keys(new Object()), urln: "" },
    objGetInfoConstanciaSolicitada = { Id: 0, info: Object.keys(new Object()), urln: "" },
    vaReimprimir = false;

const Funcionario = 1, ATM = 2, Confianza = 3, Academico = 4, Eventual = 5;

$(document).ready(function () {
    objUser.username = $("#user").text();
    objUser.usersession = $('#FolioSession').val();
    getData();
    ObtenerInfoConstancia();
});

function getData() {
    var getInfo = JSON.parse(localStorage.getItem('lecturaParams'));
    if (getInfo === null) {
        window.history.back();
    } else {
        objConstanciaParams.IdConstanciaSolicitud = getInfo.ConstanciaSolicitadaId;
        objConstanciaParams.IdConstanciaRec = getInfo.ConstanciaId;
    }
}

function ObtenerInfoConstancia() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/GET_ConstanciaSolicitadaId';
    $.ajax({
        url: objGetInfoConstancia.urln,
        type: 'POST',
        data: JSON.stringify({ Id: objConstanciaParams.IdConstanciaSolicitud }),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (data) {
                objGetInfoConstancia.info = Object.keys(new Object());
                objGetInfoConstancia.info = data;
                $('#folio').text(Mayuscula(objGetInfoConstancia.info.Folio));
                $('#tipoConstancia').text(objGetInfoConstancia.info.NombreConstancia);
                $('#idtipoConstancia').text(objGetInfoConstancia.info.ConstanciaId)
                $('#tituloConstancia').text(objGetInfoConstancia.info.NombreConstancia);
                $('#constanciaPara').text(objGetInfoConstancia.info.NombreUsuario.toUpperCase() + ' (' + objGetInfoConstancia.info.UsuarioId + ')');
                objInfoUserConstancia.NoPersonal = objGetInfoConstancia.info.NoPersonal;
                objGetInfoConstancia.tipoPersonal = objGetInfoConstancia.info.TipoPersonal;
                objInfoUserConstancia.tipoConstancia = objGetInfoConstancia.info.ConstanciaId;
                objInfoUserConstancia.username = objGetInfoConstancia.info.UsuarioId;
                objInfoUserConstancia.estado = objGetInfoConstancia.info.EstadoId;
                OcultarElementos(objInfoUserConstancia.estado);
                ObtenerConstancia(objInfoUserConstancia.tipoConstancia);
            }
        },
        error: function () {
            alert('No pudó terminar la operación');
        },
        complete: function (xhr, estatus) {
            $.each(objGetInfoConstancia.info.EstadosConstancia, function (index, value) {
                if (value.EstadoId == 2) {
                    objInfoUserConstancia.fechaAutorizada = objGetInfoConstancia.info.EstadosConstancia[index].FechaHora;
                    $('#fechaGeneracion').text(new Date(objInfoUserConstancia.fechaAutorizada).toLocaleDateString("es-ES", options));
                }
            });
        }
    });
}

function OcultarElementos(estado) {
    if (estado === 1) {
        document.getElementById("getConstancia").style.display = 'none';
        document.getElementById("reimprimirConstancia").style.display = "none";
        document.getElementById("entregarConstancia").style.display = "none";
        document.getElementById("seccionDirector").style.display = "none";
    } else if (estado === 2) {
        document.getElementById("confirmarConstancia").style.display = 'none';
        document.getElementById("cancelarConstancia").style.display = 'none';
        document.getElementById("entregarConstancia").style.display = "none";
        document.getElementById("reimprimirConstancia").style.display = "none";
    } else if (estado === 3) {
        document.getElementById("confirmarConstancia").style.display = 'none';
        document.getElementById("cancelarConstancia").style.display = 'none';
        document.getElementById("getConstancia").style.display = "none";
    } else if (estado === 4) {
        document.getElementById("confirmarConstancia").style.display = 'none';
        document.getElementById("getConstancia").style.display = 'none';
        document.getElementById("cancelarConstancia").style.display = "none";
    } else if (estado === 5) {
        document.getElementById("confirmarConstancia").style.display = 'none';
        document.getElementById("cancelarConstancia").style.display = 'none';
        document.getElementById("getConstancia").style.display = 'none';
    } else if (estado === 6) {
        document.getElementById("getConstancia").style.display = 'none';
        document.getElementById("cancelarConstancia").style.display = 'none';
        document.getElementById("confirmarConstancia").style.display = 'none';
        document.getElementById("reimprimirConstancia").style.display = "none";
        document.getElementById("entregarConstancia").style.display = "none";
    }
}

function ObtenerConstancia(id) {
    parseInt(id);
    switch (id) {
        case 1:
            if (objInfoUserConstancia.tipoPersonal === Funcionario || objInfoUserConstancia.tipoPersonal === Confianza || objInfoUserConstancia.tipoPersonal === Academico) {
                ObtenerConstanciaServicioMedico();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 2:
            if (objInfoUserConstancia.tipoPersonal === Funcionario || objInfoUserConstancia.tipoPersonal === Confianza || objInfoUserConstancia.tipoPersonal === Academico) {
                ObtenerConstanciaServcioMedicoDep();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 3:
            ObtenerConstanciaTrabajoPercepciones();
            break;
        case 4:
            ObtenerConstaHorarioLaboral();
            break;
        case 5:
            if (objInfoUserConstancia.tipoPersonal != Eventual) {
                ObtenerConstanciaAltaAfiliacionIPE();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 6:
            if (objInfoUserConstancia.tipoPersonal != Eventual) {
                ObtenerConstanciaAltaAfiliacionMagisterio();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 7:
            if (objInfoUserConstancia.tipoPersonal != Eventual) {
                ObtenerConstanciaBajaAfiliacionIPE();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 8:
            if (objInfoUserConstancia.tipoPersonal != Eventual) {
                ObtenerConstanciaBajaAfiliacionMagisterio
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 9:           
                ObtenerConstanciaVisa();
            break;
        case 10:
            if (objInfoUserConstancia.tipoPersonal === Funcionario || objInfoUserConstancia.tipoPersonal === Confianza || objInfoUserConstancia.tipoPersonal === Academico) {
                ObtenerConstanciaVisaDep();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 11:
            if (objInfoUserConstancia.tipoPersonal === Academico) {
                ObtenerConstanciaPRODep();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 12:
            if (objInfoUserConstancia.tipoPersonal === Academico) {
                ObtenerConstanciaCurricular();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 13:
            if (objInfoUserConstancia.tipoPersonal !== Eventual) {
                ObtenerHojaDeServicios();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case 14:
            if (objInfoUserConstancia.tipoPersonal !== Eventual) {
                ObtenerContanciaJubicacion();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
    }
}

function ObtenerConstanciaServicioMedico() {
    objGetInfoConstanciaSolicitada.urln = objWebRoot.route + 'api/constancias/ObtieneServMed/';
    let fecha = new Date(), valor = "", letras = "";
    $.ajax({
        url: objGetInfoConstanciaSolicitada.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objInfoUserConstancia.NoPersonal, TipoPersonal: objInfoUserConstancia.tipoPersonal }),
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {     
            if (data) {
                objGetInfoConstanciaSolicitada.info = Object.keys(new Object());
                objGetInfoConstanciaSolicitada.info = data[0];
            } else {
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        },
        complete: function (xhr, estatus) {
            $('#cuerpoConstancia').append(
                $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>, ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, recibiendo el beneficio de la prestación denominada “Servicio Médico” que brinda la institución.</span></p><br>')
            );
            if (objInfoUserConstancia.estado === 1) {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objInfoUserConstancia.folio + '</span></strong></p>')
                );
            } else {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstanciaSolicitada.info.sDirecPers+ '</span> </strong> </p>')
                );
            }
        }
    });
}

function ObtenerConstanciaServcioMedicoDep(){
    objGetInfoConstanciaSolicitada.urln = objWebRoot.route + 'api/constancias/ObtieneServMedDep/';
    let fecha = new Date(), valor = "", letras = "";

    $.ajax({
        url: objGetInfoConstanciaSolicitada.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objInfoUserConstancia.NoPersonal, TipoPersonal: objInfoUserConstancia.tipoPersonal }),
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            if (data) {
                objGetInfoConstanciaSolicitada.info = Object.keys(new Object());
                objGetInfoConstanciaSolicitada.info = data[0];
            } else {
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        },
        complete: function (xhr, estatus) {
            $('#cuerpoConstancia').append(
                $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>, ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>. Solicita la presente para hacer constar que su (RELACIÓN CON EL DEPENDIENTE ECONÓMICO) (NOMBRE DEL DEPENDIENTE ECONÓMICO), quien está registrado como su dependiente económico, recibe el beneficio de la prestación denominada “Servicio Médico” que brinda la institución. </span></p><br>')
            );
            $('#seccionDirector').append(
                $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstanciaSolicitada.info.sDirecPers + '</span> </strong> </p>')
            );
        }
    });
}

function ObtenerConstanciaTrabajoPercepciones() {
    objGetInfoConstanciaSolicitada.urln = objWebRoot.route + 'api/constancias/ObtieneTrab_Perc';
    let fecha = new Date(), valor = "", letras = "";

    $.ajax({
        url: objGetInfoConstanciaSolicitada.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objInfoUserConstancia.NoPersonal, TipoPersonal: objInfoUserConstancia.tipoPersonal}),
        datatype: "application/json",
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            if (data) {
                objGetInfoConstanciaSolicitada.info = Object.keys(new Object());
                objGetInfoConstanciaSolicitada.info = data[0];
            } else {
                
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        },
        complete: function (xhr, status) {
            if (objInfoUserConstancia.tipoPersonal !== 5) {
                valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSuelPrest).toFixed(2);
                letras = numeroALetras(valor, monedaMXN);
                $('#cuerpoConstancia').append(
                    $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo más prestaciones la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSuelPrest + '(' + letras + ') 00/100 M.N. </span></p><br>')
                );
            } else {
                valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSueldo).toFixed(2);
                letras = numeroALetras(valor, monedaMXN);
                $('#cuerpoConstancia').append(
                    $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo más prestaciones la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSueldo + '(' + letras + ') 00/100 M.N. </span></p><br>')
                );
            }

            if (objInfoUserConstancia.estado == 1) {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstancia.info.Folio + '</span></strong></p>')
                );
            } else {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstanciaSolicitada.info.sDirecPers+ '</span></strong></p>')
                );
            }
        },
    });
}

function ObtenerConstanciaVisa() {
    objGetInfoConstanciaSolicitada.urln = objWebRoot.route + 'api/constancias/ObtieneVisa';
    let fecha = new Date(), valor = "", letras = "";

    $.ajax({
        url: objGetInfoConstanciaSolicitada.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objInfoUserConstancia.NoPersonal, TipoPersonal: objInfoUserConstancia.tipoPersonal }),
        datatype: "application/json",
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            if (data) {
                objGetInfoConstanciaSolicitada.info = Object.keys(new Object());
                objGetInfoConstanciaSolicitada.info = data[0];
                objSegConstancia.director = objGetInfoConstanciaSolicitada.info.sDirecPers;
            } else {
                
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        },
        complete: function (xhr, estatus) {
            if (objInfoUserConstancia.tipoPersonal !== 5) {
                valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSuelPrest).toFixed(2);
                letras = numeroALetras(valor, monedaMXN);
                $('#cuerpoConstancia').append(
                    $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>, cuya fotografía aparece al calce,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo más prestaciones la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSuelPrest + '(' + letras + ') 00/100 M.N. </span></p><br>')
                );
            } else {
                valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSueldo).toFixed(2);
                letras = numeroALetras(valor, monedaMXN);
                $('#cuerpoConstancia').append(
                    $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>, cuya fotografía aparece al calce,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo más prestaciones la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSueldo + '(' + letras + ') 00/100 M.N. </span></p><br>')
                );
            }
            $('#seccionDirector').append(
                $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objSegConstancia.director + '</span> </strong> </p>')
            );
        }
    });
}

function ObtenerConstanciaAltaAfiliacionIPE() {
    objGetInfoConstanciaSolicitada.urln = objWebRoot.route + 'api/constancias/ObtieneIpe';
    let fecha = new Date(), valor = "", letras = "";

    $.ajax({
        url: objGetInfoConstanciaSolicitada.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objInfoUserConstancia.NoPersonal, TipoPersonal: objInfoUserConstancia.tipoPersonal }),
        datatype: "application/json",
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            if (data) {
                objGetInfoConstanciaSolicitada.info = Object.keys(new Object());
                objGetInfoConstanciaSolicitada.info = data[0];
            } else {
                
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        },
        complete: function (xhr, status) {
            if (objInfoUserConstancia.tipoPersonal !== 4) {
                valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSueldo).toFixed(2);
                letras = numeroALetras(valor, monedaMXN);
                $('#cuerpoConstancia').append(
                    $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.infosNumPer + '</span>, cuya fotografía aparece al calce,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSueldo + '(' + letras + ') 00/100 M.N. </span></p><br>')
                );
            } else {
                valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSuelPrest).toFixed(2);
                letras = numeroALetras(valor, monedaMXN);
                $('#cuerpoConstancia').append(
                    $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>, cuya fotografía aparece al calce,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSuelPres + '(' + letras + ') 00/100 M.N. </span></p><br>')
                );
            }

            if (objInfoUserConstancia.estado === 1) {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstancia.info.Folio + '</span></strong></p>')
                );
            } else {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstanciaSolicitada.info.sDirecPers + '</span> </strong> </p>')
                );
            }
        }
    });
}

function ObtenerConstanciaAltaAfiliacionMagisterio() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneMag';
    let fecha = new Date(), valor = "", letras = "";

    $.ajax({
        url: objGetInfoConstancia.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objInfoUserConstancia.NoPersonal, TipoPersonal: objInfoUserConstancia.tipoPersonal}),
        datatype: "application/json",
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            if (infoConstancia) {
                objGetInfoConstanciaSolicitada.info = Object.keys(new Object());
                objGetInfoConstanciaSolicitada.info = data[0];
                objSegConstancia.director = objGetInfoConstanciaSolicitada.info.sDirecPers;
            } else {
                
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        },
        complete: function (xhr, status) {
            if (objInfoUserConstancia.tipoPersonal !== 4) {
                valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSueldo).toFixed(2);
                letras = numeroALetras(valor, monedaMXN);
                $('#cuerpoConstancia').append(
                    $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>, cuya fotografía aparece al calce,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSueldo + '(' + letras + ') 00/100 M.N. </span></p><br>')
                );
            } else {
                valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSuelPrest).toFixed(2);
                letras = numeroALetras(valor, monedaMXN);
                $('#cuerpoConstancia').append(
                    $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>, cuya fotografía aparece al calce,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSuelPrest + '(' + letras + ') 00/100 M.N. </span></p><br>')
                );
            }

            if (objInfoUserConstancia.estado === 1) {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstancia.info.Folio + '</span></strong></p>')
                );
            } else {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objSegConstancia.director + '</span> </strong> </p>')
                );
            }
        }
    });
}

function ObtenerConstanciaBajaAfiliacionMagisterio() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneOfiBajMAG';
    let fecha = new Date(), valor = "", letras = "";

    $.ajax({
        url: objGetInfoConstancia.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objUserInfo.numPersonal, TipoPersonal: objUserInfo.tipoPersonal }),
        datatype: "application/json",
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            if (infoConstancia) {
                objGetInfoConstanciaSolicitada.info = Object.keys(new Object());
                objGetInfoConstanciaSolicitada.info = data[0];
                objSegConstancia.director = objGetInfoConstanciaSolicitada.info.sDirecPers;
            } else {
                
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        },
        complete: function (xhr, estatus) {
            $('#cuerpoConstancia').append(
                $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>, cuya fotografía aparece al calce,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSuelPrest + '(' + letras + ') 00/100 M.N. </span></p><br>')
            );
            $('#seccionDirector').append(
                $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objSegConstancia.director + '</span> </strong> </p>')
            );
        }
    });
}

function ObtenerConstanciaVisaDep() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneVisaDep';
    var infoConstancia, fecha = new Date();
    let valor = "";
    let letras = "";
    $.ajax({
        url: objGetInfoConstancia.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objInfoUserConstancia.NoPersonal, TipoPersonal: objInfoUserConstancia.tipoPersonal }),
        datatype: "application/json",
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            if (data) {
                objGetInfoConstanciaSolicitada.info = Object.keys(new Object());
                objGetInfoConstanciaSolicitada.info = data[0];
                objSegConstancia.director = objGetInfoConstanciaSolicitada.info.sDirecPers;
            } else {
                
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        },
        complete: function (xhr, estatus) {
            valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSuelPrest).toFixed(2);
            letras = numeroALetras(valor, monedaMXN);

            $('#cuerpoConstancia').append(
                $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>, cuya fotografía aparece al calce,  ingresó el <span id="fechaIngreso">' + new Date(objGetInfoConstanciaSolicitada.info.dtFIngreso).toLocaleDateString("es-ES", options) + '</span>, y actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '</span>, percibe en forma mensual de sueldo más prestaciones la cantidad de ' + '$ ' + objGetInfoConstanciaSolicitada.info.sSuelPrest + '(' + letras + ') 00/100 M.N. Solicita la presenta para trámite de VISA de su (RELACIÓN CON EL DEPENDIENTE ECONÓMICO)  (NOMBRE DEL DEPENDIENTE ECONÓMICO), cuya fotografía aparece al calce, quien está registrado como su dependiente económico. </span></p><br>')
            );

            if (objInfoUserConstancia.estado === 1) {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstancia.info.Folio + '</span> </strong> </p>')
                );
            } else {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objSegConstancia.director + '</span> </strong> </p>')
                );
            }
        }        
    });
}

function ObtenerConstanciaPRODep() {
    console.log('paso aqui');
    objGetInfoConstanciaSolicitada.urln = objWebRoot.route + 'api/constancias/ObtienePRODep';
    let fecha = new Date(), valor = "", letras = "";

    $.ajax({
        url: objGetInfoConstanciaSolicitada.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objInfoUserConstancia.NoPersonal, TipoPersonal: objInfoUserConstancia.tipoPersonal }),
        datatype: "application/json",
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            if (data) {
                objGetInfoConstanciaSolicitada.info = Object.keys(new Object());
                objGetInfoConstanciaSolicitada.info = data[0];
            } else {
                
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        },
        complete: function (xhr, status) {
            
            valor = parseFloat(objGetInfoConstanciaSolicitada.info.sSuelPrest).toFixed(2);
            letras = numeroALetras(valor, monedaMXN);
            $('#cuerpoConstancia').append(
                $('<p style="text-align: justify; line-height: 200%; hyphens: auto;"><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;hyphens: auto;">De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. <span id="nombrePersonal">' + objGetInfoConstanciaSolicitada.info.sNombre + '</span>, con número de personal <span id="numeroPersonal">' + objGetInfoConstanciaSolicitada.info.sNumPer + '</span>. Actualmente presta sus servicios en esta Casa de Estudios en el (la) <span id="depedencia">' + objGetInfoConstanciaSolicitada.info.sDescDep + '</span> de la región <span id="region">' + objGetInfoConstanciaSolicitada.info.sDesRegion + '</span>, como <span id="tipoPersonal">' + objGetInfoConstanciaSolicitada.info.sDesTPE + '</span> con tipo de contratación <span id="tipoContratacion">' + objGetInfoConstanciaSolicitada.info.sDesCont + '</span> con la categoría <span id="categoriasueldo">' + objGetInfoConstanciaSolicitada.info.sDescCat + '</span> y puesto de <span id="puesto">' + objGetInfoConstanciaSolicitada.info.sDesPuesto + '.</span><br><br><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#10146;&nbsp;&nbsp;Es Tiempo Completo con esta categoría a partir del ' + ObtenerFechaCustom(objGetInfoConstanciaSolicitada.info.sFAltPlact) + '.</span><br><br></p>')
            );
            
            if (objInfoUserConstancia.estado == 1) {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstancia.info.Folio + '</span></strong></p>')
                );
            } else {
                $('#seccionDirector').append(
                    $('<p style="text-align: center;"><strong><span style="font-family: Arial, Helvetica, sans-serif; font-size: 11pt;" id="nombredirector">' + objGetInfoConstanciaSolicitada.info.sDirecPers + '</span></strong></p>')
                );
            }
        },
    });
}

/***************************
 * Funciones de los botones *
 **************************/

function ConfirmarConstancia() {
    objSegConstancia.urln = objWebRoot.route + 'api/constancias/AddEstadoConstancias';
    var registro = new Date().toJSON(), motivo = "Constancia Autorizada";

    $.ajax({
        url: objSegConstancia.urln,
        type: 'POST',
        data: JSON.stringify({ Id: 0, SolicitudConstanciaId: objGetInfoConstancia.info.Id, UsuarioId: objGetInfoConstancia.info.UsuarioId, FechaHora: registro, EstadoId: 2, Motivo: motivo }),
        contentType: "application/json",
        beforeSend: function (xhr) {
            objSegConstancia.Fecha = registro;
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (data.CodeResponse === 0) {
                document.getElementById("confirmarConstancia").style.display = 'none';
                document.getElementById("cancelarConstancia").style.display = 'none';
                document.getElementById("reimprimirConstancia").style.display = "none";
                document.getElementById("getConstancia").style.display = '';
                document.getElementById("entregarConstancia").style.display = "";
                document.getElementById("seccionDirector").style.display = '';
                objSegConstancia.Fecha = registro;
                $('#fechaGeneracion').text(new Date(objSegConstancia.Fecha).toLocaleDateString("es-ES", options));
                console.log(objSegConstancia.director);
                document.getElementById("seccionDirector").style.display = "";
                alert("Constancia confimada correctamente.")
            } else {
                alert("Hubo un error al confirmar la constancia.");
            }
        },
        error: function () {
            alert("Hubo un error al confirmar la constancia.");
        }
    });
}

function CancelarConstancia() {
    $('#modalConstanciaCancelada').modal('show');
}

function GuardarEstadoConstaciaCancelada() {
    objSegConstancia.urln = objWebRoot.route + 'api/constancias/AddEstadoConstancias';
    var registro = new Date().toJSON(), motivo = $("#motivoCancelacion").val();;

    $.ajax({
        url: objSegConstancia.urln,
        type: 'POST',
        data: JSON.stringify({ Id: 0, SolicitudConstanciaId: objGetInfoConstancia.info.Id, UsuarioId: objGetInfoConstancia.info.UsuarioId, FechaHora: registro, EstadoId: 6, Motivo: motivo }),
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            
            if (data.CodeResponse === 0) {
                document.getElementById("getConstancia").style.display = 'none';
                document.getElementById("cancelarConstancia").style.display = 'none';
                document.getElementById("confirmarConstancia").style.display = 'none';
                document.getElementById("reimprimirConstancia").style.display = "none";
                document.getElementById("entregarConstancia").style.display = "none";
                $('#modalConstanciaCancelada').modal('hide');
            }
        },
        error: function () {
            $('#modalConstanciaCancelada').modal('hide');
            alert("Hubo un error al cancelar la constancia.");
        }
    });
}

function DescargarConstancia() {
    vaReimprimir = false;
    var pathfile = "";
    $.ajax({
        url: 'https://xmltopdf.ownersys.com/pdf/pdf_generador.php?action=downloadpdf',
        data: {
            shtml: $("#impresionPDF").html()
        },
        type: "POST",
        dataType: "json",
        beforeSend: function () {
            $('#constanciaFile').empty();
        },
        error: function () { },
        success: function (data) {
            pathfile = data.filename;
            $('#constanciaModal').modal('show');
        },
        complete: function (xhr, status) {
            console.log(pathfile);
            $('#constanciaFile').append(
                $('<iframe src="' + pathfile + '" width="100%" height="800px" frameborder="0"></iframe>')
            );
        }
    });
}

function ReimprimirConstancia() {
    vaReimprimir = true;
    var pathfile = "";
    $.ajax({
        url: 'https://xmltopdf.ownersys.com/pdf/pdf_generador.php?action=downloadpdf',
        data: {
            shtml: $("#impresionPDF").html()
        },
        type: "POST",
        dataType: "json",
        beforeSend: function () {
            $('#constanciaFile').empty();
        },
        error: function () { },
        success: function (data) {
            pathfile = data.filename;
            $('#constanciaModal').modal('show');
        },
        complete: function (xhr, status) {
            $('#constanciaFile').append(
                $('<iframe src="' + pathfile + '" width="100%" height="800px" frameborder="0"></iframe>')
            );
        }
    });
}

function GuardarEstadoConstanciaImpresa() {
    objSegConstancia.urln = objWebRoot.route + 'api/constancias/AddEstadoConstancias';
    var registro = new Date().toJSON(), motivo = "", estadoConstancia = 3;
    if (vaReimprimir === true) {
        estadoConstancia = 4;
        motivo = "Constancia reimpresa";
    } else {
        estadoConstancia = 3;
        motivo = "Constancia impresa";
    }
    $.ajax({
        url: objSegConstancia.urln,
        type: 'POST',
        data: JSON.stringify({ Id: 0, SolicitudConstanciaId: objGetInfoConstancia.info.Id, UsuarioId: objGetInfoConstancia.info.UsuarioId, FechaHora: registro, EstadoId: estadoConstancia, Motivo: motivo }),
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            if (data.CodeResponse === 0) {
                document.getElementById("getConstancia").style.display = 'none';
                document.getElementById("cancelarConstancia").style.display = 'none';
                document.getElementById("confirmarConstancia").style.display = 'none';
                document.getElementById("reimprimirConstancia").style.display = "";
                document.getElementById("entregarConstancia").style.display = "";
                $('#constanciaModal').modal('hide');
            } else {
                
            }
        },
        error: function () {
            $('#modalConstanciaCancelada').modal('hide');
            alert("Hubo un error al cancelar la constancia.");
        }
    });
}

function entregarConstancia() {
    var entregaParams = {
        ConstanciaId: objConstanciaParams.IdConstanciaSolicitud
    }
    localStorage.setItem('entregaParams', JSON.stringify(entregaParams));
    window.location.replace(objWebRoot.route +"Constancias/EntregaConstancia/");
}

/***************************
 * Funciones para utileria *
 **************************/

function Mayuscula(string) {
    return string.toUpperCase();
}

function ObtenerFechaCustom(string) {
    var fechaCustom = string.slice(0, 10);
    return new Date(fechaCustom.replace(/(\d+[/])(\d+[/])/, '$2$1')).toLocaleDateString("es-ES", options);
}

var numeroALetras = (function () {
    // Código basado en el comentario de @sapienman
    // Código basado en https://gist.github.com/alfchee/e563340276f89b22042a
    function Unidades(num) {

        switch (num) {
            case 1:
                return 'UN';
            case 2:
                return 'DOS';
            case 3:
                return 'TRES';
            case 4:
                return 'CUATRO';
            case 5:
                return 'CINCO';
            case 6:
                return 'SEIS';
            case 7:
                return 'SIETE';
            case 8:
                return 'OCHO';
            case 9:
                return 'NUEVE';
        }

        return '';
    } //Unidades()

    function Decenas(num) {

        let decena = Math.floor(num / 10);
        let unidad = num - (decena * 10);

        switch (decena) {
            case 1:
                switch (unidad) {
                    case 0:
                        return 'DIEZ';
                    case 1:
                        return 'ONCE';
                    case 2:
                        return 'DOCE';
                    case 3:
                        return 'TRECE';
                    case 4:
                        return 'CATORCE';
                    case 5:
                        return 'QUINCE';
                    default:
                        return 'DIECI' + Unidades(unidad);
                }
            case 2:
                switch (unidad) {
                    case 0:
                        return 'VEINTE';
                    default:
                        return 'VEINTI' + Unidades(unidad);
                }
            case 3:
                return DecenasY('TREINTA', unidad);
            case 4:
                return DecenasY('CUARENTA', unidad);
            case 5:
                return DecenasY('CINCUENTA', unidad);
            case 6:
                return DecenasY('SESENTA', unidad);
            case 7:
                return DecenasY('SETENTA', unidad);
            case 8:
                return DecenasY('OCHENTA', unidad);
            case 9:
                return DecenasY('NOVENTA', unidad);
            case 0:
                return Unidades(unidad);
        }
    } //Unidades()

    function DecenasY(strSin, numUnidades) {
        if (numUnidades > 0)
            return strSin + ' Y ' + Unidades(numUnidades)

        return strSin;
    } //DecenasY()

    function Centenas(num) {
        let centenas = Math.floor(num / 100);
        let decenas = num - (centenas * 100);

        switch (centenas) {
            case 1:
                if (decenas > 0)
                    return 'CIENTO ' + Decenas(decenas);
                return 'CIEN';
            case 2:
                return 'DOSCIENTOS ' + Decenas(decenas);
            case 3:
                return 'TRESCIENTOS ' + Decenas(decenas);
            case 4:
                return 'CUATROCIENTOS ' + Decenas(decenas);
            case 5:
                return 'QUINIENTOS ' + Decenas(decenas);
            case 6:
                return 'SEISCIENTOS ' + Decenas(decenas);
            case 7:
                return 'SETECIENTOS ' + Decenas(decenas);
            case 8:
                return 'OCHOCIENTOS ' + Decenas(decenas);
            case 9:
                return 'NOVECIENTOS ' + Decenas(decenas);
        }

        return Decenas(decenas);
    } //Centenas()

    function Seccion(num, divisor, strSingular, strPlural) {
        let cientos = Math.floor(num / divisor)
        let resto = num - (cientos * divisor)

        let letras = '';

        if (cientos > 0)
            if (cientos > 1)
                letras = Centenas(cientos) + ' ' + strPlural;
            else
                letras = strSingular;

        if (resto > 0)
            letras += '';

        return letras;
    } //Seccion()

    function Miles(num) {
        let divisor = 1000;
        let cientos = Math.floor(num / divisor)
        let resto = num - (cientos * divisor)

        let strMiles = Seccion(num, divisor, 'UN MIL', 'MIL');
        let strCentenas = Centenas(resto);

        if (strMiles == '')
            return strCentenas;

        return strMiles + ' ' + strCentenas;
    } //Miles()

    function Millones(num) {
        let divisor = 1000000;
        let cientos = Math.floor(num / divisor)
        let resto = num - (cientos * divisor)

        let strMillones = Seccion(num, divisor, 'UN MILLON DE', 'MILLONES DE');
        let strMiles = Miles(resto);

        if (strMillones == '')
            return strMiles;

        return strMillones + ' ' + strMiles;
    } //Millones()

    return function NumeroALetras(num, currency) {
        currency = currency || {};
        let data = {
            numero: num,
            enteros: Math.floor(num),
            centavos: (((Math.round(num * 100)) - (Math.floor(num) * 100))),
            letrasCentavos: '',
            letrasMonedaPlural: currency.plural || 'PESOS', //'PESOS', 'Dólares', 'Bolívares', 'etcs'
            letrasMonedaSingular: currency.singular || 'PESO ', //'PESO', 'Dólar', 'Bolivar', 'etc'
            letrasMonedaCentavoPlural: currency.centPlural || 'CHIQUI PESOS CHILENOS',
            letrasMonedaCentavoSingular: currency.centSingular || 'CHIQUI PESO CHILENO'
        };

        if (data.centavos > 0) {
            data.letrasCentavos = 'CON ' + (function () {
                if (data.centavos == 1)
                    return Millones(data.centavos) + ' ' + data.letrasMonedaCentavoSingular;
                else
                    return Millones(data.centavos) + ' ' + data.letrasMonedaCentavoPlural;
            })();
        };

        if (data.enteros == 0)
            return 'CERO ' + data.letrasMonedaPlural + ' ' + data.letrasCentavos;
        if (data.enteros == 1)
            return Millones(data.enteros) + ' ' + data.letrasMonedaSingular + ' ' + data.letrasCentavos;
        else
            return Millones(data.enteros) + ' ' + data.letrasMonedaPlural + ' ' + data.letrasCentavos;
    };
})();