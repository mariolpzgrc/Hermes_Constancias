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
    public class IndexModel : PageModel
    {
        private readonly IPlantillaService _plantillaService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public IndexModel(IPlantillaService plantillaService, IUsuarioClaimService usuarioClaimService)
        {
            _plantillaService = plantillaService;
            _usuarioClaimService = usuarioClaimService;
        }

        public List<PlantillaSimplificadaViewModel> Listado { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool ActivaDelegacion { get; set; }
        [BindProperty]
        [HiddenInput]
        public int BandejaPermiso { get; set; }

        public async Task OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            ActivaDelegacion = infoUsuario.ActivaDelegacion;
            BandejaPermiso = infoUsuario.BandejaPermiso;

            Listado = await _plantillaService.ObtenerPlantillasAsync(infoUsuario.BandejaUsuario);
        }
    }
}