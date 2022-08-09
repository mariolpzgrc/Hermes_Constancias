using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_EstadoDocumento
    {
        [Key]
        public int HER_EstadoDocumentoId { get; set; }
        public string HER_Nombre { get; set; }
        //---
        public IEnumerable<HER_DocumentoBase> HER_DocumentosBase { get; set; }
    }
}
