using Hermes2018.Attributes;
using Hermes2018.Helpers;
using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class SolicitudViewModel
    {
        public IEnumerable<SolicitudesViewModel> Solicitudes { get; set; }
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }
    }
    public class SolicitudesViewModel
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Display(Name = "Rol")]
        public string Rol { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [Display(Name = "Área")]
        public string Area { get; set; }

        [Display(Name = "Puesto")]
        public string Puesto { get; set; }
    }
    public class SolicitudDetalleViewModel
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Región")]
        public string RegionNombre { get; set; }

        [Display(Name = "Área")]
        public string AreaNombre { get; set; }

        [Display(Name = "Puesto")]
        public string PuestoNombre { get; set; }

        [Display(Name = "Es único")]
        public string EsUnico { get; set; }

        [Display(Name = "Rol")]
        public string RolNombre { get; set; }

        [Display(Name = "Aprobado")]
        public string EstaAprobado { get; set; }

        [Display(Name = "Fecha de registro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }
    }
    public class SolicitudResponderViewModel
    {
        [HiddenInput]
        public string AreaId { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Usuario { get; set; }

        [Display(Name = "Nombre completo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessageResourceName = "email", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Correo { get; set; }

        [Display(Name = "Puesto")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "required")]
        public string Puesto { get; set; }

        [Display(Name = "Es único")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "required")]
        public bool EsUnico { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        [Display(Name = "¿Desea aprobar esta solicitud?")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Aprobar { get; set; }

        [Display(Name = "Comentario")]
        [CondicionSimple("Aprobar", ConstAprobado.AprobadoNoN, ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(500, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.MultilineText)]
        public string Comentario { get; set; } 
    }
    public class SolicitudUsuarioViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "required")]
        public string NombreCompleto { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "required")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "required")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Región")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "required")]
        public int RegionId { get; set; }

        [Display(Name = "Área")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "required")]
        public int AreaId { get; set; }

        [Display(Name = "Puesto")]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "required")]
        public int PuestoId { get; set; }
    }
}
