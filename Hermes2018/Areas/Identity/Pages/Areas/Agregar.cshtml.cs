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
    public class AgregarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IOracleService _oracleService;
        private readonly IAreaService _areaService;
        private readonly IRegionService _regionService;

        public AgregarModel(IUsuarioClaimService usuarioClaimService,
            IOracleService oracleService,
            IAreaService areaService,
            IRegionService regionService)
        {
            _usuarioClaimService = usuarioClaimService;
            _oracleService = oracleService;
            _areaService = areaService;
            _regionService = regionService;
        }

        [BindProperty]
        public AgregarAreaViewModel Agregar { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EsAdminGral { get; set; }
        [BindProperty]
        [HiddenInput]
        public int RegionId { get; set; }
        [BindProperty]
        [HiddenInput]
        public int AreaId { get; set; }
        [BindProperty]
        [HiddenInput]
        public int? AreaPadreId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, int areaId, int regionId, int? areaPadreId)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (regionId < 1 && regionId > 5)
                return NotFound();

            if (!await _oracleService.ExisteAreaPorClaveAsync(id))
                return NotFound();

            RegionId = regionId;
            AreaId = areaId;
            AreaPadreId = areaPadreId;
            Agregar = await _oracleService.ObtenerAreaParaAgregarAsync(id);

            if (ConstRol.RolAdminRegional.Contains(infoUsuario.Rol))
            {
                if (RegionId != infoUsuario.RegionId)
                {
                    RegionId = infoUsuario.RegionId;
                }

                EsAdminGral = false;
            }
            else {
                EsAdminGral = true;
            }

            Agregar.RegionId = RegionId.ToString();
            Agregar.Area_PadreId = areaId.ToString();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (ModelState.IsValid)
            {
                if (!await _areaService.ExisteNombreArea(Agregar.Nombre))
                {
                    var result = await _areaService.AgregarAreaAsync(Agregar);
                    if (result)
                    {
                        return RedirectToPage("/Areas/Detectar", new { Area = "Identity", id = Agregar.Area_PadreId, regionId = Agregar.RegionId });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre del área que intenta registrar, ya se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}
