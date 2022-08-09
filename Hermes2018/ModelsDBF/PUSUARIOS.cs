using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hermes2018.ModelsDBF
{
    public partial class PUSUARIOS
    {
        [Key]
        public double USUARIO_ID { get; set; }
        [StringLength(255)]
        public string SHGUSUARIO_OWNER { get; set; }
        [StringLength(255)]
        public string SHGUSUARIO_NOMBRE { get; set; }
        public double? SHCDEPENDENCIA_ID { get; set; }
        [StringLength(255)]
        public string SHGUSUARIO_DEPENDENCIA { get; set; }
        [StringLength(255)]
        public string SHGUSUARIO_DEPENDENCIAGRAL { get; set; }
        public double? TITULAR { get; set; }
        [StringLength(255)]
        public string CORREO { get; set; }
        [StringLength(255)]
        public string PUESTO { get; set; }
        [StringLength(255)]
        public string TELEFONO { get; set; }
        [StringLength(255)]
        public string DIRECCION { get; set; }
        public double? AREA_ID { get; set; }
        public double? SHGUSUARIO_ALERTA { get; set; }
    }
}
