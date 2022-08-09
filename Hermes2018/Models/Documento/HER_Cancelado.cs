using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_Cancelado
    {
        [Key]
        public int HER_CanceladoId { get; set; }
        public DateTime HER_Fecha { get; set; }
        public string HER_Motivo { get; set; }
        public int HER_EnvioId { get; set; }
        [ForeignKey("HER_EnvioId")]
        public HER_Envio HER_Envio { get; set; }
    }
}
