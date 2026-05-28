using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;

namespace ClinicaDental.Api.Repositories;

public interface ICitaRepository
{
    Task<IEnumerable<Cita>> ListAsync(DateTime? fechaDesde, DateTime? fechaHasta, int? idDentista, int? idEstadoCita, int? idPaciente);
    Task<Cita?> GetByIdAsync(int id);
    Task<Cita> CreateAsync(CitaCreateDto dto);
    Task<Cita?> UpdateAsync(int id, CitaUpdateDto dto);
    Task<Cita?> CancelAsync(int id, CancelarCitaDto dto);
}
