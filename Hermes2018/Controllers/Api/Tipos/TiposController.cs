using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Tipos
{
    [ApiController]
    public class TiposController : ApiController
    {
        private readonly ITipoEnvioService _tipoService;
        private readonly JsonSerializerSettings _jsonSettings;

        public TiposController(ITipoEnvioService tipoService)
        {
            _tipoService = tipoService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpGet("tipos/envio")]
        public async Task<IActionResult> GetTiposEnvio()
        {
            return new JsonResult(await _tipoService.ObtenerTiposAsync(), _jsonSettings);
        }
    }
}