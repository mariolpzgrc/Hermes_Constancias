using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hermes2018.ViewComponents
{
    public class ProximosVencerViewComponent : ViewComponent
    {
        private readonly INotificacionService _notificacionService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public ProximosVencerViewComponent(INotificacionService notificacionService, 
            IUsuarioClaimService usuarioClaimService)
        {
            _notificacionService = notificacionService;
            _usuarioClaimService = usuarioClaimService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal User)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            List<NotificacionProximosVencerViewModel> listViewModel = new List<NotificacionProximosVencerViewModel>();
            bool mostrar = false;

            if (ConstRol.RolUsuario.Contains(infoUsuarioClaims.Rol))
            {
                listViewModel = await _notificacionService.DocumentosProximosVencerAsync(infoUsuarioClaims.BandejaUsuario);
                mostrar = true;
            }

            ViewData["Mostrar"] = mostrar;
            return View(listViewModel);
        }
    }
}
