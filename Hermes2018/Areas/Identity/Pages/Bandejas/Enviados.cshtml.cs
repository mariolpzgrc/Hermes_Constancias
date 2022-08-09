using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Bandejas
{
    [Authorize]
    public class EnviadosModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;

        public EnviadosModel(IUsuarioClaimService usuarioClaimService)
        {
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public InfoDelegarUsuarioViewModel InfoDelegar { get; set; }

        public void OnGet()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            InfoDelegar = new InfoDelegarUsuarioViewModel()
            {
                EstaActiva = infoUsuario.ActivaDelegacion,
                TipoPermiso = (infoUsuario.ActivaDelegacion) ? infoUsuario.BandejaPermiso : 0
            };

            ViewData["Bandeja"] = "Enviados";
        }
    }
}