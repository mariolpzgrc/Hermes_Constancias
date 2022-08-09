using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Anexo;
using Hermes2018.Models.Documento;
using Hermes2018.Resources;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace Hermes2018.Areas.Identity.Pages.Correspondencia
{
	[Authorize]
	public class EditarModel : PageModel
	{
		private readonly IUsuarioClaimService _usuarioClaimService;
		private readonly IUsuarioService _usuarioService;
		private readonly IDocumentoService _documentoService;
        private readonly IPlantillaService _plantillaService;
        private readonly IImportanciaService _importanciaService;
        private readonly IConfiguracionService _configuracionService;
        private readonly ICategoriaService _categoriaService;
        private readonly ITipoDocumentoService _tipoService;
        private readonly IVisibilidadService _visibilidadService;
        private readonly IAnexoService _anexoService;
        private readonly CultureInfo _cultureEs;
        private readonly ICalendarioService _calendarioService;

        public EditarModel(IUsuarioClaimService usuarioClaimService,
						IUsuarioService usuarioService,
                        IPlantillaService plantillaService,
                        IDocumentoService documentoService,
                        IImportanciaService importanciaService,
                        IConfiguracionService configuracionService,
                        ICategoriaService categoriaService,
                        ITipoDocumentoService tipoService,
                        IVisibilidadService visibilidadService,
                        IAnexoService anexoService,
                        ICalendarioService calendarioService)
		{
			_usuarioClaimService = usuarioClaimService;
			_usuarioService = usuarioService;
			_documentoService = documentoService;
            _plantillaService = plantillaService;
            _importanciaService = importanciaService;
            _configuracionService = configuracionService;
            _categoriaService = categoriaService;
            _tipoService = tipoService;
            _visibilidadService = visibilidadService;
            _anexoService = anexoService;
            _calendarioService = calendarioService;
            _cultureEs = new CultureInfo("es-MX");
        }

		[BindProperty]
		public RemitenteDocumentoViewModel Remitente { get; set; }

        [BindProperty]
        public EditarDocumentoViewModel EditarDocumento { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EstadoDelegar { get; set; }

        [BindProperty]
        public CalendarioDiasInhabilesViewModel Inhabiles { get; set; }

        public async Task OnGet(int id)
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

            Remitente = await _usuarioService.ObtenerInfoPersonaDocumentoAsync(usuarioTitular);
            
            EstadoDelegar = (!infoUsuarioClaims.ActivaDelegacion || (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN1));
            
            //Información del documento
            var documento = await _documentoService.ObtenerInfoBaseDocumentoAsync(id, usuarioTitular);
            var esPara = EstadoDelegar ?
                    (from c in documento.HER_Destinatarios
                     where c.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN1
                     orderby c.HER_DestinatarioId
                     select c.HER_UDestinatario.HER_UserName).ToArray()
                    :
                    (from c in documento.HER_Destinatarios
                     where c.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN1
                     orderby c.HER_DestinatarioId
                     select c.HER_UDestinatario.HER_NombreCompleto).ToArray();

            string cadenaPara = string.Join(",", esPara);
            var esCC = EstadoDelegar ?
                    (from c in documento.HER_Destinatarios
                     where c.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN2
                     orderby c.HER_DestinatarioId
                     select c.HER_UDestinatario.HER_UserName).ToArray()
                    :
                    (from c in documento.HER_Destinatarios
                     where c.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN2
                     orderby c.HER_DestinatarioId
                     select c.HER_UDestinatario.HER_NombreCompleto).ToArray();
            string cadenaCC = string.Join(",", esCC);

            EditarDocumento = new EditarDocumentoViewModel() {
                DocumentoBaseId = documento.HER_DocumentoBaseId,

                Extensiones = await _configuracionService.ObtenerExtensionesEnCadena(),
                AreaId = infoUsuarioClaims.AreaId,
                Asunto = documento.HER_Asunto,
                Cuerpo = documento.HER_Cuerpo,
                NoInterno = documento.HER_NoInterno,
                Folio = documento.HER_Folio,
                ImportanciaId = documento.HER_ImportanciaId,
                TipoId = documento.HER_TipoId,
                VisibilidadId = documento.HER_VisibilidadId,
                EsRevision = documento.HER_EnRevision,
                RequiereRespuesta = documento.HER_RequiereRespuesta,
                TipoSubmit = ConstTipoSubmit.TipoSubmitN1.ToString(), //Diferentes tipos de submit (Guardar y enviar), Por defecto se envia el tipo de submit como borrador pero en JS se actualiza cada vez que se presiona el boton correspondiente
                Fecha = DateTime.Now.ToString("D", _cultureEs),
                FolioSession = infoUsuarioClaims.Session,
                FechaPropuesta = (documento.HER_RequiereRespuesta)? documento.HER_FechaPropuesta.ToString("dd/MM/yyyy", _cultureEs) : string.Empty,
                Archivos = (documento.HER_AnexoId != null) ? documento.HER_Anexo.HER_AnexoArchivos : new List<HER_AnexoArchivo>(),
                
                Para =
                    cadenaPara,
                CCP =
                    cadenaCC,

                RequiereRespuestaTexto = EstadoDelegar ? 
                    string.Empty :
                    documento.HER_RequiereRespuesta? "Si" : "No",
                Importancia = EstadoDelegar ?
                    string.Empty : 
                    documento.HER_Importancia.HER_Nombre,
                Tipo = EstadoDelegar ?
                     string.Empty :
                     documento.HER_Tipo.HER_Nombre,
                Visibilidad = EstadoDelegar ?
                    string.Empty :
                    documento.HER_Visibilidad.HER_Nombre,
                
                PlantillasSelectList = new SelectList(await _plantillaService.ObtenerPlantillasAsync(usuarioTitular), "HER_Nombre", "HER_Nombre"),
                ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre"),
                TiposSelectList = new SelectList(await _tipoService.ObtenerTiposDocumentoAsync(), "HER_TipoDocumentoId", "HER_Nombre"),
                VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre")
            };

            //--
            Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaCompromisoAsync(DateTime.Now, creador.InfoUsuarioId));

            ViewData["Bandeja"] = "Borradores";
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
            var documento = await _documentoService.ObtenerInfoBaseDocumentoAsync(EditarDocumento.DocumentoBaseId, usuarioTitular);
            //--
            if (ModelState.IsValid)
            {
                List<string> usuariosPara = new List<string>();
                List<string> usuariosCCP = new List<string>();

                if (EditarDocumento.EsRevision)
                    EditarDocumento.CCP = string.Empty;

                if (!string.IsNullOrEmpty(EditarDocumento.Para))
                    usuariosPara = EditarDocumento.Para.Split(',').ToList();
                
                if (!string.IsNullOrEmpty(EditarDocumento.CCP))
                    usuariosCCP = EditarDocumento.CCP.Split(',').ToList();

                var listaPara = await _usuarioService.ObtenerIdentificadoresCompuestosUsuariosAsync(usuariosPara);
                var listaCCP = await _usuarioService.ObtenerIdentificadoresCompuestosUsuariosAsync(usuariosCCP);

                var viewModel = new ActualizarDocumentoBaseViewModel()
                {
                    DocumentoBaseId = EditarDocumento.DocumentoBaseId,
                    Folio = EditarDocumento.Folio,
                    RequiereRespuesta = EditarDocumento.RequiereRespuesta,
                    EnRevision = EditarDocumento.EsRevision,
                    Asunto = EditarDocumento.Asunto,
                    NoInterno = EditarDocumento.NoInterno,
                    Cuerpo = EditarDocumento.Cuerpo,
                    ImportanciaId = EditarDocumento.ImportanciaId,
                    TipoId = EditarDocumento.TipoId,
                    EstadoId = (EditarDocumento.EsRevision) ? ConstEstadoDocumento.EstadoDocumentoN2 : ConstEstadoDocumento.EstadoDocumentoN1,
                    VisibilidadId = EditarDocumento.VisibilidadId,
                    Destinatarios = listaPara.Select(x => x.InfoUsuarioId).ToList(),
                    CCP = listaCCP.Select(x => x.InfoUsuarioId).ToList(),
                    FechaPropuesta = (EditarDocumento.RequiereRespuesta)? EditarDocumento.FechaPropuesta : string.Empty,
                };
                //--
                var documentoBaseId = await _documentoService.ActualizarDocumentoBaseAsync(viewModel);

                //Categorias
                if (!string.IsNullOrEmpty(EditarDocumento.Categorias))
                {
                    List<string> listaNuevasCategorias = EditarDocumento.Categorias.Split(',').ToList();

                    //Crear asociación de categorias
                    await _categoriaService.ActualizarCategoriasDocumentoBaseAsync(listaNuevasCategorias, EditarDocumento.DocumentoBaseId);
                }

                //Anexos
                if (!string.IsNullOrEmpty(EditarDocumento.Anexos))
                {
                    //Anexo Adjuntos
                    var archivos = EditarDocumento.Anexos.Split(',').ToList();

                    //Guardar en la BD
                    await _anexoService.ActualizarAnexosDocumentoBaseAsync(archivos, ConstTipoAnexo.TipoAnexoN1, infoUsuarioClaims.Session, usuarioTitular, EditarDocumento.Folio, EditarDocumento.DocumentoBaseId);
                }

                //Envio
                if (EditarDocumento.EsRevision)
                {
                    //--Enviar Revisión
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
                    int? anexoId = null;
                    //--
                    var nuevoDocumentoViewModel = new NuevoDocumentoViewModel()
                    {
                        Folio = EditarDocumento.Folio,
                        Asunto = EditarDocumento.Asunto,
                        NoInterno = EditarDocumento.NoInterno,
                        Cuerpo = EditarDocumento.Cuerpo,
                        FechaCreacion = DateTime.Now,
                        TipoId = EditarDocumento.TipoId,
                        TitularId = titular.InfoUsuarioId,
                        CreadorId = creador.InfoUsuarioId
                    };
                    var documentoId = _documentoService.GuardarDocumento(nuevoDocumentoViewModel);

                    //Crear asociación de categorias
                    await _categoriaService.GuardarCategoriasDocumentoAsync(documentoId, documentoBaseId);

                    //Anexos
                    anexoId = await _anexoService.RecuperaAnexoAsync(documentoBaseId);

                    //Envio normal
                    var envioViewModel = new EnvioViewModel()
                    {
                        DocumentoId = documentoId,
                        UsuarioDeId = titular.InfoUsuarioId,
                        UsuarioDeDireccion = titular.Direccion,
                        UsuarioDeTelefono = titular.Telefono,
                        UsuarioEnviaId = creador.InfoUsuarioId,
                        TotalPara = listaPara.Count(),
                        TotalCCP = listaCCP.Count(),
                        RequiereRespuesta = EditarDocumento.RequiereRespuesta,
                        ImportanciaId = EditarDocumento.ImportanciaId,
                        VisibilidadId = EditarDocumento.VisibilidadId,
                        AnexoId = anexoId,
                        FechaPropuesta = (EditarDocumento.RequiereRespuesta)? EditarDocumento.FechaPropuesta : string.Empty
                    };
                    var envioId = await _documentoService.CrearEnvioAsync(envioViewModel);

                    //Se guardan los destinatarios (los receptores)
                    var destinatarios = new List<RecepcionViewModel>();
                    //Se recorren los destinatarios PARA
                    foreach (var para in listaPara)
                    {
                        destinatarios.Add(new RecepcionViewModel()
                        {
                            EnvioId = envioId,
                            ParaId = para.InfoUsuarioId,
                            TipoPara = ConstTipoDestinatario.TipoDestinatarioN1,
                            RequiereRespuesta = EditarDocumento.RequiereRespuesta
                        });
                    }
                    //Se recorren los destinatarios CCP
                    foreach (var ccp in listaCCP)
                    {
                        destinatarios.Add(new RecepcionViewModel()
                        {
                            EnvioId = envioId,
                            ParaId = ccp.InfoUsuarioId,
                            TipoPara = ConstTipoDestinatario.TipoDestinatarioN2,
                            RequiereRespuesta = EditarDocumento.RequiereRespuesta
                        });
                    }

                    await _documentoService.CrearRecepcionAsync(destinatarios, (EditarDocumento.RequiereRespuesta) ? EditarDocumento.FechaPropuesta : string.Empty);

                    //Elimina el Documento Borrador
                    await _documentoService.EliminarDocumentoBaseAsync(EditarDocumento.Folio, EditarDocumento.DocumentoBaseId);

                    return RedirectToPage("/Bandejas/Enviados");
                }
            }

            EditarDocumento.Extensiones = await _configuracionService.ObtenerExtensionesEnCadena();
            EditarDocumento.Fecha = DateTime.Now.ToString("D", _cultureEs);
            EditarDocumento.Archivos = (documento.HER_AnexoId != null) ? documento.HER_Anexo.HER_AnexoArchivos : new List<HER_AnexoArchivo>();

            EditarDocumento.Para = EstadoDelegar ?
                    string.Join(",", documento.HER_Destinatarios.Where(x => x.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN1).Select(w => w.HER_UDestinatario.HER_UserName).ToArray()) :
                    string.Join(",", documento.HER_Destinatarios.Where(x => x.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN1).Select(w => w.HER_UDestinatario.HER_NombreCompleto).ToArray());

            EditarDocumento.CCP = EstadoDelegar ?
                    string.Join(",", documento.HER_Destinatarios.Where(x => x.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN2).Select(x => x.HER_UDestinatario.HER_UserName).ToArray()) :
                    string.Join(",", documento.HER_Destinatarios.Where(x => x.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN2).Select(x => x.HER_UDestinatario.HER_NombreCompleto).ToArray());

            EditarDocumento.RequiereRespuestaTexto = EstadoDelegar ?
                    string.Empty :
                    documento.HER_RequiereRespuesta ? "Si" : "No";
            EditarDocumento.Importancia = EstadoDelegar ?
                string.Empty :
                documento.HER_Importancia.HER_Nombre;
            EditarDocumento.Tipo = EstadoDelegar ?
                 string.Empty :
                 documento.HER_Tipo.HER_Nombre;
            EditarDocumento.Visibilidad = EstadoDelegar ?
                string.Empty :
                documento.HER_Visibilidad.HER_Nombre;

            EditarDocumento.PlantillasSelectList = new SelectList(await _plantillaService.ObtenerPlantillasAsync(usuarioTitular), "HER_Nombre", "HER_Nombre");
            EditarDocumento.ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre");
            EditarDocumento.TiposSelectList = new SelectList(await _tipoService.ObtenerTiposDocumentoAsync(), "HER_TipoDocumentoId", "HER_Nombre");
            EditarDocumento.VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre");

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
            var documento = await _documentoService.ObtenerInfoBaseDocumentoAsync(EditarDocumento.DocumentoBaseId, usuarioTitular);
            //--
            if (ModelState.IsValid)
            {
                List<string> usuariosPara = new List<string>();
                List<string> usuariosCCP = new List<string>();
                
                if (EditarDocumento.EsRevision)
                    EditarDocumento.CCP = string.Empty;

                if (!string.IsNullOrEmpty(EditarDocumento.Para))
                    usuariosPara = EditarDocumento.Para.Split(',').ToList();

                if (!string.IsNullOrEmpty(EditarDocumento.CCP))
                    usuariosCCP = EditarDocumento.CCP.Split(',').ToList();

                var listaPara = await _usuarioService.ObtenerIdentificadoresUsuariosAsync(usuariosPara);
                var listaCCP = await _usuarioService.ObtenerIdentificadoresUsuariosAsync(usuariosCCP);

                //Documento
                var viewModel = new ActualizarDocumentoBaseViewModel()
                {
                    DocumentoBaseId = EditarDocumento.DocumentoBaseId,
                    Destinatarios = listaPara,
                    CCP = listaCCP,
                    Folio = EditarDocumento.Folio,
                    RequiereRespuesta = EditarDocumento.RequiereRespuesta,
                    ImportanciaId = EditarDocumento.ImportanciaId,
                    TipoId = EditarDocumento.TipoId,
                    VisibilidadId = EditarDocumento.VisibilidadId,
                    EstadoId = ConstEstadoDocumento.EstadoDocumentoN1, //Borrador
                    Asunto = EditarDocumento.Asunto,
                    NoInterno = EditarDocumento.NoInterno,
                    Cuerpo = EditarDocumento.Cuerpo,
                    EnRevision = EditarDocumento.EsRevision,
                    FechaPropuesta = (EditarDocumento.RequiereRespuesta)? EditarDocumento.FechaPropuesta : string.Empty
                };

                //DocumentoBaseId
                var documentoBaseId = await _documentoService.ActualizarDocumentoBaseAsync(viewModel);

                //Categorias
                if (!string.IsNullOrEmpty(EditarDocumento.Categorias))
                {
                    List<string> listaCategorias = EditarDocumento.Categorias.Split(',').ToList();
                    await _categoriaService.ActualizarCategoriasDocumentoBaseAsync(listaCategorias, documentoBaseId);
                }

                //Anexos
                if (!string.IsNullOrEmpty(EditarDocumento.Anexos))
                {
                    //Anexo Adjuntos
                    var archivos = EditarDocumento.Anexos.Split(',').ToList();

                    //Guardar en la BD
                    await _anexoService.ActualizarAnexosDocumentoBaseAsync(archivos, ConstTipoAnexo.TipoAnexoN1, infoUsuarioClaims.Session, usuarioTitular, EditarDocumento.Folio, documentoBaseId);
                }

                return RedirectToPage("/Bandejas/Borradores");
            }

            EditarDocumento.Extensiones = await _configuracionService.ObtenerExtensionesEnCadena();
            EditarDocumento.Fecha = DateTime.Now.ToString("D", _cultureEs);
            EditarDocumento.Archivos = (documento.HER_AnexoId != null) ? documento.HER_Anexo.HER_AnexoArchivos : new List<HER_AnexoArchivo>();

            EditarDocumento.Para = EstadoDelegar ?
                    string.Join(",", documento.HER_Destinatarios.Where(x => x.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN1).Select(w => w.HER_UDestinatario.HER_UserName).ToArray()) :
                    string.Join(",", documento.HER_Destinatarios.Where(x => x.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN1).Select(w => w.HER_UDestinatario.HER_NombreCompleto).ToArray());

            EditarDocumento.CCP = EstadoDelegar ?
                    string.Join(",", documento.HER_Destinatarios.Where(x => x.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN2).Select(x => x.HER_UDestinatario.HER_UserName).ToArray()) :
                    string.Join(",", documento.HER_Destinatarios.Where(x => x.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN2).Select(x => x.HER_UDestinatario.HER_NombreCompleto).ToArray());

            EditarDocumento.RequiereRespuestaTexto = EstadoDelegar ?
                    string.Empty :
                    documento.HER_RequiereRespuesta ? "Si" : "No";
            EditarDocumento.Importancia = EstadoDelegar ?
                string.Empty :
                documento.HER_Importancia.HER_Nombre;
            EditarDocumento.Tipo = EstadoDelegar ?
                 string.Empty :
                 documento.HER_Tipo.HER_Nombre;
            EditarDocumento.Visibilidad = EstadoDelegar ?
                string.Empty :
                documento.HER_Visibilidad.HER_Nombre;

            EditarDocumento.PlantillasSelectList = new SelectList(await _plantillaService.ObtenerPlantillasAsync(usuarioTitular), "HER_Nombre", "HER_Nombre");
            EditarDocumento.ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre");
            EditarDocumento.TiposSelectList = new SelectList(await _tipoService.ObtenerTiposDocumentoAsync(), "HER_TipoDocumentoId", "HER_Nombre");
            EditarDocumento.VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre");

            Inhabiles = await _calendarioService.ObtenerDiasInhabilesCalendarioActualAsync(await _usuarioService.ObtenerFechaCompromisoAsync(DateTime.Now, usuarioCreadorId));

            return Page();
		}

	}
}