using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Infrastructure;
using ClinicaDental.Api.Models;
using Dapper;
using System.Data;

namespace ClinicaDental.Api.Repositories;

public sealed class CitaRepository : ICitaRepository
{
    private readonly SqlConnectionFactory _factory;

    public CitaRepository(SqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<Cita?> CancelAsync(int id, CancelarCitaDto dto)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Cita>(
            "sp_Cita_Cancelar",
            new
            {
                id_cita = id,
                id_usuario_accion = dto.IdUsuarioAccion,
                comentario = dto.Comentario
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Cita> CreateAsync(CitaCreateDto dto)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QuerySingleAsync<Cita>(
            "sp_Cita_Crear",
            new
            {
                id_paciente = dto.IdPaciente,
                id_dentista = dto.IdDentista,
                fecha = dto.Fecha.Date,
                hora_inicio = dto.HoraInicio,
                hora_fin = dto.HoraFin,
                motivo = dto.Motivo,
                observaciones = dto.Observaciones,
                id_estado_cita = dto.IdEstadoCita,
                creada_por = dto.CreadaPor
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Cita?> GetByIdAsync(int id)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Cita>(
            "sp_Cita_ObtenerPorId",
            new { id_cita = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Cita>> ListAsync(DateTime? fechaDesde, DateTime? fechaHasta, int? idDentista, int? idEstadoCita, int? idPaciente)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<Cita>(
            "sp_Cita_Listar",
            new
            {
                fecha_desde = fechaDesde?.Date,
                fecha_hasta = fechaHasta?.Date,
                id_dentista = idDentista,
                id_estado_cita = idEstadoCita,
                id_paciente = idPaciente
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Cita?> UpdateAsync(int id, CitaUpdateDto dto)
    {
        using var connection = _factory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Cita>(
            "sp_Cita_Actualizar",
            new
            {
                id_cita = id,
                id_paciente = dto.IdPaciente,
                id_dentista = dto.IdDentista,
                fecha = dto.Fecha.Date,
                hora_inicio = dto.HoraInicio,
                hora_fin = dto.HoraFin,
                motivo = dto.Motivo,
                observaciones = dto.Observaciones,
                id_estado_cita = dto.IdEstadoCita,
                id_usuario_accion = dto.IdUsuarioAccion
            },
            commandType: CommandType.StoredProcedure);
    }
}
