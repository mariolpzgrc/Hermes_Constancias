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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hermes2018.Areas.Identity.Pages.Correspondencia
{
    [Authorize]
    public class ReenviarModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IDocumentoService _documentoService;
        private readonly IImportanciaService _importanciaService;
        private readonly IVisibilidadService _visibilidadService;
        private readonly IAnexoService _anexoService;
        private readonly IConfiguracionService _configuracionService;
        private readonly ICalendarioService _calendarioService;
        private readonly IMailService _mailService;

        public ReenviarModel(IUsuarioService usuarioService,
            IUsuarioClaimService usuarioClaimService,
            IDocumentoService documentoService,
            IImportanciaService importanciaService,
            IVisibilidadService visibilidadService,
            IAnexoService anexoService,
            IConfiguracionService configuracionService,
            ICalendarioService calendarioService,
            IMailService mailService)
        {
            _usuarioService = usuarioService;
            _usuarioClaimService = usuarioClaimService;
            _documentoService = documentoService;
            _importanciaService = importanciaService;
            _visibilidadService = visibilidadService;
            _anexoService = anexoService;
            _configuracionService = configuracionService;
            _calendarioService = calendarioService;
            _mailService = mailService;
        }

        [BindProperty]
        public ReenviarDocumentoLecturaViewModel Envio { get; set; }

        [BindProperty]
        public ReenviarDocumentoViewModel Reenviar { get; set; }

        [BindProperty]
        public CalendarioDiasInhabilesViewModel Inhabiles { get; set; }
        
        [BindProperty]
        public int Tipo { get; set; }

        public async Task OnGetAsync(int id, int tipo)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            string usuarioCreador = infoUsuarioClaims.UserName;
            Tipo = tipo;
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
            Envio = await _documentoService.ObtenerDocumentoReenviarParaLecturaAsync(id, usuarioTitular);
            if (Envio.TipoEnvioId != 4) { 
                Reenviar = new ReenviarDocumentoViewModel()
                {
                    ExisteAdjuntos = Envio.ExisteAdjuntos,
                    EnvioId = Envio.EnvioId,
                    Folio = Envio.Folio,
                    TipoEnvioId = Envio.TipoEnvioId,
                    TipoEnvio = Envio.TipoEnvio,
                    Asunto = Envio.AsuntoEnvio,
                    UsuarioDe = infoUsuarioClaims.BandejaNombre,
                    FolioSession = infoUsuarioClaims.Session,
                    Extensiones = await _configuracionService.ObtenerExtensionesEnCadena(),
                    ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre"),
                    VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre"),
                    RequiereRespuesta = Envio.TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 ? true : false,
                    EsRespuestaDefinitiva = Envio.TipoEnvioId == ConstTipoEnvio.TipoEnvioN4 ? true : false
                };
              
            }
            else if(Envio.TipoEnvioId == 4)
            {
                Reenviar = new ReenviarDocumentoViewModel()
                {
                    ExisteAdjuntos = Envio.ExisteAdjuntos,
                    EnvioId = Envio.EnvioId,
                    Folio = Envio.Folio,
                    TipoEnvioId = ConstTipoEnvio.TipoEnvioN1,
                    TipoEnvio = ConstTipoEnvio.TipoEnvioT1,
                    Asunto = Envio.AsuntoEnvio,
                    UsuarioDe = infoUsuarioClaims.BandejaNombre,
                    FolioSession = infoUsuarioClaims.Session,
                    Extensiones = await _configuracionService.ObtenerExtensionesEnCadena(),
                    ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre"),
                    VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre"),
                    RequiereRespuesta = Envio.TipoEnvioId == ConstTipoEnvio.TipoEnvioN4 ? true : false,
                    EsRespuestaDefinitiva = Envio.TipoEnvioId == ConstTipoEnvio.TipoEnvioN4 ? true : false
                };           
            }
            //--
            Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaCompromisoAsync(DateTime.Now, creador.InfoUsuarioId));
        }

        public async Task<IActionResult> OnPost()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            string usuarioCreador = infoUsuarioClaims.UserName;

            if (ModelState.IsValid)
            {
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

                if (!string.IsNullOrEmpty(Reenviar.Para))
                    valuesMailFor = Reenviar.Para.Split(',').ToList();

                if (!string.IsNullOrEmpty(Reenviar.CCP))
                    valuesMailCCP = Reenviar.CCP.Split(',').ToList();

                var listaPara = await _usuarioService.ObtenerIdentificadoresCompuestosUsuariosAsync(valuesMailFor);
                var listaCCP = await _usuarioService.ObtenerIdentificadoresCompuestosUsuariosAsync(valuesMailCCP);

                var esRespuesta = Reenviar.EsRespuestaDefinitiva;
                //Anexo Adjuntos
                int? anexoId = null;
                if (!string.IsNullOrEmpty(Reenviar.Anexos))
                {
                    List<string> archivos = Reenviar.Anexos.Split(',').ToList();

                    //Guardar en la BD
                    anexoId = await _anexoService.GuardarAnexosAsync(archivos, ConstTipoAnexo.TipoAnexoN1, infoUsuarioClaims.Session, usuarioTitular, Reenviar.Folio);
                }
           
                //Guardar el reenvio
                var reenvio = new ReenviarViewModel()
                {
                    Folio = Reenviar.Folio,
                    EnvioId = Reenviar.EnvioId,
                    TipoEnvioId = Reenviar.TipoEnvioId,
                    Usuario_DeId = usuarioTitularId.InfoUsuarioId,
                    Usuario_EnviaId = usuarioCreadorId.InfoUsuarioId,
                    Total_Para = listaPara.Count(),
                    Total_CCP = listaCCP.Count(),
                    Indicaciones = Reenviar.Indicaciones,
                    RequiereRespuesta = Reenviar.RequiereRespuesta,
                    ImportanciaId = Reenviar.ImportanciaId,
                    VisibilidadId = Reenviar.VisibilidadId,
                    AnexoId = anexoId,
                    FechaPropuesta = (Reenviar.RequiereRespuesta)? Reenviar.FechaPropuesta : string.Empty
                };             

                var reenvioId = await _documentoService.CrearReenvioAsync(reenvio);             

                //Se guardan los destinatarios (los receptores)
                var destinatarios = new List<RecepcionReenviarViewModel>();
                //Se recorren los destinatarios PARA
                foreach (var para in listaPara)
                {
                    destinatarios.Add(new RecepcionReenviarViewModel()
                    {
                        EnvioId = reenvioId,
                        Usuario_ParaId = para.InfoUsuarioId,
                        Tipo_Para = ConstTipoDestinatario.TipoDestinatarioN1,
                        RequiereRespuesta = Reenviar.RequiereRespuesta,
                    });
                }
                //Se recorren los destinatarios CCP
                foreach (var ccp in listaCCP)
                {
                    destinatarios.Add(new RecepcionReenviarViewModel()
                    {
                        EnvioId = reenvioId,
                        Usuario_ParaId = ccp.InfoUsuarioId,
                        Tipo_Para = ConstTipoDestinatario.TipoDestinatarioN2,
                        RequiereRespuesta = Reenviar.RequiereRespuesta,
                    });
                }
                //--
                await _documentoService.CrearRecepcionReenvioAsync(destinatarios, (Reenviar.RequiereRespuesta) ? Reenviar.FechaPropuesta : string.Empty, Reenviar.TipoEnvioId);
                //--
                //Envia un correo
                if (!esRespuesta)
                {
                    await _mailService.EnviarCorreoEspecialEnvio(await _documentoService.ObtenerParaCorreoDocumentoEnviadoAsync(reenvioId), _configuracionService.ObtenerPlantillaEnvioDocumento());
                }
                else 
                {
                    await _mailService.EnviarCorreoEspecialTurnarEspecial(await _documentoService.ObtenerParaCorreoDocumentoTurnadoEspecialAsync(reenvioId), _configuracionService.ObtenerPlantillaTurnarDocumento());
                }
                //-
                return RedirectToPage("/Bandejas/Enviados");
            }
            return RedirectToPage("/Correspondencia/Lectura", new { id = Reenviar.EnvioId, tipo = Reenviar.TipoEnvioId });
        }
    }
}