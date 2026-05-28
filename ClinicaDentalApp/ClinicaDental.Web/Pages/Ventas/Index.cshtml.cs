using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Ventas;

public class IndexModel : PageModel
{
    private readonly ApiClient _apiClient;

    public IndexModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)] public string? Texto { get; set; }
    [BindProperty(SupportsGet = true)] public int? IdPaciente { get; set; }
    [BindProperty(SupportsGet = true)] public int? IdUsuarioResponsable { get; set; }
    [BindProperty(SupportsGet = true)] public int? IdCategoriaServicio { get; set; }
    [BindProperty(SupportsGet = true)] public int? IdEstadoVenta { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime? FechaDesde { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime? FechaHasta { get; set; }

    public List<VentaServicioViewModel> Ventas { get; set; } = new();
    public List<PacienteCatalogoViewModel> Pacientes { get; set; } = new();
    public List<UsuarioCatalogoViewModel> Usuarios { get; set; } = new();
    public List<CategoriaServicioCatalogoViewModel> Categorias { get; set; } = new();
    public List<EstadoVentaCatalogoViewModel> Estados { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadCatalogosAsync();
        await LoadVentasAsync();
    }

    private async Task LoadCatalogosAsync()
    {
        Pacientes = await _apiClient.GetAsync<List<PacienteCatalogoViewModel>>("api/pacientes") ?? new();
        Usuarios = await _apiClient.GetAsync<List<UsuarioCatalogoViewModel>>("api/catalogos/usuarios-activos") ?? new();
        Categorias = await _apiClient.GetAsync<List<CategoriaServicioCatalogoViewModel>>("api/catalogos/categorias-servicio") ?? new();
        Estados = await _apiClient.GetAsync<List<EstadoVentaCatalogoViewModel>>("api/catalogos/estados-venta") ?? new();
    }

    private async Task LoadVentasAsync()
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(Texto)) query.Add($"texto={Uri.EscapeDataString(Texto)}");
        if (IdPaciente.HasValue && IdPaciente.Value > 0) query.Add($"idPaciente={IdPaciente.Value}");
        if (IdUsuarioResponsable.HasValue && IdUsuarioResponsable.Value > 0) query.Add($"idUsuarioResponsable={IdUsuarioResponsable.Value}");
        if (IdCategoriaServicio.HasValue && IdCategoriaServicio.Value > 0) query.Add($"idCategoriaServicio={IdCategoriaServicio.Value}");
        if (IdEstadoVenta.HasValue && IdEstadoVenta.Value > 0) query.Add($"idEstadoVenta={IdEstadoVenta.Value}");
        if (FechaDesde.HasValue) query.Add($"fechaDesde={FechaDesde.Value:yyyy-MM-dd}");
        if (FechaHasta.HasValue) query.Add($"fechaHasta={FechaHasta.Value:yyyy-MM-dd}");

        var url = "api/ventas";
        if (query.Count > 0) url += "?" + string.Join("&", query);

        Ventas = await _apiClient.GetAsync<List<VentaServicioViewModel>>(url) ?? new();
    }
}
