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

namespace Hermes2018.Areas.Identity.Pages.Recopilacion.Region
{
    [Authorize(Roles = ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
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

        public RecopilacionRegionViewModel Region { get; set; }
        public string RegionNombre { get; set; }

        public async Task OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            
            RegionNombre = infoUsuario.Region;
            Region = await _recopilacionService.ObtenerRecopilacionPorRegionAsync(infoUsuario.RegionId);
        }
    }
}
