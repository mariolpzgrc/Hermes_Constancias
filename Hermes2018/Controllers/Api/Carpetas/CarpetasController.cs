using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Carpeta;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Carpetas
{
    [ApiController]
    //[AllowAnonymous]
    public class CarpetasController : ApiController
    {
        private readonly ICarpetaService _carpetaService;
        private readonly JsonSerializerSettings _jsonSettings;

        public CarpetasController(ICarpetaService carpetaService)
        {
            _carpetaService = carpetaService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpGet("carpetas/{username}")]
        public async Task<IActionResult> GetCarpetasUsuario(string username)
        {
            return new JsonResult( await _carpetaService.ListadoCarpetasPorUsuarioAsync(username), _jsonSettings);
        }

        [HttpPost("carpetas/mover/documentos/{bandeja}")]
        public async Task<IActionResult> SetMoverDocumentosAsync(int bandeja, MoverDocumentoJsonModel solicitud)
        {
            if (bandeja == ConstBandejas.ConstTipoN1)
            {
                return new JsonResult(new { estado = await _carpetaService.MoverDocumentosRecibidosAsync(solicitud) }, _jsonSettings);
            }
            else if (bandeja == ConstBandejas.ConstTipoN2)
            {
                return new JsonResult(new { estado = await _carpetaService.MoverDocumentosEnviadosAsync(solicitud) }, _jsonSettings);
            }
            else {
                return new JsonResult(new { estado = false }, _jsonSettings);
            }
        }
    }
}