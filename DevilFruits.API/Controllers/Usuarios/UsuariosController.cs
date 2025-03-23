using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DevilFruits.API.Controllers.Usuarios
{
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
            try
            {
                var usuarios = await _usuarioService.ListaUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObtenerUsuario(id);
                if(usuario== null)
                {
                    return NotFound(new {message = "Usuario no encontrado" });
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> CrearUsuario([FromBody]UsuarioDTO usuario)
        {
            try
            {
                var usuarioCreado = await _usuarioService.CrearUsuario(usuario);
                return CreatedAtAction(nameof(ObtenerUsuario), new { id = usuarioCreado.Id }, usuarioCreado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioDTO>> EditarUsuario(int id, [FromBody] UsuarioDTO usuario)
        {
            try
            {
                if(id != usuario.Id)
                {
                    return BadRequest(new { message = "Datos incorrectos" });
                }
                var usuarioEditado = await _usuarioService.EditarUsuario(usuario, id);
                if (!usuarioEditado)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            try
            {
                var resultado = await _usuarioService.EliminarUsuario(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
