using Hermes2018.Attributes;
using Hermes2018.Models;
using Hermes2018.Models.Anexo;
using Hermes2018.Models.Documento;
using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hermes2018.ViewModels
{
    public class DocumentoBorradorViewModel
    {
        //Listado de Borradores
        public int DocumentoId { get; set; } 
        public string Folio { get; set; } 
        public string Remitente { get; set; } 
        public string Destinatario { get; set; } 
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } 
        public bool Importancia { get; set; } 
        public int Destinatarios_Extras { get; set; } 
        public bool Adjuntos { get; set; } 
    }
    public class DocumentoRevisionViewModel
    {
        public int RevisionId { get; set; }
        public string Folio { get; set; }
        public string Remitente { get; set; }
        public string Destinatario { get; set; }
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public bool Importancia { get; set; }
        public int TipoEnvio { get; set; }
        public int Estado { get; set; }
        public bool Adjuntos { get; set; }
    }
    public class NuevoDocumentoBaseViewModel
    {
        public string Folio { get; set; }
        public bool RequiereRespuesta { get; set; }
        public bool EnRevision { get; set; }
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public string Cuerpo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string FechaPropuesta { get; set; }
        public int ImportanciaId { get; set; }
        public int TipoId { get; set; }
        public int EstadoId { get; set; }
        public int VisibilidadId { get; set; }

        //DE:
        public int TitularId { get; set; }
        public int CreadorId { get; set; }

        public int? AnexoId { get; set; }

        //PARA: 
        public List<int> Destinatarios { get; set; }

        //CCP:
        public List<int> CCP { get; set; }
    }
    public class NuevoDocumentoViewModel
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
    public class ActualizarDocumentoBaseViewModel
    {
        public int DocumentoBaseId { get; set; }
        public string Folio { get; set; }
        public bool RequiereRespuesta { get; set; }
        public bool EnRevision { get; set; }
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public string Cuerpo { get; set; }

        public int ImportanciaId { get; set; }
        public int TipoId { get; set; }
        public int EstadoId { get; set; }
        public int VisibilidadId { get; set; }

        public string FechaPropuesta { get; set; }

        //PARA: 
        public List<int> Destinatarios { get; set; }

        //CCP:
        public List<int> CCP { get; set; }
    }
    public class EnviarRevisionViewModel
    {
        public int DocumentoBaseId { get; set; }
        public int RemitenteId { get; set; }
        public int DestinatarioId { get; set; }
        public int Estado_Remitente { get; set; }
        public int Estado_Destinatario { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class ActualizarEstadoRevisionViewModel
    {
        public int DocumentoBaseId { get; set; }
        public string Folio { get; set; }
        public int Estado_Remitente { get; set; }
        public int Estado_Destinatario { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class BorrarDocumentoBaseViewModel
    {
        [HiddenInput]
        public int DocumentoBaseId { get; set; }
        [HiddenInput]
        [Display (Name = "Folio")]
        public string Folio { get; set; }
        [HiddenInput]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }
        [HiddenInput]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
        [HiddenInput]
        [Display(Name = "Fecha de registro")]
        public DateTime Fecha { get; set; }
        [HiddenInput]
        [Display(Name = "Fecha propuesta")]
        public DateTime FechaPropuesta { get; set; }
    }
    public class EstadoDocumentoViewModel
    {
        public string Folio { get; set; }
        public int Estado { get; set; }
    }
    public class ActualizarRevisionRemitenteViewModel
    {
        public int DocumentoBaseId { get; set; }
        public string Folio { get; set; }

        public bool RequiereRespuesta { get; set; }
        public int ImportanciaId { get; set; }
        public int TipoId { get; set; }
        public int VisibilidadId { get; set; }
        public string NoInterno { get; set; }
        public string Cuerpo { get; set; }
    }
    public class ActualizarRevisionDestinatarioViewModel
    {
        public int DocumentoBaseId { get; set; }
        public string Folio { get; set; }
        public string NoInterno { get; set; }
        public string Cuerpo { get; set; }
    }
    public class CrearDocumentoViewModel
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Para")]
        [HiddenInput]
        public string Para { get; set; }

        [Display(Name = "CC")]
        [HiddenInput]
        public string CCP { get; set; }

        [HiddenInput]
        public string Categorias { get; set; }

        [Display(Name = "Anexos")]
        [HiddenInput]
        public string Anexos { get; set; }

        [HiddenInput]
        public string FolioSession { get; set; }

        [Condicionbolean("RequiereRespuesta", ErrorMessage = "El dato es requerido.")]
        [Display(Name = "Fecha propuesta")]
        public string FechaPropuesta { get; set; }
        
        [HiddenInput]
        public int AreaId { get; set; }

        [HiddenInput]
        public string Extensiones { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Cuerpo")]
        [HiddenInput]
        public string Cuerpo { get; set; }

        [Display(Name = "No Oficio")]
        public string NoInterno { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Folio")]
        public string Folio { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Respuesta")]
        public bool RequiereRespuesta { get; set; }

        public bool EsRevision { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Importancia")]
        public int ImportanciaId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Visibilidad")]
        public int VisibilidadId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Tipo")]
        public int TipoId { get; set; }

        [BindProperty]
        [HiddenInput]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string TipoSubmit { get; set; }

        [BindProperty]
        public string Fecha { get; set; }

        //---
        public SelectList PlantillasSelectList { get; set; }
        public SelectList ImportanciaSelectList { get; set; }
        public SelectList TiposSelectList { get; set; }
        public SelectList VisibilidadSelectList { get; set; }
    }
    public class EditarDocumentoViewModel
    {
        [BindProperty]
        [HiddenInput]
        public int DocumentoBaseId { get; set; }

        [BindProperty]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Para")]
        [HiddenInput]
        public string Para { get; set; }

        [BindProperty]
        [Display(Name = "CC")]
        [HiddenInput]
        public string CCP { get; set; }

        [BindProperty]
        [Display(Name = "Anexos")]
        [HiddenInput]
        public string Anexos { get; set; }

        [BindProperty]
        [Display(Name = "Categorías")]
        [HiddenInput]
        public string Categorias { get; set; }

        [BindProperty]
        [HiddenInput]
        public string FolioSession { get; set; }

        [Condicionbolean("RequiereRespuesta", ErrorMessage = "El dato es requerido.")]
        [Display(Name = "Fecha propuesta")]
        public string FechaPropuesta { get; set; }

        [BindProperty]
        [HiddenInput]
        public int AreaId { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Extensiones { get; set; }

        //[BindProperty]
        //[HiddenInput]
        //public string RequiereRespuesta { get; set; }

        //[BindProperty]
        //[HiddenInput]
        //public string Importancia { get; set; }

        //[BindProperty]
        //[HiddenInput]
        //public string TipoDocumento { get; set; }

        public IEnumerable<HER_AnexoArchivo> Archivos { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Cuerpo")]
        [HiddenInput]
        public string Cuerpo { get; set; }

        [Display(Name = "No Oficio")]
        public string NoInterno { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Folio")]
        public string Folio { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Respuesta")]
        public bool RequiereRespuesta { get; set; }

        public string RequiereRespuestaTexto { get; set; }

        public bool EsRevision { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Importancia")]
        public int ImportanciaId { get; set; }

        public string Importancia { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Visibilidad")]
        public int VisibilidadId { get; set; }

        public string Visibilidad { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Tipo")]
        public int TipoId { get; set; }

        public string Tipo { get; set; }

        [BindProperty]
        [HiddenInput]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string TipoSubmit { get; set; }

        [BindProperty]
        public string Fecha { get; set; }

        //--------------------
        public SelectList PlantillasSelectList { get; set; }
        public SelectList ImportanciaSelectList { get; set; }
        public SelectList TiposSelectList { get; set; }
        public SelectList VisibilidadSelectList { get; set; }
    }

    public class GuardarHistorialCorreo
    {
        public DateTime Fecha { get; set; }
        
        public int Remitente { get; set; }

        public string EsDelegado { get; set; }

        public string Destinatario { get; set; }

        public int EnvioId { get; set; }

        public int Tipo { get; set; }

        public int TipoEnvio { get; set; }

    }

    public class ListadoHistorialCorreo
    {
        public DateTime Fecha { get; set; }

        public string Remitente { get; set; }

        public string EsDelegado { get; set; }

        public string Destinatario { get; set; }

        public int EnvioId { get; set; }

        public int Tipo { get; set; }

        public int TipoEnvio { get; set; }
    }

    public class NuevoCompromisoJsonModel
    {
        [Display(Name = "Trámite")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string TramiteId { get; set; }

        [Display(Name = "Motivo")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [StringLength(500, ErrorMessageResourceName = "stringlength", ErrorMessageResourceType = typeof(SharedResource))]
        public string Motivo { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string FechaCompromiso { get; set; }

        [HiddenInput]
        public int RecepcionId { get; set; }
        [HiddenInput]
        public int EnvioId { get; set; }
        [HiddenInput]
        public int TipoEnvioId { get; set; }
    }

    public class CompromisoAceptadoJsonModel
    {
        [HiddenInput]
        public string TramiteId { get; set; }
        [HiddenInput]
        public string Motivo { get; set; }
        [HiddenInput]
        public string FechaCompromiso { get; set; }
        [HiddenInput]
        public int RecepcionId { get; set; }
        [HiddenInput]
        public int EnvioId { get; set; }
        [HiddenInput]
        public int TipoEnvioId { get; set; }
    }

    public class NumerologiaEnvioViewModel
    {
        [HiddenInput]
        public int TotalTurnados { get; set; }
    }
}
