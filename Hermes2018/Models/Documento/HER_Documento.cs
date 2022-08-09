using Hermes2018.Models.Anexo;
using Hermes2018.Models.Categoria;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_Documento
    {
        [Key]
        public int HER_DocumentoId { get; set; }

        public string HER_Folio { get; set; }
        public string HER_Asunto { get; set; }
        public string HER_NoInterno { get; set; }
        public string HER_Cuerpo { get; set; }
        public DateTime HER_FechaRegistro { get; set; }
        public string HER_Firma { get; set; }

        public int HER_TipoId { get; set; }
        [ForeignKey("HER_TipoId")]
        public HER_TipoDocumento HER_Tipo { get; set; } 

        public int HER_DocumentoTitularId { get; set; }
        [ForeignKey("HER_DocumentoTitularId")]
        public HER_InfoUsuario HER_DocumentoTitular { get; set; }

        public int HER_DocumentoCreadorId { get; set; }
        [ForeignKey("HER_DocumentoCreadorId")]
        public HER_InfoUsuario HER_DocumentoCreador { get; set; }
        
        public IEnumerable<HER_CategoriaDocumento> HER_Categorias { get; set; }
        public HER_Envio HER_Envio { get; set; }
    }
}