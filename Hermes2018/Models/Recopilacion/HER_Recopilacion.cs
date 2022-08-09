using Hermes2018.Models.Area;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Recopilacion
{
    public class HER_Recopilacion
    {
        [Key]
        public int HER_RecopilacionId { get; set; }

        public long Usuarios { get; set; }
        public long DocumentosEnviados { get; set; }
        public long DocumentosRecibidos { get; set; }

        public int HER_AreaId { get; set; }
        [ForeignKey("HER_AreaId")]
        public HER_Area HER_Area { get; set; }
    }
}
