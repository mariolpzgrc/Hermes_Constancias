using Hermes2018.Models.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IVisibilidadService
    {
        Task<List<HER_Visibilidad>> ObtenerTiposVisibilidadAsync();
    }
}
