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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hermes2018.Areas.Identity.Pages.Configuracion.Usuario
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class EditarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IConfiguracionService _configuracionService;
        private readonly IColorService _coloresServices;

        public EditarModel(IUsuarioClaimService usuarioClaimService,
            IConfiguracionService configuracionService,
            IColorService coloresServices)
        {
            _usuarioClaimService = usuarioClaimService;
            _configuracionService = configuracionService;
            _coloresServices = coloresServices;
        }

        [BindProperty]
        public EditarConfiguracionViewModel Editar { get; set; }

        [BindProperty]
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }

        public async Task OnGetAsync(int id)
        {
            InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            Editar = await _configuracionService.ObtenerConfiguracionUsuarioEditar(id, InfoUsuarioClaims.UserName);
            //Editar.Colores = new SelectList(await _coloresServices.ObtenerColoresAsync(), "HER_ColorId", "HER_Nombre");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;
                //No hay un grupo con ese nombre
                result = await _configuracionService.ActualizarConfiguracionAsync(Editar);

                if (result)
                {
                    return RedirectToPage("/Configuracion/Usuario/Index",new { area="Identity" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }
            }

            return Page();
        }
    }
}