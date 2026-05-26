using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ClinicaDental.Api.Controllers;

[ApiController]
[Route("api/categorias-servicio")]
[Authorize]
public sealed class CategoriasServicioController : ControllerBase
{
    private readonly ICategoriaServicioRepository _repository;

    public CategoriasServicioController(ICategoriaServicioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? texto, [FromQuery] bool? activo)
    {
        var categorias = await _repository.ListAsync(texto, activo);
        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _repository.GetByIdAsync(id);
        return categoria is null
            ? NotFound(new { message = "La categoría de servicio no existe." })
            : Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoriaServicioCreateDto dto)
    {
        var validation = ValidateCategoria(dto.CodigoCategoria, dto.Nombre, dto.Descripcion, dto.PrecioBase);
        if (validation is not null) return validation;

        try
        {
            var categoria = await _repository.CreateAsync(dto);
            return Ok(categoria);
        }
        catch (SqlException ex) when (ex.Number is >= 51301 and <= 51320)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoriaServicioUpdateDto dto)
    {
        if (id <= 0)
            return BadRequest(new { message = "Debe seleccionar una categoría de servicio válida." });

        var validation = ValidateCategoria(dto.CodigoCategoria, dto.Nombre, dto.Descripcion, dto.PrecioBase);
        if (validation is not null) return validation;

        try
        {
            var categoria = await _repository.UpdateAsync(id, dto);
            return Ok(categoria);
        }
        catch (SqlException ex) when (ex.Number is >= 51301 and <= 51320)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private IActionResult? ValidateCategoria(string? codigo, string? nombre, string? descripcion, decimal precio)
    {
        codigo = codigo?.Trim();
        nombre = nombre?.Trim();
        descripcion = descripcion?.Trim();

        if (string.IsNullOrWhiteSpace(codigo))
            return BadRequest(new { message = "El número o código de la categoría es obligatorio." });

        if (codigo.Length > 20)
            return BadRequest(new { message = "El número o código no debe superar 20 caracteres." });

        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest(new { message = "El nombre de la categoría es obligatorio." });

        if (nombre.Length < 2 || nombre.Length > 100)
            return BadRequest(new { message = "El nombre debe tener entre 2 y 100 caracteres." });

        if (precio <= 0)
            return BadRequest(new { message = "El precio debe ser mayor a cero." });

        if (descripcion is { Length: > 300 })
            return BadRequest(new { message = "La descripción no debe superar 300 caracteres." });

        return null;
    }
}
