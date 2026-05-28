using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LoginModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public LoginInputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        // Si llega al login, limpiamos cualquier sesión vieja o inconsistente
        // para evitar bucles de redirección.
        if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("JWToken")) &&
            string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Usuario")))
        {
            HttpContext.Session.Clear();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var client = _httpClientFactory.CreateClient(nameof(ApiClient));

            var payload = new
            {
                usuario = Input.Usuario,
                password = Input.Password
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/auth/login", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return Page();
            }

            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (loginResponse is null || string.IsNullOrWhiteSpace(loginResponse.Token))
            {
                ErrorMessage = "No se recibió un token válido desde la API.";
                return Page();
            }

            HttpContext.Session.Clear();

            HttpContext.Session.SetString("JWToken", loginResponse.Token);
            HttpContext.Session.SetString("Usuario", loginResponse.Username ?? Input.Usuario);
            HttpContext.Session.SetString("NombreCompleto", loginResponse.NombreCompleto ?? Input.Usuario);
            HttpContext.Session.SetString("Rol", loginResponse.Rol ?? string.Empty);
            HttpContext.Session.SetInt32("id_usuario", loginResponse.IdUsuario);

            return RedirectToPage("/Index");
        }
        catch
        {
            ErrorMessage = "No fue posible conectar con la API.";
            return Page();
        }
    }

    public class LoginInputModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Rol { get; set; }
        public int IdUsuario { get; set; }
    }
}