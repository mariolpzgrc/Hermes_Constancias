using Hermes2018.Models.Documento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Tramite
{
    public class HER_Tramite
    {
        [Key]
        public int HER_TramiteId { get; set; }
        public string HER_Nombre { get; set; }
        public string HER_Descripcion { get; set; }
        public int HER_Dias { get; set; }
        public int HER_Estado { get; set; } //ConstTramiteEstado
        public DateTime HER_FechaRegistro { get; set; }
        public DateTime HER_FechaActualizacion { get; set; }

        public int? HER_CreadorId { get; set; }
        [ForeignKey("HER_CreadorId")]
        public HER_InfoUsuario HER_Creador { get; set; }

        public HER_Envio HER_Envio { get; set; }
    }
}
