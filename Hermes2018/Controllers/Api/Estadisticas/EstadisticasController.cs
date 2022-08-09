using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Estadisticas
{
    [ApiController]
    public class EstadisticasController : ApiController
    {
        private readonly IEstadisticasService _estadisticasServices;
        private readonly JsonSerializerSettings _jsonSettings;

        public EstadisticasController(IEstadisticasService estadisticasService)
        {
            _estadisticasServices = estadisticasService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpGet("estadisticas/recibidos/{username}")]
        public async Task<IActionResult> GetEstadisticasOficiosRecibidos(string username = "", [FromQuery] string categoria = "", [FromQuery] string fechainicio = "", [FromQuery] string fechafin = "")
        {

            return new JsonResult(await _estadisticasServices.ObtenerEstadisticasDocumentosRecibidosPorEstadoAsync(username, categoria, fechainicio, fechafin), _jsonSettings);
        }

        [HttpGet("estadisticas/enviados/{username}")]
        public async Task<IActionResult> GetEstadisticasOficiosEnviados(string username = "", [FromQuery] string categoria = "", [FromQuery] string fechainicio = "", [FromQuery] string fechafin = "")
        {

            return new JsonResult(await _estadisticasServices.ObtenerEstadisticasDocumentosEnviadosPorEstadoAsync(username, categoria, fechainicio, fechafin), _jsonSettings);
        }
    }
}