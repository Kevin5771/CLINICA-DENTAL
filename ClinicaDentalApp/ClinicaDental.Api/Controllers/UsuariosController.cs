using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Repositories;
using ClinicaDental.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ClinicaDental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepository _repository;
    private readonly IPasswordService _passwordService;

    public UsuariosController(IUsuarioRepository repository, IPasswordService passwordService)
    {
        _repository = repository;
        _passwordService = passwordService;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? texto, [FromQuery] bool? activo)
        => Ok(await _repository.ListAsync(texto, activo));

    [HttpGet("dentistas")]
    public async Task<IActionResult> GetDentistas()
        => Ok(await _repository.ListDentistasAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UsuarioCreateDto dto)
    {
        var validation = ValidatePassword(dto.Password);
        if (validation is not null) return validation;

        var (hash, salt) = _passwordService.HashPassword(dto.Password);
        var created = await _repository.CreateAsync(dto, hash, salt);
        return CreatedAtAction(nameof(GetById), new { id = created.IdUsuario }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateDto dto)
    {
        var updated = await _repository.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.DeactivateAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    private IActionResult? ValidatePassword(string password)
    {
        if (!Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d).{6,}$"))
            return BadRequest(new { message = "La contraseña debe tener al menos una letra, un número y 6 caracteres." });

        return null;
    }
}