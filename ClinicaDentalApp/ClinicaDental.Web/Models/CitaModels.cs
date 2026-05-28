using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClinicaDental.Web.Models;

public sealed class CitaViewModel
{
    public int IdCita { get; set; }
    public int IdPaciente { get; set; }
    public int IdDentista { get; set; }
    public int IdEstadoCita { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public string? Paciente { get; set; }
    public string? Dentista { get; set; }
    public string? EstadoCita { get; set; }
}

public sealed class CitaCreateModel
{
    [Required] public int IdPaciente { get; set; }
    [Required] public int IdDentista { get; set; }
    [Required] public DateTime Fecha { get; set; } = DateTime.Today;
    [Required] public TimeSpan HoraInicio { get; set; } = new(9, 0, 0);
    [Required] public TimeSpan HoraFin { get; set; } = new(10, 0, 0);
    [Required] public string Motivo { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    [Required] public int IdEstadoCita { get; set; } = 1;
    [Required] public int CreadaPor { get; set; }
}

public sealed class CitaUpdateModel
{
    public int IdCita { get; set; }

    [Required]
    public int IdPaciente { get; set; }

    [Required]
    public int IdDentista { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
    public string HoraInicio { get; set; } = string.Empty;

    [Required(ErrorMessage = "La hora de fin es obligatoria.")]
    public string HoraFin { get; set; } = string.Empty;

    [Required(ErrorMessage = "El motivo es obligatorio.")]
    [StringLength(250, MinimumLength = 3, ErrorMessage = "El motivo debe tener entre 3 y 250 caracteres.")]
    public string Motivo { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Observaciones { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un estado válido.")]
    public int IdEstadoCita { get; set; }

    public int IdUsuarioAccion { get; set; }

    [StringLength(500)]
    [Display(Name = "Motivo de cambio")]
    public string? MotivoCambio { get; set; }

    public string PacienteReferencia { get; set; } = string.Empty;
}

public sealed class SelectItem
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public sealed class EstadoCitaOption
{
    [JsonPropertyName("idEstadoCita")]
    public int IdEstadoCita { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
}