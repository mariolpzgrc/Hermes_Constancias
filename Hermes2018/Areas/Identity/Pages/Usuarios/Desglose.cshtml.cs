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
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Areas.Identity.Pages.Usuarios
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class DesgloseModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private ApplicationDbContext _context;

        public DesgloseModel(IUsuarioClaimService usuarioClaimService,
            ApplicationDbContext context)
        {
            _usuarioClaimService = usuarioClaimService;
            _context = context;
        }

        public UsuariosDesgloseViewModel Info { get; set; }

        public async Task OnGetAsync()
        {
            Info = new UsuariosDesgloseViewModel()
            {
                InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User),
                Regiones = await _context.HER_Region.ToListAsync()
            };
        }
    }
}
