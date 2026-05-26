namespace ClinicaDental.Api.Models;

public sealed class Cita
{
    public int IdCita { get; set; }
    public int IdPaciente { get; set; }
    public string? Paciente { get; set; }
    public int IdDentista { get; set; }
    public string? Dentista { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public int IdEstadoCita { get; set; }
    public string? EstadoCita { get; set; }
    public int CreadaPor { get; set; }
    public DateTime CreadaEn { get; set; }
    public DateTime? ActualizadaEn { get; set; }
}
