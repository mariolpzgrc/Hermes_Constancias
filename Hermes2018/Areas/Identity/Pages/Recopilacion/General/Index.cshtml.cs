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
    public class IndexModel : PageModel
    {
        public readonly IRecopilacionService _recopilacionService;

        public IndexModel(IRecopilacionService recopilacionService)
        {
            _recopilacionService = recopilacionService;
        }

        public RecopilacionGeneralViewModel General { get; set; }

        public async Task OnGetAsync()
        {
            General = await _recopilacionService.ObtenerRecopilacionGeneralAsync();
        }
    }
}
