using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Historico
{
    [ApiController]
    public class HistoricoController : ApiController
    {
        private readonly IHistoricoService _historicoService;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly IConfiguracionService _configuracionService;

        public HistoricoController(IHistoricoService historicoService, IConfiguracionService configuracionService)
        {
            _historicoService = historicoService;
            _configuracionService = configuracionService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpGet("historico/bandeja/recibidos/{infoUsuarioId}/{pagina}")]
        public async Task<IActionResult> GetHistoricoRecibidos(int infoUsuarioId, int? pagina)
        {
            IQueryable<DocumentoRecibidoViewModel> fuenteQuery = _historicoService.ObtenerCorrespondenciaRecibidos(infoUsuarioId);

            int totalElementos = await fuenteQuery.CountAsync();
            int paginaActual = pagina ?? 1;
            int elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaPorUsuarioIdAsync(infoUsuarioId);
            int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
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

        [HttpGet("historico/bandeja/enviados/{infoUsuarioId}/{pagina}")]
        public async Task<IActionResult> GetHistoricoEnviados(int infoUsuarioId, int? pagina)
        {
            IQueryable<DocumentoEnviadoViewModel> fuenteQuery = _historicoService.ObtenerCorrespondenciaEnviados(infoUsuarioId);

            int totalElementos = await fuenteQuery.CountAsync();
            int paginaActual = pagina ?? 1;
            int elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaPorUsuarioIdAsync(infoUsuarioId);
            int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
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

        [HttpGet("historico/bandeja/borradores/{infoUsuarioId}/{pagina}")]
        public async Task<IActionResult> GetHistoricoBorradores(int infoUsuarioId, int? pagina)
        {
            IQueryable<DocumentoBorradorViewModel> fuenteQuery = _historicoService.ObtenerCorrespondenciaBorradores(infoUsuarioId);

            int totalElementos = await fuenteQuery.CountAsync();
            int paginaActual = pagina ?? 1;
            int elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaPorUsuarioIdAsync(infoUsuarioId);
            int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
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

        [HttpGet("historico/bandeja/revision/{infoUsuarioId}/{pagina}")]
        public async Task<IActionResult> GetHistoricoRevision(int infoUsuarioId, int? pagina)
        {
            IQueryable<DocumentoRevisionViewModel> fuenteQuery = _historicoService.ObtenerCorrespondenciaRevision(infoUsuarioId);

            int totalElementos = await fuenteQuery.CountAsync();
            int paginaActual = pagina ?? 1;
            int elementosPorPagina = await _configuracionService.ObtenerElementosPorPaginaPorUsuarioIdAsync(infoUsuarioId);
            int totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
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
    }
}