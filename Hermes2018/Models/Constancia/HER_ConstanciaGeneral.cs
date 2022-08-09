using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Constancia
{
    public abstract class HER_ConstanciaGeneral
    {
        [Key]
        public int HER_ConstanciaId { get; set; }

        public int HER_NumeroPersonal { get; set; }

        public string HER_Nombre { get; set; }

        public DateTime HER_FechaIngreso { get; set; }
       
        public string HER_DepedenciaAdscripcion { get; set; }

        public string HER_Region { get; set; }

        public string HER_TipoPersonal { get; set; }

        public string HER_TipoContratacion { get; set; }

        public string HER_PuestoActual { get; set; }

        public string HER_CategoriaActual { get; set; }

        public double HER_Sueldo { get; set; }

        public string HER_NombreDirectorPersonal { get; set; }
    }
}
