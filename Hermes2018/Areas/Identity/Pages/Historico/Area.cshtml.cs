using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Historico
{
    [Authorize]
    public class AreaModel : PageModel
    {
        private readonly IHistoricoService _historicoService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        public AreaModel(IHistoricoService historicoService,
                            IUsuarioClaimService usuarioClaimService)
        {
            _historicoService = historicoService;
            _usuarioClaimService = usuarioClaimService;
        }

        public List<HistoricoAreaViewModel> ListadoHistorico { get; set; }

        public async Task OnGetAsync()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            ListadoHistorico = await _historicoService.ObtenerHistoricoAreaAsync(infoUsuarioClaims.BandejaAreaId);
            //--
            ViewData["Bandeja"] = "Historico";
        }
    }
}