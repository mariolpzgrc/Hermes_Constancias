using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Models.Documento;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Documentos
{
    [Authorize]
    [ApiController]
	public class DocumentosController : ApiController
	{
        private readonly IDocumentoService _documentoService;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly CultureInfo _cultureEs;
        private readonly IConfiguracionService _configuracionService;

        public DocumentosController(IDocumentoService documentoService, 
            IConfiguracionService configuracionService)
		{
            _documentoService = documentoService;
            _configuracionService = configuracionService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
            _cultureEs = new CultureInfo("es-MX");
        }

        [HttpGet("documentos/recibidos/{username}/{pagina}")]
        public async Task<IActionResult> GetBandejaRecibidos(string username, int? pagina, [FromQuery] string busqueda, [FromQuery] int? categoria, [FromQuery] string fechaini, [FromQuery] string fechafin, [FromQuery] int? estado, [FromQuery] int? tipo, [FromQuery] int? proximovencer, [FromQuery] int? tramite, [FromQuery] bool enCarpeta)
        {
            IQueryable<DocumentoRecibidoViewModel> fuenteQuery;

            if (enCarpeta) 
            {
                fuenteQuery = _documentoService.ListadoDocumentosRecibidosEnCarpetas(username, busqueda, categoria, fechaini, fechafin, estado, tipo, proximovencer, tramite);
            }
            else
            {
                fuenteQuery = _documentoService.ListadoDocumentosRecibidos(username, busqueda, categoria, fechaini, fechafin, estado, tipo, proximovencer, tramite);
            }

            int totalElementos = await fuenteQuery.CountAsync();
            int elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaUsuarioAsync(username);
            int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
            int paginaActual = (pagina == null) ? 1 : (pagina > totalPaginas)? 1: (int)pagina;

            var elementos = await fuenteQuery
                .Skip((paginaActual - 1) * elementosPorPagina)
                .Take(elementosPorPagina)
                .ToListAsync();

            var resultado = new
            {
                Total_Elementos = totalElementos,
                Elementos_Por_Pagina = elementosPorPagina,
                Elementos_Pagina_Actual = elementos.Count(), 
                Pagina_Actual = paginaActual,
                Total_Paginas = totalPaginas,
                Datos = elementos
            };

            return new JsonResult(resultado, _jsonSettings);
        }

        [HttpGet("documentos/enviados/{username}/{pagina}")]
        public async Task<IActionResult> GetBandejaEnviados(string username, int? pagina, [FromQuery] string busqueda, [FromQuery] int? categoria, [FromQuery] string fechaini, [FromQuery] string fechafin, [FromQuery] int? estado, [FromQuery] int? tipo, [FromQuery] int? tramite, [FromQuery] bool enCarpeta)
        {

            IQueryable<DocumentoEnviadoViewModel> fuenteQuery;

            if (enCarpeta)
            {
                fuenteQuery = _documentoService.ListadoDocumentosEnviadosEnCarpetas(username, busqueda, categoria, fechaini, fechafin, estado, tipo, tramite);
            }
            else
            {
                fuenteQuery = _documentoService.ListadoDocumentosEnviados(username, busqueda, categoria, fechaini, fechafin, estado, tipo, tramite);
            }    
              

            int totalElementos = await fuenteQuery.CountAsync();
            int elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaUsuarioAsync(username);
            int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
            int paginaActual = (pagina == null) ? 1 : (pagina > totalPaginas) ? 1 : (int)pagina;

            var elementos = await fuenteQuery
                .Skip((paginaActual - 1) * elementosPorPagina)
                .Take(elementosPorPagina)
                .ToListAsync();

            var resultado = new
            {
                Total_Elementos = totalElementos,
                Elementos_Por_Pagina = elementosPorPagina,
                Elementos_Pagina_Actual = elementos.Count(),
                Pagina_Actual = paginaActual,
                Total_Paginas = totalPaginas,
                Datos = elementos
            };

            return new JsonResult(resultado, _jsonSettings);
        }

        [HttpGet("documentos/borradores/{username}/{pagina}")]
        public async Task<IActionResult> GetBandejaBorradores(string username, int? pagina, [FromQuery] string busqueda, [FromQuery] int? categoria, [FromQuery] string fechaini, [FromQuery] string fechafin)
        {
            IQueryable<DocumentoBorradorViewModel> fuenteQuery = _documentoService.ListadoBorradores(username, busqueda, categoria, fechaini, fechafin);

            int totalElementos = await fuenteQuery.CountAsync();
            int elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaUsuarioAsync(username);
            int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
            int paginaActual = (pagina == null) ? 1 : (pagina > totalPaginas) ? 1 : (int)pagina;

            var elementos = await fuenteQuery
                .Skip((paginaActual - 1) * elementosPorPagina)
                .Take(elementosPorPagina)
                .ToListAsync();

            var resultado = new
            {
                Total_Elementos = totalElementos,
                Elementos_Por_Pagina = elementosPorPagina,
                Elementos_Pagina_Actual = elementos.Count(),
                Pagina_Actual = paginaActual,
                Total_Paginas = totalPaginas,
                Datos = elementos
            };

            return new JsonResult(resultado, _jsonSettings);
        }

        [HttpGet("documentos/revision/{username}/{pagina}")]
        public async Task<IActionResult> GetOficiosRevisionAsync(string username, int? pagina, [FromQuery] string busqueda, [FromQuery] int? categoria, [FromQuery] string fechaini, [FromQuery] string fechafin)
        {
            IQueryable<DocumentoRevisionViewModel> fuenteQuery = _documentoService.ListadoRevisionRemitente(username, busqueda, categoria, fechaini, fechafin);

            int totalElementos = await fuenteQuery.CountAsync();
            int elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaUsuarioAsync(username);
            int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
            int paginaActual = (pagina == null) ? 1 : (pagina > totalPaginas) ? 1 : (int)pagina;

            var elementos = await fuenteQuery
                .Skip((paginaActual - 1) * elementosPorPagina)
                .Take(elementosPorPagina)
                .ToListAsync();

            var resultado = new
            {
                Total_Elementos = totalElementos,
                Elementos_Por_Pagina = elementosPorPagina,
                Elementos_Pagina_Actual = elementos.Count(),
                Pagina_Actual = paginaActual,
                Total_Paginas = totalPaginas,
                Datos = elementos
            };

            return new JsonResult(resultado, _jsonSettings);
        }

        //[Seguimiento]
        [HttpPost("documentos/seguimiento/envios/{pagina}")]
        public async Task<IActionResult> ObtenerSeguimientoEnvioAsync(int? pagina, SeguimientoSolicitudJsonModel solicitud)
        {
            IQueryable<HER_Envio> fuenteQuery = _documentoService.ObtenerSeguimientoEnvio(solicitud.Folio);
            List<SeguimientoEnvioJsonModel> listadoEnvios = new List<SeguimientoEnvioJsonModel>();
            List<SeguimientoEnvioJsonModel> listadoRespuestas = new List<SeguimientoEnvioJsonModel>();
            List<SeguimientoRecepcionJsonModel> destinatarios = new List<SeguimientoRecepcionJsonModel>();
            //--
            int totalElementos = await fuenteQuery
                .Where(x => x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 || x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2 || (x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 && x.HER_EsReenvio) || (x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN4 && x.HER_EsReenvio))
                .CountAsync();

            int elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaUsuarioAsync(solicitud.Usuario);
            int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
            int paginaActual = (pagina == null) ? 1 : (pagina > totalPaginas) ? 1 : (int)pagina;

            //---[Envios]
            var envios = await fuenteQuery
                .Where(x => x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 || x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2 || (x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 && x.HER_EsReenvio) || (x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN4 && x.HER_EsReenvio))
                .Skip((paginaActual - 1) * elementosPorPagina)
                .Take(elementosPorPagina)
                .OrderBy(x => x.HER_FechaEnvio)
                .AsNoTracking()
                .ToListAsync();

            //--
            foreach (var envio in envios)
            {
                destinatarios = new List<SeguimientoRecepcionJsonModel>();

                foreach (var recepcion in envio.HER_Recepciones.OrderBy(x => x.HER_FechaRecepcion).ToList())
                {
                    destinatarios.Add(new SeguimientoRecepcionJsonModel()
                    {
                        RecepcionId = recepcion.HER_RecepcionId,
                        Fecha = recepcion.HER_FechaRecepcion.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                        Hora = recepcion.HER_FechaRecepcion.ToString("HH:mm 'hrs.'", _cultureEs),
                        Para = recepcion.HER_Para.HER_NombreCompleto,
                        UsuarioPara = recepcion.HER_Para.HER_UserName,
                        TipoPara = recepcion.HER_TipoPara,
                        Estado = recepcion.HER_EstadoEnvioId,
                        TieneRespuesta = recepcion.HER_TieneRespuesta,
                    });
                }

                listadoEnvios.Add(new SeguimientoEnvioJsonModel()
                {
                    UsuarioOrigen = envio.HER_UsuarioOrigenId != null ? envio.HER_UsuarioOrigen.HER_UserName : null,
                    EnvioPadre = envio.HER_EnvioPadreId,
                    EnvioId = envio.HER_EnvioId,
                    TipoEnvio = (envio.HER_EsReenvio && envio.HER_EnvioPadre.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN4) ? ConstTipoEnvio.TipoEnvioN2 : envio.HER_TipoEnvioId,
                    DocumentoId = envio.HER_DocumentoId,
                    Fecha = envio.HER_FechaEnvio.ToString("D", _cultureEs),
                    Hora = envio.HER_FechaEnvio.ToString("HH:mm 'hrs.'", _cultureEs),
                    De = envio.HER_De.HER_NombreCompleto,
                    UsuarioDe = envio.HER_De.HER_UserName,                    
                    Estado = envio.HER_EstadoEnvioId,
                    EsPublico = (envio.HER_VisibilidadId == ConstVisibilidad.VisibilidadN1) ? true : false,
                    Actual = (solicitud.EnvioId == envio.HER_EnvioId) ? true : false,
                    Destinatarios = destinatarios,
                    EsReenvio = (envio.HER_EsReenvio && envio.HER_EnvioPadre.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN4) ? false: envio.HER_EsReenvio,
                    VisualizacionTipoEnvio = envio.HER_TipoEnvioId
                });
            }

            //--
            var enviosIds = envios.Select(x => x.HER_EnvioId).ToList();
            //--
            var respuestas = await fuenteQuery
                .Where(x => enviosIds.Contains((int)x.HER_EnvioPadreId) &&
                            x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN3 || (x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN4 && !x.HER_EsReenvio))
                .OrderBy(x => x.HER_EnvioPadreId)
                    .ThenBy(x => x.HER_FechaEnvio)
                .ToListAsync();

            foreach (var respuesta in respuestas)
            {
                destinatarios = new List<SeguimientoRecepcionJsonModel>();

                foreach (var recepcion in respuesta.HER_Recepciones.OrderBy(x => x.HER_FechaRecepcion).ToList())
                {
                    destinatarios.Add(new SeguimientoRecepcionJsonModel()
                    {
                        RecepcionId = recepcion.HER_RecepcionId,
                        Fecha = recepcion.HER_FechaRecepcion.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                        Hora = recepcion.HER_FechaRecepcion.ToString("HH:mm 'hrs.'", _cultureEs),
                        Para = recepcion.HER_Para.HER_NombreCompleto,
                        UsuarioPara = recepcion.HER_Para.HER_UserName,
                        TipoPara = recepcion.HER_TipoPara,
                        Estado = recepcion.HER_EstadoEnvioId,
                        TieneRespuesta = recepcion.HER_TieneRespuesta,
                    });
                }

                listadoRespuestas.Add(new SeguimientoEnvioJsonModel()
                {
                    UsuarioOrigen = respuesta.HER_UsuarioOrigenId != null ? respuesta.HER_UsuarioOrigen.HER_UserName : null,
                    EnvioPadre = respuesta.HER_EnvioPadreId,
                    EnvioId = respuesta.HER_EnvioId,
                    TipoEnvio = respuesta.HER_TipoEnvioId,
                    DocumentoId = respuesta.HER_DocumentoId,
                    Fecha = respuesta.HER_FechaEnvio.ToString("D", _cultureEs),
                    Hora = respuesta.HER_FechaEnvio.ToString("HH:mm 'hrs.'", _cultureEs),
                    De = respuesta.HER_De.HER_NombreCompleto,
                    UsuarioDe = respuesta.HER_De.HER_UserName,
                    Estado = respuesta.HER_EstadoEnvioId,
                    EsPublico = (respuesta.HER_VisibilidadId == ConstVisibilidad.VisibilidadN1) ? true : false,
                    Actual = (solicitud.EnvioId == respuesta.HER_EnvioId) ? true : false,
                    Destinatarios = destinatarios,
                    EsReenvio = respuesta.HER_EsReenvio
                });
            }

            var resultado = new
            {
                Total_Elementos = totalElementos,
                Elementos_Por_Pagina = elementosPorPagina,
                Elementos_Pagina_Actual = listadoEnvios.Count(),
                Pagina_Actual = paginaActual,
                Total_Paginas = totalPaginas,
                Datos1 = listadoEnvios,
                Datos2 = listadoRespuestas,
            };

            return new JsonResult(resultado, _jsonSettings);
        }

        [HttpGet("documentos/carpeta/personal/{username}/{pagina}/{carpetaId}/{bandejaorigen}")]
        public async Task<IActionResult> GetBandejaCarpetaPersonal(string username, int? pagina, int carpetaId, int bandejaorigen, [FromQuery] string busqueda, [FromQuery] int? categoria, [FromQuery] string fechaini, [FromQuery] string fechafin)
        {
            if (bandejaorigen >= ConstBandejas.ConstTipoN1 && bandejaorigen <= ConstBandejas.ConstTipoN2)
            {
                IQueryable<DocumentoEnCarpetaPersonalViewModel> fuenteQuery = null;

                if (bandejaorigen == ConstBandejas.ConstTipoN1)
                {
                    fuenteQuery = _documentoService.ListadoDocumentosRecibidosEnCarpetasPersonales(username, carpetaId, busqueda, categoria, fechaini, fechafin);
                }
                else if (bandejaorigen == ConstBandejas.ConstTipoN2)
                {
                    fuenteQuery = _documentoService.ListadoDocumentosEnviadosEnCarpetasPersonales(username, carpetaId, busqueda, categoria, fechaini, fechafin);
                }

                var totalElementos = await fuenteQuery.CountAsync();
                var elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaUsuarioAsync(username);
                var totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
                var paginaActual = (pagina == null) ? 1 : (pagina > totalPaginas) ? 1 : (int)pagina;

                var elementos = await fuenteQuery
                    .Skip((paginaActual - 1) * elementosPorPagina)
                    .Take(elementosPorPagina)
                    .ToListAsync();

                var resultado = new
                {
                    Total_Elementos = totalElementos,
                    Elementos_Por_Pagina = elementosPorPagina,
                    Elementos_Pagina_Actual = elementos.Count(),
                    Pagina_Actual = paginaActual,
                    Total_Paginas = totalPaginas,
                    Datos = elementos
                };

                return new JsonResult(resultado, _jsonSettings);
            }
            else {
                var resultado = new
                {
                    Total_Elementos = 0,
                    Elementos_Por_Pagina = await _configuracionService.ObtenerElementosPorPaginaUsuarioAsync(username),
                    Elementos_Pagina_Actual = 0,
                    Pagina_Actual = 1,
                    Total_Paginas = 1,
                    Datos = new List<DocumentoEnCarpetaPersonalViewModel>()
                };

                return new JsonResult(resultado, _jsonSettings);
            }
        }

    }
}
