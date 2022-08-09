using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewComponentsModels;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hermes2018.ViewComponents
{
    public class InfoUserViewComponent : ViewComponent
    {
        private readonly IUsuarioClaimService _usuarioClaimService;

        public InfoUserViewComponent(IUsuarioClaimService usuarioClaimService)
        {
            _usuarioClaimService = usuarioClaimService;
        }

        public IViewComponentResult Invoke(ClaimsPrincipal User)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            return View(infoUsuarioClaims);
        }
    }
}
