using Hermes2018.Models.Area;
using Hermes2018.Models.Configuracion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Anexo
{
    public class HER_AnexoArea
    {
        [Key]
        public int HER_AnexoAreaId { get; set; }

        public string HER_RutaComplemento { get; set; }
        public string HER_Nombre { get; set; }
        public string HER_TipoContenido { get; set; }

        public HER_Area HER_Area { get; set; }

        public int? HER_AnexoRutaId { get; set; }
        [ForeignKey("HER_AnexoRutaId")]
        public HER_AnexoRuta HER_AnexoRuta { get; set; }
    }
}
