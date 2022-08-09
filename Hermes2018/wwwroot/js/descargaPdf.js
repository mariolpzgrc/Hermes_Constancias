/*
 * Archivo JavaScript para la creación de documento con formato Portable Document Format (PDF) de los oficios.
 * Última modificación: 21/02/2020
 */


//Al cargar el documento, ejecutar
$(function () {
    //const route = $('#routeWebRoot').val() + "api/pdf";
    //const token = $('#TokenWebApi').val();
    //$("#downloadPdf").click(function () {
    //    //getPdf(route, token);
    //});
});

/**
 * Obtiene el PDF del servidor con base en el contenido del oficio cargado en la página
 * @param {string} route URL del servicio que devuelve un pdf
 * @param {string} token Token de acceso
 */

//function getPdf(route, token) {
//    $('#downloadPdf').hide();
//    $('#loading_gif').show();
//    $('#errorPdf').hide();
//    const documentoDiv = $('#documento').clone();
//    documentoDiv.find("div").removeClass();
//    documentoDiv.find("span").removeClass();
//    documentoDiv.find("p").removeClass();
//    documentoDiv.find("span").removeAttr("style");
//    documentoDiv.find("img").each(function () {
//        if ($(this).attr("id") != "imagenQr")
//            $(this).attr("src", getDataUrlImg($(this)));

//    });
//    const htmlString = '<!DOCTYPE html><html lang="es"><body>' + documentoDiv.html() + '</body></html>';
//    const dataObj = {
//        'htmlString': htmlString,
//        //'areaSuperior': $('#docAreaSuperior').text(),
//        //'area': $('#docArea').text(),
//        //'region': $('#docRegion').text()
//    };
//    console.log(htmlString);
//    const jsonData = JSON.stringify(dataObj);
//    $.ajax({
//        url: route,
//        type: 'POST',
//        contentType: 'application/json',
//        xhrFields: {
//            responseType: 'arraybuffer',
//            withCredentials: true
//        },
//        data: jsonData,
//        beforeSend: function (xhr) {
//            xhr.setRequestHeader('Authorization', 'Bearer ' + token);
//        },
//        success: function (response) {
//            const file = new File([response], "", { type: 'application/pdf' });
//            const url = window.URL.createObjectURL(file);
//            $('#download_link').attr("href", url);
//            $('#download_link')[0].click();
//            $('#download_link').show();
//        },
//        error: function (err) {
//            $('#errorPdf').show();
//            console.log("No se puede recuperar el pdf\n" + err.responseText);
//        },
//        complete: function () {
//            $('#loading_gif').hide();
//        }
//    });
//}

/**
 * 
 * @param {JQuery<HTMLImageElement>} img
 * @returns the the image in DataURL format
 */
//function getDataUrlImg(img) {
//    const tempCanvas = document.createElement('canvas');
//    tempCanvas.height = img[0].naturalHeight;
//    tempCanvas.width = img[0].naturalWidth;
//    const context = tempCanvas.getContext('2d');
//    context.drawImage(img[0], 0, 0, tempCanvas.width, tempCanvas.height);
//    return tempCanvas.toDataURL(); 
//}

//function getDataUrlImg() {
//    var url = "@Url.Action('Logo', 'Anexos', new { id = Model.Envio.Origen_UsuarioDe_AreaId })";
//    //url = url.replace("param-id", img);
//    //$("#result").load(url);
//}
