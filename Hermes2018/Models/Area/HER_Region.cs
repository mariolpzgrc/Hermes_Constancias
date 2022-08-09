using Hermes2018.Models.Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Area
{
    public class HER_Region
    {
        [Key]
        public int HER_RegionId { get; set; }

        public string HER_Nombre { get; set; }

        public IEnumerable<HER_Area> HER_Areas { get; set; }
        public IEnumerable<HER_Servicio> HER_Servicios { get; set; }
    }
}
