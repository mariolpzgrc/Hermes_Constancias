using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Anexo;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class AnexoService : IAnexoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<AnexoService> _logger;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly IAreaService _areaService;
        private readonly IConfiguracionService _configuracionService;

        public AnexoService(ApplicationDbContext context, 
            IHostingEnvironment environment,
            ILogger<AnexoService> logger,
            IAreaService areaService,
            IConfiguracionService configuracionService)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
            _areaService = areaService;
            _configuracionService = configuracionService;
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
        }

        public async Task<string> GuardarAnexoTemporalAsync(string folio, IFormFile file)
        {
            var carpeta = Path.Combine(_environment.WebRootPath, "uploads\\temp\\" + folio);
            var rutaArchivo = Path.Combine(_environment.WebRootPath, "uploads\\temp\\" + folio + "\\" + file.FileName);
            var nombreArchivo = string.Empty;

            if (!string.IsNullOrEmpty(folio))
            {
                try
                {
                    //--
                    bool existe = Directory.Exists(carpeta);
                    if (!existe)
                        Directory.CreateDirectory(carpeta);

                    if (file.Length > 0)
                    {
                        using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        nombreArchivo = file.FileName;
                    }
                    //--
                }
                catch (Exception ex)
                {
                    nombreArchivo = string.Empty;
                    _logger.LogError("AnexoService:GuardarAnexoTemporalAsync: " + ex.Message);
                }
            }

            return nombreArchivo;
        }
        public string BorrarAnexoTemporal(AnexoTempJsonModel tempViewModel)
        {
            var rutaArchivo = Path.Combine(_environment.WebRootPath, "uploads\\temp\\" + tempViewModel.Folio + "\\" + tempViewModel.NombreArchivo);
            var nombreArchivo = string.Empty;

            try
            {
                bool existe = File.Exists(rutaArchivo);
                if (existe)
                {
                    File.Delete(rutaArchivo);
                    nombreArchivo = tempViewModel.NombreArchivo;
                }
            }
            catch (Exception ex)
            {
                nombreArchivo = string.Empty;
                _logger.LogError("AnexoService:BorrarAnexoTemporal: " + ex.Message);
            }
            
            return nombreArchivo;
        }
        public bool BorrarAnexosTemporales(AnexoFolioJsonModel folioJsonModel)
        {
            bool respuesta = false;
            var carpeta = Path.Combine(_environment.WebRootPath, "uploads\\temp\\" + folioJsonModel.Folio);

            if (!string.IsNullOrEmpty(folioJsonModel.Folio))
            {
                try
                {
                    bool exists = Directory.Exists(carpeta);
                    if (exists)
                    {
                        //Borra los archivos
                        Array.ForEach(Directory.GetFiles(carpeta), File.Delete);
                        //Borra la carpeta contenedora 
                        Directory.Delete(carpeta, true);
                        //--
                        respuesta = true;
                    }
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    _logger.LogError("AnexoService:BorrarAnexosTemporales: " + ex.Message);
                }
            }

            return respuesta;
        }

        public async Task<int?> GuardarAnexosAsync(List<string> archivos, int tipo, string session, string usuarioTitular, string folio)
        {
            int? anexoId = null;
            List<HER_AnexoArchivo> listado = new List<HER_AnexoArchivo>();

            var fecha = DateTime.Now;
            var tmpOrigenArchivo = string.Empty;
            var tmpDestinoArchivo = string.Empty;
            var tipoArchivo = string.Empty;
            var provider = new FileExtensionContentTypeProvider();

            var tmpDestinoCarpeta = string.Empty;

            var anexoRutaQuery = _context.HER_AnexoRuta
                .Where(x => x.HER_Estado == ConstAnexoRuta.EstadoN1)
                .AsNoTracking()
                .AsQueryable();

            var anexoRuta = anexoRutaQuery.FirstOrDefault();

            //Anexo Adjuntos
            if (archivos.Count > 0)
            {
                //Anexo
                HER_Anexo nuevoAnexo = new HER_Anexo()
                {
                    HER_Tipo = tipo
                };
                _context.HER_Anexo.Add(nuevoAnexo);

                if (anexoRuta == null)
                    tmpDestinoCarpeta = Path.Combine(_environment.WebRootPath, "uploads\\" + fecha.Year + "\\" + fecha.Month + "\\" + usuarioTitular + "\\" + folio);
                else
                    tmpDestinoCarpeta = Path.Combine(anexoRuta.HER_RutaBase, "uploads\\" + fecha.Year + "\\" + fecha.Month + "\\" + usuarioTitular + "\\" + folio);

                //Crea la carpeta contenedora definitiva
                bool existeDestino = Directory.Exists(tmpDestinoCarpeta);
                if (!existeDestino)
                    Directory.CreateDirectory(tmpDestinoCarpeta);

                foreach (var archivo in archivos)
                {
                    //Archivo temporal
                    tmpOrigenArchivo = Path.Combine(_environment.WebRootPath, "uploads\\temp\\" + session + "\\" + archivo);
                    //Archivo definitivo
                    if (anexoRuta == null)
                        tmpDestinoArchivo = Path.Combine(_environment.WebRootPath, "uploads\\" + fecha.Year + "\\" + fecha.Month + "\\" + usuarioTitular + "\\" + folio + "\\" + archivo);
                    else
                        tmpDestinoArchivo = Path.Combine(anexoRuta.HER_RutaBase, "uploads\\" + fecha.Year + "\\" + fecha.Month + "\\" + usuarioTitular + "\\" + folio + "\\" + archivo);

                    if (File.Exists(tmpOrigenArchivo)) 
                    {
                        //Tipo de archivo
                        if (!provider.TryGetContentType(tmpOrigenArchivo, out tipoArchivo))
                        {
                            tipoArchivo = "application/octet-stream";
                        }

                        //Mueve el archivo
                        //System.IO.File.Move(tmpOrigenArchivo, tmpDestinoArchivo);
                        System.IO.File.Copy(tmpOrigenArchivo, tmpDestinoArchivo, true);
                        System.IO.File.Delete(tmpOrigenArchivo);

                        //Guarda las rutas en un listado para guardar en la BD
                        listado.Add(new HER_AnexoArchivo()
                        {
                            HER_Nombre = archivo,
                            HER_RutaComplemento = string.Format("{0}/{1}/{2}/{3}/{4}", "uploads", fecha.Year, fecha.Month, usuarioTitular, folio),
                            HER_TipoContenido = tipoArchivo,
                            HER_AnexoId = nuevoAnexo.HER_AnexoId,
                            HER_AnexoRutaId = anexoRuta?.HER_AnexoRutaId
                        });
                    }
                }

                if (listado.Count > 0) 
                {
                    //Anexo archivos
                    _context.HER_AnexoArchivo.AddRange(listado);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                        anexoId = nuevoAnexo.HER_AnexoId;
                }

                //Eliminar la carpeta contenedora temporal
                var tmpOrigenFolder = Path.Combine(_environment.WebRootPath, "uploads\\temp\\" + session);
                //Elimina la carpeta contenedora de los archivos temporales
                var existeOrigen = Directory.Exists(tmpOrigenFolder);
                if (existeOrigen)
                    Directory.Delete(tmpOrigenFolder, true);
            }
            
            return anexoId;
        }
        public async Task<bool> ActualizarAnexosDocumentoBaseAsync(List<string> archivos, int tipo, string session, string usuarioTitular, string folio, int documentoBaseId)
        {
            bool band = false;
            var result = 0;

            HER_Anexo anexo = null;
            List<string> listadoArchivosRegistrados = new List<string>();
            IQueryable<HER_Anexo> anexoQuery;
            List<HER_AnexoArchivo> listadoArchivos = new List<HER_AnexoArchivo>();
            
            var fecha = DateTime.Now;
            var tmpOrigenArchivo = string.Empty;
            var tipoArchivo = string.Empty;
            var destinoArchivo = string.Empty;
            var provider = new FileExtensionContentTypeProvider();

            var tmpDestinoCarpeta = string.Empty;

            //Buscamos el documento      
            var documentoQuery = _context.HER_DocumentoBase
                .Where(x => x.HER_Folio == folio 
                         && x.HER_DocumentoBaseId == documentoBaseId)
                .AsQueryable();

            var documento = await documentoQuery.FirstOrDefaultAsync();

            //Anexo Ruta
            var anexoRutaQuery = _context.HER_AnexoRuta
                .Where(x => x.HER_Estado == ConstAnexoRuta.EstadoN1)
                .AsNoTracking()
                .AsQueryable();

            var anexoRuta = anexoRutaQuery.FirstOrDefault();

            //Existe el documento
            if (documento != null)
            {
                //Tiene anexos asociados anteriormente
                if (documento.HER_AnexoId != null)
                {
                    //Busca el anexo creado
                    anexoQuery = _context.HER_Anexo
                        .Include(x => x.HER_AnexoArchivos)
                        .Where(x => x.HER_AnexoId == documento.HER_AnexoId)
                        .AsQueryable();

                    anexo = await anexoQuery.FirstOrDefaultAsync();
                    listadoArchivosRegistrados = anexo.HER_AnexoArchivos.Select(x => x.HER_Nombre).ToList();
                }
                else
                {
                    //No tiene anexos asociados anteriormente
                    anexo = new HER_Anexo()
                    {
                        HER_Tipo = tipo //ConstTipoAnexo.TipoAnexoN1
                    };
                    _context.HER_Anexo.Add(anexo);

                    //Agregamos el nuevo anexo en el documento 
                    documento.HER_AnexoId = anexo.HER_AnexoId;
                    _context.HER_DocumentoBase.Update(documento).State = EntityState.Modified;
                }

                if(anexoRuta == null)
                    tmpDestinoCarpeta = Path.Combine(_environment.WebRootPath, "uploads\\" + fecha.Year + "\\" + fecha.Month + "\\" + usuarioTitular + "\\" + folio);
                else
                    tmpDestinoCarpeta = Path.Combine(anexoRuta.HER_RutaBase, "uploads\\" + fecha.Year + "\\" + fecha.Month + "\\" + usuarioTitular + "\\" + folio);

                bool existeDestino = Directory.Exists(tmpDestinoCarpeta);
                if (!existeDestino)
                    Directory.CreateDirectory(tmpDestinoCarpeta);

                foreach (var archivo in archivos)
                {
                    //Archivo temporal
                    tmpOrigenArchivo = Path.Combine(_environment.WebRootPath, "uploads\\temp\\" + session + "\\" + archivo);
                    //Archivo definitivo
                    if (anexoRuta == null)
                        destinoArchivo = Path.Combine(_environment.WebRootPath, "uploads\\" + fecha.Year + "\\" + fecha.Month + "\\" + usuarioTitular + "\\" + folio + "\\" + archivo);
                    else
                        destinoArchivo = Path.Combine(anexoRuta.HER_RutaBase, "uploads\\" + fecha.Year + "\\" + fecha.Month + "\\" + usuarioTitular + "\\" + folio + "\\" + archivo);

                    if (File.Exists(tmpOrigenArchivo))
                    {
                        //Obtiene el tipo de archivo
                        if (!provider.TryGetContentType(tmpOrigenArchivo, out tipoArchivo))
                        {
                            tipoArchivo = "application/octet-stream";
                        }

                        //Mueve el archivo
                        System.IO.File.Copy(tmpOrigenArchivo, destinoArchivo, true);

                        //Guarda las rutas en un listado para guardar en la BD
                        if (!listadoArchivosRegistrados.Contains(archivo))
                        {
                            listadoArchivos.Add(new HER_AnexoArchivo()
                            {
                                HER_Nombre = archivo,
                                HER_RutaComplemento = string.Format("{0}/{1}/{2}/{3}/{4}", "uploads", fecha.Year, fecha.Month, usuarioTitular, folio),
                                HER_TipoContenido = tipoArchivo,
                                HER_AnexoId = anexo.HER_AnexoId,
                                HER_AnexoRutaId = anexoRuta?.HER_AnexoRutaId
                            });
                        }
                    }
                }

                if (listadoArchivos.Count > 0) 
                {
                    _context.HER_AnexoArchivo.AddRange(listadoArchivos);
                    result = await _context.SaveChangesAsync();
                }

                if (result > 0)
                    band = true;
            }

            //Eliminar la carpeta contenedora temporal
            var tmpOrigenCarpeta = Path.Combine(_environment.WebRootPath, "uploads\\temp\\" + session);
            var existeOrigen = Directory.Exists(tmpOrigenCarpeta);
            if (existeOrigen)
                Directory.Delete(tmpOrigenCarpeta, true);

            return band;
        }
        public async Task<string> BorrarAnexoDocumentoBaseAsync(AnexoFinalJsonModel finalJsonModel)
        {
            var nombreArchivo = string.Empty;
            string rutaBase;

            //Buscar el archivo del anexo
            var archivoQuery = _context.HER_AnexoArchivo
                .Where(x => x.HER_Anexo.HER_DocumentoBase.HER_Folio == finalJsonModel.Folio 
                         && x.HER_Nombre == finalJsonModel.NombreArchivo)
                .Include(x => x.HER_AnexoRuta)
                .AsQueryable();

            var archivo = await archivoQuery.FirstOrDefaultAsync();

            if (archivo.HER_AnexoRutaId == null)
                rutaBase = _environment.WebRootPath;
            else
                rutaBase = archivo.HER_AnexoRuta.HER_RutaBase;

            if (archivo != null)
            {
                var rutaSimpleArchivo = string.Format("{0}/{1}", archivo.HER_RutaComplemento, archivo.HER_Nombre).Replace("/", "\\");
                var rutaArchivo = Path.Combine(rutaBase, rutaSimpleArchivo);

                var rutaSimpleCarpeta = archivo.HER_RutaComplemento.Replace("/", "\\");
                var routeFolder = Path.Combine(rutaBase, rutaSimpleCarpeta);

                try
                {
                    bool existe = File.Exists(rutaArchivo);
                    if (existe)
                    {
                        //Borra el archivo del sistema de archivos
                        File.Delete(rutaArchivo);

                        //Acciones en la BD
                        //Borra el archivo de la BD
                        _context.HER_AnexoArchivo.Remove(archivo);
                        //Guarda los cambios en la BD
                        await _context.SaveChangesAsync();

                        var totalAnexosQuery = _context.HER_AnexoArchivo
                                .Where(x => x.HER_AnexoId == archivo.HER_AnexoId)
                                .Include(x => x.HER_AnexoRuta)
                                .AsQueryable();

                        var totalAnexos = await totalAnexosQuery.CountAsync();

                        if (totalAnexos == 0)
                        {
                            //Borra la carpeta contenedora 
                            Directory.Delete(routeFolder, true);

                            //Si al borrar el archivo ya no queda ninguno, entonces, se debe de eliminar el anexo y la relación de ese anexo en la tabla documento
                            var anexoQuery = _context.HER_Anexo
                                        .Where(x => x.HER_AnexoId == archivo.HER_AnexoId)
                                        .AsQueryable();

                            var anexo = await anexoQuery.FirstOrDefaultAsync();

                            if (anexo != null)
                            {
                                //Limpia la relacion que tiene en la tabla Documento (actualiza a null)
                                var datosDocumentoQuery = _context.HER_DocumentoBase
                                        .Where(x => x.HER_Folio == finalJsonModel.Folio)
                                        .AsQueryable();

                                var datosDocumento = await datosDocumentoQuery.FirstOrDefaultAsync();

                                datosDocumento.HER_AnexoId = null;
                                _context.HER_DocumentoBase.Update(datosDocumento).State = EntityState.Modified;

                                //Elimina el anexo
                                _context.HER_Anexo.Remove(anexo);
                                await _context.SaveChangesAsync();
                            }
                        }

                        //Devuelve el nombre del archivo
                        nombreArchivo = archivo.HER_Nombre;
                    }
                }
                catch (Exception ex)
                {
                    nombreArchivo = string.Empty;
                    _logger.LogError("AnexoService:BorrarAnexoDocumentoBaseAsync: " + ex.Message);
                }
            }

            return nombreArchivo;
        }
        public async Task<List<AnexosDocumentoViewModel>> ObtenerAnexosDocumentoBaseAsync(AnexoDescargaJsonModel descargaJsonModel)
        {
            var listadoArchivos = new List<AnexosDocumentoViewModel>();

            if (!string.IsNullOrEmpty(descargaJsonModel.Folio))
            {
                //Documento
                var existeDocumentoBaseQuery = _context.HER_DocumentoBase
                        .Where(x => x.HER_Folio == descargaJsonModel.Folio
                                 && x.HER_AnexoId != null)
                        .AsNoTracking()
                        .AsQueryable();

                var existeDocumentoBase = await existeDocumentoBaseQuery.AnyAsync();

                if (existeDocumentoBase)
                {
                    var archivosQuery = _context.HER_AnexoArchivo
                            .Where(x => x.HER_Anexo.HER_DocumentoBase.HER_Folio == descargaJsonModel.Folio)
                            .Include(x => x.HER_AnexoRuta)
                            .AsNoTracking()
                            .Select(x => new AnexosDocumentoViewModel
                            {
                                Nombre = x.HER_Nombre,
                                Ruta = string.Format("{0}/{1}/{2}", "Anexos", "Index", x.HER_AnexoArchivoId),
                                Tipo = x.HER_TipoContenido
                            })
                            .AsQueryable();

                    listadoArchivos = await archivosQuery.ToListAsync();
                }
            }

            return listadoArchivos;
        }

        public async Task<int?> RecuperaAnexoAsync(int documentoBaseId)
        {
            int? anexoId = null;

            //Buscamos el documento      
            var documentoQuery = _context.HER_DocumentoBase
                .Where(x =>  x.HER_DocumentoBaseId == documentoBaseId)
                .Select(x => x.HER_AnexoId)
                .AsQueryable();

            anexoId = await documentoQuery.FirstOrDefaultAsync();

            return anexoId;
        }

        public async Task<List<AnexosDocumentoViewModel>> ObtenerAnexosDocumentoEnviadoAsync(AnexoDescargaEnvioJsonModel envioJsonModel)
        {
            var listadoArchivos = new List<AnexosDocumentoViewModel>();

            if (!string.IsNullOrEmpty(envioJsonModel.Folio))
            {
                //--
                var envioQuery = _context.HER_Envio
                        .Where(x => x.HER_EnvioId == envioJsonModel.EnvioId)
                        //--
                        .Include(x => x.HER_EnvioPadre)
                        .AsNoTracking()
                        .AsQueryable();

                var envio = await envioQuery.FirstOrDefaultAsync();
                //--
                bool existeAnexo = envio.HER_AnexoId != null;
                /*bool existeAnexoOrigen = (envio.HER_EsReenvio || envio.HER_EnvioPadre.HER_TipoEnvioId == ConstTipoEnvio.TipoEnvioN4) ?
                            (envio.HER_EnvioPadre.HER_AnexoId != null) ?
                                true :
                                false :
                                false;*/

                if (existeAnexo)
                {
                    var archivosQuery = _context.HER_AnexoArchivo
                        .Where(x => x.HER_Anexo.HER_Envio.HER_EnvioId == envio.HER_EnvioId)
                        .Include(x => x.HER_AnexoRuta)
                        .AsNoTracking()
                        .Select(x => new AnexosDocumentoViewModel
                        {
                            Nombre = x.HER_Nombre,
                            Ruta = string.Format("{0}/{1}/{2}", "Anexos", "Index", x.HER_AnexoArchivoId),
                            Tipo = x.HER_TipoContenido
                        })
                        .AsQueryable();

                    listadoArchivos = await archivosQuery.ToListAsync();
                }
                //--
                /*if (existeAnexoOrigen)
                {  
                        var archivosOrigenQuery = _context.HER_AnexoArchivo
                        .Where(x => x.HER_Anexo.HER_Envio.HER_EnvioId == envio.HER_EnvioPadreId)
                        .Include(x => x.HER_AnexoRuta)
                        .AsNoTracking()
                        .Select(x => new AnexosDocumentoViewModel
                        {
                            Nombre = x.HER_Nombre,
                            Ruta = string.Format("{0}/{1}/{2}", "Anexos", "Index", x.HER_AnexoArchivoId),
                            Tipo = x.HER_TipoContenido
                        })
                        .AsQueryable();

                        listadoArchivos.AddRange(await archivosOrigenQuery.ToListAsync());                                      
                }*/
            }

            return listadoArchivos;
        }

        public async Task<List<AnexosDocumentoViewModel>> ObtenerAnexosDocumentoEnviadoOrigenAsync(AnexoDescargaEnvioJsonModel envioJsonModel)
        {
            var listadoArchivos = new List<AnexosDocumentoViewModel>();

            if (!string.IsNullOrEmpty(envioJsonModel.Folio))
            {
                var envioQuery = _context.HER_Envio
                    .Where(x => x.HER_EnvioId == envioJsonModel.EnvioId)
                        .Include(x => x.HER_EnvioPadre)
                        .AsNoTracking()
                        .AsQueryable();

                var envio = await envioQuery.FirstOrDefaultAsync();
                bool existeAnexoOrigenPadre =  (envio.HER_EsReenvio) ? (envio.HER_EnvioPadre.HER_AnexoId != null) ? true : false : false;

                if (existeAnexoOrigenPadre)
                {
                    var archivosOrigenQuery = _context.HER_AnexoArchivo
                    .Where(x => x.HER_Anexo.HER_Envio.HER_EnvioId == envio.HER_EnvioPadreId)
                    .Include(x => x.HER_AnexoRuta)
                    .AsNoTracking()
                    .Select(x => new AnexosDocumentoViewModel
                    {
                        Nombre = x.HER_Nombre,
                        Ruta = string.Format("{0}/{1}/{2}", "Anexos", "Index", x.HER_AnexoArchivoId),
                        Tipo = x.HER_TipoContenido
                    })
                    .AsQueryable();

                    listadoArchivos.AddRange(await archivosOrigenQuery.ToListAsync());
                }
                else
                {
                    var archivosQuery = _context.HER_AnexoArchivo
                        .Where(x => x.HER_Anexo.HER_Envio.HER_EnvioId == envio.HER_EnvioId)
                        .Include(x => x.HER_AnexoRuta)
                        .AsNoTracking()
                        .Select(x => new AnexosDocumentoViewModel
                        {
                            Nombre = x.HER_Nombre,
                            Ruta = string.Format("{0}/{1}/{2}", "Anexos", "Index", x.HER_AnexoArchivoId),
                            Tipo = x.HER_TipoContenido
                        })
                        .AsQueryable();

                    listadoArchivos = await archivosQuery.ToListAsync();
                }

            }

            return listadoArchivos;
        }

        public async Task<List<AnexosDocumentoViewModel>> ObtenerAnexosDocumentoTurnadoAsync(AnexoDescargaTurnarJsonModel turnarJsonModel)
        {
            var listadoArchivos = new List<AnexosDocumentoViewModel>();
            //--
            var existeAnexoQuery = _context.HER_Envio
                    .Where(x => x.HER_EnvioId == turnarJsonModel.EnvioId
                                && x.HER_AnexoId != null)
                    .AsNoTracking()
                    .AsQueryable();

            var existeAnexo = await existeAnexoQuery.AnyAsync();

            if (existeAnexo)
            {
                var archivosQuery = _context.HER_AnexoArchivo
                    .Where(x => x.HER_Anexo.HER_Envio.HER_EnvioId == turnarJsonModel.EnvioId)
                    .Include(x => x.HER_AnexoRuta)
                    .AsNoTracking()
                    .Select(x => new AnexosDocumentoViewModel
                    {
                        Nombre = x.HER_Nombre,
                        Ruta = string.Format("{0}/{1}/{2}", "Anexos", "Index", x.HER_AnexoArchivoId),
                        Tipo = x.HER_TipoContenido
                    })
                    .AsQueryable();

                listadoArchivos = await archivosQuery.ToListAsync();
            }
            //--

            return listadoArchivos;
        }
        public async Task<HER_AnexoArchivo> ObtenerAnexoAsync(int anexoArchivoId)
        {
            var anexoQuery = _context.HER_AnexoArchivo
                .Where(x => x.HER_AnexoArchivoId == anexoArchivoId)
                .Include(x => x.HER_AnexoRuta)
                .AsTracking()
                .AsQueryable();

            return await anexoQuery.FirstOrDefaultAsync();
        }

        //Guardar imágenes
        public async Task<JsonResult> GuardarImagenesTempAsync(string usuario, string folio, IFormFile file, string path)
        {
            var fecha = DateTime.Now;

            // Get the file from the POST request
            var theFile = file;

            // Get the server path, wwwroot
            string webRootPath = _environment.WebRootPath;

            // Building the path to the uploads directory
            //var fileRoute = Path.Combine(webRootPath, "uploads\\image\\" + folio);
            var fileRoute = Path.Combine(webRootPath, "uploads\\image\\" + fecha.Year + "\\" + fecha.Month + "\\" + usuario + "\\" + folio);

            // Get the mime type
            var mimeType = file.ContentType;

            // Get File Extension
            string extension = System.IO.Path.GetExtension(theFile.FileName);

            // Generate Random name.
            string name = Guid.NewGuid().ToString().Substring(0, 8) + extension;

            // Build the full path inclunding the file name
            var link = Path.Combine(fileRoute, name);

            // Create directory if it does not exist.
            Directory.CreateDirectory(fileRoute);
            //FileInfo dir = new FileInfo(fileRoute);
            //dir.Directory.Create();

            // Basic validation on mime types and file extension
            string[] imageMimetypes = { "image/gif", "image/jpeg", "image/pjpeg", "image/x-png", "image/png", "image/svg+xml", "image/PNG", };
            string[] imageExt = { ".gif", ".jpeg", ".jpg", ".png", ".svg", ".blob", ".PNG" };

            try
            {
                if (Array.IndexOf(imageMimetypes, mimeType) >= 0 && (Array.IndexOf(imageExt, extension) >= 0))
                {
                    // Copy contents to memory stream.
                    Stream stream;
                    stream = new MemoryStream();
                    theFile.CopyTo(stream);
                    stream.Position = 0;
                    String serverPath = link;

                    // Save the file
                    //using (FileStream writerFileStream = System.IO.File.Create(serverPath))
                    using (var fileStream = new FileStream(serverPath, FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                        fileStream.Dispose();
                    }

                    // Return the file path as json
                    Hashtable imageUrl = new Hashtable();
                    //imageUrl.Add("link", "/uploads/image/" + folio + "/"  + name);

                    imageUrl.Add("link", path + "uploads/image/" + fecha.Year + "/" + fecha.Month + "/" + usuario + "/" + folio + "/" + name);

                    return new JsonResult(imageUrl, _jsonSettings);
                }
                throw new ArgumentException("The image did not pass the validation");
            }



            catch (ArgumentException ex)
            {
                return new JsonResult(ex.Message, _jsonSettings);
            }

            //return nombreArchivo;
        }
        public JsonResult GuardarImagenesTemp64(string usuario, string folio, IFormFile file, string path)
        {
            // Get the mime type
            var mimeType = file.ContentType;

            // Get File Extension
            string extension = System.IO.Path.GetExtension(file.FileName);

            // Basic validation on mime types and file extension
            string[] imageMimetypes = { "image/gif", "image/jpeg", "image/pjpeg", "image/x-png", "image/png", "image/svg+xml", "image/PNG", };
            string[] imageExt = { ".gif", ".jpeg", ".jpg", ".png", ".svg", ".blob", ".PNG" };

            try
            {
                if (Array.IndexOf(imageMimetypes, mimeType) >= 0 && (Array.IndexOf(imageExt, extension) >= 0))
                {
                    var stream = new MemoryStream();
                    file.CopyTo(stream);
                    var fileBytes = stream.ToArray();
                    string s = Convert.ToBase64String(fileBytes, Base64FormattingOptions.None);
                    Hashtable imageUrl = new Hashtable
                    {
                        { "link", string.Format("{0}{1}{2}{3}{4}", "data:", mimeType, ";", "base64,", s) }
                    };

                    return new JsonResult(imageUrl, _jsonSettings);
                }
                throw new ArgumentException("The image did not pass the validation");
            }
            catch (ArgumentException ex)
            {
                return new JsonResult(ex.Message, _jsonSettings);
            }
        }
        public JsonResult EliminarImagenesTemp(string src)
        {

            src = src.Remove(0,1);
            src = src.Replace("/", "\\");

            var rutaArchivo = Path.Combine(_environment.WebRootPath, src);

            try
            {
                bool existe = File.Exists(rutaArchivo);
                if (existe)
                {
                    File.Delete(rutaArchivo);
                }

                return new JsonResult(true, _jsonSettings);
            }
            catch (Exception e)
            {
                _logger.LogError("AnexoService:EliminarImagenesTempAsync: " + e.Message);
                return new JsonResult(e, _jsonSettings); 
            }

        }

        //Logo Base 64
        public async Task<string> ObtenerLogoBase64Async(int areaId) {

            string respuesta;

            if (await _areaService.ExisteLogoInstitucionAsync(areaId))
            {
                HER_AnexoArea logo = await _areaService.ObtenerLogoInstitucionAsync(areaId);
                string rutaBase;

                if (logo.HER_AnexoRutaId == null)
                    rutaBase = _environment.WebRootPath;
                else
                    rutaBase = logo.HER_AnexoRuta.HER_RutaBase;

                respuesta = string.Format("{0}{1}{2}{3}{4}", 
                    "data:", 
                    logo.HER_TipoContenido, 
                    ";", 
                    "base64,", 
                    System.Convert.ToBase64String(System.IO.File.ReadAllBytes(System.IO.Path.Combine(new string[] { rutaBase, logo.HER_RutaComplemento, logo.HER_Nombre })))); // 
            }
            else
            {
                HER_AnexoGeneral logo = await _configuracionService.ObtenerLogoInstitucionAsync();
                
                respuesta = string.Format("{0}{1}{2}{3}{4}", 
                    "data:", 
                    logo.HER_TipoContenido, 
                    ";", 
                    "base64,", 
                    System.Convert.ToBase64String(System.IO.File.ReadAllBytes(System.IO.Path.Combine(new string[] { _environment.WebRootPath, logo.HER_RutaComplemento, logo.HER_Nombre }))));
            }

            return respuesta;
        }
    }
}
