using Hermes2018.Services;
using Hermes2018.ViewComponentsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hermes2018.ViewComponents
{
    public class BandejasViewComponent: ViewComponent
    {
        private readonly IDocumentoService _documentoService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly ICarpetaService _carpetaService;

        public BandejasViewComponent(IDocumentoService documentoService,
                                    IUsuarioClaimService usuarioClaimService,
                                    ICarpetaService carpetaService)
        {
            _documentoService = documentoService;
            _usuarioClaimService = usuarioClaimService;
            _carpetaService = carpetaService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal User)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            EstadoBandejasViewComponentModel estado = await _documentoService.ObtenerEstadoBandejasPrincipalesAsync(infoUsuarioClaims.BandejaUsuario);
            //--
            estado.ActivaDelegacion = infoUsuarioClaims.ActivaDelegacion;
            estado.BandejaPermiso = (infoUsuarioClaims.ActivaDelegacion)? infoUsuarioClaims.BandejaPermiso : 0;
            estado.EstaEnReasignacion = infoUsuarioClaims.EnReasignacion;
            return View(estado);
        }
    }
}
