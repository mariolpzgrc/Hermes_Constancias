using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Calendario
{
    public class HER_CalendarioContenido
    {
        [Key]
        public int HER_CalendarioContenidoId { get; set; }
        public int HER_Mes { get; set; }
        public int HER_Dia { get; set; }
        public DateTime HER_Fecha { get; set; }

        public int HER_CalendarioId { get; set; }
        [ForeignKey("HER_CalendarioId")]
        public HER_Calendario HER_Calendario { get; set; }
    }
}
