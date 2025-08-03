using DevShop.Application.DTOs;
using DevShop.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DevShop.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                var result = await _authService.RegisterUserAsync(registerUserDto);

                if (result)
                {
                    return Ok("Usuário registrado com sucesso.");
                }

                return BadRequest("O e-mail informado já está em uso.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                return BadRequest("O e-mail informado já está em uso (detectado pelo banco de dados).");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro inesperado no servidor.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var token = await _authService.LoginAsync(loginUserDto);
            if (string.IsNullOrEmpty(token))
                return Unauthorized("E-mail ou senha inválidos.");

            return Ok(new { Token = token });
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserDto registerUserDto)
        {
            var result = await _authService.RegisterAdminAsync(registerUserDto);
            if (!result)
            {
                return BadRequest("Não foi possível registrar o admin. O e-mail já pode estar em uso.");
            }
            return Ok("Usuário Admin registrado com sucesso.");
        }
    }
}
