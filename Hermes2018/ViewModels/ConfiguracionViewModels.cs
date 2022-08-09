using Hermes2018.Attributes;
using Hermes2018.Models.Configuracion;
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
    public class ConfiguracionUsuarioViewModel
    {
        public int ConfiguracionUsuarioId { get; set; }

        [Display(Name = "Notificaciones")]
        public string NotificacionesActivas { get; set; }

        [Display(Name = "Elementos por página")]
        public int ElementosPorPagina { get; set; }

        //public HER_Color Color { get; set; }

        [Display(Name = "Puesto")]
        public string Puesto { get; set; }

        //-- Para cambiar en el infoUsuario
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
    }
    public class EditarConfiguracionViewModel
    {
        [HiddenInput]
        public int ConfiguracionUsuarioId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Notificaciones")]
        public bool NotificacionesActivas { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Elementos por página")]
        public int ElementosPorPagina { get; set; }

        //[Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        //[Display(Name = "Color")]
        //public int ColorId { get; set; }

        //public SelectList Colores { get; set; }

        [Display(Name = "Puesto")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Puesto { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }
    }
    public class InfoConfiguracionLDAPViewModel
    {
        public string HER_IPLDAP { get; set; }
    }
    public class InfoConfiguracionEmailViewModel
    {
        public string HER_ServidorSMTP { get; set; }
        public int HER_PuertoServidorSMTP { get; set; }
        public string HER_CorreoNotificador { get; set; }
        public string HER_ContraseniaCorreoNotificador { get; set; }
    }
    public class InfoConfiguracionInstitucionViewModel
    {
        public string HER_NombreInstitucion { get; set; }
        public string HER_Logo { get; set; }
    }
    public class InfoConfiguracionPlantillasViewModel
    {
        public string HER_AsuntoOficioNotificacion { get; set; }
        public string HER_CuerpoOficioNotificacion { get; set; }

        public string HER_AsuntoRegistroNotificacion { get; set; }
        public string HER_CuerpoRegistroNotificacion { get; set; }

        public string HER_AsuntoAceptadoNotificacion { get; set; }
        public string HER_CuerpoAceptadoNotificacion { get; set; }

        public string HER_AsuntoRechazadoNotificacion { get; set; }
        public string HER_CuerpoRechazadoNotificacion { get; set; }

        public string HER_AsuntoSolicitudNotificacion { get; set; }
        public string HER_CuerpoSolicitudNotificacion { get; set; }
    }
    public class InfoConfiguracionAvisoPrivacidadViewModel
    {
        public string HER_Aviso { get; set; }
    }
    public class InfoPlantillaViewModel
    {
        public string HER_Asunto { get; set; }
        public string HER_Cuerpo { get; set; }
    }
    public class SolicitudPlantillaConfigViewModel
    {
        public string NombreCompleto { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Puesto { get; set; }
    }
    public class DetalleConfiguracion
    {
        public List<ListadoConfiguracionGeneral> Identidad { get; set; }
        public List<ListadoConfiguracionGeneral> Acceso { get; set; }
        public List<ListadoConfiguracionGeneral> General { get; set; }

        public DetalleImagenes Logo { get; set; }
        public DetalleImagenes Portada { get; set; }

        public string Extensiones { get; set; }

        public List<AnexoRutaViewModel> RutasBase { get; set; }
    }
    public class ListadoConfiguracionGeneral
    {
        [HiddenInput]
        public int ConfiguracionId { get; set; }

        [Display(Name = "Propiedad")]
        public string Propiedad { get; set; }

        [Display(Name = "Valor")]
        public string Valor { get; set; }
    }
    public class DetalleImagenes
    {
        public int ImagenId { get; set; }
        public string ImagenNombre { get; set; }
    }
    public class EditarConfiguracionGeneral
    {
        [HiddenInput]
        public int ConfiguracionId { get; set; }
        [HiddenInput]
        public string PropiedadClave { get; set; }

        [Display(Name = "Propiedad")]
        [HiddenInput]
        public string Propiedad { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Valor { get; set; }
    }
    public class EditarImagenConfiguracionGeneral
    {
        [HiddenInput]
        public int Tipo { get; set; }

        [Display(Name = "Imagen")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public IFormFile Imagen { get; set; }
    }
    public class EditarColeccionConfiguracionGeneral
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public bool Estado { get; set; }

        [HiddenInput]
        public string Valor { get; set; }
    }
    public class AvisoInhabilViewModel
    {
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }

        public string Contenido { get; set; }
    }
    //---
    public class CrearAnexoRutaViewModel
    {
        [Display(Name = "Ruta")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Ruta { get; set; }
    }

    public class EditarAnexoRutaViewModel
    {
        [HiddenInput]
        public int RutaBaseId { get; set; }

        [Display(Name = "Ruta")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Ruta { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Estado { get; set; }
    }
}
