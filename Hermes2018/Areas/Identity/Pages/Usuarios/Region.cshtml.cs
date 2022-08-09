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

namespace Hermes2018.Areas.Identity.Pages.Usuarios
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
    public class RegionModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IRegionService _regionService;
        private readonly IAreaService _areaService;

        public RegionModel(IUsuarioClaimService usuarioClaimService, 
            IRegionService regionService,
            IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _regionService = regionService;
            _areaService = areaService;
        }

        public UsuariosRegionViewModel Info { get; set; }

        public async Task<ActionResult> OnGetAsync(int? id, int? areaPadreId)
        {
            if (id == null)
            {
                return NotFound();
            }

            int tipoAreaId;


            if (areaPadreId == null)
            {
                if (ConstRegion.RegionesIds.Contains((int)id))
                    tipoAreaId = ConstTipoAreaEnRegion.TipoN1;
                else
                    tipoAreaId = ConstTipoAreaEnRegion.TipoN2;
            }
            else
            {
                if (ConstArea.AreasIds.Contains((int)areaPadreId))
                    tipoAreaId = ConstTipoAreaEnRegion.TipoN1;
                else
                    tipoAreaId = ConstTipoAreaEnRegion.TipoN2;
            }

            //Configuración inicial (Información base del usuario)
            Info = new UsuariosRegionViewModel
            {
                TipoAreaId = tipoAreaId,
                InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User)
            };

            List<ListadoAreaPorRegionViewModel> areas = new List<ListadoAreaPorRegionViewModel>();
            HER_Region region = await _regionService.ObtenerRegionSinAreasAsync((int)id);

            //Valdia si la región es null
            if (region == null)
            {
                return NotFound();
            }

            //Area Padre
            Info.AreaPadre = (areaPadreId != null)? await _areaService.ObtenerAreaPorIdAsync((int)areaPadreId) : null;

            if (ConstRol.RolAdminGral.Contains(Info.InfoUsuarioClaims.Rol))
            {
                //Cuando accede el Administrador General
                //Busca la region para detectar si existe en la base
                areas = await _areaService.ObtenerListadoAreasDeAreaPadrePorRegiónAsync((int)id, areaPadreId);
            }
            else if (ConstRol.RolAdminRegional.Contains(Info.InfoUsuarioClaims.Rol))
            {
                //Cuando acccede el Administrador Regional
                //Busca la region para detectar si existe en la base
                areas = await _areaService.ObtenerListadoAreasDeAreaPadrePorRegiónAsync(Info.InfoUsuarioClaims.RegionId, areaPadreId);
            }

            //Busca las areas de esa región
            Info.Region = region;
            Info.Areas = areas;

            return Page();
        }
    }
}