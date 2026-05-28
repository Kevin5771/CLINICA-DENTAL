using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Web.Models;

public sealed class CategoriaServicioViewModel
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

public sealed class CategoriaServicioFormModel
{
    [Required(ErrorMessage = "El número o código es obligatorio.")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "El número o código debe tener máximo 20 caracteres.")]
    [Display(Name = "Número / código")]
    public string CodigoCategoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [Display(Name = "Nombre del servicio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, 9999999999.0, ErrorMessage = "El precio debe ser mayor a cero.")]
    [Display(Name = "Precio")]
    public decimal PrecioBase { get; set; }

    [StringLength(300, ErrorMessage = "La descripción no debe superar 300 caracteres.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Display(Name = "Categoría activa")]
    public bool Activo { get; set; } = true;
}
