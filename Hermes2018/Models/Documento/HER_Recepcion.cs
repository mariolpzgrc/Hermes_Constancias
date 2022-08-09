using Hermes2018.Models.Area;
using Hermes2018.Models.Carpeta;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_Recepcion
    {
        //Bandeja de Recibidos
        //Destinatario
        [Key]
        public int HER_RecepcionId { get; set; }
        
        public int HER_TipoPara { get; set; }
        public bool HER_TieneRespuesta { get; set; }
        public bool HER_EstaLeido { get; set; }
        public DateTime HER_FechaRecepcion { get; set; }
        //--
        public int HER_EnvioId { get; set; }
        [ForeignKey("HER_EnvioId")]
        public HER_Envio HER_Envio { get; set; }

        public int HER_ParaId { get; set; }
        [ForeignKey("HER_ParaId")]
        public HER_InfoUsuario HER_Para { get; set; }

        public int? HER_CarpetaId { get; set; }
        [ForeignKey("HER_CarpetaId")]
        public HER_Carpeta HER_Carpeta { get; set; }

        public int HER_EstadoEnvioId { get; set; }
        [ForeignKey("HER_EstadoEnvioId")]
        public HER_EstadoEnvio HER_EstadoEnvio { get; set; }
        //---
        public IEnumerable<HER_RecepcionCategoria> HER_Categorias { get; set; }
        //Los inumerables se usa para la relacion de uno a muchos
        public IEnumerable<HER_Compromiso> HER_Compromiso { get; set; }
    }
}
