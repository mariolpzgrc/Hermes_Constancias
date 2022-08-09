using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    //Estadisticas del adminstrador
    public class RecopilacionGeneralViewModel
    {
        public long Areas { get; set; }
        public long Usuarios { get; set; }
        public long DocumentosEnviados { get; set; }
        public long DocumentosRecibidos { get; set; }
    }

    public class RecopilacionRegionViewModel
    {
        public string Region { get; set; }
        public int RegionId { get; set; }
        //--
        public long Areas { get; set; }
        public long Usuarios { get; set; }
        public long DocumentosEnviados { get; set; }
        public long DocumentosRecibidos { get; set; }
    }

    public class RecopilacionAreaViewModel
    {
        public string Area { get; set; }
        public int AreaId { get; set; }
        //--
        public long Usuarios { get; set; }
        public long DocumentosEnviados { get; set; }
        public long DocumentosRecibidos { get; set; }
    }
}
