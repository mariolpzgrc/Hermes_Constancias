using Hermes2018.Models.Documento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Anexo
{
	public class HER_Anexo
	{
		[Key]
		public int HER_AnexoId { get; set; }

        public int HER_Tipo { get; set; }
        public IEnumerable<HER_AnexoArchivo> HER_AnexoArchivos { get; set; }
        //--
        public HER_DocumentoBase HER_DocumentoBase { get; set; }
        public HER_Envio HER_Envio { get; set; }
    }
}
