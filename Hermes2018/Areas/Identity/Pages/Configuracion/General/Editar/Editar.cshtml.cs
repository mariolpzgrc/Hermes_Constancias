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

namespace Hermes2018.Areas.Identity.Pages.Configuracion.General
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class EditarModel : PageModel
    {
        private readonly IConfiguracionService _configuracionService;

        public EditarModel(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public EditarConfiguracionGeneral Editar { get; set; }

        public async Task OnGetAsync(int id)
        {
            Editar = await _configuracionService.ObtenerConfiguracionTextoParaEditarAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;
                //No hay un grupo con ese nombre
                result = await _configuracionService.ActualizarPropiedadConfiguracionAsync(Editar);

                if (result)
                {
                    return RedirectToPage("/Configuracion/General/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }
                
                //-----
                Editar = await _configuracionService.ObtenerConfiguracionTextoParaEditarAsync(Editar.ConfiguracionId);
            }

            return Page();
        }
    }
}