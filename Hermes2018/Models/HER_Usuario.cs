using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Hermes2018.Models.Rol;
using Hermes2018.Models.Area;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Hermes2018.Models.Grupo;
using Hermes2018.Models.Plantilla;
using Hermes2018.Models.Documento;
using Hermes2018.Models.Servicio;
using Hermes2018.Models.Configuracion;

namespace Hermes2018.Models
{
    public class HER_Usuario : IdentityUser
    {
        public string HER_NombreCompleto { get; set; }
        public bool HER_Aprobado { get; set; }
        public DateTime HER_FechaAprobado { get; set; }
        public bool HER_AceptoTerminos { get; set; }
        public DateTime HER_FechaAceptoTerminos { get; set; }
        
        public IEnumerable<HER_InfoUsuario> HER_Usuarios { get; set; }
        public IEnumerable<HER_Servicio> HER_Servicios { get; set; }
        public HER_ConfiguracionUsuario HER_Configuracion { get; set; }
    }
}
