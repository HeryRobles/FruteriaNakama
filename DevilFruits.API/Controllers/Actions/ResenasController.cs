using DevilFruits.BLL.Services.Acciones;
using DevilFruits.DTO.Models;
using DevilFruits.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.Actions
{
    //[Authorize(Roles = "Admin, user")]
    [Route("api/[controller]")]
    [ApiController]
    public class ResenasController : ControllerBase
    {
        private readonly IResenaService _resenaService;
        public ResenasController(IResenaService resenaService)
        {
            _resenaService = resenaService;
        }
        [HttpPost("agregar")]
        public async Task<ActionResult<ApiResponse<bool>>> AgregarResena([FromBody] ResenaDTO model)
        {
            var response = await _resenaService.CrearResenaAsync(model);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            return CreatedAtAction(
                nameof(ObtenerResenasPorUsuario),
                new { usuarioId = model.UsuarioId },
                apiResponse
            );
        }
        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult<ApiResponse<List<ResenaDTO>>>> ObtenerResenasPorUsuario(int usuarioId)
        {
            var response = await _resenaService.ObtenerResenasPorUsuarioAsync(usuarioId);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            if (response.Response == null || !response.Response.Any())
            {
                apiResponse.Message = "El usuario no tiene reseñas registradas";
                return Ok(apiResponse);
            }

            return Ok(apiResponse);

        }
        [HttpGet("fruta/{devilFruitId:int}")]
        public async Task<ActionResult<ApiResponse<List<ResenaDTO>>>> ObtenerResenasPorFruta(int devilFruitId)
        {
            var response = await _resenaService.ObtenerResenasPorFrutaAsync(devilFruitId);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            if (response.Response == null || !response.Response.Any())
            {
                apiResponse.Message = "La fruta no tiene reseñas registradas";
                return Ok(apiResponse);
            }

            return Ok(apiResponse);
        }
    }
}
