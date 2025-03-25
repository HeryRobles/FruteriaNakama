using DevilFruits.BLL.Services.Acciones;
using DevilFruits.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.Actions
{
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
        public async Task<ActionResult> AgregarFavorito([FromBody] FavoritoDTO model)
        {
            try
            {
                var favorito = await _favoritoService.AgregarFavorito(model);
                return Ok(favorito);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult> ObtenerFavoritosPorUsuario(int usuarioId)
        {
            try
            {
                var favoritos = await _favoritoService.ObtenerFavoritos(usuarioId);
                return Ok(favoritos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
