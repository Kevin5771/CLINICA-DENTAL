using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Web.Models;

public sealed class PacienteViewModel
{
    public int IdPaciente { get; set; }
    public string CodigoPaciente { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string Genero { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Correo { get; set; }
    public string? Alergias { get; set; }
    public string? ObservacionesGenerales { get; set; }
    public bool Activo { get; set; }

    public int Edad
    {
        get
        {
            var today = DateTime.Today;
            var edad = today.Year - FechaNacimiento.Year;
            if (FechaNacimiento.Date > today.AddYears(-edad)) edad--;
            return edad < 0 ? 0 : edad;
        }
    }
}

public class PacienteCreateModel
{
    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El código debe tener entre 2 y 20 caracteres.")]
    [Display(Name = "Código")]
    public string CodigoPaciente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El nombre solo debe contener letras.")]
    [Display(Name = "Nombre")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El apellido solo debe contener letras.")]
    [Display(Name = "Apellido")]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "El teléfono debe tener entre 8 y 20 caracteres.")]
    [RegularExpression(@"^[0-9+\-()\s]{8,20}$", ErrorMessage = "El teléfono no tiene un formato válido.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de nacimiento")]
    public DateTime FechaNacimiento { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "El género es obligatorio.")]
    [StringLength(20)]
    [Display(Name = "Género")]
    public string Genero { get; set; } = string.Empty;

    [StringLength(250)]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }

    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150)]
    [Display(Name = "Correo")]
    public string? Correo { get; set; }

    [StringLength(500)]
    [Display(Name = "Alergias")]
    public string? Alergias { get; set; }

    [StringLength(1000)]
    [Display(Name = "Otra información relevante")]
    public string? ObservacionesGenerales { get; set; }

    public int EdadCalculada
    {
        get
        {
            var today = DateTime.Today;
            var edad = today.Year - FechaNacimiento.Year;
            if (FechaNacimiento.Date > today.AddYears(-edad)) edad--;
            return edad < 0 ? 0 : edad;
        }
    }
}

public sealed class PacienteUpdateModel : PacienteCreateModel
{
    public int IdPaciente { get; set; }
    public bool Activo { get; set; } = true;
}