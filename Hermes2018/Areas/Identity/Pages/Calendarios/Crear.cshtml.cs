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
    public class CrearModel : PageModel
    {
        private readonly ICalendarioService _calendarioService;

        public CrearModel(ICalendarioService calendarioService)
        {
            _calendarioService = calendarioService;
        }

        [BindProperty]
        public CrearCalendarioViewModel Crear { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //Valida que no exista
                var existe = await _calendarioService.ExisteCalendarioAsync(Crear.NombreCalendario, int.Parse(Crear.Anio));
                //-
                var result = false;
                if (!existe)
                {
                    //No hay un grupo con ese nombre
                    result = await _calendarioService.GuardarCalendarioAsync(Crear);

                    if (result)
                    {
                        return RedirectToPage("/Calendarios/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre o el año que usted asigno a este calendario ya se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}