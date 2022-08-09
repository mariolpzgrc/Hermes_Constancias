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
    public class RecibidosModel : PageModel
    {
        private readonly IHistoricoService _historicoService;

        public RecibidosModel(IHistoricoService historicoService)
        {
            _historicoService = historicoService;
        }

        [BindProperty]
        [HiddenInput]
        public int InfoUsuarioId { get; set; }

        [BindProperty]
        [HiddenInput]
        public int TipoHistorico { get; set; }

        [BindProperty]
        [HiddenInput]
        public int Bandeja { get; set; }

        public void OnGet(int infousuarioid, int tipohistorico, int bandeja)
        {
            InfoUsuarioId = infousuarioid;
            TipoHistorico = tipohistorico;
            Bandeja = bandeja;

            //--
            ViewData["Bandeja"] = "Historico";
        }
    }
}