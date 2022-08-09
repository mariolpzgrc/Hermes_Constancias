using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IHerramientaService
    {
        string Encriptar(string texto);
        string Desencriptar(string textoEncriptado);
        //--
        TokenApiViewModel ConstruirToken(string usuario, int minutos);
        //--
    }
}
