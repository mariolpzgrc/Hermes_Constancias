using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Calendarios
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class DetallesModel : PageModel
    {
        private readonly ICalendarioService _calendarioService;

        public DetallesModel(ICalendarioService calendarioService)
        {
            _calendarioService = calendarioService;
        }

        public InfoCalendarioViewModel Calendario { get; set; }
        public List<ContenidoCalendarioViewModel> Contenido { get; set; }
        
        public async Task OnGetAsync(int id)
        {
            Calendario = await _calendarioService.ObtenerCalendarioAsync(id);
            Contenido = await _calendarioService.ObtenerContenidoCalendarioAsync(id);
        }
    }
}