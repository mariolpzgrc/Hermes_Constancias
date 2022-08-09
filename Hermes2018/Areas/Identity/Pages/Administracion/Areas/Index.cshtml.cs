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

namespace Hermes2018.Areas.Identity.Pages.Administracion.Areas
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
        public List<FamiliaAreaCompuestaViewModel> Listado { get; set; }

        [BindProperty]
        public bool ExisteEnSIIU { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }

            ExisteEnSIIU = await _areaService.ExisteAreaEnSIIU(infoUsuario.AreaId);
            Listado = await _areaService.ObtenerFamiliaAreaCompuesta(infoUsuario.AreaId);

            return Page();
        }
    }
}
