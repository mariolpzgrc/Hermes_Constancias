using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Documento;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Services
{
    public class EstadisticasService : IEstadisticasService
    {
        private readonly ApplicationDbContext _context;
        private readonly CultureInfo _cultureEs;

        public EstadisticasService(ApplicationDbContext context)
        {
            _context = context;
            _cultureEs = new CultureInfo("es-MX");
        }

        //Estadisticas del usuario
        public async Task<EstadisticasRecibidosViewModel> ObtenerEstadisticasDocumentosRecibidosPorEstadoAsync(string username, string categoriaId, string fechaInicio, string fechaFin )
        {
            IQueryable<HER_Recepcion> recibidosQuery;
            IQueryable<HER_Recepcion> respuestasQuery;

            DateTime fechaInicioTemp;
            DateTime fechaFinTemp;

            if (!string.IsNullOrEmpty(categoriaId) && string.IsNullOrEmpty(fechaInicio) && string.IsNullOrEmpty(fechaFin))
            {
                recibidosQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                         && ConstTipoEnvio.TiposEnvios.Contains(x.HER_Envio.HER_TipoEnvioId)   
                       )
                .AsNoTracking()
                .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                         && ConstTipoEnvio.TiposRespuestas.Contains(x.HER_Envio.HER_TipoEnvioId)
                      )   
                .AsNoTracking()
                .AsQueryable();

            }
            else if (!string.IsNullOrEmpty(categoriaId) && !string.IsNullOrEmpty(fechaInicio) && string.IsNullOrEmpty(fechaFin))
            {
                fechaInicioTemp = Convert.ToDateTime(fechaInicio, _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0);

                recibidosQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                         && x.HER_FechaRecepcion >= fechaInicioTemp
                         && ConstTipoEnvio.TiposEnvios.Contains(x.HER_Envio.HER_TipoEnvioId)
                         )
                .AsNoTracking()
                .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                         && x.HER_FechaRecepcion >= fechaInicioTemp
                         && ConstTipoEnvio.TiposRespuestas.Contains(x.HER_Envio.HER_TipoEnvioId)
                         )
                .AsNoTracking()
                .AsQueryable();

            }
            else if (!string.IsNullOrEmpty(categoriaId) && string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
            {
                fechaFinTemp = Convert.ToDateTime(fechaFin, _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59);

                recibidosQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                         && x.HER_FechaRecepcion <= fechaFinTemp
                         && ConstTipoEnvio.TiposEnvios.Contains(x.HER_Envio.HER_TipoEnvioId)
                         )
                .AsNoTracking()
                .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                          && x.HER_Para.HER_Activo == true
                          && x.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                          && x.HER_FechaRecepcion <= fechaFinTemp
                          && ConstTipoEnvio.TiposRespuestas.Contains(x.HER_Envio.HER_TipoEnvioId)
                          )
                .AsNoTracking()
                .AsQueryable();
            }
            else if (string.IsNullOrEmpty(categoriaId) && !string.IsNullOrEmpty(fechaInicio) && string.IsNullOrEmpty(fechaFin))
            {
                fechaInicioTemp = Convert.ToDateTime(fechaInicio, _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0);

                recibidosQuery = _context.HER_Recepcion
               .Where(x => x.HER_Para.HER_UserName == username
                        && x.HER_Para.HER_Activo == true
                        && x.HER_FechaRecepcion >= fechaInicioTemp
                        && ConstTipoEnvio.TiposEnvios.Contains(x.HER_Envio.HER_TipoEnvioId)
                        )
               .AsNoTracking()
               .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_FechaRecepcion >= fechaInicioTemp
                         && ConstTipoEnvio.TiposRespuestas.Contains(x.HER_Envio.HER_TipoEnvioId)
                         )
                .AsNoTracking()
                .AsQueryable();
            }
            else if (string.IsNullOrEmpty(categoriaId) && string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
            {
                fechaFinTemp = Convert.ToDateTime(fechaFin, _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59);

                recibidosQuery = _context.HER_Recepcion
               .Where(x => x.HER_Para.HER_UserName == username
                        && x.HER_Para.HER_Activo == true
                        && x.HER_FechaRecepcion <= fechaFinTemp
                        && ConstTipoEnvio.TiposEnvios.Contains(x.HER_Envio.HER_TipoEnvioId)
                        )
               .AsNoTracking()
               .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_FechaRecepcion <= fechaFinTemp
                         && ConstTipoEnvio.TiposRespuestas.Contains(x.HER_Envio.HER_TipoEnvioId)
                         )
                .AsNoTracking()
                .AsQueryable();
            }
            else if (string.IsNullOrEmpty(categoriaId) && !string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
            {
                fechaInicioTemp = Convert.ToDateTime(fechaInicio, _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0);
                fechaFinTemp = Convert.ToDateTime(fechaFin, _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59);

                recibidosQuery = _context.HER_Recepcion
               .Where(x => x.HER_Para.HER_UserName == username
                        && x.HER_Para.HER_Activo == true
                        && (x.HER_FechaRecepcion >= fechaInicioTemp && x.HER_FechaRecepcion <= fechaFinTemp)
                        && ConstTipoEnvio.TiposEnvios.Contains(x.HER_Envio.HER_TipoEnvioId)
                        )
               .AsNoTracking()
               .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && (x.HER_FechaRecepcion >= fechaInicioTemp && x.HER_FechaRecepcion <= fechaFinTemp)
                         && ConstTipoEnvio.TiposRespuestas.Contains(x.HER_Envio.HER_TipoEnvioId)
                         )
                .AsNoTracking()
                .AsQueryable();
            }
            else if (!string.IsNullOrEmpty(categoriaId) && !string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
            {
                fechaInicioTemp = Convert.ToDateTime(fechaInicio, _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0);
                fechaFinTemp = Convert.ToDateTime(fechaFin, _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59);

                recibidosQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                         && (x.HER_FechaRecepcion >= fechaInicioTemp && x.HER_FechaRecepcion <= fechaFinTemp)
                         && ConstTipoEnvio.TiposEnvios.Contains(x.HER_Envio.HER_TipoEnvioId)
                         )
                .AsNoTracking()
                .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                         && (x.HER_FechaRecepcion >= fechaInicioTemp && x.HER_FechaRecepcion <= fechaFinTemp)
                         && ConstTipoEnvio.TiposRespuestas.Contains(x.HER_Envio.HER_TipoEnvioId)
                         )
                .AsNoTracking()
                .AsQueryable();
            }
            else
            {
                recibidosQuery = _context.HER_Recepcion
               .Where(x => x.HER_Para.HER_UserName == username
                        && x.HER_Para.HER_Activo == true
                        && ConstTipoEnvio.TiposEnvios.Contains(x.HER_Envio.HER_TipoEnvioId)
                        )
               .AsNoTracking()
               .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && ConstTipoEnvio.TiposRespuestas.Contains(x.HER_Envio.HER_TipoEnvioId)
                         )
                .AsNoTracking()
                .AsQueryable();
            }

            //Estados (semaforos)
            var estadosQuery = _context.HER_EstadoEnvio
                .Where(x => ConstEstadoEnvio.EstadoBandejaRecibidos.Contains(x.HER_Nombre))
                .AsNoTracking()
                .AsQueryable();

            var estados = await estadosQuery.ToListAsync();

            var estadisticas = new EstadisticasRecibidosViewModel
            {
                Respuestas = respuestasQuery.Count(),
                Recibidos = estados.GroupBy(x => new { x.HER_EstadoEnvioId, x.HER_Nombre })
                .Select(y => new EstadisticasRecibidosPorEstadoViewModel
                {
                    EstadoId = y.Key.HER_EstadoEnvioId,
                    Estado = y.Key.HER_Nombre,
                    Total = recibidosQuery.Count(z => z.HER_EstadoEnvioId == y.Key.HER_EstadoEnvioId)
                }).ToList()
            };

            return estadisticas;
        }
        public async Task<EstadisticasEnviadosViewModel> ObtenerEstadisticasDocumentosEnviadosPorEstadoAsync(string username, string categoriaId, string fechaInicio, string fechaFin)
        {
            IQueryable<HER_Envio> enviadosQuery;
            IQueryable<HER_Envio> respuestasQuery;

            DateTime fechaInicioTemp;
            DateTime fechaFinTemp;

            if (!string.IsNullOrEmpty(categoriaId) && string.IsNullOrEmpty(fechaInicio) && string.IsNullOrEmpty(fechaFin))
            {
                enviadosQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && a.HER_Documento.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                             && ConstTipoEnvio.TiposEnvios.Contains(a.HER_TipoEnvioId)
                             )
                    .AsNoTracking()
                    .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && a.HER_Documento.HER_Categorias.Any(z => z.HER_CategoriaId == int.Parse(categoriaId))
                             && ConstTipoEnvio.TiposRespuestas.Contains(a.HER_TipoEnvioId)
                          )
                    .AsNoTracking()
                    .AsQueryable();

            } 
            else if (!string.IsNullOrEmpty(categoriaId) && !string.IsNullOrEmpty(fechaInicio) && string.IsNullOrEmpty(fechaFin))
            {
                fechaInicioTemp = Convert.ToDateTime(fechaInicio, _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0);

                enviadosQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && a.HER_Documento.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                             && a.HER_FechaEnvio >= fechaInicioTemp
                             && ConstTipoEnvio.TiposEnvios.Contains(a.HER_TipoEnvioId)
                             )
                    .AsNoTracking()
                    .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && a.HER_Documento.HER_Categorias.Any(z => z.HER_CategoriaId == int.Parse(categoriaId))
                             && a.HER_FechaEnvio >= fechaInicioTemp
                             && ConstTipoEnvio.TiposRespuestas.Contains(a.HER_TipoEnvioId)
                             )
                    .AsNoTracking()
                    .AsQueryable();

            }
            else if (!string.IsNullOrEmpty(categoriaId) && string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
            {
                fechaFinTemp = Convert.ToDateTime(fechaFin, _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59);

                enviadosQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && a.HER_Documento.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                             && a.HER_FechaEnvio <= fechaFinTemp
                             && ConstTipoEnvio.TiposEnvios.Contains(a.HER_TipoEnvioId)
                             )
                    .AsNoTracking()
                    .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && a.HER_Documento.HER_Categorias.Any(z => z.HER_CategoriaId == int.Parse(categoriaId))
                             && a.HER_FechaEnvio <= fechaFinTemp
                             && ConstTipoEnvio.TiposRespuestas.Contains(a.HER_TipoEnvioId)
                             )
                    .AsNoTracking()
                    .AsQueryable();
            }
            else if (string.IsNullOrEmpty(categoriaId) && !string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
            {
                fechaInicioTemp = Convert.ToDateTime(fechaInicio, _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0);
                fechaFinTemp = Convert.ToDateTime(fechaFin, _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59);

                enviadosQuery = _context.HER_Envio
                   .Where(a => a.HER_De.HER_UserName == username
                            && a.HER_De.HER_Activo == true
                            && (a.HER_FechaEnvio >= fechaInicioTemp && a.HER_FechaEnvio <= fechaFinTemp)
                            && ConstTipoEnvio.TiposEnvios.Contains(a.HER_TipoEnvioId)
                            )
                   .AsNoTracking()
                   .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && (a.HER_FechaEnvio >= fechaInicioTemp && a.HER_FechaEnvio <= fechaFinTemp)
                             && ConstTipoEnvio.TiposRespuestas.Contains(a.HER_TipoEnvioId)
                             )
                    .AsNoTracking()
                    .AsQueryable();
            }
            else if (string.IsNullOrEmpty(categoriaId) && !string.IsNullOrEmpty(fechaInicio) && string.IsNullOrEmpty(fechaFin))
            {
                fechaInicioTemp = Convert.ToDateTime(fechaInicio, _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0);

                enviadosQuery = _context.HER_Envio
                   .Where(a => a.HER_De.HER_UserName == username
                            && a.HER_De.HER_Activo == true
                            && a.HER_FechaEnvio >= fechaInicioTemp
                            && ConstTipoEnvio.TiposEnvios.Contains(a.HER_TipoEnvioId)
                            )
                   .AsNoTracking()
                   .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && a.HER_FechaEnvio >= fechaInicioTemp
                             && ConstTipoEnvio.TiposRespuestas.Contains(a.HER_TipoEnvioId)
                             )
                    .AsNoTracking()
                    .AsQueryable();
            }
            else if (string.IsNullOrEmpty(categoriaId) && string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
            {
                fechaFinTemp = Convert.ToDateTime(fechaFin, _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59);

                enviadosQuery = _context.HER_Envio
                   .Where(a => a.HER_De.HER_UserName == username
                            && a.HER_De.HER_Activo == true
                            && a.HER_FechaEnvio <= fechaFinTemp
                            && ConstTipoEnvio.TiposEnvios.Contains(a.HER_TipoEnvioId)
                            )
                   .AsNoTracking()
                   .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && a.HER_FechaEnvio <= fechaFinTemp
                             && ConstTipoEnvio.TiposRespuestas.Contains(a.HER_TipoEnvioId)
                             )
                    .AsNoTracking()
                    .AsQueryable();
            }
            else if (!string.IsNullOrEmpty(categoriaId) && !string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
            {
                fechaInicioTemp = Convert.ToDateTime(fechaInicio, _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0);
                fechaFinTemp = Convert.ToDateTime(fechaFin, _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59);

                enviadosQuery = _context.HER_Envio
                   .Where(a => a.HER_De.HER_UserName == username
                            && a.HER_De.HER_Activo == true
                            && a.HER_Documento.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                            && (a.HER_FechaEnvio >= fechaInicioTemp && a.HER_FechaEnvio <= fechaFinTemp)
                            && ConstTipoEnvio.TiposEnvios.Contains(a.HER_TipoEnvioId)
                            )
                   .AsNoTracking()
                   .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Envio
                   .Where(a => a.HER_De.HER_UserName == username
                            && a.HER_De.HER_Activo == true
                            && a.HER_Documento.HER_Categorias.Any(y => y.HER_CategoriaId == int.Parse(categoriaId))
                            && (a.HER_FechaEnvio >= fechaInicioTemp && a.HER_FechaEnvio <= fechaFinTemp)
                            && ConstTipoEnvio.TiposRespuestas.Contains(a.HER_TipoEnvioId)
                            )
                   .AsNoTracking()
                   .AsQueryable();
            }
            else
            {
                enviadosQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && ConstTipoEnvio.TiposEnvios.Contains(a.HER_TipoEnvioId))
                    .AsNoTracking()
                    .AsQueryable();

                //Respuestas
                respuestasQuery = _context.HER_Envio
                    .Where(a => a.HER_De.HER_UserName == username
                             && a.HER_De.HER_Activo == true
                             && ConstTipoEnvio.TiposRespuestas.Contains(a.HER_TipoEnvioId))
                    .AsNoTracking()
                    .AsQueryable();
            }

            //Estados (semaforos)
            var estadosQuery = _context.HER_EstadoEnvio
                .Where(x => ConstEstadoEnvio.EstadoBandejaEnviados.Contains(x.HER_Nombre))
                .AsNoTracking()
                .AsQueryable();

            var estados = await estadosQuery.ToListAsync();

            ////Busqueda
            //var info = estados.GroupBy(x => new { x.HER_EstadoEnvioId, x.HER_Nombre })
            //    .Select(y => new EstadisticasEnviadosPorEstadoViewModel
            //    {
            //        EstadoId = y.Key.HER_EstadoEnvioId,
            //        Estado = y.Key.HER_Nombre,
            //        Total = enviadosQuery.Count(z => z.HER_EstadoEnvioId == y.Key.HER_EstadoEnvioId)
            //    }).ToList();

            var estadisticas = new EstadisticasEnviadosViewModel
            {
                Respuestas = respuestasQuery.Count(),
                Enviados = estados.GroupBy(x => new { x.HER_EstadoEnvioId, x.HER_Nombre })
                    .Select(y => new EstadisticasEnviadosPorEstadoViewModel
                    {
                        EstadoId = y.Key.HER_EstadoEnvioId,
                        Estado = y.Key.HER_Nombre,
                        Total = enviadosQuery.Count(z => z.HER_EstadoEnvioId == y.Key.HER_EstadoEnvioId)
                    }).ToList()
            };

            return estadisticas;
        }
    }
}
