using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Anexo;
using Hermes2018.Models.Configuracion;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hermes2018.Services
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly ApplicationDbContext _context;
        private readonly CultureInfo _cultureEs;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<ConfiguracionService> _logger;

        public ConfiguracionService(ApplicationDbContext context,
            ILogger<ConfiguracionService> logger,
            IHostingEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
            _cultureEs = new CultureInfo("es-MX");
        }
        //---
        public async Task<InfoConfiguracionLDAPViewModel> ObtenerInfoConfiguracionLDAPAsync()
        {
            var configuracionQuery = _context.HER_Configuracion
                .Where(x => x.HER_Propiedad == ConstConfiguracionGeneral.Propiedad8)
                .Select(x => new InfoConfiguracionLDAPViewModel()
                {
                    HER_IPLDAP = x.HER_Valor
                })
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<InfoConfiguracionEmailViewModel> ObtenerInfoConfiguracionEmailAsync()
        {
            var propiedades = new InfoConfiguracionEmailViewModel();

            var configuracionQuery = _context.HER_Configuracion
                .Where(x => ConstConfiguracionGeneral.PropiedadesCorreo.Contains(x.HER_Propiedad))
                .AsNoTracking()
                .AsQueryable();

            var configuraciones = await configuracionQuery.ToListAsync();

            foreach (var conf in configuraciones)
            {
                switch (conf.HER_Propiedad)
                {
                    case ConstConfiguracionGeneral.Propiedad9:
                        propiedades.HER_ServidorSMTP = conf.HER_Valor;
                        break;
                    case ConstConfiguracionGeneral.Propiedad10:
                        propiedades.HER_PuertoServidorSMTP = Int32.Parse(conf.HER_Valor);
                        break;
                    case ConstConfiguracionGeneral.Propiedad11:
                        propiedades.HER_CorreoNotificador = conf.HER_Valor;
                        break;
                    case ConstConfiguracionGeneral.Propiedad12:
                        propiedades.HER_ContraseniaCorreoNotificador = conf.HER_Valor;
                        break;
                }
            }

            return propiedades;
        }
        public InfoConfiguracionEmailViewModel ObtenerInfoConfiguracionEmail()
        {
            var propiedades = new InfoConfiguracionEmailViewModel();

            var configuracionQuery = _context.HER_Configuracion
                .Where(x => ConstConfiguracionGeneral.PropiedadesCorreo.Contains(x.HER_Propiedad))
                .AsNoTracking()
                .AsQueryable();

            var configuraciones = configuracionQuery.ToList();

            foreach (var conf in configuraciones)
            {
                switch (conf.HER_Propiedad)
                {
                    case ConstConfiguracionGeneral.Propiedad9:
                        propiedades.HER_ServidorSMTP = conf.HER_Valor;
                        break;
                    case ConstConfiguracionGeneral.Propiedad10:
                        propiedades.HER_PuertoServidorSMTP = Int32.Parse(conf.HER_Valor);
                        break;
                    case ConstConfiguracionGeneral.Propiedad11:
                        propiedades.HER_CorreoNotificador = conf.HER_Valor;
                        break;
                    case ConstConfiguracionGeneral.Propiedad12:
                        propiedades.HER_ContraseniaCorreoNotificador = conf.HER_Valor;
                        break;
                }
            }

            return propiedades;
        }
        public async Task<InfoConfiguracionAvisoPrivacidadViewModel> ObtenerInfoConfiguracionAvisoPrivacidadAsync()
        {
            var configuracionQuery = _context.HER_Configuracion
                .Where(x => x.HER_Propiedad == ConstConfiguracionGeneral.Propiedad4)
                .Select(x => new InfoConfiguracionAvisoPrivacidadViewModel()
                {
                    HER_Aviso = x.HER_Valor,
                })
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        //------
        public async Task<ConfiguracionUsuarioViewModel> ObtenerConfiguracionUsuarioAsync(string username)
        {
            var usuarioQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Activo == true
                         && x.HER_EstaEnReasignacion == false
                         && x.HER_UserName == username)
                .Select(x => new { x.HER_Puesto, x.HER_Direccion, x.HER_Telefono })
                .AsNoTracking()
                .AsQueryable();

            var usuario = await usuarioQuery.FirstOrDefaultAsync();

            var configuracionQuery = _context.HER_ConfiguracionUsuario
                //.Include(x => x.HER_Color)
                .Where(x => x.HER_Usuario.UserName == username)
                .AsNoTracking()
                .Select(x => new ConfiguracionUsuarioViewModel()
                {
                    ConfiguracionUsuarioId = x.HER_ConfiguracionUsuarioId,
                    NotificacionesActivas = (x.HER_Notificaciones)? "Si" : "No",
                    ElementosPorPagina = x.HER_ElementosPorPagina,
                    //Color = x.HER_Color,
                    //--
                    Direccion = usuario.HER_Direccion,
                    Telefono = usuario.HER_Telefono,
                    Puesto = usuario.HER_Puesto
                })
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ObtenerEstadoNotificacionUsuarioAsync(string username)
        {
            var configuracionQuery = _context.HER_ConfiguracionUsuario
                .Where(x => x.HER_Usuario.UserName == username)
                .AsNoTracking()
                .Select(x => x.HER_Notificaciones)
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<int> ObtenerElementosPorPaginaUsuarioAsync(string username)
        {
            var configuracionQuery = _context.HER_ConfiguracionUsuario
                .Where(x => x.HER_Usuario.UserName == username)
                .AsNoTracking()
                .Select(x => x.HER_ElementosPorPagina)
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<int> ObtenerElementosPorPaginaPorUsuarioIdAsync(int infoUsuarioId)
        {
            var configuracionQuery = _context.HER_InfoUsuario
                //.Include(x => x.HER_Usuario)
                //    .ThenInclude(x => x.HER_Configuracion)
                .Where(x => x.HER_InfoUsuarioId == infoUsuarioId)
                .AsNoTracking()
                .Select(x => x.HER_Usuario.HER_Configuracion.HER_ElementosPorPagina)
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<EditarConfiguracionViewModel> ObtenerConfiguracionUsuarioEditar(int configuracionUsuarioId, string username)
        {
            var usuarioQuery = _context.HER_InfoUsuario
               .Where(x => x.HER_Activo == true
                        && x.HER_EstaEnReasignacion == false
                        && x.HER_UserName == username)
               .Select(x => new { x.HER_Puesto, x.HER_Direccion, x.HER_Telefono })
               .AsNoTracking()
               .AsQueryable();

            var usuario = await usuarioQuery.FirstOrDefaultAsync();
            //--
            var configuracionQuery = _context.HER_ConfiguracionUsuario
                //.Include(x => x.HER_Color)
                .Where(x => x.HER_ConfiguracionUsuarioId == configuracionUsuarioId 
                        &&  x.HER_Usuario.UserName == username)
                .AsNoTracking()
                .Select(x => new EditarConfiguracionViewModel()
                {
                    ConfiguracionUsuarioId = x.HER_ConfiguracionUsuarioId,
                    NotificacionesActivas = x.HER_Notificaciones,
                    ElementosPorPagina = x.HER_ElementosPorPagina,
                    //ColorId = x.HER_Color.HER_ColorId,
                    //--
                    Puesto = usuario.HER_Puesto,
                    Direccion = usuario.HER_Direccion,
                    Telefono = usuario.HER_Telefono
                })
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ActualizarConfiguracionAsync(EditarConfiguracionViewModel modelo)
        {
            int result = 0;

            //Configuración
            var configuracionQuery = _context.HER_ConfiguracionUsuario
                .Include(x => x.HER_Usuario)
                .Where(x => x.HER_ConfiguracionUsuarioId == modelo.ConfiguracionUsuarioId)
                .AsQueryable();

            var configuracion = await configuracionQuery.FirstOrDefaultAsync();

            var usuarioQuery = _context.HER_InfoUsuario
                    .Where(x => x.HER_Activo == true
                        && x.HER_EstaEnReasignacion == false
                        && x.HER_UserName == configuracion.HER_Usuario.UserName)
                    .AsQueryable();

            if (configuracion != null)
            {
                //--
                configuracion.HER_Notificaciones = modelo.NotificacionesActivas;
                configuracion.HER_ElementosPorPagina = modelo.ElementosPorPagina;
                //configuracion.HER_ColorId = modelo.ColorId;
                //--
                _context.HER_ConfiguracionUsuario.Update(configuracion).State = EntityState.Modified;

                //InfoUsuario
                var usuario = await usuarioQuery.FirstOrDefaultAsync();

                if (usuario != null)
                {
                    usuario.HER_Puesto = modelo.Puesto;
                    usuario.HER_Direccion = modelo.Direccion;
                    usuario.HER_Telefono = modelo.Telefono;

                    _context.HER_InfoUsuario.Update(usuario).State = EntityState.Modified;
                }

                result = await _context.SaveChangesAsync();
            }

            return (result > 0) ? true : false;
        }
        //-------
        public async Task<string> ObtenerInstitucionAsync()
        {
            var configuracionQuery = _context.HER_Configuracion
                .Where(x => x.HER_Propiedad == ConstConfiguracionGeneral.Propiedad1)
                .Select(x => x.HER_Valor)
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<AvisoInhabilViewModel> ObtenerAvisoInhabilAsync()
        {
            var modelo = new AvisoInhabilViewModel();

            var configuracionQuery = _context.HER_Configuracion
                .Where(x => ConstConfiguracionGeneral.PropiedadesAviso.Contains(x.HER_Propiedad))
                .AsNoTracking()
                .AsQueryable();

            var configuracion = await configuracionQuery.ToListAsync();
            if (configuracion != null) {
                modelo.Inicio = Convert.ToDateTime(configuracion.Where(x => x.HER_Propiedad == ConstConfiguracionGeneral.Propiedad14).Select(x => x.HER_Valor).FirstOrDefault(), _cultureEs).AddHours(0).AddMinutes(0).AddSeconds(0);
                modelo.Termino = Convert.ToDateTime(configuracion.Where(x => x.HER_Propiedad == ConstConfiguracionGeneral.Propiedad15).Select(x => x.HER_Valor).FirstOrDefault(), _cultureEs).AddHours(23).AddMinutes(59).AddSeconds(59);
                modelo.Contenido = configuracion.Where(x => x.HER_Propiedad == ConstConfiguracionGeneral.Propiedad13).Select(x => x.HER_Valor).FirstOrDefault();
            }

            return modelo;
        }
        public async Task<HER_AnexoGeneral> ObtenerLogoInstitucionAsync()
        {
            var configuracionQuery = _context.HER_AnexoGeneral
                .Where(x => x.HER_TipoRegistro == ConstConfiguracionGeneral.TipoImagenN1)
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<HER_AnexoGeneral> ObtenerImagenPortadaAsync()
        {
            var configuracionQuery = _context.HER_AnexoGeneral
                .Where(x => x.HER_TipoRegistro == ConstConfiguracionGeneral.TipoImagenN2)
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        
        //--
        public string ObtenerPlantillaSolicitudParaTitular()
        {
            var formato = 
                    $"<div style=\"font-family:Segoe UI; font-size: 13px; \">"+
                        "<p style=\"font-weight: bold; margin-bottom:0; \" > Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>"+
                    "</div>"+
                    "<div style=\"font-family:Segoe UI; font-size: 12px; \">"+
                        "<p>Ha recibido una solicitud de incorporación a HERMES, con los siguientes datos: </p>"+
                        "<p style=\"margin: 0; \"><strong>Nombre completo: </strong> {0} </p>"+
                        "<p style=\"margin: 0; \"><strong>Usuario: </strong> {1} </p>"+
                        "<p style=\"margin: 0; \"><strong>Correo: </strong> {2} </p>"+
                        "<p style=\"margin: 0; \"><strong>Región: </strong> {3} </p>"+
                        "<p style=\"margin: 0; \"><strong>Área: </strong> {4} </p>"+
                        "<p style=\"margin: 0; \"><strong>Puesto: </strong> {5} </p>"+
                    "</div>"+
                    "<div style=\"font-family:Segoe UI; font-size: 10px; \">"+
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>"+
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                        "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                    "</div>";

            return formato;
        }
        public string ObtenerPlantillaSolicitudParaSolicitante()
        {
            var formato =
                    $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                        "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                        "<p>Se ha enviado su solicitud al área <strong> {4} </strong>, en cuanto se atienda se le enviará un correo electrónico notificándole la respuesta. </p>" +
                        "<p style=\"margin-top:0; \"><strong>RESUMEN</strong></p>" +
                        "<p style=\"margin: 0; \"><strong>Nombre completo: </strong> {0} </p>" +
                        "<p style=\"margin: 0; \"><strong>Usuario: </strong> {1} </p>" +
                        "<p style=\"margin: 0; \"><strong>Correo: </strong> {2} </p>" +
                        "<p style=\"margin: 0; \"><strong>Región: </strong> {3} </p>" +
                        "<p style=\"margin: 0; \"><strong>Área: </strong> {4} </p>" +
                        "<p style=\"margin: 0; \"><strong>Puesto: </strong> {5} </p>" +
                        "<p>Será atendido por: <strong> {6} </strong>(<a href=\"mailto:{7}\">{7}</a>)</p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                        "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                    "</div>";

            return formato;
        }
        public string ObtenerPlantillaSolicitudAceptada()
        {
            var formato =
                    $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                        "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                        "<p>Ha recibido la respuesta a su solicitud de incorporación a HERMES: </p>" +
                        "<p style=\"margin: 0; \"><strong>Estado: </strong> {0} </p>" +
                        "<p style=\"margin: 0; \"><strong>Fecha: </strong> {1} </p>" +
                        "<br />"+
                        "<p style=\"margin: 0; \"><strong>Región: </strong> {2} </p>"+
                        "<p style=\"margin: 0; \"><strong>Área: </strong> {3} </p>"+
                        "<p style=\"margin: 0; \"><strong>Puesto: </strong> {4} </p>" +
                        "<p style=\"margin: 0; \"><strong>Dirección: </strong> {5} </p>" +
                        "<p style=\"margin: 0; \"><strong>Teléfono: </strong> {6} </p>" +
                        "<p><strong>Fue atendido por: </strong> {7} (<a href=\"mailto: {8} \">{8}</a>)</p>" +
                        "<p style=\"margin-bottom:0; \"> Para acceder al sistema, haga clic en la siguiente liga: <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                        "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                    "</div>";

            return formato;
        }
        public string ObtenerPlantillaSolicitudRechazada()
        {
            var formato =
                    $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                        "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                        "<p>Ha recibido la respuesta a su solicitud de incorporación a HERMES: </p>" +
                        "<p style=\"margin: 0; \"><strong>Estado: </strong> {0} </p>" +
                        "<p style=\"margin: 0; \"><strong>Fecha: </strong> {1} </p>" +
                        "<p style=\"margin: 0; \"><strong>Motivo: </strong> {2} </p>" +
                        "<br />" +
                        "<p style=\"margin: 0; \"><strong>Región: </strong> {3} </p>" +
                        "<p style=\"margin: 0; \"><strong>Área: </strong> {4} </p>" +
                        "<p style=\"margin: 0; \"><strong>Puesto: </strong> {5} </p>" +
                        "<p style=\"margin: 0; \"><strong>Dirección: </strong> {6} </p>" +
                        "<p style=\"margin: 0; \"><strong>Teléfono: </strong> {7} </p>" +
                        "<p><strong>Fue atendido por: </strong> {8} (<a href=\"mailto: {9} \">{9}</a>)</p>" +
                        "<p style=\"margin-bottom:0; \"> Para acceder al sistema, haga clic en la siguiente liga: <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                        "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                    "</div>";

            return formato;
        }
        public string ObtenerPlantillaRegistroUsuario()
        {
            var formato =
                    $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                        "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                        "<p>Ha sido registrado en HERMES, con los siguientes datos: </p>" +
                        "<p style=\"margin: 0; \"><strong>Nombre completo: </strong> {0} </p>" +
                        "<p style=\"margin: 0; \"><strong>Usuario: </strong> {1} </p>" +
                        "<p style=\"margin: 0; \"><strong>Correo: </strong> {2} </p>" +
                        "<p style=\"margin: 0; \"><strong>Dirección: </strong> {3} </p>" +
                        "<p style=\"margin: 0; \"><strong>Teléfono: </strong> {4} </p>" +
                        "<p style=\"margin: 0; \"><strong>Región: </strong> {5} </p>" +
                        "<p style=\"margin: 0; \"><strong>Área: </strong> {6} </p>" +
                        "<p style=\"margin: 0; \"><strong>Puesto: </strong> {7} </p>" +
                        "<p> Para acceder al sistema, haga clic en la siguiente liga: <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                        "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                    "</div>";

            return formato;
        }
        public string ObtenerPlantillaEnvioDocumento()
        {
            var formato =
                    $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                        "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                        "<p>Ha recibido un documento desde la cuenta <strong>{0}</strong> con la siguiente información: </p>" +
                        "<p style=\"margin: 0; \"><strong>No.: </strong> {1} </p>" +
                        "<p style=\"margin: 0; \"><strong>Tipo de documento: </strong> {2} </p>" +
                        "<p style=\"margin: 0; \"><strong>De: </strong> {3} </p>" +
                        "<p style=\"margin: 0; \"><strong>Para: </strong> {4} </p>" +
                        "<p style=\"margin: 0; \"><strong>CCP: </strong> {5} </p>" +
                        "<p style=\"margin: 0; \"><strong>Asunto: </strong> {6} </p>" +
                        "<div style=\"border: 0.05em solid #000; margin-top: 1em; padding: 0.7em;\">" +
                            "<p style=\"margin: 0; \"><strong>{7}</strong></p>" +
                            "<p style=\"margin: 0; \"><strong>{8}</strong></p>" +
                            "<p style=\"margin: 0; \"><strong>{9}</strong></p>" +
                            "<p style=\"margin: 0; \">{10}</p>" +
                            "<div style=\"padding: 1em 1em 1em 0; \">" +
                                "{11}" +
                            "</div>" +
                            "<p style=\"margin: 0; \">A t e n t a m e n t e</p>" +
                            "<p style=\"margin: 0; \">\"Lis de Veracruz: Arte, Ciencia, Luz\"</p>" +
                            "<br />"+
                            "<p style=\"margin: 0; \">{12}</p> " +
                            "<p style=\"margin: 0; \">{13}</p> " +
                            "<p style=\"margin: 0; \">{14}</p>" +
                            "<p style=\"margin: 0; \">Universidad Veracruzana</p>" +
                        "</div>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                        "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                    "</div>";

            return formato;
        }
        public string ObtenerPlantillaTurnarDocumento()
        {
            var formato =
                    $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                        "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                        "<p>Ha recibido un documento desde la cuenta <strong>{0}</strong> con la siguiente información: </p>" +
                        "<p style=\"margin: 0; \"><strong>De: </strong> {1} </p>" +
                        "<p style=\"margin: 0; \"><strong>Para: </strong> {2} </p>" +
                        "<p style=\"margin: 0; \"><strong>CCP: </strong> {3} </p>" +
                        "<p style=\"margin: 0; \"><strong>Mensaje de texto: </strong> {4} </p>" +
                        "<p style=\"margin: 0; \">-----------------------------------------------------------------</p>" +
                        "<p style=\"margin: 0; \"><strong>No.: </strong> {5} </p>" +
                        "<p style=\"margin: 0; \"><strong>Tipo de documento: </strong> {6} </p>" +
                        "<p style=\"margin: 0; \"><strong>De: </strong> {7} </p>" +
                        "<p style=\"margin: 0; \"><strong>Para: </strong> {8} </p>" +
                        "<p style=\"margin: 0; \"><strong>CCP: </strong> {9} </p>" +
                        "<p style=\"margin: 0; \"><strong>Asunto: </strong> {10} </p>" +
                        "<p style=\"margin: 0; \"><strong>Adjuntos: </strong> {11} </p>" +
                        "<div style=\"border: 0.05em solid #000; margin-top: 1em; padding: 0.7em;\">" +
                            "<p style=\"margin: 0; \"><strong>{12}</strong></p>" +
                            "<p style=\"margin: 0; \"><strong>{13}</strong></p>" +
                            "<p style=\"margin: 0; \"><strong>{14}</strong></p>" +
                            "<p style=\"margin: 0; \">{15}</p>" +
                            "<div style=\"padding: 1em 1em 1em 0; \">" +
                                "{16}" +
                            "</div>" +
                            "<p style=\"margin: 0; \">A t e n t a m e n t e</p>" +
                            "<p style=\"margin: 0; \">\"Lis de Veracruz: Arte, Ciencia, Luz\"</p>" +
                            "<br />" +
                            "<p style=\"margin: 0; \">{17}</p> " +
                            "<p style=\"margin: 0; \">{18}</p> " +
                            "<p style=\"margin: 0; \">{19}</p>" +
                            "<p style=\"margin: 0; \">Universidad Veracruzana</p>" +
                        "</div>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                        "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                    "</div>";

            return formato;
        }
        public string ObtenerPlantillaResponderDocumento()
        {
            var formato =
                    $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                        "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                        "<p>Ha recibido un documento desde la cuenta <strong>{0}</strong> con la siguiente información: </p>" +
                        "<p style=\"margin: 0; \"><strong>No.: </strong> {1} </p>" +
                        "<p style=\"margin: 0; \"><strong>Tipo de documento: </strong> {2} </p>" +
                        "<p style=\"margin: 0; \"><strong>De: </strong> {3} </p>" +
                        "<p style=\"margin: 0; \"><strong>Para: </strong> {4} </p>" +
                        "<p style=\"margin: 0; \"><strong>CCP: </strong> {5} </p>" +
                        "<p style=\"margin: 0; \"><strong>Asunto: </strong> {6} </p>" +
                        "<div style=\"border: 0.05em solid #000; margin-top: 1em; padding: 0.7em;\">" +
                            "<p style=\"margin: 0; \"><strong>{7}</strong></p>" +
                            "<p style=\"margin: 0; \"><strong>{8}</strong></p>" +
                            "<p style=\"margin: 0; \"><strong>{9}</strong></p>" +
                            "<p style=\"margin: 0; \">{10}</p>" +
                            "<div style=\"padding: 1em 1em 1em 0; \">" +
                                "{11}" +
                            "</div>" +
                            "<p style=\"margin: 0; \">A t e n t a m e n t e</p>" +
                            "<p style=\"margin: 0; \">\"Lis de Veracruz: Arte, Ciencia, Luz\"</p>" +
                            "<br />" +
                            "<p style=\"margin: 0; \">{12}</p> " +
                            "<p style=\"margin: 0; \">{13}</p> " +
                            "<p style=\"margin: 0; \">{14}</p>" +
                            "<p style=\"margin: 0; \">Universidad Veracruzana</p>" +
                        "</div>" +
                    "</div>" +
                    "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                        "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                        "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                    "</div>";

            return formato;
        }
        //-----
        public async Task<DetalleConfiguracion> ObtenerDetalleConfiguracionGeneralAsync()
        {
            var detalle = new DetalleConfiguracion();

            var identidadQuery = _context.HER_Configuracion
                .Where(x => ConstConfiguracionGeneral.PropiedadesIdentidad.Contains(x.HER_Propiedad))
                .Select(x => new ListadoConfiguracionGeneral() {
                    ConfiguracionId = x.HER_ConfiguracionId,
                    Propiedad = x.HER_Propiedad,
                    Valor = x.HER_Valor
                })
                .AsNoTracking()
                .AsQueryable();
            //--
            detalle.Identidad = await identidadQuery.OrderByDescending(x => x.Propiedad).ToListAsync();

            var accesoQuery = _context.HER_Configuracion
                .Where(x => ConstConfiguracionGeneral.PropiedadesAcceso.Contains(x.HER_Propiedad))
                .Select(x => new ListadoConfiguracionGeneral()
                {
                    ConfiguracionId = x.HER_ConfiguracionId,
                    Propiedad = x.HER_Propiedad,
                    Valor = x.HER_Valor
                })
                .AsNoTracking()
                .AsQueryable();
            //--
            detalle.Acceso = await accesoQuery.ToListAsync();

            var generalQuery = _context.HER_Configuracion
                .Where(x => ConstConfiguracionGeneral.PropiedadesGeneral.Contains(x.HER_Propiedad))
                .Select(x => new ListadoConfiguracionGeneral()
                {
                    ConfiguracionId = x.HER_ConfiguracionId,
                    Propiedad = x.HER_Propiedad,
                    Valor = x.HER_Valor
                })
                .AsNoTracking()
                .AsQueryable();
            //--
            detalle.General = await generalQuery.ToListAsync();

            var logoQuery = _context.HER_AnexoGeneral
                .Where(x => x.HER_TipoRegistro == ConstConfiguracionGeneral.TipoImagenN1)
                .Select(x => new DetalleImagenes()
                {
                    ImagenId = x.HER_AnexoGeneralId,
                    ImagenNombre = x.HER_Nombre
                })
                .AsNoTracking()
                .AsQueryable();
            //--
            detalle.Logo = await logoQuery.FirstOrDefaultAsync();

            var portadaQuery = _context.HER_AnexoGeneral
                .Where(x => x.HER_TipoRegistro == ConstConfiguracionGeneral.TipoImagenN2)
                .Select(x => new DetalleImagenes()
                {
                    ImagenId = (int)x.HER_AnexoGeneralId,
                    ImagenNombre = x.HER_Nombre
                })
                .AsNoTracking()
                .AsQueryable();
            //--
            detalle.Portada = await portadaQuery.FirstOrDefaultAsync();

            var extensionesQuery = _context.HER_Extension
               .Select(x => x.HER_Nombre)
               .AsNoTracking()
               .AsQueryable();
            //--
            detalle.Extensiones = String.Join(", ", await extensionesQuery.ToArrayAsync());
            //--
            var rutasBaseQuery = _context.HER_AnexoRuta
                .Select(x => new AnexoRutaViewModel {
                    RutaBaseId = x.HER_AnexoRutaId,
                    RutaBase = x.HER_RutaBase,
                    Estado = x.HER_Estado == ConstAnexoRuta.EstadoN1 ? ConstAnexoRuta.EstadoT1: ConstAnexoRuta.EstadoT2,
                    FechaActualizacion = x.HER_FechaActualizacion.ToString("dd/MM/yyyy", _cultureEs),
                    FechaRegistro = x.HER_FechaRegistro.ToString("dd/MM/yyyy", _cultureEs),
                    TotalArchivos = x.HER_AnexoArchivos.Count()
                })
                .OrderBy(x => x.Estado)
                .AsNoTracking()
                .AsQueryable();

            detalle.RutasBase = await rutasBaseQuery.ToListAsync();

            return detalle;
        }
        public async Task<EditarConfiguracionGeneral> ObtenerConfiguracionTextoParaEditarAsync(int configId)
        {
            var configuracionQuery = _context.HER_Configuracion
                .Where(x => x.HER_ConfiguracionId == configId)
                .Select(x => new EditarConfiguracionGeneral()
                {
                    ConfiguracionId = x.HER_ConfiguracionId,
                    PropiedadClave = x.HER_Propiedad,
                    Propiedad = x.HER_Propiedad,
                    Valor = x.HER_Valor
                })
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> ExistePropiedadConfiguracionAsync(string propiedad)
        {
            var configuracionQuery = _context.HER_Configuracion
                .Where(x => x.HER_Propiedad == propiedad)
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.AnyAsync();
        }
        public async Task<bool> ActualizarPropiedadConfiguracionAsync(EditarConfiguracionGeneral modelo )
        {
            int result = 0;

            var configuracionQuery = _context.HER_Configuracion
                .Where(x => x.HER_ConfiguracionId == modelo.ConfiguracionId)
                .AsNoTracking()
                .AsQueryable();

            var configuracion = await configuracionQuery.FirstOrDefaultAsync();
            configuracion.HER_Valor = modelo.Valor;

            _context.HER_Configuracion.Update(configuracion).State = EntityState.Modified;
            result = await _context.SaveChangesAsync();

            return (result > 0) ? true : false;
        }
        public async Task<bool> ExisteLogoPropiedadConfiguracionAsync(string archivo)
        {
            var configuracionQuery = _context.HER_AnexoGeneral
                .Where(x => x.HER_TipoRegistro == ConstConfiguracionGeneral.TipoImagenN1
                         && x.HER_Nombre == archivo)
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.AnyAsync();
        }
        public async Task<bool> ExistePortadaPropiedadConfiguracionAsync(string archivo)
        {
            var configuracionQuery = _context.HER_AnexoGeneral
                .Where(x => x.HER_TipoRegistro == ConstConfiguracionGeneral.TipoImagenN2
                         && x.HER_Nombre == archivo)
                .AsNoTracking()
                .AsQueryable();

            return await configuracionQuery.AnyAsync();
        }
        public async Task<bool> ActualizarImagenPropiedadConfiguracionAsync(EditarImagenConfiguracionGeneral modelo)
        {
            int result = 0;

            var anexoGeneralQuery = _context.HER_AnexoGeneral
                .Where(x => x.HER_TipoRegistro == modelo.Tipo)
                .AsQueryable();
            
            var anexoGeneral = await anexoGeneralQuery
                .FirstOrDefaultAsync();

            //--
            var nombreArchivo = modelo.Imagen.FileName;
            var carpeta = Path.Combine(_environment.WebRootPath, "images");
            var rutaArchivo = Path.Combine(_environment.WebRootPath, "images", nombreArchivo);
            string tipoArchivo = modelo.Imagen.ContentType;
            //--
            try
            {
                //--
                bool existe = Directory.Exists(carpeta);
                if (!existe)
                    Directory.CreateDirectory(carpeta);

                if (modelo.Imagen.Length > 0)
                {
                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await modelo.Imagen.CopyToAsync(fileStream);
                    }

                    //--
                    anexoGeneral.HER_Nombre = nombreArchivo;
                    anexoGeneral.HER_TipoContenido = tipoArchivo;
                    anexoGeneral.HER_RutaComplemento = "images";

                    _context.HER_AnexoGeneral.Update(anexoGeneral).State = EntityState.Modified;
                    result = await _context.SaveChangesAsync();
                }
                //--
            }
            catch (Exception ex)
            {
                _logger.LogError("AnexoService:GuardarAnexoTemporalAsync: " + ex.Message);
            }

            return (result > 0) ? true : false;
        }
        public async Task<List<EditarColeccionConfiguracionGeneral>> ObtenerColores()
        {
            var coloresQuery = _context.HER_Color
                .Select(x => new EditarColeccionConfiguracionGeneral(){
                    Id = x.HER_ColorId,
                    Valor = string.Format("{0} - {1}", x.HER_Nombre, x.HER_Codigo),
                    Estado = true
                })
                .AsNoTracking()
                .AsQueryable();

            return await coloresQuery.ToListAsync();
        }
        public async Task<List<EditarColeccionConfiguracionGeneral>> ObtenerExtensiones()
        {
            var extensionesQuery = _context.HER_Extension
                .Select(x => new EditarColeccionConfiguracionGeneral()
                {
                    Id = x.HER_ExtensionId,
                    Valor = x.HER_Nombre,
                    Estado = true
                })
                .AsNoTracking()
                .AsQueryable();

            return await extensionesQuery.ToListAsync();
        }
        public async Task<string> ObtenerExtensionesEnCadena()
        {
            var extensionesQuery = _context.HER_Extension
                .Select(x => x.HER_Nombre)
                .AsNoTracking()
                .AsQueryable();

            return string.Join(",", await extensionesQuery.ToListAsync());
        }
        public async Task<bool> ActualizarExtensionesConfiguracionAsync(List<EditarColeccionConfiguracionGeneral> actuales, List<EditarColeccionConfiguracionGeneral> nuevos)
        {
            int result = 0;

            //Eliminar actuales
            var paraEliminar = actuales
                .Where(x => x.Estado == false)
                .Select(x => x.Id).ToList();

            if (paraEliminar.Count > 0)
            {
                var eliminarQuery = _context.HER_Extension
                .Where(x => paraEliminar.Contains(x.HER_ExtensionId))
                .AsQueryable();

                //Borrar
                _context.HER_Extension.RemoveRange(await eliminarQuery.ToListAsync());
            }

            ////Agregar nuevos
            //var configuracionQuery = _context.HER_ConfiguracionColeccion
            //    .AsQueryable();
            //var configuracion = await configuracionQuery.FirstOrDefaultAsync();
            ////--
            var paraAgregar = new List<HER_Extension>();
            foreach (var nuevo in nuevos)
            {
                paraAgregar.Add(new HER_Extension() {
                    HER_Nombre = nuevo.Valor,
                    //HER_ColeccionId = configuracion.HER_ConfiguracionColeccionId
                });
            }

            _context.HER_Extension.AddRange(paraAgregar);
            result = await _context.SaveChangesAsync();

            return (result > 0) ? true : false;
        }
        //--
        //Rutas Base
        public async Task<bool> GuardarRutaBaseAsync(CrearAnexoRutaViewModel viewModel)
        {
            int result = 0;
            //--
            var rutasQuery = _context.HER_AnexoRuta
                .AsQueryable();

            var rutas = await rutasQuery.ToListAsync();
            foreach (var ruta in rutas)
            {
                if (ruta.HER_Estado == ConstAnexoRuta.EstadoN1)
                {
                    ruta.HER_Estado = ConstAnexoRuta.EstadoN2;
                }
            }
            _context.HER_AnexoRuta.UpdateRange(rutas);

            //--
            var nuevaRutaBase = new HER_AnexoRuta
            {
                HER_RutaBase = viewModel.Ruta,
                HER_Estado = ConstAnexoRuta.EstadoN1,
                HER_FechaRegistro = DateTime.Now,
                HER_FechaActualizacion = DateTime.Now
            };
            _context.HER_AnexoRuta.Add(nuevaRutaBase);

            result = await _context.SaveChangesAsync();

            return (result > 0) ? true : false;
        }
        public async Task<EditarAnexoRutaViewModel> ObtenerEditarRutaBaseAsync(int id)
        {
            //--
            var rutaQuery = _context.HER_AnexoRuta
                .Where(x => x.HER_AnexoRutaId == id)
                .Select(x => new EditarAnexoRutaViewModel {
                    RutaBaseId = x.HER_AnexoRutaId,
                    Estado = x.HER_Estado.ToString(),
                    Ruta = x.HER_RutaBase
                })
                .AsTracking()
                .AsQueryable();

            var ruta = await rutaQuery.FirstOrDefaultAsync();

            return ruta;
        }
        public async Task<bool> EditarRutaBaseAsync(EditarAnexoRutaViewModel viewModel)
        {
            int result = 0;
            //--
            var rutaQuery = _context.HER_AnexoRuta
                .Where(x => x.HER_AnexoRutaId == viewModel.RutaBaseId)
                .AsQueryable();

            var ruta = await rutaQuery.FirstOrDefaultAsync();

            ruta.HER_RutaBase = viewModel.Ruta;
            ruta.HER_Estado = int.Parse(viewModel.Estado);
            ruta.HER_FechaActualizacion = DateTime.Now;

            _context.HER_AnexoRuta.Update(ruta).State = EntityState.Modified;

            result = await _context.SaveChangesAsync();

            return (result > 0) ? true : false;
        }
        public async Task<int> TotalRutasActivasAsync(int rutaBaseId)
        {
            var rutaQuery = _context.HER_AnexoRuta
                .Where(x => x.HER_Estado == ConstAnexoRuta.EstadoN1 && x.HER_AnexoRutaId != rutaBaseId)
                .AsTracking()
                .AsQueryable();

            return await rutaQuery.CountAsync();
        }
    }
}
