
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Areas.Identity.Pages.Account;
using Hermes2018.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Hermes2018.Models.Area;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Hermes2018.Models;
using Hermes2018.ViewModels;
using Hermes2018.Services;
using Hermes2018.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Hermes2018.Areas.Identity.Pages.Areas
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
    public class CrearModel : PageModel
    {
        private readonly IAreaService _areaService;
        private readonly IRegionService _regionService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public CrearModel(IAreaService areaService, IUsuarioClaimService usuarioClaimService, IRegionService regionService)
        {
            _areaService = areaService;
            _usuarioClaimService = usuarioClaimService;
            _regionService = regionService;
        }

        [BindProperty]
        public CrearAreaViewModel Crear { get; set; }

        public SelectList Regiones { get; set; }
        public SelectList Areas { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EsAdminGral { get; set; }

        [BindProperty]
        [HiddenInput]
        public int RegionId { get; set; }

        public async Task OnGetAsync(int id)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            RegionId = id;
            EsAdminGral = ConstRol.RolAdminGral.Contains(infoUsuario.Rol);

            if (id < 1 && id > 5)
            {
                RegionId = 1;
            }

            if (ConstRol.RolAdminRegional.Contains(infoUsuario.Rol))
            {
                if (id != infoUsuario.RegionId)
                {
                    RegionId = infoUsuario.RegionId;
                }

                Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(RegionId), "HER_RegionId", "HER_Nombre");
                Areas = new SelectList(await _areaService.ObtenerAreasAsync(RegionId), "HER_AreaId", "HER_Nombre");
            }
            else {
                Regiones = new SelectList(await _regionService.ObtenerRegionesAsync(), "HER_RegionId", "HER_Nombre");
            }
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
                        var result = await _areaService.GuardarAreaAsync(Crear, RegionId);
                        if (result)
                        {
                            return RedirectToPage("/Areas/Index", new { id = RegionId });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        }
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "La clave del área que intenta registrar, ya se encuentra registrado.");
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "El nombre del área que intenta registrar, ya se encuentra registrado.");
                }

                //---
                if (EsAdminGral)
                {
                    if (Crear.AsignarAreaPadre)
                    {
                        Regiones = new SelectList(await _regionService.ObtenerRegionesAsync(), "HER_RegionId", "HER_Nombre");
                        Areas = new SelectList(await _areaService.ObtenerAreasAsync(Int32.Parse(Crear.RegionId)), "HER_AreaId", "HER_Nombre");
                    }
                    else
                    {
                        Regiones = new SelectList(await _regionService.ObtenerRegionesAsync(), "HER_RegionId", "HER_Nombre");
                    }
                }
                else
                {
                    Areas = new SelectList(await _areaService.ObtenerAreasAsync(RegionId), "HER_AreaId", "HER_Nombre");
                }
            }

            return Page();
        }
    }
}