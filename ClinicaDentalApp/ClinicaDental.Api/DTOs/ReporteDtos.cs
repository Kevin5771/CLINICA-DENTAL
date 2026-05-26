namespace ClinicaDental.Api.DTOs;

public sealed class ReporteCitaDto
{
    public int IdCita { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string Dentista { get; set; } = string.Empty;
    public string EstadoCita { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}

public sealed class ReportePacienteDto
{
    public int IdPaciente { get; set; }
    public string CodigoPaciente { get; set; } = string.Empty;
    public string Paciente { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Correo { get; set; }
    public string Genero { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public bool Activo { get; set; }
    public DateTime CreadoEn { get; set; }
    public int TotalCitas { get; set; }
    public int TotalVentas { get; set; }
}

public sealed class ReporteInventarioDto
{
    public int IdProducto { get; set; }
    public string CodigoProducto { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
    public string? Proveedor { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;
    public DateTime? FechaVencimiento { get; set; }
    public string EstadoProducto { get; set; } = string.Empty;
    public string EstadoClave { get; set; } = string.Empty;
    public decimal CostoUnitario { get; set; }
    public decimal? PrecioVenta { get; set; }
    public decimal ValorInventario { get; set; }
}

public sealed class ReporteVentaDto
{
    public int IdVenta { get; set; }
    public string NumeroVenta { get; set; } = string.Empty;
    public DateTime FechaVenta { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string UsuarioResponsable { get; set; } = string.Empty;
    public string CategoriaServicio { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public decimal Total { get; set; }
    public string EstadoVenta { get; set; } = string.Empty;
    public string MetodoPago { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public sealed class ReporteResumenDto
{
    public int TotalCitas { get; set; }
    public int TotalPacientes { get; set; }
    public int TotalProductos { get; set; }
    public int TotalVentas { get; set; }
    public decimal MontoVentas { get; set; }
    public int ProductosBajoStock { get; set; }
    public int ProductosVencidos { get; set; }
    public int VentasPagadas { get; set; }
}