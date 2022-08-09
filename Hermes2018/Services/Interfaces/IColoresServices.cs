using Hermes2018.Models.Configuracion;
using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IColorService
    {
        Task<List<HER_Color>> ObtenerColoresAsync();
    }
}
