using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Configuracion
{
    public class HER_Color
    {
        [Key]
        public int HER_ColorId { get; set; }

        public string HER_Nombre { get; set; }
        public string HER_Codigo { get; set; }

        //public int HER_ColeccionId { get; set; }
        //[ForeignKey("HER_ColeccionId")]
        //public HER_ConfiguracionColeccion HER_Coleccion { get; set; }

        public IEnumerable<HER_ConfiguracionUsuario> HER_ConfiguracionUsuarios { get; set; }
    }
}
