﻿$(document).ready(function () {
    $('#Editar_ActualizarNombre').change(function () {
        if ($("input#Editar_ActualizarNombre[type='checkbox']").is(":checked")) {
            $("#Editar_Nombre").removeClass("disabled");
            $("#Editar_Nombre").removeClass("bg-light");
            $("#Editar_Nombre").addClass("disabled");
            $("#Editar_Nombre").addClass("bg-light");

        } else {
            $("#Editar_Nombre").removeClass("bg-light");
            $("#Editar_Nombre").removeClass("disabled");
        }
    });
    //--
    $('#Editar_ActualizarClave').change(function () {
        if ($("input#Editar_ActualizarClave[type='checkbox']").is(":checked")) {
            $("#Editar_Clave").removeClass("disabled");
            $("#Editar_Clave").removeClass("bg-light");
            $("#Editar_Clave").addClass("disabled");
            $("#Editar_Clave").addClass("bg-light");
        } else {
            $("#Editar_Clave").removeClass("bg-light");
            $("#Editar_Clave").removeClass("disabled");
        }
    });
    //----------------
    $('#Editar_ActualizarNombre2').change(function () {
        if ($("input#Editar_ActualizarNombre2[type='checkbox']").is(":checked")) {
            $("#Editar_Nombre2").removeClass("disabled");
            $("#Editar_Nombre2").removeClass("bg-light");
            $("#Editar_Nombre2").addClass("disabled");
            $("#Editar_Nombre2").addClass("bg-light");

        } else {
            $("#Editar_Nombre2").removeClass("bg-light");
            $("#Editar_Nombre2").removeClass("disabled");
        }
    });
});