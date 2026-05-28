using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Infrastructure;
using ClinicaDental.Api.Models;
using Dapper;

namespace ClinicaDental.Api.Repositories;

public sealed class CatalogoRepository : ICatalogoRepository
{
    private readonly SqlConnectionFactory _factory;

    public CatalogoRepository(SqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<EstadoCita>> GetEstadosCitaAsync()
    {
        const string sql = @"
SELECT id_estado_cita AS IdEstadoCita, nombre AS Nombre
FROM dbo.EstadoCita
ORDER BY nombre;";

        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<EstadoCita>(sql);
    }

    public async Task<IEnumerable<Rol>> GetRolesAsync()
    {
        const string sql = @"
SELECT id_rol AS IdRol, nombre AS Nombre, descripcion AS Descripcion, activo AS Activo
FROM dbo.Rol
WHERE activo = 1
ORDER BY nombre;";

        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<Rol>(sql);
    }

    public async Task<IEnumerable<ProveedorCatalogoDto>> GetProveedoresAsync()
    {
        const string sql = @"
SELECT
    id_proveedor AS IdProveedor,
    nombre AS Nombre,
    nit AS Nit,
    activo AS Activo
FROM dbo.Proveedor
WHERE activo = 1
ORDER BY nombre;";

        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<ProveedorCatalogoDto>(sql);
    }

    public async Task<IEnumerable<TipoMovimientoCatalogoDto>> GetTiposMovimientoInventarioAsync()
    {
        const string sql = @"
SELECT id_tipo_movimiento AS IdTipoMovimiento, nombre AS Nombre
FROM dbo.TipoMovimientoInventario
WHERE nombre IN (N'Entrada', N'Salida', N'AjustePositivo', N'AjusteNegativo')
ORDER BY CASE nombre
    WHEN N'Entrada' THEN 1
    WHEN N'Salida' THEN 2
    WHEN N'AjustePositivo' THEN 3
    WHEN N'AjusteNegativo' THEN 4
    ELSE 5
END;";

        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<TipoMovimientoCatalogoDto>(sql);
    }

    public async Task<IEnumerable<CategoriaServicioCatalogoDto>> GetCategoriasServicioAsync()
    {
        const string sql = @"
SELECT
    id_categoria_servicio AS IdCategoriaServicio,
    codigo_categoria AS CodigoCategoria,
    nombre AS Nombre,
    precio_base AS PrecioBase
FROM dbo.CategoriaServicio
WHERE activo = 1
ORDER BY nombre;";

        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<CategoriaServicioCatalogoDto>(sql);
    }

    public async Task<IEnumerable<EstadoVentaCatalogoDto>> GetEstadosVentaAsync()
    {
        const string sql = @"
SELECT id_estado_venta AS IdEstadoVenta, nombre AS Nombre
FROM dbo.EstadoVenta
ORDER BY CASE nombre
    WHEN N'Registrada' THEN 1
    WHEN N'Pagada' THEN 2
    WHEN N'Anulada' THEN 3
    ELSE 4
END;";

        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<EstadoVentaCatalogoDto>(sql);
    }

    public async Task<IEnumerable<MetodoPagoCatalogoDto>> GetMetodosPagoAsync()
    {
        const string sql = @"
SELECT id_metodo_pago AS IdMetodoPago, nombre AS Nombre
FROM dbo.MetodoPago
ORDER BY CASE nombre
    WHEN N'Efectivo' THEN 1
    WHEN N'Tarjeta' THEN 2
    WHEN N'Transferencia' THEN 3
    WHEN N'Mixto' THEN 4
    ELSE 5
END;";

        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<MetodoPagoCatalogoDto>(sql);
    }

    public async Task<IEnumerable<UsuarioCatalogoDto>> GetUsuariosActivosAsync()
    {
        const string sql = @"
SELECT
    id_usuario AS IdUsuario,
    codigo_usuario AS CodigoUsuario,
    nombres AS Nombres,
    apellidos AS Apellidos
FROM dbo.Usuario
WHERE activo = 1
ORDER BY nombres, apellidos;";

        using var connection = _factory.CreateConnection();
        return await connection.QueryAsync<UsuarioCatalogoDto>(sql);
    }
}
