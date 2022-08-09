using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class InfoUsuarioTokenViewModel
    {
        public string UsuarioEncriptado { get; set; }
    }
    public class TokenApiViewModel
    {
        public string Token { get; set; }
        public DateTime Expiration  { get; set; }
    }
    public class TokenApiJsonModel
    {
        public string Usuario { get; set; }
    }
    public class TokenApiAnonimoJsonModel
    {
        public string Usuario { get; set; }
    }
}
