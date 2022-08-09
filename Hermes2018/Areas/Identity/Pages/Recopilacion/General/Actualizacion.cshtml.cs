using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Recopilacion.General
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class ActualizacionModel : PageModel
    {
        public readonly IRecopilacionService _recopilacionService;

        public ActualizacionModel(IRecopilacionService recopilacionService)
        {
            _recopilacionService = recopilacionService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _recopilacionService.CargarRecopilacionAsync();

            if (result)
                return RedirectToPage("/Recopilacion/General/Index");
            else {
                ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                return Page();
            }
        }
    }
}
