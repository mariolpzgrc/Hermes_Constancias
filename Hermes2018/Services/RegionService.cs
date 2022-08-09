using Hermes2018.Data;
using Hermes2018.Models.Area;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class RegionService: IRegionService
    {
        private readonly ApplicationDbContext _context;

        public RegionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HER_Region>> ObtenerRegionEnListaAsync(int regionId)
        {
            var regionQuery = _context.HER_Region
                .Where(x => x.HER_RegionId == regionId)
                .AsNoTracking()
                .AsQueryable();

            return await regionQuery.ToListAsync();
        }
        public async Task<HER_Region> ObtenerRegionConAreasAsync(int regionId)
        {
            var regionQuery = _context.HER_Region
                .Include(x => x.HER_Areas)
                .Where(x => x.HER_RegionId == regionId)
                .AsNoTracking()
                .AsQueryable();

            return await regionQuery.FirstOrDefaultAsync();
        }
        public async Task<HER_Region> ObtenerRegionSinAreasAsync(int regionId)
        {
            var regionQuery = _context.HER_Region
                .Where(x => x.HER_RegionId == regionId)
                .AsNoTracking()
                .AsQueryable();

            return await regionQuery.FirstOrDefaultAsync();
        }
        public async Task<HER_Region> ObtenerRegionSinAreasPorNombreAsync(string nombreRegion)
        {
            var regionQuery = _context.HER_Region
                .Where(x => x.HER_Nombre == nombreRegion)
                .AsNoTracking()
                .AsQueryable();

            return await regionQuery.FirstOrDefaultAsync();
        }
        public async Task<List<HER_Region>> ObtenerRegionesAsync()
        {
            var regionesQuery = _context.HER_Region
                .OrderBy(x => x.HER_RegionId)
                .AsNoTracking()
                .AsQueryable();
            
            return await regionesQuery.ToListAsync();
        }
        public async Task<List<RegionEnAreaViewModel>> ObtenerRegionesEnAreasAsync()
        {
            var regionesQuery = _context.HER_Region
                .Select(x => new RegionEnAreaViewModel()
                { 
                    RegionId = x.HER_RegionId,
                    Nombre = x.HER_Nombre
                })
                .OrderBy(x => x.RegionId)
                .AsNoTracking()
                .AsQueryable();

            return await regionesQuery.ToListAsync();
        }
        public async Task<bool> ExisteRegionAsync(int regionId)
        {
            var existeQuery = _context.HER_Region
                .Where(x => x.HER_RegionId == regionId)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<string> ObtenerNombreRegionAsync(int regionId)
        {
            var regionQuery = _context.HER_Region
                .Where(x => x.HER_RegionId == regionId)
                .Select(x => x.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await regionQuery.FirstOrDefaultAsync();
        }
        public async Task<string> ObtenerNombreRegionPorAreaIdAsync(int areaId)
        {
            var regionQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId)
                .Select(x => x.HER_Region.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await regionQuery.FirstOrDefaultAsync();
        }
    }
}
