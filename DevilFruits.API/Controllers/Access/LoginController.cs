using DevilFruits.BLL.Services;
using DevilFruits.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.Access
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
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
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("iniciosesion")]
        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var token = await _authService.Login(model);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Mensaje = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor" });
            }
        }
    }
}
