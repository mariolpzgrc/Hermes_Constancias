using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Servicio
{
    public class HER_ServicioIntegrante
    {
        [Key]
        public int HER_ServicioIntegranteId { get; set; }

        public int HER_ServicioId { get; set; }
        [ForeignKey("HER_ServicioId")]
        public HER_Servicio HER_Servicio { get; set; }

        public int HER_IntegranteId { get; set; }
        [ForeignKey("HER_IntegranteId")]
        public HER_InfoUsuario HER_Integrante { get; set; }
    }
}
