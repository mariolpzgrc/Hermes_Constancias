using Hermes2018.Data;
using Hermes2018.Models;
using Hermes2018.Models.Servicio;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class ServicioService: IServicioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsuarioService _usuarioService;

        public ServicioService(ApplicationDbContext context, IUsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
        }

        public async Task<List<ServiciosViewModel>> ObtenerListadoServiciosAsync(string username)
        {
            var serviciosQuery = _context.HER_Servicio
                .Where(x => x.HER_Creador.UserName == username)
                .AsNoTracking()
                .Select(x => new ServiciosViewModel()
                {
                    HER_ServicioId = x.HER_ServicioId,
                    HER_Servicio_Nombre = x.HER_Nombre
                })
                .AsQueryable();

            return await serviciosQuery.ToListAsync();
        }
        public async Task<List<ServiciosCompletosViewModel>> ObtenerServiciosAsync()
        {
            List<ServiciosCompletosViewModel> listadoServicios = new List<ServiciosCompletosViewModel>();
            List<UsuarioLocalJsonModel> listadoIntegrantes = new List<UsuarioLocalJsonModel>();

            var serviciosQuery = _context.HER_Servicio
                .Include(x => x.HER_Usuarios)
                    .ThenInclude(x => x.HER_Integrante)
                    .ThenInclude(x => x.HER_Area)
                    .ThenInclude(x => x.HER_Region)
                 .Include(x => x.HER_Usuarios)
                    .ThenInclude(x => x.HER_Integrante)
                .Include(x => x.HER_Region)
                .OrderBy(x => x.HER_RegionId)
                .AsNoTracking()
                .AsQueryable();

            var servicios = await serviciosQuery.ToListAsync();

            foreach (var servicio in servicios)
            {
                listadoIntegrantes = new List<UsuarioLocalJsonModel>();

                foreach (var registro in servicio.HER_Usuarios)
                {
                    listadoIntegrantes.Add(new UsuarioLocalJsonModel()
                    {
                        HER_UserName = registro.HER_Integrante.HER_UserName,
                        HER_Correo = registro.HER_Integrante.HER_Correo,
                        HER_NombreCompleto = registro.HER_Integrante.HER_NombreCompleto,
                        HER_Region = registro.HER_Integrante.HER_Area.HER_Region.HER_Nombre,
                        HER_Area = registro.HER_Integrante.HER_Area.HER_Nombre,
                        HER_Puesto = registro.HER_Integrante.HER_Puesto,
                        HER_Tipo = registro.HER_Integrante.HER_RolNombre,
                        HER_Titular = registro.HER_Integrante.HER_Titular
                    });
                }

                listadoServicios.Add(new ServiciosCompletosViewModel()
                {
                    HER_ServicioId = servicio.HER_ServicioId,
                    HER_Servicio_Nombre = servicio.HER_Nombre,
                    HER_RegionId = servicio.HER_Region.HER_RegionId,
                    HER_Region_Nombre = servicio.HER_Region.HER_Nombre,
                    HER_Integrantes = listadoIntegrantes
                });
            }

            return listadoServicios;
        }

        public async Task<DetalleServicioViewModel> ObtenerDetalleServicioAsync(int servicioId)
        {
            var servicioQuery = _context.HER_Servicio
                .Where(x => x.HER_ServicioId == servicioId)
                .Include(x => x.HER_Usuarios)
                    .ThenInclude(x => x.HER_Integrante)
                .AsNoTracking()
                .Select(x => new DetalleServicioViewModel() {
                    Nombre = x.HER_Nombre,
                    Integrantes = (from y in x.HER_Usuarios
                                   select new IntegranteServicioViewModel() {
                                      Usuario = y.HER_Integrante.HER_UserName,
                                      Nombre = y.HER_Integrante.HER_NombreCompleto,
                                      Correo = y.HER_Integrante.HER_Correo
                                  }).ToList()
                })
                .AsQueryable();

            return await servicioQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ExisteServicioAsync(string nombreServicio, string username)
        {
            var existeQuery = _context.HER_Servicio
                .Where(x => x.HER_Nombre == nombreServicio
                         && x.HER_Creador.UserName == username)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> GuardarServiciosAsync(CrearServicioViewModel crear, string username, int regionId)
        {
            int result = 0;

            //Crear el grupo
            var servicio = new HER_Servicio()
            {
                HER_Nombre = crear.NombreServicio,
                HER_CreadorId = await _usuarioService.ObtenerIdentificadorSoloUsuarioAsync(username),
                HER_RegionId = regionId
            };
            _context.HER_Servicio.Add(servicio);

            //Integrante
            var integrante = new HER_ServicioIntegrante()
            {
                HER_ServicioId = servicio.HER_ServicioId,
                HER_IntegranteId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(crear.Usuario)
            };

            _context.HER_ServicioIntegrante.Add(integrante);
            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
    }
}
