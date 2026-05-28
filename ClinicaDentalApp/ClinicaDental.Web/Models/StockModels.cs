using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Web.Models;

public sealed class StockProductoViewModel
{
    public int IdProducto { get; set; }
    public string CodigoProducto { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int? IdProveedor { get; set; }
    public string? Proveedor { get; set; }
    public decimal CantidadDisponible { get; set; }
    public decimal StockMinimo { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;
    public DateTime? FechaVencimiento { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal? PrecioVenta { get; set; }
    public string EstadoProducto { get; set; } = string.Empty;
    public string EstadoClave { get; set; } = string.Empty;
    public DateTime? UltimaActualizacion { get; set; }
    public bool Activo { get; set; }
}

public sealed class StockResumenViewModel
{
    public int TotalProductos { get; set; }
    public int Disponibles { get; set; }
    public int BajoStock { get; set; }
    public int Agotados { get; set; }
    public int Vencidos { get; set; }
    public int ProximosVencer { get; set; }
}

public sealed class ProveedorCatalogoViewModel
{
    public int IdProveedor { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Nit { get; set; }
    public bool Activo { get; set; }
}

public sealed class ProductoCreateModel : IValidatableObject
{
    [Display(Name = "Código del producto")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El código debe tener entre 2 y 20 caracteres.")]
    public string? CodigoProducto { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [Display(Name = "Nombre del producto")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido.")]
    [Display(Name = "Proveedor")]
    public int IdProveedor { get; set; }

    [Display(Name = "¿Producto perecedero?")]
    public bool EsPerecedero { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de vencimiento")]
    public DateTime? FechaVencimiento { get; set; }

    [StringLength(300, ErrorMessage = "La descripción no debe superar 300 caracteres.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Range(0.0, 9999999999.0, ErrorMessage = "El stock mínimo no puede ser negativo.")]
    [Display(Name = "Stock mínimo")]
    public decimal StockMinimo { get; set; } = 0;

    [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "La unidad de medida debe tener máximo 30 caracteres.")]
    [Display(Name = "Unidad de medida")]
    public string UnidadMedida { get; set; } = "unidad";

    [Range(0.0, 9999999999.0, ErrorMessage = "El costo unitario no puede ser negativo.")]
    [Display(Name = "Costo unitario sugerido")]
    public decimal CostoUnitario { get; set; } = 0;

    [Range(0.0, 9999999999.0, ErrorMessage = "El precio de venta no puede ser negativo.")]
    [Display(Name = "Precio de venta")]
    public decimal? PrecioVenta { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EsPerecedero && FechaVencimiento is null)
        {
            yield return new ValidationResult(
                "La fecha de vencimiento es obligatoria para productos perecederos.",
                new[] { nameof(FechaVencimiento) });
        }

        if (FechaVencimiento.HasValue && FechaVencimiento.Value.Date < new DateTime(1900, 1, 1))
        {
            yield return new ValidationResult(
                "La fecha de vencimiento no es válida.",
                new[] { nameof(FechaVencimiento) });
        }
    }
}

public sealed class ProductoEditModel : IValidatableObject
{
    public int IdProducto { get; set; }

    [Display(Name = "Código del producto")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El código debe tener entre 2 y 20 caracteres.")]
    public string? CodigoProducto { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [Display(Name = "Nombre del producto")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido.")]
    [Display(Name = "Proveedor")]
    public int IdProveedor { get; set; }

    [Required(ErrorMessage = "La cantidad disponible es obligatoria.")]
    [Range(0.0, 9999999999.0, ErrorMessage = "La cantidad disponible no puede ser negativa.")]
    [Display(Name = "Cantidad disponible")]
    public decimal CantidadDisponible { get; set; }

    [Display(Name = "¿Producto perecedero?")]
    public bool EsPerecedero { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de vencimiento")]
    public DateTime? FechaVencimiento { get; set; }

    [StringLength(300, ErrorMessage = "La descripción no debe superar 300 caracteres.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Range(0.0, 9999999999.0, ErrorMessage = "El stock mínimo no puede ser negativo.")]
    [Display(Name = "Stock mínimo")]
    public decimal StockMinimo { get; set; } = 0;

    [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "La unidad de medida debe tener máximo 30 caracteres.")]
    [Display(Name = "Unidad de medida")]
    public string UnidadMedida { get; set; } = "unidad";

    [Range(0.0, 9999999999.0, ErrorMessage = "El costo unitario no puede ser negativo.")]
    [Display(Name = "Costo unitario")]
    public decimal CostoUnitario { get; set; } = 0;

    [Range(0.0, 9999999999.0, ErrorMessage = "El precio de venta no puede ser negativo.")]
    [Display(Name = "Precio de venta")]
    public decimal? PrecioVenta { get; set; }

    [Display(Name = "Producto activo")]
    public bool Activo { get; set; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EsPerecedero && FechaVencimiento is null)
        {
            yield return new ValidationResult(
                "La fecha de vencimiento es obligatoria para productos perecederos.",
                new[] { nameof(FechaVencimiento) });
        }

        if (FechaVencimiento.HasValue && FechaVencimiento.Value.Date < new DateTime(1900, 1, 1))
        {
            yield return new ValidationResult(
                "La fecha de vencimiento no es válida.",
                new[] { nameof(FechaVencimiento) });
        }
    }
}

public sealed class CompraInventarioCreateModel
{
    [Required(ErrorMessage = "Debe seleccionar un producto.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
    [Display(Name = "Producto")]
    public int IdProducto { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(0.01, 9999999999.0, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    [Display(Name = "Cantidad que ingresa")]
    public decimal Cantidad { get; set; }

    [Range(0.0, 9999999999.0, ErrorMessage = "El costo unitario no puede ser negativo.")]
    [Display(Name = "Costo unitario")]
    public decimal? CostoUnitario { get; set; }

    [Required(ErrorMessage = "La referencia de compra es obligatoria.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "La referencia debe tener entre 2 y 100 caracteres.")]
    [Display(Name = "Compra / factura / referencia")]
    public string Referencia { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Las observaciones no deben superar 500 caracteres.")]
    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }
}

public sealed class MovimientoInventarioViewModel
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

public sealed class TipoMovimientoCatalogoViewModel
{
    public int IdTipoMovimiento { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public sealed class MovimientoInventarioCreateModel
{
    [Required(ErrorMessage = "Debe seleccionar un producto.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
    [Display(Name = "Producto")]
    public int IdProducto { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el tipo de movimiento.")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Debe seleccionar un tipo de movimiento válido.")]
    [Display(Name = "Tipo de movimiento")]
    public string TipoMovimiento { get; set; } = "Salida";

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(0.01, 9999999999.0, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    [Display(Name = "Cantidad")]
    public decimal Cantidad { get; set; }

    [Range(0.0, 9999999999.0, ErrorMessage = "El costo unitario no puede ser negativo.")]
    [Display(Name = "Costo unitario")]
    public decimal? CostoUnitario { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Fecha del movimiento")]
    public DateTime? FechaMovimiento { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El motivo es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El motivo debe tener entre 2 y 100 caracteres.")]
    [Display(Name = "Motivo")]
    public string Motivo { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "La referencia no debe superar 100 caracteres.")]
    [Display(Name = "Referencia")]
    public string? Referencia { get; set; }

    [StringLength(500, ErrorMessage = "Las observaciones no deben superar 500 caracteres.")]
    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }
}
