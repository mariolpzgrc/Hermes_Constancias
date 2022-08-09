using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_Destinatario
    {
        [Key]
        public int HER_DestinatarioId { get; set; }

        public int HER_Tipo { get; set; }

        public int HER_UDestinatarioId { get; set; }
        [ForeignKey("HER_UDestinatarioId")] 
        public HER_InfoUsuario HER_UDestinatario { get; set; }

        public int HER_DocumentoBaseId { get; set; }
        [ForeignKey("HER_DocumentoBaseId")]
        public HER_DocumentoBase HER_DocumentoBase { get; set; }
    }
}
