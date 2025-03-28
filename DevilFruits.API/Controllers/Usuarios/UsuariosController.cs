using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;
using DevilFruits.DTO.Models;
using DevilFruits.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevilFruits.API.Controllers.Usuarios
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        
        [HttpGet("lista")]
        public async Task<ActionResult<ApiResponse<List<UsuarioDTO>>>> ListaUsuarios()
        {
            var response = await _usuarioService.ListaUsuarios();
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }
            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> ObtenerUsuario(int id)
        {
            var response = await _usuarioService.ObtenerUsuario(id);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }
            return Ok(apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> CrearUsuario([FromBody] UsuarioDTO usuario)
        {
            var response = await _usuarioService.CrearUsuario(usuario);
            var apiResponse = await response.ToApiResponseAsync();

            if (response.Error)
                return StatusCode(apiResponse.StatusCode, apiResponse);
            

            if (response.Response?.Id == 0)
            {
                apiResponse.Success = false;
                apiResponse.Message = "No se pudo generar un ID válido para el usuario";
                apiResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                return BadRequest(apiResponse);
            }

            return CreatedAtAction(
                nameof(ObtenerUsuario),
                new { id = response.Response?.Id },
                apiResponse
            );

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> EditarUsuario(int id, [FromBody] UsuarioDTO usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "El ID del usuario no coincide",
                    StatusCode = (int)HttpStatusCode.BadRequest
                });
            }

            var response = await _usuarioService.EditarUsuario(usuario, id);
            var apiResponse = response.ToApiResponse();

            if (response.Error)
                return StatusCode(apiResponse.StatusCode, apiResponse);
            

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> EliminarUsuario(int id)
        {
            var response = await _usuarioService.EliminarUsuario(id);
            var apiResponse = response.ToApiResponse();

            if (response.Error)
            {
                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            return NoContent();
        }

    }
}