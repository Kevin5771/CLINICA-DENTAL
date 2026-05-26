using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Api.DTOs;

public sealed class VentaServicioCreateDto
{
    [StringLength(20, MinimumLength = 2)]
    public string? NumeroComprobante { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int IdPaciente { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int IdUsuarioResponsable { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int IdCategoriaServicio { get; set; }

    [Required]
    [Range(0.01, 9999999999.0)]
    public decimal Precio { get; set; }

    public DateTime? FechaVenta { get; set; }

    [StringLength(250)]
    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue)]
    public int? IdEstadoVenta { get; set; }

    [Range(1, int.MaxValue)]
    public int? IdMetodoPago { get; set; }
}

public sealed class VentaServicioDto
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
