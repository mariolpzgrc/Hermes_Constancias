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

namespace Hermes2018.Areas.Identity.Pages.Configuracion.General.Ruta
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class CrearModel : PageModel
    {
        private readonly IConfiguracionService _configuracionService;

        public CrearModel(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public CrearAnexoRutaViewModel Crear { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _configuracionService.GuardarRutaBaseAsync(Crear);
                
                if (result)
                    return RedirectToPage("/Configuracion/General/Index");
                else
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
            }

            return Page();
        }
    }
}
