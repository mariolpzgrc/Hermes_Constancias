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

namespace Hermes2018.Areas.Identity.Pages.Grupos
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class BorrarModel : PageModel
    {
        private IGrupoService _integrantesService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public BorrarModel(IGrupoService integrantesService,
            IUsuarioClaimService usuarioClaimService)
        {
            _integrantesService = integrantesService;
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public BorrarIntegranteGrupoViewModel Borrar { get; set; }

        [BindProperty]
        public int IdGrupo { get; set; }

        public async Task OnGetAsync(string username, int grupoId)
        {
            //Info del usuario actual
            //var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            Borrar = await _integrantesService.ObtenerIntegranteParaBorrar(username, grupoId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //Info del usuario actual
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

                //Busca si ya esta registrado el integrante que se quiere agregar
                var existeIntegrante = await _integrantesService.ExisteIntegranteAsync(Borrar.Usuario, Borrar.GrupoId);

                IdGrupo = Borrar.GrupoId;

                if (existeIntegrante)
                {
                    var result = await _integrantesService.BorrarIntegranteAsync(Borrar.Usuario, Borrar);
                    if (result)
                    {
                        return RedirectToPage("/Grupos/Detalles", new { id = IdGrupo });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El integrante que intenta borrar, no se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}
