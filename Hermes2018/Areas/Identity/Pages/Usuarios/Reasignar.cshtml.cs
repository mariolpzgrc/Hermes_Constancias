using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Areas.Identity.Pages.Usuarios
{
    [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T)] //+ "," + ConstRol.Rol7T + "," + ConstRol.Rol8T
    public class ReasignarModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IRolService _rolService;
        private readonly IRegionService _regionService;
        private ApplicationDbContext _context;
        private readonly UserManager<HER_Usuario> _userManager;
        private readonly IAreaService _areaService;

        public ReasignarModel(IUsuarioClaimService usuarioClaimService,
            IRolService rolService,
            IRegionService regionService,
            ApplicationDbContext context,
            UserManager<HER_Usuario> userManager,
            IAreaService areaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _rolService = rolService;
            _regionService = regionService;
            _context = context;
            _userManager = userManager;
            _areaService = areaService;
        }

        [BindProperty]
        public ReasignarUsuarioViewModel Reasignar { get; set; }

        public async Task OnGetAsync()
        {
            Reasignar = new ReasignarUsuarioViewModel()
            {
                InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User),
                TipoVista = ConstTipoHttp.ConstTipoVistaHttpN1
            };

            if (ConstRol.RolAdminGral.Contains(Reasignar.InfoUsuarioClaims.Rol))
            {
                //Roles
                Reasignar.Roles = new SelectList(await _rolService.ObtenerRolUsuariosNormalesAsync(), "HER_Nombre", "HER_Nombre");

                //Regiones
                Reasignar.Regiones = new SelectList(await _regionService.ObtenerRegionesAsync(), "HER_RegionId", "HER_Nombre");
            }
            else if (ConstRol.RolAdminRegional.Contains(Reasignar.InfoUsuarioClaims.Rol))
            {
                //Roles
                Reasignar.Roles = new SelectList(await _rolService.ObtenerRolUsuariosNormalesAsync(), "HER_Nombre", "HER_Nombre");

                //Regiones
                Reasignar.Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(Reasignar.InfoUsuarioClaims.RegionId), "HER_RegionId", "HER_Nombre");
            }
            else if (ConstRol.RolUsuario.Contains(Reasignar.InfoUsuarioClaims.Rol))
            {
                //Roles
                Reasignar.Roles = new SelectList(await _rolService.ObtenerRolEnListaAsync(ConstRol.Rol8T), "HER_Nombre", "HER_Nombre");

                //Regiones
                Reasignar.Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(Reasignar.InfoUsuarioClaims.RegionId), "HER_RegionId", "HER_Nombre");
            }

            Reasignar.EsTitular = true;
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (ModelState.IsValid)
            {
                //Buscar usuario
                var usuario = await _context.HER_Usuario
                    .Where(x => x.UserName == Reasignar.NombreUsuario)
                    .FirstOrDefaultAsync();

                if (usuario != null)
                {
                    //Cambiar el rol
                    var roles = await _userManager.GetRolesAsync(usuario);
                    await _userManager.RemoveFromRolesAsync(usuario, roles);
                    await _userManager.AddToRoleAsync(usuario, Reasignar.Rol);
                    //--
                    usuario.Email = Reasignar.Correo;
                    usuario.NormalizedEmail = Reasignar.Correo.ToUpper();
                    usuario.HER_NombreCompleto = Reasignar.Nombre;
                    usuario.HER_AceptoTerminos = false;
                    //--
                    await _userManager.UpdateAsync(usuario);

                    //Actualizar
                    var infoUsuario = await _context.HER_InfoUsuario
                        .Where(x => x.HER_UsuarioId == usuario.Id
                                 && x.HER_Activo == true
                                 && x.HER_EstaEnReasignacion == true)
                        .FirstOrDefaultAsync();

                    infoUsuario.HER_Activo = false;
                    infoUsuario.HER_EstaEnReasignacion = false;
                    infoUsuario.HER_FechaActualizacion = DateTime.Now;
                    infoUsuario.HER_PermisoAA = false;

                    _context.HER_InfoUsuario.Update(infoUsuario).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    //Guardando info usuario
                    var nuevoInfoUsuario = new HER_InfoUsuario()
                    {
                        HER_NombreCompleto = Reasignar.Nombre,
                        HER_Correo = Reasignar.Correo,
                        HER_UserName = Reasignar.NombreUsuario,
                        HER_Activo = true,
                        HER_EstaEnReasignacion = false,
                        HER_EstaEnBajaDefinitiva = false,
                        HER_FechaRegistro = DateTime.Now,
                        HER_FechaActualizacion = DateTime.Now,
                        HER_Direccion = Reasignar.Direccion,
                        HER_Telefono = Reasignar.Telefono,
                        HER_AreaId = int.Parse(Reasignar.AreaId),
                        HER_Puesto = Reasignar.Puesto,
                        HER_EsUnico = Reasignar.EsUnico,
                        HER_RolNombre = Reasignar.Rol,
                        HER_Titular = (Reasignar.EsTitular) ? Reasignar.NombreUsuario : Reasignar.Titular,
                        HER_UsuarioId = usuario.Id,
                        HER_BandejaPermiso = ConstDelegar.TipoN1,
                        HER_BandejaUsuario = Reasignar.NombreUsuario,
                        HER_BandejaNombre = Reasignar.Nombre,
                        HER_PermisoAA = Reasignar.Permiso
                    };

                    _context.HER_InfoUsuario.Add(nuevoInfoUsuario);
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("/Usuarios/Index");
                //return RedirectToAction(nameof(Index));
            }

            //----
            Reasignar.InfoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            Reasignar.TipoVista = ConstTipoHttp.ConstTipoVistaHttpN2;
            //----
            Reasignar.Areas = new SelectList(await _areaService.ObtenerAreasVisibleAsync(Int32.Parse(Reasignar.RegionId)), "HER_AreaId", "HER_Nombre");
            //--
            if (ConstRol.RolAdminGral.Contains(Reasignar.InfoUsuarioClaims.Rol))
            {
                //Roles
                Reasignar.Roles = new SelectList(await _rolService.ObtenerRolUsuariosNormalesAsync(), "HER_Nombre", "HER_Nombre");

                //Regiones
                Reasignar.Regiones = new SelectList(await _regionService.ObtenerRegionesAsync(), "HER_RegionId", "HER_Nombre");
            }
            else if (ConstRol.RolAdminRegional.Contains(Reasignar.InfoUsuarioClaims.Rol))
            {
                //Roles
                Reasignar.Roles = new SelectList(await _rolService.ObtenerRolUsuariosNormalesAsync(), "HER_Nombre", "HER_Nombre");

                //Regiones
                Reasignar.Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(Reasignar.InfoUsuarioClaims.RegionId), "HER_RegionId", "HER_Nombre");
            }
            else if (ConstRol.RolUsuario.Contains(Reasignar.InfoUsuarioClaims.Rol))
            {
                //Roles
                Reasignar.Roles = new SelectList(await _rolService.ObtenerRolEnListaAsync(ConstRol.Rol8T), "HER_Nombre", "HER_Nombre");

                //Regiones
                Reasignar.Regiones = new SelectList(await _regionService.ObtenerRegionEnListaAsync(Reasignar.InfoUsuarioClaims.RegionId), "HER_RegionId", "HER_Nombre");
            }
            //--

            return Page();
        }
    }
}
