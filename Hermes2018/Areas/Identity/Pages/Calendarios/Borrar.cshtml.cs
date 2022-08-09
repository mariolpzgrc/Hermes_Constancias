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

namespace Hermes2018.Areas.Identity.Pages.Calendarios
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
        public InfoCalendarioViewModel Calendario { get; set; }

        public async Task OnGetAsync(int id)
        {
            Calendario = await _calendarioService.ObtenerCalendarioAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;

                var existe = await _calendarioService.ExisteCalendarioPorIdAsync(Calendario.CalendarioId);
                if (existe)
                {
                    result = await _calendarioService.EliminarCalendarioAsync(Calendario.CalendarioId);
                    if (result)
                    {
                        return RedirectToPage("/Calendarios/Index", new { area = "Identity" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El calendario no se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}