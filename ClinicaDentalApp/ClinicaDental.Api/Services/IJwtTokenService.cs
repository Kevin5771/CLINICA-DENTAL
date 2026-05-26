using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Models;

namespace ClinicaDental.Api.Services;

public interface IJwtTokenService
{
    AuthResponseDto GenerateToken(Usuario usuario);
}
