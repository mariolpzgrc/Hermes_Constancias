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

namespace Hermes2018.Areas.Identity.Pages.Configuracion.General
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class IndexModel : PageModel
    {
        private readonly IConfiguracionService _configuracionService;

        public IndexModel(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        public DetalleConfiguracion Detalle { get; set; }

        public async Task OnGetAsync() 
        {
            Detalle = await _configuracionService.ObtenerDetalleConfiguracionGeneralAsync();
        }
    }
}