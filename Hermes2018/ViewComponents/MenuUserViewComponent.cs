using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewComponentsModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hermes2018.ViewComponents
{
    public class MenuUserViewComponent : ViewComponent
    {
        private readonly IUsuarioClaimService _usuarioClaimService;

        public MenuUserViewComponent(IUsuarioClaimService usuarioClaimService)
        {
            _usuarioClaimService = usuarioClaimService;
        }

        public IViewComponentResult Invoke(ClaimsPrincipal User)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            var regionId = 1;
            if (ConstRol.RolAdminRegional.Contains(infoUsuarioClaims.Rol))
            {
                regionId = infoUsuarioClaims.RegionId;
            }
            //--
            var modelcomponent = new RolViewComponentModel()
            {
                Rol = infoUsuarioClaims.Rol,
                PermisoAA = infoUsuarioClaims.PermisoAA
            };

            ViewData["Region"] = regionId;
            return View(modelcomponent);
        }
    }
}
