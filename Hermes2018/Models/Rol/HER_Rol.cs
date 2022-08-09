using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Models.Rol
{
    public class HER_Rol
    {
        [Key]
        public int HER_RolId { get; set; }

        public string HER_Nombre { get; set; }
    }
}
