using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IServicioService
    {
        Task<List<ServiciosCompletosViewModel>> ObtenerServiciosAsync();
        Task<List<ServiciosViewModel>> ObtenerListadoServiciosAsync(string username);
        Task<DetalleServicioViewModel> ObtenerDetalleServicioAsync(int servicioId);
        Task<bool> ExisteServicioAsync(string nombreServicio, string username);
        Task<bool> GuardarServiciosAsync(CrearServicioViewModel crear, string username, int regionId);
    }
}
