using Hermes2018.Models.Anexo;
using Hermes2018.Models.Area;
using Hermes2018.Models.Carpeta;
using Hermes2018.Models.Documento;
using Hermes2018.Models.Tramite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Documento
{
    public class HER_Envio
    {
        //Bandeja de Enviados
        //Remitente
        [Key]
        public int HER_EnvioId { get; set; }
        //--
        public string HER_DeDireccion { get; set; }
        public string HER_DeTelefono { get; set; }
        //--
        public int? HER_UsuarioOrigenId { get; set; }
        [ForeignKey("HER_UsuarioOrigenId")]
        public HER_InfoUsuario HER_UsuarioOrigen { get; set; }

        public int HER_Orden { get; set; }
        //--
        public int HER_TotalPara { get; set; }
        public int HER_TotalCCP { get; set; }

        //Solo se contabilizan las Respuestas definitivas
        public int HER_TotalParaRespuestas { get; set; }
        public int HER_TotalCCPRespuestas { get; set; }
        //--
        public string HER_Indicaciones { get; set; }
        public bool HER_RequiereRespuesta { get; set; }
        public DateTime HER_FechaEnvio { get; set; }
        //Fecha que propone el que envia el documento
        public DateTime HER_FechaPropuesta { get; set; }
        //----
        public int HER_GrupoEnvio { get; set; }
        public bool HER_EsReenvio { get; set; }
        //--
        public int HER_DocumentoId { get; set; }
        [ForeignKey("HER_DocumentoId")]
        public HER_Documento HER_Documento { get; set; }

        public int HER_DeId { get; set; }
        [ForeignKey("HER_DeId")]
        public HER_InfoUsuario HER_De { get; set; }

        public int HER_UsuarioEnviaId { get; set; }
        [ForeignKey("HER_UsuarioEnviaId")]
        public HER_InfoUsuario HER_UsuarioEnvia { get; set; }

        public int HER_TipoEnvioId { get; set; }
        [ForeignKey("HER_TipoEnvioId")]
        public HER_TipoEnvio HER_TipoEnvio { get; set; }

        public int? HER_EnvioPadreId { get; set; }
        [ForeignKey("HER_EnvioPadreId")]
        public HER_Envio HER_EnvioPadre { get; set; }

        public int HER_EstadoEnvioId { get; set; }
        [ForeignKey("HER_EstadoEnvioId")]
        public HER_EstadoEnvio HER_EstadoEnvio { get; set; }

        public int HER_VisibilidadId { get; set; }
        [ForeignKey("HER_VisibilidadId")]
        public HER_Visibilidad HER_Visibilidad { get; set; }

        public int HER_ImportanciaId { get; set; }
        [ForeignKey("HER_ImportanciaId")]
        public HER_Importancia HER_Importancia { get; set; }

        public int? HER_AnexoId { get; set; }
        [ForeignKey("HER_AnexoId")]
        public HER_Anexo HER_Anexo { get; set; }

        public int? HER_CarpetaId { get; set; }
        [ForeignKey("HER_CarpetaId")]
        public HER_Carpeta HER_Carpeta { get; set; }
        //---
        public int? HER_TramiteId { get; set; }
        [ForeignKey("HER_TramiteId")]
        public HER_Tramite HER_Tramite { get; set; }

        public bool HER_EsOculto { get; set; }
        //--
        public IEnumerable<HER_Recepcion> HER_Recepciones { get; set; }

        //Historial correo
        public IEnumerable<HER_HistorialCorreo> HER_HistorialCorreo { get; set; }

        //Oficio Cancelado
        public HER_Cancelado HER_Cancelado { get; set; }
    }
}
