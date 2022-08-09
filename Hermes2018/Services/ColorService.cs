using Hermes2018.Data;
using Hermes2018.Models.Configuracion;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class ColorService: IColorService
    {
        private readonly ApplicationDbContext _context;

        public ColorService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<HER_Color>> ObtenerColoresAsync()
        {
            var coloresQuery = _context.HER_Color
                .AsNoTracking()
                .AsQueryable();

            return await coloresQuery.ToListAsync();
        }
    }
}
