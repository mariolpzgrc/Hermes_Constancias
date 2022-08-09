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

namespace Hermes2018.Areas.Identity.Pages.Areas
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
    public class DetectarModel : PageModel
    {
        private readonly IAreaService _areaService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IOracleService _oracleService;

        public DetectarModel(IAreaService areaService, 
            IUsuarioClaimService usuarioClaimService,
            IOracleService oracleService)
        {
            _areaService = areaService;
            _usuarioClaimService = usuarioClaimService;
            _oracleService = oracleService;
        }

        public List<DetectarAreaViewModelDBF> Listado { get; set; }
        public bool EsAdminGral { get; set; }
        public int RegionId { get; set; }
        public int AreaId { get; set; }

        public int? AreaPadreId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int regionId, int? areaPadreId)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            RegionId = regionId;
            AreaId = id;
            AreaPadreId = areaPadreId;

            if (RegionId < 1 && RegionId > 5)
                return NotFound();

            if (!await _areaService.ExisteAreaEnSIIU(AreaId))
                return NotFound();

            if (ConstRol.RolAdminGral.Contains(infoUsuario.Rol))
            {
                EsAdminGral = true;
            }
            else if (ConstRol.RolAdminRegional.Contains(infoUsuario.Rol))
            {
                EsAdminGral = false;

                if (RegionId != infoUsuario.RegionId)
                {
                    RegionId = infoUsuario.RegionId;
                }
            }

            Listado = await _oracleService.ObtenerAreasAsync(AreaId);

            return Page();
        }
    }
}
