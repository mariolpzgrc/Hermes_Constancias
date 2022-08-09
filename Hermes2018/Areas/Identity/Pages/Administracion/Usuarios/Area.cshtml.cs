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

namespace Hermes2018.Areas.Identity.Pages.Administracion.Usuarios
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class AreaModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IAreaService _areaService;
        private readonly IUsuarioService _usuarioService;

        public AreaModel(IUsuarioClaimService usuarioClaimService, IAreaService areaService, IUsuarioService usuarioService)
        {
            _usuarioClaimService = usuarioClaimService;
            _areaService = areaService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public UsuariosAreaViewModel ViewModel { get; set; }

        [BindProperty]
        public int Tipo { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int tipo, int? areapadre)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }

            Tipo = tipo;
            bool esCorrecto;
            int areaId;

            if (tipo == ConstTipoArea.TipoN2)
            {
                esCorrecto = await _areaService.EsAreaHija((int)areapadre, id);
                areaId = id;
            }
            else {
                esCorrecto = true;
                areaId = infoUsuario.AreaId;
            }

            if (!esCorrecto) 
            {
                return NotFound();
            }

            ViewModel = new UsuariosAreaViewModel
            {
                InfoUsuarioClaims = infoUsuario
            };
            
            HER_Area area = await _areaService.ObtenerAreaConRegionPorIdAsync(areaId);
            if (area == null)
            {
                return NotFound();
            }

            ViewModel.Usuarios = await _usuarioService.ObtenerUsuariosPorAreaAsync(area.HER_AreaId);

            ViewModel.AreaPadreId = area.HER_Area_PadreId;
            ViewModel.Area = area.HER_Nombre;
            ViewModel.AreaId = area.HER_AreaId;
            ViewModel.Region = area.HER_Region.HER_Nombre;
            ViewModel.RegionId = area.HER_RegionId;

            return Page();
        }
    }
}
