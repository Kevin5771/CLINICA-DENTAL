using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Web.Models;

public sealed class VentaServicioViewModel
{
    public int IdVenta { get; set; }
    public string NumeroVenta { get; set; } = string.Empty;
    public int? IdPaciente { get; set; }
    public string? Paciente { get; set; }
    public int IdUsuario { get; set; }
    public string UsuarioResponsable { get; set; } = string.Empty;
    public int IdCategoriaServicio { get; set; }
    public string CategoriaServicio { get; set; } = string.Empty;
    public DateTime FechaVenta { get; set; }
    public decimal Precio { get; set; }
    public decimal Total { get; set; }
    public int IdEstadoVenta { get; set; }
    public string EstadoVenta { get; set; } = string.Empty;
    public int IdMetodoPago { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime CreadoEn { get; set; }
}

public sealed class VentaServicioCreateModel
{
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El comprobante debe tener entre 2 y 20 caracteres.")]
    [Display(Name = "Número de comprobante")]
    public string? NumeroComprobante { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un paciente.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un paciente válido.")]
    [Display(Name = "Paciente")]
    public int IdPaciente { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el personal responsable.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar personal válido.")]
    [Display(Name = "Personal responsable")]
    public int IdUsuarioResponsable { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una categoría de servicio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
    [Display(Name = "Categoría de servicio")]
    public int IdCategoriaServicio { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, 9999999999.0, ErrorMessage = "El precio debe ser mayor a cero.")]
    [Display(Name = "Precio")]
    public decimal Precio { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Fecha de venta")]
    public DateTime? FechaVenta { get; set; } = DateTime.Now;

    [StringLength(250, ErrorMessage = "La descripción no debe superar 250 caracteres.")]
    [Display(Name = "Descripción de la venta")]
    public string? Descripcion { get; set; }

    [Display(Name = "Estado de venta")]
    public int? IdEstadoVenta { get; set; }

    [Display(Name = "Método de pago")]
    public int? IdMetodoPago { get; set; }
}

public sealed class PacienteCatalogoViewModel
{
    public int IdPaciente { get; set; }
    public string CodigoPaciente { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreCompleto => $"{CodigoPaciente} - {Nombres} {Apellidos}";
}

public sealed class UsuarioCatalogoViewModel
{
    public int IdUsuario { get; set; }
    public string CodigoUsuario { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreCompleto => $"{CodigoUsuario} - {Nombres} {Apellidos}";
}

public sealed class CategoriaServicioCatalogoViewModel
{
    public int IdCategoriaServicio { get; set; }
    public string CodigoCategoria { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioBase { get; set; }
}

public sealed class EstadoVentaCatalogoViewModel
{
    public int IdEstadoVenta { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public sealed class MetodoPagoCatalogoViewModel
{
    public int IdMetodoPago { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
