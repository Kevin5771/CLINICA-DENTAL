using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;

namespace ClinicaDental.Api.Repositories;

public interface IStockRepository
{
    Task<IEnumerable<StockProducto>> ListAsync(
        string? texto,
        string? estado,
        bool soloBajoStock,
        bool soloAgotados,
        bool soloVencidos,
        bool soloProximosVencer);

    Task<StockProducto?> GetByIdAsync(int id);
    Task<StockProducto> CreateAsync(ProductoCreateDto dto);
    Task<StockProducto> UpdateAsync(int idProducto, ProductoUpdateDto dto, int idUsuario);
    Task<MovimientoInventarioDto> RegistrarCompraAsync(CompraInventarioCreateDto dto, int idUsuario);
    Task<MovimientoInventarioDto> RegistrarMovimientoAsync(MovimientoInventarioCreateDto dto, int idUsuario);
    Task<IEnumerable<MovimientoInventarioDto>> ListMovimientosAsync(string? texto, int? idProducto, DateTime? fechaDesde, DateTime? fechaHasta);
    Task<IEnumerable<MovimientoInventarioDto>> ListHistorialMovimientosAsync(string? texto, int? idProducto, string? tipoMovimiento, DateTime? fechaDesde, DateTime? fechaHasta);
    Task<StockResumenDto> GetResumenAsync();
}
