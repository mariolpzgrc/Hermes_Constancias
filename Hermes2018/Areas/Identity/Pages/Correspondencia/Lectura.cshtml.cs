using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Hermes2018.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Hermes2018.Models.Anexo;
using System.Drawing;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text;

namespace Hermes2018.Areas.Identity.Pages.Correspondencia
{
    [Authorize]
    public class LecturaModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IDocumentoService _documentoService;
        private readonly IMailService _mailService;
        private readonly IConfiguracionService _configuracionService;
        private readonly ICalendarioService _calendarioService;
        private readonly IDescargarPDF _descargarPDFService;
        private readonly IAreaService _areaService;
        private readonly IHostingEnvironment _environment;
        private readonly ITramiteService _tramiteService;
        private readonly CultureInfo _cultureEs;

        public LecturaModel(IUsuarioService usuarioService,
                        IUsuarioClaimService usuarioClaimService,
                        IDocumentoService documentoService,
                        IMailService mailService,
                        ICalendarioService calendarioService,
                        IConfiguracionService configuracionService,
                        IDescargarPDF descargarPDFService,
                        IAreaService areaService,
                        IHostingEnvironment environment,
                        ITramiteService tramiteService)
        {
            _usuarioService = usuarioService;
            _usuarioClaimService = usuarioClaimService;
            _documentoService = documentoService;
            _mailService = mailService;
            _calendarioService = calendarioService;
            _configuracionService = configuracionService;
            _descargarPDFService = descargarPDFService;
            _areaService = areaService;
            _environment = environment;
            _tramiteService = tramiteService;
            _cultureEs = new CultureInfo("es-MX");
        }

        [BindProperty]
        public DocumentoEnviadoLecturaViewModel Envio { get; set; }

        [BindProperty]
        public List<ResumenDestinatarioViewModel> ResumenDestinatarios { get; set; }

        [BindProperty]
        public InfoDelegarUsuarioViewModel InfoDelegar { get; set; }

        [BindProperty]
        public EnvioCorreoLecturaViewModel EnvioCorreo { get; set; }

        [BindProperty]
        public GuardarHistorialCorreo HistorialCorreo { get; set; }

        [BindProperty]
        public List<ListadoHistorialCorreo> ListadoHistorialCorreo { get; set; }

        [BindProperty]
        public string CodigoQR { get; set; }

        [BindProperty]
        public CalendarioDiasInhabilesViewModel Inhabiles { get; set; }

        [BindProperty]
        public DescargaPDFViewModel DescargaPDF { get; set; }

        [BindProperty]
        public NuevoCompromisoJsonModel NuevoCompromiso { get; set; }

        [BindProperty]
        public CompromisoAceptadoJsonModel CompromisoAceptado { get; set; }
        //--
        public List<ListadoTramitesViewModel> Tramites { get; set; }

        [BindProperty]
        public NumerologiaEnvioViewModel Numerologia { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EstaFechaCambiada { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool AdjuntarArchivos { get; set; }

        public async Task OnGetAsync(int id, int tipo)
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

            InfoDelegar = new InfoDelegarUsuarioViewModel()
            {
                EstaActiva = infoUsuarioClaims.ActivaDelegacion,
                TipoPermiso = (infoUsuarioClaims.ActivaDelegacion) ? infoUsuarioClaims.BandejaPermiso : 0
            };

            if (tipo == ConstTipoEnvio.TipoEnvioN1 || tipo == ConstTipoEnvio.TipoEnvioN2)
            {
                if (tipo == ConstTipoEnvio.TipoEnvioN1)
                {
                    //Información del documento
                    Envio = await _documentoService.ObtenerDocumentoEnviadoSoloLecturaAsync(id, infoUsuarioClaims.BandejaUsuario);
                }
                else if (tipo == ConstTipoEnvio.TipoEnvioN2)
                {
                    //Información del documento
                    Envio = await _documentoService.ObtenerDocumentoTurnadoSoloLecturaAsync(id, infoUsuarioClaims.BandejaUsuario);
                }

                if (!Envio.Actual_EstaLeido && Envio.Actual_Visualizacion_Tipo == ConstVisualizacionEnvio.Recepcion)
                {
                    //Cambia el estado
                    await _documentoService.CambiarRecepcionComoLeidoAsync(Envio.Actual_EnvioId, infoUsuarioClaims.BandejaUsuario);
                }

                //historial correo
                ListadoHistorialCorreo = await _documentoService.ObtenerListadoHistorialAsync(Envio.Actual_EnvioId, tipo, Envio.Actual_Visualizacion_Tipo);

            }
            else if (tipo == ConstTipoEnvio.TipoEnvioN3 || tipo == ConstTipoEnvio.TipoEnvioN4)
            {
                //Información del documento respuesta
                Envio = await _documentoService.ObtenerDocumentoRespuestaEnviadoSoloLecturaAsync(id, infoUsuarioClaims.BandejaUsuario);

                if (!Envio.Actual_EstaLeido && Envio.Actual_Visualizacion_Tipo == ConstVisualizacionEnvio.Recepcion)
                {
                    //Cambia el estado
                    await _documentoService.CambiarRecepcionRespuestaComoLeidoAsync(Envio.Actual_EnvioId, infoUsuarioClaims.BandejaUsuario);
                }

                //historial correo
                ListadoHistorialCorreo = await _documentoService.ObtenerListadoHistorialAsync(Envio.Actual_EnvioId, tipo, Envio.Actual_Visualizacion_Tipo);

            }


            if (Envio.Actual_Visualizacion_Tipo == ConstVisualizacionEnvio.Envio)
            {
                //Resumen
                ResumenDestinatarios = await _documentoService.ObtenerResumenDestinatariosAsync(id);
            }


            //--
            CodigoQR = _documentoService.ConstruirQR(
                new InfoDocumentoQRViewModel()
                {
                    De = Envio.Actual_UsuarioDe_NombreCompleto,
                    Para = Envio.Actual_UsuariosPara,
                    Asunto = Envio.Actual_AsuntoEnvio,
                    Folio = Envio.Origen_Folio,
                    Leyenda = "SISTEMA HERMES"
                }
            );

            //Indicadores temporales para el seguimiento
            TempData["EnvioId"] = Envio.Actual_EnvioId.ToString();
            TempData["TipoEnvioId"] = Envio.Actual_TipoEnvioId.ToString();
            ViewData["Bandeja"] = Envio.Actual_Visualizacion_Tipo == ConstVisualizacionEnvio.Envio ? "Enviados" : "Recibidos";

            EnvioCorreo = new EnvioCorreoLecturaViewModel()
            {
                EnvioId = Envio.Actual_EnvioId,
                Tipo = Envio.Actual_Visualizacion_Tipo,
                TipoEnvio = Envio.Actual_TipoEnvioId,
            };

            DescargaPDF = new DescargaPDFViewModel()
            {
                Folio = Envio.Origen_Folio,
                AreaSuperior = Envio.Origen_UsuarioDe_AreaPadreNombre,
                Area = Envio.Origen_UsuarioDe_AreaNombre,
                Region = Envio.Origen_UsuarioDe_Region,
                Asunto = Envio.Origen_Asunto,
                NumeroInterno = Envio.Origen_NoInterno,
                Fecha = Envio.Origen_Fecha,
                ParaNombre = Envio.Origen_UsuarioPara_NombreCompleto,
                ParaNombreEnviado = Envio.Actual_UsuariosPara,
                ParaPuesto = Envio.Origen_UsuarioPara_PuestoNombre,
                ParaArea = Envio.Origen_UsuarioPara_AreaNombre,
                DeDireccion = Envio.Origen_UsuarioDe_Direccion,
                DeTelefono = Envio.Origen_UsuarioDe_Telefono,
                DeCorreo = Envio.Origen_UsuarioDe_Correo,
                Cuerpo = Envio.Origen_Cuerpo,
                DeNombre = Envio.Origen_UsuarioDe_NombreCompleto,
                DePuesto = Envio.Origen_UsuarioDe_PuestoNombre,
                DeArea = Envio.Origen_UsuarioDe_AreaNombre,
                CCP = Envio.Origen_ListadoCcp,
                LeyendaRecibido = Envio.Actual_Fecha,
                CodigoQR = CodigoQR,
                TipoUsuario = Envio.Origen_UsuarioPara_Tipo,
                UsuariosParaCC = Envio.Actual_UsuariosPara,
                //Documento actual
                Indicaciones = Envio.Actual_Indicaciones,
                UsuariosDeActual = Envio.Actual_UsuarioDe_NombreCompleto,
                UsuariosParaActual = Envio.Actual_UsuariosPara,
                UsuariosParaCCActual = Envio.Actual_UsuariosCCP,
                FechaRecepcion = Envio.Actual_Fecha,
                FechaCompromiso = Envio.Actual_FechaCompromiso
            };

            NuevoCompromiso = new NuevoCompromisoJsonModel()
            {
                EnvioId = Envio.Actual_EnvioId,
                TipoEnvioId = Envio.Actual_TipoEnvioId,
                RecepcionId = Envio.Actual_RecepcionId,
                TramiteId = Envio.Actual_TramiteId.ToString()
            };

            CompromisoAceptado = new CompromisoAceptadoJsonModel()
            {
                EnvioId = Envio.Actual_EnvioId,
                TipoEnvioId = Envio.Actual_TipoEnvioId,
                RecepcionId = Envio.Actual_RecepcionId,
                TramiteId = Envio.Actual_TramiteId.ToString(),
                FechaCompromiso = Envio.Actual_FechaPropuesta,
                Motivo = "Fecha Aceptada"
            };

            if (NuevoCompromiso.TipoEnvioId == ConstTipoEnvio.TipoEnvioN1)
            {
                if (Envio.Actual_Visualizacion_Tipo == ConstVisualizacionEnvio.Recepcion)
                {
                    Tramites = await _tramiteService.ListadoTramitesPorNombreAsync(Envio.Actual_RecepcionId);
                }
            }
            else if (NuevoCompromiso.TipoEnvioId == ConstTipoEnvio.TipoEnvioN2)
            {

                if (Envio.Actual_Visualizacion_Tipo == ConstVisualizacionEnvio.Recepcion)
                {
                    if (Envio.Origen_RequiereRespuesta && !String.IsNullOrEmpty(Envio.Origen_FechaCompromiso))
                    {
                        if ((DateTime.Parse(Envio.Origen_FechaCompromiso, _cultureEs) - DateTime.Parse(Envio.Origen_Fecha, _cultureEs)).Days <= 5)
                        {
                            Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaProrrogaCompromisoAsync(DateTime.Parse(Envio.Origen_FechaCompromiso, _cultureEs)), Envio.Origen_EnvioId);
                        }
                        else
                        {
                            Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(DateTime.Parse(Envio.Origen_FechaCompromiso, _cultureEs));
                        }
                    }
                    else
                    {
                        Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaCompromisoAsync(DateTime.Now, creador.InfoUsuarioId));
                    }
                }

            }

            Numerologia = await _documentoService.ObtenerNumerologiaEnvioAsync(Envio.Origen_DocumentoId, Envio.Origen_GrupoEnvio);
            EstaFechaCambiada = await _documentoService.ExisteFechaModificadaAsync(Envio.Actual_RecepcionId);
            HttpContext.Response.Cookies.Append("user_idArea", Envio.Origen_UsuarioDe_AreaId.ToString());
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("Area");
            ModelState.Remove("Tipo");
            ModelState.Remove("Motivo");
            ModelState.Remove("FechaCompromiso");
            ModelState.Remove("TramiteId");
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

            return RedirectToPage("/Correspondencia/Lectura", new { id = EnvioCorreo.EnvioId, tipo = EnvioCorreo.TipoEnvio });

        }

        public async Task<FileResult> OnPostBaseAsync()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaNombre;
            var CookieUV = HttpContext.Request.Cookies["_AppHermesv2.3"];
            var idAreaUsuario = HttpContext.Request.Cookies["user_idArea"];

            byte[] logoI;
            var baseUrl = string.Format("{0}://{1}{2}", "https", Request.Host, Url.Content("~"));
            //var baseUrl = string.Format("{0}://{1}", Request.Scheme, Request.Host);

            if (await _areaService.ExisteLogoInstitucionAsync(Int32.Parse(idAreaUsuario)))
            {
                HER_AnexoArea logo = await _areaService.ObtenerLogoInstitucionAsync(Int32.Parse(idAreaUsuario));
                string rutaBase;

                if (logo.HER_AnexoRutaId == null)
                    rutaBase = _environment.WebRootPath;
                else
                    rutaBase = logo.HER_AnexoRuta.HER_RutaBase;

                logoI = System.IO.File.ReadAllBytes(System.IO.Path.Combine(new string[] { rutaBase, logo.HER_RutaComplemento, logo.HER_Nombre }));
            }
            else
            {
                HER_AnexoGeneral logo = await _configuracionService.ObtenerLogoInstitucionAsync();
                logoI = System.IO.File.ReadAllBytes(System.IO.Path.Combine(new string[] { _environment.WebRootPath, logo.HER_RutaComplemento, logo.HER_Nombre }));
            }

            var nombre = DescargaPDF.Asunto + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");

            return File(_descargarPDFService.DescargarPDF(DescargaPDF, logoI, baseUrl, usuarioTitular), System.Net.Mime.MediaTypeNames.Application.Pdf, string.Format("{0}.pdf", nombre));
            //return File(_descargarPDFService.DescargarPDF(DescargaPDF, CookieUV), System.Net.Mime.MediaTypeNames.Application.Pdf, string.Format("{0}.pdf", nombre));
        }

        public async Task<IActionResult> OnPostCompromiso()
        {
            ModelState.Remove("Area");
            ModelState.Remove("Tipo");
            ModelState.Remove("Correo");

            if (ModelState.IsValid)
            {
                if (NuevoCompromiso.TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 || NuevoCompromiso.TipoEnvioId == ConstTipoEnvio.TipoEnvioN5)
                {
                    await _documentoService.ActualizarFechaCompromisoPrincipalEnvioAsync(NuevoCompromiso);
                }
                else
                {
                    await _documentoService.ActualizarFechaCompromisoAsync(NuevoCompromiso);
                }

            }

            return RedirectToPage("/Correspondencia/Lectura", new { id = NuevoCompromiso.EnvioId, tipo = NuevoCompromiso.TipoEnvioId });
        }

        /*Cuando se genera otro onpost o post debe tener lo siguiente "OnPost" respetar este formato y tener formato CamelCase*
         */
        public async Task<IActionResult> OnPostAceptar()
        {
            ModelState.Remove("Area");
            ModelState.Remove("Tipo");
            ModelState.Remove("Correo");
            ModelState.Remove("Motivo");
            ModelState.Remove("TramiteId");
            ModelState.Remove("FechaCompromiso");
            if (ModelState.IsValid)
            {

                if (CompromisoAceptado.TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 || CompromisoAceptado.TipoEnvioId == ConstTipoEnvio.TipoEnvioN5)
                {
                    await _documentoService.AceptarFechaCompromisoPrincipalEnvioAsync(CompromisoAceptado);
                }
                else
                {
                    await _documentoService.ActualizarFechaPropuestaAsync(CompromisoAceptado);
                }
            }

            return RedirectToPage("/Correspondencia/Lectura", new { id = CompromisoAceptado.EnvioId, tipo = CompromisoAceptado.TipoEnvioId });
        }
    }
}