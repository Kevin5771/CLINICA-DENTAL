using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Api.DTOs;

public sealed class CitaCreateDto
{
    [Range(1, int.MaxValue)] public int IdPaciente { get; set; }
    [Range(1, int.MaxValue)] public int IdDentista { get; set; }
    [Required] public DateTime Fecha { get; set; }
    [Required] public TimeSpan HoraInicio { get; set; }
    [Required] public TimeSpan HoraFin { get; set; }
    [Required, StringLength(250, MinimumLength = 3)] public string Motivo { get; set; } = string.Empty;
    [StringLength(1000)] public string? Observaciones { get; set; }
    [Range(1, int.MaxValue)] public int IdEstadoCita { get; set; }
    [Range(1, int.MaxValue)] public int CreadaPor { get; set; }
}

public sealed class CitaUpdateDto
{
    [Range(1, int.MaxValue)] public int IdPaciente { get; set; }
    [Range(1, int.MaxValue)] public int IdDentista { get; set; }
    [Required] public DateTime Fecha { get; set; }
    [Required] public TimeSpan HoraInicio { get; set; }
    [Required] public TimeSpan HoraFin { get; set; }
    [Required, StringLength(250, MinimumLength = 3)] public string Motivo { get; set; } = string.Empty;
    [StringLength(1000)] public string? Observaciones { get; set; }
    [Range(1, int.MaxValue)] public int IdEstadoCita { get; set; }
    [Range(1, int.MaxValue)] public int IdUsuarioAccion { get; set; }
}

public sealed class CancelarCitaDto
{
    [Range(1, int.MaxValue)] public int IdUsuarioAccion { get; set; }
    [StringLength(500)] public string? Comentario { get; set; }
}
