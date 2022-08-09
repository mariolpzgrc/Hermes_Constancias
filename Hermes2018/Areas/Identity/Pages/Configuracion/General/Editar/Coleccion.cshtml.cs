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

namespace Hermes2018.Areas.Identity.Pages.Configuracion.General.Editar
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class ColeccionModel : PageModel
    {
        private readonly IConfiguracionService _configuracionService;

        public ColeccionModel(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public List<EditarColeccionConfiguracionGeneral> Editar { get; set; }

        [BindProperty]
        public List<EditarColeccionConfiguracionGeneral> Nuevo { get; set; }

        [BindProperty]
        [HiddenInput]
        public int Tipo { get; set; }

        public async Task OnGetAsync(int tipo)
        {
            if (ConstConfiguracionGeneral.TipoColeccionN2 != tipo)
            {
                tipo = ConstConfiguracionGeneral.TipoColeccionN2;
            }
            //--
            Tipo = tipo;
            //---
            if (Tipo == ConstConfiguracionGeneral.TipoColeccionN2)
            {
                Editar = await _configuracionService.ObtenerExtensiones();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;
                result = await _configuracionService.ActualizarExtensionesConfiguracionAsync(Editar, Nuevo);

                if (result)
                {
                    return RedirectToPage("/Configuracion/General/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }

                //-----
                if (Tipo == ConstConfiguracionGeneral.TipoColeccionN2)
                {
                    Editar = await _configuracionService.ObtenerExtensiones();
                }
            }

            return Page();
        }
    }
}