using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IRecopilacionService
    {
        Task<bool> CargarRecopilacionAsync();
        //--
        Task<RecopilacionGeneralViewModel> ObtenerRecopilacionGeneralAsync();
        Task<RecopilacionRegionViewModel> ObtenerRecopilacionPorRegionAsync(int regionId);
        Task<RecopilacionAreaViewModel> ObtenerRecopilacionPorAreaAsync(int areaId);
        //--
        Task<List<RecopilacionRegionViewModel>> ObtenerRecopilacionRegionesAsync();
        Task<List<RecopilacionAreaViewModel>> ObtenerRecopilacionAreasAsync(int regionId);
        Task<List<RecopilacionAreaViewModel>> ObtenerRecopilacionAreasAsync(int regionId, int? areaPadreId);
    }
}
