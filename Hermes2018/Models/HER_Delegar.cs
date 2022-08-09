using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models
{
    public class HER_Delegar
    {
        [Key]
        public int HER_DelegarId { get; set; }

        public DateTime HER_FechaRegistro { get; set; }
        public DateTime HER_FechaActualizacion { get; set; }
        public int HER_Tipo { get; set; }

        public int HER_TitularId { get; set; }
        [ForeignKey("HER_TitularId")]
        public HER_InfoUsuario HER_Titular { get; set; }
        
        public int HER_DelegadoId { get; set; }
        [ForeignKey("HER_DelegadoId")]
        public HER_InfoUsuario HER_Delegado { get; set; }
    }
}
