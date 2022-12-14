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
    public class NotificaUsuariosViewComponent : ViewComponent
    {
        private readonly INotificacionService _notificacionService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public NotificaUsuariosViewComponent(INotificacionService notificacionService, 
            IUsuarioClaimService usuarioClaimService)
        {
            _notificacionService = notificacionService;
            _usuarioClaimService = usuarioClaimService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal User)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            List<NotificacionUsuariosViewModel> listViewModel = new List<NotificacionUsuariosViewModel>();
            bool mostrar = false;

            if (ConstRol.RolUsuario.Contains(infoUsuarioClaims.Rol))
            {
                listViewModel = await _notificacionService.UsuariosAsync(infoUsuarioClaims.AreaId);
                mostrar = true;
            }

            ViewData["Mostrar"] = mostrar;
            return View(listViewModel);
        }
    }
}
