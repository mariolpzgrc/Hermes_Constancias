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
    public class GruposViewModel
    {
        public int HER_GrupoId { get; set; }

        [Display(Name="Grupo")]
        public string HER_Nombre { get; set; }
    }
    public class CrearGrupoViewModel
    {
        [Display(Name="Nombre del grupo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(80, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreGrupo { get; set; }
        
        /*Temporales para la busqueda.>*/
        [Display(Name="Nombre completo")]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        [Display(Name = "Correo")]
        public string Correo { get; set; }
        /*Temporales para la busqueda <.*/

        [Display(Name = "Listado de integrantes")]
        [MinimoEntero(1, ErrorMessageResourceName = "minimoentero", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessage = "El grupo debe tener al menos un integrante.")]
        [HiddenInput]
        public int TotalIntegrantes { get; set; }

        [Display(Name = "Integrantes")]
        [MinimoListado("TotalIntegrantes", ErrorMessageResourceName = "minimolistado", ErrorMessageResourceType = typeof(SharedResource))]
        public List<IntegranteViewModel> Integrantes { get; set; }
    }
    public class IntegranteViewModel
    {
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
    public class IntegranteGrupoViewModel
    {
        [Display(Name = "UsuarioId")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [HiddenInput]
        public int UsuarioId { get; set; }

        [BindProperty]
        [HiddenInput]
        public int GrupoId { get; set; }

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
    public class DetalleGrupoViewModel
    {
        [Display(Name = "Grupo")]
        public string Nombre { get; set; }

        [BindProperty]
        [HiddenInput]
        public int GrupoId { get; set; }

        public List<IntegranteGrupoViewModel> Integrantes { get; set; }
    }

    public class EditarGrupoViewModel
    {
        [Display(Name = "Grupo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(80, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [BindProperty]
        [HiddenInput]
        public int GrupoId { get; set; }
    }

    public class BorrarGrupoViewModel
    {
        [Display(Name = "Grupo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(80, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]

        [BindProperty]
        [HiddenInput]
        public string Nombre { get; set; }
        [BindProperty]
        [HiddenInput]
        public int GrupoId { get; set; }
    }


    /*Integrantes*/

    public class AgregarIntegranteGrupoViewModel
    {

        [BindProperty]
        [HiddenInput]
        public int GrupoId { get; set; }

        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Usuario { get; set; }

        [Display(Name = "Nombre completo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Correo { get; set; }
    }
    public class BorrarIntegranteGrupoViewModel
    {
        [Display(Name = "UsuarioId")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [HiddenInput]
        public int UsuarioId { get; set; }

        [HiddenInput]
        public int GrupoId { get; set; }

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
}
