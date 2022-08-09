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

namespace Hermes2018.Areas.Identity.Pages.Usuarios
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)]
    public class BuscarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;

        public BuscarModel(IUsuarioClaimService usuarioClaimService)
        {
            _usuarioClaimService = usuarioClaimService;
        }

        public UsuariosBuscarViewModel Info { get; set; }

        public void OnGet()
        {
            Info = new UsuariosBuscarViewModel()
            {
                InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User)
            };
        }
    }
}
