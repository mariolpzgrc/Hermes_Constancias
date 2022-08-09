using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Documento;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class EstadoEnvioService: IEstadoEnvioService
    {
        private ApplicationDbContext _context;

        public EstadoEnvioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EstadoEnvioViewModel>> ObtenerEstadosBandejaRecibidosAsync()
        {
            var estadosQuery = _context.HER_EstadoEnvio
                                .Where(x => ConstEstadoEnvio.EstadoBandejaRecibidosCompleto.Contains(x.HER_Nombre))
                                .Select(x => new EstadoEnvioViewModel()
                                {
                                    HER_EstadoEnvioId = x.HER_EstadoEnvioId,
                                    HER_Nombre = x.HER_Nombre
                                })
                                .AsNoTracking()
                                .AsQueryable();

            return await estadosQuery.ToListAsync();
        }
        public async Task<List<EstadoEnvioViewModel>> ObtenerEstadosBandejaEnviadosAsync()
        {
            var estadosQuery = _context.HER_EstadoEnvio
                                .Where(x => ConstEstadoEnvio.EstadoBandejaEnviadosCompleto.Contains(x.HER_Nombre))
                                .Select(x => new EstadoEnvioViewModel()
                                {
                                    HER_EstadoEnvioId = x.HER_EstadoEnvioId,
                                    HER_Nombre = x.HER_Nombre
                                })
                                .AsNoTracking()
                                .AsQueryable();

            return await estadosQuery.ToListAsync();
        }
    }
}
