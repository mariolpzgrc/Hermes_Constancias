using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewComponentsModels
{
    public class EstadoBandejasViewComponentModel
    {
        public int Recibidos { get; set; }
        //public int Enviados { get; set; }
        public int Borradores { get; set; }
        public int Revision { get; set; }
        public int Historico { get; set; }

        //--
        public bool ActivaDelegacion { get; set; }
        public int BandejaPermiso { get; set; }

        public bool EstaEnReasignacion { get; set; }
    }
}
