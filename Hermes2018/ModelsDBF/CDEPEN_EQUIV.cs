using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hermes2018.ModelsDBF
{
    public partial class CDEPEN_EQUIV
    {
        [Key]
        public double NID { get; set; }
        public double? NDEPA { get; set; }
        public double? NDEPA_EQUIV { get; set; }
    }
}
