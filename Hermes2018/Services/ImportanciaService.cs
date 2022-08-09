using Hermes2018.Data;
using Hermes2018.Models.Documento;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class ImportanciaService: IImportanciaService
    {
        private readonly ApplicationDbContext _context;

        public ImportanciaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HER_Importancia>> ObtenerTiposImportanciaAsync()
        {
            var tiposImportanciaQuery = _context.HER_Importancia
                                     .OrderByDescending(x => x.HER_Nombre)
                                     .AsNoTracking()
                                     .AsQueryable();

            return await tiposImportanciaQuery.ToListAsync();
        }

        public async Task<string> ObtenerNombreImportanciaAsync(int importanciaId)
        {
            var nombreImportanciaQuery = _context.HER_Importancia
                                     .Where(x => x.HER_ImportanciaId == importanciaId)
                                     .Select(x => x.HER_Nombre)
                                     .AsNoTracking()
                                     .AsQueryable();

            return await nombreImportanciaQuery.FirstOrDefaultAsync();
        }
    }
}
