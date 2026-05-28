using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Infrastructure;
using ClinicaDental.Api.Models;
using Dapper;
using System.Data;

namespace ClinicaDental.Api.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly SqlConnectionFactory _factory;

    public UsuarioRepository(SqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<Usuario> CreateAsync(UsuarioCreateDto dto, string passwordHash, string salt)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleAsync<Usuario>(
            "sp_Usuario_Crear",
            new
            {
                codigo_usuario = dto.CodigoUsuario,
                nombres = dto.Nombres,
                apellidos = dto.Apellidos,
                username = dto.Username,
                correo = dto.Correo,
                telefono = dto.Telefono,
                password_hash = passwordHash,
                password_salt = salt,
                id_rol = dto.IdRol
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Usuario?> DeactivateAsync(int id)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Usuario>(
            "sp_Usuario_Desactivar",
            new { id_usuario = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Usuario>(
            "sp_Usuario_ObtenerPorId",
            new { id_usuario = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Usuario?> GetByUsernameOrCodeAsync(string usuario)
    {
        const string sql = @"
SELECT TOP 1
    u.id_usuario AS IdUsuario,
    u.codigo_usuario AS CodigoUsuario,
    u.nombres AS Nombres,
    u.apellidos AS Apellidos,
    u.username AS Username,
    u.correo AS Correo,
    u.telefono AS Telefono,
    u.password_hash AS PasswordHash,
    u.password_salt AS PasswordSalt,
    u.id_rol AS IdRol,
    r.nombre AS Rol,
    u.activo AS Activo,
    u.creado_en AS CreadoEn,
    u.actualizado_en AS ActualizadoEn
FROM dbo.Usuario u
INNER JOIN dbo.Rol r ON r.id_rol = u.id_rol
WHERE u.username = @usuario OR u.codigo_usuario = @usuario;";

        using var connection = _factory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { usuario });
    }

    public async Task<IEnumerable<Usuario>> ListAsync(string? texto, bool? activo)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<Usuario>(
            "sp_Usuario_Listar",
            new { texto, activo },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<DentistaListItemDto>> ListDentistasAsync()
    {
        const string sql = @"
SELECT
    u.id_usuario AS IdUsuario,
    u.nombres AS Nombres,
    u.apellidos AS Apellidos
FROM dbo.Usuario u
INNER JOIN dbo.Rol r ON r.id_rol = u.id_rol
WHERE u.activo = 1
  AND r.nombre = @rol
ORDER BY u.nombres, u.apellidos;";

        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<DentistaListItemDto>(
            sql,
            new { rol = "Dentista" });
    }

    public async Task<Usuario?> UpdateAsync(int id, UsuarioUpdateDto dto)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Usuario>(
            "sp_Usuario_Actualizar",
            new
            {
                id_usuario = id,
                nombres = dto.Nombres,
                apellidos = dto.Apellidos,
                username = dto.Username,
                correo = dto.Correo,
                telefono = dto.Telefono,
                id_rol = dto.IdRol,
                activo = dto.Activo
            },
            commandType: CommandType.StoredProcedure);
    }
}