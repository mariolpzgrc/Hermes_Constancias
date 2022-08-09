using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_HistorialCorreo
    {

        [Key]
        public int HER_HistorialCorreoId { get; set; }

        public DateTime HER_Fecha { get; set; }

        public int HER_InfoUsuarioId { get; set; }
        [ForeignKey("HER_InfoUsuarioId")]
        public HER_InfoUsuario HER_InfoUsuario { get; set; }

        public string HER_Delegado { get; set; }

        public string HER_Destinatario {get; set;}

        public int HER_EnvioId { get; set; }
        [ForeignKey("HER_EnvioId")]
        public HER_Envio HER_Envio { get; set; }

        public int HER_Tipo { get; set; }

        public int HER_TipoEnvio { get; set; }

    }
}
