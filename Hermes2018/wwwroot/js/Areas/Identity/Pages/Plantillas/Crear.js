/**
 * 
 */
$(function () {
    $('#contiene-editor').removeClass('d-none');

    new FroalaEditor('#Crear_Texto', {
        key: "UBB7jE6D5G3I3A2A6aIVLEABVAYFKc1Ce1MYGD1c1NYVMiB3B9B6B5C2B4C3H3I3I3==",
        toolbarButtons: [
            'fullscreen',
            'bold',
            'italic',
            'underline',
            'strikeThrough',
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
        language: 'es',
        //height: 400,
        heightMin: 300,
        heightMax: 400,
        theme: 'gray',
        //zIndex: 2001,
        placeholderText: 'Escriba su documento',
        pastePlain: true,
        quickInsertTags: [''],
        //videoUpload: false,
        //imageUpload: true,
        imageMaxSize: 1024 * 1024 * 3,
        imageUploadURL: objWebRoot.route + "api/v1/UploadFiles",
        requestHeaders: {

            Authorization: "Bearer " + objWebRoot.token
        }
        /*imageInsertButtons: ['imageBack', '|', 'imageByURL', 'imageManager']*/
    });

    $('form').each(function () {
        if ($(this).data('validator'))
            $(this).data('validator').settings.ignore = ".fr-box *";
    });
});

$.validator.setDefaults({ ignore: '' });