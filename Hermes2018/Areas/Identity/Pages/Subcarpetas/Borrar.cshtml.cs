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
        public BorrarSubcarpetaViewModel Borrar { get; set; }

        [BindProperty]
        [HiddenInput]
        public int InfoUsuarioId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int carpetaPadreId)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToPage("/Subcarpetas/Index", new { area = "Identity", id = carpetaPadreId });
            }

            InfoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(infoUsuarioClaims.BandejaUsuario);
            //--
            var existe = await _carpetaService.ExisteSubcarpetaPorIdAsync(id, InfoUsuarioId, carpetaPadreId);
            //--
            if (existe)
            {
                var subcarpeta = await _carpetaService.ObtenerSubcarpetaAsync(InfoUsuarioId, id);
                Borrar = new BorrarSubcarpetaViewModel()
                {
                    SubcarpetaId = subcarpeta.HER_CarpetaId,
                    NombreSubcarpeta = subcarpeta.HER_Nombre,
                    CarpetaPadreId = subcarpeta.HER_CarpetaPadreId,
                    CarpetaPadre_Nombre = subcarpeta.HER_CarpetaPadre.HER_Nombre,
                    NivelSubcarpeta = subcarpeta.HER_Nivel
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var existe = await _carpetaService.ExisteSubcarpetaAsync(Borrar.NombreSubcarpeta, InfoUsuarioId, (int)Borrar.CarpetaPadreId, (int)Borrar.NivelSubcarpeta);

                if (existe)
                {
                    var tieneDocumentos = await _carpetaService.SubcarpetaTieneDocumentosAsociados(Borrar.SubcarpetaId, InfoUsuarioId, (int)Borrar.CarpetaPadreId, (int)Borrar.NivelSubcarpeta);
                    if (!tieneDocumentos)
                    {
                        var result = await _carpetaService.BorrarSubcarpetaAsync(Borrar, InfoUsuarioId);

                        if (result)
                        {
                            return RedirectToPage("/Subcarpetas/Index", new { area = "Identity", id = (int)Borrar.CarpetaPadreId });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "La subcarpeta que desea borrar tiene asociado uno o más documentos.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La subcarpeta que desea borrar no se encuentra registrada.");
                }
            }

            return Page();
        }
    }
}
