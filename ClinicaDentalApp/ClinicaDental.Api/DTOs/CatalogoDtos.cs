namespace ClinicaDental.Api.DTOs;

public sealed class TipoMovimientoCatalogoDto
{
    public int IdTipoMovimiento { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public sealed class CategoriaServicioCatalogoDto
{
    public int IdCategoriaServicio { get; set; }
    public string CodigoCategoria { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioBase { get; set; }
}

public sealed class EstadoVentaCatalogoDto
{
    public int IdEstadoVenta { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public sealed class MetodoPagoCatalogoDto
{
    public int IdMetodoPago { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public sealed class UsuarioCatalogoDto
{
    public int IdUsuario { get; set; }
    public string CodigoUsuario { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
}
