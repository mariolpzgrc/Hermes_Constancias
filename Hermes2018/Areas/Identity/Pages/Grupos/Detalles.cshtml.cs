using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Grupo;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Areas.Identity.Pages.Grupos
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class DetallesModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IGrupoService _grupoService;

        public DetallesModel(IUsuarioClaimService usuarioClaimService,
            IGrupoService grupoService)
        {
            _usuarioClaimService = usuarioClaimService;
            _grupoService = grupoService;
        }

        [BindProperty]
        public DetalleGrupoViewModel Detalle { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool ActivaDelegacion { get; set; }

        [BindProperty]
        [HiddenInput]
        public int BandejaPermiso { get; set; }

        public async Task OnGetAsync(int id)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (id > 0)
            {
                //Modelo Detalle
                Detalle = await _grupoService.ObtenerDetalleGrupoAsync(id);
            }
        }

    }
}