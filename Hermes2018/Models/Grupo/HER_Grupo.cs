using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Grupo
{
    public class HER_Grupo
    {
        [Key]
        public int HER_GrupoId { get; set; }

        public string HER_Nombre { get; set; }

        public int HER_CreadorId { get; set; }
        [ForeignKey("HER_CreadorId")]
        public HER_InfoUsuario HER_Creador { get; set; }

        public IEnumerable<HER_GrupoIntegrante> HER_Integrantes { get; set; }
    }
}
