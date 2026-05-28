using ClinicaDental.Api.Controllers;
using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;
using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ClinicaDental.Api.UnitTests;

public sealed class StockControllerTests
{
    [Fact]
    public async Task List_WhenEstadoIsInvalid_ReturnsBadRequest()
    {
        var controller = new StockController(new FakeStockRepository());

        var result = await controller.List(texto: null, estado: "estado_inexistente");

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_WhenPerecederoDoesNotHaveExpirationDate_ReturnsBadRequest()
    {
        var repository = new FakeStockRepository();
        var controller = new StockController(repository);
        var dto = NewValidProducto();
        dto.EsPerecedero = true;
        dto.FechaVencimiento = null;

        var result = await controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.False(repository.CreateWasCalled);
    }

    [Fact]
    public async Task Create_WhenDataIsValid_ReturnsCreatedAtAction()
    {
        var repository = new FakeStockRepository();
        var controller = new StockController(repository);

        var result = await controller.Create(NewValidProducto());

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var producto = Assert.IsType<StockProducto>(created.Value);
        Assert.Equal(25, producto.IdProducto);
        Assert.True(repository.CreateWasCalled);
    }

    private static ProductoCreateDto NewValidProducto() => new()
    {
        CodigoProducto = "PROD-001",
        Nombre = "Guantes dentales",
        IdProveedor = 1,
        EsPerecedero = false,
        Descripcion = "Caja de guantes",
        StockMinimo = 10,
        UnidadMedida = "caja",
        CostoUnitario = 45,
        PrecioVenta = 60
    };

    private sealed class FakeStockRepository : IStockRepository
    {
        public bool CreateWasCalled { get; private set; }

        public Task<IEnumerable<StockProducto>> ListAsync(string? texto, string? estado, bool soloBajoStock, bool soloAgotados, bool soloVencidos, bool soloProximosVencer)
            => Task.FromResult<IEnumerable<StockProducto>>(Array.Empty<StockProducto>());

        public Task<StockProducto?> GetByIdAsync(int id)
            => Task.FromResult<StockProducto?>(NewProducto(id));

        public Task<StockProducto> CreateAsync(ProductoCreateDto dto)
        {
            CreateWasCalled = true;
            return Task.FromResult(NewProducto(25, dto));
        }

        public Task<StockProducto> UpdateAsync(int idProducto, ProductoUpdateDto dto, int idUsuario)
            => Task.FromResult(NewProducto(idProducto));

        public Task<MovimientoInventarioDto> RegistrarCompraAsync(CompraInventarioCreateDto dto, int idUsuario)
            => Task.FromResult(NewMovimiento());

        public Task<MovimientoInventarioDto> RegistrarMovimientoAsync(MovimientoInventarioCreateDto dto, int idUsuario)
            => Task.FromResult(NewMovimiento());

        public Task<IEnumerable<MovimientoInventarioDto>> ListMovimientosAsync(string? texto, int? idProducto, DateTime? fechaDesde, DateTime? fechaHasta)
            => Task.FromResult<IEnumerable<MovimientoInventarioDto>>(Array.Empty<MovimientoInventarioDto>());

        public Task<IEnumerable<MovimientoInventarioDto>> ListHistorialMovimientosAsync(string? texto, int? idProducto, string? tipoMovimiento, DateTime? fechaDesde, DateTime? fechaHasta)
            => Task.FromResult<IEnumerable<MovimientoInventarioDto>>(Array.Empty<MovimientoInventarioDto>());

        public Task<StockResumenDto> GetResumenAsync()
            => Task.FromResult(new StockResumenDto());

        private static StockProducto NewProducto(int id, ProductoCreateDto? dto = null)
        {
            dto ??= NewValidProducto();
            return new StockProducto
            {
                IdProducto = id,
                CodigoProducto = dto.CodigoProducto ?? "PROD-001",
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                IdProveedor = dto.IdProveedor,
                Proveedor = "Proveedor pruebas",
                CantidadDisponible = 0,
                StockMinimo = dto.StockMinimo,
                UnidadMedida = dto.UnidadMedida,
                FechaVencimiento = dto.FechaVencimiento,
                CostoUnitario = dto.CostoUnitario,
                PrecioVenta = dto.PrecioVenta,
                EstadoProducto = "Disponible",
                EstadoClave = "disponible",
                Activo = true
            };
        }

        private static MovimientoInventarioDto NewMovimiento() => new()
        {
            IdMovimientoInventario = 1,
            IdProducto = 25,
            CodigoProducto = "PROD-001",
            Producto = "Guantes dentales",
            TipoMovimiento = "Entrada",
            Cantidad = 1,
            Usuario = "Admin Pruebas",
            FechaMovimiento = DateTime.UtcNow
        };
    }
}
