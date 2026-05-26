using System.ComponentModel.DataAnnotations;

namespace ClinicaDental.Api.DTOs;

public sealed class UsuarioCreateDto
{
    [Required, StringLength(20, MinimumLength = 3)]
    public string CodigoUsuario { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 3)]
    public string Nombres { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 3)]
    public string Apellidos { get; set; } = string.Empty;

    [Required, StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [EmailAddress, StringLength(150)] public string? Correo { get; set; }
    [StringLength(20)] public string? Telefono { get; set; }

    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int IdRol { get; set; }
}

public sealed class UsuarioUpdateDto
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Nombres { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 3)]
    public string Apellidos { get; set; } = string.Empty;

    [Required, StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [EmailAddress, StringLength(150)] public string? Correo { get; set; }
    [StringLength(20)] public string? Telefono { get; set; }
    [Range(1, int.MaxValue)] public int IdRol { get; set; }
    public bool Activo { get; set; } = true;
}

public sealed class LoginDto
{
    [Required] public string Usuario { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}
