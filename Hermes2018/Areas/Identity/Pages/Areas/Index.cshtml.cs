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

namespace Hermes2018.Areas.Identity.Pages.Areas
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
    public class IndexModel : PageModel
    {
        private readonly IAreaService _areaService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public IndexModel(IAreaService areaService, IUsuarioClaimService usuarioClaimService)
        {
            _areaService = areaService;
            _usuarioClaimService = usuarioClaimService;
        }

        public List<ListadoAreaViewModel> Areas { get; set; }
        public bool EsAdminGral { get; set; }
        
        public int RegionId { get; set; }
        public int? AreaPadreId { get; set; }
        public HER_Area AreaPadre { get; set; }

        public int TipoAreaId { get; set; }

        public async Task OnGetAsync(int id, int? areaPadreId)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            RegionId = id;
            AreaPadreId = areaPadreId;
            EsAdminGral = ConstRol.RolAdminGral.Contains(infoUsuario.Rol);

            if (id < 1 && id > 5)
            {
                RegionId = 1;
            }

            if (ConstRol.RolAdminRegional.Contains(infoUsuario.Rol))
            {
                if (id != infoUsuario.RegionId)
                {
                    RegionId = infoUsuario.RegionId;
                }
            }

            if (AreaPadreId == null)
            {
                if (ConstRegion.RegionesIds.Contains(RegionId))
                    TipoAreaId = ConstTipoAreaEnRegion.TipoN1;
                else
                    TipoAreaId = ConstTipoAreaEnRegion.TipoN2;
            }
            else { 
                if(ConstArea.AreasIds.Contains((int)AreaPadreId))
                    TipoAreaId = ConstTipoAreaEnRegion.TipoN1;
                else
                    TipoAreaId = ConstTipoAreaEnRegion.TipoN2;
            }

            Areas = await _areaService.ObtenerListadoAreasDeAreaPadreAsync(RegionId, AreaPadreId);

            if(AreaPadreId != null)
                AreaPadre = await _areaService.ObtenerAreaPorIdAsync((int)AreaPadreId);

        }
    }
}