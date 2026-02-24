using Application.Ports.Driven;
using Models.Response;

namespace Infrastructure.Repositories
{
    public class CitaRepositorio(ISqlPort _sql) : ICitaRepositorio
    {
        public async Task<(IEnumerable<CitaResponse> citas, int total)> ConsultarPorPlaca(string placa, int numeroPagina, int tamanoPagina)
        {
            var parametros = new { Placa = placa, NumeroPagina = numeroPagina, TamanoPagina = tamanoPagina };

            var tipos = new Type[] { typeof(CitaResponse), typeof(TotalResult) };
            var resultados = await _sql.ExecuteStoredProcedureMultipleAsync("SP_Cita_ConsultarPorPlaca", tipos, parametros);

            var citas = resultados[0].Cast<CitaResponse>();
            var total = (resultados[1].FirstOrDefault() as TotalResult)?.Total ?? 0;

            return (citas, total);
        }

        public async Task<int> ObtenerOCrearVehiculo(string placa, string marca, string modelo, int anio)
        {
            var parametros = new { Placa = placa, Marca = marca, Modelo = modelo, Anio = anio };
            var resultado = await _sql.ExecuteStoredProcedureSingleAsync<VehiculoIdResult>(
                "SP_Vehiculo_ObtenerOCrearPorPlaca", parametros);
            return resultado?.VehiculoID ?? 0;
        }

        public async Task<bool> ExisteEnHorario(DateTime fechaCita, TimeSpan horaCita)
        {
            var parametros = new { FechaCita = fechaCita.Date, HoraCita = horaCita };
            var resultado = await _sql.ExecuteStoredProcedureSingleAsync<ExisteResult>(
                "SP_Cita_ExisteEnHorario", parametros);
            return resultado?.Existe ?? false;
        }

        public async Task<int> Crear(int vehiculoId, DateTime fechaCita, TimeSpan horaCita, string descripcion)
        {
            var parametros = new
            {
                VehiculoID = vehiculoId,
                FechaCita = fechaCita.Date,
                HoraCita = horaCita,
                Descripcion = descripcion
            };
            var resultado = await _sql.ExecuteStoredProcedureSingleAsync<CitaIdResult>(
                "SP_Cita_Crear", parametros);
            return resultado?.CitaID ?? 0;
        }

        public async Task<IEnumerable<string>> ObtenerHorasOcupadas(DateTime fechaCita)
        {
            var parametros = new { FechaCita = fechaCita.Date };
            var resultado = await _sql.ExecuteStoredProcedureAsync<HoraOcupadaResult>(
                "SP_Horario_ObtenerOcupados", parametros);
            return resultado.Select(h => h.HoraCita);
        }

        private class TotalResult
        {
            public int Total { get; set; }
        }

        private class VehiculoIdResult
        {
            public int VehiculoID { get; set; }
        }

        private class CitaIdResult
        {
            public int CitaID { get; set; }
        }

        private class ExisteResult
        {
            public bool Existe { get; set; }
        }

        private class HoraOcupadaResult
        {
            public string HoraCita { get; set; } = string.Empty;
        }
    }
}
