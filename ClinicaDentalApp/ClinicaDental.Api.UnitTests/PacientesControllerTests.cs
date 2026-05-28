using ClinicaDental.Api.Controllers;
using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;
using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ClinicaDental.Api.UnitTests;

public sealed class PacientesControllerTests
{
    [Fact]
    public async Task Create_WhenBirthDateIsFuture_ReturnsBadRequest()
    {
        var repository = new FakePacienteRepository();
        var controller = new PacientesController(repository);
        var dto = NewValidPaciente();
        dto.FechaNacimiento = DateTime.Today.AddDays(1);

        var result = await controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.False(repository.CreateWasCalled);
    }

    [Fact]
    public async Task Create_WhenPhoneFormatIsInvalid_ReturnsBadRequest()
    {
        var repository = new FakePacienteRepository();
        var controller = new PacientesController(repository);
        var dto = NewValidPaciente();
        dto.Telefono = "abc123";

        var result = await controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.False(repository.CreateWasCalled);
    }

    [Fact]
    public async Task Create_WhenNameContainsNumbers_ReturnsBadRequest()
    {
        var repository = new FakePacienteRepository();
        var controller = new PacientesController(repository);
        var dto = NewValidPaciente();
        dto.Nombres = "Juan123";

        var result = await controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.False(repository.CreateWasCalled);
    }

    [Fact]
    public async Task Create_WhenDataIsValid_ReturnsCreatedAtAction()
    {
        var repository = new FakePacienteRepository();
        var controller = new PacientesController(repository);

        var result = await controller.Create(NewValidPaciente());

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var paciente = Assert.IsType<Paciente>(created.Value);
        Assert.Equal(nameof(PacientesController.GetById), created.ActionName);
        Assert.Equal(99, paciente.IdPaciente);
        Assert.True(repository.CreateWasCalled);
    }

    [Fact]
    public async Task GetById_WhenPatientDoesNotExist_ReturnsNotFound()
    {
        var controller = new PacientesController(new FakePacienteRepository(returnNullOnGet: true));

        var result = await controller.GetById(123);

        Assert.IsType<NotFoundResult>(result);
    }

    private static PacienteCreateDto NewValidPaciente() => new()
    {
        CodigoPaciente = "PAC-001",
        Nombres = "Juan",
        Apellidos = "Perez",
        Telefono = "5555-1234",
        FechaNacimiento = new DateTime(1995, 5, 10),
        Genero = "M",
        Direccion = "Guatemala",
        Correo = "juan@example.com"
    };

    private sealed class FakePacienteRepository : IPacienteRepository
    {
        private readonly bool _returnNullOnGet;

        public FakePacienteRepository(bool returnNullOnGet = false)
        {
            _returnNullOnGet = returnNullOnGet;
        }

        public bool CreateWasCalled { get; private set; }

        public Task<IEnumerable<Paciente>> ListAsync(string? texto, bool? activo)
            => Task.FromResult<IEnumerable<Paciente>>(Array.Empty<Paciente>());

        public Task<Paciente?> GetByIdAsync(int id)
            => Task.FromResult<Paciente?>(_returnNullOnGet ? null : NewPaciente(id));

        public Task<Paciente> CreateAsync(PacienteCreateDto dto)
        {
            CreateWasCalled = true;
            return Task.FromResult(NewPaciente(99, dto));
        }

        public Task<Paciente?> UpdateAsync(int id, PacienteUpdateDto dto)
            => Task.FromResult<Paciente?>(NewPaciente(id, dto));

        public Task<Paciente?> DeactivateAsync(int id)
            => Task.FromResult<Paciente?>(NewPaciente(id));

        private static Paciente NewPaciente(int id, PacienteCreateDto? dto = null)
        {
            dto ??= NewValidPaciente();
            return new Paciente
            {
                IdPaciente = id,
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
            };
        }
    }
}
