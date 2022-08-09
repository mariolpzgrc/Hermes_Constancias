using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Hermes2018.Data;
using Hermes2018.Models;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Hermes2018.Areas.Identity.Pages.Account
{
    [Authorize]
    public class TerminosCondicionesModel : PageModel
    {
        private readonly SignInManager<HER_Usuario> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguracionService _configuracionService;
        private readonly IUsuarioService _usuarioService;

        public TerminosCondicionesModel(SignInManager<HER_Usuario> signInManager, 
            ILogger<LoginModel> logger,
            IConfiguracionService configuracionService,
            IUsuarioService usuarioService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _configuracionService = configuracionService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public TerminosViewModel Terminos { get; set; }

        [BindProperty]
        public InfoConfiguracionAvisoPrivacidadViewModel Aviso { get; set; }
        
        public async Task OnGetAsync()
        {
            Aviso = await _configuracionService.ObtenerInfoConfiguracionAvisoPrivacidadAsync();
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (Terminos.Aceptar)
                {
                    var result = await _usuarioService.GuardarAceptacionTerminos(User.Identity.Name);

                    if (result)
                    {
                        return LocalRedirect(returnUrl);
                    }
                }
            }
            //--Carga de nuevo
            Aviso = await _configuracionService.ObtenerInfoConfiguracionAvisoPrivacidadAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostCancelar(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}