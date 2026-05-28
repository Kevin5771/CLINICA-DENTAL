using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ClinicaDental.Web.E2ETests;

public sealed class LoginFlowE2ETests : IClassFixture<ClinicaDentalWebE2EFactory>
{
    private readonly ClinicaDentalWebE2EFactory _factory;

    public LoginFlowE2ETests(ClinicaDentalWebE2EFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AnonymousUser_WhenOpeningHome_IsRedirectedToLogin()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync("/");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/Auth/Login", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task Login_WithValidCredentials_AllowsUserToOpenPacientesPage()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });

        var token = await GetAntiForgeryTokenAsync(client);
        var loginResponse = await client.PostAsync("/Auth/Login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Input.Usuario"] = "admin",
            ["Input.Password"] = "Admin123",
            ["__RequestVerificationToken"] = token
        }));

        Assert.Equal(HttpStatusCode.Redirect, loginResponse.StatusCode);

        var pacientesResponse = await client.GetAsync("/Pacientes");
        var html = await pacientesResponse.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, pacientesResponse.StatusCode);
        Assert.Contains("Pacientes", html);
        Assert.Contains("PAC-001", html);
        Assert.Contains("Juan", html);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_StaysOnLoginAndShowsError()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });

        var token = await GetAntiForgeryTokenAsync(client);
        var loginResponse = await client.PostAsync("/Auth/Login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Input.Usuario"] = "admin",
            ["Input.Password"] = "Mala123",
            ["__RequestVerificationToken"] = token
        }));

        var html = await loginResponse.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.Contains("Login", html, StringComparison.OrdinalIgnoreCase);
                Assert.Contains("Input.Usuario", html, StringComparison.OrdinalIgnoreCase);
                Assert.Contains("Input.Password", html, StringComparison.OrdinalIgnoreCase);
    }

    private static async Task<string> GetAntiForgeryTokenAsync(HttpClient client)
    {
        var loginPage = await client.GetStringAsync("/Auth/Login");
        var token = ExtractAntiForgeryToken(loginPage);
        Assert.False(string.IsNullOrWhiteSpace(token));
        return token;
    }

    private static string ExtractAntiForgeryToken(string html)
    {
        var match = Regex.Match(
            html,
            "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        if (!match.Success)
        {
            match = Regex.Match(
                html,
                "value=\"([^\"]+)\"[^>]*name=\"__RequestVerificationToken\"",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        return match.Success ? WebUtility.HtmlDecode(match.Groups[1].Value) : string.Empty;
    }
}
