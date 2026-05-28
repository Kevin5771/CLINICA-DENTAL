using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Infrastructure;
using ClinicaDental.Api.Models;
using Dapper;
using System.Data;

namespace ClinicaDental.Api.Repositories;

public sealed class StockRepository : IStockRepository
{
    private readonly SqlConnectionFactory _factory;

    public StockRepository(SqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<StockProducto>> ListAsync(
        string? texto,
        string? estado,
        bool soloBajoStock,
        bool soloAgotados,
        bool soloVencidos,
        bool soloProximosVencer)
    {
        using var connection = _factory.CreateConnection();

        var sql = @"
WITH StockCalculado AS
(
    SELECT
        p.id_producto AS IdProducto,
        p.codigo_producto AS CodigoProducto,
        p.nombre AS Nombre,
        p.descripcion AS Descripcion,
        p.id_proveedor AS IdProveedor,
        pr.nombre AS Proveedor,
        CASE WHEN p.stock_actual < 0 THEN 0 ELSE p.stock_actual END AS CantidadDisponible,
        p.stock_minimo AS StockMinimo,
        p.unidad_medida AS UnidadMedida,
        p.fecha_vencimiento AS FechaVencimiento,
        p.costo_unitario AS CostoUnitario,
        p.precio_venta AS PrecioVenta,
        p.activo AS Activo,
        COALESCE(p.actualizado_en, p.creado_en) AS UltimaActualizacion,
        CASE
            WHEN p.fecha_vencimiento IS NOT NULL 
                 AND p.fecha_vencimiento < CAST(GETDATE() AS DATE) 
                THEN N'Vencido'

            WHEN p.stock_actual <= 0 
                THEN N'Agotado'

            WHEN p.stock_actual <= p.stock_minimo 
                THEN N'Bajo stock'

            WHEN p.fecha_vencimiento IS NOT NULL 
                 AND p.fecha_vencimiento BETWEEN CAST(GETDATE() AS DATE) 
                 AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE)) 
                THEN N'Próximo a vencer'

            ELSE N'Disponible'
        END AS EstadoProducto,
        CASE
            WHEN p.fecha_vencimiento IS NOT NULL 
                 AND p.fecha_vencimiento < CAST(GETDATE() AS DATE) 
                THEN N'vencido'

            WHEN p.stock_actual <= 0 
                THEN N'agotado'

            WHEN p.stock_actual <= p.stock_minimo 
                THEN N'bajo_stock'

            WHEN p.fecha_vencimiento IS NOT NULL 
                 AND p.fecha_vencimiento BETWEEN CAST(GETDATE() AS DATE) 
                 AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE)) 
                THEN N'proximo_vencer'

            ELSE N'disponible'
        END AS EstadoClave
    FROM dbo.Producto p
    LEFT JOIN dbo.Proveedor pr 
        ON pr.id_proveedor = p.id_proveedor
    WHERE p.activo = 1
)
SELECT
    IdProducto,
    CodigoProducto,
    Nombre,
    Descripcion,
    IdProveedor,
    Proveedor,
    CantidadDisponible,
    StockMinimo,
    UnidadMedida,
    FechaVencimiento,
    CostoUnitario,
    PrecioVenta,
    EstadoProducto,
    EstadoClave,
    UltimaActualizacion,
    Activo
FROM StockCalculado
WHERE
    (
        @texto IS NULL 
        OR CodigoProducto LIKE '%' + @texto + '%'
        OR Nombre LIKE '%' + @texto + '%'
        OR ISNULL(Proveedor, '') LIKE '%' + @texto + '%'
        OR EstadoProducto LIKE '%' + @texto + '%'
    )
    AND (@estado IS NULL OR EstadoClave = @estado)
    AND (@soloBajoStock = 0 OR EstadoClave = N'bajo_stock')
    AND (@soloAgotados = 0 OR EstadoClave = N'agotado')
    AND (@soloVencidos = 0 OR EstadoClave = N'vencido')
    AND (@soloProximosVencer = 0 OR EstadoClave = N'proximo_vencer')
ORDER BY
    CASE EstadoClave
        WHEN N'vencido' THEN 1
        WHEN N'agotado' THEN 2
        WHEN N'bajo_stock' THEN 3
        WHEN N'proximo_vencer' THEN 4
        ELSE 5
    END,
    Nombre;";

        return await connection.QueryAsync<StockProducto>(
            sql,
            new
            {
                texto = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim(),
                estado = string.IsNullOrWhiteSpace(estado) ? null : estado.Trim().ToLowerInvariant(),
                soloBajoStock,
                soloAgotados,
                soloVencidos,
                soloProximosVencer
            });
    }

    public async Task<StockProducto?> GetByIdAsync(int id)
    {
        using var connection = _factory.CreateConnection();

        const string sql = @"
WITH StockCalculado AS
(
    SELECT
        p.id_producto AS IdProducto,
        p.codigo_producto AS CodigoProducto,
        p.nombre AS Nombre,
        p.descripcion AS Descripcion,
        p.id_proveedor AS IdProveedor,
        pr.nombre AS Proveedor,
        CASE WHEN p.stock_actual < 0 THEN 0 ELSE p.stock_actual END AS CantidadDisponible,
        p.stock_minimo AS StockMinimo,
        p.unidad_medida AS UnidadMedida,
        p.fecha_vencimiento AS FechaVencimiento,
        p.costo_unitario AS CostoUnitario,
        p.precio_venta AS PrecioVenta,
        p.activo AS Activo,
        COALESCE(p.actualizado_en, p.creado_en) AS UltimaActualizacion,
        CASE
            WHEN p.fecha_vencimiento IS NOT NULL AND p.fecha_vencimiento < CAST(GETDATE() AS DATE) THEN N'Vencido'
            WHEN p.stock_actual <= 0 THEN N'Agotado'
            WHEN p.stock_actual <= p.stock_minimo THEN N'Bajo stock'
            WHEN p.fecha_vencimiento IS NOT NULL AND p.fecha_vencimiento BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE)) THEN N'Próximo a vencer'
            ELSE N'Disponible'
        END AS EstadoProducto,
        CASE
            WHEN p.fecha_vencimiento IS NOT NULL AND p.fecha_vencimiento < CAST(GETDATE() AS DATE) THEN N'vencido'
            WHEN p.stock_actual <= 0 THEN N'agotado'
            WHEN p.stock_actual <= p.stock_minimo THEN N'bajo_stock'
            WHEN p.fecha_vencimiento IS NOT NULL AND p.fecha_vencimiento BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE)) THEN N'proximo_vencer'
            ELSE N'disponible'
        END AS EstadoClave
    FROM dbo.Producto p
    LEFT JOIN dbo.Proveedor pr ON pr.id_proveedor = p.id_proveedor
    WHERE p.id_producto = @idProducto
)
SELECT * FROM StockCalculado;";

        return await connection.QueryFirstOrDefaultAsync<StockProducto>(sql, new { idProducto = id });
    }

    public async Task<StockProducto> CreateAsync(ProductoCreateDto dto)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleAsync<StockProducto>(
            "sp_Producto_Crear_RF09",
            new
            {
                codigo_producto = string.IsNullOrWhiteSpace(dto.CodigoProducto) ? null : dto.CodigoProducto.Trim(),
                nombre = dto.Nombre.Trim(),
                descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
                id_proveedor = dto.IdProveedor,
                fecha_vencimiento = dto.FechaVencimiento?.Date,
                es_perecedero = dto.EsPerecedero,
                stock_actual = 0,
                stock_minimo = dto.StockMinimo,
                unidad_medida = string.IsNullOrWhiteSpace(dto.UnidadMedida) ? "unidad" : dto.UnidadMedida.Trim(),
                costo_unitario = dto.CostoUnitario,
                precio_venta = dto.PrecioVenta
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<StockProducto> UpdateAsync(int idProducto, ProductoUpdateDto dto, int idUsuario)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleAsync<StockProducto>(
            "sp_RF12_Producto_Actualizar",
            new
            {
                id_producto = idProducto,
                codigo_producto = string.IsNullOrWhiteSpace(dto.CodigoProducto) ? null : dto.CodigoProducto.Trim(),
                nombre = dto.Nombre.Trim(),
                descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
                id_proveedor = dto.IdProveedor,
                stock_actual = dto.CantidadDisponible,
                stock_minimo = dto.StockMinimo,
                unidad_medida = string.IsNullOrWhiteSpace(dto.UnidadMedida) ? "unidad" : dto.UnidadMedida.Trim(),
                fecha_vencimiento = dto.FechaVencimiento?.Date,
                es_perecedero = dto.EsPerecedero,
                costo_unitario = dto.CostoUnitario,
                precio_venta = dto.PrecioVenta,
                activo = dto.Activo,
                realizado_por = idUsuario
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<MovimientoInventarioDto> RegistrarCompraAsync(CompraInventarioCreateDto dto, int idUsuario)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleAsync<MovimientoInventarioDto>(
            "sp_CompraInventario_Registrar",
            new
            {
                id_producto = dto.IdProducto,
                cantidad = dto.Cantidad,
                costo_unitario = dto.CostoUnitario,
                referencia = dto.Referencia.Trim(),
                observaciones = string.IsNullOrWhiteSpace(dto.Observaciones) ? null : dto.Observaciones.Trim(),
                realizado_por = idUsuario
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<MovimientoInventarioDto>> ListMovimientosAsync(
        string? texto,
        int? idProducto,
        DateTime? fechaDesde,
        DateTime? fechaHasta)
    {
        using var connection = _factory.CreateConnection();

        const string sql = @"
SELECT TOP (100)
    mi.id_movimiento_inventario AS IdMovimientoInventario,
    mi.id_producto AS IdProducto,
    p.codigo_producto AS CodigoProducto,
    p.nombre AS Producto,
    mi.id_tipo_movimiento AS IdTipoMovimiento,
    tm.nombre AS TipoMovimiento,
    mi.cantidad AS Cantidad,
    mi.costo_unitario AS CostoUnitario,
    mi.referencia AS Referencia,
    mi.observaciones AS Observaciones,
    mi.motivo AS Motivo,
    p.stock_actual AS StockDespues,
    mi.realizado_por AS RealizadoPor,
    CONCAT(u.nombres, ' ', u.apellidos) AS Usuario,
    mi.fecha_movimiento AS FechaMovimiento
FROM dbo.MovimientoInventario mi
INNER JOIN dbo.Producto p ON p.id_producto = mi.id_producto
INNER JOIN dbo.TipoMovimientoInventario tm ON tm.id_tipo_movimiento = mi.id_tipo_movimiento
INNER JOIN dbo.Usuario u ON u.id_usuario = mi.realizado_por
WHERE tm.nombre = N'Entrada'
  AND (@idProducto IS NULL OR mi.id_producto = @idProducto)
  AND (@fechaDesde IS NULL OR mi.fecha_movimiento >= @fechaDesde)
  AND (@fechaHasta IS NULL OR mi.fecha_movimiento < DATEADD(DAY, 1, @fechaHasta))
  AND (
        @texto IS NULL
        OR p.codigo_producto LIKE '%' + @texto + '%'
        OR p.nombre LIKE '%' + @texto + '%'
        OR ISNULL(mi.referencia, '') LIKE '%' + @texto + '%'
        OR ISNULL(mi.observaciones, '') LIKE '%' + @texto + '%'
      )
ORDER BY mi.fecha_movimiento DESC;";

        return await connection.QueryAsync<MovimientoInventarioDto>(
            sql,
            new
            {
                texto = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim(),
                idProducto,
                fechaDesde = fechaDesde?.Date,
                fechaHasta = fechaHasta?.Date
            });
    }


    public async Task<MovimientoInventarioDto> RegistrarMovimientoAsync(MovimientoInventarioCreateDto dto, int idUsuario)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QuerySingleAsync<MovimientoInventarioDto>(
            "sp_RF11_MovimientoInventario_Registrar",
            new
            {
                id_producto = dto.IdProducto,
                tipo_movimiento = dto.TipoMovimiento.Trim(),
                cantidad = dto.Cantidad,
                costo_unitario = dto.CostoUnitario,
                fecha_movimiento = dto.FechaMovimiento,
                motivo = dto.Motivo.Trim(),
                referencia = string.IsNullOrWhiteSpace(dto.Referencia) ? null : dto.Referencia.Trim(),
                observaciones = string.IsNullOrWhiteSpace(dto.Observaciones) ? null : dto.Observaciones.Trim(),
                realizado_por = idUsuario
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<MovimientoInventarioDto>> ListHistorialMovimientosAsync(
        string? texto,
        int? idProducto,
        string? tipoMovimiento,
        DateTime? fechaDesde,
        DateTime? fechaHasta)
    {
        using var connection = _factory.CreateConnection();

        return await connection.QueryAsync<MovimientoInventarioDto>(
            "sp_RF11_MovimientoInventario_Listar",
            new
            {
                texto = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim(),
                id_producto = idProducto,
                tipo_movimiento = string.IsNullOrWhiteSpace(tipoMovimiento) ? null : tipoMovimiento.Trim(),
                fecha_desde = fechaDesde?.Date,
                fecha_hasta = fechaHasta?.Date
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<StockResumenDto> GetResumenAsync()
    {
        using var connection = _factory.CreateConnection();

        var sql = @"
WITH StockCalculado AS
(
    SELECT
        CASE
            WHEN fecha_vencimiento IS NOT NULL 
                 AND fecha_vencimiento < CAST(GETDATE() AS DATE) 
                THEN N'vencido'

            WHEN stock_actual <= 0 
                THEN N'agotado'

            WHEN stock_actual <= stock_minimo 
                THEN N'bajo_stock'

            WHEN fecha_vencimiento IS NOT NULL 
                 AND fecha_vencimiento BETWEEN CAST(GETDATE() AS DATE) 
                 AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE)) 
                THEN N'proximo_vencer'

            ELSE N'disponible'
        END AS EstadoClave
    FROM dbo.Producto
    WHERE activo = 1
)
SELECT
    COUNT(1) AS TotalProductos,
    ISNULL(SUM(CASE WHEN EstadoClave = N'disponible' THEN 1 ELSE 0 END), 0) AS Disponibles,
    ISNULL(SUM(CASE WHEN EstadoClave = N'bajo_stock' THEN 1 ELSE 0 END), 0) AS BajoStock,
    ISNULL(SUM(CASE WHEN EstadoClave = N'agotado' THEN 1 ELSE 0 END), 0) AS Agotados,
    ISNULL(SUM(CASE WHEN EstadoClave = N'vencido' THEN 1 ELSE 0 END), 0) AS Vencidos,
    ISNULL(SUM(CASE WHEN EstadoClave = N'proximo_vencer' THEN 1 ELSE 0 END), 0) AS ProximosVencer
FROM StockCalculado;";

        return await connection.QuerySingleAsync<StockResumenDto>(sql);
    }
}
