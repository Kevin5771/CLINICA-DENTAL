using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ClinicaDental.Web.Pages.CategoriasServicios;

public class EditModel : PageModel
{
    private readonly ApiClient _apiClient;

    public EditModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public CategoriaServicioFormModel Input { get; set; } = new();

    public string? Message { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var categoria = await _apiClient.GetAsync<CategoriaServicioViewModel>($"api/categorias-servicio/{Id}");
        if (categoria is null)
        {
            TempData["ErrorMessage"] = "No se encontró la categoría de servicio solicitada.";
            return RedirectToPage("/CategoriasServicios/Index");
        }

        Input = new CategoriaServicioFormModel
        {
            CodigoCategoria = categoria.CodigoCategoria,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion,
            PrecioBase = categoria.PrecioBase,
            Activo = categoria.Activo
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        NormalizeInput();

        if (!ModelState.IsValid)
        {
            Message = "Hay campos obligatorios pendientes o inválidos.";
            return Page();
        }

        var result = await _apiClient.PutAsync<CategoriaServicioViewModel>($"api/categorias-servicio/{Id}", Input);

        if (!result.Ok)
        {
            Message = $"Error al actualizar categoría: {GetFriendlyError(result.Error)}";
            return Page();
        }

        TempData["SuccessMessage"] = "Categoría de servicio actualizada correctamente.";
        return RedirectToPage("/CategoriasServicios/Index");
    }

    private void NormalizeInput()
    {
        Input.CodigoCategoria = (Input.CodigoCategoria ?? string.Empty).Trim().ToUpperInvariant();
        Input.Nombre = (Input.Nombre ?? string.Empty).Trim();
        Input.Descripcion = string.IsNullOrWhiteSpace(Input.Descripcion) ? null : Input.Descripcion.Trim();
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
