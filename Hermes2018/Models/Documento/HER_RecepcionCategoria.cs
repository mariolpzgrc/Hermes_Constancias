using Hermes2018.Models.Categoria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_RecepcionCategoria
    {
        [Key]
        public int HER_RecepcionCategoriaId { get; set; }

        public int HER_RecepcionId { get; set; }
        [ForeignKey("HER_RecepcionId")]
        public HER_Recepcion HER_Recepcion { get; set; }

        public int HER_CategoriaId { get; set; }
        [ForeignKey("HER_CategoriaId")]
        public HER_Categoria HER_Categoria { get; set; }
    }
}
