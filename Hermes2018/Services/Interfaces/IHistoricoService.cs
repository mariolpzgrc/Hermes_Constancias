using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IHistoricoService
    {
        Task<List<HistoricoPersonaViewModel>> ObtenerHistoricoPersonaAsync(string userName);
        Task<List<HistoricoAreaViewModel>> ObtenerHistoricoAreaAsync(int areaId);
        Task<BandejasViewModel> ObtenerBandejasAsync(int infoUsuarioId);

        IQueryable<DocumentoRecibidoViewModel> ObtenerCorrespondenciaRecibidos(int infoUsuarioId);
        IQueryable<DocumentoEnviadoViewModel> ObtenerCorrespondenciaEnviados(int infoUsuarioId);
        IQueryable<DocumentoBorradorViewModel> ObtenerCorrespondenciaBorradores(int infoUsuarioId);
        IQueryable<DocumentoRevisionViewModel> ObtenerCorrespondenciaRevision(int infoUsuarioId);

        Task<HistoricoDocumentoLecturaViewModel> ObtenerDocumentoEnviadoSoloLecturaAsync(int envioId, string usuario);
        Task<HistoricoDocumentoLecturaViewModel> ObtenerDocumentoTurnadoSoloLecturaAsync(int envioId, string usuario);
        Task<HistoricoDocumentoLecturaViewModel> ObtenerDocumentoRespuestaEnviadoSoloLecturaAsync(int envioId, string usuario);

        Task<HistoricoDocumentoVisualizacionViewModel> ObtenerDocumentoEnviadoSoloVisualizacionAsync(int envioId, string usuario);
        Task<HistoricoDocumentoVisualizacionViewModel> ObtenerDocumentoTurnadoSoloVisualizacionAsync(int envioId, string usuario);
        Task<HistoricoDocumentoVisualizacionViewModel> ObtenerDocumentoRespuestaEnviadoSoloVisualizacionAsync(int envioId, string usuario);

        Task<List<HistoricoResumenDestinatarioViewModel>> ObtenerResumenDestinatariosAsync(int envioId);
    }
}
