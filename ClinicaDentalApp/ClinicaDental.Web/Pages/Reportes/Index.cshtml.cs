using System.Globalization;
using System.Text;
using ClinicaDental.Web.Models;
using ClinicaDental.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicaDental.Web.Pages.Reportes;

public class IndexModel : PageModel
{
    private readonly ApiClient _apiClient;

    private static readonly string[] TiposPermitidos =
    {
        "citas",
        "pacientes",
        "inventario",
        "ventas"
    };

    public IndexModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)]
    public ReporteFiltroModel Filtro { get; set; } = new();

    public string? ErrorMessage { get; set; }
    public ReporteResumenViewModel? Resumen { get; set; }

    public List<ReporteCitaViewModel> Citas { get; set; } = new();
    public List<ReportePacienteViewModel> Pacientes { get; set; } = new();
    public List<ReporteInventarioViewModel> Inventario { get; set; } = new();
    public List<ReporteVentaViewModel> Ventas { get; set; } = new();

    public List<ReportePacienteOption> PacientesCatalogo { get; set; } = new();
    public List<ReporteUsuarioOption> UsuariosCatalogo { get; set; } = new();
    public List<ReporteEstadoCitaOption> EstadosCita { get; set; } = new();
    public List<ReporteCategoriaServicioOption> CategoriasServicio { get; set; } = new();
    public List<ReporteEstadoVentaOption> EstadosVenta { get; set; } = new();
    public List<ReporteProveedorOption> Proveedores { get; set; } = new();

    public async Task OnGetAsync()
    {
        NormalizarFiltro();
        await CargarCatalogosAsync();

        if (!ValidarFiltro())
            return;

        await CargarResumenAsync();
        await CargarReporteAsync();
    }

    public async Task<IActionResult> OnGetExportarAsync()
    {
        NormalizarFiltro();

        if (!ValidarFiltro())
            return RedirectToPage("/Reportes/Index", Filtro);

        string csv;
        string nombre;

        switch (Filtro.TipoReporte)
        {
            case "citas":
                Citas = await _apiClient.GetAsync<List<ReporteCitaViewModel>>(UrlCitas()) ?? new();
                csv = CrearCsvCitas(Citas);
                nombre = "reporte_citas";
                break;

            case "pacientes":
                Pacientes = await _apiClient.GetAsync<List<ReportePacienteViewModel>>(UrlPacientes()) ?? new();
                csv = CrearCsvPacientes(Pacientes);
                nombre = "reporte_pacientes";
                break;

            case "inventario":
                Inventario = await _apiClient.GetAsync<List<ReporteInventarioViewModel>>(UrlInventario()) ?? new();
                csv = CrearCsvInventario(Inventario);
                nombre = "reporte_inventario";
                break;

            default:
                Ventas = await _apiClient.GetAsync<List<ReporteVentaViewModel>>(UrlVentas()) ?? new();
                csv = CrearCsvVentas(Ventas);
                nombre = "reporte_ventas";
                break;
        }

        var bytes = Encoding.UTF8.GetPreamble()
            .Concat(Encoding.UTF8.GetBytes(csv))
            .ToArray();

        return File(bytes, "text/csv; charset=utf-8", $"{nombre}_{DateTime.Now:yyyyMMdd_HHmm}.csv");
    }

    private async Task CargarReporteAsync()
    {
        switch (Filtro.TipoReporte)
        {
            case "citas":
                Citas = await _apiClient.GetAsync<List<ReporteCitaViewModel>>(UrlCitas()) ?? new();
                break;

            case "pacientes":
                Pacientes = await _apiClient.GetAsync<List<ReportePacienteViewModel>>(UrlPacientes()) ?? new();
                break;

            case "inventario":
                Inventario = await _apiClient.GetAsync<List<ReporteInventarioViewModel>>(UrlInventario()) ?? new();
                break;

            case "ventas":
                Ventas = await _apiClient.GetAsync<List<ReporteVentaViewModel>>(UrlVentas()) ?? new();
                break;
        }
    }

    private async Task CargarResumenAsync()
    {
        Resumen = await _apiClient.GetAsync<ReporteResumenViewModel>(
            ConstruirUrl("api/reportes/resumen", new Dictionary<string, string?>
            {
                ["fechaDesde"] = Fecha(Filtro.FechaDesde),
                ["fechaHasta"] = Fecha(Filtro.FechaHasta)
            }));
    }

    private async Task CargarCatalogosAsync()
    {
        PacientesCatalogo = await _apiClient.GetAsync<List<ReportePacienteOption>>("api/pacientes?activo=true") ?? new();
        UsuariosCatalogo = await _apiClient.GetAsync<List<ReporteUsuarioOption>>("api/catalogos/usuarios-activos") ?? new();
        EstadosCita = await _apiClient.GetAsync<List<ReporteEstadoCitaOption>>("api/catalogos/estados-cita") ?? new();
        CategoriasServicio = await _apiClient.GetAsync<List<ReporteCategoriaServicioOption>>("api/catalogos/categorias-servicio") ?? new();
        EstadosVenta = await _apiClient.GetAsync<List<ReporteEstadoVentaOption>>("api/catalogos/estados-venta") ?? new();
        Proveedores = await _apiClient.GetAsync<List<ReporteProveedorOption>>("api/catalogos/proveedores") ?? new();
    }

    private bool ValidarFiltro()
    {
        if (!TiposPermitidos.Contains(Filtro.TipoReporte))
        {
            ErrorMessage = "Debes seleccionar un tipo de reporte válido.";
            return false;
        }

        if (Filtro.FechaDesde.HasValue
            && Filtro.FechaHasta.HasValue
            && Filtro.FechaDesde.Value.Date > Filtro.FechaHasta.Value.Date)
        {
            ErrorMessage = "La fecha inicial no puede ser mayor que la fecha final.";
            return false;
        }

        return true;
    }

    private void NormalizarFiltro()
    {
        if (string.IsNullOrWhiteSpace(Filtro.TipoReporte))
            Filtro.TipoReporte = "ventas";

        Filtro.TipoReporte = Filtro.TipoReporte.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(Filtro.Texto))
            Filtro.Texto = null;

        if (string.IsNullOrWhiteSpace(Filtro.EstadoInventario))
            Filtro.EstadoInventario = null;

        if (Filtro.IdPaciente <= 0) Filtro.IdPaciente = null;
        if (Filtro.IdUsuarioResponsable <= 0) Filtro.IdUsuarioResponsable = null;
        if (Filtro.IdEstadoCita <= 0) Filtro.IdEstadoCita = null;
        if (Filtro.IdCategoriaServicio <= 0) Filtro.IdCategoriaServicio = null;
        if (Filtro.IdEstadoVenta <= 0) Filtro.IdEstadoVenta = null;
        if (Filtro.IdProveedor <= 0) Filtro.IdProveedor = null;
    }

    private string UrlCitas()
    {
        return ConstruirUrl("api/reportes/citas", new Dictionary<string, string?>
        {
            ["fechaDesde"] = Fecha(Filtro.FechaDesde),
            ["fechaHasta"] = Fecha(Filtro.FechaHasta),
            ["idPaciente"] = Entero(Filtro.IdPaciente),
            ["idDentista"] = Entero(Filtro.IdUsuarioResponsable),
            ["idEstadoCita"] = Entero(Filtro.IdEstadoCita)
        });
    }

    private string UrlPacientes()
    {
        return ConstruirUrl("api/reportes/pacientes", new Dictionary<string, string?>
        {
            ["texto"] = Filtro.Texto,
            ["activo"] = Booleano(Filtro.Activo)
        });
    }

    private string UrlInventario()
    {
        return ConstruirUrl("api/reportes/inventario", new Dictionary<string, string?>
        {
            ["texto"] = Filtro.Texto,
            ["estado"] = Filtro.EstadoInventario,
            ["idProveedor"] = Entero(Filtro.IdProveedor)
        });
    }

    private string UrlVentas()
    {
        return ConstruirUrl("api/reportes/ventas", new Dictionary<string, string?>
        {
            ["texto"] = Filtro.Texto,
            ["fechaDesde"] = Fecha(Filtro.FechaDesde),
            ["fechaHasta"] = Fecha(Filtro.FechaHasta),
            ["idPaciente"] = Entero(Filtro.IdPaciente),
            ["idUsuarioResponsable"] = Entero(Filtro.IdUsuarioResponsable),
            ["idCategoriaServicio"] = Entero(Filtro.IdCategoriaServicio),
            ["idEstadoVenta"] = Entero(Filtro.IdEstadoVenta)
        });
    }

    private static string ConstruirUrl(string ruta, Dictionary<string, string?> parametros)
    {
        var query = parametros
            .Where(x => !string.IsNullOrWhiteSpace(x.Value))
            .Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value!)}")
            .ToList();

        return query.Count == 0
            ? ruta
            : $"{ruta}?{string.Join("&", query)}";
    }

    private static string? Fecha(DateTime? fecha)
    {
        return fecha?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    private static string? Entero(int? valor)
    {
        return valor.HasValue && valor.Value > 0
            ? valor.Value.ToString(CultureInfo.InvariantCulture)
            : null;
    }

    private static string? Booleano(bool? valor)
    {
        return valor.HasValue
            ? valor.Value.ToString().ToLowerInvariant()
            : null;
    }

    private static string CrearCsvCitas(IEnumerable<ReporteCitaViewModel> datos)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Fecha,Hora inicio,Hora fin,Paciente,Dentista,Estado,Motivo,Observaciones");

        foreach (var item in datos)
        {
            sb.AppendLine(string.Join(",",
                Csv(item.Fecha.ToString("yyyy-MM-dd")),
                Csv(item.HoraInicio.ToString(@"hh\:mm")),
                Csv(item.HoraFin.ToString(@"hh\:mm")),
                Csv(item.Paciente),
                Csv(item.Dentista),
                Csv(item.EstadoCita),
                Csv(item.Motivo),
                Csv(item.Observaciones)));
        }

        return sb.ToString();
    }

    private static string CrearCsvPacientes(IEnumerable<ReportePacienteViewModel> datos)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Codigo,Paciente,Telefono,Correo,Genero,Fecha nacimiento,Estado,Total citas,Total ventas");

        foreach (var item in datos)
        {
            sb.AppendLine(string.Join(",",
                Csv(item.CodigoPaciente),
                Csv(item.Paciente),
                Csv(item.Telefono),
                Csv(item.Correo),
                Csv(item.Genero),
                Csv(item.FechaNacimiento.ToString("yyyy-MM-dd")),
                Csv(item.Activo ? "Activo" : "Inactivo"),
                Csv(item.TotalCitas),
                Csv(item.TotalVentas)));
        }

        return sb.ToString();
    }

    private static string CrearCsvInventario(IEnumerable<ReporteInventarioViewModel> datos)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Codigo,Producto,Proveedor,Stock actual,Stock minimo,Unidad,Fecha vencimiento,Estado,Costo unitario,Precio venta,Valor inventario");

        foreach (var item in datos)
        {
            sb.AppendLine(string.Join(",",
                Csv(item.CodigoProducto),
                Csv(item.Producto),
                Csv(item.Proveedor),
                Csv(item.StockActual),
                Csv(item.StockMinimo),
                Csv(item.UnidadMedida),
                Csv(item.FechaVencimiento?.ToString("yyyy-MM-dd")),
                Csv(item.EstadoProducto),
                Csv(item.CostoUnitario),
                Csv(item.PrecioVenta),
                Csv(item.ValorInventario)));
        }

        return sb.ToString();
    }

    private static string CrearCsvVentas(IEnumerable<ReporteVentaViewModel> datos)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Numero venta,Fecha,Paciente,Personal,Categoria,Precio,Total,Estado,Metodo pago,Descripcion");

        foreach (var item in datos)
        {
            sb.AppendLine(string.Join(",",
                Csv(item.NumeroVenta),
                Csv(item.FechaVenta.ToString("yyyy-MM-dd HH:mm")),
                Csv(item.Paciente),
                Csv(item.UsuarioResponsable),
                Csv(item.CategoriaServicio),
                Csv(item.Precio),
                Csv(item.Total),
                Csv(item.EstadoVenta),
                Csv(item.MetodoPago),
                Csv(item.Descripcion)));
        }

        return sb.ToString();
    }

    private static string Csv(object? valor)
    {
        var texto = valor switch
        {
            null => "",
            decimal d => d.ToString("0.00", CultureInfo.InvariantCulture),
            double d => d.ToString("0.00", CultureInfo.InvariantCulture),
            float f => f.ToString("0.00", CultureInfo.InvariantCulture),
            _ => valor.ToString() ?? ""
        };

        return $"\"{texto.Replace("\"", "\"\"")}\"";
    }
}