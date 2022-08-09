using Hermes2018.Models.Documento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Categoria
{
    public class HER_Categoria
    {
        [Key]
        public int HER_CategoriaId { get; set; }

        [Display(Name = "Categoría")]
        public string HER_Nombre { get; set; }

        [Display(Name = "Tipo")]
        public int HER_Tipo { get; set; }

        public int? HER_InfoUsuarioId { get; set; }
        [ForeignKey("HER_InfoUsuarioId")]
        public HER_InfoUsuario HER_InfoUsuario { get; set; }

        public IEnumerable<HER_CategoriaDocumento> HER_Documentos { get; set; }
        public IEnumerable<HER_RecepcionCategoria> HER_Recepciones { get; set; }

        public IEnumerable<HER_CategoriaDocumentoBase> HER_DocumentosBase { get; set; }
    }
}
