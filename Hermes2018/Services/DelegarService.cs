using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Services
{
    public class DelegarService : IDelegarService
    {
        private ApplicationDbContext _context;
        private IUsuarioService _usuarioService;
        private readonly CultureInfo _cultureEs;

        public DelegarService(ApplicationDbContext context, IUsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
            _cultureEs = new CultureInfo("es-MX");
        }
        public async Task<List<DelegadosViewModel>> ObtenerDelegadosAsync(string username)
        {
            var delegadosQuery = _context.HER_Delegar
                //.Include(x => x.HER_Delegado)
                //.Include(x => x.HER_Titular)
                .Where(x => x.HER_Titular.HER_UserName == username
                         && x.HER_Titular.HER_Activo == true)
                .AsNoTracking()
                .Select(x => new DelegadosViewModel()
                {
                    DelegarId = x.HER_DelegarId,
                    Nombre = x.HER_Delegado.HER_NombreCompleto,
                    Usuario = x.HER_Delegado.HER_UserName,
                    Correo = x.HER_Delegado.HER_Correo,
                    Tipo = (x.HER_Tipo == ConstDelegar.TipoN1) ? ConstDelegar.TipoT1 : ConstDelegar.TipoT2
                })
                .AsQueryable();

            return await delegadosQuery.ToListAsync();
        }
        public async Task<int> ObtenerTotalDelegadosAsync(string username)
        {
            var delegadosTotalQuery = _context.HER_Delegar
                .Where(x => x.HER_Titular.HER_UserName == username
                         && x.HER_Titular.HER_Activo == true)
                 .AsNoTracking()
                 .AsQueryable();

            return await delegadosTotalQuery.CountAsync();
        }
        public async Task<bool> ExisteDelegadoAsync(string username, string delegado)
        {
            var delegadoQuery = _context.HER_Delegar
                            .Where(x => x.HER_Titular.HER_UserName == username
                                     && x.HER_Titular.HER_Activo == true
                                     && x.HER_Delegado.HER_UserName == delegado)
                            .AsNoTracking()
                            .AsQueryable();

            return await delegadoQuery.AnyAsync();
        }
        public async Task<bool> GuardarDelegadoAsync(string titular, string delegado, int tipo)
        {
            int result = 0;
            //--
            int titularId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(titular);
            int delegadoId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(delegado);

            if (titularId > 0 && delegadoId > 0)
            {
                //Nuevo delegado
                var nuevoDelegado = new HER_Delegar()
                {
                    HER_TitularId = titularId,
                    HER_DelegadoId = delegadoId,
                    HER_FechaRegistro = DateTime.Now,
                    HER_FechaActualizacion = DateTime.Now,
                    HER_Tipo = tipo
                };

                //Guardar el delegado
                _context.HER_Delegar.Add(nuevoDelegado);
                result = await _context.SaveChangesAsync();
            }
            
            return result > 0 ? true : false;
        }

        public async Task<DelegadoDetalleViewModel> ObtenerDetalleDelegado(int delegarId)
        {
            var delegarQuery = _context.HER_Delegar
                .Where(x => x.HER_DelegarId == delegarId)
                .Select(x => new DelegadoDetalleViewModel
                {
                    DelegarId = x.HER_DelegarId,
                    Nombre = x.HER_Delegado.HER_NombreCompleto,
                    Usuario = x.HER_Delegado.HER_UserName,
                    Correo = x.HER_Delegado.HER_Correo,
                    Tipo = (x.HER_Tipo == ConstDelegar.TipoN1) ? ConstDelegar.TipoT1 : ConstDelegar.TipoT2,
                    Fecha_Actualizacion = x.HER_FechaActualizacion.ToString("dd/MM/yyyy HH:mm", _cultureEs),
                    Fecha_Registro = x.HER_FechaRegistro.ToString("dd/MM/yyyy HH:mm", _cultureEs)
                })
                .AsNoTracking()
                .AsQueryable();

            return await delegarQuery.FirstOrDefaultAsync();
        }
        public async Task<DelegadoEditarViewModel> ObtenerDelegadoParaEdicion(string username, int delegarId)
        {
            var delegarQuery = _context.HER_Delegar
                .Where(x => x.HER_Titular.HER_UserName == username 
                         && x.HER_DelegarId == delegarId)
                .Select(x => new DelegadoEditarViewModel
                {
                    DelegarId = x.HER_DelegarId,
                    Nombre = x.HER_Delegado.HER_NombreCompleto,
                    Usuario = x.HER_Delegado.HER_UserName,
                    Correo = x.HER_Delegado.HER_Correo,
                    Tipo = x.HER_Tipo
                })
                .AsNoTracking()
                .AsQueryable();

            return await delegarQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ActualizarDelegadoAsync(string username, DelegadoEditarViewModel model)
        {
            int result = 0;
            //--
            var delegarQuery = _context.HER_Delegar
                .Where(x => x.HER_Titular.HER_UserName == username  
                         && x.HER_DelegarId == model.DelegarId)
                .AsQueryable();

            var delegar = await delegarQuery.FirstOrDefaultAsync();

            if (delegar != null) {
                delegar.HER_Tipo = model.Tipo;
                delegar.HER_FechaActualizacion = DateTime.Now;

                _context.HER_Delegar.Update(delegar).State = EntityState.Modified;
                result = await _context.SaveChangesAsync();
            }
            
            return result > 0 ? true : false;
        }

        public async Task<DelegadoBorrarViewModel> ObtenerDelegadoParaBorrar(string username, int delegarId)
        {
            var delegarQuery = _context.HER_Delegar
                .Where(x => x.HER_Titular.HER_UserName == username
                         && x.HER_DelegarId == delegarId)
                .Select(x => new DelegadoBorrarViewModel
                {
                    DelegarId = x.HER_DelegarId,
                    Nombre = x.HER_Delegado.HER_NombreCompleto,
                    Usuario = x.HER_Delegado.HER_UserName,
                    Correo = x.HER_Delegado.HER_Correo,
                    Tipo = (x.HER_Tipo == ConstDelegar.TipoN1) ? ConstDelegar.TipoT1 : ConstDelegar.TipoT2
                })
                .AsNoTracking()
                .AsQueryable();

            return await delegarQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> BorrarDelegadoAsync(string username, DelegadoBorrarViewModel model)
        {
            int result = 0;
            //--
            var delegarQuery = _context.HER_Delegar
                .Where(x => x.HER_Titular.HER_UserName == username
                         && x.HER_DelegarId == model.DelegarId)
                .AsQueryable();

            var delegar = await delegarQuery.FirstOrDefaultAsync();

            if (delegar != null)
            {
                _context.HER_Delegar.Remove(delegar).State = EntityState.Deleted;
                result = await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }
    }
}
