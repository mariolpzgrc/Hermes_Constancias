using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hermes2018.Features
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<HER_Usuario, IdentityRole>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IHerramientaService _herramientaService;
        private readonly IRegionService _regionService;

        public AppClaimsPrincipalFactory(UserManager<HER_Usuario> userManager,
                RoleManager<IdentityRole> roleManager,
                IOptions<IdentityOptions> optionsAccessor,
                IUsuarioService usuarioService,
                IHerramientaService herramientaService,
                IRegionService regionService) : base(userManager, roleManager, optionsAccessor)
        {
            _usuarioService = usuarioService; 
            _herramientaService = herramientaService;
            _regionService = regionService;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(HER_Usuario user)
        {
            var principal = await base.CreateAsync(user);
            var roles = await UserManager.GetRolesAsync(user);
            var rol = roles.First();

            var region = string.Empty;
            var regionId = string.Empty;
            var area = string.Empty;
            var areaClave = string.Empty;
            var areaId = string.Empty;
            var puesto = string.Empty;
            var puestoEsUnico = "0";
            //Bandeja activa
            var bandejaUsuario = string.Empty;
            var bandejaPermiso = "0";
            var bandejaNombre = string.Empty;
            var bandejaAreaId = string.Empty;
            var bandejaRegionId = string.Empty;
            var bandejaPuesto = string.Empty;
            var bandejaPuestoEsUnico = string.Empty;
            var estaActivaDelegacion = "0";
            //Para el caso de que el usuario tenga dos cuenta, una  personal y una cuenta de alguna dependencia
            var cuentaDependencia = string.Empty; 
            //Código de la sesión del usuario para los archivos temporales
            var session = string.Empty;
            var token = _herramientaService.ConstruirToken(user.UserName, 1000).Token;
            var permisoAA = string.Empty;
            var enReasignacion = string.Empty;

            //----
            if (ConstRol.RolUsuario.Contains(rol))
            {
                //Cuenta de dependencia
                cuentaDependencia = await _usuarioService.ObtenerCuentaPersonal(user.UserName, rol);

                var info = await _usuarioService.ObtieneInfoUsuarioNormalParaClaims(user.Id, user.UserName, cuentaDependencia);

                //Info de la región
                region = info.RegionNombre;
                regionId = info.RegionId;
                //Info del área
                areaClave = info.AreaClave;
                area = info.AreaNombre;
                areaId = info.AreaId;
                //Info del puesto
                puesto = info.PuestoNombre;
                puestoEsUnico = info.PuestoEsUnico;

                //Bandeja activa
                bandejaUsuario = info.BandejaUsuario;
                bandejaPermiso = info.BandejaPermiso;
                bandejaNombre = info.BandejaNombre;
                bandejaRegionId = info.BandejaRegionId;
                bandejaAreaId = info.BandejaAreaId;
                bandejaPuesto = info.BandejaPuesto;
                bandejaPuestoEsUnico = info.BandejaPuestoEsUnico;
                //--
                session = info.Session;
                //--
                estaActivaDelegacion = info.ActivaDelegacion;
                permisoAA = info.PermisoAA;
                enReasignacion = info.EnReasignacion;
            }
            else if (ConstRol.RolAdminRegional.Contains(rol))
            {
                var tmpregion = string.Empty;
                //Busqueda de la región
                switch (rol)
                {
                    case ConstRol.Rol2T: //Xalapa
                        tmpregion = ConstRegion.Region1T;
                        break;
                    case ConstRol.Rol3T: //Veracruz
                        tmpregion = ConstRegion.Region2T;
                        break;
                    case ConstRol.Rol4T: //Orizaba
                        tmpregion = ConstRegion.Region3T;
                        break;
                    case ConstRol.Rol5T: //Poza Rica
                        tmpregion = ConstRegion.Region4T;
                        break;
                    case ConstRol.Rol6T: //Coatzacoalcos
                        tmpregion = ConstRegion.Region5T;
                        break;
                }

                //Obtener la región para el usuario administrador regional
                var regionAdmin = await _regionService.ObtenerRegionSinAreasPorNombreAsync(tmpregion);

                region = regionAdmin.HER_Nombre;
                regionId = regionAdmin.HER_RegionId.ToString();
            }

            //--
            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim("FullName", user.HER_NombreCompleto),
                new Claim("Rol", rol),
                new Claim("Region", region),
                new Claim("RegionId", regionId),
                new Claim("AreaClave", areaClave),
                new Claim("Area", area),
                new Claim("AreaId", areaId),
                new Claim("Puesto", puesto),
                new Claim("PuestoEsUnico", puestoEsUnico),

                new Claim("BandejaUsuario", bandejaUsuario), //Nombre del usuario
                new Claim("BandejaPermiso", bandejaPermiso), //Permiso
                new Claim("BandejaNombre", bandejaNombre), //Nombre completo del usuario
                new Claim("BandejaRegionId", bandejaRegionId),
                new Claim("BandejaAreaId", bandejaAreaId),
                new Claim("BandejaPuesto", bandejaPuesto),
                new Claim("BandejaPuestoEsUnico", bandejaPuestoEsUnico),

                new Claim("EstaActivaDelegacion", estaActivaDelegacion),
                new Claim("CuentaDependencia", cuentaDependencia), // Cuenta personal institucional (jbarrera), y cuenta de alguna dependencia (ddsa)

                new Claim("Session", session), //Código para guardar los archivos
                new Claim("TokenWebApi", token), //Token de seguriadad para el API interno
                new Claim("PermisoAA", permisoAA), //Permiso de tipo Administrador del Área 
                new Claim("EnReasignacion", enReasignacion)//Verfica si esta o no en reasignacion 
            }); ;

            return principal;
        }
    }
}
