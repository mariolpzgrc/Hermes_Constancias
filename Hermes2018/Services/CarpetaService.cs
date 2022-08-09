using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Carpeta;
using Hermes2018.Models.Documento;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Services
{
    public class CarpetaService : ICarpetaService
    {
        private readonly ApplicationDbContext _context;
        private readonly CultureInfo _cultureEs;

        public CarpetaService(ApplicationDbContext context)
        {
            _context = context;
            _cultureEs = new CultureInfo("es-MX");
        }

        public async Task<List<CarpetaViewModel>> ObtenerCarpetasAsync(int infoUsuarioId)
        {
            List<CarpetaViewModel> listadoCarpetas = new List<CarpetaViewModel>();

            if (infoUsuarioId > 0)
            {
                var carpetasQuery = _context.HER_Carpeta
                        .Where(x => x.HER_InfoUsuario.HER_InfoUsuarioId == infoUsuarioId
                                 && x.HER_InfoUsuario.HER_Activo == true
                                 && x.HER_Nivel == ConstNivelCarpeta.NivelN1)
                         .AsNoTracking()
                         .Select(x => new CarpetaViewModel
                         {
                             CarpetaId = x.HER_CarpetaId,
                             Nombre = x.HER_Nombre,
                             Fecha = x.HER_FechaRegistro.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs)
                         })
                         .OrderBy(x => x.Nombre)
                        .AsQueryable();

                listadoCarpetas = await carpetasQuery.ToListAsync();
            }

            return listadoCarpetas;
        }
        public async Task<bool> ExisteCarpetaAsync(string nombreCarpeta, int infoUsuarioId)
        {
            var existeQuery = _context.HER_Carpeta
                .Where(x => x.HER_Nombre == nombreCarpeta 
                         && x.HER_InfoUsuarioId == infoUsuarioId 
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN1)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> ExisteCarpetaPorIdAsync(int carpetaId, int infoUsuarioId)
        {
            var existeQuery = _context.HER_Carpeta
                .Where(x => x.HER_CarpetaId == carpetaId
                         && x.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN1)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> CarpetaTieneSubcarpetasPorIdAsync(int carpetaId, int infoUsuarioId)
        {
            var tieneQuery = _context.HER_Carpeta
                .Where(x => x.HER_CarpetaPadreId == carpetaId
                         && x.HER_InfoUsuarioId == infoUsuarioId)
                .AsNoTracking()
                .AsQueryable();

            return await tieneQuery.AnyAsync();
        }
        public async Task<bool> CarpetaTieneDocumentosAsociados(int carpetaId, int infoUsuarioId)
        {
            var tieneEnvioQuery = _context.HER_Envio
                .Where(x => x.HER_CarpetaId == carpetaId 
                         && x.HER_Carpeta.HER_InfoUsuarioId == infoUsuarioId)
                .Select(x => x.HER_CarpetaId)
                .AsQueryable();

            var tieneEnvioRecepcionQuery = _context.HER_Recepcion
                .Where(x => x.HER_CarpetaId == carpetaId
                         && x.HER_Carpeta.HER_InfoUsuarioId == infoUsuarioId)
                .Select(x => x.HER_CarpetaId)
                .AsQueryable();

            return await tieneEnvioQuery.AnyAsync() || await tieneEnvioRecepcionQuery.AnyAsync();
        }
        public async Task<bool> GuardarCarpetaAsync(CrearCarpetaViewModel modelo, int infoUsuarioId)
        {
            int result = 0;

            //Carpeta
            var carpeta = new HER_Carpeta()
            {
                HER_Nombre = modelo.NombreCarpeta,
                HER_InfoUsuarioId = infoUsuarioId,
                HER_FechaRegistro = DateTime.Now,
                HER_FechaActualizacion = DateTime.Now,
                HER_Nivel = ConstNivelCarpeta.NivelN1,
                HER_CarpetaPadreId = null
            };

            _context.HER_Carpeta.Add(carpeta);
            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
        public async Task<HER_Carpeta> ObtenerCarpetaAsync(int infoUsuarioId, int carpetaId)
        {
            var carpetaQuery = _context.HER_Carpeta
                .Where(x => x.HER_InfoUsuarioId == infoUsuarioId 
                         && x.HER_CarpetaId == carpetaId 
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN1)
                .AsNoTracking()
                .AsQueryable();

            return await carpetaQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ActualizarCarpetaAsync(EditarCarpetaViewModel modelo, int infoUsuarioId)
        {
            int result = 0;

            //Carpeta
            var carpetaQuery = _context.HER_Carpeta
                .Where(x => x.HER_CarpetaId == modelo.CarpetaId 
                         && x.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN1)
                .AsQueryable();

            var carpeta = await carpetaQuery.FirstOrDefaultAsync();

            if (carpeta != null)
            {
                carpeta.HER_Nombre = modelo.NombreCarpeta;
                carpeta.HER_FechaActualizacion = DateTime.Now;

                _context.HER_Carpeta.Update(carpeta).State = EntityState.Modified;
                result = await _context.SaveChangesAsync();
            }
            
            return (result > 0) ? true : false;
        }
        public async Task<bool> BorrarCarpetaAsync(BorrarCarpetaViewModel modelo, int infoUsuarioId)
        {
            int result = 0;

            //Carpeta
            var carpetaQuery = _context.HER_Carpeta
                .Where(x => x.HER_CarpetaId == modelo.CarpetaId
                         && x.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN1)
                .AsQueryable();

            var carpeta = await carpetaQuery.FirstOrDefaultAsync();

            if (carpeta != null)
            {
                _context.HER_Carpeta.Remove(carpeta).State = EntityState.Deleted;
                result = await _context.SaveChangesAsync();
            }

            return (result > 0) ? true : false;
        }
        //-------
        public async Task<List<SubcarpetaViewModel>> ObtenerSubcarpetasAsync(int infoUsuarioId, int carpetaId)
        {
            List<SubcarpetaViewModel> listadoSubcarpetas = new List<SubcarpetaViewModel>();

            if (infoUsuarioId > 0)
            {
                var subcarpetasQuery = _context.HER_Carpeta
                        .Where(x => x.HER_InfoUsuario.HER_InfoUsuarioId == infoUsuarioId
                                 && x.HER_InfoUsuario.HER_Activo == true
                                 && x.HER_CarpetaPadreId == carpetaId
                                 && x.HER_Nivel == ConstNivelCarpeta.NivelN2)
                         .AsNoTracking()
                         .Select(x => new SubcarpetaViewModel
                         {
                             SubcarpetaId = x.HER_CarpetaId,
                             Nombre = x.HER_Nombre,
                             Fecha = x.HER_FechaRegistro.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                         })
                         .OrderBy(x => x.Nombre)
                        .AsQueryable();

                listadoSubcarpetas = await subcarpetasQuery.ToListAsync();
            }

            return listadoSubcarpetas;
        }
        public async Task<bool> ExisteSubcarpetaAsync(string nombreSubcarpeta, int infoUsuarioId, int carpetaId)
        {
            var existeQuery = _context.HER_Carpeta
                .Where(x => x.HER_Nombre == nombreSubcarpeta
                         && x.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_CarpetaPadreId == (int)carpetaId
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN2)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> ExisteSubcarpetaPorIdAsync(int subcarpetaId, int infoUsuarioId, int carpetaId)
        {
            var existeQuery = _context.HER_Carpeta
                .Where(x => x.HER_CarpetaId == subcarpetaId
                         && x.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_CarpetaPadreId == (int)carpetaId
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN2)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> SubcarpetaTieneDocumentosAsociados(int subcarpetaId, int infoUsuarioId, int carpetaId)
        {
            var tieneEnvioQuery = _context.HER_Envio
                .Where(x => x.HER_Carpeta.HER_CarpetaId == subcarpetaId
                         && x.HER_Carpeta.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Carpeta.HER_CarpetaPadreId == carpetaId
                         && x.HER_Carpeta.HER_Nivel == ConstNivelCarpeta.NivelN2)
                .Select(x => x.HER_CarpetaId)
                .AsQueryable();

            var tieneEnvioRecepcionQuery = _context.HER_Recepcion
                .Where(x => x.HER_Carpeta.HER_CarpetaId == subcarpetaId
                         && x.HER_Carpeta.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Carpeta.HER_CarpetaPadreId == carpetaId
                         && x.HER_Carpeta.HER_Nivel == ConstNivelCarpeta.NivelN2)
                .Select(x => x.HER_CarpetaId)
                .AsQueryable();

            var respuesta = await tieneEnvioQuery.AnyAsync() || await tieneEnvioRecepcionQuery.AnyAsync();
            return respuesta;
        }
        public async Task<bool> GuardarSubcarpetasAsync(CrearSubcarpetaViewModel modelo, int infoUsuarioId, int carpetaId)
        {
            int result = 0;

            //SubCarpeta
            var subcarpeta = new HER_Carpeta()
            {
                HER_Nombre = modelo.NombreSubcarpeta,
                HER_InfoUsuarioId = infoUsuarioId,
                HER_FechaRegistro = DateTime.Now,
                HER_FechaActualizacion = DateTime.Now,
                HER_Nivel = ConstNivelCarpeta.NivelN2,
                HER_CarpetaPadreId = (int)carpetaId
            };

            _context.HER_Carpeta.Add(subcarpeta);
            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
        public async Task<HER_Carpeta> ObtenerSubcarpetaAsync(int infoUsuarioId, int subcarpetaId)
        {
            var subcarpetaQuery = _context.HER_Carpeta
                .Where(x => x.HER_InfoUsuarioId == infoUsuarioId 
                         && x.HER_CarpetaId == subcarpetaId 
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN2)
                .Include(x => x.HER_CarpetaPadre)
                .AsNoTracking()
                .AsQueryable();

            return await subcarpetaQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ActualizarSubcarpetaAsync(EditarSubcarpetaViewModel modelo, int infoUsuarioId)
        {
            int result = 0;

            //Carpeta
            var carpetaQuery = _context.HER_Carpeta
                .Where(x => x.HER_CarpetaId == modelo.SubcarpetaId
                         && x.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN2)
                .AsQueryable();

            var carpeta = await carpetaQuery.FirstOrDefaultAsync();
            if (carpeta != null)
            {
                carpeta.HER_Nombre = modelo.NombreSubcarpeta;
                carpeta.HER_CarpetaPadreId = modelo.CarpetaPadreId;
                carpeta.HER_FechaActualizacion = DateTime.Now;

                _context.HER_Carpeta.Update(carpeta).State = EntityState.Modified;
                result = await _context.SaveChangesAsync();
            }

            return (result > 0) ? true : false;
        }
        public async Task<bool> BorrarSubcarpetaAsync(BorrarSubcarpetaViewModel modelo, int infoUsuarioId)
        {
            int result = 0;

            //Subcarpeta
            var subcarpetaQuery = _context.HER_Carpeta
                .Where(x => x.HER_CarpetaId == modelo.SubcarpetaId
                         && x.HER_CarpetaPadreId == modelo.CarpetaPadreId
                         && x.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Nivel == ConstNivelCarpeta.NivelN2)
                .AsQueryable();

            var subcarpeta = await subcarpetaQuery.FirstOrDefaultAsync();
            //--
            if (subcarpeta != null)
            {
                _context.HER_Carpeta.Remove(subcarpeta).State = EntityState.Deleted;
                result = await _context.SaveChangesAsync();
            }

            return (result > 0) ? true : false;
        }
        public async Task<List<CarpetasJsonMdel>> ListadoCarpetasPorUsuarioAsync(string userName)
        {
            List<CarpetasJsonMdel> listadoCarpetas = new List<CarpetasJsonMdel>();
            List<SubcarpetasJsonModel> listadoSubcarpetas = new List<SubcarpetasJsonModel>();

            var carpetasQuery = _context.HER_Carpeta
                        .Where(x => x.HER_InfoUsuario.HER_UserName == userName
                                 && x.HER_InfoUsuario.HER_Activo == true)
                        .OrderBy(x => x.HER_Nombre)
                        .AsNoTracking()
                        .AsQueryable();

            List<HER_Carpeta> carpetas = await carpetasQuery
                .Where(x => x.HER_Nivel == ConstNivelCarpeta.NivelN1)
                .ToListAsync();

            List<HER_Carpeta> subcarpetas = await carpetasQuery
                .Where(x => x.HER_Nivel == ConstNivelCarpeta.NivelN2)
                .ToListAsync();

            foreach (var carpeta in carpetas)
            {
                listadoSubcarpetas = subcarpetas
                    .Where(x => x.HER_CarpetaPadreId == carpeta.HER_CarpetaId)
                    .Select(y => new SubcarpetasJsonModel() {
                        SubcarpetaId = y.HER_CarpetaId,
                        Nombre = y.HER_Nombre,
                    })
                    .OrderBy(y => y.Nombre)
                    .ToList();

                listadoCarpetas.Add(new CarpetasJsonMdel()
                {
                    CarpetaId = carpeta.HER_CarpetaId,
                    Nombre = carpeta.HER_Nombre,
                    Subcarpetas = listadoSubcarpetas
                });
            }

            return listadoCarpetas;
        }

        public async Task<bool> MoverDocumentosRecibidosAsync(MoverDocumentoJsonModel solicitud)
        {
            int result = 0;
            IQueryable<HER_Recepcion> recepcionQuery;
            if (solicitud.Carpeta == 0)
            {
                foreach (var par in solicitud.Valores)
                {
                    recepcionQuery = _context.HER_Recepcion
                        .Where(x => x.HER_Para.HER_UserName == solicitud.Usuario
                                 && x.HER_Para.HER_Activo == true
                                 && x.HER_EnvioId == par.Id
                                 && x.HER_Envio.HER_TipoEnvioId == par.Tipo)
                        .AsQueryable();

                    var recepcion = await recepcionQuery.FirstOrDefaultAsync();
                    recepcion.HER_CarpetaId = null;

                    _context.HER_Recepcion.Update(recepcion).State = EntityState.Modified;
                }
            }
            else
            {
                foreach (var par in solicitud.Valores)
                {
                    recepcionQuery = _context.HER_Recepcion
                        .Where(x => x.HER_Para.HER_UserName == solicitud.Usuario
                                 && x.HER_Para.HER_Activo == true
                                 && x.HER_EnvioId == par.Id
                                 && x.HER_Envio.HER_TipoEnvioId == par.Tipo)
                        .AsQueryable();

                    var recepcion = await recepcionQuery.FirstOrDefaultAsync();
                    recepcion.HER_CarpetaId = solicitud.Carpeta;

                    _context.HER_Recepcion.Update(recepcion).State = EntityState.Modified;
                }
            }

            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
        public async Task<bool> MoverDocumentosEnviadosAsync(MoverDocumentoJsonModel solicitud)
        {
            int result = 0;
            IQueryable<HER_Envio> envioQuery;

            if (solicitud.Carpeta == 0)
            {
                foreach (var par in solicitud.Valores)
                {
                    envioQuery = _context.HER_Envio
                        .Where(x => x.HER_De.HER_UserName == solicitud.Usuario
                                    && x.HER_De.HER_Activo == true
                                    && x.HER_EnvioId == par.Id
                                    && x.HER_TipoEnvioId == par.Tipo)
                        .AsQueryable();

                    var envio = await envioQuery.FirstOrDefaultAsync();
                    envio.HER_CarpetaId = null;

                    _context.HER_Envio.Update(envio).State = EntityState.Modified;
                }
            }
            else
            {
                foreach (var par in solicitud.Valores)
                {
                    envioQuery = _context.HER_Envio
                        .Where(x => x.HER_De.HER_UserName == solicitud.Usuario
                                    && x.HER_De.HER_Activo == true
                                    && x.HER_EnvioId == par.Id
                                    && x.HER_TipoEnvioId == par.Tipo)
                        .AsQueryable();

                    var envio = await envioQuery.FirstOrDefaultAsync();
                    envio.HER_CarpetaId = solicitud.Carpeta;

                    _context.HER_Envio.Update(envio).State = EntityState.Modified;
                }
            }
            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<DetallesCarpetaViewModel> ObtenerDetallesCarpetaAsync(int carpetaId, string username)
        {
            var carpetaQuery = _context.HER_Carpeta
                .Where(x => x.HER_CarpetaId == carpetaId
                         && x.HER_InfoUsuario.HER_UserName == username 
                         && x.HER_InfoUsuario.HER_Activo == true)
                .AsNoTracking()
                .Select(x => new DetallesCarpetaViewModel() {
                    CarpetaId = x.HER_CarpetaId,
                    Nombre = x.HER_Nombre,
                    Fecha_Creacion = x.HER_FechaRegistro.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                    Fecha_Modificacion = x.HER_FechaActualizacion.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                    Nivel = (x.HER_Nivel == ConstNivelCarpeta.NivelN1)? ConstNivelCarpeta.NivelT1 : ConstNivelCarpeta.NivelT2,
                    CarpetaPadre = (x.HER_CarpetaPadreId != null)? x.HER_CarpetaPadre.HER_Nombre : string.Empty,
                    CarpetaPadreId = x.HER_CarpetaPadreId
                })
                .AsQueryable();

            return await carpetaQuery.FirstOrDefaultAsync();
        }
    }
}
