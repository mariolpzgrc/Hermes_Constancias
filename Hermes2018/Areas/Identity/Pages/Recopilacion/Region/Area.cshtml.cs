using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Models.Area;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Recopilacion.Region
{
    [Authorize(Roles = ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
    public class AreaModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        public readonly IRecopilacionService _recopilacionService;
        public readonly IAreaService _areaService;

        public AreaModel(IUsuarioClaimService usuarioClaimService,
            IRecopilacionService recopilacionService,
            IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _recopilacionService = recopilacionService;
            _areaService = areaService;
        }

        public List<RecopilacionAreaViewModel> Areas { get; set; }
        public string RegionNombre { get; set; }

        public int RegionId { get; set; }
        public int? AreaPadreId { get; set; }
        public HER_Area AreaPadre { get; set; }

        public int TipoAreaId { get; set; }

        public async Task OnGetAsync(int? areaPadreId)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            RegionId = infoUsuario.RegionId;
            RegionNombre = infoUsuario.Region;

            AreaPadreId = areaPadreId;

            if (AreaPadreId == null)
            {
                if (ConstRegion.RegionesIds.Contains(RegionId))
                    TipoAreaId = ConstTipoAreaEnRegion.TipoN1;
                else
                    TipoAreaId = ConstTipoAreaEnRegion.TipoN2;
            }
            else
            {
                if (ConstArea.AreasIds.Contains((int)AreaPadreId))
                    TipoAreaId = ConstTipoAreaEnRegion.TipoN1;
                else
                    TipoAreaId = ConstTipoAreaEnRegion.TipoN2;
            }

            Areas = await _recopilacionService.ObtenerRecopilacionAreasAsync(RegionId, AreaPadreId);

            if (AreaPadreId != null)
                AreaPadre = await _areaService.ObtenerAreaPorIdAsync((int)AreaPadreId);
        }
    }
}
