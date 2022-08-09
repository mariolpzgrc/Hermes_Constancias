using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Grupo
{
    public class HER_GrupoIntegrante
    {
       [Key]
       public int HER_GrupoIntegranteId { get; set; }
        
       public int HER_GrupoId { get; set; }
       [ForeignKey("HER_GrupoId")]
       public HER_Grupo HER_Grupo { get; set; }

       public int HER_IntegranteId { get; set; }
       [ForeignKey("HER_IntegranteId")]
       public HER_InfoUsuario HER_Integrante { get; set; }
    }
}
