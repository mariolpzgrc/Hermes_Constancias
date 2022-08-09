using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hermes2018.Controllers
{
    [Authorize]
    public class NotificacionController : Controller
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly INotificacionService _notificacionService;

        public NotificacionController(IUsuarioClaimService usuarioClaimService, INotificacionService notificacionService)
        {
            _usuarioClaimService = usuarioClaimService;
            _notificacionService = notificacionService;
        }

        [Authorize(Roles = ConstRol.Rol7T)]
        public async Task<JsonResult> UsuariosAsync()
        {
            return Json(await _notificacionService.UsuariosAsync(_usuarioClaimService.ObtenerInfoUsuarioClaims(User).AreaId));
        }
    }
}