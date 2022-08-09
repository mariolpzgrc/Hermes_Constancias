using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class AnexoTempJsonModel
    {
        public string Folio { get; set; }
        public string NombreArchivo { get; set; }
    }
    public class AnexoFinalJsonModel
    {
        public string Folio { get; set; }
        public string NombreArchivo { get; set; }
    }
    public class AnexoArchivosViewModel
    {
        public string NombreArchivo { get; set; }
        public string TipoArchivo { get; set; }
        public string RutaArchivo { get; set; }
    }
    public class AnexoDocumentoBusquedaViewModel
    {
        public string Folio { get; set; }
        public string Usuario { get; set; }
    }
    public class AnexosDocumentoViewModel
    {
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string Tipo { get; set; }
    }
    public class AnexoFolioJsonModel
    {
        public string Folio { get; set; }
    }
    public class AnexoDescargaJsonModel
    {
        public string Folio { get; set; }
    }
    public class AnexoDescargaEnvioJsonModel
    {
        public string Folio { get; set; }
        public int EnvioId { get; set; }
        public int TipoEnvioId { get; set; }
    }
    public class AnexoDescargaTurnarJsonModel
    {
        public int EnvioId { get; set; }
    }

    public class AnexoRutaViewModel
    {
        public int RutaBaseId { get; set; }
        public string RutaBase { get; set; }
        public string Estado { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaActualizacion { get; set; }
        public int TotalArchivos { get; set; }
    }
}
