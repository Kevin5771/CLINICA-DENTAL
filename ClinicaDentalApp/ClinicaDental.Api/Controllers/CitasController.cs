using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaDental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CitasController : ControllerBase
{
    private readonly ICitaRepository _repository;

    public CitasController(ICitaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        [FromQuery] int? idDentista,
        [FromQuery] int? idEstadoCita,
        [FromQuery] int? idPaciente)
        => Ok(await _repository.ListAsync(fechaDesde, fechaHasta, idDentista, idEstadoCita, idPaciente));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CitaCreateDto dto)
    {
        var validation = Validate(dto.Fecha, dto.HoraInicio, dto.HoraFin);
        if (validation is not null) return validation;

        var created = await _repository.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdCita }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CitaUpdateDto dto)
    {
        var validation = Validate(dto.Fecha, dto.HoraInicio, dto.HoraFin);
        if (validation is not null) return validation;

        var updated = await _repository.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id:int}/cancelar")]
    public async Task<IActionResult> Cancel(int id, [FromBody] CancelarCitaDto dto)
    {
        var result = await _repository.CancelAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    private IActionResult? Validate(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
    {
        if (fecha.Date < DateTime.Today)
            return BadRequest(new { message = "No se permiten citas en fechas pasadas para este arranque inicial." });

        if (horaFin <= horaInicio)
            return BadRequest(new { message = "La hora final debe ser mayor a la hora inicial." });

        return null;
    }
}
