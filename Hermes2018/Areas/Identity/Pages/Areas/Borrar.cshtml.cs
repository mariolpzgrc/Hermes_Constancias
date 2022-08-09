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
    public class BorrarModel : PageModel
    {
        private readonly IAreaService _areaService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public BorrarModel(IAreaService areaService, IUsuarioClaimService usuarioClaimService)
        {
            _areaService = areaService;
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public BorrarAreaViewModel Borrar { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EsAdminGral { get; set; }

        [BindProperty]
        [HiddenInput]
        public int RegionId { get; set; }

        public int? AreaPadreId { get; set; }

        public async Task OnGetAsync(int id, int regionId, int? areaPadreId)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            RegionId = regionId;
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
            Borrar = await _areaService.ObtenerAreaParaBorrarAsync(id, RegionId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;
                var enUso = false;

                var existe = await _areaService.ExisteAreaPorIdAsync(Borrar.AreaId);
                if (existe)
                {
                    enUso = await _areaService.DetectaAreaEnUsoAsync(Borrar.AreaId);
                    if (!enUso)
                    {
                        result = await _areaService.EliminarAreaAsync(Borrar.AreaId);
                        if (result)
                        {
                            return RedirectToPage("/Areas/Index", new { id = RegionId });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "El área que intenta borrar se encuentra en uso.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El área no se encuentra registrado.");
                }
            }

            Borrar = await _areaService.ObtenerAreaParaBorrarAsync(Borrar.AreaId, RegionId);

            return Page();
        }
    }
}