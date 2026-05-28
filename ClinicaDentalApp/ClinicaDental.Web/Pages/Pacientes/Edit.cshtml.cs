using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Pacientes;

public class EditModel : PageModel
{
    private readonly ApiClient _apiClient;

    public EditModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public PacienteUpdateModel Input { get; set; } = new();

    public string? Message { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var paciente = await _apiClient.GetAsync<PacienteViewModel>($"api/pacientes/{id}");
        if (paciente is null)
        {
            return RedirectToPage("/Pacientes/Index");
        }

        Input = new PacienteUpdateModel
        {
            IdPaciente = paciente.IdPaciente,
            CodigoPaciente = paciente.CodigoPaciente,
            Nombres = paciente.Nombres,
            Apellidos = paciente.Apellidos,
            Telefono = paciente.Telefono,
            FechaNacimiento = paciente.FechaNacimiento,
            Genero = paciente.Genero,
            Direccion = paciente.Direccion,
            Correo = paciente.Correo,
            Alergias = paciente.Alergias,
            ObservacionesGenerales = paciente.ObservacionesGenerales,
            Activo = paciente.Activo
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var pacienteActual = await _apiClient.GetAsync<PacienteViewModel>($"api/pacientes/{Input.IdPaciente}");

        if (pacienteActual is null)
        {
            Message = "No se encontró el paciente a editar.";
            return Page();
        }

        if (string.IsNullOrWhiteSpace(Input.CodigoPaciente))
        {
            Input.CodigoPaciente = pacienteActual.CodigoPaciente;
        }

        if (Input.FechaNacimiento.Date > DateTime.Today)
            ModelState.AddModelError("Input.FechaNacimiento", "La fecha de nacimiento no puede ser futura.");

        if (Input.EdadCalculada < 0)
            ModelState.AddModelError("Input.FechaNacimiento", "La edad debe ser coherente con la fecha de nacimiento.");

        if (!ModelState.IsValid)
        {
            Message = "Verifica los campos obligatorios antes de continuar.";
            return Page();
        }

        var payload = new
        {
            codigoPaciente = Input.CodigoPaciente,
            nombres = Input.Nombres,
            apellidos = Input.Apellidos,
            telefono = Input.Telefono,
            fechaNacimiento = Input.FechaNacimiento.ToString("yyyy-MM-dd"),
            genero = Input.Genero,
            direccion = Input.Direccion,
            correo = Input.Correo,
            alergias = Input.Alergias,
            observacionesGenerales = Input.ObservacionesGenerales,
            activo = Input.Activo
        };

        var result = await _apiClient.PutAsync<PacienteViewModel>($"api/pacientes/{Input.IdPaciente}", payload);
        if (!result.Ok)
        {
            Message = $"Error al actualizar paciente: {result.Error}";
            return Page();
        }

        TempData["SuccessMessage"] = "Paciente actualizado correctamente.";
        return RedirectToPage("/Pacientes/Index");
    }
}