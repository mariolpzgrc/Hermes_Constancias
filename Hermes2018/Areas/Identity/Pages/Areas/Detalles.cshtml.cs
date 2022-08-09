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
    public class DetallesModel : PageModel
    {
        private readonly IAreaService _areaService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;

        public DetallesModel(IAreaService areaService, 
            IUsuarioClaimService usuarioClaimService, 
            IUsuarioService usuarioService)
        {
            _areaService = areaService;
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public DetalleAreaViewModel Detalle { get; set; }

        [BindProperty]
        public List<UsuariosPorAreaViewModel> Usuarios { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EsAdminGral { get; set; }

        [BindProperty]
        [HiddenInput]
        public int RegionId { get; set; }

        public int? AreaPadreId { get; set; }

        public async Task OnGetAsync(int id, int regionId, int? areaPadreId)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            RegionId = regionId;
            AreaPadreId = areaPadreId;
            EsAdminGral = ConstRol.RolAdminGral.Contains(infoUsuario.Rol);
            
            if (regionId < 1 && regionId > 5)
            {
                RegionId = 1;
            }
            
            if (ConstRol.RolAdminRegional.Contains(infoUsuario.Rol))
            {
                if (regionId != infoUsuario.RegionId)
                {
                    RegionId = infoUsuario.RegionId;
                }
            }
            //--

            Detalle = await _areaService.ObtenerDetallesArea(id, RegionId);
            Usuarios = await _usuarioService.ObtenerUsuariosPorAreaAsync(id);
        }
    }
}