using Hermes2018.Models.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IImportanciaService
    {
        Task<List<HER_Importancia>> ObtenerTiposImportanciaAsync();
        Task<string> ObtenerNombreImportanciaAsync(int importanciaId);
    }
}
