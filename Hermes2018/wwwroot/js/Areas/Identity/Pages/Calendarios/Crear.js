$(document).ready(function () {
    $('#Crear_Anio').datepicker({
        format: 'yyyy',
        orientation: 'bottom',
        language: 'es',
        startView: 2,
        minViewMode: 2,
        autoclose: true,
        startDate: moment().format('yyyy'),
        enableOnReadonly: true
    });
});