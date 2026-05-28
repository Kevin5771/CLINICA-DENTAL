using ClinicaDental.Api.DTOs;

namespace ClinicaDental.Api.Repositories;

public interface IReporteRepository
{
    Task<IEnumerable<ReporteCitaDto>> GetCitasAsync(
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        int? idPaciente,
        int? idDentista,
        int? idEstadoCita);

    Task<IEnumerable<ReportePacienteDto>> GetPacientesAsync(
        string? texto,
        bool? activo);

    Task<IEnumerable<ReporteInventarioDto>> GetInventarioAsync(
        string? texto,
        string? estado,
        int? idProveedor);

    Task<IEnumerable<ReporteVentaDto>> GetVentasAsync(
        string? texto,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        int? idPaciente,
        int? idUsuarioResponsable,
        int? idCategoriaServicio,
        int? idEstadoVenta);

    Task<ReporteResumenDto> GetResumenAsync(
        DateTime? fechaDesde,
        DateTime? fechaHasta);
}