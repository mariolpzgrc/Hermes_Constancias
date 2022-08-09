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
    public class ImagenModel : PageModel
    {
        private readonly IConfiguracionService _configuracionService;

        public ImagenModel(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public EditarImagenConfiguracionGeneral Editar { get; set; }

        public void OnGet(int tipo)
        {
            if (ConstConfiguracionGeneral.TipoImagenN1 != tipo && ConstConfiguracionGeneral.TipoImagenN2 != tipo)
            {
                tipo = ConstConfiguracionGeneral.TipoImagenN1;
            }
            //--
            Editar = new EditarImagenConfiguracionGeneral { Tipo = tipo };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var existe = false;
                //--
                if (ConstConfiguracionGeneral.TipoImagenN1 == Editar.Tipo)
                {
                    existe = await _configuracionService.ExisteLogoPropiedadConfiguracionAsync(Editar.Imagen.FileName);
                }
                else if ( ConstConfiguracionGeneral.TipoImagenN2 == Editar.Tipo)
                {
                    existe = await _configuracionService.ExistePortadaPropiedadConfiguracionAsync(Editar.Imagen.FileName);
                }

                if (!existe)
                {
                    //No hay un grupo con ese nombre
                    var result = await _configuracionService.ActualizarImagenPropiedadConfiguracionAsync(Editar);

                    if (result)
                    {
                        return RedirectToPage("/Configuracion/General/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "Un archivo con el mismo nombre ya se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}