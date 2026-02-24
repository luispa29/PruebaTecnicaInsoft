using Models.Request;
using Models.Response;

namespace Application.Ports.Driving
{
    public interface ICitaServicio
    {
        Task<(IEnumerable<CitaResponse> citas, int total, string mensaje)> ConsultarPorPlaca(string placa, int numeroPagina, int tamanoPagina);
        Task<(int id, string mensaje)> AgendarCita(AgendarCitaRequest solicitud);
        Task<IEnumerable<HorarioDisponibleResponse>> ObtenerHorariosDisponibles(DateTime fecha);
    }
}
