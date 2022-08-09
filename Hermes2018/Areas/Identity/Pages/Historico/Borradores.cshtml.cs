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
    public class BorradoresModel : PageModel
    {
        private readonly IHistoricoService _historicoService;

        public BorradoresModel(IHistoricoService historicoService)
        {
            _historicoService = historicoService;
        }

        [HiddenInput]
        public int InfoUsuarioId { get; set; }
        public int TipoHistorico { get; set; }
        public int Bandeja { get; set; }
        public int Pagina { get; set; }

        public void OnGet(int id, int tipo, int bandeja, int? pagina)
        {
            InfoUsuarioId = id;
            TipoHistorico = tipo;
            Bandeja = bandeja;

            //--
            ViewData["Bandeja"] = "Historico";
        }
    }
}