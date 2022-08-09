using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hermes2018.Models.Constancia
{
    public class HER_ConstanciaSeguimiento
    {
        [Key]
        public int HER_Folio{ get; set; }

        [ForeignKey("HER_ConstanciaId")]
        
        public HER_ConstanciaGeneral HER_Constancia { get; set; }
        
        public int HER_TipoConstanciaId { get; set; }
        
        [ForeignKey("HER_TipoConstanciaId")]
        public HER_TipoConstancia HER_TipoConstancia { get; set; }
        
        public string HER_Mensaje { get; set; }
        
        public DateTime HER_FechaSolictud { get; set; }
        
        public DateTime HER_FechaImpresion { get; set; }
        
        //public IEnumerable<HER_ConstanciaEstado> HER_ConstanciaEstado { get; set; } 
    }
}
