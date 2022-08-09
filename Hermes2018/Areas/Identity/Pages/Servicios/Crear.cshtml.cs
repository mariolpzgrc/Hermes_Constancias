using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Models.Servicio;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Areas.Identity.Pages.Servicios
{
    [Authorize]
    public class CrearModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly IServicioService _servicioService;

        public CrearModel(ApplicationDbContext context, 
            IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService,
            IServicioService servicioService)
        {
            _context = context;
            _usuarioService = usuarioService;
            _usuarioClaimService = usuarioClaimService;
            _servicioService = servicioService;
        }

        [BindProperty]
        public CrearServicioViewModel Crear { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                //Usuario actual
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

                var existe = await _servicioService.ExisteServicioAsync(Crear.NombreServicio, infoUsuarioClaims.UserName);

                if (!existe)
                {
                    var result = await _servicioService.GuardarServiciosAsync(Crear, infoUsuarioClaims.UserName, infoUsuarioClaims.RegionId);
                    if (result)
                    {
                        return RedirectToPage("/Servicios/Index");
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre que usted asigno a este servicio ya se encuentra registrado.");
                }
            }

            return Page();
        }
    }
}