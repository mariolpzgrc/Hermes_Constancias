/*Floating label y validación de campos obligatorios del login*/

$(document).ready(function () {
    $('#Input_UserName').focus();
});


if ($('.datos-existentes').length) {
    $("#Input_UserName").addClass("mensaje-datos-existente");
    $("#Input_Password").addClass("mensaje-datos-existente");
    $(".user-label").addClass("mensaje-datos-existente-label");
    $(".pass-label").addClass("mensaje-datos-existente-label");
} else {
    $("#Input_UserName").removeClass("mensaje-datos-existente");
    $("#Input_Password").removeClass("mensaje-datos-existente");
    $(".user-label").removeClass("mensaje-datos-existente-label");
    $(".pass-label").removeClass("mensaje-datos-existente-label");
}

if ($('.datos-incorrectos').length) {
    $("#Input_UserName").addClass("mensaje-datos-incorrecto");
    $("#Input_Password").addClass("mensaje-datos-incorrecto");
    $(".user-label").addClass("datos-incorrectos");
    $(".pass-label").addClass("datos-incorrectos");
    $(".toggle-password").addClass("eye-incorrecto");

} else {
    $("#Input_UserName").removeClass("mensaje-datos-incorrecto");
    $("#Input_Password").removeClass("mensaje-datos-incorrecto");
    $(".user-label").removeClass("datos-incorrectos");
    $(".pass-label").removeClass("datos-incorrectos");
    $(".toggle-password").removeClass("eye-incorrecto");
}


var settings = {
    validClass: "is-valid",
    errorClass: "is-invalid"

};
$.validator.setDefaults(settings);
$.validator.unobtrusive.options = settings;
$.validator.unobtrusive.options = settings;


$(".toggle-password").click(function () {

    $(this).toggleClass("fa-eye fa-eye-slash");
    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});


$("#btn-inicio-sesion").click(function () {

    if ($("#Input_UserName").val().length < 1 && $("#Input_Password").val().length < 1) {
        $(".user-label").blur(); 
        $(".pass-label").blur(); 
        $("#Input_UserName").addClass("mensaje-datos-incorrecto");
        $("#Input_Password").addClass("mensaje-datos-incorrecto");
        $(".user-label").addClass("datos-incorrectos");
        $(".pass-label").addClass("datos-incorrectos");
        $(".toggle-password").addClass("eye-incorrecto");

    } else {
        $("#Input_UserName").removeClass("mensaje-datos-incorrecto");
        $("#Input_Password").removeClass("mensaje-datos-incorrecto");
        $(".user-label").removeClass("datos-incorrectos");
        $(".pass-label").removeClass("datos-incorrectos");
        $(".toggle-password").removeClass("eye-incorrecto");
    }

});






