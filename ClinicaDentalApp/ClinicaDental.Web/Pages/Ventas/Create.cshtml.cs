using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ClinicaDental.Web.Pages.Ventas;

public class CreateModel : PageModel
{
    private readonly ApiClient _apiClient;

    public CreateModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public VentaServicioCreateModel Input { get; set; } = new();

    public List<PacienteCatalogoViewModel> Pacientes { get; set; } = new();
    public List<UsuarioCatalogoViewModel> Usuarios { get; set; } = new();
    public List<CategoriaServicioCatalogoViewModel> Categorias { get; set; } = new();
    public List<EstadoVentaCatalogoViewModel> Estados { get; set; } = new();
    public List<MetodoPagoCatalogoViewModel> MetodosPago { get; set; } = new();
    public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        await LoadCatalogosAsync();
        Input.FechaVenta = DateTime.Now;
        Input.IdMetodoPago = MetodosPago.FirstOrDefault(x => x.Nombre == "Efectivo")?.IdMetodoPago;
        Input.IdEstadoVenta = Estados.FirstOrDefault(x => x.Nombre == "Pagada")?.IdEstadoVenta;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadCatalogosAsync();

        if (!ModelState.IsValid)
        {
            Message = "Hay campos obligatorios pendientes o inválidos.";
            return Page();
        }

        var result = await _apiClient.PostAsync<VentaServicioViewModel>("api/ventas/servicio", Input);

        if (!result.Ok)
        {
            Message = $"Error al registrar venta: {GetFriendlyError(result.Error)}";
            return Page();
        }

        TempData["SuccessMessage"] = "Venta registrada correctamente.";
        return RedirectToPage("/Ventas/Index");
    }

    private async Task LoadCatalogosAsync()
    {
        Pacientes = await _apiClient.GetAsync<List<PacienteCatalogoViewModel>>("api/pacientes") ?? new();
        Usuarios = await _apiClient.GetAsync<List<UsuarioCatalogoViewModel>>("api/catalogos/usuarios-activos") ?? new();
        Categorias = await _apiClient.GetAsync<List<CategoriaServicioCatalogoViewModel>>("api/catalogos/categorias-servicio") ?? new();
        Estados = await _apiClient.GetAsync<List<EstadoVentaCatalogoViewModel>>("api/catalogos/estados-venta") ?? new();
        MetodosPago = await _apiClient.GetAsync<List<MetodoPagoCatalogoViewModel>>("api/catalogos/metodos-pago") ?? new();
    }

    private static string GetFriendlyError(string? error)
    {
        if (string.IsNullOrWhiteSpace(error)) return "No se recibió detalle del error.";

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
