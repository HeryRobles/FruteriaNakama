using DevilFruits.BLL.Services.Acciones;
using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;
using DevilFruits.DTO.ExternalModel;
using DevilFruits.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.Actions
{
    //[Authorize(Roles = "Admin, user")]
    [Route("api/frutas-detalle")]
    [ApiController]
    public class FrutasDetalleController : ControllerBase
    {
        private readonly IFrutaResenaService _frutaResenaService;

        public FrutasDetalleController(IFrutaResenaService frutaResenaService)
        {
            _frutaResenaService = frutaResenaService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<FrutaDetalleDTO>>> ObtenerFrutaConResenas(int id)
        {
            var response = await _frutaResenaService.ObtenerFrutaConResenasAsync(id);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            return Ok(apiResponse);
        }
    }
}
