using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.Actions
{
    [Authorize(Roles = "Admin, user")]
    [Route("api/frutas-detalle")]
    [ApiController]
    public class FrutasDetalleController : ControllerBase
    {
        private readonly IFrutaService _frutaService;
        public FrutasDetalleController(IFrutaService frutaService)
        {
            _frutaService = frutaService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerFrutaConResenas(int id)
        {
            try
            {
                var fruta = await _frutaService.ObtenerFrutaAsync(id);
                if (fruta == null)
                
                    return NotFound(new { message = "Fruta no encontrada" });
                
                return Ok(fruta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al obtener detalles", error = ex.Message });
            }
        }
    }
}
