using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class NotificacionUsuariosViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
    public class NotificacionDocumentosViewModel
    {
        public string Asunto { get; set; }
        public string Remitente { get; set; }
        public DateTime Fecha { get; set; }

        public int EnvioId { get; set; }
        public int TipoEnvio { get; set; }
    }
    public class NotificacionProximosVencerViewModel
    {
        public string Asunto { get; set; }
        public string Remitente { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Compromiso { get; set; }

        public int EnvioId { get; set; }
        public int TipoEnvio { get; set; }
    }
}
