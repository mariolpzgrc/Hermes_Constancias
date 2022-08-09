using Hermes2018.Models.Documento;
using Hermes2018.ViewComponentsModels;
using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IDocumentoService
    {
        //[**FOLIO**]
        Task<string> GenerarFolioAsync();

        //[**BORRADOR**]
        //Guardar
        int GuardarDocumento(NuevoDocumentoViewModel nuevoDocumento);

        int GuardarDocumentoBase(NuevoDocumentoBaseViewModel nuevoOficio);
        Task<int> ActualizarDocumentoBaseAsync(ActualizarDocumentoBaseViewModel oficio);
        //Obtener documento para editar
        Task<HER_DocumentoBase> ObtenerInfoBaseDocumentoAsync(int id, string username);
        //Listado de documentos en borrador
        IQueryable<DocumentoBorradorViewModel> ListadoBorradores(string username, string busqueda, int? categoria, string fechaini, string fechafin);
        Task<bool> EliminarDocumentoBaseAsync(string folio, int documentoBaseId);
        Task<BorrarDocumentoBaseViewModel> ObtenerInfoBaseParaBorrarDocumentoAsync(int id, string username);
        Task<bool> ExisteDocumentoBaseAsync(string folio, int documentobaseId);
        Task<bool> BorrarDocumentoBaseAsync(string folio, int documentobaseId);

        //[**REVISIÓN**]
        Task<bool> ActualizarEstadoDocumentoAsync(EstadoDocumentoViewModel estadoViewModel);
        IQueryable<DocumentoRevisionViewModel> ListadoRevisionRemitente(string username, string busqueda, int? categoria, string fechaini, string fechafin);
        Task<DocumentoEnRevisionViewModel> ObtenerEnvioRevisionDocumentoAsync(int revisionId, string usuarioActual);
        Task<bool> ValidaRevisionAsync(int revisionId, string usuarioActual);
        Task<bool> CrearEnvioRevisionAsync(EnviarRevisionViewModel revision);
        Task<int> ActualizarRevisionRemitenteAsync(ActualizarRevisionRemitenteViewModel oficio);
        Task<int> ActualizarRevisionDestinatarioAsync(ActualizarRevisionDestinatarioViewModel oficio);
        Task<int> ActualizarEstadoRevisionAsync(ActualizarEstadoRevisionViewModel revision);
        Task<bool> EliminarEnvioRevisionAsync(string folio);

        //[**ENVIO**]
        Task<int> CrearEnvioAsync(EnvioViewModel envioViewModel);
        Task<bool> CrearRecepcionAsync(List<RecepcionViewModel> listadoRecepcionViewModel, string fechaPropuesta);
        //[Recibidos]
        Task<bool> ActualizaEstadoDocumentoRecibidoAsync(string username);
        IQueryable<DocumentoRecibidoViewModel> ListadoDocumentosRecibidos(string username, string busqueda, int? categoria, string fechaini, string fechafin, int? estado, int? tipo, int? proximovencer, int? tramite);
        IQueryable<DocumentoRecibidoViewModel> ListadoDocumentosRecibidosEnCarpetas(string username, string busqueda, int? categoria, string fechaini, string fechafin, int? estado, int? tipo, int? proximovencer, int? tramite);
        //Bandeja Enviados
        IQueryable<DocumentoEnviadoViewModel> ListadoDocumentosEnviados(string username, string busqueda, int? categoria, string fechaini, string fechafin, int? estado, int? tipo, int? tramite);
        //
        IQueryable<DocumentoEnviadoViewModel> ListadoDocumentosEnviadosEnCarpetas(string username, string busqueda, int? categoria, string fechaini, string fechafin, int? estado, int? tipo, int? tramite);
        //Visualizacion oficio 
        Task<DocumentoEnviadoVisualizacionViewModel> ObtenerDocumentoEnviadoSoloVisualizacionAsync(int envioId, string usuario);
        Task<DocumentoEnviadoVisualizacionViewModel> ObtenerDocumentoTurnadoSoloVisualizacionAsync(int envioId, string usuario);
        Task<DocumentoEnviadoVisualizacionViewModel> ObtenerDocumentoRespuestaEnviadoSoloVisualizacionAsync(int envioId, string usuario);

        //Lectura Oficio
        Task<DocumentoEnviadoLecturaViewModel> ObtenerDocumentoEnviadoSoloLecturaAsync(int envioId, string usuario);
        Task<DocumentoEnviadoLecturaViewModel> ObtenerDocumentoTurnadoSoloLecturaAsync(int envioId, string usuario);
        Task<DocumentoEnviadoLecturaViewModel> ObtenerDocumentoRespuestaEnviadoSoloLecturaAsync(int envioId, string usuario);
        
        Task<List<ResumenDestinatarioViewModel>> ObtenerResumenDestinatariosAsync(int envioId);
        Task<bool> CambiarRecepcionComoLeidoAsync(int envioId, string usuario);
        Task<bool> CambiarRecepcionRespuestaComoLeidoAsync(int envioId, string usuario);
        Task<TurnarDocumentoLecturaViewModel> ObtenerDocumentoTurnarParaLecturaAsync(int envioId, string usuario);
        Task<int> CrearTurnarAsync(TurnarViewModel turnarViewModel);
        Task<bool> CrearRecepcionTurnarAsync(List<RecepcionTurnarViewModel> listadoTurnarViewModel, string fechaPropuesta);
        //[Respuesta]
        Task<DocumentoRespuestaLecturaViewModel> ObtenerDocumentoRespuestaParaLecturaAsync(int envioId, string usuario);
        Task<int> GuardarDocumentoRespuestaAsync(NuevoDocumentoRespuestaViewModel nuevoOficioRespuesta);
        Task<int> CrearEnvioRespuestaAsync(EnvioRespuestaViewModel envioRespuestaViewModel);
        Task<bool> CrearRecepcionRespuestaAsync(List<RecepcionRespuestaViewModel> listadoRecepcionRespuestaViewModel);
        
        Task<bool> ActualizarEstadoEnvioAsync(int envioId, int usuarioId);
        Task<bool> ActualizarEstadoRecepcionAsync(int envioId, int usuarioId);

        //[Seguimiento]
        Task<EncabezadoSeguimientoViewModel> ObtenerEncabezadoSeguimientoAsync(int envioId, int tipo, string usuario);
        IQueryable<HER_Envio> ObtenerSeguimientoEnvio(string folio);
        //[Bandejas]
        Task<EstadoBandejasViewComponentModel> ObtenerEstadoBandejasPrincipalesAsync(string userName);

        //[Carpetas]
        IQueryable<DocumentoEnCarpetaPersonalViewModel> ListadoDocumentosRecibidosEnCarpetasPersonales(string username, int carpetaId, string busqueda, int? categoria, string fechaini, string fechafin);
        IQueryable<DocumentoEnCarpetaPersonalViewModel> ListadoDocumentosEnviadosEnCarpetasPersonales(string username, int carpetaId, string busqueda, int? categoria, string fechaini, string fechafin);
        
        //Notificación por correo
        Task<ResumenEnvioDocumentoCorreoViewModel> ObtenerParaCorreoDocumentoEnviadoAsync(int envioId);
        Task<ResumenTurnarDocumentoCorreoViewModel> ObtenerParaCorreoDocumentoTurnadoAsync(int envioId);
        Task<ResumenTurnarEspecialDocumentoCorreoViewModel> ObtenerParaCorreoDocumentoTurnadoEspecialAsync(int envioId);
        Task<ResumenResponderDocumentoCorreoViewModel> ObtenerParaCorreoDocumentoRespondidoAsync(int envioId);
        //--
        string ConstruirQR(InfoDocumentoQRViewModel model);

        //Reenviar
        Task<ReenviarDocumentoLecturaViewModel> ObtenerDocumentoReenviarParaLecturaAsync(int envioId, string usuario);
        Task<int> CrearReenvioAsync(ReenviarViewModel reenvioViewModel);
        Task<bool> CrearRecepcionReenvioAsync(List<RecepcionReenviarViewModel> listadoReenvioViewModel, string fechaPropuesta, int tipoEnvioId);
        Task<bool> ActualizarFechaCompromisoPrincipalEnvioAsync(NuevoCompromisoJsonModel model);
        Task<bool> AceptarFechaCompromisoPrincipalEnvioAsync(CompromisoAceptadoJsonModel model);
        Task<bool> ActualizarFechaCompromisoAsync(NuevoCompromisoJsonModel model);
        Task<bool> ActualizarFechaPropuestaAsync(CompromisoAceptadoJsonModel model);

        //Historial envío de correo Lectura
        Task<bool> GuardarHistorialCorreo(GuardarHistorialCorreo historialCorreo);

        Task<List<ListadoHistorialCorreo>> ObtenerListadoHistorialAsync(int envioId, int tipoEnvio, int tipo);
        Task<NumerologiaEnvioViewModel> ObtenerNumerologiaEnvioAsync(int documentoId, int grupoEnvio);
        Task<bool> ExisteFechaModificadaAsync(int recepcionId);
    }
}
