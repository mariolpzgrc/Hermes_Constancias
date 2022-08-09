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

namespace Hermes2018.Areas.Identity.Pages.Administracion.Usuarios
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class DetallesModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly IAreaService _areaService;

        public DetallesModel(IUsuarioClaimService usuarioClaimService, 
            IUsuarioService usuarioService, 
            IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _areaService = areaService;
        }

        [BindProperty]
        public UsuariosDetallesPlusViewModel ViewModel { get; set; }

        [BindProperty]
        public int Tipo { get; set; }

        [BindProperty]
        public int Padre { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, int tipo)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Tipo = tipo;

            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }

            ViewModel = new UsuariosDetallesPlusViewModel
            {
                InfoUsuarioClaims = infoUsuario
            };


            if (await _usuarioService.ExisteUsuarioActivoAsync(id))
            {
                string rolUsuario = await _usuarioService.ObtenerRolUsuarioAsync(id);
                ViewModel.Detalles = await _usuarioService.ObtenerDetalleUsuarioAsync(rolUsuario, id);
                //--
                if (Tipo == ConstTipoArea.TipoN2)
                {
                    Padre = await _areaService.ObtieneAreaPadre(ViewModel.Detalles.AreaId);
                }
                else {
                    Padre = 0;
                }
            }
            else
            {
                return NotFound();
            }

            return Page();
        }
    }
}
