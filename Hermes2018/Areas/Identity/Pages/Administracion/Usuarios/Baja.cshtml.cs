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
    public class BajaModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly IAreaService _areaService;

        public BajaModel(IUsuarioClaimService usuarioClaimService, 
            IUsuarioService usuarioService,
            IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _areaService = areaService;
        }

        [BindProperty]
        public BajaUsuarioViewModel ViewModel { get; set; }

        [BindProperty]
        [HiddenInput]
        public int Tipo { get; set; }

        [BindProperty]
        public UsuariosDetallesPlusViewModel ViewModelPlus { get; set; }

        [BindProperty]
        [HiddenInput]
        public int Padre { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, int tipo)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            Tipo = tipo;

            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }

            if (infoUsuario.UserName == id)
            {
                return RedirectToPage("/Administracion/Usuarios/Detalles", new { area = "Identity", id = id });
            }
            else
            {
                ViewModel = await _usuarioService.ObtenerInfoUsuarioParaBajaAsync(id, infoUsuario);
                //--
                if (Tipo == ConstTipoArea.TipoN2)
                {
                    Padre = await _areaService.ObtieneAreaPadre(ViewModel.AreaId);
                }
                else
                {
                    Padre = 0;
                }
            }



            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (await _usuarioService.ExisteUsuarioSinReasignarPorIdAsync(ViewModel.InfoUsuarioId))
                {
                    var result = await _usuarioService.DarDeBajaUsuarioAsync(ViewModel, await _usuarioService.ObtenerRolesUsuarioAsync(ViewModel.NombreUsuario));

                    if (result)
                    {
                        if(Tipo == ConstTipoArea.TipoN1)
                        {
                            return RedirectToPage("/Administracion/Usuarios/Area", new {id = ViewModel.AreaId, tipo = Tipo } );
                        }
                        else
                        {
                            return RedirectToPage("/Administracion/Usuarios/Area", new { id = ViewModel.AreaId, tipo = Tipo, areapadre = Padre });
                        }
                        
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
            }

            //info usuarioClaim
            ViewModel.InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            return Page();
        }
    }
}
