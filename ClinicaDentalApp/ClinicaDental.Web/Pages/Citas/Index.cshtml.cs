using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Citas;

public class IndexModel : PageModel
{
    private readonly ApiClient _apiClient;

    public IndexModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)]
    public DateTime? FechaDesde { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? FechaHasta { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? IdEstadoCita { get; set; }

    public string? SuccessMessage { get; set; }

    public List<SelectItem> Estados { get; set; } = new();
    public List<CitaViewModel> Items { get; set; } = new();

    public async Task OnGetAsync()
    {
        SuccessMessage = TempData["SuccessMessage"]?.ToString();
        Estados = await _apiClient.GetAsync<List<SelectItem>>("api/catalogos/estados-cita") ?? new();

        var query = new List<string>();
        if (FechaDesde.HasValue)
            query.Add($"fechaDesde={FechaDesde.Value:yyyy-MM-dd}");
        if (FechaHasta.HasValue)
            query.Add($"fechaHasta={FechaHasta.Value:yyyy-MM-dd}");
        if (IdEstadoCita.HasValue)
            query.Add($"idEstadoCita={IdEstadoCita.Value}");

        var url = "api/citas" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        Items = await _apiClient.GetAsync<List<CitaViewModel>>(url) ?? new();
    }
}