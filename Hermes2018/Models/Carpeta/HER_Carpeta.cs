using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Carpeta
{
    public class HER_Carpeta
    {
        [Key]
        public int HER_CarpetaId { get; set; } 

        public string HER_Nombre { get; set; }
        public DateTime HER_FechaRegistro { get; set; }
        public DateTime HER_FechaActualizacion { get; set; }
        public int HER_Nivel { get; set; }

        public int? HER_CarpetaPadreId { get; set; }
        [ForeignKey("HER_CarpetaPadreId")]
        public HER_Carpeta HER_CarpetaPadre { get; set; }

        public int HER_InfoUsuarioId { get; set; }
        [ForeignKey("HER_InfoUsuarioId")]
        public HER_InfoUsuario HER_InfoUsuario { get; set; }
    }
}
