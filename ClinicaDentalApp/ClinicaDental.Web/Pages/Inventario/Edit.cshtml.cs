using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ClinicaDental.Web.Pages.Inventario;

public class EditModel : PageModel
{
    private readonly ApiClient _apiClient;

    public EditModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public ProductoEditModel Input { get; set; } = new();

    public List<ProveedorCatalogoViewModel> Proveedores { get; set; } = new();

    public string? Message { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        await LoadProveedoresAsync();

        var producto = await _apiClient.GetAsync<StockProductoViewModel>($"api/stock/{id}");
        if (producto is null)
        {
            TempData["ErrorMessage"] = "No se encontró el producto seleccionado.";
            return RedirectToPage("/Inventario/Index");
        }

        Input = new ProductoEditModel
        {
            IdProducto = producto.IdProducto,
            CodigoProducto = producto.CodigoProducto,
            Nombre = producto.Nombre,
            IdProveedor = producto.IdProveedor ?? 0,
            CantidadDisponible = producto.CantidadDisponible,
            EsPerecedero = producto.FechaVencimiento.HasValue,
            FechaVencimiento = producto.FechaVencimiento,
            Descripcion = producto.Descripcion,
            StockMinimo = producto.StockMinimo,
            UnidadMedida = string.IsNullOrWhiteSpace(producto.UnidadMedida) ? "unidad" : producto.UnidadMedida,
            CostoUnitario = producto.CostoUnitario,
            PrecioVenta = producto.PrecioVenta,
            Activo = producto.Activo
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await LoadProveedoresAsync();

        Input.IdProducto = id > 0 ? id : Input.IdProducto;

        if (!ModelState.IsValid)
        {
            Message = "Hay campos obligatorios pendientes o inválidos.";
            return Page();
        }

        if (!Input.EsPerecedero)
        {
            Input.FechaVencimiento = null;
        }

        var result = await _apiClient.PutAsync<StockProductoViewModel>($"api/stock/{Input.IdProducto}", Input);

        if (!result.Ok)
        {
            Message = $"Error al actualizar producto: {GetFriendlyError(result.Error)}";
            return Page();
        }

        TempData["SuccessMessage"] = "Producto actualizado correctamente.";
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

            if (doc.RootElement.TryGetProperty("title", out var title))
                return title.GetString() ?? error;
        }
        catch
        {
            return error;
        }

        return error;
    }
}
