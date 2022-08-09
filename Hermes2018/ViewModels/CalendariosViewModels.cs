using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class InfoCalendarioViewModel
    {
        [HiddenInput]
        public int CalendarioId { get; set; }

        [Display(Name = "Nombre")]
        [HiddenInput]
        public string Titulo { get; set; }

        [Display(Name = "Año")]
        [HiddenInput]
        public int Anio { get; set; }
    }
    public class CalendarioViewModel
    {
        [HiddenInput]
        public int CalendarioId { get; set; }

        [Display(Name = "Nombre")]
        public string Titulo { get; set; }

        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Display(Name = "Total de días")]
        public int TotalDias { get; set; }
    }
    public class CrearCalendarioViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreCalendario { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Anio { get; set; }
    }
    public class CrearContenidoCalendarioViewModel
    {
        [Display(Name = "Rango de fechas")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Fecha { get; set; }
    }
    public class ContenidoCalendarioViewModel
    {
        [Display(Name = "Mes")]
        public string Mes { get; set; }

        public List<ContenidoParcialCalendarioViewModel> Listado { get; set; }
    }
    public class ContenidoParcialCalendarioViewModel
    {
        public int ContenidoId { get; set; }

        [Display(Name = "Día")]
        public int Dia { get; set; }

        [Display(Name = "Fecha completa")]
        public string FechaCompleta { get; set; }
    }
    public class ResumenContenidoCalendarioViewModel
    {
        [HiddenInput]
        public int CalendarioId { get; set; }

        [Display(Name = "Calendario")]
        [HiddenInput]
        public string Calendario_Titulo { get; set; }

        [Display(Name = "Año")]
        [HiddenInput]
        public int Calendario_Anio { get; set; }

        [HiddenInput]
        public int ContenidoId { get; set; }

        [Display(Name = "Fecha")]
        [HiddenInput]
        public string Contenido_Fecha { get; set; }
    }

    //--
    public class CalendarioDiasInhabilesViewModel
    {
        [HiddenInput]
        public string Dias { get; set; }

        [HiddenInput]
        public string Inicio { get; set; }
        
        [HiddenInput]
        public string Limite { get; set; }

        [HiddenInput]
        public int EsVigente { get; set; }
    }
}
