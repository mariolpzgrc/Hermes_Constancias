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

namespace Hermes2018.Areas.Identity.Pages.Recopilacion.General
{
    [Authorize(Roles = ConstRol.Rol1T)]
    public class RegionModel : PageModel
    {
        public readonly IRecopilacionService _recopilacionService;

        public RegionModel(IRecopilacionService recopilacionService)
        {
            _recopilacionService = recopilacionService;
        }

        public List<RecopilacionRegionViewModel> Regiones { get; set; }

        public async Task OnGetAsync()
        {
            Regiones = await _recopilacionService.ObtenerRecopilacionRegionesAsync();
        }
    }
}
