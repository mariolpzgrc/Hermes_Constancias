using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hermes2018.ModelsDBF
{
    public partial class HER_SolicitudConstancia
    {
        public int Id { get; set; }
        public int ConstanciaId { get; set; }
        [Required]
        [StringLength(50)]
        public string UsuarioId { get; set; }
        public int NoPersonal { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaSolicitud { get; set; }
        public int EstadoId { get; set; }
        [Required]
        [StringLength(50)]
        public string Folio { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Mensaje { get; set; }
        public int CveDep { get; set; }
        [Required]
        public string NombreDep { get; set; }
        public int TipoPersonal { get; set; }
        public int? CampusId { get; set; }
        public string NombreCampus { get; set; }
    }
}
