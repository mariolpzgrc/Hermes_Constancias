using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Models;
using Hermes2018.Models.Grupo;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Grupos
{
    [ApiController]
    public class GruposController : ApiController
    {
        private readonly IGrupoService _grupoService;
        private readonly JsonSerializerSettings _jsonSettings;

        public GruposController(IGrupoService grupoService)
        {
            _grupoService = grupoService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }
        
        [HttpGet("grupos/{username}")]
        public async Task<IActionResult> GetGroupsUserAsync(string username)
        {
            List<GruposViewModel> listadogrupos = await _grupoService.ObtenerGruposAsync(username);

            return new JsonResult(listadogrupos, _jsonSettings);
        }

        [HttpGet("grupos/usuarios/{grupoid}")]
        public async Task<IActionResult> GetUsersInGroupAsync(int grupoid)
        {
            List<UsuarioLocalJsonModel> listadoIntegrantes = await _grupoService.ObtenerIntegrantesGrupoAsync(grupoid);

            return new JsonResult(listadoIntegrantes, _jsonSettings);
        }
    }
}
