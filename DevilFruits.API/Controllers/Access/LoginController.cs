using DevilFruits.BLL.Services;
using DevilFruits.DTO;
using DevilFruits.DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.Access
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IAuthService authService, ILogger<LoginController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("registrarse")]
        public async Task<ActionResult> RegistrarAsync([FromBody] UsuarioDTO model)
        {
            try
            {
                var usuario = await _authService.Registro(model);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("iniciosesion")]
        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var token = await _authService.Login(model);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Intento de inicio de sesión fallido");
                return Unauthorized(new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el inicio de sesión");
                return StatusCode(500, new { Mensaje = "Error interno del servidor" });
            }
        }
    }
}
