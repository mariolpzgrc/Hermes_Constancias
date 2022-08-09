using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Models.Calendario;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Services
{
    public class CalendarioService : ICalendarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly CultureInfo _cultureEs;
        private readonly string[] _mesesTexto;

        public CalendarioService(ApplicationDbContext context)
        {
            _context = context;
            _cultureEs = new CultureInfo("es-MX");
            _mesesTexto = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
        }

        public async Task<List<CalendarioViewModel>> ObtenerCalendariosAsync()
        {
            var calendariosQuery = _context.HER_Calendario
                         //.Include(x => x.HER_Contenido)
                         .AsNoTracking()
                         .OrderBy(x => x.HER_Anio)
                         .Select(x => new CalendarioViewModel
                         {
                             CalendarioId = x.HER_CalendarioId,
                             Titulo = x.HER_Titulo,
                             Anio = x.HER_Anio,
                             TotalDias = x.HER_Contenido.Count()
                         })
                        .AsQueryable();

            return await calendariosQuery.ToListAsync();
        }
        public async Task<bool> ExisteCalendarioAsync(string nombreCalendario, int anio)
        {
            var existeQuery = _context.HER_Calendario
                .Where(x => x.HER_Titulo == nombreCalendario || x.HER_Anio == anio)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> ExisteCalendarioPorIdAsync(int calendarioId)
        {
            var existeQuery = _context.HER_Calendario
                .Where(x => x.HER_CalendarioId == calendarioId)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> GuardarCalendarioAsync(CrearCalendarioViewModel modelo)
        {
            int result = 0;

            //Calendario 
            var calendario = new HER_Calendario()
            {
                HER_Titulo = modelo.NombreCalendario,
                HER_Anio = int.Parse(modelo.Anio)
            };

            _context.HER_Calendario.Add(calendario);
            result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
        public async Task<InfoCalendarioViewModel> ObtenerCalendarioAsync(int calendarioId)
        {
            var calendariosQuery = _context.HER_Calendario
                         .Where(x => x.HER_CalendarioId == calendarioId)
                         .AsNoTracking()
                         .Select(x => new InfoCalendarioViewModel
                         {
                             CalendarioId = x.HER_CalendarioId,
                             Titulo = x.HER_Titulo,
                             Anio = x.HER_Anio
                         })
                        .AsQueryable();

            return await calendariosQuery.FirstOrDefaultAsync();
        }

        public async Task<bool> GuardarContenidoCalendarioAsync(List<DateTime> listado, int calendarioId)
        {
            int result = 0;
            List<HER_CalendarioContenido> listadoContenido = new List<HER_CalendarioContenido>();

            if (listado.Count > 0)
            {
                foreach (var fecha in listado)
                {
                    listadoContenido.Add(new HER_CalendarioContenido()
                    {
                        HER_Mes = fecha.Month,
                        HER_Dia = fecha.Day,
                        HER_Fecha = fecha,
                        HER_CalendarioId = calendarioId
                    });
                }

                _context.HER_CalendarioContenido.AddRange(listadoContenido);
                result = await _context.SaveChangesAsync();
            }

            return result > 0 ? true : false;
        }
        public async Task<List<ContenidoCalendarioViewModel>> ObtenerContenidoCalendarioAsync(int calendarioId)
        {
            var contenidoQuery = _context.HER_CalendarioContenido
                .Where(x => x.HER_CalendarioId == calendarioId)
                .AsNoTracking()
                .OrderBy(x => x.HER_Fecha)
                .GroupBy(x => x.HER_Mes, (x, y) => new
                {
                    Mes = _mesesTexto[x - 1],
                    Listado = y.Select(a => new ContenidoParcialCalendarioViewModel { ContenidoId = a.HER_CalendarioContenidoId, Dia = a.HER_Dia, FechaCompleta = a.HER_Fecha.ToString("dd/MM/yyyy", _cultureEs) }).ToList()
                })
                .Select(x => new ContenidoCalendarioViewModel
                {
                    Mes = x.Mes,
                    Listado = x.Listado
                })
                .AsQueryable();

            return await contenidoQuery.ToListAsync();
        }
        public async Task<bool> ExisteContenidoCalendarioAsync(int calendarioId, int mes, int dia)
        {
            var existeQuery = _context.HER_CalendarioContenido
               .Where(x => x.HER_CalendarioId == calendarioId && x.HER_Mes == mes && x.HER_Dia == dia)
               .AsNoTracking()
               .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<bool> ExisteContenidoCalendarioPorIdAsync(int contenidoId)
        {
            var existeQuery = _context.HER_CalendarioContenido
               .Where(x => x.HER_CalendarioContenidoId == contenidoId)
               .AsNoTracking()
               .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<List<DateTime>> ProcesaRangoFechasAsync(int calendarioId, DateTime fechaInicio, DateTime fechaFin)
        {
            List<DateTime> listado = new List<DateTime>();
            DateTime tmpFecha = fechaInicio;

            var contenidoQuery = _context.HER_CalendarioContenido
                .Where(x => x.HER_CalendarioId == calendarioId)
                .Select(x => x.HER_Fecha)
                .AsQueryable();

            var fechas = await contenidoQuery.ToListAsync();

            if (fechaInicio <= fechaFin)
            {
                do
                {
                    if (!fechas.Contains(tmpFecha) && (tmpFecha.DayOfWeek != DayOfWeek.Saturday || tmpFecha.DayOfWeek != DayOfWeek.Sunday))
                    {
                        listado.Add(tmpFecha);
                    }
                    
                    tmpFecha = tmpFecha.AddDays(1);
                } while (tmpFecha <= fechaFin);
            }

            return listado;
        }

        public async Task<bool> EliminarCalendarioAsync(int calendarioId)
        {
            int result = 0;

            var contenidoQuery = _context.HER_CalendarioContenido
               .Where(x => x.HER_CalendarioId == calendarioId)
               .AsNoTracking()
               .AsQueryable();

            var contenido = await contenidoQuery.ToListAsync();

            //Borrar
            _context.HER_CalendarioContenido.RemoveRange(contenido);
            result = _context.SaveChanges();

            if (result > 0)
            {
                var calendarioQuery = _context.HER_Calendario
                    .Where(x => x.HER_CalendarioId == calendarioId)
                    .AsNoTracking()
                    .AsQueryable();

                var calendario = await calendarioQuery.FirstOrDefaultAsync();

                //Borrar
                _context.HER_Calendario.Remove(calendario);
                result = _context.SaveChanges();
            }

            return result > 0 ? true : false;
        }

        public async Task<ResumenContenidoCalendarioViewModel> ObtenerResumenContenidoCalendarioAsync(int contenidoId)
        {
            var contenidoQuery = _context.HER_CalendarioContenido
                .Where(x => x.HER_CalendarioContenidoId == contenidoId)
                //.Include(x => x.HER_Calendario)
                .AsNoTracking()
                .Select(x => new ResumenContenidoCalendarioViewModel() {
                    CalendarioId = x.HER_CalendarioId,
                    Calendario_Titulo = x.HER_Calendario.HER_Titulo,
                    Calendario_Anio = x.HER_Calendario.HER_Anio,
                    Contenido_Fecha = x.HER_Fecha.ToString("dd/MM/yyyy", _cultureEs),
                    ContenidoId = x.HER_CalendarioContenidoId
                })
                .AsQueryable();

            return await contenidoQuery.FirstOrDefaultAsync();
        }
        public async Task<bool> EliminarContenidoCalendarioAsync(int contenidoId)
        {
            int result = 0;

            var contenidoQuery = _context.HER_CalendarioContenido
                .Where(x => x.HER_CalendarioContenidoId == contenidoId)
                .AsNoTracking()
                .AsQueryable();

            var contenido = await contenidoQuery.FirstOrDefaultAsync();

            //Borrar
            _context.HER_CalendarioContenido.Remove(contenido);
            result = _context.SaveChanges();

            return result > 0 ? true : false;
        }

        public async Task<CalendarioDiasInhabilesViewModel> ObtenerDiasInhabilesCalendarioActualAsync(DateTime fechaGenerada)
        {
            if(fechaGenerada.DayOfWeek == DayOfWeek.Saturday)
            {
                fechaGenerada = fechaGenerada.AddDays(2);
            }else if(fechaGenerada.DayOfWeek == DayOfWeek.Sunday)
            {
                fechaGenerada = fechaGenerada.AddDays(1);
            }

            CalendarioDiasInhabilesViewModel contenido = new CalendarioDiasInhabilesViewModel();
            List<string> listado = new List<string>();
            //--
            DateTime tmp = DateTime.Now;
            DateTime fechaInicioCompromiso = new DateTime(tmp.Year, tmp.Month, tmp.Day, 0, 0, 0);

            //Comparacion dl siguiente dia
            var diasFestivosQuery = _context.HER_CalendarioContenido
               .Where(x => x.HER_Fecha >= fechaInicioCompromiso)
               .Select(x => x.HER_Fecha)
               .Take(60)
               .AsQueryable();
           

            //--
            var contenidoActualQuery = _context.HER_CalendarioContenido
                    .Where(x => x.HER_Fecha >= fechaInicioCompromiso && x.HER_Fecha <= fechaGenerada)
                    .AsNoTracking()
                    .OrderBy(x => x.HER_Fecha)
                    .Select(x => x.HER_Fecha.ToString("dd/MM/yyyy", _cultureEs))
                    .AsQueryable();

            listado.AddRange(await contenidoActualQuery.ToListAsync());
            //--
            DateTime inicio = fechaInicioCompromiso;
            DateTime primerDia = inicio;
            DateTime ultimoDia = fechaGenerada;
            TimeSpan diferencia = ultimoDia - primerDia;

            int totalDias = diferencia.Days;
            DateTime actualDia = new DateTime();
            //--

            for (var i = 0; i <= totalDias; i++)
            {
                actualDia = primerDia.AddDays(i);
                switch (actualDia.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        listado.Add(actualDia.ToString("dd/MM/yyyy", _cultureEs));
                        break;
                }
            }
            listado.Add(inicio.ToString("dd/MM/yyyy", _cultureEs));
            contenido.Dias = string.Join(",", listado);
            contenido.Inicio = inicio.ToString("dd/MM/yyyy", _cultureEs);           
            contenido.Limite = ultimoDia.ToString("dd/MM/yyyy", _cultureEs);
            contenido.EsVigente = DateTime.Compare(ultimoDia, fechaInicioCompromiso);

            return contenido;
        }
        public async Task<CalendarioDiasInhabilesViewModel> ObtenerDiasInhabilesCalendarioActualAsync(DateTime fechaGenerada, int envioId)
        {
            CalendarioDiasInhabilesViewModel contenido = new CalendarioDiasInhabilesViewModel();
            List<string> listado = new List<string>();
            //--
            DateTime hoy = DateTime.Now;

            //Comparacion dl siguiente dia
            var diasFestivosQuery = _context.HER_CalendarioContenido
               .Where(x => x.HER_Fecha >= hoy)
               .Select(x => x.HER_Fecha)
               .Take(60)
               .AsQueryable();

            var diasFestivos = await diasFestivosQuery.ToListAsync();       

            //--
            var contenidoActualQuery = _context.HER_CalendarioContenido
                    .Where(x => x.HER_Fecha >= hoy && x.HER_Fecha <= fechaGenerada)
                    .AsNoTracking()
                    .OrderBy(x => x.HER_Fecha)
                    .Select(x => x.HER_Fecha.ToString("dd/MM/yyyy", _cultureEs))
                    .AsQueryable();

            listado.AddRange(await contenidoActualQuery.ToListAsync());
            //--
            DateTime inicio = hoy;
            DateTime primerDia = inicio;
            DateTime ultimoDia = fechaGenerada;
            TimeSpan diferencia = ultimoDia - primerDia;

            int totalDias = diferencia.Days;
            DateTime actualDia = new DateTime();
            //--

            for (var i = 0; i <= totalDias; i++)
            {
                actualDia = primerDia.AddDays(i);
                switch (actualDia.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        listado.Add(actualDia.ToString("dd/MM/yyyy", _cultureEs));
                        break;
                }
            }
            listado.Add(inicio.ToString("dd/MM/yyyy", _cultureEs));
            contenido.Dias = string.Join(",", listado);
            contenido.Inicio = inicio.ToString("dd/MM/yyyy", _cultureEs);
            contenido.Limite = ultimoDia.ToString("dd/MM/yyyy", _cultureEs);
            contenido.EsVigente = DateTime.Compare(ultimoDia, hoy);

            return contenido;
        }
        public async Task<CalendarioDiasInhabilesViewModel> ObtenerDiasInhabilesCalendarioActualPorDiasDadosAsync(int numDias)
        {
            DateTime tmp = DateTime.Now;
            DateTime hoy = new DateTime(tmp.Year, tmp.Month, tmp.Day, 0, 0, 0);
            DateTime fechaGenerada = hoy;
            fechaGenerada = fechaGenerada.AddDays(-1);
            List<string> listado = new List<string>();

            IQueryable<HER_CalendarioContenido> calendarioContenidoQuery = _context.HER_CalendarioContenido
                .AsNoTracking()
                .AsQueryable();

            List<DateTime> diasFestivos = await calendarioContenidoQuery
                .Where(x => x.HER_Fecha >= hoy)
                .Select(x => x.HER_Fecha)
                .Take(60)
                .ToListAsync();

            int n = 0;
            do
            {
                //Aumenta los dias
                fechaGenerada = fechaGenerada.AddDays(1);

                if (fechaGenerada.DayOfWeek == DayOfWeek.Saturday || fechaGenerada.DayOfWeek == DayOfWeek.Sunday)
                {
                    listado.Add(fechaGenerada.ToString("dd/MM/yyyy", _cultureEs));
                }
                else if (!diasFestivos.Contains(fechaGenerada))
                {
                    //Si la fecha es valida, aumenta el contador
                    n++;
                }

            } while (n < numDias);


            listado.AddRange(await calendarioContenidoQuery
                    .Where(x => x.HER_Fecha >= hoy && x.HER_Fecha <= fechaGenerada)
                    .OrderBy(x => x.HER_Fecha)
                    .Select(x => x.HER_Fecha.ToString("dd/MM/yyyy", _cultureEs))
                    .ToListAsync());

            CalendarioDiasInhabilesViewModel contenido = new CalendarioDiasInhabilesViewModel
            {
                Dias = string.Join(",", listado),
                Inicio = hoy.ToString("dd/MM/yyyy", _cultureEs),
                Limite = fechaGenerada.ToString("dd/MM/yyyy", _cultureEs),
                EsVigente = DateTime.Compare(fechaGenerada, hoy)
            };

            return contenido;
        }
        public async Task<CalendarioDiasInhabilesViewModel> ObtenerDiasInhabilesCalendarioActualPorDiasDadosAsync(int numDias, int recepcionId)
        {
            DateTime ahora = DateTime.Now;
            DateTime hoy = new DateTime(ahora.Year, ahora.Month, ahora.Day, 0, 0, 0);

            IQueryable<Models.Documento.HER_Recepcion> fechaRecepcionQuery = _context.HER_Recepcion
                .Where(x => x.HER_RecepcionId == recepcionId)
                .AsNoTracking()
                .AsQueryable();

            DateTime fechaRecepcion = await fechaRecepcionQuery
                .Select(x => x.HER_FechaRecepcion)
                .FirstOrDefaultAsync();

            DateTime tmp = new DateTime(fechaRecepcion.Year, fechaRecepcion.Month, fechaRecepcion.Day, 0, 0, 0);
            DateTime fechaGenerada = tmp;

            DateTime inicio = tmp.AddDays(1);
            List<string> listado = new List<string>();

            IQueryable<HER_CalendarioContenido> calendarioContenidoQuery = _context.HER_CalendarioContenido
                .AsNoTracking()
                .AsQueryable();

            List<DateTime> diasFestivos = await calendarioContenidoQuery
                .Where(x => x.HER_Fecha >= inicio)
                .Select(x => x.HER_Fecha)
                .Take(60)
                .ToListAsync();

            if(hoy>fechaRecepcion)
            {
                inicio = hoy.AddDays(1);
            }

            int n = 0;
            do
            {
                //Aumenta los dias
                fechaGenerada = fechaGenerada.AddDays(1);

                if (fechaGenerada.DayOfWeek == DayOfWeek.Saturday || fechaGenerada.DayOfWeek == DayOfWeek.Sunday)
                {
                    listado.Add(fechaGenerada.ToString("dd/MM/yyyy", _cultureEs));
                }
                else if (!diasFestivos.Contains(fechaGenerada))
                {
                    //Si la fecha es valida, aumenta el contador
                    n++;
                }

            } while (n < numDias);


            listado.AddRange(await calendarioContenidoQuery
                    .Where(x => x.HER_Fecha >= inicio && x.HER_Fecha <= fechaGenerada)
                    .OrderBy(x => x.HER_Fecha)
                    .Select(x => x.HER_Fecha.ToString("dd/MM/yyyy", _cultureEs))
                    .ToListAsync());

            CalendarioDiasInhabilesViewModel contenido = new CalendarioDiasInhabilesViewModel
            {
                Dias = string.Join(",", listado),
                Inicio = inicio.ToString("dd/MM/yyyy", _cultureEs),
                Limite = fechaGenerada.ToString("dd/MM/yyyy", _cultureEs),
                EsVigente = DateTime.Compare(fechaGenerada, hoy)
            };

            return contenido;
        }
       
    }
}
