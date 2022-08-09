using Hermes2018.Attributes;
using Hermes2018.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class AreaViewModel
    {
        public int HER_AreaId { get; set; }
        public string HER_Nombre { get; set; }
        public string HER_Direccion { get; set; }
        public string HER_Telefono { get; set; }
    }
    public class TitularAreaViewModel
    {
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
    }
    public class ListadoAreaPorRegionViewModel
    {
        public int AreaId { get; set; }
        public string Nombre { get; set; }
        public string TienePadre { get; set; }
        public int AreaPadreId { get; set; }
        public int TotalUsuarios { get; set; }
    }
    public class ListadoAreaViewModel
    {
        public int AreaId { get; set; }
        public string Nombre { get; set; }
        public string Region { get; set; }
        public string TienePadre { get; set; }
        public string Visible { get; set; }
        public int AreaPadreId { get; set; }
        //--
        public string Estado { get; set; }
        public int TotalUsuarios { get; set; }
        //--
        public bool ExisteEnSIIU { get; set; }
    }
    public class DetalleAreaViewModel
    {
        [HiddenInput]
        public int AreaId { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Dias compromiso")]
        public int Dias_Compromiso { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [Display(Name = "Área padre")]
        public string Area_Padre { get; set; }

        [Display(Name = "Visible")]
        public string Visible { get; set; }

        [HiddenInput]
        public bool HayLogoActual { get; set; }
        //--
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        
        [HiddenInput]
        public int EstadoId { get; set; }
    }
    public class CrearAreaViewModel
    {
        [Display(Name ="Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(120, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Clave")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Clave { get; set; }

        [Display(Name = "Dias compromiso")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Range(0, int.MaxValue, ErrorMessage = "Introduzca un número válido")]
        public int Dias_Compromiso { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        [Display(Name = "Región")]
        [Condicionbolean("AsignarAreaPadre", ErrorMessage = "El dato es requerido.")]
        public string RegionId { get; set; }

        [Display(Name = "Área padre")]
        [Condicionbolean("AsignarAreaPadre", ErrorMessage = "El dato es requerido.")]
        public string Area_PadreId { get; set; }

        [Display(Name = "Visible")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool Visible { get; set; }

        [Display(Name = "Asignar área padre")]
        public bool AsignarAreaPadre { get; set; }

        [Display(Name = "Agregar un logo para el área")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool AgregarLogo { get; set; }

        [Display(Name = "Logo")]
        [CondicionArchivo("AgregarLogo", "JPG,JPEG,PNG", ErrorMessage = "No se ha seleccionado un archivo o es de tipo no permitido.")]
        public IFormFile Logo { get; set; }

        public bool BajaPorInactividad { get; set; }
    }
    public class EditarAreaViewModel
    {
        [HiddenInput]
        public int AreaId { get; set; }

        [HiddenInput]
        public bool TieneLogo { get; set; }

        //---
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        
        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [Display(Name = "Área padre")]
        public string Area_Padre { get; set; }
        //---

        [Display(Name = "Dias compromiso")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Range(0, int.MaxValue, ErrorMessage = "Introduzca un número válido")]
        public int Dias_Compromiso { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        [Display(Name = "Visible")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool Visible { get; set; }

        public bool BajaPorInactividad { get; set; }

    }
    public class EditarAreaEnAdminViewModel
    {
        [HiddenInput]
        public int AreaId { get; set; }

        [HiddenInput]
        public bool TieneLogo { get; set; }

        //---
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [Display(Name = "Área padre")]
        public string Area_Padre { get; set; }
        //---

        [Display(Name = "Dias compromiso")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Range(0, int.MaxValue, ErrorMessage = "Introduzca un número válido")]
        public int Dias_Compromiso { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        [Display(Name = "Visible")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool Visible { get; set; }
        public bool BajaPorInactividad { get; set; }

    }
    public class BorrarAreaViewModel
    {
        [HiddenInput]
        public int AreaId { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [Display(Name = "Área padre")]
        public string Area_Padre { get; set; }

        [Display(Name = "Visible")]
        public string Visible { get; set; }

        public bool BajaPorInactividad { get; set; }

    }
    public class DarDeBajaAreaViewModel
    {
        [HiddenInput]
        public int AreaId { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [Display(Name = "Área padre")]
        public string Area_Padre { get; set; }

        [Display(Name = "Visible")]
        public string Visible { get; set; }
        
        [BindProperty]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Tipo de baja")]
        public string TipoBaja { get; set; }
    }
    public class AreaEsViewModel
    {
        public int HER_AreaId { get; set; }
        public string HER_Nombre { get; set; }
    }
    public class ReasignarAreaViewModel
    {
        [HiddenInput]
        public int AreaId { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [Display(Name = "Área padre")]
        public string Area_Padre { get; set; }
        //---

        [Display(Name = "Dias compromiso")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Range(0, int.MaxValue, ErrorMessage = "Introduzca un número válido")]
        public int Dias_Compromiso { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        [Display(Name = "Visible")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool Visible { get; set; }
    }
    public class FamiliaAreaViewModel
    {
        [HiddenInput]
        public int AreaPadreId { get; set; }

        [HiddenInput]
        public int AreaId { get; set; }

        [HiddenInput]
        public int TipoId { get; set; }

        public string Tipo { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
    }
    public class FamiliaAreaCompuestaViewModel
    {
        [HiddenInput]
        public int AreaPadreId { get; set; }
        [HiddenInput]
        public int AreaId { get; set; }
        [HiddenInput]
        public int TipoId { get; set; }

        public string Tipo { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Área padre")]
        public string TienePadre { get; set; }

        [Display(Name = "Visible")]
        public string Visible { get; set; }

        [Display(Name = "Usuarios")]
        public int TotalUsuarios { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }
    }
    public class InfoAreaViewModel
    {
        [HiddenInput]
        public int AreaPadreId { get; set; }

        [HiddenInput]
        public int AreaId { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [HiddenInput]
        public int RegionId { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [HiddenInput]
        public int Tipo { get; set; }
    }
    public class AgregarAreaViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(120, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Clave")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(20, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Clave { get; set; }

        [Display(Name = "Dias compromiso")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Range(0, int.MaxValue, ErrorMessage = "Introduzca un número válido")]
        public int Dias_Compromiso { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        //----
        [Display(Name = "Región")]
        [HiddenInput]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string RegionId { get; set; }

        [Display(Name = "Área padre")]
        [HiddenInput]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Area_PadreId { get; set; }

        [Display(Name = "Visible")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool Visible { get; set; }

        [Display(Name = "Agregar un logo para el área")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool AgregarLogo { get; set; }

        [Display(Name = "Logo")]
        [CondicionArchivo("AgregarLogo", "JPG,JPEG,PNG", ErrorMessage = "No se ha seleccionado un archivo o el tipo no es permitido.")]
        public IFormFile Logo { get; set; }
        public bool BajaPorInactividad { get; set; }

    }

    //Buscar
    public class BuscarAreaViewModel
    {
        public int AreaId { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Dias compromiso")]
        public int DiasCompromiso { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [Display(Name = "Área padre")]
        public string AreaPadre { get; set; }
    }
}
