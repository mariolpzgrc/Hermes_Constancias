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
    public class DetallesModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IAreaService _areaService;
        private readonly IUsuarioService _usuarioService;

        public DetallesModel(IUsuarioClaimService usuarioClaimService,
            IAreaService areaService,
            IUsuarioService usuarioService)
        {
            _usuarioClaimService = usuarioClaimService;
            _areaService = areaService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public DetalleAreaViewModel Detalle { get; set; }

        [BindProperty]
        public List<UsuariosPorAreaViewModel> Usuarios { get; set; }

        public async Task OnGetAsync(int id)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            Detalle = await _areaService.ObtenerDetallesArea(id);
            Usuarios = await _usuarioService.ObtenerUsuariosPorAreaAsync(id);
        }
    }
}
