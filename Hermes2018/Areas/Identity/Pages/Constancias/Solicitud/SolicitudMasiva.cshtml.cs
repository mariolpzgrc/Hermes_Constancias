using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Constancias.Solicitud
{
    [Authorize]
    public class SolicitudMasivaModel : PageModel
    {

        public void OnGet()
        {
        }
    }
}
