using Hermes2018.Models.Documento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_EnvioRevision
    {
        [Key]
        public int HER_EnvioRevisionId { get; set; }
        //--
        public int HER_EstadoRemitente { get; set; }
        public int HER_EstadoDestinatario { get; set; }
        public DateTime HER_Fecha { get; set; }
        //---
        public int HER_DocumentoBaseId { get; set; } 
        [ForeignKey("HER_DocumentoBaseId")]
        public HER_DocumentoBase HER_DocumentoBase { get; set; }

        public int HER_RevisionRemitenteId { get; set; }
        [ForeignKey("HER_RevisionRemitenteId")]
        public HER_InfoUsuario HER_RevisionRemitente { get; set; }

        public int HER_RevisionDestinatarioId { get; set; }
        [ForeignKey("HER_RevisionDestinatarioId")]
        public HER_InfoUsuario HER_RevisionDestinatario { get; set; }
    }
}
