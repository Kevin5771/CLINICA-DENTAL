using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;

namespace ClinicaDental.Api.Repositories;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> ListAsync(string? texto, bool? activo);
    Task<IEnumerable<DentistaListItemDto>> ListDentistasAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario> CreateAsync(UsuarioCreateDto dto, string passwordHash, string salt);
    Task<Usuario?> UpdateAsync(int id, UsuarioUpdateDto dto);
    Task<Usuario?> DeactivateAsync(int id);
    Task<Usuario?> GetByUsernameOrCodeAsync(string usuario);
}