using DevilFruits.BLL.Services.Acciones;
using DevilFruits.DTO.ExternalModel;
using DevilFruits.DTO.Models;
using DevilFruits.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.Actions
{
    //[Authorize(Roles = "Admin, user")]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritoService _favoritoService;

        public FavoritosController(IFavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        [HttpPost("agregar")]
        public async Task<ActionResult<ApiResponse<bool>>> AgregarFavorito([FromBody] FavoritoDTO model)
        {
            var response = await _favoritoService.AgregarFavorito(model);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            return CreatedAtAction(
                nameof(ObtenerFavoritosPorUsuario),
                new { usuarioId = model.UsuarioId },
                apiResponse
            );
        }

        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult<ApiResponse<List<FrutaDTO>>>> ObtenerFavoritosPorUsuario(int usuarioId)
        {
            var response = await _favoritoService.ObtenerFavoritos(usuarioId);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }
            
            return Ok(apiResponse);
        }
    }
}
