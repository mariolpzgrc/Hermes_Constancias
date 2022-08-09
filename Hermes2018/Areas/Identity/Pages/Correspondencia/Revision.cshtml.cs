using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Documento;
using Hermes2018.Models.Plantilla;
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
    public class RevisionModel : PageModel
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly IDocumentoService _documentoService;
        private readonly IPlantillaService _plantillaService;
        private readonly IImportanciaService _importanciaService;
        private readonly ITipoDocumentoService _tipoService;
        private readonly IVisibilidadService _visibilidadService;
        private readonly ICategoriaService _categoriaService;
        private readonly IAnexoService _anexoService;
        private readonly IConfiguracionService _configuracionService;

        public RevisionModel(IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService,
            IDocumentoService documentoService,
            IPlantillaService plantillaService,
            IImportanciaService importanciaService,
            ITipoDocumentoService tipoService,
            IVisibilidadService visibilidadService,
            ICategoriaService categoriaService,
            IAnexoService anexoService,
            IConfiguracionService configuracionService)
		{
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _documentoService = documentoService;
            _plantillaService = plantillaService;
            _importanciaService = importanciaService;
            _tipoService = tipoService;
            _visibilidadService = visibilidadService;
            _categoriaService = categoriaService;
            _anexoService = anexoService;
            _configuracionService = configuracionService;
        }

        [BindProperty]
        public RemitenteDocumentoViewModel Remitente { get; set; }

        [BindProperty]
		public DocumentoEnRevisionViewModel RevisarDocumento { get; set; }

        [BindProperty]
        [HiddenInput]
        public bool EstadoDelegar { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            EstadoDelegar = (!infoUsuarioClaims.ActivaDelegacion || (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN1));

            //--
            if (await _documentoService.ValidaRevisionAsync(id, infoUsuarioClaims.BandejaUsuario))
            {
                RevisarDocumento = await _documentoService.ObtenerEnvioRevisionDocumentoAsync(id, infoUsuarioClaims.BandejaUsuario);
                Remitente = await _usuarioService.ObtenerInfoPersonaDocumentoAsync(RevisarDocumento.RemitenteUsuario);

                RevisarDocumento.Extensiones = await _configuracionService.ObtenerExtensionesEnCadena();
                RevisarDocumento.FolioSession = infoUsuarioClaims.Session;
                RevisarDocumento.PlantillasSelectList = new SelectList(await _plantillaService.ObtenerPlantillasAsync(infoUsuarioClaims.BandejaUsuario), "HER_Nombre", "HER_Nombre");
                RevisarDocumento.ImportanciaSelectList = new SelectList(await _importanciaService.ObtenerTiposImportanciaAsync(), "HER_ImportanciaId", "HER_Nombre");
                RevisarDocumento.TiposSelectList = new SelectList(await _tipoService.ObtenerTiposDocumentoAsync(), "HER_TipoDocumentoId", "HER_Nombre");
                RevisarDocumento.VisibilidadSelectList = new SelectList(await _visibilidadService.ObtenerTiposVisibilidadAsync(), "HER_VisibilidadId", "HER_Nombre");

                ViewData["Bandeja"] = "Revision";
                return Page();
            }
            else
            {
                return RedirectToPage("/Bandejas/Revision");
            }
        }
        public async Task<IActionResult> OnPost()
		{
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            //---
            //Anexos
            if (!string.IsNullOrEmpty(RevisarDocumento.Anexos))
            {
                //Anexo Adjuntos
                var anexos = RevisarDocumento.Anexos.Split(',').ToList();

                //Guardar en la BD
                await _anexoService.ActualizarAnexosDocumentoBaseAsync(anexos, ConstTipoAnexo.TipoAnexoN1, infoUsuarioClaims.Session, usuarioTitular, RevisarDocumento.Folio, RevisarDocumento.DocumentoBaseId);
            }

            //Actualizar la informacion de Remitente
            var info = new ActualizarRevisionRemitenteViewModel()
            {
                DocumentoBaseId = RevisarDocumento.DocumentoBaseId,
                Folio = RevisarDocumento.Folio,
                RequiereRespuesta = RevisarDocumento.RequiereRespuesta,
                ImportanciaId = RevisarDocumento.ImportanciaId,
                TipoId = RevisarDocumento.TipoId,
                VisibilidadId = RevisarDocumento.VisibilidadId,
                NoInterno = RevisarDocumento.NoInterno,
                Cuerpo = RevisarDocumento.Cuerpo
            };

            var documentoBaseId = await _documentoService.ActualizarRevisionRemitenteAsync(info);

            //Categorias
            if (documentoBaseId > 0)
            {
                //Categorias a agregar
                List<string> listaNuevasCategorias = RevisarDocumento.Categorias.Split(',').ToList();

                //Crear asociación de categorias
                await _categoriaService.ActualizarCategoriasDocumentoBaseAsync(listaNuevasCategorias, documentoBaseId);
            }

            //Estado del oficio
            //------
            if (RevisarDocumento.EstadoRemitente == ConstEstadoRevision.EstadoRemitenteN2)
            {
                //--
                await _documentoService.EliminarEnvioRevisionAsync(RevisarDocumento.Folio);

                //Cambiar el estado a borrador y redirigir a la edición
                var estado = new EstadoDocumentoViewModel()
                {
                    Folio = RevisarDocumento.Folio,
                    Estado = ConstEstadoDocumento.EstadoDocumentoN1
                };
                await _documentoService.ActualizarEstadoDocumentoAsync(estado);

                return RedirectToPage("/Correspondencia/Editar", new { id = documentoBaseId });
            }
            else {
                return RedirectToPage("/Bandejas/Revision");
            }
        }
        public async Task<IActionResult> OnPostActualizarRemitente()
        {
            //Anexo Adjuntos
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            string usuarioTitular = infoUsuarioClaims.BandejaUsuario;
            
            //Anexos
            if (!string.IsNullOrEmpty(RevisarDocumento.Anexos))
            {
                //Anexo Adjuntos
                var anexos = RevisarDocumento.Anexos.Split(',').ToList();

                //Guardar en la BD
                await _anexoService.ActualizarAnexosDocumentoBaseAsync(anexos, ConstTipoAnexo.TipoAnexoN1, infoUsuarioClaims.Session, usuarioTitular, RevisarDocumento.Folio, RevisarDocumento.DocumentoBaseId);
            }

            //Actualiza la informacion de Remitente
            var info = new ActualizarRevisionRemitenteViewModel()
            {
                DocumentoBaseId = RevisarDocumento.DocumentoBaseId,
                Folio = RevisarDocumento.Folio,
                RequiereRespuesta = RevisarDocumento.RequiereRespuesta,
                ImportanciaId = RevisarDocumento.ImportanciaId,
                TipoId = RevisarDocumento.TipoId,
                VisibilidadId = RevisarDocumento.VisibilidadId,
                NoInterno = RevisarDocumento.NoInterno,
                Cuerpo = RevisarDocumento.Cuerpo
            };
            var documentoBaseId = await _documentoService.ActualizarRevisionRemitenteAsync(info);

            //Categorias
            if (documentoBaseId > 0)
            {
                //Categorias a agregar
                List<string> listaNuevasCategorias = RevisarDocumento.Categorias.Split(',').ToList();

                //Crear asociación de categorias
                await _categoriaService.ActualizarCategoriasDocumentoBaseAsync(listaNuevasCategorias, documentoBaseId);
            }

            return RedirectToPage("/Bandejas/Revision");
        }
        public async Task<IActionResult> OnPostTomarControl()
        {
            //Actualizar el estado del envio
            var estado = new ActualizarEstadoRevisionViewModel()
            {
                DocumentoBaseId = RevisarDocumento.DocumentoBaseId,
                Folio = RevisarDocumento.Folio,
                Fecha = DateTime.Now,
                Estado_Destinatario = ConstEstadoRevision.EstadoDestinatarioN2,
                Estado_Remitente = ConstEstadoRevision.EstadoRemitenteN2
            };
            var documentoBaseId = await _documentoService.ActualizarEstadoRevisionAsync(estado);

            return RedirectToPage("/Correspondencia/Revision", new { id = documentoBaseId });
        }
        public async Task<IActionResult> OnPostActualizarDestinatario()
        {
            //Actualizar la información de Destinatario
            var info = new ActualizarRevisionDestinatarioViewModel()
            {
                DocumentoBaseId = RevisarDocumento.DocumentoBaseId,
                Folio = RevisarDocumento.Folio,
                NoInterno = RevisarDocumento.NoInterno,
                Cuerpo = RevisarDocumento.Cuerpo
            };
            await _documentoService.ActualizarRevisionDestinatarioAsync(info);

            return RedirectToPage("/Bandejas/Revision");
        }
        public async Task<IActionResult> OnPostVoBoDestinatario()
        {
            //Actualiza información de Destinatario 
            var info = new ActualizarRevisionDestinatarioViewModel()
            {
                DocumentoBaseId = RevisarDocumento.DocumentoBaseId,
                Folio = RevisarDocumento.Folio,
                NoInterno = RevisarDocumento.NoInterno,
                Cuerpo = RevisarDocumento.Cuerpo
            };
            await _documentoService.ActualizarRevisionDestinatarioAsync(info);

            //Actualizar el estado del envio
            var estado = new ActualizarEstadoRevisionViewModel() {
                DocumentoBaseId = RevisarDocumento.DocumentoBaseId,
                Folio = RevisarDocumento.Folio,
                Fecha = DateTime.Now,
                Estado_Destinatario = ConstEstadoRevision.EstadoDestinatarioN2,
                Estado_Remitente = ConstEstadoRevision.EstadoRemitenteN2
            };
            await _documentoService.ActualizarEstadoRevisionAsync(estado);

            return RedirectToPage("/Bandejas/Revision");
        }
    }
}