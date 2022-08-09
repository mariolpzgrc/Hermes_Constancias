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
    public class CrearModel : PageModel
    {
        private readonly ICalendarioService _calendarioService;
        private readonly CultureInfo _cultureEs;

        public CrearModel(ICalendarioService calendarioService)
        {
            _calendarioService = calendarioService;
            _cultureEs = new CultureInfo("es-MX");
        }

        [BindProperty]
        public CrearContenidoCalendarioViewModel Crear { get; set; }

        [BindProperty]
        public InfoCalendarioViewModel InfoCalendario { get; set; }

        public async Task OnGetAsync(int id)
        {
            InfoCalendario = await _calendarioService.ObtenerCalendarioAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;
                string[] arrRango = Crear.Fecha.Replace(" ", string.Empty).Split("-");
                
                //Valida que no exista
                var existe = await _calendarioService.ExisteCalendarioPorIdAsync(InfoCalendario.CalendarioId);
                List<DateTime> fechas = new List<DateTime>();

                if (existe)
                {
                    fechas = await _calendarioService.ProcesaRangoFechasAsync(
                            InfoCalendario.CalendarioId,
                            Convert.ToDateTime(arrRango[0], _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0),
                            Convert.ToDateTime(arrRango[1], _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59)
                        );

                    if (fechas.Count > 0)
                    {
                        result = await _calendarioService.GuardarContenidoCalendarioAsync(fechas, InfoCalendario.CalendarioId);

                        if (result)
                        {
                            return RedirectToPage("/Calendarios/Detalles", new { area ="Identity", id = InfoCalendario.CalendarioId });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        }
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "Algunos elementos que usted ha seleccionado han sido registrados anteriormente.");
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