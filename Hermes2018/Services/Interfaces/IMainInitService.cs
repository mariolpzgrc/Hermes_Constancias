using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IMainInitService
    {
        void Inicial();
        Task UsuariosAsync();
        void Tramites();
    }
}
