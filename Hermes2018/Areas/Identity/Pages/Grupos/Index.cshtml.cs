using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Grupos
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class IndexModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IGrupoService _grupoService;

        public IndexModel(IUsuarioClaimService usuarioClaimService,
            IGrupoService grupoService)
        {
            _usuarioClaimService = usuarioClaimService;
            _grupoService = grupoService;
        }

        public IList<GruposViewModel> Grupos { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool ActivaDelegacion { get; set; }

        [BindProperty]
        [HiddenInput]
        public int BandejaPermiso { get; set; }

        public void OnGet()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            ActivaDelegacion = infoUsuarioClaims.ActivaDelegacion;
            BandejaPermiso = infoUsuarioClaims.BandejaPermiso;
                
            Grupos = _grupoService.ObtenerGrupos(infoUsuarioClaims.BandejaUsuario);
        }
    }
}