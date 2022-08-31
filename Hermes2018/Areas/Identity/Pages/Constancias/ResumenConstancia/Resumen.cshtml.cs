using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Constancias.ResumenConstancia
{

    public enum Personal
    {
        Funcionario = 1,
        ATM = 2,
        Confianza = 3,
        Academico = 4, 
        Eventual = 5
    }
    [Authorize]
    public class ResumenModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly CultureInfo _cultureEs;
        public ResumenModel(
            IUsuarioService usuarioService,
            IUsuarioClaimService usuarioClaimService)
        {
            _usuarioClaimService = usuarioClaimService;
            _cultureEs = new CultureInfo("es-MX");
        }
        [BindProperty]
        public int ValueTipoConstancia { get; set; }
        [BindProperty]
        public int ValueTipoPersonal { get; set; }
        public int Tipo { get; set; }
        public int TipoPersonal { get; set; }

        public void OnGet(int tipo, int tipoPersonal)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuario = infoUsuarioClaims.UserName;
            Tipo = tipo;
            TipoPersonal = tipoPersonal;

        }
    }
}
