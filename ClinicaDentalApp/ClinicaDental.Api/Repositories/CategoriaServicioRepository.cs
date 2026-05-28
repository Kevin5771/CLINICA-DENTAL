using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Infrastructure;
using Dapper;
using System.Data;

namespace ClinicaDental.Api.Repositories;

public sealed class CategoriaServicioRepository : ICategoriaServicioRepository
{
    private readonly SqlConnectionFactory _factory;

    public CategoriaServicioRepository(SqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<CategoriaServicioDto>> ListAsync(string? texto, bool? activo)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<CategoriaServicioDto>(
            "sp_RF13_CategoriaServicio_Listar",
            new
            {
                texto = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim(),
                activo
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CategoriaServicioDto?> GetByIdAsync(int idCategoriaServicio)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<CategoriaServicioDto>(
            "sp_RF13_CategoriaServicio_ObtenerPorId",
            new { id_categoria_servicio = idCategoriaServicio },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CategoriaServicioDto> CreateAsync(CategoriaServicioCreateDto dto)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleAsync<CategoriaServicioDto>(
            "sp_RF13_CategoriaServicio_Crear",
            new
            {
                codigo_categoria = dto.CodigoCategoria.Trim().ToUpperInvariant(),
                nombre = dto.Nombre.Trim(),
                descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
                precio_base = dto.PrecioBase,
                activo = dto.Activo
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<CategoriaServicioDto> UpdateAsync(int idCategoriaServicio, CategoriaServicioUpdateDto dto)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleAsync<CategoriaServicioDto>(
            "sp_RF13_CategoriaServicio_Actualizar",
            new
            {
                id_categoria_servicio = idCategoriaServicio,
                codigo_categoria = dto.CodigoCategoria.Trim().ToUpperInvariant(),
                nombre = dto.Nombre.Trim(),
                descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
                precio_base = dto.PrecioBase,
                activo = dto.Activo
            },
            commandType: CommandType.StoredProcedure);
    }
}
