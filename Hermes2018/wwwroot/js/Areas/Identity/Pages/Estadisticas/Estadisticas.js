/**********************************************************************************************************
 * Archivo JS para eventos y acciones en vista estadísticas del sistema Hermes
 * Últimos cambios el día 25/04/2019
 **********************************************************************************************************/
var chartRecibidos;
var chartEnviados;
var objWebRoot = { route: "", token: "" };
var objUser = { username: "" };

/*****************************************************************************************************************
 * Método inicializador
*****************************************************************************************************************/
(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objWebRoot.token = $('#TokenWebApi').val();
    objUser.username = $("#user").text();
    setupCategorias();
    //setupStartDate();
    //setupEndDate();
    setupGraficos();
    $("#checkFiltrar").prop("checked", false);
    $("#datePickerStart").prop('max', function () {
        return new Date().toJSON().split('T')[0];
    });

    $("#datePickerEnd").prop('max', function () {
        return new Date().toJSON().split('T')[0];
    });

})();
/***************************************************************************************************************
 * Selector de categorias
 ***************************************************************************************************************/
function setupCategorias() {
    var objCategorias = { urln: "" };
    objCategorias.urln = objWebRoot.route + "api/v1/categorias/" + objUser.username;
    var selectCategorias = document.getElementById("categoriasSelect");

    //--
    $.ajax({
        url: objCategorias.urln,
        type: "GET",
        datatype: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + objWebRoot.token);
        },
        success: function (result) {
            $.each(result, function (i, item) {
                selectCategorias.options[selectCategorias.options.length] = new Option(item.Nombre, item.CategoriaId);
            });
        }
    });
    //--
}
/*****************************************************************************************************************
 * Componentes de seleción de fechas (bootstrap-datepicker-1.6.4)
 *****************************************************************************************************************/

/*function setupStartDate() {
    $("#datePickerStart").datepicker({
        todayBtn: "linked",
        language: "es",
        endDate: "today",
        autoclose: true,
        orientation: "bottom"
    });
}
function setupEndDate() {
    $("#datePickerEnd").datepicker({
        endDate: "today",
        todayBtn: "linked",
        language: "es",
        autoclose: true,
        orientation: "bottom"
    });
}
$("#datePickerStart").datepicker()
    .on('changeDate', function (e) {
        $('#datePickerEnd').datepicker('setStartDate', e.date);
    });
function getCleanDate(date) {
    //The first month it's 0
    var date = {
        day: date.getDate(),
        month: (date.getMonth() + 1),
        year: date.getFullYear()
    }
    return date.day + "/" + date.month + "/" + date.year
}*/
/*****************************************************************************************************************
 * Gráficos (Chart.js-2.8.0)
 *****************************************************************************************************************/
function setupInitDataRecibidos() {
    var chartData = [];
    var routeRecibidos = objWebRoot.route + "api/v1/estadisticas/recibidos/" + objUser.username;
    fetch(routeRecibidos,
        {
            headers: {
                'Authorization': "Bearer " + objWebRoot.token
            }
        })
        .then(RES => RES.json())
        .then(function (data) {
            chartData = transformDataforSetupChart(data);
            chartRecibidos.data.labels = chartData.labels;
            chartRecibidos.data.datasets.push(chartData.datasets);
            chartRecibidos.update();
            if (chartData.checkSum == 0) {
                notifyFacade("No hay información que mostrar");
            }      
        })
        .catch(error => {
            notifyFacade("Sin servicio, intenta más tarde");
            console.log("Algo fue mal en la recuperación de recibidos")
        });
}
function setupInitDataEnviados() {
    var chartData = [];
    var routeEnviados = objWebRoot.route + "api/v1/estadisticas/enviados/" + objUser.username;
    fetch(routeEnviados,
   {
        headers: {
            'Authorization': "Bearer " + objWebRoot.token
        }
    })
        .then(RES => RES.json())
        .then(function (data) {
            chartData = transformDataforSetupChart(data);
            chartEnviados.data.datasets.push(chartData.datasets);
            chartEnviados.data.labels = chartData.labels;
            chartEnviados.update();
            if (chartData.checkSum == 0) {
                notifyFacade("No hay información que mostrar En Enviados");
            }
        })
        .catch(error => {
            notifyFacade("Sin servicio, intenta más tarde");
            console.log("Algo fue mal en la recuperación de enviados")
        });
}
function setupNewDataRecibidos(chartRoute) {
    removeData(chartRecibidos);
    var chartData = [];
    fetch(chartRoute,
        {
            headers: {
                'Authorization': "Bearer " + objWebRoot.token
            }
        })
        .then(RES => RES.json())
        .then(function (data) {
            chartData = transformDataforChart(data);
            addData(chartRecibidos, chartData.datasets);
            if (chartData.checkSum == 0) {
                notifyFacade("No hay información que mostrar en Recibidos");
            } 
        })
        .catch(error => {
            notifyFacade("Sin servicio, intenta más tarde");
            console.log("Algo fue mal en la actualización derecibidos")
        });
}
function setupNewDataEnviados(chartRoute) {
    removeData(chartEnviados);
    var chartData = [];
    fetch(chartRoute,
        {
            headers: {
                'Authorization': "Bearer " + objWebRoot.token
            }
        })
        .then(RES => RES.json())
        .then(function (data) {
            chartData = transformDataforChart(data);
            addData(chartEnviados, chartData.datasets);
            if (chartData.checkSum == 0) {
                notifyFacade("No hay información que mostrar");
            }
        })
        .catch(error => {
            notifyFacade("Sin servicio, intenta más tarde");
            console.log("Algo fue mal en la actualización de enviados")
        });
}
function createChartRoute(idCategoria, startDate, endDate, type) {
    var routeRecibidos = objWebRoot.route + "api/v1/estadisticas/" + type + "/" + objUser.username + "?";
    if (idCategoria) {
        if (idCategoria != "all") {
            routeRecibidos += "categoria=" + idCategoria;
        }   
    }
    if (startDate && !endDate) {
        var cleanDateStart = getCleanDate(startDate);
        var cleanDateEnd = getCleanDate(new Date());
        routeRecibidos += "&fechainicio=" + cleanDateStart + "&fechafin=" + cleanDateEnd;
    }
    if (!startDate && endDate) {
        var cleanDateEnd = getCleanDate(endDate);
        routeRecibidos += "&fechafin=" + cleanDateEnd;
    }
    if (startDate && endDate) {
        var cleanDateStart = getCleanDate(startDate);
        var cleanDateEnd = getCleanDate(endDate);
        routeRecibidos += "&fechainicio=" + cleanDateStart + "&fechafin=" + cleanDateEnd;
    }
    return routeRecibidos;
}
function transformDataforChart(rawData) {
    var chartData = [];
    var dataFrequency = []; //Elements added in labels ORDER
    var sumToCheckEmptiness = 0;
    var array;
    if (rawData.Recibidos) {
        array = rawData.Recibidos;
    }
    if (rawData.Enviados) {
        array = rawData.Enviados;
    }
    sumToCheckEmptiness += rawData.Respuestas;
    dataFrequency.push(rawData.Respuestas);
    array.forEach(function (item) {
        sumToCheckEmptiness += item.Total;
        dataFrequency.push(item.Total);
    });
    chartData = {
        datasets: {
            data: dataFrequency,
            backgroundColor: [
                '#FF7267',
                '#FFCF67',
                '#4FB1BF',
                '#7C5FCB',
                '#E15AA1',
                "#FF9F46",
            ]  
        },
        checkSum: sumToCheckEmptiness,
    }
    return chartData;
}
function transformDataforSetupChart(rawData) {
    var chartData = [];
    var dataFrequency = []; //Elements added in labels ORDER
    var dataEstados = [];
    var sumToCheckEmptiness = 0;
    var array;
    if (rawData.Recibidos) {
        array = rawData.Recibidos;
    }
    if (rawData.Enviados) {
        array = rawData.Enviados;
    }
    sumToCheckEmptiness += rawData.Respuestas;
    dataFrequency.push(rawData.Respuestas);
    dataEstados.push("Respuestas"); //This is not a estado but it's considered as one
    array.forEach(function (item) {
        sumToCheckEmptiness += item.Total;
        dataEstados.push(item.Estado);
        dataFrequency.push(item.Total);
    });
    chartData = {
        datasets: {
            data: dataFrequency,
            backgroundColor: [
                '#FF7267',
                '#FFCF67',
                '#4FB1BF',
                '#7C5FCB',
                '#E15AA1',
                "#FF9F46",
            ],
        },
        labels: dataEstados,
        checkSum: sumToCheckEmptiness,
    }
    return chartData;
}
function setupGraficos() {
    Chart.defaults.global.plugins.labels = {
        render: 'percentage',
        fontColor: '#ffffff',
        fontStyle: 'bold',
        fontSize: 14
    };
    const canvasGraficoRecibido = $("#graficoRecibidos");
    const canvasGraficoEnviado = $("#graficoEnviados"); 
    chartRecibidos = new Chart(canvasGraficoRecibido, {
        type: 'doughnut',
        options: {
            responsive: true,
        }
    });
    setupInitDataRecibidos();
    chartEnviados = new Chart(canvasGraficoEnviado, {
        type: 'pie',
        options: {
            responsive: true
        }
    });
    setupInitDataEnviados();
}
//remove dataset but no labels
function removeData(chart) {
    chart.data.datasets.splice(0, 1);
    chart.update();
}
function addData(chart, datasets) {
    chart.data.datasets.push(datasets);
    chart.update();
}
/*******************************************************************************************
 * Otros
 *********************************************************************************************/
function notifyFacade(mensaje) {
    $.getScript(objWebRoot.route + "lib/bootstrap-notify/bootstrap-notify.js", function () {
        $.notify({
            message: mensaje,
        }, {
                type: "info",
                newest_on_top: true,
                delay: 3000,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                }
            });
    });
}
$("#aplicarFiltroBoton").click(function () {
    //Toma los valores de los elementos de filtrado para asignar el resultado
    var selectCategoria = document.getElementById("categoriasSelect");
    var idCategoria = selectCategoria.options[selectCategoria.selectedIndex].value;
    var endDate = $('#datePickerEnd').datepicker('getDate');
    var startDate = $('#datePickerStart').datepicker('getDate');
    //Establece la nueva información en las gráficas
    const routeRecibidosChart = createChartRoute(idCategoria, startDate, endDate, "recibidos");
    setupNewDataRecibidos(routeRecibidosChart);
    const routeEnviadosChart = createChartRoute(idCategoria, startDate, endDate, "enviados");
    setupNewDataEnviados(routeEnviadosChart);
});
$("#checkFiltrar").change(function () {
    if (!this.checked) {
        //Restablece los valores para filtrar
        $("#datePickerStart").datepicker('clearDates');
        $("#datePickerEnd").datepicker('clearDates');
        $("#categoriasSelect").val($("#categoriasSelect").data("default-value"));
        //Reasigna la información inicial en las gráficas
        var route = objWebRoot.route + "api/v1/estadisticas/recibidos/" + objUser.username;
        setupNewDataRecibidos(route);
        route = objWebRoot.route + "api/v1/estadisticas/enviados/" + objUser.username;
        setupNewDataEnviados(route);
    }
});
