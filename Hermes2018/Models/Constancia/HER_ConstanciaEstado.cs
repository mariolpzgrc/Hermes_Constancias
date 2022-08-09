using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hermes2018.Models.Constancia
{
    public class HER_ConstanciaEstado
    {
        [Key]
        public int HER_EstadoId { get; set; }

        public string HER_Nombre { get; set; }

        //public IEnumerable<HER_ConstanciaSeguimiento> HER_ConstanciaSeguimiento { get; set; }
    }
}
