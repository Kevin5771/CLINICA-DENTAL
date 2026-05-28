using System.Text.Json;
using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Calendario;

public class IndexModel : PageModel
{
    private readonly ApiClient _apiClient;

    public IndexModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public List<CitaViewModel> Citas { get; set; } = new();
    public string EventsJson { get; set; } = "[]";
    public int TotalCitas { get; set; }
    public int CitasHoy { get; set; }
    public int CitasSemana { get; set; }
    public int CitasPendientes { get; set; }

    public async Task OnGetAsync()
    {
        Citas = await _apiClient.GetAsync<List<CitaViewModel>>("api/citas") ?? new();

        var today = DateTime.Today;
        TotalCitas = Citas.Count;
        CitasHoy = Citas.Count(c => c.Fecha.Date == today);
        CitasSemana = Citas.Count(c => c.Fecha.Date >= today && c.Fecha.Date <= today.AddDays(7) && !EsCancelada(c.EstadoCita));
        CitasPendientes = Citas.Count(c => EsPendiente(c.EstadoCita));

        var events = Citas.Select(c =>
        {
            var colors = GetColorByEstado(c.EstadoCita);
            var paciente = string.IsNullOrWhiteSpace(c.Paciente) ? "Paciente sin nombre" : c.Paciente;
            var dentista = string.IsNullOrWhiteSpace(c.Dentista) ? "No asignado" : c.Dentista;
            var estado = string.IsNullOrWhiteSpace(c.EstadoCita) ? "Sin estado" : c.EstadoCita;
            var motivo = string.IsNullOrWhiteSpace(c.Motivo) ? "Sin motivo registrado" : c.Motivo;
            var observaciones = string.IsNullOrWhiteSpace(c.Observaciones) ? "Sin observaciones" : c.Observaciones;

            return new CalendarEvent
            {
                Id = c.IdCita.ToString(),
                Title = $"{c.HoraInicio:hh\\:mm} · {paciente}",
                Start = c.Fecha.Date.Add(c.HoraInicio).ToString("yyyy-MM-ddTHH:mm:ss"),
                End = c.Fecha.Date.Add(c.HoraFin).ToString("yyyy-MM-ddTHH:mm:ss"),
                BackgroundColor = colors.Background,
                BorderColor = colors.Border,
                TextColor = colors.Text,
                ExtendedProps = new Dictionary<string, string?>
                {
                    ["paciente"] = paciente,
                    ["dentista"] = dentista,
                    ["estado"] = estado,
                    ["motivo"] = motivo,
                    ["observaciones"] = observaciones,
                    ["fecha"] = c.Fecha.ToString("dd/MM/yyyy"),
                    ["hora"] = $"{c.HoraInicio:hh\\:mm} - {c.HoraFin:hh\\:mm}",
                    ["editUrl"] = Url.Page("/Citas/Edit", new { id = c.IdCita }) ?? $"/Citas/Edit?id={c.IdCita}"
                }
            };
        }).ToList();

        EventsJson = JsonSerializer.Serialize(events, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private static bool EsPendiente(string? estado)
    {
        return Normalize(estado).Contains("pend");
    }

    private static bool EsCancelada(string? estado)
    {
        return Normalize(estado).Contains("cancel");
    }

    private static string Normalize(string? value)
    {
        return (value ?? string.Empty).Trim().ToLowerInvariant();
    }

    private static CalendarColors GetColorByEstado(string? estado)
    {
        var value = Normalize(estado);

        if (value.Contains("cancel"))
            return new CalendarColors("#fee2e2", "#ef4444", "#991b1b");

        if (value.Contains("complet") || value.Contains("realiz") || value.Contains("final"))
            return new CalendarColors("#dbeafe", "#2563eb", "#1e3a8a");

        if (value.Contains("confirm"))
            return new CalendarColors("#ccfbf1", "#0f766e", "#134e4a");

        if (value.Contains("pend"))
            return new CalendarColors("#fef3c7", "#d97706", "#92400e");

        return new CalendarColors("#e0f2fe", "#0891b2", "#164e63");
    }

    private sealed record CalendarColors(string Background, string Border, string Text);

    public sealed class CalendarEvent
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string BorderColor { get; set; } = string.Empty;
        public string TextColor { get; set; } = string.Empty;
        public Dictionary<string, string?> ExtendedProps { get; set; } = new();
    }
}
