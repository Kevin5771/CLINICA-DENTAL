namespace ClinicaDental.Api.Models;

public sealed class StockProducto
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
