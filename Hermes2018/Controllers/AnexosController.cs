using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Models.Anexo;
using Hermes2018.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Hermes2018.Controllers
{
    [Authorize]
    public class AnexosController : Controller
    {
        private readonly IAnexoService _anexoService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguracionService _configuracionService;
        private readonly IAreaService _areaService;

        public AnexosController(IAnexoService anexoService, 
            IHostingEnvironment hostingEnvironment, 
            IConfiguracionService configuracionService, 
            IAreaService areaService, 
            IDescargarPDF descargarPDFService)
        {
            _anexoService = anexoService;
            _hostingEnvironment = hostingEnvironment;
            _configuracionService = configuracionService;
            _areaService = areaService;
        }

        public async Task<FileResult> Index(int id)
        {
            var infoAnexo = await _anexoService.ObtenerAnexoAsync(id);
            string rutaBase;

            if (infoAnexo.HER_AnexoRutaId == null)
                rutaBase = _hostingEnvironment.WebRootPath;
            else
                rutaBase = infoAnexo.HER_AnexoRuta.HER_RutaBase;

            IFileProvider provider = new PhysicalFileProvider(Path.Combine(rutaBase, infoAnexo.HER_RutaComplemento));
            IFileInfo fileInfo = provider.GetFileInfo(infoAnexo.HER_Nombre);
            Stream readStream = fileInfo.CreateReadStream();

            return File(readStream, infoAnexo.HER_TipoContenido, infoAnexo.HER_Nombre);
        }

        [AllowAnonymous]
        public async Task<FileResult> Portada()
        {
            var portada = await _configuracionService.ObtenerImagenPortadaAsync();

            return File(System.IO.File.ReadAllBytes(System.IO.Path.Combine(new string[] { _hostingEnvironment.WebRootPath, portada.HER_RutaComplemento, portada.HER_Nombre })), 
                portada.HER_TipoContenido, 
                portada.HER_Nombre);
        }

        public async Task<FileResult> Logo(int id)
        {
            if (await _areaService.ExisteLogoInstitucionAsync(id))
            {
                HER_AnexoArea logo = await _areaService.ObtenerLogoInstitucionAsync(id);
                string rutaBase;

                if (logo.HER_AnexoRutaId == null)
                    rutaBase = _hostingEnvironment.WebRootPath;
                else
                    rutaBase = logo.HER_AnexoRuta.HER_RutaBase;

                return File(System.IO.File.ReadAllBytes(System.IO.Path.Combine(new string[] { rutaBase, logo.HER_RutaComplemento, logo.HER_Nombre })),
                    logo.HER_TipoContenido, 
                    logo.HER_Nombre);
            }
            else
            {
                HER_AnexoGeneral logo = await _configuracionService.ObtenerLogoInstitucionAsync();
                
                return File(System.IO.File.ReadAllBytes(System.IO.Path.Combine(new string[] { _hostingEnvironment.WebRootPath, logo.HER_RutaComplemento, logo.HER_Nombre })),
                    logo.HER_TipoContenido, 
                    logo.HER_Nombre);
            }
        }

        public async Task<ActionResult> LogoPorDefecto(int id)
        {
            HER_AnexoGeneral logo = await _configuracionService.ObtenerLogoInstitucionAsync();

            return File(System.IO.File.ReadAllBytes(System.IO.Path.Combine(new string[] { _hostingEnvironment.WebRootPath, logo.HER_RutaComplemento, logo.HER_Nombre })),
                logo.HER_TipoContenido, 
                logo.HER_Nombre);
        }

        
        //public FileResult DescargarPDF()
        //{
        //    string fileName = "Oficio.pdf";
        //    return File(_descargarPDFService.DescargarPDF(), System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        //}
    }
}