using ClinicaDental.Api.DTOs;
using ClinicaDental.Api.Repositories;
using ClinicaDental.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaDental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IUsuarioRepository usuarioRepository, IPasswordService passwordService, IJwtTokenService jwtTokenService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var usuario = await _usuarioRepository.GetByUsernameOrCodeAsync(dto.Usuario);
        if (usuario is null || !usuario.Activo)
            return Unauthorized(new { message = "Usuario no válido o inactivo." });

        var isValid = _passwordService.VerifyPassword(dto.Password, usuario.PasswordHash, usuario.PasswordSalt);
        if (!isValid)
            return Unauthorized(new { message = "Credenciales inválidas." });

        return Ok(_jwtTokenService.GenerateToken(usuario));
    }
}
