using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Controllers
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class SolicitudesController : Controller
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly ISolicitudesService _solicitudesService;
        private readonly IAreaService _areaService;

        private readonly IMailService _mailService;
        private readonly IConfiguracionService _configuracionService;
        private readonly CultureInfo _cultureEs;
        
        public SolicitudesController(
            IUsuarioClaimService usuarioClaimService,
            ISolicitudesService solicitudesService,
            IAreaService areaService,
            IMailService mailService,
            IConfiguracionService configuracionService)
        {
            _usuarioClaimService = usuarioClaimService;
            _solicitudesService = solicitudesService;
            _areaService = areaService;
            _mailService = mailService;
            _configuracionService = configuracionService;
            _cultureEs = new CultureInfo("es-MX");
        }

        public async Task<IActionResult> Index()
        {
            //return NotFound();
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            if (!infoUsuario.PermisoAA) 
            {
                return NotFound();
            }
            //--
            var viewModel = new SolicitudViewModel()
            {
                InfoUsuarioClaims = infoUsuario,
            };

            viewModel.Solicitudes = await _solicitudesService.ObtenerSolicitudesAsync(viewModel.InfoUsuarioClaims.AreaId);

            return View(viewModel);
        }

        public async Task<IActionResult> Detalles(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            //Obtiene la informacion del usuario (Que ha iniciado la sesión)
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (!infoUsuarioClaims.PermisoAA)
            {
                return NotFound();
            }

            var existe = await _solicitudesService.ExisteSolicitudUsuarioAsync(id, infoUsuarioClaims.AreaId);

            //Valida si existe
            if (!existe)
            {
                return NotFound();
            }

            return View(await _solicitudesService.ObtenerDetalleSolicitudAsync(id, infoUsuarioClaims.AreaId));
        }

        public async Task<IActionResult> Responder(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            //Obtiene la informacion del usuario (Que ha iniciado la sesión)
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (!infoUsuarioClaims.PermisoAA)
            {
                return NotFound();
            }

            var existe = await _solicitudesService.ExisteSolicitudUsuarioAsync(id, infoUsuarioClaims.AreaId);
            
            //Valida si existe
            if (!existe)
            {
                return NotFound();
            }

            var viewModel = await _solicitudesService.ObtenerResumenResponderAsync(id, infoUsuarioClaims.AreaId);
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Responder(SolicitudResponderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var existe = await _solicitudesService.ExisteSolicitudUsuarioAsync(viewModel.Usuario, int.Parse(viewModel.AreaId));
                if (!existe)
                {
                    return NotFound();
                }

                var result = await _solicitudesService.ResponderSolicitudAsync(viewModel);
                if (result)
                {
                    var titular = await _areaService.ObtenerTitularConAreaVisible(int.Parse(viewModel.AreaId));
                    var area = await _areaService.ObtenerAreaConRegionPorIdAsync(int.Parse(viewModel.AreaId));

                    if (int.Parse(viewModel.Aprobar) == ConstAprobado.AprobadoSiN)
                    {
                        if (titular != null)
                        {
                            //Envio de correo
                            var cuerpo = string.Format(_configuracionService.ObtenerPlantillaSolicitudAceptada(),
                                "Aceptada",
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                                area.HER_Region.HER_Nombre, //viewModel.Region
                                area.HER_Nombre,//viewModel.Area
                                viewModel.Puesto, //viewModel.Puesto
                                viewModel.Direccion,
                                viewModel.Telefono,
                                titular.NombreCompleto,
                                titular.Correo);

                            await _mailService.EnviarCorreo(new string[] { titular.Correo }, null, null, ConstPlantillaCorreo.AsuntoT5, cuerpo);
                        }
                    }
                    else if (int.Parse(viewModel.Aprobar) == ConstAprobado.AprobadoNoN)
                    {
                        //Envio de correo
                        if (titular != null)
                        {
                            //Envio de correo
                            var cuerpo = string.Format(_configuracionService.ObtenerPlantillaSolicitudRechazada(),
                                "Rechazada",
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                                viewModel.Comentario,
                                area.HER_Region.HER_Nombre,
                                area.HER_Nombre,
                                viewModel.Puesto,
                                viewModel.Direccion,
                                viewModel.Telefono,
                                titular.NombreCompleto,
                                titular.Correo);

                            await _mailService.EnviarCorreo(new string[] { titular.Correo }, null, null, ConstPlantillaCorreo.AsuntoT6, cuerpo);
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                else {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                }
            }

            return View(viewModel);
        }
    }
}