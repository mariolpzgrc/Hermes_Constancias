//using Hermes2018.ModelsDBF;
using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IOracleService
    {
        BaseUsuarioOracleViewModel ObtieneUsuariosOracleAsync(string usuario);
        Task<List<DetectarAreaViewModelDBF>> ObtenerAreasAsync(string areaPadreId, int areaId);
        Task<List<DetectarAreaViewModelDBF>> ObtenerAreasAsync(int areaId);
        Task<AgregarAreaViewModel> ObtenerAreaParaAgregarAsync(string clave);
        Task<bool> ExisteAreaPorClaveAsync(string clave);
        Task<InfoUsuarioOracleViewModel> ObtieneInfoUsuarioOracleAsync(string usuario);
    }
}
