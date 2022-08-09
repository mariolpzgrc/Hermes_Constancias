using Hermes2018.Data;
using Hermes2018.Models.Grupo;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class GrupoService: IGrupoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsuarioService _usuarioService;

        public GrupoService(ApplicationDbContext context, 
                IUsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
        }

        public List<GruposViewModel> ObtenerGrupos(string userName)
        {
            List<GruposViewModel> listadoGrupos = new List<GruposViewModel>();
            if (!string.IsNullOrEmpty(userName))
            {
                var gruposQuery = _context.HER_Grupo
                        .Where(x => x.HER_Creador.HER_UserName == userName
                                 && x.HER_Creador.HER_Activo == true)
                         .AsNoTracking()
                         .Select(x => new GruposViewModel
                         {
                             HER_GrupoId = x.HER_GrupoId,
                             HER_Nombre = x.HER_Nombre
                         })
                        .AsQueryable();

                listadoGrupos = gruposQuery.ToList();
            }

            return listadoGrupos;
        }
        public async Task<List<GruposViewModel>> ObtenerGruposAsync(string userName)
        {
            List<GruposViewModel> listadoGrupos = new List<GruposViewModel>();
            if (!string.IsNullOrEmpty(userName))
            {
                var gruposQuery = _context.HER_Grupo
                        .Where(x => x.HER_Creador.HER_UserName == userName
                                 && x.HER_Creador.HER_Activo == true)
                        .AsNoTracking()
                        .Select(x => new GruposViewModel
                        {
                            HER_GrupoId = x.HER_GrupoId,
                            HER_Nombre = x.HER_Nombre
                        })
                        .AsQueryable();

                listadoGrupos = await gruposQuery.ToListAsync();
            }

            return listadoGrupos;
        }
        public async Task<List<UsuarioLocalJsonModel>> ObtenerIntegrantesGrupoAsync(int grupoId)
        {
            var listadoIntegrantes = new List<UsuarioLocalJsonModel>();
            
            if (grupoId > 0)
            {
                var integrantesQuery = _context.HER_GrupoIntegrante
                                          .Where(x => x.HER_GrupoId == grupoId
                                                   && x.HER_Integrante.HER_Activo == true)
                                           .Select(x => new UsuarioLocalJsonModel {
                                               HER_UserName = x.HER_Integrante.HER_UserName,
                                               HER_Correo = x.HER_Integrante.HER_Correo,
                                               HER_NombreCompleto = x.HER_Integrante.HER_NombreCompleto,
                                               HER_Region = x.HER_Integrante.HER_Area.HER_Region.HER_Nombre,
                                               HER_Area = x.HER_Integrante.HER_Area.HER_Nombre,
                                               HER_Puesto = x.HER_Integrante.HER_Puesto,
                                               HER_Tipo = x.HER_Integrante.HER_RolNombre,
                                               HER_Titular = x.HER_Integrante.HER_Titular
                                           })
                                          .AsQueryable();

                listadoIntegrantes = await integrantesQuery.ToListAsync();
            }

            return listadoIntegrantes;
        }

        public async Task<bool> ExisteGrupoAsync(string nombreGrupo, int usuarioCreadorId)
        {
            var existeQuery =  _context.HER_Grupo
                    .Where(x => x.HER_Nombre == nombreGrupo
                             && x.HER_CreadorId == usuarioCreadorId)
                    .AsNoTracking()
                    .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> GuardarGrupoAsync(string nombreGrupo, int usuarioCreadorId, List<IntegranteViewModel> integrantes)
        {
            int result = 0;

            //Grupo
            var grupo = new HER_Grupo()
            {
                HER_Nombre = nombreGrupo,
                HER_CreadorId = usuarioCreadorId,
            };
            _context.HER_Grupo.Add(grupo);

            //Integrantes
            var integrantesGrupo = new List<HER_GrupoIntegrante>();
            //--
            var integrantesUsuario = integrantes.Select(x => x.Usuario).ToList();
            var integrantesId = await _usuarioService.ObtenerIdentificadoresUsuariosAsync(integrantesUsuario);
            //--
            foreach (var integranteId in integrantesId)
            {
                integrantesGrupo.Add(new HER_GrupoIntegrante()
                {
                    HER_GrupoId = grupo.HER_GrupoId,
                    HER_IntegranteId = integranteId
                });
            }

            _context.HER_GrupoIntegrante.AddRange(integrantesGrupo);
            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
        public async Task<DetalleGrupoViewModel> ObtenerDetalleGrupoAsync(int grupoId)
        {
            var grupoQuery = _context.HER_Grupo
                .Where(x => x.HER_GrupoId == grupoId)
                .Include(x => x.HER_Integrantes)
                    .ThenInclude(x => x.HER_Integrante)
                .AsNoTracking()
                .Select(x => new DetalleGrupoViewModel() {
                    Nombre = x.HER_Nombre,
                    GrupoId = x.HER_GrupoId,
                    Integrantes = (from y in x.HER_Integrantes
                                   where y.HER_Integrante.HER_Activo == true
                                   select new IntegranteGrupoViewModel()
                                   {
                                       GrupoId = y.HER_GrupoId,
                                       UsuarioId = y.HER_Integrante.HER_InfoUsuarioId,
                                       Usuario = y.HER_Integrante.HER_UserName,
                                       Nombre = y.HER_Integrante.HER_NombreCompleto,
                                       Correo = y.HER_Integrante.HER_Correo
                                   }).ToList()
                })
                .AsQueryable();

            return await grupoQuery.FirstOrDefaultAsync();
        }
        public async Task<EditarGrupoViewModel> ObtenerGrupoParaEdicion(int grupoId)
        {
            var grupoQuery = _context.HER_Grupo
                .Where(x => x.HER_GrupoId == grupoId)
                .Select(x => new EditarGrupoViewModel
                {
                    GrupoId = x.HER_GrupoId,
                    Nombre = x.HER_Nombre
                })
                .AsNoTracking()
                .AsQueryable();

            return await grupoQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ActualizarGrupoAsync(EditarGrupoViewModel model)
        {
            int result = 0;
    
            var grupoQuery = _context.HER_Grupo
                .Where(x =>  x.HER_GrupoId == model.GrupoId)
                .AsQueryable();

            var grupo = await grupoQuery.FirstOrDefaultAsync();

            if (grupo != null)
            {
                grupo.HER_Nombre = model.Nombre;

                _context.HER_Grupo.Update(grupo).State = EntityState.Modified;
                result = await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }

        public async Task<bool> GrupoTieneIntegrantesAsociados(int infoUsuarioId, int grupoId)
        {
            var tieneIntegranteQuery = _context.HER_GrupoIntegrante
                .Where(x => x.HER_GrupoId == grupoId
                         && x.HER_Grupo.HER_CreadorId == infoUsuarioId
                         && x.HER_Integrante.HER_Activo == true)
                .Select(x => x.HER_GrupoId)
                .AsQueryable();

            return await tieneIntegranteQuery.AnyAsync();
        }

        public async Task<bool> GrupoTieneIntegrantesAsociadosInactivos(int infoUsuarioId, int grupoId)
        {
            var tieneIntegranteInactivoQuery = _context.HER_GrupoIntegrante
                 .Where(x => x.HER_GrupoId == grupoId
                         && x.HER_Grupo.HER_CreadorId == infoUsuarioId
                         && x.HER_Integrante.HER_Activo == false)
                .Select(x => x.HER_GrupoId)
                .AsQueryable();

            return await tieneIntegranteInactivoQuery.AnyAsync();
        }

        public async Task<BorrarGrupoViewModel> ObtenerGrupoParaBorrar(string username, int grupoId)
        {
            var grupoQuery = _context.HER_Grupo
                .Where(x => x.HER_Creador.HER_BandejaUsuario == username
                         && x.HER_GrupoId == grupoId)
                .Select(x => new BorrarGrupoViewModel
                {
                    GrupoId = x.HER_GrupoId,
                    Nombre = x.HER_Nombre
                })
                .AsNoTracking()
                .AsQueryable();

            return await grupoQuery.FirstOrDefaultAsync();
        }

        public async Task<bool> BorrarGrupoAsync(string username, BorrarGrupoViewModel modelo)
        {
            int result = 0;

            //Grupo
            var grupoQuery = _context.HER_Grupo
                .Where(x => x.HER_GrupoId == modelo.GrupoId
                         && x.HER_Creador.HER_BandejaUsuario == username)
                .AsQueryable();

            var grupo = await grupoQuery.FirstOrDefaultAsync();

            if (grupo != null)
            {
                _context.HER_Grupo.Remove(grupo).State = EntityState.Deleted;
                result = await _context.SaveChangesAsync();
            }

            return (result > 0) ? true : false;
        }


        //Integrantes

        public async Task<bool> ExisteIntegranteAsync(string username, int grupoId)
        {
            var grupoQuery = _context.HER_GrupoIntegrante
                            .Where(x => x.HER_Integrante.HER_UserName == username 
                                     && x.HER_Integrante.HER_Activo == true
                                     && x.HER_GrupoId == grupoId)
                            .AsNoTracking()
                            .AsQueryable();

            return await grupoQuery.AnyAsync();
        }

        public async Task<AgregarIntegranteGrupoViewModel> ObtenerGrupoIntegranteParaAgregar(int grupoId)
        {
            var grupoQuery = _context.HER_Grupo
                .Where(x => x.HER_GrupoId == grupoId)
                .Select(x => new AgregarIntegranteGrupoViewModel
                {
                    GrupoId = x.HER_GrupoId
                })
                .AsNoTracking()
                .AsQueryable();

            return await grupoQuery.FirstOrDefaultAsync();
        }

        public async Task<bool> AgregarIntegranteGrupoAsync(string username, int grupoId)
        {
            int result = 0;
            int integranteId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(username);

            //Nuevo integrante
            var nuevoIntegrante = new HER_GrupoIntegrante()
            {
                HER_IntegranteId = integranteId,
                HER_GrupoId = grupoId
            };

            //Guardar el integrante
            _context.HER_GrupoIntegrante.Add(nuevoIntegrante);
            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }


        public async Task<BorrarIntegranteGrupoViewModel> ObtenerIntegranteParaBorrar(string username, int grupoId)
        {
            var grupoQuery = _context.HER_GrupoIntegrante
                .Where(x => x.HER_GrupoId == grupoId
                         && x.HER_Integrante.HER_UserName == username
                         && x.HER_Integrante.HER_Activo == true)
                .Select(x => new BorrarIntegranteGrupoViewModel
                {
                    GrupoId = x.HER_GrupoId,
                    UsuarioId = x.HER_Integrante.HER_InfoUsuarioId,
                    Usuario = x.HER_Integrante.HER_UserName,
                    Nombre = x.HER_Integrante.HER_NombreCompleto,
                    Correo = x.HER_Integrante.HER_Correo
                })
                .AsNoTracking()
                .AsQueryable();

            return await grupoQuery.FirstOrDefaultAsync();
        }

        public async Task<List<BorrarIntegranteGrupoViewModel>> ObtenerIntegrantesInactivoParaBorrar(int grupoId)
        {
            var listadoIntegrantesInactivos = new List<BorrarIntegranteGrupoViewModel>();

            if (grupoId > 0)
            {

                var integrantesInactivosQuery = _context.HER_GrupoIntegrante
                .Where(x => x.HER_GrupoId == grupoId
                         && x.HER_Integrante.HER_Activo == false)
                .Select(x => new BorrarIntegranteGrupoViewModel
                {
                    GrupoId = x.HER_GrupoId,
                    UsuarioId = x.HER_Integrante.HER_InfoUsuarioId,
                    Usuario = x.HER_Integrante.HER_UserName,
                    Nombre = x.HER_Integrante.HER_NombreCompleto,
                    Correo = x.HER_Integrante.HER_Correo
                })
                .AsNoTracking()
                .AsQueryable();

                listadoIntegrantesInactivos = await integrantesInactivosQuery.ToListAsync();
            }

            return listadoIntegrantesInactivos;
        }

        public async Task<bool> BorrarIntegranteAsync(string username, BorrarIntegranteGrupoViewModel model)
        {
            int result = 0;
            //--
            var integrantesQuery = _context.HER_GrupoIntegrante 
                .Where(x => x.HER_Integrante.HER_UserName == username
                         && x.HER_IntegranteId == model.UsuarioId
                         && x.HER_GrupoId == model.GrupoId)
                .AsQueryable();

            var integrante = await integrantesQuery.FirstOrDefaultAsync();

            if (integrante != null)
            {
                _context.HER_GrupoIntegrante.Remove(integrante).State = EntityState.Deleted;
                result = await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }

        public async Task<bool> BorrarIntegranteInactivoAsync(string username, BorrarIntegranteGrupoViewModel model)
        {
            int result = 0;
            //--
            var integrantesQuery = _context.HER_GrupoIntegrante
                .Where(x => x.HER_Integrante.HER_UserName == username
                         && x.HER_Integrante.HER_Activo == false   
                         && x.HER_IntegranteId == model.UsuarioId
                         && x.HER_GrupoId == model.GrupoId)
                .AsQueryable();

            var integrante = await integrantesQuery.FirstOrDefaultAsync();

            if (integrante != null)
            {
                _context.HER_GrupoIntegrante.Remove(integrante).State = EntityState.Deleted;
                result = await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }

        public async Task<BorrarIntegranteGrupoViewModel> ObtenerIntegranteInactivoParaBorrar(string username, int grupoId)
        {
            var grupoQuery = _context.HER_GrupoIntegrante
                           .Where(x => x.HER_GrupoId == grupoId
                                    && x.HER_Integrante.HER_UserName == username
                                    && x.HER_Integrante.HER_Activo == true)
                           .Select(x => new BorrarIntegranteGrupoViewModel
                           {
                               GrupoId = x.HER_GrupoId,
                               UsuarioId = x.HER_Integrante.HER_InfoUsuarioId,
                               Usuario = x.HER_Integrante.HER_UserName,
                               Nombre = x.HER_Integrante.HER_NombreCompleto,
                               Correo = x.HER_Integrante.HER_Correo
                           })
                           .AsNoTracking()
                           .AsQueryable();

            return await grupoQuery.FirstOrDefaultAsync();
        }
    }
}
