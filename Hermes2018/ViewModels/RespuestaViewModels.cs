using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class DocumentoRespuestaLecturaViewModel
    {
        //[Origen]
        //Documento Origen
        public string Origen_Asunto { get; set; }
        public string Origen_Folio { get; set; }
        public string Origen_NoInterno { get; set; }
        public string Origen_Fecha { get; set; }
        public string Origen_Cuerpo { get; set; }
        public string Origen_TipoDocumento { get; set; }
        public List<string> Origen_ListadoCcp { get; set; }
        public List<string> Origen_ListadoCcpCorreo { get; set; }
        public bool ExisteAdjuntos { get; set; }
        public int EnvioIdOrigenAdjunto { get; set; }
        //Usuario Origen (De inicial)
        public string Origen_UsuarioDe_Correo { get; set; }
        public string Origen_UsuarioDe_NombreCompleto { get; set; }
        public string Origen_UsuarioDe_Direccion { get; set; }
        public string Origen_UsuarioDe_Telefono { get; set; }
        //--
        public string Origen_UsuarioDe_AreaPadreNombre { get; set; }
        public string Origen_UsuarioDe_Region { get; set; }
        public string Origen_UsuarioDe_AreaNombre { get; set; }
        public int Origen_UsuarioDe_AreaId { get; set; }
        public string Origen_UsuarioDe_PuestoNombre { get; set; }
        //--
        //Usuario (Para inicial)
        public string Origen_UsuarioPara_NombreCompleto { get; set; }
        public string Origen_UsuarioPara_AreaNombre { get; set; }
        public string Origen_UsuarioPara_PuestoNombre { get; set; }
        public int Origen_UsuarioPara_Tipo { get; set; }

        //[Envio origen]
        public int Origen_EnvioId { get; set; }
        public string Origen_FechaEnvio { get; set; }
        public int Origen_TipoEnvioId { get; set; }

        //[Actual]
        //Usuario que Responde (De)
        public string Actual_UsuarioDe_Correo { get; set; }
        public string Actual_UsuarioDe_NombreCompleto { get; set; }
        public string Actual_UsuarioDe_Direccion { get; set; }
        public string Actual_UsuarioDe_Telefono { get; set; }
        //--
        public string Actual_UsuarioDe_AreaPadreNombre { get; set; }
        public string Actual_UsuarioDe_Region { get; set; }
        public string Actual_UsuarioDe_AreaNombre { get; set; }
        public int Actual_UsuarioDe_AreaId { get; set; }
        public string Actual_UsuarioDe_PuestoNombre { get; set; }
        
        //Usuario a Responder (Para)
        public string Actual_UsuarioPara_NombreUsuario { get; set; }
        public string Actual_UsuarioPara_Correo { get; set; }
        public string Actual_UsuarioPara_NombreCompleto { get; set; }
        public string Actual_UsuarioPara_AreaNombre { get; set; }
        public string Actual_UsuarioPara_PuestoNombre { get; set; }
        public int Actual_UsuarioPara_UsuarioId { get; set; }

        //[Envio actual]
        public int Actual_EnvioId { get; set; }
        public string Actual_FechaEnvio { get; set; }
        public int Actual_TipoEnvioId { get; set; }
        //Respuesta 
        public string Actual_AsuntoRespuesta { get; set; }
        //Turnado
        public bool Actual_EsTurnado { get; set; }
        public string Actual_Indicaciones { get; set; }
        //Tipo de usuario que lee (Para o CCP)
        public int Actual_UsuarioLee_Tipo { get; set; }

        //[Estado de la respuesta]
        public bool TieneRespuesta { get; set; }
    }
    public class RespuestaDocumentoViewModel
    {
        [HiddenInput]
        public int Origen_EnvioId { get; set; }
        [HiddenInput]
        public int Origen_TipoEnvioId { get; set; }

        [HiddenInput]
        public int Actual_EnvioId { get; set; }
        [HiddenInput]
        public int Actual_TipoEnvioId { get; set; }

        [HiddenInput]
        public int UsuarioParaId { get; set; }

        [HiddenInput]
        public string NombreUsuarioPara { get; set; }

        [Display(Name = "Folio")]
        public string Folio { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [HiddenInput]
        [Display(Name = "Para")]
        public string Para { get; set; }

        [HiddenInput]
        [Display(Name = "CC")]
        public string CCP { get; set; }

        [HiddenInput]
        [Display(Name = "Anexos")]
        public string Anexos { get; set; }

        [HiddenInput]
        public string Categorias { get; set; }

        [Display(Name = "NoInterno")]
        public string NoInterno { get; set; }

        public string Fecha { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Cuerpo")]
        [HiddenInput]
        public string Cuerpo { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Visibilidad")]
        public int VisibilidadId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Tipo de documento")]
        public int TipoId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Tipo de respuesta")]
        public int TipoRespuestaId { get; set; }

        //--
        [BindProperty]
        [HiddenInput]
        public string FolioSession { get; set; }

        public SelectList PlantillasSelectList { get; set; }
        public SelectList TiposSelectList { get; set; }
        public SelectList VisibilidadSelectList { get; set; }
        public SelectList TiposRespuestaSelectList { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Extensiones { get; set; }
    }
    public class NuevoDocumentoRespuestaViewModel
    {
        //CONTENIDO:
        public string Folio { get; set; }
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public string Cuerpo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public int TipoId { get; set; }

        //DE:
        public int TitularId { get; set; }
        public int CreadorId { get; set; }
    }
    public class CategoriaDocumentoRespuestaViewModel
    {
        public int CategoriaId { get; set; }
        public int OficioRespuestaId { get; set; }
    }
    public class EnvioRespuestaViewModel
    {
        public int EnvioActualId { get; set; }
        public int EnvioId { get; set; }
        //--
        public int DocumentoRespuestaId { get; set; }
        public string Folio { get; set; }
        //--
        public int UsuarioDeId { get; set; }
        public string UsuarioDeDireccion { get; set; }
        public string UsuarioDeTelefono { get; set; }
        public int UsuarioEnviaId { get; set; }
        public int TotalPara { get; set; }
        public int TotalCCP { get; set; }
        
        public int TipoRespuestaId { get; set; }
        public int VisibilidadId { get; set; }
        public int? AnexoId { get; set; }
    }
    public class RecepcionRespuestaViewModel
    {
        public int RespuestaEnvioId { get; set; }
        public int ParaId { get; set; }
        public int TipoPara { get; set; }
    }
}
