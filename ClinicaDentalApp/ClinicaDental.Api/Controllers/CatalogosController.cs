using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaDental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CatalogosController : ControllerBase
{
    private readonly ICatalogoRepository _repository;

    public CatalogosController(ICatalogoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("roles")]
    public async Task<IActionResult> Roles() => Ok(await _repository.GetRolesAsync());

    [HttpGet("estados-cita")]
    public async Task<IActionResult> EstadosCita() => Ok(await _repository.GetEstadosCitaAsync());

    [HttpGet("proveedores")]
    public async Task<IActionResult> Proveedores() => Ok(await _repository.GetProveedoresAsync());

    [HttpGet("tipos-movimiento-inventario")]
    public async Task<IActionResult> TiposMovimientoInventario() => Ok(await _repository.GetTiposMovimientoInventarioAsync());

    [HttpGet("categorias-servicio")]
    public async Task<IActionResult> CategoriasServicio() => Ok(await _repository.GetCategoriasServicioAsync());

    [HttpGet("estados-venta")]
    public async Task<IActionResult> EstadosVenta() => Ok(await _repository.GetEstadosVentaAsync());

    [HttpGet("metodos-pago")]
    public async Task<IActionResult> MetodosPago() => Ok(await _repository.GetMetodosPagoAsync());

    [HttpGet("usuarios-activos")]
    public async Task<IActionResult> UsuariosActivos() => Ok(await _repository.GetUsuariosActivosAsync());
}
