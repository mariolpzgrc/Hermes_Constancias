using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Historico
{
    public class BandejasModel : PageModel
    {
        private readonly IHistoricoService _historicoService;

        public BandejasModel(IHistoricoService historicoService)
        {
            _historicoService = historicoService;
        }

        public int InfoUsuarioId { get; set; }
        public int TipoHistorico { get; set; }

        public BandejasViewModel Bandejas { get; set; }

        public async Task OnGetAsync(int infoUsuarioId, int tipoHistorico)
        {
            InfoUsuarioId = infoUsuarioId;
            TipoHistorico = tipoHistorico;
            //--
            Bandejas = await _historicoService.ObtenerBandejasAsync(infoUsuarioId);
            //--
            ViewData["Bandeja"] = "Historico";
        }
    }
}