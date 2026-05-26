namespace ClinicaDental.Api.DTOs;

public class DentistaListItemDto
{
    public int IdUsuario { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
}