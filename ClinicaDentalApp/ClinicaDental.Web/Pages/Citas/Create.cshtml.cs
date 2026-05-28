using System.ComponentModel.DataAnnotations;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Citas;

public class CreateModel : PageModel
{
    private readonly ApiClient _apiClient;

    public CreateModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public CitaCreateInputModel Input { get; set; } = new();

    public string? Message { get; set; }

    public List<PacienteOption> Pacientes { get; set; } = new();
    public List<DentistaOption> Dentistas { get; set; } = new();

    public string NombreUsuario { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        await CargarCatalogosAsync();

        Input.IdEstadoCita = 1;
        Input.CreadaPor = HttpContext.Session.GetInt32("id_usuario") ?? 0;
        Input.Fecha = DateTime.Today;
        Input.HoraInicio = "09:00";
        Input.HoraFin = "10:00";

        NombreUsuario = ObtenerNombreUsuario();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await CargarCatalogosAsync();

        Input.IdEstadoCita = 1;
        Input.CreadaPor = HttpContext.Session.GetInt32("id_usuario") ?? 0;
        NombreUsuario = ObtenerNombreUsuario();

        if (Input.IdPaciente <= 0)
            ModelState.AddModelError("Input.IdPaciente", "Debes seleccionar un paciente válido.");

        if (Input.IdDentista <= 0)
            ModelState.AddModelError("Input.IdDentista", "Debes seleccionar un dentista válido.");

        if (Input.HoraFin.CompareTo(Input.HoraInicio) <= 0)
            ModelState.AddModelError("Input.HoraFin", "La hora fin debe ser mayor que la hora inicio.");

        if (!ModelState.IsValid)
            return Page();

        var payload = new
        {
            idPaciente = Input.IdPaciente,
            idDentista = Input.IdDentista,
            idEstadoCita = Input.IdEstadoCita,
            creadaPor = Input.CreadaPor,
            fecha = Input.Fecha.ToString("yyyy-MM-dd"),
            horaInicio = NormalizarHora(Input.HoraInicio),
            horaFin = NormalizarHora(Input.HoraFin),
            motivo = Input.Motivo,
            observaciones = Input.Observaciones
        };

        var result = await _apiClient.PostAsync<CitaResponseModel>("api/citas", payload);

        if (!result.Ok)
        {
            Message = $"No fue posible guardar la cita: {result.Error}";
            return Page();
        }

        TempData["SuccessMessage"] = "Cita guardada correctamente.";
        return RedirectToPage("/Citas/Index");
    }

    private async Task CargarCatalogosAsync()
    {
        var pacientesApi = await _apiClient.GetAsync<List<PacienteOption>>("api/pacientes");
        Pacientes = pacientesApi ?? new List<PacienteOption>();

        var dentistasApi = await _apiClient.GetAsync<List<DentistaOption>>("api/usuarios/dentistas");
        Dentistas = dentistasApi ?? new List<DentistaOption>();
    }

    private string ObtenerNombreUsuario()
    {
        return HttpContext.Session.GetString("NombreCompleto")
            ?? HttpContext.Session.GetString("Usuario")
            ?? "Usuario actual";
    }

    private static string NormalizarHora(string hora)
    {
        if (string.IsNullOrWhiteSpace(hora))
            return "00:00:00";

        return hora.Length == 5 ? $"{hora}:00" : hora;
    }

    public class CitaCreateInputModel
    {
        [Required]
        public int IdPaciente { get; set; }

        [Required]
        public int IdDentista { get; set; }

        public int IdEstadoCita { get; set; }

        public int CreadaPor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        public string HoraInicio { get; set; } = string.Empty;

        [Required]
        public string HoraFin { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Motivo { get; set; } = string.Empty;

        public string? Observaciones { get; set; }
    }

    public class PacienteOption
    {
        public int IdPaciente { get; set; }
        public string CodigoPaciente { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;

        public string NombreCompleto => $"{CodigoPaciente} - {Nombres} {Apellidos}";
    }

    public class DentistaOption
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;

        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }

    public class CitaResponseModel
    {
        public int IdCita { get; set; }
    }
}