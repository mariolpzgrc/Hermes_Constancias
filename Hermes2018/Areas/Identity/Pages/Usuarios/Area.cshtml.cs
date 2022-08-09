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
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)] //+ "," + ConstRol.Rol7T + "," + ConstRol.Rol8T
    public class AreaModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IAreaService _areaService;
        private readonly IUsuarioService _usuarioService;

        public AreaModel(IUsuarioClaimService usuarioClaimService,
            IAreaService areaService,
            IUsuarioService usuarioService)
        {
            _usuarioClaimService = usuarioClaimService;
            _areaService = areaService;
            _usuarioService = usuarioService;
        }

        public UsuariosAreaViewModel Info { get; set; }

        public async Task<ActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HER_Area area = null;

            //Configuración inicial (Información base del usuario)
            Info = new UsuariosAreaViewModel
            {
                InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User)
            };

            //Validación de acuerdo al rol
            if (ConstRol.RolAdminGral.Contains(Info.InfoUsuarioClaims.Rol))
            {
                //Cuando accede el Administrador General
                area = await _areaService.ObtenerAreaConRegionPorIdAsync((int)id);
            }
            if (ConstRol.RolAdminRegional.Contains(Info.InfoUsuarioClaims.Rol))
            {
                //Cuando accede el Administrador Regional
                area = await _areaService.ObtenerAreaConRegionPorIdPorRegionAsync((int)id, Info.InfoUsuarioClaims.RegionId);
            }
            else if (ConstRol.RolUsuario.Contains(Info.InfoUsuarioClaims.Rol))
            {
                //Cuando acccede el Titular de un área
                area = await _areaService.ObtenerAreaConRegionPorIdAsync(Info.InfoUsuarioClaims.AreaId);
            }

            //Valida si el area es null
            if (area == null)
            {
                return NotFound();
            }

            //Se agregan los usuarios
            Info.Usuarios = await _usuarioService.ObtenerUsuariosPorAreaAsync(area.HER_AreaId);
            Info.AreaPadreId = area.HER_Area_PadreId;
            Info.Area = area.HER_Nombre;
            Info.AreaId = area.HER_AreaId;
            Info.Region = area.HER_Region.HER_Nombre;
            Info.RegionId = area.HER_RegionId;

            return Page();
        }
    }
}
