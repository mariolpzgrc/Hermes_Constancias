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

namespace Hermes2018.Areas.Identity.Pages.Subcarpetas
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class CrearModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly ICarpetaService _carpetaService;

        public CrearModel(IUsuarioClaimService usuarioClaimService, 
            IUsuarioService usuarioService, 
            ICarpetaService carpetaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _carpetaService = carpetaService;
        }

        [BindProperty]
        public CrearSubcarpetaViewModel Crear { get; set; }

        [BindProperty]
        [HiddenInput]
        public int CarpetaPadreId { get; set; }

        public  IActionResult OnGet(int id)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToPage("/Subcarpetas/Index", new { area = "Identity", id = "" });
            }
            CarpetaPadreId = id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
                //Usuario actual
                var infoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(infoUsuarioClaims.BandejaUsuario);
                var subcarpetaNivel = await _carpetaService.ObtenerNivelSubCarpetaAsync(infoUsuarioId, CarpetaPadreId);
                var nivelActual = subcarpetaNivel.HER_Nivel + 1;

                //Valida que no exista
                var existe = await _carpetaService.ExisteSubcarpetaAsync(Crear.NombreSubcarpeta, infoUsuarioId, CarpetaPadreId, nivelActual);
                //-
                var result = false;
                if (!existe)
                {
                    //No hay un grupo con ese nombre
                    result = await _carpetaService.GuardarSubcarpetasAsync(Crear, infoUsuarioId, CarpetaPadreId, nivelActual);

                    if (result)
                    {
                        return RedirectToPage("/Subcarpetas/Index", new { id = CarpetaPadreId });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre que usted asigno a esta subcarpeta ya se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}