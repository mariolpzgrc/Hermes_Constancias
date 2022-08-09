using Hermes2018.Data;
using Hermes2018.Models.Plantilla;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class PlantillaService : IPlantillaService
    {
        private readonly ApplicationDbContext _context;
        private readonly CultureInfo _cultureEs;

        public PlantillaService(ApplicationDbContext context)
        {
            _context = context;
            _cultureEs = new CultureInfo("es-MX");
        }

        public async Task<bool> ExistePlantillaAsync(string nombre, string userName)
        {
            var existeQuery = _context.HER_Plantilla
                .Where(x => x.HER_Nombre == nombre
                            && x.HER_InfoUsuario.HER_UserName == userName
                            && x.HER_InfoUsuario.HER_Activo == true)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> ExistePlantillaPorIdAsync(int plantillaId, string userName)
        {
            var existeQuery = _context.HER_Plantilla
                .Where(x => x.HER_PlantillaId == plantillaId
                            && x.HER_InfoUsuario.HER_UserName == userName
                            && x.HER_InfoUsuario.HER_Activo == true)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> GuardarPlantillaAsync(NuevaPlantillaViewModel nuevaPlantilla)
        {
            int result = 0;

            var infoUsuarioIdQuery = _context.HER_InfoUsuario
                    .Where(x => x.HER_UserName == nuevaPlantilla.HER_Usuario
                             && x.HER_Activo == true)
                    .Select(x => x.HER_InfoUsuarioId)
                    .AsQueryable();

            var infoUsuarioId = await infoUsuarioIdQuery.FirstOrDefaultAsync();

            var plantilla = new HER_Plantilla
            {
                HER_Nombre = nuevaPlantilla.HER_Nombre,
                HER_Texto = nuevaPlantilla.HER_Texto,
                HER_Fecha_Registro = DateTime.Now,
                HER_InfoUsuarioId = infoUsuarioId
            };
            await _context.HER_Plantilla.AddAsync(plantilla);
            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
        public async Task<PlantillaViewModel> ObtenerPlantillaAsync(string nombre, string userName)
        {
            var plantillaQuery = _context.HER_Plantilla
                .Where(x => x.HER_Nombre == nombre
                         && x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true)
                .AsNoTracking()
                .Select(x => new PlantillaViewModel {
                    HER_PlantillaId = x.HER_PlantillaId,
                    HER_Nombre = x.HER_Nombre,
                    HER_Texto = x.HER_Texto
                })
                .AsQueryable();

            return await plantillaQuery.FirstOrDefaultAsync();
        }
        public async Task<PlantillaViewModel> ObtenerPlantillaPorIdAsync(int plantillaId, string userName)
        {
            var plantillaQuery = _context.HER_Plantilla
                .Where(x => x.HER_PlantillaId == plantillaId
                         && x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true)
                .AsNoTracking()
                .Select(x => new PlantillaViewModel
                {
                    HER_PlantillaId = x.HER_PlantillaId,
                    HER_Nombre = x.HER_Nombre,
                    HER_Texto = x.HER_Texto
                })
                .AsQueryable();

            return await plantillaQuery.FirstOrDefaultAsync();
        }
        public async Task<List<PlantillaSimplificadaViewModel>> ObtenerPlantillasAsync(string userName)
        {
            var listado = new List<PlantillaSimplificadaViewModel>();

            var plantillasQuery = _context.HER_Plantilla
                .Where(x => x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true)
                .AsNoTracking()
                .OrderBy(x => x.HER_Nombre)
                .Select(x => new PlantillaSimplificadaViewModel
                {
                    HER_PlantillaId = x.HER_PlantillaId,
                    HER_Nombre = x.HER_Nombre
                })
                .AsQueryable();

            return await plantillasQuery.ToListAsync();
        }
        public async Task<PlantillaDetalleViewModel> ObtenerDetallePlantillaAsync(int plantillaId, string userName)
        {
            var plantillaQuery = _context.HER_Plantilla
                .Where(x => x.HER_PlantillaId == plantillaId
                         && x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true)
                .AsNoTracking()
                .Select(x => new PlantillaDetalleViewModel
                {
                    HER_PlantillaId = x.HER_PlantillaId,
                    HER_Nombre = x.HER_Nombre,
                    HER_Texto = x.HER_Texto,
                    HER_Fecha = x.HER_Fecha_Registro.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs)
                })
                .AsQueryable();

            return await plantillaQuery.FirstOrDefaultAsync();
        }
        public async Task<EditarPlantillaViewModel> ObtenerInfoEditarPlantillaAsync(int plantillaId, string userName)
        {
            var plantillaQuery = _context.HER_Plantilla
                .Where(x => x.HER_PlantillaId == plantillaId
                         && x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true)
                .AsNoTracking()
                .Select(x => new EditarPlantillaViewModel
                {
                    PlantillaId = x.HER_PlantillaId,
                    Nombre = x.HER_Nombre,
                    Texto = x.HER_Texto
                })
                .AsQueryable();

            return await plantillaQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ActualizarPlantillaAsync(EditarPlantillaViewModel modelo, string username)
        {
            int result = 0;

            //Plantilla
            var plantillaQuery = _context.HER_Plantilla
                .Where(x => x.HER_PlantillaId == modelo.PlantillaId
                    && x.HER_InfoUsuario.HER_UserName == username
                    && x.HER_InfoUsuario.HER_Activo == true)
                .AsQueryable();

            var plantilla = await plantillaQuery.FirstOrDefaultAsync();
            if (plantilla != null)
            {
                plantilla.HER_Nombre = modelo.Nombre;
                plantilla.HER_Texto = modelo.Texto;

                _context.HER_Plantilla.Update(plantilla).State = EntityState.Modified;
                result = await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }
        public async Task<bool> GuardarPlantillaDesdeFormularioAsync(CrearPlantillaViewModel nuevaPlantilla, int infoUsuarioId)
        {
            int result = 0;

            var plantilla = new HER_Plantilla
            {
                HER_Nombre = nuevaPlantilla.Nombre,
                HER_Texto = nuevaPlantilla.Texto,
                HER_Fecha_Registro = DateTime.Now,
                HER_InfoUsuarioId = infoUsuarioId
            };
            await _context.HER_Plantilla.AddAsync(plantilla);
            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
        public async Task<bool> EliminarPlantillaAsync(int plantillaId, string username)
        {
            int result = 0;

            var plantillaQuery = _context.HER_Plantilla
                    .Where(x => x.HER_PlantillaId == plantillaId
                        && x.HER_InfoUsuario.HER_UserName == username)
                    .AsNoTracking()
                    .AsQueryable();

            var plantilla = await plantillaQuery.FirstOrDefaultAsync();

            //Borrar
            _context.HER_Plantilla.Remove(plantilla);
            result = _context.SaveChanges();

            return result > 0 ? true : false;
        }
    }
}
