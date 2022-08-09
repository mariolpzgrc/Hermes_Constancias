using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Configuracion
{
    public class HER_ConfiguracionUsuario
    {
        [Key]
        public int HER_ConfiguracionUsuarioId { get; set; }
        //--
        public bool HER_Notificaciones { get; set; }
        public int HER_ElementosPorPagina { get; set; }
        //--
        public string HER_UsuarioId { get; set; }
        [ForeignKey("HER_UsuarioId")]
        public HER_Usuario HER_Usuario { get; set; }

        public int HER_ColorId { get; set; }
        [ForeignKey("HER_ColorId")]
        public HER_Color HER_Color { get; set; }
    }
}
