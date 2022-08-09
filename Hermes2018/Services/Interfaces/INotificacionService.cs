using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface INotificacionService
    {
        Task<List<NotificacionUsuariosViewModel>> UsuariosAsync(int areaId);
        Task<List<NotificacionDocumentosViewModel>> DocumentosAsync(string username);
        Task<List<NotificacionProximosVencerViewModel>> DocumentosProximosVencerAsync(string username);
    }
}
