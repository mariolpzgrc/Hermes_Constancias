using Hermes2018.Data;
using Hermes2018.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hermes2018.Areas.Identity.Pages.Estadisticas
{
    [Authorize]
    public class EstadisticasModel : PageModel
    {       
        public void OnGet()
        {
            
        }
    }
}