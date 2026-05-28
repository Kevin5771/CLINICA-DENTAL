using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ClinicaDental.Web.Pages.Inventario;

public class ComprasModel : PageModel
{
    private readonly ApiClient _apiClient;

    public ComprasModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public CompraInventarioCreateModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Texto { get; set; }

    public List<StockProductoViewModel> Productos { get; set; } = new();
    public List<MovimientoInventarioViewModel> Movimientos { get; set; } = new();
    public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        await LoadDataAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadProductosAsync();

        if (!ModelState.IsValid)
        {
            Message = "Hay campos obligatorios pendientes o inválidos.";
            await LoadMovimientosAsync();
            return Page();
        }

        var result = await _apiClient.PostAsync<MovimientoInventarioViewModel>("api/stock/compras", Input);

        if (!result.Ok)
        {
            Message = $"Error al registrar la compra: {GetFriendlyError(result.Error)}";
            await LoadMovimientosAsync();
            return Page();
        }

        TempData["SuccessMessage"] = "Compra registrada correctamente. El stock del producto fue actualizado.";
        return RedirectToPage("/Inventario/Compras");
    }

    private async Task LoadDataAsync()
    {
        await LoadProductosAsync();
        await LoadMovimientosAsync();
    }

    private async Task LoadProductosAsync()
    {
        Productos = await _apiClient.GetAsync<List<StockProductoViewModel>>("api/stock") ?? new();
    }

    private async Task LoadMovimientosAsync()
    {
        var url = "api/stock/movimientos";

        if (!string.IsNullOrWhiteSpace(Texto))
        {
            url += $"?texto={Uri.EscapeDataString(Texto)}";
        }

        Movimientos = await _apiClient.GetAsync<List<MovimientoInventarioViewModel>>(url) ?? new();
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
