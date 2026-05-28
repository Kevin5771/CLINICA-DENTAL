using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ClinicaDental.Web.Pages.CategoriasServicios;

public class CreateModel : PageModel
{
    private readonly ApiClient _apiClient;

    public CreateModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public CategoriaServicioFormModel Input { get; set; } = new() { Activo = true };

    public string? Message { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        NormalizeInput();

        if (!ModelState.IsValid)
        {
            Message = "Hay campos obligatorios pendientes o inválidos.";
            return Page();
        }

        var result = await _apiClient.PostAsync<CategoriaServicioViewModel>("api/categorias-servicio", Input);

        if (!result.Ok)
        {
            Message = $"Error al crear categoría: {GetFriendlyError(result.Error)}";
            return Page();
        }

        TempData["SuccessMessage"] = "Categoría de servicio creada correctamente.";
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
