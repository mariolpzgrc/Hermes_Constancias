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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hermes2018.Areas.Identity.Pages.Usuarios
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)] //+ "," + ConstRol.Rol7T + "," + ConstRol.Rol8T
    public class CrearModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IRolService _rolService;
        private readonly IRegionService _regionService;
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguracionService _configuracionService;
        private readonly IAreaService _areaService;
        private readonly IMailService _mailService;

        public CrearModel(IUsuarioClaimService usuarioClaimService, 
            IRolService rolService,
            IRegionService regionService,
            IUsuarioService usuarioService,
            IConfiguracionService configuracionService,
            IAreaService areaService,
            IMailService mailService)
        {
            _usuarioClaimService = usuarioClaimService;
            _rolService = rolService;
            _regionService = regionService;
            _usuarioService = usuarioService;
            _configuracionService = configuracionService;
            _areaService = areaService;
            _mailService = mailService;
        }

        [BindProperty]
        public UsuariosCrearViewModel Crear { get; set; }

        public async Task OnGetAsync()
        {
            Crear = new UsuariosCrearViewModel()
            {
                InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User),
                TipoVista = ConstTipoHttp.ConstTipoVistaHttpN1
            };

            if (ConstRol.RolAdminGral.Contains(Crear.InfoUsuarioClaims.Rol))
            {
                //Roles
                Crear.Roles = new SelectList(await _rolService.ObtenerRolUsuariosNormalesAsync(), "HER_Nombre", "HER_Nombre");
                //Regiones
                Crear.Regiones = new SelectList(await _regionService.ObtenerRegionesAsync(), "HER_RegionId", "HER_Nombre");
            }
            else if (ConstRol.RolAdminRegional.Contains(Crear.InfoUsuarioClaims.Rol))
            {
                //Roles
                Crear.Roles = new SelectList(await _rolService.ObtenerRolUsuariosNormalesAsync(), "HER_Nombre", "HER_Nombre");
                //Regiones
                Crear.Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(Crear.InfoUsuarioClaims.RegionId), "HER_RegionId", "HER_Nombre");
            }
            else if (ConstRol.RolUsuario.Contains(Crear.InfoUsuarioClaims.Rol))
            {
                //Roles
                Crear.Roles = new SelectList(await _rolService.ObtenerRolEnListaAsync(ConstRol.Rol8T), "HER_Nombre", "HER_Nombre");
                //Regiones
                Crear.Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(Crear.InfoUsuarioClaims.RegionId), "HER_RegionId", "HER_Nombre");
            }

            Crear.EsTitular = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;
                var titularExiste = false;

                //Valida si el nombre del usuario contiene una ñ
                if (Crear.NombreUsuario.Contains("ñ")) {
                    Crear.NombreUsuario = Crear.NombreUsuario.Replace("ñ", "n");           
                }

                //Valida si ya el usuario se encuentra registrado
                if (!await _usuarioService.ExisteUsuarioActivoAsync(Crear.NombreUsuario))
                {
                    if (ConstRol.RolUsuario.Contains(Crear.Rol))
                    {
                        if (ConstRol.Rol7T.Contains(Crear.Rol))
                        {
                            titularExiste = await _usuarioService.ExisteUsuarioTitularAsync(int.Parse(Crear.AreaId));

                            if (titularExiste)
                            {
                                ModelState.AddModelError(string.Empty, "Ya existe un titular para el área especificada.");
                            }
                        }
                    }

                    if (!titularExiste)
                    {
                        result = await _usuarioService.GuardarUsuarioAsync(Crear);

                        if (result)
                        {
                            if (ConstRol.RolUsuario.Contains(Crear.Rol))
                            {
                                //--Enviar correo 
                                var cuerpo = string.Format(_configuracionService.ObtenerPlantillaRegistroUsuario(),
                                   Crear.Nombre,
                                   Crear.NombreUsuario,
                                   Crear.Correo,
                                   Crear.Direccion,
                                   Crear.Telefono,
                                   await _regionService.ObtenerNombreRegionAsync(int.Parse(Crear.RegionId)),
                                   await _areaService.ObtenerNombreAreaVisibleAsync(int.Parse(Crear.AreaId)),
                                   Crear.Puesto); //puesto

                                await _mailService.EnviarCorreo(new string[] { Crear.Correo }, null, null, ConstPlantillaCorreo.AsuntoT2, cuerpo);
                            }

                            return RedirectToPage("/Usuarios/Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El usuario que intenta registrar ya se encuentra registrado.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "La información registrada contiene elementos no válidos. Por favor revísela.");
            }

            //Carga la información
            Crear.InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            Crear.TipoVista = ConstTipoHttp.ConstTipoVistaHttpN2;
            //--
            Crear.Areas = new SelectList(await _areaService.ObtenerAreasVisibleAsync(Int32.Parse(Crear.RegionId)), "HER_AreaId", "HER_Nombre");

            if (ConstRol.RolAdminGral.Contains(Crear.InfoUsuarioClaims.Rol))
            {
                //Roles
                Crear.Roles = new SelectList(await _rolService.ObtenerRolUsuariosNormalesAsync(), "HER_Nombre", "HER_Nombre");
                //Regiones
                Crear.Regiones = new SelectList(await _regionService.ObtenerRegionesAsync(), "HER_RegionId", "HER_Nombre");
            }
            else if (ConstRol.RolAdminRegional.Contains(Crear.InfoUsuarioClaims.Rol))
            {
                //Roles
                Crear.Roles = new SelectList(await _rolService.ObtenerRolUsuariosNormalesAsync(), "HER_Nombre", "HER_Nombre");
                //Regiones
                Crear.Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(Crear.InfoUsuarioClaims.RegionId), "HER_RegionId", "HER_Nombre");
            }
            else if (ConstRol.RolUsuario.Contains(Crear.InfoUsuarioClaims.Rol))
            {
                //Roles
                Crear.Roles = new SelectList(await _rolService.ObtenerRolEnListaAsync(ConstRol.Rol8T), "HER_Nombre", "HER_Nombre");
                //Regiones
                Crear.Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(Crear.InfoUsuarioClaims.RegionId), "HER_RegionId", "HER_Nombre");
            }

            return Page();
        }
    }
}
