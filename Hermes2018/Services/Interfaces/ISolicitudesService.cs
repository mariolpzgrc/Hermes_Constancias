using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface ISolicitudesService
    {
        Task<List<SolicitudesViewModel>> ObtenerSolicitudesAsync(int areaId);
        Task<SolicitudDetalleViewModel> ObtenerDetalleSolicitudAsync(string username, int areaId);
        Task<bool> ExisteSolicitudUsuarioAsync(string username, int areaId);
        Task<SolicitudResponderViewModel> ObtenerResumenResponderAsync(string username, int areaId);
        Task<bool> ResponderSolicitudAsync(SolicitudResponderViewModel viewModel);
    }
}
