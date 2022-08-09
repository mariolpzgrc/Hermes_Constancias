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
    public class BorrarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IPlantillaService _plantillaService;

        public BorrarModel(IUsuarioClaimService usuarioClaimService,
            IPlantillaService plantillaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _plantillaService = plantillaService;
        }

        [BindProperty]
        [HiddenInput]
        public bool Existe { get; set; }

        [BindProperty]
        public PlantillaViewModel InfoPlantilla { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuario.ActivaDelegacion && infoUsuario.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToPage("/Plantillas/Index", new { area = "Identity", id = "" });
            }
            //-----
            Existe = await _plantillaService.ExistePlantillaPorIdAsync(id, infoUsuario.BandejaUsuario);
            if (Existe)
            {
                InfoPlantilla = await _plantillaService.ObtenerPlantillaPorIdAsync(id, infoUsuario.BandejaUsuario);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;
                var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

                var existe = await _plantillaService.ExistePlantillaPorIdAsync(InfoPlantilla.HER_PlantillaId, infoUsuario.BandejaUsuario);
                if (existe)
                {
                    result = await _plantillaService.EliminarPlantillaAsync(InfoPlantilla.HER_PlantillaId, infoUsuario.BandejaUsuario);
                    if (result)
                    {
                        return RedirectToPage("/Plantillas/Index", new { area = "Identity" });
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