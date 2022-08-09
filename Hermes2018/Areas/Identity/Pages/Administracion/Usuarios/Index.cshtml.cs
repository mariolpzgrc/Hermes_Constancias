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

namespace Hermes2018.Areas.Identity.Pages.Administracion.Usuarios
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class IndexModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IAreaService _areaService;

        public IndexModel(IUsuarioClaimService usuarioClaimService, 
            IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _areaService = areaService;
        }

        [BindProperty]
        public List<FamiliaAreaViewModel> Listado { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (!infoUsuario.PermisoAA) 
            {
                return NotFound();
            }

            Listado = await _areaService.ObtenerFamiliaArea(infoUsuario.AreaId, infoUsuario.Area);

            return Page();
        }
    }
}
