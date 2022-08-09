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

namespace Hermes2018.Areas.Identity.Pages.Areas
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
    public class BajaModel : PageModel
    {
        private readonly IAreaService _areaService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public BajaModel(IAreaService areaService,
            IUsuarioClaimService usuarioClaimService)
        {
            _areaService = areaService;
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public DarDeBajaAreaViewModel BajaViewModel { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EsAdminGral { get; set; }

        [BindProperty]
        [HiddenInput]
        public int RegionId { get; set; }

        [BindProperty]
        [HiddenInput]
        public int AreaId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int regionId)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            RegionId = regionId;
            AreaId = id;
            EsAdminGral = ConstRol.RolAdminGral.Contains(infoUsuario.Rol);

            if (regionId < 1 && regionId > 5)
            {
                RegionId = 1;
            }

            if (ConstRol.RolAdminRegional.Contains(infoUsuario.Rol))
            {
                if (regionId != infoUsuario.RegionId)
                {
                    RegionId = infoUsuario.RegionId;
                }
            }
            //---
            var existe = await _areaService.ExisteAreaPorIdAsync(AreaId);
            if (!existe)
            {
                return NotFound();
            }

            BajaViewModel = await _areaService.ObtenerAreaParaDarDeBajaAsync(AreaId, RegionId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (!await _areaService.ExisteAreaEnProcesoDeCambioPorIdAsync(BajaViewModel.AreaId))
                {
                    var result = await _areaService.DarDeBajaAreaAsync(BajaViewModel);

                    if (result)
                    {
                        return RedirectToPage("/Areas/Index", new { area = "Identity", id = RegionId });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El área que usted que usted eligió no esta disponible.");
                }
            }

            return Page();
        }
    }
}
