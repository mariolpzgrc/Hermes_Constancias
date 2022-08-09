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
    public class DetallesModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;

        public DetallesModel(IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
        }

        public UsuariosDetallesPlusViewModel Info { get; set; }

        public async Task<ActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Info = new UsuariosDetallesPlusViewModel
            {
                InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User)
            };

            if (await _usuarioService.ExisteUsuarioActivoAsync(id))
            {
                string rolUsuario = await _usuarioService.ObtenerRolUsuarioAsync(id);
                Info.Detalles = await _usuarioService.ObtenerDetalleUsuarioAsync(rolUsuario, id);
            }
            else
            {
                return NotFound();
            }

            return Page();
        }
    }
}
