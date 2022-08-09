using Hermes2018.Models.Anexo;
using Hermes2018.Models.Configuracion;
using Hermes2018.Models.Documento;
using Hermes2018.Models.Recopilacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Area
{
    public class HER_Area
    {
        [Key]
        public int HER_AreaId { get; set; }

        public string HER_Nombre { get; set; }
        public string HER_Clave { get; set; }
        public int HER_DiasCompromiso { get; set; }
        public string HER_Direccion { get; set; }
        public string HER_Telefono { get; set; }
        public bool HER_Visible { get; set; }
        public int HER_Estado { get; set; } //Para dar de baja o reasignar
        public bool HER_ExisteEnSIIU { get; set; }
        public bool HER_BajaInactividad { get; set; }
        public int HER_RegionId { get; set; }
		[ForeignKey("HER_RegionId")]
		public HER_Region HER_Region { get; set; }

        public int? HER_Area_PadreId { get; set; }
        [ForeignKey("HER_Area_PadreId")]
        public HER_Area HER_Area_Padre { get; set; }

        public int? HER_LogoId { get; set; }
        [ForeignKey("HER_LogoId")]
        public HER_AnexoArea HER_Logo { get; set; }
        
        public IEnumerable<HER_InfoUsuario> HER_Usuarios { get; set; }
        public HER_Recopilacion HER_Recopilacion { get; set; }
    }
}