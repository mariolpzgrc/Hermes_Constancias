using Hermes2018.Models.Area;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IRegionService
    {

        Task<List<HER_Region>> ObtenerRegionEnListaAsync(int regionId);
        Task<HER_Region> ObtenerRegionConAreasAsync(int regionId);
        Task<HER_Region> ObtenerRegionSinAreasAsync(int regionId);
        Task<HER_Region> ObtenerRegionSinAreasPorNombreAsync(string nombreRegion);

        Task<List<HER_Region>> ObtenerRegionesAsync();
        Task<List<RegionEnAreaViewModel>> ObtenerRegionesEnAreasAsync();
        Task<bool> ExisteRegionAsync(int regionId);

        Task<string> ObtenerNombreRegionAsync(int regionId);
        Task<string> ObtenerNombreRegionPorAreaIdAsync(int areaId);
    }
}
