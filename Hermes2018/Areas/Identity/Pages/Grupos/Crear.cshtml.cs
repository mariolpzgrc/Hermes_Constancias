using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Grupo;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Grupos
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class CrearModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly IGrupoService _grupoService;

        public CrearModel(IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService,
            IGrupoService grupoService)
        {
            _usuarioService = usuarioService;
            _usuarioClaimService = usuarioClaimService;
            _grupoService = grupoService;
        }

        [BindProperty]
        public CrearGrupoViewModel Crear { get; set; }

        public IActionResult OnGet()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToPage("/Grupos/Index", new { area = "Identity", id = "" });
            }
            //--
            var crearView = new CrearGrupoViewModel()
            {
                Integrantes = new List<IntegranteViewModel>()
            };
            Crear = crearView;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
                //Usuario actual
                var infoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(infoUsuarioClaims.BandejaUsuario);
                //Integrantes
                var integrantes = new List<HER_GrupoIntegrante>();
                //Valida que no exista un grupo con el mismo nombre
                var existe = await _grupoService.ExisteGrupoAsync(Crear.NombreGrupo, infoUsuarioId);
                //-
                var result = false;
                if (!existe)
                {
                    //No hay un grupo con ese nombre
                    result =  await _grupoService.GuardarGrupoAsync(Crear.NombreGrupo, infoUsuarioId, Crear.Integrantes);

                    if (result)
                    {
                        return RedirectToPage("/Grupos/Index");
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "El nombre que usted asigno a este grupo ya se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}