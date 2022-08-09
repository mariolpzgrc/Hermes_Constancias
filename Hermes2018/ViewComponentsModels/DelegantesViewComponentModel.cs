using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewComponentsModels
{
    public class BandejasViewComponentModel
    {
        public string Usuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Tipo { get; set; }
    }

    public class CambiarBandejaViewComponentModel
    {
        public string Usuario { get; set; }
        public List<BandejasViewComponentModel> Bandejas { get; set; }
        public string Leyenda { get; set; }
    }
}
