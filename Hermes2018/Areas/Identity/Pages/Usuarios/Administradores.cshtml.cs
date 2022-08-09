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
    [Authorize(Roles = ConstRol.Rol1T)]
    public class AdministradoresModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public AdministradoresModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public List<UsuariosAdministradoresViewModel> Listado { get; set; }
        
        public void OnGet()
        {
            Listado = _usuarioService.ObtenerUsuariosAdministradores(User.Identity.Name);
        }
    }
}
