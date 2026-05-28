using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;

namespace ClinicaDental.Api.Repositories;

public interface ICatalogoRepository
{
    Task<IEnumerable<Rol>> GetRolesAsync();
    Task<IEnumerable<EstadoCita>> GetEstadosCitaAsync();
    Task<IEnumerable<ProveedorCatalogoDto>> GetProveedoresAsync();
    Task<IEnumerable<TipoMovimientoCatalogoDto>> GetTiposMovimientoInventarioAsync();
    Task<IEnumerable<CategoriaServicioCatalogoDto>> GetCategoriasServicioAsync();
    Task<IEnumerable<EstadoVentaCatalogoDto>> GetEstadosVentaAsync();
    Task<IEnumerable<MetodoPagoCatalogoDto>> GetMetodosPagoAsync();
    Task<IEnumerable<UsuarioCatalogoDto>> GetUsuariosActivosAsync();
}
