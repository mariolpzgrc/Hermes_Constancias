using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Bitacora
{
    public class HER_Bitacora
    {
        [Key]
        public int HER_BitacoraId { get; set; }

        public string HER_Operacion { get; set; }
        public string HER_Ip { get; set; }
        public DateTime HER_Fecha { get; set; }
        public string HER_UserName { get; set; }
        public string HER_NombreUsuario { get; set; }
        [ForeignKey("HER_InfoUsuarioId")]
        public int HER_InfoUsuarioId { get; set; }      
        public HER_InfoUsuario HER_InfoUsuario { get; set; }
    }
}
