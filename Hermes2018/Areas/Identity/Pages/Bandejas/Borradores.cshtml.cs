using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Bandejas
{
    [Authorize]
    public class BorradoresModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Bandeja"] = "Borradores";
        }
    }
}