using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Carpeta;
using Hermes2018.Models.Documento;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class HistoricoService : IHistoricoService
    {
        private readonly ApplicationDbContext _context;
        private readonly CultureInfo _cultureEs;

        public HistoricoService(ApplicationDbContext context)
        {
            _context = context;
            _cultureEs = new CultureInfo("es-MX");
        }

        public async Task<List<HistoricoPersonaViewModel>> ObtenerHistoricoPersonaAsync(string userName)
        {
            var historico = new List<HistoricoPersonaViewModel>();

            var usuariosQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Activo == false
                         && x.HER_Puesto != "Sin definir"
                         && x.HER_UserName == userName)
                .OrderByDescending(x => x.HER_FechaRegistro)
                .Select(x => new HistoricoPersonaViewModel
                 {
                     InfoUsuarioId = x.HER_InfoUsuarioId,
                     UserName = x.HER_UserName,
                     NombreCompleto = x.HER_NombreCompleto,
                     FechaRegistro = x.HER_FechaRegistro.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                     FechaActualizacion = x.HER_FechaActualizacion.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                     Region = x.HER_Area.HER_Region.HER_Nombre,
                     Area = x.HER_Area.HER_Nombre,
                     Puesto = x.HER_Puesto
                 })
                .AsNoTracking()
                .AsQueryable();
            //--
            historico = await usuariosQuery.ToListAsync();

            return historico;
        }
        public async Task<List<HistoricoAreaViewModel>> ObtenerHistoricoAreaAsync(int areaId)
        {
            var historico = new List<HistoricoAreaViewModel>();

            var usuariosQuery = _context.HER_InfoUsuario
                .Where(x => x.HER_Activo == false
                    && x.HER_AreaId == areaId
                    && x.HER_EsUnico == true)
                .OrderBy(x => x.HER_FechaRegistro)
                .AsNoTracking()
                .Select(x => new HistoricoAreaViewModel {
                    InfoUsuarioId = x.HER_InfoUsuarioId,
                    UserName = x.HER_UserName,
                    NombreCompleto = x.HER_NombreCompleto,
                    FechaRegistro = x.HER_FechaRegistro.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                    Puesto = x.HER_Puesto
                })
                .AsQueryable();
            //--
            historico = await usuariosQuery.ToListAsync();

            return historico;
        }
        public async Task<BandejasViewModel> ObtenerBandejasAsync(int infoUsuarioId)
        {
            var totales = new BandejasViewModel();

            //Recibidos
            var documentosRecibidosQuery = _context.HER_Recepcion
                              .Where(a => a.HER_Para.HER_InfoUsuarioId == infoUsuarioId
                                        && a.HER_Para.HER_Activo == false
                                        && a.HER_EstaLeido == false)
                              .AsNoTracking()
                              .AsQueryable();
            //--
            totales.Recibidos = await documentosRecibidosQuery.CountAsync();

            //DESTINATARIO (Los que recibe)
            var documentosRecibidosRevisionQuery = _context.HER_DocumentoBase
                .Where(x => x.HER_EstadoId == ConstEstadoDocumento.EstadoDocumentoN2
                         && x.HER_Revision.HER_RevisionDestinatario.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Revision.HER_RevisionDestinatario.HER_Activo == false
                         && x.HER_Revision.HER_EstadoDestinatario != ConstEstadoRevision.EstadoDestinatarioN2)
                .AsNoTracking()
                .AsQueryable();
            //--
            totales.Revision = await documentosRecibidosRevisionQuery.CountAsync();

            return totales;
        }

        public IQueryable<DocumentoRecibidoViewModel> ObtenerCorrespondenciaRecibidos(int infoUsuarioId)
        {
            IQueryable<DocumentoRecibidoViewModel> listadoRecibidos;

            //Busqueda
            var recepcionQuery = _context.HER_Recepcion
                       .Where(x => x.HER_Para.HER_InfoUsuarioId == infoUsuarioId
                                && x.HER_Para.HER_Activo == false)
                       .AsNoTracking()
                       .Select(x => new DocumentoRecibidoViewModel
                       {
                           EnvioId = x.HER_EnvioId,
                           FechaRecepcion = x.HER_FechaRecepcion,
                           Leido = x.HER_EstaLeido,
                           Estado = x.HER_EstadoEnvio.HER_Nombre,
                           TipoEnvio = x.HER_Envio.HER_TipoEnvioId,
                           Folio = x.HER_Envio.HER_Documento.HER_Folio,
                           Asunto = (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2) ? string.Format("{0} {1}", "", x.HER_Envio.HER_Documento.HER_Asunto) : x.HER_Envio.HER_Documento.HER_Asunto,
                           Adjuntos = (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1) ? (x.HER_Envio.HER_AnexoId != null) ? true : false : (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2)? (x.HER_Envio.HER_AnexoId != null) ? true : false : false,
                           Remitente = x.HER_Envio.HER_De.HER_NombreCompleto,
                           Importancia = (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1) ? (x.HER_Envio.HER_ImportanciaId == ConstImportancia.ImportanciaN1) ? true : false : (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2)? (x.HER_Envio.HER_ImportanciaId == ConstImportancia.ImportanciaN1) ? true : false: false,
                           //--
                           TipoPara = x.HER_TipoPara
                       })
                       .OrderByDescending(x => x.FechaRecepcion)
                       .AsQueryable();

            listadoRecibidos = recepcionQuery.OrderByDescending(x => x.FechaRecepcion);

            return listadoRecibidos;
        }
        public IQueryable<DocumentoEnviadoViewModel> ObtenerCorrespondenciaEnviados(int infoUsuarioId)
        {
            IQueryable<DocumentoEnviadoViewModel> listadoEnviados;
            //--
            //Busqueda DOCUMENTOS ENVIADOS y TURNADOS
            var enviosQuery = _context.HER_Envio
                .Where(a => a.HER_De.HER_InfoUsuarioId == infoUsuarioId
                         && a.HER_De.HER_Activo == false)
                .AsNoTracking()
                .Select(x => new DocumentoEnviadoViewModel
                {
                    EnvioId = x.HER_EnvioId,
                    FechaEnvio = x.HER_FechaEnvio,
                    TipoEnvio = x.HER_TipoEnvioId,
                    Estado = x.HER_EstadoEnvio.HER_Nombre,
                    Folio = x.HER_Documento.HER_Folio,
                    Asunto = (x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2) ? string.Format("{0} {1}", "", x.HER_Documento.HER_Asunto) : x.HER_Documento.HER_Asunto,
                    Adjuntos = (x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1) ? (x.HER_AnexoId != null) ? true : false : (x.HER_AnexoId != null) ? true : false,
                    Importancia = (x.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1) ? (x.HER_ImportanciaId == ConstImportancia.ImportanciaN1) ? true : false : (x.HER_ImportanciaId == ConstImportancia.ImportanciaN1) ? true : false,
                    Destinatario = x.HER_Recepciones.First().HER_Para.HER_NombreCompleto,
                    Destinatarios_Extras = (x.HER_Recepciones.Count() - 1)
                })
                .OrderByDescending(x => x.FechaEnvio)
                .AsQueryable();

            listadoEnviados = enviosQuery.OrderByDescending(x => x.FechaEnvio);

            return listadoEnviados;
        }
        public IQueryable<DocumentoBorradorViewModel> ObtenerCorrespondenciaBorradores(int infoUsuarioId)
        {
            IQueryable<DocumentoBorradorViewModel> listadoBorradorQuery = _context.HER_DocumentoBase
                 .Where(x => x.HER_EstadoId == ConstEstadoDocumento.EstadoDocumentoN1
                         && x.HER_DocumentoBaseTitular.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_DocumentoBaseTitular.HER_Activo == false)
                .AsNoTracking()
                .Select(x => new DocumentoBorradorViewModel
                {
                    DocumentoId = x.HER_DocumentoBaseId,
                    Folio = x.HER_Folio,
                    Remitente = x.HER_DocumentoBaseTitular.HER_NombreCompleto,
                    Destinatario = x.HER_Destinatarios.Where(y => y.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN1).First().HER_UDestinatario.HER_NombreCompleto,
                    Asunto = x.HER_Asunto,
                    Fecha = x.HER_FechaRegistro,
                    Tipo = x.HER_Tipo.HER_Nombre,
                    Importancia = (x.HER_Importancia.HER_Nombre == ConstImportancia.ImportanciaT1)? true: false,
                    Destinatarios_Extras = (x.HER_Destinatarios.Where(y => y.HER_Tipo == ConstTipoDestinatario.TipoDestinatarioN1).Count() - 1),
                    Adjuntos = (x.HER_AnexoId != null) ? true : false
                })
                .OrderByDescending(x => x.Fecha)
                .AsQueryable();

            return listadoBorradorQuery;
        }
        public IQueryable<DocumentoRevisionViewModel> ObtenerCorrespondenciaRevision(int infoUsuarioId)
        {
            IQueryable<DocumentoRevisionViewModel> listadoRevision;

            var revisionRemitenteQuery = _context.HER_DocumentoBase
                .Where(x => x.HER_EstadoId == ConstEstadoDocumento.EstadoDocumentoN2
                         && x.HER_Revision.HER_RevisionRemitente.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Revision.HER_RevisionRemitente.HER_Activo == false)
                .AsNoTracking()
                .Select(x => new DocumentoRevisionViewModel
                {
                    RevisionId = x.HER_Revision.HER_EnvioRevisionId,
                    Folio = x.HER_Folio,
                    Remitente = x.HER_Revision.HER_RevisionRemitente.HER_NombreCompleto,
                    Destinatario = x.HER_Revision.HER_RevisionDestinatario.HER_NombreCompleto,
                    Asunto = x.HER_Asunto,
                    Fecha = x.HER_FechaRegistro,
                    Tipo = x.HER_Tipo.HER_Nombre,
                    Importancia = (x.HER_Importancia.HER_Nombre == ConstImportancia.ImportanciaT1)? true : false,
                    TipoEnvio = ConstTipoEnvioRevision.TipoEnvioRevisionN1,
                    Estado = x.HER_Revision.HER_EstadoRemitente,
                    Adjuntos = (x.HER_AnexoId != null) ? true : false
                })
                .OrderByDescending(x => x.Fecha)
                .AsQueryable();

            var revisionDestinatarioQuery = _context.HER_DocumentoBase
                .Where(x => x.HER_EstadoId == ConstEstadoDocumento.EstadoDocumentoN2
                         && x.HER_Revision.HER_RevisionDestinatario.HER_InfoUsuarioId == infoUsuarioId
                         && x.HER_Revision.HER_RevisionDestinatario.HER_Activo == false
                         && x.HER_Revision.HER_EstadoDestinatario != ConstEstadoRevision.EstadoDestinatarioN2)
                .AsNoTracking()
                .Select(x => new DocumentoRevisionViewModel
                {
                    RevisionId = x.HER_Revision.HER_EnvioRevisionId,
                    Folio = x.HER_Folio,
                    Remitente = x.HER_Revision.HER_RevisionRemitente.HER_NombreCompleto,
                    Destinatario = x.HER_Revision.HER_RevisionDestinatario.HER_NombreCompleto,
                    Asunto = x.HER_Asunto,
                    Fecha = x.HER_FechaRegistro,
                    Tipo = x.HER_Tipo.HER_Nombre,
                    Importancia = (x.HER_Importancia.HER_Nombre == ConstImportancia.ImportanciaT1)? true : false,
                    TipoEnvio = ConstTipoEnvioRevision.TipoEnvioRevisionN2,
                    Estado = x.HER_Revision.HER_EstadoDestinatario,
                    Adjuntos = (x.HER_Revision.HER_DocumentoBase.HER_AnexoId != null) ? true : false,
                })
                .OrderByDescending(x => x.Fecha)
                .AsQueryable();

            listadoRevision = revisionRemitenteQuery.Union(revisionDestinatarioQuery)
                .OrderByDescending(x => x.Fecha);

            return listadoRevision;
        }

        //Lectura documento
        public async Task<HistoricoDocumentoLecturaViewModel> ObtenerDocumentoEnviadoSoloLecturaAsync(int envioId, string usuario)
        {
            var documentoEnviado = new HistoricoDocumentoLecturaViewModel();
            bool lectorEsRemitente = false;
            bool lectorEsDestinatario = false;

            //---[Actual]---
            HER_Envio envioActual = null;

            var envioActualQuery = _context.HER_Envio
                .Where(x => x.HER_EnvioId == envioId)
                //--
                .Include(x => x.HER_TipoEnvio)
                .Include(x => x.HER_EnvioPadre)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Visibilidad)
                .Include(x => x.HER_Importancia)
                .Include(x => x.HER_Anexo)
                .Include(x => x.HER_Carpeta)
                //--
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_Tipo)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoCreador)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoTitular)
                //--
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Region)
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Area_Padre)
                //--
                .AsNoTracking()
                .AsQueryable();

            var recepcionesActualQuery = _context.HER_Recepcion
                .Where(x => x.HER_Envio.HER_EnvioId == envioId)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Para)
                    .ThenInclude(x => x.HER_Area)
                //--
                .Include(x => x.HER_Compromiso)
                .AsNoTracking()
                .AsQueryable();

            envioActual = await envioActualQuery.FirstOrDefaultAsync();
            //--
            if (envioActual != null)
            {
                //De
                lectorEsRemitente = (envioActual.HER_De.HER_UserName == usuario);

                if (lectorEsRemitente)
                {
                    //[Actual]
                    //Remitente
                    //Visualización
                    documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Envio;
                    //--
                    documentoEnviado.Actual_FechaPropuesta = envioActual.HER_FechaPropuesta.ToString("dd/MM/yyyy", _cultureEs);
                    documentoEnviado.Actual_FechaCompromiso = string.Empty;
                    documentoEnviado.Actual_EstaLeido = true;
                    documentoEnviado.Actual_TieneRespuesta = false;
                    documentoEnviado.Actual_EstadoId = envioActual.HER_EstadoEnvioId;
                    documentoEnviado.Actual_Estado = envioActual.HER_EstadoEnvio.HER_Nombre;
                    documentoEnviado.Actual_CarpetaId = envioActual.HER_CarpetaId;
                    documentoEnviado.Actual_Fecha = envioActual.HER_FechaEnvio.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                    //Tipo de usuario que lee (Para o CCP)
                    documentoEnviado.Actual_UsuarioLee_Tipo = ConstTipoEmisor.TipoEmisorN3;
                    documentoEnviado.Actual_RequiereRespuesta = envioActual.HER_RequiereRespuesta;
                    //--
                    documentoEnviado.Actual_RecepcionId = 0;

                    //[Origen]
                    documentoEnviado.Origen_UsuarioPara_NombreCompleto = string.Empty;
                    documentoEnviado.Origen_UsuarioPara_AreaNombre = string.Empty;
                    documentoEnviado.Origen_UsuarioPara_PuestoNombre = string.Empty;
                    documentoEnviado.Origen_UsuarioPara_Tipo = 0;

                    documentoEnviado.Origen_RequiereRespuesta = envioActual.HER_RequiereRespuesta;
                    //--
                    if (documentoEnviado.Actual_CarpetaId != null)
                    {
                        documentoEnviado.Actual_NombreCarpeta = envioActual.HER_Carpeta.HER_Nombre;

                        if (envioActual.HER_Carpeta.HER_CarpetaPadreId != null)
                            documentoEnviado.Actual_NombreCarpetaPadre = envioActual.HER_Carpeta.HER_CarpetaPadre.HER_Nombre;
                        else
                            documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                    }
                    else
                    {
                        documentoEnviado.Actual_NombreCarpeta = string.Empty;
                        documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                    }
                    //--
                }
                else {
                    //Para
                    lectorEsDestinatario = await recepcionesActualQuery
                        .Where(x => x.HER_Para.HER_UserName == usuario)
                        .AnyAsync();

                    if (lectorEsDestinatario)
                    {
                        var recepcionActual = await recepcionesActualQuery
                            .Where(x => x.HER_Para.HER_UserName == usuario)
                            .FirstOrDefaultAsync();

                        //[Actual]
                        //Destinatario
                        //Visualización
                        documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Recepcion;
                        //--
                        documentoEnviado.Actual_FechaPropuesta = string.Empty;
                        documentoEnviado.Actual_FechaCompromiso = recepcionActual.HER_Compromiso.Count() > 0 ?
                            recepcionActual.HER_Compromiso
                            .Where(x => x.HER_Estado == ConstCompromiso.EstadoN1).Select(x => x.HER_Fecha).First().ToString("dd/MM/yyyy", _cultureEs)
                            : string.Empty;
                        documentoEnviado.Actual_EstaLeido = recepcionActual.HER_EstaLeido;
                        documentoEnviado.Actual_TieneRespuesta = recepcionActual.HER_TieneRespuesta;
                        documentoEnviado.Actual_EstadoId = recepcionActual.HER_EstadoEnvioId;
                        documentoEnviado.Actual_Estado = recepcionActual.HER_EstadoEnvio.HER_Nombre;
                        documentoEnviado.Actual_CarpetaId = recepcionActual.HER_CarpetaId;
                        documentoEnviado.Actual_Fecha = recepcionActual.HER_FechaRecepcion.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                        //Tipo de usuario que lee (Para o CCP)
                        documentoEnviado.Actual_UsuarioLee_Tipo = recepcionActual.HER_TipoPara;
                        documentoEnviado.Actual_RequiereRespuesta = (recepcionActual.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1) ? envioActual.HER_RequiereRespuesta : false;
                        //--
                        documentoEnviado.Actual_RecepcionId = recepcionActual.HER_RecepcionId;

                        //[Origen]
                        //Usuario Para
                        documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionActual.HER_Para.HER_NombreCompleto.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionActual.HER_Para.HER_Area.HER_Nombre.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionActual.HER_Para.HER_Puesto.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_Tipo = recepcionActual.HER_TipoPara;
                        //--
                        documentoEnviado.Origen_RequiereRespuesta = (recepcionActual.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1) ? envioActual.HER_RequiereRespuesta : false;

                        //--
                        if (documentoEnviado.Actual_CarpetaId != null)
                        {
                            documentoEnviado.Actual_NombreCarpeta = recepcionActual.HER_Carpeta.HER_Nombre;

                            if (recepcionActual.HER_Carpeta.HER_CarpetaPadreId != null)
                                documentoEnviado.Actual_NombreCarpetaPadre = recepcionActual.HER_Carpeta.HER_CarpetaPadre.HER_Nombre;
                            else
                                documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                        }
                        else
                        {
                            documentoEnviado.Actual_NombreCarpeta = string.Empty;
                            documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                        }
                    }
                }

                if (lectorEsRemitente || lectorEsDestinatario)
                {
                    var nombreUsuariosPara = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    var nombreUsuariosCCP = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    //[Actual]
                    //Envio 
                    documentoEnviado.Actual_AsuntoEnvio = envioActual.HER_Documento.HER_Asunto;
                    documentoEnviado.Actual_EnvioId = envioActual.HER_EnvioId;
                    documentoEnviado.Actual_TipoEnvioId = envioActual.HER_TipoEnvioId;
                    documentoEnviado.Actual_TipoEnvio = envioActual.HER_TipoEnvio.HER_Nombre;
                    documentoEnviado.Actual_EsReenvio = envioActual.HER_EsReenvio;
                    //Usuario Envia De
                    documentoEnviado.Actual_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                    documentoEnviado.Actual_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                    documentoEnviado.Actual_UsuarioDe_NombreUsuario = envioActual.HER_De.HER_UserName;
                    //Usuario Envia Para
                    documentoEnviado.Actual_UsuariosPara = string.Join(", ", nombreUsuariosPara.ToArray());
                    documentoEnviado.Actual_UsuariosCCP = string.Join(", ", nombreUsuariosCCP.ToArray());
                    //Turnado
                    documentoEnviado.Actual_EsTurnado = false;
                    documentoEnviado.Actual_Indicaciones = (envioActual.HER_EsReenvio) ? envioActual.HER_Indicaciones : string.Empty;
                    documentoEnviado.Actual_ExisteAdjuntos = (envioActual.HER_EsReenvio) ?
                        (envioActual.HER_AnexoId != null || envioActual.HER_EnvioPadre.HER_AnexoId != null) ? true : false
                        :
                        (envioActual.HER_AnexoId != null) ? true : false;
                    //--
                    documentoEnviado.Actual_Importancia = envioActual.HER_Importancia.HER_Nombre;
                    documentoEnviado.Actual_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;

                    //[Origen]
                    documentoEnviado.Origen_EnvioId = envioActual.HER_EnvioId;
                    documentoEnviado.Origen_TipoEnvioId = envioActual.HER_TipoEnvioId;
                    documentoEnviado.Origen_TipoEnvio = envioActual.HER_TipoEnvio.HER_Nombre;
                    //Documento
                    documentoEnviado.Origen_Folio = envioActual.HER_Documento.HER_Folio;
                    documentoEnviado.Origen_Asunto = envioActual.HER_Documento.HER_Asunto;
                    documentoEnviado.Origen_NoInterno = envioActual.HER_Documento.HER_NoInterno;
                    documentoEnviado.Origen_Cuerpo = envioActual.HER_Documento.HER_Cuerpo;
                    documentoEnviado.Origen_TipoDocumento = envioActual.HER_Documento.HER_Tipo.HER_Nombre;
                    documentoEnviado.Origen_TipoDocumentoId = envioActual.HER_Documento.HER_TipoId;
                    //--
                    documentoEnviado.Origen_NombreCreador = envioActual.HER_Documento.HER_DocumentoCreador.HER_NombreCompleto;
                    documentoEnviado.Origen_UsuarioCreador = envioActual.HER_Documento.HER_DocumentoCreador.HER_UserName;
                    documentoEnviado.Origen_NombreTitular = envioActual.HER_Documento.HER_DocumentoTitular.HER_NombreCompleto;
                    documentoEnviado.Origen_UsuarioTitular = envioActual.HER_Documento.HER_DocumentoTitular.HER_UserName;
                    //--
                    documentoEnviado.Origen_Fecha = envioActual.HER_FechaEnvio.ToString("D", _cultureEs);
                    documentoEnviado.Origen_Importancia = envioActual.HER_Importancia.HER_Nombre;
                    documentoEnviado.Origen_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;
                    documentoEnviado.Origen_ExisteAdjuntos = (envioActual.HER_AnexoId != null) ? true : false;
                    documentoEnviado.Origen_ListadoCcp = nombreUsuariosCCP.ToList();
                    //Usuario De
                    documentoEnviado.Origen_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                    documentoEnviado.Origen_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                    documentoEnviado.Origen_UsuarioDe_AreaPadreNombre = (envioActual.HER_De.HER_Area.HER_Area_PadreId != null) ? envioActual.HER_De.HER_Area.HER_Area_Padre.HER_Nombre : string.Empty;
                    documentoEnviado.Origen_UsuarioDe_AreaNombre = envioActual.HER_De.HER_Area.HER_Nombre;
                    documentoEnviado.Origen_UsuarioDe_AreaId = envioActual.HER_De.HER_Area.HER_AreaId;
                    documentoEnviado.Origen_UsuarioDe_Direccion = envioActual.HER_DeDireccion;
                    documentoEnviado.Origen_UsuarioDe_Telefono = envioActual.HER_DeTelefono;
                    documentoEnviado.Origen_UsuarioDe_Region = envioActual.HER_De.HER_Area.HER_Region.HER_Nombre;
                    documentoEnviado.Origen_UsuarioDe_PuestoNombre = envioActual.HER_De.HER_Puesto;
                }
            }

            return documentoEnviado;
        }
        public async Task<HistoricoDocumentoLecturaViewModel> ObtenerDocumentoTurnadoSoloLecturaAsync(int envioId, string usuario)
        {
            var documentoEnviado = new HistoricoDocumentoLecturaViewModel();
            bool lectorEsRemitente = false;
            bool lectorEsDestinatario = false;

            //---[Actual]---
            var envioActualQuery = _context.HER_Envio
                .Where(x => x.HER_EnvioId == envioId)
                //--
                .Include(x => x.HER_TipoEnvio)
                .Include(x => x.HER_EnvioPadre)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Visibilidad)
                .Include(x => x.HER_Importancia)
                .Include(x => x.HER_Anexo)
                .Include(x => x.HER_Carpeta)
                //--
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_Tipo)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoCreador)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoTitular)
                //--
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Region)
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Area_Padre)
                //--
                .AsNoTracking()
                .AsQueryable();

            var recepcionesActualQuery = _context.HER_Recepcion
                .Where(x => x.HER_Envio.HER_EnvioId == envioId)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Para)
                    .ThenInclude(x => x.HER_Area)
                //--
                .Include(x => x.HER_Compromiso)
                .AsNoTracking()
                .AsQueryable();

            var envioActual = await envioActualQuery.FirstOrDefaultAsync();
            //--
            if (envioActual != null)
            {
                //De
                lectorEsRemitente = (envioActual.HER_De.HER_UserName == usuario);

                if (lectorEsRemitente)
                {
                    //[Actual]
                    //Remitente
                    //Visualización
                    documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Envio;

                    documentoEnviado.Actual_FechaPropuesta = envioActual.HER_FechaPropuesta.ToString("dd/MM/yyyy", _cultureEs);
                    documentoEnviado.Actual_FechaCompromiso = string.Empty;
                    documentoEnviado.Actual_EstaLeido = true;
                    documentoEnviado.Actual_TieneRespuesta = false;
                    documentoEnviado.Actual_EstadoId = envioActual.HER_EstadoEnvioId;
                    documentoEnviado.Actual_Estado = envioActual.HER_EstadoEnvio.HER_Nombre;
                    documentoEnviado.Actual_CarpetaId = envioActual.HER_CarpetaId;
                    documentoEnviado.Actual_Fecha = envioActual.HER_FechaEnvio.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                    //Tipo de usuario que lee (Para o CCP)
                    documentoEnviado.Actual_UsuarioLee_Tipo = ConstTipoEmisor.TipoEmisorN3;
                    documentoEnviado.Actual_RequiereRespuesta = envioActual.HER_RequiereRespuesta;
                    //--
                    documentoEnviado.Actual_RecepcionId = 0;
                    //--
                    if (documentoEnviado.Actual_CarpetaId != null)
                    {
                        documentoEnviado.Actual_NombreCarpeta = envioActual.HER_Carpeta.HER_Nombre;

                        if (envioActual.HER_Carpeta.HER_CarpetaPadreId != null)
                            documentoEnviado.Actual_NombreCarpetaPadre = envioActual.HER_Carpeta.HER_CarpetaPadre.HER_Nombre;
                        else
                            documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                    }
                    else
                    {
                        documentoEnviado.Actual_NombreCarpeta = string.Empty;
                        documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                    }
                }
                else {
                    //Para
                    lectorEsDestinatario = await recepcionesActualQuery
                        .Where(x => x.HER_Para.HER_UserName == usuario)
                        .AnyAsync();

                    if (lectorEsDestinatario)
                    {
                        var recepcionActual = await recepcionesActualQuery
                                .Where(x => x.HER_Para.HER_UserName == usuario)
                                .FirstOrDefaultAsync();

                        //[Actual]
                        //Destinatario
                        //Visualización
                        documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Recepcion;
                        //Recepcion (Para)
                        documentoEnviado.Actual_FechaPropuesta = string.Empty;
                        documentoEnviado.Actual_FechaCompromiso = recepcionActual.HER_Compromiso
                            .Where(x => x.HER_Estado == ConstCompromiso.EstadoN1).Select(x => x.HER_Fecha).First().ToString("dd/MM/yyyy", _cultureEs);
                        documentoEnviado.Actual_EstaLeido = recepcionActual.HER_EstaLeido;
                        documentoEnviado.Actual_TieneRespuesta = recepcionActual.HER_TieneRespuesta;
                        documentoEnviado.Actual_EstadoId = recepcionActual.HER_EstadoEnvioId;
                        documentoEnviado.Actual_Estado = recepcionActual.HER_EstadoEnvio.HER_Nombre;
                        documentoEnviado.Actual_CarpetaId = recepcionActual.HER_CarpetaId;
                        documentoEnviado.Actual_Fecha = recepcionActual.HER_FechaRecepcion.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                        //Tipo de usuario que lee (Para o CCP)
                        documentoEnviado.Actual_UsuarioLee_Tipo = recepcionActual.HER_TipoPara;
                        documentoEnviado.Actual_RequiereRespuesta = (recepcionActual.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1) ? envioActual.HER_RequiereRespuesta : false;
                        //---
                        documentoEnviado.Actual_RecepcionId = recepcionActual.HER_RecepcionId;
                        //--
                        if (documentoEnviado.Actual_CarpetaId != null)
                        {
                            documentoEnviado.Actual_NombreCarpeta = recepcionActual.HER_Carpeta.HER_Nombre;

                            if (recepcionActual.HER_Carpeta.HER_CarpetaPadreId != null)
                                documentoEnviado.Actual_NombreCarpetaPadre = recepcionActual.HER_Carpeta.HER_CarpetaPadre.HER_Nombre;
                            else
                                documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                        }
                        else
                        {
                            documentoEnviado.Actual_NombreCarpeta = string.Empty;
                            documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                        }
                    }
                }

                if (lectorEsRemitente || lectorEsDestinatario)
                {
                    var nombreUsuariosPara = recepcionesActualQuery
                            .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                            .Select(y => y.HER_Para.HER_NombreCompleto)
                            .AsQueryable();

                    var nombreUsuariosCCP = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    //[Actual]
                    //Envio 
                    documentoEnviado.Actual_AsuntoEnvio = string.Format("{0} {1}", "", envioActual.HER_Documento.HER_Asunto);
                    documentoEnviado.Actual_EnvioId = envioActual.HER_EnvioId;
                    documentoEnviado.Actual_TipoEnvioId = envioActual.HER_TipoEnvioId;
                    documentoEnviado.Actual_TipoEnvio = envioActual.HER_TipoEnvio.HER_Nombre;
                    documentoEnviado.Actual_EsReenvio = envioActual.HER_EsReenvio;
                    //Usuario Envia De
                    documentoEnviado.Actual_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                    documentoEnviado.Actual_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                    documentoEnviado.Actual_UsuarioDe_NombreUsuario = envioActual.HER_De.HER_UserName;
                    //Usuario Envia Para
                    documentoEnviado.Actual_UsuariosPara = string.Join(", ", nombreUsuariosPara.ToArray());
                    documentoEnviado.Actual_UsuariosCCP = string.Join(", ", nombreUsuariosCCP.ToArray());
                    //Turnado
                    documentoEnviado.Actual_EsTurnado = true;
                    documentoEnviado.Actual_Indicaciones = envioActual.HER_Indicaciones;
                    documentoEnviado.Actual_ExisteAdjuntos = (envioActual.HER_AnexoId != null) ? true : false;
                    //--
                    documentoEnviado.Actual_Importancia = envioActual.HER_Importancia.HER_Nombre;
                    documentoEnviado.Actual_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;
                    //************************************************************************************************
                    //[Origen]
                    //Si es un turnado se busca recuperar la informacion origen
                    var envioOrigenQuery = _context.HER_Envio
                        .Where(x => x.HER_DocumentoId == envioActual.HER_DocumentoId && x.HER_GrupoEnvio == envioActual.HER_GrupoEnvio)
                        //--
                        .Include(x => x.HER_TipoEnvio)
                        .Include(x => x.HER_EnvioPadre)
                        .Include(x => x.HER_EstadoEnvio)
                        .Include(x => x.HER_Visibilidad)
                        .Include(x => x.HER_Importancia)
                        .Include(x => x.HER_Anexo)
                        .Include(x => x.HER_Carpeta)
                        //--
                        .Include(x => x.HER_Documento)
                            .ThenInclude(x => x.HER_Tipo)
                        .Include(x => x.HER_Documento)
                            .ThenInclude(x => x.HER_DocumentoCreador)
                        .Include(x => x.HER_Documento)
                            .ThenInclude(x => x.HER_DocumentoTitular)
                        //--
                        .Include(x => x.HER_De)
                            .ThenInclude(x => x.HER_Area)
                                .ThenInclude(x => x.HER_Region)
                        .Include(x => x.HER_De)
                            .ThenInclude(x => x.HER_Area)
                                .ThenInclude(x => x.HER_Area_Padre)
                        //--
                        .OrderBy(x => x.HER_Orden)
                        .AsNoTracking()
                        .AsQueryable();

                    var envioOrigen = await envioOrigenQuery.FirstOrDefaultAsync();

                    if (envioOrigen != null)
                    {
                        documentoEnviado.Origen_EnvioId = envioOrigen.HER_EnvioId;
                        documentoEnviado.Origen_TipoEnvioId = envioOrigen.HER_TipoEnvioId;
                        documentoEnviado.Origen_TipoEnvio = envioOrigen.HER_TipoEnvio.HER_Nombre;

                        //Documento
                        documentoEnviado.Origen_Folio = envioOrigen.HER_Documento.HER_Folio;
                        documentoEnviado.Origen_Asunto = envioOrigen.HER_Documento.HER_Asunto;
                        documentoEnviado.Origen_NoInterno = envioOrigen.HER_Documento.HER_NoInterno;
                        documentoEnviado.Origen_Fecha = envioOrigen.HER_FechaEnvio.ToString("D", _cultureEs);
                        documentoEnviado.Origen_Cuerpo = envioOrigen.HER_Documento.HER_Cuerpo;
                        documentoEnviado.Origen_TipoDocumento = envioOrigen.HER_Documento.HER_Tipo.HER_Nombre;
                        documentoEnviado.Origen_TipoDocumentoId = envioOrigen.HER_Documento.HER_TipoId;
                        documentoEnviado.Origen_Importancia = envioOrigen.HER_Importancia.HER_Nombre;
                        documentoEnviado.Origen_Visibilidad = envioOrigen.HER_Visibilidad.HER_Nombre;
                        documentoEnviado.Origen_ExisteAdjuntos = (envioOrigen.HER_EsReenvio) ?
                            (envioOrigen.HER_AnexoId != null || envioOrigen.HER_EnvioPadre.HER_AnexoId != null) ? true : false
                            :
                            (envioOrigen.HER_AnexoId != null) ? true : false;

                        //Usuario De
                        documentoEnviado.Origen_UsuarioDe_Correo = envioOrigen.HER_De.HER_Correo;
                        documentoEnviado.Origen_UsuarioDe_NombreCompleto = envioOrigen.HER_De.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioDe_AreaPadreNombre = (envioOrigen.HER_De.HER_Area.HER_Area_PadreId != null) ? envioOrigen.HER_De.HER_Area.HER_Area_Padre.HER_Nombre : string.Empty;
                        documentoEnviado.Origen_UsuarioDe_AreaNombre = envioOrigen.HER_De.HER_Area.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_AreaId = envioOrigen.HER_De.HER_Area.HER_AreaId;
                        documentoEnviado.Origen_UsuarioDe_Direccion = envioOrigen.HER_DeDireccion;
                        documentoEnviado.Origen_UsuarioDe_Telefono = envioOrigen.HER_DeTelefono;
                        documentoEnviado.Origen_UsuarioDe_Region = envioOrigen.HER_De.HER_Area.HER_Region.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_PuestoNombre = envioOrigen.HER_De.HER_Puesto;
                        documentoEnviado.Origen_RequiereRespuesta = envioOrigen.HER_RequiereRespuesta;

                        //--
                        documentoEnviado.Origen_NombreCreador = envioOrigen.HER_Documento.HER_DocumentoCreador.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioCreador = envioOrigen.HER_Documento.HER_DocumentoCreador.HER_UserName;
                        documentoEnviado.Origen_NombreTitular = envioOrigen.HER_Documento.HER_DocumentoTitular.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioTitular = envioOrigen.HER_Documento.HER_DocumentoTitular.HER_UserName;

                        //Recepción
                        var recepcionesOrigenQuery = _context.HER_Recepcion
                            .Where(x => x.HER_Envio.HER_EnvioId == envioOrigen.HER_EnvioId)
                            .Include(x => x.HER_EstadoEnvio)
                            .Include(x => x.HER_Para)
                                .ThenInclude(x => x.HER_Area)
                            .AsNoTracking()
                            .AsQueryable();

                        var recepcionOrigen = await recepcionesOrigenQuery
                            .Where(x => x.HER_ParaId == envioActual.HER_UsuarioOrigenId)
                            .FirstOrDefaultAsync();

                        if (recepcionOrigen != null)
                        {
                            var nombreUsuariosCCPOrigen = recepcionesOrigenQuery
                                .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                                .Select(y => y.HER_Para.HER_NombreCompleto)
                                .AsQueryable();

                            documentoEnviado.Origen_ListadoCcp = nombreUsuariosCCPOrigen.ToList();

                            //Usuario Para
                            documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionOrigen.HER_Para.HER_NombreCompleto.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionOrigen.HER_Para.HER_Area.HER_Nombre.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionOrigen.HER_Para.HER_Puesto.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_Tipo = recepcionOrigen.HER_TipoPara;
                            //--
                        }
                    }
                }
            }

            return documentoEnviado;
        }
        public async Task<HistoricoDocumentoLecturaViewModel> ObtenerDocumentoRespuestaEnviadoSoloLecturaAsync(int envioId, string usuario)
        {
            var documentoEnviado = new HistoricoDocumentoLecturaViewModel();
            bool lectorEsRemitente = false;
            bool lectorEsDestinatario = false;

            //---[Actual]---
            var envioActualQuery = _context.HER_Envio
                .Where(x => x.HER_EnvioId == envioId)
                //--
                .Include(x => x.HER_TipoEnvio)
                .Include(x => x.HER_EnvioPadre)
                    .ThenInclude(x => x.HER_TipoEnvio)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Visibilidad)
                .Include(x => x.HER_Importancia)
                .Include(x => x.HER_Anexo)
                .Include(x => x.HER_Carpeta)
                //--
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_Tipo)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoCreador)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoTitular)
                //--
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Region)
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Area_Padre)
                //--
                .AsNoTracking()
                .AsQueryable();

            var recepcionesActualQuery = _context.HER_Recepcion
                .Where(x => x.HER_Envio.HER_EnvioId == envioId)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Para)
                    .ThenInclude(x => x.HER_Area)
                //--
                .Include(x => x.HER_Compromiso)
                .AsNoTracking()
                .AsQueryable();

            var envioActual = await envioActualQuery.FirstOrDefaultAsync();

            if (envioActual != null)
            {
                var envioPadreQuery = _context.HER_Envio
                    .Where(x => x.HER_EnvioId == envioActual.HER_EnvioPadreId)
                    //--
                    .Include(x => x.HER_TipoEnvio)
                    .Include(x => x.HER_EnvioPadre)
                        .ThenInclude(x => x.HER_TipoEnvio)
                    .Include(x => x.HER_EstadoEnvio)
                    .Include(x => x.HER_Visibilidad)
                    .Include(x => x.HER_Importancia)
                    .Include(x => x.HER_Anexo)
                    .Include(x => x.HER_Carpeta)
                    //--
                    .Include(x => x.HER_Documento)
                        .ThenInclude(x => x.HER_Tipo)
                    .Include(x => x.HER_Documento)
                        .ThenInclude(x => x.HER_DocumentoCreador)
                    .Include(x => x.HER_Documento)
                        .ThenInclude(x => x.HER_DocumentoTitular)
                    //--
                    .Include(x => x.HER_De)
                        .ThenInclude(x => x.HER_Area)
                            .ThenInclude(x => x.HER_Region)
                    .Include(x => x.HER_De)
                        .ThenInclude(x => x.HER_Area)
                            .ThenInclude(x => x.HER_Area_Padre)
                    //--
                    .AsNoTracking()
                    .AsQueryable();

                var recepcionesPadreQuery = _context.HER_Recepcion
                   .Where(x => x.HER_Envio.HER_EnvioId == envioActual.HER_EnvioPadreId)
                   .Include(x => x.HER_EstadoEnvio)
                   .Include(x => x.HER_Para)
                       .ThenInclude(x => x.HER_Area)
                   //--
                   .Include(x => x.HER_Compromiso)
                   .AsNoTracking()
                   .AsQueryable();

                //De
                lectorEsRemitente = (envioActual.HER_De.HER_UserName == usuario);

                if (lectorEsRemitente)
                {
                    //[Actual]
                    //Remitente
                    var recepcion = await recepcionesActualQuery
                           .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                           .FirstOrDefaultAsync();

                    if (envioActual.HER_EsReenvio)
                    {
                        var recepcionPadre = await recepcionesPadreQuery
                          .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                          .FirstOrDefaultAsync();

                        //Usuario Destino(para Actual)
                        documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionPadre.HER_Para.HER_NombreCompleto.ToUpper();  
                        documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionPadre.HER_Para.HER_Area.HER_Nombre.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionPadre.HER_Para.HER_Puesto.ToUpper();
                    }
                    else {
                        //Usuario Destino(para Actual)
                        documentoEnviado.Origen_UsuarioPara_NombreCompleto = string.Empty; //recepcion.HER_Para.HER_NombreCompleto.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_AreaNombre = string.Empty;  //recepcion.HER_Para.HER_Area.HER_Nombre.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_PuestoNombre = string.Empty; //recepcion.HER_Para.HER_Puesto.HER_Nombre.ToUpper();
                    }

                    documentoEnviado.Origen_UsuarioPara_Tipo = 0;
                    //--

                    //Visualización
                    documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Envio;
                    documentoEnviado.Actual_EstaLeido = true;
                    //Tipo de usuario que lee (Para o CCP)
                    documentoEnviado.Actual_UsuarioLee_Tipo = ConstTipoEmisor.TipoEmisorN3;
                    
                    documentoEnviado.Actual_CarpetaId = envioActual.HER_CarpetaId;
                    documentoEnviado.Actual_Fecha = envioActual.HER_FechaEnvio.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                    documentoEnviado.Actual_FechaCompromiso = string.Empty;
                    documentoEnviado.Actual_FechaPropuesta = string.Empty;
                    //Estado para envio o recepción
                    documentoEnviado.Actual_EstadoId = envioActual.HER_EstadoEnvio.HER_EstadoEnvioId;
                    documentoEnviado.Actual_Estado = envioActual.HER_EstadoEnvio.HER_Nombre;
                    //--
                    documentoEnviado.Actual_RecepcionId = 0;
                    //--
                    if (documentoEnviado.Actual_CarpetaId != null)
                    {
                        documentoEnviado.Actual_NombreCarpeta = envioActual.HER_Carpeta.HER_Nombre;

                        if (envioActual.HER_Carpeta.HER_CarpetaPadreId != null)
                            documentoEnviado.Actual_NombreCarpetaPadre = envioActual.HER_Carpeta.HER_CarpetaPadre.HER_Nombre;
                        else
                            documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                    }
                    else
                    {
                        documentoEnviado.Actual_NombreCarpeta = string.Empty;
                        documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                    }
                }
                else
                {
                    //Para
                    lectorEsDestinatario = await recepcionesActualQuery
                        .Where(x => x.HER_Para.HER_UserName == usuario)
                        .AnyAsync();

                    if (lectorEsDestinatario)
                    {
                        var recepcionActual = await recepcionesActualQuery
                            .Where(x => x.HER_Para.HER_UserName == usuario)
                            .FirstOrDefaultAsync();

                        if (envioActual.HER_EsReenvio)
                        {
                            var recepcionPadre = await recepcionesPadreQuery
                                    .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                                    .FirstOrDefaultAsync();

                            //Usuario Destino(para Actual)
                            documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionPadre.HER_Para.HER_NombreCompleto.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionPadre.HER_Para.HER_Area.HER_Nombre.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionPadre.HER_Para.HER_Puesto.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_Tipo = recepcionPadre.HER_TipoPara;
                        }
                        else {
                            //Usuario Destino(para Actual)
                            documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionActual.HER_Para.HER_NombreCompleto.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionActual.HER_Para.HER_Area.HER_Nombre.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionActual.HER_Para.HER_Puesto.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_Tipo = recepcionActual.HER_TipoPara;
                        }

                        //[Actual]
                        //Destinatario
                        //Visualización
                        documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Recepcion;
                        documentoEnviado.Actual_EstaLeido = recepcionActual.HER_EstaLeido;

                        documentoEnviado.Actual_CarpetaId = recepcionActual.HER_CarpetaId;
                        documentoEnviado.Actual_Fecha = recepcionActual.HER_FechaRecepcion.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                        documentoEnviado.Actual_FechaCompromiso = string.Empty;
                        documentoEnviado.Actual_FechaPropuesta = string.Empty;
                        /*recepcionActual.HER_Compromiso
                        .Where(x => x.HER_Estado == ConstCompromiso.EstadoN1).Select(x => x.HER_Fecha).First().ToString("dd/MM/yyyy", _cultureEs);*/
                        //Estado para envio o recepción
                        documentoEnviado.Actual_EstadoId = recepcionActual.HER_EstadoEnvio.HER_EstadoEnvioId;
                        documentoEnviado.Actual_Estado = recepcionActual.HER_EstadoEnvio.HER_Nombre;
                        //Tipo de usuario que lee (Para o CCP)
                        documentoEnviado.Actual_UsuarioLee_Tipo = recepcionActual.HER_TipoPara;
                        //--
                        documentoEnviado.Actual_RecepcionId = recepcionActual.HER_RecepcionId;
                        //--
                        if (documentoEnviado.Actual_CarpetaId != null)
                        {
                            documentoEnviado.Actual_NombreCarpeta = recepcionActual.HER_Carpeta.HER_Nombre;

                            if (recepcionActual.HER_Carpeta.HER_CarpetaPadreId != null)
                                documentoEnviado.Actual_NombreCarpetaPadre = recepcionActual.HER_Carpeta.HER_CarpetaPadre.HER_Nombre;
                            else
                                documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                        }
                        else
                        {
                            documentoEnviado.Actual_NombreCarpeta = string.Empty;
                            documentoEnviado.Actual_NombreCarpetaPadre = string.Empty;
                        }
                    }
                }

                if (lectorEsRemitente || lectorEsDestinatario)
                {
                    var nombreUsuariosPara = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    var nombreUsuariosCCP = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    //[Actual]
                    //Envio 
                    documentoEnviado.Actual_AsuntoEnvio = envioActual.HER_Documento.HER_Asunto;
                    documentoEnviado.Actual_EnvioId = envioActual.HER_EnvioId;
                    documentoEnviado.Actual_RequiereRespuesta = false;
                    documentoEnviado.Actual_Importancia = string.Empty;
                    documentoEnviado.Actual_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;
                    documentoEnviado.Actual_TipoEnvioId = envioActual.HER_TipoEnvioId;
                    documentoEnviado.Actual_TipoEnvio = envioActual.HER_TipoEnvio.HER_Nombre;
                    documentoEnviado.Actual_EsReenvio = envioActual.HER_EsReenvio;
                    
                    //Usuario Envia De
                    documentoEnviado.Actual_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                    documentoEnviado.Actual_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                    documentoEnviado.Actual_UsuarioDe_NombreUsuario = envioActual.HER_De.HER_UserName;
                    //Usuario Envia Para
                    documentoEnviado.Actual_UsuariosPara = string.Join(", ", nombreUsuariosPara.ToArray());
                    documentoEnviado.Actual_UsuariosCCP = string.Join(", ", nombreUsuariosCCP.ToArray());
                    //Turnado
                    documentoEnviado.Actual_EsTurnado = false;
                    documentoEnviado.Actual_Indicaciones = string.Empty;
                    documentoEnviado.Actual_ExisteAdjuntos = (envioActual.HER_AnexoId != null) ? true : false;
                    //Respuesta
                    documentoEnviado.Actual_TieneRespuesta = false; //Como es una respuesta, no tiene respuesta
                    //---------------------------------

                    if (envioActual.HER_EsReenvio)
                    {
                        var envioPadre = await envioPadreQuery.FirstOrDefaultAsync();

                        var nombreUsuariosCCPPadre = recepcionesPadreQuery
                            .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                            .Select(y => y.HER_Para.HER_NombreCompleto)
                            .AsQueryable();

                        //[Origen]
                        documentoEnviado.Origen_EnvioId = (int)envioPadre.HER_EnvioId;
                        documentoEnviado.Origen_TipoEnvioId = envioPadre.HER_TipoEnvioId;
                        documentoEnviado.Origen_TipoEnvio = envioPadre.HER_TipoEnvio.HER_Nombre;

                        documentoEnviado.Origen_Folio = envioPadre.HER_Documento.HER_Folio;
                        documentoEnviado.Origen_Asunto = envioPadre.HER_Documento.HER_Asunto;
                        documentoEnviado.Origen_NoInterno = envioPadre.HER_Documento.HER_NoInterno;
                        documentoEnviado.Origen_Fecha = envioPadre.HER_FechaEnvio.ToString("D", _cultureEs);
                        documentoEnviado.Origen_Cuerpo = envioPadre.HER_Documento.HER_Cuerpo;

                        documentoEnviado.Origen_Importancia = string.Empty;
                        documentoEnviado.Origen_TipoDocumento = envioPadre.HER_Documento.HER_Tipo.HER_Nombre;
                        documentoEnviado.Origen_TipoDocumentoId = envioPadre.HER_Documento.HER_TipoId;
                        documentoEnviado.Origen_Visibilidad = envioPadre.HER_Visibilidad.HER_Nombre;
                        documentoEnviado.Origen_RequiereRespuesta = false;
                        documentoEnviado.Origen_ExisteAdjuntos = (envioPadre.HER_AnexoId != null) ? true : false;
                        documentoEnviado.Origen_ListadoCcp = nombreUsuariosCCPPadre.ToList();

                        documentoEnviado.Origen_NombreCreador = envioPadre.HER_Documento.HER_DocumentoCreador.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioCreador = envioPadre.HER_Documento.HER_DocumentoCreador.HER_UserName;
                        documentoEnviado.Origen_NombreTitular = envioPadre.HER_Documento.HER_DocumentoTitular.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioTitular = envioPadre.HER_Documento.HER_DocumentoTitular.HER_UserName;

                        //Usuario Origen
                        documentoEnviado.Origen_UsuarioDe_Correo = envioPadre.HER_De.HER_Correo;
                        documentoEnviado.Origen_UsuarioDe_NombreCompleto = envioPadre.HER_De.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioDe_AreaPadreNombre = (envioPadre.HER_De.HER_Area.HER_Area_PadreId != null) ? envioPadre.HER_De.HER_Area.HER_Area_Padre.HER_Nombre : string.Empty;
                        documentoEnviado.Origen_UsuarioDe_AreaNombre = envioPadre.HER_De.HER_Area.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_AreaId = envioPadre.HER_De.HER_Area.HER_AreaId;
                        documentoEnviado.Origen_UsuarioDe_Direccion = envioPadre.HER_DeDireccion;
                        documentoEnviado.Origen_UsuarioDe_Telefono = envioPadre.HER_DeTelefono;
                        documentoEnviado.Origen_UsuarioDe_Region = envioPadre.HER_De.HER_Area.HER_Region.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_PuestoNombre = envioPadre.HER_De.HER_Puesto;

                    }
                    else {
                        //[Origen]
                        documentoEnviado.Origen_EnvioId = (int)envioActual.HER_EnvioPadreId;
                        documentoEnviado.Origen_TipoEnvioId = envioActual.HER_EnvioPadre.HER_TipoEnvioId;
                        documentoEnviado.Origen_TipoEnvio = envioActual.HER_EnvioPadre.HER_TipoEnvio.HER_Nombre;

                        documentoEnviado.Origen_Folio = envioActual.HER_Documento.HER_Folio;
                        documentoEnviado.Origen_Asunto = envioActual.HER_Documento.HER_Asunto;
                        documentoEnviado.Origen_NoInterno = envioActual.HER_Documento.HER_NoInterno;
                        documentoEnviado.Origen_Fecha = envioActual.HER_FechaEnvio.ToString("D", _cultureEs);
                        documentoEnviado.Origen_Cuerpo = envioActual.HER_Documento.HER_Cuerpo;

                        documentoEnviado.Origen_Importancia = string.Empty;
                        documentoEnviado.Origen_TipoDocumento = envioActual.HER_Documento.HER_Tipo.HER_Nombre;
                        documentoEnviado.Origen_TipoDocumentoId = envioActual.HER_Documento.HER_TipoId;
                        documentoEnviado.Origen_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;
                        documentoEnviado.Origen_RequiereRespuesta = false;
                        documentoEnviado.Origen_ExisteAdjuntos = (envioActual.HER_AnexoId != null) ? true : false;
                        documentoEnviado.Origen_ListadoCcp = nombreUsuariosCCP.ToList();

                        documentoEnviado.Origen_NombreCreador = envioActual.HER_Documento.HER_DocumentoCreador.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioCreador = envioActual.HER_Documento.HER_DocumentoCreador.HER_UserName;
                        documentoEnviado.Origen_NombreTitular = envioActual.HER_Documento.HER_DocumentoTitular.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioTitular = envioActual.HER_Documento.HER_DocumentoTitular.HER_UserName;

                        //Usuario Origen
                        documentoEnviado.Origen_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                        documentoEnviado.Origen_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioDe_AreaPadreNombre = (envioActual.HER_De.HER_Area.HER_Area_PadreId != null) ? envioActual.HER_De.HER_Area.HER_Area_Padre.HER_Nombre : string.Empty;
                        documentoEnviado.Origen_UsuarioDe_AreaNombre = envioActual.HER_De.HER_Area.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_AreaId = envioActual.HER_De.HER_Area.HER_AreaId;
                        documentoEnviado.Origen_UsuarioDe_Direccion = envioActual.HER_DeDireccion;
                        documentoEnviado.Origen_UsuarioDe_Telefono = envioActual.HER_DeTelefono;
                        documentoEnviado.Origen_UsuarioDe_Region = envioActual.HER_De.HER_Area.HER_Region.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_PuestoNombre = envioActual.HER_De.HER_Puesto;
                    }
                }
            }

            return documentoEnviado;
        }

        //Visualización de documento
        public async Task<HistoricoDocumentoVisualizacionViewModel> ObtenerDocumentoEnviadoSoloVisualizacionAsync(int envioId, string usuario)
        {
            var documentoEnviado = new HistoricoDocumentoVisualizacionViewModel();
            bool lectorEsRemitente = false;
            bool lectorEsDestinatario = false;

            //---[Actual]---
            var envioActualQuery = _context.HER_Envio
                .Where(x => x.HER_EnvioId == envioId)
                .Include(x => x.HER_EnvioPadre) //Para detectar los anexos originales
                 //--
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Importancia)
                .Include(x => x.HER_Visibilidad)
                .Include(x => x.HER_TipoEnvio)
                //--
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_Tipo)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoCreador)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoTitular)
                 //--
                 .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Region)
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Area_Padre)
                //--
                .AsNoTracking()
                .AsQueryable();

            var recepcionesActualQuery = _context.HER_Recepcion
                .Where(x => x.HER_Envio.HER_EnvioId == envioId
                    && x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Para)
                    .ThenInclude(x => x.HER_Area)
                .Include(x => x.HER_Compromiso)
                .AsNoTracking()
                .AsQueryable();

            var envioActual = await envioActualQuery.FirstOrDefaultAsync();

            if (envioActual != null)
            {
                //De
                lectorEsRemitente = (envioActual.HER_De.HER_UserName == usuario);

                if (lectorEsRemitente)
                {
                    //[Actual]
                    //Remitente
                    //Visualización
                    documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Envio;
                    documentoEnviado.Actual_FechaPropuesta = envioActual.HER_FechaPropuesta.ToString("dd/MM/yyyy", _cultureEs);
                    documentoEnviado.Actual_FechaCompromiso = string.Empty;
                    documentoEnviado.Actual_TieneRespuesta = false;
                    documentoEnviado.Actual_EstadoId = envioActual.HER_EstadoEnvioId;
                    documentoEnviado.Actual_Estado = envioActual.HER_EstadoEnvio.HER_Nombre;
                    documentoEnviado.Actual_Fecha = envioActual.HER_FechaEnvio.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                    documentoEnviado.Actual_RequiereRespuesta = envioActual.HER_RequiereRespuesta;

                    //[Origen]
                    documentoEnviado.Origen_UsuarioPara_NombreCompleto = string.Empty;
                    documentoEnviado.Origen_UsuarioPara_AreaNombre = string.Empty;
                    documentoEnviado.Origen_UsuarioPara_PuestoNombre = string.Empty;
                    documentoEnviado.Actual_UsuarioLee_Tipo = ConstTipoEmisor.TipoEmisorN3;
                    documentoEnviado.Origen_RequiereRespuesta = envioActual.HER_RequiereRespuesta;
                }
                else {
                    //Para
                    lectorEsDestinatario = await recepcionesActualQuery
                        .Where(x => x.HER_Para.HER_UserName == usuario)
                        .AnyAsync();

                    if (lectorEsDestinatario)
                    {
                        var recepcionActual = await recepcionesActualQuery
                            .Where(x => x.HER_Para.HER_UserName == usuario)
                            .FirstOrDefaultAsync();

                        //[Actual]
                        //Destinatario
                        //Visualización
                        documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Recepcion;
                        documentoEnviado.Actual_FechaPropuesta = string.Empty;
                        documentoEnviado.Actual_FechaCompromiso = recepcionActual.HER_Compromiso
                            .Where(x => x.HER_Estado == ConstCompromiso.EstadoN1).Select(x => x.HER_Fecha).First().ToString("dd/MM/yyyy", _cultureEs);
                        documentoEnviado.Actual_TieneRespuesta = recepcionActual.HER_TieneRespuesta;
                        documentoEnviado.Actual_EstadoId = recepcionActual.HER_EstadoEnvioId;
                        documentoEnviado.Actual_Estado = recepcionActual.HER_EstadoEnvio.HER_Nombre;
                        documentoEnviado.Actual_Fecha = recepcionActual.HER_FechaRecepcion.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                        documentoEnviado.Actual_RequiereRespuesta = (recepcionActual.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1) ? envioActual.HER_RequiereRespuesta : false;

                        //[Origen]
                        documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionActual.HER_Para.HER_NombreCompleto.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionActual.HER_Para.HER_Area.HER_Nombre.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionActual.HER_Para.HER_Puesto.ToUpper();

                        //Tipo de usuario que lee (Para o CCP)
                        documentoEnviado.Actual_UsuarioLee_Tipo = recepcionActual.HER_TipoPara;
                        documentoEnviado.Origen_RequiereRespuesta = (recepcionActual.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1) ? envioActual.HER_RequiereRespuesta : false;
                    }
                }

                if (lectorEsRemitente || lectorEsDestinatario)
                {
                    var nombreUsuariosPara = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    var nombreUsuariosCCP = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    //[Actual]
                    //Envio
                    documentoEnviado.Actual_AsuntoEnvio = envioActual.HER_Documento.HER_Asunto;
                    documentoEnviado.Actual_EnvioId = envioActual.HER_EnvioId;
                    documentoEnviado.Actual_TipoEnvioId = envioActual.HER_TipoEnvioId;
                    documentoEnviado.Actual_EsReenvio = envioActual.HER_EsReenvio;
                    documentoEnviado.Actual_TipoEnvio = envioActual.HER_TipoEnvio.HER_Nombre;
                    //--
                    documentoEnviado.Actual_ExisteAdjuntos = (envioActual.HER_EsReenvio) ?
                                                                (envioActual.HER_AnexoId != null || envioActual.HER_EnvioPadre.HER_AnexoId != null) ? true : false
                                                             :
                                                                (envioActual.HER_AnexoId != null) ? true : false;
                    documentoEnviado.Actual_EsTurnado = false;
                    documentoEnviado.Actual_Indicaciones = (envioActual.HER_EsReenvio) ? envioActual.HER_Indicaciones : string.Empty;
                    //Usuario Envia De
                    documentoEnviado.Actual_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                    documentoEnviado.Actual_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                    //Usuario Envia Para
                    documentoEnviado.Actual_UsuariosPara = string.Join(", ", nombreUsuariosPara.ToArray());
                    documentoEnviado.Actual_UsuariosCCP = string.Join(", ", nombreUsuariosCCP.ToArray());
                    documentoEnviado.Actual_Importancia = envioActual.HER_Importancia.HER_Nombre;
                    documentoEnviado.Actual_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;

                    //[Origen]
                    //Documento Origen
                    documentoEnviado.Origen_EnvioId = envioActual.HER_EnvioId;
                    documentoEnviado.Origen_Folio = envioActual.HER_Documento.HER_Folio;
                    documentoEnviado.Origen_Asunto = envioActual.HER_Documento.HER_Asunto;
                    documentoEnviado.Origen_NoInterno = envioActual.HER_Documento.HER_NoInterno;
                    documentoEnviado.Origen_Cuerpo = envioActual.HER_Documento.HER_Cuerpo;
                    documentoEnviado.Origen_TipoDocumento = envioActual.HER_Documento.HER_Tipo.HER_Nombre;
                    documentoEnviado.Origen_TipoDocumentoId = envioActual.HER_Documento.HER_TipoId;
                    documentoEnviado.Origen_TipoEnvioId = envioActual.HER_TipoEnvioId;
                    documentoEnviado.Origen_TipoEnvio = envioActual.HER_TipoEnvio.HER_Nombre;
                    documentoEnviado.Origen_NombreCreador = envioActual.HER_Documento.HER_DocumentoCreador.HER_NombreCompleto;
                    documentoEnviado.Origen_UsuarioCreador = envioActual.HER_Documento.HER_DocumentoCreador.HER_UserName;
                    documentoEnviado.Origen_NombreTitular = envioActual.HER_Documento.HER_DocumentoTitular.HER_NombreCompleto;
                    documentoEnviado.Origen_UsuarioTitular = envioActual.HER_Documento.HER_DocumentoTitular.HER_UserName;
                    documentoEnviado.Origen_Fecha = envioActual.HER_FechaEnvio.ToString("D", _cultureEs);
                    documentoEnviado.Origen_Importancia = envioActual.HER_Importancia.HER_Nombre;
                    documentoEnviado.Origen_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;
                    documentoEnviado.Origen_ExisteAdjuntos = (envioActual.HER_AnexoId != null) ? true : false;
                    documentoEnviado.Origen_ListadoCcp = nombreUsuariosCCP.ToList();

                    //Usuario Origen
                    documentoEnviado.Origen_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                    documentoEnviado.Origen_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                    documentoEnviado.Origen_UsuarioDe_AreaPadreNombre = (envioActual.HER_De.HER_Area.HER_Area_PadreId != null) ? envioActual.HER_De.HER_Area.HER_Area_Padre.HER_Nombre : string.Empty;
                    documentoEnviado.Origen_UsuarioDe_AreaNombre = envioActual.HER_De.HER_Area.HER_Nombre;
                    documentoEnviado.Origen_UsuarioDe_AreaId = envioActual.HER_De.HER_Area.HER_AreaId;
                    documentoEnviado.Origen_UsuarioDe_Direccion = envioActual.HER_DeDireccion;
                    documentoEnviado.Origen_UsuarioDe_Telefono = envioActual.HER_DeTelefono;
                    documentoEnviado.Origen_UsuarioDe_Region = envioActual.HER_De.HER_Area.HER_Region.HER_Nombre;
                    documentoEnviado.Origen_UsuarioDe_PuestoNombre = envioActual.HER_De.HER_Puesto;
                }
            }

            return documentoEnviado;
        }
        public async Task<HistoricoDocumentoVisualizacionViewModel> ObtenerDocumentoTurnadoSoloVisualizacionAsync(int envioId, string usuario)
        {
            var documentoEnviado = new HistoricoDocumentoVisualizacionViewModel();
            bool lectorEsRemitente = false;
            bool lectorEsDestinatario = false;

            //---[Actual]---
            var envioActualQuery = _context.HER_Envio
                .Where(x => x.HER_EnvioId == envioId)
                //--
                .Include(x => x.HER_TipoEnvio)
                .Include(x => x.HER_EnvioPadre)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Visibilidad)
                .Include(x => x.HER_Importancia)
                .Include(x => x.HER_Anexo)
                .Include(x => x.HER_Carpeta)
                //--
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_Tipo)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoCreador)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoTitular)
                //--
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Region)
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Area_Padre)
                //--
                .AsNoTracking()
                .AsQueryable();

            var recepcionesActualQuery = _context.HER_Recepcion
                .Where(x => x.HER_Envio.HER_EnvioId == envioId)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Para)
                    .ThenInclude(x => x.HER_Area)
                .Include(x => x.HER_Compromiso)
                .AsNoTracking()
                .AsQueryable();

            var envioActual = await envioActualQuery.FirstOrDefaultAsync();

            if (envioActual != null)
            {
                //De
                lectorEsRemitente = (envioActual.HER_De.HER_UserName == usuario);

                if (lectorEsRemitente)
                {
                    //[Actual]
                    //Remitente
                    //Visualización
                    documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Envio;
                    documentoEnviado.Actual_FechaPropuesta = envioActual.HER_FechaPropuesta.ToString("dd/MM/yyyy", _cultureEs);
                    documentoEnviado.Actual_FechaCompromiso = string.Empty;
                    documentoEnviado.Actual_TieneRespuesta = false;
                    documentoEnviado.Actual_EstadoId = envioActual.HER_EstadoEnvioId;
                    documentoEnviado.Actual_Estado = envioActual.HER_EstadoEnvio.HER_Nombre;
                    documentoEnviado.Actual_Fecha = envioActual.HER_FechaEnvio.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                    documentoEnviado.Actual_RequiereRespuesta = envioActual.HER_RequiereRespuesta;

                    //Tipo de usuario que lee (Para o CCP)
                    documentoEnviado.Actual_UsuarioLee_Tipo = ConstTipoEmisor.TipoEmisorN3;
                }
                else
                {
                    //Para
                    lectorEsDestinatario = await recepcionesActualQuery
                        .Where(x => x.HER_Para.HER_UserName == usuario)
                        .AnyAsync();

                    if (lectorEsDestinatario)
                    {
                        var recepcionActual = await recepcionesActualQuery
                            .Where(x => x.HER_Para.HER_UserName == usuario)
                            .FirstOrDefaultAsync();

                        //[Actual]
                        //Destinatario
                        //Visualización
                        documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Recepcion;
                        documentoEnviado.Actual_FechaPropuesta = string.Empty;
                        documentoEnviado.Actual_FechaCompromiso = recepcionActual.HER_Compromiso
                            .Where(x => x.HER_Estado == ConstCompromiso.EstadoN1).Select(x => x.HER_Fecha).First().ToString("dd/MM/yyyy", _cultureEs);
                        documentoEnviado.Actual_TieneRespuesta = recepcionActual.HER_TieneRespuesta;
                        documentoEnviado.Actual_EstadoId = recepcionActual.HER_EstadoEnvioId;
                        documentoEnviado.Actual_Estado = recepcionActual.HER_EstadoEnvio.HER_Nombre;
                        documentoEnviado.Actual_Fecha = recepcionActual.HER_FechaRecepcion.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                        documentoEnviado.Actual_RequiereRespuesta = (recepcionActual.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1) ? envioActual.HER_RequiereRespuesta : false;

                        //Tipo de usuario que lee (Para o CCP)
                        documentoEnviado.Actual_UsuarioLee_Tipo = recepcionActual.HER_TipoPara;
                    }
                }

                if (lectorEsRemitente || lectorEsDestinatario)
                {
                    var nombreUsuariosPara = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    var nombreUsuariosCCP = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    //[Actual]
                    //Envio 
                    documentoEnviado.Actual_AsuntoEnvio = string.Format("{0} {1}", "", envioActual.HER_Documento.HER_Asunto);
                    documentoEnviado.Actual_EnvioId = envioActual.HER_EnvioId;
                    documentoEnviado.Actual_TipoEnvioId = envioActual.HER_TipoEnvioId;
                    documentoEnviado.Actual_EsReenvio = envioActual.HER_EsReenvio;
                    documentoEnviado.Actual_TipoEnvio = envioActual.HER_TipoEnvio.HER_Nombre;
                    //--
                    documentoEnviado.Actual_ExisteAdjuntos = (envioActual.HER_AnexoId != null) ? true : false;
                    documentoEnviado.Actual_EsTurnado = true;
                    documentoEnviado.Actual_Indicaciones = envioActual.HER_Indicaciones;
                    //Usuario Envia De
                    documentoEnviado.Actual_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                    documentoEnviado.Actual_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                    //Usuario Envia Para
                    documentoEnviado.Actual_UsuariosPara = string.Join(", ", nombreUsuariosPara.ToArray());
                    documentoEnviado.Actual_UsuariosCCP = string.Join(", ", nombreUsuariosCCP.ToArray());
                    documentoEnviado.Actual_Importancia = envioActual.HER_Importancia.HER_Nombre;
                    documentoEnviado.Actual_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;

                    //*********************************************************************************************************************************/
                    //---[Origen]---
                    HER_Envio envioOrigen = null;
                    //-------------------------------------
                    //Si es un turnado se busca recuperar la informacion origen
                    var envioOrigenQuery = _context.HER_Envio
                        .Where(x => x.HER_DocumentoId == envioActual.HER_DocumentoId && x.HER_GrupoEnvio == envioActual.HER_GrupoEnvio)
                        //--
                        .Include(x => x.HER_TipoEnvio)
                        .Include(x => x.HER_EnvioPadre)
                        .Include(x => x.HER_EstadoEnvio)
                        .Include(x => x.HER_Visibilidad)
                        .Include(x => x.HER_Importancia)
                        .Include(x => x.HER_Anexo)
                        .Include(x => x.HER_Carpeta)
                        //--
                        .Include(x => x.HER_Documento)
                            .ThenInclude(x => x.HER_Tipo)
                        .Include(x => x.HER_Documento)
                            .ThenInclude(x => x.HER_DocumentoCreador)
                        .Include(x => x.HER_Documento)
                            .ThenInclude(x => x.HER_DocumentoTitular)
                        //--
                        .Include(x => x.HER_De)
                            .ThenInclude(x => x.HER_Area)
                                .ThenInclude(x => x.HER_Region)
                        .Include(x => x.HER_De)
                            .ThenInclude(x => x.HER_Area)
                                .ThenInclude(x => x.HER_Area_Padre)
                        //--
                        .OrderBy(x => x.HER_Orden)
                        .AsNoTracking()
                        .AsQueryable();

                    envioOrigen = await envioOrigenQuery.FirstOrDefaultAsync();

                    if (envioOrigen != null)
                    {
                        //Documento
                        documentoEnviado.Origen_EnvioId = envioOrigen.HER_EnvioId;
                        documentoEnviado.Origen_Folio = envioOrigen.HER_Documento.HER_Folio;
                        documentoEnviado.Origen_Asunto = envioOrigen.HER_Documento.HER_Asunto;
                        documentoEnviado.Origen_NoInterno = envioOrigen.HER_Documento.HER_NoInterno;
                        documentoEnviado.Origen_Cuerpo = envioOrigen.HER_Documento.HER_Cuerpo;
                        documentoEnviado.Origen_TipoDocumento = envioOrigen.HER_Documento.HER_Tipo.HER_Nombre;
                        documentoEnviado.Origen_TipoDocumentoId = envioOrigen.HER_Documento.HER_TipoId;
                        documentoEnviado.Origen_TipoEnvioId = envioOrigen.HER_TipoEnvioId;
                        documentoEnviado.Origen_TipoEnvio = envioOrigen.HER_TipoEnvio.HER_Nombre;
                        documentoEnviado.Origen_Fecha = envioOrigen.HER_FechaEnvio.ToString("D", _cultureEs);
                        documentoEnviado.Origen_Importancia = envioOrigen.HER_Importancia.HER_Nombre;
                        documentoEnviado.Origen_Visibilidad = envioOrigen.HER_Visibilidad.HER_Nombre;
                        documentoEnviado.Origen_ExisteAdjuntos = (envioOrigen.HER_EsReenvio) ?
                                                                (envioOrigen.HER_AnexoId != null || envioOrigen.HER_EnvioPadre.HER_AnexoId != null) ? true : false
                                                            :
                                                                (envioOrigen.HER_AnexoId != null) ? true : false;
                        documentoEnviado.Origen_RequiereRespuesta = envioOrigen.HER_RequiereRespuesta;

                        //Usuario Origen
                        documentoEnviado.Origen_UsuarioDe_Correo = envioOrigen.HER_De.HER_Correo;
                        documentoEnviado.Origen_UsuarioDe_NombreCompleto = envioOrigen.HER_De.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioDe_AreaPadreNombre = (envioOrigen.HER_De.HER_Area.HER_Area_PadreId != null) ? envioOrigen.HER_De.HER_Area.HER_Area_Padre.HER_Nombre : string.Empty;
                        documentoEnviado.Origen_UsuarioDe_AreaNombre = envioOrigen.HER_De.HER_Area.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_AreaId = envioOrigen.HER_De.HER_Area.HER_AreaId;
                        documentoEnviado.Origen_UsuarioDe_Direccion = envioOrigen.HER_DeDireccion;
                        documentoEnviado.Origen_UsuarioDe_Telefono = envioOrigen.HER_DeTelefono;
                        documentoEnviado.Origen_UsuarioDe_Region = envioOrigen.HER_De.HER_Area.HER_Region.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_PuestoNombre = envioOrigen.HER_De.HER_Puesto;

                        documentoEnviado.Origen_NombreCreador = envioOrigen.HER_Documento.HER_DocumentoCreador.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioCreador = envioOrigen.HER_Documento.HER_DocumentoCreador.HER_UserName;
                        documentoEnviado.Origen_NombreTitular = envioOrigen.HER_Documento.HER_DocumentoTitular.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioTitular = envioOrigen.HER_Documento.HER_DocumentoTitular.HER_UserName;

                        //Recepción
                        var recepcionesOrigenQuery = _context.HER_Recepcion
                            .Where(x => x.HER_Envio.HER_EnvioId == envioOrigen.HER_EnvioId)
                            .Include(x => x.HER_EstadoEnvio)
                            .Include(x => x.HER_Para)
                                .ThenInclude(x => x.HER_Area)
                            .AsNoTracking()
                            .AsQueryable();

                        var recepcionOrigen = await recepcionesOrigenQuery
                            .Where(x => x.HER_ParaId == envioActual.HER_UsuarioOrigenId)
                            .FirstOrDefaultAsync();

                        if (recepcionOrigen != null)
                        {
                            var nombreUsuariosCCPOrigen = recepcionesOrigenQuery
                                .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                                .Select(y => y.HER_Para.HER_NombreCompleto)
                                .AsQueryable();

                            documentoEnviado.Origen_ListadoCcp = nombreUsuariosCCPOrigen.ToList();

                            //Usuario Destino(para Actual)
                            documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionOrigen.HER_Para.HER_NombreCompleto.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionOrigen.HER_Para.HER_Area.HER_Nombre.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionOrigen.HER_Para.HER_Puesto.ToUpper();
                        }
                    }
                }
            }

            return documentoEnviado;
        }
        public async Task<HistoricoDocumentoVisualizacionViewModel> ObtenerDocumentoRespuestaEnviadoSoloVisualizacionAsync(int envioId, string usuario)
        {
            var documentoEnviado = new HistoricoDocumentoVisualizacionViewModel();
            bool lectorEsRemitente = false;
            bool lectorEsDestinatario = false;

            //---[Actual]---
            //--------------------------------------
            var envioActualQuery = _context.HER_Envio
                .Where(x => x.HER_EnvioId == envioId)
                //--
                .Include(x => x.HER_TipoEnvio)
                .Include(x => x.HER_EnvioPadre)
                    .ThenInclude(x => x.HER_TipoEnvio)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Visibilidad)
                .Include(x => x.HER_Importancia)
                .Include(x => x.HER_Anexo)
                .Include(x => x.HER_Carpeta)
                //--
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_Tipo)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoCreador)
                .Include(x => x.HER_Documento)
                    .ThenInclude(x => x.HER_DocumentoTitular)
                //--
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Region)
                .Include(x => x.HER_De)
                    .ThenInclude(x => x.HER_Area)
                        .ThenInclude(x => x.HER_Area_Padre)
                //--
                .AsNoTracking()
                .AsQueryable();

            var recepcionesActualQuery = _context.HER_Recepcion
                .Where(x => x.HER_Envio.HER_EnvioId == envioId)
                .Include(x => x.HER_EstadoEnvio)
                .Include(x => x.HER_Para)
                    .ThenInclude(x => x.HER_Area)
                .AsNoTracking()
                .AsQueryable();

            var envioActual = await envioActualQuery.FirstOrDefaultAsync();

            if (envioActual != null)
            {
                var envioPadreQuery = _context.HER_Envio
                    .Where(x => x.HER_EnvioId == envioActual.HER_EnvioPadreId)
                    //--
                    .Include(x => x.HER_TipoEnvio)
                    .Include(x => x.HER_EnvioPadre)
                        .ThenInclude(x => x.HER_TipoEnvio)
                    .Include(x => x.HER_EstadoEnvio)
                    .Include(x => x.HER_Visibilidad)
                    .Include(x => x.HER_Importancia)
                    .Include(x => x.HER_Anexo)
                    .Include(x => x.HER_Carpeta)
                    //--
                    .Include(x => x.HER_Documento)
                        .ThenInclude(x => x.HER_Tipo)
                    .Include(x => x.HER_Documento)
                        .ThenInclude(x => x.HER_DocumentoCreador)
                    .Include(x => x.HER_Documento)
                        .ThenInclude(x => x.HER_DocumentoTitular)
                    //--
                    .Include(x => x.HER_De)
                        .ThenInclude(x => x.HER_Area)
                            .ThenInclude(x => x.HER_Region)
                    .Include(x => x.HER_De)
                        .ThenInclude(x => x.HER_Area)
                            .ThenInclude(x => x.HER_Area_Padre)
                    //--
                    .AsNoTracking()
                    .AsQueryable();

                var recepcionesPadreQuery = _context.HER_Recepcion
                    .Where(x => x.HER_Envio.HER_EnvioId == envioActual.HER_EnvioPadreId)
                    .Include(x => x.HER_EstadoEnvio)
                    .Include(x => x.HER_Para)
                        .ThenInclude(x => x.HER_Area)
                    //--
                    .Include(x => x.HER_Compromiso)
                    .AsNoTracking()
                    .AsQueryable();

                //De
                lectorEsRemitente = (envioActual.HER_De.HER_UserName == usuario);

                if (lectorEsRemitente)
                {
                    //[Actual]
                    //Remitente
                    var recepcion = await recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                        .FirstOrDefaultAsync();

                    if (envioActual.HER_EsReenvio)
                    {
                        var recepcionPadre = await recepcionesPadreQuery
                          .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                          .FirstOrDefaultAsync();

                        //Usuario Destino(para Actual)
                        documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionPadre.HER_Para.HER_NombreCompleto.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionPadre.HER_Para.HER_Area.HER_Nombre.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionPadre.HER_Para.HER_Puesto.ToUpper();
                    }
                    else{
                        //Usuario Destino(para Actual)
                        documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcion.HER_Para.HER_NombreCompleto.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcion.HER_Para.HER_Area.HER_Nombre.ToUpper();
                        documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcion.HER_Para.HER_Puesto.ToUpper();
                    }

                    documentoEnviado.Actual_Fecha = envioActual.HER_FechaEnvio.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                    //Visualización
                    documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Envio;
                    //Tipo de usuario que lee (Para o CCP)
                    documentoEnviado.Actual_UsuarioLee_Tipo = ConstTipoEmisor.TipoEmisorN3;

                    documentoEnviado.Actual_EstadoId = recepcion.HER_EstadoEnvioId;
                    documentoEnviado.Actual_Estado = recepcion.HER_EstadoEnvio.HER_Nombre;
                }
                else
                {
                    //Para
                    lectorEsDestinatario = await recepcionesActualQuery
                        .Where(x => x.HER_Para.HER_UserName == usuario)
                        .AnyAsync();

                    if (lectorEsDestinatario)
                    {
                        var recepcionActual = await recepcionesActualQuery
                            .Where(x => x.HER_Para.HER_UserName == usuario)
                            .FirstOrDefaultAsync();

                        if (envioActual.HER_EsReenvio)
                        {
                            var recepcionPadre = await recepcionesPadreQuery
                                    .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                                    .FirstOrDefaultAsync();

                            //[Actual]
                            //Destinatario
                            //Usuario Destino(para Actual)
                            documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionPadre.HER_Para.HER_NombreCompleto.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionPadre.HER_Para.HER_Area.HER_Nombre.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionPadre.HER_Para.HER_Puesto.ToUpper();
                        }
                        else {
                            //[Actual]
                            //Destinatario
                            //Usuario Destino(para Actual)
                            documentoEnviado.Origen_UsuarioPara_NombreCompleto = recepcionActual.HER_Para.HER_NombreCompleto.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_AreaNombre = recepcionActual.HER_Para.HER_Area.HER_Nombre.ToUpper();
                            documentoEnviado.Origen_UsuarioPara_PuestoNombre = recepcionActual.HER_Para.HER_Puesto.ToUpper();
                        }

                        documentoEnviado.Actual_Fecha = recepcionActual.HER_FechaRecepcion.ToString("dd/MM/yyyy HH:mm", _cultureEs);
                        //Visualización
                        documentoEnviado.Actual_Visualizacion_Tipo = ConstVisualizacionEnvio.Recepcion;
                        //Tipo de usuario que lee (Para o CCP)
                        documentoEnviado.Actual_UsuarioLee_Tipo = recepcionActual.HER_TipoPara;

                        documentoEnviado.Actual_EstadoId = recepcionActual.HER_EstadoEnvioId;
                        documentoEnviado.Actual_Estado = recepcionActual.HER_EstadoEnvio.HER_Nombre;
                    }
                }

                if (lectorEsRemitente || lectorEsDestinatario)
                {
                    var nombreUsuariosPara = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN1)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    var nombreUsuariosCCP = recepcionesActualQuery
                        .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                        .Select(y => y.HER_Para.HER_NombreCompleto)
                        .AsQueryable();

                    //Envio 
                    documentoEnviado.Actual_EnvioId = envioActual.HER_EnvioId;
                    documentoEnviado.Actual_RequiereRespuesta = false;
                    documentoEnviado.Actual_Importancia = string.Empty;
                    documentoEnviado.Actual_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;
                    documentoEnviado.Actual_TipoEnvioId = envioActual.HER_TipoEnvioId;
                    documentoEnviado.Actual_EsReenvio = envioActual.HER_EsReenvio;
                    documentoEnviado.Actual_TipoEnvio = envioActual.HER_TipoEnvio.HER_Nombre;
                    //--
                    documentoEnviado.Actual_ExisteAdjuntos = (envioActual.HER_AnexoId != null) ? true : false;
                    documentoEnviado.Actual_EsTurnado = false;
                    documentoEnviado.Actual_Indicaciones = string.Empty;
                    //Usuario Envia De
                    documentoEnviado.Actual_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                    documentoEnviado.Actual_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                    //Usuario Envia Para
                    documentoEnviado.Actual_UsuariosPara = string.Join(", ", nombreUsuariosPara.ToArray());
                    documentoEnviado.Actual_UsuariosCCP = string.Join(", ", nombreUsuariosCCP.ToArray());
                    //Estado para la recepción
                    documentoEnviado.Actual_FechaCompromiso = string.Empty;
                    documentoEnviado.Actual_FechaPropuesta = string.Empty;

                    //Respuesta
                    documentoEnviado.Actual_TieneRespuesta = false;
                    //---------------------------------

                    if (envioActual.HER_EsReenvio)
                    {
                        var envioPadre = await envioPadreQuery.FirstOrDefaultAsync();

                        var nombreUsuariosCCPPadre = recepcionesPadreQuery
                            .Where(x => x.HER_TipoPara == ConstTipoDestinatario.TipoDestinatarioN2)
                            .Select(y => y.HER_Para.HER_NombreCompleto)
                            .AsQueryable();

                        //Documento Origen
                        documentoEnviado.Origen_EnvioId = envioPadre.HER_EnvioId;
                        documentoEnviado.Origen_Folio = envioPadre.HER_Documento.HER_Folio;
                        documentoEnviado.Origen_Asunto = envioPadre.HER_Documento.HER_Asunto;
                        documentoEnviado.Origen_NoInterno = envioPadre.HER_Documento.HER_NoInterno;
                        documentoEnviado.Origen_Fecha = envioPadre.HER_FechaEnvio.ToString("D", _cultureEs);
                        documentoEnviado.Origen_Cuerpo = envioPadre.HER_Documento.HER_Cuerpo;
                        //--
                        documentoEnviado.Origen_Importancia = string.Empty;
                        documentoEnviado.Origen_Visibilidad = envioPadre.HER_Visibilidad.HER_Nombre;
                        documentoEnviado.Origen_TipoDocumento = envioPadre.HER_Documento.HER_Tipo.HER_Nombre;
                        documentoEnviado.Origen_TipoDocumentoId = envioPadre.HER_Documento.HER_TipoId;
                        documentoEnviado.Origen_TipoEnvioId = envioPadre.HER_TipoEnvioId;
                        documentoEnviado.Origen_TipoEnvio = envioPadre.HER_TipoEnvio.HER_Nombre;
                        documentoEnviado.Origen_RequiereRespuesta = false;
                        documentoEnviado.Origen_ExisteAdjuntos = (envioPadre.HER_AnexoId != null) ? true : false;
                        //--
                        documentoEnviado.Origen_ListadoCcp = nombreUsuariosCCPPadre.ToList();

                        documentoEnviado.Origen_NombreCreador = envioPadre.HER_Documento.HER_DocumentoCreador.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioCreador = envioPadre.HER_Documento.HER_DocumentoCreador.HER_UserName;
                        documentoEnviado.Origen_NombreTitular = envioPadre.HER_Documento.HER_DocumentoTitular.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioTitular = envioPadre.HER_Documento.HER_DocumentoTitular.HER_UserName;

                        //Usuario Origen
                        documentoEnviado.Origen_UsuarioDe_Correo = envioPadre.HER_De.HER_Correo;
                        documentoEnviado.Origen_UsuarioDe_NombreCompleto = envioPadre.HER_De.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioDe_AreaPadreNombre = (envioPadre.HER_De.HER_Area.HER_Area_PadreId != null) ? envioPadre.HER_De.HER_Area.HER_Area_Padre.HER_Nombre : string.Empty;
                        documentoEnviado.Origen_UsuarioDe_AreaNombre = envioPadre.HER_De.HER_Area.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_AreaId = envioPadre.HER_De.HER_Area.HER_AreaId;
                        documentoEnviado.Origen_UsuarioDe_Direccion = envioPadre.HER_DeDireccion;
                        documentoEnviado.Origen_UsuarioDe_Telefono = envioPadre.HER_DeTelefono;
                        documentoEnviado.Origen_UsuarioDe_Region = envioPadre.HER_De.HER_Area.HER_Region.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_PuestoNombre = envioPadre.HER_De.HER_Puesto;
                    }
                    else {
                        //Documento Origen
                        documentoEnviado.Origen_EnvioId = envioActual.HER_EnvioId;
                        documentoEnviado.Origen_Folio = envioActual.HER_Documento.HER_Folio;
                        documentoEnviado.Origen_Asunto = envioActual.HER_Documento.HER_Asunto;
                        documentoEnviado.Origen_NoInterno = envioActual.HER_Documento.HER_NoInterno;
                        documentoEnviado.Origen_Fecha = envioActual.HER_FechaEnvio.ToString("D", _cultureEs);
                        documentoEnviado.Origen_Cuerpo = envioActual.HER_Documento.HER_Cuerpo;
                        //--
                        documentoEnviado.Origen_Importancia = string.Empty;
                        documentoEnviado.Origen_Visibilidad = envioActual.HER_Visibilidad.HER_Nombre;
                        documentoEnviado.Origen_TipoDocumento = envioActual.HER_Documento.HER_Tipo.HER_Nombre;
                        documentoEnviado.Origen_TipoDocumentoId = envioActual.HER_Documento.HER_TipoId;
                        documentoEnviado.Origen_TipoEnvioId = envioActual.HER_TipoEnvioId;
                        documentoEnviado.Origen_TipoEnvio = envioActual.HER_TipoEnvio.HER_Nombre;
                        documentoEnviado.Origen_RequiereRespuesta = false;
                        documentoEnviado.Origen_ExisteAdjuntos = (envioActual.HER_AnexoId != null) ? true : false;
                        //--
                        documentoEnviado.Origen_ListadoCcp = nombreUsuariosCCP.ToList();

                        documentoEnviado.Origen_NombreCreador = envioActual.HER_Documento.HER_DocumentoCreador.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioCreador = envioActual.HER_Documento.HER_DocumentoCreador.HER_UserName;
                        documentoEnviado.Origen_NombreTitular = envioActual.HER_Documento.HER_DocumentoTitular.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioTitular = envioActual.HER_Documento.HER_DocumentoTitular.HER_UserName;

                        //Usuario Origen
                        documentoEnviado.Origen_UsuarioDe_Correo = envioActual.HER_De.HER_Correo;
                        documentoEnviado.Origen_UsuarioDe_NombreCompleto = envioActual.HER_De.HER_NombreCompleto;
                        documentoEnviado.Origen_UsuarioDe_AreaPadreNombre = (envioActual.HER_De.HER_Area.HER_Area_PadreId != null) ? envioActual.HER_De.HER_Area.HER_Area_Padre.HER_Nombre : string.Empty;
                        documentoEnviado.Origen_UsuarioDe_AreaNombre = envioActual.HER_De.HER_Area.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_AreaId = envioActual.HER_De.HER_Area.HER_AreaId;
                        documentoEnviado.Origen_UsuarioDe_Direccion = envioActual.HER_DeDireccion;
                        documentoEnviado.Origen_UsuarioDe_Telefono = envioActual.HER_DeTelefono;
                        documentoEnviado.Origen_UsuarioDe_Region = envioActual.HER_De.HER_Area.HER_Region.HER_Nombre;
                        documentoEnviado.Origen_UsuarioDe_PuestoNombre = envioActual.HER_De.HER_Puesto;
                    }

                    documentoEnviado.Actual_AsuntoEnvio = envioActual.HER_Documento.HER_Asunto;
                }
            }

            return documentoEnviado;
        }

        public async Task<List<HistoricoResumenDestinatarioViewModel>> ObtenerResumenDestinatariosAsync(int envioId)
        {
            var destinatariosQuery = _context.HER_Recepcion
                .Where(x => x.HER_EnvioId == envioId)
                .OrderBy(x => x.HER_TipoPara)
                .AsNoTracking()
                .Select(x => new HistoricoResumenDestinatarioViewModel
                    {
                        RecepcionId = x.HER_RecepcionId,
                        NombreCompleto = x.HER_Para.HER_NombreCompleto,
                        FechaRecepcion = x.HER_FechaRecepcion.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                        FechaCompromiso = (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 || x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2) ?
                             (from co in x.HER_Compromiso where co.HER_Estado == ConstCompromiso.EstadoN1 select co.HER_Fecha.ToString("dd/MM/yyyy", _cultureEs)).FirstOrDefault()
                             :
                             string.Empty,
                        EstadoEnvioId = x.HER_EstadoEnvioId,
                        EstadoEnvio = x.HER_EstadoEnvio.HER_Nombre,
                        EstaLeido = x.HER_EstaLeido,
                        TieneRespuesta = x.HER_TieneRespuesta,
                        EnvioRespuestaId = (x.HER_TieneRespuesta) ? x.HER_EnvioId : 0,
                        TipoEnvioRespuesta = (x.HER_TieneRespuesta) ? x.HER_Envio.HER_TipoEnvioId : 0,
                        FechasCompromiso = (x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 || x.HER_Envio.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN2) ?
                        (from co in x.HER_Compromiso
                         orderby co.HER_Registro descending
                         select new HistoricoFechasCompromisoDestinatariosViewModel
                         {
                             CompromisoId = co.HER_CompromisoId,
                             Registro = co.HER_Registro.ToString("dd/MM/yyyy HH:mm 'hrs.'", _cultureEs),
                             Fecha = co.HER_Fecha.ToString("dd/MM/yyyy", _cultureEs),
                             Tipo = co.HER_Tipo,
                             Estado = co.HER_Estado
                         }).ToList()
                        :
                        new List<HistoricoFechasCompromisoDestinatariosViewModel>()
                })
                .AsQueryable();

            return await destinatariosQuery.ToListAsync();
        }
    }
}
