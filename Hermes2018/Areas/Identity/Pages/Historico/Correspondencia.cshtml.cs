using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Historico
{
    public class CorrespondenciaModel : PageModel
    {
        private readonly IHistoricoService _historicoService;

        public CorrespondenciaModel(IHistoricoService historicoService)
        {
            _historicoService = historicoService;
        }

        public int InfoUsuarioId { get; set; }
        public int TipoHistorico { get; set; }
        public int Bandeja { get; set; }
        public int? Carpeta { get; set; }

        public void OnGet(int id, int tipo, int bandeja, int? carpeta)
        {
            InfoUsuarioId = id;
            TipoHistorico = tipo;
            Bandeja = bandeja;
            Carpeta = carpeta;
        }
    }
}