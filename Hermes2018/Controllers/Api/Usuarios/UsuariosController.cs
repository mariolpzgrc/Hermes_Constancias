using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
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

namespace Hermes2018.Controllers.Api.Usuarios
{
	[ApiController]
	public class UsuariosController : ApiController
	{
        private readonly IUsuarioService _usuarioService;
        private readonly JsonSerializerSettings _jsonSettings;

        public UsuariosController(IUsuarioService usuarioService)
		{
            _usuarioService = usuarioService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

		[HttpGet("users/ldap/{keyword?}")]
		public async Task<IActionResult> GetUserDataAsync(string keyword)
		{
            List<UsuarioADViewModel> usuarios = await _usuarioService.BusquedaUsuariosDirectorioActivoAsync(keyword);

            return new JsonResult(usuarios, _jsonSettings);
		}

        [HttpGet("users/local/{keyword?}")]
        public async Task<IActionResult> GetLocalUserDataAsync(string keyword)
        {
            List<UsuarioLocalJsonModel> usuarios = await _usuarioService.BusquedaUsuariosLocalesAsync(keyword);

            return new JsonResult(usuarios, _jsonSettings);
        }

        [HttpPost("users/local/coleccion")]
        public async Task<IActionResult> GetLocalUsersDataAsync(UsuariosLocalesColeccionJsonModel modelo)
        {
            var lista = modelo.usuarios.Split(',').ToList();

            List<UsuarioLocalJsonModel> resultado = await _usuarioService.BusquedaUsuariosLocalesColeccionAsync(lista);

            return new JsonResult(resultado, _jsonSettings);
        }

        [HttpGet("users/local/info/{usercurrent}/{keyword?}")]
        public async Task<IActionResult> GetLocalUserInfoAsync(string usercurrent, string keyword)
        {
            List<UsuarioLocalJsonModel> usuarios = await _usuarioService.BusquedaUsuariosLocalesDelegarAsync(usercurrent, keyword);

            return new JsonResult(usuarios, _jsonSettings);
        }

        [HttpGet("users/revision/{usercurrent}/{areaid}/{keyword?}")]
        public async Task<IActionResult> GetLocalUsersRevisionAsync(string usercurrent, int areaid, string keyword)
        {
            List<UsuarioLocalJsonModel> usuarios = await _usuarioService.BusquedaUsuariosLocalesRevisionAsync(usercurrent, areaid, keyword);

            return new JsonResult(usuarios, _jsonSettings);
        }

        [HttpGet("users/reasignacion")]
        public async Task<IActionResult> GetUsuariosEnReasignacionAsync([FromQuery] bool contain, [FromQuery] string selected, [FromQuery] string original, [FromQuery] string keyword)
        {
            string busqueda = string.IsNullOrEmpty(keyword) ? string.IsNullOrEmpty(original) ? string.Empty : original : keyword;

            var usuarios = await _usuarioService.BusquedaUsuariosLocalesReasignacionAsync(busqueda);

            return new JsonResult(usuarios, _jsonSettings);
        }

        [HttpGet("users/listactive")]
        public async Task<IActionResult> GetUserActiveAsync([FromQuery] bool contain, [FromQuery] string selected, [FromQuery] string original, [FromQuery] string keyword)
        {
            string busqueda = string.IsNullOrEmpty(keyword) ? string.IsNullOrEmpty(original) ? string.Empty : original : keyword;

            List<UsuarioADViewModel> usuarios = await _usuarioService.BusquedaUsuariosDirectorioActivoAsync(busqueda);

            return new JsonResult(await _usuarioService.LimpiarBusquedaUsuariosDirectorioActivoAsync(usuarios), _jsonSettings);
        }

        [HttpGet("users/delegar/{usercurrent}")]
        public async Task<IActionResult> GetLocalUserDelegarAsync(string usercurrent, [FromQuery] bool contain, [FromQuery] string selected, [FromQuery] string original, [FromQuery] string keyword)
        {
            string busqueda = string.IsNullOrEmpty(keyword) ? string.IsNullOrEmpty(original) ? string.Empty : original : keyword;

            List<UsuarioLocalJsonModel> usuarios = await _usuarioService.BusquedaUsuariosLocalesDelegarAsync(usercurrent, busqueda);

            return new JsonResult(usuarios, _jsonSettings);
        }

        [HttpGet("users/busqueda/local/{usercurrent}")]
        public async Task<IActionResult> GetLocalUsersAsync(string usercurrent, [FromQuery] bool contain, [FromQuery] string selected, [FromQuery] string original, [FromQuery] string keyword)
        {
            string busqueda = string.IsNullOrEmpty(keyword) ? string.IsNullOrEmpty(original) ? string.Empty : original : keyword;

            List<UsuarioLocalJsonModel> usuarios = await _usuarioService.BusquedaUsuariosLocalesAsync(busqueda, usercurrent);

            return new JsonResult(usuarios, _jsonSettings);
        }

        //[HttpGet("users/buscar")]
        //public async Task<IActionResult> GetLocalUserBuscarDataAsync(string keyword)
        //{
        //    List<UsuarioLocalJsonModel> usuarios = await _usuarioService.BusquedaUsuariosLocalesBuscarAsync(keyword);

        //    return new JsonResult(usuarios, _jsonSettings);
        //}
        //[HttpGet("users/buscar")]
        //public async Task<IActionResult> GetLocalUserBuscarDataAsync([FromQuery] bool contain, [FromQuery] string selected, [FromQuery] string original, [FromQuery] string keyword)
        //{
        //    string busqueda = string.IsNullOrEmpty(keyword) ? string.IsNullOrEmpty(original) ? string.Empty : original : keyword;

        //    List<UsuariosDetallesViewModel> usuarios = await _usuarioService.BusquedaUsuariosLocalesBuscarAsync(busqueda);

        //    return new JsonResult(await _usuarioService.LimpiarBusquedaUsuariosBuscarAsync(usuarios), _jsonSettings); 
        //}
        [HttpGet("users/buscar")]
        public async Task<IActionResult> GetLocalUserBuscarDataAsync([FromQuery] bool contain, [FromQuery] string selected, [FromQuery] string original, [FromQuery] string keyword)
        {
            string busqueda = string.IsNullOrEmpty(keyword) ? string.IsNullOrEmpty(original) ? string.Empty : original : keyword;

            List<UsuariosBuscarViewModel> usuarios = await _usuarioService.BusquedaUsuariosLocalesBuscarAsync(busqueda);

            return new JsonResult(usuarios, _jsonSettings);
        }

    }
}
