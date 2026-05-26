using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace ClinicaDental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class VentasController : ControllerBase
{
    private readonly IVentaRepository _repository;

    public VentasController(IVentaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? texto,
        [FromQuery] int? idPaciente,
        [FromQuery] int? idUsuarioResponsable,
        [FromQuery] int? idCategoriaServicio,
        [FromQuery] int? idEstadoVenta,
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta)
    {
        var ventas = await _repository.ListServiciosAsync(
            texto,
            idPaciente,
            idUsuarioResponsable,
            idCategoriaServicio,
            idEstadoVenta,
            fechaDesde,
            fechaHasta);

        return Ok(ventas);
    }

    [HttpPost("servicio")]
    public async Task<IActionResult> RegistrarServicio([FromBody] VentaServicioCreateDto dto)
    {
        var idUsuarioSesion = GetCurrentUserId();
        if (idUsuarioSesion <= 0)
        {
            return Unauthorized(new { message = "No se pudo identificar el usuario autenticado." });
        }

        var validation = ValidateVentaServicio(dto);
        if (validation is not null) return validation;

        try
        {
            var venta = await _repository.RegistrarServicioAsync(dto, idUsuarioSesion);
            return Ok(venta);
        }
        catch (SqlException ex) when (ex.Number is >= 50901 and <= 50930)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private int GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        return int.TryParse(value, out var idUsuario) ? idUsuario : 0;
    }

    private IActionResult? ValidateVentaServicio(VentaServicioCreateDto dto)
    {
        dto.NumeroComprobante = string.IsNullOrWhiteSpace(dto.NumeroComprobante) ? null : dto.NumeroComprobante.Trim();
        dto.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();

        if (dto.IdPaciente <= 0)
            return BadRequest(new { message = "Debe seleccionar un paciente válido." });

        if (dto.IdUsuarioResponsable <= 0)
            return BadRequest(new { message = "Debe seleccionar el personal responsable." });

        if (dto.IdCategoriaServicio <= 0)
            return BadRequest(new { message = "Debe seleccionar una categoría de servicio válida." });

        if (dto.Precio <= 0)
            return BadRequest(new { message = "El precio debe ser mayor a cero." });

        if (dto.NumeroComprobante is { Length: > 20 })
            return BadRequest(new { message = "El número de comprobante no debe superar 20 caracteres." });

        if (dto.Descripcion is { Length: > 250 })
            return BadRequest(new { message = "La descripción no debe superar 250 caracteres." });

        if (dto.FechaVenta.HasValue && dto.FechaVenta.Value.Date < new DateTime(1900, 1, 1))
            return BadRequest(new { message = "La fecha de venta no es válida." });

        return null;
    }
}
