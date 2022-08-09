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

namespace Hermes2018.Areas.Identity.Pages.Grupos
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class BorrarModelG : PageModel
    {
        private IGrupoService _grupoService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;

        public BorrarModelG(IGrupoService grupoService,
            IUsuarioService usuarioService,
            IUsuarioClaimService usuarioClaimService)
        {
            _usuarioService = usuarioService;
            _grupoService = grupoService;
            _usuarioClaimService = usuarioClaimService;
        }

        [BindProperty]
        public BorrarGrupoViewModel Borrar { get; set; }

        public async Task OnGetAsync(int id)
        {
            //Info del usuario actual
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            Borrar = await _grupoService.ObtenerGrupoParaBorrar(infoUsuarioClaims.UserName, id);
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            List<int> validarResultados = new List<int>();
            if (ModelState.IsValid)
            {
                //Info del usuario actual
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
                //Usuario actual
                var infoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(infoUsuarioClaims.BandejaUsuario);

                //Busca si ya esta registrado el grupo que se quiere agregar
                var existeGrupo = await _grupoService.ExisteGrupoAsync(Borrar.Nombre, infoUsuarioId);
                if (existeGrupo) 
                {
                    var tieneDocumentos = await _grupoService.GrupoTieneIntegrantesAsociados(infoUsuarioId, Borrar.GrupoId);
                    
                    var tieneIntegrantesInactivos = await _grupoService.GrupoTieneIntegrantesAsociadosInactivos(infoUsuarioId, Borrar.GrupoId);

                    if (!tieneDocumentos)
                    {
                        if (tieneIntegrantesInactivos)
                        {
                            List<BorrarIntegranteGrupoViewModel> listaIntegrantesInactivo = await _grupoService.ObtenerIntegrantesInactivoParaBorrar(Borrar.GrupoId);
                            int numeroIntegrantesA = listaIntegrantesInactivo.Count();

                            foreach (var item in listaIntegrantesInactivo) {
                                var resultDelete = await _grupoService.BorrarIntegranteAsync(item.Usuario, item);
                                if (resultDelete)
                                {
                                    validarResultados.Add(1);
                                }
                            }

                            if(validarResultados.Count() == numeroIntegrantesA)
                            {
                                var result = await _grupoService.BorrarGrupoAsync(infoUsuarioClaims.UserName, Borrar);
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
                                ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                            }
                        }
                        else
                        {
                            var result = await _grupoService.BorrarGrupoAsync(infoUsuarioClaims.UserName, Borrar);
                            if (result)
                            {
                                return RedirectToPage("/Grupos/Index");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                            }

                        }                     
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "El grupo que desea borrar tiene asociado uno o más integrantes.");
                    } 
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El grupo que intenta borrar, no se encuentra registrado.");
                }
            }
            return Page();
        }
    }
}
