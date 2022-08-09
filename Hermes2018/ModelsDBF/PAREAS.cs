using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hermes2018.ModelsDBF
{
    public partial class PAREAS
    {
        [Key]
        public double Area_Id { get; set; }
        [StringLength(255)]
        public string Nombre { get; set; }
        [StringLength(255)]
        public string Clave { get; set; }
        public double? Dias { get; set; }
        [StringLength(255)]
        public string Direccion { get; set; }
        [StringLength(255)]
        public string Telefono { get; set; }
        public double? Region_Id { get; set; }
        public double? Area_Padre_Id { get; set; }
        [StringLength(255)]
        public string Area_Padre { get; set; }
        public bool EsSIIU { get; set; }
    }
}
