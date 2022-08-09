using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IDelegarService
    {
        Task<List<DelegadosViewModel>> ObtenerDelegadosAsync(string username);
        Task<int> ObtenerTotalDelegadosAsync(string username);

        Task<bool> ExisteDelegadoAsync(string username, string delegado);
        Task<bool> GuardarDelegadoAsync(string titular, string delegado, int tipo);
        Task<DelegadoDetalleViewModel> ObtenerDetalleDelegado(int delegarId);
        Task<DelegadoEditarViewModel> ObtenerDelegadoParaEdicion(string username, int delegarId);
        Task<bool> ActualizarDelegadoAsync(string username, DelegadoEditarViewModel model);
        Task<DelegadoBorrarViewModel> ObtenerDelegadoParaBorrar(string username, int delegarId);
        Task<bool> BorrarDelegadoAsync(string username, DelegadoBorrarViewModel model);
    }
}
