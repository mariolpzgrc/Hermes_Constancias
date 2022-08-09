using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Areas.Identity.Pages.Grupos;
using Hermes2018.Comparers;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Configuracion;
using Hermes2018.ViewComponentsModels;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hermes2018.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<HER_Usuario> _userManager;
        private readonly IClientService _clientService;
        private readonly IConfiguracionService _configuracionService;

        public UsuarioService(ApplicationDbContext context, 
            UserManager<HER_Usuario> userManager,
            IClientService clientService,
            IConfiguracionService configuracionService)
        {
            _context = context;
            _userManager = userManager;
            _clientService = clientService;
            _configuracionService = configuracionService;
        }
        
        public async Task<RemitenteDocumentoViewModel> ObtenerInfoPersonaDocumentoAsync(string userName)
        {
            var personaDocumento = new RemitenteDocumentoViewModel();
            var areaPadre = string.Empty;
            IQueryable<string> areaPadreQuery;

            var infoConAreaQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_UserName == userName
                         && x.HER_Activo == true)
                .Include(x => x.HER_Area)
                .AsNoTracking()
                .AsQueryable();

            var infoConArea = await infoConAreaQuery.FirstOrDefaultAsync();

            if (infoConArea != null)
            {

                if (infoConArea.HER_Area.HER_Area_PadreId != null)
                {
                    areaPadreQuery = _context.HER_Area
                        .Where(x => x.HER_AreaId == infoConArea.HER_Area.HER_Area_PadreId 
                                 && x.HER_Estado == ConstEstadoArea.EstadoN1)
                        .Select(x => x.HER_Nombre)
                        .AsNoTracking()
                        .AsQueryable();

                    areaPadre = await areaPadreQuery.FirstOrDefaultAsync();
                }

                personaDocumento = new RemitenteDocumentoViewModel
                {
                    Nombre = infoConArea.HER_NombreCompleto,
                    Direccion = infoConArea.HER_Direccion,
                    Telefono = infoConArea.HER_Telefono,
                    AreaPadre = areaPadre,
                    AreaHijo = infoConArea.HER_Area.HER_Nombre,
                    AreaHijoId = infoConArea.HER_Area.HER_AreaId,
                    Puesto = infoConArea.HER_Puesto,
                    Correo = infoConArea.HER_Correo,
                    Usuario = infoConArea.HER_UserName,
                };
            }

            return personaDocumento;
        }
        public async Task<List<int>> ObtenerIdentificadoresUsuariosAsync(List<string> usuarios)
        {
            var listadoIds = new List<int>();
            var idUsuario = 0;
            

            if (usuarios.Count > 0)
            {
                foreach(string usuario in usuarios)
                {
                    var listadoIdQuery = _context.HER_InfoUsuario
                       .Where(x => x.HER_UserName == usuario
                               && x.HER_Activo == true)
                       .Select(x => x.HER_InfoUsuarioId)
                       .AsQueryable();

                     idUsuario = await listadoIdQuery.FirstOrDefaultAsync();

                    listadoIds.Add(idUsuario);
                } 
                
            }
            return listadoIds;
        }
        public async Task<int> ObtenerIdentificadorUsuarioAsync(string usuario)
        {
            var usuarioIdQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_UserName == usuario 
                            && x.HER_Activo == true)
                .Select(x => x.HER_InfoUsuarioId)
                .AsQueryable();

            return await usuarioIdQuery.FirstOrDefaultAsync();
        }
        public async Task<List<IdentificadorUsuarioCompuestoViewModel>> ObtenerIdentificadoresCompuestosUsuariosAsync(List<string> usuarios)
        {
            var listadoIds = new List<IdentificadorUsuarioCompuestoViewModel>();

            if (usuarios.Count > 0)
            {

                foreach (string usuario in usuarios) 
                {
                    var listadoIdsQuery = _context.HER_InfoUsuario
                        .Where(x => x.HER_UserName == usuario
                                && x.HER_Activo == true)
                        .Select(x =>
                        new IdentificadorUsuarioCompuestoViewModel()
                        {
                            InfoUsuarioId = x.HER_InfoUsuarioId,
                            AreaId = x.HER_AreaId,
                            DiasCompromiso = x.HER_Area.HER_DiasCompromiso,
                            Telefono = x.HER_Telefono,
                            Direccion = x.HER_Direccion
                        })
                        .AsQueryable();

                    var user = await listadoIdsQuery.FirstOrDefaultAsync();

                    listadoIds.Add(user);
                }
            }
            return listadoIds;
        }
        public async Task<IdentificadorUsuarioCompuestoViewModel> ObtenerIdentificadorCompuestoUsuarioAsync(string usuario)
        {
            var usuarioIdQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_UserName == usuario
                            && x.HER_Activo == true)
                .Select(x =>
                    new IdentificadorUsuarioCompuestoViewModel()
                    {
                        InfoUsuarioId = x.HER_InfoUsuarioId,
                        AreaId = x.HER_AreaId,
                        DiasCompromiso = x.HER_Area.HER_DiasCompromiso,
                        Telefono = x.HER_Telefono,
                        Direccion = x.HER_Direccion
                    })
                .AsQueryable();

            return await usuarioIdQuery.FirstOrDefaultAsync();
        }
        public async Task<string> ObtenerIdentificadorSoloUsuarioAsync(string usuario)
        {
            var usuarioQuery = _context.HER_Usuario
                .Where(x => x.UserName == usuario)
                .Select(x => x.Id)
                .AsNoTracking()
                .AsQueryable();

            return await usuarioQuery.FirstOrDefaultAsync();
        }
        public async Task<List<int>> ObtenerIdentificadoresPorNombreCompletoAsync(List<string> nombres)
        {
            var listadoIds = new List<int>();
            if (nombres.Count > 0)
            {
                var listadoIdsQuery = _context.HER_InfoUsuario
                    .Where(x => nombres.Contains(x.HER_NombreCompleto) 
                            && x.HER_Activo == true)
                    .Select(x => x.HER_InfoUsuarioId)
                    .AsQueryable();

                listadoIds = await listadoIdsQuery.ToListAsync();
            }
            return listadoIds;
        }
        public async Task<int> ObtenerIdentificadorPorNombreCompletoAsync(string nombre)
        {
            var usuarioIdQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_NombreCompleto == nombre 
                         && x.HER_Activo == true)
                .Select(x => x.HER_InfoUsuarioId)
                .AsQueryable();

            return await usuarioIdQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ExisteUsuarioActivoAsync(string userName)
        {
            bool existe = false;

            var usuario = await _userManager.FindByNameAsync(userName);
            if (usuario != null)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                var rol = roles.First();

                if (ConstRol.RolAdmin.Contains(rol)) {
                    existe = true;

                } else if (ConstRol.RolUsuario.Contains(rol))
                {
                    var existeInfoUsuarioQuery = _context.HER_InfoUsuario
                        .Where(x => x.HER_UserName == userName
                                 && x.HER_Activo == true)
                        .AsNoTracking()
                        .AsQueryable();
                    
                    existe = await existeInfoUsuarioQuery.AnyAsync();
                }
            }

            return existe;
        }
        public async Task<bool> ExisteSoloUsuarioAsync(string userName, bool esAdmin)
        {
            IQueryable<HER_Usuario> usuarioAdminExisteQuery;
            IQueryable<HER_Usuario> usuarioExisteQuery;

            bool respuesta = false;

            if (esAdmin)
            {
                usuarioAdminExisteQuery = _context.HER_Usuario
                    .Where(x => x.UserName.Equals(userName)
                             && x.HER_Aprobado == true)
                    .AsNoTracking()
                    .AsQueryable();
                //--
                respuesta = await usuarioAdminExisteQuery.AnyAsync();
            }
            else
            {
                usuarioAdminExisteQuery = _context.HER_Usuario
                   .Where(x => x.UserName.Equals(userName))
                   .AsNoTracking()
                   .AsQueryable();
                //--
                var usuarioAdmin = await usuarioAdminExisteQuery.FirstOrDefaultAsync();
                //--
                if (usuarioAdmin != null)
                {
                    if (usuarioAdmin.HER_Aprobado)
                    {
                        usuarioExisteQuery = _context.HER_Usuario
                           .Where(x => x.UserName.Equals(userName)
                                    && x.HER_Usuarios.Where(y => y.HER_Activo == true).Any()
                                    && x.HER_Aprobado == true)
                           .AsNoTracking()
                           .AsQueryable();
                        //--
                        respuesta = await usuarioExisteQuery.AnyAsync();
                    }
                    else
                    {
                        usuarioExisteQuery = _context.HER_Usuario
                           .Where(x => x.UserName.Equals(userName)
                                    && !x.HER_Usuarios.Where(y => y.HER_Activo == true).Any()
                                    && x.HER_Aprobado == false)
                           .AsNoTracking()
                           .AsQueryable();
                        //--
                        respuesta = await usuarioExisteQuery.AnyAsync();
                    }
                }
                else {
                    respuesta = false;
                }
            }

            return respuesta;
        }
        public async Task<bool> ExisteSoloInfoUsuarioAsync(string userName)
        {
            var usuarioExisteQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_UserName.Equals(userName))
                .AsNoTracking()
                .AsQueryable();

            return await usuarioExisteQuery.AnyAsync();
        }
        public async Task<bool> ExisteUsuarioTitularAsync(int areaId)
        {
            //Busca si ya exite un titular en el área que intenta registrar
            var cantidadTitularesQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Activo == true
                         && x.HER_RolNombre == ConstRol.Rol7T
                         && x.HER_AreaId == areaId)
                .AsNoTracking()
                .AsQueryable();

            return await cantidadTitularesQuery.AnyAsync();
        }
        public async Task<DateTime> ObtenerFechaCompromisoAsync(DateTime fechaRecepcion, int usuarioId)
        {
            var diasCompromisoQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_InfoUsuarioId == usuarioId 
                         && x.HER_Activo == true)
                .Include(x => x.HER_Area)
                .Select(x => x.HER_Area.HER_DiasCompromiso)
                .AsQueryable();

            var diasCompromiso = await diasCompromisoQuery.FirstOrDefaultAsync();
            var dias = 0;
            //--
            var contador = 1;
            //--
            var fecha = new DateTime(fechaRecepcion.Year, fechaRecepcion.Month, fechaRecepcion.Day, 0, 0, 0);

            //--
            var diasFestivosQuery = _context.HER_CalendarioContenido
                .Where(x => x.HER_Fecha >= fecha)
                .Select(x => x.HER_Fecha)
                .Take(60)
                .AsQueryable();

            var diasFestivos = await diasFestivosQuery.ToListAsync();

            if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday || diasFestivos.Contains(fecha)) { dias = diasCompromiso; } else { dias = diasCompromiso + 1; }

            do
            {
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    fecha = fecha.AddDays(1);
                }
                else if (diasFestivos.Contains(fecha))
                {              
                        fecha = fecha.AddDays(1);                    
                }
                else
                {
                    fecha = fecha.AddDays(1);
                    contador += 1;
                }
            } while (contador < dias);            
            return fecha;
        }
        public async Task<DateTime> ObtenerFechaProrrogaCompromisoAsync(DateTime fechaRecepcion)
        {
            var dias = 0;
            var contador = 1;

            var fecha = new DateTime(fechaRecepcion.Year, fechaRecepcion.Month, fechaRecepcion.Day, 0, 0, 0);

            //--
            var diasFestivosQuery = _context.HER_CalendarioContenido
                .Where(x => x.HER_Fecha >= fecha)
                .Select(x => x.HER_Fecha)
                .Take(60)
                .AsQueryable();

            var diasFestivos = await diasFestivosQuery.ToListAsync();

            if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday || diasFestivos.Contains(fecha)) { dias = 10; } else { dias = 11; }

            do
            {
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    fecha = fecha.AddDays(1);
                }
                else if (diasFestivos.Contains(fecha))
                {
                    fecha = fecha.AddDays(1);
                }
                else
                {
                    fecha = fecha.AddDays(1);
                    contador += 1;
                }
            } while (contador < dias);

            return fecha;
        }
        public async Task<string> ObtenerRolUsuarioAsync(string userName)
        {
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(userName));

            return roles.First();
        }
        public async Task<IEnumerable<string>> ObtenerRolesUsuarioAsync(string userName)
        {
            return await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(userName));
        }
        //--

        public async Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesAsync(string keyword)
        {
            List<UsuarioLocalJsonModel> listado = new List<UsuarioLocalJsonModel>();
            UsuarioLocalComparer usuarioLocalComparer = new UsuarioLocalComparer();

            if (!string.IsNullOrEmpty(keyword))
            {
                var usuariosQuery = _context.HER_InfoUsuario
                    //.Include(x => x.HER_Area).ThenInclude(x => x.HER_Region)
                    .Where(x => (EF.Functions.Like(x.HER_UserName, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_NombreCompleto, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_Area.HER_Nombre, "%" + keyword + "%"))
                              && x.HER_Activo == true
                              && x.HER_EstaEnReasignacion == false
                              && !(from iu in _context.HER_InfoUsuario
                                   where iu.HER_Titular == x.HER_UserName
                                     && (ConstRol.Rol8T == x.HER_RolNombre && iu.HER_RolNombre == ConstRol.Rol7T)
                                     && iu.HER_Activo == true
                                   select iu.HER_UserName).Any()
                           )
                    .AsNoTracking()
                    .Take(20)
                    .Select(x => new UsuarioLocalJsonModel {
                        HER_UserName = x.HER_UserName,
                        HER_Correo = x.HER_Correo,
                        HER_NombreCompleto = x.HER_NombreCompleto,
                        HER_Region = x.HER_Area.HER_Region.HER_Nombre,
                        HER_Area = x.HER_Area.HER_Nombre,
                        HER_Puesto = x.HER_Puesto,
                        HER_Tipo = x.HER_RolNombre,
                        HER_Titular = x.HER_Titular,
                    })
                    .AsQueryable();

                var usuarios = await usuariosQuery
                                    .OrderBy(x => x.HER_NombreCompleto)
                                    .ThenBy(x => x.HER_Tipo)
                                    .ThenBy(x => x.HER_Area)
                                    .ToListAsync();

                listado = usuarios.Distinct(usuarioLocalComparer).ToList();
                //listado = usuarios;
            }

            return listado;
        }
        public async Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesAsync(string keyword, string usercurrent)
        {
            List<UsuarioLocalJsonModel> listado = new List<UsuarioLocalJsonModel>();
            UsuarioLocalComparer usuarioLocalComparer = new UsuarioLocalComparer();

            if (!string.IsNullOrEmpty(keyword))
            {
                var usuariosQuery = _context.HER_InfoUsuario
                    //.Include(x => x.HER_Area).ThenInclude(x => x.HER_Region)
                    .Where(x => (EF.Functions.Like(x.HER_UserName, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_NombreCompleto, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_Area.HER_Nombre, "%" + keyword + "%"))
                              && x.HER_Activo == true
                              && x.HER_UserName != usercurrent
                              && !(from iu in _context.HER_InfoUsuario
                                   where iu.HER_Titular == x.HER_UserName
                                       && iu.HER_RolNombre == ConstRol.Rol7T
                                       && iu.HER_Activo == true
                                   select iu.HER_UserName).Any())
                    .AsNoTracking()
                    .Take(20)
                    .Select(x => new UsuarioLocalJsonModel
                    {
                        HER_UserName = x.HER_UserName,
                        HER_Correo = x.HER_Correo,
                        HER_NombreCompleto = x.HER_NombreCompleto,
                        HER_Region = x.HER_Area.HER_Region.HER_Nombre,
                        HER_Area = x.HER_Area.HER_Nombre,
                        HER_Puesto = x.HER_Puesto,
                        HER_Tipo = x.HER_RolNombre,
                        HER_Titular = x.HER_Titular
                    })
                    .AsQueryable();

                var usuarios = await usuariosQuery
                    .OrderBy(x => x.HER_NombreCompleto)
                    .ThenBy(x => x.HER_Tipo)
                    .ThenBy(x => x.HER_Area)
                    .ToListAsync();

                listado = usuarios.Distinct(usuarioLocalComparer).ToList();
            }

            return listado;
        }
        //Buscar
        public async Task<List<UsuariosBuscarViewModel>> BusquedaUsuariosLocalesBuscarAsync(string keyword)
        {
            List<UsuariosBuscarViewModel> listado = new List<UsuariosBuscarViewModel>();
            UsuarioLocalBuscarComparer usuarioLocalComparer = new UsuarioLocalBuscarComparer();

            if (!string.IsNullOrEmpty(keyword))
            {
                var usuariosQuery = _context.HER_InfoUsuario
                    //.Include(x => x.HER_Area).ThenInclude(x => x.HER_Region)
                    .Where(x => (EF.Functions.Like(x.HER_UserName, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_NombreCompleto, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_Area.HER_Nombre, "%" + keyword + "%"))
                              && x.HER_Activo == true
                              && !(from iu in _context.HER_InfoUsuario
                                   where iu.HER_Titular == x.HER_UserName
                                     && (ConstRol.Rol8T == x.HER_RolNombre && iu.HER_RolNombre == ConstRol.Rol7T)
                                     && iu.HER_Activo == true
                                   select iu.HER_UserName).Any()
                           )
                    .AsNoTracking()
                    .Take(20)
                    .Select(x => new UsuariosBuscarViewModel
                    {
                        HER_NombreCompleto = x.HER_NombreCompleto,
                        HER_UserName = x.HER_UserName,
                        HER_Correo = x.HER_Correo,
                        HER_NombreUsuario = x.HER_UserName,
                        HER_Tipo = x.HER_RolNombre,
                        HER_Puesto = x.HER_Puesto,
                        HER_EsUnico = x.HER_EsUnico ? "Si" : "No",
                        HER_Area = x.HER_Area.HER_Nombre,
                        HER_Region = x.HER_Area.HER_Region.HER_Nombre,
                        HER_LigaArea = "Usuarios/Area/" + x.HER_AreaId,
                        HER_Aprobado = (x.HER_Usuario.HER_Aprobado) ? ConstAprobado.AprobadoSi : ConstAprobado.AprobadoNo,
                        HER_FechaAprobacion = x.HER_Usuario.HER_FechaAprobado.ToString("dd/MM/yyyy HH:mm:ss"),
                        HER_Titular = x.HER_Titular,
                        HER_Estado = string.Format("{0}{1}", x.HER_Activo ? "Activo" : "Inactivo", x.HER_EstaEnReasignacion ? " - En reasignación" : string.Empty),
                        HER_AceptoTerminos = (x.HER_Usuario.HER_AceptoTerminos) ? ConstTerminos.TerminosSi : ConstTerminos.TerminosNo,
                        HER_FechaTerminos = x.HER_Usuario.HER_FechaAceptoTerminos.ToString("dd/MM/yyyy HH:mm:ss"),
                        HER_PermisoAdministradorArea = x.HER_PermisoAA ? "Si" : "No",
                        HER_FechaRegistro = x.HER_FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"),
                        HER_FechaActualizacion = x.HER_FechaActualizacion.ToString("dd/MM/yyyy HH:mm:ss")
                    })
                    .AsQueryable();

                var usuarios = await usuariosQuery
                                    .OrderBy(x => x.HER_NombreCompleto)
                                    .ThenBy(x => x.HER_Area)
                                    .ToListAsync();

                listado = usuarios.Distinct(usuarioLocalComparer).ToList();
                //listado = usuarios;
            }

            return listado;
        }

        public async Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesColeccionAsync(List<string> listUsuarios)
        {
            List<UsuarioLocalJsonModel> listadotemp = new List<UsuarioLocalJsonModel>();
            List<UsuarioLocalJsonModel> listado = new List<UsuarioLocalJsonModel>();
            UsuarioLocalComparer usuarioLocalComparer = new UsuarioLocalComparer();

            if (listUsuarios.Count > 0)
            {
                foreach (string usuario in listUsuarios)
                {
                    var usuariosQuery = _context.HER_InfoUsuario
                    .Where(x => x.HER_UserName == usuario
                              && x.HER_Activo == true
                              && !(from iu in _context.HER_InfoUsuario
                                   where iu.HER_Titular == x.HER_UserName
                                     && (ConstRol.Rol8T == x.HER_RolNombre && iu.HER_RolNombre == ConstRol.Rol7T)
                                     && iu.HER_Activo == true
                                   select iu.HER_UserName).Any()
                           )
                    .AsNoTracking()
                    .Take(50)
                    .Select(x => new UsuarioLocalJsonModel
                    {
                        HER_UserName = x.HER_UserName,
                        HER_Correo = x.HER_Correo,
                        HER_NombreCompleto = x.HER_NombreCompleto,
                        HER_Region = x.HER_Area.HER_Region.HER_Nombre,
                        HER_Area = x.HER_Area.HER_Nombre,
                        HER_Puesto = x.HER_Puesto,
                        HER_Tipo = x.HER_RolNombre,
                        HER_Titular = x.HER_Titular,
                    })
                    .AsQueryable();

                    var usuarios = await usuariosQuery.FirstOrDefaultAsync();

                    listadotemp.Add(usuarios);
                }

                listado = listadotemp.Distinct(usuarioLocalComparer).ToList();
                /*var usuariosQuery = _context.HER_InfoUsuario
                    .Where(x =>
                              listUsuarios.Contains(x.HER_UserName)
                              && x.HER_Activo == true
                              && !(from iu in _context.HER_InfoUsuario
                                   where iu.HER_Titular == x.HER_UserName
                                     && (ConstRol.Rol8T == x.HER_RolNombre && iu.HER_RolNombre == ConstRol.Rol7T)
                                     && iu.HER_Activo == true
                                   select iu.HER_UserName).Any()
                           )
                    .AsNoTracking()
                    .Take(50)
                    .Select(x => new UsuarioLocalJsonModel
                    {
                        HER_UserName = x.HER_UserName,
                        HER_Correo = x.HER_Correo,
                        HER_NombreCompleto = x.HER_NombreCompleto,
                        HER_Region = x.HER_Area.HER_Region.HER_Nombre,
                        HER_Area = x.HER_Area.HER_Nombre,
                        HER_Puesto = x.HER_Puesto,
                        HER_Tipo = x.HER_RolNombre,
                        HER_Titular = x.HER_Titular,
                    })
                    .AsQueryable();

                var usuarios = await usuariosQuery                                    
                                    .ToListAsync()

                listado = usuarios.Distinct(usuarioLocalComparer).ToList();*/
                //listado = usuarios;
            }

            return listado;
        }
        public async Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesRevisionAsync(string userName, int areaId, string keyword)
        {
            List<UsuarioLocalJsonModel> listado = new List<UsuarioLocalJsonModel>();
            UsuarioLocalComparer usuarioLocalComparer = new UsuarioLocalComparer();

            if (!string.IsNullOrEmpty(keyword))
            {
                var usuariosQuery = _context.HER_InfoUsuario
                        //.Include(x => x.HER_Area).ThenInclude(x => x.HER_Region)
                        .Where(x => x.HER_AreaId == areaId
                                && x.HER_Activo == true
                                && !x.HER_UserName.Equals(userName)
                                && (EF.Functions.Like(x.HER_UserName, "%" + keyword + "%")
                                     || EF.Functions.Like(x.HER_NombreCompleto, "%" + keyword + "%")
                                     || EF.Functions.Like(x.HER_Area.HER_Nombre, "%" + keyword + "%"))
                                && !(from iu in _context.HER_InfoUsuario
                                     where iu.HER_Titular == x.HER_UserName
                                         && iu.HER_RolNombre == ConstRol.Rol7T
                                         && iu.HER_Activo == true
                                     select iu.HER_UserName).Any())
                        .AsNoTracking()
                        .Take(20)
                        .Select(x => new UsuarioLocalJsonModel {
                            HER_UserName = x.HER_UserName,
                            HER_Correo = x.HER_Correo,
                            HER_NombreCompleto = x.HER_NombreCompleto,
                            HER_Region = x.HER_Area.HER_Region.HER_Nombre,
                            HER_Area = x.HER_Area.HER_Nombre,
                            HER_Puesto = x.HER_Puesto,
                            HER_Tipo = x.HER_RolNombre,
                            HER_Titular = x.HER_Titular
                        })
                        .AsQueryable();

                var usuarios = await usuariosQuery
                    .OrderBy(x => x.HER_NombreCompleto)
                    .ThenBy(x => x.HER_Tipo)
                    .ThenBy(x => x.HER_Area)
                    .ToListAsync();

                listado = usuarios.Distinct(usuarioLocalComparer).ToList();
            }

            return listado;
        }
        public async Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesDelegarAsync(string userName, string keyword)
        {
            List<UsuarioLocalJsonModel> listado = new List<UsuarioLocalJsonModel>();
            UsuarioLocalComparer usuarioLocalComparer = new UsuarioLocalComparer();

            if (!string.IsNullOrEmpty(keyword))
            {
                var usuariosQuery = _context.HER_InfoUsuario
                    //.Include(x => x.HER_Area).ThenInclude(x => x.HER_Region)
                    .Where(x => x.HER_Activo == true
                          && x.HER_EstaEnReasignacion == false
                          && !x.HER_UserName.Equals(userName)
                          && (EF.Functions.Like(x.HER_UserName, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_NombreCompleto, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_Area.HER_Nombre, "%" + keyword + "%"))
                         /* && !(from iu in _context.HER_InfoUsuario
                               where iu.HER_Titular == x.HER_UserName
                                   && iu.HER_RolNombre == ConstRol.Rol7T
                                   && iu.HER_Activo == true
                               select iu.HER_UserName).Any()*/
                    )
                    .AsNoTracking()
                    .Take(10)
                    .Select(x => new UsuarioLocalJsonModel {
                        HER_UserName = x.HER_UserName,
                        HER_Correo = x.HER_Correo,
                        HER_NombreCompleto = x.HER_NombreCompleto,
                        HER_Region = x.HER_Area.HER_Region.HER_Nombre,
                        HER_Area = x.HER_Area.HER_Nombre,
                        HER_Puesto = x.HER_Puesto,
                        HER_Tipo = x.HER_RolNombre,
                        HER_Titular = x.HER_Titular
                    })
                    .AsQueryable();

                var usuarios = await usuariosQuery
                            .OrderBy(x => x.HER_NombreCompleto)
                            .ThenBy(x => x.HER_Tipo)
                            .ThenBy(x => x.HER_Area)
                            .ToListAsync();

                listado =  usuarios.Distinct(usuarioLocalComparer).ToList();
            }

            return listado;
        }
        public async Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesReasignacionAsync(string keyword)
        {
            List<UsuarioLocalJsonModel> listado = new List<UsuarioLocalJsonModel>();

            if (!string.IsNullOrEmpty(keyword))
            {
                var usuariosQuery = _context.HER_InfoUsuario
                    //.Include(x => x.HER_Area).ThenInclude(x => x.HER_Region)
                    .Where(x => (EF.Functions.Like(x.HER_UserName, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_NombreCompleto, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_Area.HER_Nombre, "%" + keyword + "%"))
                              && x.HER_Activo == true
                              && x.HER_EstaEnReasignacion == true)
                    .AsNoTracking()
                    .Take(10)
                    .Select(x => new UsuarioLocalJsonModel
                    {
                        HER_UserName = x.HER_UserName,
                        HER_Correo = x.HER_Correo,
                        HER_NombreCompleto = x.HER_NombreCompleto,
                        HER_Region = x.HER_Area.HER_Region.HER_Nombre,
                        HER_Area = x.HER_Area.HER_Nombre,
                        HER_Puesto = x.HER_Puesto,
                        HER_Tipo = x.HER_RolNombre,
                        HER_Titular = x.HER_Titular
                    })
                    .AsQueryable();

                listado = await  usuariosQuery
                    .OrderBy(x => x.HER_NombreCompleto)
                    .ThenBy(x => x.HER_Tipo)
                    .ThenBy(x => x.HER_Area)
                    .ToListAsync();
            }   


            return listado;
        }
        public async Task<List<UsuarioADViewModel>> BusquedaUsuariosDirectorioActivoAsync(string keyword)
        {
            List<UsuarioADViewModel> listado = new List<UsuarioADViewModel>();

            if (!string.IsNullOrEmpty(keyword))
            {
                var configuration = await _configuracionService.ObtenerInfoConfiguracionLDAPAsync();
                
                using (var context = new PrincipalContext(ContextType.Domain, configuration.HER_IPLDAP, "hermes", "h3rm3s"))
                {
                    UserPrincipal userPrincipal = new UserPrincipal(context)
                    {
                        SamAccountName = keyword + "*",
                    };

                    using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    {
                        searcher.QueryFilter = userPrincipal;
                        ((DirectorySearcher)searcher.GetUnderlyingSearcher()).SizeLimit = 10;
                        foreach (var result in searcher.FindAll())
                        {
                            DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                            UsuarioADViewModel u = new UsuarioADViewModel
                            {
                                HER_Usuario_NombreCompleto = string.Format("{0} {1}", (de.Properties["sn"].Value != null) ? de.Properties["sn"].Value.ToString() : string.Empty, (de.Properties["givenName"].Value != null) ? de.Properties["givenName"].Value.ToString() : string.Empty),
                                HER_Usuario_Correo = de.Properties["userPrincipalName"].Value.ToString(),
                                HER_Usuario_Username = de.Properties["samaccountname"].Value.ToString(),
                            };
                            listado.Add(u);
                        }
                    }
                }
                //--
            }

            return listado;
        }

       public async Task<List<UsuarioADViewModel>> LimpiarBusquedaUsuariosDirectorioActivoAsync(List<UsuarioADViewModel> listado) 
       {
            var usuariosQuitarQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Activo == true 
                         && (x.HER_EstaEnReasignacion == false || x.HER_EstaEnReasignacion == true)
                         && listado.Select(a => a.HER_Usuario_Username).Contains(x.HER_UserName))
                .Select(x => x.HER_UserName)
                .AsNoTracking()
                .AsQueryable();

            var usuarios = await usuariosQuitarQuery.ToListAsync();

            listado.RemoveAll(x => usuarios.Contains(x.HER_Usuario_Username));

            return listado;
        }
       
        public async Task<List<BandejasViewComponentModel>> ObtenerUsuariosDelegadosAsync(string usuarioTitular)
        {
            var listadobandejas = new List<BandejasViewComponentModel>();
            //--
            var deleganQuery = _context.HER_Delegar
                    .Where(x => x.HER_Delegado.HER_UserName == usuarioTitular
                             && x.HER_Delegado.HER_Activo == true
                             && x.HER_Titular.HER_Activo == true)
                    //.Include(x => x.HER_Titular)
                    .AsNoTracking()
                    .Select(x => new BandejasViewComponentModel {
                        Usuario = x.HER_Titular.HER_UserName,
                        NombreCompleto = string.Format("{0} {1}", x.HER_Titular.HER_NombreCompleto + " (" + x.HER_Titular.HER_UserName + ")", (x.HER_Tipo == ConstDelegar.TipoN1) ? " - " + ConstDelegar.TipoTS1 : " - " + ConstDelegar.TipoTS2),
                        Tipo = (x.HER_Tipo == ConstDelegar.TipoN1) ? ConstDelegar.TipoTS1 : ConstDelegar.TipoTS2
                    })
                    .AsQueryable();

            listadobandejas = await deleganQuery.ToListAsync();

            return listadobandejas;
        }

        //Módulo de usuarios
        public List<UsuariosAdministradoresViewModel> ObtenerUsuariosAdministradores(string usuarioActual)
        {
            var usuariosQuery = ConstRol.RolAdmin.Select(role => _userManager.GetUsersInRoleAsync(role).Result)
                                .Aggregate((a, b) => a.Union(b).Where(x => x.UserName != usuarioActual).ToList())
                                .Select(x => new UsuariosAdministradoresViewModel()
                                {
                                    Nombre = x.HER_NombreCompleto,
                                    NombreUsuario = x.UserName,
                                    Rol = _userManager.GetRolesAsync(x).Result.First()
                                })
                                .AsQueryable();

            return usuariosQuery.ToList();
        }
        public async Task<List<UsuariosPorAreaViewModel>> ObtenerUsuariosPorAreaAsync(int areaId)
        {
            var usuariosQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Area.HER_AreaId == areaId 
                         && x.HER_Usuario.HER_Aprobado == true 
                         && x.HER_Activo == true)
                .OrderBy(x => x.HER_NombreCompleto)
                //.Include(u => u.HER_Usuario)
                //.Include(a => a.HER_Area)
                .Select(x => new UsuariosPorAreaViewModel() {
                    Nombre = x.HER_NombreCompleto,
                    NombreUsuario = x.HER_UserName,
                    Rol = x.HER_RolNombre,
                    Puesto = x.HER_Puesto,
                    EsUnico = x.HER_EsUnico ? "Si": "No",
                    Estado = (x.HER_Area.HER_Estado == ConstEstadoArea.EstadoN1)? string.Format("{0}{1}", x.HER_Activo ? "Activo" : "Inactivo", x.HER_EstaEnReasignacion ? " - En reasignación" : string.Empty) : ConstEstadoArea.EstadoParaUsurioT2,
                    Permiso = x.HER_PermisoAA? "Si": "No"
                })
                //.OrderBy(x => x.Rol).ThenBy(x => x.Puesto).ThenBy(x => x.Estado)
                .AsNoTracking()
                .AsQueryable();

            return await usuariosQuery.ToListAsync();
        }
        public async Task<UsuariosDetallesViewModel> ObtenerDetalleUsuarioAsync(string rol, string username)
        {
            IQueryable<UsuariosDetallesViewModel> usuarioQuery;

            if (ConstRol.RolAdmin.Contains(rol))
            {
                usuarioQuery = _context.HER_Usuario
                    .Where(x => x.UserName == username
                             && x.HER_Aprobado == true)
                    .AsNoTracking()
                    .Select(x => new UsuariosDetallesViewModel() {
                        Nombre = x.HER_NombreCompleto,
                        NombreUsuario = x.UserName,
                        Correo = x.Email,
                        RolNombre = rol,
                        AceptoTerminos = (x.HER_AceptoTerminos) ? ConstTerminos.TerminosSi : ConstTerminos.TerminosNo,
                        FechaTerminos = x.HER_FechaAceptoTerminos,
                        EstaAprobado = (x.HER_Aprobado) ? ConstAprobado.AprobadoSi : ConstAprobado.AprobadoNo,
                        FechaAprobado = x.HER_FechaAprobado,
                        Tipo = 1,
                        //---
                        Activo = true,
                        RegionId = 0,
                        RegionNombre = string.Empty,
                        AreaPadreId = 0,
                        AreaId = 0,
                        AreaNombre = string.Empty,
                        Estado = string.Empty,
                        EstaEnReasignacion = false,
                        FechaActualizacionRegistro = DateTime.Now,
                        FechaRegistro = DateTime.Now, 
                        PuestoNombre = string.Empty,
                        EsUnico = string.Empty,
                        Titular = string.Empty,
                        Permiso = string.Empty
                    })
                    .AsQueryable();
            }
            else
            {

                usuarioQuery = _context.HER_InfoUsuario
                  //.Include(x => x.HER_Usuario)
                  //.Include(x => x.HER_Area)HER_Titular
                  //      .ThenInclude(x => x.HER_Region)
                  .Where(x => x.HER_Usuario.UserName == username
                           && x.HER_Usuario.HER_Aprobado == true
                           && x.HER_Activo == true
                  )
                  .AsNoTracking()
                  .Select(x => new UsuariosDetallesViewModel()
                  {
                      Nombre = x.HER_NombreCompleto,
                      NombreUsuario = x.HER_UserName,
                      Correo = x.HER_Correo,
                      Titular = x.HER_Titular,
                      RegionId = x.HER_Area.HER_RegionId,
                      RegionNombre = x.HER_Area.HER_Region.HER_Nombre,
                      AreaPadreId = x.HER_Area.HER_Area_PadreId,
                      AreaId = x.HER_Area.HER_AreaId,
                      AreaNombre = x.HER_Area.HER_Nombre,
                      PuestoNombre = x.HER_Puesto,
                      EsUnico = x.HER_EsUnico? "Si" : "No",
                      RolNombre = rol,
                      AceptoTerminos = (x.HER_Usuario.HER_AceptoTerminos) ? ConstTerminos.TerminosSi : ConstTerminos.TerminosNo,
                      FechaTerminos = x.HER_Usuario.HER_FechaAceptoTerminos,
                      EstaAprobado = (x.HER_Usuario.HER_Aprobado) ? ConstAprobado.AprobadoSi : ConstAprobado.AprobadoNo,
                      FechaAprobado = x.HER_Usuario.HER_FechaAprobado,
                      FechaRegistro = x.HER_FechaRegistro,
                      FechaActualizacionRegistro = x.HER_FechaActualizacion,
                      Estado = string.Format("{0}{1}", x.HER_Activo ? "Activo" : "Inactivo", x.HER_EstaEnReasignacion ? " - En reasignación" : string.Empty),
                      Activo = x.HER_Activo,
                      EstaEnReasignacion = x.HER_EstaEnReasignacion,
                      //--
                      Tipo = 2,
                      Permiso = x.HER_PermisoAA ? "Si" : "No"
                  })
                  .AsQueryable();
            }
            var tmp = await usuarioQuery.FirstOrDefaultAsync();
            return tmp;
        }
        public async Task<bool> GuardarUsuarioAsync(UsuariosCrearViewModel viewModel)
        {
            int result = 0;

            var usuarioQuery = _context.HER_Usuario
                .Where(x => x.UserName == viewModel.NombreUsuario 
                        && !x.HER_Usuarios.Any(y => y.HER_Activo == true))
                .AsQueryable();

            var usuario = await usuarioQuery.FirstOrDefaultAsync();

            if (usuario != null)
            {
                usuario.Email = viewModel.Correo;
                usuario.HER_NombreCompleto = viewModel.Nombre;
                usuario.HER_Aprobado = true;
                usuario.HER_FechaAprobado = DateTime.Now;
                usuario.HER_AceptoTerminos = false;
                usuario.HER_FechaAceptoTerminos = DateTime.Now;

                await _userManager.RemoveFromRolesAsync(usuario, await _userManager.GetRolesAsync(usuario));
                await _userManager.AddToRoleAsync(usuario, viewModel.Rol);
                await _userManager.UpdateAsync(usuario);
            }

            if (ConstRol.RolAdminRegional.Contains(viewModel.Rol))
            {
                //Administradores
                if (usuario == null)
                {
                    var userAdmin = new HER_Usuario
                    {
                        SecurityStamp = "UV",
                        UserName = viewModel.NombreUsuario,
                        Email = viewModel.Correo,
                        HER_AceptoTerminos = false,
                        HER_Aprobado = true,
                        HER_FechaAprobado = DateTime.Now,
                        HER_NombreCompleto = viewModel.Nombre
                    };
                    var resultAdmin = await _userManager.CreateAsync(userAdmin, string.Format("{0}{1}", ConstMasterKey.Key1, viewModel.NombreUsuario));

                    if (resultAdmin.Succeeded)
                    {
                        //Asociacion del usuario con el rol
                        await _userManager.AddToRoleAsync(userAdmin, viewModel.Rol);
                    }

                    //Crear la configuración por defecto para el usuario
                    var configAdmin = new HER_ConfiguracionUsuario()
                    {
                        HER_Notificaciones = true,
                        HER_ElementosPorPagina = 25,
                        HER_UsuarioId = userAdmin.Id,
                        HER_ColorId = _context.HER_Color.Where(x => x.HER_Nombre == ConstColor.ColorT1).Select(x => x.HER_ColorId).FirstOrDefault()
                    };

                    _context.HER_ConfiguracionUsuario.Add(configAdmin);
                    result = await _context.SaveChangesAsync();
                }
            }
            else if (ConstRol.RolUsuario.Contains(viewModel.Rol))
            {
                //Usuarios normales (Titular o usuario)
                if (usuario == null)
                {
                    var userNormal = new HER_Usuario
                    {
                        SecurityStamp = "UV",
                        UserName = viewModel.NombreUsuario,
                        Email = viewModel.Correo,
                        HER_AceptoTerminos = false,
                        HER_Aprobado = true,
                        HER_FechaAprobado = DateTime.Now,
                        HER_NombreCompleto = viewModel.Nombre
                    };
                    var resultNormal = await _userManager.CreateAsync(userNormal, string.Format("{0}{1}", ConstMasterKey.Key1, viewModel.NombreUsuario));

                    if (resultNormal.Succeeded)
                    {
                        //Asociacion del usuario con el rol
                        await _userManager.AddToRoleAsync(userNormal, viewModel.Rol);
                    }

                    //Crear la configuración por defecto para el usuario
                    var configNormal = new HER_ConfiguracionUsuario()
                    {
                        HER_Notificaciones = true,
                        HER_ElementosPorPagina = 25,
                        HER_UsuarioId = userNormal.Id,
                        HER_ColorId = _context.HER_Color.Where(x => x.HER_Nombre == ConstColor.ColorT1).Select(x => x.HER_ColorId).FirstOrDefault()
                    };
                    _context.HER_ConfiguracionUsuario.Add(configNormal);

                    //Crea InfoUsuarios
                    var infoNormal = new HER_InfoUsuario()
                    {
                        HER_NombreCompleto = viewModel.Nombre,
                        HER_Correo = viewModel.Correo,
                        HER_UserName = viewModel.NombreUsuario,
                        HER_Activo = true,
                        HER_EstaEnReasignacion = false,
                        HER_EstaEnBajaDefinitiva = false,
                        HER_FechaRegistro = DateTime.Now,
                        HER_Telefono = viewModel.Telefono,
                        HER_Direccion = viewModel.Direccion,
                        HER_FechaActualizacion = DateTime.Now,
                        HER_AreaId = int.Parse(viewModel.AreaId),
                        HER_Puesto = viewModel.Puesto,
                        HER_EsUnico = viewModel.EsUnico,
                        HER_RolNombre = viewModel.Rol,
                        HER_UsuarioId = userNormal.Id,
                        HER_Titular = (viewModel.EsTitular) ? viewModel.NombreUsuario : viewModel.Titular,
                        HER_BandejaUsuario = viewModel.NombreUsuario,
                        HER_BandejaPermiso = ConstDelegar.TipoN1,
                        HER_BandejaNombre = viewModel.Nombre,
                        HER_PermisoAA = viewModel.Permiso
                    };
                    _context.HER_InfoUsuario.Add(infoNormal);

                    result = await _context.SaveChangesAsync();
                }
                else {
                    //Crea InfoUsuarios
                    var infoNormal = new HER_InfoUsuario()
                    {
                        HER_NombreCompleto = viewModel.Nombre,
                        HER_Correo = viewModel.Correo,
                        HER_UserName = viewModel.NombreUsuario,
                        HER_Activo = true,
                        HER_EstaEnReasignacion = false,
                        HER_EstaEnBajaDefinitiva = false,
                        HER_FechaRegistro = DateTime.Now,
                        HER_Telefono = viewModel.Telefono,
                        HER_Direccion = viewModel.Direccion,
                        HER_FechaActualizacion = DateTime.Now,
                        HER_AreaId = int.Parse(viewModel.AreaId),
                        HER_Puesto = viewModel.Puesto,
                        HER_EsUnico = viewModel.EsUnico,
                        HER_RolNombre = viewModel.Rol,
                        HER_UsuarioId = usuario.Id,
                        HER_Titular = (viewModel.EsTitular) ? viewModel.NombreUsuario : viewModel.Titular,
                        HER_BandejaUsuario = viewModel.NombreUsuario,
                        HER_BandejaPermiso = ConstDelegar.TipoN1,
                        HER_BandejaNombre = viewModel.Nombre,
                        HER_PermisoAA = viewModel.Permiso
                    };
                    _context.HER_InfoUsuario.Add(infoNormal);

                    result = await _context.SaveChangesAsync();
                }
            }

            return result > 0 ? true : false;
        }
        public async Task<bool> AdminGuardarUsuarioAsync(AdminUsuariosCrearViewModel viewModel)
        {
            int result = 0;

            var usuarioQuery = _context.HER_Usuario
                .Where(x => x.UserName == viewModel.NombreUsuario
                        && !x.HER_Usuarios.Any(y => y.HER_Activo == true))
                .AsQueryable();

            var usuario = await usuarioQuery.FirstOrDefaultAsync();

            if (usuario != null)
            {
                usuario.Email = viewModel.Correo;
                usuario.HER_NombreCompleto = viewModel.Nombre;
                usuario.HER_Aprobado = true;
                usuario.HER_FechaAprobado = DateTime.Now;
                usuario.HER_AceptoTerminos = false;
                usuario.HER_FechaAceptoTerminos = DateTime.Now;

                await _userManager.RemoveFromRolesAsync(usuario, await _userManager.GetRolesAsync(usuario));
                await _userManager.AddToRoleAsync(usuario, viewModel.Rol);
                await _userManager.UpdateAsync(usuario);
            }

            if (ConstRol.RolUsuario.Contains(viewModel.Rol))
            {
                //Usuarios normales (Titular o usuario)
                if (usuario == null)
                {
                    var userNormal = new HER_Usuario
                    {
                        SecurityStamp = "UV",
                        UserName = viewModel.NombreUsuario,
                        Email = viewModel.Correo,
                        HER_AceptoTerminos = false,
                        HER_Aprobado = true,
                        HER_FechaAprobado = DateTime.Now,
                        HER_NombreCompleto = viewModel.Nombre
                    };
                    var resultNormal = await _userManager.CreateAsync(userNormal, string.Format("{0}{1}", ConstMasterKey.Key1, viewModel.NombreUsuario));

                    if (resultNormal.Succeeded)
                    {
                        //Asociacion del usuario con el rol
                        await _userManager.AddToRoleAsync(userNormal, viewModel.Rol);
                    }

                    //Crear la configuración por defecto para el usuario
                    var configNormal = new HER_ConfiguracionUsuario()
                    {
                        HER_Notificaciones = true,
                        HER_ElementosPorPagina = 25,
                        HER_UsuarioId = userNormal.Id,
                        HER_ColorId = _context.HER_Color.Where(x => x.HER_Nombre == ConstColor.ColorT1).Select(x => x.HER_ColorId).FirstOrDefault()
                    };
                    _context.HER_ConfiguracionUsuario.Add(configNormal);

                    //Crea InfoUsuarios
                    var infoNormal = new HER_InfoUsuario()
                    {
                        HER_NombreCompleto = viewModel.Nombre,
                        HER_Correo = viewModel.Correo,
                        HER_UserName = viewModel.NombreUsuario,
                        HER_Activo = true,
                        HER_EstaEnReasignacion = false,
                        HER_EstaEnBajaDefinitiva = false,
                        HER_FechaRegistro = DateTime.Now,
                        HER_Telefono = viewModel.Telefono,
                        HER_Direccion = viewModel.Direccion,
                        HER_FechaActualizacion = DateTime.Now,
                        HER_AreaId = int.Parse(viewModel.AreaId),
                        HER_Puesto = viewModel.Puesto,
                        HER_EsUnico = viewModel.EsUnico,
                        HER_RolNombre = viewModel.Rol,
                        HER_UsuarioId = userNormal.Id,
                        HER_Titular = (viewModel.EsTitular) ? viewModel.NombreUsuario : viewModel.Titular,
                        HER_BandejaUsuario = viewModel.NombreUsuario,
                        HER_BandejaPermiso = ConstDelegar.TipoN1,
                        HER_BandejaNombre = viewModel.Nombre,
                        HER_PermisoAA = viewModel.Permiso
                    };
                    _context.HER_InfoUsuario.Add(infoNormal);

                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    //Crea InfoUsuarios
                    var infoNormal = new HER_InfoUsuario()
                    {
                        HER_NombreCompleto = viewModel.Nombre,
                        HER_Correo = viewModel.Correo,
                        HER_UserName = viewModel.NombreUsuario,
                        HER_Activo = true,
                        HER_EstaEnReasignacion = false,
                        HER_EstaEnBajaDefinitiva = false,
                        HER_FechaRegistro = DateTime.Now,
                        HER_Telefono = viewModel.Telefono,
                        HER_Direccion = viewModel.Direccion,
                        HER_FechaActualizacion = DateTime.Now,
                        HER_AreaId = int.Parse(viewModel.AreaId),
                        HER_Puesto = viewModel.Puesto,
                        HER_EsUnico = viewModel.EsUnico,
                        HER_RolNombre = viewModel.Rol,
                        HER_UsuarioId = usuario.Id,
                        HER_Titular = (viewModel.EsTitular) ? viewModel.NombreUsuario : viewModel.Titular,
                        HER_BandejaUsuario = viewModel.NombreUsuario,
                        HER_BandejaPermiso = ConstDelegar.TipoN1,
                        HER_BandejaNombre = viewModel.Nombre,
                        HER_PermisoAA = viewModel.Permiso
                    };
                    _context.HER_InfoUsuario.Add(infoNormal);

                    result = await _context.SaveChangesAsync();
                }
            }

            return result > 0 ? true : false;
        }

        public async Task<BajaUsuarioViewModel> ObtenerInfoUsuarioParaBajaAsync(string username, InfoConfigUsuarioViewModel info)
        {
            var infoUsuarioQuery = _context.HER_InfoUsuario
               .Where(x => x.HER_UserName == username 
                        && x.HER_Activo == true 
                        && x.HER_EstaEnReasignacion == false)
               //.Include(x => x.HER_Area)
               //     .ThenInclude(x => x.HER_Region)
               //.Include(x => x.HER_Puesto)
               .AsNoTracking()
               .Select(x => new BajaUsuarioViewModel()
               {
                   InfoUsuarioClaims = info,
                   Nombre = x.HER_NombreCompleto,
                   NombreUsuario = x.HER_UserName,
                   Correo = x.HER_Correo,
                   RegionNombre = x.HER_Area.HER_Region.HER_Nombre,
                   AreaNombre = x.HER_Area.HER_Nombre,
                   PuestoNombre = x.HER_Puesto,
                   RolNombre = x.HER_RolNombre,
                   InfoUsuarioId = x.HER_InfoUsuarioId,
                   RegionId = x.HER_Area.HER_RegionId,
                   AreaId = x.HER_AreaId,
                   EsUnico = x.HER_EsUnico ? "Si" : "No"
               })
               .AsQueryable();


            return await infoUsuarioQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ExisteUsuarioSinReasignarAsync(string username)
        {
            var infoUsuarioQuery = _context.HER_InfoUsuario
               .Where(x => x.HER_UserName == username
                        && x.HER_Activo == true
                        && x.HER_EstaEnReasignacion == false)
               .AsNoTracking()
               .AsQueryable();

            return await infoUsuarioQuery.AnyAsync();
        }
        public async Task<bool> ExisteUsuarioSinReasignarPorIdAsync(int infoUsuarioId)
        {
            var infoUsuarioQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Activo == true
                         && x.HER_EstaEnReasignacion == false)
                .AsNoTracking()
                .AsQueryable();

            return await infoUsuarioQuery.AnyAsync();
        }

        public async Task<bool> GuardarReasignacion(ReasignarUsuarioViewModel viewModel) 
        {
            int result = 0;

            //Buscar usuario
            var usuarioQuery = _context.HER_Usuario
                .Where(x => x.UserName == viewModel.NombreUsuario)
                .AsQueryable();

            var usuario = await usuarioQuery.FirstOrDefaultAsync();

            if (usuario != null)
            {
                //Cambiar el rol
                var roles = await _userManager.GetRolesAsync(usuario);
                await _userManager.RemoveFromRolesAsync(usuario, roles);
                await _userManager.AddToRoleAsync(usuario, viewModel.Rol);
                //--
                usuario.Email = viewModel.Correo;
                usuario.NormalizedEmail = viewModel.Correo.ToUpper();
                usuario.HER_NombreCompleto = viewModel.Nombre;
                usuario.HER_AceptoTerminos = false;
                //--
                await _userManager.UpdateAsync(usuario);

                //Actualizar
                var infoUsuarioQuery = _context.HER_InfoUsuario
                    .Where(x => x.HER_UsuarioId == usuario.Id
                             && x.HER_Activo == true
                             && x.HER_EstaEnReasignacion == true)
                    .AsQueryable();

                var infoUsuario = await infoUsuarioQuery.FirstOrDefaultAsync();

                infoUsuario.HER_Activo = false;
                infoUsuario.HER_EstaEnReasignacion = false;
                infoUsuario.HER_FechaActualizacion = DateTime.Now;
                infoUsuario.HER_PermisoAA = false;

                _context.HER_InfoUsuario.Update(infoUsuario).State = EntityState.Modified;
                result += await _context.SaveChangesAsync();

                //Guardando info usuario
                var nuevoInfoUsuario = new HER_InfoUsuario()
                {
                    HER_NombreCompleto = viewModel.Nombre,
                    HER_Correo = viewModel.Correo,
                    HER_UserName = viewModel.NombreUsuario,
                    HER_Activo = true,
                    HER_EstaEnReasignacion = false,
                    HER_EstaEnBajaDefinitiva = false,
                    HER_FechaRegistro = DateTime.Now,
                    HER_FechaActualizacion = DateTime.Now,
                    HER_Direccion = viewModel.Direccion,
                    HER_Telefono = viewModel.Telefono,
                    HER_AreaId = int.Parse(viewModel.AreaId),
                    HER_Puesto = viewModel.Puesto,
                    HER_RolNombre = viewModel.Rol,
                    HER_Titular = (viewModel.EsTitular) ? viewModel.NombreUsuario : viewModel.Titular,
                    HER_UsuarioId = usuario.Id,
                    HER_BandejaPermiso = ConstDelegar.TipoN1,
                    HER_BandejaUsuario = viewModel.NombreUsuario,
                    HER_BandejaNombre = viewModel.Nombre,
                    HER_PermisoAA = viewModel.Permiso
                };

                _context.HER_InfoUsuario.Add(nuevoInfoUsuario);
                result +=  await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }
        public async Task<bool> AdminGuardarReasignacion(AdminReasignarUsuarioViewModel viewModel)
        {
            int result = 0;

            //Buscar usuario
            var usuarioQuery = _context.HER_Usuario
                .Where(x => x.UserName == viewModel.NombreUsuario)
                .AsQueryable();

            var usuario = await usuarioQuery.FirstOrDefaultAsync();

            if (usuario != null)
            {
                //Cambiar el rol
                var roles = await _userManager.GetRolesAsync(usuario);
                await _userManager.RemoveFromRolesAsync(usuario, roles);
                await _userManager.AddToRoleAsync(usuario, viewModel.Rol);
                //--
                usuario.Email = viewModel.Correo;
                usuario.NormalizedEmail = viewModel.Correo.ToUpper();
                usuario.HER_NombreCompleto = viewModel.Nombre;
                usuario.HER_AceptoTerminos = false;
                //--
                await _userManager.UpdateAsync(usuario);

                //Actualizar
                var infoUsuarioQuery = _context.HER_InfoUsuario
                    .Where(x => x.HER_UsuarioId == usuario.Id
                             && x.HER_Activo == true
                             && x.HER_EstaEnReasignacion == true)
                    .AsQueryable();

                var infoUsuario = await infoUsuarioQuery.FirstOrDefaultAsync();

                infoUsuario.HER_Activo = false;
                infoUsuario.HER_EstaEnReasignacion = false;
                infoUsuario.HER_FechaActualizacion = DateTime.Now;
                infoUsuario.HER_PermisoAA = false;

                _context.HER_InfoUsuario.Update(infoUsuario).State = EntityState.Modified;
                result += await _context.SaveChangesAsync();

                //Guardando info usuario
                var nuevoInfoUsuario = new HER_InfoUsuario()
                {
                    HER_NombreCompleto = viewModel.Nombre,
                    HER_Correo = viewModel.Correo,
                    HER_UserName = viewModel.NombreUsuario,
                    HER_Activo = true,
                    HER_EstaEnReasignacion = false,
                    HER_EstaEnBajaDefinitiva = false,
                    HER_FechaRegistro = DateTime.Now,
                    HER_FechaActualizacion = DateTime.Now,
                    HER_Direccion = viewModel.Direccion,
                    HER_Telefono = viewModel.Telefono,
                    HER_AreaId = int.Parse(viewModel.AreaId),
                    HER_Puesto = viewModel.Puesto,
                    HER_EsUnico = viewModel.EsUnico,
                    HER_RolNombre = viewModel.Rol,
                    HER_Titular = (viewModel.EsTitular) ? viewModel.NombreUsuario : viewModel.Titular,
                    HER_UsuarioId = usuario.Id,
                    HER_BandejaPermiso = ConstDelegar.TipoN1,
                    HER_BandejaUsuario = viewModel.NombreUsuario,
                    HER_BandejaNombre = viewModel.Nombre,
                    HER_PermisoAA = viewModel.Permiso
                };

                _context.HER_InfoUsuario.Add(nuevoInfoUsuario);
                result += await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }
        public async Task<bool> DarDeBajaUsuarioAsync(BajaUsuarioViewModel bajaViewModel, IEnumerable<string> roles)
        {
            var infoUsuarioQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_InfoUsuarioId == bajaViewModel.InfoUsuarioId 
                         && x.HER_Activo == true
                         && x.HER_EstaEnReasignacion == false)
                .Include(x => x.HER_Usuario)
                .AsQueryable();

            var infoUsuario = await infoUsuarioQuery.FirstOrDefaultAsync();
            var usuario = infoUsuario.HER_Usuario;
            int result = 0;

            if (Int32.Parse(bajaViewModel.TipoBaja) == ConstTipoDeBaja.TipoDeBajaN1)
            {
                usuario.HER_Aprobado = true;

                if (!roles.Contains(ConstRol.Rol8T))
                { 
                    await _userManager.RemoveFromRolesAsync(usuario, roles);
                    await _userManager.AddToRoleAsync(usuario, ConstRol.Rol8T);
                }

                await _userManager.UpdateAsync(usuario);

                //Actualizar
                infoUsuario.HER_Activo = false;
                infoUsuario.HER_EstaEnReasignacion = false;
                infoUsuario.HER_FechaActualizacion = DateTime.Now;

                _context.HER_InfoUsuario.Update(infoUsuario).State = EntityState.Modified;

                //-----------------------------------
                //Crear el usuario con el nuevo estado
                var nuevoInfoUsuario = new HER_InfoUsuario()
                {
                    HER_NombreCompleto = infoUsuario.HER_NombreCompleto,
                    HER_Correo = infoUsuario.HER_Correo,
                    HER_UserName = infoUsuario.HER_UserName,
                    HER_Direccion = infoUsuario.HER_Direccion,
                    HER_Telefono = infoUsuario.HER_Telefono,
                    HER_FechaRegistro = DateTime.Now,
                    HER_FechaActualizacion = DateTime.Now,
                    HER_Activo = true,
                    HER_EstaEnReasignacion = true,
                    HER_EstaEnBajaDefinitiva = false,
                    HER_Titular = infoUsuario.HER_UserName,
                    HER_AreaId = infoUsuario.HER_AreaId,
                    HER_Puesto = "Sin definir",
                    HER_RolNombre = ConstRol.Rol8T,
                    HER_UsuarioId = infoUsuario.HER_UsuarioId,
                    HER_BandejaPermiso = ConstDelegar.TipoN1,
                    HER_BandejaUsuario = infoUsuario.HER_UserName,
                    HER_BandejaNombre = infoUsuario.HER_NombreCompleto
                };

                _context.HER_InfoUsuario.Add(nuevoInfoUsuario);
            }
            else {
                infoUsuario.HER_Activo = false;
                infoUsuario.HER_EstaEnReasignacion = false;
                infoUsuario.HER_EstaEnBajaDefinitiva = true;
                infoUsuario.HER_FechaActualizacion = DateTime.Now;
                _context.HER_InfoUsuario.Update(infoUsuario).State = EntityState.Modified;
            }

            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
        public async Task<bool> UsuarioEstaAprobadoAsync(string username)
        {
            var userQuery = _context.HER_Usuario
                        .Where(x => x.UserName.Equals(username)
                                 && x.HER_Aprobado == true)
                        .AsNoTracking()
                        .AsQueryable();

            return await userQuery.AnyAsync();
        }
        public async Task<bool> UsuarioAceptoTerminosAsync(string username)
        {
            var userQuery = _context.HER_Usuario
                        .Where(x => x.UserName.Equals(username)
                                 && x.HER_AceptoTerminos == true)
                        .AsNoTracking()
                        .AsQueryable();

            return await userQuery.AnyAsync();
        }
        public async Task<HER_Usuario> ObtenerUsuarioAsync(string username)
        {
            var userQuery = _context.HER_Usuario
                        .Where(x => x.UserName.Equals(username))
                        .AsQueryable();

            return await userQuery.FirstOrDefaultAsync();
        }
        public async Task<SolicitudUsuarioViewModel> ObtenerUsuarioADSolicitudAsync(string username)
        {
            UsuarioADViewModel infoUsuario = await _clientService.ObtenerInfoUsuarioADAsync(username);

            SolicitudUsuarioViewModel solicitudParcial = new SolicitudUsuarioViewModel()
            {
                NombreUsuario = infoUsuario.HER_Usuario_Username,
                NombreCompleto = infoUsuario.HER_Usuario_NombreCompleto,
                Correo = infoUsuario.HER_Usuario_Correo
            };

            return solicitudParcial;
        }
        public async Task<bool> GuardarUsuarioConSolicitudAsync(bool existeUsuario, bool existeInfoUsuario, bool estaActivo, InfoUsuarioOracleViewModel solicitud, string direccion, string telefono)
        {
            int result = 0;

            if (!existeUsuario && !existeInfoUsuario)
            {
                //Prepara el nuevo usuario
                var usuario = new HER_Usuario
                {
                    UserName = solicitud.Usuario,
                    HER_NombreCompleto = solicitud.Nombre,
                    Email = solicitud.Correo,
                    HER_Aprobado = false,
                    HER_AceptoTerminos = false,
                    SecurityStamp = "UV",
                };

                //Registra las credenciales del usuario
                var response = await _userManager.CreateAsync(usuario, string.Format("{0}{1}", ConstMasterKey.Key1, solicitud.Usuario));

                if (response.Succeeded)
                {
                    //Asigna las credenciales
                    await _userManager.AddToRoleAsync(usuario, ConstRol.Rol8T);

                    //Prepara la información completa del usuario
                    var infoUsuario = new HER_InfoUsuario()
                    {
                        HER_NombreCompleto = solicitud.Nombre,
                        HER_Correo = solicitud.Correo,
                        HER_UserName = solicitud.Usuario,
                        HER_Activo = false,
                        HER_EstaEnReasignacion = false,
                        HER_EstaEnBajaDefinitiva = false,
                        HER_FechaRegistro = DateTime.Now,
                        HER_Direccion = direccion,
                        HER_Telefono = telefono,
                        HER_FechaActualizacion = DateTime.Now,
                        HER_AreaId = int.Parse(solicitud.AreaId),
                        HER_Puesto = solicitud.Puesto,
                        HER_EsUnico = false,
                        HER_RolNombre = ConstRol.Rol8T,
                        HER_UsuarioId = usuario.Id,
                        HER_Titular = solicitud.Usuario,
                        HER_BandejaUsuario = string.Empty,
                        HER_BandejaPermiso = 0,
                        HER_BandejaNombre = string.Empty
                    };

                    //Registra la información del usuario
                    _context.HER_InfoUsuario.Add(infoUsuario);
                    result = await _context.SaveChangesAsync();
                }
            }
            else if (existeUsuario && existeInfoUsuario && !estaActivo)
            {
                var infoUsuarioQuery = _context.HER_Usuario
                    .Where(x => x.UserName == solicitud.Usuario)
                    .AsQueryable();

                var usuario = await infoUsuarioQuery.FirstOrDefaultAsync();
                if (usuario != null)
                {
                    usuario.UserName = solicitud.Usuario;
                    usuario.HER_NombreCompleto = solicitud.Nombre;
                    usuario.Email = solicitud.Correo;
                    usuario.HER_Aprobado = false;
                    usuario.HER_AceptoTerminos = false;
                    usuario.SecurityStamp = "UV";

                    await _userManager.RemoveFromRolesAsync(usuario, await _userManager.GetRolesAsync(usuario));
                    await _userManager.AddToRoleAsync(usuario, ConstRol.Rol8T);
                    await _userManager.UpdateAsync(usuario);

                    //Prepara la información completa del usuario
                    var infoUsuario = new HER_InfoUsuario()
                    {
                        HER_NombreCompleto = solicitud.Nombre,
                        HER_Correo = solicitud.Correo,
                        HER_UserName = solicitud.Usuario,
                        HER_Activo = false,
                        HER_EstaEnReasignacion = false,
                        HER_FechaRegistro = DateTime.Now,
                        HER_Direccion = direccion,
                        HER_Telefono = telefono,
                        HER_FechaActualizacion = DateTime.Now,
                        HER_AreaId = int.Parse(solicitud.AreaId),
                        HER_Puesto = solicitud.Puesto,
                        HER_EsUnico = false,
                        HER_RolNombre = ConstRol.Rol8T,
                        HER_UsuarioId = usuario.Id,
                        HER_Titular = solicitud.Usuario,
                        HER_BandejaUsuario = string.Empty,
                        HER_BandejaPermiso = 0,
                        HER_BandejaNombre = string.Empty
                    };

                    //Registra la información del usuario
                    _context.HER_InfoUsuario.Add(infoUsuario);
                    result = await _context.SaveChangesAsync();
                }
            }
            
            return result > 0 ? true : false;
        }
        public async Task<string> ObtenerIdUsuarioTitularAsync(int areaId)
        {
            var usuarioTitularIdQuery = _context.HER_InfoUsuario
                    .Where(x => x.HER_AreaId == areaId
                             && x.HER_Activo == true 
                             && x.HER_RolNombre == ConstRol.Rol7T)
                    .Select(x => x.HER_UsuarioId)
                    .AsNoTracking()
                    .AsQueryable();

            return await usuarioTitularIdQuery.FirstOrDefaultAsync();
        }
        public async Task<string> ObtenerNombreUsuarioDelInfoUsuarioPorIdAsync(int infoUsuarioId)
        {
            var usuarioIdQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_InfoUsuarioId == infoUsuarioId)
                .Select(x => x.HER_UserName)
                .AsQueryable();

            return await usuarioIdQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> EsTipoAdministradorAsync(string userName)
        {
            var usuarioEsAdministradorQuery = _context.HER_Usuario
                .Where(x => x.UserName.Equals(userName) && x.HER_Usuarios.Count() == 0)
                .AsNoTracking()
                .AsQueryable();

            return await usuarioEsAdministradorQuery.AnyAsync();
        }
        //Termino
        public async Task<bool> GuardarAceptacionTerminos(string username)
        {
            int result = 0;

            var usuarioQuery = _context.HER_Usuario
                .Where(x => x.UserName == username)
                .AsQueryable();

            var usuario = await usuarioQuery.FirstOrDefaultAsync();
            if (usuario != null)
            {
                usuario.HER_AceptoTerminos = true;
                usuario.HER_FechaAceptoTerminos = DateTime.Now;

                _context.HER_Usuario.Update(usuario).State = EntityState.Modified;
                result = await _context.SaveChangesAsync();
            }

            return (result > 0) ? true : false;
        }
        
        //Detecta cuando un usuario tiene dos cuentas una como titular y otra como usuario,
        public async Task<bool> DetectaCuentaDependenciaAsync(string username)
        {
            bool tieneCuentaTitular = false;

            var usuarioQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_UserName == username
                         && x.HER_Activo == true)
                .Select(x => new UsuarioDetectaCuentaViewModel
                {
                    HER_Titular = x.HER_Titular,
                    HER_Rol_Nombre = x.HER_RolNombre
                })
                .AsNoTracking()
                .AsQueryable();
            //--
            var usuario = await usuarioQuery.FirstOrDefaultAsync();

            if (usuario.HER_Rol_Nombre == ConstRol.Rol8T)
            {
                var tieneCuentaTitularQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Titular == usuario.HER_Titular
                    && x.HER_RolNombre == ConstRol.Rol7T
                    && x.HER_Activo == true)
                .AsNoTracking()
                .AsQueryable();

                tieneCuentaTitular = await tieneCuentaTitularQuery.AnyAsync();
            }

            return tieneCuentaTitular;
        }
        public async Task<string> ObtenerCuentaPersonal(string username, string rol)
        {
            string cuenta = string.Empty;

            //Obtiene la cuenta dependencia en caso de que el usuario tenga una cuenta relacionada
            if (rol == ConstRol.Rol7T)
            {
                var titularQuery = _context.HER_InfoUsuario
                    .Where(x => x.HER_UserName == username 
                             && x.HER_Activo == true)
                    .Select(x => x.HER_Titular )
                    .AsNoTracking()
                    .AsQueryable();
                //--
                var titular = await titularQuery.FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(titular)) {
                    //----
                    var cuentaPersonalQuery = _context.HER_InfoUsuario
                        .Where(x => x.HER_Titular == titular
                                 && x.HER_RolNombre == ConstRol.Rol8T
                                 && x.HER_Activo == true)
                        .Select(x => x.HER_UserName)
                        .AsNoTracking()
                        .AsQueryable();

                    var cuentaPersonal = await cuentaPersonalQuery.FirstOrDefaultAsync();
                    //--
                    if (!string.IsNullOrEmpty(cuentaPersonal))
                    {
                        cuenta = cuentaPersonal;
                    }
                }
            }

            return cuenta;
        }
        //Obtener información del usuario para llenar los Claims
        public async Task<InfoUsuarioNormalClaims> ObtieneInfoUsuarioNormalParaClaims(string userId, string username, string cuentaDependencia)
        {
            var fecha = DateTime.Now;
            var info = new InfoUsuarioNormalClaims();
            var tieneCuentaDependencia = !string.IsNullOrEmpty(cuentaDependencia);
            //--
            IQueryable<InfoComplementariaClaims> infoCuentaDependenciaQuery;
            InfoComplementariaClaims infoCuentaDependencia = new InfoComplementariaClaims();

            if (tieneCuentaDependencia) 
            {
                infoCuentaDependenciaQuery = _context.HER_InfoUsuario
                    .Where(a => a.HER_Activo == true && a.HER_UserName == cuentaDependencia)
                    .Select(a => new InfoComplementariaClaims {
                        HER_RegionId = a.HER_Area.HER_RegionId.ToString(),
                        HER_AreaId = a.HER_AreaId.ToString(),
                        HER_Puesto = a.HER_Puesto,
                        HER_Puesto_EsUnico = a.HER_EsUnico ? "1" : "0"
                    })
                    .AsNoTracking()
                    .AsQueryable();

                infoCuentaDependencia = await infoCuentaDependenciaQuery.FirstOrDefaultAsync();
            }

            var infoQuery = _context.HER_InfoUsuario
                    //.Include(x => x.HER_Area)
                    //    .ThenInclude(x => x.HER_Region)
                    //.Include(x => x.HER_Titulares)
                    //    .ThenInclude(x => x.HER_Titular)
                    //        .ThenInclude(x => x.HER_Area)
                    //.Include(x => x.HER_Titulares)
                    //    .ThenInclude(x => x.HER_Titular)
                    .Where(x => x.HER_UsuarioId == userId
                                && x.HER_Activo == true)
                    .Select(x => new InfoUsuarioNormalClaims
                    {
                        InfoUsuarioId = x.HER_InfoUsuarioId.ToString(),
                        Username = x.HER_UserName,
                        RegionNombre = x.HER_Area.HER_Region.HER_Nombre,
                        RegionId = x.HER_Area.HER_RegionId.ToString(),
                        AreaClave = x.HER_Area.HER_Clave,
                        AreaNombre = x.HER_Area.HER_Nombre,
                        AreaId = x.HER_AreaId.ToString(),
                        PuestoNombre = x.HER_Puesto,
                        PuestoEsUnico = (x.HER_EsUnico) ? "1" : "0",
                        //--
                        BandejaUsuario = x.HER_BandejaUsuario,
                        BandejaPermiso = x.HER_BandejaPermiso.ToString(),
                        BandejaNombre = x.HER_BandejaNombre,
                        
                        //---
                        BandejaRegionId = (!tieneCuentaDependencia)? (x.HER_BandejaUsuario == x.HER_UserName) ? x.HER_Area.HER_RegionId.ToString() 
                                                                                                                : (from a in x.HER_Titulares
                                                                                                                   where a.HER_Titular.HER_Activo == true && a.HER_Titular.HER_UserName == x.HER_BandejaUsuario
                                                                                                                   select a.HER_Titular.HER_Area.HER_RegionId.ToString()).FirstOrDefault() 
                                                                    : infoCuentaDependencia.HER_RegionId,

                        BandejaAreaId = (!tieneCuentaDependencia) ? (x.HER_BandejaUsuario == x.HER_UserName) ? x.HER_AreaId.ToString() : (from a in x.HER_Titulares
                                                                                                               where a.HER_Titular.HER_Activo == true && a.HER_Titular.HER_UserName == x.HER_BandejaUsuario
                                                                                                               select a.HER_Titular.HER_AreaId.ToString()).FirstOrDefault()
                                                                    : infoCuentaDependencia.HER_AreaId,

                        BandejaPuesto = (!tieneCuentaDependencia) ?  (x.HER_BandejaUsuario == x.HER_UserName) ? x.HER_Puesto : (from a in x.HER_Titulares
                                                                                                                   where a.HER_Titular.HER_Activo == true && a.HER_Titular.HER_UserName == x.HER_BandejaUsuario
                                                                                                                   select a.HER_Titular.HER_Puesto).FirstOrDefault()
                                                                    : infoCuentaDependencia.HER_Puesto,

                        BandejaPuestoEsUnico = (!tieneCuentaDependencia) ? (x.HER_BandejaUsuario == x.HER_UserName) ? (x.HER_EsUnico) ? "1" : "0" : (from a in x.HER_Titulares
                                                                                                                                     where a.HER_Titular.HER_Activo == true && a.HER_Titular.HER_UserName == x.HER_BandejaUsuario
                                                                                                                                     select a.HER_Titular.HER_EsUnico).FirstOrDefault() ? "1" : "0"
                                                                    : infoCuentaDependencia.HER_Puesto_EsUnico,
                        //---
                        Session = (x.HER_BandejaUsuario == x.HER_UserName) ? string.Format("{0}_{1}{2}{3}", x.HER_BandejaUsuario, fecha.Year, fecha.Month, fecha.Day) : string.Format("{0}_{1}_{2}{3}{4}", x.HER_BandejaUsuario, username, fecha.Year, fecha.Month, fecha.Day),
                        //--
                        ActivaDelegacion = (x.HER_BandejaUsuario == x.HER_UserName) ? "0" : "1",
                        //--
                        Rol = x.HER_RolNombre,
                        Titular = x.HER_Titular,
                        PermisoAA = x.HER_PermisoAA ? "1": "0",
                        EnReasignacion = x.HER_EstaEnReasignacion ? "1" : "0"
                    })
                    .AsNoTracking()
                    .AsQueryable();

            info = await infoQuery.FirstOrDefaultAsync();

            return info;
        }
        public async Task<PermisoAdminAreaViewModel> ObtenerInfoUsuarioParaPermisoAsync(string username, InfoConfigUsuarioViewModel info)
        {
            var infoUsuarioQuery = _context.HER_InfoUsuario
               .Where(x => x.HER_UserName == username
                        && x.HER_Activo == true)
               //.Include(x => x.HER_Area)
               //     .ThenInclude(x => x.HER_Region)
               .AsNoTracking()
               .Select(x => new PermisoAdminAreaViewModel()
               {
                   InfoUsuarioClaims = info,
                   Nombre = x.HER_NombreCompleto,
                   NombreUsuario = x.HER_UserName,
                   Correo = x.HER_Correo,
                   RegionNombre = x.HER_Area.HER_Region.HER_Nombre,
                   AreaNombre = x.HER_Area.HER_Nombre,
                   PuestoNombre = x.HER_Puesto,
                   EsUnico = x.HER_EsUnico? "Si": "No",
                   RolNombre = x.HER_RolNombre,
                   InfoUsuarioId = x.HER_InfoUsuarioId,
                   RegionId = x.HER_Area.HER_RegionId,
                   AreaId = x.HER_AreaId,
                   Permiso = x.HER_PermisoAA
               })
               .AsQueryable();

            return await infoUsuarioQuery.FirstOrDefaultAsync();
        }

        public async Task<bool> GuardarPermisoUsuarioAsync(PermisoAdminAreaViewModel viewModel)
        {
            int result = 0;
            var infoUsuarioQuery = _context.HER_InfoUsuario
               .Where(x => x.HER_InfoUsuarioId == viewModel.InfoUsuarioId
                        && x.HER_Activo == true)
               .AsQueryable();

            var infoUsuario = await infoUsuarioQuery.FirstOrDefaultAsync();
            //--
            infoUsuario.HER_PermisoAA = viewModel.Permiso;
            _context.HER_InfoUsuario.Update(infoUsuario).State = EntityState.Modified;
            //--
            result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> ExisteAdministradorArea(int areaId)
        {
            var usuariosAdminAreaQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Activo == true
                        && x.HER_EstaEnReasignacion == false
                        && x.HER_AreaId == areaId
                        && x.HER_Area.HER_Estado == ConstEstadoArea.EstadoN1
                        && x.HER_PermisoAA == true)
                .AsTracking()
                .AsQueryable();

            var usuariosAdminArea = await usuariosAdminAreaQuery.CountAsync();

            return usuariosAdminArea >= 1;
        }
    }
}
