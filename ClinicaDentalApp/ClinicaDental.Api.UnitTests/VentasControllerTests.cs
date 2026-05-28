using System.Security.Claims;
using ClinicaDental.Api.Controllers;
using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ClinicaDental.Api.UnitTests;

public sealed class VentasControllerTests
{
    [Fact]
    public async Task RegistrarServicio_WhenAuthenticatedUserCannotBeIdentified_ReturnsUnauthorized()
    {
        var repository = new FakeVentaRepository();
        var controller = new VentasController(repository)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var result = await controller.RegistrarServicio(NewValidVenta());

        Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.False(repository.RegistrarWasCalled);
    }

    [Fact]
    public async Task RegistrarServicio_WhenPriceIsZero_ReturnsBadRequest()
    {
        var repository = new FakeVentaRepository();
        var controller = NewControllerWithUser(repository, userId: 1);
        var dto = NewValidVenta();
        dto.Precio = 0;

        var result = await controller.RegistrarServicio(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.False(repository.RegistrarWasCalled);
    }

    [Fact]
    public async Task RegistrarServicio_WhenDataIsValid_ReturnsOk()
    {
        var repository = new FakeVentaRepository();
        var controller = NewControllerWithUser(repository, userId: 1);

        var result = await controller.RegistrarServicio(NewValidVenta());

        var ok = Assert.IsType<OkObjectResult>(result);
        var venta = Assert.IsType<VentaServicioDto>(ok.Value);
        Assert.Equal(55, venta.IdVenta);
        Assert.True(repository.RegistrarWasCalled);
    }

    private static VentasController NewControllerWithUser(FakeVentaRepository repository, int userId)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, authenticationType: "Test");

        return new VentasController(repository)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            }
        };
    }

    private static VentaServicioCreateDto NewValidVenta() => new()
    {
        IdPaciente = 1,
        IdUsuarioResponsable = 1,
        IdCategoriaServicio = 1,
        Precio = 250,
        Descripcion = "Limpieza dental",
        IdEstadoVenta = 1,
        IdMetodoPago = 1
    };

    private sealed class FakeVentaRepository : IVentaRepository
    {
        public bool RegistrarWasCalled { get; private set; }

        public Task<VentaServicioDto> RegistrarServicioAsync(VentaServicioCreateDto dto, int idUsuarioSesion)
        {
            RegistrarWasCalled = true;
            return Task.FromResult(new VentaServicioDto
            {
                IdVenta = 55,
                NumeroVenta = "V-00055",
                IdPaciente = dto.IdPaciente,
                Paciente = "Juan Perez",
                IdUsuario = dto.IdUsuarioResponsable,
                UsuarioResponsable = "Dra. Ana López",
                IdCategoriaServicio = dto.IdCategoriaServicio,
                CategoriaServicio = "Limpieza",
                FechaVenta = DateTime.UtcNow,
                Precio = dto.Precio,
                Total = dto.Precio,
                IdEstadoVenta = dto.IdEstadoVenta ?? 1,
                EstadoVenta = "Pagada",
                IdMetodoPago = dto.IdMetodoPago ?? 1,
                MetodoPago = "Efectivo",
                Descripcion = dto.Descripcion,
                CreadoEn = DateTime.UtcNow
            });
        }

        public Task<IEnumerable<VentaServicioDto>> ListServiciosAsync(string? texto, int? idPaciente, int? idUsuarioResponsable, int? idCategoriaServicio, int? idEstadoVenta, DateTime? fechaDesde, DateTime? fechaHasta)
            => Task.FromResult<IEnumerable<VentaServicioDto>>(Array.Empty<VentaServicioDto>());
    }
}
