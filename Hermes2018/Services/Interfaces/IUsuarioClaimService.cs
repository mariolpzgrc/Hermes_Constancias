using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IUsuarioClaimService
    {
        InfoConfigUsuarioViewModel ObtenerInfoUsuarioClaims(ClaimsPrincipal User);
    }
}
