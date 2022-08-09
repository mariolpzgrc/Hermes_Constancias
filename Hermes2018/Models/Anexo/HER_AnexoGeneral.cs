using Hermes2018.Models.Configuracion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Anexo
{
    public class HER_AnexoGeneral
    {
        [Key]
        public int HER_AnexoGeneralId { get; set; }

        public string HER_RutaComplemento { get; set; }
        public string HER_Nombre { get; set; }
        public string HER_TipoContenido { get; set; }
        public int HER_TipoRegistro { get; set; }
    }
}
