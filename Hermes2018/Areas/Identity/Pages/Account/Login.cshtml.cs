using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using Hermes2018.Data;
using Hermes2018.Models;
using System.Security.Claims;
using Hermes2018.Helpers;
using Hermes2018.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Hermes2018.Services;

namespace Hermes2018.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<HER_Usuario> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguracionService _configuracionService;
        private readonly IMailService _mailService;

        public LoginModel(
            SignInManager<HER_Usuario> signInManager, 
            ILogger<LoginModel> logger, 
            IUsuarioService usuarioService,
            IConfiguracionService configuracionService,
            IMailService mailService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _usuarioService = usuarioService;
            _configuracionService = configuracionService;
            _mailService = mailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string UserName { get; set; }

        [TempData]
        public string MensajeRespuesta { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public DateTime Hoy { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
            [Display(Name = "Usuario")]
            public string UserName { get; set; }

            [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
            [Display(Name = "Contraseña")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task OnGet(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            Hoy = DateTime.Now;
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;

            ViewData["Estado"] = ConstEstadoUsuario.Estado1T;
            await _signInManager.SignOutAsync();
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            
            Hoy = DateTime.Now;

            if (ModelState.IsValid)
            {
                var configuration = await _configuracionService.ObtenerInfoConfiguracionLDAPAsync();
                bool useMasterKey;
                bool isInUVDB = false;

                //Aqui se implementea para lo de la llave maestra.
                useMasterKey = (Input.Password == ConstMasterKey.Key2) ? true : false;
                if (useMasterKey)
                {
                    isInUVDB = true;
                }
                else
                {
                    //Aqui se realiza de usuarios con el usuarios con el directorio activo
                    using (var context = new PrincipalContext(ContextType.Domain, configuration.HER_IPLDAP, Input.UserName, Input.Password))
                    {
                        if (context.ValidateCredentials(Input.UserName, Input.Password))
                        {
                            isInUVDB = true;
                        }
                    }
                }

                bool esAdmin = await _usuarioService.EsTipoAdministradorAsync(Input.UserName);
                bool isInLocalDB = await _usuarioService.ExisteSoloUsuarioAsync(Input.UserName, esAdmin);
                bool usuarioActivo;
                bool tieneCuentaDependencia = false;

                if (isInLocalDB && isInUVDB)
                {
                    var user = await _usuarioService.ObtenerUsuarioAsync(Input.UserName);

                    if (user.HER_Aprobado)
                    {
                        usuarioActivo = await _usuarioService.ExisteUsuarioActivoAsync(Input.UserName);

                        if (usuarioActivo)
                        {
                            if (!esAdmin) 
                            {
                                tieneCuentaDependencia = await _usuarioService.DetectaCuentaDependenciaAsync(Input.UserName);
                            }
                            //--
                            if (!tieneCuentaDependencia)
                            {
                                var result = await _signInManager.PasswordSignInAsync(user.UserName, string.Format("{0}{1}", ConstMasterKey.Key1, user.UserName), false, lockoutOnFailure: true);
                                if (result.Succeeded)
                                {
                                    if (user.HER_AceptoTerminos)
                                    {
                                        ViewData["Estado"] = ConstEstadoUsuario.Estado7T;

                                        _logger.LogInformation("User logged in.");
                                        return LocalRedirect(returnUrl);
                                    }
                                    else
                                    {
                                        ViewData["Estado"] = ConstEstadoUsuario.Estado5T;

                                        _logger.LogInformation("Redirect.");
                                        return RedirectToPage("TerminosCondiciones");
                                    }
                                }
                            }
                            else {
                                ViewData["Estado"] = ConstEstadoUsuario.Estado8T;
                                ViewData["WarningPass"] = "Usted tiene una cuenta activa como titular, por el momento no puede ingresar con una cuenta personal.";
                                return Page();
                            }
                        }
                        else {
                            ViewData["Estado"] = ConstEstadoUsuario.Estado6T;
                            ViewData["WarningPass"] = "Su cuenta no esta asiganda a un área. Comuníquese con el administrador para poder acceder al sistema.";
                            return Page();
                        }
                    }
                    else
                    {
                        ViewData["Estado"] = ConstEstadoUsuario.Estado4T;
                        ViewData["WarningPass"] = "Su cuenta aún no es aprobada. Cuando esto pase se le notificará por correo y entonces podrá acceder al sistema.";
                        return Page();
                    }
                }
                else if (!isInLocalDB && isInUVDB)
                {
                    ViewData["Estado"] = ConstEstadoUsuario.Estado3T;
                    ViewData["Warning"] = "Sus datos no se encuentran en el sistema. Puede enviar su solicitud dando clic ";
                    UserName = Input.UserName;

                    return Page();
                }
                else
                {
                    ViewData["Estado"] = ConstEstadoUsuario.Estado2T;
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña no válidos.");

                    return Page();
                }
            }

            ViewData["Estado"] = ConstEstadoUsuario.Estado2T;
            ModelState.AddModelError(string.Empty, "Usuario o contraseña no válidos.");

            return Page();
        }

    }
}
