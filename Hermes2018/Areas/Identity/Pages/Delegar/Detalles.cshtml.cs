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

namespace Hermes2018.Areas.Identity.Pages.Delegar
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class DetallesModel : PageModel
    {
        private IDelegarService _delegarService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public DetallesModel(IDelegarService delegarService,
            IUsuarioClaimService usuarioClaimService)
        {
            _delegarService = delegarService;
            _usuarioClaimService = usuarioClaimService;
        }

        public DelegadoDetalleViewModel Detalles { get; set; }

        public async Task OnGetAsync(int id)
        {
            Detalles = await  _delegarService.ObtenerDetalleDelegado(id);
        }
    }
}
