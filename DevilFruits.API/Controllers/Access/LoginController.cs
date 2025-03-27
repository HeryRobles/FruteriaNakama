using DevilFruits.BLL.Services;
using DevilFruits.DTO;
using DevilFruits.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<ActionResult<UsuarioDTO>> RegistrarAsync([FromBody] UsuarioDTO usuarioDTO)
        {
            var response = await _authService.Registro(usuarioDTO);
            if(response.Error)
            {
                _logger.LogWarning("Error al registrar usuario: {Mensaje}", 
                    await response.GetErrorMessageAsync());
                return StatusCode((int)response.StatusCode, new 
                { 
                    Mensaje = await response.GetErrorMessageAsync() 
                });
            }
            return Ok(response);
        }

        [HttpPost("iniciosesion")]
        public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.Login(loginDTO);
            if (response.Error)
            {
                var errorMessage = await response.GetErrorMessageAsync();
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("Intento de inicio de sesión fallido para {Email}: {Message}",
                    loginDTO.Email, errorMessage);
                    return Unauthorized(new
                    {
                        Mensaje = await response.GetErrorMessageAsync()
                    });
                }
                
                _logger.LogError("Error durante login para {Email} (Código {StatusCode}): {Message}",
                    loginDTO.Email, (int)response.StatusCode, errorMessage);

                return StatusCode((int)response.StatusCode, new { Message = errorMessage });

            }
            _logger.LogInformation("Login exitoso para: {Email}", loginDTO.Email);
            return Ok(response.Response);
        }
    }
}
