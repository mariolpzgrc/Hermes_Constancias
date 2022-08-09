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

namespace Hermes2018.Areas.Identity.Pages.Plantillas
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class DetallesModel : PageModel
    {
        private readonly IPlantillaService _plantillaService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public DetallesModel(IPlantillaService plantillaService, IUsuarioClaimService usuarioClaimService)
        {
            _plantillaService = plantillaService;
            _usuarioClaimService = usuarioClaimService;
        }

        public PlantillaDetalleViewModel Detalles { get; set; }

        public async Task OnGetAsync(int id)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            Detalles = await _plantillaService.ObtenerDetallePlantillaAsync(id, infoUsuario.BandejaUsuario);
        }
    }
}