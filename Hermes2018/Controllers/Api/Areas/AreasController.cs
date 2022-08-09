using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Area;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Controllers.Api.Areas
{
    [ApiController]
    public class AreasController : ApiController
    {
        private readonly IAreaService _areaService;
        private readonly IRegionService _regionService;
        private readonly IUsuarioService _usuarioService;
        private readonly JsonSerializerSettings _jsonSettings;

        public AreasController(IAreaService areaService,
                               IRegionService regionService,
                               IUsuarioService usuarioService)
        {
            _areaService = areaService;
            _regionService = regionService;
            _usuarioService = usuarioService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpGet("areas/{regionId}")]
        public async Task<IActionResult> GetAreasByRegionAsync(string regionId)
        {
            List<HER_Area> areas = await _areaService.ObtenerAreasVisibleAsync(Convert.ToInt32(regionId));

            return new JsonResult(areas, _jsonSettings);
        }

        [HttpGet("areas/sinorigen/{regionId}/{areaId}")]
        public async Task<IActionResult> GetAreasSinOrigenConRegionAsync(string regionId, string areaId)
        {
            List<HER_Area> areas = await _areaService.ObtenerAreasVisibleAsync(Convert.ToInt32(regionId), Convert.ToInt32(areaId));

            return new JsonResult(areas, _jsonSettings);
        }

        [HttpGet("areas/{username}/{regionId}")]
        public async Task<IActionResult> GetAreasByRegionByUser(string username, int regionId)
        {
            //Valida los parametros
            if (string.IsNullOrEmpty(username)  || regionId == 0)
            {
                return new JsonResult(Array.Empty<string>(), _jsonSettings);
            }

            var existeRegion = await _regionService.ExisteRegionAsync(regionId);
            var existeUsuario = await _usuarioService.ExisteUsuarioActivoAsync(username);
            
            if (!existeRegion || !existeUsuario)
            {
                return new JsonResult(Array.Empty<string>(), _jsonSettings);
            }

            List<AreaViewModel> listadoAreas = await _areaService.ObtenerAreasVisiblesPorRegionPorUsuarioAsync(regionId, username);

            return new JsonResult(listadoAreas, _jsonSettings);
        }

        [HttpGet("areas/buscar")]
        public async Task<IActionResult> GetAreasBuscarAsync([FromQuery] bool contain, [FromQuery] string selected, [FromQuery] string original, [FromQuery] string keyword)
        {
            string busqueda = string.IsNullOrEmpty(keyword) ? string.IsNullOrEmpty(original) ? string.Empty : original : keyword;

            List<BuscarAreaViewModel> usuarios = await _areaService.BusquedaAreasAsync(busqueda);

            return new JsonResult(usuarios, _jsonSettings);
        }

        [HttpGet("areas/padresConHijas/{areaId}")]
        public async Task<IActionResult> GetAreasPadresConHijas(int areaId)
        {
            //Valida los parametros
            if(areaId == 0)
            {
                return new JsonResult(Array.Empty<string>(), _jsonSettings);
            }

            List<AreaViewModel> listaAreas = await _areaService.ObtenerAreasVisiblesPorAreaPadreConHijasAsync(areaId);

            return new JsonResult(listaAreas, _jsonSettings);
        }
    }
}
