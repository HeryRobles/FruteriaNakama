using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO.ExternalModel;
using DevilFruits.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<ApiResponse<List<FrutaDTO>>>> ListadoFruits()
        {
            var response = await _fruitService.ListadoFrutas();
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            return Ok(apiResponse);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<FrutaDTO>>> ObtenerFruta(int id)
        {
            var response = await _fruitService.ObtenerFrutaAsync(id);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            return Ok(apiResponse);
        }
    }
}
