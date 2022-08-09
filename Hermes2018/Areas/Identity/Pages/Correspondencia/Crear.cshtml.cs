using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Areas.Identity.Pages.Correspondencia
{
    [Authorize]
	public class CrearModel : PageModel
	{
		private readonly IUsuarioService _usuarioService;
		private readonly IDocumentoService _documentoService;
        private readonly IPlantillaService _plantillaService;
        private readonly IImportanciaService _importanciaService;
        private readonly ITipoDocumentoService _tipoService;
        private readonly IConfiguracionService _configuracionService;
        private readonly IAnexoService _anexoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IVisibilidadService _visibilidadService;
        private readonly IMailService _mailService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly CultureInfo _cultureEs;
        private readonly ICalendarioService _calendarioService;

        public CrearModel(
						IUsuarioClaimService usuarioClaimService, 
						IUsuarioService usuarioService, 
						IDocumentoService documentoService,
                        IPlantillaService plantillaService,
                        IImportanciaService importanciaService,
                        ITipoDocumentoService tipoService,
                        IConfiguracionService configuracionService,
                        IAnexoService anexoService,
                        ICategoriaService categoriaService,
                        IMailService mailService,
                        IVisibilidadService visibilidadService,
                        ICalendarioService calendarioService)
		{
			_usuarioService = usuarioService;
			_documentoService = documentoService;
            _plantillaService = plantillaService;
            _importanciaService = importanciaService;
            _tipoService = tipoService;
            _configuracionService = configuracionService;
            _anexoService = anexoService;
            _categoriaService = categoriaService;
            _visibilidadService = visibilidadService;
            _mailService = mailService;
            _usuarioClaimService = usuarioClaimService;
            _calendarioService = calendarioService;
            _cultureEs = new CultureInfo("es-MX");
        }

		[BindProperty]
		public RemitenteDocumentoViewModel Remitente { get; set; }

        [BindProperty]
        public CrearDocumentoViewModel CrearDocumento { get; set; }

        [BindProperty]
        public CalendarioDiasInhabilesViewModel Inhabiles { get; set; }
        
        public async Task<ActionResult> OnGet()
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
            if (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToPage("/Bandejas/Recibidos", new { area = "Identity", id = "" });
            }
            //--
            Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaCompromisoAsync(DateTime.Now, creador.InfoUsuarioId));
            //--
            Remitente = await _usuarioService.ObtenerInfoPersonaDocumentoAsync(usuarioTitular);
            //--
            CrearDocumento = new CrearDocumentoViewModel()
            {
                Folio = await _documentoService.GenerarFolioAsync(),
                TipoSubmit = ConstTipoSubmit.TipoSubmitN1.ToString(), //Diferentes tipos de submit (Guardar y enviar), Por defecto se envia el tipo de submit como borrador pero en JS se actualiza cada vez que se presiona el boton correspondiente
                Fecha = DateTime.Now.ToString("D", _cultureEs),
                Extensiones = await _configuracionService.ObtenerExtensionesEnCadena(),
                AreaId = infoUsuarioClaims.AreaId,
                FolioSession = infoUsuarioClaims.Session,
                PlantillasSelectList = new SelectList(await _plantillaService.ObtenerPlantillasAsync(usuarioTitular), "HER_Nombre", "HER_Nombre"),
                ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre"),
                TiposSelectList = new SelectList(await _tipoService.ObtenerTiposDocumentoAsync(), "HER_TipoDocumentoId", "HER_Nombre"),
                VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre"),
                RequiereRespuesta = true
            };
            
            return Page();
		}

        public async Task<IActionResult> OnPost()
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
            if (ModelState.IsValid)
            {
                List<string> usuariosPara = new List<string>();
                List<string> usuariosCCP = new List<string>();
                
                if (CrearDocumento.EsRevision)
                    CrearDocumento.CCP = string.Empty;

                if (!string.IsNullOrEmpty(CrearDocumento.Para))
                    usuariosPara = CrearDocumento.Para.Split(',').ToList();

                if (!string.IsNullOrEmpty(CrearDocumento.CCP))
                    usuariosCCP = CrearDocumento.CCP.Split(',').ToList();

                var listaPara = await _usuarioService.ObtenerIdentificadoresCompuestosUsuariosAsync(usuariosPara);
                var listaCCP = await _usuarioService.ObtenerIdentificadoresCompuestosUsuariosAsync(usuariosCCP);
                //---
                //Anexo Adjuntos
                int? anexoId = null;
                if (!string.IsNullOrEmpty(CrearDocumento.Anexos))
                {
                    List<string> archivos = CrearDocumento.Anexos.Split(',').ToList();
                    
                    //Guardar en la BD
                    anexoId = await _anexoService.GuardarAnexosAsync(archivos, ConstTipoAnexo.TipoAnexoN1, infoUsuarioClaims.Session, usuarioTitular, CrearDocumento.Folio);
                }

                //Categorías
                List<string> listaCategorias = CrearDocumento.Categorias.Split(',').ToList();

                //--
                if (CrearDocumento.EsRevision)
                {
                    var viewModel = new NuevoDocumentoBaseViewModel()
                    {
                        TitularId = titular.InfoUsuarioId,
                        CreadorId = creador.InfoUsuarioId,
                        Destinatarios = listaPara.Select(x => x.InfoUsuarioId).ToList(),
                        CCP = listaCCP.Select(x => x.InfoUsuarioId).ToList(),
                        Folio = CrearDocumento.Folio,
                        RequiereRespuesta = CrearDocumento.RequiereRespuesta,
                        ImportanciaId = CrearDocumento.ImportanciaId,
                        TipoId = CrearDocumento.TipoId,
                        EstadoId = ConstEstadoDocumento.EstadoDocumentoN2,
                        VisibilidadId = CrearDocumento.VisibilidadId,
                        AnexoId = anexoId,
                        FechaRegistro = DateTime.Now,
                        FechaPropuesta = (CrearDocumento.RequiereRespuesta)? CrearDocumento.FechaPropuesta : string.Empty,
                        Asunto = CrearDocumento.Asunto,
                        NoInterno = CrearDocumento.NoInterno,
                        Cuerpo = CrearDocumento.Cuerpo,
                        EnRevision = CrearDocumento.EsRevision,
                    };
                    var documentoBaseId = _documentoService.GuardarDocumentoBase(viewModel);

                    //Crear asociación de categorias
                    await _categoriaService.GuardarCategoriasDocumentoBaseAsync(listaCategorias, documentoBaseId);
                    
                    //--
                    var enviarRevision = new EnviarRevisionViewModel()
                    {
                        DocumentoBaseId = documentoBaseId,
                        RemitenteId = titular.InfoUsuarioId,
                        DestinatarioId = listaPara.Select(x => x.InfoUsuarioId).First(),
                        Estado_Remitente = ConstEstadoRevision.EstadoRemitenteN1,
                        Estado_Destinatario = ConstEstadoRevision.EstadoDestinatarioN1,
                        Fecha = DateTime.Now
                    };

                    await _documentoService.CrearEnvioRevisionAsync(enviarRevision);

                    return RedirectToPage("/Bandejas/Revision");
                }
                else
                {
                    var nuevoDocumentoViewModel = new NuevoDocumentoViewModel()
                    {
                        Folio = CrearDocumento.Folio,
                        Asunto = CrearDocumento.Asunto,
                        NoInterno = CrearDocumento.NoInterno,
                        Cuerpo = CrearDocumento.Cuerpo,
                        FechaCreacion = DateTime.Now,
                        TipoId = CrearDocumento.TipoId,
                        TitularId = titular.InfoUsuarioId,
                        CreadorId = creador.InfoUsuarioId,
                    };
                    var documentoId = _documentoService.GuardarDocumento(nuevoDocumentoViewModel);

                    //Crear asociación de categorias
                    await _categoriaService.GuardarCategoriasDocumentoAsync(listaCategorias, documentoId);

                    //Envio Normal
                    var envioViewModel = new EnvioViewModel()
                    {
                        DocumentoId = documentoId,
                        UsuarioDeId = titular.InfoUsuarioId,
                        UsuarioDeDireccion = titular.Direccion,
                        UsuarioDeTelefono = titular.Telefono,
                        UsuarioEnviaId = creador.InfoUsuarioId,
                        TotalPara = listaPara.Count(),
                        TotalCCP = listaCCP.Count(),
                        RequiereRespuesta = CrearDocumento.RequiereRespuesta,
                        ImportanciaId = CrearDocumento.ImportanciaId,
                        VisibilidadId = CrearDocumento.VisibilidadId,
                        AnexoId = anexoId,
                        FechaPropuesta = (CrearDocumento.RequiereRespuesta) ? CrearDocumento.FechaPropuesta : string.Empty
                    };
                    var envioId = await _documentoService.CrearEnvioAsync(envioViewModel);

                    //Se guardan los destinatarios (los receptores)
                    var destinatarios = new List<RecepcionViewModel>();
                    //--
                    foreach (var para in listaPara)
                    {
                        destinatarios.Add(new RecepcionViewModel()
                        {
                            EnvioId = envioId,
                            ParaId = para.InfoUsuarioId,
                            TipoPara = ConstTipoDestinatario.TipoDestinatarioN1,
                            RequiereRespuesta = CrearDocumento.RequiereRespuesta,
                        });
                    }
                    foreach (var ccp in listaCCP)
                    {
                        destinatarios.Add(new RecepcionViewModel()
                        {
                            EnvioId = envioId,
                            ParaId = ccp.InfoUsuarioId,
                            TipoPara = ConstTipoDestinatario.TipoDestinatarioN2,
                            RequiereRespuesta = CrearDocumento.RequiereRespuesta,
                        });
                    }
                    //--
                    await _documentoService.CrearRecepcionAsync(destinatarios, (CrearDocumento.RequiereRespuesta) ? CrearDocumento.FechaPropuesta : string.Empty);

                    //Envia un correo
                    //await Task.Factory.StartNew(async () => await _mailService.EnviarCorreoEspecialEnvio(await _documentoService.ObtenerParaCorreoDocumentoEnviadoAsync(envioId), _configuracionService.ObtenerPlantillaEnvioDocumento()));
                    await _mailService.EnviarCorreoEspecialEnvio(await _documentoService.ObtenerParaCorreoDocumentoEnviadoAsync(envioId), _configuracionService.ObtenerPlantillaEnvioDocumento());

                    return RedirectToPage("/Bandejas/Enviados");
                }
            }

            CrearDocumento.Para = string.Empty;
            CrearDocumento.CCP = string.Empty;
            CrearDocumento.Categorias = string.Empty;
            CrearDocumento.Anexos = string.Empty;

            Remitente = await _usuarioService.ObtenerInfoPersonaDocumentoAsync(usuarioTitular);
            CrearDocumento.Extensiones = await _configuracionService.ObtenerExtensionesEnCadena();
            CrearDocumento.AreaId = infoUsuarioClaims.AreaId;
            CrearDocumento.Fecha = DateTime.Now.ToString("D", _cultureEs);
            CrearDocumento.PlantillasSelectList = new SelectList(await _plantillaService.ObtenerPlantillasAsync(usuarioTitular), "HER_Nombre", "HER_Nombre");
            CrearDocumento.ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre");
            CrearDocumento.TiposSelectList = new SelectList(await _tipoService.ObtenerTiposDocumentoAsync(), "HER_TipoDocumentoId", "HER_Nombre");
            CrearDocumento.VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre");

            Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaCompromisoAsync(DateTime.Now, creador.InfoUsuarioId));

            return Page();
        }

        public async Task<IActionResult> OnPostBorrador()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            string usuarioCreador = infoUsuarioClaims.UserName;
            //--
            int usuarioTitularId;
            int usuarioCreadorId;

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
            //--
            if (ModelState.IsValid)
            {
                List<string> usuariosPara = new List<string>();
                List<string> usuariosCCP = new List<string>();

                if (CrearDocumento.EsRevision)
                    CrearDocumento.CCP = string.Empty;

                if (!string.IsNullOrEmpty(CrearDocumento.Para))
                    usuariosPara = CrearDocumento.Para.Split(',').ToList();

                if (!string.IsNullOrEmpty(CrearDocumento.CCP))
                    usuariosCCP = CrearDocumento.CCP.Split(',').ToList();

                var listaPara = await _usuarioService.ObtenerIdentificadoresUsuariosAsync(usuariosPara);
                var listaCCP = await _usuarioService.ObtenerIdentificadoresUsuariosAsync(usuariosCCP);

                //Anexo Adjuntos
                int? anexoId = null;
                if (!string.IsNullOrEmpty(CrearDocumento.Anexos))
                {
                    List<string> archivos = CrearDocumento.Anexos.Split(',').ToList();

                    //Guardar en la BD
                    anexoId = await _anexoService.GuardarAnexosAsync(archivos, ConstTipoAnexo.TipoAnexoN1, infoUsuarioClaims.Session, usuarioTitular, CrearDocumento.Folio);
                }

                //Nuevo oficio
                var viewModel = new NuevoDocumentoBaseViewModel()
                {
                    TitularId = usuarioTitularId,
                    CreadorId = usuarioCreadorId,
                    Destinatarios = listaPara,
                    CCP = listaCCP,
                    Folio = CrearDocumento.Folio,
                    RequiereRespuesta = CrearDocumento.RequiereRespuesta,
                    ImportanciaId = CrearDocumento.ImportanciaId,
                    TipoId = CrearDocumento.TipoId,
                    EstadoId = ConstEstadoDocumento.EstadoDocumentoN1,
                    VisibilidadId = CrearDocumento.VisibilidadId,
                    AnexoId = anexoId,
                    FechaRegistro = DateTime.Now,
                    FechaPropuesta = (CrearDocumento.RequiereRespuesta)? CrearDocumento.FechaPropuesta : string.Empty,
                    Asunto = CrearDocumento.Asunto,
                    NoInterno = CrearDocumento.NoInterno,
                    Cuerpo = CrearDocumento.Cuerpo,
                    EnRevision = CrearDocumento.EsRevision,
                };

                //Regresa DocumentoBaseId
                var documentoBaseId = _documentoService.GuardarDocumentoBase(viewModel);

                if (!string.IsNullOrEmpty(CrearDocumento.Categorias))
                {
                    //Crear asociación de categorias
                    List<string> listCategorias = CrearDocumento.Categorias.Split(',').ToList();
                    await _categoriaService.GuardarCategoriasDocumentoBaseAsync(listCategorias, documentoBaseId);
                }

                return RedirectToPage("/Bandejas/Borradores");
            }

            CrearDocumento.Para = string.Empty;
            CrearDocumento.CCP = string.Empty;
            CrearDocumento.Categorias = string.Empty;
            CrearDocumento.Anexos = string.Empty;

            Remitente = await _usuarioService.ObtenerInfoPersonaDocumentoAsync(usuarioTitular);
            CrearDocumento.Extensiones = await _configuracionService.ObtenerExtensionesEnCadena();
            CrearDocumento.AreaId = infoUsuarioClaims.AreaId;
            CrearDocumento.Fecha = DateTime.Now.ToString("D", _cultureEs);
            CrearDocumento.PlantillasSelectList = new SelectList(await _plantillaService.ObtenerPlantillasAsync(usuarioTitular), "HER_Nombre", "HER_Nombre");
            CrearDocumento.ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre");
            CrearDocumento.TiposSelectList = new SelectList(await _tipoService.ObtenerTiposDocumentoAsync(), "HER_TipoDocumentoId", "HER_Nombre");
            CrearDocumento.VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre");

            Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaCompromisoAsync(DateTime.Now, usuarioCreadorId));

            return Page();
        }
	}
}