using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Calendario
{
    public class HER_Calendario
    {
        [Key]
        public int HER_CalendarioId { get; set; } 
        public string HER_Titulo { get; set; }
        public int HER_Anio { get; set; }

        public IEnumerable<HER_CalendarioContenido> HER_Contenido { get; set; }
    }
}
