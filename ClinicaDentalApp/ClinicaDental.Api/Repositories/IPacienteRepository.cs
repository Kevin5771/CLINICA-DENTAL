using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;

namespace ClinicaDental.Api.Repositories;

public interface IPacienteRepository
{
    Task<IEnumerable<Paciente>> ListAsync(string? texto, bool? activo);
    Task<Paciente?> GetByIdAsync(int id);
    Task<Paciente> CreateAsync(PacienteCreateDto dto);
    Task<Paciente?> UpdateAsync(int id, PacienteUpdateDto dto);
    Task<Paciente?> DeactivateAsync(int id);
}
