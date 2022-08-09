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

namespace Hermes2018.Areas.Identity.Pages.Carpetas
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class EditarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly ICarpetaService _carpetaService;

        public EditarModel(IUsuarioClaimService usuarioClaimService, IUsuarioService usuarioService, ICarpetaService carpetaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _carpetaService = carpetaService;
        }

        [BindProperty]
        public EditarCarpetaViewModel Editar { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool Existe { get; set; }

        [BindProperty]
        [HiddenInput]
        public int InfoUsuarioId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToPage("/Carpetas/Index", new { area = "Identity", id = "" });
            }

            InfoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(infoUsuarioClaims.BandejaUsuario);
            Existe = await _carpetaService.ExisteCarpetaPorIdAsync(id, InfoUsuarioId);

            if (Existe)
            {
                var carpeta = await _carpetaService.ObtenerCarpetaAsync(InfoUsuarioId, id);
                Editar = new EditarCarpetaViewModel()
                {
                    CarpetaId = carpeta.HER_CarpetaId,
                    NombreCarpeta = carpeta.HER_Nombre
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Existe = await _carpetaService.ExisteCarpetaAsync(Editar.NombreCarpeta, InfoUsuarioId);
                //-
                var result = false;
                if (!Existe)
                {
                    //No hay un grupo con ese nombre
                    result = await _carpetaService.ActualizarCarpetaAsync(Editar, InfoUsuarioId);

                    if (result)
                    {
                        return RedirectToPage("/Carpetas/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre que usted asigno a esta carpeta ya se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}