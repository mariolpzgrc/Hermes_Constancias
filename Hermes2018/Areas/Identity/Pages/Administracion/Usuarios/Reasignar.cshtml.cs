using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Areas.Identity.Pages.Administracion.Usuarios
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class ReasignarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly IRegionService _regionService;
        private readonly IAreaService _areaService;

        public ReasignarModel(IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService,
            IRolService rolService, 
            IRegionService regionService,
            IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _rolService = rolService;
            _regionService = regionService;
            _areaService = areaService;
        }

        [BindProperty]
        public AdminReasignarUsuarioViewModel ViewModel { get; set; }

        [BindProperty]
        [HiddenInput]
        public List<AreaViewModel> ListaAreas { get; set; }
        [BindProperty]
        [HiddenInput]
        public int AreaId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }

            AreaId = infoUsuario.AreaId;

            ViewModel = new AdminReasignarUsuarioViewModel()
            {
                InfoUsuarioClaims = infoUsuario,
                TipoVista = ConstTipoHttp.ConstTipoVistaHttpN1,
                Roles = new SelectList(await _rolService.ObtenerRolEnListaAsync(ConstRol.Rol8T), "HER_Nombre", "HER_Nombre"),
                EsTitular = true,
                RegionId = infoUsuario.RegionId.ToString()
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                bool result = await _usuarioService.AdminGuardarReasignacion(ViewModel);
                //--
                if (result)
                {
                    return RedirectToPage("/Administracion/Usuarios/Index", new { area = "Identity" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }
            }
            //---
            //ListaAreas = await _areaService.ObtenerAreasVisiblesPorAreaPadreConHijasAsync(ViewModel.InfoUsuarioClaims.AreaId);
            //----
            ViewModel.InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            ViewModel.TipoVista = ConstTipoHttp.ConstTipoVistaHttpN2;
            ViewModel.Roles = new SelectList(await _rolService.ObtenerRolEnListaAsync(ConstRol.Rol8T), "HER_Nombre", "HER_Nombre");
            //---
            return Page();
        }
    }
}
