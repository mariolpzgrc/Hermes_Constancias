using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Calendario;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class TramiteService : ITramiteService
    {
        private readonly ApplicationDbContext _context;

        public TramiteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListadoTramitesViewModel>> ListadoTramitesPorNombreAsync()
        {
            IQueryable<ListadoTramitesViewModel> tramitesQuery = _context.HER_Tramite
                .Where(x => x.HER_Estado == ConstTramiteEstado.EstadoN1)
                .Select(x => new ListadoTramitesViewModel()
                {
                    TramiteId = x.HER_TramiteId,
                    Nombre = x.HER_Nombre,
                    Dias = x.HER_Dias
                })
                .OrderBy(x => x.Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await tramitesQuery.ToListAsync();
        }
        public async Task<List<ListadoTramitesViewModel>> ListadoTramitesPorNombreAsync(int recepcionId)
        {
            IQueryable<Models.Documento.HER_Recepcion> recepcionQuery = _context.HER_Recepcion
                .Where(x => x.HER_RecepcionId == recepcionId)
                .Include(x => x.HER_Para)
                    .ThenInclude(x => x.HER_Area)
                .AsNoTracking()
                .AsQueryable();

            var recepcion = await recepcionQuery
                .Select(x => new { x.HER_FechaRecepcion, x.HER_Para.HER_Area.HER_DiasCompromiso })
                .FirstOrDefaultAsync();

            IQueryable<ListadoTramitesViewModel> tramitesQuery = _context.HER_Tramite
                .Where(x => x.HER_Estado == ConstTramiteEstado.EstadoN1)
                .Select(x => new ListadoTramitesViewModel()
                {
                    Dias = (x.HER_TramiteId == ConstTramite.TipoN1) ? recepcion.HER_DiasCompromiso : x.HER_Dias,
                    TramiteId = x.HER_TramiteId,
                    Nombre = string.Format("{0} ({1}{2}{3})", x.HER_Nombre, (x.HER_TramiteId == ConstTramite.TipoN1) ? "Manual, ": string.Empty, (x.HER_TramiteId == ConstTramite.TipoN1) ? recepcion.HER_DiasCompromiso : x.HER_Dias, " días")
                })
                .OrderBy(x => x.Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await tramitesQuery.ToListAsync();
        }
    }
}
