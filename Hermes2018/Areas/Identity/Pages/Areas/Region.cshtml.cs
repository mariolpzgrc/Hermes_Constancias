using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Models.Area;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Areas
{
    [Authorize(Roles = ConstRol.Rol1T )]
    public class RegionModel : PageModel
    {
        private readonly IRegionService _regionService;

        public RegionModel(IRegionService regionService)
        {
            _regionService = regionService;
        }

        public List<RegionEnAreaViewModel> Regiones { get; set; }

        public async Task OnGetAsync()
        {
            Regiones = await _regionService.ObtenerRegionesEnAreasAsync();
        }
    }
}