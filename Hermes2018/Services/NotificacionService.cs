using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hermes2018.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly ApplicationDbContext _context;

        public NotificacionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificacionUsuariosViewModel>> UsuariosAsync(int areaId)
        {
            var usuariosQuery= _context.HER_InfoUsuario
                        .Where(x => x.HER_AreaId == areaId 
                                 && x.HER_Activo == false 
                                 && x.HER_Usuario.HER_Aprobado == false)
                        .AsNoTracking()
                        .OrderBy(x => x.HER_FechaRegistro)
                        .Take(5)
                        .Select(x => new NotificacionUsuariosViewModel
                        {
                            FullName = x.HER_NombreCompleto,
                            UserName = x.HER_UserName,
                            Email = x.HER_Correo,
                            FechaRegistro = x.HER_FechaRegistro
                        })
                        .AsNoTracking()
                        .AsQueryable();
            
            return await usuariosQuery.ToListAsync();
        }

        public async Task<List<NotificacionDocumentosViewModel>> DocumentosAsync(string username)
        {
            //Busqueda 
            var recibidosQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_Envio.HER_EsOculto == false
                         && x.HER_CarpetaId == null
                         && x.HER_EstaLeido == false) //No esta clasificada en alguna carpeta
                .AsNoTracking()
                .Select(x => new NotificacionDocumentosViewModel
                {
                    EnvioId = x.HER_EnvioId,
                    Fecha = x.HER_FechaRecepcion,
                    TipoEnvio = x.HER_Envio.HER_TipoEnvioId,
                    Asunto = (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2) ? string.Format("{0} {1}", "", x.HER_Envio.HER_Documento.HER_Asunto) : x.HER_Envio.HER_Documento.HER_Asunto,
                    Remitente = x.HER_Envio.HER_De.HER_NombreCompleto
                })
                .OrderByDescending(x => x.Fecha)
                .AsQueryable();

            return await recibidosQuery.Take(5).ToListAsync();
        }

        public async Task<List<NotificacionProximosVencerViewModel>> DocumentosProximosVencerAsync(string username)
        {
            var porcentaje = ConstProximosVencer.Porcentaje;

            //Busqueda 
            //todos los documentos que estan en bandejas principales los id's de carpetas estan en null
            //todos los docmentos que tengan numero en el carpetaid estan asociadas a dicha carpeta
            //la ultima condicion 
            var recibidosQuery = _context.HER_Recepcion
                .Where(x => x.HER_Para.HER_UserName == username
                         && x.HER_Para.HER_Activo == true
                         && x.HER_CarpetaId == null
                         //--
                         && (x.HER_Compromiso.Count() > 0) ?
                             (
                                 DateTime.Now >=
                                      (
                                        (from co in x.HER_Compromiso where co.HER_Estado == ConstCompromiso.EstadoN1 select co.HER_Fecha).FirstOrDefault()
                                            .AddDays(-(Math.Round(((from co in x.HER_Compromiso where co.HER_Estado == ConstCompromiso.EstadoN1 select co.HER_Fecha).FirstOrDefault() - x.HER_FechaRecepcion).Days * porcentaje)))
                                      )
                                 && DateTime.Now <=
                                      (from co in x.HER_Compromiso where co.HER_Estado == ConstCompromiso.EstadoN1 select co.HER_Fecha).FirstOrDefault()
                                 && (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 || x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2)
                                 && x.HER_EstadoEnvioId == ConstEstadoEnvio.EstadoEnvioN1
                             )
                             : false
                   //---
                   )
                .Select(x => new NotificacionProximosVencerViewModel
                {
                    EnvioId = x.HER_EnvioId,
                    Fecha = x.HER_FechaRecepcion,
                    Compromiso = (from comp in x.HER_Compromiso where comp.HER_Estado == ConstCompromiso.EstadoN1 select comp.HER_Fecha).FirstOrDefault(),
                    TipoEnvio = x.HER_Envio.HER_TipoEnvioId,
                    Asunto = (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2) ? string.Format("{0} {1}", "", x.HER_Envio.HER_Documento.HER_Asunto) : x.HER_Envio.HER_Documento.HER_Asunto,
                    Remitente = x.HER_Envio.HER_De.HER_NombreCompleto
                })
                .OrderByDescending(x => x.Fecha)
                .AsQueryable();

            return await recibidosQuery.Take(10).ToListAsync();
        }
    }
}
