using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Plantilla
{
    public class HER_Plantilla
    {
        [Key]
        public int HER_PlantillaId { get; set; }

        public string HER_Nombre { get; set; }
        public string HER_Texto { get; set; }
        public DateTime HER_Fecha_Registro { get; set; }

        public int HER_InfoUsuarioId { get; set; }
        [ForeignKey("HER_InfoUsuarioId")]
        public HER_InfoUsuario HER_InfoUsuario { get; set; }
    }
}
