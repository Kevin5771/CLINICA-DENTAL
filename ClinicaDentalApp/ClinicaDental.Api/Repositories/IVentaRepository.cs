using ClinicaDental.Api.DTOs;

namespace ClinicaDental.Api.Repositories;

public interface IVentaRepository
{
    Task<VentaServicioDto> RegistrarServicioAsync(VentaServicioCreateDto dto, int idUsuarioSesion);
    Task<IEnumerable<VentaServicioDto>> ListServiciosAsync(
        string? texto,
        int? idPaciente,
        int? idUsuarioResponsable,
        int? idCategoriaServicio,
        int? idEstadoVenta,
        DateTime? fechaDesde,
        DateTime? fechaHasta);
}
