using DevilFruits.BLL.Services;
using DevilFruits.DTO;
using DevilFruits.DTO.Models;
using DevilFruits.DTO.Responses;
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
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> RegistrarAsync([FromBody] UsuarioDTO usuarioDTO)
        {
            var response = await _authService.Registro(usuarioDTO);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                _logger.LogWarning("Error al registrar usuario: {Mensaje}", apiResponse.Message);
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            return Ok(apiResponse);
        }
        

        [HttpPost("iniciosesion")]
        public async Task<ActionResult<ApiResponse<TokenDTO>>> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.Login(loginDTO);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("Autenticación fallida para {Email}", loginDTO.Email);
                }
                else
                {
                    _logger.LogError("Error durante login: {Message}", apiResponse.Message);
                }
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            _logger.LogInformation("Login exitoso para: {Email}", loginDTO.Email);
            return Ok(apiResponse);
        }
    }
}
