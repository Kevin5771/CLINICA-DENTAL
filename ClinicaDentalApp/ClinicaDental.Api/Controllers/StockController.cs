using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace ClinicaDental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class StockController : ControllerBase
{
    private static readonly HashSet<string> EstadosPermitidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "disponible",
        "bajo_stock",
        "agotado",
        "vencido",
        "proximo_vencer"
    };

    private static readonly HashSet<string> TiposMovimientoPermitidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "Entrada",
        "Salida",
        "AjustePositivo",
        "AjusteNegativo"
    };

    private readonly IStockRepository _repository;

    public StockController(IStockRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? texto,
        [FromQuery] string? estado,
        [FromQuery] bool soloBajoStock = false,
        [FromQuery] bool soloAgotados = false,
        [FromQuery] bool soloVencidos = false,
        [FromQuery] bool soloProximosVencer = false)
    {
        if (!string.IsNullOrWhiteSpace(estado) && !EstadosPermitidos.Contains(estado))
        {
            return BadRequest(new { message = "El estado solicitado no es válido." });
        }

        var items = await _repository.ListAsync(
            texto,
            estado,
            soloBajoStock,
            soloAgotados,
            soloVencidos,
            soloProximosVencer);

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? NotFound(new { message = "Producto no encontrado." }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductoCreateDto dto)
    {
        var validation = ValidateCreate(dto);
        if (validation is not null) return validation;

        try
        {
            var created = await _repository.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.IdProducto }, created);
        }
        catch (SqlException ex) when (ex.Number is >= 50401 and <= 50420)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductoUpdateDto dto)
    {
        var idUsuario = GetCurrentUserId();
        if (idUsuario <= 0)
        {
            return Unauthorized(new { message = "No se pudo identificar el usuario autenticado." });
        }

        var validation = ValidateUpdate(dto);
        if (validation is not null) return validation;

        try
        {
            var updated = await _repository.UpdateAsync(id, dto, idUsuario);
            return Ok(updated);
        }
        catch (SqlException ex) when (ex.Number is >= 50801 and <= 50830)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("compras")]
    public async Task<IActionResult> RegistrarCompra([FromBody] CompraInventarioCreateDto dto)
    {
        var idUsuario = GetCurrentUserId();
        if (idUsuario <= 0)
        {
            return Unauthorized(new { message = "No se pudo identificar el usuario autenticado." });
        }

        var validation = ValidateCompra(dto);
        if (validation is not null) return validation;

        try
        {
            var created = await _repository.RegistrarCompraAsync(dto, idUsuario);
            return Ok(created);
        }
        catch (SqlException ex) when (ex.Number is >= 50601 and <= 50620)
        {
            return BadRequest(new { message = ex.Message });
        }
    }



    [HttpPost("movimientos")]
    public async Task<IActionResult> RegistrarMovimiento([FromBody] MovimientoInventarioCreateDto dto)
    {
        var idUsuario = GetCurrentUserId();
        if (idUsuario <= 0)
        {
            return Unauthorized(new { message = "No se pudo identificar el usuario autenticado." });
        }

        var validation = ValidateMovimiento(dto);
        if (validation is not null) return validation;

        try
        {
            var created = await _repository.RegistrarMovimientoAsync(dto, idUsuario);
            return Ok(created);
        }
        catch (SqlException ex) when (ex.Number is >= 50701 and <= 50730)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (SqlException ex) when (ex.Number is 50000)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("historial-movimientos")]
    public async Task<IActionResult> HistorialMovimientos(
        [FromQuery] string? texto,
        [FromQuery] int? idProducto,
        [FromQuery] string? tipoMovimiento,
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta)
    {
        if (!string.IsNullOrWhiteSpace(tipoMovimiento) && !TiposMovimientoPermitidos.Contains(tipoMovimiento))
        {
            return BadRequest(new { message = "El tipo de movimiento solicitado no es válido." });
        }

        var items = await _repository.ListHistorialMovimientosAsync(texto, idProducto, tipoMovimiento, fechaDesde, fechaHasta);
        return Ok(items);
    }

    [HttpGet("movimientos")]
    public async Task<IActionResult> Movimientos(
        [FromQuery] string? texto,
        [FromQuery] int? idProducto,
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta)
    {
        var items = await _repository.ListMovimientosAsync(texto, idProducto, fechaDesde, fechaHasta);
        return Ok(items);
    }

    [HttpGet("resumen")]
    public async Task<IActionResult> Resumen()
    {
        var resumen = await _repository.GetResumenAsync();
        return Ok(resumen);
    }

    private int GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        return int.TryParse(value, out var idUsuario) ? idUsuario : 0;
    }

    private IActionResult? ValidateCreate(ProductoCreateDto dto)
    {
        dto.Nombre = dto.Nombre?.Trim() ?? string.Empty;
        dto.CodigoProducto = string.IsNullOrWhiteSpace(dto.CodigoProducto) ? null : dto.CodigoProducto.Trim();
        dto.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();
        dto.UnidadMedida = string.IsNullOrWhiteSpace(dto.UnidadMedida) ? "unidad" : dto.UnidadMedida.Trim();

        if (dto.Nombre.Length < 2 || dto.Nombre.Length > 100)
            return BadRequest(new { message = "El nombre del producto debe tener entre 2 y 100 caracteres." });

        if (dto.IdProveedor <= 0)
            return BadRequest(new { message = "Debe seleccionar un proveedor válido." });

        if (dto.StockMinimo < 0)
            return BadRequest(new { message = "El stock mínimo no puede ser negativo." });

        if (dto.CostoUnitario < 0)
            return BadRequest(new { message = "El costo unitario no puede ser negativo." });

        if (dto.PrecioVenta.HasValue && dto.PrecioVenta.Value < 0)
            return BadRequest(new { message = "El precio de venta no puede ser negativo." });

        if (dto.EsPerecedero && dto.FechaVencimiento is null)
            return BadRequest(new { message = "La fecha de vencimiento es obligatoria para productos perecederos." });

        if (dto.FechaVencimiento.HasValue && dto.FechaVencimiento.Value.Date < new DateTime(1900, 1, 1))
            return BadRequest(new { message = "La fecha de vencimiento no es válida." });

        if (dto.CodigoProducto is { Length: > 20 })
            return BadRequest(new { message = "El código del producto no debe superar 20 caracteres." });

        if (dto.Descripcion is { Length: > 300 })
            return BadRequest(new { message = "La descripción no debe superar 300 caracteres." });

        return null;
    }


    private IActionResult? ValidateUpdate(ProductoUpdateDto dto)
    {
        dto.Nombre = dto.Nombre?.Trim() ?? string.Empty;
        dto.CodigoProducto = string.IsNullOrWhiteSpace(dto.CodigoProducto) ? null : dto.CodigoProducto.Trim();
        dto.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();
        dto.UnidadMedida = string.IsNullOrWhiteSpace(dto.UnidadMedida) ? "unidad" : dto.UnidadMedida.Trim();

        if (dto.Nombre.Length < 2 || dto.Nombre.Length > 100)
            return BadRequest(new { message = "El nombre del producto debe tener entre 2 y 100 caracteres." });

        if (dto.IdProveedor <= 0)
            return BadRequest(new { message = "Debe seleccionar un proveedor válido." });

        if (dto.CantidadDisponible < 0)
            return BadRequest(new { message = "La cantidad disponible no puede ser negativa." });

        if (dto.StockMinimo < 0)
            return BadRequest(new { message = "El stock mínimo no puede ser negativo." });

        if (dto.CostoUnitario < 0)
            return BadRequest(new { message = "El costo unitario no puede ser negativo." });

        if (dto.PrecioVenta.HasValue && dto.PrecioVenta.Value < 0)
            return BadRequest(new { message = "El precio de venta no puede ser negativo." });

        if (dto.EsPerecedero && dto.FechaVencimiento is null)
            return BadRequest(new { message = "La fecha de vencimiento es obligatoria para productos perecederos." });

        if (dto.FechaVencimiento.HasValue && dto.FechaVencimiento.Value.Date < new DateTime(1900, 1, 1))
            return BadRequest(new { message = "La fecha de vencimiento no es válida." });

        if (dto.CodigoProducto is { Length: > 20 })
            return BadRequest(new { message = "El código del producto no debe superar 20 caracteres." });

        if (dto.Descripcion is { Length: > 300 })
            return BadRequest(new { message = "La descripción no debe superar 300 caracteres." });

        return null;
    }

    private IActionResult? ValidateMovimiento(MovimientoInventarioCreateDto dto)
    {
        dto.TipoMovimiento = dto.TipoMovimiento?.Trim() ?? string.Empty;
        dto.Motivo = dto.Motivo?.Trim() ?? string.Empty;
        dto.Referencia = string.IsNullOrWhiteSpace(dto.Referencia) ? null : dto.Referencia.Trim();
        dto.Observaciones = string.IsNullOrWhiteSpace(dto.Observaciones) ? null : dto.Observaciones.Trim();

        if (dto.IdProducto <= 0)
            return BadRequest(new { message = "Debe seleccionar un producto válido." });

        if (!TiposMovimientoPermitidos.Contains(dto.TipoMovimiento))
            return BadRequest(new { message = "Debe seleccionar un tipo de movimiento válido." });

        if (dto.Cantidad <= 0)
            return BadRequest(new { message = "La cantidad debe ser mayor a cero." });

        if (dto.CostoUnitario.HasValue && dto.CostoUnitario.Value < 0)
            return BadRequest(new { message = "El costo unitario no puede ser negativo." });

        if (dto.Motivo.Length < 2 || dto.Motivo.Length > 100)
            return BadRequest(new { message = "El motivo debe tener entre 2 y 100 caracteres." });

        if (dto.Referencia is { Length: > 100 })
            return BadRequest(new { message = "La referencia no debe superar 100 caracteres." });

        if (dto.Observaciones is { Length: > 500 })
            return BadRequest(new { message = "Las observaciones no deben superar 500 caracteres." });

        if (dto.FechaMovimiento.HasValue && dto.FechaMovimiento.Value.Date < new DateTime(1900, 1, 1))
            return BadRequest(new { message = "La fecha del movimiento no es válida." });

        return null;
    }

    private IActionResult? ValidateCompra(CompraInventarioCreateDto dto)
    {
        dto.Referencia = dto.Referencia?.Trim() ?? string.Empty;
        dto.Observaciones = string.IsNullOrWhiteSpace(dto.Observaciones) ? null : dto.Observaciones.Trim();

        if (dto.IdProducto <= 0)
            return BadRequest(new { message = "Debe seleccionar un producto válido." });

        if (dto.Cantidad <= 0)
            return BadRequest(new { message = "La cantidad de compra debe ser mayor a cero." });

        if (dto.CostoUnitario.HasValue && dto.CostoUnitario.Value < 0)
            return BadRequest(new { message = "El costo unitario no puede ser negativo." });

        if (dto.Referencia.Length < 2 || dto.Referencia.Length > 100)
            return BadRequest(new { message = "La referencia de compra debe tener entre 2 y 100 caracteres." });

        if (dto.Observaciones is { Length: > 500 })
            return BadRequest(new { message = "Las observaciones no deben superar 500 caracteres." });

        return null;
    }
}
