using Hermes2018.Models.Anexo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Configuracion
{
    public class HER_Configuracion
    {
        [Key]
        public int HER_ConfiguracionId { get; set; }

        public string HER_Propiedad { get; set; }
        public string HER_Valor { get; set; }
    }
}
