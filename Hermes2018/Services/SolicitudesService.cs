using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Configuracion;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class SolicitudesService: ISolicitudesService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<HER_Usuario> _userManager;

        public SolicitudesService(ApplicationDbContext context,
            UserManager<HER_Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<List<SolicitudesViewModel>> ObtenerSolicitudesAsync(int areaId)
        {
            var solicitudQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Usuario.HER_Aprobado == false 
                         && x.HER_Activo == false
                         && x.HER_EstaEnReasignacion == false
                         && x.HER_EstaEnBajaDefinitiva == false
                         && x.HER_AreaId == areaId)
                //.Include(x => x.HER_Area)
                //    .ThenInclude(x => x.HER_Region)         
                .AsNoTracking()
                .Select(x => new SolicitudesViewModel()
                {
                    Nombre = x.HER_NombreCompleto,
                    NombreUsuario = x.HER_UserName,
                    Correo = x.HER_Correo,
                    Rol = x.HER_RolNombre,
                    Region = x.HER_Area.HER_Region.HER_Nombre,
                    Area = x.HER_Area.HER_Nombre,
                    Puesto = x.HER_Puesto
                })
                .AsQueryable();

            return await solicitudQuery.ToListAsync();
        }
        public async Task<SolicitudDetalleViewModel> ObtenerDetalleSolicitudAsync(string username, int areaId)
        {
            var usuarioQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Activo == false
                         && x.HER_Usuario.HER_Aprobado == false
                         && x.HER_EstaEnReasignacion == false
                         && x.HER_EstaEnBajaDefinitiva == false
                         && x.HER_Usuario.UserName == username 
                         && x.HER_AreaId == areaId)
                //.Include(x => x.HER_Usuario)
                //.Include(x => x.HER_Area).ThenInclude(x => x.HER_Region)         
                .AsNoTracking()
                .Select(x => new SolicitudDetalleViewModel()
                {
                    Nombre = x.HER_NombreCompleto,
                    NombreUsuario = x.HER_UserName,
                    Correo = x.HER_Correo,
                    RegionNombre = x.HER_Area.HER_Region.HER_Nombre,
                    AreaNombre = x.HER_Area.HER_Nombre,
                    PuestoNombre = x.HER_Puesto,
                    EsUnico = x.HER_EsUnico? "Si" : "No",
                    RolNombre = x.HER_RolNombre,
                    EstaAprobado = (x.HER_Usuario.HER_Aprobado) ? ConstAprobado.AprobadoSi : ConstAprobado.AprobadoNo,
                    FechaRegistro = x.HER_FechaRegistro
                })
                .AsQueryable();

            return await usuarioQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ExisteSolicitudUsuarioAsync(string username, int areaId)
        {
            var usuarioQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Activo == false
                         && x.HER_Usuario.HER_Aprobado == false
                         && x.HER_EstaEnReasignacion == false
                         && x.HER_EstaEnBajaDefinitiva == false
                         && x.HER_Usuario.UserName == username
                         && x.HER_AreaId == areaId)
                .AsNoTracking()
                .AsQueryable();

            return await usuarioQuery.AnyAsync();
        }
        public async Task<SolicitudResponderViewModel> ObtenerResumenResponderAsync(string username, int areaId)
        {
            var usuarioQuery = _context.HER_InfoUsuario
                //.Include(x => x.HER_Area)
                //    .ThenInclude(x => x.HER_Region)
                .Where(x => x.HER_Activo == false
                         && x.HER_Usuario.HER_Aprobado == false
                         && x.HER_EstaEnReasignacion == false
                         && x.HER_EstaEnBajaDefinitiva == false
                         && x.HER_Usuario.UserName == username
                         && x.HER_AreaId == areaId)
                .AsNoTracking()
                .Select(x => new SolicitudResponderViewModel()
                {
                    AreaId = x.HER_AreaId.ToString(),
                    Usuario = x.HER_UserName,
                    Nombre = x.HER_NombreCompleto,
                    Correo = x.HER_Correo,
                    Direccion = x.HER_Area.HER_Direccion,
                    Telefono = x.HER_Area.HER_Telefono,
                    //--
                    Aprobar = null,
                    Comentario = null,
                    //--
                    Puesto = x.HER_Puesto,
                })
                .AsQueryable();

            return await usuarioQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ResponderSolicitudAsync(SolicitudResponderViewModel viewModel)
        {
            var usuarioQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_UserName == viewModel.Usuario 
                         && x.HER_Usuario.HER_Aprobado == false
                         && x.HER_Activo == false)
                .Include(x => x.HER_Usuario)
                .AsQueryable();

            var infoUsuario = await usuarioQuery.FirstOrDefaultAsync();
            var usuario = infoUsuario.HER_Usuario;

            int result = 0;

            if (int.Parse(viewModel.Aprobar) == ConstAprobado.AprobadoSiN)
            {
                //Aprobado
                usuario.HER_Aprobado = true;
                usuario.HER_FechaAprobado = DateTime.Now;
                //--
                await _userManager.UpdateAsync(usuario);

                if (infoUsuario != null)
                {
                    infoUsuario.HER_Puesto = viewModel.Puesto;
                    infoUsuario.HER_EsUnico = viewModel.EsUnico;
                    infoUsuario.HER_Direccion = viewModel.Direccion;
                    infoUsuario.HER_Telefono = viewModel.Telefono;

                    infoUsuario.HER_Activo = true;
                    infoUsuario.HER_EstaEnReasignacion = false;
                    infoUsuario.HER_EstaEnBajaDefinitiva = false;
                    //--
                    infoUsuario.HER_BandejaUsuario = usuario.UserName;
                    infoUsuario.HER_BandejaPermiso = ConstDelegar.TipoN1;
                    infoUsuario.HER_BandejaNombre = usuario.HER_NombreCompleto;
                    //---
                    _context.HER_InfoUsuario.Update(infoUsuario).State = EntityState.Modified;
                    //--
                    //Agregar configuración usuario
                    var config = new HER_ConfiguracionUsuario
                    {
                        HER_UsuarioId = infoUsuario.HER_UsuarioId,
                        HER_Notificaciones = false,
                        HER_ElementosPorPagina = 25,
                        HER_ColorId = _context.HER_Color.Where(x => x.HER_Nombre == ConstColor.ColorT1).Select(x => x.HER_ColorId).FirstOrDefault()
                    };
                    _context.HER_ConfiguracionUsuario.Add(config);
                    //--
                    result = await _context.SaveChangesAsync();
                }
            }
            else if (int.Parse(viewModel.Aprobar) == ConstAprobado.AprobadoNoN)
            {
                //No aprobado
                if (infoUsuario != null)
                {
                    //Borra el infousuario
                    _context.HER_InfoUsuario.Remove(infoUsuario);
                    result = await _context.SaveChangesAsync();
                }
                //Borra el usuario
                await _userManager.DeleteAsync(usuario);
            }

            return (result > 0) ? true : false;
        }
    }
}
