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
using Newtonsoft.Json;

namespace Hermes2018.Areas.Identity.Pages.Administracion.Usuarios
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
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
        public AdminUsuariosCrearViewModel Crear { get; set; }

        [BindProperty]
        [HiddenInput]
        public List<AreaViewModel> ListaAreas { get; set; }

        [BindProperty]
        [HiddenInput]
        public int AreaId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            if (!infoUsuario.PermisoAA)
            {
                return NotFound();
            }

            AreaId = infoUsuario.AreaId;
            //ListaAreas = await _areaService.ObtenerAreasVisiblesPorAreaPadreConHijasAsync(infoUsuario.AreaId);

            Crear = new AdminUsuariosCrearViewModel()
            {
                InfoUsuarioClaims = infoUsuario,
                RegionId = infoUsuario.RegionId.ToString(),
                TipoVista = ConstTipoHttp.ConstTipoVistaHttpN1,
                Roles = new SelectList(await _rolService.ObtenerRolEnListaAsync(ConstRol.Rol8T), "HER_Nombre", "HER_Nombre"),
                EsTitular = true
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = false;
                var titularExiste = false;

                //Valida si el nombre del usuario contiene una �
                if (Crear.NombreUsuario.Contains("�"))
                {
                    Crear.NombreUsuario = Crear.NombreUsuario.Replace("�", "n");
                }

                //Valida si ya el usuario se encuentra registrado
                if (!await _usuarioService.ExisteUsuarioActivoAsync(Crear.NombreUsuario))
                {
                    if (ConstRol.RolUsuario.Contains(Crear.Rol))
                    {
                        if (ConstRol.Rol7T == Crear.Rol)
                        {
                            titularExiste = await _usuarioService.ExisteUsuarioTitularAsync(int.Parse(Crear.AreaId));

                            if (titularExiste)
                            {
                                ModelState.AddModelError(string.Empty, "Ya existe un titular para el �rea especificada.");
                            }
                        }
                    }

                    if (!titularExiste)
                    {
                        result = await _usuarioService.AdminGuardarUsuarioAsync(Crear);

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
                                    Crear.Puesto);

                                await _mailService.EnviarCorreo(new string[] { Crear.Correo }, null, null, ConstPlantillaCorreo.AsuntoT2, cuerpo);
                            }

                            return RedirectToPage("/Administracion/Usuarios/Index", new { area = "Identity" });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error int�ntelo m�s tarde.");
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
                ModelState.AddModelError(string.Empty, "La informaci�n registrada contiene elementos no v�lidos. Por favor rev�sela.");
            }


            //Carga la informaci�n
            Crear.InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            Crear.TipoVista = ConstTipoHttp.ConstTipoVistaHttpN2;
            //--
            //Roles
            Crear.Roles = new SelectList(await _rolService.ObtenerRolEnListaAsync(ConstRol.Rol8T), "HER_Nombre", "HER_Nombre");

            //ListaAreas = await _areaService.ObtenerAreasVisiblesPorAreaPadreConHijasAsync(Crear.InfoUsuarioClaims.AreaId);

            return Page();
        }
    }
}
