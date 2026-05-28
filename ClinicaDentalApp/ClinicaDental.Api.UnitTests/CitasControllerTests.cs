using ClinicaDental.Api.Controllers;
using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;
using ClinicaDental.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ClinicaDental.Api.UnitTests;

public sealed class CitasControllerTests
{
    [Fact]
    public async Task Create_WhenDateIsInPast_ReturnsBadRequest()
    {
        var repository = new FakeCitaRepository();
        var controller = new CitasController(repository);
        var dto = NewValidCita();
        dto.Fecha = DateTime.Today.AddDays(-1);

        var result = await controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.False(repository.CreateWasCalled);
    }

    [Fact]
    public async Task Create_WhenEndTimeIsBeforeStartTime_ReturnsBadRequest()
    {
        var repository = new FakeCitaRepository();
        var controller = new CitasController(repository);
        var dto = NewValidCita();
        dto.HoraInicio = new TimeSpan(10, 0, 0);
        dto.HoraFin = new TimeSpan(9, 0, 0);

        var result = await controller.Create(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.False(repository.CreateWasCalled);
    }

    [Fact]
    public async Task Create_WhenDataIsValid_ReturnsCreatedAtAction()
    {
        var repository = new FakeCitaRepository();
        var controller = new CitasController(repository);

        var result = await controller.Create(NewValidCita());

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var cita = Assert.IsType<Cita>(created.Value);
        Assert.Equal(77, cita.IdCita);
        Assert.True(repository.CreateWasCalled);
    }

    [Fact]
    public async Task Cancel_WhenCitaDoesNotExist_ReturnsNotFound()
    {
        var controller = new CitasController(new FakeCitaRepository(returnNullOnCancel: true));

        var result = await controller.Cancel(100, new CancelarCitaDto { IdUsuarioAccion = 1 });

        Assert.IsType<NotFoundResult>(result);
    }

    private static CitaCreateDto NewValidCita() => new()
    {
        IdPaciente = 1,
        IdDentista = 2,
        Fecha = DateTime.Today.AddDays(1),
        HoraInicio = new TimeSpan(9, 0, 0),
        HoraFin = new TimeSpan(10, 0, 0),
        Motivo = "Limpieza dental",
        IdEstadoCita = 1,
        CreadaPor = 1
    };

    private sealed class FakeCitaRepository : ICitaRepository
    {
        private readonly bool _returnNullOnCancel;

        public FakeCitaRepository(bool returnNullOnCancel = false)
        {
            _returnNullOnCancel = returnNullOnCancel;
        }

        public bool CreateWasCalled { get; private set; }

        public Task<IEnumerable<Cita>> ListAsync(DateTime? fechaDesde, DateTime? fechaHasta, int? idDentista, int? idEstadoCita, int? idPaciente)
            => Task.FromResult<IEnumerable<Cita>>(Array.Empty<Cita>());

        public Task<Cita?> GetByIdAsync(int id)
            => Task.FromResult<Cita?>(NewCita(id));

        public Task<Cita> CreateAsync(CitaCreateDto dto)
        {
            CreateWasCalled = true;
            return Task.FromResult(NewCita(77, dto));
        }

        public Task<Cita?> UpdateAsync(int id, CitaUpdateDto dto)
            => Task.FromResult<Cita?>(NewCita(id));

        public Task<Cita?> CancelAsync(int id, CancelarCitaDto dto)
            => Task.FromResult<Cita?>(_returnNullOnCancel ? null : NewCita(id));

        private static Cita NewCita(int id, CitaCreateDto? dto = null)
        {
            dto ??= NewValidCita();
            return new Cita
            {
                IdCita = id,
                IdPaciente = dto.IdPaciente,
                Paciente = "Juan Perez",
                IdDentista = dto.IdDentista,
                Dentista = "Dra. Ana López",
                Fecha = dto.Fecha,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                Motivo = dto.Motivo,
                Observaciones = dto.Observaciones,
                IdEstadoCita = dto.IdEstadoCita,
                EstadoCita = "Programada",
                CreadaPor = dto.CreadaPor,
                CreadaEn = DateTime.UtcNow
            };
        }
    }
}
