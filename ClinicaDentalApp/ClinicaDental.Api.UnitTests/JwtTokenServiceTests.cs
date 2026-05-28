using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ClinicaDental.Api.Infrastructure;
using ClinicaDental.Api.Models;
using ClinicaDental.Api.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace ClinicaDental.Api.UnitTests;

public sealed class JwtTokenServiceTests
{
    [Fact]
    public void GenerateToken_WhenUserIsValid_ReturnsTokenWithExpectedClaims()
    {
        var settings = Options.Create(new JwtSettings
        {
            Key = "TEST_KEY_SUPER_SECRETA_PARA_PRUEBAS_2026_CLINICA_DENTAL",
            Issuer = "ClinicaDental.Api.Tests",
            Audience = "ClinicaDental.Web.Tests",
            ExpirationMinutes = 60
        });

        var service = new JwtTokenService(settings);
        var user = new Usuario
        {
            IdUsuario = 7,
            CodigoUsuario = "USR-007",
            Username = "admin",
            Nombres = "Admin",
            Apellidos = "Pruebas",
            Rol = "Administrador"
        };

        var response = service.GenerateToken(user);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(response.Token);

        Assert.False(string.IsNullOrWhiteSpace(response.Token));
        Assert.Equal("admin", response.Username);
        Assert.Equal("Admin Pruebas", response.NombreCompleto);
        Assert.Equal("Administrador", response.Rol);
        Assert.Equal(7, response.IdUsuario);
        Assert.Equal("ClinicaDental.Api.Tests", token.Issuer);
        Assert.Contains(token.Audiences, audience => audience == "ClinicaDental.Web.Tests");
        Assert.Contains(token.Claims, claim =>
            (claim.Type == ClaimTypes.NameIdentifier || claim.Type == "nameid" || claim.Type == JwtRegisteredClaimNames.Sub) &&
            claim.Value == "7");
        Assert.Contains(token.Claims, claim =>
            (claim.Type == ClaimTypes.Role || claim.Type == "role") &&
            claim.Value == "Administrador");
        Assert.Contains(token.Claims, claim => claim.Type == "codigo_usuario" && claim.Value == "USR-007");
    }
}
