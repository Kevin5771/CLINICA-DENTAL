namespace ClinicaDental.Api.DTOs;

public sealed class StockResumenDto
{
    public int TotalProductos { get; set; }
    public int Disponibles { get; set; }
    public int BajoStock { get; set; }
    public int Agotados { get; set; }
    public int Vencidos { get; set; }
    public int ProximosVencer { get; set; }
}