using DevilFruits.BLL.Services.Acciones;
using DevilFruits.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.Actions
{
    [Authorize(Roles = "Admin, user")]
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
        public async Task<ActionResult> AgregarResena([FromBody] ResenaDTO model)
        {
            try
            {
                var resena = await _resenaService.CrearResenaAsync(model);
                return Ok(resena);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult> ObtenerResenasPorUsuario(int usuarioId)
        {
            try
            {
                var resenas = await _resenaService.ObtenerResenasPorUsuarioAsync(usuarioId);
                if (resenas == null || !resenas.Any())
                {
                    return Ok(new
                    {
                        message = "El usuario no tiene reseñas registradas",
                        data = new List<ResenaDTO>()
                    });
                }

                return Ok(new { data = resenas });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error interno al obtener reseñas",
                    error = ex.Message
                });
            }
        }
        [HttpGet("fruta/{devilFruitId:int}")]
        public async Task<ActionResult> ObtenerResenasPorFruta(int devilFruitId)
        {
            try
            {
                var resenas = await _resenaService.ObtenerResenasPorFrutaAsync(devilFruitId);
                if (resenas == null || !resenas.Any())
                {
                    return Ok(new
                    {
                        message = "La fruta no tiene reseñas registradas",
                        data = new List<ResenaDTO>()
                    });
                }
                return Ok(new { data = resenas });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error interno al obtener reseñas",
                    error = ex.Message
                });
            }
        }
    }
}
