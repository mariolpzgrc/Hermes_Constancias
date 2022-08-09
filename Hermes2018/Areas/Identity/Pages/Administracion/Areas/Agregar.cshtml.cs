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

namespace Hermes2018.Areas.Identity.Pages.Administracion.Areas
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
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

        public SelectList Regiones { get; set; }
        public SelectList Areas { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (!infoUsuario.PermisoAA)
                return NotFound();

            if (!await _oracleService.ExisteAreaPorClaveAsync(id))
                return NotFound();

            Agregar = await _oracleService.ObtenerAreaParaAgregarAsync(id);
            Agregar.RegionId = infoUsuario.RegionId.ToString();
            Agregar.Area_PadreId = infoUsuario.AreaId.ToString();

            Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(infoUsuario.RegionId), "HER_RegionId", "HER_Nombre");
            Areas = new SelectList(await _areaService.ObtenerAreaEnListaAsync(infoUsuario.AreaId), "HER_AreaId", "HER_Nombre");

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
                        return RedirectToPage("/Administracion/Areas/Detectar", new { Area = "Identity" });
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

                //---
                Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(infoUsuario.RegionId), "HER_RegionId", "HER_Nombre");
                Areas = new SelectList(await _areaService.ObtenerAreaEnListaAsync(infoUsuario.AreaId), "HER_AreaId", "HER_Nombre");
            }

            return Page();
        }
    }
}
