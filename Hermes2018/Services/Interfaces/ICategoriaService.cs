using Hermes2018.Models.Categoria;
using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface ICategoriaService
    {
        List<HER_Categoria> ObtenerCategoriasUsuario(string userName);
        HER_Categoria ObtenerCategoria(int categoriaId, string userName);
        bool CategoriaEnUso(int categoriaId, string userName);
        bool ExisteCategoria(int categoriaId, string userName);
        bool ExisteCategoriaPorNombre(string nombreCategoria, string userName);
        bool GuardarCategoria(NuevaCategoriaViewModel nuevaCategoria);
        bool ActualizarCategoria(ActualizarCategoriaViewModel actualizarCategoria);
        bool EliminarCategoria(int categoriaId, string userName);

        //---
        Task<List<CategoriaViewModel>> ObtenerCategoriasAsync(string userName);
        Task<bool> ExisteCategoriaPorNombreAsync(string nombreCategoria, string userName);
        Task<HER_Categoria> GuardarCategoriaAsync(NuevaCategoriaViewModel nuevaCategoria);
        Task<List<CategoriaSeleccionadaViewModel>> ObtenerCategoriasSeleccionadasAsync(string folio, string userName);
        Task<List<CategoriaSeleccionadaViewModel>> ObtenerCategoriasSeleccionadasEnvioAsync(SolicitudCategoriaJsonModel solicitudCategoriaJson);
        Task<List<CategoriaSeleccionadaViewModel>> ObtenerCategoriasSeleccionadasRecepcionAsync(SolicitudCategoriaJsonModel solicitudCategoriaJson);
        Task<bool> ActualizarRecepcionCategoriaAsync(ActualizacionCategoriaDocumentoJsonModel actualizacionJsonModel);

        //[**CATEGORIAS**]
        Task<bool> GuardarCategoriasDocumentoAsync(List<string> listaCategorias, int documentoId);
        Task<bool> GuardarCategoriasDocumentoAsync(int documentoId, int documentoBaseId);
        Task<bool> ActualizarCategoriasDocumentoAsync(List<CategoriaDocumentoViewModel> listCategoriasView, string folio);
        //--
        Task<bool> GuardarCategoriasDocumentoBaseAsync(List<string> listaCategorias, int documentoBaseId);
        Task<bool> ActualizarCategoriasDocumentoBaseAsync(List<string> listaNuevasCategorias, int documentoBaseId);
        //--
        Task<bool> GuardarCategoriasDeDocumentoRespuestaAsync(List<string> listaCategorias, int documentoId);
    }
}
