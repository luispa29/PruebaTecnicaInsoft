namespace Models.Request
{
    public class AgendarCitaRequest
    {
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public DateTime FechaCita { get; set; }
        public string HoraCita { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
