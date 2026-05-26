using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Api.DTOs;

public sealed class CategoriaServicioDto
{
    public int IdCategoriaServicio { get; set; }
    public string CodigoCategoria { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioBase { get; set; }
    public bool Activo { get; set; }
    public string Estado => Activo ? "Activa" : "Inactiva";
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }
}

public sealed class CategoriaServicioCreateDto
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string CodigoCategoria { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Descripcion { get; set; }

    [Required]
    [Range(0.01, 9999999999.0)]
    public decimal PrecioBase { get; set; }

    public bool Activo { get; set; } = true;
}

public sealed class CategoriaServicioUpdateDto
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string CodigoCategoria { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Descripcion { get; set; }

    [Required]
    [Range(0.01, 9999999999.0)]
    public decimal PrecioBase { get; set; }

    public bool Activo { get; set; } = true;
}
