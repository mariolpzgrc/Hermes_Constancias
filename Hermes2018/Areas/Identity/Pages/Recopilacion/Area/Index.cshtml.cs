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

namespace Hermes2018.Areas.Identity.Pages.Recopilacion.Area
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class IndexModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        public readonly IRecopilacionService _recopilacionService;

        public IndexModel(IUsuarioClaimService usuarioClaimService, 
            IRecopilacionService recopilacionService)
        {
            _usuarioClaimService = usuarioClaimService;
            _recopilacionService = recopilacionService;
        }

        public RecopilacionAreaViewModel Area { get; set; }

        public async Task OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            Area = await _recopilacionService.ObtenerRecopilacionPorAreaAsync(infoUsuario.AreaId);
        }
    }
}
