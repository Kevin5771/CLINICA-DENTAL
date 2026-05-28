using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;
using ClinicaDental.Api.Repositories;
using ClinicaDental.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ClinicaDental.Api.IntegrationTests;

public sealed class ClinicaDentalApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IUsuarioRepository>();
            services.RemoveAll<IPacienteRepository>();
            services.RemoveAll<ICatalogoRepository>();

            services.AddScoped<IUsuarioRepository, FakeUsuarioRepository>();
            services.AddScoped<IPacienteRepository, FakePacienteRepository>();
            services.AddScoped<ICatalogoRepository, FakeCatalogoRepository>();
        });
    }

    private sealed class FakeUsuarioRepository : IUsuarioRepository
    {
        private readonly IPasswordService _passwordService;

        public FakeUsuarioRepository(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public Task<IEnumerable<Usuario>> ListAsync(string? texto, bool? activo)
            => Task.FromResult<IEnumerable<Usuario>>(new[] { BuildAdminUser() });

        public Task<IEnumerable<DentistaListItemDto>> ListDentistasAsync()
            => Task.FromResult<IEnumerable<DentistaListItemDto>>(new[]
            {
                new DentistaListItemDto { IdUsuario = 1, Nombres = "Ana", Apellidos = "López" }
            });

        public Task<Usuario?> GetByIdAsync(int id)
            => Task.FromResult<Usuario?>(id == 1 ? BuildAdminUser() : null);

        public Task<Usuario> CreateAsync(UsuarioCreateDto dto, string passwordHash, string salt)
            => Task.FromResult(BuildAdminUser());

        public Task<Usuario?> UpdateAsync(int id, UsuarioUpdateDto dto)
            => Task.FromResult<Usuario?>(id == 1 ? BuildAdminUser() : null);

        public Task<Usuario?> DeactivateAsync(int id)
            => Task.FromResult<Usuario?>(id == 1 ? BuildInactiveAdminUser() : null);

        public Task<Usuario?> GetByUsernameOrCodeAsync(string usuario)
        {
            if (!string.Equals(usuario, "admin", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(usuario, "USR-001", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult<Usuario?>(null);
            }

            return Task.FromResult<Usuario?>(BuildAdminUser());
        }

        private Usuario BuildInactiveAdminUser()
        {
            var user = BuildAdminUser();
            user.Activo = false;
            return user;
        }

        private Usuario BuildAdminUser()
        {
            var (hash, salt) = _passwordService.HashPassword("Admin123");
            return new Usuario
            {
                IdUsuario = 1,
                CodigoUsuario = "USR-001",
                Nombres = "Admin",
                Apellidos = "Pruebas",
                Username = "admin",
                Correo = "admin@clinica.test",
                Telefono = "5555-0000",
                PasswordHash = hash,
                PasswordSalt = salt,
                IdRol = 1,
                Rol = "Administrador",
                Activo = true,
                CreadoEn = DateTime.UtcNow
            };
        }
    }

    private sealed class FakePacienteRepository : IPacienteRepository
    {
        private static readonly Paciente SeedPaciente = new()
        {
            IdPaciente = 1,
            CodigoPaciente = "PAC-001",
            Nombres = "Juan",
            Apellidos = "Pérez",
            Telefono = "5555-1234",
            FechaNacimiento = new DateTime(1995, 5, 10),
            Genero = "M",
            Direccion = "Guatemala",
            Correo = "juan.perez@clinica.test",
            Activo = true,
            CreadoEn = DateTime.UtcNow
        };

        public Task<IEnumerable<Paciente>> ListAsync(string? texto, bool? activo)
            => Task.FromResult<IEnumerable<Paciente>>(new[] { SeedPaciente });

        public Task<Paciente?> GetByIdAsync(int id)
            => Task.FromResult<Paciente?>(id == 1 ? SeedPaciente : null);

        public Task<Paciente> CreateAsync(PacienteCreateDto dto)
            => Task.FromResult(new Paciente
            {
                IdPaciente = 2,
                CodigoPaciente = dto.CodigoPaciente,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Telefono = dto.Telefono,
                FechaNacimiento = dto.FechaNacimiento,
                Genero = dto.Genero,
                Direccion = dto.Direccion,
                Correo = dto.Correo,
                Alergias = dto.Alergias,
                ObservacionesGenerales = dto.ObservacionesGenerales,
                Activo = true,
                CreadoEn = DateTime.UtcNow
            });

        public Task<Paciente?> UpdateAsync(int id, PacienteUpdateDto dto)
            => Task.FromResult<Paciente?>(id == 1 ? SeedPaciente : null);

        public Task<Paciente?> DeactivateAsync(int id)
            => Task.FromResult<Paciente?>(id == 1 ? SeedPaciente : null);
    }

    private sealed class FakeCatalogoRepository : ICatalogoRepository
    {
        public Task<IEnumerable<Rol>> GetRolesAsync()
            => Task.FromResult<IEnumerable<Rol>>(new[]
            {
                new Rol { IdRol = 1, Nombre = "Administrador", Activo = true },
                new Rol { IdRol = 2, Nombre = "Dentista", Activo = true }
            });

        public Task<IEnumerable<EstadoCita>> GetEstadosCitaAsync()
            => Task.FromResult<IEnumerable<EstadoCita>>(new[]
            {
                new EstadoCita { IdEstadoCita = 1, Nombre = "Programada" }
            });

        public Task<IEnumerable<ProveedorCatalogoDto>> GetProveedoresAsync()
            => Task.FromResult<IEnumerable<ProveedorCatalogoDto>>(Array.Empty<ProveedorCatalogoDto>());

        public Task<IEnumerable<TipoMovimientoCatalogoDto>> GetTiposMovimientoInventarioAsync()
            => Task.FromResult<IEnumerable<TipoMovimientoCatalogoDto>>(Array.Empty<TipoMovimientoCatalogoDto>());

        public Task<IEnumerable<CategoriaServicioCatalogoDto>> GetCategoriasServicioAsync()
            => Task.FromResult<IEnumerable<CategoriaServicioCatalogoDto>>(Array.Empty<CategoriaServicioCatalogoDto>());

        public Task<IEnumerable<EstadoVentaCatalogoDto>> GetEstadosVentaAsync()
            => Task.FromResult<IEnumerable<EstadoVentaCatalogoDto>>(Array.Empty<EstadoVentaCatalogoDto>());

        public Task<IEnumerable<MetodoPagoCatalogoDto>> GetMetodosPagoAsync()
            => Task.FromResult<IEnumerable<MetodoPagoCatalogoDto>>(Array.Empty<MetodoPagoCatalogoDto>());

        public Task<IEnumerable<UsuarioCatalogoDto>> GetUsuariosActivosAsync()
            => Task.FromResult<IEnumerable<UsuarioCatalogoDto>>(new[]
            {
                new UsuarioCatalogoDto { IdUsuario = 1, CodigoUsuario = "USR-001", Nombres = "Admin", Apellidos = "Pruebas" }
            });
    }
}
