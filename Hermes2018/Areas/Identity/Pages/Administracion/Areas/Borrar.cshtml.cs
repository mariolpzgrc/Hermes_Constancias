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

namespace Hermes2018.Areas.Identity.Pages.Administracion.Areas
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class BorrarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IAreaService _areaService;

        public BorrarModel(IUsuarioClaimService usuarioClaimService,
             IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _areaService = areaService;
        }

        [BindProperty]
        public BorrarAreaViewModel Borrar { get; set; }

        public async Task OnGetAsync(int id)
        {
            Borrar = await _areaService.ObtenerAreaParaBorrarAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var existe = await _areaService.ExisteAreaPorIdAsync(Borrar.AreaId);

                if (existe)
                {
                    var enUso = await _areaService.DetectaAreaEnUsoAsync(Borrar.AreaId);
                    var tieneUsuariosBajaDefinitiva = await _areaService.DetectaAreaUsuariosDadosBajaAsync(Borrar.AreaId);

                    if (!enUso)
                    {
                        var result = await _areaService.EliminarAreaAsync(Borrar.AreaId);
                        if (result)
                        {
                            return RedirectToPage("/Administracion/Areas/Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        }
                    }
                    else if(enUso && tieneUsuariosBajaDefinitiva)
                    {
                        var result2 = await _areaService.DarBajaAreaByUsuariosInactivosAsync(Borrar.AreaId);

                        if (result2)
                        {
                            return RedirectToPage("/Administracion/Areas/Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");

                        }
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "El área que intenta borrar se encuentra en uso.");
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "El área no se encuentra registrado.");
                }
            }

            Borrar = await _areaService.ObtenerAreaParaBorrarAsync(Borrar.AreaId);

            return Page();
        }
    }
}
