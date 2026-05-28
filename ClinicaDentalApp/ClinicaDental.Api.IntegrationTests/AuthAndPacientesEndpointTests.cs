using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;
using Xunit;

namespace ClinicaDental.Api.IntegrationTests;

public sealed class AuthAndPacientesEndpointTests : IClassFixture<ClinicaDentalApiFactory>
{
    private readonly ClinicaDentalApiFactory _factory;

    public AuthAndPacientesEndpointTests(ClinicaDentalApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task RootEndpoint_ReturnsApiHealthMessage()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("ClinicaDental API funcionando", body);
    }

    [Fact]
    public async Task PacientesEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/pacientes");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsJwtToken()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Usuario = "admin",
            Password = "Admin123"
        });

        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(auth);
        Assert.False(string.IsNullOrWhiteSpace(auth!.Token));
        Assert.Equal("admin", auth.Username);
        Assert.Equal("Administrador", auth.Rol);
    }

    [Fact]
    public async Task PacientesEndpoint_WithValidToken_ReturnsSeedPatients()
    {
        var client = _factory.CreateClient();
        await AuthenticateAsync(client);

        var response = await client.GetAsync("/api/pacientes");
        var patients = await response.Content.ReadFromJsonAsync<List<Paciente>>(JsonOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(patients);
        Assert.Single(patients!);
        Assert.Equal("PAC-001", patients![0].CodigoPaciente);
        Assert.Equal("Juan", patients[0].Nombres);
    }

    [Fact]
    public async Task CreatePaciente_WithValidTokenAndPayload_ReturnsCreated()
    {
        var client = _factory.CreateClient();
        await AuthenticateAsync(client);

        var payload = new PacienteCreateDto
        {
            CodigoPaciente = "PAC-002",
            Nombres = "Maria",
            Apellidos = "Lopez",
            Telefono = "5555-9876",
            FechaNacimiento = new DateTime(1998, 1, 20),
            Genero = "F",
            Correo = "maria.lopez@clinica.test"
        };

        var response = await client.PostAsJsonAsync("/api/pacientes", payload);
        var created = await response.Content.ReadFromJsonAsync<Paciente>(JsonOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(created);
        Assert.Equal("PAC-002", created!.CodigoPaciente);
        Assert.Equal("Maria", created.Nombres);
    }

    [Fact]
    public async Task CatalogosRoles_WithValidToken_ReturnsRoles()
    {
        var client = _factory.CreateClient();
        await AuthenticateAsync(client);

        var response = await client.GetAsync("/api/catalogos/roles");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Administrador", body);
        Assert.Contains("Dentista", body);
    }

    private static async Task AuthenticateAsync(HttpClient client)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Usuario = "admin",
            Password = "Admin123"
        });

        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);
        Assert.NotNull(auth);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);
    }

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };
}
