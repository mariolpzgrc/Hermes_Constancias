using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Categoria;
using Hermes2018.Models.Documento;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hermes2018.Controllers.Api.Categorias
{
    [ApiController]
    public class CategoriasController : ApiController
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IUsuarioService _usuarioService;
        private readonly JsonSerializerSettings _jsonSettings;

        public CategoriasController(ICategoriaService categoriaService,
                                    IUsuarioService usuarioService)
        {
            _categoriaService = categoriaService;
            _usuarioService = usuarioService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        [HttpGet("categorias/{username}")]
        public async Task<IActionResult> GetCategoriasUsuario (string username)
        {
            List<CategoriaViewModel> listadoCategorias = new List<CategoriaViewModel>();
            if (!string.IsNullOrEmpty(username))
            {
                listadoCategorias = await _categoriaService.ObtenerCategoriasAsync(username);
            }

            return new JsonResult(listadoCategorias, _jsonSettings);
        }

        [HttpPost("categorias/agregar")]
        public async Task<IActionResult> AgregarCategoriasUsuarioAsync(CrearCategoriaJsonModel crearCategoriaJson)
        {
            //Modelo respuesta 
            CategoriaJsonModel resultadoCategoria = new CategoriaJsonModel();

            if (!string.IsNullOrEmpty(crearCategoriaJson.Categoria) && !string.IsNullOrEmpty(crearCategoriaJson.Usuario))
            {
                var existe = await _categoriaService.ExisteCategoriaPorNombreAsync(crearCategoriaJson.Categoria, crearCategoriaJson.Usuario);
                
                if (!existe)
                {
                    //No existe, entonces se agrega al listado
                    var usuarioId = await  _usuarioService.ObtenerIdentificadorUsuarioAsync(crearCategoriaJson.Usuario);

                    if (usuarioId > 0)
                    {
                        var nueva = new NuevaCategoriaViewModel() {
                            Nombre = crearCategoriaJson.Categoria,
                            Tipo = ConstTipoCategoria.TipoCategoriaN2,
                            InfoUsuarioId = usuarioId
                        };

                        var nuevaCategoria = await _categoriaService.GuardarCategoriaAsync(nueva);

                        resultadoCategoria.CategoriaId = nuevaCategoria.HER_CategoriaId;
                        resultadoCategoria.Nombre = nuevaCategoria.HER_Nombre;
                        resultadoCategoria.Estado = ConstRespuestaCategoria.RespuestaCategoriaN1;
                    }
                    else
                    {
                        resultadoCategoria.CategoriaId = 0;
                        resultadoCategoria.Nombre = string.Empty;
                        resultadoCategoria.Estado = ConstRespuestaCategoria.RespuestaCategoriaN2;
                    }
                }
                else {
                    //Existe la categoría
                    resultadoCategoria.CategoriaId = 0;
                    resultadoCategoria.Nombre = string.Empty;
                    resultadoCategoria.Estado = ConstRespuestaCategoria.RespuestaCategoriaN3;
                }
            }
            else
            {
                resultadoCategoria.CategoriaId = 0;
                resultadoCategoria.Nombre = string.Empty;
                resultadoCategoria.Estado = ConstRespuestaCategoria.RespuestaCategoriaN4;
            }

            return new JsonResult(resultadoCategoria, _jsonSettings);
        }

        [HttpPost("categorias/seleccionadas")]
        public async Task<IActionResult> GetCategoriasSeleccionadas(BorradorCategoriaJsonModel borradorCategoriaJson)
        {
            List<CategoriaSeleccionadaViewModel> listadoCategorias = new List<CategoriaSeleccionadaViewModel>();
            if (!string.IsNullOrEmpty(borradorCategoriaJson.Usuario))
            {
                listadoCategorias =  await _categoriaService.ObtenerCategoriasSeleccionadasAsync(borradorCategoriaJson.Folio, borradorCategoriaJson.Usuario);
            }

            return new JsonResult(listadoCategorias, _jsonSettings);
        }

        [HttpPost("categorias/envio/seleccionadas")]
        public async Task<IActionResult> GetCategoriasEnvioSeleccionadas(SolicitudCategoriaJsonModel solicitudCategoriaJson)
        {
            List<CategoriaSeleccionadaViewModel> listadoCategorias = new List<CategoriaSeleccionadaViewModel>();
            if (!string.IsNullOrEmpty(solicitudCategoriaJson.Usuario))
            {
                listadoCategorias = await _categoriaService.ObtenerCategoriasSeleccionadasEnvioAsync(solicitudCategoriaJson);
            }

            return new JsonResult(listadoCategorias, _jsonSettings);
        }

        [HttpPost("categorias/recepcion/seleccionadas")]
        public async Task<IActionResult> GetCategoriasRecepcionSeleccionadas(SolicitudCategoriaJsonModel solicitudCategoriaJson)
        {
            List<CategoriaSeleccionadaViewModel> listadoCategorias = new List<CategoriaSeleccionadaViewModel>();
            if (!string.IsNullOrEmpty(solicitudCategoriaJson.Usuario))
            {
                listadoCategorias = await _categoriaService.ObtenerCategoriasSeleccionadasRecepcionAsync(solicitudCategoriaJson);
            }

            return new JsonResult(listadoCategorias, _jsonSettings);
        }

        [HttpPost("categorias/recepcion/actualizar")]
        public async Task<IActionResult> GetCategoriasRecepcionActualizar(ActualizacionCategoriaDocumentoJsonModel actualizacionJsonModel)
        {
            var respuesta = new RespuestaCategoriaOficioJsonModel() { Estado = 0 }; 

            if (!string.IsNullOrEmpty(actualizacionJsonModel.Usuario) && !string.IsNullOrEmpty(actualizacionJsonModel.Categorias))
            {
                var result = await _categoriaService.ActualizarRecepcionCategoriaAsync(actualizacionJsonModel);
                respuesta.Estado = result? 1 : 0;
            }

            return new JsonResult(respuesta, _jsonSettings);
        }
    }
}