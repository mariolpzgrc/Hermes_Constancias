using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Models.Documento;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Correspondencia
{
    [Authorize]
    public class SeguimientoModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IDocumentoService _documentoService;

        public SeguimientoModel(IUsuarioClaimService usuarioClaimService, 
            IDocumentoService documentoService)
        {
            _usuarioClaimService = usuarioClaimService;
            _documentoService = documentoService;
        }

        [BindProperty]
        public EncabezadoSeguimientoViewModel Encabezado { get; set; }

        public async Task OnGetAsync(int id, int tipo)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            //Obtener la información del encabezado
            Encabezado = await _documentoService.ObtenerEncabezadoSeguimientoAsync(id, tipo, infoUsuarioClaims.BandejaUsuario);

            ViewData["Bandeja"] = Encabezado.Visualizacion_Tipo == ConstVisualizacionEnvio.Envio ? "Enviados" : "Recibidos";
        }
    }
}