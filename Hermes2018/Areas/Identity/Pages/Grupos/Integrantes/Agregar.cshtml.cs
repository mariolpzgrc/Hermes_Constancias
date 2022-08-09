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
    public class AgregarModel : PageModel
    {
        private IGrupoService _grupoService;
        private readonly IUsuarioClaimService _usuarioClaimService;

        public AgregarModel(IGrupoService grupoService,
            IUsuarioClaimService usuarioClaimService)
        {
            _grupoService = grupoService;
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public AgregarIntegranteGrupoViewModel Agregar { get; set; }

        [BindProperty]
        public int IdGrupo { get; set; }

        [BindProperty]
        public int ID { get; set; }

        public async Task OnGetAsync(int grupoid)
        {
            ID = grupoid;
            //Info del grupo actual
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            Agregar = await _grupoService.ObtenerGrupoIntegranteParaAgregar(grupoid);
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                //Info del usuario actual
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

                //Busca si ya esta registrado el integrante que se quiere agregar
                var existeIntegrante = await _grupoService.ExisteIntegranteAsync(Agregar.Usuario, Agregar.GrupoId);

                IdGrupo = Agregar.GrupoId;

                if (!existeIntegrante)
                {
                    var result = await _grupoService.AgregarIntegranteGrupoAsync(Agregar.Usuario, Agregar.GrupoId);
                    if (result)
                    {
                        return RedirectToPage("/Grupos/Detalles", new { id = IdGrupo });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El usuario que intenta registrar como integrante, ya se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}
