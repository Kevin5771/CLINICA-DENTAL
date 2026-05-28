using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Infrastructure;
using Dapper;
using System.Data;

namespace ClinicaDental.Api.Repositories;

public sealed class VentaRepository : IVentaRepository
{
    private readonly SqlConnectionFactory _factory;

    public VentaRepository(SqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<VentaServicioDto> RegistrarServicioAsync(VentaServicioCreateDto dto, int idUsuarioSesion)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleAsync<VentaServicioDto>(
            "sp_RF14_VentaServicio_Registrar",
            new
            {
                numero_comprobante = string.IsNullOrWhiteSpace(dto.NumeroComprobante) ? null : dto.NumeroComprobante.Trim(),
                id_paciente = dto.IdPaciente,
                id_usuario_responsable = dto.IdUsuarioResponsable,
                id_categoria_servicio = dto.IdCategoriaServicio,
                precio = dto.Precio,
                fecha_venta = dto.FechaVenta,
                descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
                id_estado_venta = dto.IdEstadoVenta,
                id_metodo_pago = dto.IdMetodoPago,
                id_usuario_registra = idUsuarioSesion
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<VentaServicioDto>> ListServiciosAsync(
        string? texto,
        int? idPaciente,
        int? idUsuarioResponsable,
        int? idCategoriaServicio,
        int? idEstadoVenta,
        DateTime? fechaDesde,
        DateTime? fechaHasta)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<VentaServicioDto>(
            "sp_RF14_VentaServicio_Listar",
            new
            {
                texto = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim(),
                id_paciente = idPaciente,
                id_usuario_responsable = idUsuarioResponsable,
                id_categoria_servicio = idCategoriaServicio,
                id_estado_venta = idEstadoVenta,
                fecha_desde = fechaDesde?.Date,
                fecha_hasta = fechaHasta?.Date
            },
            commandType: CommandType.StoredProcedure);
    }
}
