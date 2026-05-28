using System.Net;
using System.Text;
using System.Text.Json;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace ClinicaDental.Web.E2ETests;

public sealed class ClinicaDentalWebE2EFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ApiSettings:BaseUrl"] = "http://fake-api/"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.Configure<HttpClientFactoryOptions>(nameof(ApiClient), options =>
            {
                options.HttpMessageHandlerBuilderActions.Add(messageHandlerBuilder =>
                {
                    messageHandlerBuilder.PrimaryHandler = new FakeApiMessageHandler();
                });
            });
        });
    }

    private sealed class FakeApiMessageHandler : HttpMessageHandler
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.AbsolutePath.Trim('/').ToLowerInvariant() ?? string.Empty;

            if (request.Method == HttpMethod.Post && path == "api/auth/login")
            {
                var body = request.Content is null
                    ? string.Empty
                    : await request.Content.ReadAsStringAsync(cancellationToken);

                if (body.Contains("admin", StringComparison.OrdinalIgnoreCase) &&
                    body.Contains("Admin123", StringComparison.Ordinal))
                {
                    return JsonResponse(HttpStatusCode.OK, new
                    {
                        token = "fake-jwt-token-for-e2e-tests",
                        username = "admin",
                        nombreCompleto = "Admin Pruebas",
                        rol = "Administrador",
                        idUsuario = 1,
                        expiraEn = DateTime.UtcNow.AddHours(8)
                    });
                }

                return JsonResponse(HttpStatusCode.Unauthorized, new { message = "Credenciales inválidas." });
            }

            if (request.Method == HttpMethod.Get && path == "api/pacientes")
            {
                return JsonResponse(HttpStatusCode.OK, new[]
                {
                    new
                    {
                        idPaciente = 1,
                        codigoPaciente = "PAC-001",
                        nombres = "Juan",
                        apellidos = "Pérez",
                        telefono = "5555-1234",
                        fechaNacimiento = new DateTime(1995, 5, 10),
                        genero = "M",
                        direccion = "Guatemala",
                        correo = "juan.perez@clinica.test",
                        activo = true,
                        creadoEn = DateTime.UtcNow
                    }
                });
            }

            if (request.Method == HttpMethod.Get && path == "api/citas")
            {
                return JsonResponse(HttpStatusCode.OK, new[]
                {
                    new
                    {
                        idCita = 1,
                        idPaciente = 1,
                        paciente = "Juan Pérez",
                        idDentista = 1,
                        dentista = "Dra. Ana López",
                        fecha = DateTime.Today.AddDays(1),
                        horaInicio = "09:00:00",
                        horaFin = "10:00:00",
                        motivo = "Limpieza dental",
                        observaciones = "Prueba E2E",
                        idEstadoCita = 1,
                        estadoCita = "Programada"
                    }
                });
            }

            if (request.Method == HttpMethod.Get && path == "api/usuarios/dentistas")
            {
                return JsonResponse(HttpStatusCode.OK, new[]
                {
                    new { idUsuario = 1, nombres = "Ana", apellidos = "López" }
                });
            }

            return JsonResponse(HttpStatusCode.NotFound, new { message = $"Ruta fake no configurada: {path}" });
        }

        private static HttpResponseMessage JsonResponse(HttpStatusCode statusCode, object payload)
        {
            var json = JsonSerializer.Serialize(payload, JsonOptions);
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
    }
}
