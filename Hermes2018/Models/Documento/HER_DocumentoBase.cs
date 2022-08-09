using Hermes2018.Models.Anexo;
using Hermes2018.Models.Categoria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_DocumentoBase
    {
        [Key]
        public int HER_DocumentoBaseId { get; set; }

        public string HER_Folio { get; set; }
        public bool HER_RequiereRespuesta { get; set; }
        public bool HER_EnRevision { get; set; }
        public string HER_Asunto { get; set; }
        public string HER_NoInterno { get; set; }
        public string HER_Cuerpo { get; set; }
        public DateTime HER_FechaRegistro { get; set; }
        //Fecha que propone el que envia el documento
        public DateTime HER_FechaPropuesta { get; set; }

        public int HER_ImportanciaId { get; set; }
        [ForeignKey("HER_ImportanciaId")]
        public HER_Importancia HER_Importancia { get; set; }

        public int HER_TipoId { get; set; }
        [ForeignKey("HER_TipoId")]
        public HER_TipoDocumento HER_Tipo { get; set; } 

        public int HER_EstadoId { get; set; }
        [ForeignKey("HER_EstadoId")]
        public HER_EstadoDocumento HER_Estado { get; set; }

        public int HER_VisibilidadId { get; set; }
        [ForeignKey("HER_VisibilidadId")]
        public HER_Visibilidad HER_Visibilidad { get; set; }

        public int HER_DocumentoBaseTitularId { get; set; }
        [ForeignKey("HER_DocumentoBaseTitularId")]
        public HER_InfoUsuario HER_DocumentoBaseTitular { get; set; }

        public int HER_DocumentoBaseCreadorId { get; set; }
        [ForeignKey("HER_DocumentoBaseCreadorId")]
        public HER_InfoUsuario HER_DocumentoBaseCreador { get; set; }

        public int? HER_AnexoId { get; set; }
        [ForeignKey("HER_AnexoId")]
        public HER_Anexo HER_Anexo { get; set; }
        //--
        public IEnumerable<HER_Destinatario> HER_Destinatarios { get; set; }
        public IEnumerable<HER_CategoriaDocumentoBase> HER_Categorias { get; set; }
        public HER_EnvioRevision HER_Revision { get; set; }
    }
}
