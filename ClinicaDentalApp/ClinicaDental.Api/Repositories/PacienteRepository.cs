using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Infrastructure;
using ClinicaDental.Api.Models;
using Dapper;
using System.Data;

namespace ClinicaDental.Api.Repositories;

public sealed class PacienteRepository : IPacienteRepository
{
    private readonly SqlConnectionFactory _factory;

    public PacienteRepository(SqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<Paciente> CreateAsync(PacienteCreateDto dto)
    {
        using var connection = _factory.CreateConnection();

        var result = await connection.QuerySingleAsync<Paciente>(
            "sp_Paciente_Crear",
            new
            {
                codigo_paciente = dto.CodigoPaciente,
                nombres = dto.Nombres,
                apellidos = dto.Apellidos,
                telefono = dto.Telefono,
                fecha_nacimiento = dto.FechaNacimiento.Date,
                genero = dto.Genero,
                direccion = dto.Direccion,
                correo = dto.Correo,
                alergias = dto.Alergias,
                observaciones_generales = dto.ObservacionesGenerales
            },
            commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<Paciente?> DeactivateAsync(int id)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Paciente>(
            "sp_Paciente_Desactivar",
            new { id_paciente = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Paciente?> GetByIdAsync(int id)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Paciente>(
            "sp_Paciente_ObtenerPorId",
            new { id_paciente = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Paciente>> ListAsync(string? texto, bool? activo)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<Paciente>(
            "sp_Paciente_Listar",
            new { texto, activo },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Paciente?> UpdateAsync(int id, PacienteUpdateDto dto)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Paciente>(
            "sp_Paciente_Actualizar",
            new
            {
                id_paciente = id,
                nombres = dto.Nombres,
                apellidos = dto.Apellidos,
                telefono = dto.Telefono,
                fecha_nacimiento = dto.FechaNacimiento.Date,
                genero = dto.Genero,
                direccion = dto.Direccion,
                correo = dto.Correo,
                alergias = dto.Alergias,
                observaciones_generales = dto.ObservacionesGenerales,
                activo = dto.Activo
            },
            commandType: CommandType.StoredProcedure);
    }
}