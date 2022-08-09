using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Areas.Identity.Pages.Correspondencia
{
    [Authorize]
    public class ResponderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IDocumentoService _documentoService;
        private readonly IPlantillaService _plantillaService;
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoDocumentoService _tipoService;
        private readonly IVisibilidadService _visibilidadService;
        private readonly IAnexoService _anexoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IHostingEnvironment _environment;
        private readonly CultureInfo _cultureEs;
        private readonly IMailService _mailService;
        private readonly IConfiguracionService _configuracionService;

        public ResponderModel(ApplicationDbContext context, 
                IUsuarioClaimService usuarioClaimService, 
                IDocumentoService documentoService,
                IPlantillaService plantillaService,
                IUsuarioService usuarioService,
                ITipoDocumentoService tipoService,
                IVisibilidadService visibilidadService,
                IAnexoService anexoService,
                ICategoriaService categoriaService,
                IHostingEnvironment environment,
                IMailService mailService,
                IConfiguracionService configuracionService)
        {
            _context = context;
            _usuarioClaimService = usuarioClaimService;
            _documentoService = documentoService;
            _plantillaService = plantillaService;
            _usuarioService = usuarioService;
            _tipoService = tipoService;
            _visibilidadService = visibilidadService;
            _anexoService = anexoService;
            _categoriaService = categoriaService;
            _environment = environment;
            _cultureEs = new CultureInfo("es-MX");
            _mailService = mailService;
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public DocumentoRespuestaLecturaViewModel Envio { get; set; }

        [BindProperty]
        public RespuestaDocumentoViewModel Responder { get; set; }

        [BindProperty]
        public string listaCCTemp { get; set; }

        public async Task OnGetAsync(int id)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            List<SelectListItem> tiposRespuesta = new List<SelectListItem>();
            List<String> correosTemp = new List<string>();
            //--
            Envio = await _documentoService.ObtenerDocumentoRespuestaParaLecturaAsync(id, infoUsuarioClaims.BandejaUsuario);

            if (Envio.Actual_TipoEnvioId == ConstTipoEnvio.TipoEnvioN3)
            {
                //opciones para respuestas parciales
                tiposRespuesta = new List<SelectListItem>() {
                    new SelectListItem() { Text = ConstTipoRespuesta.TipoRespuestaT2, Value = ConstTipoRespuesta.TipoRespuestaN2.ToString() }
                };
            }
            else if (Envio.Actual_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 || Envio.Actual_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2)
            {
                //opciones generales
                if (Envio.TieneRespuesta)
                {
                    tiposRespuesta = new List<SelectListItem>() {
                        new SelectListItem() { Text = ConstTipoRespuesta.TipoRespuestaT2, Value = ConstTipoRespuesta.TipoRespuestaN2.ToString() }
                    };
                }
                else {
                    tiposRespuesta = new List<SelectListItem>() {
                        new SelectListItem() { Text = ConstTipoRespuesta.TipoRespuestaT1, Value = ConstTipoRespuesta.TipoRespuestaN1.ToString() },
                        new SelectListItem() { Text = ConstTipoRespuesta.TipoRespuestaT2, Value = ConstTipoRespuesta.TipoRespuestaN2.ToString() }
                    };
                }
            }

            if (Envio.Origen_ListadoCcp != null)
            {
                foreach(string item in Envio.Origen_ListadoCcpCorreo)
                {
                    if (!item.Equals(infoUsuarioClaims.BandejaUsuario))
                    {
                        correosTemp.Add(item);
                    }
                }

                listaCCTemp = string.Join(",", correosTemp);
            } 
            
            Responder = new RespuestaDocumentoViewModel()
            {
                Origen_EnvioId = Envio.Origen_EnvioId,
                Origen_TipoEnvioId = Envio.Origen_TipoEnvioId,
                //--
                Actual_EnvioId = Envio.Actual_EnvioId,
                Actual_TipoEnvioId = Envio.Actual_TipoEnvioId,
                //--
                Folio = Envio.Origen_Folio,
                Para = Envio.Actual_UsuarioPara_NombreCompleto,
                UsuarioParaId = Envio.Actual_UsuarioPara_UsuarioId,
                NombreUsuarioPara = Envio.Actual_UsuarioPara_NombreUsuario,
                Fecha = DateTime.Now.ToString("D", _cultureEs),
                Asunto = Envio.Actual_AsuntoRespuesta,
                CCP = listaCCTemp,
                //--
                Extensiones = await _configuracionService.ObtenerExtensionesEnCadena(),
                FolioSession = infoUsuarioClaims.Session,
                //--
                PlantillasSelectList = new SelectList(await _plantillaService.ObtenerPlantillasAsync(infoUsuarioClaims.BandejaUsuario), "HER_Nombre", "HER_Nombre"),
                TiposSelectList = new SelectList(await _tipoService.ObtenerTiposDocumentoAsync(), "HER_TipoDocumentoId", "HER_Nombre"),
                VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre"),
                TiposRespuestaSelectList = new SelectList(tiposRespuesta, "Value", "Text")
            };           
        }

        public async Task<IActionResult> OnPost()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            string usuarioCreador = infoUsuarioClaims.UserName;

            //--
            if (ModelState.IsValid)
            {
                //Responder.Extensiones = await _configuracionService.ObtenerExtensionesEnCadena();
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
                //------
                List<string> usuariosCCP = new List<string>();

                if (!string.IsNullOrEmpty(Responder.CCP))
                    usuariosCCP = Responder.CCP.Split(',').ToList();

                var para = await _usuarioService.ObtenerIdentificadorCompuestoUsuarioAsync(Responder.NombreUsuarioPara);
                var listaCCP = await _usuarioService.ObtenerIdentificadoresCompuestosUsuariosAsync(usuariosCCP);

                //Anexo Adjuntos
                int? anexoId = null;
                if (!string.IsNullOrEmpty(Responder.Anexos))
                {
                    List<string> archivos = Responder.Anexos.Split(',').ToList();

                    //Guardar en la BD
                    anexoId = await _anexoService.GuardarAnexosAsync(archivos, ConstTipoAnexo.TipoAnexoN3, infoUsuarioClaims.Session, usuarioTitular, Responder.Folio);
                }

                var viewModel = new NuevoDocumentoRespuestaViewModel()
                {
                    //Datos documento
                    Folio = Responder.Folio,
                    Asunto = Responder.Asunto,
                    NoInterno = Responder.NoInterno,
                    Cuerpo = Responder.Cuerpo,
                    FechaCreacion = DateTime.Now,
                    TipoId = Responder.TipoId,
                    //De
                    TitularId = titular.InfoUsuarioId,
                    CreadorId = creador.InfoUsuarioId
                };

                //Regresa documento RespuestaId
                var documentoRespuestaId = await _documentoService.GuardarDocumentoRespuestaAsync(viewModel);

                //Crear asociación de categorias
                List<string> listaCategorias = Responder.Categorias.Split(',').ToList();
                await _categoriaService.GuardarCategoriasDeDocumentoRespuestaAsync(listaCategorias, documentoRespuestaId);

                //Crear el Envio de la Respuesta 
                var envioRespuestaViewModel = new EnvioRespuestaViewModel()
                {
                    EnvioActualId = Responder.Actual_EnvioId,
                    EnvioId = (Responder.Actual_TipoEnvioId == ConstTipoEnvio.TipoEnvioN3)? Responder.Origen_EnvioId : Responder.Actual_EnvioId,
                    //--
                    DocumentoRespuestaId = documentoRespuestaId,
                    Folio = Responder.Folio,
                    //--
                    UsuarioDeId = titular.InfoUsuarioId,
                    //--
                    UsuarioDeDireccion = titular.Direccion,
                    UsuarioDeTelefono = titular.Telefono,
                    UsuarioEnviaId = creador.InfoUsuarioId,
                    TotalPara = 1,
                    TotalCCP = listaCCP.Count(),
                    TipoRespuestaId = Responder.TipoRespuestaId,
                    VisibilidadId = Responder.VisibilidadId,
                    AnexoId = anexoId,
                };
                var envioRespuestaId = await _documentoService.CrearEnvioRespuestaAsync(envioRespuestaViewModel);

                //Crear Recepcion de la Respuesta
                var destinatarios = new List<RecepcionRespuestaViewModel>();
                //Para
                destinatarios.Add(new RecepcionRespuestaViewModel()
                {
                    RespuestaEnvioId = envioRespuestaId,
                    ParaId = para.InfoUsuarioId,
                    TipoPara = ConstTipoDestinatario.TipoDestinatarioN1
                });
                //Se recorren los destinatarios CCP
                if (listaCCP != null) 
                {
                    foreach (var ccp in listaCCP)
                    {
                        destinatarios.Add(new RecepcionRespuestaViewModel()
                        {
                            RespuestaEnvioId = envioRespuestaId,
                            ParaId = ccp.InfoUsuarioId,
                            TipoPara = ConstTipoDestinatario.TipoDestinatarioN2
                        });
                    }
                }
                
                await _documentoService.CrearRecepcionRespuestaAsync(destinatarios);

                if (Responder.TipoRespuestaId == ConstTipoRespuesta.TipoRespuestaN1)
                {
                    //[Respuesta Definitiva]
                    //Actualizar estado del envio
                    await _documentoService.ActualizarEstadoEnvioAsync(Responder.Actual_EnvioId, titular.InfoUsuarioId);

                    //Actualizar el estado de la recepción
                    await _documentoService.ActualizarEstadoRecepcionAsync(Responder.Actual_EnvioId, titular.InfoUsuarioId);
                    //--
                }

                //Envia un correo
                await _mailService.EnviarCorreoEspecialResponder(await _documentoService.ObtenerParaCorreoDocumentoRespondidoAsync(envioRespuestaId), _configuracionService.ObtenerPlantillaResponderDocumento());
            }

            return RedirectToPage("/Bandejas/Enviados");
        }

        /*public async Task<IActionResult> OnPostBorrador()
        {
            var infoUsurario = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsurario.BandejaUsuario;
            string usuarioCreador = infoUsurario.UserName;

            int usuarioTitularId, usuarioCreadorId;

            if (usuarioTitular == usuarioCreador) 
            {
                usuarioTitularId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(usuarioTitular);
                usuarioCreadorId = usuarioTitularId;
            }
            else
            {
                usuarioTitularId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(usuarioTitular);
                usuarioCreadorId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(usuarioCreador);
            }

            if (ModelState.IsValid)
            {
                List<string> usuariosPara = new List<string>();
                List<string> usuariosCCP = new List<string>();

                if (!string.IsNullOrEmpty(Responder.NombreUsuarioPara))
                {
                    usuariosPara = Responder.NombreUsuarioPara.Split(',').ToList();
                }

                if (!string.IsNullOrEmpty(Responder.CCP))
                {
                    usuariosCCP = Responder.CCP.Split(',').ToList();
                }

                var listaPara = await _usuarioService.ObtenerIdentificadoresUsuariosAsync(usuariosPara);
                var listaCCP = await _usuarioService.ObtenerIdentificadoresUsuariosAsync(usuariosCCP);

                int? anexoId = null;
                if (!string.IsNullOrEmpty(Responder.Anexos))
                {
                    List<string> archivos = Responder.Anexos.Split(',').ToList();

                    anexoId = await _anexoService.GuardarAnexosAsync(archivos, ConstTipoAnexo.TipoAnexoN3, infoUsurario.Session, usuarioTitular, Responder.Folio);
                }

                //Nuevo oficio
                var nuevoBorrador = new NuevoDocumentoBaseViewModel()
                {
                    TitularId = usuarioTitularId,
                    CreadorId = usuarioTitularId,
                    Destinatarios = listaPara,
                    CCP = listaCCP,
                    Folio = Responder.Folio,
                    RequiereRespuesta = false,
                    ImportanciaId = ConstImportancia.ImportanciaN2,
                    TipoId = Responder.TipoId,
                    EstadoId = ConstEstadoDocumento.EstadoDocumentoN1,
                    VisibilidadId = Responder.VisibilidadId,
                    AnexoId = anexoId,
                    FechaRegistro = DateTime.Now,
                    FechaPropuesta = string.Empty,
                    Asunto = Responder.Asunto,
                    NoInterno = string.Empty,
                    Cuerpo = Responder.Cuerpo,
                    EnRevision = false,
                };

                //Regresa DocumentoBaseId
                var documentoBaseId = _documentoService.GuardarDocumentoBase(nuevoBorrador);

                if (!string.IsNullOrEmpty(Responder.Categorias))
                {
                    //Crear asociación de categorias
                    List<string> listCategorias = Responder.Categorias.Split(',').ToList();
                    await _categoriaService.GuardarCategoriasDocumentoBaseAsync(listCategorias, documentoBaseId);
                }

                return RedirectToPage("/Bandejas/Borradores");
            }

            return Page();
        }*/
    }
}