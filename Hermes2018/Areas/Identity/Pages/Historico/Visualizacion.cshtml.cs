using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Historico
{
    [Authorize]
    public class VisualizacionModel : PageModel
    {
        private readonly IHistoricoService _historicoService;

        public VisualizacionModel(IHistoricoService historicoService)
        {
            _historicoService = historicoService;
        }

        [BindProperty]
        public HistoricoDocumentoVisualizacionViewModel Documento { get; set; }

        [BindProperty]
        [HiddenInput]
        public int InfoUsuarioId { get; set; }
        [BindProperty]
        [HiddenInput]
        public int TipoHistorico { get; set; }
        [BindProperty]
        [HiddenInput]
        public int Bandeja { get; set; }
        [BindProperty]
        [HiddenInput]
        public int EnvioId { get; set; }
        [BindProperty]
        [HiddenInput]
        public int TipoEnvio { get; set; }
        [BindProperty]
        [HiddenInput]
        public string Usuario { get; set; }

        public async Task OnGetAsync(int infoUsuarioId, int tipoHistorico, int bandeja, int envioId, int tipoEnvio, string usuario = "")
        {
            if (tipoEnvio == ConstTipoEnvio.TipoEnvioN1)
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    //Información del documento (Envio)
                    Documento = await _historicoService.ObtenerDocumentoEnviadoSoloVisualizacionAsync(envioId, usuario);
                }
            }
            else if (tipoEnvio == ConstTipoEnvio.TipoEnvioN2)
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    //Información del dcumento
                    Documento = await _historicoService.ObtenerDocumentoTurnadoSoloVisualizacionAsync(envioId, usuario);
                }
            }
            else if (tipoEnvio == ConstTipoEnvio.TipoEnvioN3)
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    //Información del documento respuesta
                    Documento = await _historicoService.ObtenerDocumentoRespuestaEnviadoSoloVisualizacionAsync(envioId, usuario);
                }
            }
        }
    }
}