using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Anexo
{
	public class HER_AnexoArchivo
	{
		[Key]
		public int HER_AnexoArchivoId { get; set; }

        public string HER_RutaComplemento { get; set; }
        public string HER_Nombre { get; set; }
        public string HER_TipoContenido { get; set; }

        public int HER_AnexoId { get; set; }
        [ForeignKey("HER_AnexoId")]
        public HER_Anexo HER_Anexo { get; set; }

        public int? HER_AnexoRutaId { get; set; }
        [ForeignKey("HER_AnexoRutaId")]
        public HER_AnexoRuta HER_AnexoRuta { get; set; }
    }
}
