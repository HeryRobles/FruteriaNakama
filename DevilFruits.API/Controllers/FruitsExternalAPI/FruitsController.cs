using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.FruitsExternalAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitsController : ControllerBase
    {
        private readonly IFrutaService _fruitService;

        public FruitsController(IFrutaService fruitService)
        {
            _fruitService = fruitService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<FrutaDTO>>> ListadoFruits()
        {
            try
            {
                var fruits = await _fruitService.ListadoFrutas();
                return Ok(fruits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno al procesar la solicitud.");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FrutaDTO>> ObtenerFruta(int id)
        {
            try
            {
                var fruit = await _fruitService.ObtenerFrutaAsync(id);
                if (fruit == null)
                {
                    return NotFound(new { message = "Fruta no encontrada" });
                }
                return Ok(fruit);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno al procesar la solicitud.");
            }
        }
    }
}
