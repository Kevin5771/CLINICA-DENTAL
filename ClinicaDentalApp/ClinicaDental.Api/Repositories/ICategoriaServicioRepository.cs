using ClinicaDental.Api.DTOs;

namespace ClinicaDental.Api.Repositories;

public interface ICategoriaServicioRepository
{
    Task<IEnumerable<CategoriaServicioDto>> ListAsync(string? texto, bool? activo);
    Task<CategoriaServicioDto?> GetByIdAsync(int idCategoriaServicio);
    Task<CategoriaServicioDto> CreateAsync(CategoriaServicioCreateDto dto);
    Task<CategoriaServicioDto> UpdateAsync(int idCategoriaServicio, CategoriaServicioUpdateDto dto);
}
