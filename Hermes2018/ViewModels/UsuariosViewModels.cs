using Hermes2018.Attributes;
using Hermes2018.Helpers;
using Hermes2018.Models.Area;
using Hermes2018.Models.Rol;
using Hermes2018.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class UsuariosAdministradoresViewModel
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Rol")]
        public string Rol { get; set; }
    }
    public class UsuariosPorAreaViewModel
    {   
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Rol")]
        public string Rol { get; set; }

        [Display(Name = "Puesto")]
        public string Puesto { get; set; }

        [Display(Name = "Es único")]
        public string EsUnico { get; set; }

        public string Estado { get; set; }

        [Display(Name = "Permiso")]
        public string Permiso { get; set; }
    }
    public class UsuariosDetallesViewModel
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Titular")]
        public string Titular { get; set; }

        public int RegionId { get; set; }

        [Display(Name = "Región")]
        public string RegionNombre { get; set; }

        public int? AreaPadreId { get; set; }

        public int AreaId { get; set; }

        [Display(Name = "Área")]
        public string AreaNombre { get; set; }
        
        [Display(Name = "Puesto")]
        public string PuestoNombre { get; set; }

        [Display(Name = "Es único")]
        public string EsUnico { get; set; }

        [Display(Name = "Rol")]
        public string RolNombre { get; set; }

        [Display(Name = "Acepto Términos")]
        public string AceptoTerminos { get; set; }

        [Display(Name = "Fecha en la que aceptó los términos")]
        [DataType(DataType.DateTime)]
        public DateTime FechaTerminos { get; set; }

        [Display(Name = "Aprobado")]
        public string EstaAprobado { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        public bool Activo { get; set; }

        public bool EstaEnReasignacion { get; set; }

        [Display(Name = "Fecha de aprobación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaAprobado { get; set; }

        [Display(Name = "Fecha de registro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }

        [Display(Name = "Fecha de actualización")]
        [DataType(DataType.DateTime)]
        public DateTime FechaActualizacionRegistro { get; set; }

        public int Tipo { get; set; }

        //--Permiso como Administrador de Área
        [Display(Name = "Permiso como Administrador del área")]
        public string Permiso { get; set; }
    }
    public class UsuariosCrearViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreUsuario { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessageResourceName = "email", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Correo { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Rol { get; set; }

        [Display(Name = "Región")]
        [Dependiente("Rol", new string []{ ConstRol.Rol7T, ConstRol.Rol8T }, ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string RegionId { get; set; }

        [Display(Name = "Área")]
        [Dependiente("Rol", new string[] { ConstRol.Rol7T, ConstRol.Rol8T }, ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string AreaId { get; set; }

        [Display(Name = "Puesto")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Puesto { get; set; }

        [Display(Name = "Es único")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool EsUnico { get; set; }

        [Display(Name = "Utilizar el mismo usuario como titular")]
        public bool EsTitular { get; set; }

        [Display(Name = "Titular")]
        [Condicion("EsTitular", "NombreUsuario", ErrorMessageResourceName = "titular", ErrorMessageResourceType = typeof(SharedResource))]
        public string Titular { get; set; }

        [Display(Name = "Permiso como Administrador del Área")]
        public bool Permiso { get; set; }

        public SelectList Roles { get; set; }
        public SelectList Areas { get; set; }
        public SelectList Regiones { get; set; }

        [HiddenInput]
        public int TipoVista { get; set; }

        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }

    }
    public class AdminUsuariosCrearViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreUsuario { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessageResourceName = "email", ErrorMessageResourceType = typeof(SharedResource))]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Correo { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Rol { get; set; }

        [Display(Name = "Región")]
        [HiddenInput]
        public string RegionId { get; set; }

        [Display(Name = "Área")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string AreaId { get; set; }

        [Display(Name = "Puesto")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Puesto { get; set; }

        [Display(Name = "Es único")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool EsUnico { get; set; }

        [Display(Name = "Utilizar el mismo usuario como titular")]
        public bool EsTitular { get; set; }

        [Display(Name = "Titular")]
        [Condicion("EsTitular", "NombreUsuario", ErrorMessageResourceName = "titular", ErrorMessageResourceType = typeof(SharedResource))]
        public string Titular { get; set; }

        [Display(Name = "Permiso como Administrador del Área")]
        public bool Permiso { get; set; }

        public SelectList Roles { get; set; }
        public SelectList Areas { get; set; }
        //public SelectList Regiones { get; set; }

        [HiddenInput]
        public int TipoVista { get; set; }

        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }

    }
    public class InfoConfigUsuarioViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Rol { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }

        public string AreaClave { get; set; }
        public int AreaId { get; set; }
        public string Area { get; set; }

        public string Puesto { get; set; }
        public bool PuestoEsUnico { get; set; }

        public string BandejaUsuario { get; set; }
        public int BandejaPermiso { get; set; }
        public string BandejaNombre { get; set; }
        public int BandejaRegionId { get; set; }
        public int BandejaAreaId { get; set; }
        public string BandejaPuesto  { get; set; }
        public bool BandejaPuestoEsUnico { get; set; }
        public bool ActivaDelegacion { get; set; }

        public string  Session { get; set; }
        public string TokenWebApi { get; set; }

        public string CuentaPersonal { get; set; }

        public bool PermisoAA { get; set; }
        public bool EnReasignacion { get; set; }
    }
    public class InfoDelegarUsuarioViewModel
    {
        [HiddenInput]
        public bool EstaActiva { get; set; }
        [HiddenInput]
        public int TipoPermiso { get; set; }
    }
    public class UsuariosViewModel
    {
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }
    }
    public class UsuariosRegionViewModel
    {
        //Area padre 
        public int TipoAreaId { get; set; }
        //--
        public HER_Area AreaPadre { get; set; }
        public HER_Region Region { get; set; }
        public IEnumerable<ListadoAreaPorRegionViewModel> Areas { get; set; }
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }
    }
    public class UsuariosAreaViewModel
    {
        public int? AreaPadreId { get; set; }
        public string Area { get; set; }
        public int AreaId { get; set; }
        public string Region { get; set; }
        public int RegionId { get; set; }
        //--
        public IEnumerable<UsuariosPorAreaViewModel> Usuarios { get; set; }
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }
    }
    public class UsuariosDetallesPlusViewModel
    {
        public UsuariosDetallesViewModel Detalles { get; set; }
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }
    }
    public class UsuariosDesgloseViewModel
    {
        public IEnumerable<HER_Region> Regiones { get; set; }
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }
    }
    //---DocumentosUsuarios
    public class RemitenteDocumentoViewModel
    {
        public int RemitenteId { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string AreaHijo { get; set; }
        public int AreaHijoId { get; set; }
        public string AreaPadre { get; set; }
        public string Usuario { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Puesto { get; set; }
    }
    public class BajaUsuarioViewModel
    {
        //Usuario actual 
        [BindProperty]
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }

        //Info usuario
        [BindProperty]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [BindProperty]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [BindProperty]
        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [BindProperty]
        [Display(Name = "Región")]
        public string RegionNombre { get; set; }

        [BindProperty]
        [Display(Name = "Área")]
        public string AreaNombre { get; set; }

        [BindProperty]
        [Display(Name = "Puesto")]
        public string PuestoNombre { get; set; }

        [BindProperty]
        [Display(Name = "Es único")]
        public string EsUnico { get; set; }

        [BindProperty]
        [Display(Name = "Rol")]
        public string RolNombre { get; set; }

        //Baja
        [BindProperty]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Tipo de baja")]
        public string TipoBaja { get; set; }

        [BindProperty]
        [HiddenInput]
        public int InfoUsuarioId { get; set; }

        [BindProperty]
        [HiddenInput]
        public int RegionId { get; set; }
        
        [BindProperty]
        [HiddenInput]
        public int AreaId { get; set; }
    }
    public class ReasignarUsuarioViewModel
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreUsuario { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Correo { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Rol { get; set; }

        [Display(Name = "Región")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string RegionId { get; set; }

        [Display(Name = "Área")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string AreaId { get; set; }

        [Display(Name = "Puesto")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Puesto { get; set; }

        [Display(Name = "Es único")]
        public bool EsUnico { get; set; }

        [Display(Name = "Utilizar el mismo usuario como titular")]
        public bool EsTitular { get; set; }

        [Display(Name = "Titular")]
        [Condicion("EsTitular", "NombreUsuario", ErrorMessageResourceName = "titular", ErrorMessageResourceType = typeof(SharedResource))]
        public string Titular { get; set; }

        [Display(Name = "Permiso como Administrador del Área")]
        public bool Permiso { get; set; }

        public SelectList Roles { get; set; }
        public SelectList Areas { get; set; }
        public SelectList Regiones { get; set; }

        [HiddenInput]
        public int TipoVista { get; set; }

        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }
    }
    public class AdminReasignarUsuarioViewModel
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string NombreUsuario { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Correo { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(200, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(100, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Telefono { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Rol { get; set; }

        [Display(Name = "Región")]
        [HiddenInput]
        public string RegionId { get; set; }

        [Display(Name = "Área")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string AreaId { get; set; }

        [Display(Name = "Puesto")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Puesto { get; set; }

        [Display(Name = "Es único")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool EsUnico { get; set; }

        [Display(Name = "Utilizar el mismo usuario como titular")]
        public bool EsTitular { get; set; }

        [Display(Name = "Titular")]
        [Condicion("EsTitular", "NombreUsuario", ErrorMessageResourceName = "titular", ErrorMessageResourceType = typeof(SharedResource))]
        public string Titular { get; set; }

        [Display(Name = "Permiso como Administrador del Área")]
        public bool Permiso { get; set; }

        public SelectList Roles { get; set; }
        //public SelectList Areas { get; set; }
        //public SelectList Regiones { get; set; }

        [HiddenInput]
        public int TipoVista { get; set; }

        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }
    }
    public class UsuariosLocalesColeccionJsonModel
    {
        public string usuarios { get; set; }
    }
    public class UsuarioLocalJsonModel
    {
        public string HER_UserName { get; set; }
        public string HER_Correo { get; set; }
        public string HER_NombreCompleto { get; set; }

        public string HER_Region { get; set; }
        public string HER_Area { get; set; }
        public string HER_Puesto { get; set; }

        public string HER_Titular { get; set; }
        public string HER_Tipo { get; set; } //Tipo Rol
        //public bool HER_TieneCuentaTitular { get; set; }

    }

    public class GuardadoUsuarioRespuestaViewModel
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
    }
    public class TerminosViewModel : IValidatableObject
    {
        public bool Aceptar { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Aceptar)
            {
                yield return new ValidationResult($"Debe aceptar el aviso de privacidad para continuar.", new[] { "Aceptar" });
            }
        }
    }
    public class UsuarioDetectaCuentaViewModel
    {
        public string HER_Titular { get; set; }
        public string HER_Rol_Nombre { get; set; }
    }
    //Obtener información del usuario para llenar los Claims
    public class InfoUsuarioNormalClaims
    {
        public string InfoUsuarioId { get; set; }
        public string Username { get; set; }
        public string RegionNombre { get; set; }
        public string RegionId { get; set; }
        public string AreaClave { get; set; }
        public string AreaNombre { get; set; }
        public string AreaId { get; set; }
        public string PuestoNombre { get; set; }
        public string PuestoEsUnico { get; set; }
        public string BandejaUsuario { get; set; }
        public string BandejaPermiso { get; set; }
        public string BandejaNombre { get; set; }
        public string BandejaRegionId { get; set; }
        public string BandejaAreaId { get; set; }
        public string BandejaPuesto { get; set; }
        public string BandejaPuestoEsUnico { get; set; }
        public string Session { get; set; }
        public string ActivaDelegacion { get; set; }

        //Cuenta de dependencia
        public string Rol { get; set; }
        public string Titular { get; set; }

        //Permiso tipo Administrador del área 
        public string PermisoAA { get; set; }
        //Verifica si esta en reasignacion 
        public string EnReasignacion { get; set; }
    }
    public class InfoComplementariaClaims
    {
        public string HER_RegionId { get; set; }
        public string HER_AreaId { get; set; }
        public string HER_Puesto { get; set; }
        public string HER_Puesto_EsUnico { get; set; }
    }
    public class IdentificadorUsuarioCompuestoViewModel 
    {
        public int InfoUsuarioId { get; set; }
        public int AreaId { get; set; }
        public int DiasCompromiso { get; set; }
        //--
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }
    //Permiso especial
    public class PermisoAdminAreaViewModel
    {
        //Usuario actual 
        [BindProperty]
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }

        //Info usuario
        [BindProperty]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [BindProperty]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [BindProperty]
        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [BindProperty]
        [Display(Name = "Región")]
        public string RegionNombre { get; set; }

        [BindProperty]
        [Display(Name = "Área")]
        public string AreaNombre { get; set; }

        [BindProperty]
        [Display(Name = "Puesto")]
        public string PuestoNombre { get; set; }

        [BindProperty]
        [Display(Name = "Es único")]
        public string EsUnico { get; set; }

        [BindProperty]
        [Display(Name = "Rol")]
        public string RolNombre { get; set; }
        
        [BindProperty]
        [Display(Name = "Tiene permiso")]
        public bool Permiso { get; set; }

        [BindProperty]
        [HiddenInput]
        public int InfoUsuarioId { get; set; }

        [BindProperty]
        [HiddenInput]
        public int RegionId { get; set; }

        [BindProperty]
        [HiddenInput]
        public int AreaId { get; set; }
    }

    public class UsuariosBuscarViewModel
    {
        public string HER_NombreCompleto { get; set; }
        public string HER_UserName { get; set; }
        public string HER_Correo { get; set; }
        public string HER_NombreUsuario { get; set; }
        public string HER_Tipo { get; set; }
        public string HER_Puesto { get; set; }
        public string HER_EsUnico { get; set; }
        public string HER_Area { get; set; }
        public string HER_Region { get; set; }
        public string HER_LigaArea { get; set; }
        public string HER_Aprobado { get; set; }
        public string HER_FechaAprobacion { get; set; }
        public string HER_Titular { get; set; }
        public string HER_Estado { get; set; }
        public string HER_AceptoTerminos { get; set; }
        public string HER_FechaTerminos { get; set; }
        public string HER_PermisoAdministradorArea { get; set; }
        public string HER_FechaRegistro { get; set; }
        public string HER_FechaActualizacion { get; set; }
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }
    }
}
