using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Estados
{
    [ApiController]
    public class EstadosController : ApiController
    {
        private readonly IEstadoEnvioService _estadoService;
        private readonly JsonSerializerSettings _jsonSettings;

        public EstadosController(IEstadoEnvioService estadoService)
        {
            _estadoService = estadoService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpGet("estados/bandeja/recibidos")]
        public async Task<IActionResult> GetEstadosBandejaRecibidos()
        {
            return new JsonResult(await _estadoService.ObtenerEstadosBandejaRecibidosAsync(), _jsonSettings);
        }

        [HttpGet("estados/bandeja/enviados")]
        public async Task<IActionResult> GetEstadosBandejaEnviados()
        {
            return new JsonResult(await _estadoService.ObtenerEstadosBandejaEnviadosAsync(), _jsonSettings);
        }
    }
}