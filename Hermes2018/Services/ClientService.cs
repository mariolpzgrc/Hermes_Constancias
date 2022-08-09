using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.ViewModels;

namespace Hermes2018.Services
{
    public class ClientService : IClientService
    {
        private readonly IConfiguracionService _configuracionService;

        public ClientService(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        public async Task<UsuarioADViewModel> ObtenerInfoUsuarioADAsync(string userName)
        {
            var configuration = await _configuracionService.ObtenerInfoConfiguracionLDAPAsync();
            UsuarioADViewModel infoUsuario = new UsuarioADViewModel();

            using (var context = new PrincipalContext(ContextType.Domain, configuration.HER_IPLDAP, "hermes", "h3rm3s"))
            {
                UserPrincipal userPrincipal = new UserPrincipal(context)
                {
                    SamAccountName = userName + "*"
                };

                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    searcher.QueryFilter = userPrincipal;
                    ((DirectorySearcher)searcher.GetUnderlyingSearcher()).SizeLimit = 1;

                    var result = searcher.FindOne();

                    DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                    infoUsuario.HER_Usuario_NombreCompleto = string.Format("{0} {1}", (de.Properties["sn"].Value != null) ? de.Properties["sn"].Value.ToString() : string.Empty, (de.Properties["givenName"].Value != null) ? de.Properties["givenName"].Value.ToString() : string.Empty);
                    infoUsuario.HER_Usuario_Correo = de.Properties["userPrincipalName"].Value.ToString();
                    infoUsuario.HER_Usuario_Username = de.Properties["samaccountname"].Value.ToString();
                }
            }

            return infoUsuario;
        }
    }
}
