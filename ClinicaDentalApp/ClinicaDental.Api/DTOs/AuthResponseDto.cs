namespace ClinicaDental.Api.DTOs;

public sealed class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public DateTime ExpiraEn { get; set; }
    public int IdUsuario { get; set; }
}
