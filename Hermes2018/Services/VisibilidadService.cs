using Hermes2018.Data;
using Hermes2018.Models.Documento;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class VisibilidadService: IVisibilidadService
    {
        private ApplicationDbContext _context;

        public VisibilidadService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HER_Visibilidad>> ObtenerTiposVisibilidadAsync()
        {
            var visibilidadQuery = _context.HER_Visibilidad
                                .AsNoTracking()
                                .AsQueryable();

            return await visibilidadQuery.ToListAsync();
        }
    }
}
