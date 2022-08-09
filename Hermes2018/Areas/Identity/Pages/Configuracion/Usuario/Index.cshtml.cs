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

namespace Hermes2018.Areas.Identity.Pages.Configuracion.Usuario
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class IndexModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IConfiguracionService _configuracionService;

        public IndexModel(IUsuarioClaimService usuarioClaimService, 
            IConfiguracionService configuracionService)
        {
            _usuarioClaimService = usuarioClaimService;
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public ConfiguracionUsuarioViewModel InfoConfiguracion { get; set; }

        [BindProperty]
        public InfoConfigUsuarioViewModel InfoUsuarioClaims { get; set; }

        public async Task OnGetAsync()
        {
            InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            InfoConfiguracion = await _configuracionService.ObtenerConfiguracionUsuarioAsync(InfoUsuarioClaims.UserName);
        }
    }
}