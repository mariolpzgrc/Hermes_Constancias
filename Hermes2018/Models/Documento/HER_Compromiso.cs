using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_Compromiso
    {
        [Key]
        public int HER_CompromisoId { get; set; }

        //Fecha compromiso
        public DateTime HER_Fecha { get; set; }
        public int HER_Tipo { get; set; }
        public int HER_Estado { get; set; }
        public DateTime HER_Registro { get; set; }
        public string HER_Motivo { get; set; }

        public int HER_RecepcionId { get; set; }
        [ForeignKey("HER_RecepcionId")]
        public HER_Recepcion HER_Recepcion { get; set; }
    }
}
