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
    public class IndexModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly ICarpetaService _carpetaService;

        public IndexModel(IUsuarioClaimService usuarioClaimService, IUsuarioService usuarioService, ICarpetaService carpetaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _carpetaService = carpetaService;
        }

        public List<CarpetaViewModel> Carpetas { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool ActivaDelegacion { get; set; }

        [BindProperty]
        [HiddenInput]
        public int BandejaPermiso { get; set; }

        public async Task OnGetAsync()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            ActivaDelegacion = infoUsuarioClaims.ActivaDelegacion;
            BandejaPermiso = (infoUsuarioClaims.ActivaDelegacion) ? infoUsuarioClaims.BandejaPermiso : 0;

            var infoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(infoUsuarioClaims.BandejaUsuario);
            Carpetas = await _carpetaService.ObtenerCarpetasAsync(infoUsuarioId);
        }
    }
}