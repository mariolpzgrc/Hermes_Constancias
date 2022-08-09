var objWebRoot = { route: "", token: "" },
    objGetInfoConstancia = { Id: 0, info: Object.keys(new Object()), urln: "" },
    objSegConstancia = { Id: 0, info: Object.keys(new Object()), urln: "", CveDep: 0, NombreDep: "", idRegion:0, Region: ""},
    objUser = { username: "", usersession: "" },
    objUserInfo = { numPersonal: 0, tipoPersonal: 0, },
    monedaMXN = { plural: "PESOS", singular: "PESO", centPlural: "CENTAVOS", centSingular: "CENTAVO" },
    options = { year: 'numeric', month: 'long', day: 'numeric' },
    dataSeguimiento = { NoPersonal: 0, ConstanciaId: 0, EstadoId: 0, FechaSolicitud: ""};

const Funcionario = 1, ATM = 2, Confianza = 3, Academico = 4, Eventual = 5;

/******************************************************************************
*                        Función: Main                                        *
******************************************************************************/

$(document).ready(function () {
    objUser.username = $("#user").text();
    objUser.usersession = $('#FolioSession').val();
    getData();
    ObtenerConstancia();
    ControlToggle();
    ControlToggleConstancia();
});

function getData() {
    var getInfo = JSON.parse(localStorage.getItem('queryParams'));
    if (localStorage.getItem('queryParams') === null) {
        window.history.back();
    } else {
        objUserInfo.numPersonal = getInfo.numeroPersonal;
        objUserInfo.tipoPersonal = getInfo.tipoPersonal;
        objGetInfoConstancia.Id = getInfo.tipoContancia;
    }
}

/**    Control de Toggle **/
function ControlToggle() {
    $("#control-toggle").click(function () {
        $("#contenido-info").toggle({
            duration: 'fast',
            complete: function () {
                if ($('#contenido-info:visible').length === 0) {
                    $("#control-toggle i").removeClass('fas fa-angle-down').addClass('fas fa-angle-right');
                } else {
                    $("#control-toggle i").removeClass('fas fa-angle-right').addClass('fas fa-angle-down');
                }
            }
        });
    });
}

function ControlToggleConstancia() {
    $("#control-toggle-constancia").click(function () {
        $("#contenido-info-constancia").toggle({
            duration: 'fast',
            complete: function () {
                if ($('#contenido-info-constancia:visible').length === 0) {
                    $("#control-toggle-constancia i").removeClass('fas fa-angle-down').addClass('fas fa-angle-right');
                } else {
                    $("#control-toggle-constancia i").removeClass('fas fa-angle-right').addClass('fas fa-angle-down');
                }
            }
        });
    });
}

/**
 *  Método para la selección de constancias
 **/

function ObtenerConstancia() {
    var id = objGetInfoConstancia.Id;
    switch (id) {
        case "1":
            if (objUserInfo.tipoPersonal === Funcionario || objUserInfo.tipoPersonal === Confianza || objUserInfo.tipoPersonal === Academico) {
                ObtenerConstanciaServcioMedico();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "2":
            if (objUserInfo.tipoPersonal === Funcionario || objUserInfo.tipoPersonal === Confianza || objUserInfo.tipoPersonal === Academico) {
                ObtenerConstanciaServcioMedicoDep();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "3":
            ObtenerConstanciaTrabajoPercepciones();
            break;
        case "4":
            ObtenerConstaHorarioLaboral();
            break;
        case "5":
            if (objUserInfo.tipoPersonal != Eventual) {
                ObtenerConstanciaAltaAfiliacionIPE();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "6":
            if (objUserInfo.tipoPersonal != Eventual) {
                ObtenerConstanciaAltaAfiliacionMagisterio();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "7":
            if (objUserInfo.tipoPersonal != Eventual) {
                ObtenerConstanciaBajaAfiliacionIPE();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "8":
            if (objUserInfo.tipoPersonal != Eventual) {
                ObtenerConstanciaBajaAfiliacionMagisterio();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "9":
                ObtenerConstanciaVisa();          
            break;
        case "10":
            if (objUserInfo.tipoPersonal === Funcionario || objUserInfo.tipoPersonal === Confianza || objUserInfo.tipoPersonal === Academico) {
                ObtenerConstanciaVisaDep();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "11":
            if (objUserInfo.tipoPersonal === Academico) {
                ObtenerConstanciaPRODEP();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "12":
            if (objUserInfo.tipoPersonal === Academico) {
                ObtenerConstanciaCurricular();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "13":
            if (objUserInfo.tipoPersonal !== Eventual) {
                ObtenerHojaDeServicios();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
        case "14":
            if (objUserInfo.tipoPersonal !== Eventual) {
                ObtenerContanciaJubicacion();
            } else {
                window.history.back();
                alert("Su tipo de personal no puede solicitar esta constancia.");
            }
            break;
    }
}

/**
 *  Métodos para obtener infomarción de las constancias 
 * */

function ObtenerConstanciaServcioMedico() {

    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneServMed/';

    $.ajax({
        url: objGetInfoConstancia.urln,
        type: "POST",
        data: JSON.stringify({ NumPersonal: objUserInfo.numPersonal, TipoPersonal: objUserInfo.tipoPersonal}),
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token)
        },
        success: function (data) {
            console.log(data);
            objGetInfoConstancia.info = Object.keys(new Object());
            objGetInfoConstancia.info = data[0];
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion.toUpperCase());
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSuelPrest).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSuelPrest + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }        
    });
}

function ObtenerConstanciaServcioMedicoDep() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneServMedDep';

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
            objGetInfoConstancia.info = Object.keys(new Object());
            objGetInfoConstancia.info = data[0];
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSuelPrest).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSuelPrest + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstanciaTrabajoPercepciones() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneTrab_Perc';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSuelPrest).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSuelPrest + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstaHorarioLaboral() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneServMedDep';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSuelPrest).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSuelPrest + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstanciaAltaAfiliacionIPE() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneIpe';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSueldo).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSueldo + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstanciaAltaAfiliacionMagisterio() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneMag';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSueldo).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSueldo + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstanciaBajaAfiliacionIPE() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneOfiBajIPE';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                let valor = parseFloat(objGetInfoConstancia.info.sSuelPrest).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSuelPrest + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstanciaBajaAfiliacionMagisterio() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneOfiBajMAG';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSuelPrest).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSuelPrest + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstanciaVisa() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneVisa';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSuelPrest).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSuelPrest + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstanciaVisaDep() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneVisaDep';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSuelPrest).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSuelPrest + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstanciaPRODEP() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtienePRODep';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                let valor = parseFloat(objGetInfoConstancia.info.sSuelPrest).toFixed(2);
                let letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + objGetInfoConstancia.info.sSuelPrest + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerConstanciaCurricular() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneServMedDep';

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
             objGetInfoConstancia.info = Object.keys(new Object());
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                $('#sueldo').text(objGetInfoConstancia.info.sSuelPrest);

            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerHojaDeServicios() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneServMedDep';

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
            var infoConstancia = JSON.parse(data);
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                objSegConstancia.NombreDep = objGetInfoConstancia.info.sDescDep;
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);
                $('#sueldo').text(objGetInfoConstancia.info.sSuelPrest);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}

function ObtenerContanciaJubicacion() {
    objGetInfoConstancia.urln = objWebRoot.route + 'api/constancias/ObtieneServMedDep';

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
            var infoConstancia = JSON.parse(data);
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text(Mayuscula(new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options)));
                objSegConstancia.CveDep = objGetInfoConstancia.info.sNumDep;
                $('#dependencia').text(objGetInfoConstancia.info.sDescDep);
                $('#region').text(objGetInfoConstancia.info.sDesRegion);
                objSegConstancia.idRegion = objGetInfoConstancia.info.sRegion;
                objSegConstancia.Region = objGetInfoConstancia.info.sDesRegion;
                $('#tipoPersonal').text(objGetInfoConstancia.info.sDesTPE);
                $('#tipoContratacion').text(objGetInfoConstancia.info.sDesCont);
                $('#categoriasueldo').text(objGetInfoConstancia.info.sDescCat);
                $('#puesto').text(objGetInfoConstancia.info.sDesPuesto);

                $('#sueldo').text(objGetInfoConstancia.info.sSuelPrest);
            } else {
                console.log(data);
            }
        },
        error: function () {
            alert("No se puede llenar el formulario");
        }
    });
}


/*** Funcion de guardar solcitud de constancia ***/

function GuardarSolicitudConstancia() {
    objSegConstancia.urln = objWebRoot.route + 'api/constancias/AddSolicitudConstancia';
    var registro = new Date();

    $.ajax({
        url: objSegConstancia.urln,
        type: 'POST',
        data: JSON.stringify({ Id: 0, ConstanciaId: objGetInfoConstancia.Id, NoPersonal: objUserInfo.numPersonal, Folio: 'TEST2022-22', Mensaje: "Constancia en proceso", UsuarioId: objUser.username, FechaSolicitud: registro, EstadoId: 1, CveDep: objSegConstancia.CveDep, NombreDep: objSegConstancia.NombreDep, TipoPersonal: objUserInfo.tipoPersonal, CampusId: objSegConstancia.idRegion, NombreCampus: objSegConstancia.Region }),
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (data) {
            console.log(data);
            console.log(data.CodeResponse);
            if (data.CodeResponse === 0) {
                $('#solicitudModal').modal('show');
            } else {
                alert("Hubo un error al solcitar la constancia.");
            }
        },
        error: function () {
            alert("Hubo un error al solcitar la constancia.");
        }
    });
}

/***************************
 * Funciones para utileria *
 **************************/


function Mayuscula(string) {
    return string.toUpperCase();
}

/** Rutina para convertir numero a letras**/
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