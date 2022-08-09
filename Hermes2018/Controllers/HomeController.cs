using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hermes2018.Models;
using Microsoft.AspNetCore.Authorization;
using Hermes2018.Helpers;
using Hermes2018.ViewComponentsModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Hermes2018.Data;
using Microsoft.EntityFrameworkCore;
using Hermes2018.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Cors;

namespace Hermes2018.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SignInManager<HER_Usuario> _signInManager;
        private readonly UserManager<HER_Usuario> _userManager;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private ApplicationDbContext _context;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly IHerramientaService _herramientaService;

        public HomeController(SignInManager<HER_Usuario> signInManager,
            UserManager<HER_Usuario> userManager,
            IUsuarioClaimService usuarioClaimService,
            ApplicationDbContext context,
            IHerramientaService herramientaService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _usuarioClaimService = usuarioClaimService;
            _context = context;
            _herramientaService = herramientaService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }
        
        [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T + "," + ConstRol.Rol7T + "," + ConstRol.Rol8T )]
        public IActionResult Index()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            var regionId = 1;

            if (ConstRol.RolAdmin.Contains(infoUsuarioClaims.Rol))
            {
                if (ConstRol.RolAdminRegional.Contains(infoUsuarioClaims.Rol))
                {
                    regionId = infoUsuarioClaims.RegionId;
                }

                ViewData["Rol"] = infoUsuarioClaims.Rol;
                ViewData["Region"] = regionId;

                return View(); 
            }
            else
            {
                return RedirectToPage("/Bandejas/Recibidos", new { Area= "Identity"});
            }
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
        public async Task<IActionResult> Cambiar(CambiarBandejaViewComponentModel viewcomponentmodel)
        {
            if (ModelState.IsValid)
            {
                var result = 0;

                var infoUsuarioQuery = _context.HER_InfoUsuario
                    .Where(x => x.HER_Usuario.UserName == User.Identity.Name
                        && x.HER_Activo == true)
                    .AsQueryable();

                //Usuario actual
                var infoUsuario = await infoUsuarioQuery.FirstOrDefaultAsync();

                if (infoUsuario != null)
                {
                    if (infoUsuario.HER_UserName == viewcomponentmodel.Usuario)
                    {
                        infoUsuario.HER_BandejaUsuario = infoUsuario.HER_UserName;
                        infoUsuario.HER_BandejaPermiso = ConstDelegar.TipoN1;
                        infoUsuario.HER_BandejaNombre = infoUsuario.HER_NombreCompleto;

                        _context.HER_InfoUsuario.Update(infoUsuario);
                        result = await _context.SaveChangesAsync();

                        if (result > 0)
                        {
                            //Actualiza los claims del usuario
                            var user = await _userManager.FindByNameAsync(User.Identity.Name);
                            await _signInManager.RefreshSignInAsync(user);
                        }
                    }
                    else {
                        //--
                        if (infoUsuario.HER_Titular == viewcomponentmodel.Usuario)
                        {
                            //Actualizamos
                            infoUsuario.HER_BandejaUsuario = viewcomponentmodel.Usuario;
                            infoUsuario.HER_BandejaPermiso = ConstDelegar.TipoN2;
                            infoUsuario.HER_BandejaNombre = infoUsuario.HER_NombreCompleto;

                            _context.HER_InfoUsuario.Update(infoUsuario);
                            result = await _context.SaveChangesAsync();

                            if (result > 0)
                            {
                                //Actualiza los claims del usuario
                                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                                await _signInManager.RefreshSignInAsync(user);
                            }
                        }
                        else {
                            //Titular elegido al que delegaron la bandeja
                            var infoNuevoQuery = _context.HER_Delegar
                                            //.Include(x => x.HER_Titular)
                                            .Where(x => x.HER_Titular.HER_UserName == viewcomponentmodel.Usuario
                                                     && x.HER_Titular.HER_Activo == true
                                                     && x.HER_DelegadoId == infoUsuario.HER_InfoUsuarioId)
                                            .Select(x => new {
                                                Usuario = x.HER_Titular.HER_UserName,
                                                Permiso = x.HER_Tipo,
                                                NombreCompleto = x.HER_Titular.HER_NombreCompleto
                                            })
                                            .AsQueryable();

                            var infoNuevo = await infoNuevoQuery.FirstOrDefaultAsync();

                            if (infoNuevo != null)
                            {
                                //Actualizamos
                                infoUsuario.HER_BandejaUsuario = infoNuevo.Usuario;
                                infoUsuario.HER_BandejaPermiso = infoNuevo.Permiso;
                                infoUsuario.HER_BandejaNombre = infoNuevo.NombreCompleto;

                                _context.HER_InfoUsuario.Update(infoUsuario);
                                result = await _context.SaveChangesAsync();

                                if (result > 0)
                                {
                                    //Actualiza los claims del usuario
                                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                                    await _signInManager.RefreshSignInAsync(user);
                                }
                            }
                        }
                    }
                }
            }

            var rutaOrigen =  Request.Headers["Referer"].ToString();
            if (!rutaOrigen.Contains("Bandejas/Personales"))
            {
                return Redirect(rutaOrigen); //Origen
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost, ActionName("Anonimo")]
        [AllowAnonymous]
        public IActionResult TokenWebApiAnonimo(TokenApiAnonimoJsonModel model)
        {
            var info = _herramientaService.ConstruirToken(model.Usuario, 1);
            //--
            return new JsonResult(new { estado = info.Token }, _jsonSettings);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = ConstRol.Rol1T + "," + ConstRol.Rol2T + "," + ConstRol.Rol3T + "," + ConstRol.Rol4T + "," + ConstRol.Rol5T + "," + ConstRol.Rol6T + "," + ConstRol.Rol7T + "," + ConstRol.Rol8T )]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
