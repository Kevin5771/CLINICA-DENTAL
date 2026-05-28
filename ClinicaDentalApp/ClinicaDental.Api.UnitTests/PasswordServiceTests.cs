using ClinicaDental.Api.Services;
using Xunit;

namespace ClinicaDental.Api.UnitTests;

public sealed class PasswordServiceTests
{
    private readonly PasswordService _service = new();

    [Fact]
    public void HashPassword_WhenPasswordIsValid_GeneratesHashAndSalt()
    {
        var result = _service.HashPassword("Admin123");

        Assert.False(string.IsNullOrWhiteSpace(result.Hash));
        Assert.False(string.IsNullOrWhiteSpace(result.Salt));
        Assert.NotEqual("Admin123", result.Hash);
    }

    [Fact]
    public void VerifyPassword_WhenPasswordMatches_ReturnsTrue()
    {
        var result = _service.HashPassword("Admin123");

        var isValid = _service.VerifyPassword("Admin123", result.Hash, result.Salt);

        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_WhenPasswordDoesNotMatch_ReturnsFalse()
    {
        var result = _service.HashPassword("Admin123");

        var isValid = _service.VerifyPassword("Incorrecta123", result.Hash, result.Salt);

        Assert.False(isValid);
    }

    [Theory]
    [InlineData("", "salt")]
    [InlineData("hash", "")]
    [InlineData("", "")]
    public void VerifyPassword_WhenHashOrSaltIsMissing_ReturnsFalse(string hash, string salt)
    {
        var isValid = _service.VerifyPassword("Admin123", hash, salt);

        Assert.False(isValid);
    }
}
