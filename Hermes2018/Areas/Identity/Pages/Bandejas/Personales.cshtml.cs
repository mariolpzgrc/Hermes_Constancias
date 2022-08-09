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

namespace Hermes2018.Areas.Identity.Pages.Bandejas
{
    [Authorize]
    public class PersonalesModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;

        public PersonalesModel(IUsuarioClaimService usuarioClaimService)
        {
            _usuarioClaimService = usuarioClaimService;
        }

        [HiddenInput]
        public int CarpetaId { get; set; }

        [HiddenInput]
        public int TipoBandeja { get; set; }

        [BindProperty]
        public InfoDelegarUsuarioViewModel InfoDelegar { get; set; }

        public void OnGet(int id, int tipo)
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            InfoDelegar = new InfoDelegarUsuarioViewModel()
            {
                EstaActiva = infoUsuario.ActivaDelegacion,
                TipoPermiso = (infoUsuario.ActivaDelegacion) ? infoUsuario.BandejaPermiso : 0
            };
            //--
            CarpetaId = id;
            TipoBandeja = (tipo == ConstBandejas.ConstTipoN1)? ConstBandejas.ConstTipoN1 : (tipo == ConstBandejas.ConstTipoN2)? ConstBandejas.ConstTipoN2 : ConstBandejas.ConstTipoN1;
        }
    }
}