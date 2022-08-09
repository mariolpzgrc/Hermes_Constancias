using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class DelegadosViewModel
    {
        [HiddenInput]
        public int DelegarId { get; set; }

        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
    }
    public class DelegadosCrearViewModel
    {
        [Display(Name = "Nombre completo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Usuario { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessageResourceName = "email", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Correo { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public int Tipo { get; set; }
    }
    public class DelegadoDetalleViewModel
    {
        [HiddenInput]
        public int DelegarId { get; set; }

        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Display(Name = "Fecha de registro")]
        public string Fecha_Registro { get; set; }

        [Display(Name = "Fecha del último cambio")]
        public string Fecha_Actualizacion { get; set; }
    }
    public class DelegadoEditarViewModel
    {
        [HiddenInput]
        public int DelegarId { get; set; }

        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public int Tipo { get; set; }
    }
    public class DelegadoBorrarViewModel
    {
        [HiddenInput]
        public int DelegarId { get; set; }

        [Display(Name = "Nombre completo")]
        [HiddenInput]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        [HiddenInput]
        public string Usuario { get; set; }

        [Display(Name = "Correo")]
        [HiddenInput]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Tipo")]
        [HiddenInput]
        public string Tipo { get; set; }
    }
}
