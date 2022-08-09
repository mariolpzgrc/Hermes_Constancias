using Hermes2018.Models.Documento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
	public class HER_EstadoEnvio
	{
		[Key]
		public int HER_EstadoEnvioId { get; set; }
		public string HER_Nombre { get; set; }
        //---
        public IEnumerable<HER_Envio> HER_Envios { get; set; }
        public IEnumerable<HER_Recepcion> HER_Recepciones { get; set; }
    }
}
