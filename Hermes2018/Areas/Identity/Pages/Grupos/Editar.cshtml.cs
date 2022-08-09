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
    public class EditarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly IGrupoService _grupoService;

        public EditarModel(IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService,
            IGrupoService grupoService)
        {
            _usuarioService = usuarioService;
            _usuarioClaimService = usuarioClaimService;
            _grupoService = grupoService;
        }

        [BindProperty]
        public EditarGrupoViewModel Editar { get; set; }

        public async Task OnGetAsync(int id)
        {
            //Info del grupo actual
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            Editar = await _grupoService.ObtenerGrupoParaEdicion(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                //Info del grupo actual
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
                var infoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(infoUsuarioClaims.BandejaUsuario);

                //Busca si ya esta registrado el grupo que se quiere agregar
                var existeGrupo = await _grupoService.ExisteGrupoAsync(Editar.Nombre, infoUsuarioId);
                if (!existeGrupo)
                {
                    var result = await _grupoService.ActualizarGrupoAsync(Editar);
                    if (result)
                    {
                        return RedirectToPage("/Grupos/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un grupo con este nombre.");
                }
            }

            return Page();
        }
    }
}