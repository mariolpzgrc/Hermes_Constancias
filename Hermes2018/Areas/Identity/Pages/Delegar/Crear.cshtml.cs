using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Delegar
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class CrearModel : PageModel
    {
        private IDelegarService _delegarService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public CrearModel(IDelegarService delegarService,
            IUsuarioClaimService usuarioClaimService)
        {
            _delegarService = delegarService;
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public DelegadosCrearViewModel Crear { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //Info del usuario actual
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

                //Busca todos los delegados del usuario actual
                var totalDelegados = await _delegarService.ObtenerTotalDelegadosAsync(infoUsuarioClaims.UserName);

                //Valida que solo sean 3 el maximo de delegados
                if (totalDelegados > 3)
                {
                    ModelState.AddModelError(string.Empty, "Solo se pueden registrar hasta 3 delegados, y usted ya ha superado este limite.");
                }
                else
                {
                    //Busca si ya esta registrado el delegado que se quiere agregar
                    var existeDelegado = await _delegarService.ExisteDelegadoAsync(infoUsuarioClaims.UserName, Crear.Usuario);
                    if (!existeDelegado)
                    {
                        var result = await _delegarService.GuardarDelegadoAsync(infoUsuarioClaims.UserName, Crear.Usuario, Crear.Tipo);
                        if (result)
                        {
                            return RedirectToPage("/Delegar/Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        }
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "El usuario que intenta registrar como delegado, ya se encuentra registrado.");
                    }
                }
            }

            return Page();
        }
    }
}
