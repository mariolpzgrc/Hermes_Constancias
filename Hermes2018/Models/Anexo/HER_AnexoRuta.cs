using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Anexo
{
    public class HER_AnexoRuta
    {
        [Key]
        public int HER_AnexoRutaId { get; set; }

        public string HER_RutaBase { get; set; }
        public int HER_Estado { get; set; }
        public DateTime HER_FechaRegistro { get; set; }
        public DateTime HER_FechaActualizacion { get; set; }

        public IEnumerable<HER_AnexoArchivo> HER_AnexoArchivos { get; set; }
        public IEnumerable<HER_AnexoArea> HER_AnexoAreas { get; set; }
    }
}
