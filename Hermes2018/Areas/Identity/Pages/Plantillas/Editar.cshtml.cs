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

namespace Hermes2018.Areas.Identity.Pages.Plantillas
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class EditarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IPlantillaService _plantillaService;

        public EditarModel(IUsuarioClaimService usuarioClaimService, 
            IPlantillaService plantillaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _plantillaService = plantillaService;
        }

        [BindProperty]
        [HiddenInput]
        public bool Existe { get; set; }

        [BindProperty]
        public EditarPlantillaViewModel Editar { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuario.ActivaDelegacion && infoUsuario.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToPage("/Plantillas/Index", new { area = "Identity", id = "" });
            }

            Editar = await _plantillaService.ObtenerInfoEditarPlantillaAsync(id, infoUsuario.BandejaUsuario);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

                Existe = await _plantillaService.ExistePlantillaPorIdAsync(Editar.PlantillaId, infoUsuario.BandejaUsuario);
                //-
                var result = false;
                if (Existe)
                {
                    result = await _plantillaService.ActualizarPlantillaAsync(Editar, infoUsuario.BandejaUsuario);

                    if (result)
                    {
                        return RedirectToPage("/Plantillas/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La plantilla no se encuentra registrada.");
                }
            }

            return Page();
        }
    }
}