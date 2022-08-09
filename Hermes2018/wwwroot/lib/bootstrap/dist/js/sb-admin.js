var objBandeja = {
    estaContraido: $("body").hasClass('sidenav-toggled'),
    estaOcultoCarpeta: $("#collapseCarpetas").hasClass('show'),
    estaOcultoConstancia: $("#collapseConstancias").hasClass('show')
};

(function ($) {    
    "use strict"; // Start of use strict
    // Configure tooltips for collapsed side navigation
    $('.navbar-sidenav [data-toggle="tooltip"]').tooltip({
        template: '<div class="tooltip navbar-sidenav-tooltip" role="tooltip" style="pointer-events: none;"><div class="arrow"></div><div class="tooltip-inner"></div></div>'
    });
    //Toggle the side navigation
    $("#contiene-reductor").on("click", "li", function (e) {
        e.preventDefault();
        //--
        objBandeja.estaContraido = $("body").hasClass('sidenav-toggled');
        //--
        if (objBandeja.estaContraido)
        {
            //Se quita la contracción
            $("body").removeClass("sidenav-toggled");
            $("#contenedor-carpeta-base").addClass("collapsed");
            $("#contenedor-constancias-base").addClass("collapsed");
            $("#collapseCarpetas").addClass("show");
            $("#collapseConstancias").addClass("show");
            //--
            //Contenedor contraido
            $('#nuevoDocLink').removeClass('contenedor-btn-nuevo-contraido');
            $('#nuevoDocLink').addClass('contenedor-btn-nuevo-sin-contraer');
            //--
            //Boton sin contraer
            $('#big-nav-link').removeClass('btn-nuevo-contraido');
            $('#big-nav-link').addClass('btn-nuevo-sin-contraer');
            //--
            DesactivarValorMenuCookie(".AspNet.Menu");
            DesactivarValorMenuCookie(".AspNet.Carpeta");
            DesactivarValorMenuCookie(".AspNet.Constancia");
        }
        else {
            //Se agrega la contracción
            $("body").addClass("sidenav-toggled");
            $("#contenedor-carpeta-base").removeClass("collapsed");
            $("#constancias-base").removeClass("collapsed");
            $("#collapseCarpetas").removeClass("show");
            $("#collapseConstancias").removeClass("show");
            //--
            //Contenedor sin contraer
            $('#nuevoDocLink').removeClass('contenedor-btn-nuevo-sin-contraer');
            $('#nuevoDocLink').addClass('contenedor-btn-nuevo-contraido');
            //--
            //Boton contraido
            $('#big-nav-link').removeClass('btn-nuevo-sin-contraer');
            $('#big-nav-link').addClass('btn-nuevo-contraido');
            //--
            ActivarValorMenuCookie(".AspNet.Menu");
            ActivarValorMenuCookie(".AspNet.Carpeta");
            ActivarValorMenuCookie(".AspNet.Constancia");
        }
    });
    //--
    $("#carpetasLink").on("click", "a#contenedor-carpeta-base", function (e) {
        //--
        objBandeja.estaContraido = $("body").hasClass('sidenav-toggled');
        objBandeja.estaOcultoCarpeta = $("#collapseCarpetas").hasClass('show');
        //--
        if (objBandeja.estaContraido) {
            //Se quita la contracción
            $("body").removeClass("sidenav-toggled");
            //--
            //Contenedor contraido
            $('#nuevoDocLink').removeClass('contenedor-btn-nuevo-contraido');
            $('#nuevoDocLink').addClass('contenedor-btn-nuevo-sin-contraer');
            //--
            //Boton sin contraer
            $('#big-nav-link').removeClass('btn-nuevo-contraido');
            $('#big-nav-link').addClass('btn-nuevo-sin-contraer');
            //--
            DesactivarValorMenuCookie(".AspNet.Menu");
            DesactivarValorMenuCookie(".AspNet.Carpeta");
            DesactivarValorMenuCookie(".AspNet.Constancia");
        } else {
            if (objBandeja.estaOcultoCarpeta)
            {
                ActivarValorMenuCookie(".AspNet.Carpeta");

            } else
            {
                DesactivarValorMenuCookie(".AspNet.Carpeta");
            }
        }
    });

    $("#constanciasLink").on("click", "a#contenedor-constancias-base", function (e) {
        objBandeja.estaContraido = $("body").hasClass('sidenav-toggled');
        objBandeja.estaOcultoConstancia = $("#collapseConstancias").hasClass('show');

        if (objBandeja.estaContraido) {
            $("body").removeClass("sidenav-toggled");
            //--
            //Contenedor contraido
            $('#nuevoDocLink').removeClass('contenedor-btn-nuevo-contraido');
            $('#nuevoDocLink').addClass('contenedor-btn-nuevo-sin-contraer');
            //--
            //Boton sin contraer
            $('#big-nav-link').removeClass('btn-nuevo-contraido');
            $('#big-nav-link').addClass('btn-nuevo-sin-contraer');
            //--
            DesactivarValorMenuCookie(".AspNet.Menu");
            DesactivarValorMenuCookie(".AspNet.Carpeta");
            DesactivarValorMenuCookie(".AspNet.Constancia");
        } else {
            if (objBandeja.estaOcultoConstancia) {
                ActivarValorMenuCookie(".AspNet.Constancia");

            } else {
                DesactivarValorMenuCookie(".AspNet.Constancia");
            }
        }
    });
    //--
    // Prevent the content wrapper from scrolling when the fixed side navigation hovered over
    $('body.fixed-nav .navbar-sidenav, body.fixed-nav .sidenav-toggler, body.fixed-nav .navbar-collapse').on('mousewheel DOMMouseScroll', function(e) {
        var e0 = e.originalEvent, delta = e0.wheelDelta || -e0.detail;
        this.scrollTop += (delta < 0 ? 1 : -1) * 30;
        e.preventDefault();
    });
    // Scroll to top button appear
    $(document).scroll(function() {
        var scrollDistance = $(this).scrollTop();
        if (scrollDistance > 100) {
          $('.scroll-to-top').fadeIn();
        } else {
          $('.scroll-to-top').fadeOut();
        }
    });
    // Configure tooltips globally
    $('[data-toggle="tooltip"]').tooltip()
    // Smooth scrolling using jQuery easing
    $(document).on('click', 'a.scroll-to-top', function(event) {
        var $anchor = $(this);
        $('html, body').stop().animate({ scrollTop: ($($anchor.attr('href')).offset().top)}, 1000, 'easeInOutExpo');
        event.preventDefault();
     });
})(jQuery); // End of use strict
