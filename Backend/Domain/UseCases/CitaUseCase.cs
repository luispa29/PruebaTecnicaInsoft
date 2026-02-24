using Application.Ports.Driven;
using Application.Ports.Driving;
using Models.Request;
using Models.Response;

namespace Domain.UseCases
{
    public class CitaUseCase(ICitaRepositorio _repositorio) : ICitaServicio
    {
        private static readonly TimeSpan HoraInicio = new(8, 0, 0);
        private static readonly TimeSpan HoraFin = new(14, 0, 0);
        private const int IntervaloMinutos = 30;

        public async Task<(IEnumerable<CitaResponse> citas, int total, string mensaje)> ConsultarPorPlaca(string placa, int numeroPagina, int tamanoPagina)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(placa))
                    return (Enumerable.Empty<CitaResponse>(), 0, "La placa es requerida.");

                var (citas, total) = await _repositorio.ConsultarPorPlaca(placa.Trim().ToUpper(), numeroPagina, tamanoPagina);

                if (!citas.Any())
                    return (citas, 0, "No se encontraron citas para la placa indicada.");

                return (citas, total, "Citas obtenidas exitosamente.");
            }
            catch (Exception)
            {
                return (Enumerable.Empty<CitaResponse>(), 0, "Ocurrió un error inesperado al procesar la solicitud.");
            }
        }

        public async Task<(int id, string mensaje)> AgendarCita(AgendarCitaRequest solicitud)
        {
            try
            {
                var errorValidacion = ValidarSolicitud(solicitud);
                if (!string.IsNullOrEmpty(errorValidacion))
                    return (0, errorValidacion);

                if (!TimeSpan.TryParseExact(solicitud.HoraCita, "hh\\:mm", null, out var horaCita))
                    return (0, "El formato de la hora es inválido. Use HH:mm (ej: 08:00).");

                var errorNegocio = ValidarReglasNegocio(solicitud.FechaCita, horaCita);
                if (!string.IsNullOrEmpty(errorNegocio))
                    return (0, errorNegocio);

                var ocupado = await _repositorio.ExisteEnHorario(solicitud.FechaCita, horaCita);
                if (ocupado)
                    return (0, "El horario seleccionado ya está ocupado. Por favor elija otro.");

                var vehiculoId = await _repositorio.ObtenerOCrearVehiculo(
                    solicitud.Placa.Trim().ToUpper(),
                    solicitud.Marca,
                    solicitud.Modelo,
                    solicitud.Anio);

                if (vehiculoId == 0)
                    return (0, "No se pudo registrar el vehículo.");

                var citaId = await _repositorio.Crear(vehiculoId, solicitud.FechaCita, horaCita, solicitud.Descripcion);

                if (citaId == 0)
                    return (0, "No se pudo agendar la cita.");

                return (citaId, "Cita agendada exitosamente.");
            }
            catch (Exception)
            {
                return (0, "Ocurrió un error inesperado al procesar la solicitud.");
            }
        }

        public async Task<IEnumerable<HorarioDisponibleResponse>> ObtenerHorariosDisponibles(DateTime fecha)
        {
            try
            {
                var slotsGenerados = GenerarSlots();
                var horasOcupadas = await _repositorio.ObtenerHorasOcupadas(fecha);

                return slotsGenerados.Select(slot => new HorarioDisponibleResponse
                {
                    Hora = slot,
                    Disponible = !horasOcupadas.Contains(slot)
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string ValidarSolicitud(AgendarCitaRequest solicitud)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(solicitud.Placa))
                errores.Add("La placa es requerida");

            if (string.IsNullOrWhiteSpace(solicitud.HoraCita))
                errores.Add("La hora de la cita es requerida");

            if (solicitud.FechaCita == default)
                errores.Add("La fecha de la cita es requerida");

            return string.Join(", ", errores);
        }

        private static string ValidarReglasNegocio(DateTime fechaCita, TimeSpan horaCita)
        {
            var diaSemana = fechaCita.DayOfWeek;

            if (diaSemana == DayOfWeek.Saturday || diaSemana == DayOfWeek.Sunday)
                return "Las citas solo se pueden agendar de lunes a viernes.";

            if (horaCita < HoraInicio || horaCita >= HoraFin)
                return $"Las citas solo se pueden agendar entre las {HoraInicio:hh\\:mm} y las {HoraFin:hh\\:mm}.";

            if (horaCita.Minutes % IntervaloMinutos != 0)
                return $"Los intervalos de atención son de {IntervaloMinutos} minutos (ej: 08:00, 08:30, 09:00).";

            return string.Empty;
        }

        private static List<string> GenerarSlots()
        {
            var slots = new List<string>();
            var actual = HoraInicio;

            while (actual < HoraFin)
            {
                slots.Add(actual.ToString(@"hh\:mm"));
                actual = actual.Add(TimeSpan.FromMinutes(IntervaloMinutos));
            }

            return slots;
        }
    }
}
