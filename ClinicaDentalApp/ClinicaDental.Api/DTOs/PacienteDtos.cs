using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Api.DTOs;

public class PacienteCreateDto
{
    [Required, StringLength(20, MinimumLength = 2)]
    public string CodigoPaciente { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 2)]
    public string Nombres { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 2)]
    public string Apellidos { get; set; } = string.Empty;

    [Required, StringLength(20, MinimumLength = 8)]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [Required, StringLength(20)]
    public string Genero { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Direccion { get; set; }

    [EmailAddress, StringLength(150)]
    public string? Correo { get; set; }

    [StringLength(500)]
    public string? Alergias { get; set; }

    [StringLength(1000)]
    public string? ObservacionesGenerales { get; set; }
}

public class PacienteUpdateDto : PacienteCreateDto
{
    public bool Activo { get; set; } = true;
}