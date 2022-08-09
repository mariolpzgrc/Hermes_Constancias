using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Plantillas
{
    public class CrearModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPlantillaService _plantillaService;

        public CrearModel(IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService,
            IPlantillaService plantillaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _plantillaService = plantillaService;
        }

        [BindProperty]
        public CrearPlantillaViewModel Crear { get; set; }

        public IActionResult OnGet()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToPage("/Plantillas/Index", new { area = "Identity", id = "" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
                //Usuario actual
                var infoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(infoUsuarioClaims.BandejaUsuario);
                //Valida que no exista
                var existe = await _plantillaService.ExistePlantillaAsync(Crear.Nombre, infoUsuarioClaims.BandejaUsuario);
                //-
                var result = false;
                if (!existe)
                {
                    //No hay un grupo con ese nombre
                    result = await _plantillaService.GuardarPlantillaDesdeFormularioAsync(Crear, infoUsuarioId);

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
                    ModelState.AddModelError(string.Empty, "El nombre que usted asigno a esta plantilla ya se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}