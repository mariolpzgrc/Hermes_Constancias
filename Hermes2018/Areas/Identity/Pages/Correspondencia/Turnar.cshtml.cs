using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Hermes2018.Data;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Hosting;
using Hermes2018.Helpers;
using System.Globalization;

namespace Hermes2018.Areas.Identity.Pages.Correspondencia
{
    [Authorize]
    public class TurnarModel : PageModel
	{
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IDocumentoService _documentoService;
        private readonly IImportanciaService _importanciaService;
        private readonly IVisibilidadService _visibilidadService;
        private readonly IAnexoService _anexoService;
        private readonly IMailService _mailService;
        private readonly IConfiguracionService _configuracionService;
        private readonly ICalendarioService _calendarioService;
        private readonly CultureInfo _cultureEs;

        public TurnarModel(IUsuarioService usuarioService,
                IUsuarioClaimService usuarioClaimService, 
                IDocumentoService documentoService,
                IImportanciaService importanciaService,
                IVisibilidadService visibilidadService,
                IAnexoService anexoService,
                IHostingEnvironment environment,
                IMailService mailService,
                IConfiguracionService configuracionService,
                ICalendarioService calendarioService)
		{
            _usuarioService = usuarioService;
            _usuarioClaimService = usuarioClaimService;
            _documentoService = documentoService;
            _importanciaService = importanciaService;
            _visibilidadService = visibilidadService;
            _anexoService = anexoService;
            _mailService = mailService;
            _configuracionService = configuracionService;
            _calendarioService = calendarioService;
            _cultureEs = new CultureInfo("es-ES");
        }

		[BindProperty]
		public TurnarDocumentoLecturaViewModel Envio { get; set; }

		[BindProperty]
		public TurnarDocumentoViewModel Turnar { get; set; }

        [BindProperty]
        public CalendarioDiasInhabilesViewModel Inhabiles { get; set; }

        public async Task OnGetAsync(int id)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            string usuarioCreador = infoUsuarioClaims.UserName;
            //--
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
            //--
            Envio = await _documentoService.ObtenerDocumentoTurnarParaLecturaAsync(id, infoUsuarioClaims.BandejaUsuario);
            Turnar = new TurnarDocumentoViewModel() { 
                EnvioId = Envio.EnvioId, 
                Folio = Envio.Folio,
                TipoEnvioId = Envio.TipoEnvioId,
                TipoEnvio = Envio.TipoEnvio,
                Asunto = Envio.AsuntoEnvio,
                UsuarioDe_Turnar = infoUsuarioClaims.BandejaNombre,
                FolioSession = infoUsuarioClaims.Session,
                Extensiones = await _configuracionService.ObtenerExtensionesEnCadena(),
                ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre"),
                VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre"),
                RequiereRespuesta = true
            };
            //--
            if (!string.IsNullOrEmpty(Envio.FechaCompromiso))
            {
                if (DateTime.Now > DateTime.Parse(Envio.FechaCompromiso, _cultureEs))
                {
                    Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaProrrogaCompromisoAsync(DateTime.Now), Envio.EnvioId);
                }
                else if ((DateTime.Parse(Envio.FechaCompromiso, _cultureEs) - DateTime.Parse(Envio.FechaEnvio.Remove(10, 10))).Days <= 5) 
                {
                    Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaProrrogaCompromisoAsync(DateTime.Parse(Envio.FechaCompromiso, _cultureEs)), Envio.EnvioId);
                }
                else
                {
                    Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(DateTime.Parse(Envio.FechaCompromiso, _cultureEs), Envio.EnvioId);
                }
            }
            else {
                Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaCompromisoAsync(DateTime.Now, creador.InfoUsuarioId), Envio.EnvioId);
            }
        }

        public async Task<IActionResult> OnPost()
		{
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            string usuarioCreador = infoUsuarioClaims.UserName;

            if (ModelState.IsValid)
            {
                //Turnar.Extensiones = await _configuracionService.ObtenerExtensionesEnCadena();

                IdentificadorUsuarioCompuestoViewModel usuarioTitularId;
                IdentificadorUsuarioCompuestoViewModel usuarioCreadorId;

                if (usuarioTitular == usuarioCreador)
                {
                    usuarioTitularId = await _usuarioService.ObtenerIdentificadorCompuestoUsuarioAsync(usuarioTitular);
                    usuarioCreadorId = usuarioTitularId;
                }
                else
                {
                    usuarioTitularId = await _usuarioService.ObtenerIdentificadorCompuestoUsuarioAsync(usuarioTitular);
                    usuarioCreadorId = await _usuarioService.ObtenerIdentificadorCompuestoUsuarioAsync(usuarioCreador);
                }

                List<string> valuesMailFor = new List<string>();
                List<string> valuesMailCCP = new List<string>();

                if (!string.IsNullOrEmpty(Turnar.Para))
                    valuesMailFor = Turnar.Para.Split(',').ToList();

                if (!string.IsNullOrEmpty(Turnar.CCP))
                    valuesMailCCP = Turnar.CCP.Split(',').ToList();

                var listaPara = await _usuarioService.ObtenerIdentificadoresCompuestosUsuariosAsync(valuesMailFor);
                var listaCCP = await _usuarioService.ObtenerIdentificadoresCompuestosUsuariosAsync(valuesMailCCP);

                //Anexo Adjuntos
                int? anexoId = null;
                if (!string.IsNullOrEmpty(Turnar.Anexos))
                {
                    List<string> archivos = Turnar.Anexos.Split(',').ToList();

                    //Guardar en la BD
                    anexoId = await _anexoService.GuardarAnexosAsync(archivos, ConstTipoAnexo.TipoAnexoN2, infoUsuarioClaims.Session, usuarioTitular, Turnar.Folio);
                }

                //Guardar el turnar
                var turnar = new TurnarViewModel() {
                    Folio = Turnar.Folio,
                    EnvioId = Turnar.EnvioId,
                    Usuario_DeId = usuarioTitularId.InfoUsuarioId,
                    Usuario_EnviaId = usuarioCreadorId.InfoUsuarioId,
                    Total_Para = listaPara.Count(),
                    Total_CCP = listaCCP.Count(),
                    Indicaciones = Turnar.Indicaciones,
                    RequiereRespuesta = Turnar.RequiereRespuesta,
                    ImportanciaId = Turnar.ImportanciaId,
                    VisibilidadId = Turnar.VisibilidadId,
                    AnexoId = anexoId,
                    FechaPropuesta = (Turnar.RequiereRespuesta)? Turnar.FechaPropuesta : string.Empty
                };

                var turnarId = await _documentoService.CrearTurnarAsync(turnar);

                //Se guardan los destinatarios (los receptores)
                var destinatarios = new List<RecepcionTurnarViewModel>();
                //Se recorren los destinatarios PARA
                foreach (var para in listaPara)
                {
                    destinatarios.Add(new RecepcionTurnarViewModel()
                    {
                        EnvioId = turnarId,
                        Usuario_ParaId = para.InfoUsuarioId,
                        Tipo_Para = ConstTipoDestinatario.TipoDestinatarioN1,
                        RequiereRespuesta = Turnar.RequiereRespuesta,
                    });
                }
                //Se recorren los destinatarios CCP
                foreach (var ccp in listaCCP)
                {
                    destinatarios.Add(new RecepcionTurnarViewModel()
                    {
                        EnvioId = turnarId,
                        Usuario_ParaId = ccp.InfoUsuarioId,
                        Tipo_Para = ConstTipoDestinatario.TipoDestinatarioN2,
                        RequiereRespuesta = Turnar.RequiereRespuesta,
                    });
                }
                //--
                await _documentoService.CrearRecepcionTurnarAsync(destinatarios, (Turnar.RequiereRespuesta) ? Turnar.FechaPropuesta : string.Empty);
                //--
                //Envia un correo
                await _mailService.EnviarCorreoEspecialTurnar(await _documentoService.ObtenerParaCorreoDocumentoTurnadoAsync(turnarId), _configuracionService.ObtenerPlantillaTurnarDocumento());
                //-
                return RedirectToPage("/Bandejas/Enviados");
            }

            return RedirectToPage("/Correspondencia/Lectura", new { id = Turnar.EnvioId, tipo = Turnar.TipoEnvioId });
		}
    }
}