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
    public class EditarModel : PageModel
    {
        private readonly IConfiguracionService _configuracionService;

        public EditarModel(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public EditarAnexoRutaViewModel Editar { get; set; }

        public async Task OnGetAsync(int id)
        {
            Editar = await _configuracionService.ObtenerEditarRutaBaseAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Editar.Estado == ConstAnexoRuta.EstadoN1.ToString())
                {
                    var total = await _configuracionService.TotalRutasActivasAsync(Editar.RutaBaseId);

                    if (total == 0)
                    {
                        var result = await _configuracionService.EditarRutaBaseAsync(Editar);

                        if (result)
                            return RedirectToPage("/Configuracion/General/Index");
                        else
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una ruta activa.");
                    }
                }
                else {
                    var result = await _configuracionService.EditarRutaBaseAsync(Editar);

                    if (result)
                        return RedirectToPage("/Configuracion/General/Index");
                    else
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }
            }

            return Page();
        }
    }
}
