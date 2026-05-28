using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Inventario;

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
    public string? Estado { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool SoloBajoStock { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool SoloAgotados { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool SoloVencidos { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool SoloProximosVencer { get; set; }

    public List<StockProductoViewModel> Items { get; set; } = new();

    public StockResumenViewModel Resumen { get; set; } = new();

    public async Task OnGetAsync()
    {
        Resumen = await _apiClient.GetAsync<StockResumenViewModel>("api/stock/resumen") ?? new();

        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(Texto))
            query.Add($"texto={Uri.EscapeDataString(Texto)}");

        if (!string.IsNullOrWhiteSpace(Estado))
            query.Add($"estado={Uri.EscapeDataString(Estado)}");

        if (SoloBajoStock)
            query.Add("soloBajoStock=true");

        if (SoloAgotados)
            query.Add("soloAgotados=true");

        if (SoloVencidos)
            query.Add("soloVencidos=true");

        if (SoloProximosVencer)
            query.Add("soloProximosVencer=true");

        var url = "api/stock" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);

        Items = await _apiClient.GetAsync<List<StockProductoViewModel>>(url) ?? new();
    }

    public static string GetEstadoClasses(string? estadoClave)
    {
        return estadoClave switch
        {
            "disponible" => "bg-emerald-100 text-emerald-700",
            "bajo_stock" => "bg-amber-100 text-amber-700",
            "agotado" => "bg-rose-100 text-rose-700",
            "vencido" => "bg-red-100 text-red-700",
            "proximo_vencer" => "bg-cyan-100 text-cyan-700",
            _ => "bg-slate-100 text-slate-700"
        };
    }
}