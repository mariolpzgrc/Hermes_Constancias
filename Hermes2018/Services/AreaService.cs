using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Anexo;
using Hermes2018.Models.Area;
using Hermes2018.ModelsDBF;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{

    public class AreaService: IAreaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        public readonly DBFContext _DBFContext;
        private readonly IUsuarioService _usuarioService;
        private readonly UserManager<HER_Usuario> _userManager;
        private readonly ILogger<AreaService> _logger;

        public AreaService(ApplicationDbContext context,
                IHostingEnvironment environment,
                DBFContext DBFContext,
                UserManager<HER_Usuario> userManager, 
                IUsuarioService usuarioService,
                ILogger<AreaService> logger)
        {
            _context = context;
            _environment = environment;
            _DBFContext = DBFContext;
            _userManager = userManager;
            _usuarioService = usuarioService;
            _logger = logger;
        }
        public async Task<List<HER_Area>> ObtenerAreaEnListaAsync(int areaId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_Estado == ConstEstadoArea.EstadoN1
                         && x.HER_BajaInactividad == false)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.ToListAsync();
        }

        public async Task<List<HER_Area>> ObtenerAreasAsync(int regionId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_RegionId == regionId 
                         && x.HER_Estado == ConstEstadoArea.EstadoN1
                         && x.HER_BajaInactividad == false)
                .OrderBy(x => x.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.ToListAsync();
        }
        public async Task<List<HER_Area>> ObtenerAreasAsync(int regionId, int areaId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_RegionId == regionId 
                         && x.HER_AreaId != areaId
                         && x.HER_Estado == ConstEstadoArea.EstadoN1
                         && x.HER_BajaInactividad == false)
                .OrderBy(x => x.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.ToListAsync();
        }
        public async Task<List<HER_Area>> ObtenerAreasVisibleAsync(int regionId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_RegionId == regionId 
                         && x.HER_Visible == true
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .OrderBy(x => x.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.ToListAsync();
        }
        public async Task<List<HER_Area>> ObtenerAreasVisibleAsync(int regionId, int areaId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_RegionId == regionId 
                         && x.HER_Visible == true
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1
                         && x.HER_AreaId != areaId)
                .OrderBy(x => x.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.ToListAsync();
        }
        public async Task<List<AreaViewModel>> ObtenerAreasPorRegionPorUsuarioAsync(int regionId, string userName)
        {
            //Obtener el rol del usuario
            var rol = await _usuarioService.ObtenerRolUsuarioAsync(userName);
            var infoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(userName);

            //Areas
            IQueryable<AreaViewModel> areasQuery;
            List<AreaViewModel> listadoAreas = new List<AreaViewModel>();

            if (ConstRol.RolAdmin.Contains(rol))
            {
                //Administradores
                areasQuery = _context.HER_Area
                    .Where(x => x.HER_RegionId == regionId
                             && x.HER_BajaInactividad == false
                             && x.HER_Estado == ConstEstadoArea.EstadoN1)
                    .AsNoTracking()
                    .OrderBy(x => x.HER_Nombre)
                    .Select(x => new AreaViewModel {
                        HER_AreaId = x.HER_AreaId,
                        HER_Nombre = x.HER_Nombre,
                        HER_Direccion = x.HER_Direccion,
                        HER_Telefono = x.HER_Telefono
                    })
                    .AsQueryable();

                listadoAreas = await areasQuery.ToListAsync();

            }
            else if (ConstRol.Rol7T == rol)
            {
                //Titular de área
                areasQuery = _context.HER_InfoUsuario
                    //.Include(x => x.HER_Area)
                    .Where(x => x.HER_InfoUsuarioId == infoUsuarioId 
                             && x.HER_Area.HER_RegionId == regionId
                             && x.HER_Area.HER_BajaInactividad == false
                             && x.HER_Area.HER_Estado == ConstEstadoArea.EstadoN1)                  
                    .Select(x => x.HER_Area)
                    .OrderBy(x => x.HER_Nombre)
                    .AsNoTracking()
                    .Select(x => new AreaViewModel
                    {
                        HER_AreaId = x.HER_AreaId,
                        HER_Nombre = x.HER_Nombre,
                        HER_Direccion = x.HER_Direccion,
                        HER_Telefono = x.HER_Telefono
                    })
                    .AsQueryable();

                listadoAreas = await areasQuery.ToListAsync();
            }

            return listadoAreas;
        }
        public async Task<List<AreaViewModel>> ObtenerAreasVisiblesPorRegionPorUsuarioAsync(int regionId, string userName)
        {
            //Obtener el rol del usuario
            var rol = await _usuarioService.ObtenerRolUsuarioAsync(userName);
            var infoUsuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(userName);

            //Areas
            IQueryable<AreaViewModel> areasQuery;
            List<AreaViewModel> listadoAreas = new List<AreaViewModel>();

            if (ConstRol.RolAdmin.Contains(rol))
            {
                //Administradores
                areasQuery = _context.HER_Area
                    .Where(x => x.HER_RegionId == regionId 
                             && x.HER_Visible == true
                             && x.HER_BajaInactividad == false
                             && x.HER_Estado == ConstEstadoArea.EstadoN1)
                    .OrderBy(x => x.HER_Nombre)
                    .AsNoTracking()
                    .Select(x => new AreaViewModel
                    {
                        HER_AreaId = x.HER_AreaId,
                        HER_Nombre = x.HER_Nombre,
                        HER_Direccion = x.HER_Direccion,
                        HER_Telefono = x.HER_Telefono
                    })
                    .AsQueryable();

                listadoAreas = await areasQuery.ToListAsync();

            }
            else if (ConstRol.RolUsuario.Contains(rol))
            {
                //Titular de área
                areasQuery = _context.HER_InfoUsuario
                    //.Include(x => x.HER_Area)
                    .Where(x => x.HER_InfoUsuarioId == infoUsuarioId
                             && x.HER_Area.HER_RegionId == regionId
                             && x.HER_Area.HER_Visible == true
                             && x.HER_Area.HER_BajaInactividad == false
                             && x.HER_Area.HER_Estado == ConstEstadoArea.EstadoN1)
                    .Select(x => x.HER_Area)
                    .OrderBy(x => x.HER_Nombre)
                    .AsNoTracking()
                    .Select(x => new AreaViewModel
                    {
                        HER_AreaId = x.HER_AreaId,
                        HER_Nombre = x.HER_Nombre,
                        HER_Direccion = x.HER_Direccion,
                        HER_Telefono = x.HER_Telefono
                    })
                    .AsQueryable();

                listadoAreas = await areasQuery.ToListAsync();
            }

            return listadoAreas;
        }
        public async Task<List<AreaViewModel>> ObtenerAreasVisiblesPorAreaPadreConHijasAsync(int areaPadreId)
        {
            //Areas
            List<AreaViewModel> listadoAreas = new List<AreaViewModel>();

            var areaPadreQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaPadreId
                         && x.HER_Visible == true
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsNoTracking()
                .Select(x => new AreaViewModel
                {
                    HER_AreaId = x.HER_AreaId,
                    HER_Nombre = x.HER_Nombre,
                    HER_Direccion = x.HER_Direccion,
                    HER_Telefono = x.HER_Telefono
                })
                .AsQueryable();

            listadoAreas.AddRange(await areaPadreQuery.ToListAsync());

            var areasQuery = _context.HER_Area
                .Where(x => x.HER_Area_PadreId == areaPadreId
                         && x.HER_Visible == true
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsNoTracking()
                .Select(x => new AreaViewModel
                {
                    HER_AreaId = x.HER_AreaId,
                    HER_Nombre = string.Format("  {0} {1}", "-", x.HER_Nombre),
                    HER_Direccion = x.HER_Direccion,
                    HER_Telefono = x.HER_Telefono
                })
                .AsQueryable();

            listadoAreas.AddRange(await areasQuery.ToListAsync());

            return listadoAreas;
        }
        //--
        public async Task<HER_Area> ObtenerAreaConRegionPorIdAsync(int areaId)
        {
            var areasQuery = _context.HER_Area
                .Include(x => x.HER_Region)
                .Where(x => x.HER_AreaId == areaId 
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<HER_Area> ObtenerAreaVisibleConRegionPorIdAsync(int areaId)
        {
            var areasQuery = _context.HER_Area
                .Include(x => x.HER_Region)
                .Where(x => x.HER_AreaId == areaId 
                         && x.HER_Visible == true
                          && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<HER_Area> ObtenerAreaConRegionPorIdPorRegionAsync(int areaId, int regionId)
        {
            var areasQuery = _context.HER_Area
                .Include(x => x.HER_Region)
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_RegionId == regionId
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<HER_Area> ObtenerAreaVisibleConRegionPorIdPorRegionAsync(int areaId, int regionId)
        {
            var areasQuery = _context.HER_Area
                .Include(x => x.HER_Region)
                .Where(x => x.HER_AreaId == areaId 
                         && x.HER_RegionId == regionId 
                         && x.HER_Visible == true
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<HER_Area> ObtenerAreaPorIdAsync(int areaId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Include(x => x.HER_Area_Padre)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<HER_Area> ObtenerAreaPorIdVisibleAsync(int areaId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_Visible == true
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<string> ObtenerNombreAreaAsync(int areaId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => x.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<string> ObtenerNombreAreaVisibleAsync(int areaId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId 
                         && x.HER_Visible == true
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => x.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<TitularAreaViewModel> ObtenerTitular(int areaId)
        {
            var areasQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_AreaId == areaId 
                    && x.HER_RolNombre == ConstRol.Rol7T
                    && x.HER_Activo == true
                    && x.HER_Area.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => new TitularAreaViewModel() {
                    NombreCompleto = x.HER_NombreCompleto,
                    Correo = x.HER_Correo
                })
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<TitularAreaViewModel> ObtenerTitularConAreaVisible(int areaId)
        {
            var areasQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_AreaId == areaId
                    && x.HER_RolNombre == ConstRol.Rol7T
                    && x.HER_Activo == true
                    && x.HER_Area.HER_Visible == true
                    && x.HER_Area.HER_BajaInactividad == false
                    && x.HER_Area.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => new TitularAreaViewModel(){
                    NombreCompleto = x.HER_NombreCompleto,
                    Correo = x.HER_Correo
                })
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        //--
        public async Task<List<ListadoAreaViewModel>> ObtenerListadoAreasAsync(int regionId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_RegionId == regionId && x.HER_Estado != ConstEstadoArea.EstadoN3)
                .Select(x => new ListadoAreaViewModel() {
                    AreaId = x.HER_AreaId,
                    Nombre = x.HER_Nombre,
                    Region = x.HER_Region.HER_Nombre,
                    TienePadre = x.HER_Area_PadreId != null? "Si" : "No",
                    Visible = x.HER_Visible ? "Si": "No",
                    AreaPadreId = x.HER_Area_PadreId != null ? (int)x.HER_Area_PadreId: 0,
                    TotalUsuarios = x.HER_Usuarios.Count(),
                    Estado = x.HER_Estado == ConstEstadoArea.EstadoN1? ConstEstadoArea.EstadoT1 : x.HER_Estado == ConstEstadoArea.EstadoN2 ? ConstEstadoArea.EstadoT2: ConstEstadoArea.EstadoT3,
                    //--
                    ExisteEnSIIU = x.HER_ExisteEnSIIU
                })
                .OrderBy(x => x.Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.ToListAsync();
        }
        public async Task<List<ListadoAreaViewModel>> ObtenerListadoAreasAsync(int regionId, int areaId)
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_RegionId == regionId && x.HER_AreaId != areaId)
                //.Include(x => x.HER_InfoUsuarios)
                //.Include(x => x.HER_Region)
                //.Include(x => x.HER_Area_Padre)
                .Select(x => new ListadoAreaViewModel()
                {
                    AreaId = x.HER_AreaId,
                    Nombre = x.HER_Nombre,
                    Region = x.HER_Region.HER_Nombre,
                    TienePadre = x.HER_Area_PadreId != null ? "Si" : "No",
                    Visible = x.HER_Visible ? "Si" : "No",
                    AreaPadreId = x.HER_Area_PadreId != null ? (int)x.HER_Area_PadreId : 0,
                    TotalUsuarios = x.HER_Usuarios.Where(y => y.HER_Activo == true).Count(),
                    Estado = x.HER_Estado == ConstEstadoArea.EstadoN1 ? ConstEstadoArea.EstadoT1 : x.HER_Estado == ConstEstadoArea.EstadoN2 ? ConstEstadoArea.EstadoT2 : ConstEstadoArea.EstadoT3,
                    //--
                    ExisteEnSIIU = x.HER_ExisteEnSIIU
                })
                .OrderBy(x => x.Nombre)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.ToListAsync();
        }
        //--
        public async Task<List<ListadoAreaPorRegionViewModel>> ObtenerListadoAreasDeAreaPadrePorRegiónAsync(int regionId, int? areaPadreId)
        {
            List<ListadoAreaPorRegionViewModel> listado = new List<ListadoAreaPorRegionViewModel>();
            IQueryable<ListadoAreaPorRegionViewModel> areasQuery;

            if (areaPadreId != null)
            {
                areasQuery = _context.HER_Area
                    .Where(x => x.HER_RegionId == regionId
                        && x.HER_Area_PadreId == areaPadreId
                        && x.HER_BajaInactividad == false
                        && x.HER_Estado != ConstEstadoArea.EstadoN3)
                    .Select(x => new ListadoAreaPorRegionViewModel()
                    {
                        AreaId = x.HER_AreaId,
                        Nombre = x.HER_Nombre,
                        TienePadre = x.HER_Area_PadreId != null ? "Si" : "No",
                        AreaPadreId = x.HER_Area_PadreId != null ? (int)x.HER_Area_PadreId : 0,
                        TotalUsuarios = x.HER_Usuarios.Count()
                    })
                    .OrderBy(x => x.Nombre)
                    .AsNoTracking()
                    .AsQueryable();

                listado = await areasQuery.ToListAsync();
            }
            else
            {
                areasQuery = _context.HER_Area
                        .Where(x => x.HER_RegionId == regionId
                            && ConstRegion.RegionesIds.Contains(x.HER_RegionId)
                            && x.HER_BajaInactividad == false
                            && x.HER_Estado != ConstEstadoArea.EstadoN3
                            && x.HER_Area_PadreId == ((ConstArea.AreasIdsSNXalapa.Contains(x.HER_AreaId)) ? x.HER_Area_PadreId : null)
                            )
                        .Select(x => new ListadoAreaPorRegionViewModel()
                        {
                            AreaId = x.HER_AreaId,
                            Nombre = x.HER_Nombre,
                            TienePadre = x.HER_Area_PadreId != null ? "Si" : "No",
                            AreaPadreId = x.HER_Area_PadreId != null ? (int)x.HER_Area_PadreId : 0,
                            TotalUsuarios = x.HER_Usuarios.Count()
                        })
                        .OrderBy(x => x.Nombre)
                        .AsNoTracking()
                        .AsQueryable();

                listado = await areasQuery.ToListAsync();
            }

            return listado;
        }
        public async Task<List<ListadoAreaViewModel>> ObtenerListadoAreasDeAreaPadreAsync(int regionId, int? areaPadreId)
        {
            List<ListadoAreaViewModel> listado = new List<ListadoAreaViewModel>();
            IQueryable<ListadoAreaViewModel> areasQuery;

            if (areaPadreId != null)
            {
                areasQuery = _context.HER_Area
                    .Where(x => x.HER_RegionId == regionId
                        && x.HER_Area_PadreId == areaPadreId
                        && x.HER_BajaInactividad == false
                        && x.HER_Estado != ConstEstadoArea.EstadoN3)
                    .OrderBy(x => x.HER_Nombre)
                    .Select(x => new ListadoAreaViewModel()
                    {
                        AreaId = x.HER_AreaId,
                        Nombre = x.HER_Nombre,
                        Region = x.HER_Region.HER_Nombre,
                        TienePadre = x.HER_Area_PadreId != null ? "Si" : "No",
                        Visible = x.HER_Visible ? "Si" : "No",
                        AreaPadreId = x.HER_Area_PadreId != null ? (int)x.HER_Area_PadreId : 0,
                        TotalUsuarios = x.HER_Usuarios.Count(),
                        Estado = x.HER_Estado == ConstEstadoArea.EstadoN1 ? ConstEstadoArea.EstadoT1 : x.HER_Estado == ConstEstadoArea.EstadoN2 ? ConstEstadoArea.EstadoT2 : ConstEstadoArea.EstadoT3,
                        //--
                        ExisteEnSIIU = x.HER_ExisteEnSIIU
                    })
                    .OrderBy(x => x.AreaPadreId)
                    .AsNoTracking()
                    .AsQueryable();

                listado = await areasQuery.ToListAsync();
            }
            else {
                areasQuery = _context.HER_Area
                        .Where(x => x.HER_RegionId == regionId
                            && ConstRegion.RegionesIds.Contains(x.HER_RegionId)
                            && x.HER_BajaInactividad == false
                            && x.HER_Estado != ConstEstadoArea.EstadoN3
                            && x.HER_Area_PadreId == ((ConstArea.AreasIdsSNXalapa.Contains(x.HER_AreaId)) ? x.HER_Area_PadreId : null)
                            )
                        .Select(x => new ListadoAreaViewModel()
                        {
                            AreaId = x.HER_AreaId,
                            Nombre = x.HER_Nombre,
                            Region = x.HER_Region.HER_Nombre,
                            TienePadre = x.HER_Area_PadreId != null ? "Si" : "No",
                            Visible = x.HER_Visible ? "Si" : "No",
                            AreaPadreId = x.HER_Area_PadreId != null ? (int)x.HER_Area_PadreId : 0,
                            TotalUsuarios = x.HER_Usuarios.Count(),
                            Estado = x.HER_Estado == ConstEstadoArea.EstadoN1 ? ConstEstadoArea.EstadoT1 : x.HER_Estado == ConstEstadoArea.EstadoN2 ? ConstEstadoArea.EstadoT2 : ConstEstadoArea.EstadoT3,
                            //--
                            ExisteEnSIIU = x.HER_ExisteEnSIIU
                        })
                        .OrderBy(x => x.AreaId)
                        .AsNoTracking()
                        .AsQueryable();

                listado = await areasQuery.ToListAsync();
            }

            return listado;
        }
        public async Task<bool> ExisteNombreArea(string area)
        {
            var areasQuery = _context.HER_Area
               .Where(x => x.HER_Nombre == area
                        && x.HER_BajaInactividad == false
                        && x.HER_Estado == ConstEstadoArea.EstadoN1)
               .AsNoTracking()
               .AsQueryable();

            return await areasQuery.AnyAsync();
        }
        public async Task<bool> ExisteAreaPorIdAsync(int areaId)
        {
            var areasQuery = _context.HER_Area
               .Where(x => x.HER_AreaId == areaId)
               .AsNoTracking()
               .AsQueryable();

            return await areasQuery.AnyAsync();
        }
        public async Task<bool> GuardarAreaAsync(CrearAreaViewModel model, int regionId)
        {
            int result = 0;

            //Clave
            var existeEnSIIU = false;

            if (!string.IsNullOrEmpty(model.Clave))
            {
                var areasQuery = _DBFContext.CDEPEN
                      .Where(x => x.NDEP == model.Clave)
                      .AsNoTracking()
                      .AsQueryable();

                existeEnSIIU = await areasQuery.AnyAsync();
            }

            //Logo
            if (model.AgregarLogo)
            {
                try
                {
                    //Carpeta
                    var rutaCarpeta = Path.Combine(_environment.WebRootPath, @"images\logo_areas");
                    bool existeCarpeta = Directory.Exists(rutaCarpeta);

                    if (!existeCarpeta)
                        Directory.CreateDirectory(rutaCarpeta);
                    //------
                    //Logo
                    var rutaArchivo = Path.Combine(_environment.WebRootPath, @"images\logo_areas", model.Logo.FileName);

                    if (model.Logo.Length > 0)
                    {
                        using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                        {
                            await model.Logo.CopyToAsync(fileStream);
                        }
                    }

                    //Logo
                    var logo = new HER_AnexoArea
                    {
                        HER_Nombre = model.Logo.FileName,
                        HER_TipoContenido = model.Logo.ContentType,
                        HER_RutaComplemento = @"images\logo_areas",
                        HER_AnexoRutaId = null
                    };
                    _context.HER_AnexoArea.Add(logo);

                    //Area
                    var area = new HER_Area()
                    {
                        HER_Nombre = model.Nombre,
                        HER_Clave = model.Clave,
                        HER_DiasCompromiso = model.Dias_Compromiso,
                        HER_Direccion = model.Direccion,
                        HER_Telefono = model.Telefono,
                        HER_Visible = model.Visible,
                        HER_RegionId = regionId,
                        HER_LogoId = logo.HER_AnexoAreaId,
                        HER_Estado = ConstEstadoArea.EstadoN1,
                        HER_ExisteEnSIIU = existeEnSIIU,
                        HER_BajaInactividad = false
                    };

                    if (model.AsignarAreaPadre)
                    {
                        area.HER_Area_PadreId = int.Parse(model.Area_PadreId);
                    }

                    _context.HER_Area.Add(area);
                    result = await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError("AreaService:GuardarAreaAsync: " + ex.Message);
                }
            }
            else {
                //Area
                var area = new HER_Area()
                {
                    HER_Nombre = model.Nombre,
                    HER_Clave = model.Clave,
                    HER_DiasCompromiso = model.Dias_Compromiso,
                    HER_Direccion = model.Direccion,
                    HER_Telefono = model.Telefono,
                    HER_Visible = model.Visible,
                    HER_RegionId = regionId,
                    HER_LogoId = null,
                    HER_Estado = ConstEstadoArea.EstadoN1,
                    HER_ExisteEnSIIU = existeEnSIIU,
                    HER_BajaInactividad = false
                };

                if (model.AsignarAreaPadre)
                {
                    area.HER_Area_PadreId = int.Parse(model.Area_PadreId);
                }

                _context.HER_Area.Add(area);
                result = await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }
        public async Task<DetalleAreaViewModel> ObtenerDetallesArea(int areaId, int regionId)
        {
            var areasQuery = _context.HER_Area
               .Where(x => x.HER_AreaId == areaId 
                        && x.HER_RegionId == regionId)
               .Select(x => new DetalleAreaViewModel() {
                   AreaId = x.HER_AreaId,
                   Nombre = x.HER_Nombre,
                   Clave = x.HER_Clave,
                   Dias_Compromiso = x.HER_DiasCompromiso,
                   Direccion = x.HER_Direccion,
                   Telefono = x.HER_Telefono,
                   Region = x.HER_Region.HER_Nombre,
                   Area_Padre = x.HER_Area_PadreId != null ? x.HER_Area_Padre.HER_Nombre : null,
                   Visible = x.HER_Visible ? "Si" : "No",
                   HayLogoActual = x.HER_LogoId != null ? true: false,
                   Estado = x.HER_Estado == ConstEstadoArea.EstadoN1 ? ConstEstadoArea.EstadoT1 : x.HER_Estado == ConstEstadoArea.EstadoN2 ? ConstEstadoArea.EstadoT2 : ConstEstadoArea.EstadoT3,
                   EstadoId = x.HER_Estado
               })
               .AsNoTracking()
               .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<DetalleAreaViewModel> ObtenerDetallesArea(int areaId)
        {
            var areasQuery = _context.HER_Area
               .Where(x => x.HER_AreaId == areaId && x.HER_BajaInactividad == false)
               .Select(x => new DetalleAreaViewModel()
               {
                   AreaId = x.HER_AreaId,
                   Nombre = x.HER_Nombre,
                   Clave = x.HER_Clave,
                   Dias_Compromiso = x.HER_DiasCompromiso,
                   Direccion = x.HER_Direccion,
                   Telefono = x.HER_Telefono,
                   Region = x.HER_Region.HER_Nombre,
                   Area_Padre = x.HER_Area_PadreId != null ? x.HER_Area_Padre.HER_Nombre : null,
                   Visible = x.HER_Visible ? "Si" : "No",
                   HayLogoActual = x.HER_LogoId != null ? true : false,
                   Estado = x.HER_Estado == ConstEstadoArea.EstadoN1 ? ConstEstadoArea.EstadoT1 : x.HER_Estado == ConstEstadoArea.EstadoN2 ? ConstEstadoArea.EstadoT2 : ConstEstadoArea.EstadoT3,
                   EstadoId = x.HER_Estado
               })
               .AsNoTracking()
               .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<EditarAreaViewModel> ObtenerAreaParaEditar(int areaId, int regionId)
        {
            var areasQuery = _context.HER_Area
               .Where(x => x.HER_AreaId == areaId 
                        && x.HER_RegionId == regionId)
               .Select(x => new EditarAreaViewModel()
               {
                   AreaId = x.HER_AreaId, //id del area registrada
                   TieneLogo = x.HER_LogoId != null ? true : false,
                   Nombre = x.HER_Nombre,
                   Clave = x.HER_Clave,
                   Region = x.HER_Region.HER_Nombre,
                   Area_Padre = x.HER_Area_PadreId != null ? x.HER_Area_Padre.HER_Nombre : null,
                   Dias_Compromiso = x.HER_DiasCompromiso,
                   Direccion = x.HER_Direccion,
                   Telefono = x.HER_Telefono,
                   Visible = x.HER_Visible
               })
               .AsNoTracking()
               .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ActualizarAreaAsync(EditarAreaViewModel modelo, int regionId)
        {
            int result = 0;

            //Area
            var areaQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == modelo.AreaId
                         && x.HER_BajaInactividad == false
                         && x.HER_RegionId == regionId)
                .AsQueryable();

            var area = await areaQuery.FirstOrDefaultAsync();
            if (area != null)
            {
                area.HER_DiasCompromiso = modelo.Dias_Compromiso;
                area.HER_Direccion = modelo.Direccion;
                area.HER_Telefono = modelo.Telefono;
                area.HER_Visible = modelo.Visible;

                _context.HER_Area.Update(area).State = EntityState.Modified;

                result = await _context.SaveChangesAsync();
            }

            return (result > 0) ? true : false;
        }
        public async Task<EditarAreaEnAdminViewModel> AdminObtenerAreaParaEditar(int areaId)
        {
            var areasQuery = _context.HER_Area
               .Where(x => x.HER_AreaId == areaId && x.HER_BajaInactividad == false)
               .Select(x => new EditarAreaEnAdminViewModel()
               {
                   AreaId = x.HER_AreaId, //id del area registrada
                   TieneLogo = x.HER_LogoId != null ? true : false,
                   Nombre = x.HER_Nombre,
                   Clave = x.HER_Clave,
                   Region = x.HER_Region.HER_Nombre,
                   Area_Padre = x.HER_Area_PadreId != null ? x.HER_Area_Padre.HER_Nombre : null,
                   Dias_Compromiso = x.HER_DiasCompromiso,
                   Direccion = x.HER_Direccion,
                   Telefono = x.HER_Telefono,
                   Visible = x.HER_Visible,
               })
               .AsNoTracking()
               .AsQueryable();

            return await areasQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> AdminActualizarAreaAsync(EditarAreaEnAdminViewModel modelo)
        {
            int result = 0;

            //Area
            var areaQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == modelo.AreaId)
                .AsQueryable();

            var area = await areaQuery.FirstOrDefaultAsync();
            if (area != null)
            {
                area.HER_DiasCompromiso = modelo.Dias_Compromiso;
                area.HER_Direccion = modelo.Direccion;
                area.HER_Telefono = modelo.Telefono;
                area.HER_Visible = modelo.Visible;

                _context.HER_Area.Update(area).State = EntityState.Modified;

                result = await _context.SaveChangesAsync();
            }

            return (result > 0) ? true : false;
        }
        public async Task<BorrarAreaViewModel> ObtenerAreaParaBorrarAsync(int areaId, int regionId)
        {
            var areaQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_RegionId == regionId)
                .Select(x => new BorrarAreaViewModel()
                {
                    AreaId = x.HER_AreaId,
                    Nombre = x.HER_Nombre,
                    Direccion = x.HER_Direccion,
                    Telefono = x.HER_Telefono,
                    Region = x.HER_Region.HER_Nombre,
                    Area_Padre = x.HER_Area_PadreId != null? x.HER_Area_Padre.HER_Nombre : string.Empty,
                    Visible = x.HER_Visible ? "Si" : "No"
                })
                .AsNoTracking()
                .AsQueryable();

            return await areaQuery.FirstOrDefaultAsync();
        }
        public async Task<BorrarAreaViewModel> ObtenerAreaParaBorrarAsync(int areaId)
        {
            var areaQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId && x.HER_BajaInactividad == false)
                .Select(x => new BorrarAreaViewModel()
                {
                    AreaId = x.HER_AreaId,
                    Nombre = x.HER_Nombre,
                    Direccion = x.HER_Direccion,
                    Telefono = x.HER_Telefono,
                    Region = x.HER_Region.HER_Nombre,
                    Area_Padre = x.HER_Area_PadreId != null ? x.HER_Area_Padre.HER_Nombre : string.Empty,
                    Visible = x.HER_Visible ? "Si" : "No"
                })
                .AsNoTracking()
                .AsQueryable();

            return await areaQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> DetectaAreaEnUsoAsync(int areaId)
        {
            var areaQuery = _context.HER_Area
               .Where(x => x.HER_AreaId == areaId
                        && x.HER_BajaInactividad == false
                        && x.HER_Usuarios.Any()
                        && x.HER_Estado == ConstEstadoArea.EstadoN1)
               .AsNoTracking()
               .AsQueryable();

            return await areaQuery.AnyAsync();
        }
        public async Task<bool> EliminarAreaAsync(int areaId)
        {
            int result = 0;

            var areaQuery = _context.HER_Area
                    .Where(x => x.HER_AreaId == areaId)
                    .AsQueryable();

            var area = await areaQuery.FirstOrDefaultAsync();

            if (area.HER_LogoId != null)
            {
                //Borrar Anexos 
                var logoQuery = _context.HER_AnexoArea
                           .Where(x => x.HER_AnexoAreaId == area.HER_LogoId)
                           .AsQueryable();

                var logo = await logoQuery.FirstOrDefaultAsync();
                string rutaBase;

                if (logo.HER_AnexoRutaId == null)
                    rutaBase = _environment.WebRootPath;
                else
                    rutaBase = logo.HER_AnexoRuta.HER_RutaBase;
                
                //---
                var rutaArchivo = Path.Combine(rutaBase, logo.HER_RutaComplemento, logo.HER_Nombre);

                try
                {
                    bool existe = File.Exists(rutaArchivo);
                    if (existe)
                    {
                        File.Delete(rutaArchivo);
                    }

                    //Borrar en la base
                    _context.HER_AnexoArea.Remove(logo);

                    //Borrar
                    _context.HER_Area.Remove(area);
                    result = _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError("AreaService:EliminarAreaAsync: " + ex.Message);
                }
            }
            else {
                //Borrar
                _context.HER_Area.Remove(area);
                result = _context.SaveChanges();
            }
            
            return result > 0 ? true : false;
        }
        //--
        public async Task<HER_AnexoArea> ObtenerLogoInstitucionAsync(int areaId)
        {
            var configuracionQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => x.HER_Logo)
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ExisteLogoInstitucionAsync(int areaId)
        {
            Console.WriteLine(areaId);
            var configuracionQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_LogoId != null
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsNoTracking()
                .AsQueryable();

            Console.WriteLine(configuracionQuery);
            return await configuracionQuery.AnyAsync();
        }
        //--
        public async Task<DarDeBajaAreaViewModel> ObtenerAreaParaDarDeBajaAsync(int areaId, int regionId)
        {
            var areaQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_RegionId == regionId)
                .Select(x => new DarDeBajaAreaViewModel()
                {
                    AreaId = x.HER_AreaId,
                    Nombre = x.HER_Nombre,
                    Direccion = x.HER_Direccion,
                    Telefono = x.HER_Telefono,
                    Region = x.HER_Region.HER_Nombre,
                    Area_Padre = x.HER_Area_PadreId != null ? x.HER_Area_Padre.HER_Nombre : string.Empty,
                    Visible = x.HER_Visible ? "Si" : "No"
                })
                .AsNoTracking()
                .AsQueryable();

            return await areaQuery.FirstOrDefaultAsync();
        }
        //--
        public async Task<bool> ExisteAreaEnProcesoDeCambioPorIdAsync(int areaId) 
        {
            var areaQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId 
                         && x.HER_Estado == ConstEstadoArea.EstadoN2)
                .AsNoTracking()
                .AsQueryable();

            return await areaQuery.AnyAsync();
        }
        public async Task<bool> DarDeBajaAreaAsync(DarDeBajaAreaViewModel bajaAreaViewModel)
        {
            //Area actual
            var areaActualQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == bajaAreaViewModel.AreaId
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsQueryable();

            var usuariosQuery = _context.HER_InfoUsuario
                .Include(x => x.HER_Usuario)
                .Where(x => x.HER_EstaEnBajaDefinitiva == false 
                            && x.HER_AreaId == bajaAreaViewModel.AreaId)
                .AsQueryable();

            List<HER_InfoUsuario> nuevosUsuarios = new List<HER_InfoUsuario>();
            IEnumerable<string> roles;
            HER_Area areaActual = await areaActualQuery.FirstOrDefaultAsync();
            int result = 0;
            
            if (areaActual!= null) 
            {
                var usuarios = await usuariosQuery.ToListAsync();

                if (Int32.Parse(bajaAreaViewModel.TipoBaja) == ConstTipoDeBajaArea.TipoDeBajaN1)
                {
                    //ACTUAL
                    //Área actual - cambia a no activa
                    areaActual.HER_Estado = ConstEstadoArea.EstadoN3;
                    //--
                    //NUEVA
                    var nuevaArea = new HER_Area()
                    {
                        HER_Nombre = areaActual.HER_Nombre,
                        HER_Clave = areaActual.HER_Clave,
                        HER_DiasCompromiso = areaActual.HER_DiasCompromiso,
                        HER_Direccion = areaActual.HER_Direccion,
                        HER_Telefono = areaActual.HER_Telefono,
                        HER_Visible = areaActual.HER_Visible,
                        HER_RegionId = areaActual.HER_RegionId,
                        HER_LogoId = areaActual.HER_LogoId,
                        HER_Estado = ConstEstadoArea.EstadoN2,
                        HER_Area_PadreId = areaActual.HER_Area_PadreId,
                        HER_ExisteEnSIIU = areaActual.HER_ExisteEnSIIU,
                        HER_BajaInactividad = false
                    };
                    //--
                    _context.HER_Area.Add(nuevaArea);
                    _context.HER_Area.Update(areaActual).State = EntityState.Modified;
                    //--
                    result += await _context.SaveChangesAsync();

                    //USUARIOS
                    foreach (var usuario in usuarios)
                    {
                        //--ACTUAL
                        usuario.HER_Activo = false;
                        usuario.HER_EstaEnReasignacion = false;
                        usuario.HER_EstaEnBajaDefinitiva = false;
                        usuario.HER_FechaActualizacion = DateTime.Now;

                        //--NUEVA
                        nuevosUsuarios.Add(new HER_InfoUsuario() {
                            HER_NombreCompleto = usuario.HER_NombreCompleto,
                            HER_Correo = usuario.HER_Correo,
                            HER_UserName = usuario.HER_UserName,
                            HER_Direccion = usuario.HER_Direccion,
                            HER_Telefono = usuario.HER_Telefono,
                            HER_FechaRegistro = DateTime.Now,
                            HER_FechaActualizacion = DateTime.Now,
                            HER_Activo = true,
                            HER_EstaEnReasignacion = true,
                            HER_EstaEnBajaDefinitiva = false,
                            HER_Titular = usuario.HER_UserName,
                            HER_AreaId = nuevaArea.HER_AreaId,
                            HER_Puesto = usuario.HER_Puesto,
                            HER_RolNombre = ConstRol.Rol8T,
                            HER_UsuarioId = usuario.HER_UsuarioId,
                            HER_BandejaPermiso = ConstDelegar.TipoN1,
                            HER_BandejaUsuario = usuario.HER_UserName,
                            HER_BandejaNombre = usuario.HER_NombreCompleto
                        });
                        //--
                        roles = await _usuarioService.ObtenerRolesUsuarioAsync(usuario.HER_UserName);
                        //--
                        if (!roles.Contains(ConstRol.Rol8T))
                        {
                            await _userManager.RemoveFromRolesAsync(usuario.HER_Usuario, roles);
                            await _userManager.AddToRoleAsync(usuario.HER_Usuario, ConstRol.Rol8T);
                            //--
                            await _userManager.UpdateAsync(usuario.HER_Usuario);
                        }
                    }
                    
                    _context.HER_InfoUsuario.UpdateRange(usuarios);
                    //Se crea nuevos usuarios temporales
                    _context.HER_InfoUsuario.AddRange(nuevosUsuarios);
                    //--
                    result += await _context.SaveChangesAsync();
                }
                else if (Int32.Parse(bajaAreaViewModel.TipoBaja) == ConstTipoDeBajaArea.TipoDeBajaN2)
                {
                    //Baja definitiva
                    areaActual.HER_Estado = ConstEstadoArea.EstadoN3;
                    //--
                    foreach (var usuario in usuarios)
                    {
                        usuario.HER_Activo = false;
                        usuario.HER_EstaEnReasignacion = false;
                        usuario.HER_EstaEnBajaDefinitiva = true;
                        usuario.HER_FechaActualizacion = DateTime.Now;
                    }
                    //--
                    _context.HER_InfoUsuario.UpdateRange(usuarios);
                    _context.HER_Area.Update(areaActual).State = EntityState.Modified;
                    //--
                    result += await _context.SaveChangesAsync();
                }
            }

            return result > 0 ? true : false;
        }
        public async Task<List<AreaEsViewModel>> ObtenerAreasEnCambiosAsync()
        {
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_Estado == ConstEstadoArea.EstadoN2 && x.HER_BajaInactividad == false)
                .Select(x => new AreaEsViewModel() {
                    HER_AreaId = x.HER_AreaId,
                    HER_Nombre = x.HER_Nombre
                })
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.ToListAsync();
        }
        public async Task<List<FamiliaAreaViewModel>> ObtenerFamiliaArea(int areaPadreId, string areaPadreNombre)
        {
            var listado = new List<FamiliaAreaViewModel>() 
            {
                new FamiliaAreaViewModel(){
                    AreaPadreId = 0,
                    AreaId = areaPadreId,
                    Nombre = areaPadreNombre,
                    TipoId = ConstTipoArea.TipoN1,
                    Tipo = ConstTipoArea.TipoT1
                }
            };
            
            var areasQuery = _context.HER_Area
                .Where(x => x.HER_Area_PadreId == areaPadreId
                    && x.HER_BajaInactividad == false
                    && x.HER_Area_Padre.HER_Estado == ConstEstadoArea.EstadoN1
                    && x.HER_Area_Padre.HER_BajaInactividad == false
                    && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => new FamiliaAreaViewModel() { 
                    AreaPadreId = (int)x.HER_Area_PadreId,
                    AreaId = x.HER_AreaId,
                    Nombre = x.HER_Nombre,
                    TipoId = ConstTipoArea.TipoN2,
                    Tipo = ConstTipoArea.TipoT2
                })
                .AsTracking()
                .AsQueryable();

            var areas = await areasQuery.ToListAsync();

            if (areas.Count() > 0) 
                listado.AddRange(areas);

            return listado;
        }
        public async Task<bool> EsAreaHija(int areaPadreId, int areaId)
        {
            var areaExisteQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                    && x.HER_Estado == ConstEstadoArea.EstadoN1
                    && x.HER_BajaInactividad == false
                    && x.HER_Area_PadreId == areaPadreId
                    && x.HER_Area_Padre.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsTracking()
                .AsQueryable();

            return await areaExisteQuery.AnyAsync();
        }
        public async Task<int> ObtieneAreaPadre(int areaId) 
        {
            var areaPadreQuery = _context.HER_Area
                    .Where(x => x.HER_AreaId == areaId
                        && x.HER_BajaInactividad == false
                        && x.HER_Estado == ConstEstadoArea.EstadoN1)
                    .Select(x => x.HER_Area_PadreId)
                    .AsQueryable();

            var areaPadreId = await areaPadreQuery.FirstOrDefaultAsync();

            return areaPadreId == null ? 0: (int)areaPadreId;
        }
        public async Task<List<FamiliaAreaCompuestaViewModel>> ObtenerFamiliaAreaCompuesta(int areaPadreId)
        {
            var listado = new List<FamiliaAreaCompuestaViewModel>();
            //--
            var areaPadreQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaPadreId
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => new FamiliaAreaCompuestaViewModel()
                {
                    AreaPadreId = x.HER_Area_PadreId != null ? (int)x.HER_Area_PadreId : 0,
                    AreaId = x.HER_AreaId,
                    Nombre = x.HER_Nombre,
                    TipoId = ConstTipoArea.TipoN1,
                    Tipo = ConstTipoArea.TipoT1,
                    //--
                    TienePadre = x.HER_Area_PadreId != null ? "Si" : "No",
                    Visible = x.HER_Visible ? "Si" : "No",
                    TotalUsuarios = x.HER_Usuarios.Where(a => a.HER_Activo == true && a.HER_EstaEnReasignacion == false).Count(),
                    Estado = x.HER_Estado == ConstEstadoArea.EstadoN1 ? ConstEstadoArea.EstadoT1 : x.HER_Estado == ConstEstadoArea.EstadoN2 ? ConstEstadoArea.EstadoT2 : ConstEstadoArea.EstadoT3
                })
                .AsTracking()
                .AsQueryable();

            var areaPadre = await areaPadreQuery.FirstOrDefaultAsync();
            listado.Add(areaPadre);

            //--
            var areasHijasQuery = _context.HER_Area
                .Where(x => x.HER_Area_PadreId == areaPadreId
                    && x.HER_Area_Padre.HER_Estado == ConstEstadoArea.EstadoN1
                    && x.HER_Area_Padre.HER_BajaInactividad == false
                    && x.HER_BajaInactividad == false
                    && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => new FamiliaAreaCompuestaViewModel()
                {
                    AreaPadreId = (int)x.HER_Area_PadreId,
                    AreaId = x.HER_AreaId,
                    Nombre = x.HER_Nombre,
                    TipoId = ConstTipoArea.TipoN2,
                    Tipo = ConstTipoArea.TipoT2,
                    //--
                    TienePadre = x.HER_Area_PadreId != null ? "Si" : "No",
                    Visible = x.HER_Visible ? "Si" : "No",
                    TotalUsuarios = x.HER_Usuarios.Where(a => a.HER_Activo == true && a.HER_EstaEnReasignacion == false).Count(),
                    Estado = x.HER_Estado == ConstEstadoArea.EstadoN1 ? ConstEstadoArea.EstadoT1 : x.HER_Estado == ConstEstadoArea.EstadoN2 ? ConstEstadoArea.EstadoT2 : ConstEstadoArea.EstadoT3
                })
                .OrderBy(x => x.AreaId)
                .AsTracking()
                .AsQueryable();

            var areasHijas = await areasHijasQuery.ToListAsync();
            
            if (areasHijas.Count() > 0)
                listado.AddRange(areasHijas);

            return listado;
        }
        public async Task<List<InfoAreaViewModel>> ObtenerAreasConSusHijasPorClaveAsync(List<string> claves)
        {
            var listado = new List<InfoAreaViewModel>();

            //--------------------*
            var equivQuery = _DBFContext.CDEPEN_EQUIV
                    .Where(x => claves.Contains(x.NDEPA.ToString()))
                    .AsTracking()
                    .AsQueryable();

            var equiv = await equivQuery.ToListAsync();

            foreach (var eq in equiv)
            {
                claves[claves.FindIndex(a => a.Equals(eq.NDEPA.ToString()))] = eq.NDEPA_EQUIV.ToString();
            }
            //--------------------*

            var areasPadreQuery = _context.HER_Area
                .Where(x => claves.Contains(x.HER_Clave)
                    && x.HER_BajaInactividad == false
                    && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => new InfoAreaViewModel() { 
                    AreaPadreId = 0,
                    AreaId = x.HER_AreaId,
                    Clave = x.HER_Clave,
                    Nombre = x.HER_Nombre,
                    Direccion = x.HER_Direccion,
                    Telefono = x.HER_Telefono,
                    RegionId = x.HER_RegionId,
                    Region = x.HER_Region.HER_Nombre,
                    Tipo = ConstTipoArea.TipoN1
                })
                .OrderBy(x => x.Clave)
                .AsNoTracking()
                .AsQueryable();

            var areasPadre = await areasPadreQuery.ToListAsync();

            var areasHijaQuery = _context.HER_Area
                .Where(x => x.HER_Estado == ConstEstadoArea.EstadoN1 && x.HER_BajaInactividad == false)
                .OrderBy(x => x.HER_Clave)
                .AsNoTracking()
                .AsQueryable();

            foreach (var padre in areasPadre)
            {
                listado.Add(padre);
                listado.AddRange(await areasHijaQuery
                    .Where(x => x.HER_Area_PadreId == padre.AreaId && x.HER_BajaInactividad == false)
                    .Select(x => new InfoAreaViewModel()
                    {
                        AreaPadreId = (int)x.HER_Area_PadreId,
                        AreaId = x.HER_AreaId,
                        Clave = x.HER_Clave,
                        Nombre = string.Format("{0} {1}", "-",x.HER_Nombre),
                        Direccion = x.HER_Direccion,
                        Telefono = x.HER_Telefono,
                        RegionId = x.HER_RegionId,
                        Region = x.HER_Region.HER_Nombre,
                        Tipo = ConstTipoArea.TipoN2
                    }).ToListAsync());
            }

            return listado;
        }
        public async Task<bool> AgregarAreaAsync(AgregarAreaViewModel model)
        {
            int result = 0;

            //Clave
            var existeEnSIIU = false;

            if (!string.IsNullOrEmpty(model.Clave))
            {
                var areasQuery = _DBFContext.CDEPEN
                      .Where(x => x.NDEP == model.Clave)
                      .AsNoTracking()
                      .AsQueryable();

                existeEnSIIU = await areasQuery.AnyAsync();
            }

            //Logo
            if (model.AgregarLogo)
            {
                try
                {
                    //Carpeta
                    var rutaCarpeta = Path.Combine(_environment.WebRootPath, @"images\logo_areas");
                    bool existeCarpeta = Directory.Exists(rutaCarpeta);

                    if (!existeCarpeta)
                        Directory.CreateDirectory(rutaCarpeta);
                    //------
                    //Logo
                    var rutaArchivo = Path.Combine(_environment.WebRootPath, @"images\logo_areas", model.Logo.FileName);

                    if (model.Logo.Length > 0)
                    {
                        using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                        {
                            await model.Logo.CopyToAsync(fileStream);
                        }
                    }

                    //Logo
                    var logo = new HER_AnexoArea
                    {
                        HER_Nombre = model.Logo.FileName,
                        HER_TipoContenido = model.Logo.ContentType,
                        HER_RutaComplemento = @"images\logo_areas",
                        HER_AnexoRutaId = null
                    };
                    _context.HER_AnexoArea.Add(logo);

                    //
                    var area = new HER_Area()
                    {
                        HER_Nombre = model.Nombre,
                        HER_Clave = model.Clave,
                        HER_DiasCompromiso = model.Dias_Compromiso,
                        HER_Direccion = model.Direccion,
                        HER_Telefono = model.Telefono,
                        HER_Visible = model.Visible,
                        HER_RegionId = int.Parse(model.RegionId),
                        HER_LogoId = logo.HER_AnexoAreaId,
                        HER_Estado = ConstEstadoArea.EstadoN1,
                        HER_ExisteEnSIIU = existeEnSIIU,
                        HER_Area_PadreId = int.Parse(model.Area_PadreId),
                        HER_BajaInactividad = false
                    };

                    _context.HER_Area.Add(area);
                    result = await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("AreaService:AgregarAreaAsync: " + ex.Message);
                }
            }
            else
            {
                //
                var area = new HER_Area()
                {
                    HER_Nombre = model.Nombre,
                    HER_Clave = model.Clave,
                    HER_DiasCompromiso = model.Dias_Compromiso,
                    HER_Direccion = model.Direccion,
                    HER_Telefono = model.Telefono,
                    HER_Visible = model.Visible,
                    HER_RegionId = int.Parse(model.RegionId),
                    HER_LogoId = null,
                    HER_Estado = ConstEstadoArea.EstadoN1,
                    HER_ExisteEnSIIU = existeEnSIIU,
                    HER_Area_PadreId = int.Parse(model.Area_PadreId),
                    HER_BajaInactividad = false
                };

                _context.HER_Area.Add(area);
                result = await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }
        public async Task<bool> AgregarAreaManualAsync(AgregarAreaViewModel model)
        {
            int result = 0;

            //Clave
            var existeEnSIIU = false;

            if (!string.IsNullOrEmpty(model.Clave))
            {
                var areasQuery = _DBFContext.CDEPEN
                      .Where(x => x.NDEP == model.Clave)
                      .AsNoTracking()
                      .AsQueryable();

                existeEnSIIU = await areasQuery.AnyAsync();
            }

            //Logo
            if (model.AgregarLogo)
            {
                try
                {
                    //Carpeta
                    var rutaCarpeta = Path.Combine(_environment.WebRootPath, @"images\logo_areas");
                    bool existeCarpeta = Directory.Exists(rutaCarpeta);

                    if (!existeCarpeta)
                        Directory.CreateDirectory(rutaCarpeta);
                    //------
                    //Logo
                    var rutaArchivo = Path.Combine(_environment.WebRootPath, @"images\logo_areas", model.Logo.FileName);

                    if (model.Logo.Length > 0)
                    {
                        using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                        {
                            await model.Logo.CopyToAsync(fileStream);
                        }
                    }

                    //logo
                    var logo = new HER_AnexoArea
                    {
                        HER_Nombre = model.Logo.FileName,
                        HER_TipoContenido = model.Logo.ContentType,
                        HER_RutaComplemento = @"images\logo_areas",
                        HER_AnexoRutaId = null
                    };
                    _context.HER_AnexoArea.Add(logo);

                    //
                    var area = new HER_Area()
                    {
                        HER_Nombre = model.Nombre,
                        HER_Clave = model.Clave,
                        HER_DiasCompromiso = model.Dias_Compromiso,
                        HER_Direccion = model.Direccion,
                        HER_Telefono = model.Telefono,
                        HER_Visible = model.Visible,
                        HER_RegionId = int.Parse(model.RegionId),
                        HER_LogoId = logo.HER_AnexoAreaId,
                        HER_Estado = ConstEstadoArea.EstadoN1,
                        HER_ExisteEnSIIU = existeEnSIIU,
                        HER_Area_PadreId = int.Parse(model.Area_PadreId),
                        HER_BajaInactividad = false
                    };

                    _context.HER_Area.Add(area);
                    result = await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("AnexoService:AgregarAreaManualAsync: " + ex.Message);
                }
            }
            else
            {
                //
                var area = new HER_Area()
                {
                    HER_Nombre = model.Nombre,
                    HER_Clave = model.Clave,
                    HER_DiasCompromiso = model.Dias_Compromiso,
                    HER_Direccion = model.Direccion,
                    HER_Telefono = model.Telefono,
                    HER_Visible = model.Visible,
                    HER_RegionId = int.Parse(model.RegionId),
                    HER_LogoId = null,
                    HER_Estado = ConstEstadoArea.EstadoN1,
                    HER_ExisteEnSIIU = existeEnSIIU,
                    HER_Area_PadreId = int.Parse(model.Area_PadreId),
                    HER_BajaInactividad = false
                };

                _context.HER_Area.Add(area);
                result = await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }
        public async Task<string> GenerarClaveAreaHijoAsync(int areaPadreId, string claveAreaPadre)
        {
            var areaClaveQuery = _context.HER_Area
                .Where(x => x.HER_Area_PadreId == areaPadreId
                        &&  x.HER_Estado == ConstEstadoArea.EstadoN1
                        && x.HER_ExisteEnSIIU == false)
                .AsTracking()
                .AsQueryable();

            var areaClave = string.Format("{0}-{1}", claveAreaPadre, (await areaClaveQuery.CountAsync() + 1));

            return areaClave;
        }
        public async Task<bool> ExisteAreaEnSIIU(int areaId)
        {
            var areaClaveQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                        && x.HER_BajaInactividad == false
                        && x.HER_Estado == ConstEstadoArea.EstadoN1
                        && x.HER_ExisteEnSIIU == true)
                .AsTracking()
                .AsQueryable();

            return await areaClaveQuery.AnyAsync();
        }
        public async Task<bool> ExisteClave(string clave)
        {
            var areaClaveQuery = _context.HER_Area
                    .Where(x => x.HER_Clave == clave
                            && x.HER_BajaInactividad == false
                            && x.HER_Estado == ConstEstadoArea.EstadoN1)
                    .AsTracking()
                    .AsQueryable();

            return await areaClaveQuery.AnyAsync();
        }

        //Buscar
        public async Task<List<BuscarAreaViewModel>> BusquedaAreasAsync(string keyword)
        {
            List<BuscarAreaViewModel> listado = new List<BuscarAreaViewModel>();

            if (!string.IsNullOrEmpty(keyword))
            {
                var areasQuery = _context.HER_Area
                    .Where(x => (EF.Functions.Like(x.HER_Nombre, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_Clave, "%" + keyword + "%")
                              || EF.Functions.Like(x.HER_Direccion, "%" + keyword + "%"))
                              && x.HER_Visible == true
                              && x.HER_BajaInactividad == false
                           )
                    .AsNoTracking()
                    .Take(10)
                    .Select(x => new BuscarAreaViewModel
                    {
                        AreaId = x.HER_AreaId,
                        Nombre = x.HER_Nombre,
                        Clave = x.HER_Clave,
                        DiasCompromiso = x.HER_DiasCompromiso,
                        Direccion = x.HER_Direccion,
                        Telefono = x.HER_Telefono,
                        Region = x.HER_Region.HER_Nombre,
                        AreaPadre = x.HER_Area_PadreId == null ? string.Empty : x.HER_Area_Padre.HER_Nombre
                    })
                    .AsQueryable();

                listado = await areasQuery
                                    .OrderBy(x => x.Nombre)
                                    .ThenBy(x => x.Region)
                                    .ToListAsync();
            }

            return listado;
        }

        public async Task<bool> DarBajaAreaByUsuariosInactivosAsync(int areaId)
        {
            int result = 0;

            //Area
            var areaQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false)
                .AsQueryable();

            var area = await areaQuery.FirstOrDefaultAsync();

            if(area != null)
            {
                area.HER_BajaInactividad = true;

                _context.HER_Area.Update(area).State = EntityState.Modified;

                result = await _context.SaveChangesAsync();
            }

            return (result > 0) ? true : false;

        }

        public async Task<bool> DetectaAreaUsuariosDadosBajaAsync(int areaId)
        {
            var query = _context.HER_InfoUsuario
                .Where(x => x.HER_AreaId == areaId
                    && (x.HER_EstaEnBajaDefinitiva == true 
                    || x.HER_Activo == false)
                    && x.HER_Area.HER_BajaInactividad == false)
                .AsNoTracking()
                .AsQueryable();
            
            return await query.AnyAsync();
        }
    }
}
