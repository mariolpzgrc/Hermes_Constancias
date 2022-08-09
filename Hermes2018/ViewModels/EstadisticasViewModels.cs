using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class EstadisticasRecibidosPorEstadoViewModel
    {
        public int EstadoId { get; set; }
        public string Estado { get; set; }
        public int Total { get; set; }
    }
    public class EstadisticasEnviadosPorEstadoViewModel
    {
        public int EstadoId { get; set; }
        public string Estado { get; set; }
        public int Total { get; set; }
    }
    public class EstadisticasRecibidosViewModel
    {
        public int Respuestas { get; set; }

        public List<EstadisticasRecibidosPorEstadoViewModel> Recibidos { get; set; }
    }
    public class EstadisticasEnviadosViewModel
    {
        public int Respuestas { get; set; }

        public List<EstadisticasEnviadosPorEstadoViewModel> Enviados { get; set; }
    }
}
