using Hermes2018.Models.Area;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Servicio
{
    public class HER_Servicio
    {
        [Key]
        public int HER_ServicioId { get; set; }

        public string HER_Nombre { get; set; }

        public string HER_CreadorId { get; set; }
        [ForeignKey("HER_CreadorId")]
        public HER_Usuario HER_Creador { get; set; }

        public int HER_RegionId { get; set; }
        [ForeignKey("HER_RegionId")]
        public HER_Region HER_Region { get; set; }

        public IEnumerable<HER_ServicioIntegrante> HER_Usuarios { get; set; }
    }
}
