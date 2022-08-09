using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IPlantillaService
    {
        Task<bool> ExistePlantillaAsync(string nombre, string userName);
        Task<bool> ExistePlantillaPorIdAsync(int plantillaId, string userName);
        Task<bool> GuardarPlantillaAsync(NuevaPlantillaViewModel nuevaPlantilla);
        Task<PlantillaViewModel> ObtenerPlantillaAsync(string nombre, string userName);
        Task<PlantillaViewModel> ObtenerPlantillaPorIdAsync(int plantillaId, string userName);
        Task<List<PlantillaSimplificadaViewModel>> ObtenerPlantillasAsync(string userName);
        Task<PlantillaDetalleViewModel> ObtenerDetallePlantillaAsync(int plantillaId, string userName);
        //--
        Task<EditarPlantillaViewModel> ObtenerInfoEditarPlantillaAsync(int plantillaId, string userName);
        Task<bool> ActualizarPlantillaAsync(EditarPlantillaViewModel modelo, string username);
        Task<bool> GuardarPlantillaDesdeFormularioAsync(CrearPlantillaViewModel nuevaPlantilla, int infoUsuarioId);

        Task<bool> EliminarPlantillaAsync(int plantillaId, string username);
    }
}
