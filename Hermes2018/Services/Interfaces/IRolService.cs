using Hermes2018.Models.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IRolService
    {

        Task<List<HER_Rol>> ObtenerRolesAsync();
        Task<List<HER_Rol>> ObtenerRolUsuariosNormalesAsync();
        Task<List<HER_Rol>> ObtenerRolAdministradoresRegionalesAsync();
        Task<List<HER_Rol>> ObtenerRolEnListaAsync(string rol);
    }
}
