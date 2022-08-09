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

namespace Hermes2018.Areas.Identity.Pages.Subcarpetas
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class DetallesModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly ICarpetaService _carpetaService;

        public DetallesModel(IUsuarioClaimService usuarioClaimService, ICarpetaService carpetaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _carpetaService = carpetaService;
        }

        public DetallesCarpetaViewModel Detalles { get; set; }

        public async Task OnGetAsync(int id, int carpetaPadreId)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            Detalles = await _carpetaService.ObtenerDetallesCarpetaAsync(id, infoUsuarioClaims.BandejaUsuario);
        }
    }
}