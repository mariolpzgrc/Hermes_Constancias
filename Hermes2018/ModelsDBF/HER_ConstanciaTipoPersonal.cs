using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hermes2018.ModelsDBF
{
    public partial class HER_ConstanciaTipoPersonal
    {
        public int Id { get; set; }
        public int HER_ConstanciaId { get; set; }
        public int HER_TipoPersonal { get; set; }
    }
}
