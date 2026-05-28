using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.CategoriasServicios;

public class IndexModel : PageModel
{
    private readonly ApiClient _apiClient;

    public IndexModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)] public string? Texto { get; set; }
    [BindProperty(SupportsGet = true)] public string? Estado { get; set; }

    public List<CategoriaServicioViewModel> Categorias { get; set; } = new();

    public async Task OnGetAsync()
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(Texto))
            query.Add($"texto={Uri.EscapeDataString(Texto)}");

        if (Estado == "activa")
            query.Add("activo=true");
        else if (Estado == "inactiva")
            query.Add("activo=false");

        var url = "api/categorias-servicio";
        if (query.Count > 0) url += "?" + string.Join("&", query);

        Categorias = await _apiClient.GetAsync<List<CategoriaServicioViewModel>>(url) ?? new();
    }
}
