using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IGrupoService
    {
        Task<List<GruposViewModel>> ObtenerGruposAsync(string userName);
        List<GruposViewModel> ObtenerGrupos(string userName);
        Task<List<UsuarioLocalJsonModel>> ObtenerIntegrantesGrupoAsync(int grupoId);
        Task<bool> ExisteGrupoAsync(string nombreGrupo, int usuarioCreadorId);
        Task<bool> GuardarGrupoAsync(string nombreGrupo, int usuarioCreadorId, List<IntegranteViewModel> integrantes);
        Task<DetalleGrupoViewModel> ObtenerDetalleGrupoAsync(int grupoId);
        Task<EditarGrupoViewModel> ObtenerGrupoParaEdicion(int grupoId);
        Task<bool> ActualizarGrupoAsync(EditarGrupoViewModel model);
        Task<bool> GrupoTieneIntegrantesAsociados(int infoUsuarioId, int grupoId);
        Task<bool> GrupoTieneIntegrantesAsociadosInactivos(int infoUsuarioId, int grupoId);
        Task<BorrarGrupoViewModel> ObtenerGrupoParaBorrar(string username, int grupoId);
        Task<bool> BorrarGrupoAsync(string username, BorrarGrupoViewModel model);
        Task<bool> ExisteIntegranteAsync(string integrante, int grupoId);
        Task<AgregarIntegranteGrupoViewModel> ObtenerGrupoIntegranteParaAgregar(int grupoId);
        Task<bool> AgregarIntegranteGrupoAsync(string username, int grupoId);
        Task<BorrarIntegranteGrupoViewModel> ObtenerIntegranteParaBorrar(string username, int grupoId);
        Task<List<BorrarIntegranteGrupoViewModel>> ObtenerIntegrantesInactivoParaBorrar(int grupoId);
        Task<BorrarIntegranteGrupoViewModel> ObtenerIntegranteInactivoParaBorrar(string username, int grupoId);
        Task<bool> BorrarIntegranteAsync(string username, BorrarIntegranteGrupoViewModel model);
        Task<bool> BorrarIntegranteInactivoAsync(string username, BorrarIntegranteGrupoViewModel model);
    }
}
