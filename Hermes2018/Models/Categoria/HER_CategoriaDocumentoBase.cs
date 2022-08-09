using Hermes2018.Models.Documento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Categoria
{
    public class HER_CategoriaDocumentoBase
    {
        [Key]
        public int HER_CategoriaDocumentoBaseId { get; set; }

        public int HER_CategoriaId { get; set; }
        [ForeignKey("HER_CategoriaId")]
        public HER_Categoria HER_Categoria { get; set; }

        public int HER_DocumentoBaseId { get; set; }
        [ForeignKey("HER_DocumentoBaseId")]
        public HER_DocumentoBase HER_DocumentoBase { get; set; }
    }
}
