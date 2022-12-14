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

namespace Hermes2018.Areas.Identity.Pages.Delegar
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class EditarModel : PageModel
    {
        private IDelegarService _delegarService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public EditarModel(IDelegarService delegarService,
            IUsuarioClaimService usuarioClaimService)
        {
            _delegarService = delegarService;
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public DelegadoEditarViewModel Editar { get; set; }

        public async Task OnGetAsync(int id)
        {
            //Info del usuario actual
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            Editar = await _delegarService.ObtenerDelegadoParaEdicion(infoUsuarioClaims.UserName, id);
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (ModelState.IsValid) {

                //Info del usuario actual
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

                //Busca si ya esta registrado el delegado que se quiere agregar
                var existeDelegado = await _delegarService.ExisteDelegadoAsync(infoUsuarioClaims.UserName, Editar.Usuario);
                if (existeDelegado)
                {
                    var result = await _delegarService.ActualizarDelegadoAsync(infoUsuarioClaims.UserName, Editar);
                    if (result)
                    {
                        return RedirectToPage("/Delegar/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error int?ntelo m?s tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El delegado que intenta actualizar, no se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}
