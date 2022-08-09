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
    public class CrearModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IOracleService _oracleService;
        private readonly IAreaService _areaService;
        private readonly IRegionService _regionService;

        public CrearModel(IUsuarioClaimService usuarioClaimService,
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
        public AgregarAreaViewModel Crear { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }
            Crear = new AgregarAreaViewModel() {
                RegionId = infoUsuario.RegionId.ToString(),
                Area_PadreId = infoUsuario.AreaId.ToString(),
                Clave = await _areaService.GenerarClaveAreaHijoAsync(infoUsuario.AreaId, infoUsuario.AreaClave)
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (ModelState.IsValid)
            {
                if (!await _areaService.ExisteNombreArea(Crear.Nombre))
                {
                    if (!await _areaService.ExisteClave(Crear.Clave)) 
                    {
                        var result = await _areaService.AgregarAreaManualAsync(Crear);
                        if (result)
                        {
                            return RedirectToPage("/Administracion/Areas/Index", new { Area = "Identity" });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        }
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "La clave del área que intenta registrar, ya se encuentra registrada.");
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
