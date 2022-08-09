using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Correspondencia
{
    public class BorrarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IDocumentoService _documentoService;
        private readonly CultureInfo _cultureEs;

        public BorrarModel(IUsuarioClaimService usuarioClaimService,                                        
                        IDocumentoService documentoService)
        {
            _usuarioClaimService = usuarioClaimService;
            _documentoService = documentoService;
           

            _cultureEs = new CultureInfo("es-MX");
        }

        [BindProperty]
        public BorrarDocumentoBaseViewModel Borrar { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EstadoDelegar { get; set; }
        public async Task OnGetAsync(int id)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            Borrar = await _documentoService.ObtenerInfoBaseParaBorrarDocumentoAsync(id, infoUsuarioClaims.UserName);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

                var existeBorrador = await _documentoService.ExisteDocumentoBaseAsync(Borrar.Folio, Borrar.DocumentoBaseId);

                if (existeBorrador)
                {
                    var result = await _documentoService.BorrarDocumentoBaseAsync(Borrar.Folio, Borrar.DocumentoBaseId);
                    if (result)
                    {
                        return RedirectToPage("/Bandejas/Borradores");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El borrador que intenta eliminar, no se encuentra registrado.");
                }
            }
            return Page();
        }
    }
}
