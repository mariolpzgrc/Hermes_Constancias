using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Bandejas
{
    [Authorize]
    public class RecibidosModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IDocumentoService _documentoService;

        public RecibidosModel(IUsuarioClaimService usuarioClaimService, 
            IDocumentoService documentoService)
        {
            _usuarioClaimService = usuarioClaimService;
            _documentoService = documentoService;
        }

        public int Pagina { get; set; }
        [BindProperty]
        public bool enCarpeta { get; set; }
        [BindProperty]
        public InfoDelegarUsuarioViewModel InfoDelegar { get; set; }

        public async Task OnGetAsync()
        {
            var infoUsuario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            //--
            InfoDelegar = new InfoDelegarUsuarioViewModel()
            {
                EstaActiva = infoUsuario.ActivaDelegacion,
                TipoPermiso = (infoUsuario.ActivaDelegacion) ? infoUsuario.BandejaPermiso : 0
            };

            //--Actualizar el estado de los documentos que ya han vencido
            await _documentoService.ActualizaEstadoDocumentoRecibidoAsync(infoUsuario.BandejaUsuario);

            ViewData["Bandeja"] = "Recibidos";
        }
    }
}