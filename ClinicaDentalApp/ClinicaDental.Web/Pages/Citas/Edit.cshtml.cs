using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Citas;

public class EditModel : PageModel
{
    private readonly ApiClient _apiClient;

    public EditModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public CitaUpdateModel Input { get; set; } = new();

    public string? Message { get; set; }
    public string DentistaSeleccionado { get; set; } = string.Empty;
    public List<SelectItem> Dentistas { get; set; } = new();
    public List<SelectItem> Estados { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        await CargarCatalogosAsync();

        var cita = await _apiClient.GetAsync<CitaViewModel>($"api/citas/{id}");
        if (cita is null)
            return RedirectToPage("/Citas/Index");

        Input = new CitaUpdateModel
        {
            IdCita = cita.IdCita,
            IdPaciente = cita.IdPaciente,
            IdDentista = cita.IdDentista,
            Fecha = cita.Fecha,
            HoraInicio = cita.HoraInicio.ToString(@"hh\:mm"),
            HoraFin = cita.HoraFin.ToString(@"hh\:mm"),
            Motivo = cita.Motivo,
            Observaciones = cita.Observaciones,
            IdEstadoCita = cita.IdEstadoCita,
            IdUsuarioAccion = HttpContext.Session.GetInt32("id_usuario") ?? 0,
            PacienteReferencia = cita.Paciente ?? string.Empty
        };

        DentistaSeleccionado = cita.Dentista ?? string.Empty;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await CargarCatalogosAsync();
        Input.IdUsuarioAccion = HttpContext.Session.GetInt32("id_usuario") ?? 0;

        if (Input.IdDentista <= 0)
            ModelState.AddModelError("Input.IdDentista", "Debes seleccionar un responsable válido.");

        if (Input.IdEstadoCita <= 0)
            ModelState.AddModelError("Input.IdEstadoCita", "Debes seleccionar un estado válido.");

        if (Input.Fecha.Date < DateTime.Today)
            ModelState.AddModelError("Input.Fecha", "No se permiten fechas pasadas.");

        if (string.Compare(Input.HoraFin, Input.HoraInicio, StringComparison.Ordinal) <= 0)
            ModelState.AddModelError("Input.HoraFin", "La hora fin debe ser mayor a la hora de inicio.");

        if (!ModelState.IsValid)
        {
            DentistaSeleccionado = Dentistas.FirstOrDefault(d => d.Id == Input.IdDentista)?.Nombre ?? string.Empty;
            return Page();
        }

        var observaciones = Input.Observaciones;
        if (!string.IsNullOrWhiteSpace(Input.MotivoCambio))
        {
            observaciones = string.IsNullOrWhiteSpace(observaciones)
                ? $"Motivo de cambio: {Input.MotivoCambio}"
                : $"{observaciones}\nMotivo de cambio: {Input.MotivoCambio}";
        }

        var payload = new
        {
            idPaciente = Input.IdPaciente,
            idDentista = Input.IdDentista,
            fecha = Input.Fecha.ToString("yyyy-MM-dd"),
            horaInicio = NormalizarHora(Input.HoraInicio),
            horaFin = NormalizarHora(Input.HoraFin),
            motivo = Input.Motivo,
            observaciones,
            idEstadoCita = Input.IdEstadoCita,
            idUsuarioAccion = Input.IdUsuarioAccion
        };

        var result = await _apiClient.PutAsync<CitaViewModel>($"api/citas/{Input.IdCita}", payload);
        if (!result.Ok)
        {
            DentistaSeleccionado = Dentistas.FirstOrDefault(d => d.Id == Input.IdDentista)?.Nombre ?? string.Empty;
            Message = $"No fue posible actualizar la cita: {result.Error}";
            return Page();
        }

        TempData["SuccessMessage"] = "Cita actualizada correctamente.";
        return RedirectToPage("/Citas/Index");
    }

    private async Task CargarCatalogosAsync()
    {
        var dentistasApi = await _apiClient.GetAsync<List<DentistaOption>>("api/usuarios/dentistas") ?? new();
        Dentistas = dentistasApi
            .Select(d => new SelectItem
            {
                Id = d.IdUsuario,
                Nombre = $"{d.Nombres} {d.Apellidos}"
            })
            .ToList();

        var estadosApi = await _apiClient.GetAsync<List<EstadoCitaOption>>("api/catalogos/estados-cita") ?? new();
        Estados = estadosApi
            .Select(e => new SelectItem
            {
                Id = e.IdEstadoCita,
                Nombre = e.Nombre
            })
            .ToList();
    }

    private static string NormalizarHora(string hora)
    {
        if (string.IsNullOrWhiteSpace(hora))
            return "00:00:00";

        return hora.Length == 5 ? $"{hora}:00" : hora;
    }

    public class DentistaOption
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
    }
}