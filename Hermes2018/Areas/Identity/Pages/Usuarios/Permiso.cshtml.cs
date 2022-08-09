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

namespace Hermes2018.Areas.Identity.Pages.Usuarios
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
    public class PermisoModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;

        public PermisoModel(IUsuarioClaimService usuarioClaimService, 
            IUsuarioService usuarioService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public PermisoAdminAreaViewModel Permiso { get; set; }

        public async Task<ActionResult> OnGetAsync(string id)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (infoUsuario.UserName == id)
            {
                return RedirectToPage("/Usuario/Detalles", new { id = id });
            }
            else
            {
                Permiso = await _usuarioService.ObtenerInfoUsuarioParaPermisoAsync(id, infoUsuario);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (ModelState.IsValid)
            {
                var result = await _usuarioService.GuardarPermisoUsuarioAsync(Permiso);
                if (result)
                {
                    return RedirectToPage("/Usuario/Detalles", new { id = Permiso.NombreUsuario });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }
            }

            Permiso.InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            return Page();
        }
    }
}
