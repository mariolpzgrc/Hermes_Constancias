using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.ViewModels;

namespace Hermes2018.Services
{
    public class UsuarioClaimService : IUsuarioClaimService
    {
        public InfoConfigUsuarioViewModel ObtenerInfoUsuarioClaims(ClaimsPrincipal User)
        {
            var rol = User.Claims.Where(x => x.Type == "Rol").Single().Value;

            var infoUsuario = new InfoConfigUsuarioViewModel()
            {
                //Información: Para todos los usuarios
                UserName = User.Identity.Name,
                FullName = User.Claims.Where(x => x.Type == "FullName").Single().Value,
                Rol = rol
            };
            
            if (ConstRol.RolAdminRegional.Contains(rol))
            {
                //Información: Administradores regionales
                infoUsuario.RegionId = int.Parse(User.Claims.Where(x => x.Type == "RegionId").Single().Value);
                infoUsuario.Region = User.Claims.Where(x => x.Type == "Region").Single().Value;
            }
            else if (ConstRol.RolUsuario.Contains(rol))
            {
                infoUsuario.BandejaUsuario = User.Claims.Where(x => x.Type == "BandejaUsuario").Single().Value;
                infoUsuario.BandejaPermiso = int.Parse(User.Claims.Where(x => x.Type == "BandejaPermiso").Single().Value);
                infoUsuario.BandejaNombre = User.Claims.Where(x => x.Type == "BandejaNombre").Single().Value;
                infoUsuario.BandejaRegionId = int.Parse(User.Claims.Where(x => x.Type == "BandejaRegionId").Single().Value);
                infoUsuario.BandejaAreaId = int.Parse(User.Claims.Where(x => x.Type == "BandejaAreaId").Single().Value);
                infoUsuario.BandejaPuesto = User.Claims.Where(x => x.Type == "BandejaPuesto").Single().Value;
                infoUsuario.BandejaPuestoEsUnico = User.Claims.Where(x => x.Type == "BandejaPuestoEsUnico").Single().Value == "1" ? true : false;

                //Información: Titulares, Usuarios normales, delegado, delegado revisor
                infoUsuario.RegionId = int.Parse(User.Claims.Where(x => x.Type == "RegionId").Single().Value);
                infoUsuario.Region = User.Claims.Where(x => x.Type == "Region").Single().Value;
                infoUsuario.AreaClave = User.Claims.Where(x => x.Type == "AreaClave").Single().Value;
                infoUsuario.AreaId = int.Parse(User.Claims.Where(x => x.Type == "AreaId").Single().Value);
                infoUsuario.Area = User.Claims.Where(x => x.Type == "Area").Single().Value;
                infoUsuario.Puesto = User.Claims.Where(x => x.Type == "Puesto").Single().Value;
                infoUsuario.PuestoEsUnico = User.Claims.Where(x => x.Type == "PuestoEsUnico").Single().Value == "1"? true : false;

                infoUsuario.ActivaDelegacion = User.Claims.Where(x => x.Type == "EstaActivaDelegacion").Single().Value == "1" ? true : false;
                infoUsuario.CuentaPersonal = User.Claims.Where(x => x.Type == "CuentaDependencia").Single().Value;

                infoUsuario.Session = User.Claims.Where(x => x.Type == "Session").Single().Value;
                infoUsuario.PermisoAA = User.Claims.Where(x => x.Type == "PermisoAA").Single().Value == "1" ? true : false;
                infoUsuario.EnReasignacion = User.Claims.Where(x => x.Type == "EnReasignacion").Single().Value == "1" ? true : false;
            }

            infoUsuario.TokenWebApi = User.Claims.Where(x => x.Type == "TokenWebApi").Single().Value;

            return infoUsuario;
        }
    }
}
