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

namespace Hermes2018.Controllers.Api.Tramites
{
    [ApiController]
    public class TramitesController : ApiController
    {
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly ITramiteService _tramiteService;

        public TramitesController(ITramiteService tramiteService)
        {
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
            _tramiteService = tramiteService;
        }

        [HttpGet("tramites")]
        public async Task<IActionResult> GetTramites()
        {
            List<ListadoTramitesViewModel> listadoTramites = await _tramiteService.ListadoTramitesPorNombreAsync();

            return new JsonResult(listadoTramites, _jsonSettings);
        }
    }
}
