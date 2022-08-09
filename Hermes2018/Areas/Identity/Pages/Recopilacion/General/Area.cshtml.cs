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

namespace Hermes2018.Areas.Identity.Pages.Recopilacion.General
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class AreaModel : PageModel
    {
        public readonly IRecopilacionService _recopilacionService;
        public readonly IAreaService _areaService;

        public AreaModel(IRecopilacionService recopilacionService, 
                IAreaService areaService)
        {
            _recopilacionService = recopilacionService;
            _areaService = areaService;
        }

        public List<RecopilacionAreaViewModel> Areas { get; set; }

        public int RegionId { get; set; }
        public int? AreaPadreId { get; set; }
        public HER_Area AreaPadre { get; set; }

        public int TipoAreaId { get; set; }

        public async Task OnGetAsync(int id, int? areaPadreId)
        {
            if (id < 1 && id > 5)
                id = 1;

            RegionId = id;
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

            Areas = await _recopilacionService.ObtenerRecopilacionAreasAsync(id, AreaPadreId);

            if (AreaPadreId != null)
                AreaPadre = await _areaService.ObtenerAreaPorIdAsync((int)AreaPadreId);
        }
    }
}
