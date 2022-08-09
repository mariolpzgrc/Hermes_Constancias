using Hermes2018.Models.Anexo;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IAnexoService
    {
        Task<string> GuardarAnexoTemporalAsync(string folio, IFormFile file);
        string BorrarAnexoTemporal(AnexoTempJsonModel tempViewModel);
        bool BorrarAnexosTemporales(AnexoFolioJsonModel folioJsonModel);

        Task<int?> GuardarAnexosAsync(List<string> archivos, int tipo, string session, string usuarioTitular, string folio);
        Task<bool> ActualizarAnexosDocumentoBaseAsync(List<string> archivos, int tipo, string session, string usuarioTitular, string folio, int documentoBaseId);
        Task<string> BorrarAnexoDocumentoBaseAsync(AnexoFinalJsonModel finalJsonModel);
        Task<List<AnexosDocumentoViewModel>> ObtenerAnexosDocumentoBaseAsync(AnexoDescargaJsonModel descargaJsonModel);

        Task<int?> RecuperaAnexoAsync(int documentoBaseId);

        Task<List<AnexosDocumentoViewModel>> ObtenerAnexosDocumentoEnviadoAsync(AnexoDescargaEnvioJsonModel envioJsonModel);
        Task<List<AnexosDocumentoViewModel>> ObtenerAnexosDocumentoEnviadoOrigenAsync(AnexoDescargaEnvioJsonModel envioJsonModel);
        Task<List<AnexosDocumentoViewModel>> ObtenerAnexosDocumentoTurnadoAsync(AnexoDescargaTurnarJsonModel turnarJsonModel);
        
        Task<HER_AnexoArchivo> ObtenerAnexoAsync(int anexoArchivoId);
        
        Task<JsonResult> GuardarImagenesTempAsync(string usuario, string folio, IFormFile file, string path);
        JsonResult GuardarImagenesTemp64(string usuario, string folio, IFormFile file, string path);
        JsonResult EliminarImagenesTemp(string src);
        Task<string> ObtenerLogoBase64Async(int areaId);
    }
}
