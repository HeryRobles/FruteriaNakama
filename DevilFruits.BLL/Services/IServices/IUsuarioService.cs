using DevilFruits.DTO.Models;
using DevilFruits.BLL.Response;

namespace DevilFruits.BLL.Services.IServices
{
    public interface IUsuarioService
    {
        Task<HttpResponseWrapper<List<UsuarioDTO>>> ListaUsuarios();
        Task<HttpResponseWrapper<UsuarioDTO>> ObtenerUsuario(int id);
        Task<HttpResponseWrapper<UsuarioDTO>> CrearUsuario(UsuarioDTO usuario);
        Task<HttpResponseWrapper<bool>> EditarUsuario(UsuarioDTO usuario, int usuarioActual);
        Task<HttpResponseWrapper<bool>> EliminarUsuario(int id);
    }
}