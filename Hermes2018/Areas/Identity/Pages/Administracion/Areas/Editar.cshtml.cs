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

namespace Hermes2018.Areas.Identity.Pages.Administracion.Areas
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class EditarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IAreaService _areaService;

        public EditarModel(IUsuarioClaimService usuarioClaimService,
             IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _areaService = areaService;
        }

        [BindProperty]
        public EditarAreaEnAdminViewModel Editar { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }

            var existe = await _areaService.ExisteAreaPorIdAsync(id);
            if (!existe)
            {
                return NotFound();
            }

            Editar = await _areaService.AdminObtenerAreaParaEditar(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _areaService.AdminActualizarAreaAsync(Editar);
                if (result)
                {
                    return RedirectToPage("/Administracion/Areas/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }

                Editar = await _areaService.AdminObtenerAreaParaEditar(Editar.AreaId);
            }

            return Page();
        }
    }
}
