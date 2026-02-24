namespace Models.Response
{
    public class RespuestaApi<T>
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public T? Datos { get; set; }
        public InfoPaginacion? Paginacion { get; set; }

        public static RespuestaApi<T> Exitosa(T datos, string mensaje = "Operación exitosa")
        {
            return new RespuestaApi<T>
            {
                Codigo = 200,
                Mensaje = mensaje,
                Datos = datos
            };
        }

        public static RespuestaApi<T> ExitosaConPaginacion(T datos, int totalRegistros, int pagina, int tamanoPagina, string mensaje = "Operación exitosa")
        {
            return new RespuestaApi<T>
            {
                Codigo = 200,
                Mensaje = mensaje,
                Datos = datos,
                Paginacion = new InfoPaginacion
                {
                    TotalRegistros = totalRegistros,
                    Pagina = pagina,
                    TamanoPagina = tamanoPagina,
                    TotalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina)
                }
            };
        }

        public static RespuestaApi<T> Error(string mensaje, int codigo = 400)
        {
            return new RespuestaApi<T>
            {
                Codigo = codigo,
                Mensaje = mensaje,
                Datos = default
            };
        }
    }

    public class InfoPaginacion
    {
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalPaginas { get; set; }
    }
}
