var objWebRoot = { route: "" };
var objUser = { username: "", usersession: "" };
var objConstConfigGeneral = { Propiedad4: $('#Propiedad4').val(), Propiedad13: $("#Propiedad13").val(), Propiedad14: $("#Propiedad14").val(), Propiedad15: $("#Propiedad15").val() };
var objModelo = { Propiedad: $("#Editar_PropiedadClave").val() };

/****************************************************************************************************************************************
*                                                            Función: Main                                                              *
*****************************************************************************************************************************************/
$(function () {
    objWebRoot.route = $('#routeWebRoot').val();
    objUser.username = $("#user").text();
    objUser.usersession = $('#FolioSession').val();
    //--
    //-- Editor 
    if (objConstConfigGeneral.Propiedad4 === objModelo.Propiedad || objConstConfigGeneral.Propiedad13 === objModelo.Propiedad) {

        new FroalaEditor('#Editar_Valor', {
            key: "UBB7jE6D5G3I3A2A6aIVLEABVAYFKc1Ce1MYGD1c1NYVMiB3B9B6B5C2B4C3H3I3I3==",
            toolbarButtons: [
                'fullscreen',
                'bold',
                'italic',
                'underline',
                'subscript',
                'superscript',
                '|',
                'align',
                'formatOL',
                'formatUL',
                'outdent',
                'indent',
                '|',
                'insertLink',
                //'insertImage',
                //'insertVideo',
                'insertTable',
                '|',
                'insertHR',
                'clearFormatting',
                'selectAll',
                '|',
                'undo',
                'redo'
            ],
            attribution: false,
            fontSizeSelection: true,
            fontSize: ['8', '10', '12', '14', '16', '18'],
            fontSizeDefaultSelection: '16',
            language: 'es',
            //height: 400,
            heightMin: 300,
            heightMax: 400,
            theme: 'gray',
            //zIndex: 2001,
            placeholderText: 'Escriba su documento',
            pastePlain: true,
            quickInsertTags: [''],
            listAdvancedTypes: false,
            //videoUpload: false,
            //imageUpload: false,
            imageInsertButtons: ['imageBack', '|', 'imageByURL', 'imageManager']
        });
        //--
        $('form').each(function () {
            if ($(this).data('validator'))
                $(this).data('validator').settings.ignore = ".fr-box *";
        });
    } else if (objConstConfigGeneral.Propiedad14 === objModelo.Propiedad || objConstConfigGeneral.Propiedad15 === objModelo.Propiedad) {
        $('#Editar_Valor').datepicker({
            format: 'dd/mm/yyyy',
            orientation: 'bottom',
            language: 'es',
            startView: 0,
            minViewMode: 0,
            maxViewMode: 2,
            autoclose: true,
            enableOnReadonly: true
        });
    }

});
$.validator.setDefaults({ ignore: [] });