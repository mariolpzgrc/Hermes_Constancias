using Hermes2018.Models.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface ITipoDocumentoService
    {
        Task<List<HER_TipoDocumento>> ObtenerTiposDocumentoAsync();
        Task<List<HER_TipoDocumento>> ObtenerSoloTipoOficioAsync();
        Task<string> ObtenerNombreTipo(int tipoId);
    }
}
