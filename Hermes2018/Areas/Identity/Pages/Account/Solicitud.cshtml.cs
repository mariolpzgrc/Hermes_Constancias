using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hermes2018.Areas.Identity.Pages.Account
{
    public class SolicitudModel : PageModel
    {
        private readonly IRegionService _regionService;
        private readonly IUsuarioService _usuarioService;
        private readonly IAreaService _areaService;
        private readonly IConfiguracionService _configuracionService;
        private readonly IMailService _mailService;
        private readonly IOracleService _oracleService;

        public SolicitudModel(
            IRegionService regionService,
            IUsuarioService usuarioService,
            IAreaService areaService,
            IConfiguracionService configuracionService,
            IMailService mailService,
            IOracleService oracleService)
        {
            _regionService = regionService;
            _usuarioService = usuarioService;
            _areaService = areaService;
            _configuracionService = configuracionService;
            _mailService = mailService;
            _oracleService = oracleService;
        }

        [TempData]
        public string UserName { get; set; }

        [TempData]
        public string MensajeRespuesta { get; set; }

        [BindProperty]
        public InfoUsuarioOracleViewModel Solicitud { get; set; }

        public SelectList Areas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (TempData.ContainsKey("UserName"))
            {
                var returnUrl = Url.Content("~/");
                Solicitud = await _oracleService.ObtieneInfoUsuarioOracleAsync(UserName);
                //--
                if (Solicitud != null)
                {
                    Areas = new SelectList(Solicitud.Areas, "AreaId", "Nombre", null, "Region");

                    return Page();
                }
                else {
                    MensajeRespuesta = "El usuario no se encuentra disponible.";
                    return LocalRedirect(returnUrl);
                }
            }
            else
            {
                var returnUrl = Url.Content("~/");
                Solicitud = await _oracleService.ObtieneInfoUsuarioOracleAsync("hbonola");
                //--
                if (Solicitud != null) {
                    Areas = new SelectList(Solicitud.Areas, "AreaId", "Nombre", null, "Region");

                    return Page();
                }
                else
                {
                    MensajeRespuesta = "El usuario no se encuentra disponible.";
                    return LocalRedirect(returnUrl);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                bool esAdmin = await _usuarioService.EsTipoAdministradorAsync(Solicitud.Usuario);
                //--
                var existeUsuario = await _usuarioService.ExisteSoloUsuarioAsync(Solicitud.Usuario, esAdmin);
                var existeInfoUsuario = await _usuarioService.ExisteSoloInfoUsuarioAsync(Solicitud.Usuario);
                var estaActivo = await _usuarioService.ExisteUsuarioActivoAsync(Solicitud.Usuario);
                var cuerpo = string.Empty;

                //Si no existe, se guarda
                if ((!existeUsuario && !existeInfoUsuario) || (existeUsuario && existeInfoUsuario && !estaActivo))
                {
                    //var usuarioTitularId = await _usuarioService.ObtenerIdUsuarioTitularAsync(int.Parse(Solicitud.AreaId));
                    var area = await _areaService.ObtenerAreaPorIdVisibleAsync(int.Parse(Solicitud.AreaId));
                    var result = await _usuarioService.GuardarUsuarioConSolicitudAsync(existeUsuario, existeInfoUsuario, estaActivo, Solicitud, area.HER_Direccion, area.HER_Telefono);

                    if (result)
                    {
                        var titular = await _areaService.ObtenerTitularConAreaVisible(int.Parse(Solicitud.AreaId));
                        if (titular != null)
                        {
                            //Envia un correo al usuario que solicito el registro
                            cuerpo = string.Format(_configuracionService.ObtenerPlantillaSolicitudParaSolicitante(),
                               Solicitud.Nombre,
                               Solicitud.Usuario,
                               Solicitud.Correo,
                               await _regionService.ObtenerNombreRegionPorAreaIdAsync(int.Parse(Solicitud.AreaId)),
                               await _areaService.ObtenerNombreAreaVisibleAsync(int.Parse(Solicitud.AreaId)),
                               Solicitud.Puesto,
                               titular.NombreCompleto,
                               titular.Correo);

                            await _mailService.EnviarCorreo(new string[] { Solicitud.Correo }, null, null, ConstPlantillaCorreo.AsuntoT4, cuerpo);

                            //Envia un correo al titular
                            cuerpo = string.Format(_configuracionService.ObtenerPlantillaSolicitudParaTitular(),
                                Solicitud.Nombre,
                                Solicitud.Usuario,
                                Solicitud.Correo,
                                await _regionService.ObtenerNombreRegionPorAreaIdAsync(int.Parse(Solicitud.AreaId)),
                                await _areaService.ObtenerNombreAreaVisibleAsync(int.Parse(Solicitud.AreaId)),
                                Solicitud.Puesto);

                            await _mailService.EnviarCorreo(new string[] { titular.Correo }, null, null, ConstPlantillaCorreo.AsuntoT3, cuerpo);
                        }

                        MensajeRespuesta = "Su solicitud ha sido enviada, se le enviará un correo con la respuesta.";
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        MensajeRespuesta = "Su solicitud de registro no ha podido ser procesada, intentelo más tarde.";
                        return LocalRedirect(returnUrl);
                    }
                }
            else
            {
                MensajeRespuesta = "Su solicitud de registro no ha podido ser procesada, el usuario ya existe en el sistema.";
                return LocalRedirect(returnUrl);
            }
        }

            return Page();
        }
    }
}
