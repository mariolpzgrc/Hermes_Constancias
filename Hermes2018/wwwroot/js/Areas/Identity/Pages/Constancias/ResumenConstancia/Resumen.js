var objWebRoot = { route: "", token: "" },
    objGetInfoConstancia = { Id: 0, info: Object.keys(new Object()), urln: "", urlCons: "", mensajeError: "No se puede llenar el formulario.", mensajeAdvertencia: "Su tipo de personal no puede solicitar esta constancia." },
    objSegConstancia = { Id: 0, info: Object.keys(new Object()), urln: "", CveDep: 0, NombreDep: "", idRegion: 0, Region: "" },
    objUser = { username: "", usersession: "" },
    objUserInfo = { numPersonal: 0, tipoPersonal: 0, },
    monedaMXN = { plural: "PESOS", singular: "PESO", centPlural: "CENTAVOS", centSingular: "CENTAVO" },
    options = { year: 'numeric', month: 'long', day: 'numeric' },
    dataSeguimiento = { NoPersonal: 0, ConstanciaId: 0, EstadoId: 0, FechaSolicitud: "" };

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
        ObtenerConstancia(objGetInfoConstancia.Id)

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

function ObtenerConstancia(id) {
    switch (parseInt(id)) {
        case 1:
            if (objUserInfo.tipoPersonal === Funcionario || objUserInfo.tipoPersonal === Confianza || objUserInfo.tipoPersonal === Academico) {
                objGetInfoConstancia.urlCons = 'api/constancias/ObtieneServMed';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 2:
            if (objUserInfo.tipoPersonal === Funcionario || objUserInfo.tipoPersonal === Confianza || objUserInfo.tipoPersonal === Academico) {
                objGetInfoConstancia.urlCons = 'api/constancias/ObtieneServMedDep';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 3:
            objGetInfoConstancia.urlCons = 'api/constancias/ObtieneTrab_Perc';
            ObtenerInfoConstancia();
            break;
        case 4:
            objGetInfoConstancia.urlCons = '';
            ObtenerInfoConstancia();
            break;
        case 5:
            if (objUserInfo.tipoPersonal != Eventual) {
                objGetInfoConstancia.urlCons = 'api/constancias/ObtieneIpe';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 6:
            if (objUserInfo.tipoPersonal != Eventual) {
                objGetInfoConstancia.urlCons = 'api/constancias/ObtieneMag';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 7:
            if (objUserInfo.tipoPersonal != Eventual) {
                objGetInfoConstancia.urlCons = 'api/constancias/ObtieneOfiBajIPE';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 8:
            if (objUserInfo.tipoPersonal != Eventual) {
                objGetInfoConstancia.urlCons = 'api/constancias/ObtieneOfiBajMAG';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 9:
            objGetInfoConstancia.urlCons = 'api/constancias/ObtieneVisa';
            ObtenerInfoConstancia();
            break;
        case 10:
            if (objUserInfo.tipoPersonal === Funcionario || objUserInfo.tipoPersonal === Confianza || objUserInfo.tipoPersonal === Academico) {
                objGetInfoConstancia.urlCons = 'api/constancias/ObtieneVisaDep';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 11:
            if (objUserInfo.tipoPersonal === Academico) {
                objGetInfoConstancia.urlCons = 'api/constancias/ObtienePRODep';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 12:
            if (objUserInfo.tipoPersonal === Academico) {
                objGetInfoConstancia.urlCons = '';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 13:
            if (objUserInfo.tipoPersonal !== Eventual) {
                objGetInfoConstancia.urlCons = '';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
        case 14:
            if (objUserInfo.tipoPersonal !== Eventual) {
                objGetInfoConstancia.urlCons = '';
                ObtenerInfoConstancia();
            } else {
                window.history.back();
                alert(objGetInfoConstancia.mensajeAdvertencia);
            }
            break;
    }
}

/**
 *  Métodos para obtener infomarción de las constancias 
 * */
function ObtenerInfoConstancia() {
    let valor, letras;
    var sueldo;

    objGetInfoConstancia.urln = objWebRoot.route + objGetInfoConstancia.urlCons;
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
            if (data) {
                objGetInfoConstancia.info = Object.keys(new Object());
                objGetInfoConstancia.info = data[0];
            }
            else {
                alert(objGetInfoConstancia.mensajeError);
            }
        },
        error: function () {
            alert(objGetInfoConstancia.mensajeError);
        },
        complete: function (xhr, estatus) {
            if (objGetInfoConstancia.info) {
                $('#nombrePersonal').text(objGetInfoConstancia.info.sNombre);
                $('#numeroPersonal').text(objGetInfoConstancia.info.sNumPer);
                $('#fechaIngreso').text((new Date(objGetInfoConstancia.info.dtFIngreso).toLocaleDateString("es-ES", options).toLocaleUpperCase()));
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
                sueldo = (objGetInfoConstancia.info.sSueldo != null) ? objGetInfoConstancia.info.sSueldo : (objGetInfoConstancia.info.sSuelPrest != null) ? objGetInfoConstancia.info.sSuelPrest : "0.00";
                valor = parseFloat(sueldo).toFixed(2);
                letras = numeroALetras(valor, monedaMXN);
                $('#sueldo').text("$ " + sueldo + " - " + letras + " 00/100 M.N.");
                $('#periodo').text(objGetInfoConstancia.info.sNPeri);
                $('#horas').text(objGetInfoConstancia.info.sHrs);
            } else {
                alert("No se pudo carggar la información.");
            }
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
            if (data.CodeResponse === 0) {
                window.alert("Solicitud completa.")
                window.location.href = objWebRoot.route + 'Constancias/Solicitud/SolicitudConstancia';
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