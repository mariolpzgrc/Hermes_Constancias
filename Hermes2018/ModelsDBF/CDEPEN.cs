using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hermes2018.ModelsDBF
{
    public partial class CDEPEN
    {
        public double? NSUBZ { get; set; }
        public double? NAREA { get; set; }
        [StringLength(255)]
        public string TDEP { get; set; }
        public double? NIVD { get; set; }
        public double? CVEDEP { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FALT { get; set; }
        [StringLength(255)]
        public string FBAJ { get; set; }
        [StringLength(255)]
        public string DDEP { get; set; }
        [StringLength(255)]
        public string DDEPP { get; set; }
        [StringLength(255)]
        public string DCDEP { get; set; }
        public double? NSSBP { get; set; }
        [StringLength(255)]
        public string CLAVE { get; set; }
        public double? NSUBP { get; set; }
        public double? NDEPPT { get; set; }
        public double? NDEPN { get; set; }
        public double? NDEPC { get; set; }
        public double? NZON { get; set; }
        public double? NURES { get; set; }
        [StringLength(255)]
        public string NDES { get; set; }
        [StringLength(255)]
        public string DIREC1 { get; set; }
        [StringLength(255)]
        public string DIREC2 { get; set; }
        [StringLength(255)]
        public string EDIF { get; set; }
        [StringLength(255)]
        public string PISO { get; set; }
        [StringLength(255)]
        public string CIUDAD { get; set; }
        public double? ESTADO { get; set; }
        public double? CPOST { get; set; }
        [Key]
        [StringLength(50)]
        public string NDEP { get; set; }
        [Required]
        [StringLength(50)]
        public string NDEPA { get; set; }
    }
}
