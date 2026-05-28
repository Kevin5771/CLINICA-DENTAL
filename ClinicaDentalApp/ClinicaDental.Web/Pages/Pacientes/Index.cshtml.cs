using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Pacientes;

public class IndexModel : PageModel
{
    private readonly ApiClient _apiClient;

    public IndexModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)]
    public string? Texto { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool? Activo { get; set; }

    public string? SuccessMessage { get; set; }

    public List<PacienteViewModel> Items { get; set; } = new();

    public async Task OnGetAsync()
    {
        SuccessMessage = TempData["SuccessMessage"]?.ToString();

        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(Texto))
            query.Add($"texto={Uri.EscapeDataString(Texto)}");
        if (Activo.HasValue)
            query.Add($"activo={Activo.Value.ToString().ToLowerInvariant()}");

        var url = "api/pacientes" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        Items = await _apiClient.GetAsync<List<PacienteViewModel>>(url) ?? new();
    }
}