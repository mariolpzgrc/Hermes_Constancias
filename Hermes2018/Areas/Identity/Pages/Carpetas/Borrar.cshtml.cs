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
    public class BorrarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly ICarpetaService _carpetaService;

        public BorrarModel(IUsuarioClaimService usuarioClaimService, 
            IUsuarioService usuarioService, 
            ICarpetaService carpetaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _carpetaService = carpetaService;
        }

        [BindProperty]
        public BorrarCarpetaViewModel Borrar { get; set; }

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
            //--
            var existe = await _carpetaService.ExisteCarpetaPorIdAsync(id, InfoUsuarioId);

            if (existe) 
            {
                var carpeta = await _carpetaService.ObtenerCarpetaAsync(InfoUsuarioId, id);
                Borrar = new BorrarCarpetaViewModel()
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
                var existe = await _carpetaService.ExisteCarpetaPorIdAsync(Borrar.CarpetaId, InfoUsuarioId);

                if (existe)
                {
                    var tieneDocumentos = await _carpetaService.CarpetaTieneDocumentosAsociados(Borrar.CarpetaId, InfoUsuarioId);
                    if (!tieneDocumentos)
                    {
                        var tieneSubcarpetas = await _carpetaService.CarpetaTieneSubcarpetasPorIdAsync(Borrar.CarpetaId, InfoUsuarioId);

                        if (!tieneSubcarpetas)
                        {
                            var result = await _carpetaService.BorrarCarpetaAsync(Borrar, InfoUsuarioId);

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
                            ModelState.AddModelError(string.Empty, "La carpeta que desea borrar tiene asociada una o más subcarpetas.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "La carpeta que desea borrar tiene asociado uno o más documentos.");
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "La carpeta que desea borrar no se encuentra registrada.");
                }
            }

            return Page();
        }
    }
}
