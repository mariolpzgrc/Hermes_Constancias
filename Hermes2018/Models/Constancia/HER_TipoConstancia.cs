using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Constancia
{
    public class HER_TipoConstancia
    {
        [Key]
        public int HER_TipoConstanciaId { get; set; }

        public string HER_NombreTipoConstancia { get; set; }

        public IEnumerable<HER_TipoPersonalConstancia> HER_TipoPersonalConstancias { get; set; }

        public HER_ConstanciaSeguimiento HER_ConstanciaSeguimiento { get; set; }
    }
}
