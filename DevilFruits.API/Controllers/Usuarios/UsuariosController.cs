using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;
using DevilFruits.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<UsuarioDTO>>> ListaUsuarios()
        {
            var response = await _usuarioService.ListaUsuarios();
            if(response.Error)
            {

                return BadRequest(new { message = await response.GetErrorMessageAsync() });
            }
            return Ok(response.Response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuario(int id)
        {
            var response = await _usuarioService.ObtenerUsuario(id);
            if (response.Error)
            {
                var errorMessage = await response.GetErrorMessageAsync();
                return StatusCode((int)response.StatusCode, new { message = errorMessage });
            }
            return Ok(response.Response);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> CrearUsuario([FromBody] UsuarioDTO usuario)
        {
            var response = await _usuarioService.CrearUsuario(usuario);
            if (response.Error)
            {
                return StatusCode((int)response.StatusCode, new { message = await response.GetErrorMessageAsync() });
            }

            if (response.Response == null || response.Response.Id == 0)
            {
                return BadRequest(new { message = "No se pudo generar un ID válido para el usuario." });
            }
            return CreatedAtAction(nameof(ObtenerUsuario), new { id = response.Response.Id }, response.Response);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioDTO>> EditarUsuario(int id, [FromBody] UsuarioDTO usuario)
        {
            if (id != usuario.Id)
            
                return BadRequest(new { message = "El Id del usuario no coincide" });
            
            var response = await _usuarioService.EditarUsuario(usuario, id);
            if (response.Error)
            {
                return StatusCode((int)response.StatusCode, new { message = await response.GetErrorMessageAsync() });
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            var response = await _usuarioService.EliminarUsuario(id);

            if (response.Error)
            {
                return StatusCode((int)response.StatusCode, new { message = await response.GetErrorMessageAsync() });
            }
            return NoContent();
        }

    }
}