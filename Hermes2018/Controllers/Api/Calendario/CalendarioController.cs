using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Controllers.Api.Calendario
{
    [ApiController]
    public class CalendarioController : ApiController
    {
        private readonly ICalendarioService _calendarioService;
        private readonly JsonSerializerSettings _jsonSettings;

        public CalendarioController(ICalendarioService calendarioService)
        {
            _calendarioService = calendarioService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpGet("calendario/info/{dias}/{recepcionId}")]
        public async Task<IActionResult> GetInfoCalendarioAsync(int dias, int recepcionId)
        {
            CalendarioDiasInhabilesViewModel infoCalendario = await _calendarioService.ObtenerDiasInhabilesCalendarioActualPorDiasDadosAsync(dias, recepcionId);

            return new JsonResult(infoCalendario, _jsonSettings);
        }
    }
}
