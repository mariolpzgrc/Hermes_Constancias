using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Models;
using Hermes2018.Models.Plantilla;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Plantillas
{
    [ApiController]
    public class PlantillasController : ApiController
    {
        private readonly IPlantillaService _plantillaService;
        private readonly JsonSerializerSettings _jsonSettings;

        public PlantillasController(IPlantillaService plantillaService)
        {
            _plantillaService = plantillaService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpPost("plantillas/{username}")]
        public async Task<IActionResult> SetPlantillaAsync(string username, PlantillaJsonModel  plantillaJsonModel)
        {
            bool result = false;
            var existe = await _plantillaService.ExistePlantillaAsync(plantillaJsonModel.HER_Nombre, username);
            if (!existe)
            {
                NuevaPlantillaViewModel nuevaPlantilla = new NuevaPlantillaViewModel() {
                    HER_Usuario = username,
                    HER_Nombre = plantillaJsonModel.HER_Nombre,
                    HER_Texto = plantillaJsonModel.HER_Texto
                };

                result = await _plantillaService.GuardarPlantillaAsync(nuevaPlantilla);

                if (result)
                {
                    return new JsonResult("1", _jsonSettings);
                }
                else
                {
                    return new JsonResult("0", _jsonSettings);
                }
            }
            else
            {
                return new JsonResult("2", _jsonSettings);
            }
        }

        [HttpGet("plantillas/{username}")]
        public async Task<IActionResult> GetPlantillasAsync(string username)
        {
            List<PlantillaSimplificadaViewModel> plantillas = await _plantillaService.ObtenerPlantillasAsync(username);

            return new JsonResult(plantillas, _jsonSettings);
        }

        [HttpGet("plantillas/{nombreplantilla}:{username}")]
        public async Task<IActionResult> GetPlantillaAsync(string nombrePlantilla, string username)
        { 
            var existe = await _plantillaService.ExistePlantillaAsync(nombrePlantilla, username);
            if (existe)
            {
                PlantillaViewModel plantilla = await _plantillaService.ObtenerPlantillaAsync(nombrePlantilla, username);

                return new JsonResult(plantilla, _jsonSettings);
            }
            else
            {
                return new JsonResult("0", _jsonSettings);
            }
        }
    }
}
