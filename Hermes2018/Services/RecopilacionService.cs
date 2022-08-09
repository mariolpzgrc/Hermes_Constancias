using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Recopilacion;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class RecopilacionService : IRecopilacionService
    {
        private readonly ApplicationDbContext _context;

        public RecopilacionService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Estadisticas de administradores
        public async Task<bool> CargarRecopilacionAsync()
        {
            int result = 0;
            
            var documentosEnviadosQuery = _context.HER_Envio
                .AsNoTracking()
                .AsQueryable();

            var documentosRecibidosQuery = _context.HER_Recepcion
                .AsNoTracking()
                .AsQueryable();

            var recopilacionActualQuery = _context.HER_Recopilacion
                     .AsNoTracking()
                     .AsQueryable();

            var recopilacionActual = await recopilacionActualQuery.ToListAsync();

            if (recopilacionActual.Count() > 0)
            {
                _context.HER_Recopilacion.RemoveRange(recopilacionActual);
                result += await _context.SaveChangesAsync();

                //Reiniciar el id de la tabla
                _context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('HER_Recopilacion', RESEED, 0)");
            }

            var recopilacionQuery = _context.HER_Area
                   .Where(x => x.HER_Estado == ConstEstadoArea.EstadoN1)
                   .Select(x => new HER_Recopilacion()
                   {
                       HER_AreaId = x.HER_AreaId,
                       Usuarios = x.HER_Usuarios.Where(a => a.HER_Activo == true).Count(),
                       DocumentosEnviados = documentosEnviadosQuery.Where(a => a.HER_De.HER_AreaId == x.HER_AreaId).Count(),
                       DocumentosRecibidos = documentosRecibidosQuery.Where(a => a.HER_Para.HER_AreaId == x.HER_AreaId).Count()
                   })
                   .OrderByDescending(x => x.HER_AreaId)
                   .AsNoTracking()
                   .AsQueryable();

            var recopilacion = await recopilacionQuery.ToListAsync();

            _context.HER_Recopilacion.AddRange(recopilacion);
            result += await _context.SaveChangesAsync();

            return result > 0;
        }

        //--Generales
        public async Task<RecopilacionGeneralViewModel> ObtenerRecopilacionGeneralAsync()
        {
            var recopilacionQuery = _context.HER_Recopilacion
                .OrderByDescending(x => x.HER_AreaId)
                .AsNoTracking()
                .AsQueryable();

            RecopilacionGeneralViewModel modelo = new RecopilacionGeneralViewModel() {
                Areas = await recopilacionQuery.CountAsync(),
                Usuarios = await recopilacionQuery.SumAsync(x => x.Usuarios),
                DocumentosEnviados = recopilacionQuery.Sum(x => x.DocumentosEnviados),
                DocumentosRecibidos = recopilacionQuery.Sum(x=> x.DocumentosRecibidos)
            };

            return modelo;
        }
        public async Task<RecopilacionRegionViewModel> ObtenerRecopilacionPorRegionAsync(int regionId)
        {
            var regionQuery = _context.HER_Region
                .Where(x => x.HER_RegionId == regionId)
                .AsNoTracking()
                .AsQueryable();

            var region = await regionQuery.FirstOrDefaultAsync();

            var recopilacionQuery = _context.HER_Recopilacion
                .Include(x => x.HER_Area)
                    .ThenInclude(x => x.HER_Region)
                .Where(x => x.HER_Area.HER_RegionId == region.HER_RegionId)
                .OrderByDescending(x => x.HER_AreaId)
                .AsNoTracking()
                .AsQueryable();

            var recopilacion = await recopilacionQuery.ToListAsync();

            RecopilacionRegionViewModel modelo = new RecopilacionRegionViewModel()
            {
                Region = region.HER_Nombre,
                RegionId = region.HER_RegionId,
                Areas = recopilacion.Count(),
                Usuarios = recopilacion.Sum(x => x.Usuarios),
                DocumentosEnviados = recopilacion.Sum(x => x.DocumentosEnviados),
                DocumentosRecibidos = recopilacion.Sum(x => x.DocumentosRecibidos)
            };

            return modelo;
        }
        public async Task<RecopilacionAreaViewModel> ObtenerRecopilacionPorAreaAsync(int areaId)
        {
            var recopilacionQuery = _context.HER_Recopilacion
                .Include(x => x.HER_Area)
                    .ThenInclude(x => x.HER_Region)
                .Where(x => x.HER_AreaId == areaId)
                .OrderByDescending(x => x.HER_AreaId)
                .AsNoTracking()
                .AsQueryable();

            var recopilacion = await recopilacionQuery.FirstOrDefaultAsync();

            RecopilacionAreaViewModel modelo = new RecopilacionAreaViewModel()
            {
                AreaId = recopilacion.HER_AreaId,
                Area = recopilacion.HER_Area.HER_Nombre,
                Usuarios = recopilacion.Usuarios,
                DocumentosEnviados = recopilacion.DocumentosEnviados,
                DocumentosRecibidos = recopilacion.DocumentosRecibidos
            };

            return modelo;
        }
        
        //--Especificas
        public async Task<List<RecopilacionRegionViewModel>> ObtenerRecopilacionRegionesAsync()
        {
            var recopilacionQuery = _context.HER_Recopilacion
                .Include(x => x.HER_Area)
                    .ThenInclude(x => x.HER_Region)
                .GroupBy(x => x.HER_Area.HER_Region)
                .Select(x => new RecopilacionRegionViewModel() {
                    Region = x.Key.HER_Nombre,
                    RegionId = x.Key.HER_RegionId,
                    Areas = x.Count(),
                    DocumentosEnviados = x.Sum(a => a.DocumentosEnviados),
                    DocumentosRecibidos = x.Sum(b => b.DocumentosRecibidos),
                    Usuarios = x.Sum(c => c.Usuarios)
                })
                .AsNoTracking()
                .AsQueryable();

            var recopilacion = await recopilacionQuery.ToListAsync();

            return recopilacion;
        }
        public async Task<List<RecopilacionAreaViewModel>> ObtenerRecopilacionAreasAsync(int regionId)
        {
            var recopilacionQuery = _context.HER_Recopilacion
                .Include(x => x.HER_Area)
                    .ThenInclude(x => x.HER_Region)
                .Where(x => x.HER_Area.HER_RegionId == regionId)
                .GroupBy(x => x.HER_Area)
                .Select(x => new RecopilacionAreaViewModel()
                {
                    Area = x.Key.HER_Nombre,
                    AreaId = x.Key.HER_AreaId,
                    DocumentosEnviados = x.Sum(a => a.DocumentosEnviados),
                    DocumentosRecibidos = x.Sum(b => b.DocumentosRecibidos),
                    Usuarios = x.Sum(c => c.Usuarios)
                })
                .AsNoTracking()
                .AsQueryable();

            var recopilacion = await recopilacionQuery.ToListAsync();

            return recopilacion;
        }
        public async Task<List<RecopilacionAreaViewModel>> ObtenerRecopilacionAreasAsync(int regionId, int? areaPadreId)
        {
            List<RecopilacionAreaViewModel> listado = new List<RecopilacionAreaViewModel>();
            IQueryable<RecopilacionAreaViewModel> recopilacionQuery;

            if (areaPadreId != null)
            {
                recopilacionQuery = _context.HER_Recopilacion
                    .Include(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Region)
                    .Where(x => x.HER_Area.HER_RegionId == regionId
                             && x.HER_Area.HER_Area_PadreId == areaPadreId)
                    .GroupBy(x => x.HER_Area)
                    .Select(x => new RecopilacionAreaViewModel()
                    {
                        Area = x.Key.HER_Nombre,
                        AreaId = x.Key.HER_AreaId,
                        DocumentosEnviados = x.Sum(a => a.DocumentosEnviados),
                        DocumentosRecibidos = x.Sum(b => b.DocumentosRecibidos),
                        Usuarios = x.Sum(c => c.Usuarios)
                    })
                    .AsNoTracking()
                    .AsQueryable();

                listado = await recopilacionQuery.ToListAsync();
            }
            else {
                recopilacionQuery = _context.HER_Recopilacion
                    .Include(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Region)
                    .Where(x => x.HER_Area.HER_RegionId == regionId
                            && ConstRegion.RegionesIds.Contains(x.HER_Area.HER_RegionId))
                    .GroupBy(x => x.HER_Area)
                    .Select(x => new RecopilacionAreaViewModel()
                    {
                        Area = x.Key.HER_Nombre,
                        AreaId = x.Key.HER_AreaId,
                        DocumentosEnviados = x.Sum(a => a.DocumentosEnviados),
                        DocumentosRecibidos = x.Sum(b => b.DocumentosRecibidos),
                        Usuarios = x.Sum(c => c.Usuarios)
                    })
                    .AsNoTracking()
                    .AsQueryable();

                listado.Add(await recopilacionQuery.FirstOrDefaultAsync());
            }
            
            return listado;
        }
    }
}
