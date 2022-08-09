var objCalendario = { anio: 0 };

$(document).ready(function ()
{
    objCalendario.anio = parseInt($('#InfoCalendario_Anio').val());
    //--
    ControlFechas();
});

function ControlFechas() {
    $('input[name="Crear.Fecha"]').daterangepicker({
        opens: 'center',
        autoUpdateInput: false,
        minDate: moment([objCalendario.anio, 0, 1, 0, 0, 0, 0]),
        maxDate: moment([objCalendario.anio, 11, 31, 23, 59, 59, 0]),
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
        },
        isInvalidDate: function (date) {
            
            if (moment(date).weekday() === 6 || moment(date).weekday() === 0) {
                return true;
            }
        }
    });

    $('input[name="Crear.Fecha"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
    });

    $('input[name="Crear.Fecha"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });
}