using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IEstadisticasService
    {
        Task<EstadisticasRecibidosViewModel> ObtenerEstadisticasDocumentosRecibidosPorEstadoAsync(string username, string categoriaId, string fechaInicio, string fechaFin);
        Task<EstadisticasEnviadosViewModel> ObtenerEstadisticasDocumentosEnviadosPorEstadoAsync(string username, string categoriaId, string fechaInicio, string fechaFin);
    }
}
