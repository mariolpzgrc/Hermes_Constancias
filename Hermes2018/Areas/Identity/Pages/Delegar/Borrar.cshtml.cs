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
    public class BorrarModel : PageModel
    {
        private IDelegarService _delegarService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public BorrarModel(IDelegarService delegarService,
            IUsuarioClaimService usuarioClaimService)
        {
            _delegarService = delegarService;
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public DelegadoBorrarViewModel Borrar { get; set; }

        public async Task OnGetAsync(int id)
        {
            //Info del usuario actual
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            Borrar = await _delegarService.ObtenerDelegadoParaBorrar(infoUsuarioClaims.UserName, id);
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (ModelState.IsValid)
            {
                //Info del usuario actual
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

                //Busca si ya esta registrado el delegado que se quiere agregar
                var existeDelegado = await _delegarService.ExisteDelegadoAsync(infoUsuarioClaims.UserName, Borrar.Usuario);
                if (existeDelegado) 
                {
                    var result = await _delegarService.BorrarDelegadoAsync(infoUsuarioClaims.UserName, Borrar);
                    if (result)
                    {
                        return RedirectToPage("/Delegar/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El delegado que intenta borrar, no se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}
