using Hermes2018.Attributes;
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
    public class EnvioViewModel
    {
        public int DocumentoId { get; set; }
        public int UsuarioDeId { get; set; }
        public string UsuarioDeDireccion { get; set; }
        public string UsuarioDeTelefono { get; set; }
        public int UsuarioEnviaId { get; set; }
        public int TotalPara { get; set; }
        public int TotalCCP { get; set; }
        public bool RequiereRespuesta { get; set; }

        public int ImportanciaId { get; set; }
        public int VisibilidadId { get; set; }
        public int? AnexoId { get; set; }

        public string FechaPropuesta { get; set; }
    }
    public class RecepcionViewModel
    {
        public int EnvioId { get; set; }
        public int ParaId { get; set; }
        public int TipoPara { get; set; }
        public bool RequiereRespuesta { get; set; }
    }
    public class DocumentoEnviadoViewModel
    {
        public int EnvioId { get; set; }
        public int TipoEnvio { get; set; }
        //Elemento comodin para la mostracion de los iconos dado que  hay un reenviar como turnado
        public int TipoEnvioParaIcono { get; set; }
        public bool Importancia { get; set; }
        public string Estado { get; set; }

        public string Folio { get; set; }
        public string NoInterno { get; set; }
        public string Asunto { get; set; }
        public bool Adjuntos { get; set; }

        //--
        public string Destinatario { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        //--
        public int Destinatarios_Extras { get; set; }
       
        public DateTime FechaEnvio { get; set; }
        public bool ConFechaCompromiso { get; set; }

        //--Tramite
        public int? TramiteId { get; set; }
        public string Tramite { get; set; }
        public string Carpeta { get; set; }
    }
    public class DocumentoRecibidoViewModel
    {
        public int EnvioId { get; set; }
        public int TipoEnvio { get; set; }
        public int TipoEnvioParaIcono { get; set; }
        public bool Importancia { get; set; } //Prioridad
        public bool RequiereRespuesta { get; set; }
        public string Estado { get; set; } //EstadoEnvio

        //--Tramite
        public int? TramiteId { get; set; }
        public string Tramite { get; set; }

        //--Documento
        public string Folio { get; set; }
        public string NoInterno { get; set; }
        public string Asunto { get; set; }
        public bool Adjuntos { get; set; }

        //--De
        public string Remitente { get; set; } 
        public string Area { get; set; }
        public string Region { get; set; }

        //--Para actual
        public DateTime FechaRecepcion { get; set; }
        public DateTime FechaCompromiso { get; set; }
        public int TipoPara { get; set; } //Tipo del destinatario: para o ccp
        public bool Leido { get; set; }
        public string Carpeta { get; set; }

        public ComplementoDocumentoRecibidoViewModel Proximo { get; set; }
    }
    public class ComplementoDocumentoRecibidoViewModel
    {
        //Siguiente turnado del documento actual

        public int EnvioId { get; set; }
        public int TipoEnvio { get; set; }
        public DateTime FechaEnvio { get; set; }

        //--Para
        public List<ComplementoDestinararios> Destinatarios { get; set; }
    }
    public class ComplementoDestinararios
    {
        public string Destinatario { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }

        public DateTime FechaRecepcion { get; set; }
        public DateTime FechaCompromiso { get; set; }
        public int TipoPara { get; set; } //Tipo del destinatario: para o ccp
    }
    public class DocumentoEnviadoLecturaViewModel
    {
        //Documento Origen
        public int Origen_DocumentoId { get; set; }
        public string Origen_Asunto { get; set; }
        public string Origen_Folio { get; set; }
        public string Origen_NoInterno { get; set; }
        public string Origen_Fecha { get; set; }
        public string Origen_Cuerpo { get; set; }
        //--
        public int? Origen_TramiteId { get; set; }
        public string Origen_Tramite { get; set; }
        public string Origen_Importancia { get; set; }
        public string Origen_Visibilidad { get; set; }
        public string Origen_TipoDocumento { get; set; }
        public int Origen_TipoDocumentoId { get; set; }
        public bool Origen_RequiereRespuesta { get; set; }
        public bool Origen_ExisteAdjuntos { get; set; }
        public List<string> Origen_ListadoCcp { get; set; }
        public List<string> Origen_ListadoCcpCorreo { get; set; }

        //Usuario De Origen
        public string Origen_UsuarioDe_Correo { get; set; }
        public string Origen_UsuarioDe_NombreUsuario { get; set; }
        public string Origen_UsuarioDe_NombreCompleto { get; set; }
        public string Origen_UsuarioDe_Direccion { get; set; }
        public string Origen_UsuarioDe_Telefono { get; set; }
        public string Origen_UsuarioDe_AreaPadreNombre { get; set; }
        public string Origen_UsuarioDe_AreaNombre { get; set; }
        public int Origen_UsuarioDe_AreaId { get; set; }
        public string Origen_UsuarioDe_Region { get; set; }
        public string Origen_UsuarioDe_PuestoNombre { get; set; }
        
        //Usuario Para Origen
        public string Origen_UsuarioPara_NombreCompleto { get; set; }
        public string Origen_UsuarioPara_AreaNombre { get; set; }
        public string Origen_UsuarioPara_PuestoNombre { get; set; }
        public int Origen_UsuarioPara_Tipo { get; set; }

        //--
        public string Origen_FechaCompromiso { get; set; }
        public bool Origen_FijoFechaCompromiso { get; set; }

        //Nombre de quien lo ha creado/ ya sea el titular o el delegado
        public string Origen_NombreCreador { get; set; }
        public string Origen_UsuarioCreador { get; set; }
        public string Origen_NombreTitular { get; set; }
        public string Origen_UsuarioTitular { get; set; }

        //Envio Origen
        public int Origen_EnvioId { get; set; }
        public int Origen_TipoEnvioId { get; set; }
        public string Origen_TipoEnvio { get; set; }
        public int Origen_GrupoEnvio { get; set; }

        //Actual
        public int Actual_EnvioId { get; set; }
        [HiddenInput]
        public int Actual_RecepcionId { get; set; }
        public string Actual_AsuntoEnvio { get; set; }
        public int? Actual_TramiteId { get; set; }
        public string Actual_Tramite { get; set; }
        public string Actual_Importancia { get; set; }
        public string Actual_Visibilidad { get; set; }
        public bool Actual_RequiereRespuesta { get; set; }
        public string Actual_Fecha { get; set; } /*- Fecha de envio/recepción -*/
        public string Actual_FechaCompromiso { get; set; }
        public bool Actual_FijoFechaCompromiso { get; set; }
        public string Actual_FechaPropuesta { get; set; }
        public bool Actual_EstaLeido { get; set; }
        public int Actual_TipoEnvioId { get; set; }
        public bool Actual_EsReenvio { get; set; }
        public bool Actual_SupleTurnado { get; set; }
        public string Actual_TipoEnvio { get; set; }
        public bool Actual_ExisteAdjuntos { get; set; }

        //Usuario De
        public string Actual_UsuarioDe_Correo { get; set; }
        public string Actual_UsuarioDe_NombreCompleto { get; set; }
        public string Actual_UsuarioDe_NombreUsuario { get; set; }

        //Usuario Envia Para
        public string Actual_UsuariosPara { get; set; }
        public string Actual_UsuariosCCP { get; set; }
        public string Actual_CorreosCCP { get; set; }


        //Visualización
        public int Actual_Visualizacion_Tipo { get; set; }

        //Turnado
        public bool Actual_EsTurnado { get; set; }
        public string Actual_Indicaciones { get; set; }
        
        //Respuesta
        public bool Actual_TieneRespuesta { get; set; }

        //Estado Envio-Recepcion   
        public int Actual_EstadoId { get; set; }
        public string Actual_Estado { get; set; }

        public int Actual_UsuarioLee_Tipo { get; set; }
        
        //Carpeta
        public int? Actual_CarpetaId { get; set; }
    }
    public class DocumentoEnviadoVisualizacionViewModel
    {
        //Documento Origen
        public int Origen_EnvioId { get; set; }
        public string Origen_Asunto { get; set; }
        public string Origen_Folio { get; set; }
        public string Origen_NoInterno { get; set; }
        public string Origen_Fecha { get; set; }
        public string Origen_Cuerpo { get; set; }
        //--
        public int? Origen_TramiteId { get; set; }
        public string Origen_Tramite { get; set; }
        public string Origen_Importancia { get; set; }
        public string Origen_Visibilidad { get; set; }
        public string Origen_TipoDocumento { get; set; }
        public int Origen_TipoDocumentoId { get; set; }
        //--
        public int Origen_TipoEnvioId { get; set; }
        public string Origen_TipoEnvio { get; set; }
        //--
        public bool Origen_RequiereRespuesta { get; set; }
        public bool Origen_ExisteAdjuntos { get; set; }
        public List<string> Origen_ListadoCcp { get; set; }
        public List<string> Origen_ListadoCcpCorreo { get; set; }

        //Usuario De Origen
        public string Origen_UsuarioDe_NombreCompleto { get; set; }
        public string Origen_UsuarioDe_Correo { get; set; }
        public string Origen_UsuarioDe_Direccion { get; set; }
        public string Origen_UsuarioDe_Telefono { get; set; }

        public string Origen_UsuarioDe_AreaPadreNombre { get; set; }
        public string Origen_UsuarioDe_AreaNombre { get; set; }
        public int Origen_UsuarioDe_AreaId { get; set; }
        public string Origen_UsuarioDe_Region { get; set; }
        public string Origen_UsuarioDe_PuestoNombre { get; set; }

        //Usuario Para Origen
        public string Origen_UsuarioPara_NombreCompleto { get; set; }
        public string Origen_UsuarioPara_AreaNombre { get; set; }
        public string Origen_UsuarioPara_PuestoNombre { get; set; }
        public int Origen_UsuarioPara_Tipo { get; set; }

        //Nombre de quien lo ha creado/ ya sea el titular o el delegado
        public string Origen_NombreCreador { get; set; }
        public string Origen_UsuarioCreador { get; set; }
        public string Origen_NombreTitular { get; set; }
        public string Origen_UsuarioTitular { get; set; }

        //Actual 
        public int Actual_EnvioId { get; set; }
        public string Actual_AsuntoEnvio { get; set; }
        public int? Actual_TramiteId { get; set; }
        public string Actual_Tramite { get; set; }
        public string Actual_Importancia { get; set; }
        public string Actual_Visibilidad { get; set; }
        public bool Actual_RequiereRespuesta { get; set; }
        public string Actual_Fecha { get; set; }
        public string Actual_FechaCompromiso { get; set; }
        public string Actual_FechaPropuesta { get; set; }
        public int Actual_TipoEnvioId { get; set; }
        public bool Actual_EsReenvio { get; set; }
        public string Actual_TipoEnvio { get; set; }
        //--
        public bool Actual_ExisteAdjuntos { get; set; }
        public bool Actual_EsTurnado { get; set; }
        public string Actual_Indicaciones { get; set; }
        public bool Actual_SupleTurnado { get; set; }
        //Usuario De
        public string Actual_UsuarioDe_Correo { get; set; }
        public string Actual_UsuarioDe_NombreCompleto { get; set; }
        //Usuarios Para y CCP
        public string Actual_UsuariosPara { get; set; }
        public string Actual_UsuariosCCP { get; set; }

        //Estado Envio-Recepcion   
        public int Actual_EstadoId { get; set; }
        public string Actual_Estado { get; set; }

        //Respuesta
        public bool Actual_TieneRespuesta { get; set; }

        //Visualización
        public int Actual_Visualizacion_Tipo { get; set; }

        //Tipo de usuario que lee (Para o CCP)
        public int Actual_UsuarioLee_Tipo { get; set; }
    }
    public class ResumenDestinatarioViewModel
    {
        //Usuario
        public string NombreCompleto { get; set; }
        //Recepción
        public string FechaRecepcion { get; set; }
        public string FechaCompromiso { get; set; }
        public int EstadoEnvioId { get; set; }
        public string EstadoEnvio { get; set; }
        public bool EstaLeido { get; set; }
        public bool RequiereRespuesta { get; set; }
        //--Respuesta
        public bool TieneRespuesta { get; set; }
        public bool ConFechaCompromiso { get; set; }
        public int EnvioId { get; set; } 
        public int TipoEnvioId { get; set; }
        public string FechaRespuesta { get; set; }

        public int RecepcionId { get; set; }

        public FechasCompromisoDestinatariosViewModel FechaCompromisoActual { get; set; }

        public List<FechasCompromisoDestinatariosViewModel> FechasCompromisoHistorico { get; set; }
    }
    public class FechasCompromisoDestinatariosViewModel
    {
        public int CompromisoId { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
        public string Fecha { get; set; }
        public string Motivo { get; set; }
        public string Registro { get; set; }
    }
    public class ResumenEnvioViewModel
    {
        public string Usuario { get; set; }
        public int EnvioId { get; set; }
        public int TipoEnvioId { get; set; }
        public string FechaRespuesta { get; set; }
    }
    public class ResumenRecepcionViewModel
    {
        public int RecepcionId { get; set; }

        public string Usuario { get; set; }
        public string NombreCompleto { get; set; }
        public int TipoPara { get; set; }
        public string FechaRecepcion { get; set; }
        public string FechaCompromiso { get; set; }
        public int EstadoEnvioId { get; set; }
        public string EstadoEnvio { get; set; }
        public bool EstaLeido { get; set; }
        public bool ConFechaCompromiso { get; set; }
        public bool RequiereRespuesta { get; set; }
        public bool TieneRespuesta { get; set; }

        public List<FechasCompromisoDestinatariosViewModel> FechasCompromiso { get; set; }
    }
    public class TurnarDocumentoLecturaViewModel
    {
        //Documento Origen
        public string Asunto { get; set; }
        public string Folio { get; set; }
        public string NoInterno { get; set; }
        public string Fecha { get; set; }
        public string Cuerpo { get; set; }
        public string TipoDocumento { get; set; }
        public List<string> ListadoCcp { get; set; }
        public List<string> ListadoCcpCorreo { get; set; }
        //Usuario Origen
        public string UsuarioCorreo { get; set; }
        public string UsuarioNombreCompleto { get; set; }
        public string UsuarioDireccion { get; set; }
        public string UsuarioTelefono { get; set; }
        public string AreaPadreNombre { get; set; }
        public string AreaNombre { get; set; }
        public int AreaId { get; set; }
        public string RegionNombre { get; set; }
        public string PuestoNombre { get; set; }
        public bool ExisteAdjuntos { get; set; }
        public int EnvioIdOrigenAdjunto { get; set; }

        //Usuario Para Actual
        public string UsuarioPara_NombreCompleto { get; set; }
        public string UsuarioPara_AreaNombre { get; set; }
        public string UsuarioPara_PuestoNombre { get; set; }
        //Tipo de usuario que lee (Para o CCP)
        public int UsuarioPara_Tipo { get; set; }

        //[Actual]
        //Envio
        public int EnvioId { get; set; }
        public int TipoEnvioId { get; set; }
        public string TipoEnvio { get; set; }
        public string AsuntoEnvio { get; set; }
        public string FechaEnvio { get; set; }
        public string Indicaciones { get; set; }

        public bool RequiereRespuesta { get; set; }

        //Recepcion
        public string FechaCompromiso { get; set; }
    }
    public class TurnarDocumentoViewModel
    {
        [HiddenInput]
        public int EnvioId { get; set; }

        [HiddenInput]
        public int TipoEnvioId { get; set; }

        [HiddenInput]
        public string TipoEnvio { get; set; }

        [HiddenInput]
        public string Folio { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [HiddenInput]
        [Display(Name = "Para")]
        public string Para { get; set; }

        [HiddenInput]
        [Display(Name = "CC")]
        public string CCP { get; set; }

        [HiddenInput]
        public string Anexos { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool RequiereRespuesta { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Importancia")]
        public int ImportanciaId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Visibilidad")]
        public int VisibilidadId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Indicaciones { get; set; }

        [Condicionbolean("RequiereRespuesta", ErrorMessage = "El dato es requerido.")]
        [Display(Name = "Fecha propuesta")]
        public string FechaPropuesta { get; set; }

        [BindProperty]
        public string UsuarioDe_Turnar { get; set; }

        [BindProperty]
        [HiddenInput]
        public string FolioSession { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Extensiones { get; set; }

        public SelectList ImportanciaSelectList { get; set; }
        public SelectList VisibilidadSelectList { get; set; }
    }
    public class TurnarViewModel
    {
        public string Folio { get; set; }
        public int EnvioId { get; set; }
        public int Usuario_DeId { get; set; }
        public int Usuario_EnviaId { get; set; }
        public int Total_Para { get; set; }
        public int Total_CCP { get; set; }
        public string Indicaciones { get; set; }
        public bool RequiereRespuesta { get; set; }
        public int ImportanciaId { get; set; }
        public int VisibilidadId { get; set; }
        public int? AnexoId { get; set; }

        public string FechaPropuesta { get; set; }
    }
    public class RecepcionTurnarViewModel
    {
        //envioId
        public int EnvioId { get; set; }
        public int Usuario_ParaId { get; set; }
        public int Tipo_Para { get; set; }
        public bool RequiereRespuesta { get; set; }
    }
    public class DocumentoEnCarpetaPersonalViewModel
    {
        public string Bandeja_Origen { get; set; }

        public int EnvioId { get; set; }
        public string Folio { get; set; }
        public string Remitente { get; set; }
        public string Destinatario { get; set; }
        public int Destinatarios_Extra { get; set; }
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public DateTime Fecha { get; set; }
        public bool Importancia { get; set; }
        public bool Adjuntos { get; set; }
        public int TipoEnvio { get; set; }
        public string Estado { get; set; }
        public bool Leido { get; set; }

        public int TipoPara { get; set; }
    }
    public class ResumenEnvioDocumentoCorreoViewModel
    {
        //Origen
        //Oficio
        public string Folio { get; set; }
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public string Fecha { get; set; }
        public string Cuerpo { get; set; }
        public string Importancia { get; set; }
        public string TipoDocumento { get; set; }
        public int TipoEnvioId { get; set; }
        public string Indicaciones { get; set; }
        public bool EsReenvio { get; set; }

        //Usuario De Origen
        public ResumenInfoRemitenteOrigenViewModel Origen_UsuarioDe { get; set; }
        //Usuario Para Origen
        public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioPara { get; set; }
        //Usuario CCP Origen
        public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioCcp { get; set; }
        //Nombre de quien lo ha creado/ ya sea el titular o el delegado
        public string Origen_NombreCreador { get; set; }
        public string Origen_UsuarioCreador { get; set; }
        public string Origen_NombreTitular { get; set; }
        public string Origen_UsuarioTitular { get; set; }
        //Anexos Origen
        public List<string> Origen_Anexos { get; set; }
    }
    public class ResumenTurnarDocumentoCorreoViewModel
    {
        //Origen
        //Documento
        public string Folio { get; set; }
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public string Fecha { get; set; }
        public string Cuerpo { get; set; }
        public string Importancia { get; set; }
        public string TipoDocumento { get; set; }

        //Usuario De Origen
        public ResumenInfoRemitenteOrigenViewModel Origen_UsuarioDe { get; set; }
        //Usuario Para Origen
        public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioPara { get; set; }
        public ResumenInfoDestinatariosOrigenViewModel Origen_UsuarioParaBase { get; set; }
        //Usuario CCP Origen
        public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioCcp { get; set; }
        //Nombre de quien lo ha creado/ ya sea el titular o el delegado
        public string Origen_NombreCreador { get; set; }
        public string Origen_UsuarioCreador { get; set; }
        public string Origen_NombreTitular { get; set; }
        public string Origen_UsuarioTitular { get; set; }
        //Anexos Origen
        public List<string> Origen_Anexos { get; set; }

        //Actual 
        public string Actual_AsuntoEnvio { get; set; }
        public string Actual_Importancia { get; set; }
        public string Actual_Fecha { get; set; }
        public string Actual_Indicaciones { get; set; }
        //Usuario De
        public ResumenInfoRemitenteViewModel Actual_UsuarioDe { get; set; }
        //Usuarios Para
        public List<ResumenInfoDestinatariosViewModel> Actual_UsuarioPara { get; set; }
        //Usuarios Ccp
        public List<ResumenInfoDestinatariosViewModel> Actual_UsuarioCcp { get; set; }
        //Anexos
        public List<string> Actual_Anexos { get; set; }
    }
    public class ResumenTurnarEspecialDocumentoCorreoViewModel
    {
        //Origen
        //Documento
        public string Folio { get; set; }
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public string Fecha { get; set; }
        public string Cuerpo { get; set; }
        public string Importancia { get; set; }
        public string TipoDocumento { get; set; }

        //Usuario De Origen
        public ResumenInfoRemitenteOrigenViewModel Origen_UsuarioDe { get; set; }
        //Usuario Para Origen
        public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioPara { get; set; }
        public ResumenInfoDestinatariosOrigenViewModel Origen_UsuarioParaBase { get; set; }
        //Usuario CCP Origen
        public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioCcp { get; set; }
        //Nombre de quien lo ha creado/ ya sea el titular o el delegado
        public string Origen_NombreCreador { get; set; }
        public string Origen_UsuarioCreador { get; set; }
        public string Origen_NombreTitular { get; set; }
        public string Origen_UsuarioTitular { get; set; }
        //Anexos Origen
        public List<string> Origen_Anexos { get; set; }

        //Actual 
        public string Actual_AsuntoEnvio { get; set; }
        public string Actual_Importancia { get; set; }
        public string Actual_Fecha { get; set; }
        public string Actual_Indicaciones { get; set; }
        //Usuario De
        public ResumenInfoRemitenteViewModel Actual_UsuarioDe { get; set; }
        //Usuarios Para
        public List<ResumenInfoDestinatariosViewModel> Actual_UsuarioPara { get; set; }
        //Usuarios Ccp
        public List<ResumenInfoDestinatariosViewModel> Actual_UsuarioCcp { get; set; }
        //Anexos
        public List<string> Actual_Anexos { get; set; }
    }
    public class ResumenResponderDocumentoCorreoViewModel
    {
        //Origen
        //Oficio
        public string Folio { get; set; }
        public string TipoDocumento { get; set; }
        public string Asunto { get; set; }
        public string NoInterno { get; set; }
        public string Fecha { get; set; }
        public string Cuerpo { get; set; }
        public string TipoEnvio { get; set; }

        //Usuario De Origen
        public ResumenInfoRemitenteOrigenViewModel Origen_UsuarioDe { get; set; }
        //Usuario Para Origen
        public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioPara { get; set; }
        //Usuario CCP Origen
        public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioCcp { get; set; }
        //Nombre de quien lo ha creado/ ya sea el titular o el delegado
        public string Origen_NombreCreador { get; set; }
        public string Origen_UsuarioCreador { get; set; }
        public string Origen_NombreTitular { get; set; }
        public string Origen_UsuarioTitular { get; set; }
        //Anexos Origen
        public List<string> Origen_Anexos { get; set; }
    }
    public class ResumenInfoRemitenteOrigenViewModel
    {
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string AreaPadre { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string Puesto { get; set; }
    }
    public class ResumenInfoRemitenteViewModel
    {
        public string NombreCompleto { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Area { get; set; }
        public string Puesto { get; set; }
    }
    public class ResumenInfoDestinatariosOrigenViewModel
    {
        public string NombreCompleto { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Area { get; set; }
        public string Puesto { get; set; }
        public bool EstaActivaNotificacion { get; set; }
    }
    public class ResumenInfoDestinatariosViewModel
    {
        public string NombreCompleto { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Area { get; set; }
        public string Puesto { get; set; }
        public bool EstaActivaNotificacion { get; set; }
    }
    public class InfoDocumentoQRViewModel
    {
        public string De { get; set; }
        public string Para { get; set; }
        public string Asunto { get; set; }
        public string Folio { get; set; }
        public string Leyenda { get; set; }
    }
    //--Reenviar
    public class ReenviarDocumentoLecturaViewModel
    {
        //Documento Origen
        public string Asunto { get; set; }
        public string Folio { get; set; }
        public string NoInterno { get; set; }
        public string Fecha { get; set; }
        public string Cuerpo { get; set; }
        public string TipoDocumento { get; set; }
        public List<string> ListadoCcp { get; set; }
 
        //Usuario Origen
        public string UsuarioCorreo { get; set; }
        public string UsuarioNombreCompleto { get; set; }
        public string UsuarioDireccion { get; set; }
        public string UsuarioTelefono { get; set; }
        public string AreaPadreNombre { get; set; }
        public string AreaNombre { get; set; }
        public int AreaId { get; set; }
        public string RegionNombre { get; set; }
        public string PuestoNombre { get; set; }

        //[Actual]
        //Envio
        public int EnvioId { get; set; }
        public int TipoEnvioId { get; set; }
        public string TipoEnvio { get; set; }
        public string AsuntoEnvio { get; set; }
        public string FechaEnvio { get; set; }
        public bool ExisteAdjuntos { get; set; }

        //[Para] (Para las respuestas)
        public string UsuarioPara_NombreCompleto { get; set; }
        public string UsuarioPara_PuestoNombre { get; set; }
        public string UsuarioPara_AreaNombre { get; set; }
        public int UsuarioPara_Tipo { get; set; }
    }
    public class ReenviarDocumentoViewModel
    {
        [HiddenInput]
        public bool ExisteAdjuntos { get; set; }

        [HiddenInput]
        public int EnvioId { get; set; }

        [HiddenInput]
        public int TipoEnvioId { get; set; }

        [HiddenInput]
        public string TipoEnvio { get; set; }

        public string Folio { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [HiddenInput]
        [Display(Name = "Para")]
        public string Para { get; set; }

        [HiddenInput]
        [Display(Name = "CC")]
        public string CCP { get; set; }

        [HiddenInput]
        public string Anexos { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public bool RequiereRespuesta { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Importancia")]
        public int ImportanciaId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Visibilidad")]
        public int VisibilidadId { get; set; }

        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Indicaciones { get; set; }

        [Condicionbolean("RequiereRespuesta", ErrorMessage = "El dato es requerido.")]
        [Display(Name = "Fecha propuesta")]
        public string FechaPropuesta { get; set; }

        [BindProperty]
        public string UsuarioDe { get; set; }

        [BindProperty]
        [HiddenInput]
        public string FolioSession { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Extensiones { get; set; }

        public SelectList ImportanciaSelectList { get; set; }
        public SelectList VisibilidadSelectList { get; set; }

        //Comprueba si es respuesta o un nuevo documento
        [HiddenInput]
        public bool EsRespuestaDefinitiva { get; set; }

    }
    public class ReenviarViewModel
    {
        public string Folio { get; set; }
        public int EnvioId { get; set; }
        public int TipoEnvioId { get; set; }
        public int Usuario_DeId { get; set; }
        public int Usuario_EnviaId { get; set; }
        public int Total_Para { get; set; }
        public int Total_CCP { get; set; }
        public string Indicaciones { get; set; }
        public bool RequiereRespuesta { get; set; }
        public int ImportanciaId { get; set; }
        public int VisibilidadId { get; set; }
        public int? AnexoId { get; set; }

        public string FechaPropuesta { get; set; }
    }
    public class RecepcionReenviarViewModel
    {
        public int EnvioId { get; set; }
        public int Usuario_ParaId { get; set; }
        public int Tipo_Para { get; set; }
        public bool RequiereRespuesta { get; set; }
    }
    public class EnvioCorreoLecturaViewModel
    {
        [BindProperty]
        [HiddenInput]
        public int EnvioId { get; set; }

        [BindProperty]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [Display(Name = "Correo")]
        //[EmailAddress]
        [HiddenInput]
        public string Correo { get; set; }

        [BindProperty]
        [HiddenInput]
        public int Tipo { get; set; }

        [BindProperty]
        [HiddenInput]
        public int TipoEnvio { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool Adjuntar { get; set; }
    }
}
