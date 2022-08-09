using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Services
{
    public class TipoEnvioService : ITipoEnvioService
    {
        private ApplicationDbContext _context;

        public TipoEnvioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoEnvioViewModel>> ObtenerTiposAsync()
        {
            var tiposQuery = _context.HER_TipoEnvio
                    .Select(x => new TipoEnvioViewModel()
                    {
                        TipoEnvioId = x.HER_TipoEnvioId,
                        Nombre = x.HER_Nombre
                    })
                    .AsNoTracking()
                    .AsQueryable();

            var tipos = await tiposQuery.ToListAsync();

            tipos.Add( new TipoEnvioViewModel() {
                    TipoEnvioId = ConstTipoEnvio.TipoEnvioN5,
                    Nombre = ConstTipoEnvio.TipoEnvioT5
            });

            return tipos.OrderBy(x => x.Nombre).ToList();
        }
    }
}
