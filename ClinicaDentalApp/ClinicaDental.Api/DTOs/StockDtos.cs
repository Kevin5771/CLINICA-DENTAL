using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Api.DTOs;

public sealed class ProductoCreateDto
{
    [StringLength(20, MinimumLength = 2)]
    public string? CodigoProducto { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int IdProveedor { get; set; }

    public bool EsPerecedero { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    [StringLength(300)]
    public string? Descripcion { get; set; }

    [Range(0.0, 9999999999.0)]
    public decimal StockMinimo { get; set; } = 0;

    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string UnidadMedida { get; set; } = "unidad";

    [Range(0.0, 9999999999.0)]
    public decimal CostoUnitario { get; set; } = 0;

    [Range(0.0, 9999999999.0)]
    public decimal? PrecioVenta { get; set; }
}

public sealed class ProductoUpdateDto
{
    [StringLength(20, MinimumLength = 2)]
    public string? CodigoProducto { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int IdProveedor { get; set; }

    [Required]
    [Range(0.0, 9999999999.0)]
    public decimal CantidadDisponible { get; set; }

    public bool EsPerecedero { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    [StringLength(300)]
    public string? Descripcion { get; set; }

    [Range(0.0, 9999999999.0)]
    public decimal StockMinimo { get; set; } = 0;

    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string UnidadMedida { get; set; } = "unidad";

    [Range(0.0, 9999999999.0)]
    public decimal CostoUnitario { get; set; } = 0;

    [Range(0.0, 9999999999.0)]
    public decimal? PrecioVenta { get; set; }

    public bool Activo { get; set; } = true;
}

public sealed class CompraInventarioCreateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int IdProducto { get; set; }

    [Required]
    [Range(0.01, 9999999999.0)]
    public decimal Cantidad { get; set; }

    [Range(0.0, 9999999999.0)]
    public decimal? CostoUnitario { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Referencia { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Observaciones { get; set; }
}

public sealed class MovimientoInventarioDto
{
    public int IdMovimientoInventario { get; set; }
    public int IdProducto { get; set; }
    public string CodigoProducto { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
    public int IdTipoMovimiento { get; set; }
    public string TipoMovimiento { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal? CostoUnitario { get; set; }
    public string? Referencia { get; set; }
    public string? Observaciones { get; set; }
    public string? Motivo { get; set; }
    public decimal? StockDespues { get; set; }
    public int RealizadoPor { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public DateTime FechaMovimiento { get; set; }
}

public sealed class MovimientoInventarioCreateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int IdProducto { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 2)]
    public string TipoMovimiento { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 9999999999.0)]
    public decimal Cantidad { get; set; }

    [Range(0.0, 9999999999.0)]
    public decimal? CostoUnitario { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Motivo { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Referencia { get; set; }

    [StringLength(500)]
    public string? Observaciones { get; set; }
}

public sealed class ProveedorCatalogoDto
{
    public int IdProveedor { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Nit { get; set; }
    public bool Activo { get; set; }
}
