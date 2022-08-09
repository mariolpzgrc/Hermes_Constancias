using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Models;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Servicios
{
    [ApiController]
    public class ServiciosController : ApiController
    {
        private readonly IServicioService _servicioService;
        private readonly JsonSerializerSettings _jsonSettings;

        public ServiciosController(IServicioService servicioService)
        {
            _servicioService = servicioService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpGet("servicios")]
        public async Task<IActionResult> GetServicesAsync()
        {
            List<ServiciosCompletosViewModel> listadoServicios = await _servicioService.ObtenerServiciosAsync();

            return new JsonResult(listadoServicios, _jsonSettings);
        }
    }
}
