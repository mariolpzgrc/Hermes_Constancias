var objaprobar = { seleccion: 0 };
var objopciones = { si:"1", no:"2" };

$(function () {
    ControlAprobar();
    //--

    return false;
});


function ControlAprobar()
{
    $('#Aprobar').change(function () {
        if (objopciones.no === $(this).val()) 
        {
            $('#ContenedorComentario').removeClass("d-none");
        } else {
            $('#ContenedorComentario').removeClass("d-none");
            $('#ContenedorComentario').addClass("d-none");
            $('#Comentario').val("");
        }
    });
}

//Se agrega el método de evaluación condición
$.validator.addMethod('simple',
    function (value, element, params) {
        //Valor del campo en el que se depende
        objaprobar.seleccion = $('#' + params[0] + " option:selected").val();

        if (objaprobar.seleccion === params[1]) {
            //Valida para volver requeridos estos campos
            if ($.trim(value).length === 0)
            {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            return true;
        }
    });

$.validator.unobtrusive.adapters.add('simple',
    ['dependencia', 'valor'],
    function (options) {
        options.rules['simple'] = [options.params['dependencia'], options.params['valor']];
        options.messages['simple'] = options.message;
    });
//-----------------------------------------