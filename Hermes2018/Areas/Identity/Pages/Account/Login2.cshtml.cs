using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;

namespace Hermes2018.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class Login2Model : PageModel
    {
        private readonly SignInManager<HER_Usuario> _signInManager;
        private readonly ILogger<Login2Model> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguracionService _configuracionService;

        public Login2Model(SignInManager<HER_Usuario> signInManager,
            ILogger<Login2Model> logger,
            IUsuarioService usuarioService,
            IConfiguracionService configuracionService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _usuarioService = usuarioService;
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public bool AceptoTerminos { get; set; }

        [BindProperty]
        public bool ConAcceso { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index","Home", new { Area = "", Id = ""});
        }

        public async Task<IActionResult> OnPost()
        {
            await _signInManager.SignOutAsync();

            bool isInUVDB = true;
            bool usuarioActivo;
            bool tieneCuentaDependencia = false;
            string usuario = string.Empty;
            string valor = string.Empty;

            ConAcceso = false;
            AceptoTerminos = false;

            try
            {
                valor = Request.Form["sLogin"];

                if (!string.IsNullOrEmpty(valor))
                {
                    WSDatos.wsDesencriptaDatosRequest inValue = new WSDatos.wsDesencriptaDatosRequest { Body = new WSDatos.wsDesencriptaDatosRequestBody { sDatos = valor } };
                    WSDatos.wsDesencriptaDatosResponse retVal = await ((WSDatos.WSEncriptaDatosSoap)(new WSDatos.WSEncriptaDatosSoapClient(WSDatos.WSEncriptaDatosSoapClient.EndpointConfiguration.WSEncriptaDatosSoap))).wsDesencriptaDatosAsync(inValue);
                    usuario = retVal.Body.wsDesencriptaDatosResult;

                    bool esAdmin = await _usuarioService.EsTipoAdministradorAsync(usuario);
                    bool isInLocalDB = await _usuarioService.ExisteSoloUsuarioAsync(usuario, esAdmin);

                    if (isInLocalDB && isInUVDB)
                    {
                        var user = await _usuarioService.ObtenerUsuarioAsync(usuario);
                        
                        if (user.HER_Aprobado)
                        {
                            usuarioActivo = await _usuarioService.ExisteUsuarioActivoAsync(usuario);

                            if (usuarioActivo)
                            {
                                if (!esAdmin)
                                {
                                    tieneCuentaDependencia = await _usuarioService.DetectaCuentaDependenciaAsync(usuario);
                                }
                                //--
                                if (!tieneCuentaDependencia)
                                {
                                    var result = await _signInManager.PasswordSignInAsync(user, string.Format("{0}{1}", ConstMasterKey.Key1, user.UserName), false, lockoutOnFailure: false);

                                    if (result.Succeeded)
                                    {
                                        AceptoTerminos = user.HER_AceptoTerminos;
                                        ConAcceso = true;

                                        return Page();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Error catch: " + ex.Message);
            }

            return Page();
        }
    }
}
