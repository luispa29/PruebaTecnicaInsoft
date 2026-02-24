using Models.Response;

namespace Application.Ports.Driven
{
    public interface ICitaRepositorio
    {
        Task<(IEnumerable<CitaResponse> citas, int total)> ConsultarPorPlaca(string placa, int numeroPagina, int tamanoPagina);
        Task<int> ObtenerOCrearVehiculo(string placa, string marca, string modelo, int anio);
        Task<bool> ExisteEnHorario(DateTime fechaCita, TimeSpan horaCita);
        Task<int> Crear(int vehiculoId, DateTime fechaCita, TimeSpan horaCita, string descripcion);
        Task<IEnumerable<string>> ObtenerHorasOcupadas(DateTime fechaCita);
    }
}
