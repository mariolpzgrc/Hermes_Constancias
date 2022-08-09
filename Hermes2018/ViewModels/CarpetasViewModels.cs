using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class CarpetaViewModel
    {
        public int CarpetaId { get; set; }
        public string Nombre { get; set; }
        public string Fecha { get; set; }
    }
    public class CrearCarpetaViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(30, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreCarpeta { get; set; }
    }
    public class EditarCarpetaViewModel
    {
        [HiddenInput]
        public int CarpetaId { get; set; }

        [Display(Name = "Nombre de la carpeta")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(30, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreCarpeta { get; set; }
    }
    public class BorrarCarpetaViewModel
    {
        [HiddenInput]
        public int CarpetaId { get; set; }

        [Display(Name = "Nombre de la carpeta")]
        [HiddenInput]
        public string NombreCarpeta { get; set; }
    }
    public class SubcarpetaViewModel
    {
        public int SubcarpetaId { get; set; }
        public string Nombre { get; set; }
        public string Fecha { get; set; }
    }
    public class CrearSubcarpetaViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(15, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreSubcarpeta { get; set; }
    }
    public class EditarSubcarpetaViewModel
    {
        [HiddenInput]
        public int SubcarpetaId { get; set; }

        [HiddenInput]
        public int? CarpetaPadreId { get; set; }

        [Display(Name = "Nombre de la carpeta")]
        [HiddenInput]
        public string CarpetaPadre_Nombre { get; set; }

        [Display(Name = "Nombre de la subcarpeta")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(15, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreSubcarpeta { get; set; }
    }
    public class BorrarSubcarpetaViewModel
    {
        [HiddenInput]
        public int SubcarpetaId { get; set; }

        [HiddenInput]
        public int? CarpetaPadreId { get; set; }

        [Display(Name = "Nombre de la carpeta")]
        [HiddenInput]
        public string CarpetaPadre_Nombre { get; set; }

        [Display(Name = "Nombre de la subcarpeta")]
        [HiddenInput]
        public string NombreSubcarpeta { get; set; }
    }
    public class CarpetasJsonMdel
    {
        public int CarpetaId { get; set; }
        public string Nombre { get; set; }
        public List<SubcarpetasJsonModel> Subcarpetas { get; set; }
    }
    public class SubcarpetasJsonModel
    {
        public int SubcarpetaId { get; set; }
        public string Nombre { get; set; }
    }
    public class MoverDocumentoJsonModel
    {
        public string Usuario { get; set; }
        public int Carpeta { get; set; }
        public List<ParDocumentoJsonModel> Valores { get; set; }
    }
    public class ParDocumentoJsonModel
    {
        public int Id { get; set; }
        public int Tipo { get; set; }
    }
    public class DetallesCarpetaViewModel
    {
        [HiddenInput]
        public int CarpetaId { get; set; }

        public string Nombre { get; set; }

        [Display(Name = "Fecha de creación")]
        public string Fecha_Creacion { get; set; }

        [Display(Name = "Fecha de modificación")]
        public string Fecha_Modificacion { get; set; }
        public string Nivel { get; set; }

        [Display(Name = "Carpeta padre")]
        public string CarpetaPadre { get; set; }

        [HiddenInput]
        public int? CarpetaPadreId { get; set; }
    }
}
