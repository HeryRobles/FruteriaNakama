using DevilFruits.DTO;

namespace DevilFruits.BLL.Services.IServices
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> ListaUsuarios();
        Task<UsuarioDTO> ObtenerUsuario(int id);
        Task<UsuarioDTO> CrearUsuario(UsuarioDTO usuario);
        Task<bool> EditarUsuario(UsuarioDTO usuario, int usuarioActual);
        Task<bool> EliminarUsuario(int id);
    }
}
