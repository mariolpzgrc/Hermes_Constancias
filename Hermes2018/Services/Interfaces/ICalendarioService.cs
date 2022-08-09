using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface ICalendarioService
    {
        Task<InfoCalendarioViewModel> ObtenerCalendarioAsync(int calendarioId);
        Task<List<CalendarioViewModel>> ObtenerCalendariosAsync();
        Task<bool> ExisteCalendarioAsync(string nombreCalendario, int anio);
        Task<bool> ExisteCalendarioPorIdAsync(int calendarioId);
        Task<bool> GuardarCalendarioAsync(CrearCalendarioViewModel modelo);
        Task<bool> EliminarCalendarioAsync(int calendarioId);

        Task<List<ContenidoCalendarioViewModel>> ObtenerContenidoCalendarioAsync(int calendarioId);
        Task<ResumenContenidoCalendarioViewModel> ObtenerResumenContenidoCalendarioAsync(int contenidoId);
        Task<bool> ExisteContenidoCalendarioAsync(int calendarioId, int mes, int dia);
        Task<bool> ExisteContenidoCalendarioPorIdAsync(int contenidoId);
        Task<bool> GuardarContenidoCalendarioAsync(List<DateTime> listado, int calendarioId);
        Task<bool> EliminarContenidoCalendarioAsync(int contenidoId);

        Task<List<DateTime>> ProcesaRangoFechasAsync(int calendarioId, DateTime fechaInicio, DateTime fechaFin);
        Task<CalendarioDiasInhabilesViewModel> ObtenerDiasInhabilesCalendarioActualAsync(DateTime fechaGenerada);
        Task<CalendarioDiasInhabilesViewModel> ObtenerDiasInhabilesCalendarioActualAsync(DateTime fechaGenerada, int envioId);
        Task<CalendarioDiasInhabilesViewModel> ObtenerDiasInhabilesCalendarioActualPorDiasDadosAsync(int numDias);
        Task<CalendarioDiasInhabilesViewModel> ObtenerDiasInhabilesCalendarioActualPorDiasDadosAsync(int numDias, int recepcionId);
    }
}
