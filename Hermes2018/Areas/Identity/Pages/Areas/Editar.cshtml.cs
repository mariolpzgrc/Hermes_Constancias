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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hermes2018.Areas.Identity.Pages.Areas
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
    public class EditarModel : PageModel
    {
        private readonly IAreaService _areaService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public EditarModel(IAreaService areaService,
            IUsuarioClaimService usuarioClaimService)
        {
            _areaService = areaService;
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public EditarAreaViewModel Editar { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EsAdminGral { get; set; }

        [BindProperty]
        [HiddenInput]
        public int RegionId { get; set; }

        [BindProperty]
        [HiddenInput]
        public int AreaId { get; set; }

        public int? AreaPadreId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int regionId, int? areaPadreId)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            RegionId = regionId;
            AreaId = id;
            AreaPadreId = areaPadreId;
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
            //--
            var existe = await _areaService.ExisteAreaPorIdAsync(AreaId);
            if (!existe) 
            {
                return NotFound();
            }

            Editar = await _areaService.ObtenerAreaParaEditar(AreaId, RegionId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _areaService.ActualizarAreaAsync(Editar, RegionId);
                if (result)
                {
                    return RedirectToPage("/Areas/Index", new { id = RegionId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }

                Editar = await _areaService.ObtenerAreaParaEditar(AreaId, RegionId);
            }

            return Page();
        }
    }
}