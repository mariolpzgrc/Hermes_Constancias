using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class PuestoViewModel
    {
        public int HER_PuestoId { get; set; }
        public string HER_Nombre { get; set; }
        public bool Disponibilidad { get; set; }
    }
    public class PuestoListadoViewModel
    {
        [HiddenInput]
        public int PuestoId { get; set; }

        [Display(Name = "Nombre (M)")]
        public string Nombre { get; set; }

        [Display(Name = "Nombre (F)")]
        public string Nombre2 { get; set; }
        
        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Es único")]
        public string EsUnico { get; set; }

        [Display(Name = "Visible")]
        public string Visible { get; set; }
    }
    public class CrearPuestoViewModel
    {
        [Display(Name = "Nombre (M)")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Nombre (F)")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre2 { get; set; }

        [Display(Name = "Clave")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Clave { get; set; }

        [Display(Name = "Es único")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool EsUnico { get; set; }

        [Display(Name = "Visible")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool Visible { get; set; }
    }
    public class PuestoDetalleViewModel
    {
        [HiddenInput]
        public int PuestoId { get; set; }

        [Display(Name = "Nombre (M)")]
        public string Nombre { get; set; }

        [Display(Name = "Nombre (F)")]
        public string Nombre2 { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Es único")]
        public string EsUnico { get; set; }

        [Display(Name = "Visible")]
        public string Visible { get; set; }
    }
    public class EditarPuestoViewModel
    {
        [HiddenInput]
        public int PuestoId { get; set; }

        [Display(Name = "Nombre (M)")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool ActualizarNombre { get; set; }

        [Display(Name = "Nombre (F)")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre2 { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool ActualizarNombre2 { get; set; }
        
        [Display(Name = "Clave")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(50, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Clave { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool ActualizarClave { get; set; }

        [Display(Name = "Es único")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool EsUnico { get; set; }

        [Display(Name = "Visible")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool Visible { get; set; }
    }
    public class BorrarPuestoViewModel
    {
        [HiddenInput]
        public int PuestoId { get; set; }

        [Display(Name = "Nombre (M)")]
        public string Nombre { get; set; }

        [Display(Name = "Nombre (F)")]
        public string Nombre2 { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Es único")]
        public string EsUnico { get; set; }

        [Display(Name = "Visible")]
        public string Visible { get; set; }
    }
    public class InfoPuestoViewModel
    {
        [HiddenInput]
        public int PuestoId { get; set; }
        
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }
    }
}
