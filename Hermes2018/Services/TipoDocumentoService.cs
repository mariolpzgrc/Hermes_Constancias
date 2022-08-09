using Hermes2018.Data;
using Hermes2018.Models.Documento;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class TipoDocumentoService : ITipoDocumentoService
    {
        private readonly ApplicationDbContext _context;

        public TipoDocumentoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HER_TipoDocumento>> ObtenerTiposDocumentoAsync()
        {
            var tiposQuery = _context.HER_TipoDocumento
                .AsNoTracking()
                .AsQueryable();

            return await tiposQuery.ToListAsync();
        }
        public async Task<List<HER_TipoDocumento>> ObtenerSoloTipoOficioAsync()
        {
            var tiposQuery = _context.HER_TipoDocumento
                .Where(x => x.HER_Nombre.ToUpper() == "OFICIO")
                .AsNoTracking()
                .AsQueryable();

            return await tiposQuery.ToListAsync();
        }
        public async Task<string> ObtenerNombreTipo(int tipoId)
        {
            var tiposQuery = _context.HER_TipoDocumento
                .Where(x => x.HER_TipoDocumentoId == tipoId)
                .Select(x => x.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await tiposQuery.FirstOrDefaultAsync();
        }
    }
}
