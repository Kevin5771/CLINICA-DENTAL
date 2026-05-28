using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ClinicaDental.Web.Pages.Inventario;

public class CreateModel : PageModel
{
    private readonly ApiClient _apiClient;

    public CreateModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public ProductoCreateModel Input { get; set; } = new();

    public List<ProveedorCatalogoViewModel> Proveedores { get; set; } = new();

    public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        await LoadProveedoresAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadProveedoresAsync();

        if (!ModelState.IsValid)
        {
            Message = "Hay campos obligatorios pendientes o inválidos.";
            return Page();
        }

        var result = await _apiClient.PostAsync<StockProductoViewModel>("api/stock", Input);

        if (!result.Ok)
        {
            Message = $"Error al guardar producto: {GetFriendlyError(result.Error)}";
            return Page();
        }

        TempData["SuccessMessage"] = "Producto registrado correctamente.";
        return RedirectToPage("/Inventario/Index");
    }

    private async Task LoadProveedoresAsync()
    {
        Proveedores = await _apiClient.GetAsync<List<ProveedorCatalogoViewModel>>("api/catalogos/proveedores") ?? new();
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
