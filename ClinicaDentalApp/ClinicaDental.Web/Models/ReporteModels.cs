using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Web.Models;

public sealed class ReporteFiltroModel
{
    [Display(Name = "Tipo de reporte")]
    public string TipoReporte { get; set; } = "ventas";

    [DataType(DataType.Date)]
    [Display(Name = "Fecha inicio")]
    public DateTime? FechaDesde { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha fin")]
    public DateTime? FechaHasta { get; set; }

    [Display(Name = "Texto de búsqueda")]
    public string? Texto { get; set; }

    [Display(Name = "Paciente")]
    public int? IdPaciente { get; set; }

    [Display(Name = "Personal responsable")]
    public int? IdUsuarioResponsable { get; set; }

    [Display(Name = "Estado de cita")]
    public int? IdEstadoCita { get; set; }

    [Display(Name = "Categoría de servicio")]
    public int? IdCategoriaServicio { get; set; }

    [Display(Name = "Estado de venta")]
    public int? IdEstadoVenta { get; set; }

    [Display(Name = "Proveedor")]
    public int? IdProveedor { get; set; }

    [Display(Name = "Estado de inventario")]
    public string? EstadoInventario { get; set; }

    [Display(Name = "Estado del paciente")]
    public bool? Activo { get; set; }
}

public sealed class ReporteResumenViewModel
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

public sealed class ReporteCitaViewModel
{
    public int IdCita { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public string Paciente { get; set; } = "";
    public string Dentista { get; set; } = "";
    public string EstadoCita { get; set; } = "";
    public string Motivo { get; set; } = "";
    public string? Observaciones { get; set; }
}

public sealed class ReportePacienteViewModel
{
    public int IdPaciente { get; set; }
    public string CodigoPaciente { get; set; } = "";
    public string Paciente { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string? Correo { get; set; }
    public string Genero { get; set; } = "";
    public DateTime FechaNacimiento { get; set; }
    public bool Activo { get; set; }
    public DateTime CreadoEn { get; set; }
    public int TotalCitas { get; set; }
    public int TotalVentas { get; set; }
}

public sealed class ReporteInventarioViewModel
{
    public int IdProducto { get; set; }
    public string CodigoProducto { get; set; } = "";
    public string Producto { get; set; } = "";
    public string? Proveedor { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public string UnidadMedida { get; set; } = "";
    public DateTime? FechaVencimiento { get; set; }
    public string EstadoProducto { get; set; } = "";
    public string EstadoClave { get; set; } = "";
    public decimal CostoUnitario { get; set; }
    public decimal? PrecioVenta { get; set; }
    public decimal ValorInventario { get; set; }
}

public sealed class ReporteVentaViewModel
{
    public int IdVenta { get; set; }
    public string NumeroVenta { get; set; } = "";
    public DateTime FechaVenta { get; set; }
    public string Paciente { get; set; } = "";
    public string UsuarioResponsable { get; set; } = "";
    public string CategoriaServicio { get; set; } = "";
    public decimal Precio { get; set; }
    public decimal Total { get; set; }
    public string EstadoVenta { get; set; } = "";
    public string MetodoPago { get; set; } = "";
    public string? Descripcion { get; set; }
}

public sealed class ReportePacienteOption
{
    public int IdPaciente { get; set; }
    public string CodigoPaciente { get; set; } = "";
    public string Nombres { get; set; } = "";
    public string Apellidos { get; set; } = "";
    public string NombreCompleto => $"{CodigoPaciente} - {Nombres} {Apellidos}".Trim();
}

public sealed class ReporteUsuarioOption
{
    public int IdUsuario { get; set; }
    public string CodigoUsuario { get; set; } = "";
    public string Nombres { get; set; } = "";
    public string Apellidos { get; set; } = "";
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
}

public sealed class ReporteEstadoCitaOption
{
    public int IdEstadoCita { get; set; }
    public string Nombre { get; set; } = "";
}

public sealed class ReporteCategoriaServicioOption
{
    public int IdCategoriaServicio { get; set; }
    public string CodigoCategoria { get; set; } = "";
    public string Nombre { get; set; } = "";
    public decimal PrecioBase { get; set; }
}

public sealed class ReporteEstadoVentaOption
{
    public int IdEstadoVenta { get; set; }
    public string Nombre { get; set; } = "";
}

public sealed class ReporteProveedorOption
{
    public int IdProveedor { get; set; }
    public string Nombre { get; set; } = "";
    public string? Nit { get; set; }
    public bool Activo { get; set; }
}