using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Servicios
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IServicioService _servicioService;

        public IndexModel(IUsuarioClaimService usuarioClaimService, 
            IServicioService servicioService)
        {
            _usuarioClaimService = usuarioClaimService;
            _servicioService = servicioService;
        }

        public IList<ServiciosViewModel> Servicios { get; set; }

        public async Task OnGetAsync()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            Servicios = await _servicioService.ObtenerListadoServiciosAsync(infoUsuarioClaims.UserName);
        }
    }
}