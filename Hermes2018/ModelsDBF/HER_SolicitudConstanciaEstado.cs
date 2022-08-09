using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hermes2018.ModelsDBF
{
    public partial class HER_SolicitudConstanciaEstado
    {
        public int Id { get; set; }
        public int SolicitudConstanciaId { get; set; }
        [Required]
        [StringLength(50)]
        public string UsuarioId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaHora { get; set; }
        public int EstadoId { get; set; }
        public string Motivo { get; set; }
    }
}
