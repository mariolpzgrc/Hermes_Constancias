using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Rol;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class RolService:IRolService
    {
        private readonly ApplicationDbContext _context;

        public RolService(ApplicationDbContext context)
        {
            _context = context;  
        }

        public async Task<List<HER_Rol>> ObtenerRolesAsync()
        {
            var rolesQuery = _context.HER_Rol
                    .Where(x => !ConstRol.RolAdminGral.Contains(x.HER_Nombre))
                    .OrderBy(x => x.HER_RolId)
                    .AsNoTracking()
                    .AsQueryable();

            return await rolesQuery.ToListAsync();
        }
        public async Task<List<HER_Rol>> ObtenerRolUsuariosNormalesAsync()
        {
            var rolesQuery = _context.HER_Rol
                .Where(x => ConstRol.RolUsuario.Contains(x.HER_Nombre))
                .OrderBy(x => x.HER_RolId)
                .AsNoTracking()
                .AsQueryable();
            
            return await rolesQuery.ToListAsync();
        }
        public async Task<List<HER_Rol>> ObtenerRolAdministradoresRegionalesAsync()
        {
            var rolesQuery = _context.HER_Rol
                .Where(x => ConstRol.RolAdminRegional.Contains(x.HER_Nombre))
                .OrderBy(x => x.HER_RolId)
                .AsNoTracking()
                .AsQueryable();

            return await rolesQuery.ToListAsync();
        }
        public async Task<List<HER_Rol>> ObtenerRolEnListaAsync(string rol)
        {
            var rolEnListaQuery = _context.HER_Rol
                .Where(x => x.HER_Nombre == rol)
                .AsNoTracking()
                .AsQueryable();

            return await rolEnListaQuery.ToListAsync();
        }
    }
}
