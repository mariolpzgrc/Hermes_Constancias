using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Constancias
{
    [Authorize]
    public class SolicitudConstanciaModel : PageModel
    {
        [BindProperty]
        public int TipoConstancia { get; set; }

        private readonly IUsuarioClaimService _usuarioClaimService;
       
        public SolicitudConstanciaModel(IUsuarioClaimService usuarioClaimService)
        {
            _usuarioClaimService = usuarioClaimService;
        }

        public void OnPostSummit(int tipo)
        {
            TipoConstancia = tipo;
            Console.WriteLine(TipoConstancia);
        }
    }
}
