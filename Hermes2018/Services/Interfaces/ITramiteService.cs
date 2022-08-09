using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface ITramiteService
    {
        Task<List<ListadoTramitesViewModel>> ListadoTramitesPorNombreAsync();
        Task<List<ListadoTramitesViewModel>> ListadoTramitesPorNombreAsync(int recepcionId);
    }
}
