using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ClinicaDental.Web.Pages.Inventario;

public class MovimientosModel : PageModel
{
    private readonly ApiClient _apiClient;

    public MovimientosModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public MovimientoInventarioCreateModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Texto { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? IdProducto { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? TipoMovimiento { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? FechaDesde { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? FechaHasta { get; set; }

    public List<StockProductoViewModel> Productos { get; set; } = new();
    public List<TipoMovimientoCatalogoViewModel> TiposMovimiento { get; set; } = new();
    public List<MovimientoInventarioViewModel> Movimientos { get; set; } = new();
    public string? Message { get; set; }
    public string UsuarioActual { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        UsuarioActual = ObtenerNombreUsuario();
        await LoadDataAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        UsuarioActual = ObtenerNombreUsuario();
        await LoadCatalogosAsync();

        if (!ModelState.IsValid)
        {
            Message = "Hay campos obligatorios pendientes o inválidos.";
            await LoadHistorialAsync();
            return Page();
        }

        var result = await _apiClient.PostAsync<MovimientoInventarioViewModel>("api/stock/movimientos", Input);

        if (!result.Ok)
        {
            Message = $"Error al registrar movimiento: {GetFriendlyError(result.Error)}";
            await LoadHistorialAsync();
            return Page();
        }

        TempData["SuccessMessage"] = "Movimiento registrado correctamente. El stock fue actualizado.";
        return RedirectToPage("/Inventario/Movimientos");
    }

    private async Task LoadDataAsync()
    {
        await LoadCatalogosAsync();
        await LoadHistorialAsync();
    }

    private async Task LoadCatalogosAsync()
    {
        Productos = await _apiClient.GetAsync<List<StockProductoViewModel>>("api/stock") ?? new();
        TiposMovimiento = await _apiClient.GetAsync<List<TipoMovimientoCatalogoViewModel>>("api/catalogos/tipos-movimiento-inventario") ?? new();
    }

    private async Task LoadHistorialAsync()
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(Texto))
            query.Add($"texto={Uri.EscapeDataString(Texto)}");

        if (IdProducto.HasValue && IdProducto.Value > 0)
            query.Add($"idProducto={IdProducto.Value}");

        if (!string.IsNullOrWhiteSpace(TipoMovimiento))
            query.Add($"tipoMovimiento={Uri.EscapeDataString(TipoMovimiento)}");

        if (FechaDesde.HasValue)
            query.Add($"fechaDesde={FechaDesde.Value:yyyy-MM-dd}");

        if (FechaHasta.HasValue)
            query.Add($"fechaHasta={FechaHasta.Value:yyyy-MM-dd}");

        var url = "api/stock/historial-movimientos";
        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        Movimientos = await _apiClient.GetAsync<List<MovimientoInventarioViewModel>>(url) ?? new();
    }

    private string ObtenerNombreUsuario()
    {
        return HttpContext.Session.GetString("NombreCompleto")
            ?? HttpContext.Session.GetString("Usuario")
            ?? "Usuario actual";
    }

    private static string GetFriendlyError(string? error)
    {
        if (string.IsNullOrWhiteSpace(error))
            return "No se recibió detalle del error.";

        try
        {
            using var doc = JsonDocument.Parse(error);
            if (doc.RootElement.TryGetProperty("message", out var message))
                return message.GetString() ?? error;
        }
        catch
        {
            return error;
        }

        return error;
    }
}
