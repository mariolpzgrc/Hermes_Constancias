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
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)] //+ "," + ConstRol.Rol7T + "," + ConstRol.Rol8T
    public class BajaModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;

        public BajaModel(IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public BajaUsuarioViewModel Baja { get; set; }

        public async Task<ActionResult> OnGetAsync(string id)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (infoUsuario.UserName == id)
            {
                return RedirectToPage("/Usuarios/Detalles", new { id = id });
            }
            else
            {
                Baja = await _usuarioService.ObtenerInfoUsuarioParaBajaAsync(id, infoUsuario);

                return Page();
            }
        }

        public async Task<IActionResult> OnPost() 
        {
            if (await _usuarioService.ExisteUsuarioSinReasignarPorIdAsync(Baja.InfoUsuarioId))
            {
                var result = await _usuarioService.DarDeBajaUsuarioAsync(Baja, await _usuarioService.ObtenerRolesUsuarioAsync(Baja.NombreUsuario));

                if (result)
                {
                    return RedirectToPage("/Usuarios/Area", new { id = Baja.AreaId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El usuario que usted que usted eligió no esta disponible para se dado de baja.");
            }

            //info usuarioClaim
            Baja.InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            return Page();
        }
    }
}
