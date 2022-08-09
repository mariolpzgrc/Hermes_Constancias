using Hermes2018.Attributes;
using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class ServiciosViewModel
    {
        public int HER_ServicioId { get; set; }

        [Display(Name = "Servicio")]
        public string HER_Servicio_Nombre { get; set; }
    }
    public class ServiciosCompletosViewModel
    {
        //Servicio
        public int HER_ServicioId { get; set; }
        public string HER_Servicio_Nombre { get; set; }

        //Región
        public string HER_Region_Nombre { get; set; }
        public int HER_RegionId { get; set; }

        //Integrates
        public List<UsuarioLocalJsonModel> HER_Integrantes { get; set; }
    }
    public class CrearServicioViewModel
    {
        [Display(Name = "Nombre del servicio")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(80, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreServicio { get; set; }

        /*Temporales para la busqueda.>*/
        [Display(Name = "Nombre completo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Correo")]
        public string Correo { get; set; }
        
    }
    public class IntegranteServicioViewModel
    {
        //[Display(Name = "UsuarioId")]
        //[Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        //[HiddenInput]
        //public int UsuarioId { get; set; }

        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [HiddenInput]
        public string Usuario { get; set; }

        [Display(Name = "Nombre completo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [HiddenInput]
        public string Nombre { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [HiddenInput]
        public string Correo { get; set; }
    }
    public class DetalleServicioViewModel
    {
        [Display(Name = "Servicio")]
        public string Nombre { get; set; }

        public List<IntegranteServicioViewModel> Integrantes { get; set; }
    }
}
