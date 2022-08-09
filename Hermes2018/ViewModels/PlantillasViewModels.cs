using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class PlantillaViewModel
    {
        [HiddenInput]
        public int HER_PlantillaId { get; set; }
        public string HER_Nombre { get; set; }
        public string HER_Texto { get; set; }
    }
    public class PlantillaSimplificadaViewModel
    {
        public int HER_PlantillaId { get; set; }

        [Display(Name ="Nombre")]
        public string HER_Nombre { get; set; }
    }
    public class NuevaPlantillaViewModel
    {
        public string HER_Usuario { get; set; }
        public string HER_Nombre { get; set; }
        public string HER_Texto { get; set; }
    }
    public class PlantillaJsonModel
    {
        public string HER_Nombre { get; set; }
        public string HER_Texto { get; set; }
    }
    public class PlantillaDetalleViewModel
    {
        [HiddenInput]
        public int HER_PlantillaId { get; set; }

        [Display(Name = "Nombre")]
        public string HER_Nombre { get; set; }

        [Display(Name = "Contenido")]
        public string HER_Texto { get; set; }

        [Display(Name = "Fecha de creación")]
        public string HER_Fecha { get; set; }
    }
    public class EditarPlantillaViewModel
    {
        [HiddenInput]
        public int PlantillaId { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Contenido")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Texto { get; set; }
    }
    public class CrearPlantillaViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Contenido")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Texto { get; set; }
    }
}
