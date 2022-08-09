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

namespace Hermes2018.Areas.Identity.Pages.Correspondencia
{
    [Authorize]
    public class VisualizacionModel : PageModel
    {
        private readonly IDocumentoService _documentoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IMailService _mailService;
        private readonly IDescargarPDF _descargarPDFService;

        public VisualizacionModel(IDocumentoService documentoService,
                           IUsuarioService usuarioService,
                           IUsuarioClaimService usuarioClaimService,
                           IMailService mailService,
                           IDescargarPDF descargarPDFService)
        {
            _usuarioService = usuarioService;
            _usuarioClaimService = usuarioClaimService;
            _documentoService = documentoService;
            _mailService = mailService;
            _descargarPDFService = descargarPDFService;


        }

        [BindProperty]
        public DocumentoEnviadoVisualizacionViewModel Envio { get; set; }
        [BindProperty]
        public List<ListadoHistorialCorreo> ListadoHistorialCorreo { get; set; }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public EnvioCorreoLecturaViewModel EnvioCorreo { get; set; }

        public async Task OnGetAsync(int id, int tipo, string usuario = "")
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            string usuarioCreador = infoUsuarioClaims.UserName;

            Username = usuario;

            IdentificadorUsuarioCompuestoViewModel titular;
            IdentificadorUsuarioCompuestoViewModel creador;

            if (tipo == ConstTipoEnvio.TipoEnvioN1)
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    //Información del oficio (Envio)
                    Envio = await _documentoService.ObtenerDocumentoEnviadoSoloVisualizacionAsync(id, usuario);
                }
                //historial correo
                ListadoHistorialCorreo = await _documentoService.ObtenerListadoHistorialAsync(Envio.Actual_EnvioId, tipo, Envio.Actual_Visualizacion_Tipo);
            }
            else if (tipo == ConstTipoEnvio.TipoEnvioN2)
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    //Información del documento 
                    Envio = await _documentoService.ObtenerDocumentoTurnadoSoloVisualizacionAsync(id, usuario);
                }
                //historial correo
                ListadoHistorialCorreo = await _documentoService.ObtenerListadoHistorialAsync(Envio.Actual_EnvioId, tipo, Envio.Actual_Visualizacion_Tipo);
            }
            else if (tipo == ConstTipoEnvio.TipoEnvioN3 || tipo == ConstTipoEnvio.TipoEnvioN4)
            {
                if (!string.IsNullOrEmpty(usuario))
                {
                    //Información del oficio respuesta
                    Envio = await _documentoService.ObtenerDocumentoRespuestaEnviadoSoloVisualizacionAsync(id, usuario);
                }
                //historial correo
                ListadoHistorialCorreo = await _documentoService.ObtenerListadoHistorialAsync(Envio.Actual_EnvioId, tipo, Envio.Actual_Visualizacion_Tipo);
            }

            EnvioCorreo = new EnvioCorreoLecturaViewModel()
            {
                EnvioId = Envio.Actual_EnvioId,
                Tipo = Envio.Actual_Visualizacion_Tipo,
                TipoEnvio = Envio.Actual_TipoEnvioId,
            };

        }
        public async Task<IActionResult> OnPost()
        {
            var usuarioRedirect = Username;
            //---
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            string usuarioCreador = infoUsuarioClaims.UserName;
            DateTime date = DateTime.Now;

            IdentificadorUsuarioCompuestoViewModel titular;
            IdentificadorUsuarioCompuestoViewModel creador;

            if (usuarioTitular == usuarioCreador)
            {
                titular = await _usuarioService.ObtenerIdentificadorCompuestoUsuarioAsync(usuarioTitular);
                creador = titular;
            }
            else
            {
                titular = await _usuarioService.ObtenerIdentificadorCompuestoUsuarioAsync(usuarioTitular);
                creador = await _usuarioService.ObtenerIdentificadorCompuestoUsuarioAsync(usuarioCreador);
            }

            if (ModelState.IsValid)
            {
                //Envía correo
                await _mailService.EnviarCorreoEspecialLectura(usuarioTitular, EnvioCorreo.Correo, EnvioCorreo.Adjuntar,
                    await _documentoService.ObtenerParaCorreoDocumentoEnviadoAsync(EnvioCorreo.EnvioId));

                var delegado = usuarioCreador + "@uv.mx";

                if (usuarioTitular == usuarioCreador)
                {
                    delegado = null;
                }
                //Historial de correo
                await _documentoService.GuardarHistorialCorreo(new GuardarHistorialCorreo()
                {
                    Fecha = date,
                    Remitente = titular.InfoUsuarioId,
                    EsDelegado = delegado,
                    Destinatario = EnvioCorreo.Correo,
                    EnvioId = EnvioCorreo.EnvioId,
                    Tipo = EnvioCorreo.Tipo,
                    TipoEnvio = EnvioCorreo.TipoEnvio,
                });
            }

            return RedirectToPage("/Correspondencia/Visualizacion", new { id = EnvioCorreo.EnvioId, tipo = EnvioCorreo.TipoEnvio, usuario = usuarioRedirect });
        }
    }
}