namespace ClinicaDental.Api.Models;

public sealed class Paciente
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
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }
}
