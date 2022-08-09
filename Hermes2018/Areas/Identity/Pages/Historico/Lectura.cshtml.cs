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
    public class LecturaModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IHistoricoService _historicoService;
        private readonly IUsuarioService _usuarioService;

        public LecturaModel(IUsuarioClaimService usuarioClaimService, 
            IHistoricoService historicoService,
            IUsuarioService usuarioService)
        {
            _usuarioClaimService = usuarioClaimService;
            _historicoService = historicoService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public HistoricoDocumentoLecturaViewModel Documento { get; set; }

        [BindProperty]
        public List<HistoricoResumenDestinatarioViewModel> ResumenDestinatarios { get; set; }

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

        public async Task OnGetAsync(int infoUsuarioId, int tipoHistorico, int bandeja, int envioId, int tipoEnvio)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            InfoUsuarioId = infoUsuarioId;
            TipoHistorico = tipoHistorico;
            Bandeja = bandeja;
            EnvioId = envioId;
            TipoEnvio = tipoEnvio;
            Usuario = (tipoHistorico == ConstHistorico.ConstTipoN1)? infoUsuarioClaims.BandejaUsuario : await _usuarioService.ObtenerNombreUsuarioDelInfoUsuarioPorIdAsync(infoUsuarioId);

            if (tipoEnvio == ConstTipoEnvio.TipoEnvioN1 || tipoEnvio == ConstTipoEnvio.TipoEnvioN2)
            {
                if (tipoEnvio == ConstTipoEnvio.TipoEnvioN1)
                {
                    //Información
                    Documento = await _historicoService.ObtenerDocumentoEnviadoSoloLecturaAsync(envioId, Usuario);
                }
                else if (tipoEnvio == ConstTipoEnvio.TipoEnvioN2)
                {
                    //Información
                    Documento = await _historicoService.ObtenerDocumentoTurnadoSoloLecturaAsync(envioId, Usuario);
                }

                //Resumen
                ResumenDestinatarios = await _historicoService.ObtenerResumenDestinatariosAsync(envioId);
            }
            else if (tipoEnvio == ConstTipoEnvio.TipoEnvioN3 || tipoEnvio == ConstTipoEnvio.TipoEnvioN4)
            {
                //Información
                Documento = await _historicoService.ObtenerDocumentoRespuestaEnviadoSoloLecturaAsync(envioId, Usuario);
            }

            //--
            ViewData["Bandeja"] = "Historico";
        }
    }
}