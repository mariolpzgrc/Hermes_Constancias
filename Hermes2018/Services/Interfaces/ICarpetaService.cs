using Hermes2018.Models.Carpeta;
using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface ICarpetaService
    {
        //Carpetas
        Task<List<CarpetaViewModel>> ObtenerCarpetasAsync(int infoUsuarioId);
        Task<bool> ExisteCarpetaAsync(string nombreCarpeta, int infoUsuarioId);
        Task<bool> ExisteCarpetaPorIdAsync(int carpetaId, int infoUsuarioId);
        Task<bool> CarpetaTieneSubcarpetasPorIdAsync(int carpetaId, int infoUsuarioId);
        Task<bool> CarpetaTieneDocumentosAsociados(int carpetaId, int infoUsuarioId);
        Task<bool> GuardarCarpetaAsync(CrearCarpetaViewModel modelo, int infoUsuarioId);
        Task<HER_Carpeta> ObtenerCarpetaAsync(int infoUsuarioId, int carpetaId);
        Task<bool> ActualizarCarpetaAsync(EditarCarpetaViewModel modelo, int infoUsuarioId);
        Task<bool> BorrarCarpetaAsync(BorrarCarpetaViewModel modelo, int infoUsuarioId);

        //Subcarpetas
        Task<List<SubcarpetaViewModel>> ObtenerSubcarpetasAsync(int infoUsuarioId, int carpetaId);
        Task<bool> ExisteSubcarpetaAsync(string nombreSubcarpeta, int infoUsuarioId, int carpetaId);
        Task<bool> ExisteSubcarpetaPorIdAsync(int subcarpetaId, int infoUsuarioId, int carpetaId);
        Task<bool> SubcarpetaTieneDocumentosAsociados(int subcarpetaId, int infoUsuarioId, int carpetaId);
        Task<bool> GuardarSubcarpetasAsync(CrearSubcarpetaViewModel modelo, int infoUsuarioId, int carpetaId);
        Task<HER_Carpeta> ObtenerSubcarpetaAsync(int infoUsuarioId, int subcarpetaId);
        Task<bool> ActualizarSubcarpetaAsync(EditarSubcarpetaViewModel modelo, int infoUsuarioId);
        Task<bool> BorrarSubcarpetaAsync(BorrarSubcarpetaViewModel modelo, int infoUsuarioId);
        Task<List<CarpetasJsonMdel>> ListadoCarpetasPorUsuarioAsync(string userName);
        //--
        Task<bool> MoverDocumentosRecibidosAsync(MoverDocumentoJsonModel solicitud);
        Task<bool> MoverDocumentosEnviadosAsync(MoverDocumentoJsonModel solicitud);
        //--
        Task<DetallesCarpetaViewModel> ObtenerDetallesCarpetaAsync(int carpetaId, string username);
    }
}
