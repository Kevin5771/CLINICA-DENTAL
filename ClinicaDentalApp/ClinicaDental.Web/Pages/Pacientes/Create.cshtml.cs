using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Pacientes;

public class CreateModel : PageModel
{
    private readonly ApiClient _apiClient;

    public CreateModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public PacienteCreateModel Input { get; set; } = new();

    public string? Message { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Message = "Hay campos obligatorios pendientes o inválidos.";
            return Page();
        }

        var result = await _apiClient.PostAsync<PacienteViewModel>("api/pacientes", Input);

        if (!result.Ok)
        {
            Message = $"Error al guardar paciente: {result.Error}";
            return Page();
        }

        TempData["SuccessMessage"] = "Paciente guardado correctamente.";
        return RedirectToPage("/Pacientes/Index");
    }
}