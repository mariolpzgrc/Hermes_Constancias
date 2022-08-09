using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Documento;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hermes2018.Controllers.Api.Anexos
{
    
    [ApiController]
    public class AnexosController : ApiController
	{
        private readonly IAnexoService _anexoService;
        private readonly JsonSerializerSettings _jsonSettings;


        public AnexosController(IAnexoService anexoService)
		{
            _anexoService = anexoService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        //--Archivos Temporales
        [HttpPost("anexos/subir/archivo/temporal/{folio}")]
        public async Task<IActionResult> GuardarAnexoTemporalAsync(string folio, IFormFile file)
        {
            string nombreArchivo = await _anexoService.GuardarAnexoTemporalAsync(folio, file);
             
            return new JsonResult(nombreArchivo, _jsonSettings);
        }

        [HttpPost("anexos/borrar/archivo/temporal")]
        public IActionResult BorrarAnexoTemporal(AnexoTempJsonModel tempViewModel)
        {
            string nombrArchivo = _anexoService.BorrarAnexoTemporal(tempViewModel);

            return new JsonResult(nombrArchivo, _jsonSettings);
        }

        [HttpPost("anexos/borrar/archivos/temporales")]
        public IActionResult BorrarAnexosTemporales(AnexoFolioJsonModel folioJsonModel)
        {
            bool respuesta = _anexoService.BorrarAnexosTemporales(folioJsonModel);
            
            return new JsonResult(respuesta, _jsonSettings);
        }

        //--Archivos Finales
        [HttpPost("anexos/descarga/archivos/documento")]
        public async Task<IActionResult> ObtenerArchivosFinalesAsync(AnexoDescargaJsonModel descargaJsonModel)
        {
            var listadoArchivos = await _anexoService.ObtenerAnexosDocumentoBaseAsync(descargaJsonModel);
            
            return new JsonResult(listadoArchivos, _jsonSettings);
        }

        [HttpPost("anexos/borrar/archivo/documento")]
        public async Task<IActionResult> BorrarArchivoFinalAsync(AnexoFinalJsonModel finalJsonModel)
        {
            string archivoNombre = await _anexoService.BorrarAnexoDocumentoBaseAsync(finalJsonModel);

            return new JsonResult(archivoNombre, _jsonSettings);
        }

        ////-------------------

        [HttpPost("anexos/descarga/envios")]
        public async Task<IActionResult> ObtenerAnexosDocumentoEnviadoAsync(AnexoDescargaEnvioJsonModel envioJsonModel)
        {
            List<AnexosDocumentoViewModel> listadoArchivos = await _anexoService.ObtenerAnexosDocumentoEnviadoAsync(envioJsonModel);

            return new JsonResult(listadoArchivos, _jsonSettings);
        }

        [HttpPost("anexos/descarga/enviosOrigen")]
        public async Task<IActionResult> ObtenerAnexosDocumentoEnviadoOrigenAsync(AnexoDescargaEnvioJsonModel envioJsonModel)
        {
            List<AnexosDocumentoViewModel> listadoArchivos = await _anexoService.ObtenerAnexosDocumentoEnviadoOrigenAsync(envioJsonModel);

            return new JsonResult(listadoArchivos, _jsonSettings);
        }

        [HttpPost("anexos/descarga/turnados")]
        public async Task<IActionResult> ObtenerAnexosDocumentoTurnadoAsync(AnexoDescargaTurnarJsonModel turnarJsonModel)
        {
            List<AnexosDocumentoViewModel> listadoArchivos = await _anexoService.ObtenerAnexosDocumentoTurnadoAsync(turnarJsonModel);

            return new JsonResult(listadoArchivos, _jsonSettings);
        }

        //-------------------
        [HttpPost("CargaImagenes/{usuario}/{folio}")]
        public IActionResult AddImage(string usuario, string folio, IFormFile file)
        {
            var baseUrl = Url.Content("~/");

            //return await _anexoService.GuardarImagenesTempAsync(usuario, folio, file, baseUrl);
            return _anexoService.GuardarImagenesTemp64(usuario, folio, file, baseUrl);
        }

        [HttpPost("EliminaImagenes")]
        public IActionResult DeleteImage(string src)
        {
            var deletedImagen = HttpContext.Request.Form["src"];
            return _anexoService.EliminarImagenesTemp(deletedImagen);
        }

    }
}
