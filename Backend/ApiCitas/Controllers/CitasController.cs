using Application.Ports.Driving;
using Microsoft.AspNetCore.Mvc;
using Models.Request;
using Models.Response;

namespace ApiCitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController(ICitaServicio _citaServicio) : ControllerBase
    {
        [HttpGet("placa/{placa}")]
        public async Task<IActionResult> ConsultarPorPlaca(
            string placa,
            [FromQuery] int numeroPagina = 1,
            [FromQuery] int tamanoPagina = 10)
        {
            try
            {
                var (citas, total, mensaje) = await _citaServicio.ConsultarPorPlaca(placa, numeroPagina, tamanoPagina);

                if (!citas.Any())
                    return NotFound(RespuestaApi<object>.Error(mensaje, 404));

                return Ok(RespuestaApi<IEnumerable<CitaResponse>>.ExitosaConPaginacion(
                    citas, total, numeroPagina, tamanoPagina, mensaje));
            }
            catch (Exception ex)
            {
                return StatusCode(500, RespuestaApi<object>.Error($"Error interno: {ex.Message}", 500));
            }
        }

        [HttpGet("horarios-disponibles")]
        public async Task<IActionResult> ObtenerHorariosDisponibles([FromQuery] DateTime fecha)
        {
            try
            {
                var horarios = await _citaServicio.ObtenerHorariosDisponibles(fecha);
                return Ok(RespuestaApi<IEnumerable<HorarioDisponibleResponse>>.Exitosa(horarios));
            }
            catch (Exception ex)
            {
                return StatusCode(500, RespuestaApi<object>.Error($"Error interno: {ex.Message}", 500));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AgendarCita([FromBody] AgendarCitaRequest solicitud)
        {
            try
            {
                var (id, mensaje) = await _citaServicio.AgendarCita(solicitud);

                if (id == 0)
                    return BadRequest(RespuestaApi<object>.Error(mensaje));

                return Ok(RespuestaApi<object>.Exitosa(new { CitaID = id }, mensaje));
            }
            catch (Exception ex)
            {
                return StatusCode(500, RespuestaApi<object>.Error($"Error interno: {ex.Message}", 500));
            }
        }
    }
}
