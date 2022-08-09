using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Bandejas
{
    [Authorize]
    public class HistoricoModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;

        public HistoricoModel(IUsuarioClaimService usuarioClaimService)
        {
            _usuarioClaimService = usuarioClaimService;
        }

        public bool PuestoEsUnico { get; set; }

        public void OnGet()
        {
            PuestoEsUnico = _usuarioClaimService.ObtenerInfoUsuarioClaims(User).BandejaPuestoEsUnico;
            //--
            ViewData["Bandeja"] = "Historico";
        }
    }
}