using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Calendarios.Contenido
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class BorrarModel : PageModel
    {
        private readonly ICalendarioService _calendarioService;

        public BorrarModel(ICalendarioService calendarioService)
        {
            _calendarioService = calendarioService;
        }

        [BindProperty]
        public ResumenContenidoCalendarioViewModel Resumen { get; set; }

        public async Task OnGetAsync(int id)
        {
            Resumen = await _calendarioService.ObtenerResumenContenidoCalendarioAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;
                var existe = await _calendarioService.ExisteContenidoCalendarioPorIdAsync(Resumen.ContenidoId);
                if (existe)
                {
                    result = await _calendarioService.EliminarContenidoCalendarioAsync(Resumen.ContenidoId);

                    if (result)
                    {
                        return RedirectToPage("/Calendarios/Detalles", new { area = "Identity", id = Resumen.CalendarioId });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La fecha no se encuentra registrada.");
                }
            }

            return Page();
        }
    }
}