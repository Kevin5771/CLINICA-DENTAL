using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Infrastructure;
using Dapper;
using System.Data;

namespace ClinicaDental.Api.Repositories;

public sealed class ReporteRepository : IReporteRepository
{
    private readonly SqlConnectionFactory _factory;

    public ReporteRepository(SqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<ReporteCitaDto>> GetCitasAsync(
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        int? idPaciente,
        int? idDentista,
        int? idEstadoCita)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<ReporteCitaDto>(
            "sp_RF15_ReporteCitas",
            new
            {
                fecha_desde = fechaDesde?.Date,
                fecha_hasta = fechaHasta?.Date,
                id_paciente = idPaciente,
                id_dentista = idDentista,
                id_estado_cita = idEstadoCita
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ReportePacienteDto>> GetPacientesAsync(
        string? texto,
        bool? activo)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<ReportePacienteDto>(
            "sp_RF15_ReportePacientes",
            new
            {
                texto = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim(),
                activo
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ReporteInventarioDto>> GetInventarioAsync(
        string? texto,
        string? estado,
        int? idProveedor)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<ReporteInventarioDto>(
            "sp_RF15_ReporteInventario",
            new
            {
                texto = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim(),
                estado = string.IsNullOrWhiteSpace(estado) ? null : estado.Trim(),
                id_proveedor = idProveedor
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ReporteVentaDto>> GetVentasAsync(
        string? texto,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        int? idPaciente,
        int? idUsuarioResponsable,
        int? idCategoriaServicio,
        int? idEstadoVenta)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<ReporteVentaDto>(
            "sp_RF15_ReporteVentas",
            new
            {
                texto = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim(),
                fecha_desde = fechaDesde?.Date,
                fecha_hasta = fechaHasta?.Date,
                id_paciente = idPaciente,
                id_usuario_responsable = idUsuarioResponsable,
                id_categoria_servicio = idCategoriaServicio,
                id_estado_venta = idEstadoVenta
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ReporteResumenDto> GetResumenAsync(
        DateTime? fechaDesde,
        DateTime? fechaHasta)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleAsync<ReporteResumenDto>(
            "sp_RF15_ReporteResumen",
            new
            {
                fecha_desde = fechaDesde?.Date,
                fecha_hasta = fechaHasta?.Date
            },
            commandType: CommandType.StoredProcedure);
    }
}