using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Areas.Identity.Pages.Servicios
{
    [Authorize]
    public class DetallesModel : PageModel
    {
        private readonly IServicioService _servicioService;

        public DetallesModel(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        [BindProperty]
        public DetalleServicioViewModel Detalle { get; set; }

        public async Task OnGetAsync(int id)
        {   
            Detalle = await _servicioService.ObtenerDetalleServicioAsync(id);
        }
    }
}