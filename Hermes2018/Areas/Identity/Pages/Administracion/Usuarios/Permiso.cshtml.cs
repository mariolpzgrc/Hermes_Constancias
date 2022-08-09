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
    public class PermisoModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly IAreaService _areaService;

        public PermisoModel(IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService,
            IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _areaService = areaService;
        }

        [BindProperty]
        public PermisoAdminAreaViewModel ViewModel { get; set; }

        [BindProperty]
        public int Tipo { get; set; }

        [BindProperty]
        public int Padre { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, int tipo)
        {
            if (string.IsNullOrEmpty(id) || tipo == 0)
            {
                return NotFound();
            }

            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }

            if (!await _usuarioService.ExisteUsuarioActivoAsync(id))
            {
                return NotFound();
            }

            Tipo = tipo;
            ViewModel = await _usuarioService.ObtenerInfoUsuarioParaPermisoAsync(id, infoUsuario);
            //--
            if (Tipo == ConstTipoArea.TipoN2)
            {
                Padre = await _areaService.ObtieneAreaPadre(ViewModel.AreaId);
            }
            
             return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid) 
            {
                var result = await _usuarioService.GuardarPermisoUsuarioAsync(ViewModel);
                if (result)
                {
                    return RedirectToPage("/Administracion/Usuarios/Detalles", new {id  = ViewModel.NombreUsuario, tipo = Tipo} );
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }
            }
            ViewModel.InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            return Page();
        }
    }
}
