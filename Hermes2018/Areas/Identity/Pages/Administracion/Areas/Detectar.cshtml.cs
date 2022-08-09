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
    public class DetectarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IOracleService _oracleService;
        private readonly IAreaService _areaService;

        public DetectarModel(IUsuarioClaimService usuarioClaimService, 
            IOracleService oracleService,
            IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _oracleService = oracleService;
            _areaService = areaService;
        }

        public List<DetectarAreaViewModelDBF> Listado { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }

            if (!await _areaService.ExisteAreaEnSIIU(infoUsuario.AreaId)) 
            {
                return NotFound();
            }

            Listado = await _oracleService.ObtenerAreasAsync(infoUsuario.AreaClave, infoUsuario.AreaId);

            return Page();
        }
    }
}
