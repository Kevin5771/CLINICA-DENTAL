using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ClinicaDental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PacientesController : ControllerBase
{
    private readonly IPacienteRepository _repository;

    public PacientesController(IPacienteRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? texto, [FromQuery] bool? activo)
        => Ok(await _repository.ListAsync(texto, activo));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PacienteCreateDto dto)
    {
        var validation = Validate(dto);
        if (validation is not null) return validation;

        var created = await _repository.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdPaciente }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PacienteUpdateDto dto)
    {
        var validation = Validate(dto);
        if (validation is not null) return validation;

        var updated = await _repository.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.DeactivateAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    private IActionResult? Validate(PacienteCreateDto dto)
    {
        if (dto.FechaNacimiento.Date > DateTime.Today)
            return BadRequest(new { message = "La fecha de nacimiento no puede ser futura." });

        if (!Regex.IsMatch(dto.Telefono, @"^[0-9+\-()\s]{8,20}$"))
            return BadRequest(new { message = "El teléfono no tiene un formato válido." });

        if (!Regex.IsMatch(dto.Nombres, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$"))
            return BadRequest(new { message = "Los nombres no deben contener números." });

        if (!Regex.IsMatch(dto.Apellidos, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$"))
            return BadRequest(new { message = "Los apellidos no deben contener números." });

        return null;
    }
}
