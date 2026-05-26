using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ClinicaDental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ReportesController : ControllerBase
{
    private readonly IReporteRepository _repository;

    public ReportesController(IReporteRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("citas")]
    public async Task<IActionResult> Citas(
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        [FromQuery] int? idPaciente,
        [FromQuery] int? idDentista,
        [FromQuery] int? idEstadoCita)
    {
        if (RangoFechasInvalido(fechaDesde, fechaHasta))
            return BadRequest(new { message = "La fecha inicial no puede ser mayor que la fecha final." });

        try
        {
            var data = await _repository.GetCitasAsync(fechaDesde, fechaHasta, idPaciente, idDentista, idEstadoCita);
            return Ok(data);
        }
        catch (SqlException ex) when (ex.Number is >= 51501 and <= 51599)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("pacientes")]
    public async Task<IActionResult> Pacientes(
        [FromQuery] string? texto,
        [FromQuery] bool? activo)
    {
        var data = await _repository.GetPacientesAsync(texto, activo);
        return Ok(data);
    }

    [HttpGet("inventario")]
    public async Task<IActionResult> Inventario(
        [FromQuery] string? texto,
        [FromQuery] string? estado,
        [FromQuery] int? idProveedor)
    {
        var estadosValidos = new[] { "disponible", "bajo_stock", "agotado", "vencido", "proximo_vencer" };

        if (!string.IsNullOrWhiteSpace(estado) && !estadosValidos.Contains(estado))
            return BadRequest(new { message = "El estado de inventario no es válido." });

        var data = await _repository.GetInventarioAsync(texto, estado, idProveedor);
        return Ok(data);
    }

    [HttpGet("ventas")]
    public async Task<IActionResult> Ventas(
        [FromQuery] string? texto,
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        [FromQuery] int? idPaciente,
        [FromQuery] int? idUsuarioResponsable,
        [FromQuery] int? idCategoriaServicio,
        [FromQuery] int? idEstadoVenta)
    {
        if (RangoFechasInvalido(fechaDesde, fechaHasta))
            return BadRequest(new { message = "La fecha inicial no puede ser mayor que la fecha final." });

        try
        {
            var data = await _repository.GetVentasAsync(
                texto,
                fechaDesde,
                fechaHasta,
                idPaciente,
                idUsuarioResponsable,
                idCategoriaServicio,
                idEstadoVenta);

            return Ok(data);
        }
        catch (SqlException ex) when (ex.Number is >= 51501 and <= 51599)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("resumen")]
    public async Task<IActionResult> Resumen(
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta)
    {
        if (RangoFechasInvalido(fechaDesde, fechaHasta))
            return BadRequest(new { message = "La fecha inicial no puede ser mayor que la fecha final." });

        try
        {
            var data = await _repository.GetResumenAsync(fechaDesde, fechaHasta);
            return Ok(data);
        }
        catch (SqlException ex) when (ex.Number is >= 51501 and <= 51599)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private static bool RangoFechasInvalido(DateTime? fechaDesde, DateTime? fechaHasta)
    {
        return fechaDesde.HasValue
            && fechaHasta.HasValue
            && fechaDesde.Value.Date > fechaHasta.Value.Date;
    }
}